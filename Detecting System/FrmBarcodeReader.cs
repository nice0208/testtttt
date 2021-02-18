using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HalconDotNet;

namespace Detecting_System
{
    public partial class FrmBarcodeReader : Form
    {
        FrmParent parent;
        FrmRun Run;
        public FrmBarcodeReader(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;
            InitializeComponent();
        }
        public class Result
        {
            public string Barcode = "NA";
            public double MeanLight = 0;
            public object[] Overall_Quality;
            public object[] Cell_Contrast;
            public object[] Print_Growth;
            public object[] Unused_Error_Correction;
            public object[] Cell_Modulation;
            public object[] Fixed_Pattern_Damage;
            public object[] Grid_Nonuniformity;
            public object[] Decode;
            public Result()
            {
                Overall_Quality = new object[2] { "F", 0};
                Cell_Contrast = new object[2] { "F", 0};
                Print_Growth = new object[2] { "F", 0};
                Unused_Error_Correction = new object[2] { "F", 0};
                Cell_Modulation = new object[2] { "F", 0};
                Fixed_Pattern_Damage = new object[2] { "F", 0};
                Grid_Nonuniformity = new object[2] { "F", 0};
                Decode = new object[2] { "F", 0};
            }

        }
        #region halcon參數1
        // Chapter: Graphics / Text
        // Short Description: Set font independent of OS 
        public void set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font,
            HTuple hv_Bold, HTuple hv_Slant)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_OS = null, hv_Fonts = new HTuple();
            HTuple hv_Style = null, hv_Exception = new HTuple(), hv_AvailableFonts = null;
            HTuple hv_Fdx = null, hv_Indices = new HTuple();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();

