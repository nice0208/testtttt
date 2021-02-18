using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;
using System.IO;

namespace Detecting_System
{
    public partial class Frm8982SomaVisionSet : Form
    {
        FrmParent parent;
        FrmRun Run;
        public Frm8982SomaVisionSet(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;
            InitializeComponent();
        }
        public HTuple dFirstCircleRow = 0;
        public HTuple dFirstCircleColumn = 0;
        //檢測區域半徑
        public static double dReduceRadius = 1;
        //檢測區域圖片
        public static HObject ho_ReducedImage;
        //初始圓環
        public static HTuple dFirstCircleRadius = 0;
        //抓圓中的灰度閥值
        public static double dGraythreshold = 1;
        //擬合圓線條長度
        public static double dLength = 1;
        //擬合圓線條長度
        public static double dWidth = 1;
        //擬合圓灰度差異
        public static double dMeasureThreshold = 20;
        //擬合圓白找黑或黑找白
        public static string sGenParamValue = "negative";
        //檢測Soma區域圖片
        public static HObject ho_SomaReducedImage;
        //Soma半徑
        public static double dSomaReduceRadius = 0;
        //Soma灰度閥值
        public static double dSomaGraythreshold = 1;


        #region Halcon參數1
        // Procedures 
        // External procedures 
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

        // Chapter: Develop
        // Short Description: Open a new graphics window that preserves the aspect ratio of the given image. 
        public void dev_open_window_fit_image(HObject ho_Image, HTuple hv_Row, HTuple hv_Column,
            HTuple hv_WidthLimit, HTuple hv_HeightLimit, out HTuple hv_WindowHandle)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_MinWidth = new HTuple(), hv_MaxWidth = new HTuple();
            HTuple hv_MinHeight = new HTuple(), hv_MaxHeight = new HTuple();
            HTuple hv_ResizeFactor = null, hv_ImageWidth = null, hv_ImageHeight = null;
            HTuple hv_TempWidth = null, hv_TempHeight = null, hv_WindowWidth = new HTuple();
            HTuple hv_WindowHeight = null;
            // Initialize local and output iconic variables 
            hv_WindowHandle = new HTuple();
            //This procedure opens a new graphics window and adjusts the size
            //such that it fits into the limits specified by WidthLimit
            //and HeightLimit, but also maintains the correct image aspect ratio.
            //
            //If it is impossible to match the minimum and maximum extent requirements
            //at the same time (f.e. if the image is very long but narrow),
            //the maximum value gets a higher priority,
            //
            //Parse input tuple WidthLimit
            if ((int)((new HTuple((new HTuple(hv_WidthLimit.TupleLength())).TupleEqual(0))).TupleOr(
                new HTuple(hv_WidthLimit.TupleLess(0)))) != 0)
            {
                hv_MinWidth = 500;
                hv_MaxWidth = 800;
            }
            else if ((int)(new HTuple((new HTuple(hv_WidthLimit.TupleLength())).TupleEqual(
                1))) != 0)
            {
                hv_MinWidth = 0;
                hv_MaxWidth = hv_WidthLimit.Clone();
            }
            else
            {
                hv_MinWidth = hv_WidthLimit.TupleSelect(0);
                hv_MaxWidth = hv_WidthLimit.TupleSelect(1);
            }
            //Parse input tuple HeightLimit
            if ((int)((new HTuple((new HTuple(hv_HeightLimit.TupleLength())).TupleEqual(0))).TupleOr(
                new HTuple(hv_HeightLimit.TupleLess(0)))) != 0)
            {
                hv_MinHeight = 400;
                hv_MaxHeight = 600;
            }
            else if ((int)(new HTuple((new HTuple(hv_HeightLimit.TupleLength())).TupleEqual(
                1))) != 0)
            {
                hv_MinHeight = 0;
                hv_MaxHeight = hv_HeightLimit.Clone();
            }
            else
            {
                hv_MinHeight = hv_HeightLimit.TupleSelect(0);
                hv_MaxHeight = hv_HeightLimit.TupleSelect(1);
            }
            //
            //Test, if window size has to be changed.
            hv_ResizeFactor = 1;
            HOperatorSet.GetImageSize(ho_Image, out hv_ImageWidth, out hv_ImageHeight);
            //First, expand window to the minimum extents (if necessary).
            if ((int)((new HTuple(hv_MinWidth.TupleGreater(hv_ImageWidth))).TupleOr(new HTuple(hv_MinHeight.TupleGreater(
                hv_ImageHeight)))) != 0)
            {
                hv_ResizeFactor = (((((hv_MinWidth.TupleReal()) / hv_ImageWidth)).TupleConcat(
                    (hv_MinHeight.TupleReal()) / hv_ImageHeight))).TupleMax();
            }
            hv_TempWidth = hv_ImageWidth * hv_ResizeFactor;
            hv_TempHeight = hv_ImageHeight * hv_ResizeFactor;
            //Then, shrink window to maximum extents (if necessary).
            if ((int)((new HTuple(hv_MaxWidth.TupleLess(hv_TempWidth))).TupleOr(new HTuple(hv_MaxHeight.TupleLess(
                hv_TempHeight)))) != 0)
            {
                hv_ResizeFactor = hv_ResizeFactor * ((((((hv_MaxWidth.TupleReal()) / hv_TempWidth)).TupleConcat(
                    (hv_MaxHeight.TupleReal()) / hv_TempHeight))).TupleMin());
            }
            hv_WindowWidth = hv_ImageWidth * hv_ResizeFactor;
            hv_WindowHeight = hv_ImageHeight * hv_ResizeFactor;
            //Resize window
            //dev_open_window(...);
            HOperatorSet.SetPart(hv_ExpDefaultWinHandle, 0, 0, hv_ImageHeight - 1, hv_ImageWidth - 1);

