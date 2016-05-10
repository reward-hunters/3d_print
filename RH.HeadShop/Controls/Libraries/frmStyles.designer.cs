namespace RH.HeadShop.Controls.Libraries
{
    partial class frmStyles
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
            this.btnClearProperties = new System.Windows.Forms.Button();
            this.btnSavePositionAndSize = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAddNewMaterial = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarSize = new System.Windows.Forms.TrackBar();
            this.imageListView = new RH.ImageListView.ImageListViewEx();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.trackBarSize);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(290, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 362);
            this.panel1.TabIndex = 1;
            // 
            // btnClearProperties
            // 
            this.btnClearProperties.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClearProperties.Location = new System.Drawing.Point(71, 19);
            this.btnClearProperties.Name = "btnClearProperties";
            this.btnClearProperties.Size = new System.Drawing.Size(59, 29);
            this.btnClearProperties.TabIndex = 13;
            this.btnClearProperties.Text = "Clear";
            this.btnClearProperties.UseVisualStyleBackColor = true;
            this.btnClearProperties.Click += new System.EventHandler(this.btnClearProperties_Click);
            // 
            // btnSavePositionAndSize
            // 
            this.btnSavePositionAndSize.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSavePositionAndSize.Location = new System.Drawing.Point(6, 19);
            this.btnSavePositionAndSize.Name = "btnSavePositionAndSize";
            this.btnSavePositionAndSize.Size = new System.Drawing.Size(59, 29);
            this.btnSavePositionAndSize.TabIndex = 12;
            this.btnSavePositionAndSize.Text = "Save";
            this.btnSavePositionAndSize.UseVisualStyleBackColor = true;
            this.btnSavePositionAndSize.Click += new System.EventHandler(this.btnSavePositionAndSize_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDelete.Location = new System.Drawing.Point(71, 19);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(59, 29);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAddNewMaterial
            // 
            this.btnAddNewMaterial.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddNewMaterial.Location = new System.Drawing.Point(8, 19);
            this.btnAddNewMaterial.Name = "btnAddNewMaterial";
            this.btnAddNewMaterial.Size = new System.Drawing.Size(57, 29);
            this.btnAddNewMaterial.TabIndex = 8;
            this.btnAddNewMaterial.Text = "Add";
            this.btnAddNewMaterial.UseVisualStyleBackColor = true;
            this.btnAddNewMaterial.Click += new System.EventHandler(this.btnAddNewMaterial_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(30, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Length";
            // 
            // trackBarSize
            // 
            this.trackBarSize.Location = new System.Drawing.Point(49, 31);
            this.trackBarSize.Maximum = 20;
            this.trackBarSize.Minimum = 1;
            this.trackBarSize.Name = "trackBarSize";
            this.trackBarSize.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarSize.Size = new System.Drawing.Size(45, 170);
            this.trackBarSize.TabIndex = 0;
            this.trackBarSize.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarSize.Value = 10;
            this.trackBarSize.Scroll += new System.EventHandler(this.trackBarSize_Scroll);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnImport);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.btnSavePositionAndSize);
            this.groupBox1.Controls.Add(this.btnClearProperties);
            this.groupBox1.Location = new System.Drawing.Point(8, 207);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(133, 89);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAddNewMaterial);
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Location = new System.Drawing.Point(8, 302);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(133, 54);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Styles";
            // 
            // frmStyles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 362);
            this.Controls.Add(this.imageListView);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStyles";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Styles";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.frmStyles_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmStyles_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarSize;
        private ImageListView.ImageListViewEx imageListView;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnAddNewMaterial;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClearProperties;
        private System.Windows.Forms.Button btnSavePositionAndSize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
    }
}