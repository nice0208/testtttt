namespace Detecting_System
{
    partial class FrmParent
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmParent));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblVersions = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblFactory = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblMachineID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblProduction = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.deviceListView = new System.Windows.Forms.ListView();
            this.updateDeviceListTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblY = new System.Windows.Forms.Label();
            this.lblX = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblCount2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblToFrmVisionSet = new System.Windows.Forms.Label();
            this.btnToFrmVisionSet = new System.Windows.Forms.Button();
            this.lblInFrmRun = new System.Windows.Forms.Label();
            this.lblInFrmSetUp = new System.Windows.Forms.Label();
            this.lblInLogIn = new System.Windows.Forms.Label();
            this.lbProduceInfo = new System.Windows.Forms.ListBox();
            this.btnToFrmLogIn = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnToFrmSetUp = new System.Windows.Forms.Button();
            this.btnToFrmRun = new System.Windows.Forms.Button();
            this.timerRunningTime = new System.Windows.Forms.Timer(this.components);
            this.ScanPlc = new System.ComponentModel.BackgroundWorker();
            this.Reconnect = new System.ComponentModel.BackgroundWorker();
            this.ScanTrigger = new System.ComponentModel.BackgroundWorker();
            this.lblFunction = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.com1 = new System.IO.Ports.SerialPort(this.components);
            this.BarcodeData = new System.ComponentModel.BackgroundWorker();
            this.lblRunTime = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label1.Location = new System.Drawing.Point(261, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(732, 78);
            this.label1.TabIndex = 515;
            this.label1.Text = "外觀檢查機";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("Georgia", 42F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(260, 78);
            this.label3.TabIndex = 528;
            this.label3.Text = "GSEO";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.OliveDrab;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.Gainsboro;
            this.label8.Location = new System.Drawing.Point(992, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 26);
            this.label8.TabIndex = 535;
            this.label8.Text = "運轉";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.OliveDrab;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.Gainsboro;
            this.label6.Location = new System.Drawing.Point(992, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 26);
            this.label6.TabIndex = 534;
            this.label6.Text = "時間";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.OliveDrab;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Gainsboro;
            this.label4.Location = new System.Drawing.Point(992, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 26);
            this.label4.TabIndex = 533;
            this.label4.Text = "日期";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVersions
            // 
            this.lblVersions.BackColor = System.Drawing.Color.OliveDrab;
            this.lblVersions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVersions.Font = new System.Drawing.Font("Cambria", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersions.ForeColor = System.Drawing.Color.Blue;
            this.lblVersions.Location = new System.Drawing.Point(1057, 78);
            this.lblVersions.Name = "lblVersions";
            this.lblVersions.Size = new System.Drawing.Size(221, 26);
            this.lblVersions.TabIndex = 531;
            this.lblVersions.Text = "V20201208-20201208-20201208";
            this.lblVersions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.OliveDrab;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.Gainsboro;
            this.label7.Location = new System.Drawing.Point(992, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 26);
            this.label7.TabIndex = 532;
            this.label7.Text = "版本";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTime
            // 
            this.lblTime.BackColor = System.Drawing.Color.OliveDrab;
            this.lblTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTime.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(1057, 26);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(221, 26);
            this.lblTime.TabIndex = 530;
            this.lblTime.Text = "10：00：00";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDate
            // 
            this.lblDate.BackColor = System.Drawing.Color.OliveDrab;
            this.lblDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDate.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.lblDate.ForeColor = System.Drawing.Color.White;
            this.lblDate.Location = new System.Drawing.Point(1057, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(221, 26);
            this.lblDate.TabIndex = 529;
            this.lblDate.Text = "2018-08-09 星期四";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFactory
            // 
            this.lblFactory.BackColor = System.Drawing.Color.OliveDrab;
            this.lblFactory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFactory.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.lblFactory.ForeColor = System.Drawing.Color.White;
            this.lblFactory.Location = new System.Drawing.Point(57, 78);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(50, 26);
            this.lblFactory.TabIndex = 545;
            this.lblFactory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.OliveDrab;
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label19.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.label19.ForeColor = System.Drawing.Color.Gainsboro;
            this.label19.Location = new System.Drawing.Point(-1, 78);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(58, 26);
            this.label19.TabIndex = 544;
            this.label19.Text = "廠區";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMachineID
            // 
            this.lblMachineID.BackColor = System.Drawing.Color.OliveDrab;
            this.lblMachineID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMachineID.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.lblMachineID.ForeColor = System.Drawing.Color.White;
            this.lblMachineID.Location = new System.Drawing.Point(212, 78);
            this.lblMachineID.Name = "lblMachineID";
            this.lblMachineID.Size = new System.Drawing.Size(79, 26);
            this.lblMachineID.TabIndex = 543;
            this.lblMachineID.Text = "VI0001";
            this.lblMachineID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.OliveDrab;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Gainsboro;
            this.label2.Location = new System.Drawing.Point(107, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 26);
            this.label2.TabIndex = 542;
            this.label2.Text = "本機編號";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUser
            // 
            this.lblUser.BackColor = System.Drawing.Color.OliveDrab;
            this.lblUser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblUser.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.lblUser.ForeColor = System.Drawing.Color.Blue;
            this.lblUser.Location = new System.Drawing.Point(494, 78);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(79, 26);
            this.lblUser.TabIndex = 538;
            this.lblUser.Text = "-";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblUser.Click += new System.EventHandler(this.lblUser_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.OliveDrab;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.Gainsboro;
            this.label5.Location = new System.Drawing.Point(438, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 26);
            this.label5.TabIndex = 539;
            this.label5.Text = "用戶";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProduction
            // 
            this.lblProduction.BackColor = System.Drawing.Color.OliveDrab;
            this.lblProduction.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProduction.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.lblProduction.ForeColor = System.Drawing.Color.White;
            this.lblProduction.Location = new System.Drawing.Point(346, 78);
            this.lblProduction.Name = "lblProduction";
            this.lblProduction.Size = new System.Drawing.Size(92, 26);
            this.lblProduction.TabIndex = 540;
            this.lblProduction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.OliveDrab;
            this.label18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label18.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.label18.ForeColor = System.Drawing.Color.Gainsboro;
            this.label18.Location = new System.Drawing.Point(291, 78);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(55, 26);
            this.label18.TabIndex = 541;
            this.label18.Text = "機種";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // deviceListView
            // 
            this.deviceListView.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.deviceListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.deviceListView.Location = new System.Drawing.Point(562, 47);
            this.deviceListView.MultiSelect = false;
            this.deviceListView.Name = "deviceListView";
            this.deviceListView.ShowItemToolTips = true;
            this.deviceListView.Size = new System.Drawing.Size(10, 10);
            this.deviceListView.TabIndex = 534;
            this.deviceListView.UseCompatibleStateImageBehavior = false;
            this.deviceListView.View = System.Windows.Forms.View.Tile;
            this.deviceListView.Visible = false;
            // 
            // updateDeviceListTimer
            // 
            this.updateDeviceListTimer.Enabled = true;
            this.updateDeviceListTimer.Interval = 3000;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.lblY);
            this.panel1.Controls.Add(this.lblX);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.lblCount2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lblCount);
            this.panel1.Controls.Add(this.deviceListView);
            this.panel1.Controls.Add(this.lblToFrmVisionSet);
            this.panel1.Controls.Add(this.btnToFrmVisionSet);
            this.panel1.Controls.Add(this.lblInFrmRun);
            this.panel1.Controls.Add(this.lblInFrmSetUp);
            this.panel1.Controls.Add(this.lblInLogIn);
            this.panel1.Controls.Add(this.lbProduceInfo);
            this.panel1.Controls.Add(this.btnToFrmLogIn);
            this.panel1.Controls.Add(this.btnQuit);
            this.panel1.Controls.Add(this.btnToFrmSetUp);
            this.panel1.Controls.Add(this.btnToFrmRun);
            this.panel1.Location = new System.Drawing.Point(1, 949);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1278, 73);
            this.panel1.TabIndex = 547;
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(79, 33);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(11, 12);
            this.lblY.TabIndex = 556;
            this.lblY.Text = "0";
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(79, 13);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(11, 12);
            this.lblX.TabIndex = 556;
            this.lblX.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(56, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 12);
            this.label11.TabIndex = 555;
            this.label11.Text = "Y:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(56, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 555;
            this.label9.Text = "X:";
            // 
            // lblCount2
            // 
            this.lblCount2.AutoSize = true;
            this.lblCount2.Location = new System.Drawing.Point(10, 13);
            this.lblCount2.Name = "lblCount2";
            this.lblCount2.Size = new System.Drawing.Size(11, 12);
            this.lblCount2.TabIndex = 554;
            this.lblCount2.Text = "0";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(42, 23);
            this.button1.TabIndex = 553;
            this.button1.Text = "歸零";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(10, 33);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(11, 12);
            this.lblCount.TabIndex = 552;
            this.lblCount.Text = "0";
            // 
            // lblToFrmVisionSet
            // 
            this.lblToFrmVisionSet.BackColor = System.Drawing.Color.Silver;
            this.lblToFrmVisionSet.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblToFrmVisionSet.Location = new System.Drawing.Point(1068, 12);
            this.lblToFrmVisionSet.Name = "lblToFrmVisionSet";
            this.lblToFrmVisionSet.Size = new System.Drawing.Size(10, 10);
            this.lblToFrmVisionSet.TabIndex = 550;
            // 
            // btnToFrmVisionSet
            // 
            this.btnToFrmVisionSet.BackColor = System.Drawing.Color.LavenderBlush;
            this.btnToFrmVisionSet.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnToFrmVisionSet.Location = new System.Drawing.Point(998, 4);
            this.btnToFrmVisionSet.Name = "btnToFrmVisionSet";
            this.btnToFrmVisionSet.Size = new System.Drawing.Size(90, 66);
            this.btnToFrmVisionSet.TabIndex = 549;
            this.btnToFrmVisionSet.Text = "影像設定";
            this.btnToFrmVisionSet.UseVisualStyleBackColor = false;
            this.btnToFrmVisionSet.Click += new System.EventHandler(this.btnImageSetUp_Click);
            // 
            // lblInFrmRun
            // 
            this.lblInFrmRun.BackColor = System.Drawing.Color.Silver;
            this.lblInFrmRun.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblInFrmRun.Location = new System.Drawing.Point(1162, 11);
            this.lblInFrmRun.Name = "lblInFrmRun";
            this.lblInFrmRun.Size = new System.Drawing.Size(10, 10);
            this.lblInFrmRun.TabIndex = 537;
            // 
            // lblInFrmSetUp
            // 
            this.lblInFrmSetUp.BackColor = System.Drawing.Color.Silver;
            this.lblInFrmSetUp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblInFrmSetUp.Location = new System.Drawing.Point(979, 12);
            this.lblInFrmSetUp.Name = "lblInFrmSetUp";
            this.lblInFrmSetUp.Size = new System.Drawing.Size(10, 10);
            this.lblInFrmSetUp.TabIndex = 535;
            // 
            // lblInLogIn
            // 
            this.lblInLogIn.BackColor = System.Drawing.Color.Silver;
            this.lblInLogIn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblInLogIn.Location = new System.Drawing.Point(887, 12);
            this.lblInLogIn.Name = "lblInLogIn";
            this.lblInLogIn.Size = new System.Drawing.Size(10, 10);
            this.lblInLogIn.TabIndex = 534;
            // 
            // lbProduceInfo
            // 
            this.lbProduceInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbProduceInfo.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbProduceInfo.FormattingEnabled = true;
            this.lbProduceInfo.HorizontalScrollbar = true;
            this.lbProduceInfo.ItemHeight = 14;
            this.lbProduceInfo.Location = new System.Drawing.Point(11, 4);
            this.lbProduceInfo.Name = "lbProduceInfo";
            this.lbProduceInfo.Size = new System.Drawing.Size(10, 2);
            this.lbProduceInfo.TabIndex = 533;
            // 
            // btnToFrmLogIn
            // 
            this.btnToFrmLogIn.BackColor = System.Drawing.Color.LavenderBlush;
            this.btnToFrmLogIn.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.btnToFrmLogIn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Blue;
            this.btnToFrmLogIn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnToFrmLogIn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnToFrmLogIn.Location = new System.Drawing.Point(815, 3);
            this.btnToFrmLogIn.Name = "btnToFrmLogIn";
            this.btnToFrmLogIn.Size = new System.Drawing.Size(90, 66);
            this.btnToFrmLogIn.TabIndex = 7;
            this.btnToFrmLogIn.Text = "登入";
            this.btnToFrmLogIn.UseVisualStyleBackColor = false;
            this.btnToFrmLogIn.Click += new System.EventHandler(this.btnToFrmLogIn_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.BackColor = System.Drawing.Color.LavenderBlush;
            this.btnQuit.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuit.ForeColor = System.Drawing.Color.Red;
            this.btnQuit.Location = new System.Drawing.Point(1180, 3);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(90, 66);
            this.btnQuit.TabIndex = 5;
            this.btnQuit.Text = "離開";
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnToFrmSetUp
            // 
            this.btnToFrmSetUp.BackColor = System.Drawing.Color.LavenderBlush;
            this.btnToFrmSetUp.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnToFrmSetUp.Location = new System.Drawing.Point(907, 4);
            this.btnToFrmSetUp.Name = "btnToFrmSetUp";
            this.btnToFrmSetUp.Size = new System.Drawing.Size(90, 66);
            this.btnToFrmSetUp.TabIndex = 2;
            this.btnToFrmSetUp.Text = "設定";
            this.btnToFrmSetUp.UseVisualStyleBackColor = false;
            this.btnToFrmSetUp.Click += new System.EventHandler(this.btnToFrmSetUp_Click);
            // 
            // btnToFrmRun
            // 
            this.btnToFrmRun.BackColor = System.Drawing.Color.LavenderBlush;
            this.btnToFrmRun.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnToFrmRun.Location = new System.Drawing.Point(1089, 3);
            this.btnToFrmRun.Name = "btnToFrmRun";
            this.btnToFrmRun.Size = new System.Drawing.Size(90, 66);
            this.btnToFrmRun.TabIndex = 1;
            this.btnToFrmRun.Text = "運行";
            this.btnToFrmRun.UseVisualStyleBackColor = false;
            this.btnToFrmRun.Click += new System.EventHandler(this.btnToFrmRun_Click);
            // 
            // timerRunningTime
            // 
            this.timerRunningTime.Interval = 1000;
            this.timerRunningTime.Tick += new System.EventHandler(this.timerRunningTime_Tick);
            // 
            // ScanPlc
            // 
            this.ScanPlc.WorkerSupportsCancellation = true;
            this.ScanPlc.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ScanPlc_DoWork);
            // 
            // Reconnect
            // 
            this.Reconnect.WorkerSupportsCancellation = true;
            this.Reconnect.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Reconnect_DoWork);
            // 
            // ScanTrigger
            // 
            this.ScanTrigger.WorkerSupportsCancellation = true;
            this.ScanTrigger.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ScanTrigger_DoWork);
            // 
            // lblFunction
            // 
            this.lblFunction.BackColor = System.Drawing.Color.OliveDrab;
            this.lblFunction.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFunction.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.lblFunction.ForeColor = System.Drawing.Color.Blue;
            this.lblFunction.Location = new System.Drawing.Point(640, 78);
            this.lblFunction.Name = "lblFunction";
            this.lblFunction.Size = new System.Drawing.Size(353, 26);
            this.lblFunction.TabIndex = 549;
            this.lblFunction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.OliveDrab;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.label10.ForeColor = System.Drawing.Color.Gainsboro;
            this.label10.Location = new System.Drawing.Point(573, 78);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 26);
            this.label10.TabIndex = 550;
            this.label10.Text = "功能";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BarcodeData
            // 
            this.BarcodeData.WorkerSupportsCancellation = true;
            this.BarcodeData.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BarcodeData_DoWork);
            // 
            // lblRunTime
            // 
            this.lblRunTime.BackColor = System.Drawing.Color.OliveDrab;
            this.lblRunTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblRunTime.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.lblRunTime.ForeColor = System.Drawing.Color.White;
            this.lblRunTime.Location = new System.Drawing.Point(1057, 52);
            this.lblRunTime.Name = "lblRunTime";
            this.lblRunTime.Size = new System.Drawing.Size(221, 26);
            this.lblRunTime.TabIndex = 536;
            this.lblRunTime.Text = " 0天 0时 0分 0秒";
            this.lblRunTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmParent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 1024);
            this.Controls.Add(this.lblFunction);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblFactory);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.lblMachineID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblProduction);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.lblRunTime);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblVersions);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.Name = "FrmParent";
            this.Text = "外觀檢查機";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmParent_FormClosing);
            this.Load += new System.EventHandler(this.FrmParent_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblDate;
        public System.Windows.Forms.Label lblFactory;
        private System.Windows.Forms.Label label19;
        public System.Windows.Forms.Label lblMachineID;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label lblProduction;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ListView deviceListView;
        private System.Windows.Forms.Timer updateDeviceListTimer;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label lblInFrmRun;
        public System.Windows.Forms.Label lblInFrmSetUp;
        public System.Windows.Forms.Label lblInLogIn;
        public System.Windows.Forms.ListBox lbProduceInfo;
        public System.Windows.Forms.Button btnToFrmLogIn;
        private System.Windows.Forms.Button btnQuit;
        public System.Windows.Forms.Button btnToFrmSetUp;
        public System.Windows.Forms.Button btnToFrmRun;
        private System.Windows.Forms.Timer timerRunningTime;
        public System.Windows.Forms.Button btnToFrmVisionSet;
        public System.Windows.Forms.Label lblToFrmVisionSet;
        private System.ComponentModel.BackgroundWorker ScanPlc;
        private System.ComponentModel.BackgroundWorker Reconnect;
        private System.ComponentModel.BackgroundWorker ScanTrigger;
        public System.Windows.Forms.Label lblFunction;
        private System.Windows.Forms.Label label10;
        public System.IO.Ports.SerialPort com1;
        private System.ComponentModel.BackgroundWorker BarcodeData;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblCount2;
        private System.Windows.Forms.Label lblRunTime;
        public System.Windows.Forms.Label lblVersions;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
    }
}

