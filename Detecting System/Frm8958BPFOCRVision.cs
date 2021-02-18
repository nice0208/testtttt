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
using System.Threading;

namespace Detecting_System
{
    public partial class Frm8958BPFOCRVision : Form
    {
        FrmParent parent;
        FrmRun Run;
        bool bValueChange=false;
        public Frm8958BPFOCRVision(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;
            InitializeComponent();
        }
        private void Frm8958BPFOCRVision_Load(object sender, EventArgs e)
        {
            ReadPara();
            TimeUI.Enabled = true;
        }
        public void ReadPara()
        {
            bValueChange = true;
            LoadSettingLight();
            CCDSetPara();
            VisionPara();
            bValueChange = false;
        }
        #region 单拍
        private void btnOneShot_Click(object sender, EventArgs e)
        {
            parent.OneShot();
        }
        #endregion
        #region 连续拍摄
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
        #endregion
        #region 检测
        private void btnImageProPlus_Click(object sender, EventArgs e)
        {
           
            OCRDiscern();
        }
        #endregion
        #region 保存图片
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
            //string UpLoadpath = Sys.ImageSavePath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Result";
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
            ////建立上傳圖片資料夾
            //if (!Directory.Exists(UpLoadpath))
            //{
            //    Directory.CreateDirectory(UpLoadpath);
            //}
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
                    HOperatorSet.WriteImage(MyLensCrack_AVI.ResultImage, "png", 0, Resultpath + "\\" + Namepath + "_OK_" + Time);
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
                    HOperatorSet.WriteImage(MyLensCrack_AVI.ResultImage, "png", 0, Resultpath + "\\" + Namepath + "_NG_" + Time);
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
                    HOperatorSet.WriteImage(MyLensCrack_AVI.ResultImage, "png", 0, Resultpath + "\\" + Namepath + "_NG2_" + Time);
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
                    HOperatorSet.WriteImage(MyLensCrack_AVI.ResultImage, "png", 0, Resultpath + "\\" + Namepath + "_NG3_" + Time);
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
                    HOperatorSet.WriteImage(MyLensCrack_AVI.ResultImage, "png", 0, Resultpath + "\\" + Namepath + "_Miss_" + Time);
                    bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_Miss_" + Time + ".png", pathNG + "\\" + Namepath + "_Miss_" + Time, 360, 270, 100);
                }
            }

            if (Sys.OptionOriginal)
            {
                //儲存原始圖片
                HOperatorSet.WriteImage(My.ho_Image, "bmp", 0, Originalpath + "\\" + Namepath + Time);
            }
        }
        #endregion
        #region 打开图片
        private void btnOpenImg_Click(object sender, EventArgs e)
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
                readImage = ReadPicture(hWindowControl1.getHWindowControl().HalconWindow, ImagePath);

                HOperatorSet.CopyImage(readImage, out My.ho_Image);
                // 读取这张图片并将图片赋值给readImage,这句就是直接调的halcon类了，下边public定义的的是他的类
                hWindowControl1.HobjectToHimage(My.ho_Image);
            }

        }
        public HObject ReadPicture(HWindow window, string ImagePath)
        {
            // 得到图片显示的窗口句柄
            HObject ho_Image = new HObject();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_Image);
            ho_Image.Dispose();
            HOperatorSet.ReadImage(out ho_Image, ImagePath); //从这个路径读取图片

            {//旋轉0度
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

            //HOperatorSet.SetWindowAttr("background_color", "black");
            ////调整窗口显示大小以适应图片，这句一定要加上，不然图片显示的不全
            //HOperatorSet.SetPart(window, 0, 0, hv_Height - 1, hv_Width - 1);
            //HOperatorSet.DispObj(ho_Image, window); //将图像在该窗口进行显示
            return ho_Image; //返回这个图像
        }
        #endregion
        #region 视觉部分
        #region Halcon全局变量
        /// <summary>
        /// 图片处理半径
        /// </summary>
        int dReduceRadius = 0;
        /// <summary>
        /// 灰度上限
        /// </summary>
        int dGraythresholdUp = 255;
        /// <summary>
        /// 灰度下限
        /// </summary>
        int dGraythresholdDown = 0;
        /// <summary>
        /// 面积上限
        /// </summary>
        int dAreaUp = 0;
        /// <summary>
        /// 面积下限
        /// </summary>
        int dAreaDown = 0;
        /// <summary>
        /// 长度1上限
        /// </summary>
        int dLength1Up = 0;
        /// <summary>
        /// 长度1下限
        /// </summary>
        int dLength1Down = 0;
        /// <summary>
        /// 长度2上限
        /// </summary>
        int dLength2Up = 0;
        /// <summary>
        /// 长度2下限
        /// </summary>
        int dLength2Down = 0;
        /// <summary>
        /// 边阈值
        /// </summary>
        int dthreshold = 30;
        /// <summary>
        /// 卡尺数量
        /// </summary>
        int dcliperNum = 20;
        /// <summary>
        /// 卡尺长
        /// </summary>
        int dcliperlength = 80;
        /// <summary>
        /// 卡尺宽
        /// </summary>
        int  dcliperwidth = 80;
        /// <summary>
        /// 极性 黑找白或白找黑
        /// </summary>
        string spolarity = "positive";    
        string sedgeSelect = "first";
        /// <summary>
        /// 字符到中心距离
        /// </summary>
        int dOCRCenterDistance = 850;
        /// <summary>
        /// 字符矩形长度
        /// </summary>
        int dOCRRectangleLength = 80;
        /// <summary>
        /// 字符矩形宽度
        /// </summary>
        int dOCRRectangleWidth = 80;
        /// <summary>
        /// OCR字符灰度分割上限
        /// </summary>
        int dOCRthresholdUp = 255;
        /// <summary>
        /// OCR字符灰度分割下限
        /// </summary>
        int dOCRthresholdDown = 0;
        /// <summary>
        /// OCR字符宽度上限
        /// </summary>
        int dOCRWidthUp = 100;
        /// <summary>
        /// OCR字符宽度下限
        /// </summary>
        int dOCRWidthDown = 0;
        /// <summary>
        ///  OCR字符高度上限
        /// </summary>
        int dOCRHeightUp = 100;
        /// <summary>
        /// OCR字符高度下限
        /// </summary>
        int dOCRHeightDown = 0;
        int dMeanfilte = 40;
        //数据传递
        HTuple InitialRect2Row = 0;
        HTuple InitialRect2Col = 0;
        HTuple InitialRect2Phi = 0;
        HTuple InitialRect2Length1 = 0;
        HTuple InitialRect2Length2 = 0;
        HTuple ResultRect2Row = 0;
        HTuple ResultRect2Col = 0;
        HTuple ResultRect2Phi = 0;
        HTuple ResultRect2Length1 = 0;
        HTuple ResultRect2Length2 = 0;
        bool binGegion = false;
        int iThresholdSelect = 0;   //0:dynthreshol  1:thresholdUp
        int dCloseWidth = 1;
        int dCloseHeight = 1;
        int dOpenWidth = 1;
        int dOpenHeight = 1;
        bool bClosing = false;
        bool bOpenging = false;
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
            HOperatorSet.QueryFont(hv_WindowHandle, out hv_AvailableFonts);
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
            HOperatorSet.SetFont(hv_WindowHandle, hv_Font_COPY_INP_TMP);
            // dev_set_preferences(...); only in hdevelop

            return;
        }
        #endregion
        #region Vision参数读取
        public void VisionPara()
        {
            UCDetectionRadius.Value = MyLens_Mold_Cave.dReduceRadius;
            UCGraythresholdUp.Value = MyLens_Mold_Cave.dGraythresholdUp;
            UCGraythresholdDown.Value = MyLens_Mold_Cave.dGraythresholdDown;
            UCAreaUp.Value = MyLens_Mold_Cave.dAreaUp;
            UCAreaDown.Value = MyLens_Mold_Cave.dAreaDown;
            UCLength1Up.Value = MyLens_Mold_Cave.dLength1Up;
            UCLength1Down.Value = MyLens_Mold_Cave.dLength1Down;
            UCLength2Up.Value = MyLens_Mold_Cave.dLength2Up;
            UCLength2Down.Value = MyLens_Mold_Cave.dLength2Down;
            UCthreshold.Value = MyLens_Mold_Cave.dthreshold;
            UCcliperNum.Value = MyLens_Mold_Cave.dcliperNum;
            UCcliperlength.Value = MyLens_Mold_Cave.dcliperlength;
            UCcliperWidth.Value = MyLens_Mold_Cave.dcliperwidth;
            spolarity = MyLens_Mold_Cave.spolarity;

            switch (spolarity)
            {
                case "positive": cbPolarity.SelectedIndex = 0; break;
                case "negative": cbPolarity.SelectedIndex = 1; break;

            }
            sedgeSelect = MyLens_Mold_Cave.sedgeSelect;
            switch (sedgeSelect)
            {
                case "first": cbEdgeSelect.SelectedIndex = 0; break;
                case "last": cbEdgeSelect.SelectedIndex = 1; break;
                case "all": cbEdgeSelect.SelectedIndex = 2; break;

            }
            UCCenterDistance.Value =MyLens_Mold_Cave.dOCRCenterDistance;
            UCRectangleLength.Value =MyLens_Mold_Cave.dOCRRectangleLength;
            UCRectangleWidth.Value=MyLens_Mold_Cave.dOCRRectangleWidth;
            UCOCRthresholdUp.Value =MyLens_Mold_Cave.dOCRthresholdUp;
            UCOCRthresholdDown.Value=MyLens_Mold_Cave.dOCRthresholdDown;
            UCOCRWidthUp.Value =MyLens_Mold_Cave.dOCRWidthUp;
            UCOCRWidthDown.Value=MyLens_Mold_Cave.dOCRWidthDown;
            UCOCRHeightUp.Value =MyLens_Mold_Cave.dOCRHeightUp;
            UCOCRHeightDown.Value =MyLens_Mold_Cave.dOCRHeightDown ;
            binGegion = MyLens_Mold_Cave.binGegion;
            UCMeanfilter.Value = MyLens_Mold_Cave.dMeanfilte;
            if (binGegion)
            {
                rbtnLightOnDark.Checked = true;
           

            }
            else
            {
                rbtnDarkOnLight.Checked = true;
            }
            iThresholdSelect = MyLens_Mold_Cave.iThresholdSelect;
            if (iThresholdSelect == 0)
            {
                cbDynthreshold.Checked = true;
                cbthreshold.Checked = false; 

            }
            else
            {
                cbDynthreshold.Checked = false;
                cbthreshold.Checked = true; 
 
            }
            nudCloseWidth.Value = MyLens_Mold_Cave.dCloseWidth;
            nudCloseHeight.Value = MyLens_Mold_Cave.dCloseHeight;
            nudOpenWidth.Value = MyLens_Mold_Cave.dOpenWidth;
            nudOpenHeight.Value = MyLens_Mold_Cave.dOpenHeight;
            if (MyLens_Mold_Cave.bClosing)
            {
                cbClosing.Checked = true;
            }
            else
            {
                cbClosing.Checked = false;
 
            }
            if (MyLens_Mold_Cave.bOpenging)
            {
                cbOpening.Checked = true;
            }
            else
            {
                cbOpening.Checked = false;

            }

        }
        #endregion
        #region  图像检测区域减小
        private void UCDetectionRadius_ValueChanged(int CurrentValue)
        {
            dReduceRadius = UCDetectionRadius.Value;
            if (bValueChange)
                return;
            try
            {
                HObject ho_Image = null;
                HObject ho_Circle = null;
                HObject ho_ReducedImage = null;
                HOperatorSet.GenEmptyObj(out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
                HTuple hv_Width;
                HTuple hv_Height;
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
                hWindowControl1.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, dReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                hWindowControl1.HobjectToHimage(ho_ReducedImage);
                Thread.Sleep(1);
            }
            catch
            {
  
            }

        }
        #endregion
        #region  阈值处理获取矩形区域
        private void UCGraythresholdUp_ValueChanged(int CurrentValue)
        {
            dGraythresholdUp = UCGraythresholdUp.Value;
            if (bValueChange)
                return;
            if (dGraythresholdUp < dGraythresholdDown)
            {
                UCGraythresholdDown.Value = dGraythresholdUp;

            }
            GetInitialRectangle2();

          
            
            

            


        }
        private void UCGraythresholdDown_ValueChanged(int CurrentValue)
        {
            dGraythresholdDown = UCGraythresholdDown.Value;
            if (bValueChange)
                return;
            if(dGraythresholdUp < dGraythresholdDown)
            {
                UCGraythresholdUp.Value = dGraythresholdDown;

            }
            GetInitialRectangle2();
        }
        private void UCAreaUp_ValueChanged(int CurrentValue)
        {
            dAreaUp = UCAreaUp.Value;
            if (bValueChange)
                return;
            if (dAreaUp < dAreaDown)
            {
                UCAreaDown.Value = dAreaUp;

            }
            GetInitialRectangle2();

        }

        private void UCAreaDown_ValueChanged(int CurrentValue)
        {
            dAreaDown = UCAreaDown.Value;
            if (bValueChange)
                return;
            if (dAreaUp < dAreaDown)
            {
                UCAreaUp.Value = dAreaDown;

            }
            GetInitialRectangle2();

        }
        private void UCLength1Up_ValueChanged(int CurrentValue)
        {
            dLength1Up = UCLength1Up.Value;
            if (bValueChange)
                return;
            if (dLength1Up < dLength1Down)
            {
                UCLength1Down.Value = dLength1Up;

            }
            GetInitialRectangle2();


        }

        private void UCLength1Down_ValueChanged(int CurrentValue)
        {
            dLength1Down = UCLength1Down.Value;
            if (bValueChange)
                return;
            if (dLength1Up < dLength1Down)
            {
                UCLength1Up.Value = dLength1Down;

            }
            GetInitialRectangle2();


        }

        private void UCLength2Up_ValueChanged(int CurrentValue)
        {
            dLength2Up = UCLength2Up.Value;
            if (bValueChange)
                return;
            if (dLength2Up < dLength2Down)
            {
                UCLength2Down.Value = dLength2Up;

            }
            GetInitialRectangle2();
        }

        private void UCLength2Down_ValueChanged(int CurrentValue)
        {
            dLength2Down = UCLength2Down.Value;
            if (bValueChange)
                return;
            if (dLength2Up < dLength2Down)
            {
                UCLength2Up.Value = dLength2Down;

            }
            GetInitialRectangle2();
        }
        private void btnGetRectangle2_Click(object sender, EventArgs e)
        {
            MyLens_Mold_Cave.bShowMessage = true;
            GetInitialRectangle2();
        }
        public void GetInitialRectangle2()
        {

                HObject ho_Image = null;
                HObject ho_Circle = null;
                HObject ho_ReducedImage = null;
                HObject ho_ImageMedian = null;
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
                HOperatorSet.GenEmptyObj(out ho_Regions);
                HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
                HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                HOperatorSet.GenEmptyObj(out ho_RegionTrans);
                HOperatorSet.GenEmptyObj(out ho_InitialReectangle2);
                HTuple hv_Width;
                HTuple hv_Height;
                HTuple hv_Row;
                HTuple hv_Column;
                HTuple hv_Phi;
                HTuple hv_Length1;
                HTuple hv_Length2;
                HTuple hv_area;
        try
        {
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hWindowControl1.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, dReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 7, 7);
                ho_Regions.Dispose();
                HOperatorSet.Threshold(ho_ImageMedian, out ho_Regions, dGraythresholdDown, dGraythresholdUp);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_Regions, out ho_RegionFillUp);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, ((new HTuple("area")).TupleConcat(
     "rect2_len1")).TupleConcat("rect2_len2"), "and", ((new HTuple(dAreaDown)).TupleConcat(
     dLength1Down)).TupleConcat(dLength2Down), ((new HTuple(dAreaUp)).TupleConcat(dLength1Up)).TupleConcat(
     dLength2Up));
                ho_RegionTrans.Dispose();
                //HOperatorSet.ShapeTrans(ho_SelectedRegions, out ho_RegionTrans, "rectangle1");
                hWindowControl1.HobjectToHimage(ho_Image);
                hWindowControl1.DispObj(ho_SelectedRegions, "red");
                HOperatorSet.SmallestRectangle2(ho_SelectedRegions, out hv_Row, out hv_Column, out hv_Phi,
                    out hv_Length1, out hv_Length2);
                ho_InitialReectangle2.Dispose();                 
                HOperatorSet.GenRectangle2(out ho_InitialReectangle2, hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2);
                InitialRect2Row = hv_Row;
                InitialRect2Col = hv_Column;
                InitialRect2Phi = hv_Phi;
                InitialRect2Length1 = hv_Length1;
                InitialRect2Length2 = hv_Length2;
                if (MyLens_Mold_Cave.bShowMessage)
                {                   
                    MyLens_Mold_Cave.bShowMessage = false;
                    
                    HOperatorSet.AreaCenter(ho_InitialReectangle2, out hv_area, out hv_Row, out hv_Column);
                    hWindowControl1.DispObj(ho_InitialReectangle2, "blue");
                    HOperatorSet.SetColor(hWindowControl1.getHWindowControl().HalconWindow, "green");
                    HOperatorSet.SetTposition(hWindowControl1.getHWindowControl().HalconWindow, hv_Row, hv_Column);
                    HOperatorSet.WriteString(hWindowControl1.getHWindowControl().HalconWindow, "面积:"+hv_area);
                    HOperatorSet.SetTposition(hWindowControl1.getHWindowControl().HalconWindow, hv_Row + 30, hv_Column);
                    HOperatorSet.WriteString(hWindowControl1.getHWindowControl().HalconWindow, "长 :" + hv_Length1);
                    HOperatorSet.SetTposition(hWindowControl1.getHWindowControl().HalconWindow, hv_Row + 60, hv_Column);
                    HOperatorSet.WriteString(hWindowControl1.getHWindowControl().HalconWindow, "宽 :" + hv_Length2);
                }
           
            }
            catch
            {
                MyLens_Mold_Cave.bShowMessage = false;
 
            }
           ho_Image.Dispose();
           ho_Circle.Dispose();
           ho_ReducedImage.Dispose();
           ho_ImageMedian.Dispose();
           ho_Regions.Dispose();
           ho_RegionFillUp.Dispose();
           ho_ConnectedRegions.Dispose();
           ho_SelectedRegions.Dispose();
           ho_RegionTrans.Dispose();
           ho_InitialReectangle2.Dispose();
        }
 
        
        #endregion
        #region  根据矩形获取中心
        private void UCthreshold_ValueChanged(int CurrentValue)
        {
            dthreshold = UCthreshold.Value;
            if (bValueChange)
                return;
            GetRectangle2Center();
            
        }

        private void UCcliperNum_ValueChanged(int CurrentValue)
        {
            dcliperNum = UCcliperNum.Value;
            if (bValueChange)
                return;
            GetRectangle2Center();
        }

        private void UCcliperlength_ValueChanged(int CurrentValue)
        {
            dcliperlength = UCcliperlength.Value;
            if (bValueChange)
                return;
            GetRectangle2Center();
        }

        private void UCcliperWidth_ValueChanged(int CurrentValue)
        {

            dcliperwidth = UCcliperWidth.Value;
            if (bValueChange)
                return;
            GetRectangle2Center();
        }

        private void cbPolarity_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbPolarity.SelectedIndex)
            {
                case 0: spolarity = "positive"; break;
                case 1: spolarity = "negative"; break;
                default: spolarity = "positive"; break;

            }
            if (bValueChange)
                return;
            GetRectangle2Center();
                
        }
        private void cbEdgeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbPolarity.SelectedIndex)
            {
                case 0: sedgeSelect = "first"; break;
                case 1: sedgeSelect = "last"; break;
                case 2: sedgeSelect = "all"; break;
                default: sedgeSelect = "first"; break;

            }
            if (bValueChange)
                return;
            GetRectangle2Center();
        }
        #endregion
        public void GetRectangle2Center()
        {
            try
            {
                if (My.ho_Image == null)
                    return;
                HObject ho_Image = null;
                HObject ho_Circle = null;
                HObject ho_ReducedImage = null;
                HObject ho_ImageMedian = null; 
         
                HOperatorSet.GenEmptyObj(out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
                HOperatorSet.GenEmptyObj(out ho_ImageMedian);
                //
                HObject ho_Contour= null;
                HObject ho_UsedEdges= null;
                HObject ho_ResultContours= null; 
                HObject ho_CrossCenter= null; 
                HOperatorSet.GenEmptyObj(out ho_Contour);
                HOperatorSet.GenEmptyObj(out ho_UsedEdges);
                HOperatorSet.GenEmptyObj(out ho_ResultContours);

                HTuple hv_Width;
                HTuple hv_Height;
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hWindowControl1.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, dReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 7, 7);
                gen_rectangle2_center(ho_ImageMedian, out ho_Contour, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, 
                    InitialRect2Row, InitialRect2Col, InitialRect2Phi, InitialRect2Length1, InitialRect2Length2, dcliperlength, dthreshold, spolarity, sedgeSelect, 
                    out ResultRect2Row, out ResultRect2Col, out ResultRect2Phi, out ResultRect2Length1, out ResultRect2Length2);
                hWindowControl1.HobjectToHimage(ho_Image);
                hWindowControl1.DispObj(ho_Contour, "green");
                //hWindowControl1.DispObj(ho_UsedEdges, "bule");
                HOperatorSet.SetColor(hWindowControl1.HWindowHalconID, "blue");
                HOperatorSet.DispObj(ho_UsedEdges, hWindowControl1.HWindowHalconID);        
                hWindowControl1.DispObj(ho_CrossCenter, "red");
                hWindowControl1.DispObj(ho_ResultContours, "yellow");
            }
            catch
            {
               
 
            }


        }

        public void gen_rectangle2_center(HObject ho_Image, out HObject ho_Contour, out HObject ho_UsedEdges,
    out HObject ho_ResultContours, out HObject ho_CrossCenter, HTuple hv_InitialRow,
    HTuple hv_InitialColumn, HTuple hv_InitialPhi, HTuple hv_InitialLength1, HTuple hv_InitialLength2,
    HTuple hv_MeasureLength, HTuple hv_MeasureThreshold, HTuple hv_MeasureTransition,
    HTuple hv_MeasureSelect, out HTuple hv_ResultRow, out HTuple hv_ResultColumn,
    out HTuple hv_ResultPhi, out HTuple hv_ResultLength1, out HTuple hv_ResultLength2)
        {




            // Local iconic variables 

            HObject ho_ModelContour, ho_Contours;

            // Local control variables 

            HTuple hv_MetrologyHandle = null, hv_circleIndices = null;
            HTuple hv_circleParameter = null, hv_Row = null, hv_Column = null;
            HTuple hv_UsedRow = null, hv_UsedColumn = null, hv_PointOrder1 = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            HOperatorSet.GenEmptyObj(out ho_ModelContour);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
            //創建方形索引區域
            HOperatorSet.AddMetrologyObjectGeneric(hv_MetrologyHandle, "rectangle2", ((((((hv_InitialRow.TupleConcat(
                hv_InitialColumn))).TupleConcat(hv_InitialPhi))).TupleConcat(hv_InitialLength1))).TupleConcat(
                hv_InitialLength2), hv_MeasureLength, 5, 1, hv_MeasureThreshold, new HTuple(),
                new HTuple(), out hv_circleIndices);
            ho_ModelContour.Dispose();
            HOperatorSet.GetMetrologyObjectModelContour(out ho_ModelContour, hv_MetrologyHandle,
                "all", 1.5);
            //第一個點或最後一個點
            HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices, "measure_select",
                hv_MeasureSelect);
            HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices, "min_score",
                0.2);
            HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
            HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_circleIndices, "all",
                "result_type", "all_param", out hv_circleParameter);
            //白找黑('negative')或黑找白('positive')
            ho_Contour.Dispose();
            HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour, hv_MetrologyHandle, "all",
                hv_MeasureTransition, out hv_Row, out hv_Column);
            ho_Contours.Dispose();
            HOperatorSet.GetMetrologyObjectResultContour(out ho_Contours, hv_MetrologyHandle,
                "all", "all", 1.5);
            HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "used_edges",
                "row", out hv_UsedRow);
            HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "used_edges",
                "column", out hv_UsedColumn);
            ho_UsedEdges.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_UsedEdges, hv_UsedRow, hv_UsedColumn,
                10, (new HTuple(45)).TupleRad());
            ho_ResultContours.Dispose();
            HOperatorSet.GetMetrologyObjectResultContour(out ho_ResultContours, hv_MetrologyHandle,
                "all", "all", 1.5);
            HOperatorSet.FitRectangle2ContourXld(ho_ResultContours, "regression", -1, 0,
                0, 3, 2, out hv_ResultRow, out hv_ResultColumn, out hv_ResultPhi, out hv_ResultLength1,
                out hv_ResultLength2, out hv_PointOrder1);
            ho_CrossCenter.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_CrossCenter, hv_ResultRow, hv_ResultColumn,
                50, hv_ResultPhi);
            HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
            ho_ModelContour.Dispose();
            ho_Contours.Dispose();

            return;
        }
        #region OCR设置
        private void UCCenterDistance_ValueChanged(int CurrentValue)
        {

            dOCRCenterDistance = UCCenterDistance.Value;
            if (bValueChange)
                return;
            SHowOCRregion();
        }

        private void UCRectangleLength_ValueChanged(int CurrentValue)
        {
            dOCRRectangleLength = UCRectangleLength.Value;
            if (bValueChange)
                return;
            SHowOCRregion();
        }

        private void UCRectangleWidth_ValueChanged(int CurrentValue)
        {
            dOCRRectangleWidth = UCRectangleWidth.Value;
            if (bValueChange)
                return;
            SHowOCRregion();

        }

        private void UCOCRthresholdUp_ValueChanged(int CurrentValue)
        {
            dOCRthresholdUp = UCOCRthresholdUp.Value;
            if (bValueChange)
                return;
            if (dOCRthresholdUp < dOCRthresholdDown)
            {
                UCOCRthresholdDown.Value = dOCRthresholdUp;
            }
            SHowOCRSelectRegionn();
        }

        private void UCOCRthresholdDown_ValueChanged(int CurrentValue)
        {
            dOCRthresholdDown = UCOCRthresholdDown.Value;
            if (bValueChange)
                return;
            if (dOCRthresholdUp < dOCRthresholdDown)
            {
                UCOCRthresholdUp.Value = dOCRthresholdDown;
            }
            SHowOCRSelectRegionn();


        }

        private void UCOCRWidthUp_ValueChanged(int CurrentValue)
        {
            dOCRWidthUp = UCOCRWidthUp.Value;
            if (bValueChange)
                return;
            if (dOCRWidthUp < dOCRWidthDown)
            {
                UCOCRWidthDown.Value = dOCRWidthUp;
            }
            SHowOCRSelectRegionn();
        }

        private void UCOCRWidthDown_ValueChanged(int CurrentValue)
        {
            dOCRWidthDown = UCOCRWidthDown.Value;
            if (bValueChange)
                return;
            if (dOCRWidthUp < dOCRWidthDown)
            {
                UCOCRWidthUp.Value = dOCRWidthDown;
            }
            SHowOCRSelectRegionn();


        }

        private void UCOCRHeightUp_ValueChanged(int CurrentValue)
        {
            dOCRHeightUp = UCOCRHeightUp.Value;
            if (bValueChange)
                return;
            if (dOCRHeightUp < dOCRHeightDown)
            {
                UCOCRHeightDown.Value = dOCRHeightUp;

            }
            SHowOCRSelectRegionn();
        }

        private void UCOCRHeightDown_ValueChanged(int CurrentValue)
        {
            dOCRHeightDown = UCOCRHeightDown.Value;
            if (bValueChange)
                return;
            if (dOCRHeightUp < dOCRHeightDown)
            {
                UCOCRHeightUp.Value = dOCRHeightDown;

            }
            SHowOCRSelectRegionn();
        }
        private void rbtnDarkOnLight_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rbtnDarkOnLight.Checked)
            binGegion = false;

        }

        private void rbtnLightOnDark_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rbtnLightOnDark.Checked)
            binGegion = true;
        }
        private void cbDynthreshold_CheckedChanged(object sender, EventArgs e)
        {
            if (bValueChange)
                return;
            if (cbDynthreshold.Checked)
            {
                cbDynthreshold.Checked = true;
                cbthreshold.Checked = false;
                iThresholdSelect = 0;
            }
            else
            {
                cbDynthreshold.Checked = false;
                cbthreshold.Checked = true;
                iThresholdSelect = 1;
 
            }
            SHowOCRSelectRegionn();
        }

        private void cbthreshold_CheckedChanged(object sender, EventArgs e)
        {
            if (bValueChange)
                return;
            if (cbthreshold.Checked)
            {
                cbDynthreshold.Checked = false;
                cbthreshold.Checked = true;

                iThresholdSelect = 1;
            }
            else
            {
                cbDynthreshold.Checked = true;
                cbthreshold.Checked = false;

                iThresholdSelect = 0;
 
            }
            SHowOCRSelectRegionn();
        }

        private void UCMeanfilter_ValueChanged(int CurrentValue)
        {
            dMeanfilte = UCMeanfilter.Value;
            if (bValueChange)
                return;
            SHowOCRSelectRegionn();

        }
        private void nudCloseWidth_ValueChanged(object sender, EventArgs e)
        {
            dCloseWidth = Convert.ToInt32(nudCloseWidth.Value);
            if (bValueChange)
                return;
            SHowOCRSelectRegionn();
        }

        private void nudCloseHeight_ValueChanged(object sender, EventArgs e)
        {
            dCloseHeight = Convert.ToInt32(nudCloseHeight.Value);
            if (bValueChange)
                return;
            SHowOCRSelectRegionn();

        }

        private void nudOpenWidth_ValueChanged(object sender, EventArgs e)
        {
             dOpenWidth = Convert.ToInt32(nudOpenWidth.Value);
             if (bValueChange)
                 return;
             SHowOCRSelectRegionn();
        }

        private void nudOpenHeight_ValueChanged(object sender, EventArgs e)
        {
            dOpenHeight = Convert.ToInt32(nudOpenHeight.Value);
            if (bValueChange)
                return;
            SHowOCRSelectRegionn();
        }

        private void cbClosing_CheckedChanged(object sender, EventArgs e)
        {
            if (bValueChange)
                return;
           
            if (cbClosing.Checked)
            {
                bClosing = true;
            }
            else
            {
                bClosing = false;
            }
        }

        private void cbOpening_CheckedChanged(object sender, EventArgs e)
        {
            if (bValueChange)
                return;
            SHowOCRSelectRegionn();
            if (cbOpening.Checked)
            {
                bOpenging = true;
            }
            else
            {
                bOpenging = false;
            }
        }

        public void SHowOCRregion()
        {
            if (My.ho_Image == null)
                return;
            try
            {

                HObject ho_Image = null;
                HObject ho_Circle = null;
                HObject ho_ReducedImage = null;
                HObject ho_ImageMedian = null;
                HObject InvertImage = null;
                HOperatorSet.GenEmptyObj(out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
                HOperatorSet.GenEmptyObj(out ho_ImageMedian);
                HOperatorSet.GenEmptyObj(out InvertImage);
                HTuple hv_Width;
                HTuple hv_Height;
                //
                HObject ho_Rectangle = null;
                HObject ho_Rectangle1 = null;
                HObject ho_RegionUnion = null;
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_Rectangle1);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);

                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hWindowControl1.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, dReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 7, 7);
          
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, ResultRect2Row + dOCRCenterDistance, ResultRect2Col,
                    0, dOCRRectangleLength, dOCRRectangleWidth);
                ho_Rectangle1.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle1, ResultRect2Row - dOCRCenterDistance, ResultRect2Col,
                    0, dOCRRectangleLength, dOCRRectangleWidth);
                ho_RegionUnion.Dispose();
                HOperatorSet.Union2(ho_Rectangle, ho_Rectangle1, out ho_RegionUnion);
                hWindowControl1.HobjectToHimage(ho_Image);
                hWindowControl1.DispObj(ho_RegionUnion, "green");

            }
            catch
            {
            }
        }
        public void SHowOCRSelectRegionn()
        {
            if (My.ho_Image == null)
                return;
            try
            {

                HObject ho_Image = null;
                HObject ho_Circle = null;
                HObject ho_ReducedImage = null;
                HObject ho_ImageMedian = null;
                HObject ho_ImageEmphasize = null;
                HOperatorSet.GenEmptyObj(out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
                HOperatorSet.GenEmptyObj(out ho_ImageMedian);
                HOperatorSet.GenEmptyObj(out ho_ImageEmphasize);
                HTuple hv_Width;
                HTuple hv_Height;
                //
                HObject ho_Rectangle = null;
                HObject ho_Rectangle1 = null;
                HObject ho_RegionUnion = null;
                HObject ho_ImageReduced = null;
                HObject ho_Regions1 = null;
                HObject ho_ConnectedRegions1 = null;
                HObject ho_SelectedRegions1 = null;
                HObject ho_RegionsClosing = null;
                HObject ho_ImageInvert = null;
                HObject ho_ImageMean = null;
                HObject ho_ImageMean1 = new HObject();
                HObject ho_ImageMean2 = new HObject();
                HObject ho_RegionDynThresh = null;
                HObject ho_RegionOpening = null;
                HObject ho_RegionFillUp = null;
                HObject ho_RegionOpeningRectangle1 = null;
                HObject ho_RegionIntersection = null;
                HObject ho_RegionClosing= null;
                HObject ho_RegionThresh = null;
                HObject ho_ImageGrayRangeRect = null;
                HObject ho_RegionClosingRectangle1 = null;
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_Rectangle1);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                HOperatorSet.GenEmptyObj(out ho_Regions1);
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
                HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
                HOperatorSet.GenEmptyObj(out ho_RegionsClosing);
                HOperatorSet.GenEmptyObj(out ho_ImageInvert);
                HOperatorSet.GenEmptyObj(out ho_ImageMean);
                HOperatorSet.GenEmptyObj(out ho_RegionDynThresh);
                HOperatorSet.GenEmptyObj(out ho_RegionOpening);
                HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
                HOperatorSet.GenEmptyObj(out ho_RegionOpeningRectangle1);
                HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
                HOperatorSet.GenEmptyObj(out ho_RegionClosing);
                HOperatorSet.GenEmptyObj(out ho_RegionThresh);
                HOperatorSet.GenEmptyObj(out ho_ImageGrayRangeRect);
                HOperatorSet.GenEmptyObj(out ho_RegionClosingRectangle1);
                HTuple hv_OCRHandle;
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hWindowControl1.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                //hWindowControl1.HobjectToHimage(ho_Image);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, dReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 7, 7);
                ho_ImageEmphasize.Dispose();
                HOperatorSet.Emphasize(ho_ReducedImage, out ho_ImageEmphasize, 7, 7,
    1);
         
                ho_ImageInvert.Dispose();
                if (binGegion)
                {
                    HOperatorSet.InvertImage(ho_ImageEmphasize, out ho_ImageInvert);
                }
                else
                {
                    HOperatorSet.CopyImage(ho_ImageEmphasize, out ho_ImageInvert);
                }
                hWindowControl1.HobjectToHimage(ho_ImageInvert);
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, ResultRect2Row + dOCRCenterDistance, ResultRect2Col,
                    0, dOCRRectangleLength, dOCRRectangleWidth);
                ho_Rectangle1.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle1, ResultRect2Row - dOCRCenterDistance, ResultRect2Col,
                    0, dOCRRectangleLength, dOCRRectangleWidth);
                ho_RegionUnion.Dispose();
                HOperatorSet.Union2(ho_Rectangle, ho_Rectangle1, out ho_RegionUnion);
                hWindowControl1.DispObj(ho_RegionUnion, "green");
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageInvert, ho_RegionUnion, out ho_ImageReduced);
                ho_RegionThresh.Dispose();
                if (iThresholdSelect == 0)
                {                   

                    HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean, dMeanfilte, dMeanfilte);
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean, out ho_RegionThresh,
                            dMeanfilte, "dark");
                }
                else
                {                
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_RegionThresh, dOCRthresholdDown, dOCRthresholdUp);
                }
                //ho_ImageMean1.Dispose();
                //HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, 3, 3);
                //ho_ImageMean2.Dispose();
                //HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean2, dMeanfilte, dMeanfilte);
                
                //ho_RegionDynThresh.Dispose();         
                //HOperatorSet.DynThreshold(ho_ImageMean2, ho_ImageMean1, out ho_RegionDynThresh,
                //        dMeanfilte-3, "dark");
                  
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionThresh, out ho_RegionFillUp);
                if (bClosing)
                {

                    ho_RegionClosingRectangle1.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionThresh, out ho_RegionClosingRectangle1, dCloseWidth, dCloseHeight);
                    ho_RegionFillUp.Dispose();
                    ho_RegionFillUp = ho_RegionClosingRectangle1;
                    
                }
                if (bOpenging)
                {
                    ho_RegionOpeningRectangle1.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionFillUp, out ho_RegionOpeningRectangle1, dOpenWidth, dOpenHeight);
                    ho_RegionFillUp.Dispose();
                    ho_RegionFillUp = ho_RegionOpeningRectangle1;
                }

                ho_ConnectedRegions1.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions1);
             
                ho_SelectedRegions1.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions1, ((new HTuple("width")).TupleConcat(
    "height")), "and", ((new HTuple(dOCRWidthDown)).TupleConcat(
    dOCRHeightDown)), ((new HTuple(dOCRWidthUp)).TupleConcat(dOCRHeightUp)));            
                
                hWindowControl1.DispObj(ho_SelectedRegions1, "red");




            }
            catch
            {

            }


        }
        public void OCRDiscern()
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
            HObject ho_ImageMean1 = null;
            HObject ho_ImageMean2 = null;
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
            HOperatorSet.GenEmptyObj(out ho_ImageMean1);
            HOperatorSet.GenEmptyObj(out ho_ImageMean2);
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
            try
            {
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hWindowControl1.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                hWindowControl1.HobjectToHimage(ho_Image);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, dReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 7, 7);
                ho_ImageEmphasize.Dispose();
                HOperatorSet.Emphasize(ho_ReducedImage, out ho_ImageEmphasize, 7, 7,
    1); 
                ho_ImageInvert.Dispose();
                if (binGegion)
                {
                    HOperatorSet.InvertImage(ho_ImageEmphasize, out ho_ImageInvert);
                }
                else
                {
                    HOperatorSet.CopyImage(ho_ImageEmphasize, out ho_ImageInvert);

                }
                ho_Regions.Dispose();
                HOperatorSet.Threshold(ho_ImageMedian, out ho_Regions, dGraythresholdDown, dGraythresholdUp);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_Regions, out ho_RegionFillUp);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, ((new HTuple("area")).TupleConcat(
     "rect2_len1")).TupleConcat("rect2_len2"), "and", ((new HTuple(dAreaDown)).TupleConcat(
     dLength1Down)).TupleConcat(dLength2Down), ((new HTuple(dAreaUp)).TupleConcat(dLength1Up)).TupleConcat(
     dLength2Up));
                HOperatorSet.SmallestRectangle2(ho_SelectedRegions, out hv_Row, out hv_Column, out hv_Phi,
                    out hv_Length1, out hv_Length2);
                ho_InitialReectangle2.Dispose();
                HOperatorSet.GenRectangle2(out ho_InitialReectangle2, hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2);
                InitialRect2Row = hv_Row;
                InitialRect2Col = hv_Column;
                InitialRect2Phi = hv_Phi;
                InitialRect2Length1 = hv_Length1;
                InitialRect2Length2 = hv_Length2;
                gen_rectangle2_center(ho_ImageMedian, out ho_Contour, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter,
                   InitialRect2Row, InitialRect2Col, InitialRect2Phi, InitialRect2Length1, InitialRect2Length2, dcliperlength, dthreshold, spolarity, sedgeSelect,
                   out ResultRect2Row, out ResultRect2Col, out ResultRect2Phi, out ResultRect2Length1, out ResultRect2Length2);
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, ResultRect2Row + dOCRCenterDistance, ResultRect2Col,
                    0, dOCRRectangleLength, dOCRRectangleWidth);
                ho_Rectangle1.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle1, ResultRect2Row - dOCRCenterDistance, ResultRect2Col,
                    0, dOCRRectangleLength, dOCRRectangleWidth);
                ho_RegionUnion.Dispose();
               
                //上面区域处理          
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageInvert, ho_Rectangle1, out ho_ImageReduced);
                ho_Regions1.Dispose();
                //ho_RegionDynThresh.Dispose();
                //HOperatorSet.Threshold(ho_ImageReduced, out ho_RegionDynThresh, dOCRthresholdDown, dOCRthresholdUp);

                //ho_ImageMean.Dispose();
                //HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean, dMeanfilte, dMeanfilte);
                //ho_ImageMean1.Dispose();
                //HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, 3, 3);
                //ho_ImageMean2.Dispose();
                //HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean2, dMeanfilte, dMeanfilte);

                ho_RegionThresh.Dispose();
                if (iThresholdSelect == 0)
                {

                    HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean, dMeanfilte, dMeanfilte);
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean, out ho_RegionThresh,
                            dMeanfilte, "dark");
                }
                else
                {
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_RegionThresh, dOCRthresholdDown, dOCRthresholdUp);
                }

                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionThresh, out ho_RegionFillUp);
                if (bClosing)
                {

                    ho_RegionClosingRectangle1.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionThresh, out ho_RegionClosingRectangle1, dCloseWidth, dCloseHeight);
                    ho_RegionFillUp.Dispose();
                    ho_RegionFillUp = ho_RegionClosingRectangle1;

                }
                if (bOpenging)
                {
                    ho_RegionOpeningRectangle1.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionFillUp, out ho_RegionOpeningRectangle1, dOpenWidth, dOpenHeight);
                    ho_RegionFillUp.Dispose();
                    ho_RegionFillUp = ho_RegionOpeningRectangle1;
                }
                ho_ConnectedRegions1.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions1);
             
                    ho_SelectedRegions1.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions1, ((new HTuple("width")).TupleConcat(
        "height")), "and", ((new HTuple(dOCRWidthDown)).TupleConcat(
        dOCRHeightDown)), ((new HTuple(dOCRWidthUp)).TupleConcat(dOCRHeightUp)));
                    hWindowControl1.DispObj(ho_SelectedRegions1);
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
                ho_RegionThresh.Dispose();
                if (iThresholdSelect == 0)
                {

                    HOperatorSet.MeanImage(ho_ImageReduced1, out ho_ImageMean, dMeanfilte, dMeanfilte);
                    HOperatorSet.DynThreshold(ho_ImageReduced1, ho_ImageMean, out ho_RegionThresh,
                            dMeanfilte, "dark");
                }
                else
                {
                    HOperatorSet.Threshold(ho_ImageReduced1, out ho_RegionThresh, dOCRthresholdDown, dOCRthresholdUp);
                }
                //ho_ImageMean1.Dispose();
                //HOperatorSet.MeanImage(ho_ImageReduced1, out ho_ImageMean1, 3, 3);
                //ho_ImageMean2.Dispose();
                //HOperatorSet.MeanImage(ho_ImageReduced1, out ho_ImageMean2, dMeanfilte, dMeanfilte);

                //ho_RegionDynThresh.Dispose();
                //HOperatorSet.DynThreshold(ho_ImageMean2, ho_ImageMean1, out ho_RegionDynThresh1,
                //        dMeanfilte - 3, "dark");


                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionThresh, out ho_RegionFillUp);
                if (bClosing)
                {

                    ho_RegionClosingRectangle1.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionThresh, out ho_RegionClosingRectangle1, dCloseWidth, dCloseHeight);
                    ho_RegionFillUp.Dispose();
                    ho_RegionFillUp = ho_RegionClosingRectangle1;

                }
                if (bOpenging)
                {
                    ho_RegionOpeningRectangle1.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionFillUp, out ho_RegionOpeningRectangle1, dOpenWidth, dOpenHeight);
                    ho_RegionFillUp.Dispose();
                    ho_RegionFillUp = ho_RegionOpeningRectangle1;
                }        
                ho_ConnectedRegions2.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions2);

                HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions2, ((new HTuple("width")).TupleConcat(
            "height")), "and", ((new HTuple(dOCRWidthDown)).TupleConcat(
            dOCRHeightDown)), ((new HTuple(dOCRWidthUp)).TupleConcat(dOCRHeightUp)));
             
                ho_ImgBinSrc1.Dispose();
               
                HOperatorSet.RegionToBin(ho_SelectedRegions2, out ho_ImgBinSrc1, 0, 255, hv_Width,
    hv_Height);
                hWindowControl1.DispObj(ho_SelectedRegions2);
                ho_SortRgn2.Dispose();
                HOperatorSet.SortRegion(ho_SelectedRegions2, out ho_SortRgn2, "character", "true", "row");
                //调用系统的分类器
               
                //HOperatorSet.ReadOcrClassMlp("Industrial_0-9A-Z_NoRej.omc", out hv_OCRHandle);
                HOperatorSet.ReadOcrClassMlp("Document_0-9A-Z_NoRej.omc", out hv_OCRHandle);
                HOperatorSet.DoOcrMultiClassMlp(ho_SortRgn1, ho_ImgBinSrc, hv_OCRHandle,
                    out hv_Class, out hv_Confidence);
                HOperatorSet.DoOcrMultiClassMlp(ho_SortRgn2, ho_ImgBinSrc1, hv_OCRHandle,
                    out hv_Class1, out hv_Confidence1);
                

                //显示
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
                HOperatorSet.SetColor(hWindowControl1.getHWindowControl().HalconWindow, "green");
                HOperatorSet.SetTposition(hWindowControl1.getHWindowControl().HalconWindow, 10, 10);
                HOperatorSet.WriteString(hWindowControl1.getHWindowControl().HalconWindow, str);
                HOperatorSet.SetTposition(hWindowControl1.getHWindowControl().HalconWindow, 50, 10);
                HOperatorSet.WriteString(hWindowControl1.getHWindowControl().HalconWindow, str1);

            }

            catch
            {

            }
        }
        #endregion
        #region 视觉参数保存
        private void btnVisionSave_Click(object sender, EventArgs e)
        {
            MyLens_Mold_Cave.dReduceRadius = UCDetectionRadius.Value;
            MyLens_Mold_Cave.dGraythresholdUp = UCGraythresholdUp.Value;
            MyLens_Mold_Cave.dGraythresholdDown = UCGraythresholdDown.Value;
            MyLens_Mold_Cave.dAreaUp = UCAreaUp.Value;
            MyLens_Mold_Cave.dAreaDown = UCAreaDown.Value;
            MyLens_Mold_Cave.dLength1Up = UCLength1Up.Value;
            MyLens_Mold_Cave.dLength1Down = UCLength1Down.Value;
            MyLens_Mold_Cave.dLength2Up = UCLength2Up.Value;
            MyLens_Mold_Cave.dLength2Down = UCLength2Down.Value;
            MyLens_Mold_Cave.dthreshold = UCthreshold.Value;
            MyLens_Mold_Cave.dcliperNum = UCcliperNum.Value;
            MyLens_Mold_Cave.dcliperlength = UCcliperlength.Value;
            MyLens_Mold_Cave.dcliperwidth = UCcliperWidth.Value;
            MyLens_Mold_Cave.spolarity = spolarity;
            MyLens_Mold_Cave.sedgeSelect = sedgeSelect;
                                         
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "ReduceRadius", MyLens_Mold_Cave.dReduceRadius.ToString(), Path);
            IniFile.Write("Setting", "GraythresholdUp", MyLens_Mold_Cave.dGraythresholdUp.ToString(), Path);
            IniFile.Write("Setting", "GraythresholdDown", MyLens_Mold_Cave.dGraythresholdDown.ToString(), Path);
            IniFile.Write("Setting", "AreaUp", MyLens_Mold_Cave.dAreaUp.ToString(), Path);
            IniFile.Write("Setting", "AreaDown", MyLens_Mold_Cave.dAreaDown.ToString(), Path);
            IniFile.Write("Setting", "Length1Up", MyLens_Mold_Cave.dLength1Up.ToString(), Path);
            IniFile.Write("Setting", "Length1Down", MyLens_Mold_Cave.dLength1Down.ToString(), Path);
            IniFile.Write("Setting", "Length2Up", MyLens_Mold_Cave.dLength2Up.ToString(), Path);
            IniFile.Write("Setting", "Length2Down", MyLens_Mold_Cave.dLength2Down.ToString(), Path);
            IniFile.Write("Setting", "threshold", MyLens_Mold_Cave.dthreshold.ToString(), Path);
            IniFile.Write("Setting", "cliperNum", MyLens_Mold_Cave.dcliperNum.ToString(), Path);
            IniFile.Write("Setting", "cliperlength", MyLens_Mold_Cave.dcliperlength.ToString(), Path);
            IniFile.Write("Setting", "cliperwidth", MyLens_Mold_Cave.dcliperwidth.ToString(), Path);
            IniFile.Write("Setting", "polarity", MyLens_Mold_Cave.spolarity.ToString(), Path);
            IniFile.Write("Setting", "edgeSelect", MyLens_Mold_Cave.sedgeSelect.ToString(), Path);
           

           
       
        }
        private void btnOCRSave_Click(object sender, EventArgs e)
        {
            MyLens_Mold_Cave.dOCRCenterDistance = UCCenterDistance.Value;
            MyLens_Mold_Cave.dOCRRectangleLength = UCRectangleLength.Value;
            MyLens_Mold_Cave.dOCRRectangleWidth = UCRectangleWidth.Value;
            MyLens_Mold_Cave.dOCRthresholdUp = UCOCRthresholdUp.Value;
            MyLens_Mold_Cave.dOCRthresholdDown = UCOCRthresholdDown.Value;
            MyLens_Mold_Cave.dOCRWidthUp = UCOCRWidthUp.Value;
            MyLens_Mold_Cave.dOCRWidthDown = UCOCRWidthDown.Value;
            MyLens_Mold_Cave.dOCRHeightUp = UCOCRHeightUp.Value;
            MyLens_Mold_Cave.dOCRHeightDown = UCOCRHeightDown.Value;
            MyLens_Mold_Cave.binGegion = binGegion;
            MyLens_Mold_Cave.dMeanfilte = dMeanfilte;
            MyLens_Mold_Cave.iThresholdSelect = iThresholdSelect;
            MyLens_Mold_Cave.dCloseWidth = dCloseWidth;
            MyLens_Mold_Cave.dCloseHeight = dCloseHeight;
            MyLens_Mold_Cave.dOpenWidth = dOpenWidth;
            MyLens_Mold_Cave.dOpenHeight = dOpenHeight;
            MyLens_Mold_Cave.bClosing = bClosing;
            MyLens_Mold_Cave.bOpenging = bOpenging;
         
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "OCRCenterDistance", MyLens_Mold_Cave.dOCRCenterDistance.ToString(), Path);
            IniFile.Write("Setting", "OCRRectangleLength", MyLens_Mold_Cave.dOCRRectangleLength.ToString(), Path);
            IniFile.Write("Setting", "OCRRectangleWidth", MyLens_Mold_Cave.dOCRRectangleWidth.ToString(), Path);
            IniFile.Write("Setting", "OCRthresholdUp", MyLens_Mold_Cave.dOCRthresholdUp.ToString(), Path);
            IniFile.Write("Setting", "OCRthresholdDown", MyLens_Mold_Cave.dOCRthresholdDown.ToString(), Path);
            IniFile.Write("Setting", "OCRWidthUp", MyLens_Mold_Cave.dOCRWidthUp.ToString(), Path);
            IniFile.Write("Setting", "OCRWidthDown", MyLens_Mold_Cave.dOCRWidthDown.ToString(), Path);
            IniFile.Write("Setting", "OCRHeightUp", MyLens_Mold_Cave.dOCRHeightUp.ToString(), Path);
            IniFile.Write("Setting", "OCRHeightDown", MyLens_Mold_Cave.dOCRHeightDown.ToString(), Path);
            IniFile.Write("Setting", "binGegion", MyLens_Mold_Cave.binGegion.ToString(), Path);
            IniFile.Write("Setting", "dMeanfilte", MyLens_Mold_Cave.dMeanfilte.ToString(), Path);
            IniFile.Write("Setting", "iThresholdSelect", MyLens_Mold_Cave.iThresholdSelect.ToString(), Path);
            IniFile.Write("Setting", "dCloseWidth", MyLens_Mold_Cave.dCloseWidth.ToString(), Path);
            IniFile.Write("Setting", "dCloseHeight", MyLens_Mold_Cave.dCloseHeight.ToString(), Path);
            IniFile.Write("Setting", "dOpenWidth", MyLens_Mold_Cave.dOpenWidth.ToString(), Path);
            IniFile.Write("Setting", "dOpenHeight", MyLens_Mold_Cave.dOpenHeight.ToString(), Path);
            IniFile.Write("Setting", "bClosing", MyLens_Mold_Cave.bClosing.ToString(), Path);
            IniFile.Write("Setting", "bOpenging", MyLens_Mold_Cave.bOpenging.ToString(), Path);
        }
        #endregion

        

        #endregion


   

       
        #region 光源部分
        #region 光源通道参数读取
        public void LoadSettingLight()
        {
            nudLightSet_1.Value = Convert.ToInt32(Light.LightSet_1);
            nudLightSet_2.Value = Convert.ToInt32(Light.LightSet_2);
            nudLightSet_3.Value = Convert.ToInt32(Light.LightSet_3);
            nudLightSet_4.Value = Convert.ToInt32(Light.LightSet_4);
        }
        #endregion
        #region 光源通道1调节
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
        #endregion
        #region 光源通道2调节
        private void nudLightSet_2_ValueChanged(object sender, EventArgs e)
        {
            nudLightSet_2.Value = tbLightSet_2.Value;
        }

        private void tbLightSet_2_ValueChanged(object sender, EventArgs e)
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
        #endregion
        #region 光源通道3调节
        private void nudLightSet_3_ValueChanged(object sender, EventArgs e)
        {
            nudLightSet_3.Value = tbLightSet_3.Value;
        }

        private void tbLightSet_3_ValueChanged(object sender, EventArgs e)
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
        #endregion
        #region 光源通道4调节
        private void nudLightSet_4_ValueChanged(object sender, EventArgs e)
        {
            nudLightSet_4.Value = tbLightSet_4.Value;
        }

        private void tbLightSet_4_ValueChanged(object sender, EventArgs e)
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
        #endregion
        #region 光源通道参数保存
        private void btnLightSave_Click(object sender, EventArgs e)
        {

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("SettingLight", "Light1", tbLightSet_1.Value.ToString(), Path);
            IniFile.Write("SettingLight", "Light2", tbLightSet_2.Value.ToString(), Path);
            IniFile.Write("SettingLight", "Light3", tbLightSet_3.Value.ToString(), Path);
            IniFile.Write("SettingLight", "Light4", tbLightSet_4.Value.ToString(), Path);
            
        }
        #endregion
        #region 光源通道调节cmd
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
      #endregion
        #endregion
        #region 相机部分
        #region 相机参数读取
        public void CCDSetPara()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            CCD.Gain = double.Parse(IniFile.Read("Setting", "Gain", "0", Path));
            CCD.ExposureTime = double.Parse(IniFile.Read("Setting", "ExposureTime", "35000", Path));
            nudGain_1.Value = Convert.ToInt32(CCD.Gain);
            nudExposureTime_1.Value = Convert.ToInt32(CCD.ExposureTime); 
        }
        #endregion
        #region 相机增益调节
        private void tbGain_1_ValueChanged(object sender, EventArgs e)
        {
            nudGain_1.Value = tbGain_1.Value;

        }
        private void nudGain_1_ValueChanged(object sender, EventArgs e)
        {
            tbGain_1.Value = Convert.ToInt32(nudGain_1.Value);
            CCD.Gain = (double)nudGain_1.Value;
            parent.SetGain(tbGain_1.Value);
        }
        #endregion
        #region 相机曝光时间调节

        private void tbExposureTime_1_ValueChanged(object sender, EventArgs e)
        {
            nudExposureTime_1.Value = tbExposureTime_1.Value;
        }

        private void nudExposureTime_1_ValueChanged(object sender, EventArgs e)
        {
            tbExposureTime_1.Value = Convert.ToInt32(nudExposureTime_1.Value);

            parent.SetExposureTime(tbExposureTime_1.Value);


        }
        #endregion
        #region 相机参数保存
        private void btnCCDSetSave_1_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            CCD.Gain = (double)nudGain_1.Value;
            CCD.ExposureTime = (double)nudExposureTime_1.Value;
            IniFile.Write("Setting", "Gain", CCD.Gain.ToString(), Path);
            IniFile.Write("Setting", "ExposureTime", CCD.ExposureTime.ToString(), Path);
        }
        #endregion

        #endregion
        #region UI更新
        private void TimeUI_Tick(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                //parent.OneShot();

                btnContinueShot.BackColor = Color.Green;
                hWindowControl1.DrawModel = true;
                btnContinueShot.Text = "停止";
            }
            else
            {
                btnContinueShot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                hWindowControl1.DrawModel = false;
                btnContinueShot.Text = "預覽";
            }
        }
        #endregion

        private void btnOCRAffine_Click(object sender, EventArgs e)
        {
            HTuple hv_HomMat2D;
            HObject ho_ImageAffineTrans = null;
            HObject ho_RegionAffineTrans = null;
            HObject ho_Image = null;
            HObject ho_Circle = null;
            HObject ho_ReducedImage = null;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_ReducedImage);
            HTuple hv_Width;
            HTuple hv_Height;
            if (My.ho_Image == null)
                return;
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_ReducedImage);
            hWindowControl1.ClearWindow();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.GenEmptyObj(out ho_ImageAffineTrans);
            HOperatorSet.VectorAngleToRigid(ResultRect2Row, ResultRect2Col, ResultRect2Phi,
    ResultRect2Row, ResultRect2Col, 0, out hv_HomMat2D);
            ho_ImageAffineTrans.Dispose();
            HOperatorSet.AffineTransImage(ho_Image, out ho_ImageAffineTrans,
                hv_HomMat2D, "constant", "false");
            hWindowControl1.HobjectToHimage(ho_ImageAffineTrans);
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

   

   
     

     
      
       










































    }
}
