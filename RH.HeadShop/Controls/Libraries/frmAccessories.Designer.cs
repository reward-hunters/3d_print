namespace RH.HeadShop.Controls.Libraries
{
    partial class frmAccessories
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAddNewMaterial = new System.Windows.Forms.Button();
            this.teAngle = new System.Windows.Forms.TextBox();
            this.ctrlAngle = new RH.HeadShop.Controls.ctrlAngleSelector();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarSize = new System.Windows.Forms.TrackBar();
            this.imageListView = new RH.ImageListView.ImageListViewEx();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.teAngle);
            this.panel1.Controls.Add(this.ctrlAngle);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.trackBarSize);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(290, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 362);
            this.panel1.TabIndex = 1;
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDelete.Location = new System.Drawing.Point(71, 21);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(56, 27);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAddNewMaterial
            // 
            this.btnAddNewMaterial.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddNewMaterial.Location = new System.Drawing.Point(7, 21);
            this.btnAddNewMaterial.Name = "btnAddNewMaterial";
            this.btnAddNewMaterial.Size = new System.Drawing.Size(58, 27);
            this.btnAddNewMaterial.TabIndex = 8;
            this.btnAddNewMaterial.Text = "Add";
            this.btnAddNewMaterial.UseVisualStyleBackColor = true;
            this.btnAddNewMaterial.Click += new System.EventHandler(this.btnAddNewMaterial_Click);
            // 
            // teAngle
            // 
            this.teAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.teAngle.Location = new System.Drawing.Point(86, 94);
            this.teAngle.Name = "teAngle";
            this.teAngle.Size = new System.Drawing.Size(51, 24);
            this.teAngle.TabIndex = 7;
            this.teAngle.Text = "0";
            this.teAngle.TextChanged += new System.EventHandler(this.teAngle_TextChanged);
            this.teAngle.Validating += new System.ComponentModel.CancelEventHandler(this.teAngle_Validating);
            // 
            // ctrlAngle
            // 
            this.ctrlAngle.Angle = 0;
            this.ctrlAngle.Location = new System.Drawing.Point(86, 41);
            this.ctrlAngle.Name = "ctrlAngle";
            this.ctrlAngle.Size = new System.Drawing.Size(47, 47);
            this.ctrlAngle.TabIndex = 3;
            this.ctrlAngle.OnAngleChanged += new RH.HeadShop.Controls.ctrlAngleSelector.AngleChangedDelegate(this.ctrlAngle_OnAngleChanged);
            this.ctrlAngle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrlAngle_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(88, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Angle";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(18, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Size";
            // 
            // trackBarSize
            // 
            this.trackBarSize.Location = new System.Drawing.Point(26, 32);
            this.trackBarSize.Maximum = 100;
            this.trackBarSize.Minimum = 1;
            this.trackBarSize.Name = "trackBarSize";
            this.trackBarSize.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarSize.Size = new System.Drawing.Size(45, 145);
            this.trackBarSize.TabIndex = 0;
            this.trackBarSize.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarSize.Value = 50;
            this.trackBarSize.Scroll += new System.EventHandler(this.trackBarSize_Scroll);
            this.trackBarSize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackBarSize_MouseDown);
            // 
            // imageListView
            // 
            this.imageListView.AllowDrag = true;
            this.imageListView.AllowMultyuse = false;
            this.imageListView.ColumnHeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.imageListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListView.GroupHeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.imageListView.Location = new System.Drawing.Point(0, 0);
            this.imageListView.Name = "imageListView";
            this.imageListView.PersistentCacheFile = "";
            this.imageListView.PersistentCacheSize = ((long)(100));
            this.imageListView.Size = new System.Drawing.Size(290, 362);
            this.imageListView.TabIndex = 3;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Controls.Add(this.btnAddNewMaterial);
            this.groupBox2.Location = new System.Drawing.Point(5, 198);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(133, 54);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Accesssories";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnImport);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(5, 258);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(133, 89);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // btnImport
            // 
            this.btnImport.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnImport.Location = new System.Drawing.Point(71, 54);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(59, 29);
            this.btnImport.TabIndex = 15;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExport.Location = new System.Drawing.Point(6, 54);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(59, 29);
            this.btnExport.TabIndex = 14;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(6, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 29);
            this.button1.TabIndex = 12;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnSavePositionAndSize_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(71, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(59, 29);
            this.button2.TabIndex = 13;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnClearProperties_Click);
            // 
            // frmAccessories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 362);
            this.Controls.Add(this.imageListView);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAccessories";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Accessories";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.frmAccessories_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAccessories_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ctrlAngleSelector ctrlAngle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarSize;
        private ImageListView.ImageListViewEx imageListView;
        private System.Windows.Forms.TextBox teAngle;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnAddNewMaterial;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}