            return;
        }

        // Chapter: Develop
        // Short Description: Switch dev_update_pc, dev_update_var and dev_update_window to 'off'. 
        public void dev_update_off()
        {

            // Initialize local and output iconic variables 
            //This procedure sets different update settings to 'off'.
            //This is useful to get the best performance and reduce overhead.
            //
            // dev_update_pc(...); only in hdevelop
            // dev_update_var(...); only in hdevelop
            // dev_update_window(...); only in hdevelop

            return;
        }
        #endregion

        #region Halcon參數2
        // Local iconic variables 
        public HTuple hv_ExpDefaultWinHandle;

        HObject ho_Image, ho_Circle = null;
        HObject ho_Region = null, ho_Connection = null, ho_SelectedRegions = null;
        HObject ho_ConnectedRegions = null, ho_RegionFillUp = null;
        HObject ho_ModelContour = null, ho_MeasureContour = null, ho_Contour = null;
        HObject ho_CrossCenter = null, ho_Contours = null, ho_Cross = null;
        HObject ho_UsedEdges = null, ho_ResultContours = null, ho_CrossCenter_1 = null;
        HObject ho_SecondCircle = null, ho_ReducedImage2 = null, ho_Region2 = null;
        HObject ho_RegionClosing2 = null, ho_RegionFillUp2 = null, ho_ConnectedRegions2 = null;
        HObject ho_SelectedRegions2 = null, ho_CrossCenter_2 = null;

        // Local control variables 

