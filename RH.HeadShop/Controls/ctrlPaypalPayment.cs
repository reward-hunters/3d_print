using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using PayPal;
using PayPal.Api;

namespace RH.HeadShop.Controls
{
    public partial class ctrlPaypalPayment : UserControl
    {
        private ChromiumWebBrowser chromeBrowser;
        public ctrlPaypalPayment()
        {
            InitializeComponent();
            // Start the browser after initialize global component
            InitializeChromium();
        }
        public void InitializeChromium()
        {
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser("http://google.com");
            chromeBrowser.LoadingStateChanged += ChromeBrowser_LoadingStateChanged;

            // Add it to the form and fill it to the form window.
            Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
        }
        public bool IsSuccess { get; private set; }
        private void ChromeBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            try
            {
                if (chromeBrowser.Address == "http://www.abalonellc.com/hairshop-10-coming-so10.html")
                    IsSuccess = false;
                else if (chromeBrowser.Address.Contains("abalonellc")) // оплата прошла?
                {
                    /*          // Возвращает return url вида:
                    http://www.abalonellc.com/hairshop-10-coming-so10.html?
                    paymentId=PAY-3PR23826GT693594MLALPVYA&
                    token=EC-6H734593WP4251039&
                    PayerID=6YDRH9BGLQN9G
                    */

                    var parse = HttpUtility.ParseQueryString(chromeBrowser.Address);
                    var paymentId = parse.Get("paymentId");
                    var token = parse.Get("token");
                    var payerId = parse.Get("PayerID");

                    if (string.IsNullOrEmpty(payerId))
                    {
                        IsSuccess = false;
                        throw new Exception();
                    }

                    createdPayment.Execute(apiContext, new PaymentExecution() { payer_id = payerId, transactions = createdPayment.transactions });
                    IsSuccess = true;
                    ((Form)TopLevelControl).Close();
                }
            }
            catch
            {
                IsSuccess = false;
                ((Form)TopLevelControl).Close();
            }
        }

        /// <summary>
        /// Gets a random invoice number to be used with a sample request that requires an invoice number.
        /// </summary>
        /// <returns>A random invoice number in the range of 0 to 999999</returns>
        public static string GetRandomInvoiceNumber()
        {
            return new Random().Next(999999).ToString();
        }


        private APIContext apiContext;
        private Payment createdPayment;
        public bool CreatePayment(string priceStr, string description)
        {
            try
            {
                // Authenticate with PayPal
                var config = ConfigManager.Instance.GetProperties();
                var accessToken = new OAuthTokenCredential(config).GetAccessToken();
                apiContext = new APIContext(accessToken);

                var itemList = new ItemList()
                {
                    items = new List<Item>()
                    {
                        new Item()
                        {
                            name = "PrintAhead print",
                            currency = "USD",
                            price = priceStr,
                            quantity = "1",
                            sku = "sku"
                        }
                    }
                };

                var payer = new Payer() { payment_method = "paypal" };
                var redirUrls = new RedirectUrls()
                {
                    cancel_url = "http://www.abalonellc.com/hairshop-10-coming-so10.html",
                    return_url = "http://www.abalonellc.com/"
                };

                var details = new Details()
                {
                    tax = "0",
                    shipping = "0",
                    subtotal = priceStr
                };

                var amount = new Amount()
                {
                    currency = "USD",
                    total = priceStr, // Total must be equal to sum of shipping, tax and subtotal.
                    details = details
                };

                var transactionList = new List<Transaction>
                {
                    new Transaction()
                    {
                        description = description, // transaction description
                        invoice_number = GetRandomInvoiceNumber(),
                        amount = amount,
                        item_list = itemList
                    }
                };



                var payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };


                createdPayment = payment.Create(apiContext);
                var links = createdPayment.links.GetEnumerator();
                var hasGoodLink = false;
                while (links.MoveNext())
                {
                    var link = links.Current;
                    if (link != null && link.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        chromeBrowser.Load(link.href);
                        hasGoodLink = true;
                        break;
                    }
                }

                if (!hasGoodLink)
                    return false;
            }
            catch (PaymentsException ex)
            {
                // Get the details of this exception with ex.Details.  If you have logging setup for your project, this information will also be automatically logged to your logfile.
                var sb = new StringBuilder();
                sb.AppendLine("Error:    " + ex.Details.name);
                sb.AppendLine("Message:  " + ex.Details.message);
                sb.AppendLine("URI:      " + ex.Details.information_link);
                sb.AppendLine("Debug ID: " + ex.Details.debug_id);
                MessageBox.Show(sb.ToString());
                return false;
            }
            return true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
