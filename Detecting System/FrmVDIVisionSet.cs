using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;
using System.Threading;
using System.IO;

namespace Detecting_System
{
    public partial class FrmVDIVisionSet : Form
    {
        FrmParent parent;
        public FrmVDIVisionSet(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;
            InitializeComponent();
        }
        public HTuple dFirstCircleRow = 0;
        public HTuple dFirstCircleColumn = 0;
        public HTuple dFirstCircleRadius = 0;

        public static bool Miss = false;
        //檢測區域半徑
        public static double dReduceRadius = 1;
        //抓圓中的灰度閥值
        public static double dGraythreshold = 1;
        //擬合圓線條長度
        public static double dLength = 1;
        //擬合圓灰度差異
        public static double dMeasureThreshold = 20;
        //擬合圓白找黑或黑找白
        public static string sGenParamValue = "negative";

        //鍍膜邊緣初始圓
        public static double dFirstRadius2 = 1;
        //鍍膜邊緣灰度設定
        public static double dGraythreshold2 = 1;
        //鍍膜邊緣寬度設定
        public static double dLength2 = 1;
        //鍍膜邊緣寬度設定
        public static double dMeasureThreshold2 = 1;
        //鍍膜邊緣白找黑或黑找白
        public static string sGenParamValue2 = "positive";

        public static double dMissRadius = 1;
        public static double dMissGray = 1;
    public static double dMissArea = 1;
        #region halcon參數1
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
        public HTuple hv_ExpDefaultWinHandle;

        // Stack for temporary objects 
        HObject[] OTemp = new HObject[20];

        // Local iconic variables 

        HObject ho_Image, ho_Circle,ho_Circle2, ho_ReducedImage,ho_RegionOpening;
        HObject ho_Region, ho_Connection, ho_SelectedRegions0, ho_ModelContour;
        HObject ho_MeasureContour, ho_Contour, ho_CrossCenter, ho_Contours;
        HObject ho_Cross, ho_UsedEdges, ho_ResultContours, ho_ImageMedian;
        HObject ho_ModelContour2, ho_MeasureContour2, ho_Contour2;
        HObject ho_Contours2, ho_Cross2, ho_UsedEdges2, ho_ResultContours2,ho_MissRegion;
        HObject ho_MissSelectedRegions,ho_MissConnectedRegions;

        // Local control variables 

        HTuple hv_WindowHandle = new HTuple(), hv_Width = null, hv_Width2 = null, hv_Height2 = null;
        HTuple hv_Height = null, hv_FirstRadius = null, hv_Radious0 = null;
        HTuple hv_GraySetting = null, hv_Area0 = null, hv_Row0 = null;
        HTuple hv_Column0 = null, hv_MetrologyHandle = null, hv_circleIndices = null;
        HTuple hv_Row = null, hv_Column = null, hv_GenParamValue = null;
        HTuple hv_Length1 = null, hv_Width1 = null, hv_Measure_Threshold = null;
        HTuple hv_circleParameter = null, hv_Row1 = null, hv_Column1 = null;
        HTuple hv_UsedRow = null, hv_UsedColumn = null, hv_ResultRow = null;
        HTuple hv_ResultColumn = null, hv_ResultRadius = null;
        HTuple hv_StartPhi = null, hv_EndPhi = null, hv_PointOrder = null;
        HTuple hv_FirstRadius2 = null, hv_MetrologyHandle2 = null;
        HTuple hv_circleIndices2 = null, hv_Row2 = null, hv_Column2 = null;
        HTuple hv_GenParamValue2 = null, hv_circleParameter2 = null;
        HTuple hv_UsedRow2 = null, hv_UsedColumn2 = null, hv_ResultRow2 = null;
        HTuple hv_ResultColumn2 = null, hv_ResultRadius2 = null;
        HTuple hv_StartPhi2 = null, hv_EndPhi2 = null, hv_PointOrder2 = null;
        HTuple hv_Distance1 = null;
        HTuple hv_MissGray = null,hv_MissAreaSet = null,hv_MissNumber = null;
        #endregion

        private void FrmVDIVisionSet_Load(object sender, EventArgs e)
        {
            ReadPara();
            LoadSettingLight();
            parent.LightOn_All();
            TimerUI.Enabled = true;
        }
        public void ReadPara()
        {
            dFirstCircleRadius = My.dFirstCircleRadius;
            tbDetectionRadius.Value = (int)My.dFirstCircleRadius;
            tbDetectionRadius.Value = (int)My.dReduceRadius;
            tbGraythreshold.Value = (int)My.dGraythreshold;
            tbLength.Value = (int)My.dLength;

            if (My.sGenParamValue == "positive")
            {
                tbBlackToWhite.Value = (int)My.dMeasureThreshold;
            }
            else
            {
                tbWhiteToBlack.Value = (int)My.dMeasureThreshold;
            }
            nudAimCirR.Value = (decimal)My.VDI.AimCirR;
            nudCoatRMin.Value = (decimal)My.VDI.dCoatRMin;
            nudCoatRMax.Value = (decimal)My.VDI.dCoatRMax;
            nudPositiveOffSet.Value = (decimal)My.VDI.dPositiveOffSet;
            nudNegativeOffSet.Value = (decimal)My.VDI.dNegativeOffSet;
            tbFirstRadius.Value = (int)My.VDI.dFirstCircleRadius2;
            tbLength2.Value = (int)My.VDI.dLength2;
            if (My.VDI.sGenParamValue2 == "positive")
            {
                tbBlackToWhite2.Value = (int)My.VDI.dMeasureThreshold2;
            }
            else
            {
                tbWhiteToBlack2.Value = (int)My.VDI.dMeasureThreshold2;
            }
            tbMissGray.Value = (int)My.VDI.dMissGray;
            tbMissArea.Value = (int)My.VDI.dMissArea;
            cbDarkLightChoice.SelectedIndex = My.VDI.DarkLightChoice;
                
        }