        HTuple hv_WindowHandle = new HTuple(), hv_Width = null;
        HTuple hv_Height = null, hv_FirstRow = null, hv_FirstColumn1 = null;
        HTuple hv_FirstRadius = null, hv_i = null, hv_Radious0 = new HTuple();
        HTuple hv_GraySetting1 = new HTuple(), hv_Area = new HTuple();
        HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
        HTuple hv_MetrologyHandle = new HTuple(), hv_circleIndices = new HTuple();
        HTuple hv_GenParamValue = new HTuple(), hv_Length1 = new HTuple();
        HTuple hv_Width1 = new HTuple(), hv_Measure_Threshold = new HTuple();
        HTuple hv_circleParameter = new HTuple(), hv_Row1 = new HTuple();
        HTuple hv_Column1 = new HTuple(), hv_UsedRow = new HTuple();
        HTuple hv_UsedColumn = new HTuple(), hv_ResultRow = new HTuple();
        HTuple hv_ResultColumn = new HTuple(), hv_ResultRadius = new HTuple();
        HTuple hv_StartPhi = new HTuple(), hv_EndPhi = new HTuple();
        HTuple hv_PointOrder = new HTuple(), hv_SecondRadius0 = new HTuple();
        HTuple hv_Area2 = new HTuple(), hv_Row2 = new HTuple();
        HTuple hv_Column2 = new HTuple(), hv_Circularity = new HTuple();
        HTuple hv_CircularitySet = new HTuple(), hv_Area3 = new HTuple();
        HTuple hv_Row3 = new HTuple(), hv_Column3 = new HTuple();
        HTuple hv_Distance = new HTuple();
        #endregion

        private void Frm8982SomaVisionSet_Load(object sender, EventArgs e)
        {
            ReadPara();
        }