            // Initialize local and output iconic variables 
            //This procedure sets the text font of the current window with
            //the specified attributes.
            //
            //Input parameters:
            //WindowHandle: The graphics window for which the font will be set
            //Size: The font size. If Size=-1, the default of 16 is used.
            //Bold: If set to 'true', a bold font is used
            //Slant: If set to 'true', a slanted font is used
            //
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            // dev_get_preferences(...); only in hdevelop
            // dev_set_preferences(...); only in hdevelop
            if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
            {
                hv_Size_COPY_INP_TMP = 16;
            }
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                //Restore previous behaviour
                hv_Size_COPY_INP_TMP = ((1.13677 * hv_Size_COPY_INP_TMP)).TupleInt();
            }
            else
            {
                hv_Size_COPY_INP_TMP = hv_Size_COPY_INP_TMP.TupleInt();
            }
            if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Courier";
                hv_Fonts[1] = "Courier 10 Pitch";
                hv_Fonts[2] = "Courier New";
                hv_Fonts[3] = "CourierNew";
                hv_Fonts[4] = "Liberation Mono";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Consolas";
                hv_Fonts[1] = "Menlo";
                hv_Fonts[2] = "Courier";
                hv_Fonts[3] = "Courier 10 Pitch";
                hv_Fonts[4] = "FreeMono";
                hv_Fonts[5] = "Liberation Mono";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Luxi Sans";
                hv_Fonts[1] = "DejaVu Sans";
                hv_Fonts[2] = "FreeSans";
                hv_Fonts[3] = "Arial";
                hv_Fonts[4] = "Liberation Sans";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Times New Roman";
                hv_Fonts[1] = "Luxi Serif";
                hv_Fonts[2] = "DejaVu Serif";
                hv_Fonts[3] = "FreeSerif";
                hv_Fonts[4] = "Utopia";
                hv_Fonts[5] = "Liberation Serif";
            }
            else
            {
                hv_Fonts = hv_Font_COPY_INP_TMP.Clone();
            }
            hv_Style = "";
            if ((int)(new HTuple(hv_Bold.TupleEqual("true"))) != 0)
            {
                hv_Style = hv_Style + "Bold";
            }
            else if ((int)(new HTuple(hv_Bold.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Bold";
                throw new HalconException(hv_Exception);
            }
            if ((int)(new HTuple(hv_Slant.TupleEqual("true"))) != 0)
            {
                hv_Style = hv_Style + "Italic";
            }
            else if ((int)(new HTuple(hv_Slant.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Slant";
                throw new HalconException(hv_Exception);
            }
            if ((int)(new HTuple(hv_Style.TupleEqual(""))) != 0)
            {
                hv_Style = "Normal";
            }
            HOperatorSet.QueryFont(hv_ExpDefaultWinHandle, out hv_AvailableFonts);
            hv_Font_COPY_INP_TMP = "";
            for (hv_Fdx = 0; (int)hv_Fdx <= (int)((new HTuple(hv_Fonts.TupleLength())) - 1); hv_Fdx = (int)hv_Fdx + 1)
            {
                hv_Indices = hv_AvailableFonts.TupleFind(hv_Fonts.TupleSelect(hv_Fdx));
                if ((int)(new HTuple((new HTuple(hv_Indices.TupleLength())).TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple(((hv_Indices.TupleSelect(0))).TupleGreaterEqual(0))) != 0)
                    {
                        hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(hv_Fdx);
                        break;
                    }
                }
            }
            if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(""))) != 0)
            {
                throw new HalconException("Wrong value of control parameter Font");
            }
            hv_Font_COPY_INP_TMP = (((hv_Font_COPY_INP_TMP + "-") + hv_Style) + "-") + hv_Size_COPY_INP_TMP;
            HOperatorSet.SetFont(hv_ExpDefaultWinHandle, hv_Font_COPY_INP_TMP);
            // dev_set_preferences(...); only in hdevelop

            return;
        }
        #endregion
        #region halcon參數2
        // Local iconic variables 

        public HTuple hv_ExpDefaultWinHandle;
        HObject ho_Image, ho_Region, ho_RegionClosing;
        HObject ho_ConnectedRegions, ho_RegionFillUp, ho_SelectedRegions;
        HObject ho_Rectangle, ho_ImageReduced, ho_SymbolXLDs;

        // Local control variables 

        HTuple hv_Width = null, hv_Height = null, hv_WindowHandle = new HTuple();
        HTuple hv_OK = null, hv_NG = null, hv_DataCodeHandle = null;
        HTuple hv_HandleRow1 = null, hv_HandleColumn1 = null, hv_HandleRow2 = null;
        HTuple hv_HandleColumn2 = null, hv_T1 = null, hv_ResultHandles = null;
        HTuple hv_DecodedDataStrings = null, hv_ResultValues1 = null;
        HTuple hv_ResultValues2 = null, hv_ResultValues3 = null;
        HTuple hv_T2 = null, hv_Time = null, hv_DecodedData = new HTuple();
        HTuple hv_A = new HTuple();
        #endregion
        private void btnOneShot_Click(object sender, EventArgs e)
        {
            parent.OneShot();
        }

        private void btnImageProPlus_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            ImageProPlus(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);          
        }

        private void btnImageSave_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            //結果圖/原圖臨時存放區
            string Resultpath = Sys.ImageSavePath0 + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Result";
            string Originalpath = Sys.ImageSavePath0 + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Original";
            //結果圖上傳存放區
            string UpLoadpath = Sys.ImageSavePath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Result";
            string TrayBarcode = "NA";
            
                if (Tray.Barcode_1 != "")
                    TrayBarcode = Tray.Barcode_1;
            
            string LensBarcode = "NA";
            //檔名
            string Namepath = TrayBarcode + "_(" + string.Format("{0}.{1}", Tray.CurrentRow, Tray.CurrentColumn) + ")_" + LensBarcode;
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
            if (Vision.VisionResult[Tray.n] == "OK")
            {
                if (Sys.OptionOK)
                {
                    //儲存擷取當前畫面圖片
                    string pathOK = Sys.ImageSavePath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Result";
                    if (!Directory.Exists(pathOK))
                    {
                        Directory.CreateDirectory(pathOK);
                    }
                    //儲存圖片
                    HOperatorSet.WriteImage(ho_Image, "png", 0, Resultpath + "\\" + Namepath + "_OK_" + Time);
                    bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_OK_" + Time + ".png", pathOK + "\\" + Namepath + "_OK_" + Time, 360, 270, 100);
                }
            }
            else if (Vision.VisionResult[Tray.n] == "NG")
            {
                if (Sys.OptionNG)
                {
                    //儲存擷取當前畫面圖片
                    string pathNG = Sys.ImageSavePath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Result";
                    if (!Directory.Exists(pathNG))
                    {
                        Directory.CreateDirectory(pathNG);
                    }
                    //儲存圖片
                    HOperatorSet.WriteImage(ho_Image, "png", 0, Resultpath + "\\" + Namepath + "_NG_" + Time);
                    bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_NG_" + Time + ".png", pathNG + "\\" + Namepath + "_NG_" + Time, 360, 270, 100);
                }
            }
            else if (Vision.VisionResult[Tray.n] == "NG2")
            {
                if (Sys.OptionNG)
                {
                    //儲存擷取當前畫面圖片
                    string pathNG = Sys.ImageSavePath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Result";
                    if (!Directory.Exists(pathNG))
                    {
                        Directory.CreateDirectory(pathNG);
                    }
                    //儲存圖片
                    HOperatorSet.WriteImage(ho_Image, "png", 0, Resultpath + "\\" + Namepath + "_NG2_" + Time);
                    bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_NG2_" + Time + ".png", pathNG + "\\" + Namepath + "_NG2_" + Time, 320, 240, 100);
                }
            }
            else if (Vision.VisionResult[Tray.n] == "NG3")
            {
                if (Sys.OptionNG)
                {
                    //儲存擷取當前畫面圖片
                    string pathNG = Sys.ImageSavePath + "\\" + DateTime.Now.ToString("yyyy_MM_dd");
                    if (!Directory.Exists(pathNG))
                    {
                        Directory.CreateDirectory(pathNG);
                    }
                    //儲存圖片
                    HOperatorSet.WriteImage(ho_Image, "png", 0, Resultpath + "\\" + Namepath + "_NG3_" + Time);
                    bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_NG3_" + Time + ".png", pathNG + "\\" + Namepath + "_NG3_" + Time, 360, 270, 100);
                }
            }
            else//Miss
            {
                if (Sys.OptionNG)
                {
                    //儲存擷取當前畫面圖片
                    string pathNG = Sys.ImageSavePath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Result";
                    if (!Directory.Exists(pathNG))
                    {
                        Directory.CreateDirectory(pathNG);
                    }
                    //儲存圖片
                    HOperatorSet.WriteImage(ho_Image, "png", 0, Resultpath + "\\" + Namepath + "_Miss_" + Time);
                    bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_Miss_" + Time + ".png", pathNG + "\\" + Namepath + "_Miss_" + Time, 360, 270, 100);
                }
            }

            if (Sys.OptionOriginal)
            {
                //儲存原始圖片
                HOperatorSet.WriteImage(My.ho_Image, "bmp", 0, Originalpath + "\\" + Namepath + Time);
            }
        }

        private void btnContinueShot_Click(object sender, EventArgs e)
        {
            if (!My.ContinueShot)
            {
                parent.ContinuousShot();
                My.ContinueShot = true;
            }
            else
            {
                parent.Stop();
                My.ContinueShot = false;
            }
        }

        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            string ImagePath;           // 定义模板图片的路径
            HObject readImage = null;	// 定义一个图像常量
            OpenFileDialog openFileDialog1 = new OpenFileDialog(); //打开文件进行选择
            openFileDialog1.Filter = "BMP文件|*.bmp*|PNG文件|*.png*|JPEG文件|*.jpg*";     //图片的文件格式
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // 如果可以打开该文件路径，将该文件路径的图片显示在hWindowControl1窗口，并将图片变量赋值给readImage
                ImagePath = openFileDialog1.FileName;
                readImage = ReadPicture(hWindowControl1.HalconWindow, ImagePath);

                HOperatorSet.CopyImage(readImage, out My.ho_Image);
                // 读取这张图片并将图片赋值给readImage,这句就是直接调的halcon类了，下边public定义的的是他的类
            }

        }

        public HObject ReadPicture(HTuple window, string ImagePath)
        {
            // 得到图片显示的窗口句柄
            hv_ExpDefaultWinHandle = window; //从上个函数传进来的窗口句柄
            HOperatorSet.GenEmptyObj(out ho_Image);
            ho_Image.Dispose();
            HOperatorSet.ReadImage(out ho_Image, ImagePath); //从这个路径读取图片
            {//旋轉270度
                HObject ExpTmpOutVar_0;
                HOperatorSet.RotateImage(ho_Image, out ExpTmpOutVar_0, 0, "constant");
                ho_Image.Dispose();
                ho_Image = ExpTmpOutVar_0;
            }
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height); //得到他的大小
            //轉成灰度圖像
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.Rgb1ToGray(ho_Image, out ExpTmpOutVar_0);
                ho_Image.Dispose();
                ho_Image = ExpTmpOutVar_0;
            }
            HOperatorSet.SetWindowAttr("background_color", "black");
            //调整窗口显示大小以适应图片，这句一定要加上，不然图片显示的不全
            HOperatorSet.SetPart(hv_ExpDefaultWinHandle, 0, 0, hv_Height - 1, hv_Width - 1);
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle); //将图像在该窗口进行显示
            return ho_Image; //返回这个图像
        }

        private void FrmBarcodeReader_Load(object sender, EventArgs e)
        {
            ReadPara();
            LoadSettingLight();
        }

        public void ReadPara()
        {
            #region
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);
            #endregion
            cmbProduction.SelectedIndex = MyBarcodeReader.Production;
            cbVerification.Checked = MyBarcodeReader.Verification;
            cmbOkAddition.SelectedIndex = MyBarcodeReader.OkAddition;
        }
        
        private void btnRangeSet_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            ho_Image = My.ho_Image;
            if (ho_Image == null)
                return;
            //畫檢視範圍
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);

            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
            HOperatorSet.DrawRectangle1(hv_ExpDefaultWinHandle, out MyBarcodeReader.RangeRow1, out MyBarcodeReader.RangeColumn1, out MyBarcodeReader.RangeRow2, out MyBarcodeReader.RangeColumn2);
            //興趣範圍
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, MyBarcodeReader.RangeRow1, MyBarcodeReader.RangeColumn1, MyBarcodeReader.RangeRow2, MyBarcodeReader.RangeColumn2);

            HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
            HOperatorSet.DispObj(ho_Rectangle, hv_ExpDefaultWinHandle);

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "RangeRow1", MyBarcodeReader.RangeRow1.ToString(), Path);
            IniFile.Write("Setting", "RangeColumn1", MyBarcodeReader.RangeColumn1.ToString(), Path);
            IniFile.Write("Setting", "RangeRow2", MyBarcodeReader.RangeRow2.ToString(), Path);
            IniFile.Write("Setting", "RangeColumn2", MyBarcodeReader.RangeColumn2.ToString(), Path);
        }

        private void cbVerification_CheckedChanged(object sender, EventArgs e)
        {
            MyBarcodeReader.Verification = cbVerification.Checked;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "Verification", MyBarcodeReader.Verification.ToString(), Path);
        }

        public void ImageProPlus(HWindow Window, HObject theImage, int n)
        {
            if (theImage == null)
                return;
            HOperatorSet.CopyImage(theImage,out ho_Image);
            hv_ExpDefaultWinHandle = Window;
            Window.ClearWindow();
            Result result = new Result();
            string sBarcode = "", _sBarcode = "";
            
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            try
            {
                //bool a = true;
                //if (a)
                //{
                //    string ErrorMessage = "";
                //    bool error = false;
                //    HTuple ReadBarcodeTime;
                //    BarcodeReader.Barcode.Read(hv_ExpDefaultWinHandle, ho_Image, "standard_recognition", MyBarcodeReader.RangeRow1, MyBarcodeReader.RangeColumn1, MyBarcodeReader.RangeRow2, MyBarcodeReader.RangeColumn2, out ho_SymbolXLDs, out sBarcode, out ReadBarcodeTime, out error, out ErrorMessage);
                
                //}
                //else
                //{
                set_display_font(hv_ExpDefaultWinHandle, 15, "mono", "true", "false");
                //掃碼設置
                HOperatorSet.CreateDataCode2dModel("Data Matrix ECC 200", "default_parameters", "maximum_recognition", out hv_DataCodeHandle);
                HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "polarity", "any");
                HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "symbol_rows_min", 14);
                HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "symbol_rows_max", 18);
                if(MyBarcodeReader.Production==1)//讀取LensBarcode
                    HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "decoding_scheme", "raw");
                HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "persistence", 0);
                HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "timeout", 300);
                //剪出掃碼區域
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle, MyBarcodeReader.RangeRow1, MyBarcodeReader.RangeColumn1, MyBarcodeReader.RangeRow2, MyBarcodeReader.RangeColumn2);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.CropDomain(ho_ImageReduced, out ExpTmpOutVar_0);
                    ho_ImageReduced.Dispose();
                    ho_ImageReduced = ExpTmpOutVar_0;
                }
                ho_SymbolXLDs.Dispose();
                HOperatorSet.FindDataCode2d(ho_ImageReduced, out ho_SymbolXLDs, hv_DataCodeHandle, "stop_after_result_num", "1", out hv_ResultHandles, out hv_DecodedDataStrings);


                if ((int)(new HTuple((new HTuple(hv_DecodedDataStrings.TupleLength())).TupleEqual(0))) != 0)
                {
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("未能識別二維碼"));
                    HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                    Vision.VisionResult[n] = "Miss";
                    
                        Vision.Images_1[n] = ho_Image;
                        Vision.Images_Now[n] = Vision.Images_1[n];
                        Vision.ImagesOriginal_1[n] = theImage;
                    
                    HOperatorSet.ClearDataCode2dModel(hv_DataCodeHandle);
                }
                else
                {
                    HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandles.TupleSelect(0), "decoded_data", out hv_DecodedData);

                    Vision.VisionResult[n] = "OK";
                    if (MyBarcodeReader.Production == 0)
                    {
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                        int Length = 0;
                        Length = hv_DecodedData.Length;
                        int[] Barcode = new int[Length];
                        for (int i = 0; i <= Length-1; i++)
                        {
                            Barcode[i] = hv_DecodedData.TupleSelect(i);
                            _sBarcode = Convert.ToString(Barcode[i], 16);
                            //不足兩位補0
                            if (_sBarcode.Length < 2)
                            {
                                _sBarcode = "0" + _sBarcode;
                            }
                            _sBarcode = _sBarcode.Substring(_sBarcode.Length - 2);
                            sBarcode = sBarcode + _sBarcode;
                        }
                    }
                    else if (MyBarcodeReader.Production == 1)
                    {
                        int[] Barcode = new int[8];

                        for (int i = 0; i <= 7; i++)
                        {
                            Barcode[i] = (((hv_DecodedData.TupleSelect(i)) - ((149 * (i + 1)) % 255)) - 1) % 256;
                            _sBarcode = Convert.ToString(Barcode[i], 16);
                            //不足兩位補0
                            if (_sBarcode.Length < 2)
                            {
                                _sBarcode = "0" + _sBarcode;
                            }
                            _sBarcode = _sBarcode.Substring(_sBarcode.Length - 2);
                            sBarcode = sBarcode + _sBarcode;
                        }
                        if (sBarcode.Substring(2, 2) != "40")
                                sBarcode = "ERROR";
                    }
                    else if (MyBarcodeReader.Production == 2)
                    {
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                        int Length = 0;
                        Length = hv_DecodedData.Length;
                        int[] Barcode = new int[Length];
                        for (int i = 0; i <= Length-1; i++)
                        {
                            Barcode[i] = hv_DecodedData.TupleSelect(i);
                            _sBarcode = ((char)Barcode[i]).ToString();
                            //_sBarcode = Convert.ToString(Barcode[i], 16);
                            //不足兩位補0
                            //if (_sBarcode.Length < 2)
                            //{
                            //    _sBarcode = "0" + _sBarcode;
                            //}
                            //_sBarcode = _sBarcode.Substring(_sBarcode.Length - 2);
                            sBarcode = sBarcode + _sBarcode;
                        }
                        result.Barcode = sBarcode;
                    }
                    if (MyBarcodeReader.Verification)
                    {
                        #region 掃碼驗證
                        HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandles.TupleSelect(0), "quality_isoiec_tr_29158", out hv_ResultValues1);
                        HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandles.TupleSelect(0), "quality_isoiec_tr_29158_values", out hv_ResultValues3);
                        result.MeanLight = hv_ResultValues1[8];


                        //等級轉換
                        result.Overall_Quality[0] = LevelConversion(hv_ResultValues1[0]);//整體質量
                        result.Cell_Contrast[0] = LevelConversion(hv_ResultValues1[1]);//单元格对比
                        result.Print_Growth[0] = LevelConversion(hv_ResultValues1[10]);//打印过粗
                        result.Unused_Error_Correction[0] = LevelConversion(hv_ResultValues1[7]);//未使用的错误校正
                        result.Cell_Modulation[0] = LevelConversion(hv_ResultValues1[2]);//单元格调制
                        result.Fixed_Pattern_Damage[0] = LevelConversion(hv_ResultValues1[3]);//固定图案损坏
                        result.Grid_Nonuniformity[0] = LevelConversion(hv_ResultValues1[6]);//轴向不均匀性
                        result.Decode[0] = LevelConversion(hv_ResultValues1[4]);//解码
                        //等級結果
                        result.Overall_Quality[1] = ((double)hv_ResultValues1[0]).ToString("f3");
                        result.Cell_Contrast[1] = Math.Round((double)hv_ResultValues1[1], 3).ToString("f3");
                        result.Print_Growth[1] = Math.Round((double)hv_ResultValues1[10], 3).ToString("f3");
                        result.Unused_Error_Correction[1] = ((double)hv_ResultValues1[7]).ToString("f3");
                        result.Cell_Modulation[1] = ((double)hv_ResultValues1[2]).ToString("f3");
                        result.Fixed_Pattern_Damage[1] = ((double)hv_ResultValues1[3]).ToString("f3");
                        result.Grid_Nonuniformity[1] = Math.Round((double)hv_ResultValues1[6], 3).ToString("f3");
                        result.Decode[1] = ((double)hv_ResultValues1[4]).ToString("f3");
                        ////等級轉換
                        //result.Overall_Quality[0] = LevelConversion(hv_ResultValues1[0]);
                        //result.Cell_Contrast[0] = LevelConversion(hv_ResultValues1[1]);
                        //result.Print_Growth[0] = LevelConversion(hv_ResultValues1[10]);
                        //result.Unused_Error_Correction[0] = LevelConversion(hv_ResultValues1[7]);
                        //result.Cell_Modulation[0] = LevelConversion(hv_ResultValues1[2]);
                        //result.Fixed_Pattern_Damage[0] = LevelConversion(hv_ResultValues1[3]);
                        //result.Grid_Nonuniformity[0] = LevelConversion(hv_ResultValues1[6]);
                        //result.Decode[0] = LevelConversion(hv_ResultValues1[4]);
                        ////等級結果
                        //result.Overall_Quality[1] = ((double)hv_ResultValues1[0]).ToString("f3");
                        //result.Cell_Contrast[1] = Math.Round((double)hv_ResultValues3[1], 3).ToString("f3");
                        //result.Print_Growth[1] = Math.Round((double)hv_ResultValues3[10], 3).ToString("f3");
                        //result.Unused_Error_Correction[1] = ((double)hv_ResultValues1[7]).ToString("f3");
                        //result.Cell_Modulation[1] = ((double)hv_ResultValues1[2]).ToString("f3");
                        //result.Fixed_Pattern_Damage[1] = ((double)hv_ResultValues1[3]).ToString("f3");
                        //result.Grid_Nonuniformity[1] = Math.Round((double)hv_ResultValues3[6], 3).ToString("f3");
                        //result.Decode[1] = ((double)hv_ResultValues1[4]).ToString("f3");
                        #endregion
                        if (hv_ResultValues1[0] >= MyBarcodeReader.OkAddition)
                        {
                            Vision.VisionResult[n] = "OK";
                        }
                        else
                        {
                            Vision.VisionResult[n] = "NG";
                        }
                    }
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_SymbolXLDs, out ho_Region, "margin");
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.MoveRegion(ho_Region, out ExpTmpOutVar_0, MyBarcodeReader.RangeRow1, MyBarcodeReader.RangeColumn1);
                        ho_Region.Dispose();
                        ho_Region = ExpTmpOutVar_0;
                    }

                    //顯示結果
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(ho_Region, hv_ExpDefaultWinHandle);
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Code:" + sBarcode);
                    if (MyBarcodeReader.Verification)
                    {
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 50, 0);
                        string sMeanLight = ((double)result.MeanLight).ToString("f3");
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "MeanLight:" + sMeanLight);

                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 150, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, result.Overall_Quality[0] + ", " + result.Overall_Quality[1] + ", " + "Overall_Quality");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, result.Cell_Contrast[0] + ", " + result.Cell_Contrast[1] + ", " + "Cell_Contrast");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 250, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, result.Print_Growth[0] + ", " + result.Print_Growth[1] + ", " + "Print_Growth");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 300, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, result.Unused_Error_Correction[0] + ", " + result.Unused_Error_Correction[1] + ", " + "Unused_Error_Correct");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 350, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, result.Cell_Modulation[0] + ", " + result.Cell_Modulation[1] + ", " + "Cell_Modulation");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, result.Fixed_Pattern_Damage[0] + ", " + result.Fixed_Pattern_Damage[1] + ", " + "Fixed_Pattern_Damage");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 450, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, result.Grid_Nonuniformity[0] + ", " + result.Grid_Nonuniformity[1] + ", " + "Grid_Nonuniformity");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 500, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, result.Decode[0] + ", " + result.Decode[1] + ", " + "Decode");

                        set_display_font(hv_ExpDefaultWinHandle, 20, "mono", "true", "false");
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 550, 0);

                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Grade = " + result.Overall_Quality[0]);
                        if (0.7 <= result.MeanLight && result.MeanLight <= 0.86)
                        {

                        }
                        else
                        {
                            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                            HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1850, 0);
                            if (result.MeanLight < 0.7)
                                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "平均亮度過低,請調整光源或曝光以提升亮度");
                            else if (0.86 < result.MeanLight)
                                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "平均亮度過高,請調整光源或曝光以降低亮度");
                        }
                    }
                    //HOperatorSet.ClearDataCode2dModel(hv_DataCodeHandle);
                    HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                    Vision.VisionResult[n] = "OK";
                    
                        Vision.Images_1[n] = ho_Image;
                        Vision.Images_Now[n] = Vision.Images_1[n];
                        Vision.ImagesOriginal_1[n] = theImage;
                    
                    //}

                    HOperatorSet.ClearDataCode2dModel(hv_DataCodeHandle);
                    ho_SymbolXLDs.Dispose();
                }
                //Vision.Images_1[n] = ho_Image;
                //Vision.Images_Now[n] = Vision.Images_1[n];
                //Vision.ImagesOriginal_1[n] = theImage;
                //ho_Image.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced.Dispose();
                

            }
            catch
            {
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("未能識別二維碼NG"));
                Vision.VisionResult[n] = "Miss";
                
                Vision.Images_1[n] = ho_Image;
                Vision.Images_Now[n] = Vision.Images_1[n];
                Vision.ImagesOriginal_1[n] = theImage;
                
            }
            try
            {
                result.Barcode = sBarcode;
            }
            catch
            {
            }
            Vision.VisionBarcodeResult[n] = new Vision.BarcodeResult();
            Vision.VisionBarcodeResult[n].Barcode = result.Barcode;
            WriteLog(n, Vision.VisionResult[n], result);
        }
        public void WriteLog(int n, string ResultOK, Result result)
        {
            if (Plc.Status == 1)
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
                    string Log = Path + "\\" + Vision.FileName;
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
                            "\tOperatorID\tMachine No.\tTime\tCT\tResult\tLensBarcode\t" +
                        "Overall_Quality_Level\tOverall_Quality_Value\t" +
                        "Cell_Contrast_Level\tCell_Contrast_Value" +
                        "Print_Growth_Level\tPrint_Growth_Value\t" +
                        "Unused_Error_Correction_Level\tUnused_Error_Correction_Value\t" +
                        "Cell_Modulation_Level\tCell_Modulation_Value\t" +
                        "Fixed_Pattern_Damage_Level\tFixed_Pattern_Damage_Value\t" +
                        "Grid_Nonuniformity_Level\tGrid_Nonuniformity_Value\t" +
                        "Decode_Level\tDecode_Value"+
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
                                     result.Barcode + "\t" +
                                     result.Overall_Quality[0] + "\t" +
                                     result.Overall_Quality[1] + "\t" +
                                     result.Cell_Contrast[0] + "\t" +
                                     result.Cell_Contrast[1] + "\t" +
                                     result.Print_Growth[0] + "\t" +
                                     result.Print_Growth[1] + "\t" +
                                     result.Unused_Error_Correction[0] + "\t" +
                                     result.Unused_Error_Correction[1] + "\t" +
                                     result.Cell_Modulation[0] + "\t" +
                                     result.Cell_Modulation[1] + "\t" +
                                     result.Fixed_Pattern_Damage[0] + "\t" +
                                     result.Fixed_Pattern_Damage[1] + "\t" +
                                     result.Grid_Nonuniformity[0] + "\t" +
                                     result.Grid_Nonuniformity[1] + "\t" +
                                     result.Decode[0] + "\t" +
                                     result.Decode[1]);
                    }
                }
                catch
                {
                }

            }
        }

        public object LevelConversion(int result)
        {
            object Level = "F";
            switch (result)
            {
                case 4:
                        Level = "A"; break;
                case 3:
                    Level = "B"; break;
                case 2:
                    Level = "C"; break;
                case 1:
                    Level = "D"; break;
                default:
                    Level = "F"; break;
            }
            return Level;
        }

        private void cmbProduction_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReader.Production = cmbProduction.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "Production", MyBarcodeReader.Production.ToString(), Path);
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                btnContinueShot.BackColor = Color.Green;
                btnContinueShot.Text = "停止";
            }
            else
            {
                btnContinueShot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                btnContinueShot.Text = "預覽";
            }
        }

        #region 光源設置
        public void LoadSettingLight()
        {
            nudLightSet_1.Value = Convert.ToInt32(Light.LightSet_1);
            nudLightSet_2.Value = Convert.ToInt32(Light.LightSet_2);
            nudLightSet_3.Value = Convert.ToInt32(Light.LightSet_3);
            nudLightSet_4.Value = Convert.ToInt32(Light.LightSet_4);
        }

        private void tbLightSet_1_ValueChanged(object sender, EventArgs e)
        {
            nudLightSet_1.Value = tbLightSet_1.Value;
        }

        private void nudLightSet_1_ValueChanged(object sender, EventArgs e)
        {
            tbLightSet_1.Value = Convert.ToInt32(nudLightSet_1.Value);
            Light.LightSet_1 = ((int)tbLightSet_1.Value);
            try
            {
                //這段要另外用的
                LightSetting(1 - 1, tbLightSet_1.Value);
            }
            catch
            {
            }
        }

        private void tbLightSet_2_ValueChanged(object sender, EventArgs e)
        {
            nudLightSet_2.Value = tbLightSet_2.Value;
        }

        private void nudLightSet_2_ValueChanged(object sender, EventArgs e)
        {
            tbLightSet_2.Value = Convert.ToInt32(nudLightSet_2.Value);
            Light.LightSet_2 = ((int)tbLightSet_2.Value);
            try
            {
                //這段要另外用的
                LightSetting(2 - 1, tbLightSet_2.Value);
            }
            catch
            {
            }
        }

        private void tbLightSet_3_ValueChanged(object sender, EventArgs e)
        {
            nudLightSet_3.Value = tbLightSet_3.Value;
        }

        private void nudLightSet_3_ValueChanged(object sender, EventArgs e)
        {
            tbLightSet_3.Value = Convert.ToInt32(nudLightSet_3.Value);
            Light.LightSet_3 = ((int)tbLightSet_3.Value);
            try
            {
                //這段要另外用的
                LightSetting(3 - 1, tbLightSet_3.Value);
            }
            catch
            {
            }
        }

        private void tbLightSet_4_ValueChanged(object sender, EventArgs e)
        {
            nudLightSet_4.Value = tbLightSet_4.Value;
        }

        private void nudLightSet_4_ValueChanged(object sender, EventArgs e)
        {
            tbLightSet_4.Value = Convert.ToInt32(nudLightSet_4.Value);
            Light.LightSet_4 = ((int)tbLightSet_4.Value);
            try
            {
                //這段要另外用的
                LightSetting(4 - 1, tbLightSet_4.Value);
            }
            catch
            {
            }
        }

        private void btnOn_1_Click(object sender, EventArgs e)
        {
            ReverseOnOff(btnOn_1);
        }

        private void btnOn_2_Click(object sender, EventArgs e)
        {
            ReverseOnOff(btnOn_2);
        }

        private void btnOn_3_Click(object sender, EventArgs e)
        {
            ReverseOnOff(btnOn_3);
        }

        private void btnOn_4_Click(object sender, EventArgs e)
        {
            ReverseOnOff(btnOn_4);
        }

        private void btnLightSave_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("SettingLight", "Light1", tbLightSet_1.Value.ToString(), Path);
            IniFile.Write("SettingLight", "Light2", tbLightSet_2.Value.ToString(), Path);
            IniFile.Write("SettingLight", "Light3", tbLightSet_3.Value.ToString(), Path);
            IniFile.Write("SettingLight", "Light4", tbLightSet_4.Value.ToString(), Path);
        }

        public void LightSetting(int ch, int brit)
        {
            try
            {
                byte[] cmd = Lighter.SetBrit(ch, brit);
                //ShowCmd(cmd);
                parent.com1.Write(cmd, 0, 8);
            }
            catch
            {
            }
        }

        public void ReverseOnOff(Button sender)
        {
            string caption = sender.Name;
            int ch = int.Parse(caption.Substring(6)) - 1;
            bool flag = false;
            if (sender.Text == "打开")
            {
                flag = true;
                sender.Text = "关闭";
            }
            else
            {
                flag = false;
                sender.Text = "打开";
            }
            byte[] cmd = Lighter.SetOnOff(ch, flag);
            parent.com1.Write(cmd, 0, cmd.Length);
            //ShowCmd(cmd);
        }
        #endregion

        private void cmbOkAddition_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReader.OkAddition = cmbOkAddition.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "OkAddition", MyBarcodeReader.OkAddition.ToString(), Path);
        }

        private void cbTransformOpen_CheckedChanged(object sender, EventArgs e)
        {

        }

        
        private void hWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            if (cbTransformOpen.Checked)
                parent.hWindowControl1_HMouseDown(hWindowControl1.HalconWindow);
        }

        private void hWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {
            if (cbTransformOpen.Checked)
                parent.hWindowControl1_HMouseUp(hWindowControl1.HalconWindow);
        }

        private void hWindowControl1_HMouseWheel(object sender, HMouseEventArgs e)
        {
            if (cbTransformOpen.Checked)
                parent.hWindowControl1_HMouseWheel(hWindowControl1.HalconWindow, e);
        }

        private void btnShowOriginalImage_Click(object sender, EventArgs e)
        {
            parent.ShowOriginalImage(My.ho_Image, hWindowControl1.HalconWindow);
        }
    }
}
