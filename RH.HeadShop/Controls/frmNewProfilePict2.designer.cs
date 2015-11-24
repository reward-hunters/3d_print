namespace RH.HeadShop.Controls
{
    partial class frmNewProfilePict2
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
            this.textName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureTemplate = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnInfo = new System.Windows.Forms.PictureBox();
            this.btnPlay = new System.Windows.Forms.PictureBox();
            this.btnQuestion = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.RenderTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureTemplate)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnQuestion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // textName
            // 
            this.textName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.textName.Location = new System.Drawing.Point(121, 15);
            this.textName.Margin = new System.Windows.Forms.Padding(4);
            this.textName.Name = "textName";
            this.textName.ReadOnly = true;
            this.textName.Size = new System.Drawing.Size(953, 28);
            this.textName.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(17, 20);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 22);
            this.label5.TabIndex = 12;
            this.label5.Text = "Name";
            // 
            // pictureTemplate
            // 
            this.pictureTemplate.Location = new System.Drawing.Point(16, 52);
            this.pictureTemplate.Margin = new System.Windows.Forms.Padding(4);
            this.pictureTemplate.Name = "pictureTemplate";
            this.pictureTemplate.Size = new System.Drawing.Size(1060, 565);
            this.pictureTemplate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureTemplate.TabIndex = 15;
            this.pictureTemplate.TabStop = false;
            this.pictureTemplate.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureTemplate_Paint);
            this.pictureTemplate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureTemplate_MouseDown);
            this.pictureTemplate.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureTemplate_MouseMove);
            this.pictureTemplate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureTemplate_MouseUp);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::RH.HeadShop.Properties.Resources.bgWizard2;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnInfo);
            this.panel1.Controls.Add(this.btnPlay);
            this.panel1.Controls.Add(this.btnQuestion);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Location = new System.Drawing.Point(16, 629);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1060, 123);
            this.panel1.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(324, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 22);
            this.label1.TabIndex = 15;
            this.label1.Text = "Move dots if needed";
            // 
            // btnInfo
            // 
            this.btnInfo.Image = global::RH.HeadShop.Properties.Resources.btnInfoNormal;
            this.btnInfo.Location = new System.Drawing.Point(975, 14);
            this.btnInfo.Margin = new System.Windows.Forms.Padding(4);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(45, 42);
            this.btnInfo.TabIndex = 14;
            this.btnInfo.TabStop = false;
            this.btnInfo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnInfo_MouseDown);
            this.btnInfo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnInfo_MouseUp);
            // 
            // btnPlay
            // 
            this.btnPlay.Image = global::RH.HeadShop.Properties.Resources.btnPlayNormal;
            this.btnPlay.Location = new System.Drawing.Point(909, 14);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(4);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(45, 42);
            this.btnPlay.TabIndex = 13;
            this.btnPlay.TabStop = false;
            this.btnPlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPlay_MouseDown);
            this.btnPlay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPlay_MouseUp);
            // 
            // btnQuestion
            // 
            this.btnQuestion.Image = global::RH.HeadShop.Properties.Resources.btnQuestionNormal;
            this.btnQuestion.Location = new System.Drawing.Point(844, 14);
            this.btnQuestion.Margin = new System.Windows.Forms.Padding(4);
            this.btnQuestion.Name = "btnQuestion";
            this.btnQuestion.Size = new System.Drawing.Size(45, 42);
            this.btnQuestion.TabIndex = 12;
            this.btnQuestion.TabStop = false;
            this.btnQuestion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnQuestion_MouseDown);
            this.btnQuestion.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnQuestion_MouseUp);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::RH.HeadShop.Properties.Resources.splitter;
            this.pictureBox2.Location = new System.Drawing.Point(809, -5);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(13, 119);
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.SystemColors.Control;
            this.btnNext.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnNext.Location = new System.Drawing.Point(925, 69);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(127, 46);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = false;
            // 
            // RenderTimer
            // 
            this.RenderTimer.Interval = 40;
            this.RenderTimer.Tick += new System.EventHandler(this.RenderTimer_Tick);
            // 
            // frmNewProfilePict2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureTemplate);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textName);
            this.Controls.Add(this.label5);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmNewProfilePict2";
            this.Size = new System.Drawing.Size(1092, 767);
            this.Resize += new System.EventHandler(this.frmNewProject2_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureTemplate)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnQuestion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureTemplate;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox btnQuestion;
        private System.Windows.Forms.PictureBox btnInfo;
        private System.Windows.Forms.PictureBox btnPlay;
        public System.Windows.Forms.Timer RenderTimer;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btnNext;
    }
}