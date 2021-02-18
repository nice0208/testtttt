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
using System.Diagnostics;
using System.IO;

namespace Detecting_System
{
    public partial class FrmVDI_NIR : Form
    {
        FrmParent parent;
        FrmRun Run;
        double mm_2_pixel = 360.0;
        Processor Pr;
        public static bool DrawBarcodeNow = false;
        bool bReadPara = false;
        public FrmVDI_NIR(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;
            InitializeComponent();
            ToolTip label4_tool = new ToolTip();
            ToolTip label5_tool = new ToolTip();
            ToolTip label6_tool = new ToolTip();
            ToolTip label9_tool = new ToolTip();
            ToolTip label8_tool = new ToolTip();
            ToolTip label10_tool = new ToolTip();
            ToolTip label11_tool = new ToolTip();
            ToolTip label2_tool = new ToolTip();
     
            label4_tool.SetToolTip(this.label4, "程式会自动依照照片判定胶水阀值，而此数值则是对程式选取的阀值进行调整，数值越小越容易误判为胶水, 数值越大则越可能漏判为胶水");
            label5_tool.SetToolTip(this.label5, "程式只能接受样品在此角度范围内旋转(已水平为基准");
            label6_tool.SetToolTip(this.label6, "亮点阀值,越大亮点越小");
            label9_tool.SetToolTip(this.label9, "对于落在圆半径的黑色区域，程式会忽略");
            label8_tool.SetToolTip(this.label8, "独立胶水的最小面积");
            label10_tool.SetToolTip(this.label10, "红色面积标准%");
            label11_tool.SetToolTip(this.label11, "青色面积标准%");
            label2_tool.SetToolTip(this.label2, "黄色面积标准%");
       
        }

        public HTuple hv_ExpDefaultWinHandle;

        public HObject ho_Image = null, ResultRegion = null, SpotRegion = null, LeakageRegion = null,inside_scar_region = null, ho_Rectangle = null; 
        HTuple hv_Width, hv_Height,hv_Row10,hv_Column10,hv_Phi10,hv_Length10,hv_Length20;

        HTuple BarcodeRange_Row1 = null,BarcodeRange_Column1 = null,BarcodeRange_Row2 = null,BarcodeRange_Column2 = null;
        HObject BarcodeRange = null,ho_BarcodeResultXLD = new HObject();
                

        #region halcon參數一
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
        public void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
           HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_GenParamName = null, hv_GenParamValue = null;
            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_CoordSystem_COPY_INP_TMP = hv_CoordSystem.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();

