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
using System.Threading;

namespace Detecting_System
{
    public partial class FrmSomaDetectionVS : Form
    {
        FrmParent parent;
        public FrmSomaDetectionVS(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;
            InitializeComponent();
        }
        //檢測區域半徑
        public static double dReduceRadius = 1;
        //抓圓中的灰度閥值
        public static double dGraythreshold = 1;
        //正面半徑
        public static double dRadiusSet_Front = 1;
        //正面寬度
        public static double dWidthSet_Front = 1;
        //反面半徑
        public static double dRadiusSet_Reverse = 1;
        //反面寬度
        public static double dWidthSet_Reverse = 1;
        //無料半徑
        public static double dRadiusSet_Miss = 1;
        //無料寬度
        public static double dWidthSet_Miss = 1;
        //外徑長度
        public static double dLengthTD = 1;
        //外徑灰度差異
        public static double dMeasureThresholdTD = 20;
        //外徑白找黑或黑找白
        public static string sGenParamValueTD = "negative";
        //內徑長度
        public static double dLengthID = 1;
        //內徑灰度差異
        public static double dMeasureThresholdID = 20;
        //內徑白找黑或黑找白
        public static string sGenParamValueID = "negative";
        //外徑縮小
        public static double dTopDaimShrink = 1;
        //內徑放大
        public static double dInnerDiamMagnify = 1;
        //缺陷檢測灰度值
        public static double dGraySet = 1;
        //過濾小面積
        public static double dFilterArea = 1;
        //刮痕
        public static double dScratchLength = 1;
        //大面積
        public static double dLargeArea = 1;
        //數量設置
        public static double dNumber = 1;
        

        #region halcon參數1

        
        // Procedures 
        // External procedures 
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

        // Chapter: Filters / Arithmetic
        // Short Description: Scale the gray values of an image from the interval [Min,Max] to [0,255] 
        public void scale_image_range(HObject ho_Image, out HObject ho_ImageScaled, HTuple hv_Min,
            HTuple hv_Max)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageSelected = null, ho_SelectedChannel = null;
            HObject ho_LowerRegion = null, ho_UpperRegion = null, ho_ImageSelectedScaled = null;

            // Local copy input parameter variables 
            HObject ho_Image_COPY_INP_TMP;
            ho_Image_COPY_INP_TMP = ho_Image.CopyObj(1, -1);



            // Local control variables 

