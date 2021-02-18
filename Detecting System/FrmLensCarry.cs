using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;
using UC_FirCircle;
using System.IO;

namespace Detecting_System
{
    public partial class FrmLensCarry : Form
    {
        FrmParent parent;
        FrmRun Run;
        UC_FitCircleTool ucFitCircleTool_CircleCenter;
        UC_FitCircleTool ucFitCircleTool_Correction;
        UC_FitRectangle2Tool ucFitRectangle2Tool_Angle1;
        public FrmLensCarry(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;
            InitializeComponent();
            Reconnect();
        }

        bool bReadingPara = false;
        bool bDrawing = false;
        
        #region Halcon參數1
        public void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
          HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {
            // Local control variables 
            HTuple hv_Red, hv_Green, hv_Blue, hv_Row1Part;
            HTuple hv_Column1Part, hv_Row2Part, hv_Column2Part, hv_RowWin;
            HTuple hv_ColumnWin, hv_WidthWin, hv_HeightWin, hv_MaxAscent;
            HTuple hv_MaxDescent, hv_MaxWidth, hv_MaxHeight, hv_R1 = new HTuple();
            HTuple hv_C1 = new HTuple(), hv_FactorRow = new HTuple(), hv_FactorColumn = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple(), hv_Ascent = new HTuple();
            HTuple hv_Descent = new HTuple(), hv_W = new HTuple(), hv_H = new HTuple();
            HTuple hv_FrameHeight = new HTuple(), hv_FrameWidth = new HTuple();
            HTuple hv_R2 = new HTuple(), hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_CurrentColor = new HTuple();

            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

            // Initialize local and output iconic variables 

            //This procedure displays text in a graphics window.        
            //prepare window
            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part,
                out hv_Column2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
            //
            //default settings
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Color_COPY_INP_TMP = "";
            }
            //
            hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
            //
            //Estimate extentions of text depending on font size.
            HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                out hv_MaxWidth, out hv_MaxHeight);
            if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
            {
                hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                hv_C1 = hv_Column_COPY_INP_TMP.Clone();
            }
            else
            {
                //transform image to window coordinates
                hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
            }
            //
            //display text box depending on text size
            if ((int)(new HTuple(hv_Box.TupleEqual("true"))) != 0)
            {
                //calculate box extents
                hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                hv_Width = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                    hv_Width = hv_Width.TupleConcat(hv_W);
                }
                hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    ));
                hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                hv_R2 = hv_R1 + hv_FrameHeight;
                hv_C2 = hv_C1 + hv_FrameWidth;
                //display rectangles
                HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                HOperatorSet.SetColor(hv_WindowHandle, "light gray");
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 3, hv_C1 + 3, hv_R2 + 3, hv_C2 + 3);
                HOperatorSet.SetColor(hv_WindowHandle, "white");
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
            }
            else if ((int)(new HTuple(hv_Box.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Box";
                throw new HalconException(hv_Exception);
            }
            //Write text.
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                    )));
                if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                    "auto")))) != 0)
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                }
                else
                {
                    HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                }
                hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                    hv_Index));
            }
            //reset changed window settings
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                hv_Column2Part);

            return;
        }
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

        private void FrmLensCarry_Load(object sender, EventArgs e)
        {
            LoadSettingLight();
            CCDSetPara();
        }

        public void Reconnect()
        {
            ucFitCircleTool_Correction = new UC_FitCircleTool();
            ucFitCircleTool_CircleCenter = new UC_FitCircleTool();
            ucFitRectangle2Tool_Angle1 = new UC_FitRectangle2Tool();
            bReadingPara = true;
            ReadPara();
            bReadingPara = false;
            ucFitCircleTool_Correction.ucFitCircle = ucFitCircle_Correction;
            ucFitCircleTool_Correction.Reconnect();

            ucFitCircleTool_CircleCenter.ucFitCircle = ucFitCircle_CircleCenter;
            ucFitCircleTool_CircleCenter.Reconnect();

            ucFitRectangle2Tool_Angle1.ucFitRectangle2 = ucFitRectangle2_Angle1;
            ucFitRectangle2Tool_Angle1.Reconnect();
        }

        public void ReadPara()
        {
            try
            {
                txtAdeviation_Correction.Text = parent.m_LensCarry.m_Correction.Angle.D.ToString();
                txtPixelmm_Correction.Text = parent.m_LensCarry.m_Correction.Pixelmm.D.ToString();
                lblPosition_CCDCenter_X_Correction.Text = Protocol.Position_Distance_X.ToString();
                lblPosition_CCDCenter_Y_Correction.Text = Protocol.Position_Distance_Y.ToString();

                ucFirstRadius_CircleCenter.Value = parent.m_LensCarry.m_CircleCenter.FirstRadius;
                cmbModelMode_CircleCenter.SelectedIndex = parent.m_LensCarry.m_CircleCenter.ModelMode;
                nudModelGrade_CircleCenter.Value = parent.m_LensCarry.m_CircleCenter.ModelGrade;
                ucFitCircle_CircleCenter.SetValue(parent.m_LensCarry.m_CircleCenter.Radius,
                    parent.m_LensCarry.m_CircleCenter.Measure_Transition,
                    parent.m_LensCarry.m_CircleCenter.Measure_Select,
                    parent.m_LensCarry.m_CircleCenter.Num_Measures,
                    parent.m_LensCarry.m_CircleCenter.Measure_Length1,
                    parent.m_LensCarry.m_CircleCenter.Measure_Length2,
                    parent.m_LensCarry.m_CircleCenter.Measure_Threshold);

                ucOuterRadius_Angle1.Value = parent.m_LensCarry.m_Angle1.OuterRadius;
                ucInnerRadius_Angle1.Value = parent.m_LensCarry.m_Angle1.InnerRadius;
                cbInterSectingRectangle_Angle1.Checked = parent.m_LensCarry.m_Angle1.InterSectingRectangle;
                ucFitRectangle2_Angle1.SetValue(parent.m_LensCarry.m_Angle1.Length1,
                    parent.m_LensCarry.m_Angle1.Length2,
                    parent.m_LensCarry.m_Angle1.Measure_Transition,
                    parent.m_LensCarry.m_Angle1.Measure_Select,
                    parent.m_LensCarry.m_Angle1.Num_Measures,
                    parent.m_LensCarry.m_Angle1.Measure_Length1,
                    parent.m_LensCarry.m_Angle1.Measure_Length2,
                    parent.m_LensCarry.m_Angle1.Measure_Threshold);

                cmbContrastSet_Angle1.SelectedIndex = parent.m_LensCarry.m_Angle1.ContrastSet;
                ucGray_Angle1.Value = parent.m_LensCarry.m_Angle1.Gray;

                cbClosing_Angle1.Checked = parent.m_LensCarry.m_Angle1.Closing;
                ucClosingValue_Angle1.Value = parent.m_LensCarry.m_Angle1.ClosingValue;
                cbOpening_Angle1.Checked = parent.m_LensCarry.m_Angle1.Opening;
                ucOpeningValue_Angle1.Value = parent.m_LensCarry.m_Angle1.OpeningValue;
                
                ucRect2_Len1_Upper_Angle1.Value = parent.m_LensCarry.m_Angle1.Rect2_Len1_Upper;
                ucRect2_Len1_Lower_Angle1.Value = parent.m_LensCarry.m_Angle1.Rect2_Len1_Lower;
                ucRect2_Len2_Upper_Angle1.Value = parent.m_LensCarry.m_Angle1.Rect2_Len2_Upper;
                ucRect2_Len2_Lower_Angle1.Value = parent.m_LensCarry.m_Angle1.Rect2_Len2_Lower;
                cbSelectAreaMaximum_Angle1.Checked = parent.m_LensCarry.m_Angle1.SelectAreaMaximum;
                nudStandardAngle_Angle1.Value = parent.m_LensCarry.m_Angle1.StandardAngle;
            }
            catch
            {
            }
        }

        public void LoadSettingLight()
        {
            ucLightSet_1.ValueChanged -= new UC_Slider.UC_Slider.ValueChangeEventHandler(ucLightSet_1_ValueChanged);
            ucLightSet_2.ValueChanged -= new UC_Slider.UC_Slider.ValueChangeEventHandler(ucLightSet_2_ValueChanged);
            ucLightSet_3.ValueChanged -= new UC_Slider.UC_Slider.ValueChangeEventHandler(ucLightSet_3_ValueChanged);
            ucLightSet_4.ValueChanged -= new UC_Slider.UC_Slider.ValueChangeEventHandler(ucLightSet_4_ValueChanged);
            ucLightSet_1.Value = Convert.ToInt32(Light.LightSet_1);
            ucLightSet_2.Value = Convert.ToInt32(Light.LightSet_2);
            ucLightSet_3.Value = Convert.ToInt32(Light.LightSet_3);
            ucLightSet_4.Value = Convert.ToInt32(Light.LightSet_4);
            ucLightSet_1.ValueChanged += new UC_Slider.UC_Slider.ValueChangeEventHandler(ucLightSet_1_ValueChanged);
            ucLightSet_2.ValueChanged += new UC_Slider.UC_Slider.ValueChangeEventHandler(ucLightSet_2_ValueChanged);
            ucLightSet_3.ValueChanged += new UC_Slider.UC_Slider.ValueChangeEventHandler(ucLightSet_3_ValueChanged);
            ucLightSet_4.ValueChanged += new UC_Slider.UC_Slider.ValueChangeEventHandler(ucLightSet_4_ValueChanged);
        }
        public void CCDSetPara()
        {
            ucGain.ValueChanged -= new UC_Slider.UC_Slider.ValueChangeEventHandler(ucGain_ValueChanged);
            ucExposureTime.ValueChanged -= new UC_Slider.UC_Slider.ValueChangeEventHandler(ucExposureTime_ValueChanged);
            
            ucGain.Maximum = Convert.ToInt32(CCD.GainMaximum);
            ucGain.Minimum = Convert.ToInt32(CCD.GainMinimum);
            ucExposureTime.Maximum = Convert.ToInt32(CCD.ExposureTimeMaximum);
            ucExposureTime.Minimum = Convert.ToInt32(CCD.ExposureTimeMinimum);
            ucGain.Value = Convert.ToInt32(CCD.Gain);
            ucExposureTime.Value = Convert.ToInt32(CCD.ExposureTime);

            ucGain.ValueChanged += new UC_Slider.UC_Slider.ValueChangeEventHandler(ucGain_ValueChanged);
            ucExposureTime.ValueChanged += new UC_Slider.UC_Slider.ValueChangeEventHandler(ucExposureTime_ValueChanged);
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

        public HObject ReadPicture(HWindow window, string ImagePath)
        {
            HObject ho_Image = new HObject();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();

            // 得到图片显示的窗口句柄
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
            HOperatorSet.SetPart(window, 0, 0, hv_Height - 1, hv_Width - 1);
            HOperatorSet.DispObj(ho_Image, window); //将图像在该窗口进行显示
            return ho_Image; //返回这个图像
        }

        private void ucFirstRadius_CircleCenter_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            parent.m_LensCarry.m_CircleCenter.FirstRadius = ucFirstRadius_CircleCenter.Value;
            HObject ho_Image = new HObject(), ho_Circle = new HObject(), ho_ReducedImage = new HObject();
            HTuple hv_Height = new HTuple(), hv_Width = new HTuple();
            HWindow Window = hWindowControl1.HalconWindow;
            try
            {
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);

                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, parent.m_LensCarry.m_CircleCenter.FirstRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                Window.ClearWindow();
                HOperatorSet.DispObj(ho_ReducedImage, Window);
            }
            catch
            {
            }
            ho_Image.Dispose();
            ho_Circle.Dispose();
            ho_ReducedImage.Dispose();
        }

        private void btnFindModel_CircleCenter_Click(object sender, EventArgs e)
        {
            if (My.ho_Image == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;

            int iImageprocess = 0;
            HObject ho_Circle = new HObject();
            HTuple hv_CenterRow = new HTuple(), hv_CenterColumn = new HTuple(), hv_Score = new HTuple(), hv_CenterPhi = new HTuple();
            iImageprocess = FindFirstModel_CircleCenter(Window, My.ho_Image, out ho_Circle, out hv_CenterRow, out hv_CenterColumn, out hv_Score, out hv_CenterPhi);

            HOperatorSet.SetDraw(Window, "margin");
            Window.ClearWindow();     
            My.ho_Image.DispObj(Window);
            if (iImageprocess == 1)
            {
                Window.SetColor("yellow");
                ho_Circle.DispObj(Window);
                disp_message(Window, "模組分數:" + Math.Round(hv_Score.D * 100, 0), "", 0, 0, "green", "false");
                //把參數寫入自製控建
                ucFitCircleTool_CircleCenter.Window = hWindowControl1.HalconWindow;
                ucFitCircleTool_CircleCenter.ho_Image = My.ho_Image;
                ucFitCircleTool_CircleCenter.hv_InitialRow = hv_CenterRow;
                ucFitCircleTool_CircleCenter.hv_InitialColumn = hv_CenterColumn;

                ucFitRectangle2Tool_Angle1.Window = hWindowControl1.HalconWindow;
                ucFitRectangle2Tool_Angle1.ho_Image = My.ho_Image;
                parent.m_LensCarry.m_Result.hv_CenterPhi = ucFitRectangle2Tool_Angle1.hv_InitialPhi = hv_CenterPhi;
            }
            else
            {
                disp_message(Window, "模組匹配失敗:", "", 0, 0, "red", "false");
            }
        }

        /// <summary>
        /// 找初始圓心模組(1:成功,-2:找不到模組,-3:模組異常)
        /// </summary>
        /// <param name="Window"></param>
        /// <param name="iCCDChoice"></param>
        /// <param name="ho_Circle"></param>
        public int FindFirstModel_CircleCenter(HWindow Window, HObject ho_Image, out HObject ho_ResultRegion, out HTuple hv_CenterRow, out HTuple hv_CenterColumn, out HTuple hv_Score, out HTuple hv_CenterPhi)
        {
            int iImageProcessResult = 0;
            ho_ResultRegion = new HObject();
            hv_CenterRow = 0;
            hv_CenterColumn = 0;
            hv_Score = 0;
            hv_CenterPhi = 0;
            HObject ho_ReducedImage = new HObject(), ho_ImageMedian = new HObject(), ho_Circle = new HObject();
            HObject ho_ImageEmphasize = new HObject(), ho_ModelContours = new HObject(), ho_TransContours = new HObject();
            HObject ho_RegionUnion = new HObject(), ho_Region = new HObject();
            HTuple hv_ModelID = new HTuple(), hv_HomMat2D = new HTuple();
            HTuple hv_Height = new HTuple(), hv_Width = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple(), hv_Radius = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_RectangleLength1 = new HTuple(), hv_RectangleLength2 = new HTuple();
            HTuple hv_Angle = new HTuple();
            HTuple hv_CircleRadius = new HTuple();
            HTuple hv_FirstRadius = new HTuple();

            hv_FirstRadius = parent.m_LensCarry.m_CircleCenter.FirstRadius;

            try
            {
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model";
                HOperatorSet.ReadShapeModel(Path, out hv_ModelID);
            }
            catch
            {
                iImageProcessResult = -3;
                MessageBox.Show("請建立初始模組");
                return iImageProcessResult;
            }
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
            ho_Circle.Dispose();
            HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_FirstRadius);
            ho_ReducedImage.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
            //ho_ImageMedian.Dispose();
            //HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 10, 10);
            //ho_ImageEmphasize.Dispose();
            //HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);
            try
            {
                ho_ModelContours.Dispose();
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                HOperatorSet.FindShapeModel(ho_ReducedImage, hv_ModelID, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), (double)parent.m_LensCarry.m_CircleCenter.ModelGrade / 100, 1, 0.5, "least_squares", (new HTuple(7)).TupleConcat(1), 0.75, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                if (hv_Score.TupleGreater(0) != 0)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle, 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row, hv_Column, out hv_HomMat2D);
                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_Region, out ho_RegionUnion);
                    switch (parent.m_LensCarry.m_CircleCenter.ModelMode)
                    {
                        case 0:
                            {
                                HOperatorSet.SmallestCircle(ho_RegionUnion, out hv_CenterRow, out hv_CenterColumn, out hv_CircleRadius);
                                ho_ResultRegion.Dispose();
                                HOperatorSet.GenCircle(out ho_ResultRegion, hv_CenterRow, hv_CenterColumn, hv_CircleRadius);
                            } break;
                        case 1:
                            {
                                HOperatorSet.SmallestRectangle2(ho_RegionUnion, out hv_CenterRow, out hv_CenterColumn, out hv_CenterPhi, out hv_RectangleLength1, out hv_RectangleLength2);
                                ho_ResultRegion.Dispose();
                                HOperatorSet.GenRectangle2(out ho_ResultRegion, hv_CenterRow, hv_CenterColumn, hv_CenterPhi, hv_RectangleLength1, hv_RectangleLength2);

                            } break;
                    }
                    iImageProcessResult = 1;
                }
                else
                {
                    iImageProcessResult = -2;
                }
            }
            catch
            {
                iImageProcessResult = -3;
            }
            HOperatorSet.ClearShapeModel(hv_ModelID);
            //ho_Image.Dispose();
            //ho_Circle.Dispose();
            ho_ReducedImage.Dispose();
            ho_ImageMedian.Dispose();
            ho_ImageEmphasize.Dispose();
            ho_ModelContours.Dispose();
            ho_Region.Dispose();
            ho_RegionUnion.Dispose();
            //ho_Circle.Dispose();
            return iImageProcessResult;
        }

        private void btnDrawModel_CircleCenter_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            if (bDrawing)
                return;
            bDrawing = true;
           
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_Image = new HObject();
            HObject ho_Circle = new HObject(), ho_ReducedImage = new HObject(), ho_ImageMedian = new HObject(), ho_ImageEmphasize = new HObject();
            HObject ho_Circle_Outer = new HObject(), ho_Circle_Inner = new HObject(), ho_RegionIntersection = new HObject();
            HObject ho_RegionDifference = new HObject(), ho_Region = new HObject(), ho_ModelContours = new HObject(), ho_TransContours = new HObject();
            HObject ho_RegionUnion = new HObject(), ho_ReducedImage_2 = new HObject(), ho_Rectangle = new HObject(), ho_ResultRegion = new HObject();
            HTuple hv_Height = new HTuple(), hv_Width = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple(), hv_Radius = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple(), hv_Length2 = new HTuple(), hv_FirstRadius = new HTuple();
            HTuple hv_ModelID = new HTuple(), hv_NumLevels = new HTuple(), hv_AngleStart = new HTuple(), hv_AngleExtent = new HTuple(), hv_AngleStep = new HTuple();
            HTuple hv_ScaleMin = new HTuple(), hv_ScaleMax = new HTuple(), hv_ScaleStep = new HTuple(), hv_Metric = new HTuple(), hv_MinContrast = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
            HTuple hv_RectangleRow = new HTuple(), hv_RectangleColumn = new HTuple(), hv_RectanglePhi = new HTuple(), hv_RectangleLength1 = new HTuple(), hv_RectangleLength2 = new HTuple();
            HTuple hv_HomMat2D = new HTuple(), hv_CircleRow = new HTuple(), hv_CircleColumn = new HTuple(), hv_CircleRadius = new HTuple();
            HOperatorSet.SetDraw(Window, "margin");
            HOperatorSet.SetColor(Window, "green");

            try
            {
                hv_FirstRadius = parent.m_LensCarry.m_CircleCenter.FirstRadius;
                ho_Image.Dispose();
                ho_Image = My.ho_Image.CopyObj(1, -1);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_FirstRadius);
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //ho_ImageMedian.Dispose();
                //HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 10, 10);
                //ho_ImageEmphasize.Dispose();
                //HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);
                Window.ClearWindow();
                ho_ReducedImage.DispObj(Window);
                switch (parent.m_LensCarry.m_CircleCenter.ModelMode)
                {
                    case 0:
                        {
                            disp_message(Window, "1.畫產品外圍", "", 0, 0, "green", "false");
                            HOperatorSet.DrawCircle(Window, out hv_Row, out hv_Column, out hv_Radius);
                            ho_Circle_Outer.Dispose();
                            HOperatorSet.GenCircle(out ho_Circle_Outer, hv_Row, hv_Column, hv_Radius);
                            ho_Circle_Outer.DispObj(Window);
                            disp_message(Window, "2.畫產品內圍", "", 30, 0, "green", "false");
                            HOperatorSet.DrawCircle(Window, out hv_Row, out hv_Column, out hv_Radius);
                            ho_Circle_Inner.Dispose();
                            HOperatorSet.GenCircle(out ho_Circle_Inner, hv_Row, hv_Column, hv_Radius);
                            ho_Circle_Inner.DispObj(Window);
                            //內外圓相減
                            ho_RegionDifference.Dispose();
                            HOperatorSet.Difference(ho_Circle_Outer, ho_Circle_Inner, out ho_RegionDifference);

                            ho_ReducedImage_2.Dispose();
                            HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ReducedImage_2);
                        } break;
                    case 1:
                        {
                            disp_message(Window, "1.畫產品外圍", "", 0, 0, "green", "false");
                            HOperatorSet.DrawCircle(Window, out hv_Row, out hv_Column, out hv_Radius);
                            ho_Circle.Dispose();
                            HOperatorSet.GenCircle(out ho_Circle_Outer, hv_Row, hv_Column, hv_Radius);
                            ho_Circle_Outer.DispObj(Window);
                            disp_message(Window, "2.畫產品內圍", "", 30, 0, "green", "false");
                            HOperatorSet.DrawCircle(Window, out hv_Row, out hv_Column, out hv_Radius);
                            ho_Circle.Dispose();
                            HOperatorSet.GenCircle(out ho_Circle_Inner, hv_Row, hv_Column, hv_Radius);
                            ho_Circle_Inner.DispObj(Window);
                            //內外圓相減
                            ho_RegionDifference.Dispose();
                            HOperatorSet.Difference(ho_Circle_Outer, ho_Circle_Inner, out ho_RegionDifference);
                            disp_message(Window, "3.畫產品方形", "", 60, 0, "green", "false");
                            HOperatorSet.DrawRectangle2(Window, out hv_Row, out hv_Column, out hv_Phi, out hv_Length1, out hv_Length2);
                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2);
                            ho_RegionIntersection.Dispose();
                            HOperatorSet.Intersection(ho_RegionDifference, ho_Rectangle, out ho_RegionIntersection);
                            ho_ReducedImage_2.Dispose();
                            HOperatorSet.ReduceDomain(ho_Image, ho_RegionIntersection, out ho_ReducedImage_2);
                        } break;
                }
            }
            catch
            {
            }
            try
            {
                HOperatorSet.CreateShapeModel(ho_ReducedImage_2, 7, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), "auto", "auto", "use_polarity", "auto", "auto", out hv_ModelID);
                HOperatorSet.GetShapeModelParams(hv_ModelID, out hv_NumLevels, out hv_AngleStart, out hv_AngleExtent, out hv_AngleStep, out hv_ScaleMin, out hv_ScaleMax, out hv_ScaleStep, out hv_Metric, out hv_MinContrast);
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                //HOperatorSet.FindShapeModel(ho_ImageEmphasize, hv_ModelID, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), 0.5, 1, 0.5, "least_squares", (new HTuple(7)).TupleConcat(1), 0.75, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                HOperatorSet.FindShapeModel(ho_ReducedImage, hv_ModelID, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), (double)parent.m_LensCarry.m_CircleCenter.ModelGrade / 100, 1, 0.5, "least_squares", (new HTuple(7)).TupleConcat(1), 0.75, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                if (hv_Score.TupleGreater(0) != 0)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle, 0, 0, out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row, hv_Column, out hv_HomMat2D);
                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_Region, out ho_RegionUnion);
                    switch (parent.m_LensCarry.m_CircleCenter.ModelMode)
                    {
                        case 0:
                            {
                                HOperatorSet.SmallestCircle(ho_RegionUnion, out hv_CircleRow, out hv_CircleColumn, out hv_CircleRadius);
                                ho_ResultRegion.Dispose();
                                HOperatorSet.GenCircle(out ho_ResultRegion, hv_CircleRow, hv_CircleColumn, hv_CircleRadius);
                            } break;
                        case 1:
                            {
                                HOperatorSet.SmallestRectangle2(ho_RegionUnion, out hv_RectangleRow, out hv_RectangleColumn, out hv_RectanglePhi, out hv_RectangleLength1, out hv_RectangleLength2);
                                ho_ResultRegion.Dispose();
                                HOperatorSet.GenRectangle2(out ho_ResultRegion, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);

                            }break;
                    }
                    Window.ClearWindow();
                    ho_Image.DispObj(Window);
                    Window.SetColor("yellow");
                    ho_ResultRegion.DispObj(Window);
                    disp_message(Window, "模組分數:" + Math.Round(hv_Score.D * 100, 0), "", 0, 0, "green", "false");
                    if (MessageBox.Show("是否儲存模組?", "模組設定", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {

                        string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model";
                        HOperatorSet.WriteShapeModel(hv_ModelID, Path);
                    }
                    //parent.m_LensCarry.m_Result.hv_FirstCenterRow = hv_CircleRow;
                    //parent.m_LensCarry.m_Result.hv_FirstCenterColumn = hv_CircleColumn;
                }
                HOperatorSet.ClearShapeModel(hv_ModelID);
            }
            catch
            {
                HOperatorSet.ClearShapeModel(hv_ModelID);
            }
            bDrawing = false;
            ho_Image.Dispose();
            ho_Circle.Dispose();
            ho_ReducedImage.Dispose();
            ho_ImageMedian.Dispose();
            ho_ImageEmphasize.Dispose();
            ho_ModelContours.Dispose();
            ho_Region.Dispose();
            ho_RegionUnion.Dispose();
            ho_Circle_Outer.Dispose();
            ho_Circle_Inner.Dispose();
            ho_RegionDifference.Dispose();
            ho_RegionIntersection.Dispose();
        }

        private void nudModelGrade_CircleCenter_ValueChanged(object sender, EventArgs e)
        {
            parent.m_LensCarry.m_CircleCenter.ModelGrade = (int)nudModelGrade_CircleCenter.Value;
        }

        private void btnSave_CenterCircle_Click(object sender, EventArgs e)
        {
            parent.m_LensCarry.m_Result.hv_CenterRow = ucFitCircleTool_CircleCenter.hv_CenterRow;
            parent.m_LensCarry.m_Result.hv_CenterColumn = ucFitCircleTool_CircleCenter.hv_CenterColumn;
            //參數寫入方形角度自製控建
            ucFitRectangle2Tool_Angle1.hv_InitialRow = parent.m_LensCarry.m_Result.hv_CenterRow;
            ucFitRectangle2Tool_Angle1.hv_InitialColumn = parent.m_LensCarry.m_Result.hv_CenterColumn;

            parent.m_LensCarry.m_CircleCenter.Radius = ucFitCircleTool_CircleCenter.radius;
            parent.m_LensCarry.m_CircleCenter.Measure_Transition = ucFitCircleTool_CircleCenter.measure_transition;
            parent.m_LensCarry.m_CircleCenter.Measure_Select = ucFitCircleTool_CircleCenter.measure_select;
            parent.m_LensCarry.m_CircleCenter.Num_Measures = ucFitCircleTool_CircleCenter.num_measures;
            parent.m_LensCarry.m_CircleCenter.Measure_Length1 = ucFitCircleTool_CircleCenter.measure_length1;
            parent.m_LensCarry.m_CircleCenter.Measure_Length2 = ucFitCircleTool_CircleCenter.measure_length2;
            parent.m_LensCarry.m_CircleCenter.Measure_Threshold = ucFitCircleTool_CircleCenter.measure_threshold;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("CircleCenter", "FirstRadius", parent.m_LensCarry.m_CircleCenter.FirstRadius.ToString(), Path);
            IniFile.Write("CircleCenter", "ModelGrade", parent.m_LensCarry.m_CircleCenter.ModelGrade.ToString(), Path);
            IniFile.Write("CircleCenter", "Radius", parent.m_LensCarry.m_CircleCenter.Radius.ToString(), Path);
            IniFile.Write("CircleCenter", "Measure_Transition", parent.m_LensCarry.m_CircleCenter.Measure_Transition, Path);
            IniFile.Write("CircleCenter", "Measure_Select", parent.m_LensCarry.m_CircleCenter.Measure_Select, Path);
            IniFile.Write("CircleCenter", "Num_Measures", parent.m_LensCarry.m_CircleCenter.Num_Measures.ToString(), Path);
            IniFile.Write("CircleCenter", "Measure_Length1", parent.m_LensCarry.m_CircleCenter.Measure_Length1.ToString(), Path);
            IniFile.Write("CircleCenter", "Measure_Length2", parent.m_LensCarry.m_CircleCenter.Measure_Length2.ToString(), Path);
            IniFile.Write("CircleCenter", "Measure_Threshold", parent.m_LensCarry.m_CircleCenter.Measure_Threshold.ToString(), Path);
        }
        

        private void btnDrawFirstCircle_Correction_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple(), hv_Radius = new HTuple();
            HWindow Window = hWindowControl1.HalconWindow;
            HOperatorSet.SetColor(Window, "red");
            HOperatorSet.SetDraw(Window, "margin");
            HObject ho_Circle = new HObject();
            //找出初始半徑
            HOperatorSet.DrawCircle(Window, out  parent.m_LensCarry.m_Correction.hv_FirstRow, out parent.m_LensCarry.m_Correction.hv_FirstColumn, out parent.m_LensCarry.m_Correction.hv_FirstRadius);
            ho_Circle.Dispose();
            HOperatorSet.GenCircle(out ho_Circle, parent.m_LensCarry.m_Correction.hv_FirstRow, parent.m_LensCarry.m_Correction.hv_FirstColumn, parent.m_LensCarry.m_Correction.hv_FirstRadius);
            HOperatorSet.DispObj(ho_Circle, Window);
            ho_Circle.Dispose();


            ucFitCircleTool_Correction.Window = hWindowControl1.HalconWindow;
            ucFitCircleTool_Correction.ho_Image = My.ho_Image;
            ucFitCircleTool_Correction.hv_InitialRow = parent.m_LensCarry.m_Correction.hv_FirstRow;
            ucFitCircleTool_Correction.hv_InitialColumn = parent.m_LensCarry.m_Correction.hv_FirstColumn;
        }

        private void cmbDarkLightChoice_Correction_SelectedIndexChanged(object sender, EventArgs e)
        {
            parent.m_LensCarry.m_Correction.DarkLightChoice = cmbDarkLightChoice_Correction.SelectedIndex;
        }

        private void ucGray_Correction_ValueChanged(object sender, EventArgs e)
        {
            parent.m_LensCarry.m_Correction.Gray = ucGray_Correction.Value;
            if (bReadingPara)
                return;
            HObject ho_Image = new HObject(), ho_Circle = new HObject(), ho_ImageReduced = new HObject(), ho_Region = new HObject();
            HObject ho_ConnectedRegions = new HObject(), ho_SelectedRegions = new HObject();
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            try
            {
                ho_Image = My.ho_Image.CopyObj(1, -1);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, parent.m_LensCarry.m_Correction.hv_FirstRow, parent.m_LensCarry.m_Correction.hv_FirstColumn, parent.m_LensCarry.m_Correction.hv_FirstRadius);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ImageReduced);
                if (parent.m_LensCarry.m_Correction.DarkLightChoice == 1)
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, parent.m_LensCarry.m_Correction.Gray, 255);
                else
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 0, parent.m_LensCarry.m_Correction.Gray);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
                HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", hv_Area.TupleMax(), hv_Area.TupleMax() + 1);
                HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
                HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "fill");
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "red");
                ho_Image.DispObj(hWindowControl1.HalconWindow);
                ho_Region.DispObj(hWindowControl1.HalconWindow);
                HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "blue");
                ho_SelectedRegions.DispObj(hWindowControl1.HalconWindow);

                ucFitCircleTool_Correction.hv_InitialRow = hv_Row;
                ucFitCircleTool_Correction.hv_InitialColumn = hv_Column;
            }
            catch
            {
            }
            ho_Image.Dispose();
            ho_Circle.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_ImageReduced.Dispose();
            ho_Region.Dispose();
            ho_SelectedRegions.Dispose();
        }

        private void btnAngleY1_Correction_1_Click(object sender, EventArgs e)
        {
            parent.m_LensCarry.m_Correction.hv_Row1 = ucFitCircleTool_Correction.hv_CenterRow;
            parent.m_LensCarry.m_Correction.hv_Column1 = ucFitCircleTool_Correction.hv_CenterColumn;

            parent.m_LensCarry.m_Correction.hv_PlcX1 = Protocol.AxisCoordinate_X;
            parent.m_LensCarry.m_Correction.hv_PlcY1 = Protocol.AxisCoordinate_Y;

            txtA_X1_Correction_1.Text = parent.m_LensCarry.m_Correction.hv_Row1.D.ToString("f3");
            txtA_Y1_Correction_1.Text = parent.m_LensCarry.m_Correction.hv_Column1.D.ToString("f3");
        }

        private void btnAngleY2_Correction_1_Click(object sender, EventArgs e)
        {
            parent.m_LensCarry.m_Correction.hv_Row2 = ucFitCircleTool_Correction.hv_CenterRow;
            parent.m_LensCarry.m_Correction.hv_Column2 = ucFitCircleTool_Correction.hv_CenterColumn;

            parent.m_LensCarry.m_Correction.hv_PlcX2 = Protocol.AxisCoordinate_X;
            parent.m_LensCarry.m_Correction.hv_PlcY2 = Protocol.AxisCoordinate_Y;

            txtA_X2_Correction_1.Text = parent.m_LensCarry.m_Correction.hv_Row2.D.ToString("f3");
            txtA_Y2_Correction_1.Text = parent.m_LensCarry.m_Correction.hv_Column2.D.ToString("f3");
        }

        private void btnAngleCel_Correction_1_Click(object sender, EventArgs e)
        {
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple(), hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_PlcRow1 = new HTuple(), hv_PlcColumn1 = new HTuple(), hv_PlcRow2 = new HTuple(), hv_PlcColumn2 = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Distance = new HTuple(), hv_Pixelmm = new HTuple();

            hv_Row1 = parent.m_LensCarry.m_Correction.hv_Row1;
            hv_Column1 = parent.m_LensCarry.m_Correction.hv_Column1;
            hv_Row2 = parent.m_LensCarry.m_Correction.hv_Row2;
            hv_Column2 = parent.m_LensCarry.m_Correction.hv_Column2;

            hv_PlcRow1 = parent.m_LensCarry.m_Correction.hv_PlcY1;
            hv_PlcColumn1 = parent.m_LensCarry.m_Correction.hv_PlcX1;
            hv_PlcRow2 = parent.m_LensCarry.m_Correction.hv_PlcY2;
            hv_PlcColumn2 = parent.m_LensCarry.m_Correction.hv_PlcX2;

            HOperatorSet.AngleLx(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Angle);
            //if (hv_Angle.D > 0)
            //    hv_Angle = (hv_Angle.D) * -1;
            //else
            //    hv_Angle = (hv_Angle.D) * -1;

            if (hv_Angle.D > 0)
                hv_Angle = (hv_Angle.D + Math.PI / 2) * -1;
            else
                hv_Angle = (hv_Angle.D + Math.PI / 2) * -1;
            
            txtAdeviation_Correction.Text = Math.Round(hv_Angle.D, 4).ToString("f4");
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Correction", "Angle", hv_Angle.D.ToString(), Path);

            parent.m_LensCarry.m_Correction.Angle = hv_Angle;
        }

        private void btnCurrectionOpen_Click(object sender, EventArgs e)
        {
            if (!My.bCurrention)
                My.bCurrention = true;
            else
                My.bCurrention = false;
        }

        private void btnSave_Correction_1_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Correction", "Angle", parent.m_LensCarry.m_Correction.Angle.ToString(), Path);
        }

        private void btnAngleY1_Correction_2_Click(object sender, EventArgs e)
        {
            parent.m_LensCarry.m_Correction.hv_Row1 = ucFitCircleTool_Correction.hv_CenterRow;
            parent.m_LensCarry.m_Correction.hv_Column1 = ucFitCircleTool_Correction.hv_CenterColumn;

            parent.m_LensCarry.m_Correction.hv_PlcX1 = Protocol.AxisCoordinate_X;
            parent.m_LensCarry.m_Correction.hv_PlcY1 = Protocol.AxisCoordinate_Y;

            txtA_X1_Correction_2.Text = parent.m_LensCarry.m_Correction.hv_Row1.D.ToString("f3");
            txtA_Y1_Correction_2.Text = parent.m_LensCarry.m_Correction.hv_Column1.D.ToString("f3");
        }

        private void btnAngleY2_Correction_2_Click(object sender, EventArgs e)
        {
            parent.m_LensCarry.m_Correction.hv_Row2 = ucFitCircleTool_Correction.hv_CenterRow;
            parent.m_LensCarry.m_Correction.hv_Column2 = ucFitCircleTool_Correction.hv_CenterColumn;

            parent.m_LensCarry.m_Correction.hv_PlcX2 = Protocol.AxisCoordinate_X;
            parent.m_LensCarry.m_Correction.hv_PlcY2 = Protocol.AxisCoordinate_Y;

            txtA_X2_Correction_2.Text = parent.m_LensCarry.m_Correction.hv_Row2.D.ToString("f3");
            txtA_Y2_Correction_2.Text = parent.m_LensCarry.m_Correction.hv_Column2.D.ToString("f3");
        }

        private void btnAngleCel_Correction_2_Click(object sender, EventArgs e)
        {
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple(), hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_PlcRow1 = new HTuple(), hv_PlcColumn1 = new HTuple(), hv_PlcRow2 = new HTuple(), hv_PlcColumn2 = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Distance = new HTuple(), hv_PlcDistance = new HTuple(), hv_Pixelmm = new HTuple();

            hv_Row1 = parent.m_LensCarry.m_Correction.hv_Row1;
            hv_Column1 = parent.m_LensCarry.m_Correction.hv_Column1;
            hv_Row2 = parent.m_LensCarry.m_Correction.hv_Row2;
            hv_Column2 = parent.m_LensCarry.m_Correction.hv_Column2;

            hv_PlcRow1 = parent.m_LensCarry.m_Correction.hv_PlcY1;
            hv_PlcColumn1 = parent.m_LensCarry.m_Correction.hv_PlcX1;
            hv_PlcRow2 = parent.m_LensCarry.m_Correction.hv_PlcY2;
            hv_PlcColumn2 = parent.m_LensCarry.m_Correction.hv_PlcX2;

            HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Distance);
            HOperatorSet.DistancePp(hv_PlcRow1, hv_PlcColumn1, hv_PlcRow2, hv_PlcColumn2, out hv_PlcDistance);
            parent.m_LensCarry.m_Correction.Pixelmm = hv_PlcDistance / hv_Distance.D;
            txtPixelmm_Correction.Text = parent.m_LensCarry.m_Correction.Pixelmm.D.ToString();
        }

        private void btnSave_Correction_2_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Correction", "Pixelmm", parent.m_LensCarry.m_Correction.Pixelmm.ToString(), Path);
        }

        private void cbTransformOpen_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnShowOriginalImage_Click(object sender, EventArgs e)
        {
            parent.ShowOriginalImage(My.ho_Image, hWindowControl1.HalconWindow);
        }

        private void ucOuterRadius_Angle1_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HTuple hv_OuterRadius = parent.m_LensCarry.m_Angle1.OuterRadius = ucOuterRadius_Angle1.Value;
            HTuple hv_InnerRadius = parent.m_LensCarry.m_Angle1.InnerRadius;
            HTuple hv_CenterRow = parent.m_LensCarry.m_Result.hv_CenterRow;
            HTuple hv_CenterColumn = parent.m_LensCarry.m_Result.hv_CenterColumn;
            try
            {
                //這段要另外用的
                DetectingRing(Window, hv_CenterRow, hv_CenterColumn, hv_OuterRadius, hv_InnerRadius);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void ucInnerRadius_Angle1_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HTuple hv_OuterRadius = parent.m_LensCarry.m_Angle1.OuterRadius;
            HTuple hv_InnerRadius = parent.m_LensCarry.m_Angle1.InnerRadius = ucInnerRadius_Angle1.Value;
            HTuple hv_CenterRow = parent.m_LensCarry.m_Result.hv_CenterRow;
            HTuple hv_CenterColumn = parent.m_LensCarry.m_Result.hv_CenterColumn;
            try
            {
                //這段要另外用的
                DetectingRing(Window, hv_CenterRow, hv_CenterColumn, hv_OuterRadius, hv_InnerRadius);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void cmbContrastSet_Angle1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            int ContrastSet = parent.m_LensCarry.m_Angle1.ContrastSet = cmbContrastSet_Angle1.SelectedIndex;

        }

        private void ucGray_Angle1_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            parent.m_LensCarry.m_Angle1.Gray = ucGray_Angle1.Value;

            if (parent.m_LensCarry.m_Result.hv_CenterRow.D == 0)
            {
                MessageBox.Show("請先求圓心!");
                return;
            }

            HTuple hv_CenterRow = parent.m_LensCarry.m_Result.hv_CenterRow;
            HTuple hv_CenterColumn = parent.m_LensCarry.m_Result.hv_CenterColumn;
            HTuple hv_CenterPhi = parent.m_LensCarry.m_Result.hv_CenterPhi;
            HObject ho_ConnectedRegions = new HObject(), ho_RegionsResult_Angle1 = new HObject();
            HTuple hv_Result_Rect2_Len1 = new HTuple(), hv_Result_Rect2_Len2 = new HTuple(), hv_AngleCenterRow = new HTuple(), hv_AngleCenterColumn = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HWindow Window = hWindowControl1.HalconWindow;
            FindAngle_Angle1(My.ho_Image, hv_CenterRow, hv_CenterColumn,hv_CenterPhi,
             out ho_ConnectedRegions, out ho_RegionsResult_Angle1, out hv_Result_Rect2_Len1, out hv_Result_Rect2_Len2, out hv_AngleCenterRow, out hv_AngleCenterColumn);

            HOperatorSet.RegionFeatures(ho_ConnectedRegions, "rect2_len1", out hv_Result_Rect2_Len1);
            HOperatorSet.RegionFeatures(ho_ConnectedRegions, "rect2_len2", out hv_Result_Rect2_Len2);
            HOperatorSet.RegionFeatures(ho_ConnectedRegions, "row", out hv_Row);
            HOperatorSet.RegionFeatures(ho_ConnectedRegions, "column", out hv_Column);

            Window.ClearWindow();
            My.ho_Image.DispObj(Window);
            Window.SetDraw("fill");
            Window.SetColor("yellow");
            ho_ConnectedRegions.DispObj(Window);
            set_display_font(Window, 15, "mono", "true", "false");
            for (int i = 0; i < hv_Row.Length; i++)
            {
                disp_message(Window, Math.Round(hv_Result_Rect2_Len1.DArr[i]).ToString(), "", hv_Row.DArr[i], hv_Column.DArr[i], "green", "false");
                disp_message(Window, Math.Round(hv_Result_Rect2_Len2.DArr[i]).ToString(), "", hv_Row.DArr[i] + 30, hv_Column.DArr[i], "blue", "false");
            }
        }

        private void ucRect2_Len1_Upper_Angle1_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            parent.m_LensCarry.m_Angle1.Rect2_Len1_Upper = ucRect2_Len1_Upper_Angle1.Value;
            Angle1Show();

        }

        private void ucRect2_Len1_Lower_Angle1_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            parent.m_LensCarry.m_Angle1.Rect2_Len1_Lower = ucRect2_Len1_Lower_Angle1.Value;
            Angle1Show();
        }

        private void ucRect2_Len2_Upper_Angle1_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            parent.m_LensCarry.m_Angle1.Rect2_Len2_Upper = ucRect2_Len2_Upper_Angle1.Value;
            Angle1Show();
        }

        private void ucRect2_Len2_Lower_Angle1_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            parent.m_LensCarry.m_Angle1.Rect2_Len2_Lower = ucRect2_Len2_Lower_Angle1.Value;
            Angle1Show();
        }

        private void cbSelectAreaMaximum_Angle1_CheckedChanged(object sender, EventArgs e)
        {
            parent.m_LensCarry.m_Angle1.SelectAreaMaximum = cbSelectAreaMaximum_Angle1.Checked;

        }

        private void btnFindAngle_Angle1_Click(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            Angle1Show();

            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_Cross = new HObject(), ho_CenterCross = new HObject();
            HTuple hv_ResultPhi = new HTuple();
            HTuple hv_CenterRow = ucFitCircleTool_CircleCenter.hv_CenterRow;
            HTuple hv_CenterColumn = ucFitCircleTool_CircleCenter.hv_CenterColumn;
            HTuple hv_AngleCenterRow = parent.m_LensCarry.m_Result.hv_AngleCenterRow;
            HTuple hv_AngleCenterColumn = parent.m_LensCarry.m_Result.hv_AngleCenterColumn;
            ho_Cross.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_AngleCenterRow, hv_AngleCenterColumn, 50, 0);
            ho_CenterCross.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_CenterCross, hv_CenterRow, hv_CenterColumn, 50, 0);
            HOperatorSet.AngleLx(hv_CenterRow, hv_CenterColumn, hv_AngleCenterRow, hv_AngleCenterColumn, out hv_ResultPhi);

            double ResultAngle = Math.Round(hv_ResultPhi.TupleDeg().D, 0);
            set_display_font(Window, 20, "mono", "true", "false");
           
            Window.SetColor("yellow");
            ho_CenterCross.DispObj(Window);
            Window.SetColor("green");
            ho_Cross.DispObj(Window);
            Window.SetColor("blue");
            Window.DispLine(hv_CenterRow, hv_CenterColumn, hv_AngleCenterRow, hv_AngleCenterColumn);
            disp_message(Window, "角度:" + ResultAngle, "", 0, 0, "green", "false");
            //if (ResultAngle < 0)
            //    ResultAngle = ResultAngle + 180;
            parent.m_LensCarry.m_Result.hv_ResultAngle = ResultAngle-90;
            parent.m_LensCarry.m_Result.hv_ResultPhi = hv_ResultPhi-90;
        }
        public int FindAngle_Angle1(HObject ho_Image, HTuple hv_CenterRow, HTuple hv_CenterColumn, HTuple hv_CenterPhi,
           out HObject ho_ConnectedRegions, out HObject ho_RegionsResult_Angle1, out HTuple hv_Result_Rect2_Len1, out HTuple hv_Result_Rect2_Len2,
           out HTuple hv_AngleCenterRow, out HTuple hv_AngleCenterColumn)
        {
            int iImageProcessResult = 0;
            hv_Result_Rect2_Len1 = 0;
            hv_Result_Rect2_Len2 = 0;
            hv_AngleCenterRow = 0;
            hv_AngleCenterColumn = 0;
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionsResult_Angle1);

            HTuple hv_Rect2_Len1_Upper = parent.m_LensCarry.m_Angle1.Rect2_Len1_Upper;
            HTuple hv_Rect2_Len1_Lower = parent.m_LensCarry.m_Angle1.Rect2_Len1_Lower;
            HTuple hv_Rect2_Len2_Upper = parent.m_LensCarry.m_Angle1.Rect2_Len2_Upper;
            HTuple hv_Rect2_Len2_Lower = parent.m_LensCarry.m_Angle1.Rect2_Len2_Lower;
            HTuple hv_OuterRadius = parent.m_LensCarry.m_Angle1.OuterRadius;
            HTuple hv_InnerRadius = parent.m_LensCarry.m_Angle1.InnerRadius;
            int ContrastSet = parent.m_LensCarry.m_Angle1.ContrastSet;
            bool SelectAreaMaximum = parent.m_LensCarry.m_Angle1.SelectAreaMaximum;
            HTuple hv_Gray = parent.m_LensCarry.m_Angle1.Gray;

            HObject ho_ImageMedian = new HObject(), ho_ImageEmphasize = new HObject();
            HObject ho_OuterCircle = new HObject(), ho_InnerCircle = new HObject(), ho_RegionDifference = new HObject();
            HObject ho_ImageReduced = new HObject(), ho_RegionDynThresh1 = new HObject();
            HObject ho_SelectedRegions1 = new HObject(), ho_SelectedRegions2 = new HObject();
            HObject ho_Region = new HObject();
            HTuple hv_Area = new HTuple();
            try
            {
                ho_OuterCircle.Dispose();
                HOperatorSet.GenCircle(out ho_OuterCircle, hv_CenterRow, hv_CenterColumn, hv_OuterRadius);//外圈

                ho_InnerCircle.Dispose();
                HOperatorSet.GenCircle(out ho_InnerCircle, hv_CenterRow, hv_CenterColumn, hv_InnerRadius);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_OuterCircle, ho_InnerCircle, out ho_RegionDifference);
                //檢測範圍與方形相交
                if (parent.m_LensCarry.m_Angle1.InterSectingRectangle)
                {
                    HObject ho_ResultContours = new HObject(), ho_CrossCenter = new HObject();
                    ucFitRectangle2Tool_Angle1.hv_InitialRow = hv_CenterRow;
                    ucFitRectangle2Tool_Angle1.hv_InitialColumn = hv_CenterColumn;
                    ucFitRectangle2Tool_Angle1.hv_InitialPhi = hv_CenterPhi;
                    ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                    iImageProcessResult = ucFitRectangle2Tool_Angle1.ImageProcess_FitRectangle2(ho_Image, out ho_ResultContours, out ho_CrossCenter, out hv_CenterRow, out hv_CenterColumn, out hv_CenterPhi);
                    if (hv_CenterRow.Length > 0)
                    {
                        ho_Region.Dispose();
                        HOperatorSet.GenRegionContourXld(ho_ResultContours, out ho_Region, "filled");
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Intersection(ho_RegionDifference, ho_Region, out ExpTmpOutVar_0);
                            ho_RegionDifference.Dispose();
                            ho_RegionDifference = ExpTmpOutVar_0;
                        }
                    }
                }
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                //ho_ImageMedian.Dispose();
                //HOperatorSet.MedianRect(ho_ImageReduced, out ho_ImageMedian, 10, 10);
                //ho_ImageEmphasize.Dispose();
                //HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);
                ho_RegionDynThresh1.Dispose();
                if (ContrastSet == 0)
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_RegionDynThresh1, hv_Gray, 255);
                else
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_RegionDynThresh1, 0, hv_Gray);
                //區域連接/斷開開啟
                if(parent.m_LensCarry.m_Angle1.Closing||parent.m_LensCarry.m_Angle1.Opening)
                {
                    HObject ho_RegionOpening = new HObject(), ho_RegionFillUp = new HObject(), ho_RegionClosing = new HObject();
                    
                    if (parent.m_LensCarry.m_Angle1.Closing)
                    {
                        ho_RegionClosing.Dispose();
                        HOperatorSet.ClosingCircle(ho_RegionDynThresh1, out ho_RegionClosing, parent.m_LensCarry.m_Angle1.ClosingValue);
                        ho_RegionFillUp.Dispose();
                        HOperatorSet.FillUp(ho_RegionClosing, out ho_RegionDynThresh1);
                    }
                    if (parent.m_LensCarry.m_Angle1.Opening)
                    {
                        ho_RegionOpening.Dispose();
                        HOperatorSet.OpeningCircle(ho_RegionDynThresh1, out ho_RegionOpening, parent.m_LensCarry.m_Angle1.OpeningValue);
                        ho_RegionFillUp.Dispose();
                        HOperatorSet.FillUp(ho_RegionOpening, out ho_RegionDynThresh1);
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Intersection(ho_RegionDynThresh1, ho_RegionDifference, out ExpTmpOutVar_0);
                        ho_RegionDynThresh1.Dispose();
                        ho_RegionDynThresh1 = ExpTmpOutVar_0;
                    }
                }
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionDynThresh1, out ho_ConnectedRegions);
                ho_SelectedRegions1.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions1, "rect2_len1", "and", hv_Rect2_Len1_Lower, hv_Rect2_Len1_Upper);
                ho_SelectedRegions2.Dispose();
                HOperatorSet.SelectShape(ho_SelectedRegions1, out ho_SelectedRegions2, "rect2_len2", "and", hv_Rect2_Len2_Lower, hv_Rect2_Len2_Upper);
                if (SelectAreaMaximum)
                {
                    HOperatorSet.RegionFeatures(ho_SelectedRegions2, "area", out hv_Area);
                    HOperatorSet.SelectShape(ho_SelectedRegions2, out ho_RegionsResult_Angle1, "area", "and", hv_Area.TupleMax(), hv_Area.TupleMax());
                }
                else
                {
                    ho_RegionsResult_Angle1 = ho_SelectedRegions2.CopyObj(1, -1);
                }
                HOperatorSet.RegionFeatures(ho_RegionsResult_Angle1, "rect2_len1", out hv_Result_Rect2_Len1);
                HOperatorSet.RegionFeatures(ho_RegionsResult_Angle1, "rect2_len2", out hv_Result_Rect2_Len2);
                HOperatorSet.RegionFeatures(ho_RegionsResult_Angle1, "row", out hv_AngleCenterRow);
                HOperatorSet.RegionFeatures(ho_RegionsResult_Angle1, "column", out hv_AngleCenterColumn);
                if (hv_AngleCenterRow.Length == 0)
                {
                    iImageProcessResult = -1;
                    return iImageProcessResult;
                }
            }
            catch
            {
                iImageProcessResult = -1;
                return iImageProcessResult;
            }
            iImageProcessResult = 1;
            return iImageProcessResult;

        }
        /// <summary>
        /// 設定Angle閥值設定時顯示
        /// </summary>
        public void Angle1Show()
        {
            if (parent.m_LensCarry.m_Result.hv_CenterRow.D == 0)
            {
                MessageBox.Show("請先求圓心!");
                return;
            }

            try
            {
                HTuple hv_CenterRow = parent.m_LensCarry.m_Result.hv_CenterRow;
                HTuple hv_CenterColumn = parent.m_LensCarry.m_Result.hv_CenterColumn;
                HTuple hv_CenterPhi = parent.m_LensCarry.m_Result.hv_CenterPhi;
                HObject ho_ConnectedRegions = new HObject(), ho_RegionsResult_Angle1 = new HObject();
                HTuple hv_Result_Rect2_Len1 = new HTuple(), hv_Result_Rect2_Len2 = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();

                HWindow Window = hWindowControl1.HalconWindow;
                FindAngle_Angle1(My.ho_Image, hv_CenterRow, hv_CenterColumn,hv_CenterPhi,
                out ho_ConnectedRegions, out ho_RegionsResult_Angle1, out hv_Result_Rect2_Len1, out hv_Result_Rect2_Len2, out hv_Row, out hv_Column);

                set_display_font(Window, 15, "mono", "true", "false");
                Window.ClearWindow();
                My.ho_Image.DispObj(Window);
                ho_RegionsResult_Angle1.DispObj(Window);
                for (int i = 0; i < hv_Row.Length; i++)
                {
                    disp_message(Window, Math.Round(hv_Result_Rect2_Len1.DArr[i]).ToString(), "", hv_Row.DArr[i], hv_Column.DArr[i], "green", "false");
                    disp_message(Window, Math.Round(hv_Result_Rect2_Len2.DArr[i]).ToString(), "", hv_Row.DArr[i] + 30, hv_Column.DArr[i], "blue", "false");
                }
                parent.m_LensCarry.m_Result.hv_AngleCenterRow = hv_Row;
                parent.m_LensCarry.m_Result.hv_AngleCenterColumn = hv_Column;
            }
            catch
            {
            }
        }
        public void DetectingRing(HWindow Window, HTuple hv_CenterRow, HTuple hv_CenterColumn, HTuple hv_OuterRadius, HTuple hv_InnerRadius)
        {
            HObject ho_Image = new HObject(), ho_CircleOuter = new HObject(), ho_CircleInner = new HObject();

            if (My.ho_Image == null)
                return;
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            //畫檢視範圍
            Window.ClearWindow();
            ho_CircleOuter.Dispose();
            HOperatorSet.GenCircle(out ho_CircleOuter, hv_CenterRow, hv_CenterColumn, hv_OuterRadius);
            ho_CircleInner.Dispose();
            HOperatorSet.GenCircle(out ho_CircleInner, hv_CenterRow, hv_CenterColumn, hv_InnerRadius);

            HOperatorSet.DispObj(ho_Image, Window);
            HOperatorSet.SetColor(Window, "yellow");
            HOperatorSet.SetDraw(Window, "margin");
            HOperatorSet.DispObj(ho_CircleOuter, Window);
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "orange");
            HOperatorSet.DispObj(ho_CircleInner, Window);

            ho_Image.Dispose();
            ho_CircleOuter.Dispose();
            ho_CircleInner.Dispose();
        }

        private void btnSave_Angle1_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            cbSelectAreaMaximum_Angle1.Checked = true;
            IniFile.Write("Angle1", "ContrastSet", parent.m_LensCarry.m_Angle1.ContrastSet.ToString(), Path);
            IniFile.Write("Angle1", "Gray", parent.m_LensCarry.m_Angle1.Gray.D.ToString(), Path);
            IniFile.Write("Angle1", "OpeningValue", parent.m_LensCarry.m_Angle1.OpeningValue.D.ToString(), Path);
            IniFile.Write("Angle1", "ClosingValue", parent.m_LensCarry.m_Angle1.ClosingValue.D.ToString(), Path);
            IniFile.Write("Angle1", "Rect2_Len1_Upper", parent.m_LensCarry.m_Angle1.Rect2_Len1_Upper.D.ToString(), Path);
            IniFile.Write("Angle1", "Rect2_Len1_Lower", parent.m_LensCarry.m_Angle1.Rect2_Len1_Lower.D.ToString(), Path);
            IniFile.Write("Angle1", "Rect2_Len2_Upper", parent.m_LensCarry.m_Angle1.Rect2_Len2_Upper.D.ToString(), Path);
            IniFile.Write("Angle1", "Rect2_Len2_Lower", parent.m_LensCarry.m_Angle1.Rect2_Len2_Lower.D.ToString(), Path);
            IniFile.Write("Angle1", "SelectAreaMaximum", "true", Path);
        }
        private void btnPosition_CCD_Correction_Click(object sender, EventArgs e)
        {
            if (ucFitCircleTool_Correction.hv_CenterRow.D == 0)
            {
                MessageBox.Show("請先求圓心!");
                return;
            }
            try
            {
                HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
                HOperatorSet.GetImageSize(My.ho_Image, out hv_Width, out hv_Height);
                double Difference_X = ucFitCircleTool_Correction.hv_CenterColumn.D - (hv_Width / 2).D;
                double Difference_Y = ucFitCircleTool_Correction.hv_CenterRow.D - (hv_Height / 2).D;

                if (parent.m_LensCarry.m_Correction.Pixelmm == null)
                    parent.m_LensCarry.m_Correction.Pixelmm = 4.4 / 1000;
                Protocol.Position_Distance_X = parent.m_LensCarry.m_Correction.Position_CCDDifference_X = Math.Round(Difference_X * parent.m_LensCarry.m_Correction.Pixelmm.D * -1, 2);
                Protocol.Position_Distance_Y = parent.m_LensCarry.m_Correction.Position_CCDDifference_Y = Math.Round(Difference_Y * parent.m_LensCarry.m_Correction.Pixelmm.D * -1, 2);

                lblPosition_CCDDifference_X_Correction.Text = parent.m_LensCarry.m_Correction.Position_CCDDifference_X.ToString();
                lblPosition_CCDDifference_Y_Correction.Text = parent.m_LensCarry.m_Correction.Position_CCDDifference_Y.ToString();

                lblPosition_CCDCenter_X_Correction.Text = parent.m_LensCarry.m_Correction.Position_CCDDifference_X.ToString();
                lblPosition_CCDCenter_Y_Correction.Text = parent.m_LensCarry.m_Correction.Position_CCDDifference_Y.ToString();
            }
            catch
            {
            }
        }

        private void btnSave_Correction_3_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Correction", "Position_Distance_X", Protocol.Position_Distance_X.ToString(), Path);
            IniFile.Write("Correction", "Position_Distance_Y", Protocol.Position_Distance_Y.ToString(), Path);
        }

        private void btnWritePLC_Correction_Click(object sender, EventArgs e)
        {
            Protocol.bPositionDistance_SuctionNozzleCCD = true;
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
            if (My.bCurrention)
            {
                btnCurrectionOpen.BackColor = Color.Green;
            }
            else
            {
                btnCurrectionOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            }
        }

        private void btnImageProPlus_Click(object sender, EventArgs e)
        {
            ImageProPlus(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);
        }
        /// <summary>
        /// 處理圖像(1:成功,-1:抓圓心失敗,-2:模組異常,-3:找不到模組)
        /// </summary>
        /// <param name="_Window"></param>
        /// <param name="theImage"></param>
        /// <param name="n"></param>
        public void ImageProPlus(HWindow _Window, HObject theImage, int n)
        {
            HObject ho_ResultImage = new HObject();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            double ResultAngle = 0;
            double OffsetCenterRow = 0;
            double OffsetCenterColumn = 0;
            int iImageProcessResult = 0;
            //檢測
            iImageProcessResult = ImageProcess_FindAngle(_Window, theImage, out ho_ResultImage, out ResultAngle, out OffsetCenterRow, out OffsetCenterColumn);

            HOperatorSet.GetImageSize(ho_ResultImage, out hv_Width, out hv_Height);
            HOperatorSet.SetPart(_Window, 0, 0, hv_Height - 1, hv_Width - 1);
            _Window.ClearWindow();
            ho_ResultImage.DispObj(_Window);
            switch (iImageProcessResult)
            {
                case 1:
                    {
                        Vision.VisionResult[n] = "OK";
                    } break;
                case -2://模組不匹配
                    {
                        Vision.VisionResult[n] = "Miss";
                    } break;
                default:
                    {
                        Vision.VisionResult[n] = "NG";
                    } break;
            }
            
            Vision.VisionBarcodeRotate[n] = (int)ResultAngle;
            Vision.VisionOffsetCenterRow[n] = OffsetCenterRow;
            Vision.VisionOffsetCenterColumn[n] = OffsetCenterColumn;
            Vision.Images_1[n] = ho_ResultImage.CopyObj(1,-1);
            Vision.Images_Now[n] = Vision.Images_1[n];
            Vision.ImagesOriginal_1[n] = theImage;
            WriteLog(n, Vision.VisionResult[n], ResultAngle, OffsetCenterColumn, OffsetCenterRow);
        }

        public int ImageProcess_FindAngle(HWindow Window, HObject ho_Image, out HObject ho_ResultImage, out double ResultAngle, out double OffsetCenterRow, out double OffsetCenterColumn)
        {
            HOperatorSet.GenEmptyObj(out ho_ResultImage);
            int iImageProcessResult = 0;
            ResultAngle = 0;
            OffsetCenterRow = 0;
            OffsetCenterColumn = 0;
            #region 初始中心模組匹配
            HObject ho_Circle = new HObject(), ho_CenterCross = new HObject();
            HTuple hv_CenterRow = new HTuple(), hv_CenterColumn = new HTuple(), hv_Score = new HTuple(), hv_CenterPhi = new HTuple();
            try
            {
                //中心模組匹配(1:成功,-1:模組異常,-2:找不到模組)
                iImageProcessResult = FindFirstModel_CircleCenter(Window, ho_Image, out ho_Circle, out hv_CenterRow, out hv_CenterColumn, out hv_Score, out hv_CenterPhi);
            }
            catch
            {
                iImageProcessResult = -1;
            }
            //中心模組匹配出錯
            if (iImageProcessResult != 1)
            {
                Window.ClearWindow();
                ho_Image.DispObj(Window);

                disp_message(Window, "圓心模組NG", "", 0, 0, "red", "false");
                disp_message(Window, "NG", "", 1000, 0, "green", "false");
                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                return iImageProcessResult;
            }
            else
            {
                ho_CenterCross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_CenterCross, hv_CenterRow, hv_CenterColumn, 50, 0);
            }
            #endregion
            #region 抓準確圓心
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject(), ho_CrossCenter = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject(), ho_CrossCenter2 = new HObject();
            try
            {
                ucFitCircleTool_CircleCenter.hv_InitialRow = hv_CenterRow;
                ucFitCircleTool_CircleCenter.hv_InitialColumn = hv_CenterColumn;
                iImageProcessResult = ucFitCircleTool_CircleCenter.ImageProcess_FitCircle(ho_Image,out ho_ResultContours, out ho_CrossCenter, out hv_CenterRow, out hv_CenterColumn);
            }
            catch
            {
            }
            //準確圓心出錯
            if (iImageProcessResult != 1)
            {
                Window.ClearWindow();
                ho_Image.DispObj(Window);

                disp_message(Window, "圓心NG", "", 0, 0, "red", "false");
                disp_message(Window, "NG", "", 1000, 0, "green", "false");
                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                return iImageProcessResult;
            }
            #endregion
            #region 抓角度
            HTuple hv_AngleCenterRow = new HTuple(), hv_AngleCenterColumn = new HTuple(), hv_ResultPhi = new HTuple();
            HObject ho_TransContours = new HObject(), ho_Cross = new HObject();

            
            HObject ho_ConnectedRegions = new HObject(), ho_RegionsResult_Angle2 = new HObject();
            HTuple hv_Result_Rect2_Len1 = new HTuple(), hv_Result_Rect2_Len2 = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
      
            iImageProcessResult = FindAngle_Angle1(ho_Image, hv_CenterRow, hv_CenterColumn,hv_CenterPhi,
                out ho_ConnectedRegions, out ho_RegionsResult_Angle2, out hv_Result_Rect2_Len1, out hv_Result_Rect2_Len2, out hv_AngleCenterRow, out hv_AngleCenterColumn);
            if (iImageProcessResult == 1)
            {
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_AngleCenterRow, hv_AngleCenterColumn, 50, 0);

                HOperatorSet.AngleLx(hv_CenterRow, hv_CenterColumn, hv_AngleCenterRow, hv_AngleCenterColumn, out hv_ResultPhi);

                ResultAngle = Math.Round(hv_ResultPhi.TupleDeg().D);
                ResultAngle = ResultAngle + parent.m_LensCarry.m_Angle1.StandardAngle;
                if (ResultAngle > 180)
                    ResultAngle = ResultAngle - 360;
                else if (ResultAngle < -179)
                    ResultAngle = ResultAngle + 360;
            }
            else
            {
                Window.ClearWindow();
                ho_Image.DispObj(Window);

                disp_message(Window, "角度NG", "", 0, 0, "red", "false");
                disp_message(Window, "NG", "", 1000, 0, "green", "false");
                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                return iImageProcessResult;
                ResultAngle = 0;
            }
            #region 補償圓心
            try
            {
                HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                OffsetCenterRow = Math.Round((hv_CenterRow.D - (hv_Height.D / 2)) * parent.m_LensCarry.m_Correction.Pixelmm.D * -1, 2);
                OffsetCenterColumn = Math.Round((hv_CenterColumn.D - (hv_Width.D / 2)) * parent.m_LensCarry.m_Correction.Pixelmm.D * -1, 2);
            }
            catch
            {
            }
            #endregion
            if (iImageProcessResult == 1)
            {
                set_display_font(Window, 20, "mono", "true", "false");
                Window.ClearWindow();
                ho_Image.DispObj(Window);
                Window.SetColor("yellow");
                ho_CenterCross.DispObj(Window);
                ho_ResultContours.DispObj(Window);
                Window.SetColor("green");
                ho_Cross.DispObj(Window);
                Window.SetColor("blue");
                Window.DispLine(hv_CenterRow, hv_CenterColumn, hv_AngleCenterRow, hv_AngleCenterColumn);
                disp_message(Window, "角度:" + ResultAngle, "", 0, 0, "green", "false");
                disp_message(Window, "X:" + OffsetCenterColumn, "", 40, 0, "green", "false");
                disp_message(Window, "Y:" + OffsetCenterRow, "", 80, 0, "green", "false");
                disp_message(Window, "OK", "", 1000, 0, "green", "false");
            }
            else
            {
                Window.ClearWindow();
                ho_Image.DispObj(Window);
                Window.SetColor("yellow");
                ho_CenterCross.DispObj(Window);

                disp_message(Window, "角度NG", "", 0, 0, "red", "false");
                disp_message(Window, "X:" + OffsetCenterColumn, "", 40, 0, "green", "false");
                disp_message(Window, "Y:" + OffsetCenterRow, "", 80, 0, "green", "false");
                disp_message(Window, "NG", "", 1000, 0, "red", "false");
            }
            HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
            
            #endregion
            return iImageProcessResult;
        }
        #region 光源設定
        private void btnOn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            ReverseOnOff(btn);
        }

        private void btnLightSave_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("SettingLight", "Light1", Light.LightSet_1.ToString(), Path);
            IniFile.Write("SettingLight", "Light2", Light.LightSet_2.ToString(), Path);
            IniFile.Write("SettingLight", "Light3", Light.LightSet_3.ToString(), Path);
            IniFile.Write("SettingLight", "Light4", Light.LightSet_4.ToString(), Path);
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

        private void ucLightSet_1_ValueChanged(object sender, EventArgs e)
        {
            Light.LightSet_1 = ((int)ucLightSet_1.Value);
            try
            {
                //這段要另外用的
                LightSetting(1 - 1, Light.LightSet_1);
            }
            catch
            {
            }
        }

        private void ucLightSet_2_ValueChanged(object sender, EventArgs e)
        {
            Light.LightSet_2 = ((int)ucLightSet_2.Value);
            try
            {
                //這段要另外用的
                LightSetting(2 - 1, Light.LightSet_2);
            }
            catch
            {
            }
        }

        private void ucLightSet_3_ValueChanged(object sender, EventArgs e)
        {
            Light.LightSet_3 = ((int)ucLightSet_3.Value);
            try
            {
                //這段要另外用的
                LightSetting(3 - 1, Light.LightSet_3);
            }
            catch
            {
            }
        }

        private void ucLightSet_4_ValueChanged(object sender, EventArgs e)
        {
            Light.LightSet_4 = ((int)ucLightSet_4.Value);
            try
            {
                //這段要另外用的
                LightSetting(4 - 1, Light.LightSet_4);
            }
            catch
            {
            }
        }
        #endregion

        private void btnCCDSetSave_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            CCD.Gain = ucGain.Value;
            CCD.ExposureTime = ucExposureTime.Value;
            IniFile.Write("Setting", "Gain", CCD.Gain.ToString(), Path);
            IniFile.Write("Setting", "ExposureTime", CCD.ExposureTime.ToString(), Path);
        }

        private void ucGain_ValueChanged(object sender, EventArgs e)
        {
            CCD.Gain = (double)ucGain.Value;
            parent.SetGain(CCD.Gain);
        }

        private void ucExposureTime_ValueChanged(object sender, EventArgs e)
        {
            CCD.ExposureTime = (double)ucExposureTime.Value;
            parent.SetGain(CCD.ExposureTime);
        }

        public void WriteLog(int n, string ResultOK, double Angle,double Offset_X,double Offset_Y)
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
                            "\tOperatorID\tMachine No.\tTime\tCT\tResult\tAngle\t" +
                        "Offset_X\tOffset_Y" +
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
                                     Angle + "\t" +
                                     Offset_X + "\t" +
                                     Offset_Y);
                    }
                }
                catch
                {
                }

            }
        }

        private void cbInterSectingRectangle_Angle1_CheckedChanged(object sender, EventArgs e)
        {
            parent.m_LensCarry.m_Angle1.InterSectingRectangle = cbInterSectingRectangle_Angle1.Checked;
            if (parent.m_LensCarry.m_Angle1.InterSectingRectangle)
            {
                ucFitRectangle2_Angle1.Enabled = true;
            }
            else
            {
                ucFitRectangle2_Angle1.Enabled = false;
            }

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Angle1", "InterSectingRectangle", parent.m_LensCarry.m_Angle1.InterSectingRectangle.ToString(), Path);
        }

        private void btnSave2_Angle1_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Angle1", "OuterRadius", parent.m_LensCarry.m_Angle1.OuterRadius.D.ToString(), Path);
            IniFile.Write("Angle1", "InnerRadius", parent.m_LensCarry.m_Angle1.InnerRadius.D.ToString(), Path);
            IniFile.Write("Angle1", "Length1", ucFitRectangle2_Angle1.Length1.ToString(), Path);
            IniFile.Write("Angle1", "Length2", ucFitRectangle2_Angle1.Length2.ToString(), Path);
            IniFile.Write("Angle1", "Measure_Transition", ucFitRectangle2_Angle1.Measure_Transition, Path);
            IniFile.Write("Angle1", "Measure_Select", ucFitRectangle2_Angle1.Measure_Select, Path);
            IniFile.Write("Angle1", "Num_Measures", ucFitRectangle2_Angle1.Num_Measures.ToString(), Path);
            IniFile.Write("Angle1", "Measure_Length1", ucFitRectangle2_Angle1.Measure_Length1.ToString(), Path);
            IniFile.Write("Angle1", "Measure_Length2", ucFitRectangle2_Angle1.Measure_Length2.ToString(), Path);
            IniFile.Write("Angle1", "Measure_Threshold", ucFitRectangle2_Angle1.Measure_Threshold.ToString(), Path);
        }

        private void cmbModelMode_CircleCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            parent.m_LensCarry.m_CircleCenter.ModelMode = cmbModelMode_CircleCenter.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("CircleCenter", "ModelMode", parent.m_LensCarry.m_CircleCenter.ModelMode.ToString(), Path);
        }

        private void cbClosing_Angle1_CheckedChanged(object sender, EventArgs e)
        {
            parent.m_LensCarry.m_Angle1.Closing = cbClosing_Angle1.Checked;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Angle1", "Closing", parent.m_LensCarry.m_Angle1.Closing.ToString(), Path);
        }

        private void cbOpening_Angle1_CheckedChanged(object sender, EventArgs e)
        {
            parent.m_LensCarry.m_Angle1.Opening = cbOpening_Angle1.Checked;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Angle1", "Opening", parent.m_LensCarry.m_Angle1.Opening.ToString(), Path);
        }

        private void ucClosingValue_Angle1_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            parent.m_LensCarry.m_Angle1.ClosingValue = ucClosingValue_Angle1.Value;

            if (parent.m_LensCarry.m_Result.hv_CenterRow.D == 0)
            {
                MessageBox.Show("請先求圓心!");
                return;
            }

            HTuple hv_CenterRow = parent.m_LensCarry.m_Result.hv_CenterRow;
            HTuple hv_CenterColumn = parent.m_LensCarry.m_Result.hv_CenterColumn;
            HTuple hv_CenterPhi = parent.m_LensCarry.m_Result.hv_CenterPhi;
            HObject ho_ConnectedRegions = new HObject(), ho_RegionsResult_Angle1 = new HObject();
            HTuple hv_Result_Rect2_Len1 = new HTuple(), hv_Result_Rect2_Len2 = new HTuple(), hv_AngleCenterRow = new HTuple(), hv_AngleCenterColumn = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HWindow Window = hWindowControl1.HalconWindow;
            FindAngle_Angle1(My.ho_Image, hv_CenterRow, hv_CenterColumn, hv_CenterPhi,
             out ho_ConnectedRegions, out ho_RegionsResult_Angle1, out hv_Result_Rect2_Len1, out hv_Result_Rect2_Len2, out hv_AngleCenterRow, out hv_AngleCenterColumn);

            HOperatorSet.RegionFeatures(ho_ConnectedRegions, "rect2_len1", out hv_Result_Rect2_Len1);
            HOperatorSet.RegionFeatures(ho_ConnectedRegions, "rect2_len2", out hv_Result_Rect2_Len2);
            HOperatorSet.RegionFeatures(ho_ConnectedRegions, "row", out hv_Row);
            HOperatorSet.RegionFeatures(ho_ConnectedRegions, "column", out hv_Column);

            Window.ClearWindow();
            My.ho_Image.DispObj(Window);
            Window.SetDraw("fill");
            Window.SetColor("yellow");
            ho_ConnectedRegions.DispObj(Window);
            set_display_font(Window, 15, "mono", "true", "false");
            for (int i = 0; i < hv_Row.Length; i++)
            {
                disp_message(Window, Math.Round(hv_Result_Rect2_Len1.DArr[i]).ToString(), "", hv_Row.DArr[i], hv_Column.DArr[i], "green", "false");
                disp_message(Window, Math.Round(hv_Result_Rect2_Len2.DArr[i]).ToString(), "", hv_Row.DArr[i] + 30, hv_Column.DArr[i], "blue", "false");
            }
        }

        private void ucOpeningValue_Angle1_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            parent.m_LensCarry.m_Angle1.OpeningValue = ucOpeningValue_Angle1.Value;

            if (parent.m_LensCarry.m_Result.hv_CenterRow.D == 0)
            {
                MessageBox.Show("請先求圓心!");
                return;
            }

            HTuple hv_CenterRow = parent.m_LensCarry.m_Result.hv_CenterRow;
            HTuple hv_CenterColumn = parent.m_LensCarry.m_Result.hv_CenterColumn;
            HTuple hv_CenterPhi = parent.m_LensCarry.m_Result.hv_CenterPhi;
            HObject ho_ConnectedRegions = new HObject(), ho_RegionsResult_Angle1 = new HObject();
            HTuple hv_Result_Rect2_Len1 = new HTuple(), hv_Result_Rect2_Len2 = new HTuple(), hv_AngleCenterRow = new HTuple(), hv_AngleCenterColumn = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HWindow Window = hWindowControl1.HalconWindow;
            FindAngle_Angle1(My.ho_Image, hv_CenterRow, hv_CenterColumn, hv_CenterPhi,
             out ho_ConnectedRegions, out ho_RegionsResult_Angle1, out hv_Result_Rect2_Len1, out hv_Result_Rect2_Len2, out hv_AngleCenterRow, out hv_AngleCenterColumn);

            HOperatorSet.RegionFeatures(ho_ConnectedRegions, "rect2_len1", out hv_Result_Rect2_Len1);
            HOperatorSet.RegionFeatures(ho_ConnectedRegions, "rect2_len2", out hv_Result_Rect2_Len2);
            HOperatorSet.RegionFeatures(ho_ConnectedRegions, "row", out hv_Row);
            HOperatorSet.RegionFeatures(ho_ConnectedRegions, "column", out hv_Column);

            Window.ClearWindow();
            My.ho_Image.DispObj(Window);
            Window.SetDraw("fill");
            Window.SetColor("yellow");
            ho_ConnectedRegions.DispObj(Window);
            set_display_font(Window, 15, "mono", "true", "false");
            for (int i = 0; i < hv_Row.Length; i++)
            {
                disp_message(Window, Math.Round(hv_Result_Rect2_Len1.DArr[i]).ToString(), "", hv_Row.DArr[i], hv_Column.DArr[i], "green", "false");
                disp_message(Window, Math.Round(hv_Result_Rect2_Len2.DArr[i]).ToString(), "", hv_Row.DArr[i] + 30, hv_Column.DArr[i], "blue", "false");
            }
        }

        private void nudStandardAngle_Angle1_ValueChanged(object sender, EventArgs e)
        {
            if(bReadingPara)
                return;
            parent.m_LensCarry.m_Angle1.StandardAngle = (int)nudStandardAngle_Angle1.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            cbSelectAreaMaximum_Angle1.Checked = true;
            IniFile.Write("Angle1", "StandardAngle", parent.m_LensCarry.m_Angle1.StandardAngle.ToString(), Path);
        }
    }
}
