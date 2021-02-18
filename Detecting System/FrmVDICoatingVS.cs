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
    public partial class FrmVDICoatingVS : Form
    {
        FrmParent parent;
        FrmRun Run;
        public FrmVDICoatingVS(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;
            InitializeComponent();
        }
        public static HTuple dFirstCircleRow = 1;
        public static HTuple dFirstCircleColumn = 1;
        public static HTuple dFirstCircleRadius = 1;

        public static double dMissGray = 1;
        public static double dMissArea = 1;
        public static double dMissOuterRadius = 1;

        public static double dGraythreshold = 1;
        public static double dLength = 1;
        public static double dMeasureThreshold = 1;
        public static string sGenParamValue = "negative";
        public static double dInitialRadius = 1;
        public static double dRadius_ID = 1;
        public static double dRadius_TD = 1;
        public static double dReduceRadius = 1;
        public static double dDefect_ID = 1;
        public static double dDefect_TD = 1;
        public static double dIgnore_ID = 1;
        public static double dIgnore_TD = 1;

        public static double dDefectGraySet = 1;
        public static double dFilterArea = 1;
        public static double dScratchLength = 1;
        public static double dLargeArea = 1;
        public static double dNumber = 1;

        public static double dUpperLimit_A = 1;
        public static double dLowerLimit_A = 1;
        public static double dUpperLimit_B = 1;
        public static double dLowerLimit_B = 1;

        public static double dRangeRadius = 1;
        public static double dCenterDistance = 1;
        public static double pixel2um = 4.4;
        public static string PointChoice = "first";
        public HTuple hv_ExpDefaultWinHandle;
        #region halcon參數1
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
        // Procedures 
        // External procedures 
        public void gen_circle_center(HObject ho_Image, out HObject ho_UsedEdges, out HObject ho_ResultContours,
            out HObject ho_CrossCenter, HTuple hv_InitialRow, HTuple hv_InitialColumn, HTuple hv_InitialRadius,
            HTuple hv_Length, HTuple hv_Measure_Threshold, HTuple hv_GenParamValue, HTuple hv_PointChoice,
            out HTuple hv_ResultRow, out HTuple hv_ResultColumn, out HTuple hv_ResultRadius)
        {
            // Local iconic variables 

            HObject ho_ModelContour, ho_MeasureContour;
            HObject ho_Contour, ho_Contours, ho_Cross;

            // Local control variables 

            HTuple hv_MetrologyHandle = null, hv_circleIndices = null;
            HTuple hv_Row = null, hv_Column = null, hv_circleParameter = null;
            HTuple hv_Row1 = null, hv_Column1 = null, hv_UsedRow = null;
            HTuple hv_UsedColumn = null, hv_StartPhi = null, hv_EndPhi = null;
            HTuple hv_PointOrder = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            HOperatorSet.GenEmptyObj(out ho_ModelContour);
            HOperatorSet.GenEmptyObj(out ho_MeasureContour);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.AddMetrologyObjectGeneric(hv_MetrologyHandle, "circle", ((hv_InitialRow.TupleConcat(
                    hv_InitialColumn))).TupleConcat(hv_InitialRadius), 25, 5, 1, 30, new HTuple(),
                    new HTuple(), out hv_circleIndices);
                ho_ModelContour.Dispose();
                HOperatorSet.GetMetrologyObjectModelContour(out ho_ModelContour, hv_MetrologyHandle,
                    "all", 1.5);
                ho_MeasureContour.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureContour, hv_MetrologyHandle,
                    "all", "all", out hv_Row, out hv_Column);
                //白找黑('negative')或黑找白('positive')
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices,
                    "measure_transition", hv_GenParamValue);
                //第一個點或最後一個點
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices,
                    "measure_select", hv_PointChoice);
                //長度
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices,
                    "measure_length1", hv_Length);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices,
                    "measure_length2", 5);
                //灰度差異
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
                HOperatorSet.DispObj(ho_ResultContours, hv_ExpDefaultWinHandle);
                HOperatorSet.FitCircleContourXld(ho_ResultContours, "algebraic", -1, 0, 0,
                    3, 2, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius, out hv_StartPhi,
                    out hv_EndPhi, out hv_PointOrder);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);

                ho_ModelContour.Dispose();
                ho_MeasureContour.Dispose();
                ho_Contour.Dispose();
                ho_Contours.Dispose();
                ho_Cross.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ModelContour.Dispose();
                ho_MeasureContour.Dispose();
                ho_Contour.Dispose();
                ho_Contours.Dispose();
                ho_Cross.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Procedures 
        // External procedures 
        // Chapter: Matching / Shape-Based
        // Short Description: Display the results of Shape-Based Matching. 
        public void dev_display_shape_matching_results(HTuple hv_ModelID, HTuple hv_Color,
            HTuple hv_Row, HTuple hv_Column, HTuple hv_Angle, HTuple hv_ScaleR, HTuple hv_ScaleC,
            HTuple hv_Model)
        {



            // Local iconic variables 

            HObject ho_ModelContours = null, ho_ContoursAffinTrans = null;

            // Local control variables 

            HTuple hv_NumMatches = null, hv_Index = new HTuple();
            HTuple hv_Match = new HTuple(), hv_HomMat2DIdentity = new HTuple();
            HTuple hv_HomMat2DScale = new HTuple(), hv_HomMat2DRotate = new HTuple();
            HTuple hv_HomMat2DTranslate = new HTuple();
            HTuple hv_Model_COPY_INP_TMP = hv_Model.Clone();
            HTuple hv_ScaleC_COPY_INP_TMP = hv_ScaleC.Clone();
            HTuple hv_ScaleR_COPY_INP_TMP = hv_ScaleR.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            try
            {
                //This procedure displays the results of Shape-Based Matching.
                //
                hv_NumMatches = new HTuple(hv_Row.TupleLength());
                if ((int)(new HTuple(hv_NumMatches.TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_ScaleR_COPY_INP_TMP.TupleLength())).TupleEqual(
                        1))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, hv_ScaleR_COPY_INP_TMP, out hv_ScaleR_COPY_INP_TMP);
                    }
                    if ((int)(new HTuple((new HTuple(hv_ScaleC_COPY_INP_TMP.TupleLength())).TupleEqual(
                        1))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, hv_ScaleC_COPY_INP_TMP, out hv_ScaleC_COPY_INP_TMP);
                    }
                    if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength())).TupleEqual(
                        0))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, 0, out hv_Model_COPY_INP_TMP);
                    }
                    else if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength()
                        )).TupleEqual(1))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, hv_Model_COPY_INP_TMP, out hv_Model_COPY_INP_TMP);
                    }
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ModelID.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        ho_ModelContours.Dispose();
                        HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID.TupleSelect(
                            hv_Index), 1);
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, hv_Color.TupleSelect(hv_Index % (new HTuple(hv_Color.TupleLength()
                            ))));
                        HTuple end_val18 = hv_NumMatches - 1;
                        HTuple step_val18 = 1;
                        for (hv_Match = 0; hv_Match.Continue(end_val18, step_val18); hv_Match = hv_Match.TupleAdd(step_val18))
                        {
                            if ((int)(new HTuple(hv_Index.TupleEqual(hv_Model_COPY_INP_TMP.TupleSelect(
                                hv_Match)))) != 0)
                            {
                                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                                HOperatorSet.HomMat2dScale(hv_HomMat2DIdentity, hv_ScaleR_COPY_INP_TMP.TupleSelect(
                                    hv_Match), hv_ScaleC_COPY_INP_TMP.TupleSelect(hv_Match), 0, 0,
                                    out hv_HomMat2DScale);
                                HOperatorSet.HomMat2dRotate(hv_HomMat2DScale, hv_Angle.TupleSelect(
                                    hv_Match), 0, 0, out hv_HomMat2DRotate);
                                HOperatorSet.HomMat2dTranslate(hv_HomMat2DRotate, hv_Row.TupleSelect(
                                    hv_Match), hv_Column.TupleSelect(hv_Match), out hv_HomMat2DTranslate);
                                ho_ContoursAffinTrans.Dispose();
                                HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ContoursAffinTrans,
                                    hv_HomMat2DTranslate);
                                HOperatorSet.DispObj(ho_ContoursAffinTrans, hv_ExpDefaultWinHandle);
                            }
                        }
                    }
                }
                ho_ModelContours.Dispose();
                ho_ContoursAffinTrans.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ModelContours.Dispose();
                ho_ContoursAffinTrans.Dispose();

                throw HDevExpDefaultException;
            }
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

        // Chapter: Graphics / Text
        // Short Description: This procedure writes a text message. 
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

        // Chapter: File / Misc
        // Short Description: Get all image files under the given path 
        public void list_image_files(HTuple hv_ImageDirectory, HTuple hv_Extensions, HTuple hv_Options,
            out HTuple hv_ImageFiles)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_ImageDirectoryIndex = null, hv_ImageFilesTmp = new HTuple();
            HTuple hv_CurrentImageDirectory = new HTuple(), hv_HalconImages = new HTuple();
            HTuple hv_OS = new HTuple(), hv_Directories = new HTuple();
            HTuple hv_Index = new HTuple(), hv_Length = new HTuple();
            HTuple hv_NetworkDrive = new HTuple(), hv_Substring = new HTuple();
            HTuple hv_FileExists = new HTuple(), hv_AllFiles = new HTuple();
            HTuple hv_i = new HTuple(), hv_Selection = new HTuple();
            HTuple hv_Extensions_COPY_INP_TMP = hv_Extensions.Clone();

            // Initialize local and output iconic variables 
            //This procedure returns all files in a given directory
            //with one of the suffixes specified in Extensions.
            //
            //Input parameters:
            //ImageDirectory: Directory or a tuple of directories with images.
            //   If a local directory is not found, the directory is searched
            //   under %HALCONIMAGES%/ImageDirectory. If %HALCONIMAGES% is not set,
            //   %HALCONROOT%/images is used instead.
            //Extensions: A string tuple containing the extensions to be found
            //   e.g. ['png','tif',jpg'] or others
            //If Extensions is set to 'default' or the empty string '',
            //   all image suffixes supported by HALCON are used.
            //Options: as in the operator list_files, except that the 'files'
            //   option is always used. Note that the 'directories' option
            //   has no effect but increases runtime, because only files are
            //   returned.
            //
            //Output parameter:
            //ImageFiles: A tuple of all found image file names
            //
            if ((int)((new HTuple((new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(""))))).TupleOr(new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(
                "default")))) != 0)
            {
                hv_Extensions_COPY_INP_TMP = new HTuple();
                hv_Extensions_COPY_INP_TMP[0] = "ima";
                hv_Extensions_COPY_INP_TMP[1] = "tif";
                hv_Extensions_COPY_INP_TMP[2] = "tiff";
                hv_Extensions_COPY_INP_TMP[3] = "gif";
                hv_Extensions_COPY_INP_TMP[4] = "bmp";
                hv_Extensions_COPY_INP_TMP[5] = "jpg";
                hv_Extensions_COPY_INP_TMP[6] = "jpeg";
                hv_Extensions_COPY_INP_TMP[7] = "jp2";
                hv_Extensions_COPY_INP_TMP[8] = "jxr";
                hv_Extensions_COPY_INP_TMP[9] = "png";
                hv_Extensions_COPY_INP_TMP[10] = "pcx";
                hv_Extensions_COPY_INP_TMP[11] = "ras";
                hv_Extensions_COPY_INP_TMP[12] = "xwd";
                hv_Extensions_COPY_INP_TMP[13] = "pbm";
                hv_Extensions_COPY_INP_TMP[14] = "pnm";
                hv_Extensions_COPY_INP_TMP[15] = "pgm";
                hv_Extensions_COPY_INP_TMP[16] = "ppm";
                //
            }
            hv_ImageFiles = new HTuple();
            //Loop through all given image directories.
            for (hv_ImageDirectoryIndex = 0; (int)hv_ImageDirectoryIndex <= (int)((new HTuple(hv_ImageDirectory.TupleLength()
                )) - 1); hv_ImageDirectoryIndex = (int)hv_ImageDirectoryIndex + 1)
            {
                hv_ImageFilesTmp = new HTuple();
                hv_CurrentImageDirectory = hv_ImageDirectory.TupleSelect(hv_ImageDirectoryIndex);
                if ((int)(new HTuple(hv_CurrentImageDirectory.TupleEqual(""))) != 0)
                {
                    hv_CurrentImageDirectory = ".";
                }
                HOperatorSet.GetSystem("image_dir", out hv_HalconImages);
                HOperatorSet.GetSystem("operating_system", out hv_OS);
                if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
                {
                    hv_HalconImages = hv_HalconImages.TupleSplit(";");
                }
                else
                {
                    hv_HalconImages = hv_HalconImages.TupleSplit(":");
                }
                hv_Directories = hv_CurrentImageDirectory.Clone();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_HalconImages.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    hv_Directories = hv_Directories.TupleConcat(((hv_HalconImages.TupleSelect(
                        hv_Index)) + "/") + hv_CurrentImageDirectory);
                }
                HOperatorSet.TupleStrlen(hv_Directories, out hv_Length);
                HOperatorSet.TupleGenConst(new HTuple(hv_Length.TupleLength()), 0, out hv_NetworkDrive);
                if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
                {
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        if ((int)(new HTuple(((((hv_Directories.TupleSelect(hv_Index))).TupleStrlen()
                            )).TupleGreater(1))) != 0)
                        {
                            HOperatorSet.TupleStrFirstN(hv_Directories.TupleSelect(hv_Index), 1,
                                out hv_Substring);
                            if ((int)((new HTuple(hv_Substring.TupleEqual("//"))).TupleOr(new HTuple(hv_Substring.TupleEqual(
                                "\\\\")))) != 0)
                            {
                                if (hv_NetworkDrive == null)
                                    hv_NetworkDrive = new HTuple();
                                hv_NetworkDrive[hv_Index] = 1;
                            }
                        }
                    }
                }
                hv_ImageFilesTmp = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Directories.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    HOperatorSet.FileExists(hv_Directories.TupleSelect(hv_Index), out hv_FileExists);
                    if ((int)(hv_FileExists) != 0)
                    {
                        HOperatorSet.ListFiles(hv_Directories.TupleSelect(hv_Index), (new HTuple("files")).TupleConcat(
                            hv_Options), out hv_AllFiles);
                        hv_ImageFilesTmp = new HTuple();
                        for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Extensions_COPY_INP_TMP.TupleLength()
                            )) - 1); hv_i = (int)hv_i + 1)
                        {
                            HOperatorSet.TupleRegexpSelect(hv_AllFiles, (((".*" + (hv_Extensions_COPY_INP_TMP.TupleSelect(
                                hv_i))) + "$")).TupleConcat("ignore_case"), out hv_Selection);
                            hv_ImageFilesTmp = hv_ImageFilesTmp.TupleConcat(hv_Selection);
                        }
                        HOperatorSet.TupleRegexpReplace(hv_ImageFilesTmp, (new HTuple("\\\\")).TupleConcat(
                            "replace_all"), "/", out hv_ImageFilesTmp);
                        if ((int)(hv_NetworkDrive.TupleSelect(hv_Index)) != 0)
                        {
                            HOperatorSet.TupleRegexpReplace(hv_ImageFilesTmp, (new HTuple("//")).TupleConcat(
                                "replace_all"), "/", out hv_ImageFilesTmp);
                            hv_ImageFilesTmp = "/" + hv_ImageFilesTmp;
                        }
                        else
                        {
                            HOperatorSet.TupleRegexpReplace(hv_ImageFilesTmp, (new HTuple("//")).TupleConcat(
                                "replace_all"), "/", out hv_ImageFilesTmp);
                        }
                        break;
                    }
                }
                //Concatenate the output image paths.
                hv_ImageFiles = hv_ImageFiles.TupleConcat(hv_ImageFilesTmp);
            }

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

        // Stack for temporary objects 
        HObject[] OTemp = new HObject[20];

        // Local iconic variables 

        // Local iconic variables 

        HObject ho_Image, ho_Circle = null, ho_ReducedImage = null;
        HObject ho_ImageMean = null, ho_ReducedImageR = null, ho_ReducedImageG = null;
        HObject ho_ReducedImageB = null, ho_Region2 = null, ho_ConnectedRegions2 = null;
        HObject ho_SelectedRegions2 = null, ho_UsedEdges = null, ho_ResultContours = null;
        HObject ho_CrossCenter = null, ho_RegionID = null, ho_RegionTD = null;
        HObject ho_IgnoreID = null, ho_IgnoreTD = null,ho_IgnoreDifference = null;
        HObject ho_RegionDilationID = null, ho_RegionErosionTD = null;
        HObject ho_RegionDifference = null, ho_ImageReduced = null;
        HObject ho_ImageReducedR = null, ho_ImageReducedG = null, ho_ImageReducedB = null;
        HObject ho_ImageScaled = null, ho_Region1 = null, ho_ConnectedRegions1 = null;
        HObject ho_SelectedRegionsR = null, ho_SelectedRegionsS = null;
        HObject ho_RegionUnionS = null, ho_RegionDifference1 = null;
        HObject ho_SelectedRegionsL = null, ho_RegionUnionL = null;
        HObject ho_RegionDifference2 = null, ho_RegionUnionR = null;
        HObject ho_ImageReducedR2 = null, ho_ImageReducedG2 = null;
        HObject ho_ImageReducedB2 = null, ho_RegionDifference3 = null;
        HObject ho_MissRegion = null, ho_MissConnectedRegions = null;
        HObject ho_MissSelectedRegions = null;
        HObject ho_Circle_up = null, ho_Circle_down = null, ho_Circle_left = null, ho_Circle_right = null;
        HObject ho_ImageReduced_Up = null, ho_ImageReduced_Down = null, ho_ImageReduced_Left = null, ho_ImageReduced_Right = null;

        // Local control variables 

        HTuple hv_ImageFiles = null, hv_WindowHandle = new HTuple();
        HTuple hv_Width = null, hv_Height = null, hv_i = null;
        HTuple hv_CreateModule = new HTuple(), hv_SaveModule = new HTuple();
        HTuple hv_FirstRadius = new HTuple(), hv_CenterRadius = new HTuple();
        HTuple hv_Radious0 = new HTuple(), hv_GraySet = new HTuple();
        HTuple hv_Area2 = new HTuple(), hv_Row2 = new HTuple();
        HTuple hv_Column2 = new HTuple(), hv_InitialRow = new HTuple();
        HTuple hv_InitialColumn = new HTuple(), hv_InitialRadius = new HTuple();
        HTuple hv_LengthID = new HTuple(), hv_MeasureThresholdID = new HTuple();
        HTuple hv_ResultRow = new HTuple(), hv_ResultColumn = new HTuple();
        HTuple hv_ResultRadius = new HTuple(), hv_Radius_ID = new HTuple();
        HTuple hv_Radius_TD = new HTuple(), hv_DilationSetID = new HTuple();
        HTuple hv_ErosionSetTD = new HTuple(), hv_Min = new HTuple();
        HTuple hv_Max = new HTuple(), hv_Range = new HTuple();
        HTuple hv_UnderSizeSet = new HTuple(), hv_ScratchLengthSet = new HTuple();
        HTuple hv_NumberS = new HTuple(), hv_LargeAreaSet = new HTuple();
        HTuple hv_NumberL = new HTuple(), hv_NumberR = new HTuple();
        HTuple hv_NumberSetR = new HTuple(), hv_MeanR = new HTuple();
        HTuple hv_DeviationR = new HTuple(), hv_MeanG = new HTuple();
        HTuple hv_DeviationG = new HTuple(), hv_MeanB = new HTuple();
        HTuple hv_DeviationB = new HTuple(), hv_MeanR2 = new HTuple();
        HTuple hv_DeviationR2 = new HTuple(), hv_MeanG2 = new HTuple();
        HTuple hv_DeviationG2 = new HTuple(), hv_MeanB2 = new HTuple();
        HTuple hv_DeviationB2 = new HTuple(), hv_DetectionR = new HTuple();
        HTuple hv_DetectionG = new HTuple(), hv_DetectionB = new HTuple();
        HTuple hv_DetectionRG = new HTuple(), hv_DetectionGB = new HTuple();
        HTuple hv_Detection2 = new HTuple(), hv_Detection3 = new HTuple();
        HTuple hv_SVMHandle = new HTuple(), hv_RGB_tuple = new HTuple();
        HTuple hv_new_RGB = new HTuple(), hv_Class = new HTuple();
        HTuple hv_MissAreaSet = new HTuple(), hv_MissGray = new HTuple(),hv_MissNumber = new HTuple();
        HTuple hv_SVMHandleNG = new HTuple(), hv_ClassNG = new HTuple();
        HTuple hv_AreaParticle = new HTuple(),hv_RowParticle = new HTuple(),hv_ColumnParticle = new HTuple(),hv_CountParticle = new HTuple();
        HTuple hv_j = new HTuple();
        HTuple hv_Mean_Up_R = new HTuple(), hv_Mean_Up_G = new HTuple(), hv_Mean_Up_B = new HTuple();
        HTuple hv_Mean_Down_R = new HTuple(), hv_Mean_Down_G = new HTuple(), hv_Mean_Down_B = new HTuple();
        HTuple hv_Mean_Left_R = new HTuple(), hv_Mean_Left_G = new HTuple(), hv_Mean_Left_B = new HTuple();
        HTuple hv_Mean_Right_R = new HTuple(), hv_Mean_Right_G = new HTuple(), hv_Mean_Right_B = new HTuple();
        HTuple hv_Deviation = new HTuple();

        #endregion


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
            ho_Image = My.ho_Image;
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.GenEmptyObj(out ho_Circle);
            //找出初始半徑
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
            HOperatorSet.DrawCircle(hv_ExpDefaultWinHandle, out dFirstCircleRow, out dFirstCircleColumn,
                out dFirstCircleRadius);
        }

        private void tbDetectionRadius_ValueChanged(object sender, EventArgs e)
        {
            nudDetectionRadius_1.Value = tbDetectionRadius_1.Value;
        }

        private void nudDetectionRadius_ValueChanged(object sender, EventArgs e)
        {
            dReduceRadius = tbDetectionRadius_1.Value = Convert.ToInt32(nudDetectionRadius_1.Value);
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

       

       
       
        private void FrmVDICoatingVS_Load(object sender, EventArgs e)
        {
            ReadPara();
            LoadSettingLight();
            TimerUI.Enabled = true;
        }

        public void ReadPara()
        {
            txtPpix.Text = My.VDICoating.mmpixel.ToString();
            if (My.VDICoating.PointChoice == "first")
                cbPointChoice.SelectedIndex = 0;
            else
                cbPointChoice.SelectedIndex = 1;

            cbDetectionColor.Checked = Convert.ToBoolean(My.VDICoating.DetectionColor);
            cbDetectionColor_2.Checked = Convert.ToBoolean(My.VDICoating.DetectionColor_2);
            cbTestDefect.Checked = Convert.ToBoolean(My.VDICoating.TestDefect);
            cbIgnoreOpen.Checked = Convert.ToBoolean(My.VDICoating.IgnoreOpen);
            dFirstCircleRadius = (int)My.VDICoating.dFirstCircleRadius;
            tbDetectionRadius_1.Value = (int)My.VDICoating.dReduceRadius;
            tbGraythreshold.Value = (int)My.VDICoating.dGraythreshold;
            tbLength.Value = (int)My.VDICoating.dLength;

            if (My.VDICoating.sGenParamValue == "positive")
            {
                tbBlackToWhite.Value = (int)My.VDICoating.dMeasureThreshold;
            }
            else
            {
                tbWhiteToBlack.Value = (int)My.VDICoating.dMeasureThreshold;
            }
            tbRadius_ID.Value = (int)My.VDICoating.dRadius_ID;
            tbRadius_TD.Value = (int)My.VDICoating.dRadius_TD;


            nudDefectGraySet.Value = (int)My.VDICoating.dDefectGraySet;
            nudFilterAreaLength.Value = (decimal)My.VDICoating.dFilterArea;
            nudScratchLength.Value = (decimal)My.VDICoating.dScratchLength;
            nudLargeArea.Value = (decimal)My.VDICoating.dLargeArea;
            nudNumber.Value = (int)My.VDICoating.dNumber;
            tbMissGray.Value = (int)My.VDICoating.dMissGray;
            tbMissArea.Value = (int)My.VDICoating.dMissArea;
            tbMissOuterRadius.Value = (int)My.VDICoating.dMissOuterRadius;

            tbDefect_ID.Value = (int)My.VDICoating.dDefect_ID;
            tbDefect_TD.Value = (int)My.VDICoating.dDefect_TD;
            tbIgnore_ID.Value = (int)My.VDICoating.dIgnore_ID;
            tbIgnore_TD.Value = (int)My.VDICoating.dIgnore_TD;

            cbColorRangeChoice.SelectedIndex = My.VDICoating.ColorRangeChoice;
            cbDarkLightChoice.SelectedIndex = My.VDICoating.DarkLightChoice;

            tbCenterDistance.Value = (int)My.VDICoating.dCenterDistance;
            tbRangeRadius.Value = (int)My.VDICoating.dRangeRadius;

            nudUpperLimit_A.Value = (decimal)My.VDICoating.dUpperLimit_A;
            nudLowerLimit_A.Value = (decimal)My.VDICoating.dLowerLimit_A;
            nudUpperLimit_B.Value = (decimal)My.VDICoating.dUpperLimit_B;
            nudLowerLimit_B.Value = (decimal)My.VDICoating.dLowerLimit_B;
        }

        public void LoadSettingLight()
        {
            nudLightSet_1.Value = Convert.ToInt32(Light.LightSet_1);
            nudLightSet_2.Value = Convert.ToInt32(Light.LightSet_2);
            nudLightSet_3.Value = Convert.ToInt32(Light.LightSet_3);
            nudLightSet_4.Value = Convert.ToInt32(Light.LightSet_4);
        }
     
        private void btnImageProPlus_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            ImageProPlus(hWindowControl1.HalconWindow,My.ho_Image,Tray.n); 
        }

       
        public void ImageProPlus(HWindow Window,HObject theImage,int n)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_ReducedImage);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageR);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageG);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageB);
            HOperatorSet.GenEmptyObj(out ho_RegionID);
            HOperatorSet.GenEmptyObj(out ho_RegionDilationID);
            HOperatorSet.GenEmptyObj(out ho_RegionTD);
            HOperatorSet.GenEmptyObj(out ho_RegionErosionTD);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedR);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedG);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedB);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedR2);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedG2);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedB2);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference3);
            HOperatorSet.GenEmptyObj(out ho_MissRegion);
            HOperatorSet.GenEmptyObj(out ho_MissConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_MissSelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_Circle_up);
            HOperatorSet.GenEmptyObj(out ho_Circle_down);
            HOperatorSet.GenEmptyObj(out ho_Circle_left);
            HOperatorSet.GenEmptyObj(out ho_Circle_right);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced_Up);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced_Down);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced_Left);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced_Right);
            double[] RGB = { 0, 0, 0 };
            double[] RGB_Up = { 0, 0, 0 };
            double[] RGB_Down = { 0, 0, 0 };
            double[] RGB_Left = { 0, 0, 0 };
            double[] RGB_Right = { 0, 0, 0 };

            double[] Lab = {0,0,0};
            double[] Lab_Up = { 0, 0, 0 };
            double[] Lab_Down = { 0, 0, 0 };
            double[] Lab_Left = { 0, 0, 0 };
            double[] Lab_Right = { 0, 0, 0 };
            try
            {
                Window.ClearWindow();
                ho_Image = theImage;
                hv_ExpDefaultWinHandle = Window;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, dReduceRadius);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //mean_image (ReducedImage, ImageMean, 10, 10)
                ho_ReducedImageR.Dispose(); ho_ReducedImageG.Dispose(); ho_ReducedImageB.Dispose();
                HOperatorSet.Decompose3(ho_ReducedImage, out ho_ReducedImageR, out ho_ReducedImageG,
                    out ho_ReducedImageB);

                //畫檢視範圍
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //mean_image (ReducedImage, ImageMean, 10, 10)
                ho_ReducedImageR.Dispose(); ho_ReducedImageG.Dispose(); ho_ReducedImageB.Dispose();
                HOperatorSet.Decompose3(ho_ReducedImage, out ho_ReducedImageR, out ho_ReducedImageG,
                    out ho_ReducedImageB);
                //檢查是否無料
                hv_MissGray = dMissGray;
                ho_MissRegion.Dispose();
                HOperatorSet.Threshold(ho_ReducedImageR, out ho_MissRegion, 0, hv_MissGray);
                ho_MissConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_MissRegion, out ho_MissConnectedRegions);
                hv_MissAreaSet = dMissArea;
                ho_MissSelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_MissConnectedRegions, out ho_MissSelectedRegions,
                    "area", "and", hv_MissAreaSet, 9999999);
                HOperatorSet.SelectShape(ho_MissSelectedRegions, out ho_MissSelectedRegions,
                    "outer_radius", "and", 0, dMissOuterRadius);
                HOperatorSet.CountObj(ho_MissSelectedRegions, out hv_MissNumber);
                
                //如果不是無料才進行顏色判別
                if (hv_MissNumber <= 4 && hv_MissNumber>=2)
                {
                    set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1400, 100);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                    Vision.VisionResult[n] = "Miss";
                }
                else
                {
                    hv_GraySet = dGraythreshold;
                    ho_Region2.Dispose();
                    if (My.VDICoating.DarkLightChoice == 0)
                        HOperatorSet.Threshold(ho_ReducedImageR, out ho_Region2, hv_GraySet, 255);
                    else
                        HOperatorSet.Threshold(ho_ReducedImageR, out ho_Region2, 0, hv_GraySet);

                    ho_ConnectedRegions2.Dispose();
                    HOperatorSet.Connection(ho_Region2, out ho_ConnectedRegions2);
                    HOperatorSet.AreaCenter(ho_ConnectedRegions2, out hv_Area2, out hv_Row2, out hv_Column2);
                    ho_SelectedRegions2.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions2, "area", "and", hv_Area2.TupleMax(), 9999999);
                    HOperatorSet.AreaCenter(ho_SelectedRegions2, out hv_Area2, out hv_InitialRow, out hv_InitialColumn);
                    //draw_circle (WindowHandle, Row, Column, Radius)
                    hv_InitialRadius = dFirstCircleRadius;
                    hv_LengthID = dLength;
                    hv_MeasureThresholdID = dMeasureThreshold;
                    ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                    gen_circle_center(ho_ReducedImageR, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, dFirstCircleRadius,
                         hv_LengthID, hv_MeasureThresholdID, sGenParamValue, PointChoice, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);
                
                    ho_RegionID.Dispose();
                    HOperatorSet.GenCircle(out ho_RegionID, hv_ResultRow, hv_ResultColumn, dRadius_ID);

                    //HOperatorSet.GenRegionContourXld(ho_ResultContoursID, out ho_RegionID, "filled");
                    //內徑膨脹,過濾掉過曝區域
                    hv_DilationSetID = 5;
                    ho_RegionDilationID.Dispose();
                    HOperatorSet.DilationCircle(ho_RegionID, out ho_RegionDilationID, hv_DilationSetID);
                    //外徑轉Region
                    ho_RegionTD.Dispose();
                    HOperatorSet.GenCircle(out ho_RegionTD, hv_ResultRow, hv_ResultColumn, dRadius_TD);
                    //HOperatorSet.GenRegionContourXld(ho_ResultContoursTD, out ho_RegionTD, "filled");
                    //外徑腐蝕,過濾掉過曝區域
                    hv_ErosionSetTD = 10;
                    ho_RegionErosionTD.Dispose();
                    HOperatorSet.ErosionCircle(ho_RegionTD, out ho_RegionErosionTD, hv_ErosionSetTD);
                    ho_RegionDifference.Dispose();
                    HOperatorSet.Difference(ho_RegionErosionTD, ho_RegionDilationID, out ho_RegionDifference
                        );

                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_ReducedImage, ho_RegionDifference, out ho_ImageReduced
                        );
                    ho_ImageReducedR.Dispose(); ho_ImageReducedG.Dispose(); ho_ImageReducedB.Dispose();
                    HOperatorSet.Decompose3(ho_ImageReduced, out ho_ImageReducedR, out ho_ImageReducedG,
                        out ho_ImageReducedB);

                    HOperatorSet.MinMaxGray(ho_RegionDifference, ho_ImageReducedR, 0, out hv_Min, out hv_Max, out hv_Range);
                    //將圖片最黑的黑色部分拉到0,增強對比度,白色就不拉,怕有些沒有傷痕的被影響
                    ho_ImageScaled.Dispose();
                    scale_image_range(ho_ImageReducedR, out ho_ImageScaled, hv_Min, 255);
                    ho_ImageMean.Dispose();
                    HOperatorSet.MeanImage(ho_ImageScaled, out ho_ImageMean, 3, 3);
                    ho_Region1.Dispose();
                    HOperatorSet.Threshold(ho_ImageMean, out ho_Region1, dDefectGraySet, 255);
                    if (My.VDICoating.ColorRangeChoice == 0)//取整圈平均
                    {
                        //扣掉缺陷再求顏色
                        ho_RegionDifference3.Dispose();
                        HOperatorSet.Difference(ho_RegionDifference, ho_Region1, out ho_RegionDifference3
                            );
                        HOperatorSet.Intensity(ho_RegionDifference3, ho_ImageReducedR, out hv_MeanR,
                            out hv_DeviationR);
                        HOperatorSet.Intensity(ho_RegionDifference3, ho_ImageReducedG, out hv_MeanG,
                            out hv_DeviationG);
                        HOperatorSet.Intensity(ho_RegionDifference3, ho_ImageReducedB, out hv_MeanB,
                            out hv_DeviationB);
                        //過濾超出2倍偏差值得值,重新求平均值

                        ho_ImageReducedR2.Dispose();
                        HOperatorSet.Threshold(ho_ImageReducedR, out ho_ImageReducedR2, hv_MeanR - (2 * hv_DeviationR),
                            hv_MeanR + (2 * hv_DeviationR));
                        ho_ImageReducedG2.Dispose();
                        HOperatorSet.Threshold(ho_ImageReducedG, out ho_ImageReducedG2, hv_MeanG - (2 * hv_DeviationG),
                            hv_MeanG + (2 * hv_DeviationG));
                        ho_ImageReducedB2.Dispose();
                        HOperatorSet.Threshold(ho_ImageReducedB, out ho_ImageReducedB2, hv_MeanB - (2 * hv_DeviationB),
                            hv_MeanB + (2 * hv_DeviationB));
                        HOperatorSet.Intensity(ho_ImageReducedR2, ho_ImageReducedR, out hv_MeanR2,
                            out hv_DeviationR2);
                        HOperatorSet.Intensity(ho_ImageReducedG2, ho_ImageReducedG, out hv_MeanG2,
                            out hv_DeviationG2);
                        HOperatorSet.Intensity(ho_ImageReducedB2, ho_ImageReducedB, out hv_MeanB2,
                            out hv_DeviationB2);
                        HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                        RGB[0] = Math.Round((double)hv_MeanR2,0);
                        RGB[1] = Math.Round((double)hv_MeanG2, 0);
                        RGB[2] = Math.Round((double)hv_MeanB2, 0);
                    }
                    else if(My.VDICoating.ColorRangeChoice==1)//(取四點)
                    {
                        HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                        ho_Circle_up.Dispose();
                        HOperatorSet.GenCircle(out ho_Circle_up, hv_ResultRow - dCenterDistance / pixel2um, hv_ResultColumn, dRangeRadius * pixel2um);
                        ho_Circle_down.Dispose();
                        HOperatorSet.GenCircle(out ho_Circle_down, hv_ResultRow + dCenterDistance / pixel2um, hv_ResultColumn, dRangeRadius * pixel2um);
                        ho_Circle_left.Dispose();
                        HOperatorSet.GenCircle(out ho_Circle_left, hv_ResultRow, hv_ResultColumn - dCenterDistance / pixel2um, dRangeRadius * pixel2um);
                        ho_Circle_right.Dispose();
                        HOperatorSet.GenCircle(out ho_Circle_right, hv_ResultRow, hv_ResultColumn + dCenterDistance / pixel2um, dRangeRadius * pixel2um);
                        ho_ImageReduced_Up.Dispose();
                        HOperatorSet.ReduceDomain(ho_Image, ho_Circle_up, out ho_ImageReduced_Up);
                        ho_ImageReduced_Down.Dispose();
                        HOperatorSet.ReduceDomain(ho_Image, ho_Circle_down, out ho_ImageReduced_Down);
                        ho_ImageReduced_Left.Dispose();
                        HOperatorSet.ReduceDomain(ho_Image, ho_Circle_left, out ho_ImageReduced_Left);
                        ho_ImageReduced_Right.Dispose();
                        HOperatorSet.ReduceDomain(ho_Image, ho_Circle_right, out ho_ImageReduced_Right);
                        set_display_font(hv_ExpDefaultWinHandle, 10, "mono", "true", "false");
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                        HOperatorSet.Intensity(ho_ImageReduced_Up, ho_ImageReducedR, out hv_Mean_Up_R, out hv_Deviation);
                        HOperatorSet.Intensity(ho_ImageReduced_Up, ho_ImageReducedG, out hv_Mean_Up_G, out hv_Deviation);
                        HOperatorSet.Intensity(ho_ImageReduced_Up, ho_ImageReducedB, out hv_Mean_Up_B, out hv_Deviation);

                        HOperatorSet.Intensity(ho_ImageReduced_Down, ho_ImageReducedR, out hv_Mean_Down_R, out hv_Deviation);
                        HOperatorSet.Intensity(ho_ImageReduced_Down, ho_ImageReducedG, out hv_Mean_Down_G, out hv_Deviation);
                        HOperatorSet.Intensity(ho_ImageReduced_Down, ho_ImageReducedB, out hv_Mean_Down_B, out hv_Deviation);

                        HOperatorSet.Intensity(ho_ImageReduced_Left, ho_ImageReducedR, out hv_Mean_Left_R, out hv_Deviation);
                        HOperatorSet.Intensity(ho_ImageReduced_Left, ho_ImageReducedG, out hv_Mean_Left_G, out hv_Deviation);
                        HOperatorSet.Intensity(ho_ImageReduced_Left, ho_ImageReducedB, out hv_Mean_Left_B, out hv_Deviation);

                        HOperatorSet.Intensity(ho_ImageReduced_Right, ho_ImageReducedR, out hv_Mean_Right_R, out hv_Deviation);
                        HOperatorSet.Intensity(ho_ImageReduced_Right, ho_ImageReducedG, out hv_Mean_Right_G, out hv_Deviation);
                        HOperatorSet.Intensity(ho_ImageReduced_Right, ho_ImageReducedB, out hv_Mean_Right_B, out hv_Deviation);
                        RGB_Up[0] = Math.Round((double)hv_Mean_Up_R, 0);
                        RGB_Up[1] = Math.Round((double)hv_Mean_Up_G, 0);
                        RGB_Up[2] = Math.Round((double)hv_Mean_Up_B, 0);
                        RGB_Down[0] = Math.Round((double)hv_Mean_Down_R, 0);
                        RGB_Down[1] = Math.Round((double)hv_Mean_Down_G, 0);
                        RGB_Down[2] = Math.Round((double)hv_Mean_Down_B, 0);
                        RGB_Left[0] = Math.Round((double)hv_Mean_Left_R, 0);
                        RGB_Left[1] = Math.Round((double)hv_Mean_Left_G, 0);
                        RGB_Left[2] = Math.Round((double)hv_Mean_Left_B, 0);
                        RGB_Right[0] = Math.Round((double)hv_Mean_Right_R, 0);
                        RGB_Right[1] = Math.Round((double)hv_Mean_Right_G, 0);
                        RGB_Right[2] = Math.Round((double)hv_Mean_Right_B, 0);
                    }
                    if (My.VDICoating.DetectionColor)
                    {
                        HOperatorSet.ReadClassSvm(Sys.ModulePath + "\\" + Sys.Function + "_" + Production.CurProduction + "_ColorOK", out hv_SVMHandle);
                        hv_new_RGB = new HTuple();
                        hv_new_RGB = hv_new_RGB.TupleConcat(hv_MeanR2);
                        hv_new_RGB = hv_new_RGB.TupleConcat(hv_MeanG2);
                        hv_new_RGB = hv_new_RGB.TupleConcat(hv_MeanB2);
                        HOperatorSet.ClassifyClassSvm(hv_SVMHandle, hv_new_RGB, 1, out hv_Class);
                        HOperatorSet.ClearClassSvm(hv_SVMHandle);
                        
                        //if ((int)(new HTuple(hv_Class.TupleEqual(1))) != 0)
                        //{
                            
                        //}
                        //else
                        //{
                        //    //HOperatorSet.ReadClassSvm(Sys.ModulePath + "\\" + Sys.Function + "_" + Production.CurProduction + "_ColorNG", out hv_SVMHandleNG);
                        //    //hv_new_RGB = new HTuple();
                        //    //hv_new_RGB = hv_new_RGB.TupleConcat(hv_MeanR2);
                        //    //hv_new_RGB = hv_new_RGB.TupleConcat(hv_MeanG2);
                        //    //hv_new_RGB = hv_new_RGB.TupleConcat(hv_MeanB2);
                        //    //HOperatorSet.ClassifyClassSvm(hv_SVMHandleNG, hv_new_RGB, 1, out hv_ClassNG);
                        //    //HOperatorSet.ClearClassSvm(hv_SVMHandleNG);
                        //}
                    }
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    if (My.VDICoating.TestDefect)
                    {
                        DefectDetecting(Window,theImage);
                    }
                    set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    //RGB轉成LAB
                    if (My.VDICoating.ColorRangeChoice == 0)
                    {
                        Lab = rgb2lab(hv_MeanR2, hv_MeanG2, hv_MeanB2);
                        HOperatorSet.DispObj(ho_RegionID, hv_ExpDefaultWinHandle);
                        HOperatorSet.DispObj(ho_RegionTD, hv_ExpDefaultWinHandle);
                    }
                    else if (My.VDICoating.ColorRangeChoice == 1)
                    {
                        Lab_Up = rgb2lab(hv_Mean_Up_R, hv_Mean_Up_G, hv_Mean_Up_B);
                        Lab_Down = rgb2lab(hv_Mean_Down_R, hv_Mean_Down_G, hv_Mean_Down_B);
                        Lab_Left = rgb2lab(hv_Mean_Left_R, hv_Mean_Left_G, hv_Mean_Left_B);
                        Lab_Right = rgb2lab(hv_Mean_Right_R, hv_Mean_Right_G, hv_Mean_Right_B);
                        HOperatorSet.DispObj(ho_Circle_up, hv_ExpDefaultWinHandle);
                        HOperatorSet.DispObj(ho_Circle_down, hv_ExpDefaultWinHandle);
                        HOperatorSet.DispObj(ho_Circle_left, hv_ExpDefaultWinHandle);
                        HOperatorSet.DispObj(ho_Circle_right, hv_ExpDefaultWinHandle);
                    }
                    if (My.VDICoating.DetectionColor_2)
                    {
                        if (Lab[1] > dUpperLimit_A || Lab[1] < dLowerLimit_A || Lab[2] > dUpperLimit_B || Lab[2] < dLowerLimit_B)
                        {
                            hv_Class = 0;
                        }
                        else
                        {
                            hv_Class = 1;
                        }
                    }
                    set_display_font(hv_ExpDefaultWinHandle, 20, "mono", "true", "false");
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    if (My.VDICoating.ColorRangeChoice == 0)
                    {
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "R:" + RGB[0]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "G:" + RGB[1]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "B:" + RGB[2]);
                        //HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 0);
                        //HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "L:" + Lab[0]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "A:" + Lab[1]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 500, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "B:" + Lab[2]);
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    }
                    if (My.VDICoating.ColorRangeChoice == 1)
                    {
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "R:" + RGB_Up[0]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "G:" + RGB_Up[1]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "B:" + RGB_Up[2]);
                        //HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 0);
                        //HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "L:" + Lab[0]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "A:" + Lab_Up[1]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 500, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "B:" + Lab_Up[2]);

                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 500);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "R:" + RGB_Down[0]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 500);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "G:" + RGB_Down[1]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 500);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "B:" + RGB_Down[2]);
                        //HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 0);
                        //HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "L:" + Lab[0]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 500);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "A:" + Lab_Down[1]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 500, 500);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "B:" + Lab_Down[2]);

                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 1000);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "R:" + RGB_Left[0]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 1000);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "G:" + RGB_Left[1]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 1000);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "B:" + RGB_Left[2]);
                        //HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 0);
                        //HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "L:" + Lab[0]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 1000);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "A:" + Lab_Left[1]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 500, 1000);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "B:" + Lab_Left[2]);

                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 1500);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "R:" + RGB_Right[0]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 1500);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "G:" + RGB_Right[1]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 1500);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "B:" + RGB_Right[2]);
                        //HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 0);
                        //HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "L:" + Lab[0]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 1500);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "A:" + Lab_Right[1]);
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 500, 1500);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "B:" + Lab_Right[2]);
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    }
                    Vision.VisionResult[n] = "OK";

                    set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                    if (My.VDICoating.DetectionColor || My.VDICoating.DetectionColor_2)
                        {
                            if (hv_Class == 1)
                            {
                                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2000, 100);
                                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "顏色OK");
                                Vision.VisionResult[n] = "OK";
                            }
                            else
                            {
                                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2000, 100);
                                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "NG:顏色NG");
                                Vision.VisionResult[n] = "NG";
                                //if (hv_ClassNG == 1)
                                //{
                                //    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1400, 100);
                                //    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "NG:顏色NG");
                                //    Vision.VisionResult[Tray.n] = "NG";
                                //}
                                //else
                                //{
                                //    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1400, 100);
                                //    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Other");
                                //    Vision.VisionResult[Tray.n] = "Other";
                                //}
                            }
                        }

                    if (My.VDICoating.TestDefect)
                    {
                        if ((int)(new HTuple(hv_NumberS.TupleGreater(0))) != 0)
                        {
                            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                            HOperatorSet.DispObj(ho_RegionUnionS, hv_ExpDefaultWinHandle);
                        }
                        if ((int)(new HTuple(hv_NumberL.TupleGreater(0))) != 0)
                        {
                            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                            HOperatorSet.DispObj(ho_RegionUnionL, hv_ExpDefaultWinHandle);
                        }
                        if ((int)(new HTuple(hv_NumberR.TupleGreater(0))) != 0)
                        {
                            if ((int)(new HTuple(hv_NumberR.TupleGreater(hv_NumberSetR))) != 0)
                            {
                                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
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
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                        if (hv_NumberS > 0)
                        {
                            HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2200, 100);
                            HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "刮傷NG");
                            Vision.VisionResult[n] = "NG2";
                        }
                        else if (hv_NumberL > 0)
                        {
                            HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 100);
                            HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "脫膜NG");
                            Vision.VisionResult[n] = "NG2";
                        }
                        else if (hv_NumberR > dNumber)
                        {
                            HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2400, 100);
                            HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "脫膜NG");
                            Vision.VisionResult[n] = "NG2";
                        }
                        else
                        {
                            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                            HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2200, 100);
                            HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "OK");
                            Vision.VisionResult[n] = "OK";
                        }
                    }
                }
                if (hv_MeanR2.Length == 0)
                    hv_MeanR2 = 0;
                if (hv_MeanG2.Length == 0)
                    hv_MeanG2 = 0;
                if (hv_MeanB2.Length == 0)
                    hv_MeanB2 = 0;

                WriteLog(n, Vision.VisionResult[n], RGB, RGB_Up, RGB_Down, RGB_Left, RGB_Right, Lab,Lab_Up,Lab_Down,Lab_Left,Lab_Right);
                
            }
            catch
            {
                Vision.VisionResult[n] = "Miss";
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
            ho_Circle.Dispose();
            ho_ReducedImage.Dispose();
            ho_ImageMean.Dispose();
            ho_ReducedImageR.Dispose();
            ho_ReducedImageG.Dispose();
            ho_ReducedImageB.Dispose();
            ho_MissRegion.Dispose();
            ho_MissConnectedRegions.Dispose();
            ho_MissSelectedRegions.Dispose();
            ho_Region2.Dispose();
            ho_ConnectedRegions2.Dispose();
            ho_SelectedRegions2.Dispose();
            ho_UsedEdges.Dispose();
            ho_ResultContours.Dispose();
            ho_CrossCenter.Dispose();
            ho_RegionID.Dispose();
            ho_RegionTD.Dispose();
            ho_RegionDilationID.Dispose();
            ho_RegionErosionTD.Dispose();
            ho_RegionDifference.Dispose();
            ho_ImageReduced.Dispose();
            ho_ImageReducedR.Dispose();
            ho_ImageReducedG.Dispose();
            ho_ImageReducedB.Dispose();
            ho_ImageScaled.Dispose();
            ho_Region1.Dispose();
            ho_ConnectedRegions1.Dispose();
            ho_SelectedRegionsR.Dispose();
            ho_SelectedRegionsS.Dispose();
            ho_RegionUnionS.Dispose();
            ho_RegionDifference1.Dispose();
            ho_SelectedRegionsL.Dispose();
            ho_RegionUnionL.Dispose();
            ho_RegionDifference2.Dispose();
            ho_RegionUnionR.Dispose();
            ho_RegionDifference3.Dispose();
            ho_ImageReducedR2.Dispose();
            ho_ImageReducedG2.Dispose();
            ho_ImageReducedB2.Dispose();
        }

        static double Gamma(double x)
        {
            return x > 0.04045 ? Math.Pow((x + 0.055) / 1.055f, 2.4) : x / 12.92;
        }
        //RGB轉LAB(要先除以255)
        public static double[] rgb2lab(double var_R, double var_G, double var_B)
        {
            double[] arr = new double[3];
            double B = Gamma(var_B/255);
            double G = Gamma(var_G/255);
            double R = Gamma(var_R/255);
            double X = 0.412453 * R + 0.357580 * G + 0.180423 * B;
            double Y = 0.212671 * R + 0.715160 * G + 0.072169 * B;
            double Z = 0.019334 * R + 0.119193 * G + 0.950227 * B;

            X /= 0.95047f;
            Y /= 1.0f;
            Z /= 1.08883f;

            double FX = X > 0.008856f ? Math.Pow(X, 1.0f / 3.0f) : (7.787f * X + 0.137931f);
            double FY = Y > 0.008856f ? Math.Pow(Y, 1.0f / 3.0f) : (7.787f * Y + 0.137931f);
            double FZ = Z > 0.008856f ? Math.Pow(Z, 1.0f / 3.0f) : (7.787f * Z + 0.137931f);
            arr[0] = Math.Round(Y > 0.008856f ? (116.0f * FY - 16.0f) : (903.3f * Y),4);
            arr[1] = Math.Round(500f * (FX - FY),4);
            arr[2] = Math.Round(200f * (FY - FZ),4);
            return arr;

        }



        public void WriteLog(int n, string ResultOK, double[] RGB, double[] RGB_Up, double[] RGB_Down, double[] RGB_Left, double[] RGB_Right, double[] LAB, double[] LAB_Up, double[] LAB_Down, double[] LAB_Left, double[] LAB_Right)
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
                        File.WriteAllText(Log, "Function\tCode\tTray Barcode_B\tTray No.\tToTal Count\tTray Barcode_A\tProgram ID\tProduct\tClass\tOperatorID\tMachine No.\tTime\tCT\tResult\tR\tG\tB\tL\tA\tB\tUp_R\tUp_G\tUp_B\tUp_L\tUp_A\tUp_B\tDown_R\tDown_G\tDown_B\tDown_L\tDown_A\tDown_B\tLeft_R\tLeft_G\tLeft_B\tLeft_L\tLeft_A\tLeft_B\tRight_R\tRight_G\tRight_B\tRight_L\tRight_A\tRight_B" +
                                         "\r\n");
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
                                     RGB[0].ToString("f0") + "\t" +
                                     RGB[1].ToString("f0") + "\t" +
                                     RGB[2].ToString("f0") + "\t" +
                                     LAB[0].ToString()+ "\t" +
                                     LAB[1].ToString() + "\t" +
                                     LAB[2].ToString() +"\t" +

                                     RGB_Up[0].ToString("f0") + "\t" +
                                     RGB_Up[1].ToString("f0") + "\t" +
                                     RGB_Up[2].ToString("f0") + "\t" +
                                     LAB_Up[0].ToString()+ "\t" +
                                     LAB_Up[1].ToString() + "\t" +
                                     LAB_Up[2].ToString() +"\t" +

                                     RGB_Down[0].ToString("f0") + "\t" +
                                     RGB_Down[1].ToString("f0") + "\t" +
                                     RGB_Down[2].ToString("f0") + "\t" +
                                     LAB_Down[0].ToString()+ "\t" +
                                     LAB_Down[1].ToString() + "\t" +
                                     LAB_Down[2].ToString() +"\t" +

                                     RGB_Left[0].ToString("f0") + "\t" +
                                     RGB_Left[1].ToString("f0") + "\t" +
                                     RGB_Left[2].ToString("f0") + "\t" +
                                     LAB_Left[0].ToString()+ "\t" +
                                     LAB_Left[1].ToString() + "\t" +
                                     LAB_Left[2].ToString() +"\t" +

                                     RGB_Right[0].ToString("f0") + "\t" +
                                     RGB_Right[1].ToString("f0") + "\t" +
                                     RGB_Right[2].ToString("f0") + "\t" +
                                     LAB_Right[0].ToString()+ "\t" +
                                     LAB_Right[1].ToString() + "\t" +
                                     LAB_Right[2].ToString()
                                     );
                    }
                }
                catch
                {
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
            if (sender.Text == "打開")
            {
                flag = true;
                sender.Text = "關閉";
            }
            else
            {
                flag = false;
                sender.Text = "打開";
            }
            byte[] cmd = Lighter.SetOnOff(ch, flag);
            parent.com1.Write(cmd, 0, cmd.Length);
            //ShowCmd(cmd);
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

        private void tbGraythreshold_ValueChanged(object sender, EventArgs e)
        {
            nudGraythreshold.Value = tbGraythreshold.Value;
        }

        private void nudGraythreshold_ValueChanged(object sender, EventArgs e)
        {
            dGraythreshold = tbGraythreshold.Value = Convert.ToInt32(nudGraythreshold.Value);
            try
            {
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
                HOperatorSet.GenEmptyObj(out ho_ReducedImageR);
                HOperatorSet.GenEmptyObj(out ho_ReducedImageG);
                HOperatorSet.GenEmptyObj(out ho_ReducedImageB);
                HOperatorSet.GenEmptyObj(out ho_Region2);
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
                ho_ReducedImageR.Dispose(); ho_ReducedImageG.Dispose(); ho_ReducedImageB.Dispose();
                HOperatorSet.Decompose3(ho_ReducedImage, out ho_ReducedImageR, out ho_ReducedImageG,
                    out ho_ReducedImageB);
                hv_GraySet = dGraythreshold;
                ho_Region2.Dispose();
                if (My.VDICoating.DarkLightChoice == 0)
                    HOperatorSet.Threshold(ho_Image, out ho_Region2, tbGraythreshold.Value, 255);
                else
                    HOperatorSet.Threshold(ho_Image, out ho_Region2, 0, tbGraythreshold.Value);
                HOperatorSet.DispObj(ho_Region2, hv_ExpDefaultWinHandle);
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

            HOperatorSet.GenEmptyObj(out ho_ReducedImageR);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageG);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageB);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);

            try
            {
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                //畫檢視範圍
                hWindowControl1.HalconWindow.ClearWindow();
                
                //畫檢視範圍
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //mean_image (ReducedImage, ImageMean, 10, 10)
                ho_ReducedImageR.Dispose(); ho_ReducedImageG.Dispose(); ho_ReducedImageB.Dispose();
                HOperatorSet.Decompose3(ho_ReducedImage, out ho_ReducedImageR, out ho_ReducedImageG,
                    out ho_ReducedImageB);
                HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                hv_GraySet = dGraythreshold;
                ho_Region2.Dispose();
                HOperatorSet.Threshold(ho_ReducedImageR, out ho_Region2, 0, hv_GraySet);
                ho_ConnectedRegions2.Dispose();
                HOperatorSet.Connection(ho_Region2, out ho_ConnectedRegions2);
                HOperatorSet.AreaCenter(ho_ConnectedRegions2, out hv_Area2, out hv_Row2, out hv_Column2);
                ho_SelectedRegions2.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions2, "area", "and", hv_Area2.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions2, out hv_Area2, out hv_InitialRow, out hv_InitialColumn);
                //draw_circle (WindowHandle, Row, Column, Radius)
                hv_InitialRadius = dInitialRadius;
                hv_LengthID = dLength;
                hv_MeasureThresholdID = dMeasureThreshold;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_ReducedImageR, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, dFirstCircleRadius,
                    hv_LengthID, hv_MeasureThresholdID, sGenParamValue, PointChoice, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);
                //HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                //HOperatorSet.DispObj(ho_UsedEdges, hv_ExpDefaultWinHandle);
                //HOperatorSet.DispObj(ho_CrossCenter, hv_ExpDefaultWinHandle);
                

            
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
            HOperatorSet.GenEmptyObj(out ho_ReducedImageR);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageG);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageB);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            try
            {
                //畫檢視範圍
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                //畫檢視範圍
                hWindowControl1.HalconWindow.ClearWindow();

                //畫檢視範圍
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //mean_image (ReducedImage, ImageMean, 10, 10)
                ho_ReducedImageR.Dispose(); ho_ReducedImageG.Dispose(); ho_ReducedImageB.Dispose();
                HOperatorSet.Decompose3(ho_ReducedImage, out ho_ReducedImageR, out ho_ReducedImageG,
                    out ho_ReducedImageB);
                HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                hv_GraySet = dGraythreshold;
                ho_Region2.Dispose();
                HOperatorSet.Threshold(ho_ReducedImageR, out ho_Region2, 0, hv_GraySet);
                ho_ConnectedRegions2.Dispose();
                HOperatorSet.Connection(ho_Region2, out ho_ConnectedRegions2);
                HOperatorSet.AreaCenter(ho_ConnectedRegions2, out hv_Area2, out hv_Row2, out hv_Column2);
                ho_SelectedRegions2.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions2, "area", "and", hv_Area2.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions2, out hv_Area2, out hv_InitialRow, out hv_InitialColumn);
                //draw_circle (WindowHandle, Row, Column, Radius)
                hv_InitialRadius = dInitialRadius;
                hv_LengthID = dLength;
                hv_MeasureThresholdID = dMeasureThreshold;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_ReducedImageR, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, dFirstCircleRadius,
                    hv_LengthID, hv_MeasureThresholdID, sGenParamValue, PointChoice, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);
                
                //HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                //HOperatorSet.DispObj(ho_UsedEdges, hv_ExpDefaultWinHandle);
                //HOperatorSet.DispObj(ho_CrossCenter, hv_ExpDefaultWinHandle);



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
            HOperatorSet.GenEmptyObj(out ho_ReducedImageR);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageG);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageB);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            try
            {
                //畫檢視範圍
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                //畫檢視範圍
                hWindowControl1.HalconWindow.ClearWindow();

                //畫檢視範圍
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //mean_image (ReducedImage, ImageMean, 10, 10)
                ho_ReducedImageR.Dispose(); ho_ReducedImageG.Dispose(); ho_ReducedImageB.Dispose();
                HOperatorSet.Decompose3(ho_ReducedImage, out ho_ReducedImageR, out ho_ReducedImageG,
                    out ho_ReducedImageB);
                HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                hv_GraySet = dGraythreshold;
                ho_Region2.Dispose();
                HOperatorSet.Threshold(ho_ReducedImageR, out ho_Region2, 0, hv_GraySet);
                ho_ConnectedRegions2.Dispose();
                HOperatorSet.Connection(ho_Region2, out ho_ConnectedRegions2);
                HOperatorSet.AreaCenter(ho_ConnectedRegions2, out hv_Area2, out hv_Row2, out hv_Column2);
                ho_SelectedRegions2.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions2, "area", "and", hv_Area2.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions2, out hv_Area2, out hv_InitialRow, out hv_InitialColumn);
                //draw_circle (WindowHandle, Row, Column, Radius)
                hv_InitialRadius = dInitialRadius;
                hv_LengthID = dLength;
                hv_MeasureThresholdID = dMeasureThreshold;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_ReducedImageR, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, dFirstCircleRadius,
                    hv_LengthID, hv_MeasureThresholdID, sGenParamValue, PointChoice, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);
                
                //HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                //HOperatorSet.DispObj(ho_UsedEdges, hv_ExpDefaultWinHandle);
                //HOperatorSet.DispObj(ho_CrossCenter, hv_ExpDefaultWinHandle);



            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void btnCircleCenter_Click(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageR);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageG);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageB);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            try
            {
                //畫檢視範圍
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                //畫檢視範圍
                hWindowControl1.HalconWindow.ClearWindow();

                //畫檢視範圍
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height); //得到他的大小
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //mean_image (ReducedImage, ImageMean, 10, 10)
                ho_ReducedImageR.Dispose(); ho_ReducedImageG.Dispose(); ho_ReducedImageB.Dispose();
                HOperatorSet.Decompose3(ho_ReducedImage, out ho_ReducedImageR, out ho_ReducedImageG,
                    out ho_ReducedImageB);
                HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                hv_GraySet = dGraythreshold;
                ho_Region2.Dispose();
                HOperatorSet.Threshold(ho_ReducedImageR, out ho_Region2, 0, hv_GraySet);
                ho_ConnectedRegions2.Dispose();
                HOperatorSet.Connection(ho_Region2, out ho_ConnectedRegions2);
                HOperatorSet.AreaCenter(ho_ConnectedRegions2, out hv_Area2, out hv_Row2, out hv_Column2);
                ho_SelectedRegions2.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions2, "area", "and", hv_Area2.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions2, out hv_Area2, out hv_InitialRow, out hv_InitialColumn);
                //draw_circle (WindowHandle, Row, Column, Radius)
                hv_InitialRadius = dInitialRadius;
                hv_LengthID = dLength;
                hv_MeasureThresholdID = dMeasureThreshold;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_ReducedImageR, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, dFirstCircleRadius,
                    hv_LengthID, hv_MeasureThresholdID, sGenParamValue, "last", out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);
                
                HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_ResultContours, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_CrossCenter, hv_ExpDefaultWinHandle);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void tbRadius_ID_ValueChanged(object sender, EventArgs e)
        {
            nudRadius_ID.Value = tbRadius_ID.Value;
        }

        private void nudRadius_ID_ValueChanged(object sender, EventArgs e)
        {
            dRadius_ID = tbRadius_ID.Value = Convert.ToInt32(nudRadius_ID.Value);

            HOperatorSet.GenEmptyObj(out ho_RegionID);
            HOperatorSet.GenEmptyObj(out ho_RegionTD);
            try
            {
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");

                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                ho_RegionID.Dispose();
                HOperatorSet.GenCircle(out ho_RegionID, hv_ResultRow, hv_ResultColumn, dRadius_ID);
                ho_RegionTD.Dispose();
                HOperatorSet.GenCircle(out ho_RegionTD, hv_ResultRow, hv_ResultColumn, dRadius_TD);
                HOperatorSet.DispObj(ho_RegionID, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_RegionTD, hWindowControl1.HalconWindow);
            }
            catch
            {

            }
        }

        private void tbRadius_TD_ValueChanged(object sender, EventArgs e)
        {
            nudRadius_TD.Value = tbRadius_TD.Value;
        }

        private void nudRadius_TD_ValueChanged(object sender, EventArgs e)
        {
            dRadius_TD = tbRadius_TD.Value = Convert.ToInt32(nudRadius_TD.Value);

            try
            {
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");

                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                ho_RegionID.Dispose();
                HOperatorSet.GenCircle(out ho_RegionID, hv_ResultRow, hv_ResultColumn, dRadius_ID);
                ho_RegionTD.Dispose();
                HOperatorSet.GenCircle(out ho_RegionTD, hv_ResultRow, hv_ResultColumn, dRadius_TD);
                HOperatorSet.DispObj(ho_RegionID, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_RegionTD, hWindowControl1.HalconWindow);
            }
            catch
            {

            }
        }

        private void btnCenterSave_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            My.VDICoating.PointChoice = PointChoice;
            My.VDICoating.dFirstCircleRadius = dFirstCircleRadius;
            My.VDICoating.dReduceRadius = dReduceRadius;
            My.VDICoating.dGraythreshold = dGraythreshold;
            My.VDICoating.dLength = dLength;
            My.VDICoating.dMeasureThreshold = dMeasureThreshold;
            My.VDICoating.sGenParamValue = sGenParamValue;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "FirstCircleRadius", My.VDICoating.dFirstCircleRadius.ToString(), Path);
            IniFile.Write("Setting", "ReduceRadius", My.VDICoating.dReduceRadius.ToString(), Path);
            IniFile.Write("Setting", "Graythreshold", My.VDICoating.dGraythreshold.ToString(), Path);
            IniFile.Write("Setting", "Length", My.VDICoating.dLength.ToString(), Path);
            IniFile.Write("Setting", "MeasureThreshold", My.VDICoating.dMeasureThreshold.ToString(), Path);
            IniFile.Write("Setting", "GenParamValue", My.VDICoating.sGenParamValue.ToString(), Path);
            IniFile.Write("Setting", "PointChoice", My.VDICoating.PointChoice.ToString(), Path);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            My.VDICoating.dRadius_ID = dRadius_ID;
            My.VDICoating.dRadius_TD = dRadius_TD;
            
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "Radius_ID", My.VDICoating.dRadius_ID.ToString(), Path);
            IniFile.Write("Setting", "Radius_TD", My.VDICoating.dRadius_TD.ToString(), Path);
        }

        private void btnLightSave_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("SettingLight", "Light1", tbLightSet_1.Value.ToString(), Path);
            IniFile.Write("SettingLight", "Light2", tbLightSet_2.Value.ToString(), Path);
            IniFile.Write("SettingLight", "Light3", tbLightSet_3.Value.ToString(), Path);
            IniFile.Write("SettingLight", "Light4", tbLightSet_4.Value.ToString(), Path);
        }

        private void cbTestDefect_CheckedChanged(object sender, EventArgs e)
        {
            My.VDICoating.TestDefect = (cbTestDefect.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "TestDefect", My.VDICoating.TestDefect.ToString(), Path);
        }

        private void tbDefectGraySet_ValueChanged(object sender, EventArgs e)
        {
            nudDefectGraySet.Value = tbDefectGraySet.Value;
        }

        private void nudDefectGraySet_ValueChanged(object sender, EventArgs e)
        {
            dDefectGraySet = tbDefectGraySet.Value = Convert.ToInt32(nudDefectGraySet.Value);
            try
            {
                DefectDetecting(hWindowControl1.HalconWindow, My.ho_Image);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                HOperatorSet.DispObj(ho_ImageMean, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Region1, hWindowControl1.HalconWindow);
            }
            catch
            {
            }
        }

        private void nudFilterArea_ValueChanged(object sender, EventArgs e)
        {
            dFilterArea = (double)nudFilterAreaLength.Value;
            try
            {
                DefectDetecting(hWindowControl1.HalconWindow, My.ho_Image);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                HOperatorSet.DispObj(ho_ImageMean, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_SelectedRegionsR, hWindowControl1.HalconWindow);
            }
            catch
            {
            }
        }

        private void nudScratchLength_ValueChanged(object sender, EventArgs e)
        {
            dScratchLength =(double)nudScratchLength.Value;
            try
            {
                DefectDetecting(hWindowControl1.HalconWindow, My.ho_Image);
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

        private void nudLargeArea_ValueChanged(object sender, EventArgs e)
        {
            dLargeArea = (double)nudLargeArea.Value;
            try
            {
                DefectDetecting(hWindowControl1.HalconWindow,My.ho_Image);
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

        public void DefectDetecting(HWindow Window,HObject theImage)
        {
            Window.ClearWindow();
            ho_Image = theImage;
            hv_ExpDefaultWinHandle = Window;
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ReducedImage);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedR);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedG);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedB);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegionsR);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegionsS);
            HOperatorSet.GenEmptyObj(out ho_RegionUnionS);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference1);
            HOperatorSet.GenEmptyObj(out ho_RegionUnionL);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference2);
            HOperatorSet.GenEmptyObj(out ho_RegionUnionR);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegionsR);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegionsL);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegionsR);
            HOperatorSet.GenEmptyObj(out ho_RegionErosionTD);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_RegionDilationID);
            HOperatorSet.GenEmptyObj(out ho_IgnoreID);
            HOperatorSet.GenEmptyObj(out ho_IgnoreTD);
            HOperatorSet.GenEmptyObj(out ho_IgnoreDifference);

            try
            {
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);

                ho_RegionID.Dispose();
                HOperatorSet.GenCircle(out ho_RegionID, hv_ResultRow, hv_ResultColumn, dDefect_ID);
                ho_RegionTD.Dispose();
                HOperatorSet.GenCircle(out ho_RegionTD, hv_ResultRow, hv_ResultColumn, dDefect_TD);
                ////內徑膨脹,過濾掉過曝區域
                //hv_DilationSetID = 5;
                //ho_RegionDilationID.Dispose();
                //HOperatorSet.DilationCircle(ho_RegionID, out ho_RegionDilationID, hv_DilationSetID);
                ////外徑腐蝕,過濾掉過曝區域
                //hv_ErosionSetTD = 10;
                //ho_RegionErosionTD.Dispose();
                //HOperatorSet.ErosionCircle(ho_RegionTD, out ho_RegionErosionTD, hv_ErosionSetTD);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_RegionTD, ho_RegionID, out ho_RegionDifference);

                //過濾掉忽略區域
                if (My.VDICoating.IgnoreOpen)
                {
                    ho_IgnoreID.Dispose();
                    HOperatorSet.GenCircle(out ho_IgnoreID, hv_ResultRow, hv_ResultColumn, dIgnore_ID);
                    ho_IgnoreTD.Dispose();
                    HOperatorSet.GenCircle(out ho_IgnoreTD, hv_ResultRow, hv_ResultColumn, dIgnore_TD);
                    ho_IgnoreDifference.Dispose();
                    HOperatorSet.Difference(ho_IgnoreTD, ho_IgnoreID, out ho_IgnoreDifference);
                    HOperatorSet.Difference(ho_RegionDifference, ho_IgnoreDifference, out ho_RegionDifference);
                }
                HOperatorSet.DispObj(ho_RegionDifference, hv_ExpDefaultWinHandle);

                ho_ReducedImage.Dispose();
                 //mean_image (ReducedImage, ImageMean, 10, 10)
               
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                ho_ImageReducedR.Dispose(); ho_ImageReducedG.Dispose(); ho_ImageReducedB.Dispose();
                HOperatorSet.Decompose3(ho_ImageReduced, out ho_ImageReducedR, out ho_ImageReducedG,
                    out ho_ImageReducedB);
                HOperatorSet.MinMaxGray(ho_RegionDifference, ho_ImageReducedR, 0, out hv_Min,out hv_Max, out hv_Range);
                //將圖片最黑的黑色部分拉到0,增強對比度,白色就不拉,怕有些沒有傷痕的被影響
                ho_ImageScaled.Dispose();
                scale_image_range(ho_ImageReducedR, out ho_ImageScaled, hv_Min, 255);
                ho_ImageMean.Dispose();
                HOperatorSet.MeanImage(ho_ImageScaled, out ho_ImageMean, 3, 3);
                ho_Region1.Dispose();
                HOperatorSet.Threshold(ho_ImageMean, out ho_Region1, dDefectGraySet, 255);
                HOperatorSet.ClosingCircle(ho_Region1, out ho_Region1, 10);
                ho_ConnectedRegions1.Dispose();
                HOperatorSet.Connection(ho_Region1, out ho_ConnectedRegions1);
                HOperatorSet.AreaCenter(ho_ConnectedRegions1, out hv_AreaParticle, out hv_RowParticle,out hv_ColumnParticle);
                //處理particle
                //0.過濾過小的小點點 remain剩下 簡寫R
                hv_UnderSizeSet = dFilterArea/My.VDICoating.mmpixel/1000/2;
                ho_SelectedRegionsR.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegionsR, "outer_radius", "and", hv_UnderSizeSet, 99999999);

                //1.分辨刮痕還是Particle
                //Scratch刮痕簡稱S
                hv_ScratchLengthSet = dScratchLength/My.VDICoating.mmpixel/1000/2;
                ho_SelectedRegionsS.Dispose();
                HTuple A = 0,B = 0,C = 0;
                HOperatorSet.AreaCenter(ho_SelectedRegionsR, out A, out B, out C);
                HOperatorSet.SelectShape(ho_SelectedRegionsR, out ho_SelectedRegionsS, "outer_radius", "and", hv_ScratchLengthSet, 99999999);
                ho_RegionUnionS.Dispose();
                HOperatorSet.Union1(ho_SelectedRegionsS, out ho_RegionUnionS);
                HOperatorSet.CountObj(ho_RegionUnionS, out hv_NumberS);
                //減去刮痕 
                ho_RegionDifference1.Dispose();
                HOperatorSet.Difference(ho_SelectedRegionsR, ho_SelectedRegionsS, out ho_RegionDifference1);
                //2.抓出脫膜  Large Area簡稱L
                hv_LargeAreaSet = dLargeArea/My.VDICoating.mmpixel/1000/2;
                ho_SelectedRegionsL.Dispose();
                HOperatorSet.SelectShape(ho_RegionDifference1, out ho_SelectedRegionsL, "outer_radius", "and", hv_LargeAreaSet, 99999999);
                ho_RegionUnionL.Dispose();
                HOperatorSet.Union1(ho_SelectedRegionsL, out ho_RegionUnionL);
                HOperatorSet.CountObj(ho_RegionUnionL, out hv_NumberL);
                //減去大面積的直徑
                ho_RegionDifference2.Dispose();
                HOperatorSet.Difference(ho_RegionDifference1, ho_RegionUnionL, out ho_RegionDifference2);
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
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                    HOperatorSet.DispObj(ho_RegionUnionL, hv_ExpDefaultWinHandle);
                }
                if ((int)(new HTuple(hv_NumberR.TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple(hv_NumberR.TupleGreater(hv_NumberSetR))) != 0)
                    {
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
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
                HOperatorSet.TupleLength(hv_AreaParticle, out hv_CountParticle);
                set_display_font(hv_ExpDefaultWinHandle, 10, "mono", "true", "false");
                HOperatorSet.DispObj(ho_ConnectedRegions1, hv_ExpDefaultWinHandle);
                HTuple end_val270 = hv_CountParticle - 1;
                HTuple step_val270 = 1;
                //for (hv_j = 0; hv_j.Continue(end_val270, step_val270); hv_j = hv_j.TupleAdd(step_val270))
                //{
                //    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, hv_RowParticle.TupleSelect(hv_j), hv_ColumnParticle.TupleSelect(hv_j));
                //    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, (((((hv_AreaParticle.TupleSelect( hv_j)) * My.VDICoating.mmpixel) * My.VDICoating.mmpixel*1000*1000)).TupleString(".0f")) + "um^2");
                //}
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
            My.VDICoating.dDefectGraySet = dDefectGraySet;
            My.VDICoating.dFilterArea = dFilterArea;
            My.VDICoating.dScratchLength = dScratchLength;
            My.VDICoating.dLargeArea = dLargeArea;
            My.VDICoating.dNumber = dNumber;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "DefectGraySet", My.VDICoating.dDefectGraySet.ToString(), Path);
            IniFile.Write("Setting", "FilterArea", My.VDICoating.dFilterArea.ToString(), Path);
            IniFile.Write("Setting", "ScratchLength", My.VDICoating.dScratchLength.ToString(), Path);
            IniFile.Write("Setting", "LargeArea", My.VDICoating.dLargeArea.ToString(), Path);
            IniFile.Write("Setting", "Number", My.VDICoating.dNumber.ToString(), Path);
        }

        private void cbDetectionColor_CheckedChanged(object sender, EventArgs e)
        {
            My.VDICoating.DetectionColor = (cbDetectionColor.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "DetectionColor", My.VDICoating.DetectionColor.ToString(), Path);
            if (cbDetectionColor.Checked)
            {
                cbDetectionColor_2.Checked = false;
            }
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
                HOperatorSet.GenEmptyObj(out ho_ReducedImageR);
                HOperatorSet.GenEmptyObj(out ho_ReducedImageG);
                HOperatorSet.GenEmptyObj(out ho_ReducedImageB);
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
                ho_ReducedImageR.Dispose(); ho_ReducedImageG.Dispose(); ho_ReducedImageB.Dispose();
                HOperatorSet.Decompose3(ho_ReducedImage, out ho_ReducedImageR, out ho_ReducedImageG,
                    out ho_ReducedImageB);
                hv_MissGray = dMissGray;
                ho_MissRegion.Dispose();
                HOperatorSet.Threshold(ho_ReducedImageR, out ho_MissRegion, 0, hv_MissGray);
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
                HOperatorSet.GenEmptyObj(out ho_ReducedImageR);
                HOperatorSet.GenEmptyObj(out ho_ReducedImageG);
                HOperatorSet.GenEmptyObj(out ho_ReducedImageB);
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
                ho_ReducedImageR.Dispose(); ho_ReducedImageG.Dispose(); ho_ReducedImageB.Dispose();
                HOperatorSet.Decompose3(ho_ReducedImage, out ho_ReducedImageR, out ho_ReducedImageG,
                    out ho_ReducedImageB);
                hv_MissGray = dMissGray;
                ho_MissRegion.Dispose();
                HOperatorSet.Threshold(ho_ReducedImageR, out ho_MissRegion, 0, hv_MissGray);
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
            My.VDICoating.dMissGray = dMissGray;
            My.VDICoating.dMissArea = dMissArea;
            My.VDICoating.dMissOuterRadius = dMissOuterRadius;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "MissGray", My.VDICoating.dMissGray.ToString(), Path);
            IniFile.Write("Setting", "MissArea", My.VDICoating.dMissArea.ToString(), Path);
            IniFile.Write("Setting", "MissOuterRadius", My.VDICoating.dMissOuterRadius.ToString(), Path);
        }

        private void tbDefect_ID_ValueChanged(object sender, EventArgs e)
        {
            nudDefect_ID.Value = tbDefect_ID.Value;
        }

        private void nudDefect_ID_ValueChanged(object sender, EventArgs e)
        {
            dDefect_ID = tbDefect_ID.Value = Convert.ToInt32(nudDefect_ID.Value);

            HOperatorSet.GenEmptyObj(out ho_RegionID);
            HOperatorSet.GenEmptyObj(out ho_RegionTD);
            try
            {
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                
                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                ho_RegionID.Dispose();
                HOperatorSet.GenCircle(out ho_RegionID, hv_ResultRow, hv_ResultColumn, dDefect_ID);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                ho_RegionTD.Dispose();
                HOperatorSet.GenCircle(out ho_RegionTD, hv_ResultRow, hv_ResultColumn, dDefect_TD);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                HOperatorSet.DispObj(ho_RegionID, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_RegionTD, hWindowControl1.HalconWindow);
            }
            catch
            {
                HOperatorSet.SetTposition(hWindowControl1.HalconWindow, 1600, 100);
                HOperatorSet.WriteString(hWindowControl1.HalconWindow, "請先求圓心");
            }
        }

        private void tbDefect_TD_ValueChanged(object sender, EventArgs e)
        {
            nudDefect_TD.Value = tbDefect_TD.Value;
        }

        private void nudDefect_TD_ValueChanged(object sender, EventArgs e)
        {
            dDefect_TD = tbDefect_TD.Value = Convert.ToInt32(nudDefect_TD.Value);

            HOperatorSet.GenEmptyObj(out ho_RegionID);
            HOperatorSet.GenEmptyObj(out ho_RegionTD);
            try
            {
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                
                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                ho_RegionID.Dispose();
                HOperatorSet.GenCircle(out ho_RegionID, hv_ResultRow, hv_ResultColumn, dDefect_ID);
                ho_RegionTD.Dispose();
                HOperatorSet.GenCircle(out ho_RegionTD, hv_ResultRow, hv_ResultColumn, dDefect_TD);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                HOperatorSet.DispObj(ho_RegionID, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_RegionTD, hWindowControl1.HalconWindow);
            }
            catch
            {
                HOperatorSet.SetTposition(hWindowControl1.HalconWindow, 1600, 100);
                HOperatorSet.WriteString(hWindowControl1.HalconWindow, "請先求圓心");
            }
        }

        private void btnSave5_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            My.VDICoating.dDefect_ID = dDefect_ID;
            My.VDICoating.dDefect_TD = dDefect_TD;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "Defect_ID", My.VDICoating.dDefect_ID.ToString(), Path);
            IniFile.Write("Setting", "Defect_TD", My.VDICoating.dDefect_TD.ToString(), Path);
        }

        private void btnCircleCenter2_Click(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_ReducedImageR);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageG);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageB);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            try
            {
                //畫檢視範圍
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                //畫檢視範圍
                hWindowControl1.HalconWindow.ClearWindow();

                //畫檢視範圍
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //mean_image (ReducedImage, ImageMean, 10, 10)
                ho_ReducedImageR.Dispose(); ho_ReducedImageG.Dispose(); ho_ReducedImageB.Dispose();
                HOperatorSet.Decompose3(ho_ReducedImage, out ho_ReducedImageR, out ho_ReducedImageG,
                    out ho_ReducedImageB);
                HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                hv_GraySet = dGraythreshold;
                ho_Region2.Dispose();
                HOperatorSet.Threshold(ho_ReducedImageR, out ho_Region2, 0, hv_GraySet);
                ho_ConnectedRegions2.Dispose();
                HOperatorSet.Connection(ho_Region2, out ho_ConnectedRegions2);
                HOperatorSet.AreaCenter(ho_ConnectedRegions2, out hv_Area2, out hv_Row2, out hv_Column2);
                ho_SelectedRegions2.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions2, "area", "and", hv_Area2.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions2, out hv_Area2, out hv_InitialRow, out hv_InitialColumn);
                //draw_circle (WindowHandle, Row, Column, Radius)
                hv_InitialRadius = dInitialRadius;
                hv_LengthID = dLength;
                hv_MeasureThresholdID = dMeasureThreshold;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_ReducedImageR, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, dFirstCircleRadius,
                    hv_LengthID, hv_MeasureThresholdID, sGenParamValue, "last", out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);

                HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_ResultContours, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_CrossCenter, hv_ExpDefaultWinHandle);
            }
            catch
            {
                HOperatorSet.SetTposition(hWindowControl1.HalconWindow, 1600, 100);
                HOperatorSet.WriteString(hWindowControl1.HalconWindow, "請先求圓心");
            }
        }

        private void nudUpperLimit_A_ValueChanged(object sender, EventArgs e)
        {
            dUpperLimit_A = (double)nudUpperLimit_A.Value;
        }

        private void nudLowerLimit_A_ValueChanged(object sender, EventArgs e)
        {
            dLowerLimit_A = (double)nudLowerLimit_A.Value;
        }

        private void nudUpperLimit_B_ValueChanged(object sender, EventArgs e)
        {
            dUpperLimit_B = (double)nudUpperLimit_B.Value;
        }

        private void nudLowerLimit_B_ValueChanged(object sender, EventArgs e)
        {
            dLowerLimit_B = (double)nudLowerLimit_B.Value;
        }

        private void cbDetectionColor_2_CheckedChanged(object sender, EventArgs e)
        {
            My.VDICoating.DetectionColor_2 = (cbDetectionColor_2.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "DetectionColor_2", My.VDICoating.DetectionColor_2.ToString(), Path);
            if (cbDetectionColor_2.Checked)
            {
                cbDetectionColor.Checked = false;
            }
        }

        private void btnSave_6_Click(object sender, EventArgs e)
        {
            My.VDICoating.dUpperLimit_A = dUpperLimit_A;
            My.VDICoating.dLowerLimit_A = dLowerLimit_A;
            My.VDICoating.dUpperLimit_B = dUpperLimit_B;
            My.VDICoating.dLowerLimit_B = dLowerLimit_B;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "UpperLimit_A", My.VDICoating.dUpperLimit_A.ToString(), Path);
            IniFile.Write("Setting", "LowerLimit_A", My.VDICoating.dLowerLimit_A.ToString(), Path);
            IniFile.Write("Setting", "UpperLimit_B", My.VDICoating.dUpperLimit_B.ToString(), Path);
            IniFile.Write("Setting", "LowerLimit_B", My.VDICoating.dLowerLimit_B.ToString(), Path);
        }

        private void btnCircleCenter3_Click(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageR);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageG);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageB);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            try
            {
                //畫檢視範圍
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                //畫檢視範圍
                hWindowControl1.HalconWindow.ClearWindow();

                //畫檢視範圍
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height); //得到他的大小
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //mean_image (ReducedImage, ImageMean, 10, 10)
                ho_ReducedImageR.Dispose(); ho_ReducedImageG.Dispose(); ho_ReducedImageB.Dispose();
                HOperatorSet.Decompose3(ho_ReducedImage, out ho_ReducedImageR, out ho_ReducedImageG,
                    out ho_ReducedImageB);
                HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                hv_GraySet = dGraythreshold;
                ho_Region2.Dispose();
                HOperatorSet.Threshold(ho_ReducedImageR, out ho_Region2, 0, hv_GraySet);
                ho_ConnectedRegions2.Dispose();
                HOperatorSet.Connection(ho_Region2, out ho_ConnectedRegions2);
                HOperatorSet.AreaCenter(ho_ConnectedRegions2, out hv_Area2, out hv_Row2, out hv_Column2);
                ho_SelectedRegions2.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions2, "area", "and", hv_Area2.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions2, out hv_Area2, out hv_InitialRow, out hv_InitialColumn);
                //draw_circle (WindowHandle, Row, Column, Radius)
                hv_InitialRadius = dInitialRadius;
                hv_LengthID = dLength;
                hv_MeasureThresholdID = dMeasureThreshold;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_ReducedImageR, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, dFirstCircleRadius,
                    hv_LengthID, hv_MeasureThresholdID, sGenParamValue, "last", out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);

                HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_ResultContours, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_CrossCenter, hv_ExpDefaultWinHandle);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void cbColorRangeChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbColorRangeChoice.SelectedIndex < 0)
                return;
            My.VDICoating.ColorRangeChoice = cbColorRangeChoice.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "ColorRangeChoice", My.VDICoating.ColorRangeChoice.ToString(), Path);
        }

        private void tbCenterDistance_ValueChanged(object sender, EventArgs e)
        {
            nudCenterDistance.Value = tbCenterDistance.Value;
        }

        private void nudCenterDistance_ValueChanged(object sender, EventArgs e)
        {
            dCenterDistance = tbCenterDistance.Value = Convert.ToInt32(nudCenterDistance.Value);

            HOperatorSet.GenEmptyObj(out ho_Circle_up);
            HOperatorSet.GenEmptyObj(out ho_Circle_down);
            HOperatorSet.GenEmptyObj(out ho_Circle_left);
            HOperatorSet.GenEmptyObj(out ho_Circle_right);
            try
            {
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");

                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                ho_Circle_up.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_up, hv_ResultRow - dCenterDistance / pixel2um, hv_ResultColumn, dRangeRadius * pixel2um);
                ho_Circle_down.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_down, hv_ResultRow + dCenterDistance / pixel2um, hv_ResultColumn, dRangeRadius * pixel2um);
                ho_Circle_left.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_left, hv_ResultRow, hv_ResultColumn - dCenterDistance / pixel2um, dRangeRadius * pixel2um);
                ho_Circle_right.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_right, hv_ResultRow, hv_ResultColumn + dCenterDistance / pixel2um, dRangeRadius * pixel2um);
                HOperatorSet.DispObj(ho_Circle_up, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Circle_down, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Circle_left, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Circle_right, hWindowControl1.HalconWindow);
            }
            catch
            {

            }
        }

        private void tbRangeRadius_ValueChanged(object sender, EventArgs e)
        {
            nudRangeRadius.Value = tbRangeRadius.Value;
        }

        private void nudRangeRadius_ValueChanged(object sender, EventArgs e)
        {
            dRangeRadius = tbRangeRadius.Value = Convert.ToInt32(nudRangeRadius.Value);
            
            HOperatorSet.GenEmptyObj(out ho_Circle_up);
            HOperatorSet.GenEmptyObj(out ho_Circle_down);
            HOperatorSet.GenEmptyObj(out ho_Circle_left);
            HOperatorSet.GenEmptyObj(out ho_Circle_right);
            try
            {
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");

                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                ho_Circle_up.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_up, hv_ResultRow - dCenterDistance / pixel2um, hv_ResultColumn, dRangeRadius * pixel2um);
                ho_Circle_down.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_down, hv_ResultRow + dCenterDistance / pixel2um, hv_ResultColumn, dRangeRadius * pixel2um);
                ho_Circle_left.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_left, hv_ResultRow, hv_ResultColumn - dCenterDistance / pixel2um, dRangeRadius * pixel2um);
                ho_Circle_right.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_right, hv_ResultRow, hv_ResultColumn + dCenterDistance / pixel2um, dRangeRadius * pixel2um);
                HOperatorSet.DispObj(ho_Circle_up, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Circle_down, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Circle_left, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(ho_Circle_right, hWindowControl1.HalconWindow);
            }
            catch
            {

            }
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            My.VDICoating.dRangeRadius = dRangeRadius;
            My.VDICoating.dCenterDistance = dCenterDistance;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "RangeRadius", My.VDICoating.dRangeRadius.ToString(), Path);
            IniFile.Write("Setting", "CenterDistance", My.VDICoating.dCenterDistance.ToString(), Path);
        }

        private void cbDarkLightChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDarkLightChoice.SelectedIndex < 0)
                return;
            My.VDICoating.DarkLightChoice = cbDarkLightChoice.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "DarkLightChoice", My.VDICoating.DarkLightChoice.ToString(), Path);
        }

        private void tbIgnore_ID_ValueChanged(object sender, EventArgs e)
        {
            nudIgnore_ID.Value = tbIgnore_ID.Value;
        }

        private void nudIgnore_ID_ValueChanged(object sender, EventArgs e)
        {
            dIgnore_ID = tbIgnore_ID.Value = Convert.ToInt32(nudIgnore_ID.Value);

            HOperatorSet.GenEmptyObj(out ho_IgnoreID);
            HOperatorSet.GenEmptyObj(out ho_IgnoreTD);
            try
            {
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");

                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                ho_RegionID.Dispose();
                HOperatorSet.GenCircle(out ho_IgnoreID, hv_ResultRow, hv_ResultColumn, dIgnore_ID);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                ho_RegionTD.Dispose();
                HOperatorSet.GenCircle(out ho_IgnoreTD, hv_ResultRow, hv_ResultColumn, dIgnore_TD);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                HOperatorSet.DispObj(ho_IgnoreID, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_IgnoreTD, hWindowControl1.HalconWindow);
            }
            catch
            {
                HOperatorSet.SetTposition(hWindowControl1.HalconWindow, 1600, 100);
                HOperatorSet.WriteString(hWindowControl1.HalconWindow, "請先求圓心");
            }
        }

        private void tbIgnore_TD_ValueChanged(object sender, EventArgs e)
        {
            nudIgnore_TD.Value = tbIgnore_TD.Value;
        }

        private void nudIgnore_TD_ValueChanged(object sender, EventArgs e)
        {
            dIgnore_TD = tbIgnore_TD.Value = Convert.ToInt32(nudIgnore_TD.Value);

            HOperatorSet.GenEmptyObj(out ho_IgnoreID);
            HOperatorSet.GenEmptyObj(out ho_IgnoreTD);
            try
            {
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");

                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                ho_RegionID.Dispose();
                HOperatorSet.GenCircle(out ho_IgnoreID, hv_ResultRow, hv_ResultColumn, dIgnore_ID);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                ho_RegionTD.Dispose();
                HOperatorSet.GenCircle(out ho_IgnoreTD, hv_ResultRow, hv_ResultColumn, dIgnore_TD);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                HOperatorSet.DispObj(ho_IgnoreID, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_IgnoreTD, hWindowControl1.HalconWindow);
            }
            catch
            {
                HOperatorSet.SetTposition(hWindowControl1.HalconWindow, 1600, 100);
                HOperatorSet.WriteString(hWindowControl1.HalconWindow, "請先求圓心");
            }
        }

        private void cbPointChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPointChoice.SelectedIndex == 0)
                PointChoice = "first";
            else
                PointChoice = "last";

            HOperatorSet.GenEmptyObj(out ho_ReducedImageR);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageG);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageB);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);

            try
            {
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                //畫檢視範圍
                hWindowControl1.HalconWindow.ClearWindow();

                //畫檢視範圍
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //mean_image (ReducedImage, ImageMean, 10, 10)
                ho_ReducedImageR.Dispose(); ho_ReducedImageG.Dispose(); ho_ReducedImageB.Dispose();
                HOperatorSet.Decompose3(ho_ReducedImage, out ho_ReducedImageR, out ho_ReducedImageG,
                    out ho_ReducedImageB);
                HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                hv_GraySet = dGraythreshold;
                ho_Region2.Dispose();
                HOperatorSet.Threshold(ho_ReducedImageR, out ho_Region2, 0, hv_GraySet);
                ho_ConnectedRegions2.Dispose();
                HOperatorSet.Connection(ho_Region2, out ho_ConnectedRegions2);
                HOperatorSet.AreaCenter(ho_ConnectedRegions2, out hv_Area2, out hv_Row2, out hv_Column2);
                ho_SelectedRegions2.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions2, "area", "and", hv_Area2.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions2, out hv_Area2, out hv_InitialRow, out hv_InitialColumn);
                //draw_circle (WindowHandle, Row, Column, Radius)
                hv_InitialRadius = dInitialRadius;
                hv_LengthID = dLength;
                hv_MeasureThresholdID = dMeasureThreshold;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_ReducedImageR, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, dFirstCircleRadius,
                    hv_LengthID, hv_MeasureThresholdID, sGenParamValue, PointChoice, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);
                //HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                //HOperatorSet.DispObj(ho_UsedEdges, hv_ExpDefaultWinHandle);
                //HOperatorSet.DispObj(ho_CrossCenter, hv_ExpDefaultWinHandle);



            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void cbIgnoreOpen_CheckedChanged(object sender, EventArgs e)
        {
            My.VDICoating.IgnoreOpen = (cbIgnoreOpen.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "IgnoreOpen", My.VDICoating.IgnoreOpen.ToString(), Path);
        }

        private void btnSave6_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            My.VDICoating.dIgnore_ID = dIgnore_ID;
            My.VDICoating.dIgnore_TD = dIgnore_TD;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "Ignore_ID", My.VDICoating.dIgnore_ID.ToString(), Path);
            IniFile.Write("Setting", "Ignore_TD", My.VDICoating.dIgnore_TD.ToString(), Path);
        }

        private void tbMissOuterRadius_ValueChanged(object sender, EventArgs e)
        {
            nudMissOuterRadius.Value = tbMissOuterRadius.Value;
        }

        private void nudMissOuterRadius_ValueChanged(object sender, EventArgs e)
        {
            dMissOuterRadius = tbMissOuterRadius.Value = Convert.ToInt32(nudMissOuterRadius.Value);
            try
            {
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
                HOperatorSet.GenEmptyObj(out ho_ReducedImageR);
                HOperatorSet.GenEmptyObj(out ho_ReducedImageG);
                HOperatorSet.GenEmptyObj(out ho_ReducedImageB);
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
                ho_ReducedImageR.Dispose(); ho_ReducedImageG.Dispose(); ho_ReducedImageB.Dispose();
                HOperatorSet.Decompose3(ho_ReducedImage, out ho_ReducedImageR, out ho_ReducedImageG,
                    out ho_ReducedImageB);
                hv_MissGray = dMissGray;
                ho_MissRegion.Dispose();
                HOperatorSet.Threshold(ho_ReducedImageR, out ho_MissRegion, 0, hv_MissGray);
                ho_MissConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_MissRegion, out ho_MissConnectedRegions);
                hv_MissAreaSet = dMissArea;
                ho_MissSelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_MissConnectedRegions, out ho_MissSelectedRegions,
                    "area", "and", hv_MissAreaSet, 9999999);
                HOperatorSet.SelectShape(ho_MissSelectedRegions, out ho_MissSelectedRegions,
                    "outer_radius", "and", 0, dMissOuterRadius);
                HOperatorSet.DispObj(ho_MissSelectedRegions, hv_ExpDefaultWinHandle);
            }
            catch
            {
                //MessageBox.Show("error");
            }
            Thread.Sleep(1);
        }

        private void nudAimCirR_ValueChanged(object sender, EventArgs e)
        {
            My.VDICoating.AimCirR = (double)nudAimCirR.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "AimCirR", My.VDICoating.AimCirR.ToString(), Path);
        }

        private void btnRevise_Click(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageR);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageG);
            HOperatorSet.GenEmptyObj(out ho_ReducedImageB);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            try
            {
                //畫檢視範圍
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                //畫檢視範圍
                hWindowControl1.HalconWindow.ClearWindow();

                //畫檢視範圍
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height); //得到他的大小
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                //mean_image (ReducedImage, ImageMean, 10, 10)
                ho_ReducedImageR.Dispose(); ho_ReducedImageG.Dispose(); ho_ReducedImageB.Dispose();
                HOperatorSet.Decompose3(ho_ReducedImage, out ho_ReducedImageR, out ho_ReducedImageG,
                    out ho_ReducedImageB);
                HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                hv_GraySet = dGraythreshold;
                ho_Region2.Dispose();
                HOperatorSet.Threshold(ho_ReducedImageR, out ho_Region2, 0, hv_GraySet);
                ho_ConnectedRegions2.Dispose();
                HOperatorSet.Connection(ho_Region2, out ho_ConnectedRegions2);
                HOperatorSet.AreaCenter(ho_ConnectedRegions2, out hv_Area2, out hv_Row2, out hv_Column2);
                ho_SelectedRegions2.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions2, "area", "and", hv_Area2.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions2, out hv_Area2, out hv_InitialRow, out hv_InitialColumn);
                //draw_circle (WindowHandle, Row, Column, Radius)
                hv_InitialRadius = dInitialRadius;
                hv_LengthID = dLength;
                hv_MeasureThresholdID = dMeasureThreshold;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_ReducedImageR, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, dFirstCircleRadius,
                    hv_LengthID, hv_MeasureThresholdID, sGenParamValue, "last", out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);

                HOperatorSet.DispObj(ho_ReducedImageR, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_ResultContours, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_CrossCenter, hv_ExpDefaultWinHandle);
                My.VDICoating.mmpixel = Math.Round(double.Parse(nudAimCirR.Text)/(double)hv_ResultRadius /2 , 5);
                txtPpix.Text = (Math.Round(My.VDICoating.mmpixel, 5)).ToString();
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

                IniFile.Write("Setting", "mmpixel", My.VDICoating.mmpixel.ToString(), Path);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void btnDefaultSetting_Click(object sender, EventArgs e)
        {
            My.VDICoating.mmpixel = 0.0044;
            txtPpix.Text = (Math.Round(My.VDICoating.mmpixel, 5)).ToString();
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "mmpixel", My.VDICoating.mmpixel.ToString(), Path);
        }

        private void btnCCDSetSave_1_Click(object sender, EventArgs e)
        {

        }

      

       


    }
}
