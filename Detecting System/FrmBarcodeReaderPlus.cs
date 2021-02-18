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
    public partial class FrmBarcodeReaderPlus : Form
    {
        FrmParent parent;
        FrmRun Run;
        public List<Button> TrayBtn = new List<Button>();
        //如果正在畫圓,則無法再次點擊
        public bool bDrawing = false;
        public bool bReadPara = false;
        public FrmBarcodeReaderPlus(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;
            InitializeComponent();
            Run = new FrmRun(parent);
        }
        //public class BarcodeResult
        //{
        //    public string Barcode = "NA";
        //    public double MeanLight = 0;
        //    public double BarcodeAngle = 0;//Barcode角度(依照產品旋轉角度)
        //    public HTuple hv_DecodedOrientation = 0;//Barcode角度(初始代出角度)
        //    public HTuple hv_DecodedMirrored = "no";//"yes"/"no" 是否鏡像
        //    public object[] Overall_Quality;
        //    public object[] Cell_Contrast;
        //    public object[] Print_Growth;
        //    public object[] Unused_Error_Correction;
        //    public object[] Cell_Modulation;
        //    public object[] Fixed_Pattern_Damage;
        //    public object[] Grid_Nonuniformity;
        //    public object[] Decode;
        //    public BarcodeResult()
        //    {
        //        Overall_Quality = new object[2] { "F", 0};
        //        Cell_Contrast = new object[2] { "F", 0};
        //        Print_Growth = new object[2] { "F", 0};
        //        Unused_Error_Correction = new object[2] { "F", 0};
        //        Cell_Modulation = new object[2] { "F", 0};
        //        Fixed_Pattern_Damage = new object[2] { "F", 0};
        //        Grid_Nonuniformity = new object[2] { "F", 0};
        //        Decode = new object[2] { "F", 0};
        //    }

        //}
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
            ImageProPlus(hWindowControl1.HalconWindow, My.ho_Image, Tray.n, Protocol.BarcodeReaderPlus_NowRow, Protocol.BarcodeReaderPlus_NowColumn);
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
            try
            {
                bReadPara = true;
                cmbProduction.SelectedIndex = MyBarcodeReader.Production;
                nudTrayRows.Value = MyBarcodeReaderPlus.TrayRows;
                nudTrayColumns.Value = MyBarcodeReaderPlus.TrayColumns;
                btnCreateTray.PerformClick();
                int n = MyBarcodeReaderPlus.TrayRows * MyBarcodeReaderPlus.TrayColumns;
                for (int i = 0; i < n; i++)
                {
                    if (MyBarcodeReaderPlus.TrayPointBanned[i])
                    {
                        TrayBtn[i].BackColor = Color.Gray;
                    }
                    else
                    {
                        TrayBtn[i].BackColor = Color.Blue;
                    }
                }
                nudRowCutNum.Value = MyBarcodeReaderPlus.RowCutNum;
                nudColumnCutNum.Value = MyBarcodeReaderPlus.ColumnCutNum;
                ucFirstRadius.Value = MyBarcodeReaderPlus.FirstRadius;
                //cbDarkLightChoice.SelectedIndex = MyBarcodeReaderPlus.DarkLightChoice;
                //ucGray.Value = MyBarcodeReaderPlus.Gray;
                cmbModelMode.SelectedIndex = MyBarcodeReaderPlus.ModelMode;
                nudModelGrade.Value = (int)MyBarcodeReaderPlus.ModelGrade;
                cbMeasureSelect.SelectedIndex = MyBarcodeReaderPlus.MeasureSelect == "first" ? 0 : 1;
                ucRadius.Value = MyBarcodeReaderPlus.Radius.I;
                ucLength.Value = MyBarcodeReaderPlus.Length.I;
                if (MyBarcodeReaderPlus.MeasureTransition == "positive")
                    ucBlack2White.Value = MyBarcodeReaderPlus.MeasureThreshold;
                else
                    ucWhite2Black.Value = MyBarcodeReaderPlus.MeasureThreshold;

                cbMeasureSelect2.SelectedIndex = MyBarcodeReaderPlus.MeasureSelect2 == "first" ? 0 : 1;
                ucLength2.Value = MyBarcodeReaderPlus.Length2.I;
                if (MyBarcodeReaderPlus.MeasureTransition2 == "positive")
                    ucBlack2White2.Value = MyBarcodeReaderPlus.MeasureThreshold2;
                else
                    ucWhite2Black2.Value = MyBarcodeReaderPlus.MeasureThreshold2;

                ucOuterRadius.Value = MyBarcodeReaderPlus.OuterRadius;
                ucInnerRadius.Value = MyBarcodeReaderPlus.InnerRadius;
                ucStartAngle.Value = MyBarcodeReaderPlus.StartAngle;
                ucEndAngle.Value = MyBarcodeReaderPlus.EndAngle;
                cmbRegionDarkLight.SelectedIndex = MyBarcodeReaderPlus.RegionDarkLight;
                ucRegionThreshold.Value = MyBarcodeReaderPlus.RegionThreshold;
                ucRegionRect2_Len1_Upper.Value = MyBarcodeReaderPlus.RegionRect2_Len1_Upper;
                ucRegionRect2_Len1_Lower.Value = MyBarcodeReaderPlus.RegionRect2_Len1_Lower;
                ucRegionRect2_Len2_Upper.Value = MyBarcodeReaderPlus.RegionRect2_Len2_Upper;
                ucRegionRect2_Len2_Lower.Value = MyBarcodeReaderPlus.RegionRect2_Len2_Lower;
                ucRegionErosion.Value = MyBarcodeReaderPlus.RegionErosion;
                MyBarcodeReaderPlus.ho_ImagePart = new HObject();
                cmbRegionProjectSet.SelectedIndex = MyBarcodeReaderPlus.RegionProjectSet;
                ucRegionDistance.Value = MyBarcodeReaderPlus.RegionDistance;
                ucRegionRotation.Value = MyBarcodeReaderPlus.RegionRotation;
                ucRegionLength1.Value = MyBarcodeReaderPlus.RegionLength1;
                ucRegionLength2.Value = MyBarcodeReaderPlus.RegionLength2;

                cmbMirrored.SelectedIndex = MyBarcodeReaderPlus.Mirrored == true ? 0 : 1;
                nudBarcodeAngleSet.Value = MyBarcodeReaderPlus.BarcodeAngleSet;
                nudAllowableOffsetAngle_L.Value = (decimal)MyBarcodeReaderPlus.AllowableOffsetAngle_L;
                nudAllowableOffsetAngle.Value = (decimal)MyBarcodeReaderPlus.AllowableOffsetAngle;
                
                cmbOverall_Quality.SelectedIndex = MyBarcodeReaderPlus.Overall_Quality;
                cmbCell_Contrast.SelectedIndex = MyBarcodeReaderPlus.Cell_Contrast;
                cmbPrint_Growth.SelectedIndex = MyBarcodeReaderPlus.Print_Growth;
                cmbUnused_Error_Correction.SelectedIndex = MyBarcodeReaderPlus.Unused_Error_Correction;
                cmbCell_Modulation.SelectedIndex = MyBarcodeReaderPlus.Cell_Modulation;
                cmbFixed_Pattern_Damage.SelectedIndex = MyBarcodeReaderPlus.Fixed_Pattern_Damage;
                cmbGrid_Nonuniformity.SelectedIndex = MyBarcodeReaderPlus.Grid_Nonuniformity;
                cmbDecode.SelectedIndex = MyBarcodeReaderPlus.Decode;

                nudMode_Angle.Value = MyBarcodeReaderPlus.Mode_Angle;
                cmbMeasureSelect_Angle1.SelectedIndex = MyBarcodeReaderPlus.MeasureSelect_Angle1;
                ucLength_Angle1.Value = (int)MyBarcodeReaderPlus.Length_Angle1.D;
                if (MyBarcodeReaderPlus.MeasureTransition_Angle1 == "positive")
                    ucBlack2White_Angle1.Value = (int)MyBarcodeReaderPlus.MeasureThreshold_Angle1.D;
                else
                    ucWhite2Black2_Angle1.Value = (int)MyBarcodeReaderPlus.MeasureThreshold_Angle1.D;
                bReadPara = false;

            }
            catch
            {
            }
        }

        private void btnRangeSet_Click(object sender, EventArgs e)
        {

        }

        public void ImageProPlus(HWindow _Window, HObject theImage, int n, int TrayRow_Now, int TrayColumn_Now)
        {
            if (theImage == null)
                return;
            HWindowControl _HWindow = new HWindowControl();
            _HWindow.Width = 1000;
            _HWindow.Height = 1000;
            HWindow Window = new HWindow();
            HObject ho_Image = new HObject(), ho_ImageReduced = new HObject(), ho_ImagePart = new HObject();
            HObject ho_Rectangle = new HObject(), ho_ResultImage = new HObject();
            HTuple Width = new HTuple(), Height = new HTuple(), hv_BarcodeAngle = new HTuple();
            Vision.BarcodeResult m_BarcodeResult = new Vision.BarcodeResult();
            string sBarcode = "";
            int iResult = 0, m = 0;
            try
            {
                HOperatorSet.CopyImage(theImage, out ho_Image);

                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle, MyBarcodeReaderPlus.RegionRow1, MyBarcodeReaderPlus.RegionColumn1, MyBarcodeReaderPlus.RegionRow2, MyBarcodeReaderPlus.RegionColumn2);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
                ho_ImagePart.Dispose();
                HOperatorSet.CropDomain(ho_ImageReduced, out ho_Image);

                Window = _HWindow.HalconWindow;
                Window.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                for (int y = 1; y <= MyBarcodeReaderPlus.RowCutNum; y++)
                {
                    for (int x = 1; x <= MyBarcodeReaderPlus.ColumnCutNum; x++)
                    {
                        sBarcode = "";
                        iResult = 0;
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle1(out ho_Rectangle, (y - 1) * hv_Height.D / MyBarcodeReaderPlus.RowCutNum, (x - 1) * hv_Width.D / MyBarcodeReaderPlus.ColumnCutNum, y * hv_Height.D / MyBarcodeReaderPlus.RowCutNum, x * hv_Width.D / MyBarcodeReaderPlus.ColumnCutNum);
                        ho_ImageReduced.Dispose();
                        HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
                        ho_ImagePart.Dispose();
                        HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);
                        HOperatorSet.GetImageSize(ho_ImagePart, out Width, out Height);
                        HOperatorSet.SetPart(Window, 0, 0, Height - 1, Width - 1);
                        findReadBarcodeRegion(Window, ho_ImagePart, out iResult, out ho_ResultImage, out m_BarcodeResult);

                        ho_Rectangle.Dispose();
                        ho_ImageReduced.Dispose();
                        int iRow = 0;
                        int iColumn = 0;
                        iRow = MyBarcodeReaderPlus.RowCutNum * (TrayRow_Now - 1) + y;
                        iColumn = MyBarcodeReaderPlus.ColumnCutNum * (TrayColumn_Now - 1) + x;
                        if (iColumn <= Tray.Columns_1 && iRow <= Tray.Rows_1)
                        {
                            m = Tray.Columns_1 * (iRow - 1) + iColumn - 1;
                        }
                        else
                        {
                            m = -1;
                        }

                        //switch (n)
                        //{
                        //    case 1:
                        //        {
                        //            m = Tray.Columns_1 * (y - 1) + x - 1;
                        //            iRow = y;
                        //            iColumn = x;
                        //            break;
                        //        }
                        //    case 2:
                        //        {
                        //            if (x + MyBarcodeReaderPlus.ColumnCutNum <= Tray.Columns_1)
                        //            {
                        //                m = Tray.Columns_1 * (y - 1) + x + MyBarcodeReaderPlus.ColumnCutNum - 1;
                        //                iRow = y;
                        //                iColumn = x + MyBarcodeReaderPlus.ColumnCutNum;
                        //            }
                        //            else
                        //            {
                        //                m = -1;
                        //            }
                        //            break;
                        //        }
                        //    case 3:
                        //        {
                        //            if (x + MyBarcodeReaderPlus.ColumnCutNum <= Tray.Columns_1)
                        //            {
                        //                m = Tray.Columns_1 * (y - 1 + MyBarcodeReaderPlus.RowCutNum) + x + MyBarcodeReaderPlus.ColumnCutNum - 1;
                        //                iRow = y + MyBarcodeReaderPlus.RowCutNum;
                        //                iColumn = x + MyBarcodeReaderPlus.ColumnCutNum;
                        //            }
                        //            else
                        //            {
                        //                m = -1;
                        //            }
                        //            break;
                        //        }
                        //    case 4:
                        //        {
                        //            m = Tray.Columns_1 * (y - 1 + MyBarcodeReaderPlus.RowCutNum) + x - 1;
                        //            iRow = y + MyBarcodeReaderPlus.RowCutNum;
                        //            iColumn = x;
                        //            break;
                        //        }
                        //}
                        if (m != -1)
                        {
                            Vision.Images_Now[m] = ho_ResultImage.CopyObj(1, -1);
                            Vision.VisionBarcodeResult[m] = m_BarcodeResult;
                            if (!MyBarcodeReaderPlus.TrayPointBanned[m])
                            {
                                switch (iResult)
                                {
                                    case 0:
                                        {
                                            Count.iMiss = Count.iMiss + 1;
                                            Vision.VisionResult[m] = "Miss";
                                            parent.LabelShow(m, iRow + "-" + iColumn, Color.Blue, Color.White);
                                            break;
                                        }
                                    case 1:
                                        {
                                            Count.iOK = Count.iOK + 1;
                                            Vision.VisionResult[m] = "OK";
                                            parent.LabelShow(m, iRow + "-" + iColumn, Color.Green, Color.White);
                                            break;
                                        }
                                    case -1:
                                        {
                                            Count.iNG = Count.iNG + 1;
                                            Vision.VisionResult[m] = "NG";
                                            parent.LabelShow(m, iRow + "-" + iColumn, Color.Red, Color.White);
                                            break;
                                        }
                                    case -2:
                                        {
                                            Count.iNG2 = Count.iNG2 + 1;
                                            Vision.VisionResult[m] = "NG2";
                                            parent.LabelShow(m, iRow + "-" + iColumn, Color.Orange, Color.White);
                                            break;
                                        }
                                    case -3:
                                        {
                                            Count.iNG3 = Count.iNG3 + 1;
                                            Vision.VisionResult[m] = "NG3";
                                            parent.LabelShow(m, iRow + "-" + iColumn, Color.Purple, Color.White);

                                            break;
                                        }
                                    case -4:
                                        {
                                            Count.iNG4 = Count.iNG4 + 1;
                                            Vision.VisionResult[m] = "NG4";
                                            parent.LabelShow(m, iRow + "-" + iColumn, Color.White, Color.Black);
                                            break;
                                        }
                                    case -5:
                                        {
                                            Count.iNG5 = Count.iNG5 + 1;
                                            Vision.VisionResult[m] = "NG5";
                                            parent.LabelShow(m, iRow + "-" + iColumn, Color.White, Color.Black);
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                Vision.VisionResult[m] = "Miss";
                                parent.LabelShow(m, iRow + "-" + iColumn, Color.Gray, Color.White);
                            }
                            //if (Sys.OptionNG && Vision.VisionResult[m] != "OK" && Vision.VisionResult[m] != "NG")
                            //{
                            //    string TrayBarcode = "NA";
                            //    //結果圖/原圖臨時存放區
                            //    string Resultpath = Sys.ImageSavePath0 + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Result";
                            //    //結果圖上傳存放區
                            //    string UpLoadpath = Sys.ImageSavePath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Result";

                            //    //檔名
                            //    string Namepath = TrayBarcode + "_(" + string.Format("{0}.{1}", iRow, iColumn) + ")_" + sBarcode;
                            //    //時間
                            //    string Time = DateTime.Now.ToString("yyyyMMdd_HH_mm_ss_f");
                            //    //建立結果圖片資料夾
                            //    if (!Directory.Exists(Resultpath))
                            //    {
                            //        Directory.CreateDirectory(Resultpath);
                            //    }
                            //    //建立上傳圖片資料夾
                            //    if (!Directory.Exists(UpLoadpath))
                            //    {
                            //        Directory.CreateDirectory(UpLoadpath);
                            //    }

                            //    HOperatorSet.WriteImage(Vision.Images_1[n], "png", 0, Resultpath + "\\" + Namepath + "_OK_" + Time);
                            //    bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_OK_" + Time + ".png", UpLoadpath + "\\" + Namepath + "_OK_" + Time + ".png", 360, 270, 100);

                            //    ho_ImagePart.Dispose();
                            //    ho_ResultImage.Dispose();
                            //}
                        }
                    }
                }
            }
            catch
            {
            }
            ho_Image.Dispose();
            _HWindow.HalconWindow.ClearWindow();
            _HWindow.HalconWindow.CloseWindow();

        }
        public void WriteLog(int n, string ResultOK, Vision.BarcodeResult result)
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
                        "Decode_Level\tDecode_Value" +
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

        private void btnCreateTray_Click(object sender, EventArgs e)
        {
            int Rows = (int)nudTrayRows.Value;
            int Cols = (int)nudTrayColumns.Value;
            if (Rows == 0 || Cols == 0)
            {
                MessageBox.Show("行/列不可為0!");
                return;
            }

            int height = (pnlTray.Height) / Rows;
            int width = (pnlTray.Width) / Cols;

            for (int i = 0; i < TrayBtn.Count; ++i)
            {
                TrayBtn[i].Dispose();
            }
            TrayBtn.Clear();

            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Cols; ++j)
                {
                    Button btn = new Button();
                    if (Plc.TrayMode_1 == 0 || Plc.TrayMode_1 == 1)
                    {

                        btn.AutoSize = false;
                        btn.BackColor = Color.Gray;
                        btn.Size = new System.Drawing.Size(width - 1, height - 1);
                        btn.Location = new Point(width * j, height * i);
                        btn.ForeColor = Color.White;
                        btn.TextAlign = ContentAlignment.MiddleCenter;
                        btn.Font = new Font("宋体", 6, FontStyle.Bold);
                        btn.Click += ButtonCheck;
                        pnlTray.Controls.Add(btn);
                        TrayBtn.Add(btn);
                    }
                    else if (Plc.TrayMode_1 == 2)
                    {
                        if (i % 2 == 0)
                        {
                            btn.AutoSize = false;
                            btn.BackColor = Color.Gray;
                            btn.Size = new System.Drawing.Size(width - 1, height - 1);
                            btn.Location = new Point(width * j, height * i);
                            btn.ForeColor = Color.White;
                            btn.TextAlign = ContentAlignment.MiddleCenter;
                            btn.Font = new Font("宋体", 6, FontStyle.Bold);
                            btn.Click += ButtonCheck;
                            pnlTray.Controls.Add(btn);
                            TrayBtn.Add(btn);
                        }
                        else
                        {
                            btn.AutoSize = false;
                            btn.BackColor = Color.Gray;
                            btn.Size = new System.Drawing.Size(width - 1, height - 1);
                            btn.Location = new Point(width * j + width / 2, height * i);
                            btn.ForeColor = Color.White;
                            btn.TextAlign = ContentAlignment.MiddleCenter;
                            btn.Font = new Font("宋体", 6, FontStyle.Bold);
                            if (j == Cols - 1)
                                btn.Visible = false;
                            btn.Click += ButtonCheck;
                            pnlTray.Controls.Add(btn);
                            TrayBtn.Add(btn);
                        }
                    }
                    else if (Plc.TrayMode_1 == 3)
                    {
                        if (i % 2 == 1)
                        {
                            btn.AutoSize = false;
                            btn.BackColor = Color.Gray;
                            btn.Size = new System.Drawing.Size(width - 1, height - 1);
                            btn.Location = new Point(width * j + width / 2, height * i);
                            btn.ForeColor = Color.White;
                            btn.TextAlign = ContentAlignment.MiddleCenter;
                            btn.Font = new Font("宋体", 6, FontStyle.Bold);
                            if (j == Cols - 1)
                                btn.Visible = false;
                            btn.Click += ButtonCheck;
                            pnlTray.Controls.Add(btn);
                            TrayBtn.Add(btn);
                        }
                        else
                        {
                            btn.AutoSize = false;
                            btn.BackColor = Color.Gray;
                            btn.Size = new System.Drawing.Size(width - 1, height - 1);
                            btn.Location = new Point(width * j, height * i);
                            btn.ForeColor = Color.White;
                            btn.TextAlign = ContentAlignment.MiddleCenter;
                            btn.Font = new Font("宋体", 6, FontStyle.Bold);
                            btn.Click += ButtonCheck;
                            pnlTray.Controls.Add(btn);
                            TrayBtn.Add(btn);
                        }
                    }
                }
            }
        }
        public void ButtonCheck(object obj, EventArgs e)
        {
            Button btn = obj as Button;
            if (btn.BackColor == Color.Gray)
            {
                btn.BackColor = Color.Blue;
            }
            else if (btn.BackColor == Color.Blue)
            {
                btn.BackColor = Color.Gray;
            }
        }

        private void btnAllPointBanned_Click(object sender, EventArgs e)
        {
            int n = (int)nudTrayRows.Value * (int)nudTrayColumns.Value;
            for (int i = 0; i < n; i++)
            {
                TrayBtn[i].BackColor = Color.Gray;
            }
        }

        private void btnCancelAllPointBanned_Click(object sender, EventArgs e)
        {
            int n = (int)nudTrayRows.Value * (int)nudTrayColumns.Value;
            for (int i = 0; i < n; i++)
            {
                TrayBtn[i].BackColor = Color.Blue;
            }
        }

        private void btnTraySave_Click(object sender, EventArgs e)
        {
            if (nudTrayRows.Value == 0 || nudTrayColumns.Value == 0)
            {
                MessageBox.Show("行/列不可為0");
                return;
            }
            MyBarcodeReaderPlus.TrayRows = (int)nudTrayRows.Value;
            MyBarcodeReaderPlus.TrayColumns = (int)nudTrayColumns.Value;
            int n = MyBarcodeReaderPlus.TrayRows * MyBarcodeReaderPlus.TrayColumns;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            MyBarcodeReaderPlus.TrayPointBanned = new bool[n];
            IniFile.Write("Setting", "TrayRows", MyBarcodeReaderPlus.TrayRows.ToString(), Path);
            IniFile.Write("Setting", "TrayColumns", MyBarcodeReaderPlus.TrayColumns.ToString(), Path);
            for (int i = 0; i < n; i++)
            {
                if (TrayBtn[i].BackColor == Color.Gray)
                {
                    MyBarcodeReaderPlus.TrayPointBanned[i] = true;
                    IniFile.Write("TrayPointBanned", i.ToString(), "true", Path);
                }
                else
                {
                    MyBarcodeReaderPlus.TrayPointBanned[i] = false;
                    IniFile.Write("TrayPointBanned", i.ToString(), "false", Path);
                }
            }
        }

        private void nudRowCutNum_ValueChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.RowCutNum = (int)nudRowCutNum.Value;
            DrawCross(hWindowControl1.HalconWindow, 5120, 5120, MyBarcodeReaderPlus.RowCutNum, MyBarcodeReaderPlus.ColumnCutNum);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "RowCutNum", MyBarcodeReaderPlus.RowCutNum.ToString(), Path);
        }


        private void nudColumnCutNum_ValueChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.ColumnCutNum = (int)nudColumnCutNum.Value;
            DrawCross(hWindowControl1.HalconWindow, 5120, 5120, MyBarcodeReaderPlus.RowCutNum, MyBarcodeReaderPlus.ColumnCutNum);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "ColumnCutNum", MyBarcodeReaderPlus.ColumnCutNum.ToString(), Path);
        }

        public void DrawCross(HWindow Window, HTuple hv_Width, HTuple hv_Height, HTuple RowNum, HTuple ColumnNum)
        {
            //如果=0不處理
            if (RowNum == 1 && ColumnNum == 1)
                return;
            if (My.ho_Image == null)
                return;
            HObject ho_Line = new HObject(), ho_Lines = new HObject(), ho_ImagePart = new HObject();
            HOperatorSet.GenEmptyObj(out ho_Lines);


            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, MyBarcodeReaderPlus.RegionRow1, MyBarcodeReaderPlus.RegionColumn1, MyBarcodeReaderPlus.RegionRow2, MyBarcodeReaderPlus.RegionColumn2);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(My.ho_Image, ho_Rectangle, out ho_ImageReduced);
            ho_ImagePart.Dispose();
            HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);
            HOperatorSet.GetImageSize(ho_ImagePart, out hv_Width, out  hv_Height);
            if (RowNum > 1)
            {
                HTuple Height2 = hv_Height / RowNum;

                for (int y = 1; y <= RowNum - 1; y++)
                {
                    ho_Line.Dispose();
                    HOperatorSet.GenRegionLine(out ho_Line, Height2 * y, 0, Height2 * y, hv_Width);
                    HOperatorSet.ConcatObj(ho_Lines, ho_Line, out ho_Lines);
                }
            }
            if (ColumnNum > 1)
            {
                HTuple Width2 = hv_Width / ColumnNum;
                for (int x = 1; x <= ColumnNum - 1; x++)
                {
                    ho_Line.Dispose();
                    HOperatorSet.GenRegionLine(out ho_Line, 0, Width2 * x, hv_Height, Width2 * x);
                    HOperatorSet.ConcatObj(ho_Lines, ho_Line, out ho_Lines);
                }
            }
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.MoveRegion(ho_Lines, out ExpTmpOutVar_0, MyBarcodeReaderPlus.RegionRow1, MyBarcodeReaderPlus.RegionColumn1);
                ho_Lines.Dispose();
                ho_Lines = ExpTmpOutVar_0;
            }
            Window.ClearWindow();
            HOperatorSet.GetImageSize(My.ho_Image, out hv_Width, out  hv_Height);
            HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
            My.ho_Image.DispObj(Window);
            Window.SetDraw("margin");
            HOperatorSet.SetColor(Window, "green");
            ho_Rectangle.DispObj(Window);
            HOperatorSet.SetColor(Window, "red");
            ho_Lines.DispObj(Window);

        }

        private void btnTargetShow_Click(object sender, EventArgs e)
        {
            if (My.ho_Image == null)
                return;
            HObject ho_Rectangle = new HObject(), ho_ImageReduced = new HObject(), ho_ImagePart = new HObject();
            HWindow Window = hWindowControl1.HalconWindow;
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();

            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, MyBarcodeReaderPlus.RegionRow1, MyBarcodeReaderPlus.RegionColumn1, MyBarcodeReaderPlus.RegionRow2, MyBarcodeReaderPlus.RegionColumn2);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(My.ho_Image, ho_Rectangle, out ho_ImageReduced);
            ho_ImagePart.Dispose();
            HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);

            HOperatorSet.GetImageSize(ho_ImagePart, out hv_Width, out hv_Height);
            try
            {
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle, ((double)nudTargetRow.Value - 1) * hv_Height.D / MyBarcodeReaderPlus.RowCutNum, ((double)nudTargetColumn.Value - 1) * hv_Width.D / MyBarcodeReaderPlus.ColumnCutNum, (double)nudTargetRow.Value * hv_Height.D / MyBarcodeReaderPlus.RowCutNum, (double)nudTargetColumn.Value * hv_Width.D / MyBarcodeReaderPlus.ColumnCutNum);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_ImagePart, ho_Rectangle, out ho_ImageReduced);
                ho_ImagePart.Dispose();
                HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);
                HOperatorSet.GetImageSize(ho_ImagePart, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
                ho_ImagePart.DispObj(Window);

                MyBarcodeReaderPlus.ho_ImagePart = ho_ImagePart.CopyObj(1, -1);
            }
            catch
            {
            }
            ho_Rectangle.Dispose();
            ho_ImagePart.Dispose();
            ho_Rectangle.Dispose();
            ho_ImageReduced.Dispose();
        }

        private void ucFirstRadius_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.FirstRadius = ucFirstRadius.Value;
            if (bReadPara)
                return;
            try
            {
                HObject ho_Image = new HObject(), ho_Circle = new HObject(), ho_ReducedImage = new HObject();
                HTuple hv_Height = new HTuple(), hv_Width = new HTuple();
                HWindow Window = hWindowControl1.HalconWindow;
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(MyBarcodeReaderPlus.ho_ImagePart, out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);

                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, MyBarcodeReaderPlus.FirstRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                Window.ClearWindow();
                HOperatorSet.DispObj(ho_ReducedImage, Window);
            }
            catch
            {
            }
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "FirstRadius", MyBarcodeReaderPlus.FirstRadius.ToString(), Path);
        }

        //public void Hysteresis(HObject ho_Image, HTuple GrayUpper, HTuple GrayLower)
        //{
        //    HObject ho_Circle = new HObject(),ho_ReducedImage = new HObject();
        //    HObject ho_ImageMedian = new HObject(), ho_RegionHysteresis = new HObject();
        //    HWindow Window = hWindowControl1.HalconWindow;
        //    HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
        //    //預處理 中值濾波減少干擾
        //    try
        //    {
        //        HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
        //        HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
        //        ho_Circle.Dispose();
        //        HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, MyBarcodeReaderPlus.FirstRadius);
        //        ho_ReducedImage.Dispose();
        //        HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
        //        ho_ImageMedian.Dispose();
        //        HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 20, 20);
        //        ho_RegionHysteresis.Dispose();
        //        HOperatorSet.HysteresisThreshold(ho_ImageMedian, out ho_RegionHysteresis, GrayLower, GrayUpper, 5);
        //        Window.SetDraw("fill");
        //        Window.SetColor("yellow");
        //        Window.ClearWindow();
        //        ho_Image.DispObj(Window);
        //        ho_RegionHysteresis.DispObj(Window);
        //    }
        //    catch
        //    {
        //    }
        //    ho_Circle.Dispose();
        //    ho_ReducedImage.Dispose();
        //    ho_ImageMedian.Dispose();
        //    ho_RegionHysteresis.Dispose();
        //}


        private void btnDrawRange_Click(object sender, EventArgs e)
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
            HObject ho_Rectangle = new HObject();
            HTuple Row = new HTuple(), Column = new HTuple(), Phi = new HTuple(), Length1 = new HTuple(), Length2 = new HTuple();

            try
            {
                Window = hWindowControl1.HalconWindow;
                if (MyBarcodeReaderPlus.ho_ImagePart == null)
                    return;
                Window.ClearWindow();
                MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
                HOperatorSet.SetColor(Window, "yellow");
                //畫檢視範圍
                HOperatorSet.DrawRectangle2(Window, out Row, out Column, out Phi, out Length1, out Length2);

                HOperatorSet.GenRectangle2(out ho_Rectangle, Row, Column, Phi, Length1, Length2);

                HOperatorSet.SetDraw(Window, "margin");
                MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
                ho_Rectangle.DispObj(Window);

                if (Length1 > Length2)
                {
                    MyBarcodeReaderPlus.Length21 = Length1;
                    MyBarcodeReaderPlus.Length22 = Length2;
                    MyBarcodeReaderPlus.Phi2 = Phi;
                }
                else
                {
                    MyBarcodeReaderPlus.Length21 = Length2;
                    MyBarcodeReaderPlus.Length22 = Length1;
                    MyBarcodeReaderPlus.Phi2 = Phi - (new HTuple(90)).TupleRad();
                }
            }
            catch
            {
            }
            bDrawing = false;
        }

        private void cbMeasureSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.MeasureSelect = cbMeasureSelect.SelectedIndex == 0 ? "first" : "last";
        }

        private void ucRadius_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.Radius = ucRadius.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            int Result = 0;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject();
            try
            {
                FindCenter(Window, MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out ho_UsedEdges2, out ho_Contour2, out ho_ResultContours2, out MyBarcodeReaderPlus.hv_ResultRow, out MyBarcodeReaderPlus.hv_ResultColumn, out MyBarcodeReaderPlus.hv_ResultPhi);
                Window.ClearWindow();
                MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                ho_Contour.DispObj(Window);
                Window.SetColor("blue");
                ho_ResultContours.DispObj(Window);
                Window.SetColor("red");
                ho_UsedEdges.DispObj(Window);
            }
            catch
            {
            }
        }

        private void ucLength_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.Length = ucLength.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject();
            int Result = 0;
            try
            {
                FindCenter(Window, MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out ho_UsedEdges2, out ho_Contour2, out ho_ResultContours2, out MyBarcodeReaderPlus.hv_ResultRow, out MyBarcodeReaderPlus.hv_ResultColumn, out MyBarcodeReaderPlus.hv_ResultPhi);
            }
            catch
            {
            }
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_Contour.DispObj(Window);
            Window.SetColor("blue");
            ho_ResultContours.DispObj(Window);
            Window.SetColor("red");
            ho_UsedEdges.DispObj(Window);
        }

        private void ucBlack2White_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.MeasureTransition = "positive";
            MyBarcodeReaderPlus.MeasureThreshold = ucBlack2White.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            int Result = 0;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject();
            FindCenter(Window, MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out ho_UsedEdges2, out ho_Contour2, out ho_ResultContours2, out MyBarcodeReaderPlus.hv_ResultRow, out MyBarcodeReaderPlus.hv_ResultColumn, out MyBarcodeReaderPlus.hv_ResultPhi);
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_Contour.DispObj(Window);
            Window.SetColor("blue");
            ho_ResultContours.DispObj(Window);
            Window.SetColor("red");
            ho_UsedEdges.DispObj(Window);
        }

        private void ucWhite2Black_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.MeasureTransition = "negative";
            MyBarcodeReaderPlus.MeasureThreshold = ucWhite2Black.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            int Result = 0;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject();
            FindCenter(Window, MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out ho_UsedEdges2, out ho_Contour2, out ho_ResultContours2, out MyBarcodeReaderPlus.hv_ResultRow, out MyBarcodeReaderPlus.hv_ResultColumn, out MyBarcodeReaderPlus.hv_ResultPhi);
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_Contour.DispObj(Window);
            Window.SetColor("blue");
            ho_ResultContours.DispObj(Window);
            Window.SetColor("red");
            ho_UsedEdges.DispObj(Window);
        }

        private void btnFindCenter_Click(object sender, EventArgs e)
        {
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject();
            int Result = 0;
            FindCenter(Window, MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out ho_UsedEdges2, out ho_Contour2, out ho_ResultContours2, out MyBarcodeReaderPlus.hv_ResultRow, out MyBarcodeReaderPlus.hv_ResultColumn, out MyBarcodeReaderPlus.hv_ResultPhi);
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_ResultContours.DispObj(Window);
        }
        //求準確圓心
        public void FindCenter(HWindow Window, HObject TheImage, out int iResult, out HObject ho_UsedEdges, out HObject ho_Contour, out HObject ho_ResultContours, out HObject ho_UsedEdges2, out HObject ho_Contour2, out HObject ho_ResultContours2, out HTuple hv_ResultRow, out HTuple hv_ResultColumn, out HTuple hv_ResultPhi2)
        {
            HObject ho_Image = new HObject(), ho_CrossCenter = new HObject();
            HObject ho_Circle = new HObject(), ho_ReducedImage = new HObject();
            HObject ho_ImageMedian = new HObject(), ho_ImageEmphasize = new HObject(), ho_Region = new HObject();
            HObject ho_ConnectedRegions = new HObject(), ho_RegionFillUp = new HObject();
            HObject ho_SelectedRegions = new HObject(), ho_ReducedImage2 = new HObject();
            HObject ho_RegionDilation = new HObject();

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple(), hv_Number = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple(), hv_ResultRadius = new HTuple();
            HTuple hv_Radius = new HTuple(), hv_Length = new HTuple(), hv_MeasureThreshold = new HTuple(), hv_MeasureTransition = new HTuple(), hv_MeasureSelect = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_Phi2 = new HTuple(), hv_Length21 = new HTuple(), hv_Length22 = new HTuple(), hv_MeasureThreshold2 = new HTuple(), hv_MeasureTransition2 = new HTuple(), hv_MeasureSelect2 = new HTuple();
            HTuple hv_hv_Area = new HTuple();
            HTuple hv_ResultRow2 = new HTuple(), hv_ResultColumn2 = new HTuple(), hv_ResultLength21 = new HTuple(), hv_ResultLength22 = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges2);
            HOperatorSet.GenEmptyObj(out ho_Contour2);
            HOperatorSet.GenEmptyObj(out ho_ResultContours2);

            iResult = 0;
            hv_ResultRow = 0;
            hv_ResultColumn = 0;
            hv_ResultPhi2 = 0;
            if (TheImage == null)
                return;
            //if (MyBarcodeReaderPlus.hv_FirstRow.D == 0 || MyBarcodeReaderPlus.hv_FirstColumn.D == 0)
            //    return;
            hv_Radius = MyBarcodeReaderPlus.Radius;
            hv_Length = MyBarcodeReaderPlus.Length;
            hv_MeasureThreshold = MyBarcodeReaderPlus.MeasureThreshold;
            hv_MeasureTransition = MyBarcodeReaderPlus.MeasureTransition;
            hv_MeasureSelect = MyBarcodeReaderPlus.MeasureSelect;
            hv_Phi2 = MyBarcodeReaderPlus.Phi2;
            hv_Length2 = MyBarcodeReaderPlus.Length2;
            hv_Length21 = MyBarcodeReaderPlus.Length21;
            hv_Length22 = MyBarcodeReaderPlus.Length22;
            hv_MeasureThreshold2 = MyBarcodeReaderPlus.MeasureThreshold2;
            hv_MeasureTransition2 = MyBarcodeReaderPlus.MeasureTransition2;
            hv_MeasureSelect2 = MyBarcodeReaderPlus.MeasureSelect2;
            try
            {
                ho_Image.Dispose();
                ho_Image = TheImage.CopyObj(1, -1);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, MyBarcodeReaderPlus.FirstRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 10, 10);
                ho_ImageEmphasize.Dispose();
                HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);
                //判斷是否無料
                try
                {
                    HObject ho_ModelContours = new HObject(), ho_TransContours = new HObject();
                    HObject ho_RegionUnion = new HObject();
                    HTuple hv_ModelID = new HTuple(), hv_HomMat2D = new HTuple();
                    HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
                    HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
                    HTuple hv_CircleRow = new HTuple(), hv_CircleColumn = new HTuple(), hv_CircleRadius = new HTuple();
                    try
                    {
                        string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model_Miss";
                        HOperatorSet.ReadShapeModel(Path, out hv_ModelID);
                    }
                    catch
                    {
                        MessageBox.Show("請建立無料模組");
                        return;
                    }
                    HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                    HOperatorSet.FindShapeModel(ho_ImageEmphasize, hv_ModelID, (new HTuple(-30)).TupleRad(), (new HTuple(30)).TupleRad(), 0.5, 0, 0.5, "least_squares", (new HTuple(7)).TupleConcat(1), 0.75, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
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
                        HOperatorSet.SmallestCircle(ho_RegionUnion, out hv_CircleRow, out hv_CircleColumn, out hv_CircleRadius);
                        HOperatorSet.GenCircle(out ho_Circle, hv_CircleRow, hv_CircleColumn, hv_CircleRadius);
                        HOperatorSet.ClearShapeModel(hv_ModelID);
                        iResult = 0;
                        ho_ModelContours.Dispose();
                        ho_TransContours.Dispose();
                        ho_RegionUnion.Dispose();
                        return;
                    }
                    else
                    {
                        HOperatorSet.ClearShapeModel(hv_ModelID);
                    }
                }
                catch
                {
                    MessageBox.Show("請建立初始模組");
                    return;
                }

                try
                {
                    HObject ho_ModelContours = new HObject(), ho_TransContours = new HObject();
                    HObject ho_RegionUnion = new HObject();
                    HObject ho_ModelRegion = new HObject();
                    HTuple hv_ModelID = new HTuple(), hv_HomMat2D = new HTuple();
                    HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
                    HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
                    HTuple hv_RectangleRow = new HTuple(), hv_RectangleColumn = new HTuple(), hv_RectanglePhi = new HTuple(), hv_RectangleLength1 = new HTuple(), hv_RectangleLength2 = new HTuple();
                    HTuple hv_ModelRegionArea = new HTuple(), hv_RefRow = new HTuple(), hv_RefColumn = new HTuple();
                    try
                    {
                        //string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model";
                        //HOperatorSet.ReadShapeModel(Path, out hv_ModelID);
                        if (MyBarcodeReaderPlus.hv_ModelID.Length == 0)
                        {
                            MessageBox.Show("請建立初定位模組");
                            return;
                        } 
                        hv_ModelID = MyBarcodeReaderPlus.hv_ModelID;
                    }
                    catch
                    {

                    }
                    switch (MyBarcodeReaderPlus.ModelMode)
                    {
                        case 0:
                            {
                                HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                                HOperatorSet.FindShapeModel(ho_ImageEmphasize, hv_ModelID, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), MyBarcodeReaderPlus.ModelGrade / 100, 1, 0.5, "least_squares", (new HTuple(6)).TupleConcat(1), 0.5, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
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
                                    HOperatorSet.SmallestRectangle2(ho_RegionUnion, out hv_Row, out hv_Column, out hv_RectanglePhi, out hv_RectangleLength1, out hv_RectangleLength2);
                                    HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
                                    Window.ClearWindow();
                                    ho_Image.DispObj(Window);
                                    Window.SetDraw("margin");
                                    Window.SetColor("yellow");
                                    ho_Rectangle.DispObj(Window);
                                    disp_message(Window, "模組分數:" + Math.Round(hv_Score.D * 100, 0), "", 0, 0, "green", "false");
                                    MyBarcodeReaderPlus.hv_FirstRow = hv_Row;
                                    MyBarcodeReaderPlus.hv_FirstColumn = hv_Column;
                                    //HOperatorSet.ClearShapeModel(hv_ModelID);
                                }
                                else
                                {
                                    iResult = 0;
                                    ho_Image.DispObj(Window);
                                    disp_message(Window, "Miss", "", 50, 0, "blue", "false");
                                    //HOperatorSet.ClearShapeModel(hv_ModelID);
                                    return;
                                }
                            }break;
                        case 1:
                            {
                                HOperatorSet.GetNccModelRegion(out ho_ModelRegion, hv_ModelID);
                                HOperatorSet.AreaCenter(ho_ModelRegion, out hv_ModelRegionArea, out hv_RefRow, out hv_RefColumn);
                                HOperatorSet.FindNccModel(ho_ImageEmphasize, hv_ModelID, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), MyBarcodeReaderPlus.ModelGrade / 100, 1, 0.5, "true", (new HTuple(6)).TupleConcat(1), out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                                if (hv_Score.TupleGreater(0) != 0)
                                {
                                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, -hv_RefRow, -hv_RefColumn, out hv_HomMat2D);
                                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle, 0, 0, out hv_HomMat2D);
                                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row, hv_Column, out hv_HomMat2D);
                                    ho_TransContours.Dispose();
                                    HOperatorSet.AffineTransRegion(ho_ModelRegion, out ho_Region, hv_HomMat2D, "nearest_neighbor");
                                    HOperatorSet.SmallestRectangle2(ho_Region, out hv_RectangleRow, out hv_RectangleColumn, out hv_RectanglePhi, out hv_RectangleLength1, out hv_RectangleLength2);
                                    HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);

                                    Window.ClearWindow();
                                    ho_Image.DispObj(Window);
                                    Window.SetDraw("margin");
                                    Window.SetColor("yellow");
                                    ho_Rectangle.DispObj(Window);
                                    disp_message(Window, "模組分數:" + Math.Round(hv_Score.D * 100, 0), "", 0, 0, "green", "false");

                                    //HOperatorSet.ClearNccModel(hv_ModelID);
                                }
                                else
                                {
                                    iResult = 0;
                                    ho_Image.DispObj(Window);
                                    disp_message(Window, "Miss", "", 50, 0, "blue", "false");
                                    //HOperatorSet.ClearNccModel(hv_ModelID);
                                    return;
                                }
                            }break;
                    }
                }
                catch
                {
                    iResult = 0;
                    ho_Image.DispObj(Window);
                    disp_message(Window, "Miss", "", 50, 0, "blue", "false");
                    return;
                }
                //hv_Row = MyBarcodeReaderPlus.hv_FirstRow;
                //hv_Column = MyBarcodeReaderPlus.hv_FirstColumn;
                //ho_Region.Dispose();
                //if (MyBarcodeReaderPlus.DarkLightChoice == 0)
                //{
                //    HOperatorSet.Threshold(ho_ImageEmphasize, out ho_Region, MyBarcodeReaderPlus.Gray, 255);
                //}
                //else
                //{
                //    HOperatorSet.Threshold(ho_ImageEmphasize, out ho_Region, 0, MyBarcodeReaderPlus.Gray);
                //} 
                //ho_ConnectedRegions.Dispose();
                //HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
                //ho_RegionFillUp.Dispose();
                //HOperatorSet.FillUp(ho_ConnectedRegions, out ho_RegionFillUp);
                //ho_SelectedRegions.Dispose();
                //HOperatorSet.SelectShape(ho_RegionFillUp, out ho_SelectedRegions, "outer_radius", "and", MyBarcodeReaderPlus.FirstOuterRadius-50, MyBarcodeReaderPlus.FirstOuterRadius+30);
                //HTuple hv_Area = new HTuple();
                //HOperatorSet.RegionFeatures(ho_SelectedRegions, "area", out hv_Area);
                //{
                //    HObject ExpTmpOutVar_0;
                //    HOperatorSet.SelectShape(ho_SelectedRegions, out ExpTmpOutVar_0, "area", "and", hv_Area.TupleMax(), hv_Area.TupleMax()+1);

                //    ho_SelectedRegions.Dispose();
                //    ho_SelectedRegions = ExpTmpOutVar_0;
                //}

                //HOperatorSet.CountObj(ho_SelectedRegions,out hv_Number);

                ////沒找到物料
                //if(hv_Number==0)
                //{
                //    iResult = 0;
                //    ho_Image.DispObj(Window);
                //    disp_message(Window, "Miss", "", 50, 50, "blue", "false");

                //    return;
                //}
                try
                {
                    //第一次先找圓心
                    //HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
                    ho_UsedEdges.Dispose(); ho_Contour.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                    gen_circle_center(ho_ImageEmphasize, out ho_UsedEdges, out ho_Contour, out ho_ResultContours,
                        out ho_CrossCenter, hv_Row, hv_Column, hv_Radius, hv_Length, hv_MeasureThreshold, hv_MeasureTransition,
                        hv_MeasureSelect, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);
                }
                catch
                {
                    iResult = 0;
                }
                ho_Region.Dispose();
                HOperatorSet.GenRegionContourXld(ho_ResultContours, out ho_Region, "filled");
                ho_RegionDilation.Dispose();
                HOperatorSet.DilationCircle(ho_Region, out ho_RegionDilation, 5);
                ho_ReducedImage2.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageEmphasize, ho_RegionDilation, out ho_ReducedImage2);
                ho_Contour2.Dispose(); ho_UsedEdges2.Dispose(); ho_ResultContours2.Dispose();
                gen_rectangle2_center(ho_ReducedImage2, out ho_Contour2, out ho_UsedEdges2,
                    out ho_ResultContours2, hv_ResultRow, hv_ResultColumn, hv_Phi2, hv_Length21, hv_Length22,
                    hv_Length2, hv_MeasureThreshold2, hv_MeasureTransition2, hv_MeasureSelect2, out hv_ResultRow2, out hv_ResultColumn2, out hv_ResultPhi2,
                    out hv_ResultLength21, out hv_ResultLength22);

                iResult = 1;
            }
            catch
            {
                iResult = 0;
            }
            ho_Image.Dispose();
            ho_Circle.Dispose();
            ho_ReducedImage.Dispose();
            ho_ImageMedian.Dispose();
            ho_ImageEmphasize.Dispose();
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_RegionFillUp.Dispose();
            ho_RegionDilation.Dispose();
            ho_ReducedImage2.Dispose();
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Visual", "MeasureSelect", MyBarcodeReaderPlus.MeasureSelect.ToString(), Path);
            IniFile.Write("Visual", "Radius", MyBarcodeReaderPlus.Radius.ToString(), Path);
            IniFile.Write("Visual", "Length", MyBarcodeReaderPlus.Length.ToString(), Path);
            IniFile.Write("Visual", "MeasureTransition", MyBarcodeReaderPlus.MeasureTransition.ToString(), Path);
            IniFile.Write("Visual", "MeasureThreshold", MyBarcodeReaderPlus.MeasureThreshold.ToString(), Path);
        }

        private void ucOuterRadius_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.OuterRadius = ucOuterRadius.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            if (MyBarcodeReaderPlus.hv_ResultRow == null || MyBarcodeReaderPlus.hv_ResultColumn == null)
            {
                MessageBox.Show("請先求目標圓心!");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_CircleSector = new HObject();

            GenCircleSector(MyBarcodeReaderPlus.hv_ResultRow, MyBarcodeReaderPlus.hv_ResultColumn, out ho_CircleSector);
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("orange");
            ho_CircleSector.DispObj(Window);
        }

        private void ucInnerRadius_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.InnerRadius = ucInnerRadius.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            if (MyBarcodeReaderPlus.hv_ResultRow == null || MyBarcodeReaderPlus.hv_ResultColumn == null)
            {
                MessageBox.Show("請先求目標圓心!");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_CircleSector = new HObject();

            GenCircleSector(MyBarcodeReaderPlus.hv_ResultRow, MyBarcodeReaderPlus.hv_ResultColumn, out ho_CircleSector);
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("orange");
            ho_CircleSector.DispObj(Window);
        }

        private void ucStartAngle_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.StartAngle = ucStartAngle.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            if (MyBarcodeReaderPlus.hv_ResultRow == null || MyBarcodeReaderPlus.hv_ResultColumn == null)
            {
                MessageBox.Show("請先求目標圓心!");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_CircleSector = new HObject();

            GenCircleSector(MyBarcodeReaderPlus.hv_ResultRow, MyBarcodeReaderPlus.hv_ResultColumn, out ho_CircleSector);
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("orange");
            ho_CircleSector.DispObj(Window);
        }

        private void ucEndAngle_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.EndAngle = ucEndAngle.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            if (MyBarcodeReaderPlus.hv_ResultRow == null || MyBarcodeReaderPlus.hv_ResultColumn == null)
            {
                MessageBox.Show("請先求目標圓心!");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_CircleSector = new HObject();

            GenCircleSector(MyBarcodeReaderPlus.hv_ResultRow, MyBarcodeReaderPlus.hv_ResultColumn, out ho_CircleSector);
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("orange");
            ho_CircleSector.DispObj(Window);
        }

        private void btnSave4_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Visual", "OuterRadius", MyBarcodeReaderPlus.OuterRadius.ToString(), Path);
            IniFile.Write("Visual", "InnerRadius", MyBarcodeReaderPlus.InnerRadius.ToString(), Path);
            IniFile.Write("Visual", "StartAngle", MyBarcodeReaderPlus.StartAngle.ToString(), Path);
            IniFile.Write("Visual", "EndAngle", MyBarcodeReaderPlus.EndAngle.ToString(), Path);
        }

        public void GenCircleSector(HTuple hv_ResultRow, HTuple hv_ResultColumn, out HObject ho_CircleSector)
        {
            HObject ho_OuterCircleSector_1 = new HObject(), ho_InnerCircleSector_1 = new HObject();
            HObject ho_OuterCircleSector_2 = new HObject(), ho_InnerCircleSector_2 = new HObject();
            HObject ho_RegionDifference_1 = new HObject(), ho_RegionDifference_2 = new HObject();
            HOperatorSet.GenEmptyObj(out ho_CircleSector);
            try
            {

                ho_OuterCircleSector_1.Dispose();
                HOperatorSet.GenCircleSector(out ho_OuterCircleSector_1, hv_ResultRow, hv_ResultColumn, MyBarcodeReaderPlus.OuterRadius, MyBarcodeReaderPlus.StartAngle.TupleRad(), MyBarcodeReaderPlus.EndAngle.TupleRad());
                ho_InnerCircleSector_1.Dispose();
                HOperatorSet.GenCircleSector(out ho_InnerCircleSector_1, hv_ResultRow, hv_ResultColumn, MyBarcodeReaderPlus.InnerRadius, MyBarcodeReaderPlus.StartAngle.TupleRad(), MyBarcodeReaderPlus.EndAngle.TupleRad());
                ho_RegionDifference_1.Dispose();
                HOperatorSet.Difference(ho_OuterCircleSector_1, ho_InnerCircleSector_1, out ho_RegionDifference_1);
                ho_OuterCircleSector_2.Dispose();
                HOperatorSet.GenCircleSector(out ho_OuterCircleSector_2, hv_ResultRow, hv_ResultColumn, MyBarcodeReaderPlus.OuterRadius, (MyBarcodeReaderPlus.StartAngle + 180).TupleRad(), (MyBarcodeReaderPlus.EndAngle + 180).TupleRad());
                ho_InnerCircleSector_2.Dispose();
                HOperatorSet.GenCircleSector(out ho_InnerCircleSector_2, hv_ResultRow, hv_ResultColumn, MyBarcodeReaderPlus.InnerRadius, (MyBarcodeReaderPlus.StartAngle + 180).TupleRad(), (MyBarcodeReaderPlus.EndAngle + 180).TupleRad());
                ho_RegionDifference_2.Dispose();
                HOperatorSet.Difference(ho_OuterCircleSector_2, ho_InnerCircleSector_2, out ho_RegionDifference_2);
                ho_CircleSector.Dispose();
                HOperatorSet.Union2(ho_RegionDifference_1, ho_RegionDifference_2, out ho_CircleSector);
            }
            catch
            {
            }
            ho_OuterCircleSector_1.Dispose();
            ho_InnerCircleSector_1.Dispose();
            ho_RegionDifference_1.Dispose();
            ho_OuterCircleSector_2.Dispose();
            ho_InnerCircleSector_2.Dispose();
            ho_RegionDifference_2.Dispose();
        }

        private void ucRegionThreshold_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.RegionThreshold = ucRegionThreshold.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_ConnectedRegions = new HObject(), ho_SelectedRegions = new HObject();
            HTuple hv_Rect2_Len1 = new HTuple(), hv_Rect2_Len2 = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            int Result = 0;
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            try
            {
                GenBarcodeRegion(MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_ConnectedRegions);

                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, (new HTuple("rect2_len1")).TupleConcat("rect2_len2"), "and", (new HTuple(1)).TupleConcat(1), (new HTuple(9999)).TupleConcat(9999));
                HOperatorSet.RegionFeatures(ho_SelectedRegions, "rect2_len1", out hv_Rect2_Len1);
                HOperatorSet.RegionFeatures(ho_SelectedRegions, "rect2_len2", out hv_Rect2_Len2);
                //沒找到區域就跳出
                if (Result == 0)
                    return;
                HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
                Window.SetColor("blue");
                Window.SetDraw("fill");
                ho_ConnectedRegions.DispObj(Window);
                for (int i = 0; i < hv_Row.Length; i++)
                {
                    disp_message(Window, hv_Rect2_Len1[i].D.ToString("f0"), "", (hv_Row[i] - 20), hv_Column[i], "orange", "false");
                    disp_message(Window, hv_Rect2_Len2[i].D.ToString("f0"), "", (hv_Row[i]), hv_Column[i], "yellow", "false");
                }
            }
            catch
            {
            }
        }


        //結合找圓心找出BarcodeRegion
        public void GenBarcodeRegion(HObject ho_Image, out int Result, out HObject ho_ConnectedRegions)
        {
            HWindow Window = hWindowControl1.HalconWindow;
            Result = 0;
            HObject ho_Rectangle = new HObject(), ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject(); ;
            HObject ho_ResultRegion = new HObject(), ho_CircleSector = new HObject();
            HObject ho_RegionDifference = new HObject(), ho_RegionIntersection = new HObject();
            HObject ho_ImageReduced = new HObject(), ho_ImageMedian = new HObject();
            HObject ho_Region2 = new HObject(), ho_FillUp = new HObject();
            HTuple hv_Threshold = new HTuple();
            HTuple hv_Rect2_Len1_Upper = new HTuple(), hv_Rect2_Len1_Lower = new HTuple(), hv_Rect2_Len2_Upper = new HTuple(), hv_Rect2_Len2_Lower = new HTuple();
            HTuple hv_ResultRow = new HTuple(), hv_ResultColumn = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);

            if (ho_Image == null)
                return;
            hv_Threshold = MyBarcodeReaderPlus.RegionThreshold;
            hv_Rect2_Len1_Upper = MyBarcodeReaderPlus.RegionRect2_Len1_Upper;
            hv_Rect2_Len1_Lower = MyBarcodeReaderPlus.RegionRect2_Len1_Lower;
            hv_Rect2_Len2_Upper = MyBarcodeReaderPlus.RegionRect2_Len2_Upper;
            hv_Rect2_Len2_Lower = MyBarcodeReaderPlus.RegionRect2_Len2_Lower;
            try
            {
                FindCenter(Window, MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out ho_UsedEdges2, out ho_Contour2, out ho_ResultContours2, out MyBarcodeReaderPlus.hv_ResultRow, out MyBarcodeReaderPlus.hv_ResultColumn, out MyBarcodeReaderPlus.hv_ResultPhi);
            }
            catch
            {
            }
            //找不到圓心
            if (Result == 0)
            {
                ho_Image.DispObj(Window);
                disp_message(Window, "Miss", "", 50, 50, "blue", "false");
                return;
            }
            try
            {
                hv_ResultColumn = MyBarcodeReaderPlus.hv_ResultColumn;
                hv_ResultRow = MyBarcodeReaderPlus.hv_ResultRow;
                ho_RegionIntersection.Dispose();
                HOperatorSet.IntersectionClosedContoursXld(ho_ResultContours, ho_ResultContours2, out ho_RegionIntersection);
                HOperatorSet.GenRegionContourXld(ho_RegionIntersection, out ho_Rectangle, "filled");
                //劃出找Barcode框框範圍
                GenCircleSector(hv_ResultRow, hv_ResultColumn, out ho_CircleSector);
                ho_RegionIntersection.Dispose();
                HOperatorSet.Intersection(ho_CircleSector, ho_Rectangle, out ho_RegionIntersection);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionIntersection, out ho_ImageReduced);
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianRect(ho_ImageReduced, out ho_ImageMedian, 5, 5);
                ho_Region2.Dispose();
                if (MyBarcodeReaderPlus.RegionDarkLight == 0)
                    HOperatorSet.Threshold(ho_ImageMedian, out ho_Region2, hv_Threshold, 255);
                else
                    HOperatorSet.Threshold(ho_ImageMedian, out ho_Region2, 0, hv_Threshold);
                ho_FillUp.Dispose();
                HOperatorSet.FillUp(ho_Region2, out ho_FillUp);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_FillUp, out ho_ConnectedRegions);
                //讀取框框外接方形長寬
            }
            catch
            {

            }
            ho_ResultRegion.Dispose();
            ho_CircleSector.Dispose();
            ho_RegionDifference.Dispose();
            ho_RegionIntersection.Dispose();
            ho_ImageReduced.Dispose();
            ho_ImageMedian.Dispose();
            ho_Region2.Dispose();
            ho_FillUp.Dispose();
        }

        private void cmbRegionDarkLight_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.RegionDarkLight = cmbRegionDarkLight.SelectedIndex;
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "RegionThreshold", MyBarcodeReaderPlus.RegionThreshold.ToString(), Path);
            IniFile.Write("Visual", "RegionDarkLight", MyBarcodeReaderPlus.RegionDarkLight.ToString(), Path);
            IniFile.Write("Visual", "RegionRect2_Len1_Upper", MyBarcodeReaderPlus.RegionRect2_Len1_Upper.ToString(), Path);
            IniFile.Write("Visual", "RegionRect2_Len1_Lower", MyBarcodeReaderPlus.RegionRect2_Len1_Lower.ToString(), Path);
            IniFile.Write("Visual", "RegionRect2_Len2_Upper", MyBarcodeReaderPlus.RegionRect2_Len2_Upper.ToString(), Path);
            IniFile.Write("Visual", "RegionRect2_Len2_Lower", MyBarcodeReaderPlus.RegionRect2_Len2_Lower.ToString(), Path);
        }

        private void ucRegionRect2_Len1_Upper_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.RegionRect2_Len1_Upper = ucRegionRect2_Len1_Upper.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_ConnectedRegions = new HObject(), ho_SelectedRegions = new HObject();
            HTuple hv_Rect2_Len1 = new HTuple(), hv_Rect2_Len2 = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            int Result = 0;
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            try
            {
                GenBarcodeRegion(MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_ConnectedRegions);

                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, (new HTuple("rect2_len1")).TupleConcat("rect2_len2"), "and", (new HTuple(MyBarcodeReaderPlus.RegionRect2_Len1_Lower)).TupleConcat(MyBarcodeReaderPlus.RegionRect2_Len2_Lower), (new HTuple(MyBarcodeReaderPlus.RegionRect2_Len1_Upper)).TupleConcat(MyBarcodeReaderPlus.RegionRect2_Len2_Upper));
                HOperatorSet.RegionFeatures(ho_SelectedRegions, "rect2_len1", out hv_Rect2_Len1);
                HOperatorSet.RegionFeatures(ho_SelectedRegions, "rect2_len2", out hv_Rect2_Len2);
                //沒找到區域就跳出
                if (Result == 0)
                    return;
                HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
                Window.SetColor("blue");
                Window.SetDraw("fill");
                ho_SelectedRegions.DispObj(Window);
                for (int i = 0; i < hv_Row.Length; i++)
                {
                    disp_message(Window, hv_Rect2_Len1[i].D.ToString("f0"), "", (hv_Row[i] - 20), hv_Column[i], "orange", "false");
                    disp_message(Window, hv_Rect2_Len2[i].D.ToString("f0"), "", (hv_Row[i]), hv_Column[i], "yellow", "false");
                }
            }
            catch
            {
            }
        }

        private void ucRegionRect2_Len1_Lower_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.RegionRect2_Len1_Lower = ucRegionRect2_Len1_Lower.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_ConnectedRegions = new HObject(), ho_SelectedRegions = new HObject();
            HTuple hv_Rect2_Len1 = new HTuple(), hv_Rect2_Len2 = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            int Result = 0;
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            try
            {
                GenBarcodeRegion(MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_ConnectedRegions);

                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, (new HTuple("rect2_len1")).TupleConcat("rect2_len2"), "and", (new HTuple(MyBarcodeReaderPlus.RegionRect2_Len1_Lower)).TupleConcat(MyBarcodeReaderPlus.RegionRect2_Len2_Lower), (new HTuple(MyBarcodeReaderPlus.RegionRect2_Len1_Upper)).TupleConcat(MyBarcodeReaderPlus.RegionRect2_Len2_Upper));
                HOperatorSet.RegionFeatures(ho_SelectedRegions, "rect2_len1", out hv_Rect2_Len1);
                HOperatorSet.RegionFeatures(ho_SelectedRegions, "rect2_len2", out hv_Rect2_Len2);
                //沒找到區域就跳出
                if (Result == 0)
                    return;
                HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
                Window.SetColor("blue");
                Window.SetDraw("fill");
                ho_SelectedRegions.DispObj(Window);
                for (int i = 0; i < hv_Row.Length; i++)
                {
                    disp_message(Window, hv_Rect2_Len1[i].D.ToString("f0"), "", (hv_Row[i] - 20), hv_Column[i], "orange", "false");
                    disp_message(Window, hv_Rect2_Len2[i].D.ToString("f0"), "", (hv_Row[i]), hv_Column[i], "yellow", "false");
                }
            }
            catch
            {
            }
        }

        private void ucRegionRect2_Len2_Upper_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.RegionRect2_Len2_Upper = ucRegionRect2_Len2_Upper.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_ConnectedRegions = new HObject(), ho_SelectedRegions = new HObject();
            HTuple hv_Rect2_Len1 = new HTuple(), hv_Rect2_Len2 = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            int Result = 0;
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            try
            {
                GenBarcodeRegion(MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_ConnectedRegions);

                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, (new HTuple("rect2_len1")).TupleConcat("rect2_len2"), "and", (new HTuple(MyBarcodeReaderPlus.RegionRect2_Len1_Lower)).TupleConcat(MyBarcodeReaderPlus.RegionRect2_Len2_Lower), (new HTuple(MyBarcodeReaderPlus.RegionRect2_Len1_Upper)).TupleConcat(MyBarcodeReaderPlus.RegionRect2_Len2_Upper));
                HOperatorSet.RegionFeatures(ho_SelectedRegions, "rect2_len1", out hv_Rect2_Len1);
                HOperatorSet.RegionFeatures(ho_SelectedRegions, "rect2_len2", out hv_Rect2_Len2);
                //沒找到區域就跳出
                if (Result == 0)
                    return;
                HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
                Window.SetColor("blue");
                Window.SetDraw("fill");
                ho_SelectedRegions.DispObj(Window);
                for (int i = 0; i < hv_Row.Length; i++)
                {
                    disp_message(Window, hv_Rect2_Len1[i].D.ToString("f0"), "", (hv_Row[i] - 20), hv_Column[i], "orange", "false");
                    disp_message(Window, hv_Rect2_Len2[i].D.ToString("f0"), "", (hv_Row[i]), hv_Column[i], "yellow", "false");
                }
            }
            catch
            {
            }
        }

        private void ucRegionRect2_Len2_Lower_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.RegionRect2_Len2_Lower = ucRegionRect2_Len2_Lower.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_ConnectedRegions = new HObject(), ho_SelectedRegions = new HObject();
            HTuple hv_Rect2_Len1 = new HTuple(), hv_Rect2_Len2 = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            int Result = 0;
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            try
            {
                GenBarcodeRegion(MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_ConnectedRegions);

                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, (new HTuple("rect2_len1")).TupleConcat("rect2_len2"), "and", (new HTuple(MyBarcodeReaderPlus.RegionRect2_Len1_Lower)).TupleConcat(MyBarcodeReaderPlus.RegionRect2_Len2_Lower), (new HTuple(MyBarcodeReaderPlus.RegionRect2_Len1_Upper)).TupleConcat(MyBarcodeReaderPlus.RegionRect2_Len2_Upper));
                HOperatorSet.RegionFeatures(ho_SelectedRegions, "rect2_len1", out hv_Rect2_Len1);
                HOperatorSet.RegionFeatures(ho_SelectedRegions, "rect2_len2", out hv_Rect2_Len2);
                //沒找到區域就跳出
                if (Result == 0)
                    return;
                HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
                Window.SetColor("blue");
                Window.SetDraw("fill");
                ho_SelectedRegions.DispObj(Window);
                for (int i = 0; i < hv_Row.Length; i++)
                {
                    disp_message(Window, hv_Rect2_Len1[i].D.ToString("f0"), "", (hv_Row[i] - 20), hv_Column[i], "orange", "false");
                    disp_message(Window, hv_Rect2_Len2[i].D.ToString("f0"), "", (hv_Row[i]), hv_Column[i], "yellow", "false");
                }
            }
            catch
            {
            }
        }

        private void btnReadBarocde_Click(object sender, EventArgs e)
        {
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_BarcodeAngle = new HTuple();
            Vision.BarcodeResult result = new Vision.BarcodeResult();
            int iResult = 0;
            string sResult = "";
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_ResultImage = new HObject();
            HOperatorSet.GetImageSize(MyBarcodeReaderPlus.ho_ImagePart, out hv_Width, out hv_Height);
            HOperatorSet.SetPart(Window, 0, 0, Height - 1, Width - 1);
            findReadBarcodeRegion(Window, MyBarcodeReaderPlus.ho_ImagePart, out iResult, out ho_ResultImage, out result);
        }

        public void findReadBarcodeRegion(HWindow Window, HObject ho_Image, out int Result, out HObject ho_ResultImage, out Vision.BarcodeResult m_BarcodeResult)
        {
            set_display_font(Window, 15, "mono", "true", "false");
            m_BarcodeResult = new Vision.BarcodeResult();
            HObject ho_Rectangle = new HObject(), ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject();
            HObject ho_ResultRegion = new HObject(), ho_CircleSector = new HObject();
            HObject ho_RegionDifference = new HObject(), ho_RegionIntersection = new HObject();
            HObject ho_ImageReduced = new HObject(), ho_ImageMedian = new HObject();
            HObject ho_ConnectedRegions = new HObject(), ho_SelectedRegions = new HObject();
            HObject ho_Region2 = new HObject(), ho_FillUp = new HObject();
            HObject ho_ImagePart = new HObject(), ho_SymbolXLDs = new HObject();
            HTuple hv_Threshold = new HTuple();
            HTuple hv_ResultRow = new HTuple(), hv_ResultColumn = new HTuple(), hv_ResultPhi = new HTuple();
            HTuple hv_Number = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple(), hv_Phi = new HTuple(), hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();

            HOperatorSet.GenEmptyObj(out ho_ResultImage);
            HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);
            HTuple hv_ExpDefaultWinHandle = Window;
            Result = 0;
            try
            {
                Window.SetDraw("margin");
                //set_display_font(Window, 30, "mono", "true", "false");

                if (ho_Image == null)
                    return;
                hv_Threshold = MyBarcodeReaderPlus.RegionThreshold;

                FindCenter(Window, ho_Image, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out ho_UsedEdges2, out ho_Contour2, out ho_ResultContours2, out hv_ResultRow, out hv_ResultColumn, out hv_ResultPhi);
            }
            catch
            {
                Result = -1;
                ho_Image.DispObj(Window);
                set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                disp_message(Window, "NG1", "", 60, 0, "red", "false");
                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                return;
            }
            //找不到圓心
            if (Result == 0)
            {
                ho_Image.DispObj(Window);
                set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                disp_message(Window, "Miss", "", 60, 0, "blue", "false");
                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                return;
            }
            //檢測Barcode區域方案選擇
            switch (MyBarcodeReaderPlus.RegionProjectSet)
            {
                #region 方案一
                case 0:
                    {
                        try
                        {
                            ho_RegionIntersection.Dispose();
                            HOperatorSet.IntersectionClosedContoursXld(ho_ResultContours, ho_ResultContours2, out ho_RegionIntersection);
                            HOperatorSet.GenRegionContourXld(ho_RegionIntersection, out ho_Rectangle, "filled");
                            //劃出找Barcode框框範圍
                            GenCircleSector(hv_ResultRow, hv_ResultColumn, out ho_CircleSector);
                            ho_RegionIntersection.Dispose();
                            HOperatorSet.Intersection(ho_CircleSector, ho_Rectangle, out ho_RegionIntersection);
                            ho_ImageReduced.Dispose();
                            HOperatorSet.ReduceDomain(ho_Image, ho_RegionIntersection, out ho_ImageReduced);
                            ho_ImageMedian.Dispose();
                            HOperatorSet.MedianRect(ho_ImageReduced, out ho_ImageMedian, 5, 5);
                            ho_Region2.Dispose();
                            if (MyBarcodeReaderPlus.RegionDarkLight == 0)
                                HOperatorSet.Threshold(ho_ImageMedian, out ho_Region2, hv_Threshold, 255);
                            else
                                HOperatorSet.Threshold(ho_ImageMedian, out ho_Region2, 0, hv_Threshold);
                            ho_FillUp.Dispose();
                            HOperatorSet.FillUp(ho_Region2, out ho_FillUp);
                            ho_ConnectedRegions.Dispose();
                            HOperatorSet.Connection(ho_FillUp, out ho_ConnectedRegions);
                            ho_SelectedRegions.Dispose();
                            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, (new HTuple("rect2_len1")).TupleConcat("rect2_len2"), "and", (new HTuple(MyBarcodeReaderPlus.RegionRect2_Len1_Lower)).TupleConcat(MyBarcodeReaderPlus.RegionRect2_Len2_Lower), (new HTuple(MyBarcodeReaderPlus.RegionRect2_Len1_Upper)).TupleConcat(MyBarcodeReaderPlus.RegionRect2_Len2_Upper));
                            HTuple hv_Area = new HTuple();
                            HOperatorSet.RegionFeatures(ho_SelectedRegions, "area", out hv_Area);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.SelectShape(ho_SelectedRegions, out ExpTmpOutVar_0, "area", "and", hv_Area.TupleMax(), hv_Area.TupleMax() + 1);
                                ho_SelectedRegions.Dispose();
                                ho_SelectedRegions = ExpTmpOutVar_0;
                            }

                            HOperatorSet.CountObj(ho_SelectedRegions, out hv_Number);

                            //找不到Barcode框框
                            if (hv_Number == 0)
                            {
                                Result = -1;
                                ho_Image.DispObj(Window);
                                disp_message(Window, "NG1", "", 50, 50, "red", "false");
                                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                                return;
                            }
                            //擬合矩形
                            HOperatorSet.SmallestRectangle2(ho_SelectedRegions, out hv_Row, out hv_Column, out hv_ResultPhi, out hv_Length1, out hv_Length2);
                            HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_ResultPhi, hv_Length1, hv_Length2);
                            //讀取框框外接方形長寬
                        }
                        catch
                        {
                            Result = -1;
                            ho_Image.DispObj(Window);
                            disp_message(Window, "NG1", "", 50, 50, "red", "false");
                            HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                            return;
                        }
                        ho_ResultRegion.Dispose();
                        ho_CircleSector.Dispose();
                        ho_RegionDifference.Dispose();
                        ho_RegionIntersection.Dispose();
                        ho_ImageReduced.Dispose();
                        ho_ImageMedian.Dispose();
                        ho_Region2.Dispose();
                        ho_FillUp.Dispose();


                        break;
                    }
                #endregion
                #region 方案二
                case 1:
                    {
                        ho_Rectangle.Dispose();
                        SetRegion(hv_ResultRow, hv_ResultColumn, hv_ResultPhi, out ho_Rectangle);
                        break;
                    }
                #endregion
            }
            
            #region //產品角度
            HObject ho_ResultContours_Angle1 = new HObject();
            HTuple hv_ResultPhi_Angle1 = new HTuple();
            try
            {
                switch (MyBarcodeReaderPlus.Mode_Angle)
                {
                    case 0:
                        {
                            hv_ResultPhi_Angle1 = hv_ResultPhi;
                            //角度默認為找產品時獲得的角度
                        } break;
                    case 1:
                        {
                            HObject ho_UsedEdges_Angle1 = new HObject(), ho_Contour_Angle1 = new HObject();

                            ho_UsedEdges_Angle1.Dispose(); ho_Contour_Angle1.Dispose(); ho_ResultContours_Angle1.Dispose();
                            FindRectangleCenter_Angle1(ho_Image, hv_Row, hv_Column, out ho_UsedEdges_Angle1, out ho_Contour_Angle1, out ho_ResultContours_Angle1, out hv_ResultPhi_Angle1);

                            ho_UsedEdges_Angle1.Dispose();
                            ho_Contour_Angle1.Dispose();
                            //檢測區域改為獲得的矩形
                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRegionContourXld(ho_ResultContours_Angle1, out ho_Rectangle, "filled");
                        } break;
                }
            }
            catch
            {
                ho_Image.DispObj(Window);
                set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                Window.SetColor("yellow");
                disp_message(Window, "NG2", "", 60, 0, "red", "false");
                disp_message(Window, "找不到產品角度", "", 90, 0, "red", "false");
                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                return;
            }
            //判定是否找到矩形
            if (hv_ResultPhi_Angle1 == null)
            {
                ho_Image.DispObj(Window);
                set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                Window.SetColor("yellow");
                ho_Rectangle.DispObj(Window);
                disp_message(Window, "NG2", "", 60, 0, "red", "false");
                disp_message(Window, "找不到產品角度", "", 90, 0, "red", "false");
                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
            }
            else
            {
                hv_ResultPhi = hv_ResultPhi_Angle1;
            }
            #endregion
            try
            {
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
                ho_ImagePart.Dispose();
                HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);
                ho_SymbolXLDs.Dispose();
                ReadBarcode(ho_ImagePart, out ho_SymbolXLDs, out Result, out m_BarcodeResult);
                //if(hv_ResultPhi.TupleDeg())
                // 角度轉換,產品角度與Barcode共同計算
                HTuple hv_LensAngle = hv_ResultPhi.TupleDeg();
                //轉換產品角度到-45~+45之間
                if (hv_LensAngle.D <= 45 && hv_LensAngle.D > -45)
                    hv_LensAngle = hv_LensAngle + 0;
                else if (hv_LensAngle.D <= 135 && hv_LensAngle.D > 45)
                    hv_LensAngle = hv_LensAngle - 90;
                else if (hv_LensAngle.D <= 180 && hv_LensAngle.D > 135)
                    hv_LensAngle = hv_LensAngle - 180;
                else if (hv_LensAngle.D <= -45 && hv_LensAngle.D > -135)
                    hv_LensAngle = hv_LensAngle + 90;
                else
                    hv_LensAngle = hv_LensAngle + 180;

                if (hv_Row.D <= hv_ResultRow && hv_Column.D > hv_ResultColumn.D)//二維碼在產品右上方
                {
                    m_BarcodeResult.BarcodeAngle = (m_BarcodeResult.hv_DecodedOrientation.D - hv_LensAngle.D) % 360;
                }
                else if (hv_Row.D <= hv_ResultRow && hv_Column.D <= hv_ResultColumn.D)//二維碼在產品左上方
                {
                    m_BarcodeResult.BarcodeAngle = (m_BarcodeResult.hv_DecodedOrientation.D - hv_LensAngle.D - 90) % 360;
                }
                else if (hv_Row.D > hv_ResultRow && hv_Column.D <= hv_ResultColumn.D)//二維碼在產品左下方
                {
                    m_BarcodeResult.BarcodeAngle = (m_BarcodeResult.hv_DecodedOrientation.D - hv_LensAngle.D + 180) % 360;
                }
                else//二維碼在產品右下方
                {
                    m_BarcodeResult.BarcodeAngle = (m_BarcodeResult.hv_DecodedOrientation.D - hv_LensAngle.D + 90) % 360;
                }
              
                m_BarcodeResult.BarcodeAngle = Math.Round(m_BarcodeResult.BarcodeAngle, 1);

                if (Result == -2)
                {
                    ho_Image.DispObj(Window);
                    set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                    Window.SetColor("yellow");
                    ho_Rectangle.DispObj(Window);
                    disp_message(Window, "NG2", "", 60, 0, "red", "false");
                    HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                    return;
                }
            }
            catch
            {
                ho_Image.DispObj(Window);
                set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                Window.SetColor("yellow");
                ho_Rectangle.DispObj(Window);
                disp_message(Window, "NG2", "", 60, 0, "red", "false");
                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                return;
            }
            #region//移動Barcode位置並檢查Barcode是否雕歪
            try
            {
                HOperatorSet.RegionFeatures(ho_Rectangle, "row1", out hv_Row1);
                HOperatorSet.RegionFeatures(ho_Rectangle, "column1", out hv_Column1);
                if (hv_Row1 > hv_ResultRow)
                {
                    //hv_DecodedOrientation
                }
                //判斷Barcode角度
                //角度NG
                //管控Barcode鏡像並且鏡像
                double DifferenceAngle = 0;

                //XLD轉Region
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.GenRegionContourXld(ho_SymbolXLDs, out ExpTmpOutVar_0, "margin");
                    ho_SymbolXLDs.Dispose();
                    ho_SymbolXLDs = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.MoveRegion(ho_SymbolXLDs, out ExpTmpOutVar_0, hv_Row1, hv_Column1);
                    ho_SymbolXLDs.Dispose();
                    ho_SymbolXLDs = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ErosionCircle(ho_Rectangle, out ExpTmpOutVar_0, MyBarcodeReaderPlus.RegionErosion);
                    ho_Rectangle.Dispose();
                    ho_Rectangle = ExpTmpOutVar_0;
                }
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_SymbolXLDs, ho_Rectangle, out ho_RegionDifference);
                HOperatorSet.CountObj(ho_RegionDifference, out hv_Number);
                hv_A = 0;
                HOperatorSet.RegionFeatures(ho_RegionDifference, "area", out  hv_A);
                if (hv_A.D > 0)
                {
                    Result = -3;
                    set_display_font(hv_ExpDefaultWinHandle, 15, "mono", "true", "false");
                    ho_Image.DispObj(Window);
                    Window.SetColor("yellow");
                    ho_Rectangle.DispObj(Window);
                    Window.SetColor("green");
                    ho_SymbolXLDs.DispObj(Window);
                    disp_message(Window, "Barocde:" + m_BarcodeResult.Barcode, "", 0, 0, "green", "false");
                    disp_message(Window, "Barocde角度:" + m_BarcodeResult.BarcodeAngle, "", 20, 0, "green", "false");
                    disp_message(Window, "Barocde鏡像:" + m_BarcodeResult.hv_DecodedMirrored.S, "", 40, 0, "green", "false");
                    disp_message(Window, "NG3", "", 60, 0, "red", "false");
                    disp_message(Window, m_BarcodeResult.MeanLight + ":MeanLight", "", 80, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Overall_Quality[0] + ":Overall_Quality", "", 100, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Cell_Contrast[0] + ":Cell_Contrast", "", 120, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Print_Growth[0] + ":Print_Growth", "", 140, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Unused_Error_Correction[0] + ":Unused_Error_Correction", "", 160, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Cell_Modulation[0] + ":Cell_Modulation", "", 180, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Fixed_Pattern_Damage[0] + ":Fixed_Pattern_Damage", "", 200, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Grid_Nonuniformity[0] + ":Grid_Nonuniformity", "", 220, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Decode[0] + ":Decode", "", 240, 0, "green", "false");




                    HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                    return;
                }
                //獲得ho_SymbolXLDs位置,判斷是在圖片的上面還是下面
                HOperatorSet.RegionFeatures(ho_SymbolXLDs, "row1", out hv_Row1);
                HOperatorSet.RegionFeatures(ho_SymbolXLDs, "column1", out hv_Column1);
                //如果Barcode在圓心以下,角度+or-180度

                //if (hv_Row1 > hv_ResultRow)
                //{
                //    if (m_BarcodeResult.BarcodeAngle >= 0)
                //        m_BarcodeResult.BarcodeAngle = m_BarcodeResult.BarcodeAngle - 180;
                //    else
                //        m_BarcodeResult.BarcodeAngle = m_BarcodeResult.BarcodeAngle + 180;
                //}
                //角度轉換為-179.9~180之間
                if (m_BarcodeResult.BarcodeAngle > 180)
                    m_BarcodeResult.BarcodeAngle = m_BarcodeResult.BarcodeAngle - 360;
                else if (m_BarcodeResult.BarcodeAngle <= -180)
                    m_BarcodeResult.BarcodeAngle = m_BarcodeResult.BarcodeAngle + 360;

                if (Math.Abs((m_BarcodeResult.BarcodeAngle - MyBarcodeReaderPlus.BarcodeAngleSet)) > 180)
                    DifferenceAngle = 360 - (Math.Abs(m_BarcodeResult.BarcodeAngle - MyBarcodeReaderPlus.BarcodeAngleSet));
                else
                    DifferenceAngle = Math.Abs((m_BarcodeResult.BarcodeAngle - MyBarcodeReaderPlus.BarcodeAngleSet));

                if (DifferenceAngle > MyBarcodeReaderPlus.AllowableOffsetAngle_L)
                {
                    Result = -4;
                    set_display_font(hv_ExpDefaultWinHandle, 15, "mono", "true", "false");
                    ho_Image.DispObj(Window);
                    Window.SetColor("yellow");
                    ho_Rectangle.DispObj(Window);
                    Window.SetColor("green");
                    ho_SymbolXLDs.DispObj(Window);
                    disp_message(Window, "Barocde:" + m_BarcodeResult.Barcode, "", 0, 0, "green", "false");
                    disp_message(Window, "Barocde角度:" + m_BarcodeResult.BarcodeAngle, "", 20, 0, "red", "false");
                    disp_message(Window, "Barocde鏡像:" + m_BarcodeResult.hv_DecodedMirrored.S, "", 40, 0, "green", "false");
                    disp_message(Window, "NG4", "", 60, 0, "red", "false");
                    disp_message(Window, m_BarcodeResult.MeanLight + ":MeanLight", "", 80, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Overall_Quality[0] + ":Overall_Quality", "", 100, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Cell_Contrast[0] + ":Cell_Contrast", "", 120, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Print_Growth[0] + ":Print_Growth", "", 140, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Unused_Error_Correction[0] + ":Unused_Error_Correction", "", 160, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Cell_Modulation[0] + ":Cell_Modulation", "", 180, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Fixed_Pattern_Damage[0] + ":Fixed_Pattern_Damage", "", 200, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Grid_Nonuniformity[0] + ":Grid_Nonuniformity", "", 220, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Decode[0] + ":Decode", "", 240, 0, "green", "false");
                    set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                    disp_message(Window, "L邊NG", "", 500, 0, "red", "false");
                    HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                    return;
                }
                //是否鏡像
                if (!MyBarcodeReaderPlus.Mirrored && m_BarcodeResult.hv_DecodedMirrored.S != "no")
                {
                    Result = -4;
                    set_display_font(hv_ExpDefaultWinHandle, 15, "mono", "true", "false");
                    ho_Image.DispObj(Window);
                    Window.SetColor("yellow");
                    ho_Rectangle.DispObj(Window);
                    Window.SetColor("green");
                    ho_SymbolXLDs.DispObj(Window);
                    disp_message(Window, "Barocde:" + m_BarcodeResult.Barcode, "", 0, 0, "green", "false");
                    disp_message(Window, "Barocde角度:" + m_BarcodeResult.BarcodeAngle, "", 20, 0, "green", "false");
                    disp_message(Window, "Barocde鏡像:" + m_BarcodeResult.hv_DecodedMirrored.S, "", 40, 0, "red", "false");
                    disp_message(Window, "NG4", "", 60, 0, "red", "false");
                    disp_message(Window, m_BarcodeResult.MeanLight + ":MeanLight", "", 80, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Overall_Quality[0] + ":Overall_Quality", "", 100, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Cell_Contrast[0] + ":Cell_Contrast", "", 120, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Print_Growth[0] + ":Print_Growth", "", 140, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Unused_Error_Correction[0] + ":Unused_Error_Correction", "", 160, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Cell_Modulation[0] + ":Cell_Modulation", "", 180, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Fixed_Pattern_Damage[0] + ":Fixed_Pattern_Damage", "", 200, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Grid_Nonuniformity[0] + ":Grid_Nonuniformity", "", 220, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Decode[0] + ":Decode", "", 240, 0, "green", "false");
                    set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                    disp_message(Window, "鏡像NG", "", 500, 0, "red", "false");
                    HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                    return;
                }
                //角度是否NG
                if (DifferenceAngle > MyBarcodeReaderPlus.AllowableOffsetAngle)
                {
                    Result = -4;
                    set_display_font(hv_ExpDefaultWinHandle, 15, "mono", "true", "false");
                    ho_Image.DispObj(Window);
                    Window.SetColor("yellow");
                    ho_Rectangle.DispObj(Window);
                    Window.SetColor("green");
                    ho_SymbolXLDs.DispObj(Window);
                    disp_message(Window, "Barocde:" + m_BarcodeResult.Barcode, "", 0, 0, "green", "false");
                    disp_message(Window, "Barocde角度:" + m_BarcodeResult.BarcodeAngle, "", 20, 0, "red", "false");
                    disp_message(Window, "Barocde鏡像:" + m_BarcodeResult.hv_DecodedMirrored.S, "", 40, 0, "green", "false");
                    disp_message(Window, "NG4", "", 60, 0, "red", "false");
                    disp_message(Window, m_BarcodeResult.MeanLight + ":MeanLight", "", 80, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Overall_Quality[0] + ":Overall_Quality", "", 100, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Cell_Contrast[0] + ":Cell_Contrast", "", 120, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Print_Growth[0] + ":Print_Growth", "", 140, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Unused_Error_Correction[0] + ":Unused_Error_Correction", "", 160, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Cell_Modulation[0] + ":Cell_Modulation", "", 180, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Fixed_Pattern_Damage[0] + ":Fixed_Pattern_Damage", "", 200, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Grid_Nonuniformity[0] + ":Grid_Nonuniformity", "", 220, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Decode[0] + ":Decode", "", 240, 0, "green", "false");
                    set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                    disp_message(Window, "角度NG", "", 500, 0, "red", "false");
                    HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                    return;
                }

                //等級判斷(為了去除小數點,加了substring(1,1))
                if (MyBarcodeReaderPlus.Overall_Quality <= int.Parse(m_BarcodeResult.Overall_Quality[1].ToString().Substring(0, 1)) &&
                    MyBarcodeReaderPlus.Cell_Contrast <= int.Parse(m_BarcodeResult.Cell_Contrast[1].ToString().Substring(0, 1)) &&
                    MyBarcodeReaderPlus.Print_Growth <= int.Parse(m_BarcodeResult.Print_Growth[1].ToString().Substring(0, 1)) &&
                    MyBarcodeReaderPlus.Unused_Error_Correction <= int.Parse(m_BarcodeResult.Unused_Error_Correction[1].ToString().Substring(0, 1)) &&
                    MyBarcodeReaderPlus.Cell_Modulation <= int.Parse(m_BarcodeResult.Cell_Modulation[1].ToString().Substring(0, 1)) &&
                    MyBarcodeReaderPlus.Fixed_Pattern_Damage <= int.Parse(m_BarcodeResult.Fixed_Pattern_Damage[1].ToString().Substring(0, 1)) &&
                MyBarcodeReaderPlus.Grid_Nonuniformity <= int.Parse(m_BarcodeResult.Fixed_Pattern_Damage[1].ToString().Substring(0, 1)))
                {
                    Result = 1;
                    set_display_font(hv_ExpDefaultWinHandle, 15, "mono", "true", "false");
                    ho_Image.DispObj(Window);
                    Window.SetColor("yellow");
                    ho_Rectangle.DispObj(Window);
                    Window.SetColor("green");
                    ho_SymbolXLDs.DispObj(Window);
                    disp_message(Window, "Barocde:" + m_BarcodeResult.Barcode, "", 0, 0, "green", "false");
                    disp_message(Window, "Barocde角度:" + m_BarcodeResult.BarcodeAngle, "", 20, 0, "green", "false");
                    disp_message(Window, "Barocde鏡像:" + m_BarcodeResult.hv_DecodedMirrored.S, "", 40, 0, "green", "false");
                    disp_message(Window, "OK", "", 60, 0, "green", "false");
                    disp_message(Window, Math.Round(m_BarcodeResult.MeanLight, 2) + ":MeanLight", "", 80, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Overall_Quality[0] + ":Overall_Quality", "", 100, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Cell_Contrast[0] + ":Cell_Contrast", "", 120, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Print_Growth[0] + ":Print_Growth", "", 140, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Unused_Error_Correction[0] + ":Unused_Error_Correction", "", 160, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Cell_Modulation[0] + ":Cell_Modulation", "", 180, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Fixed_Pattern_Damage[0] + ":Fixed_Pattern_Damage", "", 200, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Grid_Nonuniformity[0] + ":Grid_Nonuniformity", "", 220, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Decode[0] + ":Decode", "", 240, 0, "green", "false");

                    HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                }
                else
                {
                    Result = -4;
                    set_display_font(hv_ExpDefaultWinHandle, 15, "mono", "true", "false");
                    ho_Image.DispObj(Window);
                    Window.SetColor("yellow");
                    ho_Rectangle.DispObj(Window);
                    Window.SetColor("green");
                    ho_SymbolXLDs.DispObj(Window);
                    disp_message(Window, "Barocde:" + m_BarcodeResult.Barcode, "", 0, 0, "green", "false");
                    disp_message(Window, "Barocde角度:" + m_BarcodeResult.BarcodeAngle, "", 20, 0, "green", "false");
                    disp_message(Window, "Barocde鏡像:" + m_BarcodeResult.hv_DecodedMirrored.S, "", 40, 0, "green", "false");
                    disp_message(Window, "NG4", "", 60, 0, "green", "false");
                    disp_message(Window, Math.Round(m_BarcodeResult.MeanLight, 2) + ":MeanLight", "", 80, 0, "green", "false");
                    disp_message(Window, m_BarcodeResult.Overall_Quality[0] + ":Overall_Quality", "", 100, 0, MyBarcodeReaderPlus.Overall_Quality > int.Parse(m_BarcodeResult.Overall_Quality[1].ToString().Substring(0, 1)) ? "red" : "green", "false");
                    disp_message(Window, m_BarcodeResult.Cell_Contrast[0] + ":Cell_Contrast", "", 120, 0, MyBarcodeReaderPlus.Cell_Contrast > int.Parse(m_BarcodeResult.Cell_Contrast[1].ToString().Substring(0, 1)) ? "red" : "green", "false");
                    disp_message(Window, m_BarcodeResult.Print_Growth[0] + ":Print_Growth", "", 140, 0, MyBarcodeReaderPlus.Print_Growth > int.Parse(m_BarcodeResult.Print_Growth[1].ToString().Substring(0, 1)) ? "red" : "green", "false");
                    disp_message(Window, m_BarcodeResult.Unused_Error_Correction[0] + ":Unused_Error_Correction", "", 160, 0, MyBarcodeReaderPlus.Unused_Error_Correction > int.Parse(m_BarcodeResult.Unused_Error_Correction[1].ToString().Substring(0, 1)) ? "red" : "green", "false");
                    disp_message(Window, m_BarcodeResult.Cell_Modulation[0] + ":Cell_Modulation", "", 180, 0, MyBarcodeReaderPlus.Cell_Modulation > int.Parse(m_BarcodeResult.Cell_Modulation[1].ToString().Substring(0, 1)) ? "red" : "green", "false");
                    disp_message(Window, m_BarcodeResult.Fixed_Pattern_Damage[0] + ":Fixed_Pattern_Damage", "", 200, 0, MyBarcodeReaderPlus.Fixed_Pattern_Damage > int.Parse(m_BarcodeResult.Fixed_Pattern_Damage[1].ToString().Substring(0, 1)) ? "red" : "green", "false");
                    disp_message(Window, m_BarcodeResult.Grid_Nonuniformity[0] + ":Grid_Nonuniformity", "", 220, 0, MyBarcodeReaderPlus.Grid_Nonuniformity > int.Parse(m_BarcodeResult.Grid_Nonuniformity[1].ToString().Substring(0, 1)) ? "red" : "green", "false");
                    disp_message(Window, m_BarcodeResult.Decode[0] + ":Decode", "", 240, 0, MyBarcodeReaderPlus.Decode > int.Parse(m_BarcodeResult.Decode[1].ToString().Substring(0, 1)) ? "red" : "green", "false");

                    HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                }
            }
            catch
            {
                Result = -4;
                set_display_font(hv_ExpDefaultWinHandle, 15, "mono", "true", "false");
                ho_Image.DispObj(Window);

                Window.SetColor("blue");
                ho_Rectangle.DispObj(Window);
                Window.SetColor("green");
                ho_SymbolXLDs.DispObj(Window);
                disp_message(Window, "NG4-未知原因", "", 50, 50, "red", "false");
                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
            }
