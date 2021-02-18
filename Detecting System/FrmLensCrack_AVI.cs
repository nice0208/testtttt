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
    public partial class FrmLensCrack_AVI : Form
    {
        FrmParent parent;
        public FrmLensCrack_AVI(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;
            InitializeComponent();
        }
        public bool bReadingPara = false;
        public bool bDrawing = false;

        #region Halcon算子
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

        public void m_polor_trans_Masking_rft_dyn_threshold(HObject ho_Image, HObject ho_RegionMasking,
     out HObject ho_XYTransRegion, HTuple hv_Polor_Row, HTuple hv_Polor_Column, HTuple hv_Polor_OuterRadius,
     HTuple hv_Polor_InnerRadius, HTuple hv_rft_Sigma1, HTuple hv_rft_Sigma2, HTuple hv_Mean_MaskWidth1,
     HTuple hv_Mean_MaskWidth2, HTuple hv_dyn_Dark_Offset, HTuple hv_dyn_Light_Offset)
        {
            // Local iconic variables 

            HObject ho_PolarTransImage, ho_PolarTransRegion;
            HObject ho_RegionDilation, ho_ConnectedRegions, ho_Rectangle_Cutting = null;
            HObject ho_Rectangle_Cutting_1 = null, ho_Rectangle_Cutting_2 = null;
            HObject ho_RegionDifference, ho_ImageFFT, ho_ImageGauss;
            HObject ho_ImageConvol, ho_ImageFFT1, ho_ImageSub, ho_ImageMean1;
            HObject ho_ImageMean2, ho_RegionDynThresh_Dark, ho_RegionDynThresh_Light;
            HObject ho_RegionUnion;

            // Local control variables 
            HTuple hv_ImageWidth = new HTuple(), hv_ImageHeight = new HTuple();
            HTuple hv_pi = null, hv_Width = null, hv_Height = null;
            HTuple hv_Row1 = null, hv_Column1 = null, hv_Row2 = null;
            HTuple hv_Column2 = null, hv_Length = null, hv_Rectangle_Cutting_Column = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_XYTransRegion);
            HOperatorSet.GenEmptyObj(out ho_PolarTransImage);
            HOperatorSet.GenEmptyObj(out ho_PolarTransRegion);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_Rectangle_Cutting);
            HOperatorSet.GenEmptyObj(out ho_Rectangle_Cutting_1);
            HOperatorSet.GenEmptyObj(out ho_Rectangle_Cutting_2);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ImageFFT);
            HOperatorSet.GenEmptyObj(out ho_ImageGauss);
            HOperatorSet.GenEmptyObj(out ho_ImageConvol);
            HOperatorSet.GenEmptyObj(out ho_ImageFFT1);
            HOperatorSet.GenEmptyObj(out ho_ImageSub);
            HOperatorSet.GenEmptyObj(out ho_ImageMean1);
            HOperatorSet.GenEmptyObj(out ho_ImageMean2);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThresh_Dark);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThresh_Light);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            hv_pi = ((new HTuple(0.0)).TupleAcos()) * 2;
            hv_Width = (hv_Polor_OuterRadius + hv_Polor_InnerRadius) * hv_pi;
            hv_Height = hv_Polor_OuterRadius - hv_Polor_InnerRadius;
            //檢測區域轉為極座標
            HOperatorSet.GetImageSize(ho_Image, out hv_ImageWidth, out hv_ImageHeight);
            ho_PolarTransImage.Dispose();
            HOperatorSet.PolarTransImageExt(ho_Image, out ho_PolarTransImage, hv_Polor_Row,
                hv_Polor_Column, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad()
                , hv_Polor_InnerRadius, hv_Polor_OuterRadius, hv_Width, hv_Height, "nearest_neighbor");
            //非檢區轉為極座標
            ho_PolarTransRegion.Dispose();
            HOperatorSet.PolarTransRegion(ho_RegionMasking, out ho_PolarTransRegion, hv_Polor_Row,
                hv_Polor_Column, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad()
                , hv_Polor_InnerRadius, hv_Polor_OuterRadius, hv_Width, hv_Height, "nearest_neighbor");
            ho_RegionDilation.Dispose();
            HOperatorSet.DilationRectangle1(ho_PolarTransRegion, out ho_RegionDilation, MyLensCrack_AVI.m_Filter.Dilation_Width,
                MyLensCrack_AVI.m_Filter.Dilation_Height);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_RegionDilation, out ho_ConnectedRegions);
            HOperatorSet.SmallestRectangle1(ho_ConnectedRegions, out hv_Row1, out hv_Column1,
                out hv_Row2, out hv_Column2);
            HOperatorSet.TupleLength(hv_Row1, out hv_Length);
            //避免剪口被分兩半情況
            hv_Rectangle_Cutting_Column = 0;
            if ((int)(new HTuple(hv_Length.TupleEqual(1))) != 0)
            {
                ho_Rectangle_Cutting.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle_Cutting, 0, hv_Column1, hv_Row2,
                    hv_Column2);
                hv_Rectangle_Cutting_Column = 0;
            }
            else
            {
                ho_Rectangle_Cutting_1.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle_Cutting_1, 0, hv_Column1.TupleSelect(
                    0), hv_Row2.TupleSelect(0), hv_Column2.TupleSelect(0));
                ho_Rectangle_Cutting_2.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle_Cutting_2, 0, hv_Column1.TupleSelect(
                    1), hv_Row2.TupleSelect(1), hv_Column2.TupleSelect(1));
                ho_Rectangle_Cutting.Dispose();
                HOperatorSet.Union2(ho_Rectangle_Cutting_1, ho_Rectangle_Cutting_2, out ho_Rectangle_Cutting
                    );
                hv_Rectangle_Cutting_Column = hv_Column2.TupleMin();
            }

            //get_image_size (PolarTransImage, ImageWidth, ImageHeight)
            //畫檢測區域
            //* gen_rectangle1 (Rectangle, Polor_OuterRadius, 0, Polor_InnerRadius, ImageWidth)
            //減去隱蔽區
            //* difference (Rectangle, RegionMasking, RegionDifference)
            //* reduce_domain (PolarTransImage, RegionDifference, ImageReduced)
            //完全去掉隱蔽區,避免後續處理造成干擾
            //* crop_domain (ImageReduced, ImagePart)
            //獲得扣完隱蔽區區域
            HOperatorSet.GetImageSize(ho_PolarTransImage, out hv_Width, out hv_Height);
            //傅里叶转换
            ho_ImageFFT.Dispose();
            HOperatorSet.RftGeneric(ho_PolarTransImage, out ho_ImageFFT, "to_freq", "none",
                "complex", hv_Width);
            ho_ImageGauss.Dispose();
            HOperatorSet.GenGaussFilter(out ho_ImageGauss, hv_rft_Sigma1, hv_rft_Sigma2,
                0, "n", "rft", hv_Width, hv_Height);
            ho_ImageConvol.Dispose();
            HOperatorSet.ConvolFft(ho_ImageFFT, ho_ImageGauss, out ho_ImageConvol);
            ho_ImageFFT1.Dispose();
            HOperatorSet.RftGeneric(ho_ImageConvol, out ho_ImageFFT1, "from_freq", "none",
                "byte", hv_Width);

            ho_ImageSub.Dispose();
            HOperatorSet.SubImage(ho_PolarTransImage, ho_ImageFFT1, out ho_ImageSub, 3, 150);
            //均值濾波
            ho_ImageMean1.Dispose();
            HOperatorSet.MeanImage(ho_ImageSub, out ho_ImageMean1, hv_Mean_MaskWidth1, hv_Height / 2);
            ho_ImageMean2.Dispose();
            HOperatorSet.MeanImage(ho_ImageSub, out ho_ImageMean2, hv_Mean_MaskWidth2, hv_Height / 2);
            ho_RegionDynThresh_Dark.Dispose();
            HOperatorSet.DynThreshold(ho_ImageMean1, ho_ImageMean2, out ho_RegionDynThresh_Dark,
                hv_dyn_Dark_Offset, "dark");
            ho_RegionDynThresh_Light.Dispose();
            HOperatorSet.DynThreshold(ho_ImageMean1, ho_ImageMean2, out ho_RegionDynThresh_Light,
                hv_dyn_Light_Offset, "light");
            //Drak/Light合併
            ho_RegionUnion.Dispose();
            HOperatorSet.Union2(ho_RegionDynThresh_Dark, ho_RegionDynThresh_Light, out ho_RegionUnion
                );
            ho_RegionDifference.Dispose();
            HOperatorSet.Difference(ho_RegionUnion, ho_RegionDilation, out ho_RegionDifference
                );
            //移回Region原位置
            ho_XYTransRegion.Dispose();
            HOperatorSet.PolarTransRegionInv(ho_RegionDifference, out ho_XYTransRegion, hv_Polor_Row,
                hv_Polor_Column, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad()
                , hv_Polor_InnerRadius, hv_Polor_OuterRadius, hv_Width, hv_Height, hv_ImageWidth,
                hv_ImageHeight, "nearest_neighbor");

            ho_PolarTransImage.Dispose();
            ho_PolarTransRegion.Dispose();
            ho_RegionDilation.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_Rectangle_Cutting.Dispose();
            ho_Rectangle_Cutting_1.Dispose();
            ho_Rectangle_Cutting_2.Dispose();
            ho_RegionDifference.Dispose();
            ho_ImageFFT.Dispose();
            ho_ImageGauss.Dispose();
            ho_ImageConvol.Dispose();
            ho_ImageFFT1.Dispose();
            ho_ImageSub.Dispose();
            ho_ImageMean1.Dispose();
            ho_ImageMean2.Dispose();
            ho_RegionDynThresh_Dark.Dispose();
            ho_RegionDynThresh_Light.Dispose();
            ho_RegionUnion.Dispose();

            return;
        }
        #endregion

        private void FrmCrackTest_AVI_Load(object sender, EventArgs e)
        {
            bReadingPara = true;
            ReadPara();
            bReadingPara = false;
        }

        public void ReadPara()
        {
            try
            {
                ucFirstRadius.Value = MyLensCrack_AVI.Radius_First;
                cbMeasureSelect_First.SelectedIndex = MyLensCrack_AVI.m_First.MeasureSelect == "first" ? 0 : 1;
                ucRadius_First.Value = MyLensCrack_AVI.m_First.Radius;
                ucLength_First.Value = MyLensCrack_AVI.m_First.Length;
                if (MyLensCrack_AVI.m_First.MeasureTransition == "positive")
                    ucBlack2White_First.Value = MyLensCrack_AVI.m_First.MeasureThreshold;
                else
                    ucWhite2Black_First.Value = MyLensCrack_AVI.m_First.MeasureThreshold;
            }
            catch
            {
            }
            try
            {
                ucOuterRadius_Lens.Value = MyLensCrack_AVI.OuterRadius_Lens;
                ucInnerRadius_Lens.Value = MyLensCrack_AVI.InnerRadius_Lens;
                ucGray_Cutting.Value = MyLensCrack_AVI.m_Cutting.Gray;
                ucLength1_Upper_Cutting.Value = MyLensCrack_AVI.m_Cutting.Length1_Upper;
                ucLength1_Lower_Cutting.Value = MyLensCrack_AVI.m_Cutting.Length1_Lower;
                ucLength2_Upper_Cutting.Value = MyLensCrack_AVI.m_Cutting.Length2_Upper;
                ucLength2_Lower_Cutting.Value = MyLensCrack_AVI.m_Cutting.Length2_Lower;
                ucCuttingDilation.Value = MyLensCrack_AVI.m_Cutting.CuttingDilation;
            }
            catch
            {
            }
            try
            {
                ucOuterRadius_1.Value = MyLensCrack_AVI.m_RegionDetection_1.OuterRadous;
                ucInnerRadius_1.Value = MyLensCrack_AVI.m_RegionDetection_1.InnerRadous;
                ucOffset_Dark_1.Value = MyLensCrack_AVI.m_RegionDetection_1.Offset_Dark;
                ucOffset_Light_1.Value = MyLensCrack_AVI.m_RegionDetection_1.Offset_Light;

                ucOuterRadius_2.Value = MyLensCrack_AVI.m_RegionDetection_2.OuterRadous;
                ucInnerRadius_2.Value = MyLensCrack_AVI.m_RegionDetection_2.InnerRadous;
                ucOffset_Dark_2.Value = MyLensCrack_AVI.m_RegionDetection_2.Offset_Dark;
                ucOffset_Light_2.Value = MyLensCrack_AVI.m_RegionDetection_2.Offset_Light;

                ucOuterRadius_3.Value = MyLensCrack_AVI.m_RegionDetection_3.OuterRadous;
                ucInnerRadius_3.Value = MyLensCrack_AVI.m_RegionDetection_3.InnerRadous;
                ucOffset_Dark_3.Value = MyLensCrack_AVI.m_RegionDetection_3.Offset_Dark;
                ucOffset_Light_3.Value = MyLensCrack_AVI.m_RegionDetection_3.Offset_Light;
            }
            catch
            {
            }
            try
            {
                ucDilation_Width.Value = MyLensCrack_AVI.m_Filter.Dilation_Width;
                ucDilation_Height.Value = MyLensCrack_AVI.m_Filter.Dilation_Height;
                ucFilter_Closing.Value = MyLensCrack_AVI.m_Filter.Closing;
                ucFilter_Select_Area.Value = MyLensCrack_AVI.m_Filter.Select_Area;
            }
            catch
            {
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

        private void btnImageProPlus_Click(object sender, EventArgs e)
        {
            if (My.ho_Image == null)
                return;
            HObject ho_ResultImage = new HObject();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            string sResult = "";
            HWindow Window = hWindowControl1.HalconWindow;
            //HWindowControl _HWindow = new HWindowControl();
            //_HWindow.Width = 1000;
            //_HWindow.Height = 1000;
            ho_ResultImage.Dispose();
            ImageProcoss(Window, My.ho_Image, out ho_ResultImage, out sResult);
            //HOperatorSet.GetImageSize(ho_ResultImage, out hv_Width, out hv_Height);
            //HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
            //ho_ResultImage.DispObj(Window);

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
            HOperatorSet.SetWindowAttr("background_color", "black");
            //调整窗口显示大小以适应图片，这句一定要加上，不然图片显示的不全
            HOperatorSet.SetPart(window, 0, 0, hv_Height - 1, hv_Width - 1);
            HOperatorSet.DispObj(ho_Image, window); //将图像在该窗口进行显示
            return ho_Image; //返回这个图像
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

        private void ucFirstRadius_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.Radius_First = ucFirstRadius.Value;
            if (bReadingPara)
                return;
            try
            {
                HObject ho_Image = new HObject(), ho_Circle = new HObject(), ho_ReducedImage = new HObject();
                HTuple hv_Height = new HTuple(), hv_Width = new HTuple();
                HWindow Window = hWindowControl1.HalconWindow;
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);

                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, MyLensCrack_AVI.Radius_First);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                Window.ClearWindow();
                HOperatorSet.DispObj(ho_ReducedImage, Window);
            }
            catch
            {
            }
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("MyLensCrack_AVI", "FirstRadius", MyLensCrack_AVI.Radius_First.ToString(), Path);
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
            HObject ho_Image = new HObject();
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
            set_display_font(Window, 20, "mono", "true", "false");
            try
            {
                ho_Image.Dispose();
                ho_Image = My.ho_Image.CopyObj(1, -1);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2,MyLensCrack_AVI.Radius_First);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //ho_ImageMedian.Dispose();
                //HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 10, 10);
                //ho_ImageEmphasize.Dispose();
                //HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);
                Window.ClearWindow();
                ho_ReducedImage.DispObj(Window);
                disp_message(Window, "1.畫無料特徵外圍", "", 0, 0, "green", "false");
                HOperatorSet.DrawCircle(Window, out hv_Row, out hv_Column, out hv_Radius);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Outer, hv_Row, hv_Column, hv_Radius);
                ho_Circle_Outer.DispObj(Window);
                disp_message(Window, "2.畫無料特徵內圍", "", 50, 0, "green", "false");
                HOperatorSet.DrawCircle(Window, out hv_Row, out hv_Column, out hv_Radius);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Inner, hv_Row, hv_Column, hv_Radius);
                ho_Circle_Inner.DispObj(Window);
                //內外圓相減
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle_Outer, ho_Circle_Inner, out ho_RegionDifference);
                HOperatorSet.ReduceDomain(ho_ReducedImage, ho_RegionDifference, out ho_ReducedImage);
                HOperatorSet.CreateShapeModel(ho_ReducedImage, "auto", new HTuple(-30).TupleRad(), new HTuple(30).TupleRad(), "auto", "auto", "use_polarity", "auto", "auto", out hv_ModelID);
                HOperatorSet.GetShapeModelParams(hv_ModelID, out hv_NumLevels, out hv_AngleStart, out hv_AngleExtent, out hv_AngleStep, out hv_ScaleMin, out hv_ScaleMax, out hv_ScaleStep, out hv_Metric, out hv_MinContrast);
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                HOperatorSet.FindShapeModel(ho_ReducedImage, hv_ModelID, (new HTuple(-30)).TupleRad(), (new HTuple(30)).TupleRad(), 0.5, 0, 0.5, "least_squares", (new HTuple(6)).TupleConcat(1), 0.75, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
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
                else
                {
                    disp_message(Window, "找不到模組,請重新建立模組!", "", 0, 0, "red", "false");
                }
            }
            catch
            {
            }
            bDrawing = false;
        }

        private void btnFindModel_Miss_Click(object sender, EventArgs e)
        {
            if (My.ho_Image == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_Image = new HObject(), ho_Region = new HObject();
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
            ho_Image = My.ho_Image.CopyObj(1, -1);
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
            ho_Circle.Dispose();
            HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2,MyLensCrack_AVI.Radius_First);
            ho_ReducedImage.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
            //ho_ImageMedian.Dispose();
            //HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 10, 10);
            //ho_ImageEmphasize.Dispose();
            //HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);
            try
            {
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                HOperatorSet.FindShapeModel(ho_ReducedImage, hv_ModelID, (new HTuple(-30)).TupleRad(), (new HTuple(30)).TupleRad(), 0.5, 0, 0.5, "least_squares", (new HTuple(6)).TupleConcat(1), 0.75, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                set_display_font(Window, 20, "mono", "true", "false");
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
                else
                {
                    disp_message(Window, "找不到無料模組!", "", 0, 0, "green", "false");
                }
                HOperatorSet.ClearShapeModel(hv_ModelID);
            }
            catch
            {
            }
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
            HObject ho_Image = new HObject();
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
            set_display_font(Window, 20, "mono", "true", "false");
            try
            {
                ho_Image.Dispose();
                ho_Image = My.ho_Image.CopyObj(1, -1);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2,MyLensCrack_AVI.Radius_First);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 10, 10);
                ho_ImageEmphasize.Dispose();
                HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);
                Window.ClearWindow();
                ho_ImageEmphasize.DispObj(Window);
                disp_message(Window, "1.畫產品特徵外圍", "", 0, 0, "green", "false");
                HOperatorSet.DrawCircle(Window, out hv_Row, out hv_Column, out hv_Radius);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Outer, hv_Row, hv_Column, hv_Radius);
                ho_Circle_Outer.DispObj(Window);
                disp_message(Window, "2.畫產品特徵內圍", "", 50, 0, "green", "false");
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
                        string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model_First";
                        HOperatorSet.WriteShapeModel(hv_ModelID, Path);
                    }
                    HOperatorSet.ClearShapeModel(hv_ModelID);
                }
                else
                {
                    disp_message(Window, "找不到模組,請重新建立模組!", "", 0, 0, "red", "false");
                }
            }
            catch
            {
            }
            bDrawing = false;
        }

        private void btnFindModel_Click(object sender, EventArgs e)
        {
            if (My.ho_Image == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_Image = new HObject(), ho_Region = new HObject();
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
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model_First";
                HOperatorSet.ReadShapeModel(Path, out hv_ModelID);
            }
            catch
            {
                MessageBox.Show("先建立產品模組!");
            }
            ho_Image.Dispose();
            ho_Image = My.ho_Image.CopyObj(1, -1);
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
            ho_Circle.Dispose();
            HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, MyLensCrack_AVI.Radius_First);
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
                set_display_font(Window, 20, "mono", "true", "false");
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
                    disp_message(Window, "找到產品!", "", 50, 0, "green", "false");
                }
                else
                {
                    disp_message(Window, "找到不到產品!", "", 50, 0, "red", "false");
                }
                HOperatorSet.ClearShapeModel(hv_ModelID);
            }
            catch
            {
            }
        }

        private void cbMeasureSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
           MyLensCrack_AVI.m_First.MeasureSelect = cbMeasureSelect_First.SelectedIndex == 0 ? "first" : "last";
        }

        private void ucRadius_ValueChanged(int CurrentValue)
        {
           MyLensCrack_AVI.m_First.Radius = ucRadius_First.Value;
            if (bReadingPara)
                return;
            if (My.ho_Image == null)
                return;
            int Result = 0;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            try
            {
                FindCenter(Window, My.ho_Image, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours,out MyLensCrack_AVI.m_First.hv_ResultRow, out MyLensCrack_AVI.m_First.hv_ResultColumn);
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
            catch
            {
            }
        }

        private void ucLength_ValueChanged(int CurrentValue)
        {
           MyLensCrack_AVI.m_First.Length = ucLength_First.Value;
            if (bReadingPara)
                return;
            if (My.ho_Image == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject();
            int Result = 0;
            try
            {
                FindCenter(Window, My.ho_Image, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours,out MyLensCrack_AVI.m_First.hv_ResultRow, out MyLensCrack_AVI.m_First.hv_ResultColumn);
            }
            catch
            {
            }
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

        private void ucBlack2White_ValueChanged(int CurrentValue)
        {
           MyLensCrack_AVI.m_First.MeasureTransition = "positive";
           MyLensCrack_AVI.m_First.MeasureThreshold = ucBlack2White_First.Value;
            if (bReadingPara)
                return;
            if (My.ho_Image == null)
                return;
            int Result = 0;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject();
            FindCenter(Window, My.ho_Image, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours,out MyLensCrack_AVI.m_First.hv_ResultRow, out MyLensCrack_AVI.m_First.hv_ResultColumn);
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

        private void ucWhite2Black_ValueChanged(int CurrentValue)
        {
           MyLensCrack_AVI.m_First.MeasureTransition = "negative";
           MyLensCrack_AVI.m_First.MeasureThreshold = ucWhite2Black_First.Value;
            if (bReadingPara)
                return;
            if (My.ho_Image == null)
                return;
            int Result = 0;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            HObject ho_UsedEdges2 = new HObject(), ho_Contour2 = new HObject(), ho_ResultContours2 = new HObject();
            FindCenter(Window, My.ho_Image, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours,out MyLensCrack_AVI.m_First.hv_ResultRow, out MyLensCrack_AVI.m_First.hv_ResultColumn);
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

        private void btnFindCenter_Click(object sender, EventArgs e)
        {
            if (My.ho_Image == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            int Result = 0;
            FindCenter(Window, My.ho_Image, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours,out MyLensCrack_AVI.m_First.hv_ResultRow, out MyLensCrack_AVI.m_First.hv_ResultColumn);
            Window.ClearWindow();
            My.ho_Image.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("green");
            ho_ResultContours.DispObj(Window);
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("m_First", "MeasureSelect", MyLensCrack_AVI.m_First.MeasureSelect.ToString(), Path);
            IniFile.Write("m_First", "Radius", MyLensCrack_AVI.m_First.Radius.ToString(), Path);
            IniFile.Write("m_First", "Length", MyLensCrack_AVI.m_First.Length.ToString(), Path);
            IniFile.Write("m_First", "MeasureTransition", MyLensCrack_AVI.m_First.MeasureTransition.ToString(), Path);
            IniFile.Write("m_First", "MeasureThreshold", MyLensCrack_AVI.m_First.MeasureThreshold.ToString(), Path);
        }

        //求準確圓心
        public void FindCenter(HWindow Window, HObject TheImage, out int iResult, out HObject ho_UsedEdges, out HObject ho_Contour, out HObject ho_ResultContours, out HTuple hv_ResultRow, out HTuple hv_ResultColumn)
        {
            HObject ho_Image = new HObject(), ho_CrossCenter = new HObject();
            HObject ho_Circle = new HObject(), ho_ReducedImage = new HObject();
            HObject ho_ImageMedian = new HObject(), ho_ImageEmphasize = new HObject(), ho_Region = new HObject();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple(), hv_Number = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple(), hv_ResultRadius = new HTuple();
            HTuple hv_Radius = new HTuple(), hv_Length = new HTuple(), hv_MeasureThreshold = new HTuple(), hv_MeasureTransition = new HTuple(), hv_MeasureSelect = new HTuple();
            HTuple hv_Area = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);

            iResult = 0;
            hv_ResultRow = 0;
            hv_ResultColumn = 0;
            if (TheImage == null)
                return;
            //if (MyCrackTest_AVI.hv_FirstRow.D == 0 ||MyCrackTest_AVI.m_First.hv_FirstColumn.D == 0)
            //    return;
            hv_Radius = MyLensCrack_AVI.m_First.Radius;
            hv_Length = MyLensCrack_AVI.m_First.Length;
            hv_MeasureThreshold = MyLensCrack_AVI.m_First.MeasureThreshold;
            hv_MeasureTransition = MyLensCrack_AVI.m_First.MeasureTransition;
            hv_MeasureSelect = MyLensCrack_AVI.m_First.MeasureSelect;
            try
            {
                ho_Image.Dispose();
                ho_Image = TheImage.CopyObj(1, -1);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, MyLensCrack_AVI.Radius_First);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //ho_ImageMedian.Dispose();
                //HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 10, 10);
                //ho_ImageEmphasize.Dispose();
                //HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);
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
                    HOperatorSet.FindShapeModel(ho_ReducedImage, hv_ModelID, (new HTuple(-30)).TupleRad(), (new HTuple(30)).TupleRad(), 0.5, 0, 0.5, "least_squares", (new HTuple(7)).TupleConcat(1), 0.75, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
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
                    HTuple hv_ModelID = new HTuple(), hv_HomMat2D = new HTuple();
                    HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
                    HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
                    HTuple hv_RectangleRow = new HTuple(), hv_RectangleColumn = new HTuple(), hv_RectanglePhi = new HTuple(), hv_RectangleLength1 = new HTuple(), hv_RectangleLength2 = new HTuple();
                    try
                    {
                        string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model_First";
                        HOperatorSet.ReadShapeModel(Path, out hv_ModelID);
                    }
                    catch
                    {

                    }
                    HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                    HOperatorSet.FindShapeModel(ho_ReducedImage, hv_ModelID, (new HTuple(-30)).TupleRad(), (new HTuple(30)).TupleRad(), 0.5, 0, 0.5, "least_squares", (new HTuple(6)).TupleConcat(1), 0.75, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
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
                        HOperatorSet.SmallestCircle(ho_RegionUnion, out hv_Row, out hv_Column, out hv_ResultRadius);
                        HOperatorSet.GenCircle(out ho_Circle, hv_Row, hv_Column, hv_ResultRadius);
                        HOperatorSet.SetDraw(Window, "margin");
                        MyLensCrack_AVI.m_First.hv_ResultRow = hv_Row;
                        MyLensCrack_AVI.m_First.hv_ResultColumn = hv_Column;
                        HOperatorSet.ClearShapeModel(hv_ModelID);
                    }
                    else
                    {
                        iResult = 0;
                        ho_Image.DispObj(Window);
                        disp_message(Window, "Miss", "", 50, 0, "blue", "false");
                        HOperatorSet.ClearShapeModel(hv_ModelID);
                        return;
                    }

                }
                catch
                {
                    iResult = 0;
                    ho_Image.DispObj(Window);
                    disp_message(Window, "Miss", "", 50, 0, "blue", "false");
                    return;
                }
                try
                {
                    //第一次先找圓心
                    //HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
                    ho_UsedEdges.Dispose(); ho_Contour.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                    gen_circle_center(ho_ReducedImage, out ho_UsedEdges, out ho_Contour, out ho_ResultContours,
                        out ho_CrossCenter, hv_Row, hv_Column, hv_Radius, hv_Length, hv_MeasureThreshold, hv_MeasureTransition,
                        hv_MeasureSelect, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);
                }
                catch
                {
                    iResult = 0;
                }
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
        }

        private void ucRadius_Lens_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.OuterRadius_Lens = ucOuterRadius_Lens.Value;

            if (bReadingPara)
                return;
            try
            {
                HObject ho_Image = new HObject(), ho_OuterCircle = new HObject(), ho_InnerCircle = new HObject();
                HTuple hv_Height = new HTuple(), hv_Width = new HTuple();
                HWindow Window = hWindowControl1.HalconWindow;
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_OuterCircle);

                ho_OuterCircle.Dispose();
                HOperatorSet.GenCircle(out ho_OuterCircle, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.OuterRadius_Lens);
                ho_InnerCircle.Dispose();
                HOperatorSet.GenCircle(out ho_InnerCircle, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.InnerRadius_Lens);

                HOperatorSet.DispObj(ho_Image, Window);
                Window.SetColor("green");
                HOperatorSet.DispObj(ho_OuterCircle, Window);
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_InnerCircle, Window);
                ho_Image.Dispose();
                ho_OuterCircle.Dispose();
                ho_InnerCircle.Dispose();
            }
            catch
            {
            }
        }

        private void ucInnerRadius_Lens_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.InnerRadius_Lens = ucInnerRadius_Lens.Value;

            if (bReadingPara)
                return;
            try
            {
                HObject ho_Image = new HObject(), ho_OuterCircle = new HObject(), ho_InnerCircle = new HObject();
                HTuple hv_Height = new HTuple(), hv_Width = new HTuple();
                HWindow Window = hWindowControl1.HalconWindow;
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_OuterCircle);

                ho_OuterCircle.Dispose();
                HOperatorSet.GenCircle(out ho_OuterCircle, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.OuterRadius_Lens);
                ho_InnerCircle.Dispose();
                HOperatorSet.GenCircle(out ho_InnerCircle, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.InnerRadius_Lens);

                HOperatorSet.DispObj(ho_Image, Window);
                Window.SetColor("green");
                HOperatorSet.DispObj(ho_OuterCircle, Window);
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_InnerCircle, Window);
                ho_Image.Dispose();
                ho_OuterCircle.Dispose();
                ho_InnerCircle.Dispose();
            }
            catch
            {
            }
        }

        private void ucGray_Cutting_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_Cutting.Gray = ucGray_Cutting.Value;
            if (MyLensCrack_AVI.m_First.hv_ResultRow.D == 0)
                return;
            if (bReadingPara)
                return;
            try
            {
                HObject ho_Image = new HObject(), ho_Circle = new HObject(),ho_ReduceImage = new HObject(),ho_Region = new HObject();
                HWindow Window = hWindowControl1.HalconWindow;
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.OuterRadius_Lens);
                ho_ReduceImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReduceImage);
                //剪口灰度設定
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ReduceImage, out ho_Region, 0, MyLensCrack_AVI.m_Cutting.Gray);
                HOperatorSet.DispObj(ho_Image, Window);
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_Region, Window);
                ho_Image.Dispose();
                ho_Circle.Dispose();
            }
            catch
            {
            }
        }

        private void ucLength1_Upper_Cutting_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_Cutting.Length1_Upper = ucLength1_Upper_Cutting.Value;
            if (bReadingPara)
                return;
            if (MyLensCrack_AVI.m_First.hv_ResultRow.D == 0)
            {
                MessageBox.Show("請先求圓心!");
                return;
            }
            HObject ho_SelectedRegions = new HObject(), ho_SelectedRegions2 = new HObject();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_RegionLength1 = new HTuple(), hv_RegionLength2 = new HTuple(), hv_Number = new HTuple();
            HWindow Window = hWindowControl1.HalconWindow;
            ho_SelectedRegions.Dispose(); ho_SelectedRegions2.Dispose();
            try
            {
                FindCutting(Window, My.ho_Image, out ho_SelectedRegions, out ho_SelectedRegions2, out hv_Row, out hv_Column, out hv_RegionLength1, out hv_RegionLength2);

                HOperatorSet.TupleNumber(hv_Row, out hv_Number);
                HOperatorSet.DispObj(My.ho_Image, Window);
                Window.SetLineWidth(2);
                Window.SetDraw("fill");
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_SelectedRegions, Window);
                Window.SetDraw("margin");
                Window.SetColor("blue");
                HOperatorSet.DispObj(ho_SelectedRegions2, Window);
                for (int i = 0; i < hv_Number.D; i++)
                {
                    disp_message(Window, Math.Round(hv_RegionLength1[i].D, 0), "", hv_Row[i].D, hv_Column[i].D, "green", "false");
                    disp_message(Window, Math.Round(hv_RegionLength2[i].D, 0), "", hv_Row[i].D + 30, hv_Column[i].D, "blue", "false");
                }
            }
            catch
            {
            }
        }

        private void ucLength1_Lower_Cutting_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_Cutting.Length1_Lower = ucLength1_Lower_Cutting.Value;
            if (bReadingPara)
                return;
            if (MyLensCrack_AVI.m_First.hv_ResultRow.D == 0)
            {
                MessageBox.Show("請先求圓心!");
                return;
            }
            HObject ho_SelectedRegions = new HObject(), ho_SelectedRegions2 = new HObject();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_RegionLength1 = new HTuple(), hv_RegionLength2 = new HTuple(), hv_Number = new HTuple();
            HWindow Window = hWindowControl1.HalconWindow;
            ho_SelectedRegions.Dispose(); ho_SelectedRegions2.Dispose();
            try
            {
                FindCutting(Window, My.ho_Image, out ho_SelectedRegions, out ho_SelectedRegions2, out hv_Row, out hv_Column, out hv_RegionLength1, out hv_RegionLength2);

                HOperatorSet.TupleNumber(hv_Row, out hv_Number);
                HOperatorSet.DispObj(My.ho_Image, Window);
                Window.SetLineWidth(2);
                Window.SetDraw("fill");
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_SelectedRegions, Window);
                Window.SetDraw("margin");
                Window.SetColor("blue");
                HOperatorSet.DispObj(ho_SelectedRegions2, Window);
                for (int i = 0; i < hv_Number.D; i++)
                {
                    disp_message(Window, Math.Round(hv_RegionLength1[i].D, 0), "", hv_Row[i].D, hv_Column[i].D, "green", "false");
                    disp_message(Window, Math.Round(hv_RegionLength2[i].D, 0), "", hv_Row[i].D + 30, hv_Column[i].D, "blue", "false");
                }
            }
            catch
            {
            }
        }

        private void ucLength2_Upper_Cutting_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_Cutting.Length2_Upper = ucLength2_Upper_Cutting.Value;

            HObject ho_SelectedRegions = new HObject(), ho_SelectedRegions2 = new HObject();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_RegionLength1 = new HTuple(), hv_RegionLength2 = new HTuple(), hv_Number = new HTuple();
            HWindow Window = hWindowControl1.HalconWindow;
            ho_SelectedRegions.Dispose(); ho_SelectedRegions2.Dispose();
            try
            {
                FindCutting(Window, My.ho_Image, out ho_SelectedRegions, out ho_SelectedRegions2, out hv_Row, out hv_Column, out hv_RegionLength1, out hv_RegionLength2);

                HOperatorSet.TupleNumber(hv_Row, out hv_Number);
                HOperatorSet.DispObj(My.ho_Image, Window);
                Window.SetLineWidth(2);
                Window.SetDraw("fill");
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_SelectedRegions, Window);
                Window.SetDraw("margin");
                Window.SetColor("blue");
                HOperatorSet.DispObj(ho_SelectedRegions2, Window);

                for (int i = 0; i < hv_Number.D; i++)
                {
                    disp_message(Window, Math.Round(hv_RegionLength1[i].D, 0), "", hv_Row[i].D, hv_Column[i].D, "green", "false");
                    disp_message(Window, Math.Round(hv_RegionLength2[i].D, 0), "", hv_Row[i].D + 30, hv_Column[i].D, "blue", "false");
                }
            }
            catch
            {
            }
        }

        private void ucLength2_Lower_Cutting_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_Cutting.Length2_Lower = ucLength2_Lower_Cutting.Value; 
            
            HObject ho_SelectedRegions = new HObject(),ho_SelectedRegions2 = new HObject();
            HTuple hv_Row = new HTuple(),hv_Column = new HTuple();
            HTuple hv_RegionLength1 = new HTuple(), hv_RegionLength2 = new HTuple(), hv_Number = new HTuple();
            HWindow Window = hWindowControl1.HalconWindow;
            ho_SelectedRegions.Dispose(); ho_SelectedRegions2.Dispose();
            try
            {
                FindCutting(Window, My.ho_Image, out ho_SelectedRegions, out ho_SelectedRegions2, out hv_Row, out hv_Column, out hv_RegionLength1, out hv_RegionLength2);

                HOperatorSet.TupleNumber(hv_Row, out hv_Number);
                HOperatorSet.DispObj(My.ho_Image, Window);
                Window.SetLineWidth(2);
                Window.SetDraw("fill");
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_SelectedRegions, Window);
                Window.SetDraw("margin");
                Window.SetColor("blue");
                HOperatorSet.DispObj(ho_SelectedRegions2, Window);

                for (int i = 0; i < hv_Number.D; i++)
                {
                    disp_message(Window, Math.Round(hv_RegionLength1[i].D, 0), "", hv_Row[i].D, hv_Column[i].D, "green", "false");
                    disp_message(Window, Math.Round(hv_RegionLength2[i].D, 0), "", hv_Row[i].D + 30, hv_Column[i].D, "blue", "false");
                }
            }
            catch
            {
            }
        }

        public void FindCutting(HWindow Window, HObject TheImage, out HObject ho_SelectedRegions, out HObject ho_SelectedRegions2, out HTuple hv_Row, out HTuple hv_Column,out HTuple hv_RegionLength1,out HTuple hv_RegionLength2)
        {
            ho_SelectedRegions = new HObject();
            ho_SelectedRegions2 = new HObject();
            hv_Row = 0;
            hv_Column = 0;
            hv_RegionLength1 = 0;
            hv_RegionLength2 = 0;

            HObject ho_Image = new HObject(), ho_OuterCircle = new HObject(), ho_InnerCircle = new HObject(),ho_RegionDifference = new HObject(), ho_ReduceImage = new HObject(), ho_Region = new HObject();
            HObject ho_RegionOpening = new HObject(), ho_ConnectedRegions = new HObject(), ho_RegionFillUp = new HObject();
            HTuple hv_Area = new HTuple();

            if (TheImage == null)
                return;
            try
            {
                ho_Image.Dispose();
                HOperatorSet.CopyImage(TheImage, out ho_Image);
                ho_OuterCircle.Dispose();
                HOperatorSet.GenCircle(out ho_OuterCircle, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.OuterRadius_Lens);
                ho_InnerCircle.Dispose();
                HOperatorSet.GenCircle(out ho_InnerCircle, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.InnerRadius_Lens);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_OuterCircle, ho_InnerCircle, out ho_RegionDifference);
                ho_ReduceImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ReduceImage);
                //剪口灰度設定
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ReduceImage, out ho_Region, MyLensCrack_AVI.m_Cutting.Gray, 255);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_Region, out  ho_RegionFillUp);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(ho_RegionDifference, ho_RegionFillUp, out ExpTmpOutVar_0);
                    ho_RegionDifference.Dispose();
                    ho_RegionDifference = ExpTmpOutVar_0;
                }
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionDifference, out ho_RegionOpening, 3.5);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, (new HTuple("rect2_len1")).TupleConcat("rect2_len2"), "and", (new HTuple(MyLensCrack_AVI.m_Cutting.Length1_Lower)).TupleConcat(MyLensCrack_AVI.m_Cutting.Length2_Lower), (new HTuple(MyLensCrack_AVI.m_Cutting.Length1_Upper)).TupleConcat(MyLensCrack_AVI.m_Cutting.Length2_Upper));

                HOperatorSet.RegionFeatures(ho_SelectedRegions, "rect2_len1", out hv_RegionLength1);
                HOperatorSet.RegionFeatures(ho_SelectedRegions, "rect2_len2", out hv_RegionLength2);
                HOperatorSet.AreaCenter(ho_SelectedRegions,out hv_Area,out hv_Row,out hv_Column);
                ho_SelectedRegions2.Dispose();
                HOperatorSet.SelectShape(ho_SelectedRegions, out ho_SelectedRegions2, "area", "and", hv_Area.TupleMax(),hv_Area.TupleMax());
            }
            catch
            {
            }
            ho_Image.Dispose();
            ho_OuterCircle.Dispose();
            ho_ReduceImage.Dispose();
            ho_Region.Dispose();
            ho_RegionOpening.Dispose();
            ho_ConnectedRegions.Dispose();
        }

        private void btnSave_Cutting_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("MyLensCrack_AVI", "OuterRadius_Lens", MyLensCrack_AVI.OuterRadius_Lens.ToString(), Path);
            IniFile.Write("MyLensCrack_AVI", "InnerRadius_Lens", MyLensCrack_AVI.InnerRadius_Lens.ToString(), Path);
            IniFile.Write("m_Cutting", "Gray", MyLensCrack_AVI.m_Cutting.Gray.ToString(), Path);
            IniFile.Write("m_Cutting", "Length1_Upper", MyLensCrack_AVI.m_Cutting.Length1_Upper.ToString(), Path);
            IniFile.Write("m_Cutting", "Length1_Lower", MyLensCrack_AVI.m_Cutting.Length1_Lower.ToString(), Path);
            IniFile.Write("m_Cutting", "Length2_Upper", MyLensCrack_AVI.m_Cutting.Length2_Upper.ToString(), Path);
            IniFile.Write("m_Cutting", "Length2_Lower", MyLensCrack_AVI.m_Cutting.Length2_Lower.ToString(), Path);
            IniFile.Write("m_Cutting", "CuttingDilation", MyLensCrack_AVI.m_Cutting.CuttingDilation.ToString(), Path);
        }

        private void btnFindLensCutting_Click(object sender, EventArgs e)
        {
            HObject ho_SelectedRegions = new HObject(), ho_SelectedRegions2 = new HObject(), ho_RegionDilation = new HObject();
            HTuple hv_Row = new HTuple(),hv_Column = new HTuple();
            HTuple hv_RegionLength1 = new HTuple(), hv_RegionLength2 = new HTuple();
            HWindow Window = hWindowControl1.HalconWindow;
            ho_SelectedRegions.Dispose(); ho_SelectedRegions2.Dispose();
            try
            {
                FindCutting(Window, My.ho_Image, out ho_SelectedRegions, out ho_SelectedRegions2, out hv_Row, out hv_Column, out hv_RegionLength1, out hv_RegionLength2);
                ho_RegionDilation.Dispose();
                HOperatorSet.DilationCircle(ho_SelectedRegions2, out ho_RegionDilation, MyLensCrack_AVI.m_Cutting.CuttingDilation);
                HOperatorSet.DispObj(My.ho_Image, Window);
                Window.SetDraw("margin");
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_RegionDilation, Window);
                MyLensCrack_AVI.m_Cutting.ReionCutting = ho_RegionDilation.CopyObj(1, -1);
                
            }
            catch
            {
            }
        }

        private void ucOuterRadius_1_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_RegionDetection_1.OuterRadous = ucOuterRadius_1.Value;
            if (bReadingPara)
                return;
            try
            {
                HObject ho_Image = new HObject(), ho_Circle_Outer = new HObject(), ho_Circle_Inner = new HObject(), ho_ReducedImage = new HObject();
                HTuple hv_Height = new HTuple(), hv_Width = new HTuple();
                HWindow Window = hWindowControl1.HalconWindow;
                if (My.ho_Image == null)
                    return;
                if (MyLensCrack_AVI.m_First.hv_ResultRow.D == 0)
                {
                    MessageBox.Show("請先求圓心!");
                    return;
                }
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                ho_Circle_Outer.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Outer, MyLensCrack_AVI.m_First.hv_ResultRow.D, MyLensCrack_AVI.m_First.hv_ResultColumn.D, MyLensCrack_AVI.m_RegionDetection_1.OuterRadous);
                ho_Circle_Inner.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Inner, MyLensCrack_AVI.m_First.hv_ResultRow.D, MyLensCrack_AVI.m_First.hv_ResultColumn.D, MyLensCrack_AVI.m_RegionDetection_1.InnerRadous);
                HOperatorSet.DispObj(My.ho_Image, Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                HOperatorSet.DispObj(ho_Circle_Outer, Window);
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_Circle_Inner, Window);
            }
            catch
            {
            }
        }

        private void ucInnerRadius_1_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_RegionDetection_1.InnerRadous = ucInnerRadius_1.Value;
            if (bReadingPara)
                return;
            try
            {
                HObject ho_Image = new HObject(), ho_Circle_Outer = new HObject(), ho_Circle_Inner = new HObject(), ho_ReducedImage = new HObject();
                HTuple hv_Height = new HTuple(), hv_Width = new HTuple();
                HWindow Window = hWindowControl1.HalconWindow;
                if (My.ho_Image == null)
                    return;
                if (MyLensCrack_AVI.m_First.hv_ResultRow.D == 0)
                {
                    MessageBox.Show("請先求圓心!");
                    return;
                }
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                ho_Circle_Outer.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Outer, MyLensCrack_AVI.m_First.hv_ResultRow.D, MyLensCrack_AVI.m_First.hv_ResultColumn.D, MyLensCrack_AVI.m_RegionDetection_1.OuterRadous);
                ho_Circle_Inner.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Inner, MyLensCrack_AVI.m_First.hv_ResultRow.D, MyLensCrack_AVI.m_First.hv_ResultColumn.D, MyLensCrack_AVI.m_RegionDetection_1.InnerRadous); 
                HOperatorSet.DispObj(My.ho_Image, Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                HOperatorSet.DispObj(ho_Circle_Outer, Window);
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_Circle_Inner, Window);
            }
            catch
            {
            }
        }

        private void ucOffset_Dark_1_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_RegionDetection_1.Offset_Dark = ucOffset_Dark_1.Value;
            if (bReadingPara)
                return;
            if (My.ho_Image == null)
                return;
            if (MyLensCrack_AVI.m_Cutting.ReionCutting == null)
                return;
            HObject ho_Image = new HObject(), ho_ResultRegions = new HObject();
            HWindow Window = hWindowControl1.HalconWindow;
            try
            {
                ho_Image.Dispose();
                ho_Image = My.ho_Image.CopyObj(1, -1);
                ho_ResultRegions.Dispose();
                m_polor_trans_Masking_rft_dyn_threshold(ho_Image, MyLensCrack_AVI.m_Cutting.ReionCutting, out ho_ResultRegions, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.m_RegionDetection_1.OuterRadous, MyLensCrack_AVI.m_RegionDetection_1.InnerRadous, 100, 100, 5, 50, MyLensCrack_AVI.m_RegionDetection_1.Offset_Dark, MyLensCrack_AVI.m_RegionDetection_1.Offset_Light);
                ho_Image.DispObj(Window);
                Window.SetColor("green");
                ho_ResultRegions.DispObj(Window);
                MyLensCrack_AVI.m_RegionDetection_1.ResultRegion.Dispose();
                MyLensCrack_AVI.m_RegionDetection_1.ResultRegion = ho_ResultRegions.CopyObj(1, -1);
            }
            catch
            {
            }
            ho_Image.Dispose();
            ho_ResultRegions.Dispose();
        }

        private void ucOffset_White_1_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_RegionDetection_1.Offset_Light = ucOffset_Light_1.Value;
            if (bReadingPara)
                return;
            if (My.ho_Image == null)
                return;
            if (MyLensCrack_AVI.m_Cutting.ReionCutting == null)
                return;
            
            HObject ho_Image = new HObject(), ho_ResultRegions = new HObject();
            HWindow Window = hWindowControl1.HalconWindow;
            try
            {
                ho_Image.Dispose();
                ho_Image = My.ho_Image.CopyObj(1, -1);
                ho_ResultRegions.Dispose();
                m_polor_trans_Masking_rft_dyn_threshold(ho_Image, MyLensCrack_AVI.m_Cutting.ReionCutting, out ho_ResultRegions, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.m_RegionDetection_1.OuterRadous, MyLensCrack_AVI.m_RegionDetection_1.InnerRadous, 100, 100, 5, 50, MyLensCrack_AVI.m_RegionDetection_1.Offset_Dark, MyLensCrack_AVI.m_RegionDetection_1.Offset_Light);
                ho_Image.DispObj(Window);
                Window.SetColor("green");
                ho_ResultRegions.DispObj(Window);
                MyLensCrack_AVI.m_RegionDetection_1.ResultRegion.Dispose();
                MyLensCrack_AVI.m_RegionDetection_1.ResultRegion = ho_ResultRegions.CopyObj(1, -1);
            }
            catch
            {
            }
            ho_Image.Dispose();
            ho_ResultRegions.Dispose();
        }

        private void ucOuterRadius_2_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_RegionDetection_2.OuterRadous = ucOuterRadius_2.Value;
            if (bReadingPara)
                return;
            try
            {
                HObject ho_Image = new HObject(), ho_Circle_Outer = new HObject(), ho_Circle_Inner = new HObject(), ho_ReducedImage = new HObject();
                HTuple hv_Height = new HTuple(), hv_Width = new HTuple();
                HWindow Window = hWindowControl1.HalconWindow;
                if (My.ho_Image == null)
                    return;
                if (MyLensCrack_AVI.m_First.hv_ResultRow.D == 0)
                {
                    MessageBox.Show("請先求圓心!");
                    return;
                }
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                ho_Circle_Outer.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Outer, MyLensCrack_AVI.m_First.hv_ResultRow.D, MyLensCrack_AVI.m_First.hv_ResultColumn.D, MyLensCrack_AVI.m_RegionDetection_2.OuterRadous);
                ho_Circle_Inner.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Inner, MyLensCrack_AVI.m_First.hv_ResultRow.D, MyLensCrack_AVI.m_First.hv_ResultColumn.D, MyLensCrack_AVI.m_RegionDetection_2.InnerRadous);
                HOperatorSet.DispObj(My.ho_Image, Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                HOperatorSet.DispObj(ho_Circle_Outer, Window);
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_Circle_Inner, Window);
            }
            catch
            {
            }
        }

        private void ucInnerRadius_2_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_RegionDetection_2.InnerRadous = ucInnerRadius_2.Value;
            if (bReadingPara)
                return;
            try
            {
                HObject ho_Image = new HObject(), ho_Circle_Outer = new HObject(), ho_Circle_Inner = new HObject(), ho_ReducedImage = new HObject();
                HTuple hv_Height = new HTuple(), hv_Width = new HTuple();
                HWindow Window = hWindowControl1.HalconWindow;
                if (My.ho_Image == null)
                    return;
                if (MyLensCrack_AVI.m_First.hv_ResultRow.D == 0)
                {
                    MessageBox.Show("請先求圓心!");
                    return;
                }
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                ho_Circle_Outer.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Outer, MyLensCrack_AVI.m_First.hv_ResultRow.D, MyLensCrack_AVI.m_First.hv_ResultColumn.D, MyLensCrack_AVI.m_RegionDetection_2.OuterRadous);
                ho_Circle_Inner.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Inner, MyLensCrack_AVI.m_First.hv_ResultRow.D, MyLensCrack_AVI.m_First.hv_ResultColumn.D, MyLensCrack_AVI.m_RegionDetection_2.InnerRadous);
                HOperatorSet.DispObj(My.ho_Image, Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                HOperatorSet.DispObj(ho_Circle_Outer, Window);
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_Circle_Inner, Window);
            }
            catch
            {
            }
        }

        private void ucOffset_Dark_2_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_RegionDetection_2.Offset_Dark = ucOffset_Dark_2.Value;
            if (bReadingPara)
                return;
            if (My.ho_Image == null)
                return;
            if (MyLensCrack_AVI.m_Cutting.ReionCutting == null)
                return;
            HObject ho_Image = new HObject(), ho_ResultRegions = new HObject();
            HWindow Window = hWindowControl1.HalconWindow;
            try
            {
                ho_Image.Dispose();
                ho_Image = My.ho_Image.CopyObj(1, -1);
                ho_ResultRegions.Dispose();
                m_polor_trans_Masking_rft_dyn_threshold(ho_Image, MyLensCrack_AVI.m_Cutting.ReionCutting, out ho_ResultRegions, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.m_RegionDetection_2.OuterRadous, MyLensCrack_AVI.m_RegionDetection_2.InnerRadous, 100, 100, 5, 50, MyLensCrack_AVI.m_RegionDetection_2.Offset_Dark, MyLensCrack_AVI.m_RegionDetection_2.Offset_Light);
                ho_Image.DispObj(Window);
                Window.SetColor("green");
                ho_ResultRegions.DispObj(Window);
                MyLensCrack_AVI.m_RegionDetection_2.ResultRegion.Dispose();
                MyLensCrack_AVI.m_RegionDetection_2.ResultRegion = ho_ResultRegions.CopyObj(1, -1);
            }
            catch
            {
            }
            ho_Image.Dispose();
            ho_ResultRegions.Dispose();
        }

        private void ucOffset_Light_2_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_RegionDetection_2.Offset_Light = ucOffset_Light_2.Value;
            if (bReadingPara)
                return;
            if (My.ho_Image == null)
                return;
            if (MyLensCrack_AVI.m_Cutting.ReionCutting == null)
                return;

            HObject ho_Image = new HObject(), ho_ResultRegions = new HObject();
            HWindow Window = hWindowControl1.HalconWindow;
            try
            {
                ho_Image.Dispose();
                ho_Image = My.ho_Image.CopyObj(1, -1);
                ho_ResultRegions.Dispose();
                m_polor_trans_Masking_rft_dyn_threshold(ho_Image, MyLensCrack_AVI.m_Cutting.ReionCutting, out ho_ResultRegions, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.m_RegionDetection_2.OuterRadous, MyLensCrack_AVI.m_RegionDetection_2.InnerRadous, 100, 100, 5, 50, MyLensCrack_AVI.m_RegionDetection_2.Offset_Dark, MyLensCrack_AVI.m_RegionDetection_2.Offset_Light);
                ho_Image.DispObj(Window);
                Window.SetColor("green");
                ho_ResultRegions.DispObj(Window);
                MyLensCrack_AVI.m_RegionDetection_2.ResultRegion.Dispose();
                MyLensCrack_AVI.m_RegionDetection_2.ResultRegion = ho_ResultRegions.CopyObj(1, -1);
            }
            catch
            {
            }
            ho_Image.Dispose();
            ho_ResultRegions.Dispose();
        }

        private void ucOuterRadius_3_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_RegionDetection_3.OuterRadous = ucOuterRadius_3.Value;
            if (bReadingPara)
                return;
            try
            {
                HObject ho_Image = new HObject(), ho_Circle_Outer = new HObject(), ho_Circle_Inner = new HObject(), ho_ReducedImage = new HObject();
                HTuple hv_Height = new HTuple(), hv_Width = new HTuple();
                HWindow Window = hWindowControl1.HalconWindow;
                if (My.ho_Image == null)
                    return;
                if (MyLensCrack_AVI.m_First.hv_ResultRow.D == 0)
                {
                    MessageBox.Show("請先求圓心!");
                    return;
                }
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                ho_Circle_Outer.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Outer, MyLensCrack_AVI.m_First.hv_ResultRow.D, MyLensCrack_AVI.m_First.hv_ResultColumn.D, MyLensCrack_AVI.m_RegionDetection_3.OuterRadous);
                ho_Circle_Inner.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Inner, MyLensCrack_AVI.m_First.hv_ResultRow.D, MyLensCrack_AVI.m_First.hv_ResultColumn.D, MyLensCrack_AVI.m_RegionDetection_3.InnerRadous);
                HOperatorSet.DispObj(My.ho_Image, Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                HOperatorSet.DispObj(ho_Circle_Outer, Window);
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_Circle_Inner, Window);
            }
            catch
            {
            }
        }

        private void ucInnerRadius_3_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_RegionDetection_3.InnerRadous = ucInnerRadius_3.Value;
            if (bReadingPara)
                return;
            try
            {
                HObject ho_Image = new HObject(), ho_Circle_Outer = new HObject(), ho_Circle_Inner = new HObject(), ho_ReducedImage = new HObject();
                HTuple hv_Height = new HTuple(), hv_Width = new HTuple();
                HWindow Window = hWindowControl1.HalconWindow;
                if (My.ho_Image == null)
                    return;
                if (MyLensCrack_AVI.m_First.hv_ResultRow.D == 0)
                {
                    MessageBox.Show("請先求圓心!");
                    return;
                }
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                ho_Circle_Outer.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Outer, MyLensCrack_AVI.m_First.hv_ResultRow.D, MyLensCrack_AVI.m_First.hv_ResultColumn.D, MyLensCrack_AVI.m_RegionDetection_3.OuterRadous);
                ho_Circle_Inner.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_Inner, MyLensCrack_AVI.m_First.hv_ResultRow.D, MyLensCrack_AVI.m_First.hv_ResultColumn.D, MyLensCrack_AVI.m_RegionDetection_3.InnerRadous);
                HOperatorSet.DispObj(My.ho_Image, Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                HOperatorSet.DispObj(ho_Circle_Outer, Window);
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_Circle_Inner, Window);
            }
            catch
            {
            }
        }

        private void ucOffset_Dark_3_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_RegionDetection_3.Offset_Dark = ucOffset_Dark_3.Value;
            if (bReadingPara)
                return;
            if (My.ho_Image == null)
                return;
            if (MyLensCrack_AVI.m_Cutting.ReionCutting == null)
                return;
            HObject ho_Image = new HObject(), ho_ResultRegions = new HObject();
            HWindow Window = hWindowControl1.HalconWindow;
            try
            {
                ho_Image.Dispose();
                ho_Image = My.ho_Image.CopyObj(1, -1);
                ho_ResultRegions.Dispose();
                m_polor_trans_Masking_rft_dyn_threshold(ho_Image, MyLensCrack_AVI.m_Cutting.ReionCutting, out ho_ResultRegions, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.m_RegionDetection_3.OuterRadous, MyLensCrack_AVI.m_RegionDetection_3.InnerRadous, 100, 100, 5, 50, MyLensCrack_AVI.m_RegionDetection_3.Offset_Dark, MyLensCrack_AVI.m_RegionDetection_3.Offset_Light);
                ho_Image.DispObj(Window);
                Window.SetColor("green");
                ho_ResultRegions.DispObj(Window);
                MyLensCrack_AVI.m_RegionDetection_3.ResultRegion.Dispose();
                MyLensCrack_AVI.m_RegionDetection_3.ResultRegion = ho_ResultRegions.CopyObj(1, -1);
            }
            catch
            {
            }
            ho_Image.Dispose();
            ho_ResultRegions.Dispose();
        }

        private void ucOffset_Light_3_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_RegionDetection_3.Offset_Light = ucOffset_Light_3.Value;
            if (bReadingPara)
                return;
            if (My.ho_Image == null)
                return;
            if (MyLensCrack_AVI.m_Cutting.ReionCutting == null)
                return;

            HObject ho_Image = new HObject(), ho_ResultRegions = new HObject();
            HWindow Window = hWindowControl1.HalconWindow;
            try
            {
                ho_Image.Dispose();
                ho_Image = My.ho_Image.CopyObj(1, -1);
                ho_ResultRegions.Dispose();
                m_polor_trans_Masking_rft_dyn_threshold(ho_Image, MyLensCrack_AVI.m_Cutting.ReionCutting, out ho_ResultRegions, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.m_RegionDetection_3.OuterRadous, MyLensCrack_AVI.m_RegionDetection_3.InnerRadous, 100, 100, 5, 50, MyLensCrack_AVI.m_RegionDetection_3.Offset_Dark, MyLensCrack_AVI.m_RegionDetection_3.Offset_Light);
                ho_Image.DispObj(Window);
                Window.SetColor("green");
                ho_ResultRegions.DispObj(Window);
                MyLensCrack_AVI.m_RegionDetection_3.ResultRegion.Dispose();
                MyLensCrack_AVI.m_RegionDetection_3.ResultRegion = ho_ResultRegions.CopyObj(1, -1);
            }
            catch
            {
            }
            ho_Image.Dispose();
            ho_ResultRegions.Dispose();
        }

        private void btnSave_1_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("m_RegionDetection_1", "OuterRadous", MyLensCrack_AVI.m_RegionDetection_1.OuterRadous.ToString(), Path);
            IniFile.Write("m_RegionDetection_1", "InnerRadous", MyLensCrack_AVI.m_RegionDetection_1.InnerRadous.ToString(), Path);
            IniFile.Write("m_RegionDetection_1", "Offset_Dark", MyLensCrack_AVI.m_RegionDetection_1.Offset_Dark.ToString(), Path);
            IniFile.Write("m_RegionDetection_1", "Offset_Light", MyLensCrack_AVI.m_RegionDetection_1.Offset_Light.ToString(), Path);
        }

        private void btnSave_2_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("m_RegionDetection_2", "OuterRadous", MyLensCrack_AVI.m_RegionDetection_2.OuterRadous.ToString(), Path);
            IniFile.Write("m_RegionDetection_2", "InnerRadous", MyLensCrack_AVI.m_RegionDetection_2.InnerRadous.ToString(), Path);
            IniFile.Write("m_RegionDetection_2", "Offset_Dark", MyLensCrack_AVI.m_RegionDetection_2.Offset_Dark.ToString(), Path);
            IniFile.Write("m_RegionDetection_2", "Offset_Light", MyLensCrack_AVI.m_RegionDetection_2.Offset_Light.ToString(), Path);
        }

        private void btnSave_3_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("m_RegionDetection_3", "OuterRadous", MyLensCrack_AVI.m_RegionDetection_3.OuterRadous.ToString(), Path);
            IniFile.Write("m_RegionDetection_3", "InnerRadous", MyLensCrack_AVI.m_RegionDetection_3.InnerRadous.ToString(), Path);
            IniFile.Write("m_RegionDetection_3", "Offset_Dark", MyLensCrack_AVI.m_RegionDetection_3.Offset_Dark.ToString(), Path);
            IniFile.Write("m_RegionDetection_3", "Offset_Light", MyLensCrack_AVI.m_RegionDetection_3.Offset_Light.ToString(), Path);
        }

        private void ucDilation_Width_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_Filter.Dilation_Width = ucDilation_Width.Value;
            DilationCutting(); 
        }

        private void ucDilation_Height_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_Filter.Dilation_Height = ucDilation_Height.Value;
            DilationCutting(); 
        }

        public void DilationCutting()
        {
            
        }

        private void btnSave_Dilation_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("m_Filter", "Dilation_Width", MyLensCrack_AVI.m_Filter.Dilation_Width.ToString(), Path);
            IniFile.Write("m_Filter", "Dilation_Height", MyLensCrack_AVI.m_Filter.Dilation_Height.ToString(), Path);
        }

        private void btnShowRegion_Click(object sender, EventArgs e)
        {
            HObject ho_Image = new HObject(), ho_ResultRegions_1 = new HObject(), ho_ResultRegions_2 = new HObject(), ho_ResultRegions_3 = new HObject();
            HObject UnionResultRegion1 = new HObject(),UnionResultRegion = new HObject();
            HWindow Window = hWindowControl1.HalconWindow;
            try
            {
                ho_Image.Dispose();
                ho_Image = My.ho_Image.CopyObj(1, -1);
                ho_ResultRegions_1.Dispose();
                m_polor_trans_Masking_rft_dyn_threshold(ho_Image, MyLensCrack_AVI.m_Cutting.ReionCutting, out ho_ResultRegions_1, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.m_RegionDetection_1.OuterRadous, MyLensCrack_AVI.m_RegionDetection_1.InnerRadous, 100, 100, 5, 50, MyLensCrack_AVI.m_RegionDetection_1.Offset_Dark, MyLensCrack_AVI.m_RegionDetection_1.Offset_Light);

                ho_ResultRegions_2.Dispose();
                m_polor_trans_Masking_rft_dyn_threshold(ho_Image, MyLensCrack_AVI.m_Cutting.ReionCutting, out ho_ResultRegions_2, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.m_RegionDetection_2.OuterRadous, MyLensCrack_AVI.m_RegionDetection_2.InnerRadous, 100, 100, 5, 50, MyLensCrack_AVI.m_RegionDetection_2.Offset_Dark, MyLensCrack_AVI.m_RegionDetection_2.Offset_Light);

                ho_ResultRegions_3.Dispose();
                m_polor_trans_Masking_rft_dyn_threshold(ho_Image, MyLensCrack_AVI.m_Cutting.ReionCutting, out ho_ResultRegions_3, MyLensCrack_AVI.m_First.hv_ResultRow, MyLensCrack_AVI.m_First.hv_ResultColumn, MyLensCrack_AVI.m_RegionDetection_3.OuterRadous, MyLensCrack_AVI.m_RegionDetection_3.InnerRadous, 100, 100, 5, 50, MyLensCrack_AVI.m_RegionDetection_3.Offset_Dark, MyLensCrack_AVI.m_RegionDetection_3.Offset_Light);
                UnionResultRegion1.Dispose();
                HOperatorSet.Union2(ho_ResultRegions_1, ho_ResultRegions_2, out UnionResultRegion1);
                UnionResultRegion.Dispose();
                HOperatorSet.Union2(UnionResultRegion1, ho_ResultRegions_3, out UnionResultRegion);

                
                ho_Image.DispObj(Window);
                Window.SetColor("green");
                ho_ResultRegions_1.DispObj(Window);
                ho_ResultRegions_2.DispObj(Window);
                ho_ResultRegions_3.DispObj(Window);

                MyLensCrack_AVI.m_RegionDetection_1.ResultRegion.Dispose();
                MyLensCrack_AVI.m_RegionDetection_1.ResultRegion = ho_ResultRegions_1.CopyObj(1, -1);
                MyLensCrack_AVI.m_RegionDetection_2.ResultRegion.Dispose();
                MyLensCrack_AVI.m_RegionDetection_2.ResultRegion = ho_ResultRegions_2.CopyObj(1, -1);
                MyLensCrack_AVI.m_RegionDetection_3.ResultRegion.Dispose();
                MyLensCrack_AVI.m_RegionDetection_3.ResultRegion = ho_ResultRegions_3.CopyObj(1, -1);
                MyLensCrack_AVI.m_Filter.UnionResultRegion.Dispose();
                MyLensCrack_AVI.m_Filter.UnionResultRegion = UnionResultRegion.CopyObj(1, -1);
            }
            catch
            {
            }
            ho_Image.Dispose();
            ho_ResultRegions_1.Dispose();
            ho_ResultRegions_2.Dispose();
            ho_ResultRegions_3.Dispose();
        }

        private void ucFilter_Closing_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_Filter.Closing = ucFilter_Closing.Value;
             if (bReadingPara)
                return;
            if (My.ho_Image == null)
                return;
            if (MyLensCrack_AVI.m_Cutting.ReionCutting == null)
                return;
            if(MyLensCrack_AVI.m_Filter.UnionResultRegion==null)
                return;
            HObject ho_RegionClosing = new HObject();
            HWindow Window = hWindowControl1.HalconWindow;
            ho_RegionClosing.Dispose();
            HOperatorSet.ClosingCircle(MyLensCrack_AVI.m_Filter.UnionResultRegion, out ho_RegionClosing, MyLensCrack_AVI.m_Filter.Closing);

            My.ho_Image.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("red");
            ho_RegionClosing.DispObj(Window);
        }


        private void ucFilter_Select_Area_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_Filter.Select_Area = ucFilter_Select_Area.Value;
            if (bReadingPara)
                return;
            if (My.ho_Image == null)
                return;
            if (MyLensCrack_AVI.m_Cutting.ReionCutting == null)
                return;
            if (MyLensCrack_AVI.m_Filter.UnionResultRegion == null)
                return;
            HObject ho_RegionClosing = new HObject(),ho_ConnectedRegions = new HObject(),ho_SelectedRegions = new HObject();
            HWindow Window = hWindowControl1.HalconWindow;

            ho_RegionClosing.Dispose();
            HOperatorSet.ClosingCircle(MyLensCrack_AVI.m_Filter.UnionResultRegion, out ho_RegionClosing, MyLensCrack_AVI.m_Filter.Closing);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", MyLensCrack_AVI.m_Filter.Select_Area, 99999999);

            My.ho_Image.DispObj(Window);
            Window.SetDraw("margin");
            Window.SetColor("red");
            ho_SelectedRegions.DispObj(Window);
        }

        private void btnFilterSave_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("m_Filter", "Closing", MyLensCrack_AVI.m_Filter.Closing.ToString(), Path);
            IniFile.Write("m_Filter", "Select_Area", MyLensCrack_AVI.m_Filter.Select_Area.ToString(), Path);
        }

        public void ImageProcoss(HWindow Window,HObject TheImage,out HObject ho_ResultImage,out string sResult)
        {
            sResult = "";
            ho_ResultImage = new HObject();

            if (TheImage == null)
                return;

            HObject ho_Image = new HObject(),ho_Region = new HObject();
            HObject ho_Circle = new HObject(),ho_OuterCircle = new HObject(),ho_InnerCircle = new HObject(), ho_ReducedImage = new HObject(), ho_ImageMedian = new HObject();
            HObject ho_ImageEmphasize = new HObject(), ho_ModelContours = new HObject(), ho_TransContours = new HObject();
            HObject ho_RegionUnion = new HObject(), ho_RegionDifference = new HObject();
            HTuple hv_ModelID = new HTuple(), hv_HomMat2D = new HTuple();
            HTuple hv_Height = new HTuple(), hv_Width = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple(), hv_Radius = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
            HTuple hv_CircleRow = new HTuple(), hv_CircleColumn = new HTuple(), hv_CircleRadius = new HTuple();

            
            ho_Image.Dispose();
            ho_Image = TheImage.CopyObj(1, -1);
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
            ho_Circle.Dispose();
            HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, MyLensCrack_AVI.Radius_First);
            ho_ReducedImage.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
            ho_ImageMedian.Dispose();
            HOperatorSet.MedianRect(ho_ReducedImage, out ho_ImageMedian, 10, 10);
            ho_ImageEmphasize.Dispose();
            HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);
            #region 無料模組匹配,判斷是否無料
            //try
            //{
            //    string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model_Miss";
            //    HOperatorSet.ReadShapeModel(Path, out hv_ModelID);
            //}
            //catch
            //{
            //    MessageBox.Show("先建立無料模組!");
            //}
            //try
            //{
            //    HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
            //    HOperatorSet.FindShapeModel(ho_ReducedImage, hv_ModelID, (new HTuple(-30)).TupleRad(), (new HTuple(30)).TupleRad(), 0.5, 0, 0.5, "least_squares", (new HTuple(6)).TupleConcat(1), 0.75, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            //    set_display_font(Window, 20, "mono", "true", "false");
            //    if (hv_Score.TupleGreater(0) != 0)
            //    {
            //        HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
            //        HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle, 0, 0, out hv_HomMat2D);
            //        HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row, hv_Column, out hv_HomMat2D);
            //        ho_TransContours.Dispose();
            //        HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
            //        ho_Region.Dispose();
            //        HOperatorSet.GenRegionContourXld(ho_TransContours, out ho_Region, "filled");
            //        ho_RegionUnion.Dispose();
            //        HOperatorSet.Union1(ho_Region, out ho_RegionUnion);
            //        HOperatorSet.SmallestCircle(ho_RegionUnion, out hv_CircleRow, out hv_CircleColumn, out hv_CircleRadius);
            //        HOperatorSet.GenCircle(out ho_Circle, hv_CircleRow, hv_CircleColumn, hv_CircleRadius);
            //        HOperatorSet.SetDraw(Window, "margin");
            //        ho_Image.DispObj(Window);
            //        Window.SetColor("yellow");
            //        ho_Circle.DispObj(Window);

            //        disp_message(Window, "Miss", "", 50, 0, "blue", "false");
            //        HOperatorSet.ClearShapeModel(hv_ModelID);
            //        ho_ResultImage.Dispose();
            //        HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
            //        sResult = "Miss";
            //        return;
            //    }
            //    HOperatorSet.ClearShapeModel(hv_ModelID);
            //}
            //catch
            //{
            //}
            #endregion
            #region 模組匹配,找初始圓心
            try
            {
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_Model_First";
                HOperatorSet.ReadShapeModel(Path, out hv_ModelID);
            }
            catch
            {
                MessageBox.Show("先建立產品模組!");
            }
            try
            {
                HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                HOperatorSet.FindShapeModel(ho_ImageEmphasize, hv_ModelID, (new HTuple(-30)).TupleRad(), (new HTuple(30)).TupleRad(), 0.5, 0, 0.5, "least_squares", (new HTuple(6)).TupleConcat(1), 0.75, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                set_display_font(Window, 20, "mono", "true", "false");
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
                }
                else
                {
                    sResult = "Miss";
                    ho_Image.DispObj(Window);
                    disp_message(Window, "Miss", "", 50, 0, "blue", "false");
                    ho_ResultImage.Dispose();
                    HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                    HOperatorSet.ClearShapeModel(hv_ModelID);
                    return;
                }
                HOperatorSet.ClearShapeModel(hv_ModelID);
            }
            catch
            {
                sResult = "Miss";
                ho_Image.DispObj(Window);
                disp_message(Window, "Miss", "", 50, 0, "blue", "false");
                ho_ResultImage.Dispose();
                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                HOperatorSet.ClearShapeModel(hv_ModelID);
                return;
            }
            #endregion
            #region 找準確圓心
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject(), ho_CrossCenter = new HObject();
            HTuple hv_Length = new HTuple(), hv_MeasureThreshold = new HTuple(), hv_MeasureTransition = new HTuple(), hv_MeasureSelect = new HTuple();
            HTuple hv_ResultRow = new HTuple(), hv_ResultColumn = new HTuple(), hv_ResultRadius = new HTuple();
             try
            {
                
                hv_Radius = MyLensCrack_AVI.m_First.Radius;
                hv_Length = MyLensCrack_AVI.m_First.Length;
                hv_MeasureThreshold = MyLensCrack_AVI.m_First.MeasureThreshold;
                hv_MeasureTransition = MyLensCrack_AVI.m_First.MeasureTransition;
                hv_MeasureSelect = MyLensCrack_AVI.m_First.MeasureSelect;
                 //第一次先找圓心
                //HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
                ho_UsedEdges.Dispose(); ho_Contour.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_ReducedImage, out ho_UsedEdges, out ho_Contour, out ho_ResultContours,
                    out ho_CrossCenter, hv_CircleRow, hv_CircleColumn, hv_Radius, hv_Length, hv_MeasureThreshold, hv_MeasureTransition,
                    hv_MeasureSelect, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);
            }
            catch
            {
                sResult = "NG2";
                ho_Image.DispObj(Window);
                disp_message(Window, "圓心判斷錯誤!", "", 50, 0, "red", "false");
                ho_ResultImage.Dispose();
                HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                HOperatorSet.ClearShapeModel(hv_ModelID);
                return;
            }
             ho_UsedEdges.Dispose();
             ho_Contour.Dispose();
             ho_ResultContours.Dispose();
             ho_CrossCenter.Dispose();

            #endregion
            #region 找剪口
             HObject ho_ReionCutting = new HObject();
             try
             {
                 hv_Row = 0;
                 hv_Column = 0;
                 HObject ho_SelectedRegions_Cutting = new HObject(), ho_RegionFillUp = new HObject();
                 HObject ho_ReduceImage = new HObject(), ho_RegionOpening = new HObject(), ho_ConnectedRegions = new HObject(), ho_SelectedRegions_Cutting2 = new HObject();
                 HTuple hv_Area = new HTuple();

                 ho_Circle.Dispose();
                 HOperatorSet.GenCircle(out ho_OuterCircle, hv_ResultRow, hv_ResultColumn, MyLensCrack_AVI.OuterRadius_Lens);
                 ho_InnerCircle.Dispose();
                 HOperatorSet.GenCircle(out ho_InnerCircle, hv_ResultRow, hv_ResultColumn, MyLensCrack_AVI.InnerRadius_Lens);
                 ho_RegionDifference.Dispose();
                 HOperatorSet.Difference(ho_OuterCircle, ho_InnerCircle, out ho_RegionDifference);
                 ho_ReduceImage.Dispose();
                 HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ReduceImage);
                 //剪口灰度設定
                 ho_Region.Dispose();
                 HOperatorSet.Threshold(ho_ReduceImage, out ho_Region, MyLensCrack_AVI.m_Cutting.Gray, 255);
                 ho_RegionFillUp.Dispose();
                 HOperatorSet.FillUp(ho_Region, out  ho_RegionFillUp);
                 ho_RegionFillUp.Dispose();
                 HOperatorSet.FillUp(ho_Region, out  ho_RegionFillUp);
                 {
                     HObject ExpTmpOutVar_0;
                     HOperatorSet.Difference(ho_RegionDifference, ho_RegionFillUp, out ExpTmpOutVar_0);
                     ho_RegionDifference.Dispose();
                     ho_RegionDifference = ExpTmpOutVar_0;
                 }
                 ho_RegionOpening.Dispose();
                 HOperatorSet.OpeningCircle(ho_RegionDifference, out ho_RegionOpening, 3.5);
                 ho_ConnectedRegions.Dispose();
                 HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                 ho_SelectedRegions_Cutting.Dispose();
                 HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions_Cutting, (new HTuple("rect2_len1")).TupleConcat("rect2_len2"), "and", (new HTuple(MyLensCrack_AVI.m_Cutting.Length1_Lower)).TupleConcat(MyLensCrack_AVI.m_Cutting.Length2_Lower), (new HTuple(MyLensCrack_AVI.m_Cutting.Length1_Upper)).TupleConcat(MyLensCrack_AVI.m_Cutting.Length2_Upper));

                HOperatorSet.RegionFeatures(ho_SelectedRegions_Cutting, "area", out hv_Area);
                ho_ReionCutting.Dispose();
                HOperatorSet.SelectShape(ho_SelectedRegions_Cutting, out ho_SelectedRegions_Cutting2, "area", "and", hv_Area.TupleMax(), hv_Area.TupleMax());
                ho_ReionCutting.Dispose();
                HOperatorSet.DilationCircle(ho_SelectedRegions_Cutting2, out ho_ReionCutting, MyLensCrack_AVI.m_Cutting.CuttingDilation);
                
                 ho_Region.Dispose();
                 ho_RegionOpening.Dispose();
                 ho_ConnectedRegions.Dispose();
                 ho_SelectedRegions_Cutting2.Dispose();
             }
             catch
             {
                 sResult = "NG2";
                 ho_Image.DispObj(Window);
                 disp_message(Window, "找剪口錯誤!", "", 50, 0, "red", "false");
                 ho_ResultImage.Dispose();
                 HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                 return;
             }
            #endregion
            #region 檢測
             HObject ho_ResultRegions_1 = new HObject(), ho_ResultRegions_2 = new HObject(), ho_ResultRegions_3 = new HObject();
             HObject UnionResultRegion1 = new HObject(), UnionResultRegion = new HObject();

             try
             {
                 ho_ResultRegions_1.Dispose();
                 m_polor_trans_Masking_rft_dyn_threshold(ho_Image, ho_ReionCutting, out ho_ResultRegions_1, hv_ResultRow, hv_ResultColumn, MyLensCrack_AVI.m_RegionDetection_1.OuterRadous, MyLensCrack_AVI.m_RegionDetection_1.InnerRadous, 100, 100, 5, 50, MyLensCrack_AVI.m_RegionDetection_1.Offset_Dark, MyLensCrack_AVI.m_RegionDetection_1.Offset_Light);

                 ho_ResultRegions_2.Dispose();
                 m_polor_trans_Masking_rft_dyn_threshold(ho_Image, ho_ReionCutting, out ho_ResultRegions_2,hv_ResultRow, hv_ResultColumn, MyLensCrack_AVI.m_RegionDetection_2.OuterRadous, MyLensCrack_AVI.m_RegionDetection_2.InnerRadous, 100, 100, 5, 50, MyLensCrack_AVI.m_RegionDetection_2.Offset_Dark, MyLensCrack_AVI.m_RegionDetection_2.Offset_Light);

                 ho_ResultRegions_3.Dispose();
                 m_polor_trans_Masking_rft_dyn_threshold(ho_Image, ho_ReionCutting, out ho_ResultRegions_3, hv_ResultRow, hv_ResultColumn, MyLensCrack_AVI.m_RegionDetection_3.OuterRadous, MyLensCrack_AVI.m_RegionDetection_3.InnerRadous, 100, 100, 5, 50, MyLensCrack_AVI.m_RegionDetection_3.Offset_Dark, MyLensCrack_AVI.m_RegionDetection_3.Offset_Light);
                 UnionResultRegion1.Dispose();
                 HOperatorSet.Union2(ho_ResultRegions_1, ho_ResultRegions_2, out UnionResultRegion1);
                 UnionResultRegion.Dispose();
                 HOperatorSet.Union2(UnionResultRegion1, ho_ResultRegions_3, out UnionResultRegion);
             }
             catch
             {
                 sResult = "NG2";
                 ho_Image.DispObj(Window);
                 disp_message(Window, "檢測區域出現錯誤!", "", 50, 0, "red", "false");
                 ho_ResultImage.Dispose();
                 HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                 HOperatorSet.ClearShapeModel(hv_ModelID);
                 return;
             }
             ho_ResultRegions_1.Dispose();
             ho_ResultRegions_2.Dispose();
             ho_ResultRegions_3.Dispose();
            #endregion
            #region 篩選檢測結果
             HObject ho_RegionClosing = new HObject(), ho_SelectedResultRegions = new HObject();
             try
             {
                 HObject ho_ConnectedRegions = new HObject();
                 ho_RegionClosing.Dispose();
                 HOperatorSet.ClosingCircle(UnionResultRegion, out ho_RegionClosing, MyLensCrack_AVI.m_Filter.Closing);
                 ho_ConnectedRegions.Dispose();
                 HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
                 ho_SelectedResultRegions.Dispose();
                 HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedResultRegions, "area", "and", MyLensCrack_AVI.m_Filter.Select_Area, 99999999);
             
             }
             catch
             {
                 ho_Image.DispObj(Window);
                 disp_message(Window, "篩選結果錯誤!", "", 50, 0, "red", "false");
                 ho_ResultImage.Dispose();
                 HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
                 HOperatorSet.ClearShapeModel(hv_ModelID);
                 return;
             }
            #endregion
            #region 判斷結果
             HObject ho_UnionRegion = new HObject();
             HTuple hv_ResultArea = new HTuple();
             HOperatorSet.Union1(ho_SelectedResultRegions, out  ho_UnionRegion);
             HOperatorSet.RegionFeatures(ho_UnionRegion,"area", out hv_ResultArea);
            
             ho_Image.DispObj(Window);
             Window.SetColor("red");
             Window.SetDraw("margin");
             ho_UnionRegion.DispObj(Window);
             if (hv_ResultArea.Length > 0)
             {
                 //ho_ReionCutting.DispObj(Window);
                 sResult = "NG";
                 disp_message(Window, "鏡裂NG!", "", 50, 0, "red", "false");
                 ho_ResultImage.Dispose();
                 
                 HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
             }
             else
             {
                 sResult = "OK";
                 disp_message(Window, "OK!", "", 50, 0, "green", "false");
                 ho_ResultImage.Dispose();
                 HOperatorSet.DumpWindowImage(out ho_ResultImage, Window);
             }
            
            ho_Image.Dispose();
            ho_UnionRegion.Dispose();
            #endregion
        }

        private void btnFindCenterAndCutting_Click(object sender, EventArgs e)
        {
            if (My.ho_Image == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_UsedEdges = new HObject(), ho_Contour = new HObject(), ho_ResultContours = new HObject();
            int Result = 0;
            FindCenter(Window, My.ho_Image, out Result, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out MyLensCrack_AVI.m_First.hv_ResultRow, out MyLensCrack_AVI.m_First.hv_ResultColumn);

            HObject ho_SelectedRegions = new HObject(), ho_SelectedRegions2 = new HObject(), ho_RegionDilation = new HObject();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_RegionLength1 = new HTuple(), hv_RegionLength2 = new HTuple();
            ho_SelectedRegions.Dispose(); ho_SelectedRegions2.Dispose();
            try
            {
                FindCutting(Window, My.ho_Image, out ho_SelectedRegions, out ho_SelectedRegions2, out hv_Row, out hv_Column, out hv_RegionLength1, out hv_RegionLength2);
                ho_RegionDilation.Dispose();
                HOperatorSet.DilationCircle(ho_SelectedRegions2, out ho_RegionDilation, MyLensCrack_AVI.m_Cutting.CuttingDilation);
                
                HOperatorSet.DispObj(My.ho_Image, Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                ho_ResultContours.DispObj(Window);
                Window.SetDraw("fill");
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_RegionDilation, Window);
                MyLensCrack_AVI.m_Cutting.ReionCutting = ho_RegionDilation.CopyObj(1, -1);
            }
            catch
            {
            }
        }

        private void ucCuttingDilation_ValueChanged(int CurrentValue)
        {
            MyLensCrack_AVI.m_Cutting.CuttingDilation = ucCuttingDilation.Value;
            HObject ho_SelectedRegions = new HObject(), ho_SelectedRegions2 = new HObject(),ho_RegionDilation = new HObject();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_RegionLength1 = new HTuple(), hv_RegionLength2 = new HTuple();
            HWindow Window = hWindowControl1.HalconWindow;
            ho_SelectedRegions.Dispose(); ho_SelectedRegions2.Dispose();
            try
            {
                FindCutting(Window, My.ho_Image, out ho_SelectedRegions, out ho_SelectedRegions2, out hv_Row, out hv_Column, out hv_RegionLength1, out hv_RegionLength2);
                ho_RegionDilation.Dispose();
                HOperatorSet.DilationCircle(ho_SelectedRegions2, out ho_RegionDilation, MyLensCrack_AVI.m_Cutting.CuttingDilation);
                HOperatorSet.DispObj(My.ho_Image, Window);
                Window.SetDraw("margin");
                Window.SetColor("yellow");
                HOperatorSet.DispObj(ho_RegionDilation, Window);
                MyLensCrack_AVI.m_Cutting.ReionCutting = ho_RegionDilation.CopyObj(1, -1);
            }
            catch
            {
            }
        }

        

       

      
       
       
        
    }
}
