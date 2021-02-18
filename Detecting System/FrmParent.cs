using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;
using System.Drawing.Imaging;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Detecting_System
{
    public partial class FrmParent : Form
    {
        FrmVDIVisionSet VDIVisionSet;
        Frm8982SomaVisionSet SomaVisionSet;
        FrmVisionSet VisionSet;
        FrmSomaDetectionVS SomaDetectionVS;
        FrmVDICoatingVS VDICoatingVS;
        FrmVDI_NIR VDI_NIR;
        FrmVDI_INK VDI_INK;
        FrmClassifier Classifier;
        FrmBarcodeReader BR;
        FrmBarcodeReaderPlus BRPlus;
        FrmLensCrack_AVI LensCrack_AVI;
        Frm8958BPFOCRVision Lens_Mold_Cave;
        FrmLensCarry LensCarry;
        FrmRun Run;
        FrmSetUp setup;
        public MyBasler myBasler = new MyBasler();
        public MyHikvision myHikvision = new MyHikvision();

        public My.LensCarry m_LensCarry = new My.LensCarry(); 
        //當前的介面
        public HWindow HWindow = new HWindow();
        //視覺設定介面 true/ 自動介面 false
        //是否在自動介面
        public static bool On_Auto_InterFace = true;
        
        public static bool quit = false;
        public static bool ColorizedImage = false;
        public static bool ShowCross = false;
        AlarmMessage m_AlarmMessage = new AlarmMessage();
        //Light變量
        bool keepReading = true;

        Int64 TimeRunning = 0;
        int Days = 0, Hours = 0, Minutes = 0, Seconds = 0;
        public Socket Enet;
        IPAddress ip = IPAddress.Parse("192.18.2.2");
      
        public bool BarcodeInspect = false;//掃碼處理
        public static int count = 0;
        public static int count2 = 0;
        #region 去除X
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        #endregion
        public FrmParent()
        {
            InitializeComponent();
        }

        #region//CCD參數
        /// <summary>
        /// 關閉相機
        /// </summary>
        public void DestroyCamera()
        {
            if (CCD.IsConnected)
            {
                if (CCD.CCDBrand == 0)
                {
                    myBasler.DestroyCamera();
                }
                else if (CCD.CCDBrand == 1)
                {
                    myHikvision.DestroyCamera();
                }
            }
        }
        /// <summary>
        /// 單張拍攝
        /// </summary>
        public void OneShot()
        {
            if (CCD.IsConnected)
            {
                if (CCD.CCDBrand == 0)
                {
                    myBasler.OneShot();
                }
                else if (CCD.CCDBrand == 1)
                {
                    myHikvision.OneShot();
                }
            }
        }
        /// <summary>
        /// 連續拍攝
        /// </summary>
        public void ContinuousShot()
        {
            if (CCD.IsConnected)
            {
                if (CCD.CCDBrand == 0)
                {
                    myBasler.ContinuousShot();
                }
                else if (CCD.CCDBrand == 1)
                {
                    myHikvision.ContinuousShot();
                }
            }
        }
        /// <summary>
        /// 停止連續拍攝
        /// </summary>
        public void Stop()
        {
            if (CCD.IsConnected)
            {
                if (CCD.CCDBrand == 0)
                {
                    myBasler.Stop();
                }
                else if (CCD.CCDBrand == 1)
                {
                    myHikvision.Stop();
                }
            }
        }
        /// <summary>
        /// 獲取增益,曝光參數
        /// </summary>
        public void GetParam()
        {
            if (CCD.IsConnected)
            {
                if (CCD.CCDBrand == 0)
                {
                    CCD.GainMaximum = myBasler.GainMaximum;
                    CCD.GainMinimum = myBasler.GainMinimum;
                    CCD.ExposureTimeMaximum = myBasler.ExposureTimeMaximum;
                    CCD.ExposureTimeMinimum = myBasler.ExposureTimeMinimum;
                }
                else if (CCD.CCDBrand == 1)
                {
                    CCD.GainMaximum = myHikvision.GainMaximum;
                    CCD.GainMinimum = myHikvision.GainMinimum;
                    CCD.ExposureTimeMaximum = myHikvision.ExposureTimeMaximum;
                    CCD.ExposureTimeMinimum = myHikvision.ExposureTimeMinimum;
                }
            }
        }
        /// <summary>
        /// 設置增益
        /// </summary>
        /// <param name="Value"></param>
        public void SetGain(double Value)
        {
            if (CCD.IsConnected)
            {
                if (CCD.CCDBrand == 0)
                {
                    myBasler.SetGain(Value);
                }
                else if (CCD.CCDBrand == 1)
                {
                    myHikvision.SetGain(Value);
                }
            }
        }
        /// <summary>
        /// 設置曝光
        /// </summary>
        /// <param name="Value"></param>
        public void SetExposureTime(double Value)
        {
            if (CCD.IsConnected)
            {
                if (CCD.CCDBrand == 0)
                {
                    myBasler.SetExposureTime(Value);
                }
                else if (CCD.CCDBrand == 1)
                {
                    myHikvision.SetExposureTime(Value);
                }
            }
        }
        /// <summary>
        /// 設置IO觸發
        /// </summary>
        public void SetTriggerMode_Line1()
        {
            if (CCD.IsConnected)
            {
                if (CCD.CCDBrand == 0)
                {
                    myBasler.SetTriggerMode_Line1();
                }
                else if (CCD.CCDBrand == 1)
                {
                    myHikvision.SetTriggerMode_Line1();
                }
            }
        }
        /// <summary>
        /// 設置軟體觸發
        /// </summary>
        public void SetTriggerMode_Software()
        {
            if (CCD.IsConnected)
            {
                if (CCD.CCDBrand == 0)
                {
                    myBasler.Stop();
                    myBasler.SetTriggerMode_Software();
                }
                else if (CCD.CCDBrand == 1)
                {
                    myHikvision.SetTriggerMode_Software();
                }
            }
        }
        #endregion
        #region 報警參數

        public bool[] Alarm;
        public static string[] AlarmEN ={"A0000",
                                        "A0001",
                                        "A0002",
                                        "A0003",
                                        "A0004",
                                        "A0005",
                                        "A0006",
                                        "A0007",
                                        "A0008",
                                        "A0009",
                                        "A0010",
                                        "A0011",
                                        "A0012",
                                        "A0013",
                                        "A0014",
                                        "A0015"};
        public static string[] AlarmCh = {"X轴ALM",
                                             "Y轴ALM",
                                             "Z轴ALM",
                                             "R轴ALM",
                                             "X轴过限位",
                                             "Y轴过限位",
                                             "影像异常",
                                             "影像超时",
                                             "吸嘴吸真空异常",
                                             "Tray扫码异常",
                                             "Tray扫码超时",
                                             "",
                                             "",
                                             "",
                                             "",
                                             ""};   

        #endregion


        private void FrmParent_Load(object sender, EventArgs e) 
        {
              Protocol.Versions_PC = "20200203";//PC版本

            Run = new FrmRun(this);
            Run.Show();
            ReadSetPara();
            setup = new FrmSetUp(this);
            if (Sys.ReadBarcodeLog)
            {
                Run.lblDataBankConnect.Visible = true;
                Run.picDataBankConnect.Visible = true;
            }
            switch (Sys.Function)
            {
                case 0:
                    {
                        Run.picLightConnect.Visible = true;
                        Run.lblLightConnect.Visible = true;
                        ReadVDIPara();
                        VDIVisionSet = new FrmVDIVisionSet(this);
                        VDIVisionSet.ReadPara();
                        LightSetting();
                        LoadParaLight();
                        LightOn_All();
                        ShowCross = true;
                        break;
                    }
                case 1:
                    {
                        ReadPara();
                        LightSetting();
                        LoadParaLight();
                        LightOn_All();
                        VisionSet = new FrmVisionSet(this);
                        VisionSet.ReadPara();
                        Run.picLightConnect.Visible = true;
                        Run.lblLightConnect.Visible = true;
                        break;
                    }
                case 2:
                    {
                        ReadSomaPara();
                        SomaVisionSet = new Frm8982SomaVisionSet(this);
                        SomaVisionSet.ReadPara();
                        break;
                    }
                case 3:
                    {
                        ReadSomaDetectionVSPara();
                        SomaDetectionVS = new FrmSomaDetectionVS(this);
                        SomaDetectionVS.ReadPara();
                        break;
                    }
                case 4:
                    {
                        Run.picLightConnect.Visible = true;
                        Run.lblLightConnect.Visible = true;
                        ReadVDICoatingPara();
                        VDICoatingVS = new FrmVDICoatingVS(this);
                        VDICoatingVS.ReadPara();
                        LightSetting();
                        LoadParaLight();
                        LightOn_All();
                        ColorizedImage = true;
                        break;
                    }
                case 5:
                    {
                        Run.picReaderConnect.Visible = true;
                        Run.lblReaderConnect.Visible = true;
                        Run.picLightConnect.Visible = true;
                        Run.lblLightConnect.Visible = true;
                        ReadNIRPara();
                        VDI_NIR = new FrmVDI_NIR(this);
                        VDI_NIR.ReadPara();
                        VDI_NIR.ReadSysIni();

                        LoadReaderPara();
                        LightSetting();
                        LoadParaLight();
                        LightOn_All();
                        BarcodeData.RunWorkerAsync();
                        //Run.cbReadBarrelBarcode.Visible = true;
                        Run.cbReadBarrelBarcode.Checked = My.NIR.ReadBarrelBarcode;
                        break;
                    }
                case 6:
                    {
                        Run.picLightConnect.Visible = true;
                        Run.lblLightConnect.Visible = true;
                        ReadVDI_InkPara();
                        VDI_INK = new FrmVDI_INK(this);
                        VDI_INK.ReadPara();
                        LightSetting();
                        LoadParaLight();
                        LightOn_All();
                        break;
                    }
                case 7:
                    {
                        ReadClassifierPara();
                        Classifier = new FrmClassifier(this);
                        Classifier.ReadPara();
                        break;
                    }
                case 8:
                    {
                        LightSetting();
                        LoadParaLight();
                        LightOn_All();
                        ReadBarcodeReaderPara();
                        BR = new FrmBarcodeReader(this);
                        BR.ReadPara();
                        break;
                    }
                case 9:
                    {
                        LightSetting();
                        LoadParaLight();
                        LightOn_All();
                        ReadBarcodeReaderPlusPara();
                        BRPlus = new FrmBarcodeReaderPlus(this);
                        break;
                    }
                case 10:
                    {
                        LightSetting();
                        LoadParaLight();
                        LightOn_All();
                        ReadLensCrack_AVI_Para();
                        LensCrack_AVI = new FrmLensCrack_AVI(this);

                        break;
                    }
                case 11:
                    {
                        LightSetting();
                        LoadParaLight();
                        LightOn_All();
                        ReadLen_Mold_Cave_Para();
                        Lens_Mold_Cave = new Frm8958BPFOCRVision(this);

                        break;
                    }
                case 12:
                    {
                        LightSetting();
                        LoadParaLight();
                        LightOn_All();
                        ReadLensCarryPara();
                        LensCarry = new FrmLensCarry(this);
                        ShowCross = true;
                        break;
                    }

            }
            Enet = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Enet.ReceiveTimeout = 3000;
            Enet.SendTimeout = 3000;
            Thread ConnectPlc = new Thread(Connect);
            ConnectPlc.IsBackground = true;
            ConnectPlc.Start();
            //影像連接
            if (CCD.CCDBrand == 0)
            {
                //設置相機參數再啟動
                myBasler.ip = CCD.ip;
                myBasler.Gain=CCD.Gain;
                myBasler.ExposureTime = CCD.ExposureTime;
                myBasler.TriggerMode = 1;
                myBasler.GetTimerStart();
                myBasler.eventProcessImage += processHImage;
            }
            else
            {
                myHikvision.ip = CCD.ip;
                myHikvision.Gain = CCD.Gain;
                myHikvision.ExposureTime = CCD.ExposureTime;
                myHikvision.TriggerMode = 1;
                myHikvision.InitializeSetting();
                myHikvision.eventProcessImage += processHImage;
                
            }
            timerRunningTime.Enabled = true;
            RemoveFiles();
            ScanPlc.RunWorkerAsync();
            ScanTrigger.RunWorkerAsync();

            if (!Sys.ManualClear)
            {
                setTaskAtFixedTime();
            }
            ThreadPool.SetMaxThreads(10, 10);
            ThreadPool.SetMinThreads(1, 1);
            HOperatorSet.SetSystem("clip_region", "false");
        }
        void Connect()
        {
            try
            {
                Enet.Connect(Plc.ip, 8001);
                Plc.IsConnected = true;
            }
            catch
            {
                Enet.Close();
                Plc.IsConnected = false;
            }
            Reconnect.RunWorkerAsync();
        }
       

        public void LoadComPara()
        {
            CCD.ip = IPAddress.Parse(IniFile.Read("CCD", "Ip", "192.18.1.2", Sys.SysPath));
            CCD.Port = int.Parse(IniFile.Read("CCD", "Port", "3000", Sys.SysPath));

            Plc.ip = IPAddress.Parse(IniFile.Read("PLC", "Ip", "192.18.3.2", Sys.SysPath));
            Plc.Port = int.Parse(IniFile.Read("PLC", "Port", "3000", Sys.SysPath));
        }

        public void LoadReaderPara()
        {
            try
            {
                //Reader 1
                string port = IniFile.Read("Reader", "Port", "1", Sys.SysPath);
                int Port = 0;
                if (Int32.TryParse(port, out Port))
                {
                    Reader.Com.PortName = "COM" + port;
                }
                string baudrate = IniFile.Read("Reader", "Baudrate", "115200", Sys.SysPath);
                int Baudrate = 0;
                if (Int32.TryParse(baudrate, out Baudrate))
                {
                    Reader.Com.BaudRate = Baudrate;
                }
                string dataBit = IniFile.Read("Reader", "DataBit", "8", Sys.SysPath);
                int DataBit = 0;
                if (Int32.TryParse(dataBit, out DataBit))
                {
                    Reader.Com.DataBits = DataBit;
                }
                string parity = IniFile.Read("Reader", "Parity", "default", Sys.SysPath);
                switch (parity)
                {
                    case "Even": Reader.Com.Parity = Parity.Even; break;
                    case "Odd": Reader.Com.Parity = Parity.Odd; break;
                    case "None": Reader.Com.Parity = Parity.None; break;
                }
                string stopBit = IniFile.Read("Reader", "StopBit", "1", Sys.SysPath);
                switch (stopBit)
                {
                    case "1": Reader.Com.StopBits = StopBits.One; break;
                    case "2": Reader.Com.StopBits = StopBits.Two; break;
                }
                if (Reader.Com.IsOpen)
                {
                    MessageBox.Show("Reader串口被占用," + Reader.Com.PortName);
                    Reader.IsConnected = false;
                }
                else
                {
                    Reader.Com.Open();
                    Reader.Com.DiscardInBuffer();
                    Reader.Com.DiscardOutBuffer();
                    Reader.IsConnected = true;
                    
                }
            }
            catch
            {
                Reader.IsConnected = false;
                //MessageBox.Show("Reader1未連接");
            }
        }

        public void ReadPara()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            My.TestNoGlue = bool.Parse(IniFile.Read("Setting", "TestNoGlue", "false", Path));
            My.OuterRadius_TestNoGlue = int.Parse(IniFile.Read("Setting", "OuterRadius_TestNoGlue", "100", Path));
            My.InnerRadius_TestNoGlue = int.Parse(IniFile.Read("Setting", "InnerRadius_TestNoGlue", "100", Path));
            My.Detection_Black_TestNoGlue = bool.Parse(IniFile.Read("Setting", "Detection_Black_TestNoGlue", "false", Path));
            My.Detection_White_TestNoGlue = bool.Parse(IniFile.Read("Setting", "Detection_White_TestNoGlue", "false", Path));
            My.GraythresholdBlack_TestNoGlue = int.Parse(IniFile.Read("Setting", "GraythresholdBlack_TestNoGlue", "1", Path));
            My.GraythresholdWhite_TestNoGlue = int.Parse(IniFile.Read("Setting", "GraythresholdWhite_TestNoGlue", "1", Path));
            My.UnderSizeArea_TestNoGlue = int.Parse(IniFile.Read("Setting", "UnderSizeArea_TestNoGlue", "1", Path));
            My.AreaUpper_TestNoGlue = int.Parse(IniFile.Read("Setting", "AreaUpper_TestNoGlue", "1", Path));
            My.AreaLower_TestNoGlue = int.Parse(IniFile.Read("Setting", "AreaLower_TestNoGlue", "1", Path));

            //My.TestFixedCollar = Convert.ToBoolean(IniFile.Read("Setting", "TestFixedCollar", "False", Path));
            //默認開啟
            My.TestFixedCollar = Convert.ToBoolean("true");
            My.TestSlope = Convert.ToBoolean(IniFile.Read("Setting", "Slope", "False", Path));
            My.TestInside = Convert.ToBoolean(IniFile.Read("Setting", "Inside", "False", Path));
            My.TestGap = Convert.ToBoolean(IniFile.Read("Setting", "Gap", "False", Path));
            //默認開啟
            My.TestPlatform = Convert.ToBoolean("true");
            //My.TestPlatform = Convert.ToBoolean(IniFile.Read("Setting", "TestPlatform", "False", Path));
            My.TestDirection = Convert.ToBoolean(IniFile.Read("Setting", "TestDirection", "False", Path));

            My.Detection_Black = Convert.ToBoolean(IniFile.Read("Setting", "Detection_Black", "True", Path));
            My.Detection_White = Convert.ToBoolean(IniFile.Read("Setting", "Detection_White", "True", Path));
            My.DetectionPF_Black = Convert.ToBoolean(IniFile.Read("Setting", "DetectionPF_Black", "True", Path));
            My.DetectionPF_White = Convert.ToBoolean(IniFile.Read("Setting", "DetectionPF_White", "True", Path));

            My.dFirstCircleRadius = Double.Parse(IniFile.Read("Setting", "FirstCircleRadius", "1000", Path));
            My.dReduceRadius = Double.Parse(IniFile.Read("Setting", "ReduceRadius", "300", Path));
            My.dGraythreshold = Double.Parse(IniFile.Read("Setting", "Graythreshold", "1", Path));
            My.dCenterRadius = Double.Parse(IniFile.Read("Setting", "CenterRadius", "1000", Path));
            My.dLength = Double.Parse(IniFile.Read("Setting", "Length", "30", Path));
            My.sGenParamValue = IniFile.Read("Setting", "GenParamValue", "negative", Path);
            My.dMeasureThreshold = Double.Parse(IniFile.Read("Setting", "MeasureThreshold", "40", Path));
            
            My.dRingInRange = double.Parse(IniFile.Read("Setting", "RingInRange", "1", Path));
            My.dRingOutRange = double.Parse(IniFile.Read("Setting", "RingOutRange", "1", Path));
            My.dGraythresholdBlack = double.Parse(IniFile.Read("Setting", "GraythresholdBlack", "1", Path));
            My.dGraythresholdWhite = double.Parse(IniFile.Read("Setting", "GraythresholdWhite", "1", Path));
            My.dUnderSizeArea = double.Parse(IniFile.Read("Setting", "UnderSizeArea", "1", Path));
            My.iGlueAngleSet = int.Parse(IniFile.Read("Setting", "GlueAngleSet", "5", Path));
            My.iGlueRatioSet = int.Parse(IniFile.Read("Setting", "GlueRatioSet", "5", Path));
            My.dAngleSet = double.Parse(IniFile.Read("Setting", "AngleSet", "1", Path));
            My.dLackMaxAngleSet = double.Parse(IniFile.Read("Setting", "LackMaxAngleSet", "360", Path));
            My.MethodChoice = int.Parse(IniFile.Read("Setting", "MethodChoice", "0", Path));
            My.GlueLightDarkChoice = int.Parse(IniFile.Read("Setting", "GlueLightDarkChoice", "0", Path));
            My.dDynThresholdSet = decimal.Parse(IniFile.Read("Setting", "DynThresholdSet", "1", Path));
            My.iUnderSizeArea2 = int.Parse(IniFile.Read("Setting", "UnderSizeArea2", "1", Path));

            My.Closing = Convert.ToBoolean(IniFile.Read("Setting", "Closing", "False", Path));
            My.Opening = Convert.ToBoolean(IniFile.Read("Setting", "Opening", "False", Path));
            My.FilterWidth = Convert.ToBoolean(IniFile.Read("Setting", "FilterWidth", "False", Path));
            My.FilterHeight = Convert.ToBoolean(IniFile.Read("Setting", "FilterHeight", "False", Path));

            My.dMeanWidth_1 = double.Parse(IniFile.Read("Setting", "MeanWidth_1", "1", Path));
            My.dMeanHeight_1 = double.Parse(IniFile.Read("Setting", "MeanHeight_1", "1", Path));
            My.dMeanWidth_2 = double.Parse(IniFile.Read("Setting", "MeanWidth_2", "1", Path));
            My.dMeanHeight_2 = double.Parse(IniFile.Read("Setting", "MeanHeight_2", "1", Path));
            My.dCloseWidthValue = double.Parse(IniFile.Read("Setting", "CloseWidthValue", "1", Path));
            My.dCloseHeightValue = double.Parse(IniFile.Read("Setting", "CloseHeightValue", "1", Path));
            My.dOpenWidthValue = double.Parse(IniFile.Read("Setting", "OpenWidthValue", "1", Path));
            My.dOpenHeightValue = double.Parse(IniFile.Read("Setting", "OpenHeightValue", "1", Path));
            My.dFilterWidth_Lower = double.Parse(IniFile.Read("Setting", "FilterWidth_Lower", "1", Path));
            My.dFilterWidth_Upper = double.Parse(IniFile.Read("Setting", "FilterWidth_Upper", "1", Path));
            My.dFilterHeight_Lower = double.Parse(IniFile.Read("Setting", "FilterHeight_Lower", "1", Path));
            My.dFilterHeight_Upper = double.Parse(IniFile.Read("Setting", "FilterHeight_Upper", "1", Path));

            My.DecisionMethodChoice = int.Parse(IniFile.Read("Setting", "DecisionMethodChoice", "1", Path));
            My.dRegionDistance = double.Parse(IniFile.Read("Setting", "RegionDistance", "1", Path));
            My.iGlueCount = int.Parse(IniFile.Read("Setting", "GlueCount", "1", Path));

            My.dInRangePF = double.Parse(IniFile.Read("Setting", "InRangePF", "1", Path));
            My.dOutRangePF = double.Parse(IniFile.Read("Setting", "OutRangePF", "1", Path));
            My.dGraythresholdBlackPF = double.Parse(IniFile.Read("Setting", "GraythresholdBlackPF", "1", Path));
            My.dGraythresholdWhitePF = double.Parse(IniFile.Read("Setting", "GraythresholdWhitePF", "1", Path));
            My.dUnderSizeAreaPF = double.Parse(IniFile.Read("Setting", "UnderSizeAreaPF", "1", Path));
            My.iGlueAngleSetPF = int.Parse(IniFile.Read("Setting", "GlueAngleSetPF", "5", Path));
            My.iGlueRatioSetPF = int.Parse(IniFile.Read("Setting", "GlueRatioSetPF", "5", Path));
            My.dAngleSetPF = double.Parse(IniFile.Read("Setting", "AngleSetPF", "1", Path));
            My.dLackMaxAngleSetPF = double.Parse(IniFile.Read("Setting", "LackMaxAngleSetPF", "360", Path));

            My.DecisionMethodChoice = int.Parse(IniFile.Read("Setting", "DecisionMethodChoice", "0", Path));
            My.MethodChoice2 = int.Parse(IniFile.Read("Setting", "MethodChoice2", "0", Path));
            My.DetectionPF_Dark2 = bool.Parse(IniFile.Read("Setting", "DetectionPF_Dark2", "false", Path));
            My.DetectionPF_Light2 = bool.Parse(IniFile.Read("Setting", "DetectionPF_Light2", "false", Path));
            My.ClosingPF2 = bool.Parse(IniFile.Read("Setting", "ClosingPF2", "false", Path));
            My.OpeningPF2 = bool.Parse(IniFile.Read("Setting", "OpeningPF2", "false", Path));
            My.iDynthresholdDarkPF2 = int.Parse(IniFile.Read("Setting", "DynthresholdDarkPF2", "1", Path));
            My.iDynthresholdLightPF2 = int.Parse(IniFile.Read("Setting", "DynthresholdLightPF2", "1", Path));
            My.iGraythresholdBlackPF2 = int.Parse(IniFile.Read("Setting", "GraythresholdBlackPF2", "1", Path));
            My.iGraythresholdWhitePF2 = int.Parse(IniFile.Read("Setting", "GraythresholdWhitePF2", "1", Path));
            My.iCloseWidthPF2 = int.Parse(IniFile.Read("Setting", "CloseWidthPF2", "1", Path));
            My.iCloseHeightPF2 = int.Parse(IniFile.Read("Setting", "CloseHeightPF2", "1", Path));
            My.iOpenWidthPF2 = int.Parse(IniFile.Read("Setting", "OpenWidthPF2", "1", Path));
            My.iOpenHeightPF2 = int.Parse(IniFile.Read("Setting", "OpenHeightPF2", "1", Path));
            My.iUnderSizeAreaPF2 = int.Parse(IniFile.Read("Setting", "UnderSizeAreaPF2", "1", Path));

            My.dInRangePF2 = int.Parse(IniFile.Read("Setting", "InRangePF2", "1", Path));
            My.dOutRangePF2 = int.Parse(IniFile.Read("Setting", "OutRangePF2", "1"   , Path));
            My.dGraythresholdBlackPF3 = int.Parse(IniFile.Read("Setting", "GraythresholdBlackPF3", "1", Path));
            My.dGraythresholdWhitePF3 = int.Parse(IniFile.Read("Setting", "GraythresholdWhitePF3", "1", Path));
            My.DetectionPF2_Black = bool.Parse(IniFile.Read("Setting", "DetectionPF2_Black", "false", Path));
            My.DetectionPF2_White  = bool.Parse(IniFile.Read("Setting", "DetectionPF2_White", "false", Path));

            My.iUnderSizeAreaPF2 = int.Parse(IniFile.Read("Setting", "UnderSizeAreaPF2", "1", Path));
            My.iOpenHeightPF2 = int.Parse(IniFile.Read("Setting", "OpenHeightPF2", "1", Path));
            My.iUnderSizeAreaPF2 = int.Parse(IniFile.Read("Setting", "UnderSizeAreaPF2", "1", Path));
        
            My.dOutSlope = double.Parse(IniFile.Read("Setting", "OutSlope", "1", Path));
            My.dInSlope = double.Parse(IniFile.Read("Setting", "InSlope", "1", Path));
            My.dSpilledUnderSizeArea = double.Parse(IniFile.Read("Setting", "SpilledUnderSizeArea", "1", Path));
            My.dAngleSet2 = double.Parse(IniFile.Read("Setting", "AngleSet2", "1", Path));

            My.dOutSlope3 = double.Parse(IniFile.Read("Setting", "OutSlope3", "1", Path));
            My.dInSlope3 = double.Parse(IniFile.Read("Setting", "InSlope3", "1", Path));
            My.dGraythreshold3 = double.Parse(IniFile.Read("Setting", "Graythreshold3", "1", Path));
            My.dSpilledUnderSizeArea3 = double.Parse(IniFile.Read("Setting", "SpilledUnderSizeArea3", "1", Path));
            My.dRect2_Len1Lower = double.Parse(IniFile.Read("Setting", "Rect2_Len1Lower", "1", Path));
            My.dRect2_Len1Upper = double.Parse(IniFile.Read("Setting", "Rect2_Len1Upper", "1", Path));
            My.dRect2_Len2Lower = double.Parse(IniFile.Read("Setting", "Rect2_Len2Lower", "1", Path));
            My.dRect2_Len2Upper = double.Parse(IniFile.Read("Setting", "Rect2_Len2Upper", "1", Path));

            //缺陷檢測
            My.TestDefeat = bool.Parse(IniFile.Read("Setting", "TestDefeat", "false", Path));
            My.Detection_Dark_TestDefeat = bool.Parse(IniFile.Read("Setting", "Detection_Dark_TestDefeat", "false", Path));
            My.Detection_Light_TestDefeat = bool.Parse(IniFile.Read("Setting", "Detection_Light_TestDefeat", "false", Path));
            My.Detection_Black_TestDefeat = bool.Parse(IniFile.Read("Setting", "Detection_Black_TestDefeat", "false", Path));
            My.Detection_White_TestDefeat = bool.Parse(IniFile.Read("Setting", "Detection_White_TestDefeat", "false", Path));
            My.Closing_TestDefeat = bool.Parse(IniFile.Read("Setting", "Closing_TestDefeat", "false", Path));
            My.Opening_TestDefeat = bool.Parse(IniFile.Read("Setting", "Opening_TestDefeat", "false", Path));

            My.OuterRadius_TestDefeat = int.Parse(IniFile.Read("Setting", "OuterRadius_TestDefeat", "1", Path));
            My.InnerRadius_TestDefeat = int.Parse(IniFile.Read("Setting", "InnerRadius_TestDefeat", "1", Path));
            My.DynthresholdDark_TestDefeat = int.Parse(IniFile.Read("Setting", "DynthresholdDark_TestDefeat", "1", Path));
            My.DynthresholdLight_TestDefeat = int.Parse(IniFile.Read("Setting", "DynthresholdLight_TestDefeat", "1", Path));
            My.GraythresholdBlack_TestDefeat = int.Parse(IniFile.Read("Setting", "GraythresholdBlack_TestDefeat", "1", Path));
            My.GraythresholdWhite_TestDefeat = int.Parse(IniFile.Read("Setting", "GraythresholdWhite_TestDefeat", "1", Path));
            My.CloseWidth_TestDefeat = int.Parse(IniFile.Read("Setting", "CloseWidth_TestDefeat", "1", Path));
            My.CloseHeight_TestDefeat = int.Parse(IniFile.Read("Setting", "CloseHeight_TestDefeat", "1", Path));
            My.OpenWidth_TestDefeat = int.Parse(IniFile.Read("Setting", "OpenWidth_TestDefeat", "1", Path));
            My.OpenHeight_TestDefeat = int.Parse(IniFile.Read("Setting", "OpenHeight_TestDefeat", "1", Path));
            My.UnderSizeArea_TestDefeat = int.Parse(IniFile.Read("Setting", "UnderSizeArea_TestDefeat", "1", Path));
            My.DetectionArea_TestDefeat = int.Parse(IniFile.Read("Setting", "DetectionArea_TestDefeat", "1", Path));



            My.dMarkGraythreshold = double.Parse(IniFile.Read("DirectionSetting", "MarkGraythreshold", "1", Path));
            My.dMarkGrade = double.Parse(IniFile.Read("DirectionSetting", "MarkGrade", "1", Path));
            My.iTestQuadrant = int.Parse(IniFile.Read("DirectionSetting", "TestQuadrant", "1", Path));

            My.dMarkID = double.Parse(IniFile.Read("DirectionSetting", "MarkID", "1", Path));
            My.dMarkTD = double.Parse(IniFile.Read("DirectionSetting", "MarkTD", "1", Path));

            My.dModuleGraythreshold_2 = double.Parse(IniFile.Read("DirectionSetting", "ModuleGraythreshold_2", "1", Path));
            My.dModuleGrade_2 = double.Parse(IniFile.Read("DirectionSetting", "ModuleGrade_2", "1", Path));
            My.dModuleGrade_3 = double.Parse(IniFile.Read("DirectionSetting", "ModuleGrade_3", "1", Path));

            string addTestDirectionChoice = IniFile.Read("Setting", "TestDirectionChoice", "", Path);
            int indexTestDirectionChoice = 0;
            switch (addTestDirectionChoice)
            {
                case "方法一": indexTestDirectionChoice = 0; break;
                case "方法二": indexTestDirectionChoice = 1; break;
                case "方法三": indexTestDirectionChoice = 2; break;
            }
            My.TestDirectionChoice = indexTestDirectionChoice;

            My.bChoiceMax = bool.Parse(IniFile.Read("Setting", "ChoiceMax", "true", Path));
            My.DarkLightChoice = int.Parse(IniFile.Read("Setting", "DarkLightChoice", "0", Path));
            My.DetectionArea = Convert.ToBoolean(IniFile.Read("Setting", "DetectionArea", "False", Path));
            My.DetectionRect2_Len1 = bool.Parse(IniFile.Read("Setting", "DetectionRect2_Len1", "false", Path));
            My.DetectionRect2_Len2 = bool.Parse(IniFile.Read("Setting", "DetectionRect2_Len2", "false", Path));
            My.DetectionRoundness = Convert.ToBoolean(IniFile.Read("Setting", "DetectionRoundness", "False", Path));
            My.DetectionRectangularity = Convert.ToBoolean(IniFile.Read("Setting", "DetectionRectangularity", "False", Path));
            My.ContrastSet = int.Parse(IniFile.Read("Setting", "ContrastSet", "0", Path));
            My.ContrastValue = int.Parse(IniFile.Read("Setting", "ContrastValue", "0", Path));
            My.Area_Upper = int.Parse(IniFile.Read("Setting", "Area_Upper", "0", Path));
            My.Area_Lower = int.Parse(IniFile.Read("Setting", "Area_Lower", "0", Path));
            
            My.GraySet = int.Parse(IniFile.Read("Setting", "GraySet", "1", Path));
            My.Rect2_Len1_Lower = int.Parse(IniFile.Read("Setting", "Rect2_Len1_Lower", "1", Path));
            My.Rect2_Len1_Upper = int.Parse(IniFile.Read("Setting", "Rect2_Len1_Upper", "1", Path));
            My.Rect2_Len2_Lower = int.Parse(IniFile.Read("Setting", "Rect2_Len2_Lower", "1", Path));
            My.Rect2_Len2_Upper = int.Parse(IniFile.Read("Setting", "Rect2_Len2_Upper", "1", Path));
            
            My.Roundness_Upper = int.Parse(IniFile.Read("Setting", "Roundness_Upper", "1", Path));
            My.Roundness_Lower = int.Parse(IniFile.Read("Setting", "Roundness_Lower", "0", Path));
            My.Rectangularity_Upper = int.Parse(IniFile.Read("Setting", "Rectangularity_Upper", "1", Path));
            My.Rectangularity_Lower = int.Parse(IniFile.Read("Setting", "Rectangularity_Lower", "0", Path));
            
        }

        public void ReadSomaPara()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            My8982Soma.dFirstCircleRadius = Double.Parse(IniFile.Read("Setting", "FirstCircleRadius", "1000", Path));
            My8982Soma.dReduceRadius = Double.Parse(IniFile.Read("Setting", "ReduceRadius", "300", Path));
            My8982Soma.dGraythreshold = Double.Parse(IniFile.Read("Setting", "Graythreshold", "1", Path));
            My8982Soma.dLength = Double.Parse(IniFile.Read("Setting", "Length", "30", Path));
            My8982Soma.dWidth = Double.Parse(IniFile.Read("Setting", "Width", "5", Path));
            My8982Soma.sGenParamValue = IniFile.Read("Setting", "GenParamValue", "negative", Path);
            My8982Soma.dMeasureThreshold = Double.Parse(IniFile.Read("Setting", "MeasureThreshold", "40", Path));
            My8982Soma.dSomaReduceRadius = Double.Parse(IniFile.Read("Setting", "SomaReduceRadius", "300", Path));
            My8982Soma.dSomaGraythreshold = Double.Parse(IniFile.Read("Setting", "SomaGraythreshold", "1", Path));
         
            My8982Soma.dCircularitySet = Double.Parse(IniFile.Read("Setting", "CircularitySet", "0.97", Path));
        }

        public void ReadVDIPara()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            My.dFirstCircleRadius = Double.Parse(IniFile.Read("Setting", "FirstCircleRadius", "1000", Path));
            My.dReduceRadius = Double.Parse(IniFile.Read("Setting", "ReduceRadius", "300", Path));
            My.dGraythreshold = Double.Parse(IniFile.Read("Setting", "Graythreshold", "1", Path));
            My.dLength = Double.Parse(IniFile.Read("Setting", "Length", "30", Path));
            My.sGenParamValue = IniFile.Read("Setting", "GenParamValue", "negative", Path);
            My.dMeasureThreshold = Double.Parse(IniFile.Read("Setting", "MeasureThreshold", "40", Path));


            My.VDI.AimCirR = Double.Parse(IniFile.Read("Setting", "AimCirR", "1", Path));
            My.VDI.dCoatRMin = Double.Parse(IniFile.Read("Setting", "CoatRMin", "1", Path));
            My.VDI.dCoatRMax = Double.Parse(IniFile.Read("Setting", "CoatRMax", "1", Path));
            My.VDI.dPositiveOffSet = Double.Parse(IniFile.Read("Setting", "PositiveOffSet", "1", Path));
            My.VDI.dNegativeOffSet = Double.Parse(IniFile.Read("Setting", "NegativeOffSet", "1", Path));
            My.VDI.dFirstCircleRadius2 = Double.Parse(IniFile.Read("Setting", "FirstCircleRadius2", "1", Path));
            My.VDI.dLength2 = Double.Parse(IniFile.Read("Setting", "Length2", "30", Path));
            My.VDI.sGenParamValue2 = IniFile.Read("Setting", "GenParamValue2", "negative", Path);
            My.VDI.dMeasureThreshold2 = Double.Parse(IniFile.Read("Setting", "MeasureThreshold2", "40", Path));

            My.VDI.dMissGray = Double.Parse(IniFile.Read("Setting", "MissGray", "25", Path));
            My.VDI.dMissArea = Double.Parse(IniFile.Read("Setting", "MissArea", "50000", Path));
            My.VDI.DarkLightChoice = int.Parse(IniFile.Read("Setting", "DarkLightChoice", "0", Path));
        }

        public void ReadSomaDetectionVSPara()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            My.dReduceRadius = Double.Parse(IniFile.Read("Setting", "ReduceRadius", "300", Path));
            My.dGraythreshold = Double.Parse(IniFile.Read("Setting", "Graythreshold", "1", Path));
            My.Soma.dRadiusSet_Miss = Double.Parse(IniFile.Read("Setting", "RadiusSet_Miss", "30", Path));
            My.Soma.dWidthSet_Miss = Double.Parse(IniFile.Read("Setting", "WidthSet_Miss", "30", Path));
            My.Soma.dRadiusSet_Front = Double.Parse(IniFile.Read("Setting", "RadiusSet_Front", "30", Path));
            My.Soma.dWidthSet_Front = Double.Parse(IniFile.Read("Setting", "WidthSet_Front", "30", Path));
            My.Soma.dRadiusSet_Reverse = Double.Parse(IniFile.Read("Setting", "RadiusSet_Reverse", "30", Path));
            My.Soma.dWidthSet_Reverse = Double.Parse(IniFile.Read("Setting", "WidthSet_Reverse", "30", Path));

            My.Soma.dLengthTD = Double.Parse(IniFile.Read("Setting", "LengthTD", "30", Path));
            My.Soma.sGenParamValueTD = IniFile.Read("Setting", "GenParamValueTD", "negative", Path);
            My.Soma.dMeasureThresholdTD = Double.Parse(IniFile.Read("Setting", "MeasureThresholdTD", "40", Path));
            My.Soma.dLengthID = Double.Parse(IniFile.Read("Setting", "LengthID", "30", Path));
            My.Soma.sGenParamValueID = IniFile.Read("Setting", "GenParamValueID", "negative", Path);
            My.Soma.dMeasureThresholdID = Double.Parse(IniFile.Read("Setting", "MeasureThresholdID", "40", Path));

            My.Soma.dTopDaimShrink = Double.Parse(IniFile.Read("Setting", "TopDaimShrink", "30", Path));
            My.Soma.dInnerDiamMagnify = Double.Parse(IniFile.Read("Setting", "InnerDiamMagnify", "30", Path));
            My.Soma.dGraySet = Double.Parse(IniFile.Read("Setting", "GraySet", "30", Path));
            My.Soma.dFilterArea = Double.Parse(IniFile.Read("Setting", "FilterArea", "30", Path));
            My.Soma.dScratchLength = Double.Parse(IniFile.Read("Setting", "ScratchLength", "30", Path));
            My.Soma.dLargeArea = Double.Parse(IniFile.Read("Setting", "LargeArea", "30", Path));
            My.Soma.dNumber = Double.Parse(IniFile.Read("Setting", "Number", "30", Path));
        }

        public void ReadVDICoatingPara()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            My.VDICoating.mmpixel = Double.Parse(IniFile.Read("Setting", "mmpixel", "0.0044", Path));
            My.VDICoating.PointChoice = IniFile.Read("Setting", "PointChoice", "last", Path);
            My.VDICoating.TestDefect = Convert.ToBoolean(IniFile.Read("Setting", "TestDefect", "False", Path));
            My.VDICoating.DetectionColor = Convert.ToBoolean(IniFile.Read("Setting", "DetectionColor", "False", Path));
            My.VDICoating.IgnoreOpen = Convert.ToBoolean(IniFile.Read("Setting", "IgnoreOpen", "False", Path));

            My.VDICoating.dFirstCircleRadius = Double.Parse(IniFile.Read("Setting", "FirstCircleRadius", "1000", Path));
            My.VDICoating.dReduceRadius = Double.Parse(IniFile.Read("Setting", "ReduceRadius", "1000", Path));
            My.VDICoating.dGraythreshold = Double.Parse(IniFile.Read("Setting", "Graythreshold", "70", Path));
            My.VDICoating.dLength = Double.Parse(IniFile.Read("Setting", "Length", "30", Path));
            My.VDICoating.dMeasureThreshold = Double.Parse(IniFile.Read("Setting", "MeasureThreshold", "5", Path));
            My.VDICoating.sGenParamValue = IniFile.Read("Setting", "GenParamValue", "negative", Path);

            My.VDICoating.dRadius_ID = Double.Parse(IniFile.Read("Setting", "Radius_ID", "5", Path));
            My.VDICoating.dRadius_TD = Double.Parse(IniFile.Read("Setting", "Radius_TD", "5", Path));

            My.VDICoating.dDefectGraySet = Double.Parse(IniFile.Read("Setting", "DefectGraySet", "5", Path));
            My.VDICoating.dFilterArea = Double.Parse(IniFile.Read("Setting", "FilterArea", "5", Path));
            My.VDICoating.dScratchLength = Double.Parse(IniFile.Read("Setting", "ScratchLength", "5", Path));
            My.VDICoating.dLargeArea = Double.Parse(IniFile.Read("Setting", "LargeArea", "5", Path));
            My.VDICoating.dNumber = Double.Parse(IniFile.Read("Setting", "Number", "5", Path));

            My.VDICoating.dMissGray = Double.Parse(IniFile.Read("Setting", "MissGray", "25", Path));
            My.VDICoating.dMissArea = Double.Parse(IniFile.Read("Setting", "MissArea", "50000", Path));
            My.VDICoating.dMissOuterRadius = Double.Parse(IniFile.Read("Setting", "MissOuterRadius", "500", Path));

            My.VDICoating.dDefect_ID = Double.Parse(IniFile.Read("Setting", "Defect_ID", "5", Path));
            My.VDICoating.dDefect_TD = Double.Parse(IniFile.Read("Setting", "Defect_TD", "5", Path));
            My.VDICoating.dIgnore_ID = Double.Parse(IniFile.Read("Setting", "Ignore_ID", "5", Path));
            My.VDICoating.dIgnore_TD = Double.Parse(IniFile.Read("Setting", "Ignore_TD", "5", Path));
            
            My.VDICoating.dUpperLimit_A = Double.Parse(IniFile.Read("Setting", "UpperLimit_A", "5", Path));
            My.VDICoating.dLowerLimit_A = Double.Parse(IniFile.Read("Setting", "LowerLimit_A", "5", Path));
            My.VDICoating.dUpperLimit_B = Double.Parse(IniFile.Read("Setting", "UpperLimit_B", "5", Path));
            My.VDICoating.dLowerLimit_B = Double.Parse(IniFile.Read("Setting", "LowerLimit_B", "5", Path));

            My.VDICoating.dCenterDistance = Double.Parse(IniFile.Read("Setting", "CenterDistance", "5", Path));
            My.VDICoating.dRangeRadius = Double.Parse(IniFile.Read("Setting", "RangeRadius", "5", Path));

            My.VDICoating.ColorRangeChoice = int.Parse(IniFile.Read("Setting", "ColorRangeChoice", "0", Path));
            My.VDICoating.DarkLightChoice = int.Parse(IniFile.Read("Setting", "DarkLightChoice", "0", Path));

        }

        public void ReadVDI_InkPara()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            My.VDI_Ink.iAimCirR = int.Parse(IniFile.Read("Setting", "AimCirR", "30", Path));
            My.VDI_Ink.pixel2um = double.Parse(IniFile.Read("Setting", "pixel2um", "5", Path));
            My.VDI_Ink.iLength = int.Parse(IniFile.Read("Setting", "Length", "30", Path));
            My.VDI_Ink.dCenterRadius = double.Parse(IniFile.Read("Setting", "CenterRadius", "600", Path));
            My.VDI_Ink.iMeasureThreshold = int.Parse(IniFile.Read("Setting", "MeasureThreshold", "100", Path));
            My.VDI_Ink.sGenParamValue = IniFile.Read("Setting", "GenParamValue", "negative", Path);
            My.VDI_Ink.PointChoice = IniFile.Read("Setting", "PointChoice", "last", Path);
            My.VDI_Ink.iReduceRadius = int.Parse(IniFile.Read("Setting", "ReduceRadius", "300", Path));
            My.VDI_Ink.iOutsideDiam_Upper = int.Parse(IniFile.Read("Setting", "OutsideDiam_Upper", "300", Path));
            My.VDI_Ink.iOutsideDiam_Lower = int.Parse(IniFile.Read("Setting", "OutsideDiam_Lower", "300", Path));
            My.VDI_Ink.iInsideDiam_Upper = int.Parse(IniFile.Read("Setting", "InsideDiam_Upper", "300", Path));
            My.VDI_Ink.iInsideDiam_Lower = int.Parse(IniFile.Read("Setting", "InsideDiam_Lower", "300", Path));
            My.VDI_Ink.iGraySet = int.Parse(IniFile.Read("Setting", "GraySet", "100", Path));
            My.VDI_Ink.iUnderSizeArea = int.Parse(IniFile.Read("Setting", "UnderSizeArea", "10", Path));

            My.VDI_Ink.dAngleRange_Empty = double.Parse(IniFile.Read("Setting", "AngleRange_Empty", "180", Path));
            My.VDI_Ink.iEmptyGraySet = int.Parse(IniFile.Read("Setting", "EmptyGraySet", "180", Path));
            My.VDI_Ink.dEmptyCircleRadius = double.Parse(IniFile.Read("Setting", "EmptyCircleRadius", "180", Path));
            My.VDI_Ink.iNoInkAreaSet = int.Parse(IniFile.Read("Setting", "NoInkAreaSet", "200000", Path));
            My.VDI_Ink.dScoreSet_Empty = double.Parse(IniFile.Read("Setting", "ScoreSet_Empty", "80", Path));
            My.VDI_Ink.dAngleRange_NoInk = double.Parse(IniFile.Read("Setting", "AngleRange_NoInk", "180", Path));
            My.VDI_Ink.dScoreSet_NoInk = double.Parse(IniFile.Read("Setting", "ScoreSet_NoInk", "80", Path));
            My.VDI_Ink.iEccentricitySet = int.Parse(IniFile.Read("Setting", "EccentricitySet", "80", Path));
            
        }

        public void ReadNIRPara()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            My.NIR.ReadBarrelBarcode = Convert.ToBoolean(IniFile.Read("Setting", "ReadBarrelBarcode", "False", Path));
            My.NIR.BarcodeRange_Row1 = double.Parse(IniFile.Read("Setting", "BarcodeRange_Row1", "0", Path));
            My.NIR.BarcodeRange_Column1 = double.Parse(IniFile.Read("Setting", "BarcodeRange_Column1", "0", Path));
            My.NIR.BarcodeRange_Row2 = double.Parse(IniFile.Read("Setting", "BarcodeRange_Row2", "1900", Path));
            My.NIR.BarcodeRange_Column2 = double.Parse(IniFile.Read("Setting", "BarcodeRange_Column2", "1900", Path));

            My.NIR.BarcodePosition = int.Parse(IniFile.Read("Setting", "BarcodePosition", "0", Path));

            My.NIR.Length1_RectangleCenter = double.Parse(IniFile.Read("RectangleCenter", "Length1", "0", Sys.SysPath));
            My.NIR.Length2_RectangleCenter = double.Parse(IniFile.Read("RectangleCenter", "Length2", "0", Sys.SysPath));
            My.NIR.Phi_RectangleCenter = double.Parse(IniFile.Read("RectangleCenter", "Phi", "0", Sys.SysPath));
            My.NIR.MeasureSelect_RectangleCenter = double.Parse(IniFile.Read("RectangleCenter", "MeasureSelect", "0", Sys.SysPath));
            My.NIR.Length_RectangleCenter = int.Parse(IniFile.Read("RectangleCenter", "Length", "1", Sys.SysPath));
            My.NIR.MeasureTransition_RectangleCenter = IniFile.Read("RectangleCenter", "MeasureTransition", "negative", Sys.SysPath);
            My.NIR.MeasureThreshold_RectangleCenter = double.Parse(IniFile.Read("RectangleCenter", "MeasureThreshold", "1", Sys.SysPath));

            My.NIR.MeasureSelect_Center = double.Parse(IniFile.Read("Center", "MeasureSelect", "0", Sys.SysPath));
            My.NIR.Radius_Center = int.Parse(IniFile.Read("Center", "Radius", "1", Sys.SysPath));
            My.NIR.Length_Center = int.Parse(IniFile.Read("Center", "Length", "1", Sys.SysPath));
            My.NIR.MeasureTransition_Center = IniFile.Read("Center", "MeasureTransition", "negative", Sys.SysPath);
            My.NIR.MeasureThreshold_Center = double.Parse(IniFile.Read("Center", "MeasureThreshold", "1", Sys.SysPath));

            My.NIR.expected_holder_radius = double.Parse(IniFile.Read("OffSet", "expected_holder_radius", "2500", Sys.SysPath));
            My.NIR.DarkLightChoice_Scar = int.Parse(IniFile.Read("Scar", "DarkLightChoice", "0", Sys.SysPath));

            My.NIR.OuterRadius_Paricle = double.Parse(IniFile.Read("Paricle", "OuterRadius", "1000", Sys.SysPath));
            My.NIR.InnerRadius_Paricle = double.Parse(IniFile.Read("Paricle", "InnerRadius", "1", Sys.SysPath));

            My.NIR.Check_ExcessiveGlue = bool.Parse(IniFile.Read("ExcessiveGlue", "Check", "false", Sys.SysPath));
            My.NIR.Mode_ExcessiveGlue = int.Parse(IniFile.Read("ExcessiveGlue", "Mode", "0", Sys.SysPath));
            My.NIR.OuterRadius_ExcessiveGlue = double.Parse(IniFile.Read("ExcessiveGlue", "OuterRadius", "100", Sys.SysPath));
            My.NIR.InnerRadius_ExcessiveGlue = double.Parse(IniFile.Read("ExcessiveGlue", "InnerRadius", "100", Sys.SysPath));
            My.NIR.DarkLightChoice_ExcessiveGlue = int.Parse(IniFile.Read("ExcessiveGlue", "DarkLightChoice", "0", Sys.SysPath));
            My.NIR.OffSet_ExcessiveGlue = int.Parse(IniFile.Read("ExcessiveGlue", "OffSet", "0", Sys.SysPath));
        }

        public void ReadClassifierPara()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            //機種參數
            My.Classifier.iProductionSet = int.Parse(IniFile.Read("Setting", "ProductionSet", "0", Path));
            //無料參數
            My.Classifier.iNullthreshold = int.Parse(IniFile.Read("Setting", "Nullthreshold", "1", Path));
            My.Classifier.iNullArea_Upper = int.Parse(IniFile.Read("Setting", "NullArea_Upper", "1", Path));
            My.Classifier.iNullArea_Lower = int.Parse(IniFile.Read("Setting", "NullArea_Lower", "1", Path));
            //矩形參數
            My.Classifier.iRectangleSmooth = int.Parse(IniFile.Read("Setting", "RectangleSmooth", "1", Path));
            My.Classifier.iRectangleLightDark = int.Parse(IniFile.Read("Setting", "RectangleLightDark", "1", Path));
            My.Classifier.iRectangleOffSet = int.Parse(IniFile.Read("Setting", "RectangleOffSet", "1", Path));
            My.Classifier.iRectangleDilation = int.Parse(IniFile.Read("Setting", "RectangleDilation", "1", Path));
            My.Classifier.RectangleAreaSet = bool.Parse(IniFile.Read("Setting", "RectangleAreaSet", "false", Path));
            My.Classifier.RectangleRoundnessSet = bool.Parse(IniFile.Read("Setting", "RectangleRoundnessSet", "false", Path));
            My.Classifier.RectangleRectangularitySet = bool.Parse(IniFile.Read("Setting", "RectangleRectangularitySet", "false", Path));
            My.Classifier.iRectangleArea_Upper = int.Parse(IniFile.Read("Setting", "RectangleArea_Upper", "1", Path));
            My.Classifier.iRectangleArea_Lower = int.Parse(IniFile.Read("Setting", "RectangleArea_Lower", "1", Path));
            My.Classifier.iRectangleRoundness_Upper = int.Parse(IniFile.Read("Setting", "RectangleRoundness_Upper", "1", Path));
            My.Classifier.iRectangleRoundness_Lower = int.Parse(IniFile.Read("Setting", "RectangleRoundness_Lower", "1", Path));
            My.Classifier.iRectangleRectangularity_Upper = int.Parse(IniFile.Read("Setting", "RectangleRectangularity_Upper", "1", Path));
            My.Classifier.iRectangleRectangularity_Lower = int.Parse(IniFile.Read("Setting", "RectangleRectangularity_Lower", "1", Path));
            My.Classifier.iRectangleLength = int.Parse(IniFile.Read("Setting", "RectangleLength", "1", Path));
            My.Classifier.iRectangleLightDark2 = int.Parse(IniFile.Read("Setting", "RectangleLightDark2", "1", Path));
            My.Classifier.iRectanglePointChoice = int.Parse(IniFile.Read("Setting", "RectanglePointChoice", "1", Path));
            My.Classifier.iRectangleMeasureThreshold = int.Parse(IniFile.Read("Setting", "RectangleMeasureThreshold", "1", Path));
            //圓參數
            My.Classifier.CircleAreaSet = bool.Parse(IniFile.Read("Setting", "CircleAreaSet", "false", Path));
            My.Classifier.CircleRoundnessSet = bool.Parse(IniFile.Read("Setting", "CircleRoundnessSet", "false", Path));
            My.Classifier.CircleRectangularitySet = bool.Parse(IniFile.Read("Setting", "CircleRectangularitySet", "false", Path));
            My.Classifier.iCircleSmooth = int.Parse(IniFile.Read("Setting", "CircleSmooth", "1", Path));
            My.Classifier.iCircleLightDark = int.Parse(IniFile.Read("Setting", "CircleLightDark", "1", Path));
            My.Classifier.iCircleOffSet = int.Parse(IniFile.Read("Setting", "CircleOffSet", "1", Path));
            My.Classifier.iCircleClosing = int.Parse(IniFile.Read("Setting", "CircleClosing", "1", Path));
            My.Classifier.CircleAreaSet = bool.Parse(IniFile.Read("Setting", "CircleAreaSet", "false", Path));
            My.Classifier.CircleRoundnessSet = bool.Parse(IniFile.Read("Setting", "CircleRoundnessSet", "false", Path));
            My.Classifier.CircleRectangularitySet = bool.Parse(IniFile.Read("Setting", "CircleRectangularitySet", "false", Path));
            My.Classifier.iCircleArea_Upper = int.Parse(IniFile.Read("Setting", "CircleArea_Upper", "1", Path));
            My.Classifier.iCircleArea_Lower = int.Parse(IniFile.Read("Setting", "CircleArea_Lower", "1", Path));
            My.Classifier.iCircleRoundness_Upper = int.Parse(IniFile.Read("Setting", "CircleRoundness_Upper", "1", Path));
            My.Classifier.iCircleRoundness_Lower = int.Parse(IniFile.Read("Setting", "CircleRoundness_Lower", "1", Path));
            My.Classifier.iCircleRectangularity_Upper = int.Parse(IniFile.Read("Setting", "CircleRectangularity_Upper", "1", Path));
            My.Classifier.iCircleRectangularity_Lower = int.Parse(IniFile.Read("Setting", "CircleRectangularity_Lower", "1", Path));
            My.Classifier.iCircleLightDark2 = int.Parse(IniFile.Read("Setting", "CircleLightDark2", "1", Path));
            My.Classifier.iCirclePointChoice = int.Parse(IniFile.Read("Setting", "CirclePointChoice", "1", Path));
            My.Classifier.iCircleLength = int.Parse(IniFile.Read("Setting", "CircleLength", "1", Path));
            My.Classifier.iCircleMeasureThreshold = int.Parse(IniFile.Read("Setting", "CircleMeasureThreshold", "1", Path));
            //剪口參數
            My.Classifier.iNotchArea_Upper = int.Parse(IniFile.Read("Setting", "NotchArea_Upper", "1", Path));
            My.Classifier.iNotchArea_Lower = int.Parse(IniFile.Read("Setting", "NotchArea_Lower", "1", Path));
            //Mark參數
            My.Classifier.iMarkSmooth = int.Parse(IniFile.Read("Setting", "MarkSmooth", "1", Path));
            My.Classifier.iMarkLightDark = int.Parse(IniFile.Read("Setting", "MarkLightDark", "1", Path));
            My.Classifier.iMarkOffSet = int.Parse(IniFile.Read("Setting", "MarkOffSet", "1", Path));
            My.Classifier.MarkAreaSet = bool.Parse(IniFile.Read("Setting", "MarkAreaSet", "false", Path));
            My.Classifier.MarkRoundnessSet = bool.Parse(IniFile.Read("Setting", "MarkRoundnessSet", "false", Path));
            My.Classifier.MarkRectangularitySet = bool.Parse(IniFile.Read("Setting", "MarkRectangularitySet", "false", Path));
            My.Classifier.iMarkArea_Upper = int.Parse(IniFile.Read("Setting", "MarkArea_Upper", "1", Path));
            My.Classifier.iMarkArea_Lower = int.Parse(IniFile.Read("Setting", "MarkArea_Lower", "1", Path));
            My.Classifier.iMarkRoundness_Upper = int.Parse(IniFile.Read("Setting", "MarkRoundness_Upper", "1", Path));
            My.Classifier.iMarkRoundness_Lower = int.Parse(IniFile.Read("Setting", "MarkRoundness_Lower", "1", Path));
            My.Classifier.iMarkRectangularity_Upper = int.Parse(IniFile.Read("Setting", "MarkRectangularity_Upper", "1", Path));
            My.Classifier.iMarkRectangularity_Lower = int.Parse(IniFile.Read("Setting", "MarkRectangularity_Lower", "1", Path));
            My.Classifier.dDrawRectangle2Length1 = double.Parse(IniFile.Read("Setting", "DrawRectangle2Length1", "1", Path));
            My.Classifier.dDrawRectangle2Length2 = double.Parse(IniFile.Read("Setting", "DrawRectangle2Length2", "1", Path));
            My.Classifier.dDrawCircleRadius = double.Parse(IniFile.Read("Setting", "DrawCircleRadius", "1", Path));
        }

        public void ReadBarcodeReaderPara()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            MyBarcodeReader.Production = int.Parse(IniFile.Read("Setting", "Production", "0", Path));
            MyBarcodeReader.RangeRow1 = double.Parse(IniFile.Read("Setting", "RangeRow1", MyBarcodeReader.RangeRow1.ToString(), Path));
            MyBarcodeReader.RangeColumn1 = double.Parse(IniFile.Read("Setting", "RangeColumn1", MyBarcodeReader.RangeColumn1.ToString(), Path));
            MyBarcodeReader.RangeRow2 = double.Parse(IniFile.Read("Setting", "RangeRow2", MyBarcodeReader.RangeRow2.ToString(), Path));
            MyBarcodeReader.RangeColumn2 = double.Parse(IniFile.Read("Setting", "RangeColumn2", MyBarcodeReader.RangeColumn2.ToString(), Path));
            MyBarcodeReader.Verification = bool.Parse(IniFile.Read("Setting", "Verification", MyBarcodeReader.Verification.ToString(), Path));
            MyBarcodeReader.OkAddition = int.Parse(IniFile.Read("Setting", "OkAddition", MyBarcodeReader.OkAddition.ToString(), Path));
        }

        public void ReadBarcodeReaderPlusPara()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            try
            {
                MyBarcodeReaderPlus.Production = int.Parse(IniFile.Read("Setting", "Production", "0", Path));
                MyBarcodeReaderPlus.TrayRows = int.Parse(IniFile.Read("Setting", "TrayRows", "20", Path));
                MyBarcodeReaderPlus.TrayColumns = int.Parse(IniFile.Read("Setting", "TrayColumns", "20", Path));
                MyBarcodeReaderPlus.RegionRow1 = double.Parse(IniFile.Read("Setting", "RegionRow1", "0", Path));
                MyBarcodeReaderPlus.RegionColumn1 = double.Parse(IniFile.Read("Setting", "RegionColumn1", "0", Path));
                MyBarcodeReaderPlus.RegionRow2 = double.Parse(IniFile.Read("Setting", "RegionRow2", "0", Path));
                MyBarcodeReaderPlus.RegionColumn2 = double.Parse(IniFile.Read("Setting", "RegionColumn2", "0", Path));
                
                int n = MyBarcodeReaderPlus.TrayRows * MyBarcodeReaderPlus.TrayColumns;
                MyBarcodeReaderPlus.TrayPointBanned = new bool[n];
                for (int i = 0; i < n; i++)
                {
                    MyBarcodeReaderPlus.TrayPointBanned[i] = bool.Parse(IniFile.Read("TrayPointBanned", i.ToString(), "false", Path));
                }
                MyBarcodeReaderPlus.RowCutNum = int.Parse(IniFile.Read("Setting", "RowCutNum", "1", Path));
                MyBarcodeReaderPlus.ColumnCutNum = int.Parse(IniFile.Read("Setting", "ColumnCutNum", "1", Path));
                MyBarcodeReaderPlus.ModelMode = int.Parse(IniFile.Read("Visual", "ModelMode", "0", Path));
                string ModelPath = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model";
                //不同模組模式讀取不同模組
                try
                {
                    switch (MyBarcodeReaderPlus.ModelMode)
                    {
                        case 0: HOperatorSet.ReadShapeModel(ModelPath, out MyBarcodeReaderPlus.hv_ModelID); break;
                        case 1: HOperatorSet.ReadNccModel(ModelPath, out MyBarcodeReaderPlus.hv_ModelID); break;
                    }
                }
                catch
                {
                }
                MyBarcodeReaderPlus.ModelGrade = double.Parse(IniFile.Read("Visual", "ModelGrade", "50", Path));
                MyBarcodeReaderPlus.FirstRadius = int.Parse(IniFile.Read("Visual", "FirstRadius", "1", Path));
                MyBarcodeReaderPlus.MeasureSelect = IniFile.Read("Visual", "MeasureSelect", "first", Path);
                MyBarcodeReaderPlus.Radius = int.Parse(IniFile.Read("Visual", "Radius", "1", Path));
                MyBarcodeReaderPlus.Length = int.Parse(IniFile.Read("Visual", "Length ", "1", Path));
                MyBarcodeReaderPlus.MeasureTransition = IniFile.Read("Visual", "MeasureTransition", "negative", Path);
                MyBarcodeReaderPlus.MeasureThreshold = int.Parse(IniFile.Read("Visual", "MeasureThreshold", "1", Path));
                MyBarcodeReaderPlus.Length21 = double.Parse(IniFile.Read("Visual", "Length21", "1", Path));
                MyBarcodeReaderPlus.Length22 = double.Parse(IniFile.Read("Visual", "Length22", "1", Path));
                MyBarcodeReaderPlus.Phi2 = double.Parse(IniFile.Read("Visual", "Phi2", "1", Path));
                MyBarcodeReaderPlus.MeasureSelect2 = IniFile.Read("Visual", "MeasureSelect2", "first", Path);
                MyBarcodeReaderPlus.Length2 = int.Parse(IniFile.Read("Visual", "Length2 ", "1", Path));
                MyBarcodeReaderPlus.MeasureTransition2 = IniFile.Read("Visual", "MeasureTransition2", "negative", Path);
                MyBarcodeReaderPlus.MeasureThreshold2 = int.Parse(IniFile.Read("Visual", "MeasureThreshold2", "1", Path));
                MyBarcodeReaderPlus.OuterRadius = int.Parse(IniFile.Read("Visual", "OuterRadius", "1", Path));
                MyBarcodeReaderPlus.InnerRadius = int.Parse(IniFile.Read("Visual", "InnerRadius", "1", Path));
                MyBarcodeReaderPlus.StartAngle = int.Parse(IniFile.Read("Visual", "StartAngle", "0", Path));
                MyBarcodeReaderPlus.EndAngle = int.Parse(IniFile.Read("Visual", "EndAngle", "180", Path));
                MyBarcodeReaderPlus.RegionDarkLight = int.Parse(IniFile.Read("Visual", "RegionDarkLight", "1", Path));
                MyBarcodeReaderPlus.RegionThreshold = int.Parse(IniFile.Read("Visual", "RegionThreshold", "1", Path));
                MyBarcodeReaderPlus.RegionRect2_Len1_Upper = int.Parse(IniFile.Read("Visual", "RegionRect2_Len1_Upper", "1", Path));
                MyBarcodeReaderPlus.RegionRect2_Len1_Lower = int.Parse(IniFile.Read("Visual", "RegionRect2_Len1_Lower", "1", Path));
                MyBarcodeReaderPlus.RegionRect2_Len2_Upper = int.Parse(IniFile.Read("Visual", "RegionRect2_Len2_Upper", "1", Path));
                MyBarcodeReaderPlus.RegionRect2_Len2_Lower = int.Parse(IniFile.Read("Visual", "RegionRect2_Len2_Lower", "1", Path));
                MyBarcodeReaderPlus.RegionErosion = int.Parse(IniFile.Read("Visual", "RegionErosion", "1", Path));
                MyBarcodeReaderPlus.RegionDistance = int.Parse(IniFile.Read("Visual", "RegionDistance", "1", Path));
                MyBarcodeReaderPlus.RegionProjectSet = int.Parse(IniFile.Read("Visual", "RegionProjectSet", "1", Path));
                MyBarcodeReaderPlus.RegionRotation = int.Parse(IniFile.Read("Visual", "RegionRotation", "1", Path));
                MyBarcodeReaderPlus.RegionLength1 = int.Parse(IniFile.Read("Visual", "RegionLength1", "1", Path));
                MyBarcodeReaderPlus.RegionLength2 = int.Parse(IniFile.Read("Visual", "RegionLength2", "1", Path));
                MyBarcodeReaderPlus.Mirrored = bool.Parse(IniFile.Read("Visual", "Mirrored", "false", Path));
                MyBarcodeReaderPlus.BarcodeAngleSet = int.Parse(IniFile.Read("Visual", "BarcodeAngleSet", "0", Path));
                MyBarcodeReaderPlus.AllowableOffsetAngle_L = double.Parse(IniFile.Read("Visual", "AllowableOffsetAngle_L", "180", Path));
                MyBarcodeReaderPlus.AllowableOffsetAngle = double.Parse(IniFile.Read("Visual", "AllowableOffsetAngle", "180", Path));
                MyBarcodeReaderPlus.Overall_Quality = int.Parse(IniFile.Read("Visual", "Overall_Quality", "0", Path));
                MyBarcodeReaderPlus.Cell_Contrast = int.Parse(IniFile.Read("Visual", "Cell_Contrast", "0", Path));
                MyBarcodeReaderPlus.Print_Growth = int.Parse(IniFile.Read("Visual", "Print_Growth", "0", Path));
                MyBarcodeReaderPlus.Unused_Error_Correction = int.Parse(IniFile.Read("Visual", "Unused_Error_Correction", "0", Path));
                MyBarcodeReaderPlus.Cell_Modulation = int.Parse(IniFile.Read("Visual", "Cell_Modulation", "0", Path));
                MyBarcodeReaderPlus.Fixed_Pattern_Damage = int.Parse(IniFile.Read("Visual", "Fixed_Pattern_Damage", "0", Path));
                MyBarcodeReaderPlus.Grid_Nonuniformity = int.Parse(IniFile.Read("Visual", "Grid_Nonuniformity", "0", Path));
                MyBarcodeReaderPlus.Decode = int.Parse(IniFile.Read("Visual", "Decode", "0", Path));

                //檢測角度
                MyBarcodeReaderPlus.Mode_Angle = int.Parse(IniFile.Read("Angle", "Mode", "0", Path));
                MyBarcodeReaderPlus.Length1_Angle1 = double.Parse(IniFile.Read("Angle1", "Length1", "10", Path));
                MyBarcodeReaderPlus.Length2_Angle1 = double.Parse(IniFile.Read("Angle1", "Length2", "10", Path));
                MyBarcodeReaderPlus.Phi_Angle1 = double.Parse(IniFile.Read("Angle1", "Phi", "0", Path));
                MyBarcodeReaderPlus.Length_Angle1 = double.Parse(IniFile.Read("Angle1", "Length", "10", Path));
                MyBarcodeReaderPlus.MeasureSelect_Angle1 = int.Parse(IniFile.Read("Angle1", "MeasureSelect", "0", Path));
                MyBarcodeReaderPlus.MeasureThreshold_Angle1 = double.Parse(IniFile.Read("Angle1", "MeasureThreshold", "10", Path));
                MyBarcodeReaderPlus.MeasureTransition_Angle1 = IniFile.Read("Angle1", "MeasureTransition", "negative", Path);
                
            }
            catch 
            { 
            }

        }

        public void ReadLensCrack_AVI_Para()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            try
            {
                MyLensCrack_AVI.Radius_First = int.Parse(IniFile.Read("MyLensCrack_AVI", "FirstRadius", "1", Path));
                MyLensCrack_AVI.m_First.MeasureSelect = IniFile.Read("m_First", "MeasureSelect", "last", Path);
                MyLensCrack_AVI.m_First.Radius = int.Parse(IniFile.Read("m_First", "Radius", "1", Path));
                MyLensCrack_AVI.m_First.Length = int.Parse(IniFile.Read("m_First", "Length", "1", Path));
                MyLensCrack_AVI.m_First.MeasureTransition = IniFile.Read("m_First", "MeasureTransition", "negative", Path);
                MyLensCrack_AVI.m_First.MeasureThreshold = int.Parse(IniFile.Read("m_First", "MeasureThreshold", "1", Path));
            }
            catch
            {
            }
            try
            {
                MyLensCrack_AVI.OuterRadius_Lens = int.Parse(IniFile.Read("MyLensCrack_AVI", "OuterRadius_Lens", "1", Path));
                MyLensCrack_AVI.InnerRadius_Lens = int.Parse(IniFile.Read("MyLensCrack_AVI", "InnerRadius_Lens", "1", Path));
                MyLensCrack_AVI.m_Cutting.Gray = int.Parse(IniFile.Read("m_Cutting", "Gray", "1", Path));
                MyLensCrack_AVI.m_Cutting.Length1_Upper = int.Parse(IniFile.Read("m_Cutting", "Length1_Upper", "1", Path));
                MyLensCrack_AVI.m_Cutting.Length1_Lower = int.Parse(IniFile.Read("m_Cutting", "Length1_Lower", "1", Path));
                MyLensCrack_AVI.m_Cutting.Length2_Upper = int.Parse(IniFile.Read("m_Cutting", "Length2_Upper", "1", Path));
                MyLensCrack_AVI.m_Cutting.Length2_Lower = int.Parse(IniFile.Read("m_Cutting", "Length2_Lower", "1", Path));
                MyLensCrack_AVI.m_Cutting.CuttingDilation = int.Parse(IniFile.Read("m_Cutting", "CuttingDilation", "1", Path));
            }
            catch
            {
            }
            try
            {
                MyLensCrack_AVI.m_RegionDetection_1.OuterRadous = int.Parse(IniFile.Read("m_RegionDetection_1", "OuterRadous", "1", Path));
                MyLensCrack_AVI.m_RegionDetection_1.InnerRadous = int.Parse(IniFile.Read("m_RegionDetection_1", "InnerRadous", "1", Path));
                MyLensCrack_AVI.m_RegionDetection_1.Offset_Dark = int.Parse(IniFile.Read("m_RegionDetection_1", "Offset_Dark", "1", Path));
                MyLensCrack_AVI.m_RegionDetection_1.Offset_Light = int.Parse(IniFile.Read("m_RegionDetection_1", "Offset_Light", "1", Path));

                MyLensCrack_AVI.m_RegionDetection_2.OuterRadous = int.Parse(IniFile.Read("m_RegionDetection_2", "OuterRadous", "1", Path));
                MyLensCrack_AVI.m_RegionDetection_2.InnerRadous = int.Parse(IniFile.Read("m_RegionDetection_2", "InnerRadous", "1", Path));
                MyLensCrack_AVI.m_RegionDetection_2.Offset_Dark = int.Parse(IniFile.Read("m_RegionDetection_2", "Offset_Dark", "1", Path));
                MyLensCrack_AVI.m_RegionDetection_2.Offset_Light = int.Parse(IniFile.Read("m_RegionDetection_2", "Offset_Light", "1", Path));

                MyLensCrack_AVI.m_RegionDetection_3.OuterRadous = int.Parse(IniFile.Read("m_RegionDetection_3", "OuterRadous", "1", Path));
                MyLensCrack_AVI.m_RegionDetection_3.InnerRadous = int.Parse(IniFile.Read("m_RegionDetection_3", "InnerRadous", "1", Path));
                MyLensCrack_AVI.m_RegionDetection_3.Offset_Dark = int.Parse(IniFile.Read("m_RegionDetection_3", "Offset_Dark", "1", Path));
                MyLensCrack_AVI.m_RegionDetection_3.Offset_Light = int.Parse(IniFile.Read("m_RegionDetection_3", "Offset_Light", "1", Path));
            }
            catch
            {
            }
            try
            {
                MyLensCrack_AVI.m_Filter.Dilation_Width = int.Parse(IniFile.Read("m_Filter", "Dilation_Width", "1", Path));
                MyLensCrack_AVI.m_Filter.Dilation_Height = int.Parse(IniFile.Read("m_Filter", "Dilation_Height", "1", Path));
                MyLensCrack_AVI.m_Filter.Closing = int.Parse(IniFile.Read("m_Filter", "Closing", "1", Path));
                MyLensCrack_AVI.m_Filter.Select_Area = int.Parse(IniFile.Read("m_Filter", "Select_Area", "1", Path));
            }
            catch
            {
            }

        }
        public void ReadLen_Mold_Cave_Para()
        {
             
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            MyLens_Mold_Cave.dReduceRadius=Convert.ToInt32(IniFile.Read("Setting", "ReduceRadius", "2592", Path));
            MyLens_Mold_Cave.dGraythresholdUp = Convert.ToInt32(IniFile.Read("Setting", "GraythresholdUp", "255", Path));
            MyLens_Mold_Cave.dGraythresholdDown = Convert.ToInt32(IniFile.Read("Setting", "GraythresholdDown", "0", Path));
            MyLens_Mold_Cave.dAreaUp = Convert.ToInt32(IniFile.Read("Setting", "AreaUp", "100000", Path));
            MyLens_Mold_Cave.dAreaDown = Convert.ToInt32(IniFile.Read("Setting", "AreaDown", "0", Path));
            MyLens_Mold_Cave.dLength1Up = Convert.ToInt32(IniFile.Read("Setting", "Length1Up", "500", Path));
            MyLens_Mold_Cave.dLength1Down = Convert.ToInt32(IniFile.Read("Setting", "Length1Down", "0", Path));
            MyLens_Mold_Cave.dLength2Up = Convert.ToInt32(IniFile.Read("Setting", "Length2Up", "500", Path));
            MyLens_Mold_Cave.dLength2Down = Convert.ToInt32(IniFile.Read("Setting", "Length2Down", "0", Path));
            MyLens_Mold_Cave.dthreshold = Convert.ToInt32(IniFile.Read("Setting", "threshold", "30", Path));
            MyLens_Mold_Cave.dcliperNum = Convert.ToInt32(IniFile.Read("Setting", "cliperNum", "20", Path));
            MyLens_Mold_Cave.dcliperlength = Convert.ToInt32(IniFile.Read("Setting", "cliperlength", "80", Path));
            MyLens_Mold_Cave.dcliperwidth = Convert.ToInt32(IniFile.Read("Setting", "cliperwidth", "80", Path));
            MyLens_Mold_Cave.spolarity   = IniFile.Read("Setting", "polarity", "positive", Path);
            MyLens_Mold_Cave.sedgeSelect = IniFile.Read("Setting", "edgeSelect", "first", Path);     
            MyLens_Mold_Cave.dOCRCenterDistance = Convert.ToInt32(IniFile.Read("Setting", "OCRCenterDistance", "850", Path));
            MyLens_Mold_Cave.dOCRRectangleLength= Convert.ToInt32(IniFile.Read("Setting", "OCRRectangleLength", "80", Path));
            MyLens_Mold_Cave.dOCRRectangleWidth = Convert.ToInt32(IniFile.Read("Setting", "OCRRectangleWidth", "80", Path));
            MyLens_Mold_Cave.dOCRthresholdUp= Convert.ToInt32(IniFile.Read("Setting", "OCRthresholdUp", "255", Path));
            MyLens_Mold_Cave.dOCRthresholdDown = Convert.ToInt32(IniFile.Read("Setting", "OCRthresholdDown", "0", Path));
            MyLens_Mold_Cave.dOCRWidthUp= Convert.ToInt32(IniFile.Read("Setting", "OCRWidthUp", "100", Path));
            MyLens_Mold_Cave.dOCRWidthDown= Convert.ToInt32(IniFile.Read("Setting", "OCRWidthDown", "0", Path));
            MyLens_Mold_Cave.dOCRHeightUp= Convert.ToInt32(IniFile.Read("Setting", "OCRHeightUp", "100", Path));
            MyLens_Mold_Cave.dOCRHeightDown = Convert.ToInt32(IniFile.Read("Setting", "OCRHeightDown", "0", Path));
            MyLens_Mold_Cave.iThresholdSelect = Convert.ToInt32(IniFile.Read("Setting", "iThresholdSelect", "0", Path));
            string bstr="";
            bstr= IniFile.Read("Setting", "binGegion","True", Path);
            if (bstr == "True")
            {
                MyLens_Mold_Cave.binGegion = true;
            }
            else
            {
                MyLens_Mold_Cave.binGegion = false;
 
            }
            MyLens_Mold_Cave.dMeanfilte=Convert.ToInt32(IniFile.Read("Setting", "dMeanfilte", "40", Path));
            MyLens_Mold_Cave.dCloseWidth=Convert.ToInt32(IniFile.Read("Setting", "dCloseWidth", "1", Path));
            MyLens_Mold_Cave.dCloseHeight=Convert.ToInt32(IniFile.Read("Setting", "dCloseHeight", "1", Path));
            MyLens_Mold_Cave.dOpenWidth =Convert.ToInt32(IniFile.Read("Setting", "dOpenWidth", "1", Path));
            MyLens_Mold_Cave.dOpenHeight=Convert.ToInt32(IniFile.Read("Setting", "dOpenHeight","1", Path));
            string sClosing="";
            sClosing =IniFile.Read("Setting", "bClosing", "True", Path);
            if (sClosing == "True")
            {
                MyLens_Mold_Cave.bClosing = true;
            }
            else
            {
                MyLens_Mold_Cave.bClosing = false;

            }
            string sOpenging="";
            sOpenging=IniFile.Read("Setting", "bOpenging","True", Path);
            if (sOpenging == "True")
            {
                MyLens_Mold_Cave.bOpenging = true;
            }
            else
            {
                MyLens_Mold_Cave.bOpenging = false;

            }                                      

        }
        public void ReadLensCarryPara()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            m_LensCarry.m_Correction.Angle = double.Parse(IniFile.Read("Correction", "Angle", "0", Path));
            m_LensCarry.m_Correction.Pixelmm = double.Parse(IniFile.Read("Correction", "Pixelmm", "0.0044", Path));
            Protocol.Position_Distance_X = double.Parse(IniFile.Read("Correction", "Position_Distance_X", "0", Path));
            Protocol.Position_Distance_Y = double.Parse(IniFile.Read("Correction", "Position_Distance_Y", "0", Path));

            m_LensCarry.m_CircleCenter.FirstRadius = int.Parse(IniFile.Read("CircleCenter", "FirstRadius", "500", Path));
            m_LensCarry.m_CircleCenter.ModelGrade = int.Parse(IniFile.Read("CircleCenter", "ModelGrade", "70", Path));
            m_LensCarry.m_CircleCenter.ModelMode = int.Parse(IniFile.Read("CircleCenter", "ModelMode", "0", Path));
            m_LensCarry.m_CircleCenter.Radius = int.Parse(IniFile.Read("CircleCenter", "Radius", "100", Path));
            m_LensCarry.m_CircleCenter.Measure_Transition = IniFile.Read("CircleCenter", "Measure_Transition", "negative", Path);
            m_LensCarry.m_CircleCenter.Measure_Select = IniFile.Read("CircleCenter", "Measure_Select", "last", Path);
            m_LensCarry.m_CircleCenter.Num_Measures = int.Parse(IniFile.Read("CircleCenter", "Num_Measures", "100", Path));
            m_LensCarry.m_CircleCenter.Measure_Length1 = int.Parse(IniFile.Read("CircleCenter", "Measure_Length1", "30", Path));
            m_LensCarry.m_CircleCenter.Measure_Length2 = int.Parse(IniFile.Read("CircleCenter", "Measure_Length2", "10", Path));
            m_LensCarry.m_CircleCenter.Measure_Threshold = int.Parse(IniFile.Read("CircleCenter", "Measure_Threshold", "40", Path));

            m_LensCarry.m_Angle1.OuterRadius = int.Parse(IniFile.Read("Angle1", "OuterRadius", "1000", Path));
            m_LensCarry.m_Angle1.InnerRadius = int.Parse(IniFile.Read("Angle1", "InnerRadius", "500", Path));

            m_LensCarry.m_Angle1.InterSectingRectangle = bool.Parse(IniFile.Read("Angle1", "InterSectingRectangle", "false", Path));
            m_LensCarry.m_Angle1.Length1 = int.Parse(IniFile.Read("Angle1", "Length1", "100", Path));
            m_LensCarry.m_Angle1.Length2 = int.Parse(IniFile.Read("Angle1", "Length2", "100", Path));
            m_LensCarry.m_Angle1.Measure_Transition = IniFile.Read("Angle1", "Measure_Transition", "negative", Path);
            m_LensCarry.m_Angle1.Measure_Select = IniFile.Read("Angle1", "Measure_Select", "last", Path);
            m_LensCarry.m_Angle1.Num_Measures = int.Parse(IniFile.Read("Angle1", "Num_Measures", "100", Path));
            m_LensCarry.m_Angle1.Measure_Length1 = int.Parse(IniFile.Read("Angle1", "Measure_Length1", "30", Path));
            m_LensCarry.m_Angle1.Measure_Length2 = int.Parse(IniFile.Read("Angle1", "Measure_Length2", "10", Path));
            m_LensCarry.m_Angle1.Measure_Threshold = int.Parse(IniFile.Read("Angle1", "Measure_Threshold", "40", Path));
           
            m_LensCarry.m_Angle1.ContrastSet = int.Parse(IniFile.Read("Angle1", "ContrastSet", "0", Path));
            m_LensCarry.m_Angle1.Gray = int.Parse(IniFile.Read("Angle1", "Gray", "128", Path));

            m_LensCarry.m_Angle1.Opening = bool.Parse(IniFile.Read("Angle1", "Opening", "false", Path));
            m_LensCarry.m_Angle1.Closing = bool.Parse(IniFile.Read("Angle1", "Closing", "false", Path));
            m_LensCarry.m_Angle1.OpeningValue = int.Parse(IniFile.Read("Angle1", "OpeningValue", "5", Path));
            m_LensCarry.m_Angle1.ClosingValue = int.Parse(IniFile.Read("Angle1", "ClosingValue", "5", Path));
            m_LensCarry.m_Angle1.Rect2_Len1_Upper = int.Parse(IniFile.Read("Angle1", "Rect2_Len1_Upper", "100", Path));
            m_LensCarry.m_Angle1.Rect2_Len1_Lower = int.Parse(IniFile.Read("Angle1", "Rect2_Len1_Lower", "0", Path));
            m_LensCarry.m_Angle1.Rect2_Len2_Upper = int.Parse(IniFile.Read("Angle1", "Rect2_Len2_Upper", "20", Path));
            m_LensCarry.m_Angle1.Rect2_Len2_Lower = int.Parse(IniFile.Read("Angle1", "Rect2_Len2_Lower", "0", Path));
            m_LensCarry.m_Angle1.SelectAreaMaximum = bool.Parse(IniFile.Read("Angle1", "SelectAreaMaximum", "false", Path));
           m_LensCarry.m_Angle1.StandardAngle =  int.Parse(IniFile.Read("Angle1", "StandardAngle", "0", Path));
        }


        public void ReadSetPara()
        {
            string addFactory = lblFactory.Text = IniFile.Read("Addition", "FactoryChoice", "", Sys.SysPath);
            int indexFactory = 0;
            switch (addFactory)
            {
                case "XM": indexFactory = 1; break;
                case "JM": indexFactory = 2; break;
                case "測試": indexFactory = 3; break;
            }
            Sys.Factory = indexFactory;
            string addFunction = lblFunction.Text = IniFile.Read("Addition", "Function", "", Sys.SysPath);
            int indexFunction = 0;
            switch (addFunction)
            {
                case "VDI識別": indexFunction = 0; Sys.FunctionString = "VDI"; break;
                case "點膠識別": indexFunction = 1; Sys.FunctionString = "Dispensing"; break;
                case "Soma偏識別": indexFunction = 2; Sys.FunctionString = "SOMA"; break;
                case "Soma正反缺陷識別": indexFunction = 3; Sys.FunctionString = "SOMA Defect"; break;
                case "VDI鍍膜識別": indexFunction = 4; Sys.FunctionString = "VDI Coating"; break;
                case "NIR識別": indexFunction = 5; Sys.FunctionString = "NIR"; break;
                case "塗墨識別": indexFunction = 6; Sys.FunctionString = "Inkiness"; break;
                case "分類方向識別": indexFunction = 7; Sys.FunctionString = "Classifier"; break;
                case "BarcodeReader": indexFunction = 8; Sys.FunctionString = "BarcodeReader"; break;
                case "BarcodeReaderPlus": indexFunction = 9; Sys.FunctionString = "BarcodeReaderPlus"; break;
                case "鏡裂檢測": indexFunction = 10; Sys.FunctionString = "LensCrack_AVI"; break;
                case "模别模穴号识别": indexFunction = 11; Sys.FunctionString = "Lens_Mold_Cave"; break;
                case "鏡片搬運": indexFunction = 12; Sys.FunctionString = "LensCarry"; break;
            }
            Sys.Function = indexFunction;

            Int16 CodesIndex = Convert.ToInt16(IniFile.Read("Addition", "Codes", "0", Sys.SysPath));
            switch (CodesIndex)
            {
                case 0: Sys.Codes = "M"; break;
                case 1: Sys.Codes = "T"; break;
                case 2: Sys.Codes = "F"; break;
                case 3: Sys.Codes = "C"; break;
                case 4: Sys.Codes = "M"; break;
                case 5: Sys.Codes = "K"; break;
                case 6: Sys.Codes = "Q"; break;
                case 7: Sys.Codes = "H"; break;
                case 8: Sys.Codes = "E"; break;
                case 9: Sys.Codes = "R"; break;
                case 10: Sys.Codes = "P"; break;
                case 11: Sys.Codes = "S"; break;
                case 12: Sys.Codes = "N"; break;
                case 13: Sys.Codes = "Other"; break;
            }
            Run.cbCode.SelectedIndex = CodesIndex;

            Int16 TypeIndex = Convert.ToInt16(IniFile.Read("Addition", "Type", "0", Sys.SysPath));
            switch (TypeIndex)
            {
                case 0: Sys.Type = "Semi-finished"; break;
                case 1: Sys.Type = "finished"; break;
            }
            Run.cbType.SelectedIndex = TypeIndex;

            Production.TotalProductionCount = int.Parse(IniFile.Read("Total Production", "Total Count", "NULL", Production.ProductionPath));
            lblProduction.Text = IniFile.Read("Current Production", "Current Production", "NULL", Production.ProductionPath);
            Production.CurProduction = IniFile.Read("Current Production", "Current Production", "NULL", Production.ProductionPath);
            lblMachineID.Text = Sys.MachineID = Environment.MachineName;
            //lblMachineID.Text = Sys.MachineID = IniFile.Read("System", "MachineID", "NULL", Sys.SysPath);
            Sys.OptionOriginal = Convert.ToBoolean(IniFile.Read("System", "OriginalSave", "False", Sys.SysPath));
            Sys.OptionOK = Convert.ToBoolean(IniFile.Read("System", "OkSave", "False", Sys.SysPath));
            Sys.OptionNG = Convert.ToBoolean(IniFile.Read("System", "NGSave", "False", Sys.SysPath));
            Sys.TrayMessage = Convert.ToBoolean(IniFile.Read("System", "TrayMessage", "False", Sys.SysPath));
            Sys.LastResult = Convert.ToBoolean(IniFile.Read("System", "LastResult", "False", Sys.SysPath));
            Sys.ReadBarcodeLog = Convert.ToBoolean(IniFile.Read("System", "ReadBarcodeLog", "False", Sys.SysPath));
            Sys.VisitWebService = Convert.ToBoolean(IniFile.Read("System", "VisitWebService", "False", Sys.SysPath));
            Sys.ManualClear = Convert.ToBoolean(IniFile.Read("System", "ManualClear", "False", Sys.SysPath));
            CCD.CCDBrand = Convert.ToInt32(IniFile.Read("CCD", "CCDChoice", "0", Sys.SysPath));
            CCD.ip = IPAddress.Parse(IniFile.Read("CCD", "Ip", "192.18.1.2", Sys.SysPath));
            CCD.Port = int.Parse(IniFile.Read("CCD", "Port", "3000", Sys.SysPath));
            Plc.ip = IPAddress.Parse(IniFile.Read("PLC", "Ip", "192.18.3.2", Sys.SysPath));
            Plc.Port = int.Parse(IniFile.Read("PLC", "Port", "3000", Sys.SysPath));
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
           
            CCD.Gain = double.Parse(IniFile.Read("Setting", "Gain", "0", Path));
            CCD.ExposureTime = double.Parse(IniFile.Read("Setting", "ExposureTime", "35000", Path));
            Count.iOK = int.Parse(IniFile.Read("System", "OKCount", "0", Sys.SysPath));
            Count.iNG = int.Parse(IniFile.Read("System", "NGCount", "0", Sys.SysPath));
            Count.iNG2 = int.Parse(IniFile.Read("System", "NG2Count", "0", Sys.SysPath));
            Count.iNG3 = int.Parse(IniFile.Read("System", "NG3Count", "0", Sys.SysPath));
            Count.iNG4 = int.Parse(IniFile.Read("System", "NG4Count", "0", Sys.SysPath));
            Count.iNG5 = int.Parse(IniFile.Read("System", "NG5Count", "0", Sys.SysPath));
            Count.iMiss = int.Parse(IniFile.Read("System", "MissCount", "0", Sys.SysPath));
            Count.iTest = int.Parse(IniFile.Read("System", "TestCount", "0", Sys.SysPath));
            Count.iTestTotal = int.Parse(IniFile.Read("System", "TestTotalCount", "0", Sys.SysPath));
            Sys.bThrow_OK = bool.Parse(IniFile.Read("System", "Throw_OK","false", Sys.SysPath));
            Sys.bThrow_NG = bool.Parse(IniFile.Read("System", "Throw_NG", "false", Sys.SysPath));
            Sys.bThrow_NG2 = bool.Parse(IniFile.Read("System", "Throw_NG2", "false", Sys.SysPath));
            Sys.bThrow_NG3 = bool.Parse(IniFile.Read("System", "Throw_NG3", "false", Sys.SysPath));
            Sys.bThrow_NG4 = bool.Parse(IniFile.Read("System", "Throw_NG4", "false", Sys.SysPath));
            Sys.bThrow_NG5 = bool.Parse(IniFile.Read("System", "Throw_NG5", "false", Sys.SysPath));
            Sys.bThrow_Miss = bool.Parse(IniFile.Read("System", "Throw_Miss", "false", Sys.SysPath));
        }

        public void LoadParaLight()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            Light.LightSet_1 = int.Parse(IniFile.Read("SettingLight", "Light1", "0", Path));
            Light.LightSet_2 = int.Parse(IniFile.Read("SettingLight", "Light2", "0", Path));
            Light.LightSet_3 = int.Parse(IniFile.Read("SettingLight", "Light3", "0", Path));
            Light.LightSet_4 = int.Parse(IniFile.Read("SettingLight", "Light4", "0", Path));
        }

        public void LightSetting()
        {
            try
            {
                //Light
                string Port = IniFile.Read("Light", "Port", "1", Sys.SysPath);
                int Port_1 = 0;
                if (Int32.TryParse(Port, out Port_1))
                {
                    Light.Com.PortName = "COM" + Port;
                }
                string baudrate_1 = IniFile.Read("Light", "Baudrate", "19200", Sys.SysPath);
                int Baudrate_1 = 0;
                if (Int32.TryParse(baudrate_1, out Baudrate_1))
                {
                    Light.Com.BaudRate = Baudrate_1;
                }
                string dataBit_1 = IniFile.Read("Light", "DataBit", "8", Sys.SysPath);
                int DataBit_1 = 0;
                if (Int32.TryParse(dataBit_1, out DataBit_1))
                {
                    Light.Com.DataBits = DataBit_1;
                }
                string parity_1 = IniFile.Read("Light", "Parity", "default", Sys.SysPath);
                switch (parity_1)
                {
                    case "Even": Light.Com.Parity = Parity.Even; break;
                    case "Odd": Light.Com.Parity = Parity.Odd; break;
                    case "None": Light.Com.Parity = Parity.None; break;
                }
                string stopBit_1 = IniFile.Read("Light", "StopBit", "1", Sys.SysPath);
                switch (stopBit_1)
                {
                    case "1": Light.Com.StopBits = StopBits.One; break;
                    case "2": Light.Com.StopBits = StopBits.Two; break;
                }
                //if (Light.Com.IsOpen)
                //{
                //    MessageBox.Show("Light串口被占用," + Light.Com.PortName);
                //}
                //else
                //{
                //    Light.Com.Open();
                //    Light.Com.DiscardInBuffer();
                //    Light.Com.DiscardOutBuffer();
                //}

                //设置com1参数
                com1.PortName = Light.Com.PortName;
                com1.BaudRate = Light.Com.BaudRate;// 19200;
                com1.DataBits = Light.Com.DataBits;// 8;
                com1.Parity = Light.Com.Parity;// Parity.None;
                com1.StopBits = Light.Com.StopBits;// StopBits.One;
                com1.ReceivedBytesThreshold = 1;
                com1.Open();
                Light.IsConnected = true;

                //cmbChs.SelectedIndex = 0;
                //读取光源所有参数
                byte[] cmd = Lighter.ReadAllPara();
                //ShowCmd(cmd);
                com1.ReceivedBytesThreshold = 18;//当接收到18个字节才触发DataReceived事件，
                //以防数据被分成两次接收
                com1.Write(cmd, 0, cmd.Length);
                //启动辅助线程， 判断是否正常接收到参数
                Thread t1 = new Thread(GetStatus);
                t1.IsBackground = true;
                t1.Start();
            }
            catch
            {
                Light.IsConnected = false;
            }
        }
        //获取光源参数（状态)
        bool getStatusFlag = true;
        byte[] status_byte; //bool lightconn = false; int lightcount = 0;
        public int lit = 0, brit = 10, ch = 0;
        public void LIGHT1Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            ////启动辅助线程， 判断是否正常接收到参数
            byte[] input = new byte[Light.Com.BytesToRead];
            Light.Com.Read(input, 0, Light.Com.BytesToRead);
            //FormLoad接收光源参数
            if (getStatusFlag)
            {
                status_byte = input;
                getStatusFlag = false;
            }
        }
        public void GetStatus()//获取光源参数（状态)
        {
            int timeOut = 0;
            while (keepReading)
            {
                if (!quit && Light.Com.IsOpen)
                {
                    if (status_byte == null)
                    {
                        Thread.Sleep(10);
                        timeOut += 10;
                        if (timeOut > 1000)
                        {
                            lit++;
                            if (lit > 10)
                            {
                                lit = 0;
                                Light.IsConnected = false;
                                //MessageBox.Show("获取光源参数超时,请重新开启光源和程式！");
                                byte[] cmd = Lighter.ReadAllPara();
                                Light.Com.ReceivedBytesThreshold = 18;//当接收到18个字节才触发DataReceived事件，以防数据被分成两次接收
                                Light.Com.Write(cmd, 0, cmd.Length);
                                getStatusFlag = true;
                            }
                            //break;
                        }
                        Thread.Sleep(10);
                    }
                    else
                    {
                        lit = 0;
                        Light.IsConnected = true;
                    }
                }
                else
                {
                    break;
                }
            }
            //重新设定com1的DataReceived事件触发的条件
            Light.Com.ReceivedBytesThreshold = 1;
        }
        public void LightOn_All()
        {
            try
            {
                byte[] cmd = Lighter.SetOnOff(0, true);
                com1.Write(cmd, 0, cmd.Length);
                cmd = Lighter.SetOnOff(1, true);
                com1.Write(cmd, 0, cmd.Length);
                cmd = Lighter.SetOnOff(2, true);
                com1.Write(cmd, 0, cmd.Length);
                cmd = Lighter.SetOnOff(3, true);
                com1.Write(cmd, 0, cmd.Length);
                if (Sys.Function == 0)
                {
                    VDIVisionSet.LightSetting(1 - 1, Light.LightSet_1);
                    VDIVisionSet.LightSetting(2 - 1, Light.LightSet_2);
                    VDIVisionSet.LightSetting(3 - 1, Light.LightSet_3);
                    VDIVisionSet.LightSetting(4 - 1, Light.LightSet_4);
                }
                if (Sys.Function == 5)
                {
                    VDI_NIR.LightSetting(1 - 1, Light.LightSet_1);
                    VDI_NIR.LightSetting(2 - 1, Light.LightSet_2);
                    VDI_NIR.LightSetting(3 - 1, Light.LightSet_3);
                    VDI_NIR.LightSetting(4 - 1, Light.LightSet_4);
                }
            }
            catch
            {
                //MessageBox.Show("光源未連接");
            }
        }

        public void LightSetting(int ch, int brit)
        {
            try
            {
                byte[] cmd = Lighter.SetBrit(ch, brit);
                //ShowCmd(cmd);
                com1.Write(cmd, 0, 8);
            }
            catch
            {
            }
        }

        //指示当前激活的窗口
        public void FrmIndicate(Label frmIndex)
        {
            lblInFrmSetUp.BackColor = Color.Silver;
            lblInFrmRun.BackColor = Color.Silver;
            lblToFrmVisionSet.BackColor = Color.Silver;
            lblInLogIn.BackColor = Color.Silver;
            frmIndex.BackColor = Color.Red;
        }
        //关闭除了FrmRun窗口以外的其他窗口
        void CloseAllChildrenExceptRun()
        {
            Form[] children = this.MdiChildren;
            foreach (Form frm in children)
            {
                if (!(frm is FrmRun))
                    frm.Dispose();
            }
            GC.Collect();
        }
        void CloseAllChildren()
        {
            Form[] children = this.MdiChildren;
            foreach (Form frm in children)
            {
                frm.Dispose();
            }
            GC.Collect();
        }
        private void btnToFrmLogIn_Click(object sender, EventArgs e)
        {
            if (Plc.Status == 1)
            {
                MessageBox.Show("運行中，請等待流程完畢。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            if (User.CurrentUser != "")
            {
                return;
            }
            if (this.ActiveMdiChild is FrmLogIn)
                return;
            //ImageSetting.StopPreview();
            FrmLogIn login = new FrmLogIn(this);
            login.Show();
            FrmIndicate(lblInLogIn);
            Protocol.bPCIsConnect = false;
            Protocol.Result_PCIsConnect = 0;

        }
        public static int flag = 0;
        private void btnToFrmSetUp_Click(object sender, EventArgs e)
        {
            if (Plc.Status == 1)
            {
                MessageBox.Show("運行中，請等待流程完畢。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            if (this.ActiveMdiChild is FrmSetUp)
                return;
            //ImageSetting.StopPreview();
            if (User.CurrentUser == "")
            {
                flag = 1;
                btnToFrmLogIn.PerformClick();
                return;
            }
            //CloseAllChildren();
            CloseAllChildrenExceptRun();
            Run.Hide();
            FrmSetUp setup = new FrmSetUp(this);
            setup.Show();
            On_Auto_InterFace = true;
            FrmIndicate(lblInFrmSetUp);
            Protocol.bPCIsConnect = false;
            Protocol.Result_PCIsConnect = 0;
        }

        private void btnToFrmRun_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is FrmRun)
                return;
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            //ImageSetting.StopPreview();
            //CloseAllChildren();
            CloseAllChildrenExceptRun();
            if (Sys.Function == 6)
            {
                VDI_INK.Hide();
            }
            Run.Show();

            //關閉設定模式
            On_Auto_InterFace = true;
            FrmIndicate(lblInFrmRun);
            Protocol.bPCIsConnect = true;
            Protocol.Result_PCIsConnect = 1;
            if (Plc.TriggerMode != 2)
            {
                Stop();
                SetTriggerMode_Software();
            }
            else
            {
                SetTriggerMode_Line1();
                Protocol.bVisionOK = true;
            }
            //Protocol.bVisionOK = false;
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            if (Plc.Status == 1)
            {
                MessageBox.Show("運行中，請等待流程完畢。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            Protocol.bPCIsConnect = false;
            Protocol.Result_PCIsConnect = 0;
            DialogResult dr = MessageBox.Show("確定要離開本程序嗎?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.OK)
            {
               // ImageSetting.StopPreview();
                //LightOn_All();
                CloseAllChildren();
                this.Close();
                Application.Exit();
            }
        }

        private void timerRunningTime_Tick(object sender, EventArgs e)
        {
            lblVersions.Text = "V" + Protocol.Versions_PC + "-" + Protocol.Versions_PLC + "-" + Protocol.Versions_HMI;
            TimeRunning += 1;
            Days = (int)(TimeRunning / 86400);
            int left = (int)(TimeRunning % 86400);
            Hours = left / 3600;
            left = left % 3600;
            Minutes = left / 60;
            Seconds = left % 60;
            lblRunTime.Text = string.Format("{0,2}天{1,2}时{2,2}分{3,2}秒", Days, Hours, Minutes, Seconds);

            DateTime dt = DateTime.Now;
            string DayOfWeek = "";
            switch (dt.DayOfWeek)
            {
                case System.DayOfWeek.Monday: DayOfWeek = "星期一"; break;
                case System.DayOfWeek.Tuesday: DayOfWeek = "星期二"; break;
                case System.DayOfWeek.Wednesday: DayOfWeek = "星期三"; break;
                case System.DayOfWeek.Thursday: DayOfWeek = "星期四"; break;
                case System.DayOfWeek.Friday: DayOfWeek = "星期五"; break;
                case System.DayOfWeek.Saturday: DayOfWeek = "星期六"; break;
                case System.DayOfWeek.Sunday: DayOfWeek = "星期日"; break;
            }
            lblDate.Text = dt.ToString("yyyy/MM/dd ") + DayOfWeek;
            lblTime.Text = dt.ToString("HH:mm:ss");

            lblX.Text = Protocol.AxisCoordinate_X.ToString();
            lblY.Text = Protocol.AxisCoordinate_Y.ToString();

            if (Tray.OpDateTime.Day != DateTime.Now.Day)
                RemoveFiles();
        }
        
        public void RemoveFiles()
        {
            string strFolderPath0 = Sys.ImageSavePath0;
            if (!Directory.Exists(strFolderPath0))//如果沒資料夾就自己創建
            {
                Directory.CreateDirectory(strFolderPath0);
            }
            DirectoryInfo dyInfo = new DirectoryInfo(strFolderPath0);
            //获取文件夹下所有的文件
            foreach (DirectoryInfo dirInfo in dyInfo.GetDirectories())
            {
                //判断文件夾創建日期是否大於3天，是则删除
                if (dirInfo.CreationTime < DateTime.Now.AddDays(-3))
                    dirInfo.Delete(true);
            }

            string strFolderPath = Sys.ImageSavePath;
            if (!Directory.Exists(strFolderPath))//如果沒資料夾就自己創建
            {
                Directory.CreateDirectory(strFolderPath);
            }
            DirectoryInfo dyInfo2 = new DirectoryInfo(strFolderPath);
            //获取文件夹下所有的文件
            foreach (DirectoryInfo dirInfo2 in dyInfo2.GetDirectories())
            {
                //判断文件夾創建日期是否大於3天，是则删除
                if (dirInfo2.CreationTime < DateTime.Now.AddDays(-3))
                    dirInfo2.Delete(true);
            }
        }
        private void btnImageSetUp_Click(object sender, EventArgs e)
        {
            if (Plc.Status == 1)
            {
                MessageBox.Show("運行中，請等待流程完畢。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            //VisionSetting = true;
            
            if (this.ActiveMdiChild is FrmVisionSet)
                return;
            //ImageSetting.StopPreview();
            if (User.CurrentUser == "")
            {
                flag = 2;
                btnToFrmLogIn.PerformClick();
                return;
            }
            //CloseAllChildren();
            if (Sys.Function == 1)
            {
                if(My.TestDirection)
                {
                    VisionSet.Clear_Temple();
                }
            }
            else if (Sys.Function == 5)
            {
                VDI_NIR.Clear_Temple();
            }
            CloseAllChildrenExceptRun();
            Run.Hide();
            //不在自動介面
            On_Auto_InterFace = false;
            switch (Sys.Function)
            {
                case 0:
                    {
                        VDIVisionSet = new FrmVDIVisionSet(this);
                        VDIVisionSet.Show();
                        break;
                    }
                case 1:
                    {
                        VisionSet = new FrmVisionSet(this);
                        VisionSet.Show();
                        break;
                    }
                case 2:
                    {
                        SomaVisionSet = new Frm8982SomaVisionSet(this);
                        SomaVisionSet.Show();
                        break;
                    }
                case 3:
                    {
                        SomaDetectionVS = new FrmSomaDetectionVS(this);
                        SomaDetectionVS.Show();
                        break;
                    }
                case 4:
                    {
                        VDICoatingVS = new FrmVDICoatingVS(this);
                        VDICoatingVS.Show();
                        break;
                    }
                case 5:
                    {
                        VDI_NIR = new FrmVDI_NIR(this);
                        VDI_NIR.Show();
                        break;
                    }
                case 6:
                    {
                        VDI_INK = new FrmVDI_INK(this);
                        VDI_INK.Show();
                        break;
                    }
                case 7:
                    {
                        Classifier = new FrmClassifier(this);
                        Classifier.Show();
                        break;
                    }
                case 8:
                    {
                        BR = new FrmBarcodeReader(this);
                        BR.Show();
                        break;
                    }
                case 9:
                    {
                        BRPlus = new FrmBarcodeReaderPlus(this);
                        BRPlus.Show();
                        break;
                    }
                case 10:
                    {
                        LensCrack_AVI = new FrmLensCrack_AVI(this);
                        LensCrack_AVI.Show();
                        break;
                    }
                case 11:
                    {
                        Lens_Mold_Cave = new Frm8958BPFOCRVision(this);
                        Lens_Mold_Cave.Show();
                        break;
                    }
                case 12:
                    {
                        LensCarry = new FrmLensCarry(this);
                        LensCarry.Show();
                        break;
                    }
            }
           
            //切換為軟體觸發

            SetTriggerMode_Software();
            Stop();
            FrmIndicate(lblToFrmVisionSet);
            Protocol.bPCIsConnect = false;
            Protocol.Result_PCIsConnect = 0;
        }

        private void ScanPlc_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (ScanPlc.CancellationPending)
                {
                    return;
                }
                if (Plc.IsConnected)
                {
                    try
                    {
                        string recvData = "";
                        //D0 ~ D19
                        int len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.ReadD0toD19));
                        if (len != Protocol.ReadD0toD19.Length)
                        {
                            throw new Exception();
                        }
                        byte[] Recv = new byte[1024];
                        len = Enet.Receive(Recv, 1024, SocketFlags.None);
                        if (len >= 0)
                        {
                            SplitData1(Encoding.ASCII.GetString(Recv, 0, len));
                        }
                        else
                        {
                            throw new Exception();
                        }

                        //D30~45
                        len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.ReadD30toD45));
                        if (len != Protocol.ReadD30toD45.Length)
                        {
                            throw new Exception();
                        }
                        Recv = new byte[1024];
                        len = Enet.Receive(Recv, 1024, SocketFlags.None);
                        if (len >= 0)
                        {
                            SplitData2(Encoding.ASCII.GetString(Recv, 0, len));
                        }
                        else
                        {
                            throw new Exception();
                        }


                        //D2112 读取单颗周期（两位小数点）
                        len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.SingleCycle));
                        if (len != Protocol.SingleCycle.Length)
                        {
                            throw new Exception();
                        }
                        Recv = new byte[1024];
                        len = Enet.Receive(Recv, 1024, SocketFlags.None);
                        if (len >= 0)
                        {
                            recvData = Encoding.ASCII.GetString(Recv, 0, len);
                            if (recvData.StartsWith("8100"))
                            {
                                Protocol.Result_Cycle = (double)(Convert.ToInt32(recvData.Substring(4, 4), 16)) / 100;
                            }
                            else
                                throw new Exception();
                        }
                        else
                        {
                            throw new Exception();
                        }
                        //影像結果回傳PLC
                        if (Protocol.bVisionOK)
                        {
                            Protocol.bVisionOK = false;
                            len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.Vision + Protocol.Result_Vision.ToString("X4")));
                            if (len < 10)
                            {
                                throw new Exception();
                            }
                            Recv = new byte[1024];
                            len = Enet.Receive(Recv, 1024, SocketFlags.None);
                            string dd = Encoding.ASCII.GetString(Recv, 0, len);
                            if (!dd.StartsWith("8300"))
                            {
                                throw new Exception();
                            }
                        }
                        //Barcode結果回傳PLC
                        if (Protocol.bBarcodeOK)
                        {
                            Protocol.bBarcodeOK = false;
                            len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.BarcodeOK + Protocol.Result_BarcodeOK.ToString("X4")));
                            if (len < 10)
                            {
                                throw new Exception();
                            }
                            Recv = new byte[1024];
                            len = Enet.Receive(Recv, 1024, SocketFlags.None);
                            if (len <= 0)
                            {
                                throw new Exception();
                            }
                        }
                        /*
                        //D2122 盘1周期（一位小数点）
                        len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.P1Cycle));
                        if (len != Protocol.P1Cycle.Length)
                        {
                            throw new Exception();
                        }
                        Recv = new byte[1024];
                        len = Enet.Receive(Recv, 1024, SocketFlags.None);
                        if (len >= 0)
                        {
                            recvData = Encoding.ASCII.GetString(Recv, 0, len);
                            if (recvData.StartsWith("8100"))
                            {
                                Protocol.Result_P1Cycle = (double)(Convert.ToInt32(recvData.Substring(4, 4), 16)) / 10;
                            }
                            else
                                throw new Exception();
                        }
                        else
                        {
                            throw new Exception();
                        }

                        //D2132 盘2周期（一位小数点）
                        len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.P2Cycle));
                        if (len != Protocol.P2Cycle.Length)
                        {
                            throw new Exception();
                        }
                        Recv = new byte[1024];
                        len = Enet.Receive(Recv, 1024, SocketFlags.None);
                        if (len >= 0)
                        {
                            recvData = Encoding.ASCII.GetString(Recv, 0, len);
                            if (recvData.StartsWith("8100"))
                            {
                                Protocol.Result_P2Cycle = (double)(Convert.ToInt32(recvData.Substring(4, 4), 16)) / 10;
                            }
                            else
                                throw new Exception();
                        }
                        else
                        {
                            throw new Exception();
                        }
                        */
                        //D8340 X轴当前坐标（两位小数点）
                        len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.XCurrent));
                        if (len != Protocol.XCurrent.Length)
                        {
                            throw new Exception();
                        }
                        Recv = new byte[1024];  
                        len = Enet.Receive(Recv, 1024, SocketFlags.None);
                        if (len >= 0)
                        {
                            recvData = Encoding.ASCII.GetString(Recv, 0, len);
                            if (recvData.StartsWith("8100"))
                            {
                                Protocol.AxisCoordinate_X = (double)(Convert.ToInt32(recvData.Substring(4, 4), 16)) / 100;
                            }
                            else
                                throw new Exception();
                        }
                        else
                        {
                            throw new Exception();
                        }

                        //D8350 Y轴当前坐标（两位小数点）
                        len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.YCurrent));
                        if (len != Protocol.YCurrent.Length)
                        {
                            throw new Exception();
                        }
                        Recv = new byte[1024];
                        len = Enet.Receive(Recv, 1024, SocketFlags.None);
                        if (len >= 0)
                        {
                            recvData = Encoding.ASCII.GetString(Recv, 0, len);
                            if (recvData.StartsWith("8100"))
                            {
                                Protocol.AxisCoordinate_Y = (double)(Convert.ToInt32(recvData.Substring(4, 4), 16)) / 100;
                            }
                            else
                                throw new Exception();
                        }
                        else
                        {
                            throw new Exception();
                        }

                        //復位相機
                        if (Protocol.bTrigger)
                        {
                            Protocol.bTrigger = false;
                            len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.Trigger + Protocol.Result_Trigger.ToString("X4")));
                            if (len < 10)
                            {
                                throw new Exception();
                            }
                            Recv = new byte[1024];
                            len = Enet.Receive(Recv, 1024, SocketFlags.None);
                            string dd = Encoding.ASCII.GetString(Recv, 0, len);
                            if (!dd.StartsWith("8300"))
                            {
                                throw new Exception();
                            }
                        }
                        if (Protocol.bBarcodeTrigger)
                        {
                            Protocol.bBarcodeTrigger = false;
                            len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.BarcodeTrigger + Protocol.Result_BarcodeTrigger.ToString("X4")));
                            if (len < 10)
                            {
                                throw new Exception();
                            }
                            Recv = new byte[1024];
                            len = Enet.Receive(Recv, 1024, SocketFlags.None);
                            if (len <= 0)
                            {
                                throw new Exception();
                            }
                        }
                        //寫入吸嘴CCD距離
                        if (Protocol.bPositionDistance_SuctionNozzleCCD)
                        {
                            Protocol.bPositionDistance_SuctionNozzleCCD = false;
                            len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.Position_Distance + NToHString2((int)(Protocol.Position_Distance_X*100)) + NToHString2((int)(Protocol.Position_Distance_Y*100))));
                            if (len < 10)
                            {
                                throw new Exception();
                            }
                            Recv = new byte[1024];
                            len = Enet.Receive(Recv, 1024, SocketFlags.None);
                            if (len <= 0)
                            {
                                throw new Exception();
                            }
                        }

                        if (Plc.bPlateful_1)//盤一滿盤歸零
                        {
                            if (!Plc.VisualComplete)
                            {
                            }
                            else
                            {
                                Plc.VisualComplete = false;
                                Plc.bPlateful_1 = false;
                                len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.PlatefulClearOK_1 + Protocol.Result_PlatefulClearOK_1.ToString("X4")));
                                if (len < 10)
                                {
                                    throw new Exception();
                                }
                                Recv = new byte[1024];
                                len = Enet.Receive(Recv, 1024, SocketFlags.None);
                                string dd = Encoding.ASCII.GetString(Recv, 0, len);
                                if (dd != "8300")
                                {
                                    //throw new Exception();
                                }
                                try
                                {
                                    int ResultX = 0, ResultY = 0;
                                    if (Protocol.Result_XY_Opposite == 0)
                                    {
                                        ResultX = Tray.Columns_1;
                                        ResultY = Tray.Rows_1;
                                    }
                                    else
                                    {
                                        ResultX = Tray.Rows_1;
                                        ResultY = Tray.Columns_1;
                                    }
                                    bool SendResult = false;
                                    bool SendAngle = false;
                                    bool SendOffsetCenter = false;
                                    //判斷不同功能需要開啟的發送角度/圓心補償功能
                                    switch (Sys.Function)
                                    {
                                        case 1: SendResult = true; break;
                                        case 5: SendAngle = true; SendResult = true; break;
                                        case 9: SendAngle = true; SendResult = true; break;
                                        case 12: SendAngle = true; SendResult = true; SendOffsetCenter = true; break;
                                    }
                                    #region 發送分料結果
                                    if (SendResult)
                                    {
                                        for (int y = 1; y <= ResultY; y++)
                                        {
                                            string sResult = "";
                                            ToPlcResult_Result(y, ResultX, out sResult);
                                            Thread.Sleep(5);
                                            len = Enet.Send(Encoding.ASCII.GetBytes("03FF000A44200000" + (501 + (y - 1) * 20).ToString("X4") + "1400" + sResult));
                                            if (len <= 0)
                                            {
                                                throw new Exception();
                                            }
                                            Thread.Sleep(5);
                                            Recv = new byte[1024];
                                            len = Enet.Receive(Recv, 1024, SocketFlags.None);
                                            dd = Encoding.ASCII.GetString(Recv, 0, len);
                                            if (!dd.StartsWith("8300"))
                                            {
                                                throw new Exception();
                                            }
                                        }
                                    }
                                    if (SendAngle)
                                    {
                                        //角度結果
                                        for (int y = 1; y <= ResultY; y++)
                                        {
                                            string sAngle = "";
                                            ToPlcResult_Angle(y, ResultX, out sAngle);
                                            Thread.Sleep(5);
                                            len = Enet.Send(Encoding.ASCII.GetBytes("03FF000A44200000" + (2501 + (y - 1) * 20).ToString("X4") + "1400" + sAngle));
                                            if (len <= 0)
                                            {
                                                throw new Exception();
                                            }
                                            Thread.Sleep(5);
                                            Recv = new byte[1024];
                                            len = Enet.Receive(Recv, 1024, SocketFlags.None);
                                            dd = Encoding.ASCII.GetString(Recv, 0, len);
                                            if (!dd.StartsWith("8300"))
                                            {
                                                throw new Exception();
                                            }
                                        }
                                    }
                                    #endregion
                                    #region 發送中心補償結果
                                    if (SendOffsetCenter)
                                    {
                                        for (int y = 1; y <= ResultY; y++)
                                        {
                                            string sOffset_X = "";
                                            string sOffset_Y = "";
                                            for (int x = 1; x <= 20; x++)
                                            {
                                                if (x > ResultX)
                                                {
                                                    sOffset_X = sOffset_X + "0000";
                                                    sOffset_Y = sOffset_Y + "0000";
                                                }
                                                else
                                                {
                                                    int n = (y - 1) * ResultX + x - 1;
                                                    if (!Vision.VisionOffsetCenterColumn.ContainsKey(n))
                                                    {
                                                        sOffset_X = sOffset_X + "0000";
                                                        sOffset_Y = sOffset_Y + "0000";
                                                    }
                                                    else
                                                    {
                                                        sOffset_X = sOffset_X + NToHString((int)(Vision.VisionOffsetCenterColumn[n] * 100));
                                                        sOffset_Y = sOffset_Y + NToHString((int)(Vision.VisionOffsetCenterRow[n] * 100));
                                                    }
                                                }
                                            }
                                            Thread.Sleep(5);
                                            len = Enet.Send(Encoding.ASCII.GetBytes("03FF000A44200000" + (7001 + (y - 1) * 20).ToString("X4") + "1400" + sOffset_X));
                                            if (len <= 0)
                                            {
                                                throw new Exception();
                                            }
                                            Thread.Sleep(5);
                                            Recv = new byte[1024];
                                            Thread.Sleep(5);
                                            len = Enet.Receive(Recv, 1024, SocketFlags.None);
                                            dd = Encoding.ASCII.GetString(Recv, 0, len);
                                            if (!dd.StartsWith("8300"))
                                            {
                                                throw new Exception();
                                            }
                                            Thread.Sleep(5);
                                            len = Enet.Send(Encoding.ASCII.GetBytes("03FF000A44200000" + (7401 + (y - 1) * 20).ToString("X4") + "1400" + sOffset_Y));
                                            if (len <= 0)
                                            {
                                                throw new Exception();
                                            }
                                            Thread.Sleep(5);
                                            Recv = new byte[1024];
                                            len = Enet.Receive(Recv, 1024, SocketFlags.None);
                                            dd = Encoding.ASCII.GetString(Recv, 0, len);
                                            if (!dd.StartsWith("8300"))
                                            {
                                                throw new Exception();
                                            }
                                        }
                                    #endregion
                                    }
                                    len = Enet.Send(Encoding.ASCII.GetBytes("03FF000A44200000001801000001"));
                                    if (len <= 0)
                                    {
                                        throw new Exception();
                                    }
                                    Thread.Sleep(5);
                                    Recv = new byte[1024];
                                    len = Enet.Receive(Recv, 1024, SocketFlags.None);
                                    string cc = Encoding.ASCII.GetString(Recv, 0, len);
                                    if (!cc.StartsWith("8300"))
                                    {
                                        throw new Exception();
                                    }
                                }
                                catch
                                {
                                }
                            }
                                       
                            //}
                        }
                        if (Plc.bPlateful_2)//盤二滿盤歸零
                        {
                            Plc.bPlateful_2 = false;
                            len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.PlatefulClearOK_2 + Protocol.Result_PlatefulClearOK_2.ToString("X4")));
                            if (len < 10)
                            {
                                throw new Exception();
                            }
                            Recv = new byte[1024];
                            len = Enet.Receive(Recv, 1024, SocketFlags.None);
                            if (len <= 0)
                            {
                                throw new Exception();
                            }
                        }
                        //PC連接
                        if (Protocol.bPCIsConnect)
                        {
                            Protocol.bPCIsConnect = false;
                            len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.PCIsConnect + Protocol.Result_PCIsConnect.ToString("X4")));
                            if (len < 10)
                            {
                                throw new Exception();
                            }
                            Recv = new byte[1024];
                            len = Enet.Receive(Recv, 1024, SocketFlags.None);
                            if (len <= 0)
                            {
                                throw new Exception();
                            }
                        }
                        else
                        {
                            Protocol.bPCIsConnect = false;
                            len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.PCIsConnect + Protocol.Result_PCIsConnect.ToString("X4")));
                            if (len < 10)
                            {
                                throw new Exception();
                            }
                            Recv = new byte[1024];
                            len = Enet.Receive(Recv, 1024, SocketFlags.None);
                            if (len <= 0)
                            {
                                throw new Exception();
                            }
                        }
                        //PC連接
                        if (Protocol.bPCOK)
                        {
                            Protocol.bPCOK = false;
                            len = Enet.Send(Encoding.ASCII.GetBytes(Protocol.PCOK + Protocol.Result_PCOK.ToString("X4")));
                            if (len < 10)
                            {
                                throw new Exception();
                            }
                            Recv = new byte[1024];
                            len = Enet.Receive(Recv, 1024, SocketFlags.None);
                            if (len <= 0)
                            {
                                throw new Exception();
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        //提取数据1：
        void SplitData1(string data)
        {
            if (data.StartsWith("8100"))
            {
                string UseFul = data.Substring(4);
                if (UseFul.Length == 80)
                {
                    int buff;
                    //D0 Run or idle
                    buff = Convert.ToInt32(UseFul.Substring(0, 4), 16);
                    Plc.Status = buff;
                    //D1掃碼模式預留
                    
                    //D2 Tray1總行數
                    buff = Convert.ToInt32(UseFul.Substring(8, 4), 16);
                    Tray.Rows_1 = buff;
                    //D3 Tray1總列數
                    buff = Convert.ToInt32(UseFul.Substring(12, 4), 16);
                    Tray.Columns_1 = buff;
                    //D4 Tray2總行數
                    buff = Convert.ToInt32(UseFul.Substring(16, 4), 16);
                    Tray.Rows_2 = buff;
                    //D5 Tray2總列數
                    buff = Convert.ToInt32(UseFul.Substring(20, 4), 16);
                    Tray.Columns_2 = buff;
                    //D6 當前Tray序列
                    buff = Convert.ToInt32(UseFul.Substring(24, 4), 16);
                    //Tray.NowTray = buff;
                    //D7 當前行
                    buff = Convert.ToInt32(UseFul.Substring(28, 4), 16);
                    Tray.CurrentRow = buff;
                    //D8 當前列
                    buff = Convert.ToInt32(UseFul.Substring(32, 4), 16);
                    Tray.CurrentColumn = buff;
                    //D9 當前總點數 預留 
                    
                    //D10 觸發拍照
                    buff = Convert.ToInt32(UseFul.Substring(40, 4), 16);
                    Plc.Trigger = buff;
                    //D11 掃盤方向 1.左上 2.右上 3.左下 4.右下
                    buff = Convert.ToInt32(UseFul.Substring(44, 4), 16);
                    Plc.LDirection = buff;
                    //D12 Tray掃碼觸發
                    buff = Convert.ToInt32(UseFul.Substring(48, 4), 16);
                    Plc.BarcodeTrigger = buff;
                    //D13:R跑盘方向1左上角2右上角3左下角4右下角
                    buff = Convert.ToInt32(UseFul.Substring(52, 4), 16);
                    Plc.RDirection = buff;
                    //D14:触发模式 1软体2硬件
                    buff = Convert.ToInt32(UseFul.Substring(56, 4), 16);
                    Plc.TriggerMode = buff;
                    if(CCD.IsConnected)
                    {
                        if (Plc.TriggerMode_Last != Plc.TriggerMode)
                        {
                            Plc.TriggerMode_Last = Plc.TriggerMode;
                            if (Plc.TriggerMode != 2)
                            {
                                SetTriggerMode_Software();
                            }
                            else
                            {
                                SetTriggerMode_Line1();
                                Protocol.bVisionOK = false;
                            }
                        }
                    }
                    //D15:跑盘模式 1.仅盘1 2.仅盘2 3.盘1-盘2 4.盘2-盘1
                    buff = Convert.ToInt32(UseFul.Substring(60, 4), 16);
                    Plc.RunMode = buff;
                    //D16
                    buff = Convert.ToInt32(UseFul.Substring(64, 4), 16);
                    Plc.TrayMode_1 = buff;
                    //D17
                    buff = Convert.ToInt32(UseFul.Substring(68, 4), 16);
                    Plc.TrayMode_2 = buff;
                    //D18
                    buff = Convert.ToInt32(UseFul.Substring(72, 4), 16);
                    if (Plc.Plateful_1 == 0 && buff == 1)
                    {
                        Plc.bPlateful_1 = true;
                        Plc.bPlateful_WS = true;
                    }
                    //else if (Plc.Plateful_1 == 1 && buff == 0)
                    //{
                    //    Plc.bPlateful_WS = false;
                    //}
                    Plc.Plateful_1 = buff;

                    //D19
                    buff = Convert.ToInt32(UseFul.Substring(76, 4), 16);
                    if (Plc.Plateful_2 == 0 && buff == 1)
                        Plc.bPlateful_2 = true;
                    Plc.Plateful_2 = buff;
                }
            }
        }

        //提取数据2：
        void SplitData2(string data)
        {
            if (data.StartsWith("8100"))
            {
                string UseFul = data.Substring(4);
                if (UseFul.Length == 64)
                {
                    int buff;
                    string sbuff1;
                    string sbuff2;
                    //D30 XY坐标交换
                    buff = Convert.ToInt32(UseFul.Substring(0, 4), 16);
                    Protocol.Result_XY_Opposite = buff;
                    //D31 分料模式開啟
                    buff = Convert.ToInt32(UseFul.Substring(4, 4), 16);
                    Protocol.Separator_Open = buff;
                    //D32 BarcodeReaderPlus當前第幾張照片
                    buff = Convert.ToInt32(UseFul.Substring(8, 4), 16);
                    Protocol.BarcodeReaderPlus_n = buff;
                    //D33 BarcodeReaderPlus總行數
                    buff = Convert.ToInt32(UseFul.Substring(12, 4), 16);
                    Protocol.BarcodeReaderPlus_MaxRow = buff;
                    //D34 BarcodeReaderPlus總列數
                    buff = Convert.ToInt32(UseFul.Substring(16, 4), 16);
                    Protocol.BarcodeReaderPlus_MaxColumn = buff;
                    //D35 BarcodeReaderPlus當前行數
                    buff = Convert.ToInt32(UseFul.Substring(20, 4), 16);
                    Protocol.BarcodeReaderPlus_NowRow = buff;
                    //D36 BarcodeReaderPlus當前列數
                    buff = Convert.ToInt32(UseFul.Substring(24, 4), 16);
                    Protocol.BarcodeReaderPlus_NowColumn = buff;
                    //D37 BarcodeReaderPlus是否開啟
                    buff = Convert.ToInt32(UseFul.Substring(28, 4), 16);
                    Protocol.BarcodeReaderPlus_Open = buff;
                    //D38 PLC狀態
                    buff = Convert.ToInt32(UseFul.Substring(32, 4), 16);
                    Protocol.PlcStatus = buff;
                    //D39 PLC報警1    
                    #region
                    //先擷取字詞串,在轉換為10進制,再轉換成2進制再補上填充位
                    string Elements = Convert.ToString(Convert.ToInt32(UseFul.Substring(36, 4), 16), 2).PadLeft(16, '0');
                    {
                        m_AlarmMessage.A0000 = (Elements.Substring(Elements.Length - 1, 1) == "1");
                        m_AlarmMessage.A0001 = (Elements.Substring(Elements.Length - 2, 1) == "1");
                        m_AlarmMessage.A0002 = (Elements.Substring(Elements.Length - 3, 1) == "1");
                        m_AlarmMessage.A0003 = (Elements.Substring(Elements.Length - 4, 1) == "1");
                        m_AlarmMessage.A0004 = (Elements.Substring(Elements.Length - 5, 1) == "1");
                        m_AlarmMessage.A0005 = (Elements.Substring(Elements.Length - 6, 1) == "1");
                        m_AlarmMessage.A0006 = (Elements.Substring(Elements.Length - 7, 1) == "1");
                        m_AlarmMessage.A0007 = (Elements.Substring(Elements.Length - 8, 1) == "1");
                        m_AlarmMessage.A0008 = (Elements.Substring(Elements.Length - 9, 1) == "1");
                        m_AlarmMessage.A0009 = (Elements.Substring(Elements.Length - 10, 1) == "1");
                        m_AlarmMessage.A0010 = (Elements.Substring(Elements.Length - 11, 1) == "1");
                        m_AlarmMessage.A0011 = (Elements.Substring(Elements.Length - 12, 1) == "1");
                        m_AlarmMessage.A0012 = (Elements.Substring(Elements.Length - 13, 1) == "1");
                        m_AlarmMessage.A0013 = (Elements.Substring(Elements.Length - 14, 1) == "1");
                        m_AlarmMessage.A0014 = (Elements.Substring(Elements.Length - 15, 1) == "1");
                        m_AlarmMessage.A0015 = (Elements.Substring(Elements.Length - 16, 1) == "1");
                    }
                    if (Protocol.PlcStatus != Protocol.PlcStatus_Befort)
                    {
                        Protocol.PlcStatus_Befort = Protocol.PlcStatus; 
                        m_AlarmMessage.AlarmMessageEn = "";
                        m_AlarmMessage.AlarmMessageCH = "";
                        Alarm = new bool[16]{m_AlarmMessage.A0000,
                                        m_AlarmMessage.A0001,
                                        m_AlarmMessage.A0002,
                                        m_AlarmMessage.A0003,
                                        m_AlarmMessage.A0004,
                                        m_AlarmMessage.A0005,
                                        m_AlarmMessage.A0006,
                                        m_AlarmMessage.A0007,
                                        m_AlarmMessage.A0008,
                                        m_AlarmMessage.A0009,
                                        m_AlarmMessage.A0010,
                                        m_AlarmMessage.A0011,
                                        m_AlarmMessage.A0012,
                                        m_AlarmMessage.A0013,
                                        m_AlarmMessage.A0014,
                                        m_AlarmMessage.A0015};
                        for (int i = 0; i < 16; i++)
                        {
                            if (Alarm[i])
                            {
                                m_AlarmMessage.AlarmMessageEn = m_AlarmMessage.AlarmMessageEn + "," + AlarmEN[i];
                                m_AlarmMessage.AlarmMessageCH = m_AlarmMessage.AlarmMessageCH + "," + AlarmCh[i];
                            }
                        } 
                        if (m_AlarmMessage.AlarmMessageEn.Length > 0)
                        {
                            m_AlarmMessage.AlarmMessageEn = m_AlarmMessage.AlarmMessageEn.Substring(1, m_AlarmMessage.AlarmMessageEn.Length - 1);
                            m_AlarmMessage.AlarmMessageCH = m_AlarmMessage.AlarmMessageCH.Substring(1, m_AlarmMessage.AlarmMessageCH.Length - 1);
                        }
                        switch(Protocol.PlcStatus)
                        {
                            case 1:
                                {
                                    Process.Start(@"C:\EquipmentState\EquipmentState.exe", Sys.MachineID + ",Run");
                                }
                                break;
                            case 2:
                                {
                                    Process.Start(@"C:\EquipmentState\EquipmentState.exe", Sys.MachineID + ",Down,Message="+m_AlarmMessage.AlarmMessageEn);
                                }
                                break;
                            case 3:
                                {
                                    Process.Start(@"C:\EquipmentState\EquipmentState.exe", Sys.MachineID + ",Idle");
                                }
                                break;
                            case 4:
                                {
                                    Process.Start(@"C:\EquipmentState\EquipmentState.exe", Sys.MachineID + ",Pause");
                                }
                                break;
                        }
                        WriteAlarmLog();
                    }
                #endregion
                    //D40 PLC報警2(預留)

                    //D41-42 HMI版本
                    sbuff1 = UseFul.Substring(44, 4);
                    sbuff2 = UseFul.Substring(48, 4);
                    buff = Convert.ToInt32(sbuff2 + sbuff1, 16);
                    Protocol.Versions_HMI = buff.ToString();
                    //D43-44 PLC版本
                    sbuff1 = UseFul.Substring(52, 4);
                    sbuff2 = UseFul.Substring(56, 4);
                    buff = Convert.ToInt32(sbuff2 + sbuff1, 16);
                    Protocol.Versions_PLC = buff.ToString();
                    //D45光源切換模式
                    buff = Convert.ToInt32(UseFul.Substring(60, 4), 16);
                    Protocol.Mode_Light = buff;
                }
            }
        }



        private void Reconnect_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(3000);
            while (!Reconnect.CancellationPending)
            {
                if (!Plc.IsConnected)
                {
                    Thread.Sleep(50);
                    Enet = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    Enet.ReceiveTimeout = 3000;
                    Enet.ReceiveTimeout = 3000;
                    try
                    {
                        Enet.Connect(Plc.ip, 8001);
                        Plc.IsConnected = true;
                    }
                    catch
                    {
                        Enet.Close();
                        Plc.IsConnected = false;
                    }
                }
                if (!CCD.IsConnected)
                {
                    if (CCD.CCDBrand == 1)
                    {
                        //myHikvision.OpenCamera();
                    }
                }
                Thread.Sleep(1);
            }
        }

        private void ScanTrigger_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(500);
            bool bReading = false;
            bool bBarcodeReading = false;
            while (true)
            {
                if (ScanTrigger.CancellationPending)
                {
                    return;
                }

                //軟體觸發
                if (Plc.TriggerMode != 2)
                {
                    {
                        if (Plc.Trigger != 1)
                            bReading = false;
                        if (Plc.Trigger == 1 && !bReading)
                        {
                            Protocol.Result_Trigger = 0;
                            Protocol.bTrigger = true;
                            bReading = true;
                            try
                            {
                                //觸發相機
                                My.dt = DateTime.Now;
                               
                                    Tray.n = (Tray.CurrentRow - 1) * Tray.Columns_1 + Tray.CurrentColumn - 1;
                               
                                OneShot();
                            }
                            catch
                            {
                            }
                        }
                    }
                    {
                        if (Plc.BarcodeTrigger != 1)
                            bBarcodeReading = false;
                        if (Plc.BarcodeTrigger == 1 && !bBarcodeReading)
                        {
                            Protocol.Result_BarcodeTrigger = 0;
                            Protocol.bBarcodeTrigger = true;
                            bBarcodeReading = true;
                            try
                            {
                                BarcodeInspect = true;
                                //觸發相機
                                My.dt = DateTime.Now;
                                OneShot();
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }

        private void FrmParent_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Stop1();
            Reconnect.CancelAsync();
            ScanTrigger.CancelAsync();
            ScanPlc.CancelAsync();
            if (CCD.CCDBrand == 0)
            {
                if (CCD.IsConnected)
                {
                    myBasler.DestroyCamera();
                }
                else
                {
                    myHikvision.DestroyCamera();
                }
            }
            
            IniFile.Write("System", "OKCount", Count.iOK.ToString(), Sys.SysPath);
            IniFile.Write("System", "NGCount", Count.iNG.ToString(), Sys.SysPath);
            IniFile.Write("System", "NG2Count", Count.iNG2.ToString(), Sys.SysPath);
            IniFile.Write("System", "NG3Count", Count.iNG3.ToString(), Sys.SysPath);
            IniFile.Write("System", "NG4Count", Count.iNG4.ToString(), Sys.SysPath);
            IniFile.Write("System", "NG5Count", Count.iNG5.ToString(), Sys.SysPath);
            IniFile.Write("System", "MissCount", Count.iMiss.ToString(), Sys.SysPath);
            IniFile.Write("System", "TestCount", Count.iTest.ToString(), Sys.SysPath);
            IniFile.Write("System", "TestTotalCount", Count.iTestTotal.ToString(), Sys.SysPath);
            quit = true;
        }

        private void lblUser_Click(object sender, EventArgs e)
        {
            lblUser.Text = User.CurrentUser= "";
        }

        private void BarcodeData_DoWork(object sender, DoWorkEventArgs e)
        {
            bool bReading = false;
            char[] c = new char[] { 'E', 'R', 'R', 'O', 'R' };
            while (!BarcodeData.CancellationPending)
            {
                Thread.Sleep(200);
                if (Plc.BarcodeTrigger==2 && !bReading)
                {
                    Reader.BarcodeRead = true;
                    
                    byte[] buffer = new byte[1024];
                    int len = Reader.Com.Read(buffer, 0, Reader.Com.BytesToRead);
                    if (len <= 0)
                    {
                        continue;
                    }
                    
                        string s = Encoding.ASCII.GetString(buffer, 0, len).Trim(c);
                        s = s.Trim();
                        if (s.Length > 16)
                        {
                            if (s.Length >= 17)
                            {
                                s = s.Substring(0, 17);
                            }
                            this.Run.lblTrayBarcode_1.Invoke(new MethodInvoker(delegate
                            {
                                Run.lblTrayBarcode_1.Text = s;
                            }));
                            if (Tray.Barcode_1!="")
                                Tray.OpDateTime = DateTime.Now;
                            Tray.Barcode_1 = s;
                            Thread.Sleep(20);
                            Protocol.Result_BarcodeTrigger = 0;
                            Protocol.bBarcodeTrigger = true;
                            //if (Sys.CallWebService)
                            //{
                            //    bool bResult = false;
                            //    WS_MatchLabel.MatchLabel matchLabel = new WS_MatchLabel.MatchLabel();
                            //    bResult = matchLabel.GetLensBarcoderMatch(s, Sys.Codes, Sys.MachineID,out string sMsg);
                            //    if(bResult)
                            //        Protocol.Result_BarcodeOK = 1;
                            //    else
                            //        Protocol.Result_BarcodeOK = 2;
                            //}
                            //else
                                Protocol.Result_BarcodeOK = 1;
                            Vision.BarcodeResult_1 = "";
                            if (Sys.VisitWebService)
                            {
                                if (Sys.Function == 5)
                                {
                                    if (Sys.Codes == "M")
                                    {
                                        try
                                        {
                                            CallWithTimeout(DataBase_WS_Start, 10000);
                                        }
                                        catch
                                        {
                                            //My.sResult = "N";
                                            Protocol.Result_BarcodeOK = 2;
                                            MessageBox.Show("訪問WebService超時10秒異常,請檢查網路連線,若網路無異常請聯絡資訊 葉奇彬*3641處理", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                        }
                                        if (!WebService.NIRResult_Start)
                                        {
                                            Protocol.Result_BarcodeOK = 2;
                                            MessageBox.Show(WebService.NIRMsg_Start, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                        }
                                    }
                                }
                            }
                            Protocol.bBarcodeOK = true;
                            Reader.BarcodeRead = false;
                        }
                        else
                        {
                            Tray.Barcode_1 = "Null";
                        }
                   
                    
                }
                if (!Reader.BarcodeRead)
                {
                    bReading = false;
                }
                Thread.Sleep(50);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            count = 0;
            lblCount.Text = count.ToString() ;
            count2 = 0;
            lblCount2.Text = count2.ToString();
        }
        public void Clear_Read_Temple()
        {
            if (Sys.Function == 1)
            {
                VisionSet.Clear_Temple();
                VisionSet.Read_Temple();
            }
            else if (Sys.Function == 5)
            {
                VDI_NIR.Clear_Temple();
                VDI_NIR.Read_Temple();
            }
        }

        //超時跳出設置
        static void CallWithTimeout(Action action, int timeoutMilliseconds)
        {
            Thread threadToKill = null;
            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                action();
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                throw new TimeoutException();
            }
        }

        void DataBase_WS_Start()
        {
            string TrayBarcode = "";
            string sMsg = "";
            
                TrayBarcode = Tray.Barcode_1;
           

            if (lblFactory.Text == "XM")
            {
                WS_Eqp_NIR_XM.Eqp_NIR Eqp_NIR_XM = new WS_Eqp_NIR_XM.Eqp_NIR();
                WebService.NIRResult_Start = Eqp_NIR_XM.CheckInputTray(TrayBarcode, Sys.Codes, Sys.MachineID, DateTime.Now, out WebService.NIRMsg_Start);
            }
            else if (lblFactory.Text == "JM")
            {
                WS_Eqp_NIR_JM.Eqp_NIR Eqp_NIR_JM = new WS_Eqp_NIR_JM.Eqp_NIR();
                WebService.NIRResult_Start = Eqp_NIR_JM.CheckInputTray(TrayBarcode, Sys.Codes, Sys.MachineID, DateTime.Now, out WebService.NIRMsg_Start);
            }
        }
       
        private void setTaskAtFixedTime()//計時器每日8,20點自動歸零檢驗數據並記錄LOG
        {
            DateTime now = DateTime.Now;
            DateTime eightOClock = DateTime.Today.AddHours(8.0); //8：00
            DateTime twentyOClock = DateTime.Today.AddHours(20); //20：00
            if (now > eightOClock)
            {
                eightOClock = eightOClock.AddDays(1.0);
            }
            if (now > twentyOClock)
            {
                twentyOClock = twentyOClock.AddDays(1.0);
            }
            int msUntilFour = 0;
            if (eightOClock - now < twentyOClock - now)
                msUntilFour = (int)((eightOClock - now).TotalMilliseconds);
            else
                msUntilFour = (int)((twentyOClock - now).TotalMilliseconds);
            var t = new System.Threading.Timer(doAt);
            t.Change(msUntilFour, Timeout.Infinite);
        }
        private void doAt(object state)
        {
            //执行功能...
            WriteCountLog();
            //再次设定
            setTaskAtFixedTime();
        }
        public void WriteCountLog()
        {
            string Path = Sys.ConditionPath;
            try
            {
                //不存在文件夹，创建先
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }

                string Log = Path + "\\" + Sys.MachineID + "_ConditionLog.txt";

                //不存在文件，创建先

                if (!File.Exists(Log))
                {
                    File.WriteAllText(Log, "Function\tCurProduction\tMachineID\tTime\tTotalCount\tOKCount\tNGCount\tNG2Count\tNG3Count\tNG4Count\tNG5Count\tOKRatio\tNGRatio\tNG2Ratio\tNG3Ratio\tNG4Ratio\tNG5Ratio" +
                                        "\r\n");
                }
                //写result
                using (StreamWriter sw = new StreamWriter(Log, true))
                {
                    sw.WriteLine(Sys.FunctionString + "\t" +
                        Production.CurProduction + "\t" +
                        Sys.MachineID + "\t" +
                        DateTime.Now.ToString("yyyyMMdd_HH:mm:ss") + "\t" +
                        Count.iTotal + "\t" +
                        Count.iOK + "\t" +
                        Count.iNG + "\t" +
                        Count.iNG2 + "\t" +
                        Count.iNG3 + "\t" +
                        Count.iNG4 + "\t" +
                        Count.iNG5 + "\t" +
                        Count.dOKRatio.ToString() + "%" + "\t" +
                        Count.dNGRatio.ToString() + "%" + "\t" +
                        Count.dNG2Ratio.ToString() + "%" + "\t" +
                        Count.dNG3Ratio.ToString() + "%" + "\t" +
                        Count.dNG4Ratio.ToString() + "%" + "\t" +
                        Count.dNG5Ratio.ToString() + "%");
                }
            }
            catch
            {
            }
        }


        HTuple m_hRowB, m_hColB, m_hButton, m_hRowE, m_hColE;
        public void RePaint(HTuple hWindow)
        {
            HOperatorSet.ClearWindow(hWindow);
            HOperatorSet.DispObj(My.ho_Image, hWindow);
        }

        public void hWindowControl1_HMouseDown(HTuple hWindow)
        {
            try
            {
                HOperatorSet.SetCheck("~give_error");
                HOperatorSet.GetMposition(hWindow, out m_hRowB, out m_hColB, out m_hButton);
            }
            catch
            {
            }
        }

        public void hWindowControl1_HMouseUp(HTuple hWindow)
        {
            try
            {
                HOperatorSet.SetCheck("~give_error");
                HOperatorSet.GetMposition(hWindow, out m_hRowE, out m_hColE, out m_hButton);

                HTuple row1, col1, row2, col2;
                double dbRowMove, dbColMove;
                double dbRowB, dbColB, dbRowE, dbColE;
                dbRowB = m_hRowB;
                dbRowE = m_hRowE;
                dbColB = m_hColB;
                dbColE = m_hColE;
                dbRowMove = -dbRowE + dbRowB;
                dbColMove = -dbColE + dbColB;

                HOperatorSet.GetPart(hWindow, out row1, out col1, out row2, out col2);
                HOperatorSet.SetPart(hWindow, row1 + dbRowMove, col1 + dbColMove, row2 + dbRowMove, col2 + dbColMove);

                RePaint(hWindow);
            }
            catch
            {

            }
        }

        public void hWindowControl1_HMouseWheel(HTuple hWindow, HMouseEventArgs e)
        {
            try
            {
                double x = e.X;
                double y = e.Y;
                double Zoom = 1.0;
                double ZoomTrans = 1.0, ZoomOrg = 1.0, RowShif, ColShif;
                HTuple Row0, Column0, Row00, Column00, Ht, Wt, r1, c1, r2, c2;
                HTuple Row, Col, Button;

                HOperatorSet.SetCheck("~give_error");
                HOperatorSet.GetMposition(hWindow, out Row, out Col, out Button);
                HOperatorSet.GetPart(hWindow, out Row0, out Column0, out Row00, out Column00);

                if (e.Delta >= 0)
                { 
                    if (0 != Zoom && Zoom >= 32.0)
                    {
                        Zoom = 32.0;
                    }
                    else
                    {
                        Zoom = 1.0 + Zoom;
                    }
                }
                else
                {
                    if (0 != Zoom && Zoom <= (1.0 / 16))
                    {
                        Zoom = 1.0 / 16;
                    }
                    else
                    {
                        Zoom = Zoom / 2;
                    }
                }
                ZoomTrans = Zoom / ZoomOrg;
                ZoomOrg = Zoom;
                RowShif = 0;
                ColShif = 0;
                Ht = Row00 - Row0;
                Wt = Column00 - Column0;
                r1 = (Row0 + ((1 - (1.0 / ZoomTrans)) * (Row - Row0))) - (RowShif / ZoomTrans);
                c1 = (Column0 + ((1 - (1.0 / ZoomTrans)) * (Col - Column0))) - (ColShif / ZoomTrans);
                r2 = r1 + (Ht / ZoomTrans);
                c2 = c1 + (Wt / ZoomTrans);

                HOperatorSet.SetPart(hWindow, r1, c1, r2, c2);

                RePaint(hWindow);
            }
            catch
            {

            }
        }

        public void ShowOriginalImage(HObject ho_Image,HTuple hWindow)
        {
            HTuple hv_Height = null, hv_Width = null;
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.SetPart(hWindow, 0, 0, hv_Height, hv_Width);
            HOperatorSet.DispObj(ho_Image, hWindow);
        }



        //实现方法
    /// <summary>
    /// 无损压缩图片
    /// </summary>
    /// <param name="sFile">原图片</param>
    /// <param name="dFile">压缩后保存位置</param>
    /// <param name="dHeight">高度</param>
    /// <param name="dWidth">宽度</param>
    /// <param name="flag">压缩质量 1-100</param>
    /// <returns></returns>

        public bool GetPicThumbnail(Image image, string dFile, int dHeight, int dWidth, int flag)
    {
        ImageFormat tFormat = image.RawFormat;
        int sW = 0, sH = 0;
        //按比例缩放
        Size tem_size = new Size(image.Width, image.Height);
        if (tem_size.Width > dHeight || tem_size.Width > dWidth) 
        {
            if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
            {
                sW = dWidth;
                sH = (dWidth * tem_size.Height) / tem_size.Width;
            }
            else
            {
                sH = dHeight;
                sW = (tem_size.Width * dHeight) / tem_size.Height;
            }
        }
        else
        {
            sW = tem_size.Width;
            sH = tem_size.Height;
        }
 
        Bitmap ob = new Bitmap(dWidth, dHeight);
        Graphics g = Graphics.FromImage(ob);
        g.Clear(Color.WhiteSmoke);
        g.CompositingQuality = CompositingQuality.HighQuality;
        g.SmoothingMode = SmoothingMode.HighQuality;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.DrawImage(image, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
        g.Dispose();
        //以下代码为保存图片时，设置压缩质量
        EncoderParameters ep = new EncoderParameters();
        long[] qy = new long[1];
        qy[0] = flag;//设置压缩的比例1-100
        EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
        ep.Param[0] = eParam;
        try
        {
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
 
            ImageCodecInfo jpegICIinfo = null;
 
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICIinfo = arrayICI[x];
                    break;
                }
            }
            if (jpegICIinfo != null)
            {
                ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
            }
            else
            {
                ob.Save(dFile, tFormat);
            }
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            image.Dispose();
            ob.Dispose();
 
        }
    }
      
        class ThreadInfo
        {
            public HObject image { get; set; }
            public int n { get; set; }
            public int CurrentColumn { get; set; }
            public int CurrentRow { get; set; }
            public int NowTray { get; set; }
            public int CurrentColumn2 { get; set; }
            public int CurrentRow2 { get; set; }
            public int Status { get; set; }
        }

        public void processHImage(HObject image )
        {
            int n = 0;
            int CurrentRow = 0, CurrentColumn = 0,CurrentRow2 = 0, CurrentColumn2 = 0;
            int NowTray = 1;
            CurrentRow = Tray.CurrentRow;
            CurrentColumn = Tray.CurrentColumn;
            CurrentRow2 = Protocol.BarcodeReaderPlus_NowRow;
            CurrentColumn2 = Protocol.BarcodeReaderPlus_NowColumn;
            int Status = Plc.Status;
            Plc.VisualComplete = false;
            if (Plc.TriggerMode == 2)
            {
                    //Tray.n = (Tray.CurrentRow - 1) * Tray.Columns_1 + Tray.CurrentColumn - 1;
                    if (Plc.LDirection == 1)
                        Tray.n = (Tray.CurrentRow - 1) * Tray.Columns_1 + Tray.CurrentColumn - 1;     //(r-1) * C + (c-1) 
                    if (Plc.LDirection == 2)
                        Tray.n = ((Tray.CurrentRow - 1) * Tray.Columns_1) + (Tray.Columns_1 - Tray.CurrentColumn);  //(r-1) * C + (C-c)
                    if (Plc.LDirection == 4)
                        Tray.n = ((Tray.Rows_1 - Tray.CurrentRow) * Tray.Columns_1) + (Tray.Columns_1 - Tray.CurrentColumn); //(R-r) * C + (C-c)
                    if (Plc.LDirection == 3)
                        Tray.n = ((Tray.Rows_1 - Tray.CurrentRow) * Tray.Columns_1) + Tray.CurrentColumn - 1; //(R-r) * C + (c-1)         
                    if (Sys.Function == 9)
                    {
                        n = Protocol.BarcodeReaderPlus_n;
                    }
                    else
                    {
                        n = Tray.n;
                    }
                Protocol.Result_Vision = 1;
                Protocol.bVisionOK = true;
            }
            else
            {
                n = Tray.n;
            }
            HOperatorSet.SetSystem("global_mem_cache", "idle");
            //圖片旋轉270度
            HObject ExpTmpOutVar_0;
            HOperatorSet.RotateImage(image, out ExpTmpOutVar_0, 270, "constant");
            image.Dispose();
            image = ExpTmpOutVar_0;
            switch (Sys.Function)
            {
                case 12:
                    {
                        if (!My.bCurrention)
                        {
                            //圖片旋轉270度
                            HOperatorSet.RotateImage(image, out ExpTmpOutVar_0, m_LensCarry.m_Correction.Angle.TupleDeg(), "constant");
                            image.Dispose();
                            image = ExpTmpOutVar_0;
                        }
                    }break;
            }

            
            ThreadInfo threadInfo = new ThreadInfo();
            threadInfo.image = image.CopyObj(1, -1);
            threadInfo.CurrentColumn = CurrentColumn;
            threadInfo.CurrentRow = CurrentRow;
            threadInfo.n = n;
            threadInfo.NowTray = NowTray;
            threadInfo.CurrentRow2 = CurrentRow2;
            threadInfo.CurrentColumn2 = CurrentColumn2;
            threadInfo.Status = Status;
            ThreadPool.QueueUserWorkItem(new WaitCallback(MyImageProcoss), threadInfo);

        }

        public void MyImageProcoss(Object a)
        {
            ThreadInfo threadInfo = new ThreadInfo();
            threadInfo = a as ThreadInfo;
            int n = threadInfo.n;
            int CurrentRow = threadInfo.CurrentRow;
            int CurrentColumn = threadInfo.CurrentColumn;
            int NowTray = threadInfo.NowTray;
            int CurrentRow2 =threadInfo.CurrentRow2;
            int CurrentColumn2 =threadInfo.CurrentColumn2;
            int Status = threadInfo.Status;
            HObject theImage = null;
            HOperatorSet.GenEmptyObj(out theImage);
            theImage = threadInfo.image.CopyObj(1, -1);
            Vision.VisionBarcodeRotate[n] = 0;
            DateTime dt = DateTime.Now;
            HTuple width, height, width2, height2;
            
            DateTime beferDT = System.DateTime.Now;
            //不在自動模式,直接顯示圖片在視窗
            if(!On_Auto_InterFace)
            {
                switch (Sys.FunctionString)
                {
                    case "VDI": HWindow = VDIVisionSet.hWindowControl1.HalconWindow; break;
                    case "Dispensing": HWindow = VisionSet.hWindowControl1.HalconWindow; break;
                    case "SOMA": HWindow = SomaVisionSet.hWindowControl1.HalconWindow; break;
                    case "SOMA Defect": HWindow = SomaDetectionVS.hWindowControl1.HalconWindow; break;
                    case "VDI Coating": HWindow = VDICoatingVS.hWindowControl1.HalconWindow; break;
                    case "NIR": HWindow = VDI_NIR.hWindowControl1.HalconWindow; break;
                    case "Inkiness": HWindow = VDI_INK.hWindowControl1.HalconWindow; break;
                    case "Classifier": HWindow = Classifier.hWindowControl1.HalconWindow; break;
                    case "BarcodeReader": HWindow = BR.hWindowControl1.HalconWindow; break;
                    case "BarcodeReaderPlus": HWindow = BRPlus.hWindowControl1.HalconWindow; break;
                    case "LensCrack_AVI": HWindow = LensCrack_AVI.hWindowControl1.HalconWindow; break;
                    case "Lens_Mold_Cave": HWindow = Lens_Mold_Cave.hWindowControl1.getHWindowControl().HalconWindow; break;
                    case "LensCarry": HWindow = LensCarry.hWindowControl1.HalconWindow; break;
                }
                //8958模穴识别非自动界面Halcon窗体与原先不一样，分开写
                if(Sys.FunctionString=="Lens_Mold_Cave")
                {
                     //if (My.Myho_Image != null)
                     //   My.Myho_Image.Dispose();
                     HOperatorSet.CopyImage(theImage, out My.ho_Image);                 
                     Lens_Mold_Cave.hWindowControl1.HobjectToHimage(My.ho_Image);                   
                     if (My.ContinueShot)
                     {
                         theImage.Dispose();
                     }
                     Thread.Sleep(10);

                }
                else 
                  {
                    HOperatorSet.GetImageSize(theImage, out width, out height);
                    HOperatorSet.SetPart(HWindow, 0, 0, height, width);
                    HOperatorSet.DispObj(theImage, HWindow);
                    if (My.ho_Image != null)
                        My.ho_Image.Dispose();
                    HOperatorSet.CopyImage(theImage, out My.ho_Image);
                    if (My.ContinueShot)
                    {
                        theImage.Dispose();
                    }
                    if (ShowCross)
                    {
                        HOperatorSet.SetColor(HWindow, "red");
                        width2 = width / 2;
                        height2 = height / 2;
                        HOperatorSet.DispLine(HWindow, height2, 0, height2, width);
                        HOperatorSet.DispLine(HWindow, 0, width2, height, width2);
                    }
                }
            }
            else //自動模式要判斷當前是哪個功能,做相應的圖像處理
            {
                HWindow = Run.hWindowControl1.HalconWindow;
                HOperatorSet.GetImageSize(theImage, out width, out height);
                HOperatorSet.SetPart(HWindow, 0, 0, height, width);
                My.dResultRow = 0;
                My.dResultColumn = 0;
                switch (Sys.Function)
                {
                    case 0:
                        {
                            VDIVisionSet.ImageProPlus(HWindow, theImage, n, CurrentRow, CurrentColumn);
                            break;
                        }
                    case 1:
                        {
                            VisionSet.ImageProPlus(HWindow, theImage, n);
                            break;
                        }
                    case 2:
                        {
                            SomaVisionSet.ImageProPlus(HWindow);
                            break;
                        }
                    case 3:
                        {
                            SomaDetectionVS.ImageProPlus(HWindow);
                            break;
                        }
                    case 4:
                        {
                            VDICoatingVS.ImageProPlus(HWindow, theImage, n);
                            break;
                        }
                    case 5:
                        {
                            VDI_NIR.ImageProPlus(HWindow, theImage, n, Status);
                            break;
                        }
                    case 6:
                        {
                            VDI_INK.ImageProPlus(HWindow, theImage, n);
                            break;
                        }
                    case 7:
                        {
                            Classifier.ImageProPlus(HWindow, theImage, n);
                            break;
                        }
                    case 8:
                        {
                            BR.ImageProPlus(HWindow, theImage, n);
                            break;
                        }
                    case 9:
                        {
                            BRPlus.ImageProPlus(HWindow, theImage, n, CurrentRow2, CurrentColumn2);
                            break;
                        }
                    case 10:
                        {
                            HObject ho_ResultImage = new HObject();
                            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
                            string sResult = "";

                            HWindowControl _HWindow = new HWindowControl();
                            _HWindow.Width = 1000;
                            _HWindow.Height = 1000;
                            ho_ResultImage.Dispose();
                            LensCrack_AVI.ImageProcoss(_HWindow.HalconWindow, theImage,out ho_ResultImage, out sResult);
                            _HWindow.Dispose();
                            HOperatorSet.GetImageSize(ho_ResultImage, out hv_Width, out hv_Height);
                            HOperatorSet.SetPart(HWindow, 0, 0, hv_Height - 1, hv_Width - 1);
                            ho_ResultImage.DispObj(HWindow);


                            break;
                        }
                    case 11:
                         {
                             HObject ho_ResultImage = new HObject();
                           
                             string sResult = "";
                            
                             Mold_CaveProcoss(HWindow, theImage, Tray.NowTray, out ho_ResultImage, out sResult);
                            
                             if (Tray.NowTray == 1)
                               {
                                Vision.Images_1[n] = ho_ResultImage;
                                Vision.Images_Now[n] = Vision.Images_1[n];
                                Vision.ImagesOriginal_1[n] = theImage;
                            }
                            else if (Tray.NowTray == 2)
                            {
                                Vision.Images_2[n] = ho_ResultImage;
                                Vision.Images_Now[n] = Vision.Images_2[n];
                                Vision.ImagesOriginal_2[n] = theImage;
                            }
                            Vision.VisionResult[n] = sResult;
                         }
                        break;
                    case 12:
                        {
                            LensCarry.ImageProPlus(HWindow, theImage, n);
                        }
                        break;
                }
                #region 整盤解碼
                if (Sys.Function == 9)
                {
                    bool bProcessComplete = false;
                    if (Protocol.BarcodeReaderPlus_MaxRow == CurrentRow2)
                    {
                        if (Protocol.BarcodeReaderPlus_MaxRow % 2 != 0)
                        {
                            if (CurrentColumn2 == Protocol.BarcodeReaderPlus_MaxColumn)
                            {
                                bProcessComplete = true;
                            }
                        }
                        else
                        {
                            if (CurrentColumn2 == 1)
                            {
                                bProcessComplete = true;
                            }
                        }
                    }
                    if (bProcessComplete)
                    {
                        for (int i = 0; i < Vision.VisionResult.Count; i++)
                        {
                            try
                            {
                                WriteLog(i, Vision.VisionResult[i], Vision.VisionBarcodeResult[i]);
                            }
                            catch
                            {
                            }
                        }
                        Plc.VisualComplete = true;
                    }
                    string LensBarcode = "NA";
                    string TrayBarcode = "NA";
                    if (Sys.OptionOriginal)
                    {
                        string Originalpath = Sys.ImageSavePath0 + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Original";
                        //檔名
                        string Namepath = TrayBarcode + "_(" + string.Format("{0}.{1}", CurrentRow, CurrentColumn) + ")_" + LensBarcode;
                        //時間
                        string Time = DateTime.Now.ToString("yyyyMMdd_HH_mm_ss_f");
                        //建立原始圖片資料夾
                        if (!Directory.Exists(Originalpath))
                        {
                            Directory.CreateDirectory(Originalpath);
                        }
                        HOperatorSet.WriteImage(theImage, "bmp", 0, Originalpath + "\\" + Namepath + "_" + "OK" + "_" + Time);
                    }
                }
                #endregion
                else
                {
                    TimeSpan ts = DateTime.Now - dt;

                    this.Invoke(new MethodInvoker(delegate
                    {
                        lblCount.Text = count.ToString();
                        count = count + 1;
                        Run.lblTime.Text = Math.Round(ts.TotalMilliseconds, 0).ToString() + "ms";
                    }));
                    //結果圖/原圖臨時存放區
                    string Resultpath = Sys.ImageSavePath0 + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Result";
                    string Originalpath = Sys.ImageSavePath0 + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Original";
                    //結果圖上傳存放區
                    string UpLoadpath = Sys.ImageSavePath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Result";
                    string TrayBarcode = "NA";

                    if (Tray.Barcode_1 != "")
                        TrayBarcode = Tray.Barcode_1;

                    string LensBarcode = "NA";
                    if (Sys.ReadBarcodeLog)
                        LensBarcode = DataBank.ResultBarcode[CurrentRow - 1, CurrentColumn - 1].ToString();
                    if (My.NIR.ReadBarrelBarcode && !Sys.ReadBarcodeLog || Sys.Function == 8)
                    {
                        if (Vision.VisionBarcodeResult[n].Barcode == null)
                            Vision.VisionBarcodeResult[n].Barcode = "NA";
                        LensBarcode = Vision.VisionBarcodeResult[n].Barcode;
                    }
                    //檔名
                    string Namepath = TrayBarcode + "_(" + string.Format("{0}.{1}", CurrentRow, CurrentColumn) + ")_" + LensBarcode;
                    //時間
                    string Time = DateTime.Now.ToString("yyyyMMdd_HH_mm_ss_f");
                    //建立結果圖片資料夾
                    if (!Directory.Exists(Resultpath))
                    {
                        Directory.CreateDirectory(Resultpath);
                    }
                    //建立原始圖片資料夾
                    if (!Directory.Exists(Originalpath))
                    {
                        Directory.CreateDirectory(Originalpath);
                    }
                    //建立上傳圖片資料夾
                    if (!Directory.Exists(UpLoadpath))
                    {
                        Directory.CreateDirectory(UpLoadpath);
                    }
                    this.Invoke(new MethodInvoker(delegate
                    {
                        if (Sys.Function!=9)
                        {

                            switch (Vision.VisionResult[n])
                            {
                                case "OK":
                                    {
                                        Run.Labels_1[n].BackColor = Color.Green;
                                        if (Sys.OptionOK)
                                        {
                                            HOperatorSet.WriteImage(Vision.Images_1[n], "png", 0, Resultpath + "\\" + Namepath + "_OK_" + Time);
                                            bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_OK_" + Time + ".png", UpLoadpath + "\\" + Namepath + "_OK_" + Time + ".png", 360, 270, 100);
                                        }
                                        Count.iOK = Count.iOK + 1;
                                        break;
                                    }
                                case "NG":
                                    {
                                        Run.Labels_1[n].BackColor = Color.Red;
                                        if (Sys.OptionNG)
                                        {
                                            HOperatorSet.WriteImage(Vision.Images_1[n], "png", 0, Resultpath + "\\" + Namepath + "_NG_" + Time);
                                            bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_NG_" + Time + ".png", UpLoadpath + "\\" + Namepath + "_NG_" + Time + ".png", 360, 270, 100);
                                        }
                                        Count.iNG = Count.iNG + 1;
                                        break;
                                    }
                                case "NG2":
                                    {
                                        Run.Labels_1[n].BackColor = Color.Orange;
                                        if (Sys.OptionNG)
                                        {
                                            HOperatorSet.WriteImage(Vision.Images_1[n], "png", 0, Resultpath + "\\" + Namepath + "_NG2_" + Time);
                                            bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_NG2_" + Time + ".png", UpLoadpath + "\\" + Namepath + "_NG2_" + Time + ".png", 360, 270, 100);
                                        }
                                        Count.iNG2 = Count.iNG2 + 1;
                                        break;
                                    }
                                case "NG3":
                                    {
                                        Run.Labels_1[n].BackColor = Color.Purple;
                                        if (Sys.OptionNG)
                                        {

                                            HOperatorSet.WriteImage(Vision.Images_1[n], "png", 0, Resultpath + "\\" + Namepath + "_NG3_" + Time);
                                            bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_NG3_" + Time + ".png", UpLoadpath + "\\" + Namepath + "_NG3_" + Time + ".png", 360, 270, 100);
                                        }
                                        Count.iNG3 = Count.iNG3 + 1;
                                        break;
                                    }
                                case "NG4":
                                    {
                                        Run.Labels_1[n].BackColor = Color.White;
                                        Run.Labels_1[n].ForeColor = Color.Black;
                                        if (Sys.OptionNG)
                                        {

                                            HOperatorSet.WriteImage(Vision.Images_1[n], "png", 0, Resultpath + "\\" + Namepath + "_NG4_" + Time);
                                            bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_NG4_" + Time + ".png", UpLoadpath + "\\" + Namepath + "_NG4_" + Time + ".png", 360, 270, 100);
                                        }
                                        Count.iNG4 = Count.iNG4 + 1;
                                        break;
                                    }
                                case "NG5":
                                    {
                                        Run.Labels_1[n].BackColor = Color.Pink;
                                        if (Sys.OptionNG)
                                        {

                                            HOperatorSet.WriteImage(Vision.Images_1[n], "png", 0, Resultpath + "\\" + Namepath + "_NG5_" + Time);
                                            bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_NG5_" + Time + ".png", UpLoadpath + "\\" + Namepath + "_NG5_" + Time + ".png", 360, 270, 100);
                                        }
                                        Count.iNG5 = Count.iNG5 + 1;
                                        break;
                                    }
                                case "Miss":
                                    {
                                        Run.Labels_1[n].BackColor = Color.Blue;
                                        if (Sys.OptionNG)
                                        {
                                            HOperatorSet.WriteImage(Vision.Images_1[n], "png", 0, Resultpath + "\\" + Namepath + "_Miss_" + Time);
                                            bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_Miss_" + Time + ".png", UpLoadpath + "\\" + Namepath + "_Miss_" + Time + ".png", 360, 270, 100);
                                        }
                                        Count.iMiss = Count.iMiss + 1;
                                        break;
                                    }
                                default:
                                    {
                                        Run.Labels_1[n].BackColor = Color.Cyan;
                                        if (Sys.OptionNG)
                                        {
                                            HOperatorSet.WriteImage(Vision.Images_1[n], "png", 0, Resultpath + "\\" + Namepath + "_Other_" + Time);
                                            bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_Other_" + Time + ".png", UpLoadpath + "\\" + Namepath + "_Miss_" + Time + ".png", 360, 270, 100);
                                        }
                                        Count.iMiss = Count.iMiss + 1;
                                        break;
                                    }
                            }
                            if (Test._Test)
                            {
                                if (Vision.VisionResult[n] != "Miss")
                                    Count.iTestTotal = Count.iTestTotal + 1;
                                if (Test.Target[n])
                                {
                                    Run.Labels_1[n].BackColor = Color.Cyan;
                                    if (Sys.OptionNG)
                                    {
                                        HOperatorSet.WriteImage(Vision.Images_1[n], "png", 0, Resultpath + "\\" + Namepath + "_Test_" + Time);
                                    }
                                    Count.iTest = Count.iTest + 1;
                                }
                                else
                                {

                                }
                            }
                            if (Sys.OptionOriginal)
                            {
                                HOperatorSet.WriteImage(Vision.ImagesOriginal_1[n], "bmp", 0, Originalpath + "\\" + Namepath + "_" + Vision.VisionResult[n] + "_" + Time);
                            }
                            Run.Labels_1[n].Font = new Font("宋体", 7, FontStyle.Bold);
                            Run.Labels_1[n].Text = CurrentRow.ToString() + "-" + CurrentColumn.ToString();

                            theImage.Dispose();
                            if (NowTray == 1)
                            {
                                Vision.ImagesOriginal_1[n].Dispose();
                            }
                            else
                            {
                                Vision.ImagesOriginal_2[n].Dispose();
                            }
                            count2 = count2 + 1;
                            lblCount2.Text = count2.ToString();
                            if (Sys.Function == 5)
                                WebService.ArrResult[CurrentRow - 1][CurrentColumn - 1] = Vision.VisionResult[n];
                        }
                    }));
                    Plc.VisualComplete = true;
                }
            }
            if (Plc.TriggerMode != 2)
            {
                Protocol.Result_Vision = 1;
                Protocol.bVisionOK = true;
            }
            #region 掃碼
            if (BarcodeInspect)
            {
                BarcodeInspect = false;
                Run.ReadBarcode(Run.hWindowControl1.HalconWindow);
               
                    if (Vision.BarcodeResult_1 == "OK")
                    {
                        Protocol.Result_BarcodeOK = 1;
                        Vision.BarcodeResult_1 = "";
                    }
                    else if (Vision.BarcodeResult_1 == "NG")
                    {
                        Protocol.Result_BarcodeOK = 2;
                        Vision.BarcodeResult_1 = "";
                    }
                    else//Miss
                    {
                        Protocol.Result_BarcodeOK = 2;
                        Vision.BarcodeResult_1 = "";
                    }
               
                Protocol.bBarcodeOK = true;
            }
            
            
            #endregion
        }
        public void LabelShow(int n,string text,Color backcolor,Color forecolor)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                Run.Labels_1[n].BackColor = backcolor;
                if (backcolor != Color.Gray)
                {
                    Run.Labels_1[n].Font = new Font("宋体", 7, FontStyle.Bold);
                    Run.Labels_1[n].ForeColor = forecolor;
                    Run.Labels_1[n].Text = text;
                }
            }));
        }

        public void WriteLog(int n, string ResultOK, Vision.BarcodeResult BarcodeResult)
        {
            string Path = Sys.LogPath + "\\" + Tray.OpDateTime.ToString("yyyyMMdd");
            try
            {
                //不存在文件夹，创建先
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                Vision.FolderName = Sys.LogPath + "\\" + Tray.OpDateTime.ToString("yyyyMMdd") + "\\";
                Vision.FileName = Production.CurProduction + "-" + Sys.MachineID +
                                    Tray.OpDateTime.ToString("-yyyyMMdd_HH_mm_ss_") +
                                    Sys.Codes + "-" +
                                    Tray.Barcode_1 + "-" + Tray.Barcode_2 + ".txt";
                string Log = Path + "\\" + "Done_"+Vision.FileName;
                string Barcode = "";
                    
                    Barcode = Tray.Barcode_1;
                    
                int CurrentRow = 0;
                int CurrentColumn = 0;
                //反推行列
                    
                    CurrentRow = (int)Math.Floor((double)(n / Tray.Columns_1)) + 1;//Math.Floor無條件捨去
                    CurrentColumn = n % Tray.Columns_1 + 1;
                    


                //不存在文件，创建先

                if (!File.Exists(Log))
                {
                    File.WriteAllText(Log, "Function\tCode\tTray Barcode_B\tTray No.\tToTal Count\tTray Barcode_A\tProgram ID\tProduct\tClass" +
                        "\tOperatorID\tMachine No.\tTime\tCT\tResult\tLensBarcode\tBarcodeAngle\tDecodedMirrored\tMeanLight\tOverall_Quality\tCell_Contrast\t"+
                    "Print_Growth\tUnused_Error_Correction\tPrint_Growth\tUnused_Error_Correction\tCell_Modulation\tFixed_Pattern_Damage\tGrid_Nonuniformity\tDecode" +
                                        "\r\n");
                }

                //写result
                using (StreamWriter sw = new StreamWriter(Log, true))
                {
                    sw.WriteLine(Sys.FunctionString + "\t" +
                        Sys.Codes + "\t" +
                        Barcode + "\t" +
                        string.Format("{0}.{1}", CurrentRow, CurrentColumn) + "\t" +
                                    string.Format("{0}", Count.iOK + Count.iNG + Count.iNG2) + "\t" +
                                    "\t" +//空下Tray A版Barcode
                                    Production.CurProduction + "\t" +
                                    Sys.Type + "\t" +
                                    Tray.Class + "\t" +
                                    Tray.OperatorID + "\t" +
                                    Sys.MachineID + "\t" +
                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" +
                                    Protocol.Result_Cycle + "\t" +
                                    ResultOK + "\t" +
                                BarcodeResult.Barcode + "\t" +
                                BarcodeResult.BarcodeAngle.ToString() + "\t" +
                                BarcodeResult.hv_DecodedMirrored.ToString() + "\t" +
                                Math.Round(BarcodeResult.MeanLight,2).ToString() + "\t" +
                               BarcodeResult.Overall_Quality[0].ToString() + "\t" +
                               BarcodeResult.Cell_Contrast[0].ToString() + "\t" +
                               BarcodeResult.Print_Growth[0].ToString() + "\t" +
                               BarcodeResult.Unused_Error_Correction[0].ToString() + "\t" +
                               BarcodeResult.Cell_Modulation[0].ToString() + "\t" +
                               BarcodeResult.Fixed_Pattern_Damage[0].ToString() + "\t" +
                               BarcodeResult.Grid_Nonuniformity[0].ToString() + "\t" +
                               BarcodeResult.Decode[0].ToString());
                }
            }
            catch
            {
            }
        }

        public void WriteAlarmLog()
        {
            string Status = "";
            switch (Protocol.PlcStatus)
            {
                case 1: Status = "Run"; break;
                case 2: Status = "Down"; break;
                case 3: Status = "Idle"; break;
                case 4: Status = "Pause"; break;
            }
            string Path = Sys.AlarmPath;
            try
            {
                //不存在文件夹，创建先
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                string Log = Path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt"; ;
                //不存在文件，创建先
                if (!File.Exists(Log))
                {
                    File.WriteAllText(Log, "Time\tFactory\tMachineNO\tProductName\tMachineState\tAlarmMessage\r\n");
                }
                using (StreamWriter sw = new StreamWriter(Log, true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" +
                        lblFactory.Text + "\t" +
                        lblMachineID.Text + "\t" +
                        Production.CurProduction + "\t" +
                        Status + "\t" +
                        m_AlarmMessage.AlarmMessageCH);
                    sw.Close();
                }
            }
            catch
            {
            }
        }
        public void Mold_CaveProcoss(HWindow Window, HObject TheImage,int n ,out HObject ho_ResultImage, out string sResult)
        {
            HObject ho_Image = null;
            HObject ho_Circle = null;
            HObject ho_ReducedImage = null;
            HObject ho_ImageMedian = null;
            HObject ho_ImageEmphasize = null;
            HObject ho_Regions = null;
            HObject ho_RegionFillUp = null;
            HObject ho_ConnectedRegions = null;
            HObject ho_SelectedRegions = null;
            HObject ho_RegionTrans = null;
            HObject ho_InitialReectangle2 = null;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_ReducedImage);
            HOperatorSet.GenEmptyObj(out ho_ImageMedian);
            HOperatorSet.GenEmptyObj(out ho_ImageEmphasize);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            HOperatorSet.GenEmptyObj(out ho_InitialReectangle2);
            HObject ho_Contour = null;
            HObject ho_UsedEdges = null;
            HObject ho_ResultContours = null;
            HObject ho_CrossCenter = null;
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HObject ho_Rectangle = null;
            HObject ho_Rectangle1 = null;
            HObject ho_RegionUnion = null;
            HObject ho_ImageReduced = null;
            HObject ho_Regions1 = null;
            HObject ho_ConnectedRegions1 = null;
            HObject ho_SelectedRegions1 = null;
            HObject ho_ImageReduced1 = null;
            HObject ho_RegionAffineTrans1 = null;
            HObject ho_ImageAffineTrans1 = null;
            HObject ho_Regions2 = null;
            HObject ho_SelectedRegions2 = null;
            HObject ho_ConnectedRegions2 = null;
            HObject ho_SortRgn1 = null;
            HObject ho_SortRgn2 = null;
            HObject ho_ImgBinSrc = null;
            HObject ho_ImgBinSrc1 = null;
            HObject ho_Seg = null;
            HObject ho_ConnRgn3 = null;
            HObject ho_Seg1 = null;
            HObject ho_ConnRgn4 = null;
            HObject ho_Characters = null;
            HObject ho_RegionsClosing = null;
            HObject ho_RegionsClosing1 = null;
            HObject ho_ImageInvert = null;
            HObject ho_ImageMean = null;
            HObject ho_RegionDynThresh = null;
            HObject ho_RegionOpening = null;
            HObject ho_RegionDynThresh1 = null;
            HObject ho_RegionOpeningRectangle1 = null;
            HObject ho_RegionIntersection = null;
            HObject ho_RegionClosing = null;
            HObject ho_RegionThresh = null;
            HObject ho_RegionClosingRectangle1 = null;
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Regions1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans1);
            HOperatorSet.GenEmptyObj(out ho_ImageAffineTrans1);
            HOperatorSet.GenEmptyObj(out ho_Regions2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SortRgn1);
            HOperatorSet.GenEmptyObj(out ho_SortRgn2);
            HOperatorSet.GenEmptyObj(out ho_ImgBinSrc);
            HOperatorSet.GenEmptyObj(out ho_ImgBinSrc1);
            HOperatorSet.GenEmptyObj(out ho_Seg);
            HOperatorSet.GenEmptyObj(out ho_ConnRgn3);
            HOperatorSet.GenEmptyObj(out ho_Seg1);
            HOperatorSet.GenEmptyObj(out ho_ConnRgn4);
            HOperatorSet.GenEmptyObj(out ho_Characters);
            HOperatorSet.GenEmptyObj(out ho_RegionsClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionsClosing1);
            HOperatorSet.GenEmptyObj(out ho_ImageInvert);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThresh);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThresh1);
            HOperatorSet.GenEmptyObj(out ho_RegionOpeningRectangle1);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionThresh);
            HOperatorSet.GenEmptyObj(out ho_RegionClosingRectangle1);

            HTuple hv_Width;
            HTuple hv_Height;
            HTuple hv_Row;
            HTuple hv_Column;
            HTuple hv_Phi;
            HTuple hv_Length1;
            HTuple hv_Length2;
            HTuple hv_area;
            HTuple hv_OCRHandle;
            HTuple hv_Class;
            HTuple hv_Confidence;
            HTuple hv_Rad;
            HTuple hv_HomMat2DIdentity;
            HTuple hv_HomMat2DRotate;
            HTuple hv_Class1;
            HTuple hv_Confidence1;
            HTuple hv_Threshold;
            HTuple hv_Threshold1;
            HTuple hv_TextModel;
            HTuple hv_TextResultID;
            HTuple hv_Classes;
            HTuple ResultRect2Row;
            HTuple ResultRect2Col;
            HTuple ResultRect2Phi;
            HTuple ResultRect2Length1;
            HTuple ResultRect2Length2;
            try
            {
                sResult = "";
                ho_ResultImage = new HObject();
                if (TheImage == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(TheImage, out ho_Image);
                Window.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, Window);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, MyLens_Mold_Cave.dReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 7, 7);
                ho_ImageEmphasize.Dispose();
                HOperatorSet.Emphasize(ho_ReducedImage, out ho_ImageEmphasize, 7, 7,
    1);
                ho_ImageInvert.Dispose();
                if (MyLens_Mold_Cave.binGegion)
                {
                    HOperatorSet.InvertImage(ho_ImageEmphasize, out ho_ImageInvert);
                }
                else
                {
                    HOperatorSet.CopyImage(ho_ImageEmphasize, out ho_ImageInvert);
                }
                //HOperatorSet.CopyImage(ho_ImageEmphasize, out ho_ImageInvert);
                try
                {
                    ho_Regions.Dispose();
                    HOperatorSet.Threshold(ho_ImageMedian, out ho_Regions, MyLens_Mold_Cave.dGraythresholdDown, MyLens_Mold_Cave.dGraythresholdUp);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_Regions, out ho_RegionFillUp);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, ((new HTuple("area")).TupleConcat(
         "rect2_len1")).TupleConcat("rect2_len2"), "and", ((new HTuple(MyLens_Mold_Cave.dAreaDown)).TupleConcat(
         MyLens_Mold_Cave.dLength1Down)).TupleConcat(MyLens_Mold_Cave.dLength2Down), ((new HTuple(MyLens_Mold_Cave.dAreaUp)).TupleConcat(MyLens_Mold_Cave.dLength1Up)).TupleConcat(
         MyLens_Mold_Cave.dLength2Up));
                    HOperatorSet.SmallestRectangle2(ho_SelectedRegions, out hv_Row, out hv_Column, out hv_Phi,
                        out hv_Length1, out hv_Length2);
                    ho_InitialReectangle2.Dispose();
                    HOperatorSet.GenRectangle2(out ho_InitialReectangle2, hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2);
                    Lens_Mold_Cave.gen_rectangle2_center(ho_ImageMedian, out ho_Contour, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter,
                       hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2, MyLens_Mold_Cave.dcliperlength, MyLens_Mold_Cave.dthreshold, MyLens_Mold_Cave.spolarity, MyLens_Mold_Cave.sedgeSelect,
                       out ResultRect2Row, out ResultRect2Col, out ResultRect2Phi, out ResultRect2Length1, out ResultRect2Length2);

                    //catch
                    //{

                    //}
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle2(out ho_Rectangle, ResultRect2Row + MyLens_Mold_Cave.dOCRCenterDistance, ResultRect2Col,
                        0, MyLens_Mold_Cave.dOCRRectangleLength, MyLens_Mold_Cave.dOCRRectangleWidth);
                    ho_Rectangle1.Dispose();
                    HOperatorSet.GenRectangle2(out ho_Rectangle1, ResultRect2Row - MyLens_Mold_Cave.dOCRCenterDistance, ResultRect2Col,
                        0, MyLens_Mold_Cave.dOCRRectangleLength, MyLens_Mold_Cave.dOCRRectangleWidth);
                }
                catch
                {
                    sResult = "Miss";
                    Lens_Mold_Cave.set_display_font(Window, 40, "mono", "true", "false");
                    HOperatorSet.SetColor(Window, "blue");
                    HOperatorSet.SetTposition(Window, 2000, 100);
                    HOperatorSet.WriteString(Window, sResult);
                    HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                    return;

                }
                ho_RegionUnion.Dispose();

                //上面区域处理        
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageInvert, ho_Rectangle1, out ho_ImageReduced);
                 
                ho_RegionThresh.Dispose();
                if (MyLens_Mold_Cave.iThresholdSelect == 0)
                {

                    HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean, MyLens_Mold_Cave.dMeanfilte, MyLens_Mold_Cave.dMeanfilte);
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean, out ho_RegionThresh,
                            MyLens_Mold_Cave.dMeanfilte, "dark");
                }
                else
                {
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_RegionThresh, MyLens_Mold_Cave.dOCRthresholdDown, MyLens_Mold_Cave.dOCRthresholdUp);
                }
              
                //HObject ho_ImageMean1 = new HObject();
                //ho_ImageMean1.Dispose();
                //HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, 3, 3);
                //HObject ho_ImageMean2 = new HObject();
                //ho_ImageMean2.Dispose();
                //HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean2, MyLens_Mold_Cave.dMeanfilte, MyLens_Mold_Cave.dMeanfilte);
                //ho_RegionDynThresh.Dispose();
                //HOperatorSet.DynThreshold(ho_ImageMean2, ho_ImageMean1, out ho_RegionDynThresh,
                //    MyLens_Mold_Cave.dMeanfilte - 3, "dark");

                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionThresh, out ho_RegionFillUp);
                if (MyLens_Mold_Cave.bClosing)
                {

                    ho_RegionClosingRectangle1.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionThresh, out ho_RegionClosingRectangle1, MyLens_Mold_Cave.dCloseWidth, MyLens_Mold_Cave.dCloseHeight);
                    ho_RegionFillUp.Dispose();
                    ho_RegionFillUp = ho_RegionClosingRectangle1;

                }
                if (MyLens_Mold_Cave.bOpenging)
                {
                    ho_RegionOpeningRectangle1.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionFillUp, out ho_RegionOpeningRectangle1, MyLens_Mold_Cave.dOpenWidth, MyLens_Mold_Cave.dOpenHeight);
                    ho_RegionFillUp.Dispose();
                    ho_RegionFillUp = ho_RegionOpeningRectangle1;
                }
       
                ho_ConnectedRegions1.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions1);
              
        
                ho_SelectedRegions1.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions1, ((new HTuple("width")).TupleConcat(
    "height")), "and", ((new HTuple(MyLens_Mold_Cave.dOCRWidthDown)).TupleConcat(
    MyLens_Mold_Cave.dOCRHeightDown)), ((new HTuple(MyLens_Mold_Cave.dOCRWidthUp)).TupleConcat(MyLens_Mold_Cave.dOCRHeightUp)));

                HOperatorSet.RegionToBin(ho_SelectedRegions1, out ho_ImgBinSrc, 0, 255, hv_Width,
    hv_Height);

                ho_SortRgn1.Dispose();
                HOperatorSet.SortRegion(ho_SelectedRegions1, out ho_SortRgn1, "character", "true", "row");


                //下面区域处理

                //对下面图片进行180旋转             
                HOperatorSet.TupleRad(180, out hv_Rad);
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity, -hv_Rad, ResultRect2Row, ResultRect2Col,
                    out hv_HomMat2DRotate);
                ho_RegionAffineTrans1.Dispose();
                HOperatorSet.AffineTransRegion(ho_Rectangle1, out ho_RegionAffineTrans1,
                    hv_HomMat2DRotate, "nearest_neighbor");
                ho_ImageAffineTrans1.Dispose();
                HOperatorSet.AffineTransImage(ho_ImageInvert, out ho_ImageAffineTrans1, hv_HomMat2DRotate,
                    "constant", "false");
                ho_ImageReduced1.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageAffineTrans1, ho_Rectangle1, out ho_ImageReduced1);
                //ho_Regions2.Dispose();
                //HOperatorSet.MeanImage(ho_ImageReduced1, out ho_ImageMean1, 3, 3);
                //ho_ImageMean2.Dispose();
                //HOperatorSet.MeanImage(ho_ImageReduced1, out ho_ImageMean2, MyLens_Mold_Cave.dMeanfilte, MyLens_Mold_Cave.dMeanfilte);
                //ho_RegionDynThresh.Dispose();
                //HOperatorSet.DynThreshold(ho_ImageMean2, ho_ImageMean1, out ho_RegionDynThresh1,
                //    MyLens_Mold_Cave.dMeanfilte - 3, "dark");


                ho_RegionThresh.Dispose();
                if (MyLens_Mold_Cave.iThresholdSelect == 0)
                {

                    HOperatorSet.MeanImage(ho_ImageReduced1, out ho_ImageMean, MyLens_Mold_Cave.dMeanfilte, MyLens_Mold_Cave.dMeanfilte);
                    HOperatorSet.DynThreshold(ho_ImageReduced1, ho_ImageMean, out ho_RegionThresh,
                            MyLens_Mold_Cave.dMeanfilte, "dark");
                }
                else
                {
                    HOperatorSet.Threshold(ho_ImageReduced1, out ho_RegionThresh, MyLens_Mold_Cave.dOCRthresholdDown, MyLens_Mold_Cave.dOCRthresholdUp);
                }



                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionThresh, out ho_RegionFillUp);
                if (MyLens_Mold_Cave.bClosing)
                {

                    ho_RegionClosingRectangle1.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionThresh, out ho_RegionClosingRectangle1, MyLens_Mold_Cave.dCloseWidth, MyLens_Mold_Cave.dCloseHeight);
                    ho_RegionFillUp.Dispose();
                    ho_RegionFillUp = ho_RegionClosingRectangle1;

                }
                if (MyLens_Mold_Cave.bOpenging)
                {
                    ho_RegionOpeningRectangle1.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionFillUp, out ho_RegionOpeningRectangle1, MyLens_Mold_Cave.dOpenWidth, MyLens_Mold_Cave.dOpenHeight);
                    ho_RegionFillUp.Dispose();
                    ho_RegionFillUp = ho_RegionOpeningRectangle1;
                }
                ho_ConnectedRegions2.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions2);         
                ho_SelectedRegions2.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions2, ((new HTuple("width")).TupleConcat(
            "height")), "and", ((new HTuple(MyLens_Mold_Cave.dOCRWidthDown)).TupleConcat(
            MyLens_Mold_Cave.dOCRHeightDown)), ((new HTuple(MyLens_Mold_Cave.dOCRWidthUp)).TupleConcat(MyLens_Mold_Cave.dOCRHeightUp)));
                ho_ImgBinSrc1.Dispose();
                HOperatorSet.RegionToBin(ho_SelectedRegions2, out ho_ImgBinSrc1, 0, 255, hv_Width,
    hv_Height);

                ho_SortRgn2.Dispose();
                HOperatorSet.SortRegion(ho_SelectedRegions2, out ho_SortRgn2, "character", "true", "row");
                //调用系统的分类器
                HOperatorSet.ReadOcrClassMlp("Document_0-9A-Z_NoRej.omc", out hv_OCRHandle);
                //HOperatorSet.ReadOcrClassMlp("Industrial_0-9A-Z_NoRej.omc", out hv_OCRHandle);

                HOperatorSet.DoOcrMultiClassMlp(ho_SortRgn1, ho_ImgBinSrc, hv_OCRHandle,
                    out hv_Class, out hv_Confidence);
                HOperatorSet.DoOcrMultiClassMlp(ho_SortRgn2, ho_ImgBinSrc1, hv_OCRHandle,
                    out hv_Class1, out hv_Confidence1);

                //结果处理
                Regex r = new Regex(@"^[0-9]+$");
                Regex r1 = new Regex(@"^[A-Z]+$");
                string sCave = "";
                string sMold = "";
                if (n == 1)
                {
                    sCave = MyLens_Mold_Cave.sTray1Cave;
                    sMold = MyLens_Mold_Cave.sTray1Mold;
                }
                else
                {
                    sCave = MyLens_Mold_Cave.sTray2Cave;
                    sMold = MyLens_Mold_Cave.sTray2Mold;

                }
                string str = "";
                string str1 = "";
                for (int i = 0; i < ((string[])hv_Class).Length; i++)
                {
                    str = str + ((string[])hv_Class)[i];
                }
                for (int i = 0; i < ((string[])hv_Class1).Length; i++)
                {
                    str1 = str1 + ((string[])hv_Class1)[i];
                }
                if (str.Length > 2)
                {
                    str = str.Substring(0, 2);
                }
                if (str1.Length > 2)
                {
                    str1 = str.Substring(0, 2);
                }
                if (str == "OU")
                {
                    str = "8";
                }
                if (str1 == "OU")
                {
                    str1 = "8";
                }
                //显示
                Lens_Mold_Cave.set_display_font(Window, 40, "mono", "true", "false");
                HOperatorSet.SetColor(Window, "green");
                HOperatorSet.SetTposition(Window, 100, 10);
                HOperatorSet.WriteString(Window, str);
                HOperatorSet.SetTposition(Window, 300, 10);
                HOperatorSet.WriteString(Window, str1);
                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);

                if (r.IsMatch(str))
                {
                    if (str != sCave)
                    {
                        sResult = "NG";
                        HOperatorSet.SetColor(Window, "red");
                        HOperatorSet.SetTposition(Window, 2000, 100);
                        HOperatorSet.WriteString(Window, sResult);
                        HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                        return;
                    }

                }
                else if (r1.IsMatch(str))
                {
                    if (str != sMold)
                    {
                        sResult = "NG";
                        HOperatorSet.SetColor(Window, "red");
                        HOperatorSet.SetTposition(Window, 2000, 100);
                        HOperatorSet.WriteString(Window, sResult);
                        HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                        return;

                    }

                }
                else
                {
                    sResult = "NG";
                    HOperatorSet.SetColor(Window, "red");
                    HOperatorSet.SetTposition(Window, 2000, 100);
                    HOperatorSet.WriteString(Window, sResult);
                    HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                    return;
                }
                if (r.IsMatch(str1))
                {
                    if (str1 != sCave)
                    {
                        sResult = "NG";
                        HOperatorSet.SetColor(Window, "red");
                        HOperatorSet.SetTposition(Window, 2000, 100);
                        HOperatorSet.WriteString(Window, sResult);
                        HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                        return;
                    }

                }
                else if (r1.IsMatch(str1))
                {
                    if (str1 != sMold)
                    {
                        sResult = "NG";
                        HOperatorSet.SetColor(Window, "red");
                        HOperatorSet.SetTposition(Window, 2000, 100);
                        HOperatorSet.WriteString(Window, sResult);
                        HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                        return;

                    }

                }
                else
                {
                    sResult = "NG";
                    HOperatorSet.SetColor(Window, "red");
                    HOperatorSet.SetTposition(Window, 2000, 100);
                    HOperatorSet.WriteString(Window, sResult);
                    HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                    return;
                }
                sResult = "OK";
                HOperatorSet.SetColor(Window, "green");
                HOperatorSet.SetTposition(Window, 2000, 100);
                HOperatorSet.WriteString(Window, sResult);
                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
            }

            catch
            {
                Lens_Mold_Cave.set_display_font(Window, 40, "mono", "true", "false");
                sResult = "NG";
                HOperatorSet.SetColor(Window, "red");  
                HOperatorSet.SetTposition(Window, 2000, 100);
                HOperatorSet.WriteString(Window, sResult);                
                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                return;

            }
 
        }
        /// <summary>
        /// 进制转换(有符号十进制转十六进制、高低位置换)(雙字)
        /// </summary>
        /// <param name="iNumber"></param>
        /// <returns></returns>
        public static string NToHString2(int iNumber)
        {
            string sResult = string.Empty;
            if (iNumber < 0)
                sResult = (iNumber).ToString("X8");
            else
                sResult = Convert.ToString(iNumber, 16).PadLeft(8, '0').ToUpper();

            sResult = sResult.Substring(4, 4) + sResult.Substring(0, 4);
            return sResult;
        }
        /// <summary>
        /// 进制转换(有符号十进制转十六进制)(單字)
        /// </summary>
        /// <param name="iNumber"></param>
        /// <returns></returns>
        public static string NToHString(int iNumber)
        {
            string sResult = string.Empty;
            if (iNumber < 0)
            {
                sResult = Convert.ToString(iNumber, 16).PadLeft(4, '0').ToUpper();
                if (sResult.Length == 8)
                    sResult = sResult.Substring(4, 4);
            }
            else
            {
                sResult = Convert.ToString(iNumber, 16).PadLeft(4, '0').ToUpper();
                if (sResult.Length == 8)
                    sResult = sResult.Substring(4, 4);
            } return sResult;
        }
        /// <summary>
        /// 是否旋轉
        /// </summary>
        /// <param name="y"></param>
        /// <param name="ResultX"></param>
        /// <param name="sResult"></param>
        public void  ToPlcResult_Result(int y,int ResultX,out string sResult)
        {
            sResult = "";
            for (int x = 1; x <= 20; x++)
            {
                if (x > ResultX)
                {
                    sResult = sResult + "0006";
                }
                else
                {
                    int n = (y - 1) * ResultX + x - 1;
                    if (!Vision.VisionResult.ContainsKey(n))
                    {
                        sResult = sResult + "0006";
                    }
                    else
                        switch (Vision.VisionResult[n])
                        {
                            case null:
                                {
                                    if (Sys.bThrow_Miss)
                                        sResult = sResult + "0002";
                                    else
                                        sResult = sResult + "0006";
                                    break;
                                }
                            case "Miss":
                                {
                                    if (Sys.bThrow_Miss)
                                        sResult = sResult + "0002";
                                    else
                                        sResult = sResult + "0006";
                                    break;
                                }
                            case "OK":
                                {
                                    if (Sys.bThrow_OK)
                                        sResult = sResult + "0002";
                                    else
                                    {
                                        if (!Vision.VisionBarcodeRotate.ContainsKey(n) || Vision.VisionBarcodeRotate[n] == 0)//如果旋轉結果為空or角度旋轉為0,不給旋轉NG(3)
                                        {
                                            sResult = sResult + "0001";
                                        }
                                        else
                                        {
                                            sResult = sResult + "0003";
                                        }
                                    }
                                    break;
                                }
                            case "NG":
                                {
                                    if (Sys.bThrow_NG)
                                        sResult = sResult + "0002";
                                    else
                                        sResult = sResult + "0001";
                                    break;
                                }
                            case "NG2":
                                {
                                    if (Sys.bThrow_NG2)
                                        sResult = sResult + "0002";
                                    else
                                        sResult = sResult + "0001";
                                    break;
                                }
                            case "NG3":
                                {
                                    if (Sys.bThrow_NG3)
                                        sResult = sResult + "0002";
                                    else
                                        sResult = sResult + "0001";
                                    break;
                                }
                            case "NG4":
                                {
                                    if (Sys.bThrow_NG4)
                                        sResult = sResult + "0002";
                                    else
                                        sResult = sResult + "0001";
                                    break;
                                }
                            case "NG5":
                                {
                                    if (Sys.bThrow_NG5)
                                        sResult = sResult + "0002";
                                    else
                                        sResult = sResult + "0001";
                                    break;
                                }
                        }
                }
            }
        }

        public void ToPlcResult_Angle(int y, int ResultX, out string sAngle)
        {
            sAngle = "";
            for (int x = 1; x <= 20; x++)
            {
                if (x > ResultX)
                {
                    sAngle = sAngle + "0000";
                }
                else
                {
                    int n = (y - 1) * ResultX + x - 1;
                    if (!Vision.VisionBarcodeRotate.ContainsKey(n))
                    {
                        sAngle = sAngle + "0000";
                    }
                    else
                    {
                        sAngle = sAngle + NToHString(Vision.VisionBarcodeRotate[n] * 10);
                    }
                }
            }
        }
    }
}