#endregion
        }



        public void ReadBarcode(HObject ho_Image, out HObject ho_SymbolXLDs, out int iResult, out Vision.BarcodeResult m_BarcodeResult)
        {
            iResult = 0;
            m_BarcodeResult = new Vision.BarcodeResult();
            ho_SymbolXLDs = new HObject();
            HTuple hv_DataCodeHandle = new HTuple(), hv_ResultHandles = new HTuple(), hv_DecodedDataStrings = new HTuple();
            HTuple hv_Number = new HTuple(), hv_DecodedData = new HTuple();
            string _sBarcode = "";
            HOperatorSet.CreateDataCode2dModel("Data Matrix ECC 200", "default_parameters", "maximum_recognition", out hv_DataCodeHandle);
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "polarity", "any");
            //鏡向
            //HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "mirrored", "no");
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "timeout", 2000);
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "contrast_tolerance", "high");
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "small_modules_robustness", "high");
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "symbol_rows_min", 14);
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "symbol_rows_max", 18);

            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "module_size_min", 2);
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "module_size_max", 5);
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
                HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandles.TupleSelect(0), "mirrored", out m_BarcodeResult.hv_DecodedMirrored);
                HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandles.TupleSelect(0), "orientation", out m_BarcodeResult.hv_DecodedOrientation);
                if (MyBarcodeReader.Production == 0)
                {
                    m_BarcodeResult.Barcode = "";
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
                        m_BarcodeResult.Barcode = m_BarcodeResult.Barcode + _sBarcode;
                    }
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
                        m_BarcodeResult.Barcode = m_BarcodeResult.Barcode + _sBarcode;
                    }
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
                        m_BarcodeResult.Barcode = m_BarcodeResult.Barcode + _sBarcode;
                    }
                }

                #region 掃碼驗證
                HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandles.TupleSelect(0), "quality_isoiec_tr_29158", out hv_ResultValues1);
                HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandles.TupleSelect(0), "quality_isoiec_tr_29158_values", out hv_ResultValues2);
                m_BarcodeResult.MeanLight = hv_ResultValues1[8];
                try
                {
                    //等級轉換
                    m_BarcodeResult.Overall_Quality[0] = LevelConversion(hv_ResultValues1[0]);//整體質量
                    m_BarcodeResult.Cell_Contrast[0] = LevelConversion(hv_ResultValues1[1]);//单元格对比
                    m_BarcodeResult.Print_Growth[0] = LevelConversion(hv_ResultValues1[10]);//打印过粗
                    m_BarcodeResult.Unused_Error_Correction[0] = LevelConversion(hv_ResultValues1[7]);//未使用的错误校正
                    m_BarcodeResult.Cell_Modulation[0] = LevelConversion(hv_ResultValues1[2]);//单元格调制
                    m_BarcodeResult.Fixed_Pattern_Damage[0] = LevelConversion(hv_ResultValues1[3]);//固定图案损坏
                    m_BarcodeResult.Grid_Nonuniformity[0] = LevelConversion(hv_ResultValues1[6]);//轴向不均匀性
                    m_BarcodeResult.Decode[0] = LevelConversion(hv_ResultValues1[4]);//解码
                    //等級結果
                    m_BarcodeResult.Overall_Quality[1] = ((double)hv_ResultValues1[0]).ToString("f3");
                    m_BarcodeResult.Cell_Contrast[1] = Math.Round((double)hv_ResultValues1[1], 3).ToString("f3");
                    m_BarcodeResult.Print_Growth[1] = Math.Round((double)hv_ResultValues1[10], 3).ToString("f3");
                    m_BarcodeResult.Unused_Error_Correction[1] = ((double)hv_ResultValues1[7]).ToString("f3");
                    m_BarcodeResult.Cell_Modulation[1] = ((double)hv_ResultValues1[2]).ToString("f3");
                    m_BarcodeResult.Fixed_Pattern_Damage[1] = ((double)hv_ResultValues1[3]).ToString("f3");
                    m_BarcodeResult.Grid_Nonuniformity[1] = Math.Round((double)hv_ResultValues1[6], 3).ToString("f3");
                    m_BarcodeResult.Decode[1] = ((double)hv_ResultValues1[4]).ToString("f3");
                }
                catch
                {
                }
                #endregion

                HOperatorSet.ClearDataCode2dModel(hv_DataCodeHandle);
                iResult = 1;
            }
        }

        private void ucRegionErosion_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.RegionErosion = ucRegionErosion.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_ConnectedRegions = new HObject(), ho_SelectedRegions = new HObject();
            HTuple hv_Rect2_Len1 = new HTuple(), hv_Rect2_Len2 = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            int Result = 0;
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            try
            {
                GenBarcodeRegion(MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_ConnectedRegions);

                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, (new HTuple("rect2_len1")).TupleConcat("rect2_len2"), "and", (new HTuple(MyBarcodeReaderPlus.RegionRect2_Len1_Lower)).TupleConcat(MyBarcodeReaderPlus.RegionRect2_Len2_Lower), (new HTuple(MyBarcodeReaderPlus.RegionRect2_Len1_Upper)).TupleConcat(MyBarcodeReaderPlus.RegionRect2_Len2_Upper));
                HOperatorSet.RegionFeatures(ho_SelectedRegions, "area", out hv_Area);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions, out ExpTmpOutVar_0, "area", "and", hv_Area.TupleMax(), hv_Area.TupleMax() + 1);
                    ho_SelectedRegions.Dispose();
                    ho_SelectedRegions = ExpTmpOutVar_0;
                }

                //沒找到區域就跳出
                if (Result == 0)
                    return;

                //擬合矩形
                HOperatorSet.SmallestRectangle2(ho_SelectedRegions, out hv_Row, out hv_Column, out hv_Phi, out hv_Length1, out hv_Length2);
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2);
                {
                    HObject ExpTmpOutVar_0 = new HObject();
                    HOperatorSet.ErosionCircle(ho_Rectangle, out ExpTmpOutVar_0, MyBarcodeReaderPlus.RegionErosion);
                    ho_Rectangle.Dispose();
                    ho_Rectangle = ExpTmpOutVar_0.CopyObj(1, -1);
                    ExpTmpOutVar_0.Dispose();
                }
                HOperatorSet.SetColor(Window, "blue");
                HOperatorSet.SetDraw(Window, "margin");
                ho_Rectangle.DispObj(Window);
            }
            catch
            {
            }
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_Rectangle.Dispose();


        }

        private void cbMeasureSelect2_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.MeasureSelect2 = cbMeasureSelect2.SelectedIndex == 0 ? "first" : "last";
        }

        private void ucLength2_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.Length2 = ucLength2.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            int Result = 0;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject();
            try
            {
                FindCenter(Window, MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out ho_UsedEdges2, out ho_Contour2, out ho_ResultContours2, out MyBarcodeReaderPlus.hv_ResultRow, out MyBarcodeReaderPlus.hv_ResultColumn, out MyBarcodeReaderPlus.hv_ResultPhi);
                Window.ClearWindow();
                MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                ho_Contour2.DispObj(Window);
                Window.SetColor("blue");
                ho_ResultContours2.DispObj(Window);
                Window.SetColor("red");
                ho_UsedEdges2.DispObj(Window);
            }
            catch
            {
            }
        }

        private void ucBlack2White2_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.MeasureTransition2 = "positive";
            MyBarcodeReaderPlus.MeasureThreshold2 = ucBlack2White2.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            int Result = 0;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject();
            try
            {
                FindCenter(Window, MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out ho_UsedEdges2, out ho_Contour2, out ho_ResultContours2, out MyBarcodeReaderPlus.hv_ResultRow, out MyBarcodeReaderPlus.hv_ResultColumn, out MyBarcodeReaderPlus.hv_ResultPhi);
                Window.ClearWindow();
                MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                ho_Contour2.DispObj(Window);
                Window.SetColor("blue");
                ho_ResultContours2.DispObj(Window);
                Window.SetColor("red");
                ho_UsedEdges2.DispObj(Window);
            }
            catch
            {
            }
        }

        private void ucWhite2Black2_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.MeasureTransition2 = "negative";
            MyBarcodeReaderPlus.MeasureThreshold2 = ucWhite2Black2.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            int Result = 0;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject();
            try
            {
                FindCenter(Window, MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out ho_UsedEdges2, out ho_Contour2, out ho_ResultContours2, out MyBarcodeReaderPlus.hv_ResultRow, out MyBarcodeReaderPlus.hv_ResultColumn, out MyBarcodeReaderPlus.hv_ResultPhi);
                Window.ClearWindow();
                MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                ho_Contour2.DispObj(Window);
                Window.SetColor("blue");
                ho_ResultContours2.DispObj(Window);
                Window.SetColor("red");
                ho_UsedEdges2.DispObj(Window);
            }
            catch
            {
            }
        }

        private void btnSave5_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "MeasureSelect2", MyBarcodeReaderPlus.MeasureSelect2.ToString(), Path);
            IniFile.Write("Visual", "Length21", MyBarcodeReaderPlus.Length21.ToString(), Path);
            IniFile.Write("Visual", "Length22", MyBarcodeReaderPlus.Length22.ToString(), Path);
            IniFile.Write("Visual", "Phi2", MyBarcodeReaderPlus.Phi2.ToString(), Path);
            IniFile.Write("Visual", "Length2", MyBarcodeReaderPlus.Length2.ToString(), Path);
            IniFile.Write("Visual", "MeasureTransition2", MyBarcodeReaderPlus.MeasureTransition2.ToString(), Path);
            IniFile.Write("Visual", "MeasureThreshold2", MyBarcodeReaderPlus.MeasureThreshold2.ToString(), Path);
        }

        private void btnFindRectangle_Click(object sender, EventArgs e)
        {
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject();
            HObject ho_Region = new HObject(), ho_Region2 = new HObject(), ho_ContoursIntersection = new HObject();
            int Result = 0;
            try
            {
                FindCenter(Window, MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out ho_UsedEdges2, out ho_Contour2, out ho_ResultContours2, out MyBarcodeReaderPlus.hv_ResultRow, out MyBarcodeReaderPlus.hv_ResultColumn, out MyBarcodeReaderPlus.hv_ResultPhi);
                HOperatorSet.IntersectionClosedContoursXld(ho_ResultContours, ho_ResultContours2, out ho_ContoursIntersection);
            }
            catch
            {

            }
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_ContoursIntersection.DispObj(Window);
        }

        private void ucRegionDistance_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.RegionDistance = ucRegionDistance.Value;
            if (bReadPara)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_Region = new HObject();
            if (MyBarcodeReaderPlus.hv_ResultRow.D == 0 || MyBarcodeReaderPlus.hv_ResultColumn.D == 0)
                return;
            SetRegion(MyBarcodeReaderPlus.hv_ResultRow, MyBarcodeReaderPlus.hv_ResultColumn, MyBarcodeReaderPlus.hv_ResultPhi, out ho_Region);
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_Region.DispObj(Window);
        }

        private void ucRegionRotation_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.RegionRotation = ucRegionRotation.Value;
            if (bReadPara)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_Region = new HObject();
            if (MyBarcodeReaderPlus.hv_ResultRow.D == 0 || MyBarcodeReaderPlus.hv_ResultColumn.D == 0)
                return;
            SetRegion(MyBarcodeReaderPlus.hv_ResultRow, MyBarcodeReaderPlus.hv_ResultColumn, MyBarcodeReaderPlus.hv_ResultPhi, out ho_Region);
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_Region.DispObj(Window);
        }

        private void ucRegionLength1_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.RegionLength1 = ucRegionLength1.Value;
            if (bReadPara)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_Region = new HObject();
            if (MyBarcodeReaderPlus.hv_ResultRow.D == 0 || MyBarcodeReaderPlus.hv_ResultColumn.D == 0)
                return;
            SetRegion(MyBarcodeReaderPlus.hv_ResultRow, MyBarcodeReaderPlus.hv_ResultColumn, MyBarcodeReaderPlus.hv_ResultPhi, out ho_Region);
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_Region.DispObj(Window);
        }

        private void ucRegionLength2_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.RegionLength2 = ucRegionLength2.Value;
            if (bReadPara)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_Region = new HObject();
            if (MyBarcodeReaderPlus.hv_ResultRow.D == 0 || MyBarcodeReaderPlus.hv_ResultColumn.D == 0)
                return;
            SetRegion(MyBarcodeReaderPlus.hv_ResultRow, MyBarcodeReaderPlus.hv_ResultColumn, MyBarcodeReaderPlus.hv_ResultPhi, out ho_Region);
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_Region.DispObj(Window);
        }

        public void SetRegion(HTuple hv_ResultRow, HTuple hv_ResultColumn, HTuple hv_ResultPhi, out HObject ho_Region)
        {
            HObject ho_RegionRectangle1 = new HObject(), ho_RegionRectangle2 = new HObject();
            HTuple hv_RegionWidth1 = new HTuple(), hv_RegionWidth2 = new HTuple(), hv_RegionHeight1 = new HTuple(), hv_RegionHeight2 = new HTuple();
            HTuple hv_Angle = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_Region);
            try
            {
                hv_RegionWidth1 = MyBarcodeReaderPlus.RegionDistance * (((((new HTuple(MyBarcodeReaderPlus.RegionRotation + 90)).TupleRad()) - hv_ResultPhi)).TupleSin());
                hv_RegionHeight1 = MyBarcodeReaderPlus.RegionDistance * (((((new HTuple(MyBarcodeReaderPlus.RegionRotation + 90)).TupleRad()) - hv_ResultPhi)).TupleCos());
                hv_RegionWidth2 = MyBarcodeReaderPlus.RegionDistance * (((((new HTuple(MyBarcodeReaderPlus.RegionRotation + 270)).TupleRad()) - hv_ResultPhi)).TupleSin());
                hv_RegionHeight2 = MyBarcodeReaderPlus.RegionDistance * (((((new HTuple(MyBarcodeReaderPlus.RegionRotation + 270)).TupleRad()) - hv_ResultPhi)).TupleCos());
                HOperatorSet.GenRectangle2(out ho_RegionRectangle1, hv_ResultRow + hv_RegionHeight1,
          hv_ResultColumn - hv_RegionWidth1, hv_ResultPhi + ((new HTuple(90 - MyBarcodeReaderPlus.RegionRotation)).TupleRad()
          ), MyBarcodeReaderPlus.RegionLength1, MyBarcodeReaderPlus.RegionLength2);
                ho_RegionRectangle2.Dispose();
                HOperatorSet.GenRectangle2(out ho_RegionRectangle2, hv_ResultRow + hv_RegionHeight2,
                    hv_ResultColumn - hv_RegionWidth2, hv_ResultPhi + ((new HTuple(90 - MyBarcodeReaderPlus.RegionRotation)).TupleRad()
                    ), MyBarcodeReaderPlus.RegionLength1, MyBarcodeReaderPlus.RegionLength2);
                ho_Region.Dispose();
                HOperatorSet.Union2(ho_RegionRectangle1, ho_RegionRectangle2, out ho_Region);
            }
            catch
            {
            }
        }

        private void cmbRegionProjectSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.RegionProjectSet = tabRegionProjectSet.SelectedIndex = cmbRegionProjectSet.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Visual", "RegionProjectSet", MyBarcodeReaderPlus.RegionProjectSet.ToString(), Path);
        }

        private void btnSave6_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Visual", "RegionDistance", MyBarcodeReaderPlus.RegionDistance.ToString(), Path);
            IniFile.Write("Visual", "RegionRotation", MyBarcodeReaderPlus.RegionRotation.ToString(), Path);
            IniFile.Write("Visual", "RegionLength1", MyBarcodeReaderPlus.RegionLength1.ToString(), Path);
            IniFile.Write("Visual", "RegionLength2", MyBarcodeReaderPlus.RegionLength2.ToString(), Path);
        }

        private void tabRegionProjectSet_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (tabRegionProjectSet.SelectedIndex != cmbRegionProjectSet.SelectedIndex)
                tabRegionProjectSet.SelectedIndex = cmbRegionProjectSet.SelectedIndex;
        }

        private void cmbMirrored_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.Mirrored = cmbMirrored.SelectedIndex == 0 ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Visual", "Mirrored", MyBarcodeReaderPlus.Mirrored.ToString(), Path);
        }



        private void btnDrawModel_Click(object sender, EventArgs e)
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
            HObject ho_Circle = new HObject(), ho_ReducedImage = new HObject(), ho_ImageMedian = new HObject(), ho_ImageEmphasize = new HObject();
            HObject ho_Circle_Outer = new HObject(), ho_Circle_Inner = new HObject(), ho_Rectangle = new HObject(), ho_RegionIntersection = new HObject();
            HObject ho_RegionDifference = new HObject(), ho_Region = new HObject(), ho_ModelContours = new HObject(), ho_TransContours = new HObject();
            HObject ho_RegionUnion = new HObject();
            HObject ho_ModelRegion = new HObject();
            HTuple hv_Height = new HTuple(), hv_Width = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple(), hv_Radius = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_ModelID = new HTuple(), hv_NumLevels = new HTuple(), hv_AngleStart = new HTuple(), hv_AngleExtent = new HTuple(), hv_AngleStep = new HTuple();
            HTuple hv_ScaleMin = new HTuple(), hv_ScaleMax = new HTuple(), hv_ScaleStep = new HTuple(), hv_Metric = new HTuple(), hv_MinContrast = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
            HTuple hv_HomMat2D = new HTuple(), hv_RectangleRow = new HTuple(), hv_RectangleColumn = new HTuple(), hv_RectanglePhi = new HTuple(), hv_RectangleLength1 = new HTuple(), hv_RectangleLength2 = new HTuple();
            HTuple hv_ModelRegionArea = new HTuple(),hv_RefRow = new HTuple(), hv_RefColumn = new HTuple();
            HOperatorSet.SetDraw(Window, "margin");
            HOperatorSet.SetColor(Window, "green");
            try
            {
                ho_Image.Dispose();
                ho_Image = MyBarcodeReaderPlus.ho_ImagePart.CopyObj(1, -1);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, MyBarcodeReaderPlus.FirstRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 10, 10);
                ho_ImageEmphasize.Dispose();
                HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);
                Window.ClearWindow();
                ho_ImageEmphasize.DispObj(Window);
                disp_message(Window, "1.畫產品外圍", "", 0, 50, "green", "false");
                HOperatorSet.DrawCircle(Window, out hv_Row, out hv_Column, out hv_Radius);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Outer, hv_Row, hv_Column, hv_Radius);
                ho_Circle_Outer.DispObj(Window);
                disp_message(Window, "2.畫產品內圍", "", 20, 50, "green", "false");
                HOperatorSet.DrawCircle(Window, out hv_Row, out hv_Column, out hv_Radius);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Inner, hv_Row, hv_Column, hv_Radius);
                ho_Circle_Inner.DispObj(Window);
                //內外圓相減
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle_Outer, ho_Circle_Inner, out ho_RegionDifference);
                disp_message(Window, "3.畫產品方形", "", 40, 50, "green", "false");
                HOperatorSet.DrawRectangle2(Window, out hv_Row, out hv_Column, out hv_Phi, out hv_Length1, out hv_Length2);
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2);
                ho_RegionIntersection.Dispose();
                HOperatorSet.Intersection(ho_RegionDifference, ho_Rectangle, out ho_RegionIntersection);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageEmphasize, ho_RegionIntersection, out ho_ReducedImage);
                switch (MyBarcodeReaderPlus.ModelMode)
                {
                    case 0:
                        {
                            HOperatorSet.CreateShapeModel(ho_ReducedImage, "auto", new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), "auto", "auto", "use_polarity", "auto", "auto", out hv_ModelID);
                            HOperatorSet.GetShapeModelParams(hv_ModelID, out hv_NumLevels, out hv_AngleStart, out hv_AngleExtent, out hv_AngleStep, out hv_ScaleMin, out hv_ScaleMax, out hv_ScaleStep, out hv_Metric, out hv_MinContrast);
                            HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                            HOperatorSet.FindShapeModel(ho_ImageEmphasize, hv_ModelID, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), MyBarcodeReaderPlus.ModelGrade / 100, 1, 0.5, "least_squares", (new HTuple(6)).TupleConcat(1), 0.5, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
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
                                HOperatorSet.SmallestRectangle2(ho_RegionUnion, out hv_RectangleRow, out hv_RectangleColumn, out hv_RectanglePhi, out hv_RectangleLength1, out hv_RectangleLength2);
                                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);

                                Window.ClearWindow();
                                ho_Image.DispObj(Window);
                                Window.SetDraw("margin");
                                Window.SetColor("yellow");
                                ho_Rectangle.DispObj(Window);
                                disp_message(Window, "模組分數:" + Math.Round(hv_Score.D * 100, 0), "", 0, 0, "green", "false");
                                if (MessageBox.Show("是否儲存模組?", "模組設定", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                                {
                                    string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model";
                                    HOperatorSet.WriteShapeModel(hv_ModelID, Path);
                                    if (MyBarcodeReaderPlus.hv_ModelID.Length > 0)
                                    {
                                        HOperatorSet.ClearShapeModel(MyBarcodeReaderPlus.hv_ModelID);
                                        MyBarcodeReaderPlus.hv_ModelID = hv_ModelID;
                                    }
                                }
                                //HOperatorSet.ClearShapeModel(hv_ModelID);
                            }

                        } break;
                    case 1:
                        {
                            HOperatorSet.CreateNccModel(ho_ReducedImage, "auto", new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), "auto","use_polarity", out hv_ModelID);
                            ho_ModelRegion.Dispose();
                            HOperatorSet.GetNccModelRegion(out ho_ModelRegion, hv_ModelID);
                            HOperatorSet.AreaCenter(ho_ModelRegion,out hv_ModelRegionArea, out hv_RefRow, out hv_RefColumn);
                            HOperatorSet.FindNccModel(ho_ImageEmphasize, hv_ModelID, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), MyBarcodeReaderPlus.ModelGrade / 100, 1, 0.5, "true", (new HTuple(6)).TupleConcat(1), out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                            if (hv_Score.TupleGreater(0) != 0)
                            {
                                HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                                HOperatorSet.HomMat2dTranslate(hv_HomMat2D, -hv_RefRow, -hv_RefColumn, out hv_HomMat2D);
                                HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle, 0, 0, out hv_HomMat2D);
                                HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row, hv_Column, out hv_HomMat2D);
                                ho_TransContours.Dispose();
                                HOperatorSet.AffineTransRegion(ho_ModelRegion, out ho_Region, hv_HomMat2D, "nearest_neighbor");
                                HOperatorSet.SmallestRectangle2(ho_Region, out hv_RectangleRow, out hv_RectangleColumn, out hv_RectanglePhi, out hv_RectangleLength1, out hv_RectangleLength2);
                                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);

                                Window.ClearWindow();
                                ho_Image.DispObj(Window);
                                Window.SetColor("yellow");
                                Window.SetDraw("margin");
                                ho_Rectangle.DispObj(Window);
                                disp_message(Window, "模組分數:" + Math.Round(hv_Score.D * 100, 0), "", 0, 0, "green", "false");
                                if (MessageBox.Show("是否儲存模組?", "模組設定", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                                {
                                    string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model";
                                    HOperatorSet.WriteNccModel(hv_ModelID, Path);
                                    if (MyBarcodeReaderPlus.hv_ModelID.Length > 0)
                                    {
                                        HOperatorSet.ClearNccModel(MyBarcodeReaderPlus.hv_ModelID);
                                        MyBarcodeReaderPlus.hv_ModelID = hv_ModelID;
                                    }
                                }
                                //HOperatorSet.ClearNccModel(hv_ModelID);
                            }

                        } break;
                }
                
            }
            catch
            {
            }
            bDrawing = false;
        }

        private void btnFindModel_Click(object sender, EventArgs e)
        {
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_Circle = new HObject(), ho_ReducedImage = new HObject(), ho_ImageMedian = new HObject();
            HObject ho_ImageEmphasize = new HObject(), ho_ModelContours = new HObject(), ho_TransContours = new HObject();
            HObject ho_RegionUnion = new HObject();
            HObject ho_ModelRegion = new HObject();
            HTuple hv_ModelID = new HTuple(), hv_HomMat2D = new HTuple();
            HTuple hv_Height = new HTuple(), hv_Width = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple(), hv_Radius = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
            HTuple hv_RectangleRow = new HTuple(), hv_RectangleColumn = new HTuple(), hv_RectanglePhi = new HTuple(), hv_RectangleLength1 = new HTuple(), hv_RectangleLength2 = new HTuple();
            HTuple hv_ModelRegionArea = new HTuple(), hv_RefRow = new HTuple(), hv_RefColumn = new HTuple();
            try
            {
                //string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model";
                //HOperatorSet.ReadShapeModel(Path, out hv_ModelID);
                if (MyBarcodeReaderPlus.hv_ModelID.Length > 0)
                    hv_ModelID = MyBarcodeReaderPlus.hv_ModelID;
                else
                {
                    MessageBox.Show("請建立初始模組");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("請建立初始模組");
                return;
            }
            ho_Image.Dispose();
            ho_Image = MyBarcodeReaderPlus.ho_ImagePart.CopyObj(1, -1);
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
            ho_Circle.Dispose();
            HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, MyBarcodeReaderPlus.FirstRadius);
            ho_ReducedImage.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
            ho_ImageMedian.Dispose();
            HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 10, 10);
            ho_ImageEmphasize.Dispose();
            HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);
            try
            {
                switch (MyBarcodeReaderPlus.ModelMode)
                {
                    case 0:
                        {
                            HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                            HOperatorSet.FindShapeModel(ho_ImageEmphasize, hv_ModelID, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), MyBarcodeReaderPlus.ModelGrade / 100, 0, 0.5, "least_squares", (new HTuple(6)).TupleConcat(1), 0.5, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
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
                                HOperatorSet.SmallestRectangle2(ho_RegionUnion, out hv_RectangleRow, out hv_RectangleColumn, out hv_RectanglePhi, out hv_RectangleLength1, out hv_RectangleLength2);
                                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);

                                Window.ClearWindow();
                                ho_Image.DispObj(Window);
                                Window.SetColor("yellow");
                                Window.SetDraw("margin");
                                ho_Rectangle.DispObj(Window);
                                disp_message(Window, "模組分數:" + Math.Round(hv_Score.D * 100, 0), "", 0, 0, "green", "false");
                                MyBarcodeReaderPlus.hv_FirstRow = hv_Row;
                                MyBarcodeReaderPlus.hv_FirstColumn = hv_Column;
                            }
                            //HOperatorSet.ClearShapeModel(hv_ModelID);
                        } break;
                    case 1:
                        {
                            HOperatorSet.GetNccModelRegion(out ho_ModelRegion, hv_ModelID);
                            HOperatorSet.AreaCenter(ho_ModelRegion, out hv_ModelRegionArea, out hv_RefRow, out hv_RefColumn);
                            HOperatorSet.FindNccModel(ho_ImageEmphasize, hv_ModelID, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), MyBarcodeReaderPlus.ModelGrade / 100, 1, 0.5, "true", (new HTuple(6)).TupleConcat(1), out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                            if (hv_Score.TupleGreater(0) != 0)
                            {
                                HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                                HOperatorSet.HomMat2dTranslate(hv_HomMat2D, -hv_RefRow, -hv_RefColumn, out hv_HomMat2D);
                                HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle, 0, 0, out hv_HomMat2D);
                                HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row, hv_Column, out hv_HomMat2D);
                                ho_TransContours.Dispose();
                                HOperatorSet.AffineTransRegion(ho_ModelRegion, out ho_Region, hv_HomMat2D, "nearest_neighbor");
                                HOperatorSet.SmallestRectangle2(ho_Region, out hv_RectangleRow, out hv_RectangleColumn, out hv_RectanglePhi, out hv_RectangleLength1, out hv_RectangleLength2);
                                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);

                                Window.ClearWindow();
                                ho_Image.DispObj(Window);
                                Window.SetColor("yellow");
                                Window.SetDraw("margin");
                                ho_Rectangle.DispObj(Window);
                                disp_message(Window, "模組分數:" + Math.Round(hv_Score.D * 100, 0), "", 0, 0, "green", "false");
                               
                                //HOperatorSet.ClearNccModel(hv_ModelID);
                            } break;
                        }
                }
            }
            catch
            {
            }
        }

        private void nudBarcodeAngleSet_ValueChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.BarcodeAngleSet = (int)nudBarcodeAngleSet.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "BarcodeAngleSet", MyBarcodeReaderPlus.BarcodeAngleSet.ToString(), Path);
        }

        private void nudAllowableOffsetAngle_ValueChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.AllowableOffsetAngle = (double)nudAllowableOffsetAngle.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "AllowableOffsetAngle", MyBarcodeReaderPlus.AllowableOffsetAngle.ToString(), Path);
        }

        private void btnFindModel_Miss_Click(object sender, EventArgs e)
        {
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_Circle = new HObject(), ho_ReducedImage = new HObject(), ho_ImageMedian = new HObject();
            HObject ho_ImageEmphasize = new HObject(), ho_ModelContours = new HObject(), ho_TransContours = new HObject();
            HObject ho_RegionUnion = new HObject();
            HTuple hv_ModelID = new HTuple(), hv_HomMat2D = new HTuple();
            HTuple hv_Height = new HTuple(), hv_Width = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple(), hv_Radius = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
            HTuple hv_CircleRow = new HTuple(), hv_CircleColumn = new HTuple(), hv_CircleRadius = new HTuple();
            try
            {
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model_Miss";
                HOperatorSet.ReadShapeModel(Path, out hv_ModelID);
            }
            catch
            {
                MessageBox.Show("先建立無料模組!");
            }
            ho_Image.Dispose();
            ho_Image = MyBarcodeReaderPlus.ho_ImagePart.CopyObj(1, -1);
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
            ho_Circle.Dispose();
            HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, MyBarcodeReaderPlus.FirstRadius);
            ho_ReducedImage.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
            ho_ImageMedian.Dispose();
            HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 10, 10);
            ho_ImageEmphasize.Dispose();
            HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);
            try
            {
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                HOperatorSet.FindShapeModel(ho_ImageEmphasize, hv_ModelID, (new HTuple(-30)).TupleRad(), (new HTuple(30)).TupleRad(), 0.5, 0, 0.5, "least_squares", (new HTuple(6)).TupleConcat(1), 0.75, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
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
                    HOperatorSet.SmallestCircle(ho_RegionUnion, out hv_CircleRow, out hv_CircleColumn, out hv_CircleRadius);
                    HOperatorSet.GenCircle(out ho_Circle, hv_CircleRow, hv_CircleColumn, hv_CircleRadius);
                    HOperatorSet.SetDraw(Window, "margin");
                    Window.ClearWindow();
                    ho_Image.DispObj(Window);
                    Window.SetColor("yellow");
                    ho_Circle.DispObj(Window);
                    disp_message(Window, "模組分數:" + Math.Round(hv_Score.D * 100, 0), "", 0, 0, "green", "false");
                    disp_message(Window, "Miss", "", 50, 0, "blue", "false");
                }
                HOperatorSet.ClearShapeModel(hv_ModelID);
            }
            catch
            {

            }
        }

        private void btnDrawModel_Miss_Click(object sender, EventArgs e)
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
            HObject ho_Circle = new HObject(), ho_ReducedImage = new HObject(), ho_ImageMedian = new HObject(), ho_ImageEmphasize = new HObject();
            HObject ho_Circle_Outer = new HObject(), ho_Circle_Inner = new HObject(), ho_Rectangle = new HObject(), ho_RegionIntersection = new HObject();
            HObject ho_RegionDifference = new HObject(), ho_Region = new HObject(), ho_ModelContours = new HObject(), ho_TransContours = new HObject();
            HObject ho_RegionUnion = new HObject();
            HTuple hv_Height = new HTuple(), hv_Width = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple(), hv_Radius = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_ModelID = new HTuple(), hv_NumLevels = new HTuple(), hv_AngleStart = new HTuple(), hv_AngleExtent = new HTuple(), hv_AngleStep = new HTuple();
            HTuple hv_ScaleMin = new HTuple(), hv_ScaleMax = new HTuple(), hv_ScaleStep = new HTuple(), hv_Metric = new HTuple(), hv_MinContrast = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
            HTuple hv_HomMat2D = new HTuple(), hv_CircleRow = new HTuple(), hv_CircleColumn = new HTuple(), hv_CircleRadius = new HTuple();
            HOperatorSet.SetDraw(Window, "margin");
            HOperatorSet.SetColor(Window, "green");
            try
            {
                ho_Image.Dispose();
                ho_Image = MyBarcodeReaderPlus.ho_ImagePart.CopyObj(1, -1);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, MyBarcodeReaderPlus.FirstRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 10, 10);
                ho_ImageEmphasize.Dispose();
                HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);
                Window.ClearWindow();
                ho_ImageEmphasize.DispObj(Window);
                disp_message(Window, "1.畫產品外圍", "", 0, 50, "green", "false");
                HOperatorSet.DrawCircle(Window, out hv_Row, out hv_Column, out hv_Radius);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Outer, hv_Row, hv_Column, hv_Radius);
                ho_Circle_Outer.DispObj(Window);
                disp_message(Window, "2.畫產品內圍", "", 20, 50, "green", "false");
                HOperatorSet.DrawCircle(Window, out hv_Row, out hv_Column, out hv_Radius);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Inner, hv_Row, hv_Column, hv_Radius);
                ho_Circle_Inner.DispObj(Window);
                //內外圓相減
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle_Outer, ho_Circle_Inner, out ho_RegionDifference);
                HOperatorSet.ReduceDomain(ho_ImageEmphasize, ho_RegionDifference, out ho_ReducedImage);
                HOperatorSet.CreateShapeModel(ho_ReducedImage, "auto", new HTuple(-30).TupleRad(), new HTuple(30).TupleRad(), "auto", "auto", "use_polarity", "auto", "auto", out hv_ModelID);
                HOperatorSet.GetShapeModelParams(hv_ModelID, out hv_NumLevels, out hv_AngleStart, out hv_AngleExtent, out hv_AngleStep, out hv_ScaleMin, out hv_ScaleMax, out hv_ScaleStep, out hv_Metric, out hv_MinContrast);
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                HOperatorSet.FindShapeModel(ho_ImageEmphasize, hv_ModelID, (new HTuple(-30)).TupleRad(), (new HTuple(30)).TupleRad(), 0.5, 0, 0.5, "least_squares", (new HTuple(6)).TupleConcat(1), 0.75, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
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
                    HOperatorSet.SmallestCircle(ho_RegionUnion, out hv_CircleRow, out hv_CircleColumn, out hv_CircleRadius);
                    HOperatorSet.GenCircle(out ho_Circle, hv_CircleRow, hv_CircleColumn, hv_CircleRadius);

                    Window.ClearWindow();
                    ho_Image.DispObj(Window);
                    Window.SetColor("yellow");
                    ho_Circle.DispObj(Window);
                    disp_message(Window, "模組分數:" + Math.Round(hv_Score.D * 100, 0), "", 0, 0, "green", "false");
                    if (MessageBox.Show("是否儲存模組?", "模組設定", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model_Miss";
                        HOperatorSet.WriteShapeModel(hv_ModelID, Path);
                    }
                    HOperatorSet.ClearShapeModel(hv_ModelID);
                }
            }
            catch
            {
            }
            bDrawing = false;
        }
      
        private void btnDrawRegion_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_Rectangle = new HObject();
            HOperatorSet.SetColor(Window, "red");
            //找出初始半徑
            HOperatorSet.DrawRectangle1(Window, out MyBarcodeReaderPlus.RegionRow1, out MyBarcodeReaderPlus.RegionColumn1, out MyBarcodeReaderPlus.RegionRow2, out MyBarcodeReaderPlus.RegionColumn2);
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, MyBarcodeReaderPlus.RegionRow1, MyBarcodeReaderPlus.RegionColumn1, MyBarcodeReaderPlus.RegionRow2, MyBarcodeReaderPlus.RegionColumn2);
            My.ho_Image.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_Rectangle.DispObj(Window);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "RegionRow1", MyBarcodeReaderPlus.RegionRow1.ToString(), Path);
            IniFile.Write("Setting", "RegionColumn1", MyBarcodeReaderPlus.RegionColumn1.ToString(), Path);
            IniFile.Write("Setting", "RegionRow2", MyBarcodeReaderPlus.RegionRow2.ToString(), Path);
            IniFile.Write("Setting", "RegionColumn2", MyBarcodeReaderPlus.RegionColumn2.ToString(), Path);
        }

        private void cmbOverall_Quality_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.Overall_Quality = cmbOverall_Quality.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "Overall_Quality", MyBarcodeReaderPlus.Overall_Quality.ToString(), Path);
        }

        private void cmbCell_Contrast_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.Cell_Contrast = cmbCell_Contrast.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "Cell_Contrast", MyBarcodeReaderPlus.Cell_Contrast.ToString(), Path);
        }

        private void cmbPrint_Growth_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.Print_Growth = cmbPrint_Growth.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "Print_Growth", MyBarcodeReaderPlus.Print_Growth.ToString(), Path);
        }

        private void cmbUnused_Error_Correction_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.Unused_Error_Correction = cmbUnused_Error_Correction.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "Unused_Error_Correction", MyBarcodeReaderPlus.Unused_Error_Correction.ToString(), Path);
        }

        private void cmbCell_Modulation_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.Cell_Modulation = cmbCell_Modulation.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "Cell_Modulation", MyBarcodeReaderPlus.Cell_Modulation.ToString(), Path);
        }

        private void cmbFixed_Pattern_Damage_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.Fixed_Pattern_Damage = cmbFixed_Pattern_Damage.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "Fixed_Pattern_Damage", MyBarcodeReaderPlus.Fixed_Pattern_Damage.ToString(), Path);
        }

        private void cmbGrid_Nonuniformity_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.Grid_Nonuniformity = cmbGrid_Nonuniformity.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "Grid_Nonuniformity", MyBarcodeReaderPlus.Grid_Nonuniformity.ToString(), Path);
        }

        private void cmbDecode_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.Decode = cmbDecode.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "Decode", MyBarcodeReaderPlus.Decode.ToString(), Path);
        }

        private void nudMode_Angle_ValueChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.Mode_Angle = tabMode_Angle.SelectedIndex = (int)nudMode_Angle.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Angle", "Mode", MyBarcodeReaderPlus.Mode_Angle.ToString(), Path);
        }

        private void tabMode_Angle_SelectedIndexChanged(object sender, EventArgs e)
        {
            nudMode_Angle.Value = tabMode_Angle.SelectedIndex;
        }



        private void btnFindCenter_Angle1_Click(object sender, EventArgs e)
        {
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_ConnectedRegions = new HObject(), ho_SelectedRegions = new HObject(), ho_Rectangle = new HObject();
            HTuple hv_Rect2_Len1 = new HTuple(), hv_Rect2_Len2 = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_RegionRect2_Row = new HTuple(), hv_RegionRect2_Column = new HTuple(), hv_RegionRect2_Phi = new HTuple(), hv_RegionRect2_Length1 = new HTuple(), hv_RegionRect2_Length2 = new HTuple();
            int Result = 0;
            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            try
            {
                GenBarcodeRegion(MyBarcodeReaderPlus.ho_ImagePart, out Result, out ho_ConnectedRegions);

                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, (new HTuple("rect2_len1")).TupleConcat("rect2_len2"), "and", (new HTuple(MyBarcodeReaderPlus.RegionRect2_Len1_Lower)).TupleConcat(MyBarcodeReaderPlus.RegionRect2_Len2_Lower), (new HTuple(MyBarcodeReaderPlus.RegionRect2_Len1_Upper)).TupleConcat(MyBarcodeReaderPlus.RegionRect2_Len2_Upper));
                HOperatorSet.RegionFeatures(ho_SelectedRegions, "area", out hv_Area);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions, out ExpTmpOutVar_0, "area", "and", hv_Area.TupleMax(), hv_Area.TupleMax() + 1);
                    ho_SelectedRegions.Dispose();
                    ho_SelectedRegions = ExpTmpOutVar_0;
                }
                //沒找到區域就跳出
                if (Result == 0)
                    return;
                HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
                HOperatorSet.SmallestRectangle2(ho_SelectedRegions, out hv_RegionRect2_Row, out hv_RegionRect2_Column, out hv_RegionRect2_Phi, out hv_RegionRect2_Length1, out hv_RegionRect2_Length2);

                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RegionRect2_Row, hv_RegionRect2_Column, hv_RegionRect2_Phi, hv_RegionRect2_Length1, hv_RegionRect2_Length2);

                Window.SetColor("blue");
                Window.SetDraw("margin");
                ho_Rectangle.DispObj(Window);
                MyBarcodeReaderPlus.RegionRect2_Row = hv_RegionRect2_Row;
                MyBarcodeReaderPlus.RegionRect2_Column = hv_RegionRect2_Column;
            }
            catch
            {
            }
        }

        private void btnDrawRectangle_Angle1_Click(object sender, EventArgs e)
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
            HObject ho_Rectangle = new HObject();
            HTuple Row = new HTuple(), Column = new HTuple(), Phi = new HTuple(), Length1 = new HTuple(), Length2 = new HTuple();

            try
            {
                Window = hWindowControl1.HalconWindow;
                if (MyBarcodeReaderPlus.ho_ImagePart == null)
                    return;
                Window.ClearWindow();
                MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
                HOperatorSet.SetColor(Window, "yellow");
                //畫檢視範圍
                HOperatorSet.DrawRectangle2(Window, out Row, out Column, out Phi, out Length1, out Length2);

                HOperatorSet.GenRectangle2(out ho_Rectangle, Row, Column, Phi, Length1, Length2);

                HOperatorSet.SetDraw(Window, "margin");
                MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
                ho_Rectangle.DispObj(Window);

                if (Length1 > Length2)
                {
                    MyBarcodeReaderPlus.Length1_Angle1 = Length1;
                    MyBarcodeReaderPlus.Length2_Angle1 = Length2;
                    MyBarcodeReaderPlus.Phi_Angle1 = Phi;
                }
                else
                {
                    MyBarcodeReaderPlus.Length1_Angle1 = Length2;
                    MyBarcodeReaderPlus.Length2_Angle1 = Length1;
                    MyBarcodeReaderPlus.Phi_Angle1 = Phi - (new HTuple(90)).TupleRad();
                }
            }
            catch
            {
            }
            bDrawing = false;
        }

        private void cmbMeasureSelect_Angle1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.MeasureSelect_Angle1 = cmbMeasureSelect_Angle1.SelectedIndex;
        }

        private void ucLength_Angle1_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.Length_Angle1 = ucLength_Angle1.Value;
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            if (MyBarcodeReaderPlus.RegionRect2_Row == null)
            {
                MessageBox.Show("沒有Barcode位置");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HTuple hv_ResultPhi = new HTuple();
            ho_UsedEdges.Dispose();ho_Contour.Dispose();ho_ResultContours.Dispose();
            FindRectangleCenter_Angle1(MyBarcodeReaderPlus.ho_ImagePart, MyBarcodeReaderPlus.RegionRect2_Row, MyBarcodeReaderPlus.RegionRect2_Column,
                out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out hv_ResultPhi);

            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_Contour.DispObj(Window);
            Window.SetColor("blue");
            ho_ResultContours.DispObj(Window);
            Window.SetColor("red");
            ho_UsedEdges.DispObj(Window);
        }

        private void ucBlack2White_Angle1_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.MeasureThreshold_Angle1 = ucBlack2White_Angle1.Value;
            MyBarcodeReaderPlus.MeasureTransition_Angle1 = "positive";
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            if (MyBarcodeReaderPlus.RegionRect2_Row == null)
            {
                MessageBox.Show("沒有Barcode位置");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HTuple hv_ResultPhi = new HTuple();
            ho_UsedEdges.Dispose(); ho_Contour.Dispose(); ho_ResultContours.Dispose();
            FindRectangleCenter_Angle1(MyBarcodeReaderPlus.ho_ImagePart, MyBarcodeReaderPlus.RegionRect2_Row, MyBarcodeReaderPlus.RegionRect2_Column,
                out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out hv_ResultPhi);

            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_Contour.DispObj(Window);
            Window.SetColor("blue");
            ho_ResultContours.DispObj(Window);
            Window.SetColor("red");
            ho_UsedEdges.DispObj(Window);
        }

        private void ucWhite2Black2_Angle1_ValueChanged(int CurrentValue)
        {
            MyBarcodeReaderPlus.MeasureThreshold_Angle1 = ucBlack2White_Angle1.Value;
            MyBarcodeReaderPlus.MeasureTransition_Angle1 = "negative";
            if (bReadPara)
                return;
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            if (MyBarcodeReaderPlus.RegionRect2_Row == null)
            {
                MessageBox.Show("沒有Barcode位置");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HTuple hv_ResultPhi = new HTuple();
            ho_UsedEdges.Dispose(); ho_Contour.Dispose(); ho_ResultContours.Dispose();
            FindRectangleCenter_Angle1(MyBarcodeReaderPlus.ho_ImagePart, MyBarcodeReaderPlus.RegionRect2_Row, MyBarcodeReaderPlus.RegionRect2_Column,
                out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out hv_ResultPhi);

            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_Contour.DispObj(Window);
            Window.SetColor("blue");
            ho_ResultContours.DispObj(Window);
            Window.SetColor("red");
            ho_UsedEdges.DispObj(Window);
        }

        private void btnFindRectangle_Angle1_Click(object sender, EventArgs e)
        {
            if (MyBarcodeReaderPlus.ho_ImagePart == null)
                return;
            if (MyBarcodeReaderPlus.RegionRect2_Row == null)
            {
                MessageBox.Show("沒有Barcode位置");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HTuple hv_ResultPhi = new HTuple();
            ho_UsedEdges.Dispose(); ho_Contour.Dispose(); ho_ResultContours.Dispose();
            FindRectangleCenter_Angle1(MyBarcodeReaderPlus.ho_ImagePart, MyBarcodeReaderPlus.RegionRect2_Row, MyBarcodeReaderPlus.RegionRect2_Column,
                out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out hv_ResultPhi);

            Window.ClearWindow();
            MyBarcodeReaderPlus.ho_ImagePart.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_ResultContours.DispObj(Window);

            MyBarcodeReaderPlus.hv_ResultPhi = hv_ResultPhi;
        }

        private void FindRectangleCenter_Angle1(HObject ho_Image,HTuple hv_Row,HTuple hv_Column,out HObject ho_UsedEdges,out HObject ho_Contour,out HObject ho_ResultContours,out HTuple hv_ResultPhi)
        {
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HTuple hv_ResultRow = new HTuple(), hv_ResultColumn = new HTuple(),hv_ResultLength1 = new HTuple(), hv_ResultLength2 = new HTuple();

            hv_ResultPhi = 0;
            HTuple hv_Phi = MyBarcodeReaderPlus.Phi_Angle1;
            HTuple hv_Length1 = MyBarcodeReaderPlus.Length1_Angle1;
            HTuple hv_Length2 = MyBarcodeReaderPlus.Length2_Angle1;

            HTuple hv_MeasureSelect = MyBarcodeReaderPlus.MeasureSelect_Angle1 == 0 ? "first" : "last";
            HTuple hv_Length = MyBarcodeReaderPlus.Length_Angle1;
            HTuple hv_MeasureThreshold = MyBarcodeReaderPlus.MeasureThreshold_Angle1;
            HTuple hv_MeasureTransition = MyBarcodeReaderPlus.MeasureTransition_Angle1;
            try
            {
                ho_Contour.Dispose(); ho_UsedEdges.Dispose(); ho_ResultContours.Dispose();
                gen_rectangle2_center(ho_Image, out ho_Contour, out ho_UsedEdges,
                    out ho_ResultContours, hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2,
                    hv_Length, hv_MeasureThreshold, hv_MeasureTransition, hv_MeasureSelect, out hv_ResultRow, out hv_ResultColumn, out hv_ResultPhi,
                    out hv_ResultLength1, out hv_ResultLength2);
            }
            catch
            {
            }
        }

        private void btnSave_Angle1_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Angle1", "Length1", MyBarcodeReaderPlus.Length1_Angle1.D.ToString(), Path);
            IniFile.Write("Angle1", "Length2", MyBarcodeReaderPlus.Length2_Angle1.D.ToString(), Path);
            IniFile.Write("Angle1", "Phi", MyBarcodeReaderPlus.Phi_Angle1.D.ToString(), Path);
            IniFile.Write("Angle1", "Length", MyBarcodeReaderPlus.Length_Angle1.D.ToString(), Path);
            IniFile.Write("Angle1", "MeasureSelect", MyBarcodeReaderPlus.MeasureSelect_Angle1.ToString(), Path);
            IniFile.Write("Angle1", "MeasureThreshold", MyBarcodeReaderPlus.MeasureThreshold_Angle1.D.ToString(), Path);
            IniFile.Write("Angle1", "MeasureTransition", MyBarcodeReaderPlus.MeasureTransition_Angle1.S, Path);
        }

        private void nudModelGrade_ValueChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.ModelGrade = (double)nudModelGrade.Value;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "ModelGrade", MyBarcodeReaderPlus.ModelGrade.ToString(), Path);
        }

        private void cmbModelMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.ModelMode = cmbModelMode.SelectedIndex;
            if (bReadPara)
                return;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "ModelMode", MyBarcodeReaderPlus.ModelMode.ToString(), Path);
            string message = "";
            switch (MyBarcodeReaderPlus.ModelMode)
            {
                case 0:
                    {
                        message = "形狀模式: /r/t優點:較節省電腦記憶體,建立模組速度快,/r/t缺點:邊緣不清楚時,容易找不到模組或是模組找偏的現象";
                    }break;
                case 1:
                    {
                        message = "互相關模式: /r/t優點:邊緣不清楚時模組匹配效果較好, /r/t缺點:消耗較多電腦記憶體,建立模組需較多時間,請耐心等待";
                    }break;
            }
            MessageBox.Show(message);
        }

        private void nudAllowableOffsetAngle_L_ValueChanged(object sender, EventArgs e)
        {
            MyBarcodeReaderPlus.AllowableOffsetAngle_L = (double)nudAllowableOffsetAngle_L.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Visual", "AllowableOffsetAngle_L", MyBarcodeReaderPlus.AllowableOffsetAngle_L.ToString(), Path);
        }
    }
}
