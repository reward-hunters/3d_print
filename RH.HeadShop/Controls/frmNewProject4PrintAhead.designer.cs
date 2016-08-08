using System;

namespace RH.HeadShop.Controls
{
    partial class frmNewProject4PrintAhead
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNewProject4PrintAhead));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textProjectName = new System.Windows.Forms.TextBox();
            this.textProjectFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnOpenFolderDlg = new System.Windows.Forms.Button();
            this.btnOpenFileDlg = new System.Windows.Forms.Button();
            this.textTemplateImage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureTemplate = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupLoadProject = new System.Windows.Forms.GroupBox();
            this.btnLoadProject = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textLoadProject = new System.Windows.Forms.TextBox();
            this.rbNew = new System.Windows.Forms.RadioButton();
            this.rbSaved = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnChild = new System.Windows.Forms.PictureBox();
            this.btnFemale = new System.Windows.Forms.PictureBox();
            this.btnMale = new System.Windows.Forms.PictureBox();
            this.btnInfo = new System.Windows.Forms.PictureBox();
            this.btnPlay = new System.Windows.Forms.PictureBox();
            this.rbImportObj = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.btnQuestion = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.RenderTimer = new System.Windows.Forms.Timer(this.components);
            this.rb2048 = new System.Windows.Forms.RadioButton();
            this.rb1024 = new System.Windows.Forms.RadioButton();
            this.rb512 = new System.Windows.Forms.RadioButton();
            this.CheekTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureTemplate)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupLoadProject.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnChild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFemale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnQuestion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(424, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select template jpg image(image you want to use as a template).";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(9, 220);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(440, 54);
            this.label2.TabIndex = 3;
            this.label2.Text = "You are about to start a new project. Please enter a new project name and select " +
    "a location for the project folder.\r\nAll project items will be saved in this new " +
    "folder.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(9, 288);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Project name";
            // 
            // textProjectName
            // 
            this.textProjectName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.textProjectName.Location = new System.Drawing.Point(109, 284);
            this.textProjectName.Name = "textProjectName";
            this.textProjectName.Size = new System.Drawing.Size(297, 24);
            this.textProjectName.TabIndex = 5;
            // 
            // textProjectFolder
            // 
            this.textProjectFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.textProjectFolder.Location = new System.Drawing.Point(109, 314);
            this.textProjectFolder.Name = "textProjectFolder";
            this.textProjectFolder.ReadOnly = true;
            this.textProjectFolder.Size = new System.Drawing.Size(297, 24);
            this.textProjectFolder.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(9, 318);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Project folder";
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnApply.Location = new System.Drawing.Point(661, 58);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(104, 35);
            this.btnApply.TabIndex = 8;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnOpenFolderDlg
            // 
            this.btnOpenFolderDlg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOpenFolderDlg.Location = new System.Drawing.Point(412, 314);
            this.btnOpenFolderDlg.Name = "btnOpenFolderDlg";
            this.btnOpenFolderDlg.Size = new System.Drawing.Size(33, 24);
            this.btnOpenFolderDlg.TabIndex = 9;
            this.btnOpenFolderDlg.Text = "...";
            this.btnOpenFolderDlg.UseVisualStyleBackColor = true;
            this.btnOpenFolderDlg.Click += new System.EventHandler(this.btnOpenFolderDlg_Click);
            // 
            // btnOpenFileDlg
            // 
            this.btnOpenFileDlg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOpenFileDlg.Location = new System.Drawing.Point(411, 164);
            this.btnOpenFileDlg.Name = "btnOpenFileDlg";
            this.btnOpenFileDlg.Size = new System.Drawing.Size(33, 24);
            this.btnOpenFileDlg.TabIndex = 12;
            this.btnOpenFileDlg.Text = "...";
            this.btnOpenFileDlg.UseVisualStyleBackColor = true;
            this.btnOpenFileDlg.Click += new System.EventHandler(this.btnOpenFileDlg_Click);
            // 
            // textTemplateImage
            // 
            this.textTemplateImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.textTemplateImage.Location = new System.Drawing.Point(108, 164);
            this.textTemplateImage.Name = "textTemplateImage";
            this.textTemplateImage.ReadOnly = true;
            this.textTemplateImage.Size = new System.Drawing.Size(297, 24);
            this.textTemplateImage.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(8, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Template";
            // 
            // pictureTemplate
            // 
            this.pictureTemplate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureTemplate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureTemplate.Location = new System.Drawing.Point(12, 12);
            this.pictureTemplate.Name = "pictureTemplate";
            this.pictureTemplate.Size = new System.Drawing.Size(327, 438);
            this.pictureTemplate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureTemplate.TabIndex = 0;
            this.pictureTemplate.TabStop = false;
            this.pictureTemplate.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureTemplate_Paint);
            this.pictureTemplate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureTemplate_MouseDown);
            this.pictureTemplate.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureTemplate_MouseMove);
            this.pictureTemplate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureTemplate_MouseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb2048);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.rb1024);
            this.groupBox1.Controls.Add(this.rb512);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnOpenFileDlg);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textTemplateImage);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textProjectName);
            this.groupBox1.Controls.Add(this.btnOpenFolderDlg);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textProjectFolder);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(350, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(449, 348);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Create new project";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::RH.HeadShop.Properties.Resources.lol;
            this.pictureBox1.Location = new System.Drawing.Point(16, 52);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(421, 106);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // groupLoadProject
            // 
            this.groupLoadProject.Controls.Add(this.btnLoadProject);
            this.groupLoadProject.Controls.Add(this.label6);
            this.groupLoadProject.Controls.Add(this.textLoadProject);
            this.groupLoadProject.Enabled = false;
            this.groupLoadProject.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupLoadProject.Location = new System.Drawing.Point(354, 393);
            this.groupLoadProject.Name = "groupLoadProject";
            this.groupLoadProject.Size = new System.Drawing.Size(449, 57);
            this.groupLoadProject.TabIndex = 14;
            this.groupLoadProject.TabStop = false;
            this.groupLoadProject.Text = "Load project";
            // 
            // btnLoadProject
            // 
            this.btnLoadProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLoadProject.Location = new System.Drawing.Point(409, 23);
            this.btnLoadProject.Name = "btnLoadProject";
            this.btnLoadProject.Size = new System.Drawing.Size(33, 24);
            this.btnLoadProject.TabIndex = 12;
            this.btnLoadProject.Text = "...";
            this.btnLoadProject.UseVisualStyleBackColor = true;
            this.btnLoadProject.Click += new System.EventHandler(this.btnLoadProject_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(6, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Project file";
            // 
            // textLoadProject
            // 
            this.textLoadProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.textLoadProject.Location = new System.Drawing.Point(106, 23);
            this.textLoadProject.Name = "textLoadProject";
            this.textLoadProject.ReadOnly = true;
            this.textLoadProject.Size = new System.Drawing.Size(297, 24);
            this.textLoadProject.TabIndex = 11;
            // 
            // rbNew
            // 
            this.rbNew.AutoSize = true;
            this.rbNew.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbNew.Checked = true;
            this.rbNew.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.rbNew.Location = new System.Drawing.Point(359, 366);
            this.rbNew.Name = "rbNew";
            this.rbNew.Size = new System.Drawing.Size(106, 21);
            this.rbNew.TabIndex = 15;
            this.rbNew.TabStop = true;
            this.rbNew.Text = "New Project";
            this.rbNew.UseVisualStyleBackColor = true;
            this.rbNew.CheckedChanged += new System.EventHandler(this.rbNew_CheckedChanged);
            // 
            // rbSaved
            // 
            this.rbSaved.AutoSize = true;
            this.rbSaved.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbSaved.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.rbSaved.Location = new System.Drawing.Point(519, 366);
            this.rbSaved.Name = "rbSaved";
            this.rbSaved.Size = new System.Drawing.Size(156, 21);
            this.rbSaved.TabIndex = 16;
            this.rbSaved.Text = "Open Saved Project";
            this.rbSaved.UseVisualStyleBackColor = true;
            this.rbSaved.CheckedChanged += new System.EventHandler(this.rbSaved_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::RH.HeadShop.Properties.Resources.bgWizard2;
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.rbImportObj);
            this.panel1.Controls.Add(this.btnChild);
            this.panel1.Controls.Add(this.btnFemale);
            this.panel1.Controls.Add(this.btnMale);
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Controls.Add(this.btnInfo);
            this.panel1.Controls.Add(this.btnPlay);
            this.panel1.Controls.Add(this.btnQuestion);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 461);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(811, 100);
            this.panel1.TabIndex = 17;
            // 
            // btnChild
            // 
            this.btnChild.Image = global::RH.HeadShop.Properties.Resources.btnChildGray;
            this.btnChild.Location = new System.Drawing.Point(417, 29);
            this.btnChild.Name = "btnChild";
            this.btnChild.Size = new System.Drawing.Size(59, 59);
            this.btnChild.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnChild.TabIndex = 17;
            this.btnChild.TabStop = false;
            this.btnChild.Tag = "2";
            this.btnChild.Click += new System.EventHandler(this.btnChild_Click);
            // 
            // btnFemale
            // 
            this.btnFemale.Image = global::RH.HeadShop.Properties.Resources.btnFemaleGray;
            this.btnFemale.Location = new System.Drawing.Point(312, 29);
            this.btnFemale.Name = "btnFemale";
            this.btnFemale.Size = new System.Drawing.Size(59, 59);
            this.btnFemale.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnFemale.TabIndex = 16;
            this.btnFemale.TabStop = false;
            this.btnFemale.Tag = "2";
            this.btnFemale.Click += new System.EventHandler(this.btnFemale_Click);
            // 
            // btnMale
            // 
            this.btnMale.Image = global::RH.HeadShop.Properties.Resources.btnMaleNormal;
            this.btnMale.Location = new System.Drawing.Point(211, 29);
            this.btnMale.Name = "btnMale";
            this.btnMale.Size = new System.Drawing.Size(59, 59);
            this.btnMale.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnMale.TabIndex = 15;
            this.btnMale.TabStop = false;
            this.btnMale.Tag = "1";
            this.btnMale.Click += new System.EventHandler(this.btnMale_Click);
            // 
            // rbImportObj
            // 
            this.rbImportObj.AutoSize = true;
            this.rbImportObj.BackColor = System.Drawing.Color.Transparent;
            this.rbImportObj.Location = new System.Drawing.Point(541, 56);
            this.rbImportObj.Name = "rbImportObj";
            this.rbImportObj.Size = new System.Drawing.Size(14, 13);
            this.rbImportObj.TabIndex = 18;
            this.rbImportObj.TabStop = true;
            this.rbImportObj.UseVisualStyleBackColor = false;
            this.rbImportObj.CheckedChanged += new System.EventHandler(this.rbImportObj_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(506, 12);
            this.label11.Name = "label1";
            this.label11.Size = new System.Drawing.Size(95, 18);
            this.label11.TabIndex = 19;
            this.label11.Text = "Custom obj";
            // 
            // btnInfo
            // 
            this.btnInfo.Image = global::RH.HeadShop.Properties.Resources.btnInfoNormal;
            this.btnInfo.Location = new System.Drawing.Point(731, 11);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(34, 34);
            this.btnInfo.TabIndex = 14;
            this.btnInfo.TabStop = false;
            this.btnInfo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnInfo_MouseDown);
            this.btnInfo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnInfo_MouseUp);
            // 
            // btnPlay
            // 
            this.btnPlay.Image = global::RH.HeadShop.Properties.Resources.btnPlayNormal;
            this.btnPlay.Location = new System.Drawing.Point(682, 11);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(34, 34);
            this.btnPlay.TabIndex = 13;
            this.btnPlay.TabStop = false;
            this.btnPlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPlay_MouseDown);
            this.btnPlay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPlay_MouseUp);
            // 
            // btnQuestion
            // 
            this.btnQuestion.Image = global::RH.HeadShop.Properties.Resources.btnQuestionNormal;
            this.btnQuestion.Location = new System.Drawing.Point(633, 11);
            this.btnQuestion.Name = "btnQuestion";
            this.btnQuestion.Size = new System.Drawing.Size(34, 34);
            this.btnQuestion.TabIndex = 12;
            this.btnQuestion.TabStop = false;
            this.btnQuestion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnQuestion_MouseDown);
            this.btnQuestion.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnQuestion_MouseUp);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::RH.HeadShop.Properties.Resources.splitter;
            this.pictureBox2.Location = new System.Drawing.Point(607, -4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(10, 97);
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Image = global::RH.HeadShop.Properties.Resources.splitter;
            this.pictureBox3.Location = new System.Drawing.Point(184, -4);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(10, 97);
            this.pictureBox3.TabIndex = 9;
            this.pictureBox3.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(423, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 18);
            this.label8.TabIndex = 7;
            this.label8.Text = "Child";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(308, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 18);
            this.label9.TabIndex = 6;
            this.label9.Text = "Female";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(218, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 18);
            this.label10.TabIndex = 5;
            this.label10.Text = "Male";
            // 
            // RenderTimer
            // 
            this.RenderTimer.Interval = 40;
            this.RenderTimer.Tick += new System.EventHandler(this.RenderTimer_Tick);
            // 
            // rb2048
            // 
            this.rb2048.AutoSize = true;
            this.rb2048.Location = new System.Drawing.Point(318, 194);
            this.rb2048.Name = "rb2048";
            this.rb2048.Size = new System.Drawing.Size(107, 23);
            this.rb2048.TabIndex = 24;
            this.rb2048.Text = "2048 x 2048";
            this.rb2048.UseVisualStyleBackColor = true;
            // 
            // rb1024
            // 
            this.rb1024.AutoSize = true;
            this.rb1024.Checked = true;
            this.rb1024.Location = new System.Drawing.Point(172, 194);
            this.rb1024.Name = "rb1024";
            this.rb1024.Size = new System.Drawing.Size(107, 23);
            this.rb1024.TabIndex = 23;
            this.rb1024.TabStop = true;
            this.rb1024.Text = "1024 x 1024";
            this.rb1024.UseVisualStyleBackColor = true;
            // 
            // rb512
            // 
            this.rb512.AutoSize = true;
            this.rb512.Location = new System.Drawing.Point(30, 194);
            this.rb512.Name = "rb512";
            this.rb512.Size = new System.Drawing.Size(91, 23);
            this.rb512.TabIndex = 22;
            this.rb512.Text = "512 x 512";
            this.rb512.UseVisualStyleBackColor = true;
            // 
            // CheekTimer
            // 
            this.CheekTimer.Interval = 600;
            this.CheekTimer.Tick += new System.EventHandler(this.CheekTimer_Tick);
            // 
            // frmNewProject4PrintAhead
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 561);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.rbSaved);
            this.Controls.Add(this.rbNew);
            this.Controls.Add(this.groupLoadProject);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureTemplate);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(783, 440);
            this.Name = "frmNewProject4PrintAhead";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create new or load project";
            this.Resize += new System.EventHandler(this.frmNewProject4PrintAhead_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureTemplate)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupLoadProject.ResumeLayout(false);
            this.groupLoadProject.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnChild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFemale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnQuestion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }        

        #endregion

        private System.Windows.Forms.PictureBox pictureTemplate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textProjectName;
        private System.Windows.Forms.TextBox textProjectFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnOpenFolderDlg;
        private System.Windows.Forms.Button btnOpenFileDlg;
        private System.Windows.Forms.TextBox textTemplateImage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupLoadProject;
        private System.Windows.Forms.Button btnLoadProject;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textLoadProject;
        private System.Windows.Forms.RadioButton rbNew;
        private System.Windows.Forms.RadioButton rbSaved;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox btnChild;
        private System.Windows.Forms.PictureBox btnFemale;
        private System.Windows.Forms.PictureBox btnMale;
        private System.Windows.Forms.PictureBox btnInfo;
        private System.Windows.Forms.PictureBox btnPlay;
        private System.Windows.Forms.PictureBox btnQuestion;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.Timer RenderTimer;
        private System.Windows.Forms.RadioButton rb2048;
        private System.Windows.Forms.RadioButton rb1024;
        private System.Windows.Forms.RadioButton rb512;
        public System.Windows.Forms.Timer CheekTimer;
        private System.Windows.Forms.RadioButton rbImportObj;
        private System.Windows.Forms.Label label11;
    }
}