            // Initialize local and output iconic variables 
            //This procedure displays text in a graphics window.
            //
            //Input parameters:
            //WindowHandle: The WindowHandle of the graphics window, where
            //   the message should be displayed
            //String: A tuple of strings containing the text message to be displayed
            //CoordSystem: If set to 'window', the text position is given
            //   with respect to the window coordinate system.
            //   If set to 'image', image coordinates are used.
            //   (This may be useful in zoomed images.)
            //Row: The row coordinate of the desired text position
            //   A tuple of values is allowed to display text at different
            //   positions.
            //Column: The column coordinate of the desired text position
            //   A tuple of values is allowed to display text at different
            //   positions.
            //Color: defines the color of the text as string.
            //   If set to [], '' or 'auto' the currently set color is used.
            //   If a tuple of strings is passed, the colors are used cyclically...
            //   - if |Row| == |Column| == 1: for each new textline
            //   = else for each text position.
            //Box: If Box[0] is set to 'true', the text is written within an orange box.
            //     If set to' false', no box is displayed.
            //     If set to a color string (e.g. 'white', '#FF00CC', etc.),
            //       the text is written in a box of that color.
            //     An optional second value for Box (Box[1]) controls if a shadow is displayed:
            //       'true' -> display a shadow in a default color
            //       'false' -> display no shadow
            //       otherwise -> use given string as color string for the shadow color
            //
            //It is possible to display multiple text strings in a single call.
            //In this case, some restrictions apply:
            //- Multiple text positions can be defined by specifying a tuple
            //  with multiple Row and/or Column coordinates, i.e.:
            //  - |Row| == n, |Column| == n
            //  - |Row| == n, |Column| == 1
            //  - |Row| == 1, |Column| == n
            //- If |Row| == |Column| == 1,
            //  each element of String is display in a new textline.
            //- If multiple positions or specified, the number of Strings
            //  must match the number of positions, i.e.:
            //  - Either |String| == n (each string is displayed at the
            //                          corresponding position),
            //  - or     |String| == 1 (The string is displayed n times).
            //
            //
            //Convert the parameters for disp_text.
            if ((int)((new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(new HTuple())))) != 0)
            {

                return;
            }
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            //
            //Convert the parameter Box to generic parameters.
            hv_GenParamName = new HTuple();
            hv_GenParamValue = new HTuple();
            if ((int)(new HTuple((new HTuple(hv_Box.TupleLength())).TupleGreater(0))) != 0)
            {
                if ((int)(new HTuple(((hv_Box.TupleSelect(0))).TupleEqual("false"))) != 0)
                {
                    //Display no box
                    hv_GenParamName = hv_GenParamName.TupleConcat("box");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat("false");
                }
                else if ((int)(new HTuple(((hv_Box.TupleSelect(0))).TupleNotEqual("true"))) != 0)
                {
                    //Set a color other than the default.
                    hv_GenParamName = hv_GenParamName.TupleConcat("box_color");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat(hv_Box.TupleSelect(0));
                }
            }
            if ((int)(new HTuple((new HTuple(hv_Box.TupleLength())).TupleGreater(1))) != 0)
            {
                if ((int)(new HTuple(((hv_Box.TupleSelect(1))).TupleEqual("false"))) != 0)
                {
                    //Display no shadow.
                    hv_GenParamName = hv_GenParamName.TupleConcat("shadow");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat("false");
                }
                else if ((int)(new HTuple(((hv_Box.TupleSelect(1))).TupleNotEqual("true"))) != 0)
                {
                    //Set a shadow color other than the default.
                    hv_GenParamName = hv_GenParamName.TupleConcat("shadow_color");
                    hv_GenParamValue = hv_GenParamValue.TupleConcat(hv_Box.TupleSelect(1));
                }
            }
            //Restore default CoordSystem behavior.
            if ((int)(new HTuple(hv_CoordSystem_COPY_INP_TMP.TupleNotEqual("window"))) != 0)
            {
                hv_CoordSystem_COPY_INP_TMP = "image";
            }
            //
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(""))) != 0)
            {
                //disp_text does not accept an empty string for Color.
                hv_Color_COPY_INP_TMP = new HTuple();
            }
            //
            HOperatorSet.DispText(hv_ExpDefaultWinHandle, hv_String, hv_CoordSystem_COPY_INP_TMP,
                hv_Row_COPY_INP_TMP, hv_Column_COPY_INP_TMP, hv_Color_COPY_INP_TMP, hv_GenParamName,
                hv_GenParamValue);

            return;

        }

        public void gen_circle_center(HObject ho_Image, out HObject ho_UsedEdges, out HObject ho_Contour, out HObject ho_ResultContours, out HObject ho_CrossCenter, HTuple hv_InitialRow,
           HTuple hv_InitialColumn, HTuple hv_InitialRadius, HTuple hv_Length, HTuple hv_Measure_Threshold, HTuple hv_MeasureTransition, HTuple hv_MeasureSelect, out HTuple hv_ResultRow, out HTuple hv_ResultColumn, out HTuple hv_ResultRadius)
        {
            // Local iconic variables 

            HObject ho_ModelContour, ho_Contours;

            // Local control variables 

            HTuple hv_MetrologyHandle = null, hv_circleIndices = null;
            HTuple hv_circleParameter = null, hv_Row = null, hv_Column = null;
            HTuple hv_UsedRow = null, hv_UsedColumn = null, hv_StartPhi = null;
            HTuple hv_EndPhi = null, hv_PointOrder = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            HOperatorSet.GenEmptyObj(out ho_ModelContour);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            try
            {
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.AddMetrologyObjectGeneric(hv_MetrologyHandle, "circle", ((hv_InitialRow.TupleConcat(
                    hv_InitialColumn))).TupleConcat(hv_InitialRadius), hv_Length, 5, 1, hv_Measure_Threshold,
                    new HTuple(), new HTuple(), out hv_circleIndices);
                ho_ModelContour.Dispose();
                HOperatorSet.GetMetrologyObjectModelContour(out ho_ModelContour, hv_MetrologyHandle,
                    "all", 1.5);
                //第一個點或最後一個點
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices,
                    "measure_select", hv_MeasureSelect);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices,
                    "min_score", 0.2);
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_circleIndices,
                    "all", "result_type", "all_param", out hv_circleParameter);

                //白找黑('negative')或黑找白('positive')
                ho_Contour.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour, hv_MetrologyHandle,
                    "all", hv_MeasureTransition, out hv_Row, out hv_Column);
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
                HOperatorSet.FitCircleContourXld(ho_ResultContours, "algebraic", -1, 0, 0,
                    3, 2, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius, out hv_StartPhi,
                    out hv_EndPhi, out hv_PointOrder);
                ho_CrossCenter.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_CrossCenter, hv_ResultRow, hv_ResultColumn, 50, 0);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                ho_ModelContour.Dispose();
                ho_Contours.Dispose();
                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                ho_ModelContour.Dispose();
                ho_Contours.Dispose();
                throw HDevExpDefaultException;
            }
        }

        public void gen_rectangle2_center(HObject ho_Image, out HObject ho_Contour, out HObject ho_UsedEdges,
      out HObject ho_ResultContours, HTuple hv_InitialRow, HTuple hv_InitialColumn,
      HTuple hv_InitialPhi, HTuple hv_InitialLength1, HTuple hv_InitialLength2, HTuple hv_MeasureLength,
      HTuple hv_MeasureThreshold, HTuple hv_MeasureTransition, HTuple hv_MeasureSelect,
      out HTuple hv_ResultRow, out HTuple hv_ResultColumn, out HTuple hv_ResultPhi,
      out HTuple hv_ResultLength1, out HTuple hv_ResultLength2)
        {




            // Local iconic variables 

            HObject ho_ModelContour, ho_CrossCenter, ho_Contours;

            // Local control variables 

            HTuple hv_MetrologyHandle = null, hv_circleIndices = null;
            HTuple hv_circleParameter = null, hv_Row = null, hv_Column = null;
            HTuple hv_UsedRow = null, hv_UsedColumn = null, hv_PointOrder1 = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_ModelContour);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            try
            {
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
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices,
                    "measure_select", hv_MeasureSelect);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices,
                    "min_score", 0.2);
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_circleIndices,
                    "all", "result_type", "all_param", out hv_circleParameter);
                ho_CrossCenter.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_CrossCenter, hv_circleParameter.TupleSelect(
                    0), hv_circleParameter.TupleSelect(1), 20, 0.785398);
                //白找黑('negative')或黑找白('positive')
                ho_Contour.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour, hv_MetrologyHandle,
                    "all", hv_MeasureTransition, out hv_Row, out hv_Column);
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
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                ho_ModelContour.Dispose();
                ho_CrossCenter.Dispose();
                ho_Contours.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ModelContour.Dispose();
                ho_CrossCenter.Dispose();
                ho_Contours.Dispose();

                throw HDevExpDefaultException;
            }
        }

        #endregion
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

        private void FrmVDI_NIR_Load(object sender, EventArgs e)
        {
            ReadPara();
            
            HOperatorSet.GenEmptyObj(out BarcodeRange);
            HOperatorSet.GenEmptyObj(out ho_BarcodeResultXLD);
        }

        public void ReadPara()
        {
            bReadPara = true;
            Pr = new Processor();
            Pr.Load_Template(Sys.ModulePath + "\\" + Sys.Function + "_" + Production.CurProduction + "_model");
            
            //开启照片
            My.ContinueShot = false;
            string ImagePath;           // 定义模板图片的路径
            HObject readImage = null;	// 定义一个图像常量
            cmbBarcodePosition.SelectedIndex = My.NIR.BarcodePosition;
            //ImagePath = Application.StartupPath + @"\Picture\20190907Miss.png";
            //readImage = ReadPicture(hWindowControl1.HalconWindow, ImagePath);
            cbReadBarrelBarcode.Checked = Convert.ToBoolean(My.NIR.ReadBarrelBarcode);
            My.ho_Image = readImage;

            LoadSettingLight();
            ReadSysIni();
            CCDSetPara();
            bReadPara = false;
        }

        public void LoadSettingLight()
        {
            nudLightSet_1.Value = Convert.ToInt32(Light.LightSet_1);
            nudLightSet_2.Value = Convert.ToInt32(Light.LightSet_2);
            nudLightSet_3.Value = Convert.ToInt32(Light.LightSet_3);
            nudLightSet_4.Value = Convert.ToInt32(Light.LightSet_4);
        }

        public void ReadSysIni()
        {
            nudThresh_Offset.Value = decimal.Parse(IniFile.Read("OffSet", "nudThresh_Offset", "0", Sys.SysPath));
            Pr.UpdateParameter("thresh_offset", double.Parse(nudThresh_Offset.Text));

            nudAngle_Range.Text = IniFile.Read("OffSet", "nudAngle_Range", "15", Sys.SysPath);
            Pr.UpdateParameter("angle_range", (double)nudAngle_Range.Value);

            nudSpot_Thresh.Text = IniFile.Read("OffSet", "nudSpot_Thresh", "20", Sys.SysPath);
            Pr.UpdateParameter("spot_threshold", (double)nudSpot_Thresh.Value);

            nudIgnore_Radius.Text = IniFile.Read("OffSet", "nudIgnore_Radius", "350", Sys.SysPath);
            Pr.UpdateParameter("ignore_radius", (double)nudIgnore_Radius.Value);

            nudGlue_Min_Area.Text = IniFile.Read("OffSet", "nudGlue_Min_Area", "100", Sys.SysPath);
            Pr.UpdateParameter("glue_min_area", (double)nudGlue_Min_Area.Value);

            nudRed_Area_Thresh.Text = IniFile.Read("OffSet", "nudRed_Area_Thresh", "50", Sys.SysPath);

            nudCyan_Area_Thresh.Text = IniFile.Read("OffSet", "nudCyan_Area_Thresh", "50", Sys.SysPath);

            nudYellow_Area_Thresh.Value = int.Parse(IniFile.Read("OffSet", "nudYellow_Area_Thresh", "50", Sys.SysPath));

            nudCorner_Area_Thresh.Value = int.Parse(IniFile.Read("OffSet", "nudCorner_Area_Thresh", "50", Sys.SysPath));

            nudLeakage_Numeric.Value = int.Parse(IniFile.Read("OffSet", "nudLeakage_gray_offset", "0", Sys.SysPath));

            nudAll_Area_Thresh.Text = IniFile.Read("OffSet", "nudAll_Area_Thresh", "50", Sys.SysPath);

            nudRed_Cyan_Radius.Value = int.Parse(IniFile.Read("OffSet", "nudRed_Cyan_Radius", "3500", Sys.SysPath));
           
            nudYellow_Red_Radius.Value = int.Parse(IniFile.Read("OffSet", "nudYellow_Red_Radius", "3000", Sys.SysPath));
      
            nudYellow_Inner_Radius.Value = int.Parse(IniFile.Read("OffSet", "nudYellow_Inner_Radius", "3000", Sys.SysPath));

            nudParticle_Area_Thresh.Value = int.Parse(IniFile.Read("OffSet", "nudParticle_Area_Thresh", "300", Sys.SysPath));

            nudScar_Thresh_Offset.Value = int.Parse(IniFile.Read("OffSet", "nudScar_Thresh_Offset", "0", Sys.SysPath));

            nudParticle_Area_Thresh.Value = int.Parse(IniFile.Read("OffSet", "nudParticle_Area_Thresh", "300", Sys.SysPath));

            nudScar_Outer_Radius.Value = int.Parse(IniFile.Read("OffSet", "nudScar_Outer_Radius", "3000", Sys.SysPath));

            nudScar_Inner_Radius.Value = int.Parse(IniFile.Read("OffSet", "nudScar_Inner_Radius", "3000", Sys.SysPath));

            nudScar_Area_Thresh.Value = int.Parse(IniFile.Read("OffSet", "nudScar_Area_Thresh", "300", Sys.SysPath));

            scar_checkbox.Checked = bool.Parse(IniFile.Read("OffSet", "scar_checkbox", "False", Sys.SysPath));
            cbTest.Checked = bool.Parse(IniFile.Read("Test", "Test", "False", Sys.SysPath));
            cmbCondition.SelectedIndex = int.Parse(IniFile.Read("Test", "Condition", "0", Sys.SysPath));
            nudThresh_Upper.Value = decimal.Parse(IniFile.Read("Test", "Thresh_Upper", "0", Sys.SysPath));
            nudThresh_Lower.Value = decimal.Parse(IniFile.Read("Test", "Thresh_Lower", "0", Sys.SysPath));
            nudCorner_Upper.Value = decimal.Parse(IniFile.Read("Test", "Corner_Upper", "0", Sys.SysPath));
            nudCorner_Lower.Value = decimal.Parse(IniFile.Read("Test", "Corner_Lower", "0", Sys.SysPath));

            nudexpected_holder_radius.Value = (decimal)My.NIR.expected_holder_radius.D;
            //找矩形圓心參數
            cmbMeasureSelect_RectangleCenter.SelectedIndex = (int)My.NIR.MeasureSelect_RectangleCenter.D;
            ucLength_RectangleCenter.Value = (int)My.NIR.Length_RectangleCenter.D;
            if (My.NIR.MeasureTransition_Center == "positive")
            {
                ucBlack2White_RectangleCenter.Value = (int)My.NIR.MeasureThreshold_RectangleCenter.D;
            }
            else
            {
                ucWhite2Black_RectangleCenter.Value = (int)My.NIR.MeasureThreshold_RectangleCenter.D;
            }
           //找圓心參數
            cmbMeasureSelect_Center.SelectedIndex = (int)My.NIR.MeasureSelect_Center.D;
            ucRadius_Center.Value = (int)My.NIR.Radius_Center.D;
            ucLength_Center.Value = (int)My.NIR.Length_Center.D;
            if (My.NIR.MeasureTransition_Center == "positive")
            {
                ucBlack2White_Center.Value = (int)My.NIR.MeasureThreshold_Center.D;
            }
            else
            {
                ucWhite2Black_Center.Value = (int)My.NIR.MeasureThreshold_Center.D;
            }
            cbCheck_ExcessiveGlue.Checked = My.NIR.Check_ExcessiveGlue;
            nudMode_ExcessiveGlue.Value = My.NIR.Mode_ExcessiveGlue;
            ucOuterRadius_ExcessiveGlue.Value = (int)My.NIR.OuterRadius_ExcessiveGlue;
            ucInnerRadius_ExcessiveGlue.Value = (int)My.NIR.InnerRadius_ExcessiveGlue;
            cmbDarkLightChoice_ExcessiveGlue.SelectedIndex = (int)My.NIR.DarkLightChoice_ExcessiveGlue;
            nudOffset_ExcessiveGlue.Value = (int)My.NIR.OffSet_ExcessiveGlue; 
            
            cmbDarkLightChoice_Scar.SelectedIndex = int.Parse(IniFile.Read("Scar", "DarkLightChoice", "0", Sys.SysPath));
            spot_checkbox.Checked = bool.Parse(IniFile.Read("OffSet", "spot_checkbox","false", Sys.SysPath));
            nudOuterRadius_Paricle.Value = (decimal)My.NIR.OuterRadius_Paricle;
            nudInnerRadius_Paricle.Value = (decimal)My.NIR.InnerRadius_Paricle;
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

        private void btnImageProPlus_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            if (My.ho_Image == null)
            {
                MessageBox.Show("無圖片");
                return;
            }
            ImageProPlus(hWindowControl1.HalconWindow,My.ho_Image,Tray.n,Plc.Status);   
        }

        public void ImageProPlus(HWindow Window, HObject theImage, int n, int Status)
        {
            if (bReadPara)
                return;
            Test.Target[n] =false;
            hv_ExpDefaultWinHandle = Window;
            //Window.ClearWindow();
            ho_Image = theImage;
            Result iResult;
            int iPass = -1;
            Stopwatch sp = new Stopwatch();
            sp.Restart();
            int iCode;
            string LensBarcode = "";
            Vision.VisionBarcodeResult[n] = new Vision.BarcodeResult();
            int BarcodeRotate = 0;
            bool ReadBarcodeError = false;
            if (spot_checkbox.Checked)
                iCode = Pr.Process(ho_Image, out ResultRegion, out LeakageRegion, out SpotRegion,out inside_scar_region, out iResult);
            else
                iCode = Pr.Process(ho_Image, out ResultRegion, out LeakageRegion, out inside_scar_region, out iResult);
            sp.Stop();
            Trace.WriteLine(String.Format("Duration: {0}", sp.ElapsedMilliseconds));
            string ErrorDescript = "";
            string errLog = "";
            if (My.NIR.ReadBarrelBarcode && (iCode == 0 || iCode == -5))
            {
                HTuple ReadBarcodeTime = 0;
                string ErrorMessage = "";
                HObject ho_Rectangle = new HObject(),ho_ImageReduced = new HObject(),ho_ImagePart = new HObject();
                int iResult0 = 0;
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle,My.NIR.BarcodeRange_Row1, My.NIR.BarcodeRange_Column1, My.NIR.BarcodeRange_Row2, My.NIR.BarcodeRange_Column2);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
                ho_ImagePart.Dispose();
                HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);
                //解二維碼
                ho_BarcodeResultXLD.Dispose();
                ReadBarcode(ho_ImagePart, out ho_BarcodeResultXLD, out iResult0, out LensBarcode);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.GenRegionContourXld(ho_BarcodeResultXLD, out ExpTmpOutVar_0, "margin");
                    ho_BarcodeResultXLD.Dispose();
                    ho_BarcodeResultXLD = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.MoveRegion(ho_BarcodeResultXLD, out ExpTmpOutVar_0, My.NIR.BarcodeRange_Row1, My.NIR.BarcodeRange_Column1);
                    ho_BarcodeResultXLD.Dispose();
                    ho_BarcodeResultXLD = ExpTmpOutVar_0;
                }
                try
                {
                    HTuple hv_Count = new HTuple();
                    HOperatorSet.CountObj(ResultRegion, out hv_Count);
                    if (My.NIR.BarcodePosition != 0 || hv_Count.D == 0)//判斷二維碼在上還下
                    {
                        HTuple hv_BarcodeRow = new HTuple(), hv_ResultRow = new HTuple();
                        HOperatorSet.RegionFeatures(ResultRegion, "row", out hv_ResultRow);
                        HOperatorSet.RegionFeatures(ho_BarcodeResultXLD, "row", out hv_BarcodeRow);

                        if (My.NIR.BarcodePosition == 1)//設定為上
                        {
                            if (hv_ResultRow >= hv_BarcodeRow)//二維碼在上
                            {
                                Vision.VisionBarcodeRotate[n] = 0;
                            }
                            else
                            {
                                Vision.VisionBarcodeRotate[n] = 180;
                            }
                        }
                        else if (My.NIR.BarcodePosition == 2)
                        {
                            if (hv_ResultRow <= hv_BarcodeRow)//二維碼在下
                            {
                                Vision.VisionBarcodeRotate[n] = 0;
                            }
                            else
                            {
                                Vision.VisionBarcodeRotate[n] = 180;
                            }
                        }
                    }
                }
                catch
                { }

                if (iResult0<0)
                    iCode = -6;
                ho_Rectangle.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImagePart.Dispose();
            }
            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
            HOperatorSet.SetDraw(hv_ExpDefaultWinHandle,"margin");
            HOperatorSet.DispObj(ResultRegion, hv_ExpDefaultWinHandle);
            if (My.NIR.ReadBarrelBarcode && (iCode == 0 || iCode == -5))
            {
                if (!ReadBarcodeError)
                {
                    set_display_font(hv_ExpDefaultWinHandle, 20, "mono", "true", "false");
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(ho_BarcodeResultXLD, hv_ExpDefaultWinHandle);
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 800, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Barrel Barcode:" + String.Format("{0}", LensBarcode));
                    Vision.VisionBarcodeResult[n].Barcode = LensBarcode;
                }
                else
                {
                    Vision.VisionBarcodeResult[n].Barcode = "NA";
                }
            }
            else
            {
                Vision.VisionBarcodeResult[n].Barcode = "NA";
            }
            HTuple AreaParticle = 0, RowParticle = 0, ColumnParticle = 0;
            double dAreaParticle = 0;
            HTuple AreaScar = 0, RowScar = 0, ColumnScar = 0;
            double dAreaScar = 0;
            if (spot_checkbox.Checked)
            {
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.DispObj(SpotRegion, hv_ExpDefaultWinHandle);
               
                HOperatorSet.AreaCenter(SpotRegion, out AreaParticle, out RowParticle, out ColumnParticle);
                SpotRegion.Dispose();
                dAreaParticle = Math.Round(((double)AreaParticle / mm_2_pixel / mm_2_pixel * 1000 * 1000), 0);
                //Particle超過閥值
                if (dAreaParticle > (double)nudParticle_Area_Thresh.Value)
                    iCode = -5;
            }
            
            if (My.NIR.Check_ExcessiveGlue)
            {
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(LeakageRegion, hv_ExpDefaultWinHandle);
            }
            if (scar_checkbox.Checked)
            {
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                HOperatorSet.DispObj(inside_scar_region, hv_ExpDefaultWinHandle);
                HOperatorSet.AreaCenter(inside_scar_region, out AreaScar, out RowScar, out ColumnScar);
                inside_scar_region.Dispose();
                dAreaScar = Math.Round(((double)AreaScar / mm_2_pixel / mm_2_pixel * 1000 * 1000), 0);
                //Particle超過閥值
                if (dAreaScar > (double)nudScar_Area_Thresh.Value)
                {
                    iCode = -7;
                    if(dAreaScar>10000)
                        iCode = -8;
                }
            }
            set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
            ResultRegion.Dispose();
            if (iCode < 0)
            {
                ErrorDescript = Pr.Query_Error_Code_Detail(iCode,out errLog);
            }
            if (iCode < 0 && iCode != -2 && iCode != -3 && iCode != -4 && iCode != -5 && iCode != -6 && iCode != -7 && iCode != -8)
            {
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 100);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ErrorDescript);
                Vision.VisionResult[n] = "Miss";
            }
            else if (iCode <= -2 && iCode >= -4)//明顯異常
            {
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1500, 100);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ErrorDescript);
                Vision.VisionResult[n] = "NG3";
            }
            else
            {
                iPass = Glue_Pass_Test(iResult);
                set_display_font(hv_ExpDefaultWinHandle, 20, "mono", "true", "false");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "cyan");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_cyan_ratio[0]));
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_yellow_ratio[0]));
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_red_ratio[0]));
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange"); 
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 300, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_ratio[0]));

                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "cyan");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 300);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_cyan_ratio[1]));
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 300);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_yellow_ratio[1]));
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 300);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_red_ratio[1]));
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 300, 300);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_ratio[1]));

                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "cyan");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 600);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_cyan_ratio[2]));
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 600);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_yellow_ratio[2]));
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 600);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_red_ratio[2]));
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 300, 600);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_ratio[2]));

                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "cyan");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 900);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_cyan_ratio[3]));
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 900);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_yellow_ratio[3]));
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 900);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_red_ratio[3]));
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 300, 900);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_ratio[3]));

                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Glue Area Ratio:" + String.Format("{0}%", iResult.all_ratio));
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 500, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "MinDistance:" + String.Format("{0}", (int)iResult.minDistance)+ "um");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 600, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "ParticleArea:" + String.Format("{0}", dAreaParticle)+ "um^2");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 700, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "ScarArea:" + String.Format("{0}", dAreaScar) + "um^2");
                if (My.NIR.BarcodePosition != 0)//判斷二維碼在上還下
                {
                    if (Vision.VisionBarcodeRotate[n] == 180)
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    else
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 900, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Angle:" + String.Format("{0}", Vision.VisionBarcodeRotate[n]));
                }
               
                set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                if (iPass == 0 && iCode != -5 && iCode != -6 && iCode != -7 && iCode != -8)
                {
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 100);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "OK");
                    Vision.VisionResult[n] = "OK";
                    Test.Target[n] = false;
                }
                else 
                {
                    //膠不夠
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    if ((iPass & 1) == 1)
                    {
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2000, 100);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "胶水不够");

                    }
                    if (iResult.TestTarget)//20200228測試用
                    {
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2400, 100);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, nudCorner_Lower.Value + "~" + nudCorner_Upper.Value + "個角" + nudThresh_Lower.Value + "~" + nudThresh_Upper.Value + "之間");
                        Test.Target[n] = true;
                    }
                    else
                    {
                        Test.Target[n] = false;
                    }
                    if ((iPass >> 1 & 1) == 1)
                    {
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1800, 100);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "溢出");

                    }
                    if (iCode == -5)
                    {
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2200, 100);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Particle異常");
                    }
                    if (iCode == -6)
                    {
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2400, 100);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "掃LnesBarcode異常");
                    }
                    if (iCode == -7)
                    {
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1600, 100);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "鏡片缺損");
                    }
                    if (iCode == -8)
                    {
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1600, 100);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "鏡片嚴重缺損");
                    }
                    if (iCode == -8)
                    {
                        Vision.VisionResult[n] = "NG5";
                    }
                    else if (iCode == -7)
                    {
                        Vision.VisionResult[n] = "NG4";
                    }
                    else if ((iPass >> 1 & 1) == 1)
                    {
                        Vision.VisionResult[n] = "NG2";
                    } 
                    else if(iResult.TestTarget)
                    {
                        Vision.VisionResult[n] = "NG";
                    }
                    else if ((iPass & 1) == 1)
                    {
                        Vision.VisionResult[n] = "NG";
                    }
                    else if (iCode == -5 || iCode ==-6)
                    {
                        Vision.VisionResult[n] = "NG3";
                    }
                    
                }
                
            }
            HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
           
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

            WriteLog(n, Vision.VisionResult[n], iPass, ErrorDescript, errLog, iResult, dAreaParticle, LensBarcode, Status);
        }

        public void WriteLog(int n, string ResultOK, int outcome, string ErrorDescript, string errLog, Result glue_area_ratio, double AreaParticle, string LensBarcode,int Status)
        {
            string ExcessiveGlue = "";
            if (ResultOK != "Miss" || ResultOK != "NG3")
            {
                if ((outcome&1) == 1)
                {
                    if (glue_area_ratio.four_cyan_ratio[0] < (double)nudCyan_Area_Thresh.Value)
                    {
                        errLog = errLog + "P1_Cyan_Area_NG,";
                        ErrorDescript = ErrorDescript + "P1_Cyan_Area_NG,";
                    }
                    if (glue_area_ratio.four_red_ratio[0] < (double)nudRed_Area_Thresh.Value)
                    {
                        errLog = errLog + "P1_Red_Area_NG,";
                        ErrorDescript = ErrorDescript + "P1_Red_Area_NG,";
                    }
                    if (glue_area_ratio.four_yellow_ratio[0] < (double)nudYellow_Area_Thresh.Value)
                    {
                        errLog = errLog + "P1_Cyan_Area_NG,";
                        ErrorDescript = ErrorDescript + "P1_Cyan_Area_NG,";
                    }
                    if (glue_area_ratio.four_ratio[0] < (double)nudCorner_Area_Thresh.Value)
                    {
                        errLog = errLog + "P1_Corner_Area_NG,";
                        ErrorDescript = ErrorDescript + "P1_Corner_Area_NG,";
                    }

                    if (glue_area_ratio.four_cyan_ratio[1] < (double)nudCyan_Area_Thresh.Value)
                    {
                        errLog = errLog + "P2_Cyan_Area_NG,";
                        ErrorDescript = ErrorDescript + "P2_Cyan_Area_NG,";
                    }
                    if (glue_area_ratio.four_red_ratio[1] < (double)nudRed_Area_Thresh.Value)
                    {
                        errLog = errLog + "P2_Red_Area_NG,";
                        ErrorDescript = ErrorDescript + "P2_Red_Area_NG,";
                    }
                    if (glue_area_ratio.four_yellow_ratio[1] < (double)nudYellow_Area_Thresh.Value)
                    {
                        errLog = errLog + "P2_Yellow_Area_NG,";
                        ErrorDescript = ErrorDescript + "P2_Yellow_Area_NG,";
                    }
                    if (glue_area_ratio.four_ratio[1] < (double)nudCorner_Area_Thresh.Value)
                    {
                        errLog = errLog + "P2_Corner_Area_NG,";
                        ErrorDescript = ErrorDescript + "P2_Corner_Area_NG,";
                    }

                    if (glue_area_ratio.four_cyan_ratio[2] < (double)nudCyan_Area_Thresh.Value)
                    {
                        errLog = errLog + "P3_Cyan_Area_NG,";
                        ErrorDescript = ErrorDescript + "P3_Cyan_Area_NG,";
                    }
                    if (glue_area_ratio.four_red_ratio[2] < (double)nudRed_Area_Thresh.Value)
                    {
                        errLog = errLog + "P3_Red_Area_NG,";
                        ErrorDescript = ErrorDescript + "P3_Red_Area_NG,";
                    }
                    if (glue_area_ratio.four_yellow_ratio[2] < (double)nudYellow_Area_Thresh.Value)
                    {
                        errLog = errLog + "P3_Yellow_Area_NG,";
                        ErrorDescript = ErrorDescript + "P3_Yellow_Area_NG,";
                    }
                    if (glue_area_ratio.four_ratio[2] < (double)nudCorner_Area_Thresh.Value)
                    {
                        errLog = errLog + "P3_Corner_Area_NG,";
                        ErrorDescript = ErrorDescript + "P3_Corner_Area_NG,";
                    }

                    if (glue_area_ratio.four_cyan_ratio[3] < (double)nudCyan_Area_Thresh.Value)
                    {
                        errLog = errLog + "P4_Cyan_Area_NG,";
                        ErrorDescript = ErrorDescript + "P4_Cyan_Area_NG,";
                    }
                    if (glue_area_ratio.four_red_ratio[3] < (double)nudRed_Area_Thresh.Value)
                    {
                        errLog = errLog + "P4_Red_Area_NG,";
                        ErrorDescript = ErrorDescript + "P4_Red_Area_NG,";
                    }
                    if (glue_area_ratio.four_yellow_ratio[3] < (double)nudYellow_Area_Thresh.Value)
                    {
                        errLog = errLog + "P4_Yellow_Area_NG,";
                        ErrorDescript = ErrorDescript + "P4_Yellow_Area_NG,";
                    }
                    if (glue_area_ratio.four_ratio[3] < (double)nudCorner_Area_Thresh.Value)
                    {
                        errLog = errLog + "P4_Corner_Area_NG,";
                        ErrorDescript = ErrorDescript + "P4_Corner_Area_NG,";
                    }
                }
                if ((outcome >> 1 & 1) == 1)
                {
                    ErrorDescript = ErrorDescript + "excessive glue_NG,";
                    ErrorDescript = ErrorDescript + "excessive glue_NG,";
                    ExcessiveGlue = "NG";
                }
            }


            if (Status == 1)
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
                    if (Tray.NowTray == 1)
                    {
                        Barcode = Tray.Barcode_1;
                    }
                    else if (Tray.NowTray == 2)
                    {
                        Barcode = Tray.Barcode_2;
                    }
                    int CurrentRow = 0;
                    int CurrentColumn = 0;
                    //反推行列
                    if (Tray.NowTray == 1)
                    {
                        CurrentRow = (int)Math.Floor((double)(n / Tray.Columns_1)) + 1;//Math.Floor無條件捨去
                        CurrentColumn = n % Tray.Columns_1 + 1;
                    }
                    else
                    {
                        CurrentRow = (int)Math.Floor((double)(n / Tray.Columns_2)) + 1;
                        CurrentColumn = n % Tray.Columns_2;
                    }
                    //不存在文件，创建先

                    if (!File.Exists(Log))
                    {
                        File.WriteAllText(Log, "Function\tCode\tTray Barcode_B\tTray No.\tToTal Count\tTray Barcode_A\tProgram ID\tProduct\tClass\tOperatorID\tMachine No.\tTime\tCT\tResult\tLensBarcode\tErrorDescript\tCyan_Ratio_1\tRed_Ratio_1\tYellow_Ratio_1\tRatio_1\tCyan_Ratio_2\tRed_Ratio_2\tYellow_Ratio_2\tRatio_2\tCyan_Ratio_3\tRed_Ratio_3\tYellow_Ratio_3\tRatio_3\tCyan_Ratio_4\tRed_Ratio_4\tYellow_Ratio_4\tRatio_4\tAll_Ratio\tMinDistance\tExcessiveGlue\tParticleArea\tTarget" +
                                         "\r\n");
                    }
                    //写result
                    using (StreamWriter sw = new StreamWriter(Log, true))
                    {
                        sw.WriteLine(Sys.FunctionString + "\t" + Sys.Codes + "\t" + Barcode + "\t" +
                            string.Format( "{0}.{1}", CurrentRow, CurrentColumn) + "\t" +
                                     string.Format("{0}",Count.iOK + Count.iNG + Count.iNG2) + "\t" +
                                     "\t" +//空下Tray A版Barcode
                                     Production.CurProduction + "\t" +
                                     Sys.Type + "\t" +
                                     Tray.Class + "\t" +
                                     Tray.OperatorID + "\t" +
                                     Sys.MachineID + "\t" +
                                     DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" +
                                     Protocol.Result_Cycle + "\t" +
                                     ResultOK + "\t" +
                                     LensBarcode + "\t" +
                                     errLog + "\t" +
                                     glue_area_ratio.four_cyan_ratio[0] + "\t" +
                                     glue_area_ratio.four_red_ratio[0] + "\t"+
                                     glue_area_ratio.four_yellow_ratio[0] + "\t" +
                                     glue_area_ratio.four_ratio[0] + "\t" +
                                     glue_area_ratio.four_cyan_ratio[1] + "\t" +
                                     glue_area_ratio.four_red_ratio[1] + "\t"+
                                     glue_area_ratio.four_yellow_ratio[1] + "\t" +
                                     glue_area_ratio.four_ratio[1] + "\t" +
                                     glue_area_ratio.four_cyan_ratio[2] + "\t" +
                                     glue_area_ratio.four_red_ratio[2] + "\t"+
                                     glue_area_ratio.four_yellow_ratio[2] + "\t" +
                                     glue_area_ratio.four_ratio[2] + "\t" +
                                     glue_area_ratio.four_cyan_ratio[3] + "\t" +
                                     glue_area_ratio.four_red_ratio[3] + "\t" +
                                     glue_area_ratio.four_yellow_ratio[3] + "\t" +
                                     glue_area_ratio.four_ratio[3] + "\t" +
                                     glue_area_ratio.all_ratio + "\t" +
                                     glue_area_ratio.minDistance + "\t" +
                                     ExcessiveGlue +"\t" +
                                     AreaParticle + "\t" +
                                     glue_area_ratio.TestTarget.ToString());
                    }
                }
                catch
                {
                }
            }
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

        private void btnCreateModule_Click(object sender, EventArgs e)
        {
            if (My.ho_Image == null)
            {
                MessageBox.Show("無圖片");
                return;
            }
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.DispObj(My.ho_Image, hWindowControl1.HalconWindow);
            ho_Image = My.ho_Image;
            //disp_message(hWindowControl1.HalconWindow, "劃出IR片範圍", "window", 24, 24, "black", "true");

            string ncc_Filename = Sys.ModulePath + "\\" + Sys.Function + "_" + Production.CurProduction + "_model";
            HOperatorSet.DrawRectangle2(hWindowControl1.HalconWindow, out hv_Row10, out hv_Column10, out hv_Phi10, out hv_Length10, out hv_Length20);
            Cursor.Current = Cursors.WaitCursor;
            int result = Pr.CreateTemplate_Manual(ho_Image, hv_Row10, hv_Column10, hv_Phi10, hv_Length10, hv_Length20, 30, ncc_Filename, out ho_Rectangle);
            Cursor.Current = Cursors.Arrow;
            if (result == -1)
            {
                MessageBox.Show("建模失败");
                return;
            }
            else
            {
                MessageBox.Show("建模成功");
                if (hv_Length10 > hv_Length20)
                {
                    My.NIR.Length1_RectangleCenter = hv_Length10;
                    My.NIR.Length2_RectangleCenter = hv_Length20;
                    My.NIR.Phi_RectangleCenter = hv_Phi10;
                }
                else
                {
                    My.NIR.Length1_RectangleCenter = hv_Length20;
                    My.NIR.Length2_RectangleCenter = hv_Length10;
                    My.NIR.Phi_RectangleCenter = hv_Phi10 - (new HTuple(90)).TupleRad();
                }
                IniFile.Write("RectangleCenter", "Phi", My.NIR.Phi_RectangleCenter.D.ToString(), Sys.SysPath);
                IniFile.Write("RectangleCenter", "Length1", My.NIR.Length1_RectangleCenter.D.ToString(), Sys.SysPath);
                IniFile.Write("RectangleCenter", "Length2", My.NIR.Length2_RectangleCenter.D.ToString(), Sys.SysPath);
            }
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "red");
            HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
            HOperatorSet.DispObj(ho_Rectangle, hWindowControl1.HalconWindow);
        }

        private void nudThresh_Offset_ValueChanged(object sender, EventArgs e)
        {
            Pr.UpdateParameter("thresh_offset", (double)nudThresh_Offset.Value);
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudThresh_Offset", nudThresh_Offset.Value.ToString(), Sys.SysPath);
            AfterUpdate_Process_Display();
        }

        private void nudAngle_Range_ValueChanged(object sender, EventArgs e)
        {
            Pr.UpdateParameter("angle_range", (double)nudAngle_Range.Value);
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudAngle_Range", nudAngle_Range.Value.ToString(), Sys.SysPath);
            AfterUpdate_Process_Display();
        }

        private void nudIgnore_Radius_ValueChanged(object sender, EventArgs e)
        {
            Pr.UpdateParameter("ignore_radius", (double)nudIgnore_Radius.Value);
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudIgnore_Radius", nudIgnore_Radius.Value.ToString(), Sys.SysPath);
            AfterUpdate_Process_Display();
        }

        private void nudGlue_Min_Area_ValueChanged(object sender, EventArgs e)
        {
            Pr.UpdateParameter("glue_min_area", (double)nudGlue_Min_Area.Value);
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudGlue_Min_Area", nudGlue_Min_Area.Value.ToString(), Sys.SysPath);
            
            AfterUpdate_Process_Display();
        }

        private void nudRed_Cyan_Radius_ValueChanged(object sender, EventArgs e)
        {
            Pr.UpdateParameter("red_cyan_radius", (double)(nudRed_Cyan_Radius.Value) * mm_2_pixel / 1000.0 / 2);
            if (bReadPara)
                return;
            if (nudRed_Cyan_Radius.Value <= nudYellow_Red_Radius.Value)
            {
                nudYellow_Red_Radius.Value = nudRed_Cyan_Radius.Value - 50;
                return;
            }        
            Area_Region_Change();
            IniFile.Write("OffSet", "nudRed_Cyan_Radius", nudRed_Cyan_Radius.Value.ToString(), Sys.SysPath);
        }

        private void nudYellow_Red_Radius_ValueChanged(object sender, EventArgs e)
        {             
            Pr.UpdateParameter("yellow_red_radius", (double)(nudYellow_Red_Radius.Value) * mm_2_pixel / 1000.0 / 2);
            if (bReadPara)
            return;
            IniFile.Write("OffSet", "nudYellow_Red_Radius", nudYellow_Red_Radius.Value.ToString(), Sys.SysPath);
            if (nudYellow_Red_Radius.Value >= nudRed_Cyan_Radius.Value)
            {
                nudRed_Cyan_Radius.Value = nudYellow_Red_Radius.Value + 50;
                return;
            }
            if (nudYellow_Red_Radius.Value <= nudYellow_Inner_Radius.Value)
            {
                nudYellow_Inner_Radius.Value = nudYellow_Red_Radius.Value - 50;
                return;
            }
            Area_Region_Change();
        }

        private void Gen_Circle_Center(HObject ho_Image, HTuple center_row, HTuple center_col, out HObject ho_UsedEdges, out HObject ho_Contour, out HObject ho_ResultContours, out HObject ho_CrossCenter, out HTuple hv_CenterRow, out HTuple hv_CenterColumn,out int iResult)
        {
            iResult = 0;
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            hv_CenterRow = 0;
            hv_CenterColumn = 0;
            HTuple hv_Radius = new HTuple(), hv_Length = new HTuple(), hv_MeasureThreshold = new HTuple(), hv_MeasureTransition = new HTuple();
            HTuple hv_MeasureSelect = new HTuple(),  hv_CenterRadius = new HTuple();
            
            hv_Radius = My.NIR.Radius_Center;
            hv_Length = My.NIR.Length_Center;
            hv_MeasureThreshold = My.NIR.MeasureThreshold_Center;
            hv_MeasureTransition = My.NIR.MeasureTransition_Center;
            hv_MeasureSelect = My.NIR.MeasureSelect_Center.D==0?"first":"last";
            try
            {
                //第一次先找圓心
                ho_UsedEdges.Dispose(); ho_Contour.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(My.ho_Image, out ho_UsedEdges, out ho_Contour, out ho_ResultContours,
                    out ho_CrossCenter, center_row, center_col, hv_Radius, hv_Length, hv_MeasureThreshold, hv_MeasureTransition,
                    hv_MeasureSelect, out hv_CenterRow, out hv_CenterColumn, out hv_CenterRadius);
                if (hv_CenterRow!=null)
                    iResult = 1;
            }
            catch
            {
                iResult = 0;
            }
        }

        private void Area_Region_Change()
        {
            if (My.NIR.hv_CenterRow == null)
            {
                MessageBox.Show("請先求圓心");
                return;
            }
            hWindowControl1.HalconWindow.ClearWindow();
            HTuple center_row = My.NIR.hv_CenterRow;
            HTuple center_col = My.NIR.hv_CenterColumn;
            HObject yellow_red_border, red_cyan_border, yellow_inner_border;
            //int iResult = Pr.Query_Sample_Center(My.Myho_Image, out center_row, out center_col);

            HOperatorSet.GenCircleContourXld(out yellow_red_border, center_row, center_col, (double)nudYellow_Red_Radius.Value * mm_2_pixel / 1000 / 2, 0, 3.14159 * 2, "positive", 1.0);
            HOperatorSet.GenCircleContourXld(out red_cyan_border, center_row, center_col, (double)nudRed_Cyan_Radius.Value * mm_2_pixel / 1000 / 2, 0, 3.14159 * 2, "positive", 1.0);
            HOperatorSet.GenCircleContourXld(out yellow_inner_border, center_row, center_col, (double)nudYellow_Inner_Radius.Value * mm_2_pixel / 1000 / 2, 0, 3.14159 * 2, "positive", 1.0);
            HOperatorSet.DispObj(My.ho_Image, hWindowControl1.HalconWindow);
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "cyan");
            HOperatorSet.DispObj(red_cyan_border, hWindowControl1.HalconWindow);
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "red");
            HOperatorSet.DispObj(yellow_red_border, hWindowControl1.HalconWindow);
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "yellow");
            HOperatorSet.DispObj(yellow_inner_border, hWindowControl1.HalconWindow);
            yellow_red_border.Dispose();
            red_cyan_border.Dispose();
            yellow_inner_border.Dispose();

        }

        private void Scar_Area_Region_Change(HTuple center_row,HTuple center_col)
        {
            hWindowControl1.HalconWindow.ClearWindow();
            
            HObject Scar_Outer_border, Scar_Inner_border;
            //int iResult = Pr.Query_Sample_Center(My.Myho_Image, out center_row, out center_col);
            //if (iResult == 0)
            //{
            if (center_row == null)
            {
                MessageBox.Show("請先找圓心");
                return;
            } HOperatorSet.GenCircleContourXld(out Scar_Outer_border, center_row, center_col, (double)nudScar_Outer_Radius.Value * mm_2_pixel / 1000 / 2, 0, 3.14159 * 2, "positive", 1.0);
                HOperatorSet.GenCircleContourXld(out Scar_Inner_border, center_row, center_col, (double)nudScar_Inner_Radius.Value * mm_2_pixel / 1000 / 2, 0, 3.14159 * 2, "positive", 1.0);
                  HOperatorSet.DispObj(My.ho_Image, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "cyan");
                HOperatorSet.DispObj(Scar_Outer_border, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "yellow");
                HOperatorSet.DispObj(Scar_Inner_border, hWindowControl1.HalconWindow);
                Scar_Outer_border.Dispose();
                Scar_Inner_border.Dispose();
            //}
        }

        private void nudRed_Area_Thresh_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudRed_Area_Thresh", nudRed_Area_Thresh.Value.ToString(), Sys.SysPath);
            AfterUpdate_Process_Display();
        }

        private void nudCyan_Area_Thresh_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudCyan_Area_Thresh", nudCyan_Area_Thresh.Value.ToString(), Sys.SysPath);
            AfterUpdate_Process_Display();
        }

        private void nudYellow_Area_Thresh_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudYellow_Area_Thresh", nudYellow_Area_Thresh.Value.ToString(), Sys.SysPath);           
            AfterUpdate_Process_Display();
        }

        private void AfterUpdate_Process_Display()
        {
            if (bReadPara)
                return;
            try
            {
                hWindowControl1.HalconWindow.ClearWindow();
                Result iResult;
                int iPass = -1;
                int iCode = 0;
                if (spot_checkbox.Checked)
                    iCode = Pr.Process(ho_Image, out ResultRegion, out LeakageRegion, out SpotRegion, out inside_scar_region, out iResult);
                else
                    iCode = Pr.Process(ho_Image, out ResultRegion, out LeakageRegion, out inside_scar_region, out iResult);

                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "yellow");
                HOperatorSet.DispObj(My.ho_Image, hWindowControl1.HalconWindow);
                HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
                HOperatorSet.DispObj(ResultRegion, hWindowControl1.HalconWindow);
                if (spot_checkbox.Checked)
                {
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(SpotRegion, hv_ExpDefaultWinHandle);
                    SpotRegion.Dispose();
                    HTuple AreaParticle = 0, RowParticle = 0, ColumnParticle = 0;
                    HOperatorSet.AreaCenter(SpotRegion, out AreaParticle, out RowParticle, out ColumnParticle);
                    SpotRegion.Dispose();
                    //Particle超過閥值
                    if (AreaParticle > (double)nudParticle_Area_Thresh.Value * mm_2_pixel * mm_2_pixel)
                        iCode = -5;
                }
                if (cbCheck_ExcessiveGlue.Checked)
                {
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.DispObj(LeakageRegion, hv_ExpDefaultWinHandle);
                }
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "white");
                HOperatorSet.DispObj(inside_scar_region, hv_ExpDefaultWinHandle);
                set_display_font(hWindowControl1.HalconWindow, 40, "mono", "true", "false");
                if (iCode < 0 && iCode != -5)
                {
                    string noneed = "";
                    System.String errorDescription = Pr.Query_Error_Code_Detail(iCode, out noneed);
                    HOperatorSet.SetColor(hWindowControl1.HalconWindow, "red");
                    HOperatorSet.SetTposition(hWindowControl1.HalconWindow, 2300, 100);
                    HOperatorSet.WriteString(hWindowControl1.HalconWindow, errorDescription);
                }
                else
                {
                    iPass = Glue_Pass_Test(iResult);
                    set_display_font(hv_ExpDefaultWinHandle, 20, "mono", "true", "false");
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "cyan");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_cyan_ratio[0]));
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_yellow_ratio[0]));
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_red_ratio[0]));
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 300, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_ratio[0]));

                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "cyan");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 300);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_cyan_ratio[1]));
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 300);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_yellow_ratio[1]));
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 300);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_red_ratio[1]));
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 300, 300);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_ratio[1]));

                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "cyan");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 600);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_cyan_ratio[2]));
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 600);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_yellow_ratio[2]));
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 600);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_red_ratio[2]));
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 300, 600);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_ratio[2]));

                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "cyan");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 900);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_cyan_ratio[3]));
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 900);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_yellow_ratio[3]));
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 900);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_red_ratio[3]));
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 300, 900);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, String.Format("{0}%", iResult.four_ratio[3]));

                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "All:" + String.Format("{0}%", iResult.all_ratio));
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 500, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "MinDistance:" + String.Format("{0}", (int)iResult.minDistance));

                    set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                    if (iPass == 0)
                    {
                        HOperatorSet.SetColor(hWindowControl1.HalconWindow, "blue");
                        HOperatorSet.SetTposition(hWindowControl1.HalconWindow, 2000, 100);
                        HOperatorSet.WriteString(hWindowControl1.HalconWindow, "Pass");
                    }
                    else
                    {
                        HOperatorSet.SetColor(hWindowControl1.HalconWindow, "red");
                        if ((iPass & 1) == 1)
                        {
                            HOperatorSet.SetTposition(hWindowControl1.HalconWindow, 2000, 100);
                            HOperatorSet.WriteString(hWindowControl1.HalconWindow, "胶水不够");
                        }
                        if ((iPass >> 1 & 1) == 1)
                        {
                            HOperatorSet.SetTposition(hWindowControl1.HalconWindow, 1800, 100);
                            HOperatorSet.WriteString(hWindowControl1.HalconWindow, "溢出");
                        }
                        if (iCode == -5)
                        {
                            HOperatorSet.SetTposition(hWindowControl1.HalconWindow, 2200, 100);
                            HOperatorSet.WriteString(hWindowControl1.HalconWindow, "溢出");
                        }

                    }
                }
            }
            catch
            {
            }
        }
        
        private void btnCreateTemplateAuto_Click(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.DispObj(My.ho_Image, hWindowControl1.HalconWindow);
            ho_Image = My.ho_Image;
            //disp_message(hWindowControl1.HalconWindow, "劃出IR片範圍", "window", 24, 24, "black", "true");
            string ncc_Filename = Sys.ModulePath + "\\" + Sys.Function + "_" + Production.CurProduction + "_model";           
            
            decimal threshold;
            if (InputBox("请设定阀值", "阀值:", out threshold) == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                int iCode = Pr.CreateTemplate_Auto(ho_Image, (int)(threshold), 50, ncc_Filename, out ho_Rectangle);
                Cursor.Current = Cursors.Arrow;
                if (iCode == 0)
                {
                    HOperatorSet.SetColor(hWindowControl1.HalconWindow, "red");
                    HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
                    HOperatorSet.DispObj(ho_Rectangle, hWindowControl1.HalconWindow);
                }
                else
                {
                    MessageBox.Show("建立模板失败,请改用手动，或调整阀值");
                }
            }
        }

        private void nudSpot_Thresh_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudSpot_Thresh", nudSpot_Thresh.Value.ToString(), Sys.SysPath);
            Pr.UpdateParameter("spot_threshold", (double)nudSpot_Thresh.Value);
            hWindowControl1.HalconWindow.ClearWindow();
            AfterUpdate_Process_Display();
        }

        private void tbLightSet_1_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            nudLightSet_1.Value = tbLightSet_1.Value;
        }

        private void nudLightSet_1_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
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
            if (bReadPara)
                return;
            nudLightSet_2.Value = tbLightSet_2.Value;
        }

        private void nudLightSet_2_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
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
            if (bReadPara)
                return;
            nudLightSet_3.Value = tbLightSet_3.Value;
        }

        private void nudLightSet_3_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
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
            if (bReadPara)
                return;
            nudLightSet_4.Value = tbLightSet_4.Value;
        }

        private void nudLightSet_4_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
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

        private void nudAll_Area_Thresh_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudAll_Area_Thresh", nudAll_Area_Thresh.Value.ToString(), Sys.SysPath);
            AfterUpdate_Process_Display();
        }

        private void nudCorner_Area_Thresh_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudCorner_Area_Thresh", nudCorner_Area_Thresh.Value.ToString(), Sys.SysPath);
            AfterUpdate_Process_Display();
        }

        private int Glue_Pass_Test(Result result)
        {
            int outcome = 0;
            if (result.leakage_flag == true)
                outcome |= 2;
            if (result.all_ratio < nudAll_Area_Thresh.Value)
                outcome |= 1;
            for (int i = 0; i < 4; i++)
            {
                if (result.four_cyan_ratio[i] < nudCyan_Area_Thresh.Value)
                    outcome |= 1;
                if (result.four_yellow_ratio[i] < nudYellow_Area_Thresh.Value)
                    outcome |= 1;
                if (result.four_red_ratio[i] < nudRed_Area_Thresh.Value)
                    outcome |= 1;              
                if (result.four_ratio[i] < nudCorner_Area_Thresh.Value)
                    outcome |= 1;
            }
            if (Test._Test)
            {
                result.TestTarget = false;
                int Count = 0;
                bool NG = false;
                for (int i = 0; i < 4; i++)
                {
                    switch (Test.Condition)
                    {
                        case 0:
                            {
                                if (result.four_cyan_ratio[i] < nudThresh_Upper.Value && result.four_cyan_ratio[i] >= nudThresh_Lower.Value && 
                                    result.four_yellow_ratio[i] >= nudYellow_Area_Thresh.Value &&
                                    result.four_red_ratio[i] >= nudRed_Area_Thresh.Value &&
                                    result.four_ratio[i] >= nudCorner_Area_Thresh.Value &&
                                    result.all_ratio >= nudAll_Area_Thresh.Value)
                                    Count++;
                                if (result.four_cyan_ratio[i] < nudThresh_Lower.Value)
                                    NG = true;
                                break;
                            }
                        case 1:
                            {
                                if (result.four_yellow_ratio[i] < nudThresh_Upper.Value && result.four_cyan_ratio[i] >= nudThresh_Lower.Value &&
                                    result.four_cyan_ratio[i] >= nudCyan_Area_Thresh.Value &&
                                    result.four_red_ratio[i] >= nudRed_Area_Thresh.Value &&
                                    result.four_ratio[i] >= nudCorner_Area_Thresh.Value &&
                                    result.all_ratio >= nudAll_Area_Thresh.Value)
                                    Count++;
                                if (result.four_yellow_ratio[i] < nudThresh_Lower.Value)
                                    NG = true;
                                break;
                            }
                        case 2:
                            {
                                if (result.four_red_ratio[i] < nudThresh_Upper.Value && result.four_cyan_ratio[i] >= nudThresh_Lower.Value &&
                                    result.four_cyan_ratio[i] >= nudCyan_Area_Thresh.Value &&
                                    result.four_yellow_ratio[i] >= nudYellow_Area_Thresh.Value &&
                                    result.four_ratio[i] >= nudCorner_Area_Thresh.Value &&
                                    result.all_ratio >= nudAll_Area_Thresh.Value)
                                    Count++;
                                if (result.four_red_ratio[i] < nudThresh_Lower.Value)
                                    NG = true;
                                break;
                            }
                        case 3:
                            {
                                if (result.four_ratio[i] < nudThresh_Upper.Value && result.four_cyan_ratio[i] >= nudThresh_Lower.Value &&
                                    result.four_cyan_ratio[i] >= nudCyan_Area_Thresh.Value &&
                                    result.four_yellow_ratio[i] >= nudYellow_Area_Thresh.Value &&
                                    result.four_red_ratio[i] >= nudRed_Area_Thresh.Value &&
                                    result.all_ratio >= nudAll_Area_Thresh.Value)
                                    Count++;
                                if (result.four_ratio[i] < nudThresh_Lower.Value)
                                    NG = true;
                                break;
                            }
                    }
                }
                if (Test.Corner_Lower <= Count && Count <= Test.Corner_Upper && !NG)
                {
                    result.TestTarget = true;
                }
                else
                {
                    result.TestTarget = false;
                }
            }
            return outcome;
        }

        private static DialogResult InputBox(string title, string promptText, out Decimal value)
        {
            Form form = new Form();
            Label label = new Label();
            NumericUpDown numeric_box = new NumericUpDown();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            numeric_box.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);
            numeric_box.Maximum = 255;
            numeric_box.Minimum = 20;
            numeric_box.Value = 50;

            label.AutoSize = true;
            numeric_box.Anchor = numeric_box.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, numeric_box, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = numeric_box.Value;
            return dialogResult;
        }

        private void nudYellow_Inner_Radius_ValueChanged(object sender, EventArgs e)
        {
            Pr.UpdateParameter("yellow_inner_radius", (double)(nudYellow_Inner_Radius.Value) * mm_2_pixel / 1000.0 / 2);
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudYellow_Inner_Radius", nudYellow_Inner_Radius.Value.ToString(), Sys.SysPath);
            
            if (nudYellow_Inner_Radius.Value >= nudYellow_Red_Radius.Value)
            {
                nudYellow_Red_Radius.Value = nudYellow_Inner_Radius.Value + 50;
                return;
            }
            Area_Region_Change();
        }

        private void nudLeakage_Numeric_ValueChanged(object sender, EventArgs e)
        {
            Pr.UpdateParameter("leakage_gray_offset", (double)nudLeakage_Numeric.Value);
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudLeakage_gray_offset", nudLeakage_Numeric.Value.ToString(), Sys.SysPath);
            
            AfterUpdate_Process_Display();
        }

        private void Check_ExcessiveGlue_CheckedChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            My.NIR.Check_ExcessiveGlue = (cbCheck_ExcessiveGlue.Checked ? true : false);

            IniFile.Write("ExcessiveGlue", "Check", My.NIR.Check_ExcessiveGlue.ToString(), Sys.SysPath);
        }

        private void spot_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            bool bspot_checkbox = (spot_checkbox.Checked ? true : false);
            IniFile.Write("OffSet", "spot_checkbox", bspot_checkbox.ToString(), Sys.SysPath);
        }

        private void nudParticle_Area_Thresh_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudParticle_Area_Thresh", nudParticle_Area_Thresh.Value.ToString(), Sys.SysPath);
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

        private void btnDrawBarcodeRange_Click(object sender, EventArgs e)
        {
            if (DrawBarcodeNow)
                return;
            DrawBarcodeNow = true;
            
            HOperatorSet.DrawRectangle1(hWindowControl1.HalconWindow, out BarcodeRange_Row1, out BarcodeRange_Column1, out BarcodeRange_Row2, out BarcodeRange_Column2);
            HOperatorSet.GenRectangle1(out BarcodeRange, BarcodeRange_Row1, BarcodeRange_Column1, BarcodeRange_Row2, BarcodeRange_Column2);
            HOperatorSet.DispObj(BarcodeRange, hWindowControl1.HalconWindow);
            My.NIR.BarcodeRange_Row1 = BarcodeRange_Row1;
            My.NIR.BarcodeRange_Column1 = BarcodeRange_Column1;
            My.NIR.BarcodeRange_Row2 = BarcodeRange_Row2;
            My.NIR.BarcodeRange_Column2 = BarcodeRange_Column2;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "BarcodeRange_Row1", My.NIR.BarcodeRange_Row1.ToString(), Path);
            IniFile.Write("Setting", "BarcodeRange_Column1", My.NIR.BarcodeRange_Column1.ToString(), Path);
            IniFile.Write("Setting", "BarcodeRange_Row2", My.NIR.BarcodeRange_Row2.ToString(), Path);
            IniFile.Write("Setting", "BarcodeRange_Column2", My.NIR.BarcodeRange_Column2.ToString(), Path);
            DrawBarcodeNow = false;
        }

     
        private void cbReadBarrelBarcode_CheckedChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            My.NIR.ReadBarrelBarcode = (cbReadBarrelBarcode.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "ReadBarrelBarcode", My.NIR.ReadBarrelBarcode.ToString(), Path);
        }
        public void Clear_Temple()
        {
            try
            {
                Pr.Clear_Template();
            }
            catch
            {
            }
        }
        public void Read_Temple()
        {
            Pr = new Processor();
            Pr.Load_Template(Sys.ModulePath + "\\" + Sys.Function + "_" + Production.CurProduction + "_model");
        }

        private void hWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            if(cbTransformOpen.Checked)
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

        private void cbTest_CheckedChanged(object sender, EventArgs e)
        {
            Test._Test = cbTest.Checked ? true : false;
            IniFile.Write("Test", "Test", Test._Test.ToString(), Sys.SysPath);

        }

        private void nudThresh_Upper_ValueChanged(object sender, EventArgs e)
        {
            Test.Thresh_Upper = nudThresh_Upper.Value;
        }

        private void nudThresh_Lower_ValueChanged(object sender, EventArgs e)
        {
            Test.Thresh_Lower = nudThresh_Lower.Value;
        }

        private void nudCorner_Upper_ValueChanged(object sender, EventArgs e)
        {
            Test.Corner_Upper = nudCorner_Upper.Value;
        }

        private void nudCorner_Lower_ValueChanged(object sender, EventArgs e)
        {
            Test.Corner_Lower = nudCorner_Lower.Value;
        }

        private void btnTestSave_Click(object sender, EventArgs e)
        {
            IniFile.Write("Test", "Thresh_Upper", Test.Thresh_Upper.ToString(), Sys.SysPath);
            IniFile.Write("Test", "Thresh_Lower", Test.Thresh_Lower.ToString(), Sys.SysPath);
            IniFile.Write("Test", "Corner_Upper", Test.Corner_Upper.ToString(), Sys.SysPath);
            IniFile.Write("Test", "Corner_Lower", Test.Corner_Lower.ToString(), Sys.SysPath);
        }

        private void cmbCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            Test.Condition = cmbCondition.SelectedIndex;
            Test.ConditionName = cmbCondition.SelectedItem.ToString();
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Test", "Condition", Test.Condition.ToString(), Path);
        }

        private void scar_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            bool bscar_checkbox = (scar_checkbox.Checked ? true : false);
            IniFile.Write("OffSet", "scar_checkbox", bscar_checkbox.ToString(), Sys.SysPath);
        }

        private void nudScar_Area_Thresh_ValueChanged(object sender, EventArgs e)
        {
            IniFile.Write("OffSet", "nudScar_Area_Thresh", nudScar_Area_Thresh.Value.ToString(), Sys.SysPath);

        }

        private void nudScar_Thresh_Offset_ValueChanged(object sender, EventArgs e)
        {
            Pr.UpdateParameter("scar_thresh_offset", (double)nudScar_Thresh_Offset.Value);
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudScar_Thresh_Offset", nudScar_Thresh_Offset.Value.ToString(), Sys.SysPath);
        }

        private void nudScar_Outer_Radius_ValueChanged(object sender, EventArgs e)
        {
            Pr.UpdateParameter("scar_outer_radius", (double)(nudScar_Outer_Radius.Value) * mm_2_pixel / 1000.0 / 2);
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudScar_Outer_Radius", nudScar_Outer_Radius.Value.ToString(), Sys.SysPath);

            Scar_Area_Region_Change(My.NIR.hv_CenterRow, My.NIR.hv_CenterColumn);
        }

        private void nudScar_Inner_Radius_ValueChanged(object sender, EventArgs e)
        {
            Pr.UpdateParameter("scar_inner_radius", (double)(nudScar_Inner_Radius.Value) * mm_2_pixel / 1000.0 / 2);
            if (bReadPara)
                return;
            IniFile.Write("OffSet", "nudScar_Inner_Radius", nudScar_Inner_Radius.Value.ToString(), Sys.SysPath);
            if (nudScar_Outer_Radius.Value <= nudScar_Inner_Radius.Value)
            {
                nudScar_Inner_Radius.Value = nudScar_Outer_Radius.Value - 10;
                return;
            }
            Scar_Area_Region_Change(My.NIR.hv_CenterRow, My.NIR.hv_CenterColumn);
        }

        private void cbTransformOpen_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void ReadBarcode(HObject ho_Image, out HObject ho_SymbolXLDs, out int iResult, out string sBarcode )
        {
            iResult = 0;
            Result result = new Result();
            ho_SymbolXLDs = new HObject();
            HTuple hv_DataCodeHandle = new HTuple(), hv_ResultHandles = new HTuple(), hv_DecodedDataStrings = new HTuple();
            HTuple hv_Number = new HTuple(), hv_DecodedData = new HTuple();
            string _sBarcode = "";
            sBarcode = "";
            HOperatorSet.CreateDataCode2dModel("Data Matrix ECC 200", "default_parameters", "maximum_recognition", out hv_DataCodeHandle);
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "polarity", "any");
            //鏡向
            //HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "mirrored", "no");
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "timeout", 2000);
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "contrast_tolerance", "high");
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "small_modules_robustness", "high");
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "symbol_rows_min", 14);
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "symbol_rows_max", 18);

            if (MyBarcodeReader.Production == 0)
                HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "decoding_scheme", "raw");
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "persistence", 0);
            ho_SymbolXLDs.Dispose();
            HOperatorSet.FindDataCode2d(ho_Image, out ho_SymbolXLDs, hv_DataCodeHandle, "stop_after_result_num", 1, out hv_ResultHandles, out hv_DecodedDataStrings);
            HOperatorSet.CountObj(ho_SymbolXLDs, out hv_Number);
            if (hv_Number == 0)
            {
                iResult = -2;
                HOperatorSet.ClearDataCode2dModel(hv_DataCodeHandle);
                return;
            }
            else
            {
                HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandles.TupleSelect(0), "decoded_data", out hv_DecodedData);
                if (MyBarcodeReader.Production == 0)
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
                    if (sBarcode.Substring(2, 2) == "40")//Q碼解出來第3.4要為40
                        iResult = 1;
                    else
                        iResult = -1;
                }
                else if (MyBarcodeReader.Production == 1)
                {
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    int Length = 0;
                    Length = hv_DecodedData.Length;
                    int[] Barcode = new int[Length];
                    for (int i = 0; i <= Length - 1; i++)
                    {
                        Barcode[i] = hv_DecodedData.TupleSelect(i);
                        _sBarcode = ((char)Barcode[i]).ToString();
                        sBarcode = sBarcode + _sBarcode;
                    }
                    iResult = 1;
                }
                else if (MyBarcodeReader.Production == 2)
                {
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    int Length = 0;
                    Length = hv_DecodedData.Length;
                    int[] Barcode = new int[Length];

                    for (int i = 0; i <= Length - 1; i++)
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
                    iResult = 1;
                }
                HOperatorSet.ClearDataCode2dModel(hv_DataCodeHandle);
                
            }
        }

        private void cmbBarcodePosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            My.NIR.BarcodePosition = cmbBarcodePosition.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "BarcodePosition", My.NIR.BarcodePosition.ToString(), Path);
        }

        #region 相机部分
        #region 相机参数读取
        public void CCDSetPara()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            CCD.Gain = double.Parse(IniFile.Read("Setting", "Gain", "0", Path));
            CCD.ExposureTime = double.Parse(IniFile.Read("Setting", "ExposureTime", "35000", Path));
            nudGain.Value = Convert.ToInt32(CCD.Gain);
            nudExposureTime.Value = Convert.ToInt32(CCD.ExposureTime); 
        }
        #endregion
        #region 相机增益调节
        private void tbGain_1_ValueChanged(object sender, EventArgs e)
        {
            nudGain.Value = tbGain.Value;

        }
        private void nudGain_1_ValueChanged(object sender, EventArgs e)
        {
            tbGain.Value = Convert.ToInt32(nudGain.Value);
            CCD.Gain = (double)nudGain.Value;
            parent.SetGain(tbGain.Value);
        }
        #endregion
        #region 相机曝光时间调节

        private void tbExposureTime_1_ValueChanged(object sender, EventArgs e)
        {
            nudExposureTime.Value = tbExposureTime.Value;
        }

        private void nudExposureTime_1_ValueChanged(object sender, EventArgs e)
        {
            tbExposureTime.Value = Convert.ToInt32(nudExposureTime.Value);

            parent.SetExposureTime(tbExposureTime.Value);


        }
        #endregion
        #region 相机参数保存
        private void btnCCDSetSave_1_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            CCD.Gain = (double)nudGain.Value;
            CCD.ExposureTime = (double)nudExposureTime.Value;
            IniFile.Write("Setting", "Gain", CCD.Gain.ToString(), Path);
            IniFile.Write("Setting", "ExposureTime", CCD.ExposureTime.ToString(), Path);
            
        }
        #endregion

        private void tbGain_ValueChanged(object sender, EventArgs e)
        {
            nudGain.Value = tbGain.Value;
        }

        #endregion

        private void nudGain_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            tbGain.Value = Convert.ToInt32(nudGain.Value);
            CCD.Gain = (double)nudGain.Value;
            parent.SetGain(tbGain.Value);
        }

        private void tbExposureTime_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            nudExposureTime.Value = tbExposureTime.Value;
        }

        private void nudExposureTime_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            tbExposureTime.Value = Convert.ToInt32(nudExposureTime.Value);

            parent.SetExposureTime(tbExposureTime.Value);
        }

        private void cbMeasureSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            My.NIR.MeasureSelect_Center = cmbMeasureSelect_Center.SelectedIndex;
        }

        private void ucRadius_ValueChanged(int CurrentValue)
        {
            My.NIR.Radius_Center = ucRadius_Center.Value;
            if (bReadPara)
                return;
            if (My.NIR.hv_ResultRow_CenterRectangle.D == 0)
            {
                MessageBox.Show("請先找矩形圓心");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour= new HObject(),ho_ResultContours= new HObject(),ho_CrossCenter= new HObject();
            HTuple hv_CenterRow = new HTuple(),hv_CenterColumn = new HTuple();
            int iResult = 0;
            Gen_Circle_Center(My.ho_Image, My.NIR.hv_ResultRow_CenterRectangle, My.NIR.hv_ResultColumn_CenterRectangle, out ho_UsedEdges, out  ho_Contour, out ho_ResultContours, out ho_CrossCenter, out hv_CenterRow, out hv_CenterColumn, out iResult);

            if (hv_CenterRow != null)
            {
                Window.ClearWindow();
                My.ho_Image.DispObj(Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                ho_Contour.DispObj(Window);
                Window.SetColor("blue");
                ho_ResultContours.DispObj(Window);
                Window.SetColor("red");
                ho_UsedEdges.DispObj(Window);
            }
        }

        private void ucLength_ValueChanged(int CurrentValue)
        {
            My.NIR.Length_Center = ucLength_Center.Value;
            if (bReadPara)
                return;
            if (My.NIR.hv_ResultRow_CenterRectangle.D == 0)
            {
                MessageBox.Show("請先找矩形圓心");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject(), ho_CrossCenter = new HObject();
            HTuple hv_CenterRow = new HTuple(), hv_CenterColumn = new HTuple();
            int iResult = 0;
            Gen_Circle_Center(My.ho_Image, My.NIR.hv_ResultRow_CenterRectangle, My.NIR.hv_ResultColumn_CenterRectangle, out ho_UsedEdges, out  ho_Contour, out ho_ResultContours, out ho_CrossCenter, out hv_CenterRow, out hv_CenterColumn, out iResult);

            if (hv_CenterRow != null)
            {
                Window.ClearWindow();
                My.ho_Image.DispObj(Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                ho_Contour.DispObj(Window);
                Window.SetColor("blue");
                ho_ResultContours.DispObj(Window);
                Window.SetColor("red");
                ho_UsedEdges.DispObj(Window);
            }
        }

        private void ucBlack2White_ValueChanged(int CurrentValue)
        {
            My.NIR.MeasureThreshold_Center = ucBlack2White_Center.Value;
            My.NIR.MeasureTransition_Center = "positive";
            if (bReadPara)
                return;
            if (My.NIR.hv_ResultRow_CenterRectangle.D == 0)
            {
                MessageBox.Show("請先找矩形圓心");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject(), ho_CrossCenter = new HObject();
            HTuple hv_CenterRow = new HTuple(), hv_CenterColumn = new HTuple();
            int iResult = 0;
            Gen_Circle_Center(My.ho_Image, My.NIR.hv_ResultRow_CenterRectangle, My.NIR.hv_ResultColumn_CenterRectangle, out ho_UsedEdges, out  ho_Contour, out ho_ResultContours, out ho_CrossCenter, out hv_CenterRow, out hv_CenterColumn, out iResult);

            if (hv_CenterRow != null)
            {
                Window.ClearWindow();
                My.ho_Image.DispObj(Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                ho_Contour.DispObj(Window);
                Window.SetColor("blue");
                ho_ResultContours.DispObj(Window);
                Window.SetColor("red");
                ho_UsedEdges.DispObj(Window);
            }
        }

        private void ucWhite2Black_ValueChanged(int CurrentValue)
        {
            My.NIR.MeasureThreshold_Center = ucWhite2Black_Center.Value;
            My.NIR.MeasureTransition_Center = "negative";
            if (bReadPara)
                return;
            if (My.NIR.hv_ResultRow_CenterRectangle.D == 0)
            {
                MessageBox.Show("請先找矩形圓心");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject(), ho_CrossCenter = new HObject();
            HTuple hv_CenterRow = new HTuple(), hv_CenterColumn = new HTuple();
            int iResult = 0;
            Gen_Circle_Center(My.ho_Image, My.NIR.hv_ResultRow_CenterRectangle, My.NIR.hv_ResultColumn_CenterRectangle, out ho_UsedEdges, out  ho_Contour, out ho_ResultContours, out ho_CrossCenter, out hv_CenterRow, out hv_CenterColumn, out iResult);

            if (hv_CenterRow != null)
            {
                Window.ClearWindow();
                My.ho_Image.DispObj(Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                ho_Contour.DispObj(Window);
                Window.SetColor("blue");
                ho_ResultContours.DispObj(Window);
                Window.SetColor("red");
                ho_UsedEdges.DispObj(Window);
            }
        }

        private void btnFindCenter_Click(object sender, EventArgs e)
        {
            if (My.NIR.hv_ResultRow_CenterRectangle.D == 0)
            {
                MessageBox.Show("請先找矩形圓心");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject(), ho_CrossCenter = new HObject();
            HTuple hv_CenterRow = new HTuple(), hv_CenterColumn = new HTuple();
            int iResult = 0;
            Gen_Circle_Center(My.ho_Image, My.NIR.hv_ResultRow_CenterRectangle, My.NIR.hv_ResultColumn_CenterRectangle, out ho_UsedEdges, out  ho_Contour, out ho_ResultContours, out ho_CrossCenter, out hv_CenterRow, out hv_CenterColumn, out iResult);
            Window.ClearWindow();
            My.ho_Image.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_ResultContours.DispObj(Window);
            Window.SetColor("red");
            ho_CrossCenter.DispObj(Window);
            if (hv_CenterRow != null)
            {
                My.NIR.hv_CenterRow = hv_CenterRow;
                My.NIR.hv_CenterColumn = hv_CenterColumn;
            }
        }

        private void ucOuterRadius_ExcessiveGlue_ValueChanged(int CurrentValue)
        {
            My.NIR.OuterRadius_ExcessiveGlue = ucOuterRadius_ExcessiveGlue.Value;
            if (bReadPara)
                return;
            if (My.NIR.hv_CenterRow==null)
            {
                MessageBox.Show("請先求圓心");
                return;
            }
            HObject ho_OuterCircle = new HObject(), ho_InnerCircle = new HObject();
            HWindow Window = hWindowControl1.HalconWindow;
            ho_OuterCircle.Dispose();
            HOperatorSet.GenCircle(out ho_OuterCircle, My.NIR.hv_CenterRow, My.NIR.hv_CenterColumn, My.NIR.OuterRadius_ExcessiveGlue * mm_2_pixel / 1000 / 2);
            ho_InnerCircle.Dispose();
            HOperatorSet.GenCircle(out ho_InnerCircle, My.NIR.hv_CenterRow, My.NIR.hv_CenterColumn, My.NIR.InnerRadius_ExcessiveGlue * mm_2_pixel / 1000 / 2);
            Window.ClearWindow();
            My.ho_Image.DispObj(Window);
            HOperatorSet.SetDraw(Window, "margin");
            HOperatorSet.SetColor(Window, "blue");
            HOperatorSet.DispObj(ho_OuterCircle, hv_ExpDefaultWinHandle);
            HOperatorSet.SetColor(Window, "orange");
            HOperatorSet.DispObj(ho_InnerCircle, hv_ExpDefaultWinHandle);
        }

        private void ucInnerRadius_ExcessiveGlue_ValueChanged(int CurrentValue)
        {
            My.NIR.InnerRadius_ExcessiveGlue = ucInnerRadius_ExcessiveGlue.Value;
            if (bReadPara)
                return;
            if (My.NIR.hv_CenterRow == null)
            {
                MessageBox.Show("請先求圓心");
                return;
            }
            HObject ho_OuterCircle = new HObject(), ho_InnerCircle = new HObject();
            HWindow Window = hWindowControl1.HalconWindow;
            ho_OuterCircle.Dispose();
            HOperatorSet.GenCircle(out ho_OuterCircle, My.NIR.hv_CenterRow, My.NIR.hv_CenterColumn, My.NIR.OuterRadius_ExcessiveGlue* mm_2_pixel / 1000 / 2);
            ho_InnerCircle.Dispose();
            HOperatorSet.GenCircle(out ho_InnerCircle, My.NIR.hv_CenterRow, My.NIR.hv_CenterColumn, My.NIR.InnerRadius_ExcessiveGlue * mm_2_pixel / 1000 / 2);
            Window.ClearWindow();
            My.ho_Image.DispObj(Window);
            HOperatorSet.SetDraw(Window, "margin");
            HOperatorSet.SetColor(Window, "blue");
            HOperatorSet.DispObj(ho_OuterCircle, Window);
            HOperatorSet.SetColor(Window, "orange");
            HOperatorSet.DispObj(ho_InnerCircle, Window);
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            IniFile.Write("Center", "MeasureSelect", My.NIR.MeasureSelect_Center.ToString(), Sys.SysPath);
            IniFile.Write("Center", "Radius", My.NIR.Radius_Center.ToString(), Sys.SysPath);
            IniFile.Write("Center", "Length", My.NIR.Length_Center.ToString(), Sys.SysPath);
            IniFile.Write("Center", "MeasureThreshold", My.NIR.MeasureThreshold_Center.ToString(), Sys.SysPath);
            IniFile.Write("Center", "MeasureTransition", My.NIR.MeasureTransition_Center.ToString(), Sys.SysPath);
        }

        private void nudDetection_ExcessiveGlueMode_ValueChanged(object sender, EventArgs e)
        {   
            tabMode_ExcessiveGlue.SelectedIndex = (int)nudMode_ExcessiveGlue.Value - 1;
            if (bReadPara)
                return;
            My.NIR.Mode_ExcessiveGlue = (int)nudMode_ExcessiveGlue.Value;
            IniFile.Write("ExcessiveGlue", "Mode", My.NIR.Mode_ExcessiveGlue.ToString(), Sys.SysPath);
        }

        private void cmbDarkLightChoice_Scar_SelectedIndexChanged(object sender, EventArgs e)
        {
            My.NIR.DarkLightChoice_Scar = cmbDarkLightChoice_Scar.SelectedIndex;
            IniFile.Write("Scar", "DarkLightChoice", My.NIR.DarkLightChoice_Scar.ToString(), Sys.SysPath);
        }

        private void btnSave_ExcessiveGlue_Click(object sender, EventArgs e)
        {
            IniFile.Write("ExcessiveGlue", "Mode", My.NIR.Mode_ExcessiveGlue.ToString(), Sys.SysPath);
            IniFile.Write("ExcessiveGlue", "InnerRadius", My.NIR.InnerRadius_ExcessiveGlue.ToString(), Sys.SysPath);
            IniFile.Write("ExcessiveGlue", "OuterRadius", My.NIR.OuterRadius_ExcessiveGlue.ToString(), Sys.SysPath);
            IniFile.Write("ExcessiveGlue", "DarkLightChoice", My.NIR.DarkLightChoice_ExcessiveGlue.ToString(), Sys.SysPath);
            IniFile.Write("ExcessiveGlue", "OffSet", My.NIR.OffSet_ExcessiveGlue.ToString(), Sys.SysPath);
        }

        private void cmbDarkLightChoice_ExcessiveGlue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            My.NIR.DarkLightChoice_ExcessiveGlue = cmbDarkLightChoice_ExcessiveGlue.SelectedIndex;
        }

        private void nudOffset_ExcessiveGlue_ValueChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            My.NIR.OffSet_ExcessiveGlue = (int)nudOffset_ExcessiveGlue.Value;
        }

        private void tabMode_ExcessiveGlue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bReadPara)
                return;
            nudMode_ExcessiveGlue.Value = tabMode_ExcessiveGlue.SelectedIndex + 1;
        }

        private void nudexpected_holder_radius_ValueChanged(object sender, EventArgs e)
        {
            Pr.UpdateParameter("expected_holder_radius", (double)(nudexpected_holder_radius.Value) * mm_2_pixel / 1000.0 / 2);
            My.NIR.expected_holder_radius = (double)nudexpected_holder_radius.Value;
            if (bReadPara)
                return;
            if (My.NIR.hv_CenterRow.D == 0)
            {
                MessageBox.Show("請先求圓心");
                return;
            }
            My.NIR.expected_holder_radius = (double)nudexpected_holder_radius.Value;

            HObject ho_Circle = new HObject();
            HWindow Window = hWindowControl1.HalconWindow;
            ho_Circle.Dispose();
            HOperatorSet.GenCircle(out ho_Circle, My.NIR.hv_CenterRow, My.NIR.hv_CenterColumn, My.NIR.expected_holder_radius * mm_2_pixel / 1000 / 2);
            Window.ClearWindow();
            My.ho_Image.DispObj(Window);
            HOperatorSet.SetDraw(Window, "margin");
            HOperatorSet.SetColor(Window, "orange");
            HOperatorSet.DispObj(ho_Circle, Window);
            IniFile.Write("OffSet", "expected_holder_radius", nudexpected_holder_radius.Value.ToString(), Sys.SysPath);
        }

        private void cmbMeasureSelect_RectangleCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            My.NIR.MeasureSelect_RectangleCenter = cmbMeasureSelect_RectangleCenter.SelectedIndex;
        }

        private void ucLength_RectangleCenter_ValueChanged(int CurrentValue)
        {
            if (bReadPara)
                return;
            My.NIR.Length_RectangleCenter = ucLength_RectangleCenter.Value;
            
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject(), ho_CrossCenter = new HObject();
            HTuple hv_CenterRow = new HTuple(), hv_CenterColumn = new HTuple(),hv_Phi = new HTuple(),hv_Length1 = new HTuple(),hv_Length2 = new HTuple();
            Gen_Rectangle_Center(My.ho_Image, out ho_UsedEdges, out  ho_Contour, out ho_ResultContours, 
                out hv_CenterRow, out hv_CenterColumn, out hv_Phi,out hv_Length1,out hv_Length2);

            if (hv_CenterRow != null)
            {
                Window.ClearWindow();
                My.ho_Image.DispObj(Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                ho_Contour.DispObj(Window);
                Window.SetColor("blue");
                ho_ResultContours.DispObj(Window);
                Window.SetColor("red");
                ho_UsedEdges.DispObj(Window);
            }
        }

        private void ucBlack2White_RectangleCenter_ValueChanged(int CurrentValue)
        {
            if (bReadPara)
                return;
            My.NIR.MeasureTransition_RectangleCenter = "positive";
            My.NIR.MeasureThreshold_RectangleCenter = ucBlack2White_RectangleCenter.Value;

            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject(), ho_CrossCenter = new HObject();
            HTuple hv_CenterRow = new HTuple(), hv_CenterColumn = new HTuple(), hv_Phi = new HTuple(), hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            Gen_Rectangle_Center(My.ho_Image, out ho_UsedEdges, out  ho_Contour, out ho_ResultContours,
                out hv_CenterRow, out hv_CenterColumn, out hv_Phi, out hv_Length1, out hv_Length2);

            if (hv_CenterRow != null)
            {
                Window.ClearWindow();
                My.ho_Image.DispObj(Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                ho_Contour.DispObj(Window);
                Window.SetColor("blue");
                ho_ResultContours.DispObj(Window);
                Window.SetColor("red");
                ho_UsedEdges.DispObj(Window);
            }
        }

        private void ucWhite2Black_RectangleCenter_ValueChanged(int CurrentValue)
        {
            if (bReadPara)
                return;
            My.NIR.MeasureTransition_RectangleCenter = "negative";
            My.NIR.MeasureThreshold_RectangleCenter = ucBlack2White_RectangleCenter.Value;

            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject(), ho_CrossCenter = new HObject();
            HTuple hv_CenterRow = new HTuple(), hv_CenterColumn = new HTuple(), hv_Phi = new HTuple(), hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            Gen_Rectangle_Center(My.ho_Image, out ho_UsedEdges, out  ho_Contour, out ho_ResultContours,
                out hv_CenterRow, out hv_CenterColumn, out hv_Phi, out hv_Length1, out hv_Length2);

            if (hv_CenterRow != null)
            {
                Window.ClearWindow();
                My.ho_Image.DispObj(Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                ho_Contour.DispObj(Window);
                Window.SetColor("blue");
                ho_ResultContours.DispObj(Window);
                Window.SetColor("red");
                ho_UsedEdges.DispObj(Window);
            }
        }

        private void btnFindCenter_RectangleCenter_Click(object sender, EventArgs e)
        {
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject(), ho_CrossCenter = new HObject();
            HTuple hv_CenterRow = new HTuple(), hv_CenterColumn = new HTuple(), hv_Phi = new HTuple(), hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            try
            {
                Gen_Rectangle_Center(My.ho_Image, out ho_UsedEdges, out  ho_Contour, out ho_ResultContours,
                    out hv_CenterRow, out hv_CenterColumn, out hv_Phi, out hv_Length1, out hv_Length2);

                if (hv_CenterRow != null)
                {
                    Window.ClearWindow();
                    My.ho_Image.DispObj(Window);
                    Window.SetDraw("margin");
                    Window.SetColor("green");
                    ho_ResultContours.DispObj(Window);

                    My.NIR.hv_ResultRow_CenterRectangle = hv_CenterRow;
                    My.NIR.hv_ResultColumn_CenterRectangle = hv_CenterColumn;
                }


            }
            catch
            {
            }
        }

        private void btnSave_RectangleCenter_Click(object sender, EventArgs e)
        {
            IniFile.Write("RectangleCenter", "MeasureSelect", My.NIR.MeasureSelect_RectangleCenter.ToString(), Sys.SysPath);
            IniFile.Write("RectangleCenter", "Length", My.NIR.Length_RectangleCenter.ToString(), Sys.SysPath);
            IniFile.Write("RectangleCenter", "MeasureThreshold", My.NIR.MeasureThreshold_RectangleCenter.ToString(), Sys.SysPath);
            IniFile.Write("RectangleCenter", "MeasureTransition", My.NIR.MeasureTransition_RectangleCenter.ToString(), Sys.SysPath);
        }

        private void Gen_Rectangle_Center(HObject ho_Image, out HObject ho_UsedEdges, 
            out HObject ho_Contour, out HObject ho_ResultContours , 
            out HTuple hv_CenterRow, out HTuple hv_CenterColumn,out HTuple hv_CenterPhi,out HTuple hv_CenterLength1,out HTuple hv_CenterLength2)
        {
            HTuple hv_FirstResultRow, hv_FirstResultColumn;
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            hv_CenterRow = 0;
            hv_CenterColumn = 0;
            hv_CenterPhi = 0;
            hv_CenterLength1 = 0;
            hv_CenterLength2 = 0;

            int iResult = Pr.Query_Sample_Center(ho_Image, out hv_FirstResultRow, out hv_FirstResultColumn);

            HTuple hv_Length1 = My.NIR.Length1_RectangleCenter;
            HTuple hv_Length2 = My.NIR.Length2_RectangleCenter;
            HTuple hv_Length = My.NIR.Length_RectangleCenter;
            HTuple hv_Phi = My.NIR.Phi_RectangleCenter;
            HTuple hv_MeasureThreshold = My.NIR.MeasureThreshold_RectangleCenter;
            HTuple hv_MeasureTransition = My.NIR.MeasureTransition_RectangleCenter;
            HTuple hv_MeasureSelect = My.NIR.MeasureSelect_RectangleCenter.D == 0 ? "first" : "last";
            try
            {
                //第一次先找圓心
                ho_UsedEdges.Dispose(); ho_Contour.Dispose(); ho_ResultContours.Dispose();
                gen_rectangle2_center(ho_Image, out ho_Contour, out ho_UsedEdges,
                   out ho_ResultContours, hv_FirstResultRow, hv_FirstResultColumn, hv_Phi, hv_Length1, hv_Length2,
                   hv_Length, hv_MeasureThreshold, hv_MeasureTransition, hv_MeasureSelect, out hv_CenterRow, out hv_CenterColumn, out hv_CenterPhi,
                   out hv_CenterLength1, out hv_CenterLength2);
            }
            catch
            {
                iResult = 0;
            }

        }

        private void nudOuterRadius_Paricle_ValueChanged(object sender, EventArgs e)
        {
            if (My.NIR.hv_CenterRow.D == 0)
            {
                //MessageBox.Show("請先求圓心");
                return;
            }
            hWindowControl1.HalconWindow.ClearWindow();
            HTuple center_row = My.NIR.hv_CenterRow;
            HTuple center_col = My.NIR.hv_CenterColumn;
            HObject ho_OuterCircle = new HObject(), ho_InnerCircle = new HObject();
            //int iResult = Pr.Query_Sample_Center(My.Myho_Image, out center_row, out center_col);
            ho_OuterCircle.Dispose();
            HOperatorSet.GenCircleContourXld(out ho_OuterCircle, center_row, center_col, (double)nudOuterRadius_Paricle.Value * mm_2_pixel / 1000 / 2, 0, 3.14159 * 2, "positive", 1.0);
            ho_InnerCircle.Dispose();
            HOperatorSet.GenCircleContourXld(out ho_InnerCircle, center_row, center_col, (double)nudInnerRadius_Paricle.Value * mm_2_pixel / 1000 / 2, 0, 3.14159 * 2, "positive", 1.0);
            HOperatorSet.DispObj(My.ho_Image, hWindowControl1.HalconWindow);
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "cyan");
            HOperatorSet.DispObj(ho_OuterCircle, hWindowControl1.HalconWindow);
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "red");   
            HOperatorSet.DispObj(ho_InnerCircle, hWindowControl1.HalconWindow);
            ho_OuterCircle.Dispose();
            ho_InnerCircle.Dispose();
            My.NIR.OuterRadius_Paricle = (double)nudOuterRadius_Paricle.Value;
            IniFile.Write("Paricle", "OuterRadius", My.NIR.OuterRadius_Paricle.ToString(), Sys.SysPath);
        }

        private void nudInnerRadius_Paricle_ValueChanged(object sender, EventArgs e)
        {
            if (My.NIR.hv_CenterRow.D == 0)
            {
                //MessageBox.Show("請先求圓心");
                return;
            }
            hWindowControl1.HalconWindow.ClearWindow();
            HTuple center_row = My.NIR.hv_CenterRow;
            HTuple center_col = My.NIR.hv_CenterColumn;
            HObject ho_OuterCircle = new HObject(), ho_InnerCircle = new HObject();
            //int iResult = Pr.Query_Sample_Center(My.Myho_Image, out center_row, out center_col);
            ho_OuterCircle.Dispose();
            HOperatorSet.GenCircleContourXld(out ho_OuterCircle, center_row, center_col, (double)nudOuterRadius_Paricle.Value * mm_2_pixel / 1000 / 2, 0, 3.14159 * 2, "positive", 1.0);
            ho_InnerCircle.Dispose();
            HOperatorSet.GenCircleContourXld(out ho_InnerCircle, center_row, center_col, (double)nudInnerRadius_Paricle.Value * mm_2_pixel / 1000 / 2, 0, 3.14159 * 2, "positive", 1.0);
            HOperatorSet.DispObj(My.ho_Image, hWindowControl1.HalconWindow);
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "cyan");
            HOperatorSet.DispObj(ho_OuterCircle, hWindowControl1.HalconWindow);
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "red");
            HOperatorSet.DispObj(ho_InnerCircle, hWindowControl1.HalconWindow);
            ho_OuterCircle.Dispose();
            ho_InnerCircle.Dispose();
            My.NIR.InnerRadius_Paricle = (double)nudInnerRadius_Paricle.Value;
            IniFile.Write("Paricle", "InnerRadius", My.NIR.InnerRadius_Paricle.ToString(), Sys.SysPath);
        }

      

    }
}