        public void LoadSettingLight()
        {
            nudLightSet_1.Value = Convert.ToInt32(Light.LightSet_1);
            nudLightSet_2.Value = Convert.ToInt32(Light.LightSet_2);
            nudLightSet_3.Value = Convert.ToInt32(Light.LightSet_3);
            nudLightSet_4.Value = Convert.ToInt32(Light.LightSet_4);
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

        private void btnOneShot_Click(object sender, EventArgs e)
        {
            parent.OneShot();
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

        private void btnStop_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
        }

        private void btnImageProPlus_Click(object sender, EventArgs e)
        {
            ImageProPlus(hWindowControl1.HalconWindow,My.ho_Image,Tray.n,Tray.CurrentRow,Tray.CurrentColumn);
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

                My.ho_Image = readImage;
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
                HOperatorSet.RotateImage(ho_Image, out ExpTmpOutVar_0, 270, "constant");
                ho_Image.Dispose();
                ho_Image = ExpTmpOutVar_0;
            }
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height); //得到他的大小
            HOperatorSet.SetWindowAttr("background_color", "black");
            //调整窗口显示大小以适应图片，这句一定要加上，不然图片显示的不全
            HOperatorSet.SetPart(hv_ExpDefaultWinHandle, 0, 0, hv_Height - 1, hv_Width - 1);
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle); //将图像在该窗口进行显示
            return ho_Image; //返回这个图像
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
            tbBlackToWhite.Enabled = true;
            nudBlackToWhite.Enabled = true;
            tbWhiteToBlack.Enabled = true;
            nudWhiteToBlack.Enabled = true;
        }

        private void tbDetectionRadius_ValueChanged(object sender, EventArgs e)
        {
            nudDetectionRadius.Value = tbDetectionRadius.Value;
        }

        private void nudDetectionRadius_ValueChanged(object sender, EventArgs e)
        {
            dReduceRadius = tbDetectionRadius.Value = Convert.ToInt32(nudDetectionRadius.Value);
            try
            {
                ho_Image = My.ho_Image;
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, dReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                HOperatorSet.DispObj(ho_ReducedImage, hWindowControl1.HalconWindow);
                Thread.Sleep(1);
            }
            catch
            {
            }
        }

        private void tbGraythreshold_ValueChanged(object sender, EventArgs e)
        {
            nudGraythreshold.Value = tbGraythreshold.Value;
        }

        private void nudGraythreshold_ValueChanged(object sender, EventArgs e)
        {
            dGraythreshold = tbGraythreshold.Value = Convert.ToInt32(nudGraythreshold.Value);
            try
            {
                HOperatorSet.GenEmptyObj(out ho_Region);
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                //畫檢測區域
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, dReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_Image = ho_ReducedImage;

                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                ho_Region.Dispose();
                if(My.VDI.DarkLightChoice==0)
                    HOperatorSet.Threshold(ho_Image, out ho_Region, tbGraythreshold.Value,255);
                else
                    HOperatorSet.Threshold(ho_Image, out ho_Region, 0, tbGraythreshold.Value);
                HOperatorSet.DispObj(ho_Region, hv_ExpDefaultWinHandle);
            }
            catch
            {
                //MessageBox.Show("error");
            }
            Thread.Sleep(1);
        }

        private void tbLength_ValueChanged(object sender, EventArgs e)
        {
            nudLength.Value = tbLength.Value;
        }

        private void nudLength_ValueChanged(object sender, EventArgs e)
        {
            dLength = tbLength.Value = Convert.ToInt32(nudLength.Value);
            try
            {
                //這段要另外用的
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image,out My.VDI.mmpixel,out My.VDI.dResultRow,out My.VDI.dResultColumn);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void tbBlackToWhite_ValueChanged(object sender, EventArgs e)
        {
            nudBlackToWhite.Value = tbBlackToWhite.Value;
        }

        private void nudBlackToWhite_ValueChanged(object sender, EventArgs e)
        {
            tbWhiteToBlack.Value = 1;
            dMeasureThreshold = tbBlackToWhite.Value = Convert.ToInt32(nudBlackToWhite.Value);
            sGenParamValue = "positive";
            try
            {
                //這段要另外用的
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image, out My.VDI.mmpixel, out My.VDI.dResultRow, out My.VDI.dResultColumn);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void tbWhiteToBlack_ValueChanged(object sender, EventArgs e)
        {
            nudWhiteToBlack.Value = tbWhiteToBlack.Value;
        }

        private void nudWhiteToBlack_ValueChanged(object sender, EventArgs e)
        {
            tbBlackToWhite.Value = 1;
            dMeasureThreshold = tbWhiteToBlack.Value = Convert.ToInt32(nudWhiteToBlack.Value);
            sGenParamValue = "negative";
            try
            {
                //這段要另外用的
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image, out My.VDI.mmpixel, out My.VDI.dResultRow, out My.VDI.dResultColumn);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        public void CatchCenter(HWindow Window, HObject theImage, out double mmPixel, out double ResultRow, out double ResultColumn)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_ReducedImage);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_Connection);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions0);
            HOperatorSet.GenEmptyObj(out ho_ModelContour);
            HOperatorSet.GenEmptyObj(out ho_MeasureContour);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_MissConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_MissSelectedRegions);
            try
            {
                ho_Image = theImage;
                hv_ExpDefaultWinHandle = Window;
                //畫檢視範圍
                Window.ClearWindow();
                HOperatorSet.SetColor(Window, "green");
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                ho_Circle.Dispose();
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //檢查是否無料
                hv_MissGray = dMissGray;
                ho_MissRegion.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_MissRegion, 0, hv_MissGray);
                ho_MissConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_MissRegion, out ho_MissConnectedRegions);
                hv_MissAreaSet = dMissArea;
                ho_MissSelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_MissConnectedRegions, out ho_MissSelectedRegions,
                    "area", "and", hv_MissAreaSet, 9999999);
                HOperatorSet.CountObj(ho_MissSelectedRegions, out hv_MissNumber);
                double[] Lab = {};
                //如果不是無料才進行顏色判別
                if (hv_MissNumber <= 4 && hv_MissNumber >= 2)
                {
                    set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1400, 100);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                    Miss = true;
                    mmPixel = 0.0044;
                    ResultRow = 0;
                    ResultColumn = 0;
                }
                else
                {

                    hv_FirstRadius = dFirstCircleRadius;
                    hv_GraySetting = dGraythreshold;
                    if (My.VDI.DarkLightChoice == 0)
                        HOperatorSet.Threshold(ho_Image, out ho_Region, hv_GraySetting, 255);
                    else
                        HOperatorSet.Threshold(ho_Image, out ho_Region, 0, hv_GraySetting);
                    ho_RegionOpening.Dispose();
                    HOperatorSet.OpeningCircle(ho_Region, out ho_RegionOpening, 5);
                    ho_Connection.Dispose();
                    HOperatorSet.Connection(ho_RegionOpening, out ho_Connection);

                    ho_SelectedRegions0.Dispose();
                    HOperatorSet.SelectShape(ho_Connection, out ho_SelectedRegions0, "outer_radius", "and", 0, hv_FirstRadius + 50);
                    HOperatorSet.AreaCenter(ho_SelectedRegions0, out hv_Area0, out hv_Row0, out hv_Column0);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SelectShape(ho_SelectedRegions0, out ExpTmpOutVar_0, "area", "and",
                            hv_Area0.TupleMax(), 99999999);
                        ho_SelectedRegions0.Dispose();
                        ho_SelectedRegions0 = ExpTmpOutVar_0;
                    }
                    HOperatorSet.AreaCenter(ho_SelectedRegions0, out hv_Area0, out hv_Row0, out hv_Column0);
                    //找出圓心
                    ////不追隨,以畫面中心當圓心
                    //hv_Width2 = hv_Width / 2;
                    //hv_Height2 = hv_Height / 2;
                    //HOperatorSet.DispLine(hv_ExpDefaultWinHandle, hv_Height2, 0, hv_Height2, hv_Width);
                    //HOperatorSet.DispLine(hv_ExpDefaultWinHandle, 0, hv_Width2, hv_Height, hv_Width2);
                    //hv_Row0 = hv_Height2;
                    //hv_Column0 = hv_Width2;

                    HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                    HOperatorSet.AddMetrologyObjectGeneric(hv_MetrologyHandle, "circle", ((hv_Row0.TupleConcat(
                        hv_Column0))).TupleConcat(hv_FirstRadius), 25, 5, 1, 30, new HTuple(),
                        new HTuple(), out hv_circleIndices);
                    ho_ModelContour.Dispose();
                    HOperatorSet.GetMetrologyObjectModelContour(out ho_ModelContour, hv_MetrologyHandle, "all", 1.5);
                    ho_MeasureContour.Dispose();
                    HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureContour, hv_MetrologyHandle, "all", "all", out hv_Row, out hv_Column);
                    //set_metrology_model_param (MetrologyHandle, 'reference_system', [RowRefer, ColRefer, 0])
                    //方向由內而外,白找黑('negative')或黑找白('positive')
                    hv_GenParamValue = sGenParamValue;
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices, "measure_transition", hv_GenParamValue);
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices, "measure_select", "last");
                    //長度
                    hv_Length1 = dLength;
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices, "measure_length1", hv_Length1);
                    //寬度
                    hv_Width1 = 5;
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices, "measure_length2", hv_Width1);
                    //灰度差異
                    hv_Measure_Threshold = dMeasureThreshold;
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices, "measure_threshold", hv_Measure_Threshold);
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices, "min_score", 0.2);
                    HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                    ho_Contour.Dispose();
                    HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour, hv_MetrologyHandle, "all", "positive", out hv_Row, out hv_Column);
                    HOperatorSet.DispObj(ho_Contour, hv_ExpDefaultWinHandle);
                    HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_circleIndices, "all", "result_type", "all_param", out hv_circleParameter);
                    ho_CrossCenter.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_CrossCenter, hv_circleParameter.TupleSelect(0), hv_circleParameter.TupleSelect(1), 20, 0.785398);
                    ho_Contours.Dispose();
                    HOperatorSet.GetMetrologyObjectResultContour(out ho_Contours, hv_MetrologyHandle, "all", "all", 1.5);
                    ho_Contour.Dispose();
                    HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour, hv_MetrologyHandle,
                        "all", "all", out hv_Row1, out hv_Column1);
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row1, hv_Column1, 6, 0.785398);
                    HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "used_edges", "row", out hv_UsedRow);
                    HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "used_edges", "column", out hv_UsedColumn);
                    ho_UsedEdges.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_UsedEdges, hv_UsedRow, hv_UsedColumn,
                        10, (new HTuple(45)).TupleRad());
                    HOperatorSet.DispObj(ho_UsedEdges, hv_ExpDefaultWinHandle);
                    ho_ResultContours.Dispose();
                    HOperatorSet.GetMetrologyObjectResultContour(out ho_ResultContours, hv_MetrologyHandle,
                        "all", "all", 1.5);
                    HOperatorSet.DispObj(ho_ResultContours, hv_ExpDefaultWinHandle);
                    HOperatorSet.FitCircleContourXld(ho_ResultContours, "algebraic", -1, 0, 0,
                        3, 2, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius, out hv_StartPhi,
                        out hv_EndPhi, out hv_PointOrder);
                    HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                    
                     mmPixel = My.VDI.AimCirR / hv_ResultRadius / 2;

                    ResultRow = hv_circleParameter.TupleSelect(0);
                    ResultColumn = hv_circleParameter.TupleSelect(1);
                    Miss = false;
                }
            }
            catch
            {
                Miss = true;
                mmPixel = 0.0044;
                ResultRow = 0;
                ResultColumn = 0;
            }

        }

        private void btnCircleCenter_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image, out My.VDI.mmpixel, out My.VDI.dResultRow, out My.VDI.dResultColumn);

                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "blue");
                HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_ResultRow, hv_ResultColumn, hv_ResultRadius);
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_ResultRow, hv_ResultColumn, 16,
                        0.785398);
                HOperatorSet.DispObj(My.ho_Image, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Circle, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Cross, hWindowControl1.HalconWindow);
            }
            catch
            {

            }
        }

        private void btnCenterSave_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            My.dFirstCircleRadius = dFirstCircleRadius;
            My.dReduceRadius = dReduceRadius;
            My.dGraythreshold = dGraythreshold;
            My.dLength = dLength;
            My.dMeasureThreshold = dMeasureThreshold;
            My.sGenParamValue = sGenParamValue;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "FirstCircleRadius", My.dFirstCircleRadius.ToString(), Path);
            IniFile.Write("Setting", "ReduceRadius", My.dReduceRadius.ToString(), Path);
            IniFile.Write("Setting", "Graythreshold", My.dGraythreshold.ToString(), Path);
            IniFile.Write("Setting", "Length", My.dLength.ToString(), Path);
            IniFile.Write("Setting", "MeasureThreshold", My.dMeasureThreshold.ToString(), Path);
            IniFile.Write("Setting", "GenParamValue", My.sGenParamValue.ToString(), Path);
        }

        private void tbFirstRadius_ValueChanged(object sender, EventArgs e)
        {
            nudFirstRadius.Value = tbFirstRadius.Value;
        }

        private void btnCircleCenter1_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image, out My.VDI.mmpixel, out My.VDI.dResultRow, out My.VDI.dResultColumn);

                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "blue");
                HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_ResultRow, hv_ResultColumn, hv_ResultRadius);
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_ResultRow, hv_ResultColumn, 16,
                        0.785398);
                HOperatorSet.DispObj(My.ho_Image, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Circle, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Cross, hWindowControl1.HalconWindow);

                txtPpix.Text = (Math.Round(My.VDI.mmpixel,5)).ToString();
            }
            catch
            {
            }

        }

        private void nudFirstRadius_ValueChanged(object sender, EventArgs e)
        {
            dFirstRadius2 = tbFirstRadius.Value = Convert.ToInt32(nudFirstRadius.Value);
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            try
            {
                ho_Image = My.ho_Image;
                 hWindowControl1.HalconWindow.ClearWindow();

                 HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                 ho_Circle.Dispose();
                 HOperatorSet.GenCircle(out ho_Circle, My.VDI.dResultRow, My.VDI.dResultColumn, dFirstRadius2);
                 HOperatorSet.DispObj(ho_Circle, hWindowControl1.HalconWindow);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void tbLength2_ValueChanged(object sender, EventArgs e)
        {
            nudLength2.Value = tbLength2.Value;
        }

        private void nudLength2_ValueChanged(object sender, EventArgs e)
        {
            dLength2 = tbLength2.Value = Convert.ToInt32(nudLength2.Value);
            try
            {
                //這段要另外用的
                CatchCenter2(hWindowControl1.HalconWindow, My.ho_Image,My.VDI.dResultRow, My.VDI.dResultColumn, out My.VDI.dResultRow2, out My.VDI.dResultColumn2, out My.VDI.dResultRadius2);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void tbBlackToWhite2_ValueChanged(object sender, EventArgs e)
        {
            nudBlackToWhite2.Value = tbBlackToWhite2.Value;
        }

        private void nudBlackToWhite2_ValueChanged(object sender, EventArgs e)
        {
            tbWhiteToBlack2.Value = 1;
            dMeasureThreshold2 = tbBlackToWhite2.Value = Convert.ToInt32(nudBlackToWhite2.Value);
            sGenParamValue2 = "positive";
            try
            {
                //這段要另外用的
                CatchCenter2(hWindowControl1.HalconWindow, My.ho_Image, My.VDI.dResultRow, My.VDI.dResultColumn, out My.VDI.dResultRow2, out My.VDI.dResultColumn2, out My.VDI.dResultRadius2);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void tbWhiteToBlack2_ValueChanged(object sender, EventArgs e)
        {
            nudWhiteToBlack2.Value = tbWhiteToBlack2.Value;
        }

        private void nudWhiteToBlack2_ValueChanged(object sender, EventArgs e)
        {
            tbBlackToWhite2.Value = 1;
            dMeasureThreshold2 = tbWhiteToBlack2.Value = Convert.ToInt32(nudWhiteToBlack2.Value);
            sGenParamValue2 = "negative";
            try
            {
                //這段要另外用的
                CatchCenter2(hWindowControl1.HalconWindow, My.ho_Image, My.VDI.dResultRow, My.VDI.dResultColumn, out My.VDI.dResultRow2, out My.VDI.dResultColumn2, out My.VDI.dResultRadius2);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        public void ImageProPlus(HWindow hWindowControl,HObject theImage,int n,int CurrentRow,int CurrentColumn)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Circle2);
            HOperatorSet.GenEmptyObj(out ho_Cross2);
            double mmpixel = 0.0044;
            double ResultRow = 0;
            double ResultColumn = 0;
            double ResultRow2 = 0;
            double ResultColumn2 = 0;
            double ResultRadius2 = 0;
            try
            {
                //抓圓心
                
                CatchCenter(hWindowControl, theImage,out mmpixel, out ResultRow, out ResultColumn);
                HOperatorSet.DispObj(theImage, hWindowControl);
                if(!Miss)
                {
                    Thread.Sleep(10);
                    CatchCenter2(hWindowControl, theImage,ResultRow, ResultColumn, out ResultRow2, out ResultColumn2, out ResultRadius2);
                    HOperatorSet.SetDraw(hWindowControl, "margin");
                    ho_Circle.Dispose();
                    HOperatorSet.GenCircle(out ho_Circle, ResultRow, ResultColumn, hv_ResultRadius);
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, ResultRow, ResultColumn, 16,0.785398);
                    ho_Circle2.Dispose();
                    HOperatorSet.GenCircle(out ho_Circle2, ResultRow2, ResultColumn2, ResultRadius2);
                    ho_Cross2.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross2, ResultRow2, ResultColumn2, 16,0.785398);

                    HOperatorSet.DispObj(theImage, hWindowControl);
                    HOperatorSet.SetColor(hWindowControl, "blue");
                    HOperatorSet.DispObj(ho_Circle, hWindowControl);
                    HOperatorSet.DispObj(ho_Cross, hWindowControl);
                    HOperatorSet.SetColor(hWindowControl, "green");
                    HOperatorSet.DispObj(ho_Circle2, hWindowControl);
                    HOperatorSet.DispObj(ho_Cross2, hWindowControl);
                }                
            }
            catch
            {
            }
            //計算Result
            ImagePro(hWindowControl, theImage, n,mmpixel,ResultRow,ResultColumn,ResultRow2,ResultColumn2,CurrentRow,CurrentColumn);
        }

        public void ImagePro(HWindow Window,HObject theImage,int n,double mmpixel,double ResultRow,double ResultColumn,double ResultRow2,double ResultColumn2,int CurrentRow,int CurrentColumn)
        {
            double dResultDiam = 0;
            double dDiamMin = 0;
            try
            {
                
                if (Miss)
                {
                    set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 200);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");

                    Vision.VisionResult[n] = "Miss";
                    //擷取當前畫面圖片為Image
                    HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                }
                else
                {
                    //My.VDI.mmpixel = 0.0044;
                    
                    HOperatorSet.DistancePp(ResultRow, ResultColumn, ResultRow2,ResultColumn2, out hv_Distance1);
                    //直徑要*2
                    dResultDiam = Math.Round((double)hv_ResultRadius2 * mmpixel*2, 5)-My.VDI.dNegativeOffSet;
                    dDiamMin = dResultDiam - Math.Round((2 * (double)hv_Distance1 * mmpixel), 5);

                    if (My.VDI.DiamMin < My.VDI.dCoatRMin)
                    {
                        dResultDiam = dResultDiam + My.VDI.dPositiveOffSet;
                        dDiamMin = dResultDiam - Math.Round((2 * (double)hv_Distance1 * mmpixel), 5);
                    }
                    txtDiamMin.Text = My.VDI.DiamMin.ToString();
                    set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                    

                //if (Tray.NowTray == 1)
                //    {
                //        CurrentRow = (int)Math.Floor((double)(n / Tray.Columns_1)) + 1;//Math.Floor無條件捨去
                //        CurrentColumn = n % Tray.Columns_1 + 1;
                //    }
                //    else
                //    {
                //        CurrentRow = (int)Math.Floor((double)(n / Tray.Columns_2)) + 1;
                //        CurrentColumn = n % Tray.Columns_2;
                //    }
                    string BarrelBarcode = "";
                    if (Sys.ReadBarcodeLog)
                    {
                        if (DataBank.ResultBarcode[CurrentRow - 1,CurrentColumn - 1 ] == null)
                            DataBank.ResultBarcode[CurrentRow - 1,CurrentColumn - 1 ] = "null";
                        BarrelBarcode = DataBank.ResultBarcode[CurrentRow - 1,CurrentColumn - 1 ];
                    }
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    //HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    //HOperatorSet.DispObj(ho_ResultContours2, hv_ExpDefaultWinHandle);
                    set_display_font(hv_ExpDefaultWinHandle, 20, "mono", "true", "false");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Diam:" + dResultDiam + "mm");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "DiamMin:" + dDiamMin + "mm");
                    if (Sys.ReadBarcodeLog)
                    {
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 300, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Barcode:" + BarrelBarcode);
                    }
                    //HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 0);
                    //HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "TopperDiam:" + Math.Round((double)hv_ResultRadius * My.VDI.mmpixel * 2, 5) + "mm");
                    set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                    if (Sys.ReadBarcodeLog)
                    {
                        if (BarrelBarcode != "null")
                        {
                            if (dDiamMin >= My.VDI.dCoatRMin)
                            {
                                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 200);
                                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "OK");

                                Vision.VisionResult[n] = "OK";
                                //擷取當前畫面圖片為Image
                                HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                            }
                            else
                            {
                                Vision.VisionResult[n] = "NG";
                                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 200);
                                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "NG");
                                //擷取當前畫面圖片為Image
                                HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                            }
                        }
                        else
                        {
                            Vision.VisionResult[n] = "NG2";
                            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                            HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 200);
                            HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "NG:  Barrel Barcode = null");
                            //擷取當前畫面圖片為Image
                            HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                        }
                    }
                    else
                    {
                        if (dDiamMin >= My.VDI.dCoatRMin)
                        {
                            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                            HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 200);
                            HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "OK");

                            Vision.VisionResult[n] = "OK";
                            //擷取當前畫面圖片為Image
                            HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                        }
                        else
                        {
                            Vision.VisionResult[n] = "NG";
                            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                            HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 200);
                            HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "NG");
                            //擷取當前畫面圖片為Image
                            HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                        }
                    }
                }
            }
            catch
            {
                set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 200);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                //擷取當前畫面圖片為Image
                HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                Vision.VisionResult[n] = "Miss";
            }
            
            if (Tray.NowTray == 1)
            {
                Vision.Images_1[n] = ho_Image;
                Vision.Images_Now[n] = Vision.Images_1[n];
                Vision.ImagesOriginal_1[n] = theImage;
            }
            else if (Tray.NowTray == 2)
            {
                Vision.Images_2[n] = ho_Image;
                Vision.Images_Now[n] = Vision.Images_2[n];
                Vision.ImagesOriginal_2[n] = theImage;
            }
            WriteLog(n, Vision.VisionResult[n], My.VDI.dCoatRMin, dResultDiam, My.VDI.DiamMinSet, dDiamMin,CurrentRow,CurrentColumn);
        }

        private void btnCalO1O2_Click(object sender, EventArgs e)
        {
            ImagePro(hWindowControl1.HalconWindow, My.ho_Image, Tray.n, My.VDI.mmpixel, My.VDI.dResultRow, My.VDI.dResultColumn, My.VDI.dResultRow2, My.VDI.dResultColumn2,Tray.CurrentRow,Tray.CurrentColumn);
        }

        private void btnCircleCenter2_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            hWindowControl1.HalconWindow.ClearWindow();
            try
            {
                CatchCenter2(hWindowControl1.HalconWindow, My.ho_Image, My.VDI.dResultRow, My.VDI.dResultColumn, out My.VDI.dResultRow2, out My.VDI.dResultColumn2, out My.VDI.dResultRadius2);
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "green");
                HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_ResultRow2, hv_ResultColumn2, hv_ResultRadius2);
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_ResultRow2, hv_ResultColumn2, 16,
                        0.785398);
                HOperatorSet.DispObj(My.ho_Image, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Circle, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Cross, hWindowControl1.HalconWindow);
            }
            catch
            {
            }
        }

        private void btnSaveDVISet_Click(object sender, EventArgs e)
        {
            My.VDI.dFirstCircleRadius2 = dFirstRadius2;
            My.VDI.dMeasureThreshold2 = dMeasureThreshold2;
            My.VDI.dLength2 = dLength2;
            My.VDI.sGenParamValue2 = sGenParamValue2;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "AimCirR", My.VDI.AimCirR.ToString(), Path);
            IniFile.Write("Setting", "CoatRMin", My.VDI.dCoatRMin.ToString(), Path);
            IniFile.Write("Setting", "FirstCircleRadius2", My.VDI.dFirstCircleRadius2.ToString(), Path);
            IniFile.Write("Setting", "MeasureThreshold2", My.VDI.dMeasureThreshold2.ToString(), Path);
            IniFile.Write("Setting", "Length2", My.VDI.dLength2.ToString(), Path);
            IniFile.Write("Setting", "GenParamValue2", My.VDI.sGenParamValue2.ToString(), Path);

        }
        public void CatchCenter2(HWindow Window,HObject theImage,double ResultRow,double ResultColumn,out double ResultRow2,out double ResultColumn2,out double ResultRadius2)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_ReducedImage);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_Connection);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions0);
            HOperatorSet.GenEmptyObj(out ho_ModelContour);
            HOperatorSet.GenEmptyObj(out ho_MeasureContour);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_ImageMedian);
            HOperatorSet.GenEmptyObj(out ho_ModelContour2);
            HOperatorSet.GenEmptyObj(out ho_MeasureContour2);
            HOperatorSet.GenEmptyObj(out ho_Contour2);
            HOperatorSet.GenEmptyObj(out ho_Contours2);
            HOperatorSet.GenEmptyObj(out ho_Cross2);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges2);
            HOperatorSet.GenEmptyObj(out ho_ResultContours2);
            try
            {
                Window.ClearWindow();
                hv_ExpDefaultWinHandle = Window;
                ho_Image = theImage;
                hv_Row0 = ResultRow;
                hv_Column0 = ResultColumn;
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                //找出鍍膜黃色邊
                hv_FirstRadius2 = dFirstRadius2;
                //中值濾波
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianImage(ho_Image, out ho_ImageMedian, "circle", 3, "mirrored");
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle2);
                HOperatorSet.AddMetrologyObjectGeneric(hv_MetrologyHandle2, "circle", ((hv_Row0.TupleConcat(
                    hv_Column0))).TupleConcat(hv_FirstRadius2), 25, 5, 1, 30, new HTuple(), new HTuple(), out hv_circleIndices2);
                ho_ModelContour2.Dispose();
                HOperatorSet.GetMetrologyObjectModelContour(out ho_ModelContour2, hv_MetrologyHandle2, "all", 1.5);
                ho_MeasureContour2.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureContour2, hv_MetrologyHandle2, "all", "all", out hv_Row2, out hv_Column2);

                //set_metrology_model_param (MetrologyHandle2, 'reference_system', [RowRefer, ColRefer, 0])
                //方向由內而外,白找黑('negative')或黑找白('positive')
                hv_GenParamValue = sGenParamValue2;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle2, hv_circleIndices, "measure_transition", hv_GenParamValue);
                //第幾點由內而外第一點('first')或最後一點('last')
                hv_GenParamValue2 = "first";
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle2, hv_circleIndices, "measure_select", hv_GenParamValue2);
                //長度
                hv_Length1 = dLength2;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle2, hv_circleIndices, "measure_length1", hv_Length1);
                //寬度
                hv_Width1 = 5;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle2, hv_circleIndices, "measure_length2", hv_Width1);
                //灰度差異
                hv_Measure_Threshold = dMeasureThreshold2;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle2, hv_circleIndices, "measure_threshold", hv_Measure_Threshold);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle2, hv_circleIndices, "min_score", 0.2);
                HOperatorSet.ApplyMetrologyModel(ho_ImageMedian, hv_MetrologyHandle2);
                ho_Contour2.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour2, hv_MetrologyHandle2, "all", "positive", out hv_Row2, out hv_Column2);
                HOperatorSet.DispObj(ho_Contour2, hv_ExpDefaultWinHandle);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle2, hv_circleIndices, "all", "result_type", "all_param", out hv_circleParameter2);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                ho_CrossCenter.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_CrossCenter, hv_circleParameter2.TupleSelect(0), hv_circleParameter2.TupleSelect(1), 20, 0.785398);
                ho_Contours2.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_Contours2, hv_MetrologyHandle2, "all", "all", 1.5);
                ho_Contour2.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour2, hv_MetrologyHandle2, "all", "all", out hv_Row2, out hv_Column2);
                ho_Cross2.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_Row2, hv_Column2, 6, 0.785398);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle2, "all", "all", "used_edges", "row", out hv_UsedRow2);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle2, "all", "all", "used_edges", "column", out hv_UsedColumn2);
                ho_UsedEdges2.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_UsedEdges2, hv_UsedRow2, hv_UsedColumn2, 10, (new HTuple(45)).TupleRad());
                HOperatorSet.DispObj(ho_UsedEdges2, hv_ExpDefaultWinHandle);
                ho_ResultContours2.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_ResultContours2, hv_MetrologyHandle2, "all", "all", 1.5);
                HOperatorSet.DispObj(ho_ResultContours2, hv_ExpDefaultWinHandle);
                HOperatorSet.FitCircleContourXld(ho_ResultContours2, "algebraic", -1, 0, 0, 3, 2,
                    out hv_ResultRow2, out hv_ResultColumn2, out hv_ResultRadius2, out hv_StartPhi2,
                    out hv_EndPhi2, out hv_PointOrder2);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle2);

                ResultRow2 = hv_ResultRow2;
                ResultColumn2 = hv_ResultColumn2;
                ResultRadius2 = hv_ResultRadius2;
            }
            catch
            {
                ResultRow2 = 0;
                ResultColumn2 = 0;
                ResultRadius2 = 0;
            }
        }

        public void WriteLog(int n, string ResultOK, double DiamSet, double ResultDiam, double DiamMinSet, double DiamMin,int CurrentRow,int CurrentColumn)
        {
            if (Plc.Status == 1)
            {
                string Path = Sys.LogPath  + "\\" + Tray.OpDateTime.ToString("yyyyMMdd");
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
                    
                    if (Tray.NowTray == 1)
                    {
                        Barcode = Tray.Barcode_1;
                    }
                    else if (Tray.NowTray == 2)
                    {
                        Barcode = Tray.Barcode_2;
                    }
                    //int CurrentRow = 0;
                    //int CurrentColumn = 0;
                    ////反推行列
                    //if (Tray.NowTray == 1)
                    //{
                    //    CurrentRow = (int)Math.Floor((double)(n / Tray.Columns_1)) + 1;//Math.Floor無條件捨去
                    //    CurrentColumn = n % Tray.Columns_1 + 1;
                    //}
                    //else
                    //{
                    //    CurrentRow = (int)Math.Floor((double)(n / Tray.Columns_2)) + 1;
                    //    CurrentColumn = n % Tray.Columns_2;
                    //}
                    string BarrelBarcode = "";
                    if (Sys.ReadBarcodeLog)
                    {
                        if (DataBank.ResultBarcode[CurrentRow - 1,CurrentColumn - 1 ] == null)
                            DataBank.ResultBarcode[CurrentRow - 1,CurrentColumn - 1 ] = "null";
                        BarrelBarcode = DataBank.ResultBarcode[CurrentRow - 1,CurrentColumn - 1 ];
                    }
                    //不存在文件，创建先

                    if (!File.Exists(Log))
                    {
                        File.WriteAllText(Log, "Function\tCode\tTray Barcode_B\tTray No.\tToTal Count\tTray Barcode_A\tProgram ID\tProduct\tClass\tOperatorID\tMachine No.\tTime\tCT\tResult\tCoatRMin\tCoatRMax\tResultDiam\tDiamMin\tBarrel Barcode No." +
                                         "\r\n");
                    }
                    if (ResultOK == "Miss")
                    {
                        My.VDI.dResultDiam = 0;
                        My.VDI.DiamMin = 0;
                    }
                    //写result
                    using (StreamWriter sw = new StreamWriter(Log, true))
                    {
                        sw.WriteLine(Sys.FunctionString + "\t" + Sys.Codes + "\t" + Barcode + "\t" +
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
                                     My.VDI.dCoatRMin.ToString("f5") + "\t" +
                                     My.VDI.dCoatRMax.ToString("f5") + "\t" +
                                     ResultDiam.ToString("f5") + "\t" +
                                     DiamMin.ToString("f5") + "\t" +
                                     BarrelBarcode);
                    }
                }
                catch
                {
                }
            }
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

        private void btnOn_1_Click(object sender, EventArgs e)
        {
            ReverseOnOff(btnOn_1);
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

        private void btnOn_2_Click(object sender, EventArgs e)
        {
            ReverseOnOff(btnOn_2);
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

        private void btnOn_3_Click(object sender, EventArgs e)
        {
            ReverseOnOff(btnOn_3);
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

        private void btnOn_4_Click(object sender, EventArgs e)
        {
            ReverseOnOff(btnOn_4);
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

        private void btnLightSave_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("SettingLight", "Light1", tbLightSet_1.Value.ToString(), Path);
            IniFile.Write("SettingLight", "Light2", tbLightSet_2.Value.ToString(), Path);
            IniFile.Write("SettingLight", "Light3", tbLightSet_3.Value.ToString(), Path);
            IniFile.Write("SettingLight", "Light4", tbLightSet_4.Value.ToString(), Path);
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
            if (Tray.NowTray == 1)
            {
                if (Tray.Barcode_1 != "")
                    TrayBarcode = Tray.Barcode_1;
            }
            else if (Tray.NowTray == 2)
            {
                if (Tray.Barcode_2 != "")
                    TrayBarcode = Tray.Barcode_2;
            }
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

        private void btnRevise_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image, out My.VDI.mmpixel, out My.VDI.dResultRow, out My.VDI.dResultColumn);

                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "blue");
                HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_ResultRow, hv_ResultColumn, hv_ResultRadius);
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_ResultRow, hv_ResultColumn, 16,
                        0.785398);
                HOperatorSet.DispObj(My.ho_Image, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Circle, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Cross, hWindowControl1.HalconWindow);

                txtPpix.Text = (Math.Round(My.VDI.mmpixel, 5)).ToString();
            }
            catch
            {
            }
        }

        private void btnSaveDVISet2_Click(object sender, EventArgs e)
        {

        }

        private void nudCoatRMin_ValueChanged(object sender, EventArgs e)
        {
            My.VDI.dCoatRMin = (double)nudCoatRMin.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "CoatRMin", My.VDI.dCoatRMin.ToString(), Path);
        }

        private void nudCoatRMax_ValueChanged(object sender, EventArgs e)
        {
            My.VDI.dCoatRMax = (double)nudCoatRMax.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "CoatRMax", My.VDI.dCoatRMax.ToString(), Path);
        }

        private void nudNegativeOffSet_ValueChanged(object sender, EventArgs e)
        {
            My.VDI.dNegativeOffSet = (double)nudNegativeOffSet.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "NegativeOffSet", My.VDI.dNegativeOffSet.ToString(), Path);
        }

        private void nudPositiveOffSet_ValueChanged(object sender, EventArgs e)
        {
            My.VDI.dPositiveOffSet = (double)nudPositiveOffSet.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "PositiveOffSet", My.VDI.dPositiveOffSet.ToString(), Path);
        }

        private void nudAimCirR_ValueChanged(object sender, EventArgs e)
        {
            My.VDI.AimCirR = (double)nudAimCirR.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "AimCirR", My.VDI.AimCirR.ToString(), Path);

        }

        private void tbMissGray_ValueChanged(object sender, EventArgs e)
        {
            nudMissGray.Value = tbMissGray.Value;
        }

        private void nudMissGray_ValueChanged(object sender, EventArgs e)
        {
            dMissGray = tbMissGray.Value = Convert.ToInt32(nudMissGray.Value);
            try
            {
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
                HOperatorSet.GenEmptyObj(out ho_MissRegion);
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                //畫檢測區域
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, dReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_Image = ho_ReducedImage;

                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                
                hv_MissGray = dMissGray;
                ho_MissRegion.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_MissRegion, 0, hv_MissGray);
                HOperatorSet.DispObj(ho_MissRegion, hv_ExpDefaultWinHandle);
            }
            catch
            {
                //MessageBox.Show("error");
            }
            Thread.Sleep(1);
        }

        private void tbMissArea_ValueChanged(object sender, EventArgs e)
        {
            nudMissArea.Value = tbMissArea.Value;
        }

        private void nudMissArea_ValueChanged(object sender, EventArgs e)
        {
            dMissArea = tbMissArea.Value = Convert.ToInt32(nudMissArea.Value);
            try
            {
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
                HOperatorSet.GenEmptyObj(out ho_MissRegion);
                HOperatorSet.GenEmptyObj(out ho_MissConnectedRegions);
                HOperatorSet.GenEmptyObj(out ho_MissSelectedRegions);
                HOperatorSet.GenEmptyObj(out ho_MissRegion);
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                //畫檢測區域
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, dReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_Image = ho_ReducedImage;

                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                hv_MissGray = dMissGray;
                ho_MissRegion.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_MissRegion, 0, hv_MissGray);
                ho_MissConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_MissRegion, out ho_MissConnectedRegions);
                hv_MissAreaSet = dMissArea;
                ho_MissSelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_MissConnectedRegions, out ho_MissSelectedRegions,
                    "area", "and", hv_MissAreaSet, 9999999);
                HOperatorSet.DispObj(ho_MissSelectedRegions, hv_ExpDefaultWinHandle);
            }
            catch
            {
                //MessageBox.Show("error");
            }
            Thread.Sleep(1);
        }

        private void btnSave4_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            My.VDI.dMissGray = dMissGray;
            My.VDI.dMissArea = dMissArea;
            My.VDI.dMissRadius = dMissRadius;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "MissGray", My.VDI.dMissGray.ToString(), Path);
            IniFile.Write("Setting", "MissArea", My.VDI.dMissArea.ToString(), Path);
        }

        private void cbDarkLightChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
        if (cbDarkLightChoice.SelectedIndex < 0)
                return;
            My.VDI.DarkLightChoice = cbDarkLightChoice.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "DarkLightChoice", My.VDI.DarkLightChoice.ToString(), Path);
        }

    }
}
