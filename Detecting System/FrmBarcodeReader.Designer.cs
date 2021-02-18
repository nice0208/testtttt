namespace Detecting_System
{
    partial class FrmBarcodeReader
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
            this.cbTransformOpen = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbProduction = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbOkAddition = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbVerification = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRangeSet = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.label62 = new System.Windows.Forms.Label();
            this.btnLightSave = new System.Windows.Forms.Button();
            this.tbLightSet_1 = new System.Windows.Forms.TrackBar();
            this.btnOn_4 = new System.Windows.Forms.Button();
            this.tbLightSet_4 = new System.Windows.Forms.TrackBar();
            this.label64 = new System.Windows.Forms.Label();
            this.tbLightSet_2 = new System.Windows.Forms.TrackBar();
            this.tbLightSet_3 = new System.Windows.Forms.TrackBar();
            this.label59 = new System.Windows.Forms.Label();
            this.btnOn_3 = new System.Windows.Forms.Button();
            this.btnOn_1 = new System.Windows.Forms.Button();
            this.label65 = new System.Windows.Forms.Label();
            this.nudLightSet_2 = new System.Windows.Forms.NumericUpDown();
            this.nudLightSet_1 = new System.Windows.Forms.NumericUpDown();
            this.nudLightSet_3 = new System.Windows.Forms.NumericUpDown();
            this.btnOn_2 = new System.Windows.Forms.Button();
            this.nudLightSet_4 = new System.Windows.Forms.NumericUpDown();
            this.btnShowOriginalImage = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOpenImage = new System.Windows.Forms.Button();
            this.btnImageSave = new System.Windows.Forms.Button();
            this.btnImageProPlus = new System.Windows.Forms.Button();
            this.btnOneShot = new System.Windows.Forms.Button();
            this.btnContinueShot = new System.Windows.Forms.Button();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.TimerUI = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox24.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightSet_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightSet_4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightSet_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightSet_3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLightSet_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLightSet_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLightSet_3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLightSet_4)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.cbTransformOpen);
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.btnShowOriginalImage);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.hWindowControl1);
            this.panel1.Location = new System.Drawing.Point(0, 102);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 872);
            this.panel1.TabIndex = 1;
            // 
            // cbTransformOpen
            // 
            this.cbTransformOpen.AutoSize = true;
            this.cbTransformOpen.Location = new System.Drawing.Point(450, 809);
            this.cbTransformOpen.Name = "cbTransformOpen";
            this.cbTransformOpen.Size = new System.Drawing.Size(72, 16);
            this.cbTransformOpen.TabIndex = 17;
            this.cbTransformOpen.Text = "放大縮小";
            this.cbTransformOpen.UseVisualStyleBackColor = true;
            this.cbTransformOpen.CheckedChanged += new System.EventHandler(this.cbTransformOpen_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(610, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(385, 800);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(377, 774);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "影像設置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cmbProduction);
            this.groupBox3.Location = new System.Drawing.Point(7, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(364, 74);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "目標";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "檢測目標:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbProduction
            // 
            this.cmbProduction.FormattingEnabled = true;
            this.cmbProduction.Items.AddRange(new object[] {
            "一般Barcode",
            "LensBarcode(Q Code)",
            "LensBarcode(GC Code)"});
            this.cmbProduction.Location = new System.Drawing.Point(95, 33);
            this.cmbProduction.Name = "cmbProduction";
            this.cmbProduction.Size = new System.Drawing.Size(121, 20);
            this.cmbProduction.TabIndex = 2;
            this.cmbProduction.SelectedIndexChanged += new System.EventHandler(this.cmbProduction_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cmbOkAddition);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cbVerification);
            this.groupBox2.Location = new System.Drawing.Point(7, 167);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(364, 175);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "掃碼驗證";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(7, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "合格附加條件:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbOkAddition
            // 
            this.cmbOkAddition.FormattingEnabled = true;
            this.cmbOkAddition.Items.AddRange(new object[] {
            "無",
            ">=D",
            ">=C",
            ">=B",
            ">=A",
            "",
            "",
            ""});
            this.cmbOkAddition.Location = new System.Drawing.Point(107, 91);
            this.cmbOkAddition.Name = "cmbOkAddition";
            this.cmbOkAddition.Size = new System.Drawing.Size(121, 20);
            this.cmbOkAddition.TabIndex = 2;
            this.cmbOkAddition.SelectedIndexChanged += new System.EventHandler(this.cmbOkAddition_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Location = new System.Drawing.Point(143, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(195, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "* MeanLight需調整到0.7~0.86之間\r\n否則無法驗證等級\r\n";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbVerification
            // 
            this.cbVerification.AutoSize = true;
            this.cbVerification.Location = new System.Drawing.Point(9, 30);
            this.cbVerification.Name = "cbVerification";
            this.cbVerification.Size = new System.Drawing.Size(108, 16);
            this.cbVerification.TabIndex = 0;
            this.cbVerification.Text = "開啟掃碼驗證器";
            this.cbVerification.UseVisualStyleBackColor = true;
            this.cbVerification.CheckedChanged += new System.EventHandler(this.cbVerification_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRangeSet);
            this.groupBox1.Location = new System.Drawing.Point(7, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 74);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "解碼區域";
            // 
            // btnRangeSet
            // 
            this.btnRangeSet.Location = new System.Drawing.Point(6, 22);
            this.btnRangeSet.Name = "btnRangeSet";
            this.btnRangeSet.Size = new System.Drawing.Size(75, 41);
            this.btnRangeSet.TabIndex = 0;
            this.btnRangeSet.Text = "掃碼區域\r\n設置";
            this.btnRangeSet.UseVisualStyleBackColor = true;
            this.btnRangeSet.Click += new System.EventHandler(this.btnRangeSet_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox24);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(377, 774);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "光源設定";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox24
            // 
            this.groupBox24.Controls.Add(this.label62);
            this.groupBox24.Controls.Add(this.btnLightSave);
            this.groupBox24.Controls.Add(this.tbLightSet_1);
            this.groupBox24.Controls.Add(this.btnOn_4);
            this.groupBox24.Controls.Add(this.tbLightSet_4);
            this.groupBox24.Controls.Add(this.label64);
            this.groupBox24.Controls.Add(this.tbLightSet_2);
            this.groupBox24.Controls.Add(this.tbLightSet_3);
            this.groupBox24.Controls.Add(this.label59);
            this.groupBox24.Controls.Add(this.btnOn_3);
            this.groupBox24.Controls.Add(this.btnOn_1);
            this.groupBox24.Controls.Add(this.label65);
            this.groupBox24.Controls.Add(this.nudLightSet_2);
            this.groupBox24.Controls.Add(this.nudLightSet_1);
            this.groupBox24.Controls.Add(this.nudLightSet_3);
            this.groupBox24.Controls.Add(this.btnOn_2);
            this.groupBox24.Controls.Add(this.nudLightSet_4);
            this.groupBox24.Location = new System.Drawing.Point(3, 3);
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.Size = new System.Drawing.Size(357, 300);
            this.groupBox24.TabIndex = 88;
            this.groupBox24.TabStop = false;
            this.groupBox24.Text = "光源設定";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(12, 205);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(35, 12);
            this.label62.TabIndex = 36;
            this.label62.Text = "光源4";
            // 
            // btnLightSave
            // 
            this.btnLightSave.Location = new System.Drawing.Point(276, 255);
            this.btnLightSave.Name = "btnLightSave";
            this.btnLightSave.Size = new System.Drawing.Size(75, 23);
            this.btnLightSave.TabIndex = 77;
            this.btnLightSave.Text = "儲存設定";
            this.btnLightSave.UseVisualStyleBackColor = true;
            this.btnLightSave.Click += new System.EventHandler(this.btnLightSave_Click);
            // 
            // tbLightSet_1
            // 
            this.tbLightSet_1.BackColor = System.Drawing.Color.White;
            this.tbLightSet_1.Location = new System.Drawing.Point(6, 39);
            this.tbLightSet_1.Maximum = 255;
            this.tbLightSet_1.Name = "tbLightSet_1";
            this.tbLightSet_1.Size = new System.Drawing.Size(175, 45);
            this.tbLightSet_1.TabIndex = 30;
            this.tbLightSet_1.ValueChanged += new System.EventHandler(this.tbLightSet_1_ValueChanged);
            // 
            // btnOn_4
            // 
            this.btnOn_4.Location = new System.Drawing.Point(186, 252);
            this.btnOn_4.Name = "btnOn_4";
            this.btnOn_4.Size = new System.Drawing.Size(77, 26);
            this.btnOn_4.TabIndex = 81;
            this.btnOn_4.UseVisualStyleBackColor = true;
            this.btnOn_4.Click += new System.EventHandler(this.btnOn_4_Click);
            // 
            // tbLightSet_4
            // 
            this.tbLightSet_4.BackColor = System.Drawing.Color.White;
            this.tbLightSet_4.Location = new System.Drawing.Point(6, 225);
            this.tbLightSet_4.Maximum = 255;
            this.tbLightSet_4.Name = "tbLightSet_4";
            this.tbLightSet_4.Size = new System.Drawing.Size(174, 45);
            this.tbLightSet_4.TabIndex = 31;
            this.tbLightSet_4.ValueChanged += new System.EventHandler(this.tbLightSet_4_ValueChanged);
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(12, 22);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(35, 12);
            this.label64.TabIndex = 37;
            this.label64.Text = "光源1";
            // 
            // tbLightSet_2
            // 
            this.tbLightSet_2.BackColor = System.Drawing.Color.White;
            this.tbLightSet_2.Location = new System.Drawing.Point(6, 99);
            this.tbLightSet_2.Maximum = 255;
            this.tbLightSet_2.Name = "tbLightSet_2";
            this.tbLightSet_2.Size = new System.Drawing.Size(174, 45);
            this.tbLightSet_2.TabIndex = 31;
            this.tbLightSet_2.ValueChanged += new System.EventHandler(this.tbLightSet_2_ValueChanged);
            // 
            // tbLightSet_3
            // 
            this.tbLightSet_3.BackColor = System.Drawing.Color.White;
            this.tbLightSet_3.Location = new System.Drawing.Point(6, 165);
            this.tbLightSet_3.Maximum = 255;
            this.tbLightSet_3.Name = "tbLightSet_3";
            this.tbLightSet_3.Size = new System.Drawing.Size(175, 45);
            this.tbLightSet_3.TabIndex = 30;
            this.tbLightSet_3.ValueChanged += new System.EventHandler(this.tbLightSet_3_ValueChanged);
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(12, 147);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(35, 12);
            this.label59.TabIndex = 37;
            this.label59.Text = "光源3";
            // 
            // btnOn_3
            // 
            this.btnOn_3.Location = new System.Drawing.Point(186, 191);
            this.btnOn_3.Name = "btnOn_3";
            this.btnOn_3.Size = new System.Drawing.Size(77, 26);
            this.btnOn_3.TabIndex = 80;
            this.btnOn_3.UseVisualStyleBackColor = true;
            this.btnOn_3.Click += new System.EventHandler(this.btnOn_3_Click);
            // 
            // btnOn_1
            // 
            this.btnOn_1.Location = new System.Drawing.Point(186, 64);
            this.btnOn_1.Name = "btnOn_1";
            this.btnOn_1.Size = new System.Drawing.Size(77, 26);
            this.btnOn_1.TabIndex = 78;
            this.btnOn_1.UseVisualStyleBackColor = true;
            this.btnOn_1.Click += new System.EventHandler(this.btnOn_1_Click);
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(12, 84);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(35, 12);
            this.label65.TabIndex = 36;
            this.label65.Text = "光源2";
            // 
            // nudLightSet_2
            // 
            this.nudLightSet_2.Location = new System.Drawing.Point(186, 99);
            this.nudLightSet_2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudLightSet_2.Name = "nudLightSet_2";
            this.nudLightSet_2.Size = new System.Drawing.Size(62, 21);
            this.nudLightSet_2.TabIndex = 42;
            this.nudLightSet_2.ValueChanged += new System.EventHandler(this.nudLightSet_2_ValueChanged);
            // 
            // nudLightSet_1
            // 
            this.nudLightSet_1.Location = new System.Drawing.Point(187, 37);
            this.nudLightSet_1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudLightSet_1.Name = "nudLightSet_1";
            this.nudLightSet_1.Size = new System.Drawing.Size(62, 21);
            this.nudLightSet_1.TabIndex = 41;
            this.nudLightSet_1.ValueChanged += new System.EventHandler(this.nudLightSet_1_ValueChanged);
            // 
            // nudLightSet_3
            // 
            this.nudLightSet_3.Location = new System.Drawing.Point(187, 163);
            this.nudLightSet_3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudLightSet_3.Name = "nudLightSet_3";
            this.nudLightSet_3.Size = new System.Drawing.Size(62, 21);
            this.nudLightSet_3.TabIndex = 41;
            this.nudLightSet_3.ValueChanged += new System.EventHandler(this.nudLightSet_3_ValueChanged);
            // 
            // btnOn_2
            // 
            this.btnOn_2.Location = new System.Drawing.Point(187, 129);
            this.btnOn_2.Name = "btnOn_2";
            this.btnOn_2.Size = new System.Drawing.Size(77, 26);
            this.btnOn_2.TabIndex = 79;
            this.btnOn_2.UseVisualStyleBackColor = true;
            this.btnOn_2.Click += new System.EventHandler(this.btnOn_2_Click);
            // 
            // nudLightSet_4
            // 
            this.nudLightSet_4.Location = new System.Drawing.Point(186, 225);
            this.nudLightSet_4.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudLightSet_4.Name = "nudLightSet_4";
            this.nudLightSet_4.Size = new System.Drawing.Size(62, 21);
            this.nudLightSet_4.TabIndex = 42;
            this.nudLightSet_4.ValueChanged += new System.EventHandler(this.nudLightSet_4_ValueChanged);
            // 
            // btnShowOriginalImage
            // 
            this.btnShowOriginalImage.Location = new System.Drawing.Point(528, 809);
            this.btnShowOriginalImage.Name = "btnShowOriginalImage";
            this.btnShowOriginalImage.Size = new System.Drawing.Size(75, 23);
            this.btnShowOriginalImage.TabIndex = 13;
            this.btnShowOriginalImage.Text = "顯示原圖";
            this.btnShowOriginalImage.UseVisualStyleBackColor = true;
            this.btnShowOriginalImage.Click += new System.EventHandler(this.btnShowOriginalImage_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnOpenImage);
            this.panel2.Controls.Add(this.btnImageSave);
            this.panel2.Controls.Add(this.btnImageProPlus);
            this.panel2.Controls.Add(this.btnOneShot);
            this.panel2.Controls.Add(this.btnContinueShot);
            this.panel2.Location = new System.Drawing.Point(1001, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(276, 774);
            this.panel2.TabIndex = 11;
            // 
            // btnOpenImage
            // 
            this.btnOpenImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnOpenImage.Location = new System.Drawing.Point(46, 156);
            this.btnOpenImage.Name = "btnOpenImage";
            this.btnOpenImage.Size = new System.Drawing.Size(75, 63);
            this.btnOpenImage.TabIndex = 93;
            this.btnOpenImage.Text = "開啟圖片";
            this.btnOpenImage.UseVisualStyleBackColor = false;
            this.btnOpenImage.Click += new System.EventHandler(this.btnOpenImage_Click);
            // 
            // btnImageSave
            // 
            this.btnImageSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnImageSave.Location = new System.Drawing.Point(127, 87);
            this.btnImageSave.Name = "btnImageSave";
            this.btnImageSave.Size = new System.Drawing.Size(75, 63);
            this.btnImageSave.TabIndex = 5;
            this.btnImageSave.Text = "儲存照片";
            this.btnImageSave.UseVisualStyleBackColor = false;
            this.btnImageSave.Click += new System.EventHandler(this.btnImageSave_Click);
            // 
            // btnImageProPlus
            // 
            this.btnImageProPlus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnImageProPlus.Location = new System.Drawing.Point(46, 87);
            this.btnImageProPlus.Name = "btnImageProPlus";
            this.btnImageProPlus.Size = new System.Drawing.Size(75, 63);
            this.btnImageProPlus.TabIndex = 4;
            this.btnImageProPlus.Text = "掃碼";
            this.btnImageProPlus.UseVisualStyleBackColor = false;
            this.btnImageProPlus.Click += new System.EventHandler(this.btnImageProPlus_Click);
            // 
            // btnOneShot
            // 
            this.btnOneShot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnOneShot.Location = new System.Drawing.Point(46, 18);
            this.btnOneShot.Name = "btnOneShot";
            this.btnOneShot.Size = new System.Drawing.Size(75, 63);
            this.btnOneShot.TabIndex = 1;
            this.btnOneShot.Text = "拍攝單張";
            this.btnOneShot.UseVisualStyleBackColor = false;
            this.btnOneShot.Click += new System.EventHandler(this.btnOneShot_Click);
            // 
            // btnContinueShot
            // 
            this.btnContinueShot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnContinueShot.Location = new System.Drawing.Point(127, 18);
            this.btnContinueShot.Name = "btnContinueShot";
            this.btnContinueShot.Size = new System.Drawing.Size(75, 63);
            this.btnContinueShot.TabIndex = 2;
            this.btnContinueShot.Text = "預覽";
            this.btnContinueShot.UseVisualStyleBackColor = false;
            this.btnContinueShot.Click += new System.EventHandler(this.btnContinueShot_Click);
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(3, 3);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(600, 800);
            this.hWindowControl1.TabIndex = 9;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(600, 800);
            this.hWindowControl1.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseDown);
            this.hWindowControl1.HMouseUp += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseUp);
            this.hWindowControl1.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseWheel);
            // 
            // TimerUI
            // 
            this.TimerUI.Enabled = true;
            this.TimerUI.Tick += new System.EventHandler(this.TimerUI_Tick);
            // 
            // FrmBarcodeReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 970);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmBarcodeReader";
            this.Text = "FrmBarcodeReader";
            this.Load += new System.EventHandler(this.FrmBarcodeReader_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox24.ResumeLayout(false);
            this.groupBox24.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightSet_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightSet_4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightSet_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbLightSet_3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLightSet_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLightSet_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLightSet_3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLightSet_4)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRangeSet;
        private System.Windows.Forms.Button btnShowOriginalImage;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnImageSave;
        private System.Windows.Forms.Button btnImageProPlus;
        private System.Windows.Forms.Button btnOneShot;
        private System.Windows.Forms.Button btnContinueShot;
        public HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox24;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Button btnLightSave;
        public System.Windows.Forms.TrackBar tbLightSet_1;
        public System.Windows.Forms.Button btnOn_4;
        public System.Windows.Forms.TrackBar tbLightSet_4;
        private System.Windows.Forms.Label label64;
        public System.Windows.Forms.TrackBar tbLightSet_2;
        public System.Windows.Forms.TrackBar tbLightSet_3;
        private System.Windows.Forms.Label label59;
        public System.Windows.Forms.Button btnOn_3;
        public System.Windows.Forms.Button btnOn_1;
        private System.Windows.Forms.Label label65;
        public System.Windows.Forms.NumericUpDown nudLightSet_2;
        public System.Windows.Forms.NumericUpDown nudLightSet_1;
        public System.Windows.Forms.NumericUpDown nudLightSet_3;
        public System.Windows.Forms.Button btnOn_2;
        public System.Windows.Forms.NumericUpDown nudLightSet_4;
        private System.Windows.Forms.Button btnOpenImage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbVerification;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbProduction;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer TimerUI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbOkAddition;
        private System.Windows.Forms.CheckBox cbTransformOpen;
    }
}