            HTuple hv_LowerLimit = new HTuple(), hv_UpperLimit = new HTuple();
            HTuple hv_Mult = null, hv_Add = null, hv_NumImages = null;
            HTuple hv_ImageIndex = null, hv_Channels = new HTuple();
            HTuple hv_ChannelIndex = new HTuple(), hv_MinGray = new HTuple();
            HTuple hv_MaxGray = new HTuple(), hv_Range = new HTuple();
            HTuple hv_Max_COPY_INP_TMP = hv_Max.Clone();
            HTuple hv_Min_COPY_INP_TMP = hv_Min.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ImageSelected);
            HOperatorSet.GenEmptyObj(out ho_SelectedChannel);
            HOperatorSet.GenEmptyObj(out ho_LowerRegion);
            HOperatorSet.GenEmptyObj(out ho_UpperRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageSelectedScaled);
            try
            {
                //Convenience procedure to scale the gray values of the
                //input image Image from the interval [Min,Max]
                //to the interval [0,255] (default).
                //Gray values < 0 or > 255 (after scaling) are clipped.
                //
                //If the image shall be scaled to an interval different from [0,255],
                //this can be achieved by passing tuples with 2 values [From, To]
                //as Min and Max.
                //Example:
                //scale_image_range(Image:ImageScaled:[100,50],[200,250])
                //maps the gray values of Image from the interval [100,200] to [50,250].
                //All other gray values will be clipped.
                //
                //input parameters:
                //Image: the input image
                //Min: the minimum gray value which will be mapped to 0
                //     If a tuple with two values is given, the first value will
                //     be mapped to the second value.
                //Max: The maximum gray value which will be mapped to 255
                //     If a tuple with two values is given, the first value will
                //     be mapped to the second value.
                //
                //Output parameter:
                //ImageScale: the resulting scaled image.
                //
                if ((int)(new HTuple((new HTuple(hv_Min_COPY_INP_TMP.TupleLength())).TupleEqual(
                    2))) != 0)
                {
                    hv_LowerLimit = hv_Min_COPY_INP_TMP.TupleSelect(1);
                    hv_Min_COPY_INP_TMP = hv_Min_COPY_INP_TMP.TupleSelect(0);
                }
                else
                {
                    hv_LowerLimit = 0.0;
                }
                if ((int)(new HTuple((new HTuple(hv_Max_COPY_INP_TMP.TupleLength())).TupleEqual(
                    2))) != 0)
                {
                    hv_UpperLimit = hv_Max_COPY_INP_TMP.TupleSelect(1);
                    hv_Max_COPY_INP_TMP = hv_Max_COPY_INP_TMP.TupleSelect(0);
                }
                else
                {
                    hv_UpperLimit = 255.0;
                }
                //
                //Calculate scaling parameters.
                hv_Mult = (((hv_UpperLimit - hv_LowerLimit)).TupleReal()) / (hv_Max_COPY_INP_TMP - hv_Min_COPY_INP_TMP);
                hv_Add = ((-hv_Mult) * hv_Min_COPY_INP_TMP) + hv_LowerLimit;
                //
                //Scale image.
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ScaleImage(ho_Image_COPY_INP_TMP, out ExpTmpOutVar_0, hv_Mult,
                        hv_Add);
                    ho_Image_COPY_INP_TMP.Dispose();
                    ho_Image_COPY_INP_TMP = ExpTmpOutVar_0;
                }
                //
                //Clip gray values if necessary.
                //This must be done for each image and channel separately.
                ho_ImageScaled.Dispose();
                HOperatorSet.GenEmptyObj(out ho_ImageScaled);
                HOperatorSet.CountObj(ho_Image_COPY_INP_TMP, out hv_NumImages);
                HTuple end_val49 = hv_NumImages;
                HTuple step_val49 = 1;
                for (hv_ImageIndex = 1; hv_ImageIndex.Continue(end_val49, step_val49); hv_ImageIndex = hv_ImageIndex.TupleAdd(step_val49))
                {
                    ho_ImageSelected.Dispose();
                    HOperatorSet.SelectObj(ho_Image_COPY_INP_TMP, out ho_ImageSelected, hv_ImageIndex);
                    HOperatorSet.CountChannels(ho_ImageSelected, out hv_Channels);
                    HTuple end_val52 = hv_Channels;
                    HTuple step_val52 = 1;
                    for (hv_ChannelIndex = 1; hv_ChannelIndex.Continue(end_val52, step_val52); hv_ChannelIndex = hv_ChannelIndex.TupleAdd(step_val52))
                    {
                        ho_SelectedChannel.Dispose();
                        HOperatorSet.AccessChannel(ho_ImageSelected, out ho_SelectedChannel, hv_ChannelIndex);
                        HOperatorSet.MinMaxGray(ho_SelectedChannel, ho_SelectedChannel, 0, out hv_MinGray,
                            out hv_MaxGray, out hv_Range);
                        ho_LowerRegion.Dispose();
                        HOperatorSet.Threshold(ho_SelectedChannel, out ho_LowerRegion, ((hv_MinGray.TupleConcat(
                            hv_LowerLimit))).TupleMin(), hv_LowerLimit);
                        ho_UpperRegion.Dispose();
                        HOperatorSet.Threshold(ho_SelectedChannel, out ho_UpperRegion, hv_UpperLimit,
                            ((hv_UpperLimit.TupleConcat(hv_MaxGray))).TupleMax());
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.PaintRegion(ho_LowerRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                                hv_LowerLimit, "fill");
                            ho_SelectedChannel.Dispose();
                            ho_SelectedChannel = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.PaintRegion(ho_UpperRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                                hv_UpperLimit, "fill");
                            ho_SelectedChannel.Dispose();
                            ho_SelectedChannel = ExpTmpOutVar_0;
                        }
                        if ((int)(new HTuple(hv_ChannelIndex.TupleEqual(1))) != 0)
                        {
                            ho_ImageSelectedScaled.Dispose();
                            HOperatorSet.CopyObj(ho_SelectedChannel, out ho_ImageSelectedScaled,
                                1, 1);
                        }
                        else
                        {
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.AppendChannel(ho_ImageSelectedScaled, ho_SelectedChannel,
                                    out ExpTmpOutVar_0);
                                ho_ImageSelectedScaled.Dispose();
                                ho_ImageSelectedScaled = ExpTmpOutVar_0;
                            }
                        }
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_ImageScaled, ho_ImageSelectedScaled, out ExpTmpOutVar_0
                            );
                        ho_ImageScaled.Dispose();
                        ho_ImageScaled = ExpTmpOutVar_0;
                    }
                }
                ho_Image_COPY_INP_TMP.Dispose();
                ho_ImageSelected.Dispose();
                ho_SelectedChannel.Dispose();
                ho_LowerRegion.Dispose();
                ho_UpperRegion.Dispose();
                ho_ImageSelectedScaled.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Image_COPY_INP_TMP.Dispose();
                ho_ImageSelected.Dispose();
                ho_SelectedChannel.Dispose();
                ho_LowerRegion.Dispose();
                ho_UpperRegion.Dispose();
                ho_ImageSelectedScaled.Dispose();

                throw HDevExpDefaultException;
            }
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

        HObject ho_Image, ho_Circle, ho_ReducedImage;
        HObject ho_Region, ho_RegionOpening, ho_Connection, ho_SelectedRegions0;
        HObject ho_Cross, ho_Circle0, ho_CircleTop, ho_CircleInner;
        HObject ho_ModelContourID = null, ho_MeasureContourID = null;
        HObject ho_ContourID = null, ho_CrossCenterID = null, ho_ContoursID = null;
        HObject ho_CrossID = null, ho_UsedEdgesID = null, ho_ResultContoursID = null;
        HObject ho_ModelContourTD = null, ho_MeasureContourTD = null;
        HObject ho_ContourTD = null, ho_CrossCenterTD = null, ho_ContoursTD = null;
        HObject ho_CrossTD = null, ho_UsedEdgesTD = null, ho_ResultContoursTD = null;
        HObject ho_RegionID = null, ho_RegionDilationID = null, ho_RegionTD = null;
        HObject ho_RegionErosionTD = null, ho_RegionDifference = null;
        HObject ho_ImageReduced = null, ho_ImageScaled = null, ho_ImageMean = null;
        HObject ho_Region1 = null, ho_RegionClosing = null, ho_ConnectedRegions = null;
        HObject ho_SelectedRegionsR = null, ho_SelectedRegionsS = null;
        HObject ho_RegionUnionS = null, ho_RegionDifference1 = null;
        HObject ho_SelectedRegionsL = null, ho_RegionUnionL = null;
        HObject ho_RegionDifference2 = null, ho_RegionUnionR = null;
        // Local control variables 

        HTuple hv_WindowHandle = new HTuple(), hv_Width = null;
        HTuple hv_Height = null, hv_FirstRadius = null, hv_Radious0 = null;
        HTuple hv_GraySetting = null, hv_RadiusSet_Front = null;
        HTuple hv_WidthSet_Front = null, hv_RadiusSet_Reverse = null;
        HTuple hv_WidthSet_Reverse = null, hv_RadiusSet_Miss = null;
        HTuple hv_WidthSet_Miss = null, hv_Area0 = null, hv_Row0 = null;
        HTuple hv_Column0 = null, hv_Value = null, hv_InnerDiameter_Soma = new HTuple();
        HTuple hv_TopDiameter_Soma = new HTuple(), hv_MetrologyHandleID = new HTuple();
        HTuple hv_circleIndicesID = new HTuple(), hv_RowID = new HTuple();
        HTuple hv_ColumnID = new HTuple(), hv_GenParamValueID = new HTuple();
        HTuple hv_Length1ID = new HTuple(), hv_WidthID = new HTuple();
        HTuple hv_Measure_ThresholdID = new HTuple(), hv_circleParameterID = new HTuple();
        HTuple hv_Row1ID = new HTuple(), hv_Column1ID = new HTuple();
        HTuple hv_UsedRowID = new HTuple(), hv_UsedColumnID = new HTuple();
        HTuple hv_ResultRowID = new HTuple(), hv_ResultColumnID = new HTuple();
        HTuple hv_ResultRadiusID = new HTuple(), hv_StartPhiID = new HTuple();
        HTuple hv_EndPhiID = new HTuple(), hv_PointOrderID = new HTuple();
        HTuple hv_Exception = new HTuple(), hv_MetrologyHandleTD = new HTuple();
        HTuple hv_circleIndicesTD = new HTuple(), hv_RowTD = new HTuple();
        HTuple hv_ColumnTD = new HTuple(), hv_GenParamValueTD = new HTuple();
        HTuple hv_Length1TD = new HTuple(), hv_WidthTD = new HTuple();
        HTuple hv_Measure_ThresholdTD = new HTuple(), hv_circleParameterTD = new HTuple();
        HTuple hv_Row1TD = new HTuple(), hv_Column1TD = new HTuple();
        HTuple hv_UsedRowTD = new HTuple(), hv_UsedColumnTD = new HTuple();
        HTuple hv_ResultRowTD = new HTuple(), hv_ResultColumnTD = new HTuple();
        HTuple hv_ResultRadiusTD = new HTuple(), hv_StartPhiTD = new HTuple();
        HTuple hv_EndPhiTD = new HTuple(), hv_PointOrderTD = new HTuple();
        HTuple hv_DilationSetID = new HTuple(), hv_ErosionSetTD = new HTuple();
        HTuple hv_Min = new HTuple(), hv_Max = new HTuple(), hv_Range = new HTuple();
        HTuple hv_GraySet = new HTuple(), hv_UnderSizeSet = new HTuple();
        HTuple hv_ScratchLengthSet = new HTuple(), hv_NumberS = new HTuple();
        HTuple hv_LargeAreaSet = new HTuple(), hv_NumberL = new HTuple();
        HTuple hv_NumberR = new HTuple(), hv_NumberSetR = new HTuple();
        #endregion

        private void FrmSomaDetection_JM_Load(object sender, EventArgs e)
        {
            ReadPara();
        }

        public void ReadPara()
        {
            cbTestDefect.Checked = Convert.ToBoolean(My.Soma.TestDefect);
            dReduceRadius = My.dReduceRadius;
            dGraythreshold = My.dGraythreshold;
            dRadiusSet_Miss = My.Soma.dRadiusSet_Miss;
            dWidthSet_Miss = My.Soma.dWidthSet_Miss;
            dRadiusSet_Reverse = My.Soma.dRadiusSet_Reverse;
            dWidthSet_Reverse = My.Soma.dWidthSet_Reverse;
            dRadiusSet_Front = My.Soma.dRadiusSet_Front;
            dWidthSet_Front = My.Soma.dWidthSet_Front;
            dLengthTD = My.Soma.dLengthTD;
            dMeasureThresholdTD = My.Soma.dMeasureThresholdTD;
            sGenParamValueTD = My.Soma.sGenParamValueTD;
            dLengthID = My.Soma.dLengthID;
            dMeasureThresholdID = My.Soma.dMeasureThresholdID;
            sGenParamValueID = My.Soma.sGenParamValueID;
            dTopDaimShrink = My.Soma.dTopDaimShrink;
            dInnerDiamMagnify = My.Soma.dInnerDiamMagnify;
            dGraySet = My.Soma.dGraySet;
            dFilterArea = My.Soma.dFilterArea;
            dScratchLength = My.Soma.dScratchLength;
            dLargeArea = My.Soma.dLargeArea;
            dNumber = My.Soma.dNumber;



            tbDetectionRadius.Value = (int)My.dReduceRadius;
            tbGraythreshold.Value = (int)My.dGraythreshold;
            tbMissRadius.Value = (int)My.Soma.dRadiusSet_Miss;
            tbMissWidth.Value = (int)My.Soma.dWidthSet_Miss;
            tbReverseRadius.Value = (int)My.Soma.dRadiusSet_Reverse;
            tbReverseWidth.Value = (int)My.Soma.dWidthSet_Reverse;
            tbFrontRadius.Value = (int)My.Soma.dRadiusSet_Front;
            tbFrontWidth.Value = (int)My.Soma.dWidthSet_Front;
            tbLengthTD.Value = (int)My.Soma.dLengthTD;
            if (My.Soma.sGenParamValueTD == "positive")
            {
                tbBlackToWhiteTD.Value = (int)My.Soma.dMeasureThresholdTD;
            }
            else
            {
                tbWhiteToBlackTD.Value = (int)My.Soma.dMeasureThresholdTD;
            }
            tbLengthID.Value = (int)My.Soma.dLengthID;
            if (My.Soma.sGenParamValueID == "positive")
            {
                tbBlackToWhiteID.Value = (int)My.Soma.dMeasureThresholdID;
            }
            else
            {
                tbWhiteToBlackID.Value = (int)My.Soma.dMeasureThresholdID;
            }
            tbTopDaimShrink.Value = (int)My.Soma.dTopDaimShrink;
            tbInnerDiamMagnify.Value = (int)My.Soma.dInnerDiamMagnify;
            tbGraySet.Value = (int)My.Soma.dGraySet;
            tbFilterArea.Value = (int)My.Soma.dFilterArea;
            tbScratchLength.Value = (int)My.Soma.dScratchLength;
            tbLargeArea.Value = (int)My.Soma.dLargeArea;
            nudNumber.Value = (int)My.Soma.dNumber;
        
        }
        private void btnOneShot_Click(object sender, EventArgs e)
        {
            parent.OneShot();
        }

        private void btnContinueShot_Click(object sender, EventArgs e)
        {
            if (!My.ContinueShot)
            {
                parent.OneShot();
                My.ContinueShot = true;
            }
            else
            {
                My.ContinueShot = false;
            }
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

        private void btnImageSave_Click(object sender, EventArgs e)
        {
            if (Vision.VisionResult[Tray.n] == "OK")
            {
                if (Sys.OptionOK)
                {
                    //儲存擷取當前畫面圖片
                    string pathOK = Sys.ImageSavePath + "\\" + DateTime.Now.ToString("yyyy_MM_dd");
                    if (!Directory.Exists(pathOK))
                    {
                        Directory.CreateDirectory(pathOK);
                    }
                    //儲存圖片
                    HOperatorSet.WriteImage(Vision.Images_1[Tray.n], "png", 0, pathOK + "\\" + DateTime.Now.ToString("HH_mm_ss_f") + "OK");
                }
            }
            else if (Vision.VisionResult[Tray.n] == "NG")
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
                    HOperatorSet.WriteImage(Vision.Images_1[Tray.n], "png", 0, pathNG + "\\" + DateTime.Now.ToString("HH_mm_ss_f") + "NG");
                }
            }
            else//Miss
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
                    HOperatorSet.WriteImage(Vision.Images_1[Tray.n], "png", 0, pathNG + "\\" + DateTime.Now.ToString("HH_mm_ss_f") + "Miss");
                }
            }
            if (Sys.OptionOriginal)
            {
                //儲存原始圖片
                string pathOriginal = Sys.ImageSavePath + "\\" + DateTime.Now.ToString("yyyy_MM_dd");
                if (!Directory.Exists(pathOriginal))
                {
                    Directory.CreateDirectory(pathOriginal);
                }
                HOperatorSet.WriteImage(Vision.ImagesOriginal_1[Tray.n], "png", 0, pathOriginal + "\\" + DateTime.Now.ToString("HH_mm_ss_f"));
            }
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
                HOperatorSet.Threshold(ho_Image, out ho_Region, 0, tbGraythreshold.Value);
                HOperatorSet.DispObj(ho_Region, hv_ExpDefaultWinHandle);
            }
            catch
            {
                //MessageBox.Show("error");
            }
            Thread.Sleep(1);
        }

        private void btnFirstCenter_Click(object sender, EventArgs e)
        {
            CatchFirstCenter(hWindowControl1.HalconWindow);
        }

        private void tbMissRadius_ValueChanged(object sender, EventArgs e)
        {
            nudMissRadius.Value = tbMissRadius.Value;
        }

        private void nudMissRadius_ValueChanged(object sender, EventArgs e)
        {
            dRadiusSet_Miss = tbMissRadius.Value = Convert.ToInt32(nudMissRadius.Value);
            RingSet(hWindowControl1.HalconWindow, dRadiusSet_Miss, dWidthSet_Miss);
        }

        private void tbMissWidth_ValueChanged(object sender, EventArgs e)
        {
            nudMissWidth.Value = tbMissWidth.Value;
        }

        private void nudMissWidth_ValueChanged(object sender, EventArgs e)
        {
            dWidthSet_Miss = tbMissWidth.Value = Convert.ToInt32(nudMissWidth.Value);
            RingSet(hWindowControl1.HalconWindow, dRadiusSet_Miss, dWidthSet_Miss);
        }

        private void tbFrontRadius_ValueChanged(object sender, EventArgs e)
        {
            nudFrontRadius.Value = tbFrontRadius.Value;
        }

        private void nudFrontRadius_ValueChanged(object sender, EventArgs e)
        {
            dRadiusSet_Front = tbFrontRadius.Value = Convert.ToInt32(nudFrontRadius.Value);
            RingSet(hWindowControl1.HalconWindow, dRadiusSet_Front, dWidthSet_Front);
        }

        private void tbFrontWidth_ValueChanged(object sender, EventArgs e)
        {
            nudFrontWidth.Value = tbFrontWidth.Value;
        }

        private void nudFrontWidth_ValueChanged(object sender, EventArgs e)
        {
            dWidthSet_Front = tbFrontWidth.Value = Convert.ToInt32(nudFrontWidth.Value);
            RingSet(hWindowControl1.HalconWindow, dRadiusSet_Front, dWidthSet_Front);
        }

        private void tbReverseRadius_ValueChanged(object sender, EventArgs e)
        {
            nudReverseRadius.Value = tbReverseRadius.Value;
        }

        private void nudReverseRadius_ValueChanged(object sender, EventArgs e)
        {
            dRadiusSet_Reverse = tbReverseRadius.Value = Convert.ToInt32(nudReverseRadius.Value);
            RingSet(hWindowControl1.HalconWindow, dRadiusSet_Reverse, dWidthSet_Reverse);
        }

        private void tbReverseWidth_ValueChanged(object sender, EventArgs e)
        {
            nudReverseWidth.Value = tbReverseWidth.Value;
        }

        private void nudReverseWidth_ValueChanged(object sender, EventArgs e)
        {
            dWidthSet_Reverse = tbReverseWidth.Value = Convert.ToInt32(nudReverseWidth.Value);
            RingSet(hWindowControl1.HalconWindow, dRadiusSet_Reverse, dWidthSet_Reverse);
        }

        public void RingSet(HWindow hWindowControl,HTuple Radius,HTuple Width)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            try
            {
                ho_Image = My.ho_Image;
                hWindowControl1.HalconWindow.ClearWindow();
                
                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                ho_Circle0.Dispose();
                HOperatorSet.GenCircle(out ho_Circle0, hv_Row0, hv_Column0, Radius);
                ho_CircleTop.Dispose();
                HOperatorSet.GenCircle(out ho_CircleTop, hv_Row0, hv_Column0, Radius + Width);
                ho_CircleInner.Dispose();
                HOperatorSet.SetDraw(hWindowControl, "margin");
                HOperatorSet.GenCircle(out ho_CircleInner, hv_Row0, hv_Column0, Radius - Width);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                HOperatorSet.DispObj(ho_Circle0, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.DispObj(ho_CircleTop, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_CircleInner, hWindowControl1.HalconWindow);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            My.Soma.dRadiusSet_Miss = dRadiusSet_Miss;
            My.Soma.dWidthSet_Miss = dWidthSet_Miss;
            My.Soma.dRadiusSet_Reverse = dRadiusSet_Reverse;
            My.Soma.dWidthSet_Reverse = dWidthSet_Reverse;
            My.Soma.dRadiusSet_Front = dRadiusSet_Front;
            My.Soma.dWidthSet_Front = dWidthSet_Front;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "RadiusSet_Miss", My.Soma.dRadiusSet_Miss.ToString(), Path);
            IniFile.Write("Setting", "WidthSet_Miss", My.Soma.dWidthSet_Miss.ToString(), Path);
            IniFile.Write("Setting", "RadiusSet_Reverse", My.Soma.dRadiusSet_Reverse.ToString(), Path);
            IniFile.Write("Setting", "WidthSet_Reverse", My.Soma.dWidthSet_Reverse.ToString(), Path);
            IniFile.Write("Setting", "RadiusSet_Front", My.Soma.dRadiusSet_Front.ToString(), Path);
            IniFile.Write("Setting", "WidthSet_Front", My.Soma.dWidthSet_Front.ToString(), Path);
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            My.dReduceRadius = dReduceRadius;
            My.dGraythreshold = dGraythreshold;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "ReduceRadius", My.dReduceRadius.ToString(), Path);
            IniFile.Write("Setting", "Graythreshold", My.dGraythreshold.ToString(), Path);
        }

        private void btnFirstCenter2_Click(object sender, EventArgs e)
        {
            CatchFirstCenter(hWindowControl1.HalconWindow);
        }
       
        private void tbLengthTD_ValueChanged(object sender, EventArgs e)
        {
            nudLengthTD.Value = tbLengthTD.Value;
        }

        private void nudLengthTD_ValueChanged(object sender, EventArgs e)
        {
            dLengthTD = tbLengthTD.Value = Convert.ToInt32(nudLengthTD.Value);
            try
            {
                //這段要另外用的
                CatchDiameterTD(hWindowControl1.HalconWindow);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void tbBlackToWhiteTD_ValueChanged(object sender, EventArgs e)
        {
            nudBlackToWhiteTD.Value = tbBlackToWhiteTD.Value;
        }

        private void nudBlackToWhiteTD_ValueChanged(object sender, EventArgs e)
        {
            tbWhiteToBlackTD.Value = 1;
            dMeasureThresholdTD = tbBlackToWhiteTD.Value = Convert.ToInt32(nudBlackToWhiteTD.Value);
            sGenParamValueTD = "positive";
            try
            {
                //這段要另外用的
                CatchDiameterTD(hWindowControl1.HalconWindow);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void tbWhiteToBlackTD_ValueChanged(object sender, EventArgs e)
        {
            nudWhiteToBlackTD.Value = tbWhiteToBlackTD.Value;
        }

        private void nudWhiteToBlackTD_ValueChanged(object sender, EventArgs e)
        {
            tbBlackToWhiteID.Value = 1;
            dMeasureThresholdTD = tbWhiteToBlackTD.Value = Convert.ToInt32(nudWhiteToBlackTD.Value);
            sGenParamValueTD = "negative";
            try
            {
                //這段要另外用的
                CatchDiameterTD(hWindowControl1.HalconWindow);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void btnCatchTD_Click(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_Circle);
            try
            {
                //這段要另外用的
                CatchDiameterTD(hWindowControl1.HalconWindow);
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                ho_Circle0.Dispose();
                HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "cyan");
                HOperatorSet.GenCircle(out ho_Circle, hv_ResultRowTD, hv_ResultColumnTD, hv_ResultRadiusTD);
                HOperatorSet.DispObj(ho_Circle, hWindowControl1.HalconWindow);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void btnSaveSetTD_Click(object sender, EventArgs e)
        {
            My.Soma.dLengthTD = dLengthTD;
            My.Soma.dMeasureThresholdTD = dMeasureThresholdTD;
            My.Soma.sGenParamValueTD = sGenParamValueTD;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "LengthTD", My.Soma.dLengthTD.ToString(), Path);
            IniFile.Write("Setting", "MeasureThresholdTD", My.Soma.dMeasureThresholdTD.ToString(), Path);
            IniFile.Write("Setting", "GenParamValueTD", My.Soma.sGenParamValueTD.ToString(), Path);
        }

        private void tbLengthID_ValueChanged(object sender, EventArgs e)
        {
            nudLengthID.Value = tbLengthID.Value;
        }

        private void nudLengthID_ValueChanged(object sender, EventArgs e)
        {
            dLengthID = tbLengthID.Value = Convert.ToInt32(nudLengthID.Value);
            try
            {
                //這段要另外用的
                CatchDiameterID(hWindowControl1.HalconWindow);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void tbBlackToWhiteID_ValueChanged(object sender, EventArgs e)
        {
            nudBlackToWhiteID.Value = tbBlackToWhiteID.Value;
        }

        private void nudBlackToWhiteID_ValueChanged(object sender, EventArgs e)
        {
            tbWhiteToBlackID.Value = 1;
            dMeasureThresholdID = tbBlackToWhiteID.Value = Convert.ToInt32(nudBlackToWhiteID.Value);
            sGenParamValueID = "positive";
            try
            {
                //這段要另外用的
                CatchDiameterID(hWindowControl1.HalconWindow);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void tbWhiteToBlackID_ValueChanged(object sender, EventArgs e)
        {
            nudWhiteToBlackID.Value = tbWhiteToBlackID.Value;
        }

        private void nudWhiteToBlackID_ValueChanged(object sender, EventArgs e)
        {
            tbBlackToWhiteID.Value = 1;
            dMeasureThresholdID = tbWhiteToBlackID.Value = Convert.ToInt32(nudWhiteToBlackID.Value);
            sGenParamValueID = "negative";
            try
            {
                //這段要另外用的
                CatchDiameterID(hWindowControl1.HalconWindow);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void btnCatchID_Click(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_Circle);
            try
            {
                //這段要另外用的
                CatchDiameterID(hWindowControl1.HalconWindow);
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "cyan");
                ho_Circle0.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_ResultRowID, hv_ResultColumnID, hv_ResultRadiusID);
                HOperatorSet.DispObj(ho_Circle, hWindowControl1.HalconWindow);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void btnSaveSetID_Click(object sender, EventArgs e)
        {
            My.Soma.dLengthID = dLengthID;
            My.Soma.dMeasureThresholdID = dMeasureThresholdID;
            My.Soma.sGenParamValueID = sGenParamValueID;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "LengthID", My.Soma.dLengthID.ToString(), Path);
            IniFile.Write("Setting", "MeasureThresholdID", My.Soma.dMeasureThresholdID.ToString(), Path);
            IniFile.Write("Setting", "GenParamValueID", My.Soma.sGenParamValueID.ToString(), Path);
        }

        public void CatchDiameterID(HWindow hWindow)
        {
            HOperatorSet.GenEmptyObj(out ho_ModelContourID);
            HOperatorSet.GenEmptyObj(out ho_MeasureContourID);
            HOperatorSet.GenEmptyObj(out ho_ContourID);
            HOperatorSet.GenEmptyObj(out ho_CrossCenterID);
            HOperatorSet.GenEmptyObj(out ho_ContoursID);
            HOperatorSet.GenEmptyObj(out ho_CrossID);
            HOperatorSet.GenEmptyObj(out ho_UsedEdgesID);
            HOperatorSet.GenEmptyObj(out ho_ResultContoursID);
            try
            {
                hWindow.ClearWindow();
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindow;

                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandleID);
                //抓鏡片圓心
                HOperatorSet.AddMetrologyObjectGeneric(hv_MetrologyHandleID, "circle",
                    ((hv_Row0.TupleConcat(hv_Column0))).TupleConcat(dRadiusSet_Reverse),
                    25, 5, 1, 30, new HTuple(), new HTuple(), out hv_circleIndicesID);
                ho_ModelContourID.Dispose();
                HOperatorSet.GetMetrologyObjectModelContour(out ho_ModelContourID, hv_MetrologyHandleID,"all", 1.5);
                ho_MeasureContourID.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureContourID, hv_MetrologyHandleID,"all", "all", out hv_RowID, out hv_ColumnID);
                //白找黑('negative')或黑找白('positive')
                hv_GenParamValueID = sGenParamValueID;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandleID, hv_circleIndicesID,"measure_transition", hv_GenParamValueID);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandleID, hv_circleIndicesID,"measure_select", "last");
                //長度
                hv_Length1ID = dLengthID;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandleID, hv_circleIndicesID,"measure_length1", hv_Length1ID);
                //寬度
                hv_WidthID = 2;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandleID, hv_circleIndicesID,"measure_length2", hv_WidthID);
                //灰度差異
                hv_Measure_ThresholdID = dMeasureThresholdID;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandleID, hv_circleIndicesID,"measure_threshold", hv_Measure_ThresholdID);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandleID, hv_circleIndicesID,"min_score", 0.2);
                HOperatorSet.ApplyMetrologyModel(ho_ReducedImage, hv_MetrologyHandleID);
                ho_ContourID.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_ContourID, hv_MetrologyHandleID,"all", "positive", out hv_RowID, out hv_ColumnID);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.DispObj(ho_ContourID, hv_ExpDefaultWinHandle);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandleID, hv_circleIndicesID,"all", "result_type", "all_param", out hv_circleParameterID);
                ho_CrossCenterID.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_CrossCenterID, hv_circleParameterID.TupleSelect(0), hv_circleParameterID.TupleSelect(1), 20, 0.785398);
                ho_ContoursID.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_ContoursID, hv_MetrologyHandleID,"all", "all", 1.5);
                ho_ContourID.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_ContourID, hv_MetrologyHandleID,"all", "all", out hv_Row1ID, out hv_Column1ID);
                ho_CrossID.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_CrossID, hv_Row1ID, hv_Column1ID,6, 0.785398);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandleID, "all", "all","used_edges", "row", out hv_UsedRowID);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandleID, "all", "all","used_edges", "column", out hv_UsedColumnID);
                ho_UsedEdgesID.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_UsedEdgesID, hv_UsedRowID, hv_UsedColumnID,20, (new HTuple(45)).TupleRad());
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                HOperatorSet.DispObj(ho_UsedEdgesID, hv_ExpDefaultWinHandle);
                ho_ResultContoursID.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_ResultContoursID, 
                    hv_MetrologyHandleID, "all", "all", 1.5);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.DispObj(ho_ResultContoursID, hv_ExpDefaultWinHandle);
                HOperatorSet.FitCircleContourXld(ho_ResultContoursID, "algebraic", -1, 
                    0, 0, 3, 2, out hv_ResultRowID, out hv_ResultColumnID, out hv_ResultRadiusID, 
                    out hv_StartPhiID, out hv_EndPhiID, out hv_PointOrderID);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandleID);
          }
          // catch (Exception) 
          catch (HalconException HDevExpDefaultException1)
          {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
          }
        }

        public void CatchDiameterTD(HWindow hWindow)
        {
            HOperatorSet.GenEmptyObj(out ho_ModelContourTD);
            HOperatorSet.GenEmptyObj(out ho_MeasureContourTD);
            HOperatorSet.GenEmptyObj(out ho_ContourTD);
            HOperatorSet.GenEmptyObj(out ho_CrossCenterTD);
            HOperatorSet.GenEmptyObj(out ho_ContoursTD);
            HOperatorSet.GenEmptyObj(out ho_CrossTD);
            HOperatorSet.GenEmptyObj(out ho_UsedEdgesTD);
            HOperatorSet.GenEmptyObj(out ho_ResultContoursTD);
            try
            {
                hWindow.ClearWindow();
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindow;

                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandleTD);
                //抓鏡片圓心
                HOperatorSet.AddMetrologyObjectGeneric(hv_MetrologyHandleTD, "circle",
                    ((hv_Row0.TupleConcat(hv_Column0))).TupleConcat(dRadiusSet_Front),
                    25, 5, 1, 30, new HTuple(), new HTuple(), out hv_circleIndicesTD);
                ho_ModelContourTD.Dispose();
                HOperatorSet.GetMetrologyObjectModelContour(out ho_ModelContourTD, hv_MetrologyHandleTD, "all", 1.5);
                ho_MeasureContourTD.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureContourTD, hv_MetrologyHandleTD, "all", "all", out hv_RowTD, out hv_ColumnTD);
                //白找黑('negative')或黑找白('positive')
                hv_GenParamValueTD = sGenParamValueTD;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandleTD, hv_circleIndicesTD, "measure_transition", hv_GenParamValueTD);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandleTD, hv_circleIndicesTD, "measure_select", "last");
                //長度
                hv_Length1TD = dLengthTD;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandleTD, hv_circleIndicesTD, "measure_length1", hv_Length1TD);
                //寬度
                hv_WidthTD = 5;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandleTD, hv_circleIndicesTD, "measure_length2", hv_WidthTD);
                //灰度差異
                hv_Measure_ThresholdTD = dMeasureThresholdTD;
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandleTD, hv_circleIndicesTD, "measure_threshold", hv_Measure_ThresholdTD);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandleTD, hv_circleIndicesTD, "min_score", 0.2);
                HOperatorSet.ApplyMetrologyModel(ho_ReducedImage, hv_MetrologyHandleTD);
                ho_ContourTD.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_ContourTD, hv_MetrologyHandleTD, "all", "positive", out hv_RowTD, out hv_ColumnTD);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.DispObj(ho_ContourTD, hv_ExpDefaultWinHandle);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandleTD, hv_circleIndicesTD, "all", "result_type", "all_param", out hv_circleParameterTD);
                ho_CrossCenterTD.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_CrossCenterTD, hv_circleParameterTD.TupleSelect(0), hv_circleParameterTD.TupleSelect(1), 20, 0.785398);
                ho_ContoursTD.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_ContoursTD, hv_MetrologyHandleTD, "all", "all", 1.5);
                ho_ContourTD.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_ContourTD, hv_MetrologyHandleTD, "all", "all", out hv_Row1TD, out hv_Column1TD);
                ho_CrossTD.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_CrossTD, hv_Row1TD, hv_Column1TD, 6, 0.785398);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandleTD, "all", "all", "used_edges", "row", out hv_UsedRowTD);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandleTD, "all", "all", "used_edges", "column", out hv_UsedColumnTD);
                ho_UsedEdgesTD.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_UsedEdgesTD, hv_UsedRowTD, hv_UsedColumnTD, 20, (new HTuple(45)).TupleRad());
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                HOperatorSet.DispObj(ho_UsedEdgesTD, hv_ExpDefaultWinHandle);
                ho_ResultContoursTD.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_ResultContoursTD,
                    hv_MetrologyHandleTD, "all", "all", 1.5);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.DispObj(ho_ResultContoursTD, hv_ExpDefaultWinHandle);
                HOperatorSet.FitCircleContourXld(ho_ResultContoursTD, "algebraic", -1,
                    0, 0, 3, 2, out hv_ResultRowTD, out hv_ResultColumnTD, out hv_ResultRadiusTD,
                    out hv_StartPhiTD, out hv_EndPhiTD, out hv_PointOrderTD);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandleTD);
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
            }
        }

        private void cbTestDefect_CheckedChanged(object sender, EventArgs e)
        {
            My.Soma.TestDefect = (cbTestDefect.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "TestDefect", My.Soma.TestDefect.ToString(), Path);
        }

        public void CatchFirstCenter(HWindow hWindow)
        {
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_Connection);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions0);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Circle0);
            HOperatorSet.GenEmptyObj(out ho_CircleTop);
            HOperatorSet.GenEmptyObj(out ho_CircleInner);
            try
            {
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindow;
                hWindow.ClearWindow();
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                //畫檢視範圍
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //灰度值設定
                hv_GraySetting = dGraythreshold;
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_Region, 0, hv_GraySetting);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_Region, out ho_RegionOpening, 5);
                ho_Connection.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_Connection);
                HOperatorSet.AreaCenter(ho_Connection, out hv_Area0, out hv_Row0, out hv_Column0);
                ho_SelectedRegions0.Dispose();
                HOperatorSet.SelectShape(ho_Connection, out ho_SelectedRegions0, "area", "and", hv_Area0.TupleMax(), 999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions0, out hv_Area0, out hv_Row0, out hv_Column0);
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row0, hv_Column0, 10, 0.785398);
                HOperatorSet.DispObj(ho_Cross, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
        }

        private void btnCatchDiamIDTD_Click(object sender, EventArgs e)
        {
            CatchFirstCenter(hWindowControl1.HalconWindow);
            CatchDiameterTD(hWindowControl1.HalconWindow);
            CatchDiameterID(hWindowControl1.HalconWindow);
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
            ho_Circle0.Dispose();
            HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "cyan");
            HOperatorSet.GenCircle(out ho_CircleTop, hv_ResultRowTD, hv_ResultColumnTD, hv_ResultRadiusTD);
            HOperatorSet.GenCircle(out ho_CircleInner, hv_ResultRowID, hv_ResultColumnID, hv_ResultRadiusID);
            HOperatorSet.DispObj(ho_CircleTop, hWindowControl1.HalconWindow);
            HOperatorSet.DispObj(ho_CircleInner, hWindowControl1.HalconWindow);
        }

        private void tbTopDaimShrink_ValueChanged(object sender, EventArgs e)
        {
            nudTopDaimShrink.Value = tbTopDaimShrink.Value;
        }

        private void nudTopDaimShrink_ValueChanged(object sender, EventArgs e)
        {
            dInnerDiamMagnify = tbInnerDiamMagnify.Value;
            dTopDaimShrink = tbTopDaimShrink.Value = Convert.ToInt32(nudTopDaimShrink.Value);

            HOperatorSet.GenEmptyObj(out ho_RegionErosionTD);
            HOperatorSet.GenEmptyObj(out ho_RegionDilationID);
            try
            {
                DefectDetecting(hWindowControl1.HalconWindow);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                HOperatorSet.DispObj(ho_ImageMean, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_RegionDilationID, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_RegionErosionTD, hWindowControl1.HalconWindow);
            }
            catch
            {
            }
        }

        private void tbInnerDiamMagnify_ValueChanged(object sender, EventArgs e)
        {
            nudInnerDiamMagnify.Value = tbInnerDiamMagnify.Value;
        }

        private void nudInnerDiamMagnify_ValueChanged(object sender, EventArgs e)
        {
            dTopDaimShrink = tbTopDaimShrink.Value;
            dInnerDiamMagnify = tbInnerDiamMagnify.Value = Convert.ToInt32(nudInnerDiamMagnify.Value);
            
            try
            {
                DefectDetecting(hWindowControl1.HalconWindow);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                HOperatorSet.DispObj(ho_ImageMean, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_RegionDilationID, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_RegionErosionTD, hWindowControl1.HalconWindow);
            }
            catch
            {
            }
        }

        private void tbGraySet_ValueChanged(object sender, EventArgs e)
        {
            nudGraySet.Value = tbGraySet.Value;
        }

        private void nudGraySet_ValueChanged(object sender, EventArgs e)
        {
            dGraySet = tbGraySet.Value = Convert.ToInt32(nudGraySet.Value);
            try
            {
                DefectDetecting(hWindowControl1.HalconWindow);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                HOperatorSet.DispObj(ho_ImageMean, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Region1, hWindowControl1.HalconWindow);
            }
            catch
            {
            }
        }

        private void tbFilterArea_ValueChanged(object sender, EventArgs e)
        {
            nudFilterArea.Value = tbFilterArea.Value;
        }

        private void nudFilterArea_ValueChanged(object sender, EventArgs e)
        {
            dFilterArea = tbFilterArea.Value = Convert.ToInt32(nudFilterArea.Value);
            try
            {
                DefectDetecting(hWindowControl1.HalconWindow);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                HOperatorSet.DispObj(ho_ImageMean, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_SelectedRegionsR, hWindowControl1.HalconWindow);
            }
            catch
            {
            }
        }

        private void tbScratchLength_ValueChanged(object sender, EventArgs e)
        {
            nudScratchLength.Value = tbScratchLength.Value;
        }

        private void nudScratchLength_ValueChanged(object sender, EventArgs e)
        {
            dScratchLength = tbScratchLength.Value = Convert.ToInt32(nudScratchLength.Value);
            try
            {
                DefectDetecting(hWindowControl1.HalconWindow);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                HOperatorSet.DispObj(ho_ImageMean, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_RegionUnionS, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                HOperatorSet.DispObj(ho_RegionUnionL, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_RegionUnionR, hWindowControl1.HalconWindow);
            }
            catch
            {
            }
        }

        private void tbLargeArea_ValueChanged(object sender, EventArgs e)
        {
            nudLargeArea.Value = tbLargeArea.Value;
        }

        private void nudLargeArea_ValueChanged(object sender, EventArgs e)
        {
            dLargeArea = tbLargeArea.Value = Convert.ToInt32(nudLargeArea.Value);
            try
            {
                DefectDetecting(hWindowControl1.HalconWindow);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                HOperatorSet.DispObj(ho_ImageMean, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_RegionUnionS, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                HOperatorSet.DispObj(ho_RegionUnionL, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_RegionUnionR, hWindowControl1.HalconWindow);
            }
            catch
            {
            }
        }

        private void nudNumber_ValueChanged(object sender, EventArgs e)
        {
            dNumber = Convert.ToInt32(nudNumber.Value);
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            My.Soma.dTopDaimShrink = dTopDaimShrink;
            My.Soma.dInnerDiamMagnify = dInnerDiamMagnify;
            My.Soma.dGraySet = dGraySet;
            My.Soma.dFilterArea = dFilterArea;
            My.Soma.dScratchLength = dScratchLength;
            My.Soma.dLargeArea = dLargeArea;
            My.Soma.dNumber = dNumber;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "TopDaimShrink", My.Soma.dTopDaimShrink.ToString(), Path);
            IniFile.Write("Setting", "InnerDiamMagnify", My.Soma.dInnerDiamMagnify.ToString(), Path);
            IniFile.Write("Setting", "GraySet", My.Soma.dGraySet.ToString(), Path);
            IniFile.Write("Setting", "FilterArea", My.Soma.dFilterArea.ToString(), Path);
            IniFile.Write("Setting", "ScratchLength", My.Soma.dScratchLength.ToString(), Path);
            IniFile.Write("Setting", "LargeArea", My.Soma.dLargeArea.ToString(), Path);
            IniFile.Write("Setting", "Number", My.Soma.dNumber.ToString(), Path);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_ReducedImage);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_Connection);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions0);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Circle0);
            HOperatorSet.GenEmptyObj(out ho_CircleTop);
            HOperatorSet.GenEmptyObj(out ho_CircleInner);
            try
            {
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);

                //draw_circle (WindowHandle, FirstRow, FirstColumn1, FirstRadius)

                //畫檢視範圍
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //如果正面大約170pixel 反面 75pixel,無料 147pixel以此判斷有無料正反面
                hv_GraySetting = dGraythreshold;

                //正面
                hv_RadiusSet_Front = dRadiusSet_Front;
                hv_WidthSet_Front = dWidthSet_Front;
                //背面
                hv_RadiusSet_Reverse = dRadiusSet_Reverse;
                hv_WidthSet_Reverse = dWidthSet_Reverse;
                //無料
                hv_RadiusSet_Miss = dRadiusSet_Miss;
                hv_WidthSet_Miss = dWidthSet_Miss;

                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_Region, 0, hv_GraySetting);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_Region, out ho_RegionOpening, 5);
                ho_Connection.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_Connection);
                HOperatorSet.AreaCenter(ho_Connection, out hv_Area0, out hv_Row0, out hv_Column0);
                ho_SelectedRegions0.Dispose();
                HOperatorSet.SelectShape(ho_Connection, out ho_SelectedRegions0, "area", "and",
                    hv_Area0.TupleMax(), 999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions0, out hv_Area0, out hv_Row0, out hv_Column0);
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row0, hv_Column0, 10, 0.785398);
                HOperatorSet.DispObj(ho_Cross, hv_ExpDefaultWinHandle);
                HOperatorSet.RegionFeatures(ho_SelectedRegions0, "outer_radius", out hv_Value);
                //有無料
                set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");

                ho_Circle0.Dispose();
                HOperatorSet.GenCircle(out ho_Circle0, hv_Row0, hv_Column0, hv_RadiusSet_Front);
                ho_CircleTop.Dispose();
                HOperatorSet.GenCircle(out ho_CircleTop, hv_Row0, hv_Column0, hv_RadiusSet_Front + hv_WidthSet_Front);
                ho_CircleInner.Dispose();
                HOperatorSet.GenCircle(out ho_CircleInner, hv_Row0, hv_Column0, hv_RadiusSet_Front - hv_WidthSet_Front);

                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                HOperatorSet.DispObj(ho_Circle0, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_CircleTop, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_CircleInner, hv_ExpDefaultWinHandle);

                if ((int)((new HTuple(hv_Value.TupleLess(hv_RadiusSet_Miss + hv_WidthSet_Miss))).TupleAnd(
                    new HTuple(hv_Value.TupleGreater(hv_RadiusSet_Miss - hv_WidthSet_Miss)))) != 0)
                {
                    //無料
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1500, 100);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                }
                else
                {
                    //有料
                    //正面
                    if ((int)((new HTuple(hv_Value.TupleLess(hv_RadiusSet_Front + hv_WidthSet_Front))).TupleAnd(
                        new HTuple(hv_Value.TupleGreater(hv_RadiusSet_Front - hv_WidthSet_Front)))) != 0)
                    {
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1500, 100);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "正面");
                    }
                    //反面
                    if ((int)((new HTuple(hv_Value.TupleLess(hv_RadiusSet_Reverse + 10))).TupleAnd(
                        new HTuple(hv_Value.TupleGreater(hv_RadiusSet_Reverse - 10)))) != 0)
                    {
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1500, 100);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "反面");
                    }
                }
            }
            catch
            {
            }
        }
        public void DefectDetecting(HWindow hWindowControl)
        {
            HOperatorSet.GenEmptyObj(out ho_RegionID);
            HOperatorSet.GenEmptyObj(out ho_RegionDilationID);
            HOperatorSet.GenEmptyObj(out ho_RegionTD);
            HOperatorSet.GenEmptyObj(out ho_RegionErosionTD);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegionsR);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegionsS);
            HOperatorSet.GenEmptyObj(out ho_RegionUnionS);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegionsL);
            HOperatorSet.GenEmptyObj(out ho_RegionUnionL);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference2);
            HOperatorSet.GenEmptyObj(out ho_RegionUnionR);
            try
            {
                hv_ExpDefaultWinHandle = hWindowControl;


                //內徑轉region
                ho_RegionID.Dispose();
                HOperatorSet.GenRegionContourXld(ho_ResultContoursID, out ho_RegionID,
                    "filled");
                //內徑膨脹,過濾掉過曝區域
                hv_DilationSetID = dInnerDiamMagnify;
                ho_RegionDilationID.Dispose();
                HOperatorSet.DilationCircle(ho_RegionID, out ho_RegionDilationID, hv_DilationSetID);
                //外徑轉Region
                ho_RegionTD.Dispose();
                HOperatorSet.GenRegionContourXld(ho_ResultContoursTD, out ho_RegionTD, "filled");
                //外徑腐蝕,過濾掉過曝區域
                hv_ErosionSetTD = dTopDaimShrink;
                ho_RegionErosionTD.Dispose();
                HOperatorSet.ErosionCircle(ho_RegionTD, out ho_RegionErosionTD, hv_ErosionSetTD);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_RegionErosionTD, ho_RegionDilationID, out ho_RegionDifference);

                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_ReducedImage, ho_RegionDifference, out ho_ImageReduced);

                HOperatorSet.MinMaxGray(ho_RegionDifference, ho_ImageReduced, 0, out hv_Min, out hv_Max, out hv_Range);
                //將圖片最黑的黑色部分拉到0,增強對比度,白色就不拉,怕有些沒有傷痕的被影響
                ho_ImageScaled.Dispose();
                scale_image_range(ho_ImageReduced, out ho_ImageScaled, hv_Min, 255);
                ho_ImageMean.Dispose();
                HOperatorSet.MeanImage(ho_ImageScaled, out ho_ImageMean, 3, 3);
                hv_GraySet = dGraySet;
                ho_Region1.Dispose();
                HOperatorSet.Threshold(ho_ImageScaled, out ho_Region1, hv_GraySet, 255);
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_Region1, out ho_RegionClosing, 2);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);

                //處理particle
                //0.過濾過小的小點點 remain剩下 簡寫R
                hv_UnderSizeSet = dFilterArea;
                ho_SelectedRegionsR.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegionsR,
                    "area", "and", hv_UnderSizeSet, 99999);

                //1.分辨刮痕還是Particle
                //Scratch刮痕簡稱S
                hv_ScratchLengthSet = dScratchLength;
                ho_SelectedRegionsS.Dispose();
                HOperatorSet.SelectShape(ho_SelectedRegionsR, out ho_SelectedRegionsS,
                    "outer_radius", "and", hv_ScratchLengthSet, 99999);
                ho_RegionUnionS.Dispose();
                HOperatorSet.Union1(ho_SelectedRegionsS, out ho_RegionUnionS);
                HOperatorSet.CountObj(ho_RegionUnionS, out hv_NumberS);
                //減去刮痕
                ho_RegionDifference1.Dispose();
                HOperatorSet.Difference(ho_SelectedRegionsR, ho_SelectedRegionsS, out ho_RegionDifference1
                    );
                //2.抓出大面積  Large Area簡稱L
                hv_LargeAreaSet = dLargeArea;
                ho_SelectedRegionsL.Dispose();
                HOperatorSet.SelectShape(ho_RegionDifference1, out ho_SelectedRegionsL,
                    "area", "and", hv_LargeAreaSet, 99999);
                ho_RegionUnionL.Dispose();
                HOperatorSet.Union1(ho_SelectedRegionsL, out ho_RegionUnionL);
                HOperatorSet.CountObj(ho_RegionUnionL, out hv_NumberL);
                //減去大面積
                ho_RegionDifference2.Dispose();
                HOperatorSet.Difference(ho_RegionDifference1, ho_RegionUnionL, out ho_RegionDifference2
                    );
                HOperatorSet.CountObj(ho_RegionDifference2, out hv_NumberR);
                ho_RegionUnionR.Dispose();
                HOperatorSet.Union1(ho_RegionDifference2, out ho_RegionUnionR);
                hv_NumberSetR = dNumber;
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                if ((int)(new HTuple(hv_NumberS.TupleGreater(0))) != 0)
                {
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.DispObj(ho_RegionUnionS, hv_ExpDefaultWinHandle);
                }
                if ((int)(new HTuple(hv_NumberL.TupleGreater(0))) != 0)
                {
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange red");
                    HOperatorSet.DispObj(ho_RegionUnionL, hv_ExpDefaultWinHandle);
                }
                if ((int)(new HTuple(hv_NumberR.TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple(hv_NumberR.TupleGreater(hv_NumberSetR))) != 0)
                    {
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                        HOperatorSet.DispObj(ho_RegionUnionR, hv_ExpDefaultWinHandle);
                    }
                    else
                    {
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "cyan");
                        HOperatorSet.DispObj(ho_RegionUnionR, hv_ExpDefaultWinHandle);
                    }
                }
                else
                {
                    //如果都沒有剩的小點就不管
                }
            }
            catch
            {
            }
        }
        public void ImageProPlus(HWindow hWindowControl)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_ReducedImage);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_Connection);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions0);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Circle0);
            HOperatorSet.GenEmptyObj(out ho_CircleTop);
            HOperatorSet.GenEmptyObj(out ho_CircleInner);
            try
            {
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);

                //draw_circle (WindowHandle, FirstRow, FirstColumn1, FirstRadius)

                //畫檢視範圍
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //如果正面大約170pixel 反面 75pixel,無料 147pixel以此判斷有無料正反面
                hv_GraySetting = dGraythreshold;

                //正面
                hv_RadiusSet_Front = dRadiusSet_Front;
                hv_WidthSet_Front = dWidthSet_Front;
                //背面
                hv_RadiusSet_Reverse = dRadiusSet_Reverse;
                hv_WidthSet_Reverse = dWidthSet_Reverse;
                //無料
                hv_RadiusSet_Miss = dRadiusSet_Miss;
                hv_WidthSet_Miss = dWidthSet_Miss;

                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_Region, 0, hv_GraySetting);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_Region, out ho_RegionOpening, 5);
                ho_Connection.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_Connection);
                HOperatorSet.AreaCenter(ho_Connection, out hv_Area0, out hv_Row0, out hv_Column0);
                ho_SelectedRegions0.Dispose();
                HOperatorSet.SelectShape(ho_Connection, out ho_SelectedRegions0, "area", "and",
                    hv_Area0.TupleMax(), 999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions0, out hv_Area0, out hv_Row0, out hv_Column0);
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row0, hv_Column0, 10, 0.785398);
                HOperatorSet.DispObj(ho_Cross, hv_ExpDefaultWinHandle);
                HOperatorSet.RegionFeatures(ho_SelectedRegions0, "outer_radius", out hv_Value);
                //有無料
                set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");

                ho_Circle0.Dispose();
                HOperatorSet.GenCircle(out ho_Circle0, hv_Row0, hv_Column0, hv_RadiusSet_Front);
                ho_CircleTop.Dispose();
                HOperatorSet.GenCircle(out ho_CircleTop, hv_Row0, hv_Column0, hv_RadiusSet_Front + hv_WidthSet_Front);
                ho_CircleInner.Dispose();
                HOperatorSet.GenCircle(out ho_CircleInner, hv_Row0, hv_Column0, hv_RadiusSet_Front - hv_WidthSet_Front);

                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                HOperatorSet.DispObj(ho_Circle0, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_CircleTop, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_CircleInner, hv_ExpDefaultWinHandle);

                if ((int)((new HTuple(hv_Value.TupleLess(hv_RadiusSet_Miss + hv_WidthSet_Miss))).TupleAnd(
                    new HTuple(hv_Value.TupleGreater(hv_RadiusSet_Miss - hv_WidthSet_Miss)))) != 0)
                {
                    //無料
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1500, 100);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                    Vision.VisionResult[Tray.n] = "Miss";
                    //擷取當前畫面圖片為Image
                    HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                }
                else
                {
                    //有料
                    //正面
                    if ((int)((new HTuple(hv_Value.TupleLess(hv_RadiusSet_Front + hv_WidthSet_Front))).TupleAnd(
                        new HTuple(hv_Value.TupleGreater(hv_RadiusSet_Front - hv_WidthSet_Front)))) != 0)
                    {
                        if (My.Soma.TestDefect)
                        {
                            CatchDiameterTD(hWindowControl);
                            CatchDiameterID(hWindowControl);

                            DefectDetecting(hWindowControl);
                        }

                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1500, 100);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "正面");
                        if ((int)((new HTuple((new HTuple(hv_NumberS.TupleEqual(0))).TupleAnd(new HTuple(hv_NumberL.TupleEqual(
                            0))))).TupleAnd(new HTuple(hv_NumberR.TupleLessEqual(hv_NumberSetR)))) != 0)
                        {
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1600, 100);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "OK");

                        Vision.VisionResult[Tray.n] = "OK";
                        //擷取當前畫面圖片為Image
                        HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                        }
                        else
                        {
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1650, 100);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "NG");
                        Vision.VisionResult[Tray.n] = "NG2";
                        //擷取當前畫面圖片為Image
                        HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                        }
                    }
                    //反面
                    if ((int)((new HTuple(hv_Value.TupleLess(hv_RadiusSet_Reverse + 10))).TupleAnd(
                        new HTuple(hv_Value.TupleGreater(hv_RadiusSet_Reverse - 10)))) != 0)
                    {
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1500, 100);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "反面");
                        Vision.VisionResult[Tray.n] = "NG";
                        //擷取當前畫面圖片為Image
                        HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                    }
                }
            }
            catch
            {
                set_display_font(hv_ExpDefaultWinHandle, 20, "mono", "true", "false");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                Vision.VisionResult[Tray.n] = "Miss";
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 200);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                //擷取當前畫面圖片為Image
                HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
            }
            if (Tray.NowTray == 1)
            {
                Vision.Images_1[Tray.n] = ho_Image;
                Vision.ImagesOriginal_1[Tray.n] = My.ho_Image;
            }
            else if (Tray.NowTray == 2)
            {
                Vision.Images_2[Tray.n] = ho_Image;
                Vision.ImagesOriginal_2[Tray.n] = My.ho_Image;
            }
            WriteLog(Tray.n, Vision.VisionResult[Tray.n], Vision.VisionResult[Tray.n]);

            ho_Circle.Dispose();
            ho_ReducedImage.Dispose();
            ho_Region.Dispose();
            ho_RegionOpening.Dispose();
            ho_Connection.Dispose();
            ho_SelectedRegions0.Dispose();
            ho_Cross.Dispose();
            ho_Circle0.Dispose();
            ho_CircleTop.Dispose();
            ho_CircleInner.Dispose();
            ho_ModelContourID.Dispose();
            ho_MeasureContourID.Dispose();
            ho_ContourID.Dispose();
            ho_CrossCenterID.Dispose();
            ho_ContoursID.Dispose();
            ho_CrossID.Dispose();
            ho_UsedEdgesID.Dispose();
            ho_ResultContoursID.Dispose();
            ho_ModelContourTD.Dispose();
            ho_MeasureContourTD.Dispose();
            ho_ContourTD.Dispose();
            ho_CrossCenterTD.Dispose();
            ho_ContoursTD.Dispose();
            ho_CrossTD.Dispose();
            ho_UsedEdgesTD.Dispose();
            ho_ResultContoursTD.Dispose();
            ho_RegionID.Dispose();
            ho_RegionDilationID.Dispose();
            ho_RegionTD.Dispose();
            ho_RegionErosionTD.Dispose();
            ho_RegionDifference.Dispose();
            ho_ImageReduced.Dispose();
            ho_ImageScaled.Dispose();
            ho_ImageMean.Dispose();
            ho_Region1.Dispose();
            ho_RegionClosing.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegionsR.Dispose();
            ho_SelectedRegionsS.Dispose();
            ho_RegionUnionS.Dispose();
            ho_RegionDifference1.Dispose();
            ho_SelectedRegionsL.Dispose();
            ho_RegionUnionL.Dispose();
            ho_RegionDifference2.Dispose();
            ho_RegionUnionR.Dispose();
        }
        public void WriteLog(int n, string ResultOK, string Result)
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
                                 Tray.Barcode_1 + "-" + Tray.Barcode_2 + ".txt";
                    string Barcode = "";
                    if (Tray.NowTray == 1)
                    {
                        Barcode = Tray.Barcode_1;
                    }
                    else if (Tray.NowTray == 2)
                    {
                        Barcode = Tray.Barcode_2;
                    }

                    //不存在文件，创建先

                    if (!File.Exists(Log))
                    {
                        File.WriteAllText(Log, "Tray No.\tProgram ID\tMachine No.\tOutput Tray Barcode\tTime\tResult\tResultAngle\tResultAnglePF" +
                                         "\r\n");
                    }
                    //写result
                    using (StreamWriter sw = new StreamWriter(Log, true))
                    {
                        sw.WriteLine(string.Format("{0}.{1}", Tray.CurrentRow, Tray.CurrentColumn) + "\t" +
                                     Production.CurProduction + "\t" + Sys.MachineID + "\t" + Barcode + "\t" +
                                     DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" +
                                     ResultOK + "\t" + Result);
                    }
                }
                catch
                {
                }
            }
        }
       

        }
    }