        public void ReadPara()
        {
            dFirstCircleRadius = My8982Soma.dFirstCircleRadius;
            tbDetectionRadius.Value = (int)My8982Soma.dReduceRadius;
            tbGraythreshold.Value = (int)My8982Soma.dGraythreshold;
            tbLength.Value = (int)My8982Soma.dLength;
            tbWidth.Value = (int)My8982Soma.dWidth;

            if (My.sGenParamValue == "negative")
            {
                tbBlackToWhite.Value = (int)My8982Soma.dMeasureThreshold;
            }
            else
            {
                tbBlackToWhite.Value = (int)My8982Soma.dMeasureThreshold;
            }

            tbSomaRadius.Value = (int)My8982Soma.dSomaReduceRadius;
            tbSomaGraythreshold.Value = (int)My8982Soma.dSomaGraythreshold;
            nudCircularitySet.Value = (decimal)My8982Soma.dCircularitySet;
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
                MyHalcon.ReadPicture(hWindowControl1.HalconWindow, ImagePath, out readImage);
                My.ho_Image = readImage;
                // 读取这张图片并将图片赋值给readImage,这句就是直接调的halcon类了，下边public定义的的是他的类
            }
        }

        private void tbDetectionRadius_ValueChanged(object sender, EventArgs e)
        {
            nudDetectionRadius.Value = tbDetectionRadius.Value;
        }

        private void nudDetectionRadius_ValueChanged(object sender, EventArgs e)
        {
            dReduceRadius = tbDetectionRadius.Value = Convert.ToInt32(nudDetectionRadius.Value);
            MyHalcon.GenDetectionRadius(hWindowControl1.HalconWindow, My.ho_Image, dReduceRadius, out ho_ReducedImage);
        }

        private void tbGraythreshold_ValueChanged(object sender, EventArgs e)
        {
            nudGraythreshold.Value = tbGraythreshold.Value;
        }

        private void nudGraythreshold_ValueChanged(object sender, EventArgs e)
        {
            dGraythreshold = tbGraythreshold.Value = Convert.ToInt32(nudGraythreshold.Value);
            MyHalcon.GrayThreshold1(hWindowControl1.HalconWindow, My.ho_Image, dReduceRadius, dGraythreshold, out ho_ReducedImage);
        }

        private void tbLength_ValueChanged(object sender, EventArgs e)
        {
            nudLength.Value = tbLength.Value;
        }

        private void nudLength_ValueChanged(object sender, EventArgs e)
        {
            dLength = tbLength.Value = Convert.ToInt32(nudLength.Value);
            MyHalcon.CatchCenter(hWindowControl1.HalconWindow, My.ho_Image, dReduceRadius, dGraythreshold, dFirstCircleRadius,
                sGenParamValue, dLength, dWidth, dMeasureThreshold);
        }

        private void tbWidth_ValueChanged(object sender, EventArgs e)
        {
            nudWidth.Value = tbWidth.Value;
        }

        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            dWidth = tbWidth.Value = Convert.ToInt32(nudWidth.Value);
            MyHalcon.CatchCenter(hWindowControl1.HalconWindow, My.ho_Image, dReduceRadius, dGraythreshold, dFirstCircleRadius,
                sGenParamValue, dLength, dWidth, dMeasureThreshold);
        }

        private void tbBlackToWhite_ValueChanged(object sender, EventArgs e)
        {
            nudBlackToWhite.Value = tbBlackToWhite.Value;
        }

        private void nudBlackToWhite_ValueChanged(object sender, EventArgs e)
        {
            dMeasureThreshold = tbBlackToWhite.Value = Convert.ToInt32(nudBlackToWhite.Value);
            sGenParamValue = "negative";
            MyHalcon.CatchCenter(hWindowControl1.HalconWindow, My.ho_Image, dReduceRadius, dGraythreshold, dFirstCircleRadius,
                sGenParamValue, dLength, dWidth, dMeasureThreshold);
        }

        private void tbWhiteToBlack_ValueChanged(object sender, EventArgs e)
        {
            nudWhiteToBlack.Value = tbWhiteToBlack.Value;
        }

        private void nudWhiteToBlack_ValueChanged(object sender, EventArgs e)
        {
            dMeasureThreshold = tbWhiteToBlack.Value = Convert.ToInt32(nudWhiteToBlack.Value);
            sGenParamValue = "negative";
            MyHalcon.CatchCenter(hWindowControl1.HalconWindow, My.ho_Image, dReduceRadius, dGraythreshold, dFirstCircleRadius,
                sGenParamValue, dLength, dWidth, dMeasureThreshold);
        }

        private void btnCircleCenter_Click(object sender, EventArgs e)
        {
            MyHalcon.CatchCenter2(hWindowControl1.HalconWindow, My.ho_Image, dReduceRadius, dGraythreshold, dFirstCircleRadius,
                sGenParamValue, dLength, dWidth, dMeasureThreshold);
            tbSomaRadius.Enabled = true;
            nudSomaRadius.Enabled = true;
            tbSomaGraythreshold.Enabled = true;
            nudSomaGraythreshold.Enabled = true;
        }

        private void tbSomaRadius_ValueChanged(object sender, EventArgs e)
        {
            nudSomaRadius.Value = tbSomaRadius.Value;
        }

        private void nudSomaRadius_ValueChanged(object sender, EventArgs e)
        {
            dSomaReduceRadius = tbSomaRadius.Value = Convert.ToInt32(nudSomaRadius.Value);
            MyHalcon.GenSomaDetectionRadius(hWindowControl1.HalconWindow, My.ho_Image, dSomaReduceRadius, out ho_SomaReducedImage);
        }

        private void tbSomaGraythreshold_ValueChanged(object sender, EventArgs e)
        {
            nudSomaGraythreshold.Value = tbSomaGraythreshold.Value;
        }

        private void nudSomaGraythreshold_ValueChanged(object sender, EventArgs e)
        {
            dSomaGraythreshold = tbSomaGraythreshold.Value = Convert.ToInt32(nudSomaGraythreshold.Value);
            MyHalcon.GrayThreshold2(hWindowControl1.HalconWindow, My.ho_Image, dSomaReduceRadius, dSomaGraythreshold, out ho_ReducedImage);
        }

        private void btnOneShot_Click(object sender, EventArgs e)
        {
            parent.OneShot();
        }

        private void btnContinueShot_Click(object sender, EventArgs e)
        {
            parent.OneShot();
            My.ContinueShot = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
        }

        private void btnImageProPlus_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            ImageProPlus(hWindowControl1.HalconWindow);
        }

        public void ImageProPlus(HWindow hWindowControl)
        {
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_ReducedImage);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_Connection);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_ModelContour);
            HOperatorSet.GenEmptyObj(out ho_MeasureContour);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter_1);
            HOperatorSet.GenEmptyObj(out ho_SecondCircle);
            HOperatorSet.GenEmptyObj(out ho_ReducedImage2);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing2);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter_2);
            try
            {
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl;
                hv_FirstRadius = dFirstCircleRadius;
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                //畫檢視範圍
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //畫出初始圓
                //灰度設定1
                hv_GraySetting1 = dGraythreshold;
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_Region, hv_GraySetting1, 255);
                ho_Connection.Dispose();
                HOperatorSet.Connection(ho_Region, out ho_Connection);

                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_Connection, out ho_SelectedRegions, (new HTuple("outer_radius")).TupleConcat(
                    "roundness"), "and", (new HTuple(0)).TupleConcat(0.8), ((hv_FirstRadius + 50)).TupleConcat(
                    1));
                HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions, out ExpTmpOutVar_0, "area", "and",
                        hv_Area.TupleMax(), 999999);
                    ho_SelectedRegions.Dispose();
                    ho_SelectedRegions = ExpTmpOutVar_0;
                }
                HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);

                //找出圓心
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.AddMetrologyObjectGeneric(hv_MetrologyHandle, "circle", ((hv_Row.TupleConcat(
                    hv_Column))).TupleConcat(hv_FirstRadius), 25, 5, 1, 30, new HTuple(),
                    new HTuple(), out hv_circleIndices);
                ho_ModelContour.Dispose();
                HOperatorSet.GetMetrologyObjectModelContour(out ho_ModelContour, hv_MetrologyHandle,
                    "all", 1.5);
                ho_MeasureContour.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureContour, hv_MetrologyHandle,
                    "all", "all", out hv_Row, out hv_Column);

                //set_metrology_model_param (MetrologyHandle, 'reference_system', [RowRefer, ColRefer, 0])
                //白找黑('positive')或黑找白('negative')
                hv_GenParamValue = sGenParamValue;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices,
                    "measure_transition", hv_GenParamValue);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices,
                    "measure_select", "last");
                //長度
                hv_Length1 = dLength;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices,
                    "measure_length1", hv_Length1);
                //寬度
                hv_Width1 = dWidth;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices,
                    "measure_length2", hv_Width1);
                //灰度差異
                hv_Measure_Threshold = dMeasureThreshold;

                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices,
                    "measure_threshold", hv_Measure_Threshold);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices,
                    "min_score", 0.2);

                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                ho_Contour.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour, hv_MetrologyHandle,
                    "all", "positive", out hv_Row, out hv_Column);
                HOperatorSet.DispObj(ho_Contour, hv_ExpDefaultWinHandle);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_circleIndices,
                    "all", "result_type", "all_param", out hv_circleParameter);
                ho_CrossCenter.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_CrossCenter, hv_circleParameter.TupleSelect(
                    0), hv_circleParameter.TupleSelect(1), 20, 0.785398);
                ho_Contours.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_Contours, hv_MetrologyHandle,
                    "all", "all", 1.5);
                ho_Contour.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour, hv_MetrologyHandle,
                    "all", "all", out hv_Row1, out hv_Column1);
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row1, hv_Column1, 6, 0.785398);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "used_edges",
                    "row", out hv_UsedRow);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "used_edges",
                    "column", out hv_UsedColumn);
                ho_UsedEdges.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_UsedEdges, hv_UsedRow, hv_UsedColumn,
                    10, (new HTuple(45)).TupleRad());
                HOperatorSet.DispObj(ho_UsedEdges, hv_ExpDefaultWinHandle);
                ho_ResultContours.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_ResultContours, hv_MetrologyHandle,
                    "all", "all", 1.5);

                HOperatorSet.FitCircleContourXld(ho_ResultContours, "algebraic", -1, 0, 0,
                    3, 2, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius, out hv_StartPhi,
                    out hv_EndPhi, out hv_PointOrder);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                ho_CrossCenter_1.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_CrossCenter_1, hv_circleParameter.TupleSelect(
                    0), hv_circleParameter.TupleSelect(1), 20, 0.785398);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_ResultContours, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_CrossCenter_1, hv_ExpDefaultWinHandle);

                //draw_circle (WindowHandle, FirstRow, FirstColumn1, SecondRadius)
                //找出SOMA範圍
                hv_SecondRadius0 = dSomaReduceRadius;
                ho_SecondCircle.Dispose();
                HOperatorSet.GenCircle(out ho_SecondCircle, hv_circleParameter.TupleSelect(
                    0), hv_circleParameter.TupleSelect(1), hv_SecondRadius0);
                ho_ReducedImage2.Dispose();
                HOperatorSet.ReduceDomain(ho_ReducedImage, ho_SecondCircle, out ho_ReducedImage2
                    );
                //SOMA灰度設定
                ho_Region2.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage2, out ho_Region2, 0, dSomaGraythreshold);
                ho_RegionClosing2.Dispose();
                HOperatorSet.ClosingCircle(ho_Region2, out ho_RegionClosing2, 10);
                ho_RegionFillUp2.Dispose();
                HOperatorSet.FillUp(ho_RegionClosing2, out ho_RegionFillUp2);
                ho_ConnectedRegions2.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp2, out ho_ConnectedRegions2);
                HOperatorSet.AreaCenter(ho_ConnectedRegions2, out hv_Area2, out hv_Row2,
                    out hv_Column2);
                ho_SelectedRegions2.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions2, "area",
                    "and", hv_Area2.TupleMax(), 999999);
                HOperatorSet.Circularity(ho_SelectedRegions2, out hv_Circularity);
                //顯示SOMA範圍
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_SelectedRegions2, hv_ExpDefaultWinHandle);
                //檢驗圓度
                set_display_font(hv_ExpDefaultWinHandle, 25, "mono", "true", "false");

                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "真圓度:" + hv_Circularity);
                hv_CircularitySet = My8982Soma.dCircularitySet;
                if ((int)(new HTuple(hv_Circularity.TupleGreaterEqual(hv_CircularitySet))) != 0)
                {
                    set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2000, 100);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "OK");
                    Vision.VisionResult[Tray.n] = "OK";
                    //擷取當前畫面圖片為Image1
                    HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                }
                else
                {
                    set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2000, 100);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "NG");
                    Vision.VisionResult[Tray.n] = "NG";
                    //擷取當前畫面圖片為Image
                    HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                }
                ////檢驗偏心
                ////求圓心
                //HOperatorSet.AreaCenter(ho_SelectedRegions2, out hv_Area3, out hv_Row3, out hv_Column3);
                //ho_CrossCenter_2.Dispose();
                //HOperatorSet.GenCrossContourXld(out ho_CrossCenter_2, hv_Row3, hv_Column3,
                //    20, 0.785398);
                //HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                //HOperatorSet.DispObj(ho_CrossCenter_2, hv_ExpDefaultWinHandle);
                //HOperatorSet.DistancePp(hv_circleParameter.TupleSelect(0), hv_circleParameter.TupleSelect(
                //    1), hv_Row3, hv_Column3, out hv_Distance);
                //HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 0);
                //HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "偏心:" + hv_Distance);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
               
            }
            catch (HalconException HDevExpDefaultException)
            {
                set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                Vision.VisionResult[Tray.n] = "Miss";
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2000, 100);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                //擷取當前畫面圖片為Image
                HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
            }
            if (Tray.NowTray == 1)
                    Vision.Images_1[Tray.n] = ho_Image;
                else if (Tray.NowTray == 2)
                    Vision.Images_2[Tray.n] = ho_Image;
            WriteLog(Tray.n, Vision.VisionResult[Tray.n], (double)hv_Circularity);
        }

        public void WriteLog(int n, string ResultOK, double Circularity)
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
                    string Log = Path + "\\" + Production.CurProduction + "-" + Sys.MachineID +
                                 Tray.OpDateTime.ToString("_HH_mm_ss_dd_MM_yyyy_") +
                                 Tray.Barcode_1 + "-" +  Tray.Barcode_2 + ".txt";
                    string Barcode = "";
                    if(Tray.NowTray == 1)
                    {
                        Barcode = Tray.Barcode_1;
                    }
                    else if(Tray.NowTray == 2)
                    {
                        Barcode = Tray.Barcode_2;
                    }

                    //不存在文件，创建先

                    if (!File.Exists(Log))
                    {
                        File.WriteAllText(Log, "Tray No.\tProgram ID\tMachine No.\tOutput Tray Barcode\tResult\tCircularity" + 
                                         "\r\n");
                    }
                    //写result
                    using (StreamWriter sw = new StreamWriter(Log, true))
                    {
                            sw.WriteLine(string.Format("{0}.{1}", Tray.CurrentRow, Tray.CurrentColumn) + "\t" +
                                         Production.CurProduction + "\t" + Sys.MachineID + "\t" + Barcode + "\t" +
                                         DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" +
                                         ResultOK+ "\t" + Circularity.ToString("f4"));
                        }
                    }
                catch
                {
                }
            }
        }

        private void btnDrawCircle_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
            //找出初始半徑
            HOperatorSet.DrawCircle(hv_ExpDefaultWinHandle, out dFirstCircleRow, out dFirstCircleColumn,
                out dFirstCircleRadius);
            tbDetectionRadius.Enabled = true;
            nudDetectionRadius.Enabled = true;
            tbGraythreshold.Enabled = true;
            nudGraythreshold.Enabled = true;
            tbLength.Enabled = true;
            nudLength.Enabled = true;
            tbWidth.Enabled = true;
            nudWidth.Enabled = true;
            tbBlackToWhite.Enabled = true;
            nudBlackToWhite.Enabled = true;
            tbWhiteToBlack.Enabled = true;
            nudWhiteToBlack.Enabled = true;
        }

        private void btnCenterSave_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            My8982Soma.dFirstCircleRadius = dFirstCircleRadius;
            My8982Soma.dReduceRadius = dReduceRadius;
            My8982Soma.dGraythreshold = dGraythreshold;
            My8982Soma.dLength = dLength;
            My8982Soma.dWidth = dWidth;
            My8982Soma.dMeasureThreshold = dMeasureThreshold;
            My8982Soma.sGenParamValue = sGenParamValue;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "FirstCircleRadius", My8982Soma.dFirstCircleRadius.ToString(), Path);
            IniFile.Write("Setting", "ReduceRadius", My8982Soma.dReduceRadius.ToString(), Path);
            IniFile.Write("Setting", "Graythreshold", My8982Soma.dGraythreshold.ToString(), Path);
            IniFile.Write("Setting", "Length", My8982Soma.dLength.ToString(), Path);
            IniFile.Write("Setting", "Width", My8982Soma.dWidth.ToString(), Path);
            IniFile.Write("Setting", "MeasureThreshold", My8982Soma.dMeasureThreshold.ToString(), Path);
            IniFile.Write("Setting", "GenParamValue", My8982Soma.sGenParamValue.ToString(), Path);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            My8982Soma.dCircularitySet = Convert.ToDouble(nudCircularitySet.Value);
            IniFile.Write("Setting", "CircularitySet", My8982Soma.dCircularitySet.ToString(), Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini");
        }

        private void btnSomaSave_Click(object sender, EventArgs e)
        {
            My8982Soma.dSomaReduceRadius = dSomaReduceRadius;
            My8982Soma.dSomaGraythreshold = dSomaGraythreshold;
            IniFile.Write("Setting", "SomaReduceRadius", My8982Soma.dSomaReduceRadius.ToString(), Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini");
            IniFile.Write("Setting", "SomaGraythreshold", My8982Soma.dSomaGraythreshold.ToString(), Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini");
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                btnContinueShot.BackColor = Color.Green;
            }
            else
            {
                btnContinueShot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            }
        }

        private void btnCCDSetSave_Click(object sender, EventArgs e)
        {

        }

        private void tbDetectionRadius_Scroll(object sender, EventArgs e)
        {

        }

   
    


    }
}
        
