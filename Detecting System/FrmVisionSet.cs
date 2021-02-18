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
    
    public partial class FrmVisionSet : Form
    {
        FrmParent parent;
        FrmRun Run;
        public FrmVisionSet(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;
            InitializeComponent();
        }
        bool bReadingPara = true;
        #region 參數
        public HTuple dFirstCircleRow = 0;
        public HTuple dFirstCircleColumn = 0;
        public HTuple dFirstCircleRadius = 0;
        //檢測區域半徑
        public static double dReduceRadius = 1;
        //抓圓中的灰度閥值
        public static double dGraythreshold = 1;
        //抓圓心的圓半徑
        public static HTuple dCenterRadius = 1;
        //擬合圓線條長度
        public static double dLength = 1;
        //擬合圓灰度差異
        public static double dMeasureThreshold = 20;
        //擬合圓白找黑或黑找白
        public static string sGenParamValue = "negative";
        //固定環外圍
        public static double dRingOutRange = 1;
        //固定環內圍
        public static double dRingInRange = 1;
        //方法選擇
        public static int MethodChoice = 0;
        //固定環灰度值(黑)
        public static double dGraythresholdBlack = 1;
        //固定環灰度值(白)
        public static double dGraythresholdWhite = 1;
        //過濾過小面積
        public static double dUnderSizeArea = 1;
        //方法二黑白選擇
        public static int GlueLightDarkChoice = 0;
        //方法二差異閥值
        public static decimal dDynThresholdSet = 1;
        //方法二過濾小面積
        public static int iUnderSizeArea2 = 1;
        //方法二平滑一寬
        public static double dMeanWidth_1 = 1;
        //方法二平滑一高
        public static double dMeanHeight_1 = 1;
        //方法二平滑二寬
        public static double dMeanWidth_2 = 1;
        //方法二平滑二高
        public static double dMeanHeight_2 = 1;
        //方法二連接寬
        public static double dCloseWidthValue = 1;
        //方法二連接高
        public static double dCloseHeightValue = 1;
        //方法二斷開寬
        public static double dOpenWidthValue = 1;
        //方法二斷開高
        public static double dOpenHeightValue = 1;
        //方法二過濾寬度上限
        public static double dFilterWidth_Upper = 1;
        //方法二過濾寬度下限
        public static double dFilterWidth_Lower = 1;
        //方法二過濾高度上限
        public static double dFilterHeight_Upper = 1;
        //方法二過濾高度下限
        public static double dFilterHeight_Lower = 1;
       
        //方法三黑白選擇
        public static int ContrastSet = 0;
        //方法三對比度值
        public static int ContrastValue = 0;
        //方法三面積上下限
        public static int Area_Upper = 0;
        public static int Area_Lower = 0;
        //方法三圓度上下限
        public static int Roundness_Upper = 0;
        public static int Roundness_Lower = 0;
        //方法三矩形度上下限
        public static int Rectangularity_Upper = 0;
        public static int Rectangularity_Lower = 0;
        
        //方法三檢測面積/圓度/矩形度
        public static bool DetectionArea = false;
        public static bool DetectionRoundness = false;
        public static bool DetectionRectangularity = false;
        //固定環膠水角度
        public static int iGlueAngleSet = 1;
        //固定環膠水閥值
        public static int iGlueRatioSet = 1;
        //檢測角度
        public static double dAngleSet = 1;
        //最大斷膠檢測角度設定
        public static double dLackMaxAngleSet = 1;
        //小台階外圍
        public static double dOutRangePF = 1;
        //小台階內圍
        public static double dInRangePF = 1;
        //小台階黑閥值
        public static double dGraythresholdBlackPF = 1;
        //小台階白閥值
        public static double dGraythresholdWhitePF = 1;
        //小台階過濾小面積
        public static double dUnderSizeAreaPF = 1;
        //小台階膠水角度閥值
        public static int iGlueAngleSetPF = 1;
        //小台階膠水面積閥值
        public static int iGlueRatioSetPF = 1;
        //小台階檢測角度設定
        public static double dAngleSetPF = 1;
        //小台階最大斷膠檢測角度設定
        public static double dLackMaxAngleSetPF = 1;
        //小台階方法二差異閥值
        public static int iDynthresholdDarkPF2 = 1;
        public static int iDynthresholdLightPF2 = 1;
        //小台階方法二灰度閥值
        public static int iGraythresholdBlackPF2 = 1;
        public static int iGraythresholdWhitePF2 = 1;
        //小台階方法二過濾小面積
        public static int iUnderSizeAreaPF2 = 1;
        public static int iCloseWidthPF2 = 1;
        public static int iCloseHeightPF2 = 1;
        public static int iOpenWidthPF2 = 1;
        public static int iOpenHeightPF2 = 1;
        //斜坡外圍
        public static double dOutSlope = 1;
        //斜坡內圍
        public static double dInSlope = 1;
        //斜坡過濾過小面積
        public static double dSpilledUnderSizeArea = 1;
        //溢膠外圍
        public static double dOutSlope2 = 1;
        //溢膠內圍
        public static double dInSlope2 = 1;
        //濾波閥值High
        public static double dSmoothingHigh = 1;
        //濾波閥值Low
        public static double dSmoothingLow = 1;
        //過濾溢膠過小面積
        public static double dSpilledUnderSizeArea2 = 1;
        //表面檢測外圍
        public static double dOutSlope3 = 1;
        //表面檢測內圍
        public static double dInSlope3 = 1;
        //表面灰度值
        public static double dGraythreshold3 = 1;
        //檢測角度
        public static double dAngleSet2 = 0;
        //外接矩形長軸下限值
        public static double dRect2_Len1Lower = 1;
        //外接矩形長軸上限值
        public static double dRect2_Len1Upper = 1;
        //外接矩形短軸下限值
        public static double dRect2_Len2Lower = 1;
        //外接矩形短軸上限值
        public static double dRect2_Len2Upper = 1;
        //過濾表面膠水小面積
        public static double dSpilledUnderSizeArea3 = 1;
        //縫隙外圍
        public static double dOutSlope4 = 1;
        //縫隙內圍
        public static double dInSlope4 = 1;
        //濾波閥值High
        public static double dSmoothingHigh2 = 1;
        //濾波閥值Low
        public static double dSmoothingLow2 = 1;
        //過濾溢膠過小面積
        public static double dSpilledUnderSizeArea4 = 1;
        //畫Mark點的矩形
        public static double dRow10 = 1;
        public static double dColumn10 = 1;
        public static double dPhi10 = 1;
        public static double dLength10 = 1;
        public static double dLength20 = 1;
        //Mark外徑
        public static double dMarkTD = 1;
        //Mark內徑
        public static double dMarkID = 1;
        //Mark灰度值
        public static double dMarkGraythreshold = 1;
        //Mark分數
        public static double dMarkGrade = 1;
        //模組二灰度直
        public static double dModuleGraythreshold_2 = 1;
        //模組二分數
        public static double dModuleGrade_2 = 1;
        //模組三分數
        public static double dModuleGrade_3 = 1;

        //找方向方法
        public static int iTestDirectionChoice = 1;

        public static string LogResult = "";
        #endregion
        #region Halcon參數1
        public HTuple hv_ExpDefaultWinHandle;
        public void find_angle_plus(HObject ho_Image, HObject ho_Region1, HObject ho_Region2,
     out HObject ho_SelectRegion1, out HObject ho_SelectRegion2, HTuple hv_AngleSet,
     out HTuple hv_TotalAngle, out HTuple hv_LackAngle_Max)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Rectangle = null, ho_RegionIntersection1 = null;
            HObject ho_RegionIntersection2 = null;

            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_Count = null;
            HTuple hv_OnceWidth = null, hv_Last = null, hv_Ratio = null;
            HTuple hv_LackAngle = null, hv_j = null, hv_i = null, hv_RegionArea1 = new HTuple();
            HTuple hv_RegionArea2 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_SelectRegion1);
            HOperatorSet.GenEmptyObj(out ho_SelectRegion2);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection1);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection2);
            try
            {
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                hv_Count = 360 / hv_AngleSet;
                hv_OnceWidth = hv_Width / hv_Count;
                hv_Last = 0;
                hv_Ratio = 0;
                hv_LackAngle = 0;
                hv_TotalAngle = 0;
                hv_LackAngle_Max = 0;
                hv_j = 0;
                ho_SelectRegion1.Dispose();
                HOperatorSet.GenEmptyObj(out ho_SelectRegion1);
                ho_SelectRegion2.Dispose();
                HOperatorSet.GenEmptyObj(out ho_SelectRegion2);
                //求兩次循環是為了找尋最大斷膠角度
                HTuple end_val12 = (hv_Count * 2) - 1;
                HTuple step_val12 = 1;
                for (hv_i = 0; hv_i.Continue(end_val12, step_val12); hv_i = hv_i.TupleAdd(step_val12))
                {

                    if ((int)(new HTuple(hv_i.TupleLess(hv_Count))) != 0)
                    {
                        hv_j = hv_i.Clone();
                    }
                    else
                    {
                        hv_j = hv_i - hv_Count;
                    }
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle, 0, (hv_Width * hv_j) / hv_Count,
                        hv_Height - 1, ((hv_Width * (hv_j + 1)) / hv_Count) - 1);
                    //有膠部份
                    ho_RegionIntersection1.Dispose();
                    HOperatorSet.Intersection(ho_Region1, ho_Rectangle, out ho_RegionIntersection1
                        );
                    ho_RegionIntersection2.Dispose();
                    HOperatorSet.Intersection(ho_Region2, ho_Rectangle, out ho_RegionIntersection2
                        );
                    HOperatorSet.RegionFeatures(ho_RegionIntersection1, "area", out hv_RegionArea1);
                    HOperatorSet.RegionFeatures(ho_RegionIntersection2, "area", out hv_RegionArea2);
                    //
                    if ((int)(new HTuple(hv_i.TupleLess(hv_Count))) != 0)
                    {
                        if ((int)(new HTuple(hv_RegionArea1.TupleGreater(0))) != 0)
                        {
                            hv_TotalAngle = hv_TotalAngle + hv_AngleSet;
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(ho_SelectRegion1, ho_RegionIntersection1, out ExpTmpOutVar_0
                                    );
                                ho_SelectRegion1.Dispose();
                                ho_SelectRegion1 = ExpTmpOutVar_0;
                            }
                        }
                        else if ((int)(new HTuple(hv_RegionArea2.TupleGreater(0))) != 0)
                        {
                            hv_TotalAngle = hv_TotalAngle + hv_AngleSet;
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(ho_SelectRegion2, ho_RegionIntersection2, out ExpTmpOutVar_0
                                    );
                                ho_SelectRegion2.Dispose();
                                ho_SelectRegion2 = ExpTmpOutVar_0;
                            }
                        }
                    }
                    //
                    if ((int)((new HTuple(hv_RegionArea1.TupleEqual(0))).TupleAnd(new HTuple(hv_RegionArea2.TupleEqual(
                        0)))) != 0)
                    {
                        hv_LackAngle = hv_LackAngle + hv_AngleSet;
                        if ((int)(new HTuple(hv_LackAngle_Max.TupleLess(hv_LackAngle))) != 0)
                        {
                            hv_LackAngle_Max = hv_LackAngle.Clone();
                        }
                        //
                    }
                    else
                    {
                        hv_LackAngle = 0;
                    }
                    //
                    //
                }
                //
                //
                //
                //
                //
                ho_Rectangle.Dispose();
                ho_RegionIntersection1.Dispose();
                ho_RegionIntersection2.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Rectangle.Dispose();
                ho_RegionIntersection1.Dispose();
                ho_RegionIntersection2.Dispose();

                throw HDevExpDefaultException;
            }
        }


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
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
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
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row1, hv_Column1, 6, 0.785398);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "used_edges",
                    "row", out hv_UsedRow);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "used_edges",
                    "column", out hv_UsedColumn);
                ho_UsedEdges.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_UsedEdges, hv_UsedRow, hv_UsedColumn,
                    10, (new HTuple(45)).TupleRad());
                HOperatorSet.GetMetrologyObjectResultContour(out ho_ResultContours, hv_MetrologyHandle,
                    "all", "all", 1.5);
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
        /// <summary>
        /// 計算角度
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <param name="ho_Region"></param>
        /// <param name="ho_SelectRegion"></param>
        /// <param name="ho_IgnoreRegion"></param>
        /// <param name="hv_AngleSet"></param>
        /// <param name="hv_RatioSet"></param>
        /// <param name="hv_TotalAngle"></param>
        /// <param name="hv_LackAngle_Max"></param>
        public void find_angle(HObject ho_Image, HObject ho_Region, out HObject ho_SelectRegion,
             out HObject ho_IgnoreRegion, HTuple hv_AngleSet, HTuple hv_RatioSet, out HTuple hv_TotalAngle,
             out HTuple hv_LackAngle_Max)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Rectangle = null, ho_RegionIntersection = null;

            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_Count = null;
            HTuple hv_OnceWidth = null, hv_Last = null, hv_Ratio = null;
            HTuple hv_LackAngle = null, hv_j = null, hv_i = null, hv_RectangleArea = new HTuple();
            HTuple hv_RegionArea = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_SelectRegion);
            HOperatorSet.GenEmptyObj(out ho_IgnoreRegion);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            try
            {
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                hv_Count = 360 / hv_AngleSet;
                hv_OnceWidth = hv_Width / hv_Count;
                hv_Last = 0;
                hv_Ratio = 0;
                hv_LackAngle = 0;
                hv_TotalAngle = 0;
                hv_LackAngle_Max = 0;
                hv_j = 0;
                ho_SelectRegion.Dispose();
                HOperatorSet.GenEmptyObj(out ho_SelectRegion);
                ho_IgnoreRegion.Dispose();
                HOperatorSet.GenEmptyObj(out ho_IgnoreRegion);
                //求兩次循環是為了找尋最大斷膠角度
                HTuple end_val12 = (hv_Count * 2) - 1;
                HTuple step_val12 = 1;
                for (hv_i = 0; hv_i.Continue(end_val12, step_val12); hv_i = hv_i.TupleAdd(step_val12))
                {
                    if ((int)(new HTuple(hv_i.TupleLess(hv_Count))) != 0)
                    {
                        hv_j = hv_i.Clone();
                    }
                    else
                    {
                        hv_j = hv_i - hv_Count;
                    }
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle, 0, hv_OnceWidth * hv_j, hv_Height - 1,
                        (hv_OnceWidth * (hv_j + 1)) - 1);
                    //有膠部份
                    ho_RegionIntersection.Dispose();
                    HOperatorSet.Intersection(ho_Region, ho_Rectangle, out ho_RegionIntersection
                        );
                    HOperatorSet.RegionFeatures(ho_Rectangle, "area", out hv_RectangleArea);
                    HOperatorSet.RegionFeatures(ho_RegionIntersection, "area", out hv_RegionArea);

                    hv_Ratio = (hv_RegionArea / hv_RectangleArea) * 100;
                    if (hv_Ratio >= hv_RatioSet && hv_i < hv_Count)
                    {
                        hv_TotalAngle = hv_TotalAngle + hv_AngleSet;
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_SelectRegion, ho_RegionIntersection, out ExpTmpOutVar_0
                                );
                            ho_SelectRegion.Dispose();
                            ho_SelectRegion = ExpTmpOutVar_0;
                        }
                    }
                    else if ((hv_Ratio < hv_RatioSet && hv_i < hv_Count))
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_IgnoreRegion, ho_RegionIntersection, out ExpTmpOutVar_0
                                );
                            ho_IgnoreRegion.Dispose();
                            ho_IgnoreRegion = ExpTmpOutVar_0;
                        }
                    }

                    if ((int)(new HTuple(hv_Ratio.TupleLess(hv_RatioSet))) != 0)
                    {
                        hv_LackAngle = hv_LackAngle + hv_AngleSet;
                        if ((int)(new HTuple(hv_LackAngle_Max.TupleLess(hv_LackAngle))) != 0)
                        {
                            hv_LackAngle_Max = hv_LackAngle.Clone();
                        }

                    }
                    else
                    {
                        hv_LackAngle = 0;
                    }


                }





                ho_Rectangle.Dispose();
                ho_RegionIntersection.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Rectangle.Dispose();
                ho_RegionIntersection.Dispose();

                throw HDevExpDefaultException;
            }
        }
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
        //自己做的畫傘型函數
        public void gen_sector(out HObject ho_Sector, HTuple hv_Row, HTuple hv_Column,
      HTuple hv_InnerDiam, HTuple hv_TopDiam, HTuple hv_Direction)
        {



            // Local iconic variables 

            HObject ho_Circle11, ho_Circle12, ho_Rectangle10;
            HObject ho_Rectangle20, ho_Rectangle30, ho_Rectangle40;
            HObject ho_RegionDifference0, ho_RegionDifference00, ho_ConnectedRegions10;
            HObject ho_ObjectSelected10, ho_ObjectSelected20, ho_RegionDifference10;
            HObject ho_ObjectSelected_Up, ho_ObjectSelected_Left, ho_RegionDifference20;
            HObject ho_ConnectedRegions20, ho_ObjectSelected_Right;
            HObject ho_ObjectSelected_Down, ho_RegionDifference30, ho_ConnectedRegions30;
            HObject ho_ObjectSelected30, ho_ObjectSelected40, ho_ObjectSelected_UpperLeft;
            HObject ho_ObjectSelected_UpperRight, ho_RegionDifference40;
            HObject ho_ConnectedRegions40, ho_ObjectSelected_LowerLeft;
            HObject ho_ObjectSelected_LowerRight;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Sector);
            HOperatorSet.GenEmptyObj(out ho_Circle11);
            HOperatorSet.GenEmptyObj(out ho_Circle12);
            HOperatorSet.GenEmptyObj(out ho_Rectangle10);
            HOperatorSet.GenEmptyObj(out ho_Rectangle20);
            HOperatorSet.GenEmptyObj(out ho_Rectangle30);
            HOperatorSet.GenEmptyObj(out ho_Rectangle40);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference0);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference00);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions10);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected10);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected20);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference10);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected_Up);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected_Left);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference20);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions20);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected_Right);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected_Down);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference30);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions30);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected30);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected40);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected_UpperLeft);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected_UpperRight);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference40);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions40);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected_LowerLeft);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected_LowerRight);
            try
            {

                //劃出方向檢測區域
                //內圈
                ho_Circle11.Dispose();
                HOperatorSet.GenCircle(out ho_Circle11, hv_Row, hv_Column, hv_InnerDiam);
                //外圈
                ho_Circle12.Dispose();
                HOperatorSet.GenCircle(out ho_Circle12, hv_Row, hv_Column, hv_TopDiam);
                //切割線1
                ho_Rectangle10.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle10, hv_Row, hv_Column, (new HTuple(45)).TupleRad()
                    , hv_TopDiam + 10, 1);
                //切割線2
                ho_Rectangle20.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle20, hv_Row, hv_Column, (new HTuple(-45)).TupleRad()
                    , hv_TopDiam + 10, 1);
                //切割線3
                ho_Rectangle30.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle30, hv_Row, hv_Column, (new HTuple(0)).TupleRad()
                    , hv_TopDiam + 10, 1);
                //切割線4
                ho_Rectangle40.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle40, hv_Row, hv_Column, (new HTuple(90)).TupleRad()
                    , hv_TopDiam + 10, 1);
                //相剪出扇形
                ho_RegionDifference0.Dispose();
                HOperatorSet.Difference(ho_Circle12, ho_Circle11, out ho_RegionDifference0);
                //方法一切成上下左右4邊
                //先切一半進行分出區域Up,Left,Right,Down
                ho_RegionDifference00.Dispose();
                HOperatorSet.Difference(ho_RegionDifference0, ho_Rectangle10, out ho_RegionDifference00
                    );
                ho_ConnectedRegions10.Dispose();
                HOperatorSet.Connection(ho_RegionDifference00, out ho_ConnectedRegions10);
                ho_ObjectSelected10.Dispose();
                HOperatorSet.SelectObj(ho_ConnectedRegions10, out ho_ObjectSelected10, 1);
                ho_ObjectSelected20.Dispose();
                HOperatorSet.SelectObj(ho_ConnectedRegions10, out ho_ObjectSelected20, 2);
                //上半部
                ho_RegionDifference10.Dispose();
                HOperatorSet.Difference(ho_ObjectSelected10, ho_Rectangle20, out ho_RegionDifference10
                    );
                ho_ConnectedRegions10.Dispose();
                HOperatorSet.Connection(ho_RegionDifference10, out ho_ConnectedRegions10);
                ho_ObjectSelected_Up.Dispose();
                HOperatorSet.SelectObj(ho_ConnectedRegions10, out ho_ObjectSelected_Up, 1);
                ho_ObjectSelected_Left.Dispose();
                HOperatorSet.SelectObj(ho_ConnectedRegions10, out ho_ObjectSelected_Left, 2);
                //下半部
                ho_RegionDifference20.Dispose();
                HOperatorSet.Difference(ho_ObjectSelected20, ho_Rectangle20, out ho_RegionDifference20
                    );
                ho_ConnectedRegions20.Dispose();
                HOperatorSet.Connection(ho_RegionDifference20, out ho_ConnectedRegions20);
                ho_ObjectSelected_Right.Dispose();
                HOperatorSet.SelectObj(ho_ConnectedRegions20, out ho_ObjectSelected_Right,
                    1);
                ho_ObjectSelected_Down.Dispose();
                HOperatorSet.SelectObj(ho_ConnectedRegions20, out ho_ObjectSelected_Down, 2);
                //方法二切成右上右下左上左下四邊
                //先切一半分出UpperRight,LowerRight,UpperLeft,LowerLeft
                ho_RegionDifference30.Dispose();
                HOperatorSet.Difference(ho_RegionDifference0, ho_Rectangle30, out ho_RegionDifference30
                    );
                ho_ConnectedRegions30.Dispose();
                HOperatorSet.Connection(ho_RegionDifference30, out ho_ConnectedRegions30);
                ho_ObjectSelected30.Dispose();
                HOperatorSet.SelectObj(ho_ConnectedRegions30, out ho_ObjectSelected30, 1);
                ho_ObjectSelected40.Dispose();
                HOperatorSet.SelectObj(ho_ConnectedRegions30, out ho_ObjectSelected40, 2);
                //上半部
                ho_RegionDifference30.Dispose();
                HOperatorSet.Difference(ho_ObjectSelected30, ho_Rectangle40, out ho_RegionDifference30
                    );
                ho_ConnectedRegions30.Dispose();
                HOperatorSet.Connection(ho_RegionDifference30, out ho_ConnectedRegions30);
                ho_ObjectSelected_UpperLeft.Dispose();
                HOperatorSet.SelectObj(ho_ConnectedRegions30, out ho_ObjectSelected_UpperLeft,
                    1);
                ho_ObjectSelected_UpperRight.Dispose();
                HOperatorSet.SelectObj(ho_ConnectedRegions30, out ho_ObjectSelected_UpperRight,
                    2);
                //下半部
                ho_RegionDifference40.Dispose();
                HOperatorSet.Difference(ho_ObjectSelected40, ho_Rectangle40, out ho_RegionDifference40
                    );
                ho_ConnectedRegions40.Dispose();
                HOperatorSet.Connection(ho_RegionDifference40, out ho_ConnectedRegions40);
                ho_ObjectSelected_LowerLeft.Dispose();
                HOperatorSet.SelectObj(ho_ConnectedRegions40, out ho_ObjectSelected_LowerLeft,
                    1);
                ho_ObjectSelected_LowerRight.Dispose();
                HOperatorSet.SelectObj(ho_ConnectedRegions40, out ho_ObjectSelected_LowerRight,
                    2);


                if ((int)(new HTuple(hv_Direction.TupleEqual("Up"))) != 0)
                {
                    ho_Sector.Dispose();
                    ho_Sector = ho_ObjectSelected_Up.CopyObj(1, -1);
                }
                else if ((int)(new HTuple(hv_Direction.TupleEqual("Left"))) != 0)
                {
                    ho_Sector.Dispose();
                    ho_Sector = ho_ObjectSelected_Left.CopyObj(1, -1);
                }
                else if ((int)(new HTuple(hv_Direction.TupleEqual("Right"))) != 0)
                {
                    ho_Sector.Dispose();
                    ho_Sector = ho_ObjectSelected_Right.CopyObj(1, -1);
                }
                else if ((int)(new HTuple(hv_Direction.TupleEqual("Down"))) != 0)
                {
                    ho_Sector.Dispose();
                    ho_Sector = ho_ObjectSelected_Down.CopyObj(1, -1);
                }
                else if ((int)(new HTuple(hv_Direction.TupleEqual("UpperLeft"))) != 0)
                {
                    ho_Sector.Dispose();
                    ho_Sector = ho_ObjectSelected_UpperLeft.CopyObj(1, -1);
                }
                else if ((int)(new HTuple(hv_Direction.TupleEqual("UpperRight"))) != 0)
                {
                    ho_Sector.Dispose();
                    ho_Sector = ho_ObjectSelected_UpperRight.CopyObj(1, -1);
                }
                else if ((int)(new HTuple(hv_Direction.TupleEqual("LowerLeft"))) != 0)
                {
                    ho_Sector.Dispose();
                    ho_Sector = ho_ObjectSelected_LowerLeft.CopyObj(1, -1);
                }
                else if ((int)(new HTuple(hv_Direction.TupleEqual("LowerRight"))) != 0)
                {
                    ho_Sector.Dispose();
                    ho_Sector = ho_ObjectSelected_LowerRight.CopyObj(1, -1);

                }


                ho_Circle11.Dispose();
                ho_Circle12.Dispose();
                ho_Rectangle10.Dispose();
                ho_Rectangle20.Dispose();
                ho_Rectangle30.Dispose();
                ho_Rectangle40.Dispose();
                ho_RegionDifference0.Dispose();
                ho_RegionDifference00.Dispose();
                ho_ConnectedRegions10.Dispose();
                ho_ObjectSelected10.Dispose();
                ho_ObjectSelected20.Dispose();
                ho_RegionDifference10.Dispose();
                ho_ObjectSelected_Up.Dispose();
                ho_ObjectSelected_Left.Dispose();
                ho_RegionDifference20.Dispose();
                ho_ConnectedRegions20.Dispose();
                ho_ObjectSelected_Right.Dispose();
                ho_ObjectSelected_Down.Dispose();
                ho_RegionDifference30.Dispose();
                ho_ConnectedRegions30.Dispose();
                ho_ObjectSelected30.Dispose();
                ho_ObjectSelected40.Dispose();
                ho_ObjectSelected_UpperLeft.Dispose();
                ho_ObjectSelected_UpperRight.Dispose();
                ho_RegionDifference40.Dispose();
                ho_ConnectedRegions40.Dispose();
                ho_ObjectSelected_LowerLeft.Dispose();
                ho_ObjectSelected_LowerRight.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Circle11.Dispose();
                ho_Circle12.Dispose();
                ho_Rectangle10.Dispose();
                ho_Rectangle20.Dispose();
                ho_Rectangle30.Dispose();
                ho_Rectangle40.Dispose();
                ho_RegionDifference0.Dispose();
                ho_RegionDifference00.Dispose();
                ho_ConnectedRegions10.Dispose();
                ho_ObjectSelected10.Dispose();
                ho_ObjectSelected20.Dispose();
                ho_RegionDifference10.Dispose();
                ho_ObjectSelected_Up.Dispose();
                ho_ObjectSelected_Left.Dispose();
                ho_RegionDifference20.Dispose();
                ho_ConnectedRegions20.Dispose();
                ho_ObjectSelected_Right.Dispose();
                ho_ObjectSelected_Down.Dispose();
                ho_RegionDifference30.Dispose();
                ho_ConnectedRegions30.Dispose();
                ho_ObjectSelected30.Dispose();
                ho_ObjectSelected40.Dispose();
                ho_ObjectSelected_UpperLeft.Dispose();
                ho_ObjectSelected_UpperRight.Dispose();
                ho_RegionDifference40.Dispose();
                ho_ConnectedRegions40.Dispose();
                ho_ObjectSelected_LowerLeft.Dispose();
                ho_ObjectSelected_LowerRight.Dispose();

                throw HDevExpDefaultException;
            }
        }
      public void find_target_in_four_circle (HObject ho_Image, out HObject ho_ObjectSelected_UpperLeft, 
      out HObject ho_ObjectSelected_UpperRight, out HObject ho_ObjectSelected_LowerLeft, 
      out HObject ho_ObjectSelected_LowerRight, HTuple hv_ModelID2, HTuple hv_ModelID3, 
      HTuple hv_Direction, out HTuple hv_Result, out HTuple hv_Score)
  {




    // Stack for temporary objects 
    HObject[] OTemp = new HObject[20];

    // Local iconic variables 

    HObject ho_ModelContours2, ho_ResultContours;
    HObject ho_Region_0, ho_ResultContours1, ho_Region_1, ho_ResultContours2;
    HObject ho_Region_2, ho_ResultContours3, ho_Region_3, ho_RegionUnion;
    HObject ho_ConnectedRegions2, ho_SortedRegions, ho_ObjectSelected_1;
    HObject ho_ObjectSelected_2, ho_RegionUnion_Upper, ho_ConnectedRegions_Upper;
    HObject ho_ObjectSelected_3, ho_ObjectSelected_4, ho_RegionUnion_Lower;
    HObject ho_ConnectedRegions_Lower, ho_SortedRegions_Upper;
    HObject ho_SortedRegions_Lower, ho_ImageReduced_UpperLeft;
    HObject ho_ImageReduced_UpperRight, ho_ImageReduced_LowerLeft;
    HObject ho_ImageReduced_LowerRight;

    // Local control variables 

    HTuple hv_Row_Model2 = null, hv_Column_Model2 = null;
    HTuple hv_Angle_Model2 = null, hv_Score_Model2 = null;
    HTuple hv_HomMat2D = null, hv_Row_1 = null, hv_Column_1 = null;
    HTuple hv_Angle_1 = null, hv_Score_UpperLeft = null, hv_Row_2 = null;
    HTuple hv_Column_2 = null, hv_Angle_2 = null, hv_Score_UpperRight = null;
    HTuple hv_Row_3 = null, hv_Column_3 = null, hv_Angle_3 = null;
    HTuple hv_Score_LowerLeft = null, hv_Row_4 = null, hv_Column_4 = null;
    HTuple hv_Angle_4 = null, hv_Score_LowerRight = null, hv_TargetScore = new HTuple();
    HTuple hv_OtherScore_1 = new HTuple(), hv_OtherScore_2 = new HTuple();
    HTuple hv_OtherScore_3 = new HTuple();
    HTuple   hv_Direction_COPY_INP_TMP = hv_Direction.Clone();

    // Initialize local and output iconic variables 
    HOperatorSet.GenEmptyObj(out ho_ObjectSelected_UpperLeft);
    HOperatorSet.GenEmptyObj(out ho_ObjectSelected_UpperRight);
    HOperatorSet.GenEmptyObj(out ho_ObjectSelected_LowerLeft);
    HOperatorSet.GenEmptyObj(out ho_ObjectSelected_LowerRight);
    HOperatorSet.GenEmptyObj(out ho_ModelContours2);
    HOperatorSet.GenEmptyObj(out ho_ResultContours);
    HOperatorSet.GenEmptyObj(out ho_Region_0);
    HOperatorSet.GenEmptyObj(out ho_ResultContours1);
    HOperatorSet.GenEmptyObj(out ho_Region_1);
    HOperatorSet.GenEmptyObj(out ho_ResultContours2);
    HOperatorSet.GenEmptyObj(out ho_Region_2);
    HOperatorSet.GenEmptyObj(out ho_ResultContours3);
    HOperatorSet.GenEmptyObj(out ho_Region_3);
    HOperatorSet.GenEmptyObj(out ho_RegionUnion);
    HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
    HOperatorSet.GenEmptyObj(out ho_SortedRegions);
    HOperatorSet.GenEmptyObj(out ho_ObjectSelected_1);
    HOperatorSet.GenEmptyObj(out ho_ObjectSelected_2);
    HOperatorSet.GenEmptyObj(out ho_RegionUnion_Upper);
    HOperatorSet.GenEmptyObj(out ho_ConnectedRegions_Upper);
    HOperatorSet.GenEmptyObj(out ho_ObjectSelected_3);
    HOperatorSet.GenEmptyObj(out ho_ObjectSelected_4);
    HOperatorSet.GenEmptyObj(out ho_RegionUnion_Lower);
    HOperatorSet.GenEmptyObj(out ho_ConnectedRegions_Lower);
    HOperatorSet.GenEmptyObj(out ho_SortedRegions_Upper);
    HOperatorSet.GenEmptyObj(out ho_SortedRegions_Lower);
    HOperatorSet.GenEmptyObj(out ho_ImageReduced_UpperLeft);
    HOperatorSet.GenEmptyObj(out ho_ImageReduced_UpperRight);
    HOperatorSet.GenEmptyObj(out ho_ImageReduced_LowerLeft);
    HOperatorSet.GenEmptyObj(out ho_ImageReduced_LowerRight);
    hv_Result = new HTuple();
    hv_Score = new HTuple();
    try
    {

      ho_ModelContours2.Dispose();
      HOperatorSet.GetShapeModelContours(out ho_ModelContours2, hv_ModelID2, 1);


      HOperatorSet.FindShapeModel(ho_Image, hv_ModelID2, (new HTuple(0)).TupleRad()
          , (new HTuple(360)).TupleRad(), 0.5, 4, 0.5, "least_squares", 0, 0.9, out hv_Row_Model2, 
          out hv_Column_Model2, out hv_Angle_Model2, out hv_Score_Model2);
      dev_display_shape_matching_results(hv_ModelID2, "red", hv_Row_Model2, hv_Column_Model2, 
          hv_Angle_Model2, 1, 1, 0);

      //顯示區域0
      HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
      HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle_Model2.TupleSelect(0), 0, 
          0, out hv_HomMat2D);
      HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row_Model2.TupleSelect(0), hv_Column_Model2.TupleSelect(
          0), out hv_HomMat2D);
      ho_ResultContours.Dispose();
      HOperatorSet.AffineTransContourXld(ho_ModelContours2, out ho_ResultContours, 
          hv_HomMat2D);
      ho_Region_0.Dispose();
      HOperatorSet.GenRegionContourXld(ho_ResultContours, out ho_Region_0, "filled");

      //顯示區域1
      HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
      HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle_Model2.TupleSelect(1), 0, 
          0, out hv_HomMat2D);
      HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row_Model2.TupleSelect(1), hv_Column_Model2.TupleSelect(
          1), out hv_HomMat2D);
      ho_ResultContours1.Dispose();
      HOperatorSet.AffineTransContourXld(ho_ModelContours2, out ho_ResultContours1, 
          hv_HomMat2D);
      ho_Region_1.Dispose();
      HOperatorSet.GenRegionContourXld(ho_ResultContours1, out ho_Region_1, "filled");


      //顯示區域2
      HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
      HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle_Model2.TupleSelect(2), 0, 
          0, out hv_HomMat2D);
      HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row_Model2.TupleSelect(2), hv_Column_Model2.TupleSelect(
          2), out hv_HomMat2D);
      ho_ResultContours2.Dispose();
      HOperatorSet.AffineTransContourXld(ho_ModelContours2, out ho_ResultContours2, 
          hv_HomMat2D);
      ho_Region_2.Dispose();
      HOperatorSet.GenRegionContourXld(ho_ResultContours2, out ho_Region_2, "filled");


      //顯示區域3
      HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
      HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle_Model2.TupleSelect(3), 0, 
          0, out hv_HomMat2D);
      HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row_Model2.TupleSelect(3), hv_Column_Model2.TupleSelect(
          3), out hv_HomMat2D);
      ho_ResultContours3.Dispose();
      HOperatorSet.AffineTransContourXld(ho_ModelContours2, out ho_ResultContours3, 
          hv_HomMat2D);
      ho_Region_3.Dispose();
      HOperatorSet.GenRegionContourXld(ho_ResultContours3, out ho_Region_3, "filled");


      ho_RegionUnion.Dispose();
      HOperatorSet.Union2(ho_Region_0, ho_Region_1, out ho_RegionUnion);
      {
      HObject ExpTmpOutVar_0;
      HOperatorSet.Union2(ho_RegionUnion, ho_Region_2, out ExpTmpOutVar_0);
      ho_RegionUnion.Dispose();
      ho_RegionUnion = ExpTmpOutVar_0;
      }
      {
      HObject ExpTmpOutVar_0;
      HOperatorSet.Union2(ho_RegionUnion, ho_Region_3, out ExpTmpOutVar_0);
      ho_RegionUnion.Dispose();
      ho_RegionUnion = ExpTmpOutVar_0;
      }
      ho_ConnectedRegions2.Dispose();
      HOperatorSet.Connection(ho_RegionUnion, out ho_ConnectedRegions2);

      //按照順序排列**********超好用
      ho_SortedRegions.Dispose();
      HOperatorSet.SortRegion(ho_ConnectedRegions2, out ho_SortedRegions, "upper_left", 
          "true", "row");
      //分出上面兩個
      ho_ObjectSelected_1.Dispose();
      HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected_1, 1);
      ho_ObjectSelected_2.Dispose();
      HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected_2, 2);
      ho_RegionUnion_Upper.Dispose();
      HOperatorSet.Union2(ho_ObjectSelected_1, ho_ObjectSelected_2, out ho_RegionUnion_Upper
          );
      ho_ConnectedRegions_Upper.Dispose();
      HOperatorSet.Connection(ho_RegionUnion_Upper, out ho_ConnectedRegions_Upper
          );
      //分出下面兩個
      ho_ObjectSelected_3.Dispose();
      HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected_3, 3);
      ho_ObjectSelected_4.Dispose();
      HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected_4, 4);
      ho_RegionUnion_Lower.Dispose();
      HOperatorSet.Union2(ho_ObjectSelected_3, ho_ObjectSelected_4, out ho_RegionUnion_Lower
          );
      ho_ConnectedRegions_Lower.Dispose();
      HOperatorSet.Connection(ho_RegionUnion_Lower, out ho_ConnectedRegions_Lower
          );
      //再分左右
      ho_SortedRegions_Upper.Dispose();
      HOperatorSet.SortRegion(ho_ConnectedRegions_Upper, out ho_SortedRegions_Upper, 
          "upper_left", "true", "column");
      ho_ObjectSelected_UpperLeft.Dispose();
      HOperatorSet.SelectObj(ho_SortedRegions_Upper, out ho_ObjectSelected_UpperLeft, 
          1);
      ho_ObjectSelected_UpperRight.Dispose();
      HOperatorSet.SelectObj(ho_SortedRegions_Upper, out ho_ObjectSelected_UpperRight, 
          2);

      ho_SortedRegions_Lower.Dispose();
      HOperatorSet.SortRegion(ho_ConnectedRegions_Lower, out ho_SortedRegions_Lower, 
          "upper_left", "true", "column");
      ho_ObjectSelected_LowerLeft.Dispose();
      HOperatorSet.SelectObj(ho_SortedRegions_Lower, out ho_ObjectSelected_LowerLeft, 
          1);
      ho_ObjectSelected_LowerRight.Dispose();
      HOperatorSet.SelectObj(ho_SortedRegions_Lower, out ho_ObjectSelected_LowerRight, 
          2);

      ho_ImageReduced_UpperLeft.Dispose();
      HOperatorSet.ReduceDomain(ho_Image, ho_ObjectSelected_UpperLeft, out ho_ImageReduced_UpperLeft
          );
      ho_ImageReduced_UpperRight.Dispose();
      HOperatorSet.ReduceDomain(ho_Image, ho_ObjectSelected_UpperRight, out ho_ImageReduced_UpperRight
          );
      ho_ImageReduced_LowerLeft.Dispose();
      HOperatorSet.ReduceDomain(ho_Image, ho_ObjectSelected_LowerLeft, out ho_ImageReduced_LowerLeft
          );
      ho_ImageReduced_LowerRight.Dispose();
      HOperatorSet.ReduceDomain(ho_Image, ho_ObjectSelected_LowerRight, out ho_ImageReduced_LowerRight
          );

      HOperatorSet.FindNccModel(ho_ImageReduced_UpperLeft, hv_ModelID3, (new HTuple(0)).TupleRad()
          , (new HTuple(360)).TupleRad(), 0.3, 1, 0.5, "true", 0, out hv_Row_1, out hv_Column_1, 
          out hv_Angle_1, out hv_Score_UpperLeft);
      HOperatorSet.FindNccModel(ho_ImageReduced_UpperRight, hv_ModelID3, (new HTuple(0)).TupleRad()
          , (new HTuple(360)).TupleRad(), 0.3, 1, 0.5, "true", 0, out hv_Row_2, out hv_Column_2, 
          out hv_Angle_2, out hv_Score_UpperRight);
      HOperatorSet.FindNccModel(ho_ImageReduced_LowerLeft, hv_ModelID3, (new HTuple(0)).TupleRad()
          , (new HTuple(360)).TupleRad(), 0.3, 1, 0.5, "true", 0, out hv_Row_3, out hv_Column_3, 
          out hv_Angle_3, out hv_Score_LowerLeft);
      HOperatorSet.FindNccModel(ho_ImageReduced_LowerRight, hv_ModelID3, (new HTuple(0)).TupleRad()
          , (new HTuple(360)).TupleRad(), 0.3, 1, 0.5, "true", 0, out hv_Row_4, out hv_Column_4, 
          out hv_Angle_4, out hv_Score_LowerRight);
      hv_Direction_COPY_INP_TMP = "UpperLeft";
      if ((int)(new HTuple(hv_Direction_COPY_INP_TMP.TupleEqual("UpperLeft"))) != 0)
      {
        hv_TargetScore = hv_Score_UpperLeft.Clone();
        hv_OtherScore_1 = hv_Score_UpperRight.Clone();
        hv_OtherScore_2 = hv_Score_LowerLeft.Clone();
        hv_OtherScore_3 = hv_Score_LowerRight.Clone();
      }
      else if ((int)(new HTuple(hv_Direction_COPY_INP_TMP.TupleEqual("UpperRight"))) != 0)
      {
        hv_TargetScore = hv_Score_UpperRight.Clone();
        hv_OtherScore_1 = hv_Score_UpperLeft.Clone();
        hv_OtherScore_2 = hv_Score_LowerLeft.Clone();
        hv_OtherScore_3 = hv_Score_LowerRight.Clone();
      }
      else if ((int)(new HTuple(hv_Direction_COPY_INP_TMP.TupleEqual("LowerLeft"))) != 0)
      {
        hv_TargetScore = hv_Score_LowerLeft.Clone();
        hv_OtherScore_1 = hv_Score_UpperLeft.Clone();
        hv_OtherScore_2 = hv_Score_UpperRight.Clone();
        hv_OtherScore_3 = hv_Score_LowerRight.Clone();
      }
      else if ((int)(new HTuple(hv_Direction_COPY_INP_TMP.TupleEqual("LowerRight"))) != 0)
      {
        hv_TargetScore = hv_Score_LowerRight.Clone();
        hv_OtherScore_1 = hv_Score_UpperLeft.Clone();
        hv_OtherScore_2 = hv_Score_LowerLeft.Clone();
        hv_OtherScore_3 = hv_Score_UpperRight.Clone();
      }


      if ((int)((new HTuple((new HTuple(hv_TargetScore.TupleGreater(hv_OtherScore_1))).TupleAnd(
          new HTuple(hv_TargetScore.TupleGreater(hv_OtherScore_2))))).TupleAnd(new HTuple(hv_TargetScore.TupleGreater(
          hv_OtherScore_3)))) != 0)
      {
        hv_Result = "OK";
        hv_Score = hv_TargetScore*100;
      }
      else
      {
        hv_Result = "NG2";
        hv_Score = 0;
      }

      ho_ModelContours2.Dispose();
      ho_ResultContours.Dispose();
      ho_Region_0.Dispose();
      ho_ResultContours1.Dispose();
      ho_Region_1.Dispose();
      ho_ResultContours2.Dispose();
      ho_Region_2.Dispose();
      ho_ResultContours3.Dispose();
      ho_Region_3.Dispose();
      ho_RegionUnion.Dispose();
      ho_ConnectedRegions2.Dispose();
      ho_SortedRegions.Dispose();
      ho_ObjectSelected_1.Dispose();
      ho_ObjectSelected_2.Dispose();
      ho_RegionUnion_Upper.Dispose();
      ho_ConnectedRegions_Upper.Dispose();
      ho_ObjectSelected_3.Dispose();
      ho_ObjectSelected_4.Dispose();
      ho_RegionUnion_Lower.Dispose();
      ho_ConnectedRegions_Lower.Dispose();
      ho_SortedRegions_Upper.Dispose();
      ho_SortedRegions_Lower.Dispose();
      ho_ImageReduced_UpperLeft.Dispose();
      ho_ImageReduced_UpperRight.Dispose();
      ho_ImageReduced_LowerLeft.Dispose();
      ho_ImageReduced_LowerRight.Dispose();

      return;
    }
    catch (HalconException HDevExpDefaultException)
    {
      ho_ModelContours2.Dispose();
      ho_ResultContours.Dispose();
      ho_Region_0.Dispose();
      ho_ResultContours1.Dispose();
      ho_Region_1.Dispose();
      ho_ResultContours2.Dispose();
      ho_Region_2.Dispose();
      ho_ResultContours3.Dispose();
      ho_Region_3.Dispose();
      ho_RegionUnion.Dispose();
      ho_ConnectedRegions2.Dispose();
      ho_SortedRegions.Dispose();
      ho_ObjectSelected_1.Dispose();
      ho_ObjectSelected_2.Dispose();
      ho_RegionUnion_Upper.Dispose();
      ho_ConnectedRegions_Upper.Dispose();
      ho_ObjectSelected_3.Dispose();
      ho_ObjectSelected_4.Dispose();
      ho_RegionUnion_Lower.Dispose();
      ho_ConnectedRegions_Lower.Dispose();
      ho_SortedRegions_Upper.Dispose();
      ho_SortedRegions_Lower.Dispose();
      ho_ImageReduced_UpperLeft.Dispose();
      ho_ImageReduced_UpperRight.Dispose();
      ho_ImageReduced_LowerLeft.Dispose();
      ho_ImageReduced_LowerRight.Dispose();

      throw HDevExpDefaultException;
    }
 
        }
#endregion
        #region Halcon參數2
        // Local iconic variables 

        HObject ho_Image, ho_Circle = null, ho_ReducedImage = null;
        HObject ho_Region = null, ho_Connection = null, ho_SelectedRegions0 = null;
        HObject ho_ModelContour = null, ho_MeasureContour = null, ho_Contour = null;
        HObject ho_CrossCenter = null, ho_Contours = null, ho_Cross = null;
        HObject ho_UsedEdges = null, ho_ResultContours = null, ho_Rectangle = null;
        HObject ho_ImageReduced00 = null, ho_ImageScaled = null, ho_Region00 = null;
        HObject ho_RegionClosing00 = null, ho_ConnectedRegions00 = null;
        HObject ho_SelectedRegions00 = null, ho_RegionDilation00 = null;
        HObject ho_Sector = null, ho_TransContours = null, ho_Circle1 = null;
        HObject ho_Circle2 = null, ho_RegionDifference = null, ho_ImageReduced = null;
        HObject ho_Edges = null,ho_RegionFillUp = null, ho_RegionClosing = null;
        HObject ho_ConnectedRegions = null, ho_SelectedRegions = null;
        HObject ho_RegionUnion = null, ho_RegionIntersection = null;
        HObject ho_Skeleton = null, ho_Contours1 = null, ho_UnionContours = null;
        HObject ho_RegionSelected = null, ho_CirclePF1=null;
        HObject ho_CirclePF2=null, ho_RegionDifferencePF=null, ho_ImageReducedPF=null;
        HObject ho_EdgesPF=null, ho_RegionFillUpPF=null, ho_RegionClosingPF=null;
        HObject ho_ConnectedRegionsPF=null, ho_SelectedRegionsPF=null;
        HObject ho_RegionUnionPF=null, ho_AllRegionXLDPF=null, ho_SelectRegionPF=null;
        HObject ho_SelectRegionPF1 = null, ho_SelectRegionPF2 = null;
        HObject ho_IgnoreRegionPF=null, ho_PolarTransImagePF=null;
        HObject ho_ImageMeanPF1=null, ho_ImageMeanPF2=null, ho_RegionDynThreshPF2=null;
        HObject ho_RegionDynThreshPF1=null, ho_RegionUnionPF1=null;
        HObject ho_XYTransRegionPF2=null, ho_RegionUnionPF2=null,ho_Circle3 = null, ho_Circle4 = null;
        HObject ho_RegionDifference3 = null, ho_RegionErosion = null;
        HObject ho_ImageReduced2 = null, ho_ImageSmooth2 = null, ho_Region2 = null;
        HObject ho_Region2_2 = null, ho_ImaAmp = null, ho_ImaDir = null;
        HObject ho_Region1 = null, ho_RegionUnion1 = null, ho_RegionClosing1 = null;
        HObject ho_RegionFillup2 = null, ho_ConnectedRegions1 = null;
        HObject ho_SelectedRegions1 = null, ho_Circle5 = null, ho_Circle6 = null;
        HObject ho_ImageReduced3 = null, ho_ImaAmp2 = null, ho_ImaDir2 = null;
        HObject ho_Skeleton1 = null, ho_RegionClosing3 = null, ho_RegionFillUp1 = null;
        HObject ho_ConnectedRegions3 = null, ho_SelectedRegions3 = null;
        HObject ho_RegionUnion3 = null, ho_Circle9 = null, ho_Circle10 = null;
        HObject ho_RegionDifference5 = null, ho_ImageReduced5 = null;
        HObject ho_ImaAmp3 = null, ho_ImaDir3 = null, ho_Region5 = null;
        HObject ho_Skeleton3 = null, ho_RegionFillUp2 = null, ho_ConnectedRegions4 = null;
        HObject ho_SelectedRegions5 = null, ho_RegionUnion5 = null;
        HObject ho_Circle7 = null, ho_Circle8 = null, ho_RegionDifference4 = null;
        HObject ho_ImageReduced4 = null, ho_Region4 = null, ho_SelectedRegions4 = null;
        HObject ho_RegionDifference4_2 = null, ho_RegionClosing4 = null;
        HObject ho_RegionFillUp4 = null, ho_RegionUnion4 = null, ho_RegionIntersection4 = null;
        HObject ho_Skeleton2 = null, ho_Contours2 = null, ho_UnionContours2 = null;
        HObject ho_RegionSelected_2 = null;
        HObject ho_RegionDifference_Model2 = null, ho_Circle_ModelTD2 = null, ho_Circle_ModelID2 = null;
        HObject ho_Contours_Model2 = null, ho_Region_Model2 = null, ho_ImageReduced_Model2 = null;
        HObject ho_RegionFillUp_Model2 = null,ho_ImageReduced_model3 = null,ho_PolygonRegion_model3 = null;
        HObject ho_RegionFillUp_model3 = null,ho_ObjectSelected_UpperLeft = null,ho_ObjectSelected_UpperRight = null;
        HObject ho_ObjectSelected_LowerLeft = null,ho_ObjectSelected_LowerRight = null;
        HObject ho_AllRegionXLD = null, ho_SelectRegion = null, ho_IgnoreRegion = null;
        HObject ho_PolarTransImage=null, ho_ImageMean1=null;
        HObject ho_ImageMean=null, ho_RegionDynThresh=null, ho_XYTransRegion=null;
        HObject ho_RegionFillUp5=null, ho_RegionClosing2=null, ho_RegionFillUp6=null;
        HObject ho_ConnectedRegions2=null, ho_SelectedRegions2=null;
        HObject ho_RegionUnion2 = null, ho_RegionIntersection2 = null;
        HObject ho_ImageMean2 = null, ho_ImageMean3 = null, ho_RegionDynThresh1 = null;
        HObject ho_RegionFillUp7 = null, ho_ConnectedRegions5 = null, ho_SelectedRegions6 = null;
        HObject ho_SelectedRegionsB = null, ho_SelectedRegionsA = null;
        HObject ho_ImageA = null, ho_ImageB = null;
        HObject ho_RectanglePF2 = null, ho_RegionInterSectionPF2 = null,ho_RegionInterSection_TestDefeat = new HObject();
        HObject ho_RectanglePF3 = null, ho_RegionInterSectionPF3 = null, ho_RegionUnionPF3 = null;
        // Local control variables 

        HTuple hv_Number1 = new HTuple(), hv_A = new HTuple(), hv_B = new HTuple(), hv_MinDistance = new HTuple(), hv_regionCount = new HTuple();
        HTuple  hv_MeanWidth_1 = new HTuple(),hv_MeanHeight_1 = new HTuple(), hv_MeanWidth_2 = new HTuple();
        HTuple hv_MeanHeight_2 = new HTuple(),hv_closeWidthValue = new HTuple(), hv_closeHeightValue = new HTuple();
        HTuple hv_OpenHeightValue = new HTuple(),hv_OpenWidthValue = new HTuple(), hv_filterHeight = new HTuple();
        HTuple hv_Height_Upper = new HTuple(), hv_Height_Lower = new HTuple();
        HTuple hv_filterWidth = new HTuple(), hv_Width_Upper = new HTuple(),hv_Width_Lower = new HTuple();

        HTuple hv_ImageFiles = null, hv_i = null, hv_WindowHandle = new HTuple();
        HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
        HTuple hv_FirstRadius = new HTuple(), hv_CreateDirection = new HTuple();
        HTuple hv_TestPlatform = new HTuple(), hv_TestGap = new HTuple();
        HTuple hv_TestCaliber = new HTuple(), hv_TestSlope = new HTuple();
        HTuple hv_TestInside = new HTuple(), hv_Area5Length = new HTuple();
        HTuple hv_Area6Length = new HTuple(), hv_ResultAngle_2 = new HTuple();
        HTuple hv_ResultRow = new HTuple(), hv_ResultColumn = new HTuple();
        HTuple hv_Area8Length = new HTuple(), hv_ResultAnglePF = new HTuple();
        HTuple hv_CenterRadius = new HTuple(), hv_Radious0 = new HTuple();
        HTuple hv_GraySetting = new HTuple(), hv_Area0 = new HTuple();
        HTuple hv_Row0 = new HTuple(), hv_Column0 = new HTuple();
        HTuple hv_Area1 = new HTuple(), hv_Row1 = new HTuple();
        HTuple hv_Column1 = new HTuple(), hv_MetrologyHandle = new HTuple();
        HTuple hv_circleIndices = new HTuple(), hv_Row = new HTuple();
        HTuple hv_Column = new HTuple(), hv_GenParamValue = new HTuple();
        HTuple hv_Length1 = new HTuple(), hv_Width1 = new HTuple();
        HTuple hv_Measure_Threshold = new HTuple(), hv_circleParameter = new HTuple();
        HTuple hv_UsedRow = new HTuple(), hv_UsedColumn = new HTuple();
        HTuple hv_ResultRadius = new HTuple(), hv_StartPhi = new HTuple();
        HTuple hv_EndPhi = new HTuple(), hv_PointOrder = new HTuple();
        HTuple hv_Exception = new HTuple(), hv_gray00 = new HTuple();
        HTuple hv_Row10 = new HTuple(), hv_Column10 = new HTuple();
        HTuple hv_Phi10 = new HTuple(), hv_Length10 = new HTuple();
        HTuple hv_Length20 = new HTuple(), hv_GMax = new HTuple();
        HTuple hv_GMin = new HTuple(), hv_Mult = new HTuple();
        HTuple hv_add = new HTuple(), hv_Area00 = new HTuple();
        HTuple hv_Row00 = new HTuple(), hv_Column00 = new HTuple();
        HTuple hv_ModelID00 = new HTuple(), hv_ModelRegionArea = new HTuple();
        HTuple hv_RefRow = new HTuple(), hv_RefColumn = new HTuple();
        HTuple hv_ResultRadius10 = new HTuple(), hv_ResultRadius11 = new HTuple();
        HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
        HTuple hv_CircleRadius1 = new HTuple(), hv_CircleRadius2 = new HTuple();
        HTuple hv_Grayval = new HTuple(), hv_grayBlack = new HTuple();
        HTuple hv_grayWhite = new HTuple(), hv_UnderSizeArea = new HTuple();
        HTuple hv_Area = new HTuple(), hv_Row2 = new HTuple();
        HTuple hv_Column2 = new HTuple(), hv_Number = new HTuple();
        HTuple hv_Area4 = new HTuple(), hv_Row4 = new HTuple();
        HTuple hv_Column4 = new HTuple(), hv_ResultAngle = new HTuple();
        HTuple hv_RowShortBegin = new HTuple(), hv_ColShortBegin = new HTuple();
        HTuple hv_RowShortEnd = new HTuple(), hv_ColShortEnd = new HTuple();
        HTuple hv_AngleShort = new HTuple(), hv_Row3 = new HTuple();
        HTuple hv_Col3 = new HTuple(), hv_Row3Length = new HTuple();
        HTuple hv_RowBegin = new HTuple(), hv_ColBegin = new HTuple();
        HTuple hv_RowEnd = new HTuple(), hv_ColEnd = new HTuple();
        HTuple hv_RowMiddle = new HTuple(), hv_ColMiddle = new HTuple();
        HTuple hv_Angle2 = new HTuple(), hv_Angle3 = new HTuple();
        HTuple hv_AngleA = new HTuple(), hv_CircleRadiusPF1 = new HTuple();
        HTuple hv_CircleRadiusPF2 = new HTuple(), hv_grayBlackPF = new HTuple();
        HTuple hv_grayWhitePF = new HTuple(), hv_UnderSizeAreaPF = new HTuple();
        HTuple hv_AreaPF = new HTuple(), hv_RowPF2 = new HTuple();
        HTuple hv_ColumnPF2 = new HTuple(), hv_NumberPF = new HTuple();
        HTuple hv_AreaPF4 = new HTuple(), hv_RowPF4 = new HTuple();
        HTuple hv_ColumnPF4 = new HTuple(), hv_RowShortBeginPF = new HTuple();
        HTuple hv_ColShortBeginPF = new HTuple(), hv_RowShortEndPF = new HTuple();
        HTuple hv_ColShortEndPF = new HTuple(), hv_AngleShortPF = new HTuple();
        HTuple hv_RowPF3 = new HTuple(), hv_ColPF3 = new HTuple();
        HTuple hv_RowPF3Length = new HTuple(), hv_RowBeginPF = new HTuple();
        HTuple hv_ColBeginPF = new HTuple(), hv_RowEndPF = new HTuple();
        HTuple hv_ColEndPF = new HTuple(), hv_RowMiddlePF = new HTuple();
        HTuple hv_ColMiddlePF = new HTuple(), hv_AnglePF = new HTuple();
        HTuple hv_AnglePF2 = new HTuple(), hv_AnglePF3 = new HTuple();
        HTuple hv_AngleAPF = new HTuple(), hv_CircleRadius3 = new HTuple();
        HTuple hv_CircleRadius4 = new HTuple(), hv_Area5 = new HTuple();
        HTuple hv_Mean = new HTuple(), hv_Deviation = new HTuple();
        HTuple hv_UnderSizeArea2 = new HTuple(), hv_Row5 = new HTuple();
        HTuple hv_Column5 = new HTuple(), hv_Area6 = new HTuple();
        HTuple hv_Row6 = new HTuple(), hv_Column6 = new HTuple();
        HTuple hv_Area8 = new HTuple(), hv_Row8 = new HTuple();
        HTuple hv_Column8 = new HTuple(), hv_Area7 = new HTuple();
        HTuple hv_Row7 = new HTuple(), hv_Column7 = new HTuple();
        HTuple hv_Number2 = new HTuple(), hv_RowShortBegin_2 = new HTuple();
        HTuple hv_ColShortBegin_2 = new HTuple(), hv_RowShortEnd_2 = new HTuple();
        HTuple hv_ColShortEnd_2 = new HTuple(), hv_AngleShort_2 = new HTuple();
        HTuple hv_Row3_2 = new HTuple(), hv_Col3_2 = new HTuple();
        HTuple hv_Row3Length_2 = new HTuple(), hv_RowBegin_2 = new HTuple();
        HTuple hv_ColBegin_2 = new HTuple(), hv_RowEnd_2 = new HTuple();
        HTuple hv_ColEnd_2 = new HTuple(), hv_RowMiddle_2 = new HTuple();
        HTuple hv_ColMiddle_2 = new HTuple(), hv_Angle_2 = new HTuple();
        HTuple hv_Angle2_2 = new HTuple(), hv_Angle3_2 = new HTuple();
        HTuple hv_AngleA_2 = new HTuple(), hv_AngleSet = new HTuple();
        HTuple hv_AngleSet2 = new HTuple(), hv_AngleMaximumSet = new HTuple();
        HTuple hv_AngleSetPF = new HTuple(), hv_TotalAngle = new HTuple(), hv_LackAngle_Max = new HTuple();
        HTuple hv_TotalAnglePF = new HTuple(),hv_LackAngle_Max_PF = new HTuple();

        HTuple hv_Row_ModelTD2 = new HTuple(), hv_Row_ModelID2 = new HTuple();
        HTuple hv_Column_ModelTD2 = new HTuple(), hv_Column_ModelID2 = new HTuple();
        HTuple hv_Radius_ModelTD2 = new HTuple(), hv_Radius_ModelID2 = new HTuple();
        HTuple hv_Radius_Model2 = new HTuple(), hv_StartPhi2 = new HTuple();
        HTuple hv_EndPhi2 = new HTuple(), hv_PointOrder2 = new HTuple();
        HTuple hv_Row_Model2 = new HTuple(), hv_Column_Model2 = new HTuple();
        HTuple hv_ModelID2 = new HTuple(),hv_ModelID3 = new HTuple();
        HTuple hv_Result = new HTuple(), hv_GlueLightDarkChoice = new HTuple();

        HTuple hv_contrast_Set = new HTuple(), hv_Contrast_Value = new HTuple();
        HTuple hv_Area_Upper = new HTuple(), hv_Area_Lower = new HTuple();
        HTuple hv_Roundness_Upper = new HTuple(), hv_Roundness_Lower = new HTuple();
        HTuple hv_Rectangularity_Upper = new HTuple(), hv_Rectangularity_Lower = new HTuple();

#endregion

        private void FrmRun_Load(object sender, EventArgs e)
        {
            bReadingPara = true;
            ReadPara();
            bReadingPara = false;
            LoadSettingLight();
            parent.LightOn_All();
            TimerUI.Enabled = true;
        }

        public void ReadPara()
        {
            #region HOperatorSet.GenEmptyObj
            HOperatorSet.GenEmptyObj(out ho_SelectedRegionsA);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegionsB);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp7);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions5);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions6);
            HOperatorSet.GenEmptyObj(out ho_ImageMean2);
            HOperatorSet.GenEmptyObj(out ho_ImageMean3);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThresh1);
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
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected_UpperLeft);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected_UpperRight);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected_LowerLeft);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected_LowerRight);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced00);
            HOperatorSet.GenEmptyObj(out ho_TransContours);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_Cross);
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
            HOperatorSet.GenEmptyObj(out ho_Circle1);
            HOperatorSet.GenEmptyObj(out ho_Circle2);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Edges);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            HOperatorSet.GenEmptyObj(out ho_PolarTransImage);
            HOperatorSet.GenEmptyObj(out ho_ImageMean1);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThresh);
            HOperatorSet.GenEmptyObj(out ho_XYTransRegion);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp5);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion2);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection2);
            HOperatorSet.GenEmptyObj(out ho_CirclePF1);
            HOperatorSet.GenEmptyObj(out ho_CirclePF2);
            HOperatorSet.GenEmptyObj(out ho_RegionDifferencePF);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedPF);
            HOperatorSet.GenEmptyObj(out ho_EdgesPF);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUpPF);
            HOperatorSet.GenEmptyObj(out ho_RegionClosingPF);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegionsPF);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegionsPF);
            HOperatorSet.GenEmptyObj(out ho_RegionUnionPF);
            HOperatorSet.GenEmptyObj(out ho_AllRegionXLDPF);
            HOperatorSet.GenEmptyObj(out ho_SelectRegionPF);
            HOperatorSet.GenEmptyObj(out ho_IgnoreRegionPF);
            HOperatorSet.GenEmptyObj(out ho_PolarTransImagePF);
            HOperatorSet.GenEmptyObj(out ho_ImageMeanPF1);
            HOperatorSet.GenEmptyObj(out ho_ImageMeanPF2);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThreshPF2);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThreshPF1);
            HOperatorSet.GenEmptyObj(out ho_RegionUnionPF1);
            HOperatorSet.GenEmptyObj(out ho_XYTransRegionPF2);
            HOperatorSet.GenEmptyObj(out ho_RegionUnionPF2);
            HOperatorSet.GenEmptyObj(out ho_RectanglePF2);
            HOperatorSet.GenEmptyObj(out ho_RegionInterSectionPF2);

            HOperatorSet.GenEmptyObj(out ho_RectanglePF3); 
            HOperatorSet.GenEmptyObj(out ho_RegionInterSectionPF3);
            HOperatorSet.GenEmptyObj(out ho_RegionUnionPF3);
            HOperatorSet.GenEmptyObj(out ho_SelectRegionPF1);
            HOperatorSet.GenEmptyObj(out ho_SelectRegionPF2);

            HOperatorSet.GenEmptyObj(out ho_Circle7);
            HOperatorSet.GenEmptyObj(out ho_Circle8);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference4);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced4);
            HOperatorSet.GenEmptyObj(out ho_Region4);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions4);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions4);

            HOperatorSet.GenEmptyObj(out ho_AllRegionXLD);
            HOperatorSet.GenEmptyObj(out ho_SelectRegion);
            HOperatorSet.GenEmptyObj(out ho_IgnoreRegion);
            #endregion
            cbTestFixedCollar.Checked = Convert.ToBoolean(My.TestFixedCollar);
            cbTestPlatform.Checked = Convert.ToBoolean(My.TestPlatform);
            cbTestDirection.Checked = Convert.ToBoolean(My.TestDirection);
            cbDetection_Black.Checked = Convert.ToBoolean(My.Detection_Black);
            cbDetection_White.Checked = Convert.ToBoolean(My.Detection_White);

            dFirstCircleRadius = My.dFirstCircleRadius;
            tbDetectionRadius.Value = (int)My.dReduceRadius;
            tbGraythreshold.Value = (int)My.dGraythreshold;
            dCenterRadius = My.dCenterRadius;
            tbLength.Value = (int)My.dLength;

            if (My.sGenParamValue == "positive")
            {
                tbBlackToWhite.Value = (int)My.dMeasureThreshold;
            }
            else
            {
                tbWhiteToBlack.Value = (int)My.dMeasureThreshold;
            }
            cbTestNoGlue.Checked = My.TestNoGlue;
            ucOuterRadius_TestNoGlue.Value = My.OuterRadius_TestNoGlue;
            ucInnerRadius_TestNoGlue.Value = My.InnerRadius_TestNoGlue;
            cbDetection_Black_TestNoGlue.Checked = My.Detection_Black_TestNoGlue;
            cbDetection_White_TestNoGlue.Checked = My.Detection_White_TestNoGlue;
            ucGraythresholdBlack_TestNoGlue.Value = My.GraythresholdBlack_TestNoGlue;
            ucGraythresholdWhite_TestNoGlue.Value = My.GraythresholdWhite_TestNoGlue;
            ucUnderSizeArea_TestNoGlue.Value = My.UnderSizeArea_TestNoGlue;
            ucAreaUpper_TestNoGlue.Value = My.AreaUpper_TestNoGlue;
            ucAreaLower_TestNoGlue.Value = My.AreaLower_TestNoGlue;


            tbRingOutRange.Value = (int)My.dRingOutRange;
            tbRingInRange.Value = (int)My.dRingInRange;
            tbGraythresholdBlack.Value = (int)My.dGraythresholdBlack;
            tbGraythresholdWhite.Value = (int)My.dGraythresholdWhite;
            tbUnderSizeArea.Value = (int)My.dUnderSizeArea;
            nudGlueAngleSet.Value = (int)My.iGlueAngleSet;
            nudGlueRatioSet.Value = (int)My.iGlueRatioSet;
            nudAngleSet.Value = (int)My.dAngleSet;
            nudLackMaxAngleSet.Value = (int)My.dLackMaxAngleSet;
            cbMethodChoice.SelectedIndex = My.MethodChoice;

            cbClosing.Checked = My.Closing ? true : false;
            cbOpening.Checked = My.Opening ? true : false;
            cbFilterWidth.Checked = My.FilterWidth ? true : false;
            cbFilterHeight.Checked = My.FilterHeight ? true : false;

            cbGlueLightDarkChoice.SelectedIndex = My.GlueLightDarkChoice;
            nudDynThresholdSet.Value = My.dDynThresholdSet;
            nudUnderSizeArea2.Value = My.iUnderSizeArea2;
            nudMeanWidth_1.Value = (decimal)My.dMeanWidth_1;
            nudMeanHeight_1.Value = (decimal)My.dMeanHeight_1;
            nudMeanWidth_2.Value = (decimal)My.dMeanWidth_2;
            nudMeanHeight_2.Value = (decimal)My.dMeanHeight_2;
            nudCloseWidth.Value = (decimal)My.dCloseWidthValue;
            nudCloseHeight.Value = (decimal)My.dCloseHeightValue;
            nudOpenWidth.Value = (decimal)My.dOpenWidthValue;
            nudOpenHeight.Value = (decimal)My.dOpenHeightValue;
            nudFilterWidth_Lower.Value = (decimal)My.dFilterWidth_Lower;
            nudFilterWidth_Upper.Value = (decimal)My.dFilterWidth_Upper;
            nudFilterHeight_Lower.Value = (decimal)My.dFilterHeight_Lower;
            nudFilterHeight_Upper.Value = (decimal)My.dFilterHeight_Upper;

            cbDecisionMethodChoice.SelectedIndex = My.DecisionMethodChoice;

            tbPFOutRange.Value = (int)My.dOutRangePF;
            tbPFInRange.Value = (int)My.dInRangePF;

            nudGlueAngleSetPF.Value = (int)My.iGlueAngleSetPF;
            nudGlueRatioSetPF.Value = (int)My.iGlueRatioSetPF;

            cbMethodChoice2.SelectedIndex = My.MethodChoice2;
            cbDetectionPF_Dark2.Checked = My.DetectionPF_Dark2;
            cbDetectionPF_Light2.Checked = My.DetectionPF_Light2;
            cbDetectionPF_Black2.Checked = My.DetectionPF_Black;
            cbDetectionPF_White2.Checked = My.DetectionPF_White;
            nudDynthresholdDarkPF2.Value = My.iDynthresholdDarkPF2;
            nudDynthresholdLightPF2.Value = My.iDynthresholdLightPF2;
            nudGraythresholdBlackPF2.Value = My.iGraythresholdBlackPF2;
            nudGraythresholdWhitePF2.Value = My.iGraythresholdWhitePF2;
            cbClosingPF2.Checked = My.ClosingPF2;
            cbOpeningPF2.Checked = My.OpeningPF2;
            nudCloseWidthPF2.Value = My.iCloseWidthPF2;
            nudCloseHeightPF2.Value = My.iCloseHeightPF2;
            nudOpenWidthPF2.Value = My.iOpenWidthPF2;
            nudOpenHeightPF2.Value = My.iOpenHeightPF2;
            nudUnderSizeAreaPF2.Value = My.iUnderSizeAreaPF2;
            ucGraythresholdBlackPF3.Value = My.dGraythresholdBlackPF3;
            ucGraythresholdWhitePF3.Value = My.dGraythresholdWhitePF3;
            ucPF2InRange.Value = My.dInRangePF2;
            ucPF2OutRange.Value = My.dOutRangePF2;
            cbDetectionPF2_Black2.Checked = My.DetectionPF2_Black;
            cbDetectionPF2_White2.Checked = My.DetectionPF2_White;

            nudAngleSetPF.Value = (int)My.dAngleSetPF;
            nudLackMaxAngleSetPF.Value = (int)My.dLackMaxAngleSetPF;
            dAngleSet2 = (double)My.dAngleSet2;

            nudGlueCountSet.Value = (decimal)My.iGlueCount;
            nudRegionDistance.Value = (decimal)My.dRegionDistance;

            tbMarkGraythreshold.Value = (int)My.dMarkGraythreshold;
            nudMarkGrade.Value = (int)My.dMarkGrade;

            tbMarkID.Value = (int)My.dMarkID;
            tbMarkTD.Value = (int)My.dMarkTD;
            switch (My.iTestQuadrant)
            {
                case 1: cbTestQuadrant1.Checked = true; break;
                case 2: cbTestQuadrant2.Checked = true; break;
                case 3: cbTestQuadrant3.Checked = true; break;
                case 4: cbTestQuadrant4.Checked = true; break;
                case 5: cbTestQuadrant5.Checked = true; break;
                case 6: cbTestQuadrant6.Checked = true; break;
                case 7: cbTestQuadrant7.Checked = true; break;
                case 8: cbTestQuadrant8.Checked = true; break;
            }
            cbChoiceMax.Checked = My.bChoiceMax;
            cbTestDirectionChoice.SelectedIndex = My.TestDirectionChoice;
            tbModuleGraythreshold_2.Value = (int)dModuleGraythreshold_2;
            nudModuleGrade_2.Value = (decimal)dModuleGrade_2;
            nudModuleGrade_3.Value = (decimal)dModuleGrade_3;
            cbDarkLightChoice.SelectedIndex = My.DarkLightChoice;
            cbDetectionArea.Checked = My.DetectionArea;
            cbDetectionRect2_Len1.Checked = My.DetectionRect2_Len1;
            cbDetectionRect2_Len2.Checked = My.DetectionRect2_Len2;
            cbDetectionRoundness.Checked = My.DetectionRoundness;
            cbDetectionRectangularity.Checked = My.DetectionRectangularity;
            cbContrastSet.SelectedIndex = My.ContrastSet;
            tbArea_Upper.Value = My.Area_Upper;
            tbArea_Lower.Value = My.Area_Lower;
            ucGraySet.Value = My.GraySet;
            ucRect2_Len1_Lower.Value = My.Rect2_Len1_Lower;
            ucRect2_Len1_Upper.Value = My.Rect2_Len1_Upper;
            ucRect2_Len2_Lower.Value = My.Rect2_Len2_Lower;
            ucRect2_Len2_Upper.Value = My.Rect2_Len2_Upper;

            tbRoundness_Upper.Value = My.Roundness_Upper;
            tbRoundness_Lower.Value = My.Roundness_Lower;
            tbRectangularity_Upper.Value = My.Rectangularity_Upper;
            tbRectangularity_Lower.Value = My.Rectangularity_Lower;

            //缺陷檢測
            cbTestDefeat.Checked = My.TestDefeat;
            cbDetection_Dark_TestDefeat.Checked = My.Detection_Dark_TestDefeat;
            cbDetection_Light_TestDefeat.Checked = My.Detection_Light_TestDefeat;
            cbDetection_Black_TestDefeat.Checked = My.Detection_Black_TestDefeat;
            cbDetection_White_TestDefeat.Checked = My.Detection_White_TestDefeat;

            ucOuterRadius_TestDefeat.Value = My.OuterRadius_TestDefeat;
            ucInnerRadius_TestDefeat.Value = My.InnerRadius_TestDefeat;
            nudDynthresholdDark_TestDefeat.Value = My.DynthresholdDark_TestDefeat;
            nudDynthresholdLight_TestDefeat.Value = My.DynthresholdLight_TestDefeat;
            ucGraythresholdBlack_TestDefeat.Value = My.GraythresholdBlack_TestDefeat;
            ucGraythresholdWhite_TestDefeat.Value = My.GraythresholdWhite_TestDefeat;

            nudCloseWidth_TestDefeat.Value = My.CloseWidth_TestDefeat;
            nudCloseHeight_TestDefeat.Value = My.CloseHeight_TestDefeat;
            nudOpenWidth_TestDefeat.Value = My.OpenWidth_TestDefeat;
            nudOpenHeight_TestDefeat.Value = My.OpenHeight_TestDefeat;
            ucUnderSizeArea_TestDefeat.Value = My.UnderSizeArea_TestDefeat;
            ucDetectionArea_TestDefeat.Value = My.DetectionArea_TestDefeat;
            //讀模組
            Read_Temple();

            tbGain.Value = (int)CCD.Gain;
            tbExposureTime.Value = (int)CCD.ExposureTime;
            if (CCD.IsConnected)
            {
                lblGainMax.Text = CCD.GainMaximum.ToString();
                lblGainMin.Text = CCD.GainMinimum.ToString();
                lblExposureTimeMax.Text = CCD.ExposureTimeMaximum.ToString();
                lblExposureTimeMin.Text = CCD.ExposureTimeMinimum.ToString();
            }
        }

        public void LoadSettingLight()
        {
            nudLightSet_1.Value = Convert.ToInt32(Light.LightSet_1);
            nudLightSet_2.Value = Convert.ToInt32(Light.LightSet_2);
            nudLightSet_3.Value = Convert.ToInt32(Light.LightSet_3);
            nudLightSet_4.Value = Convert.ToInt32(Light.LightSet_4);
        }

        private void tbGain_ValueChanged(object sender, EventArgs e)
        {
            nudGain.Value = tbGain.Value;
        }

        private void nudGain_ValueChanged(object sender, EventArgs e)
        {
            tbGain.Value = Convert.ToInt32(nudGain.Value);
            CCD.Gain = (double)nudGain.Value;
            parent.SetGain(tbGain.Value);
        }

        private void tbExposureTime_ValueChanged(object sender, EventArgs e)
        {
            nudExposureTime.Value = tbExposureTime.Value;
        }

        private void nudExposureTime_ValueChanged(object sender, EventArgs e)
        {
            tbExposureTime.Value = Convert.ToInt32(nudExposureTime.Value);
            
            parent.SetExposureTime(tbExposureTime.Value);
           
        }
        public void CCDSetPara()
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            CCD.Gain = double.Parse(IniFile.Read("Setting", "Gain", "0",Path));
            CCD.ExposureTime = double.Parse(IniFile.Read("Setting", "ExposureTime", "35000",Path));
        }
        private void btnCCDSetSave_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            CCD.Gain = (double)nudGain.Value;
            CCD.ExposureTime = (double)nudExposureTime.Value;
            IniFile.Write("Setting", "Gain", CCD.Gain.ToString(),Path);
            IniFile.Write("Setting", "ExposureTime", CCD.ExposureTime.ToString(),Path);
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
            //tbDetectionRadius.Enabled = true;
            //nudDetectionRadius.Enabled = true;
            //tbGraythreshold.Enabled = true;
            //nudGraythreshold.Enabled = true;
            //tbLength.Enabled = true;
            //nudLength.Enabled = true;
            //tbBlackToWhite.Enabled = true;
            //nudBlackToWhite.Enabled = true;
            //tbWhiteToBlack.Enabled = true;
            //nudWhiteToBlack.Enabled = true;
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
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
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
            dGraythreshold =  tbGraythreshold.Value = Convert.ToInt32(nudGraythreshold.Value);
            try
            {
                HOperatorSet.GenEmptyObj(out ho_Region);
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
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
                ho_Region.Dispose();
                if (My.DarkLightChoice == 0)
                    HOperatorSet.Threshold(ho_Image, out ho_Region, tbGraythreshold.Value, 255);
                else
                    HOperatorSet.Threshold(ho_Image, out ho_Region, 0 ,tbGraythreshold.Value);
                
                HOperatorSet.DispObj(ho_Region, hv_ExpDefaultWinHandle);
            }
            catch
            {
                //MessageBox.Show("error");
            }
            Thread.Sleep(1);
        }


        private void btnCircleCenter_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image);

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

        public void CatchCenter(HWindow Window, HObject theImage)
        {
            try
            {
                HOperatorSet.CopyImage(theImage,out ho_Image);
                hv_ExpDefaultWinHandle = Window;
                //畫檢視範圍
                Window.ClearWindow();
                HOperatorSet.SetColor(Window, "green");
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                hv_GraySetting = dGraythreshold;

                ho_Region.Dispose();
                if (My.DarkLightChoice == 0)
                    HOperatorSet.Threshold(ho_ReducedImage, out ho_Region, hv_GraySetting, 255);
                else
                    HOperatorSet.Threshold(ho_ReducedImage, out ho_Region, 0, hv_GraySetting);
                ho_Connection.Dispose();
                HOperatorSet.Connection(ho_Region, out ho_Connection);
                HOperatorSet.FillUp(ho_Connection, out ho_Connection);
                hv_FirstRadius = dFirstCircleRadius;
                hv_CenterRadius = dCenterRadius;
                ho_SelectedRegions0.Dispose();
                HOperatorSet.SelectShape(ho_Connection, out ho_SelectedRegions0, (new HTuple("outer_radius")).TupleConcat(
                    "roundness"), "and", (new HTuple(0)).TupleConcat(0.8), ((hv_FirstRadius + 50)).TupleConcat(
                    1));
                HOperatorSet.AreaCenter(ho_SelectedRegions0, out hv_Area0, out hv_Row0, out hv_Column0);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions0, out ExpTmpOutVar_0, "area", "and",
                        hv_Area0.TupleMax(), hv_Area0.TupleMax()+1);
                    ho_SelectedRegions0.Dispose();
                    ho_SelectedRegions0 = ExpTmpOutVar_0;
                }
                HOperatorSet.AreaCenter(ho_SelectedRegions0, out hv_Area1, out hv_Row1, out hv_Column1);
                //找出圓心
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.AddMetrologyObjectGeneric(hv_MetrologyHandle, "circle", ((hv_Row1.TupleConcat(
                    hv_Column1))).TupleConcat(hv_CenterRadius), 25, 5, 1, 30, new HTuple(),
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
                hv_Width1 = 5;
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
                HOperatorSet.SetColor(Window, "blue");
                HOperatorSet.DispObj(ho_UsedEdges, hv_ExpDefaultWinHandle);
                ho_ResultContours.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_ResultContours, hv_MetrologyHandle,
                    "all", "all", 1.5);
                HOperatorSet.DispObj(ho_ResultContours, hv_ExpDefaultWinHandle);
                HOperatorSet.FitCircleContourXld(ho_ResultContours, "algebraic", -1, 0, 0,
                    3, 2, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius, out hv_StartPhi,
                    out hv_EndPhi, out hv_PointOrder);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                My.dResultRow = hv_ResultRow;
                My.dResultColumn = hv_ResultColumn;
            }
            catch
            {
            }
            finally
            {
                ho_Image.Dispose();
                ho_Circle.Dispose();
                ho_ReducedImage.Dispose();
                ho_Region.Dispose();
                ho_Connection.Dispose();
                ho_ModelContour.Dispose();
                ho_MeasureContour.Dispose();
                ho_Contour.Dispose();
                ho_CrossCenter.Dispose();
                ho_Contours.Dispose();
                ho_Contour.Dispose();
                ho_Cross.Dispose();
                ho_UsedEdges.Dispose();
                ho_ResultContours.Dispose();
            }
        }

        public void FixationRing(HWindow Window,HTuple CircleRadius1,HTuple CircleRadius2)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle1);
            HOperatorSet.GenEmptyObj(out ho_Circle2);
            if (My.ho_Image == null)
                return;
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = Window;
            //畫檢視範圍
            Window.ClearWindow();
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
            //畫出固定環區域
            hv_CircleRadius1 = CircleRadius1;
            hv_CircleRadius2 = CircleRadius2;

            ho_Circle1.Dispose();
            HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadius1);
            ho_Circle2.Dispose();
            HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadius2);
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "yellow");
            HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
            HOperatorSet.DispObj(ho_Circle1, hWindowControl1.HalconWindow);
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "orange");
            HOperatorSet.DispObj(ho_Circle2, hWindowControl1.HalconWindow);
        }

        public void ImagePro(HWindow Window, HObject theImage, HTuple hv_ResultRow, HTuple hv_ResultColumn, int n, out HTuple hv_ResultArea_TestDefeat, out HTuple hv_ResultArea_TestNoGlue)
        {
             HObject ho_RegionInterSection_TestDefest = new HObject();
            HObject ho_RegionInterSection_TestNoGlue = new HObject();
            hv_ResultArea_TestDefeat = 0;
            hv_ResultArea_TestNoGlue = 0;
            try
            {
                HOperatorSet.CopyImage(theImage,out ho_Image);
              
                //設定初始值為會過關
                hv_Area5Length = 0;
                hv_Area6Length = 0;
                hv_Area8Length = 1;
                hv_TotalAngle = 360;
                hv_ResultAngle_2 = 360;
                hv_TotalAnglePF = 360;
                hv_regionCount = 1000;
                hv_LackAngle_Max = 0;
                hv_LackAngle_Max_PF = 0;
                #region
                if (My.TestNoGlue)
                {
                    ho_RegionInterSection_TestNoGlue.Dispose();
                    TestNoGlue(theImage, hv_ResultRow, hv_ResultColumn, out ho_RegionInterSection_TestNoGlue, out hv_ResultArea_TestNoGlue);
                }
                #endregion
                #region 求膠角度
                if (My.TestFixedCollar)
                {
                    //畫出固定環區域
                    hv_CircleRadius1 = dRingInRange;
                    hv_CircleRadius2 = dRingOutRange;

                    ho_Circle1.Dispose();
                    HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadius1);
                    ho_Circle2.Dispose();
                    HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadius2);
                    ho_RegionDifference.Dispose();
                    HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                    //分割出固定環
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                    HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                    //方法一
                    if (My.MethodChoice == 0)
                    {
                        ho_PolarTransImage.Dispose();
                        HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImage, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadius1, hv_CircleRadius2,
                           (hv_CircleRadius2 + hv_CircleRadius1) * Math.PI, hv_CircleRadius2 - hv_CircleRadius1, "nearest_neighbor");
                        //灰度設定
                        hv_grayBlack = My.dGraythresholdBlack;
                        hv_grayWhite = My.dGraythresholdWhite;

                        ho_Edges.Dispose();
                        if (My.Detection_Black && My.Detection_White)
                        {
                            HOperatorSet.Threshold(ho_PolarTransImage, out ho_Edges, (new HTuple(0)).TupleConcat(hv_grayWhite), hv_grayBlack.TupleConcat(255));
                        }
                        else if (My.Detection_Black && !My.Detection_White)
                        {
                            HOperatorSet.Threshold(ho_PolarTransImage, out ho_Edges, 0, hv_grayBlack);
                        }
                        else if (!My.Detection_Black && My.Detection_White)
                        {
                            HOperatorSet.Threshold(ho_PolarTransImage, out ho_Edges, hv_grayWhite, 255);
                        }
                        else
                        {
                            MessageBox.Show("黑白至少要檢測一項");
                            return;
                        }
                        //HOperatorSet.AreaCenter(ho_RegionUnion, out hv_Area, out hv_RowShortEndPF, out hv_Area_Upper);
                       
                        //填滿縫隙
                        ho_RegionFillUp.Dispose();
                        HOperatorSet.FillUp(ho_Edges, out ho_RegionFillUp);
                        //將相鄰的面積相連
                        ho_RegionClosing.Dispose();
                        HOperatorSet.ClosingCircle(ho_RegionFillUp, out ho_RegionClosing, 3.5);
                        //分割
                        ho_ConnectedRegions.Dispose();
                        HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
                        //開放設置去掉過小面積
                        hv_UnderSizeArea = dUnderSizeArea;
                        ho_SelectedRegions.Dispose();
                        HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                            "and", hv_UnderSizeArea, 99999999);
                        ho_RegionUnion.Dispose();
                        HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion);
                    }
                    else if (My.MethodChoice == 1)//方法二
                    {
                        HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                        hv_CircleRadius1 = dRingInRange;
                        hv_CircleRadius2 = dRingOutRange;

                        HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                        ho_Circle1.Dispose();
                        HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadius1);
                        ho_Circle2.Dispose();
                        HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadius2);
                        ho_RegionDifference.Dispose();
                        HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                        HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                        //分割出固定環
                        ho_ImageReduced.Dispose();
                        HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                        HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                        ho_PolarTransImage.Dispose();
                        HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImage, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadius1, hv_CircleRadius2,
                            (hv_CircleRadius2 + hv_CircleRadius1) * Math.PI, hv_CircleRadius2 - hv_CircleRadius1, "nearest_neighbor");

                        hv_MeanWidth_1 = (HTuple)dMeanWidth_1;
                        hv_MeanHeight_1 = (HTuple)dMeanHeight_1;
                        hv_MeanWidth_2 = (HTuple)dMeanWidth_2;
                        hv_MeanHeight_2 = (HTuple)dMeanHeight_2;
                        if (My.GlueLightDarkChoice == 0)
                            hv_GlueLightDarkChoice = "light";
                        else
                            hv_GlueLightDarkChoice = "dark";
                        ho_ImageMean1.Dispose();
                        HOperatorSet.MeanImage(ho_PolarTransImage, out ho_ImageMean1, hv_MeanWidth_1, hv_MeanHeight_1);
                        ho_ImageMean2.Dispose();
                        HOperatorSet.MeanImage(ho_PolarTransImage, out ho_ImageMean2, hv_MeanWidth_2, hv_MeanHeight_2);

                        //dyn_threshold (ImageMean1, ImageMean, RegionDynThresh, 5, GlueLightDarkChoice)
                        ho_RegionDynThresh.Dispose();
                        HOperatorSet.DynThreshold(ho_ImageMean1, ho_ImageMean2, out ho_RegionDynThresh, (HTuple)dDynThresholdSet, hv_GlueLightDarkChoice);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.FillUp(ho_RegionDynThresh, out ExpTmpOutVar_0);
                            ho_RegionDynThresh.Dispose();
                            ho_RegionDynThresh = ExpTmpOutVar_0;
                        }
                        hv_closeWidthValue = (HTuple)dCloseWidthValue;
                        hv_closeHeightValue = (HTuple)dCloseHeightValue;
                        if (My.Closing)
                        {
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ClosingRectangle1(ho_RegionDynThresh, out ExpTmpOutVar_0, hv_closeWidthValue, hv_closeHeightValue);
                                ho_RegionDynThresh.Dispose();
                                ho_RegionDynThresh = ExpTmpOutVar_0;
                            }
                        }

                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.FillUp(ho_RegionDynThresh, out ExpTmpOutVar_0);
                            ho_RegionDynThresh.Dispose();
                            ho_RegionDynThresh = ExpTmpOutVar_0;
                        }
                        hv_OpenHeightValue = (HTuple)dOpenHeightValue;
                        hv_OpenWidthValue = (HTuple)dOpenWidthValue;
                        if (My.Opening)
                        {
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.OpeningRectangle1(ho_RegionDynThresh, out ExpTmpOutVar_0, hv_OpenWidthValue, hv_OpenHeightValue);
                                ho_RegionDynThresh.Dispose();
                                ho_RegionDynThresh = ExpTmpOutVar_0;
                            }
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Connection(ho_RegionDynThresh, out ExpTmpOutVar_0);
                            ho_RegionDynThresh.Dispose();
                            ho_RegionDynThresh = ExpTmpOutVar_0;
                        }
                        hv_UnderSizeArea2 = iUnderSizeArea2;
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.SelectShape(ho_RegionDynThresh, out ExpTmpOutVar_0, "area", "and", hv_UnderSizeArea2, 99999999);
                            ho_RegionDynThresh.Dispose();
                            ho_RegionDynThresh = ExpTmpOutVar_0;
                        }
                        if (My.FilterHeight)
                        {
                            hv_Height_Upper = (HTuple)dFilterHeight_Upper;
                            hv_Height_Lower = (HTuple)dFilterHeight_Lower;
                            ho_SelectedRegionsA.Dispose();
                            HOperatorSet.SelectShape(ho_RegionDynThresh, out ho_SelectedRegionsA, "height", "and", hv_Height_Lower, hv_Height_Upper);
                            ho_RegionDynThresh.Dispose();
                            ho_RegionDynThresh = ho_SelectedRegionsA.CopyObj(1, -1);
                        }
                        if (My.FilterWidth)
                        {
                            hv_Width_Upper = (HTuple)dFilterWidth_Upper;
                            hv_Width_Lower = (HTuple)dFilterWidth_Lower;
                            ho_SelectedRegionsB.Dispose();
                            HOperatorSet.SelectShape(ho_RegionDynThresh, out ho_SelectedRegionsB, "width", "and", hv_Width_Lower, hv_Width_Upper);
                            ho_RegionDynThresh.Dispose();
                            ho_RegionDynThresh = ho_SelectedRegionsB.CopyObj(1, -1);
                        }
                        if (My.FilterWidth && My.FilterHeight)
                        {
                            ho_RegionDynThresh.Dispose();
                            HOperatorSet.Union2(ho_SelectedRegionsA, ho_SelectedRegionsB, out ho_RegionDynThresh);
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union1(ho_RegionDynThresh, out ExpTmpOutVar_0);
                            ho_RegionUnion.Dispose();
                            ho_RegionUnion = ExpTmpOutVar_0;
                        }
                        //polar_trans_region_inv (RegionDynThresh, XYTransRegion, ResultRow, ResultColumn, 0, 6.28319, CircleRadius1, CircleRadius2, 512, 512, Width, Height, 'nearest_neighbor')
                    }
                        hv_regionCount = My.iGlueCount;
                        if (My.DecisionMethodChoice == 0)
                        {
                            ho_SelectRegion.Dispose(); ho_RegionInterSectionPF3.Dispose();//ho_RegionInterSectionPF3沒用的
                            find_angle_plus(ho_PolarTransImage, ho_RegionUnion, ho_RegionUnion,
                                out ho_SelectRegion, out ho_RegionInterSectionPF3, 2, out hv_TotalAngle,
                                out hv_LackAngle_Max);
                        }
                        else if (My.DecisionMethodChoice == 1)
                        {
                            ho_ConnectedRegions2.Dispose();
                            HOperatorSet.Connection(ho_XYTransRegion, out ho_ConnectedRegions2);
                            HOperatorSet.CountObj(ho_ConnectedRegions, out hv_Number1);
                            hv_regionCount = hv_Number1.Clone();
                            HTuple end_val321 = hv_Number1;
                            HTuple step_val321 = 1;
                            for (hv_A = 1; hv_A.Continue(end_val321, step_val321); hv_A = hv_A.TupleAdd(step_val321))
                            {
                                HTuple end_val322 = hv_Number1;
                                HTuple step_val322 = 1;
                                for (hv_B = 1; hv_B.Continue(end_val322, step_val322); hv_B = hv_B.TupleAdd(step_val322))
                                {
                                    if ((int)((new HTuple(hv_A.TupleEqual(hv_B))).TupleOr(new HTuple(hv_B.TupleLess(
                                        hv_A)))) != 0)
                                    {
                                        continue;
                                    }
                                    ho_ImageA.Dispose();
                                    HOperatorSet.SelectObj(ho_ConnectedRegions2, out ho_ImageA, hv_A);
                                    ho_ImageB.Dispose();
                                    HOperatorSet.SelectObj(ho_ConnectedRegions2, out ho_ImageB, hv_B);
                                    HOperatorSet.DistanceRrMin(ho_ImageA, ho_ImageB, out hv_MinDistance,
                                        out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2);
                                    if ((int)(new HTuple(hv_MinDistance.TupleLess(My.dRegionDistance))) != 0)
                                    {
                                        hv_regionCount = hv_regionCount - 1;

                                    }
                                }
                            }
                            HOperatorSet.CopyImage(ho_XYTransRegion, out ho_IgnoreRegion);
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.PolarTransRegionInv(ho_SelectRegion, out ExpTmpOutVar_0, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadius1, hv_CircleRadius2,
                                (hv_CircleRadius2+hv_CircleRadius1) * Math.PI, hv_CircleRadius2 - hv_CircleRadius1, hv_Width, hv_Height, "nearest_neighbor");

                            ho_SelectRegion.Dispose();
                            ho_SelectRegion = ExpTmpOutVar_0;
                        }
                        //{
                        //    HObject ExpTmpOutVar_0;
                        //    HOperatorSet.PolarTransRegionInv(ho_IgnoreRegion, out ExpTmpOutVar_0, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadius1, hv_CircleRadius2,
                        //        hv_CircleRadius2 * 2 * Math.PI, hv_CircleRadius2 - hv_CircleRadius1, hv_Width, hv_Height, "nearest_neighbor");

                        //    ho_IgnoreRegion.Dispose();
                        //    ho_IgnoreRegion = ExpTmpOutVar_0;
                        //}
                    
                }
                #endregion
                #region 小台階膠水識別
                if (My.TestPlatform)
                {
                    if (My.MethodChoice2 == 0)
                    {
                        MessageBox.Show("判定縫隙膠水方法一已移除,請使用方法二");
                        return;
                    }
                    else if (My.MethodChoice2 == 1)
                    {

                        if (My.DetectionPF2_Black || My.DetectionPF2_White)
                        {
                            if (My.dOutRangePF2 > My.dOutRangePF)
                                hv_CircleRadiusPF2 = My.dOutRangePF2 + 2;
                            else
                                hv_CircleRadiusPF2 = My.dOutRangePF + 2;
                            if (My.dInRangePF2 > My.dInRangePF)
                                hv_CircleRadiusPF1 = My.dInRangePF - 2;
                            else
                                hv_CircleRadiusPF1 = My.dInRangePF2 - 2;
                        }
                        else
                        {
                            hv_CircleRadiusPF2 = My.dOutRangePF + 2;
                            hv_CircleRadiusPF1 = My.dInRangePF - 2;
                        }
                        HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                        ho_Circle1.Dispose();
                        HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF1);
                        ho_Circle2.Dispose();
                        HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF2);
                        ho_RegionDifference.Dispose();
                        HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                        HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                        //分割出固定環
                        ho_ImageReduced.Dispose();
                        HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                        //dev_set_draw ('fill')
                        //方法二
                        ho_PolarTransImagePF.Dispose();
                        HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImagePF, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                            (hv_CircleRadiusPF1 + hv_CircleRadiusPF2) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, "nearest_neighbor");

                        hv_MeanWidth_1 = 1;
                        hv_MeanHeight_1 = 1;
                        hv_MeanWidth_2 = 1;
                        hv_MeanHeight_2 = (hv_CircleRadiusPF2 - hv_CircleRadiusPF1)/2;
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.MedianRect(ho_PolarTransImagePF, out ExpTmpOutVar_0,50,2);
                            ho_PolarTransImagePF.Dispose();
                            ho_PolarTransImagePF = ExpTmpOutVar_0;
                        }
                        ho_ImageMean1.Dispose();
                        HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF1, hv_MeanWidth_1, hv_MeanHeight_1);
                        ho_ImageMean2.Dispose();
                        HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF2, hv_MeanWidth_2, hv_MeanHeight_2);
                        if (My.DetectionPF_Light2 && My.DetectionPF_Dark2)
                        {
                            ho_RegionDynThreshPF2.Dispose();
                            HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF2, iDynthresholdLightPF2, "light");
                            ho_RegionDynThreshPF1.Dispose();
                            HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF1, iDynthresholdDarkPF2, "dark");
                            ho_RegionUnionPF1.Dispose();
                            HOperatorSet.Union2(ho_RegionDynThreshPF1, ho_RegionDynThreshPF2, out ho_RegionUnionPF1);
                        }
                        else if (My.DetectionPF_Dark2)
                        {
                            ho_RegionUnionPF1.Dispose();
                            HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, iDynthresholdDarkPF2, "dark");
                        }
                        else if (My.DetectionPF_Light2)
                        {
                            ho_RegionUnionPF1.Dispose();
                            HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, iDynthresholdLightPF2, "light");
                        }

                        hv_grayBlackPF = iGraythresholdBlackPF2;
                        hv_grayWhitePF = iGraythresholdWhitePF2;
                        if (My.DetectionPF_Black && My.DetectionPF_White)
                        {
                            ho_RegionUnionPF2.Dispose();
                            HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, (new HTuple(0)).TupleConcat(hv_grayWhitePF), hv_grayBlackPF.TupleConcat(255));
                        }
                        else if (My.DetectionPF_Black)
                        {
                            ho_RegionUnionPF2.Dispose();
                            HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, 0, hv_grayBlackPF);
                        }
                        else if (My.DetectionPF_White)
                        {
                            ho_RegionUnionPF2.Dispose();
                            HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, hv_grayWhitePF, 255);
                        }
                        ho_RegionDynThreshPF2.Dispose();
                        if ((My.DetectionPF_Light2 || My.DetectionPF_Dark2) && (My.DetectionPF_White || My.DetectionPF_Black))
                            HOperatorSet.Intersection(ho_RegionUnionPF1, ho_RegionUnionPF2, out ho_RegionDynThreshPF2);
                        else if (!My.DetectionPF_White && !My.DetectionPF_Black)
                            HOperatorSet.Union1(ho_RegionUnionPF1, out ho_RegionDynThreshPF2);
                        else if (!My.DetectionPF_Dark2 && !My.DetectionPF_Light2)
                            HOperatorSet.Union1(ho_RegionUnionPF2, out ho_RegionDynThreshPF2);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.FillUp(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                            ho_RegionDynThreshPF2.Dispose();
                            ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                        }
                        if (My.ClosingPF2)
                        {
                            hv_closeWidthValue = iCloseWidthPF2;
                            hv_closeHeightValue = iCloseHeightPF2;
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ClosingRectangle1(ho_RegionDynThreshPF2, out ExpTmpOutVar_0, hv_closeWidthValue, hv_closeHeightValue);
                                ho_RegionDynThreshPF2.Dispose();
                                ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                            }
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.FillUp(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                            ho_RegionDynThreshPF2.Dispose();
                            ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                        }
                        if (My.OpeningPF2)
                        {
                            hv_OpenHeightValue = iOpenHeightPF2;
                            hv_OpenWidthValue = iOpenWidthPF2;
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.OpeningRectangle1(ho_RegionDynThreshPF2, out ExpTmpOutVar_0, hv_OpenWidthValue, hv_OpenHeightValue);
                                ho_RegionDynThreshPF2.Dispose();
                                ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                            }
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Connection(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                            ho_RegionDynThreshPF2.Dispose();
                            ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                        }
                        hv_UnderSizeArea2 = iUnderSizeAreaPF2;
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.SelectShape(ho_RegionDynThreshPF2, out ExpTmpOutVar_0, "area", "and", hv_UnderSizeArea2, 99999999);
                            ho_RegionDynThreshPF2.Dispose();
                            ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union1(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                            ho_RegionDynThreshPF2.Dispose();
                            ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                        }
                        ho_RectanglePF2.Dispose();
                        HOperatorSet.GenRectangle1(out ho_RectanglePF2, 2, 0, My.dOutRangePF - hv_CircleRadiusPF1, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                         ho_RegionInterSectionPF2.Dispose();
                         HOperatorSet.Intersection(ho_RegionDynThreshPF2, ho_RectanglePF2, out ho_RegionInterSectionPF2);
                         ho_RegionUnionPF3.Dispose();
                        if (My.DetectionPF2_Black && My.DetectionPF2_White)
                        {
                            HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF3, (new HTuple(0)).TupleConcat(My.dGraythresholdWhitePF3), ((HTuple)My.dGraythresholdBlackPF3).TupleConcat(255));
                        }
                        else if (My.DetectionPF2_Black)
                        {
                            HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF3, 0, My.dGraythresholdBlackPF3);
                        }
                        else if (My.DetectionPF2_White)
                        {
                            HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF3, My.dGraythresholdWhitePF3, 255);
                        }
                        if (My.DetectionPF2_Black || My.DetectionPF2_White)
                        {
                            ho_RectanglePF3.Dispose();
                            HOperatorSet.GenRectangle1(out ho_RectanglePF3, My.dInRangePF2 - hv_CircleRadiusPF1, 0, My.dOutRangePF2 - hv_CircleRadiusPF1, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                            ho_RegionInterSectionPF3.Dispose();
                            HOperatorSet.Intersection(ho_RegionUnionPF3, ho_RectanglePF3, out ho_RegionInterSectionPF3);
                            ho_SelectRegionPF1.Dispose(); ho_SelectRegionPF2.Dispose();
                            find_angle_plus(ho_PolarTransImagePF, ho_RegionInterSectionPF2, ho_RegionInterSectionPF3,
                                out ho_SelectRegionPF1, out ho_SelectRegionPF2, 2, out hv_TotalAnglePF,
                                out hv_LackAngle_Max_PF);
                        }
                        else
                        {
                            ho_SelectRegionPF1.Dispose(); ho_SelectRegionPF2.Dispose();
                            find_angle_plus(ho_PolarTransImagePF, ho_RegionInterSectionPF2, ho_RegionInterSectionPF2,
                                out ho_SelectRegionPF1, out ho_SelectRegionPF2, 2, out hv_TotalAnglePF,
                                out hv_LackAngle_Max_PF);
                        }
                        

                        //ho_SelectRegionPF.Dispose(); ho_IgnoreRegionPF.Dispose();
                        //find_angle(ho_PolarTransImagePF, ho_RegionDynThreshPF2, out ho_SelectRegionPF, out ho_IgnoreRegionPF, iGlueAngleSet, iGlueRatioSetPF, out hv_TotalAnglePF, out hv_LackAngle_Max_PF);
 
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.PolarTransRegionInv(ho_RegionInterSectionPF2, out ExpTmpOutVar_0, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                            (hv_CircleRadiusPF1 + hv_CircleRadiusPF2) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor");

                        ho_RegionInterSectionPF2.Dispose();
                        ho_RegionInterSectionPF2 = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.PolarTransRegionInv(ho_RegionInterSectionPF3, out ExpTmpOutVar_0, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                            (hv_CircleRadiusPF1 + hv_CircleRadiusPF2) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor");

                        ho_RegionInterSectionPF3.Dispose();
                        ho_RegionInterSectionPF3 = ExpTmpOutVar_0;
                    }
                }
                #endregion
                #region 檢測缺陷
                if (My.TestDefeat)
                {
                    hv_CircleRadiusPF2 = My.OuterRadius_TestDefeat + 2;
                    hv_CircleRadiusPF1 = My.InnerRadius_TestDefeat - 2;
                    hv_ResultArea_TestDefeat = 0;
                    HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                    ho_Circle1.Dispose();
                    HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF1);
                    ho_Circle2.Dispose();
                    HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF2);
                    ho_RegionDifference.Dispose();
                    HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                    HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                    //分割檢測區域
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                    //dev_set_draw ('fill')
                    //方法二
                    ho_PolarTransImagePF.Dispose();
                    HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImagePF, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                        (hv_CircleRadiusPF1 + hv_CircleRadiusPF2) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, "nearest_neighbor");

                    hv_MeanWidth_1 = 1;
                    hv_MeanHeight_1 = 1;
                    hv_MeanWidth_2 = 1;
                    hv_MeanHeight_2 = (hv_CircleRadiusPF2 - hv_CircleRadiusPF1)/2;
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.MedianRect(ho_PolarTransImagePF, out ExpTmpOutVar_0,10,2);
                        ho_PolarTransImagePF.Dispose();
                        ho_PolarTransImagePF = ExpTmpOutVar_0;
                    }
                    ho_ImageMean1.Dispose();
                    HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF1, hv_MeanWidth_1, hv_MeanHeight_1);
                    ho_ImageMean2.Dispose();
                    HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF2, hv_MeanWidth_2, hv_MeanHeight_2);
                    if (My.Detection_Light_TestDefeat && My.Detection_Dark_TestDefeat)
                    {
                        ho_RegionDynThreshPF2.Dispose();
                        HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF2, My.DynthresholdLight_TestDefeat, "light");
                        ho_RegionDynThreshPF1.Dispose();
                        HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF1, My.DynthresholdDark_TestDefeat, "dark");
                        ho_RegionUnionPF1.Dispose();
                        HOperatorSet.Union2(ho_RegionDynThreshPF1, ho_RegionDynThreshPF2, out ho_RegionUnionPF1);
                    }
                    else if (My.Detection_Dark_TestDefeat)
                    {
                        ho_RegionUnionPF1.Dispose();
                        HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, My.DynthresholdDark_TestDefeat, "dark");
                    }
                    else if (My.Detection_Light_TestDefeat)
                    {
                        ho_RegionUnionPF1.Dispose();
                        HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, My.DynthresholdLight_TestDefeat, "light");
                    }

                    hv_grayBlackPF = My.GraythresholdBlack_TestDefeat;
                    hv_grayWhitePF = My.GraythresholdWhite_TestDefeat;
                    if (My.Detection_Black_TestDefeat && My.Detection_White_TestDefeat)
                    {
                        ho_RegionUnionPF2.Dispose();
                        HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, (new HTuple(0)).TupleConcat(hv_grayWhitePF), hv_grayBlackPF.TupleConcat(255));
                    }
                    else if (My.Detection_Black_TestDefeat)
                    {
                        ho_RegionUnionPF2.Dispose();
                        HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, 0, hv_grayBlackPF);
                    }
                    else if (My.Detection_White_TestDefeat)
                    {
                        ho_RegionUnionPF2.Dispose();
                        HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, hv_grayWhitePF, 255);
                    }
                    ho_RegionDynThreshPF2.Dispose();
                    if ((My.Detection_Light_TestDefeat || My.Detection_Dark_TestDefeat) && (My.Detection_White_TestDefeat || My.Detection_Black_TestDefeat))
                        HOperatorSet.Intersection(ho_RegionUnionPF1, ho_RegionUnionPF2, out ho_RegionDynThreshPF2);
                    else if (!My.Detection_White_TestDefeat && !My.Detection_Black_TestDefeat)
                        HOperatorSet.Union1(ho_RegionUnionPF1, out ho_RegionDynThreshPF2);
                    else if (!My.Detection_Dark_TestDefeat && !My.Detection_Light_TestDefeat)
                        HOperatorSet.Union1(ho_RegionUnionPF2, out ho_RegionDynThreshPF2);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.FillUp(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                        ho_RegionDynThreshPF2.Dispose();
                        ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                    }
                    if (My.Closing_TestDefeat)
                    {
                        hv_closeWidthValue = My.CloseWidth_TestDefeat;
                        hv_closeHeightValue = My.CloseHeight_TestDefeat;
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ClosingRectangle1(ho_RegionDynThreshPF2, out ExpTmpOutVar_0, hv_closeWidthValue, hv_closeHeightValue);
                            ho_RegionDynThreshPF2.Dispose();
                            ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                        }
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.FillUp(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                        ho_RegionDynThreshPF2.Dispose();
                        ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                    }
                    if (My.Opening_TestDefeat)
                    {
                        hv_OpenHeightValue = My.OpenHeight_TestDefeat;
                        hv_OpenWidthValue = My.OpenWidth_TestDefeat;
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.OpeningRectangle1(ho_RegionDynThreshPF2, out ExpTmpOutVar_0, hv_OpenWidthValue, hv_OpenHeightValue);
                            ho_RegionDynThreshPF2.Dispose();
                            ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                        }
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Connection(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                        ho_RegionDynThreshPF2.Dispose();
                        ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                    }
                    hv_UnderSizeArea2 = My.UnderSizeArea_TestDefeat;
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SelectShape(ho_RegionDynThreshPF2, out ExpTmpOutVar_0, "area", "and", hv_UnderSizeArea2, 99999999);
                        ho_RegionDynThreshPF2.Dispose();
                        ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Union1(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                        ho_RegionDynThreshPF2.Dispose();
                        ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                    }
                    ho_RectanglePF2.Dispose();
                    HOperatorSet.GenRectangle1(out ho_RectanglePF2, 2, 0, hv_CircleRadiusPF2 - hv_CircleRadiusPF1 + 2, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                    ho_RegionInterSection_TestDefeat.Dispose();
                    HOperatorSet.Intersection(ho_RegionDynThreshPF2, ho_RectanglePF2, out ho_RegionInterSection_TestDefeat);
      //ho_SelectRegionPF1.Dispose(); ho_SelectRegionPF2.Dispose();
                        //find_angle_plus(ho_PolarTransImagePF, ho_RegionInterSectionPF2, ho_RegionInterSectionPF2,
                        //    out ho_SelectRegionPF1, out ho_SelectRegionPF2, 2, out hv_TotalAnglePF,
                        //    out hv_LackAngle_Max_PF);
                   
                    
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.PolarTransRegionInv(ho_RegionInterSection_TestDefeat, out ExpTmpOutVar_0, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                            (hv_CircleRadiusPF1 + hv_CircleRadiusPF2) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor");

                        ho_RegionInterSection_TestDefest.Dispose();
                        ho_RegionInterSection_TestDefest = ExpTmpOutVar_0;
                    }
                    HOperatorSet.RegionFeatures(ho_RegionInterSection_TestDefest, "area", out hv_ResultArea_TestDefeat);
                    if (hv_ResultArea_TestDefeat.Length == 0)
                        hv_ResultArea_TestDefeat = 0;
                    }
                 #endregion
                set_display_font(hv_ExpDefaultWinHandle, 20, "mono", "true", "false");
                //HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                //檢測無膠才顯示
                if (My.TestNoGlue)
                {
                    HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(ho_RegionInterSection_TestNoGlue, hv_ExpDefaultWinHandle);
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 500, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("NoGlueArea:" + Math.Round(hv_ResultArea_TestNoGlue.D)) + " pixel");

                }

                if (My.TestFixedCollar)
                {
                    HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                    HOperatorSet.DispObj(ho_SelectRegion, hv_ExpDefaultWinHandle);
                    //HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    //HOperatorSet.DispObj(ho_IgnoreRegion, hv_ExpDefaultWinHandle);
                }
                //檢測縫隙才顯示
                if (My.TestPlatform)
                {
                    HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(ho_RegionInterSectionPF2, hv_ExpDefaultWinHandle);
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                    HOperatorSet.DispObj(ho_RegionInterSectionPF3, hv_ExpDefaultWinHandle);
                }
                
                //檢測缺陷才顯示
                if (My.TestDefeat)
                {
                    HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.DispObj(ho_RegionInterSection_TestDefest, hv_ExpDefaultWinHandle);
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("DefeatArea:" + Math.Round(hv_ResultArea_TestDefeat.D)) + " pixel");
                }
                
                if (My.TestFixedCollar)
                {
                    if (My.DecisionMethodChoice == 0)
                    {
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("Angle:" + Math.Round((double)hv_TotalAngle)) + " 度");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 100, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("LackGlueAngle_Max:" + Math.Round((double)hv_LackAngle_Max)) + " 度");
                    }
                    else if (My.DecisionMethodChoice == 1)
                    {
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("數量:" + hv_regionCount));
                    }
                }
                if (My.TestPlatform)
                {
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("AnglePF:" + Math.Round((double)hv_TotalAnglePF) + " 度"));
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 300, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("LackGlueAnglePF_Max:" + Math.Round((double)hv_LackAngle_Max_PF)) + " 度");
                }
         
                hv_AngleSet = dAngleSet;
                hv_AngleSet2 = dAngleSet2;
                hv_AngleSetPF = dAngleSetPF;
                set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                //如果檢測無膠
                if (My.TestNoGlue)
                {
                    if (hv_ResultArea_TestNoGlue.D > My.AreaUpper_TestNoGlue || hv_ResultArea_TestNoGlue.D < My.AreaLower_TestNoGlue)
                    {
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, hv_Height - 500, 200);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "小台階無膠NG");
                        LogResult = "NG";

                        Vision.VisionResult[n] = "NG";
                        //擷取當前畫面圖片為Image
                        HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);

                        ho_Circle1.Dispose();
                        ho_Circle2.Dispose();
                        ho_RegionDifference.Dispose();
                        ho_ImageReduced.Dispose();
                        ho_Edges.Dispose();
                        ho_RegionFillUp.Dispose();
                        ho_RegionClosing.Dispose();
                        ho_ConnectedRegions.Dispose();
                        ho_SelectedRegions.Dispose();
                        ho_RegionUnion.Dispose();
                        ho_RegionIntersection.Dispose();
                        ho_PolarTransImage.Dispose();
                        ho_ImageMean1.Dispose();
                        ho_ImageMean.Dispose();
                        ho_RegionDynThresh.Dispose();
                        ho_XYTransRegion.Dispose();
                        ho_RegionFillUp5.Dispose();
                        ho_RegionClosing2.Dispose();
                        ho_RegionFillUp5.Dispose();
                        ho_ConnectedRegions2.Dispose();
                        ho_SelectedRegions2.Dispose();
                        ho_RegionUnion2.Dispose();
                        ho_RegionIntersection.Dispose();
                        ho_AllRegionXLD.Dispose();
                        ho_SelectRegion.Dispose();
                        ho_IgnoreRegion.Dispose();
                        ho_CirclePF1.Dispose();
                        ho_CirclePF2.Dispose();
                        ho_RegionDifferencePF.Dispose();
                        ho_EdgesPF.Dispose();
                        ho_RegionFillUpPF.Dispose();
                        ho_RegionClosingPF.Dispose();
                        ho_ConnectedRegionsPF.Dispose();
                        ho_SelectedRegionsPF.Dispose();
                        ho_RegionUnionPF.Dispose();
                        ho_AllRegionXLDPF.Dispose();
                        ho_SelectRegionPF.Dispose();
                        ho_IgnoreRegionPF.Dispose();

                        return;
                    }
                }


                //看看
                if ((int)((new HTuple((new HTuple((new HTuple((new HTuple((new HTuple((new HTuple((new HTuple(hv_TotalAngle.TupleGreaterEqual(
          hv_AngleSet))).TupleAnd(new HTuple(hv_TotalAnglePF.TupleGreaterEqual(hv_AngleSetPF))))).TupleAnd(
          new HTuple(hv_Area5Length.TupleEqual(0))))).TupleAnd(new HTuple(hv_Area6Length.TupleEqual(
          0))))).TupleAnd(new HTuple(hv_ResultAngle_2.TupleGreaterEqual(hv_AngleSet2))))).TupleAnd(
          new HTuple(hv_ResultRow.TupleGreater(0))))).TupleAnd(new HTuple(hv_ResultColumn.TupleGreater(
          0))))).TupleAnd(new HTuple(hv_Area8Length.TupleGreater(0)))) != 0 && hv_regionCount>=My.iGlueCount && hv_LackAngle_Max<=My.dLackMaxAngleSet&& hv_LackAngle_Max_PF<=My.dLackMaxAngleSetPF
                    && My.DetectionArea_TestDefeat >= hv_ResultArea_TestDefeat)
                  {
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, hv_Height - 500, 200);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "OK");
                    LogResult = "OK";

                    Vision.VisionResult[n] = "OK";
                    //擷取當前畫面圖片為Image
                    HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                }
                else if (My.dResultRow == 0 || My.dResultColumn ==0)
                {
                    set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    LogResult = "Miss";
                    Vision.VisionResult[n] = "Miss";
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, hv_Height - 400, 200);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                    //擷取當前畫面圖片為Image
                    HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                }
                else if (My.DetectionArea_TestDefeat < hv_ResultArea_TestDefeat)
                {
                    set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                   
                    Vision.VisionResult[n] = "NG4";
                    LogResult = "NG4";
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, hv_Height - 200, 200);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "缺陷NG");
                    //擷取當前畫面圖片為Image
                    HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                }
                else
                {
                    set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                    Vision.VisionResult[n] = "NG";
                    if ((int)(new HTuple(hv_TotalAnglePF.TupleLess(hv_AngleSetPF))) != 0 || hv_LackAngle_Max_PF > My.dLackMaxAngleSetPF)
                    {
                        Vision.VisionResult[n] = "NG2";
                        LogResult = "NG2";
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, hv_Height - 200, 200);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "縫隙斷膠NG");
                    }
                    else if ((int)(new HTuple(hv_TotalAngle.TupleLess(hv_AngleSet))) != 0 || hv_LackAngle_Max > My.dLackMaxAngleSet)
                    {
                        Vision.VisionResult[n] = "NG5";
                        LogResult = "NG5";
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, hv_Height - 600, 200);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "固定環斷膠NG");
                    }
                    //擷取當前畫面圖片為Image
                    HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                }
            }
            catch
            {
                set_display_font(hv_ExpDefaultWinHandle, 20, "mono", "true", "false");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                Vision.VisionResult[n] = "Miss";
                LogResult = "Miss";
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, hv_Height - 400, 200);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                //擷取當前畫面圖片為Image
                HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
            }
            
            ho_Circle1.Dispose();
            ho_Circle2.Dispose();
            ho_RegionDifference.Dispose();
            ho_ImageReduced.Dispose();
            ho_Edges.Dispose();
            ho_RegionFillUp.Dispose();
            ho_RegionClosing.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionUnion.Dispose();
            ho_RegionIntersection.Dispose();
            ho_PolarTransImage.Dispose();
            ho_ImageMean1.Dispose();
            ho_ImageMean.Dispose();
            ho_RegionDynThresh.Dispose();
            ho_XYTransRegion.Dispose();
            ho_RegionFillUp5.Dispose();
            ho_RegionClosing2.Dispose();
            ho_RegionFillUp5.Dispose();
            ho_ConnectedRegions2.Dispose();
            ho_SelectedRegions2.Dispose();
            ho_RegionUnion2.Dispose();
            ho_RegionIntersection.Dispose();
            ho_AllRegionXLD.Dispose();
            ho_SelectRegion.Dispose();
            ho_IgnoreRegion.Dispose();
            ho_CirclePF1.Dispose();
            ho_CirclePF2.Dispose();
            ho_RegionDifferencePF.Dispose();
            ho_EdgesPF.Dispose();
            ho_RegionFillUpPF.Dispose();
            ho_RegionClosingPF.Dispose();
            ho_ConnectedRegionsPF.Dispose();
            ho_SelectedRegionsPF.Dispose();
            ho_RegionUnionPF.Dispose();
            ho_AllRegionXLDPF.Dispose();
            ho_SelectRegionPF.Dispose();
            ho_IgnoreRegionPF.Dispose();
        }

        public void WriteLog(int n, string ResultOK, double ResultAngle, double ResultAnglePF, double ResultArea_TestDefeat, double ResultArea_TestNoGlue)
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
                        File.WriteAllText(Log, "Function\tCode\tTray Barcode_B\tTray No.\tToTal Count\tTray Barcode_A\tProgram ID\tProduct\tClass\tOperatorID\tMachine No.\tTime\tCT\tResult\tResultAngle\tResultAnglePF\tDefeatArea\tNoGlueArea" +
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
                                     ResultAngle.ToString("f0") + "\t" +
                                     ResultAnglePF.ToString("f0")+"\t"+
                                     ResultArea_TestDefeat.ToString("f0") +
                                     ResultArea_TestNoGlue.ToString("f0"));
                    }
                }
                catch
                {
                }
                
            }
        }

        private void tbRingOutRange_ValueChanged(object sender, EventArgs e)
        {
            nudRingOutRange.Value = tbRingOutRange.Value;
        }

        private void nudRingOutRange_ValueChanged(object sender, EventArgs e)
        {
            dRingOutRange = tbRingOutRange.Value = Convert.ToInt32(nudRingOutRange.Value);
           
            try
            {
                //這段要另外用的
                FixationRing(hWindowControl1.HalconWindow,dRingInRange,dRingOutRange);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void tbRingInRange_ValueChanged(object sender, EventArgs e)
        {
            nudRingInRange.Value = tbRingInRange.Value;
        }

        private void nudRingInRange_ValueChanged(object sender, EventArgs e)
        {
            dRingInRange = tbRingInRange.Value = Convert.ToInt32(nudRingInRange.Value);
            try
            {
                //這段要另外用的
                FixationRing(hWindowControl1.HalconWindow,dRingInRange,dRingOutRange);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void btnCenterSave_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            My.dFirstCircleRadius = dFirstCircleRadius;
            My.dReduceRadius = dReduceRadius;
            My.dGraythreshold = dGraythreshold;
            My.dCenterRadius = dCenterRadius;
            My.dLength = dLength;
            My.dMeasureThreshold = dMeasureThreshold;
            My.sGenParamValue = sGenParamValue;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "FirstCircleRadius", My.dFirstCircleRadius.ToString(),Path);
            IniFile.Write("Setting", "ReduceRadius", My.dReduceRadius.ToString(),Path);
            IniFile.Write("Setting", "Graythreshold", My.dGraythreshold.ToString(),Path);
            IniFile.Write("Setting", "CenterRadius", My.dCenterRadius.ToString(), Path);
            IniFile.Write("Setting", "Length", My.dLength.ToString(),Path);
            IniFile.Write("Setting", "MeasureThreshold", My.dMeasureThreshold.ToString(),Path);
            IniFile.Write("Setting", "GenParamValue", My.sGenParamValue.ToString(),Path);
        }

        private void btnFixationRingSave_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            My.dRingInRange = dRingInRange;
            My.dRingOutRange = dRingOutRange;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "RingInRange", My.dRingInRange.ToString(),Path);
            IniFile.Write("Setting", "RingOutRange", My.dRingOutRange.ToString(),Path);
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
                CatchCenter(hWindowControl1.HalconWindow,My.ho_Image);
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
                CatchCenter(hWindowControl1.HalconWindow,My.ho_Image);
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
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image);
            }
            catch
            {
                //MessageBox.Show("error");
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

        private void tbGraythresholdBlack_ValueChanged(object sender, EventArgs e)
        {
            nudGraythresholdBlack.Value = tbGraythresholdBlack.Value;
        }

        private void nudGraythresholdBlack_ValueChanged(object sender, EventArgs e)
        {
            dGraythresholdBlack = tbGraythresholdBlack.Value = Convert.ToInt32(nudGraythresholdBlack.Value);
            dGraythresholdWhite = tbGraythresholdWhite.Value = Convert.ToInt32(nudGraythresholdWhite.Value);
            dUnderSizeArea = tbUnderSizeArea.Value = Convert.ToInt32(nudUnderSizeArea.Value);
            HOperatorSet.GenEmptyObj(out ho_Circle1);
            HOperatorSet.GenEmptyObj(out ho_Circle2);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Edges);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            try
            {
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                //畫出固定環區域
                hv_CircleRadius1 = dRingInRange;
                hv_CircleRadius2 = dRingOutRange;
                //平滑
                //ho_ImageSmooth.Dispose();
                //HOperatorSet.SmoothImage(ho_Image, out ho_ImageSmooth, "deriche2", 0.3);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadius1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadius2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced
                    );
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                //灰度設定
                hv_grayBlack = dGraythresholdBlack;
                hv_grayWhite = dGraythresholdWhite;
                //膠水為黑色白色設定
                    ho_Edges.Dispose();
                if (My.Detection_Black && My.Detection_White)
                {
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Edges, (new HTuple(0)).TupleConcat(hv_grayWhite), hv_grayBlack.TupleConcat(255));
                }
                else if (My.Detection_Black && !My.Detection_White)
                {
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Edges, 0, hv_grayBlack);
                }
                else if (!My.Detection_Black && My.Detection_White)
                {
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Edges, hv_grayWhite, 255);
                }
                else
                {
                    MessageBox.Show("黑白至少要檢測一項");
                    return;
                }
                //填滿縫隙
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_Edges, out ho_RegionFillUp);
                //將相鄰的面積相連
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_RegionFillUp, out ho_RegionClosing, 3.5);
                //分割
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
                //開放設置去掉過小面積
                hv_UnderSizeArea = dUnderSizeArea;
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                    "and", hv_UnderSizeArea, 99999999);
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion);
                //找出與固定環區域的交集,避免360度整個填滿的情況
                ho_RegionIntersection.Dispose();
                HOperatorSet.Intersection(ho_RegionUnion, ho_RegionDifference, out ho_RegionIntersection
                    );
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_RegionIntersection, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
        }

        private void tbGraythresholdWhite_ValueChanged(object sender, EventArgs e)
        {
            nudGraythresholdWhite.Value = tbGraythresholdWhite.Value;
        }

        private void nudGraythresholdWhite_ValueChanged(object sender, EventArgs e)
        {
            dGraythresholdBlack = tbGraythresholdBlack.Value = Convert.ToInt32(nudGraythresholdBlack.Value);
            dGraythresholdWhite = tbGraythresholdWhite.Value = Convert.ToInt32(nudGraythresholdWhite.Value);
            dUnderSizeArea = tbUnderSizeArea.Value = Convert.ToInt32(nudUnderSizeArea.Value);
            HOperatorSet.GenEmptyObj(out ho_Circle1);
            HOperatorSet.GenEmptyObj(out ho_Circle2);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Edges);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            try
            {
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                //畫出固定環區域
                hv_CircleRadius1 = dRingInRange;
                hv_CircleRadius2 = dRingOutRange;
                //平滑
                //ho_ImageSmooth.Dispose();
                //HOperatorSet.SmoothImage(ho_Image, out ho_ImageSmooth, "deriche2", 0.3);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadius1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadius2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced
                    );
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                //灰度設定
                hv_grayBlack = dGraythresholdBlack;
                hv_grayWhite = dGraythresholdWhite;

                //膠水為黑色白色設定
                ho_Edges.Dispose();
                if (My.Detection_Black && My.Detection_White)
                {
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Edges, (new HTuple(0)).TupleConcat(hv_grayWhite), hv_grayBlack.TupleConcat(255));
                }
                else if (My.Detection_Black && !My.Detection_White)
                {
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Edges, 0, hv_grayBlack);
                }
                else if (!My.Detection_Black && My.Detection_White)
                {
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Edges, hv_grayWhite, 255);
                }
                else
                {
                    MessageBox.Show("黑白至少要檢測一項");
                    return;
                }
                //填滿縫隙
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_Edges, out ho_RegionFillUp);
                //將相鄰的面積相連
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_RegionFillUp, out ho_RegionClosing, 3.5);
                //分割
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
                //開放設置去掉過小面積
                hv_UnderSizeArea = dUnderSizeArea;
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                    "and", hv_UnderSizeArea, 99999999);
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion);
                //找出與固定環區域的交集,避免360度整個填滿的情況
                ho_RegionIntersection.Dispose();
                HOperatorSet.Intersection(ho_RegionUnion, ho_RegionDifference, out ho_RegionIntersection
                    );
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_RegionIntersection, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
        }

        private void nudAngleSet_ValueChanged(object sender, EventArgs e)
        {
            dAngleSet = Convert.ToInt32(nudAngleSet.Value);
            My.dAngleSet = dAngleSet;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "AngleSet", My.dAngleSet.ToString(), Path);
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
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            My.dGraythresholdBlack = dGraythresholdBlack;
            My.dGraythresholdWhite = dGraythresholdWhite;
            My.dUnderSizeArea = dUnderSizeArea;
            My.dDynThresholdSet = dDynThresholdSet;
            My.iUnderSizeArea2 = iUnderSizeArea2;
            My.dMeanWidth_1 = dMeanWidth_1;
            My.dMeanHeight_1 = dMeanHeight_1;
            My.dMeanWidth_2 = dMeanWidth_2;
            My.dMeanHeight_2 = dMeanHeight_2;
            My.dCloseWidthValue = dCloseWidthValue;
            My.dCloseHeightValue = dCloseHeightValue;
            My.dOpenWidthValue = dOpenWidthValue;
            My.dOpenHeightValue = dOpenHeightValue;
            My.dFilterWidth_Lower = dFilterWidth_Lower;
            My.dFilterWidth_Upper = dFilterWidth_Upper;
            My.dFilterHeight_Lower = dFilterHeight_Lower;
            My.dFilterHeight_Upper = dFilterHeight_Upper;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "GraythresholdBlack", My.dGraythresholdBlack.ToString(), Path);
            IniFile.Write("Setting", "GraythresholdWhite", My.dGraythresholdWhite.ToString(),Path);
            IniFile.Write("Setting", "UnderSizeArea", My.dUnderSizeArea.ToString(), Path);
            IniFile.Write("Setting", "DynThresholdSet", My.dDynThresholdSet.ToString(), Path);
            IniFile.Write("Setting", "UnderSizeArea2", My.iUnderSizeArea2.ToString(), Path);
            IniFile.Write("Setting", "MeanWidth_1", My.dMeanWidth_1.ToString(), Path);
            IniFile.Write("Setting", "MeanHeight_1", My.dMeanHeight_1.ToString(), Path);
            IniFile.Write("Setting", "MeanWidth_2", My.dMeanWidth_2.ToString(), Path);
            IniFile.Write("Setting", "MeanHeight_2", My.dMeanHeight_2.ToString(), Path);
            IniFile.Write("Setting", "CloseWidthValue", My.dCloseWidthValue.ToString(), Path);
            IniFile.Write("Setting", "CloseHeightValue", My.dCloseHeightValue.ToString(), Path);
            IniFile.Write("Setting", "OpenWidthValue", My.dOpenWidthValue.ToString(), Path);
            IniFile.Write("Setting", "OpenHeightValue", My.dOpenHeightValue.ToString(), Path);
            IniFile.Write("Setting", "FilterWidth_Lower", My.dFilterWidth_Lower.ToString(), Path);
            IniFile.Write("Setting", "FilterWidth_Upper", My.dFilterWidth_Upper.ToString(), Path);
            IniFile.Write("Setting", "FilterHeight_Lower", My.dFilterHeight_Lower.ToString(), Path);
            IniFile.Write("Setting", "FilterHeight_Upper", My.dFilterHeight_Upper.ToString(), Path);
        }

        private void btnImageProPlus_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            hWindowControl1.HalconWindow.ClearWindow();
            if (My.ho_Image == null)
                return;
            ImageProPlus(hWindowControl1.HalconWindow,My.ho_Image,Tray.n);           
        }

        public void ImageProPlus(HWindow hWindowControl,HObject theImage,int n)
        {
            HTuple hv_ResultArea_TestDefeat = new HTuple();
            HTuple hv_ResultArea_TestNoGlue = new HTuple();
            try
            {
                HOperatorSet.CopyImage(theImage, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl;
                //畫檢視範圍
                HOperatorSet.SetColor(hWindowControl, "green");
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                hv_GraySetting = dGraythreshold;

                ho_Region.Dispose();
                if (My.DarkLightChoice == 0)
                    HOperatorSet.Threshold(ho_ReducedImage, out ho_Region, hv_GraySetting, 255);
                else
                    HOperatorSet.Threshold(ho_ReducedImage, out ho_Region, 0, hv_GraySetting);
                ho_Connection.Dispose();
                HOperatorSet.Connection(ho_Region, out ho_Connection);
                HOperatorSet.FillUp(ho_Connection, out ho_Connection);
                hv_FirstRadius = dFirstCircleRadius;
                hv_CenterRadius = dCenterRadius;
                ho_SelectedRegions0.Dispose();
                HOperatorSet.SelectShape(ho_Connection, out ho_SelectedRegions0, (new HTuple("outer_radius")).TupleConcat(
                    "roundness"), "and", (new HTuple(0)).TupleConcat(0.8), ((hv_FirstRadius + 50)).TupleConcat(
                    1));
                HOperatorSet.AreaCenter(ho_SelectedRegions0, out hv_Area0, out hv_Row0, out hv_Column0);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions0, out ExpTmpOutVar_0, "area", "and",
                        hv_Area0.TupleMax(), hv_Area0.TupleMax() + 1);
                    ho_SelectedRegions0.Dispose();
                    ho_SelectedRegions0 = ExpTmpOutVar_0;
                }
                HOperatorSet.AreaCenter(ho_SelectedRegions0, out hv_Area1, out hv_Row1, out hv_Column1);
                //找出圓心
                gen_circle_center(ho_Image, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_Row1, hv_Column1, hv_CenterRadius,
                dLength, dMeasureThreshold, sGenParamValue, "last", out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);
                My.dResultRow = hv_ResultRow.D;
                My.dResultColumn = hv_ResultColumn.D;
                LogResult = "";
                //視覺辨識
                
                ImagePro(hWindowControl, theImage, hv_ResultRow, hv_ResultColumn,n,out hv_ResultArea_TestDefeat,out hv_ResultArea_TestNoGlue);
                //檢查方向
                if (My.TestDirection)
                {
                    FindDirection(hWindowControl, theImage,n);
                }
            }
            catch
            {
                hv_TotalAngle = 0;
                hv_TotalAnglePF = 0;
                hv_ResultArea_TestDefeat = 0;
                hv_ResultArea_TestNoGlue = 0;
                set_display_font(hWindowControl, 20, "mono", "true", "false");
                HOperatorSet.DispObj(theImage, hWindowControl);
                Vision.VisionResult[n] = "Miss";
                LogResult = "Miss";
                HOperatorSet.SetColor(hWindowControl, "red");
                HOperatorSet.SetTposition(hWindowControl, 2300, 200);
                HOperatorSet.WriteString(hWindowControl, "Miss");
                //擷取當前畫面圖片為Image
                HOperatorSet.DumpWindowImage(out ho_Image, hWindowControl);
            }
            finally
            {
                
                WriteLog(n, LogResult, (double)hv_TotalAngle, (double)hv_TotalAnglePF, hv_ResultArea_TestDefeat.D, hv_ResultArea_TestNoGlue.D);
                if (Tray.NowTray == 1)
                {
                    Vision.Images_1[n] = ho_Image;
                    Vision.Images_Now[n] = ho_Image;
                    Vision.ImagesOriginal_1[n] = theImage;
                }
                else if (Tray.NowTray == 2)
                {
                    Vision.Images_2[n] = ho_Image;
                    Vision.Images_Now[n] = ho_Image;
                    Vision.ImagesOriginal_2[n] = theImage;
                }
                ho_Circle.Dispose();
                ho_Cross.Dispose();
            }
        }

        private void tbUnderSizeArea_ValueChanged(object sender, EventArgs e)
        {
            nudUnderSizeArea.Value = tbUnderSizeArea.Value;
        }

        private void nudUnderSizeArea_ValueChanged(object sender, EventArgs e)
        {
            dGraythresholdWhite = tbGraythresholdWhite.Value = Convert.ToInt32(nudGraythresholdWhite.Value);
            dGraythresholdBlack = tbGraythresholdBlack.Value = Convert.ToInt32(nudGraythresholdBlack.Value);
            dUnderSizeArea = tbUnderSizeArea.Value = Convert.ToInt32(nudUnderSizeArea.Value);
            HOperatorSet.GenEmptyObj(out ho_Circle1);
            HOperatorSet.GenEmptyObj(out ho_Circle2);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Edges);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            try
            {
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                //畫出固定環區域
                hv_CircleRadius1 = dRingInRange;
                hv_CircleRadius2 = dRingOutRange;
                //平滑
                //ho_ImageSmooth.Dispose();
                //HOperatorSet.SmoothImage(ho_Image, out ho_ImageSmooth, "deriche2", 0.3);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadius1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadius2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced
                    );
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                if (My.MethodChoice == 0)
                {
                    //灰度設定
                    hv_grayBlack = dGraythresholdBlack;
                    hv_grayWhite = dGraythresholdWhite;

                    //膠水為黑色白色設定
                    ho_Edges.Dispose();
                    if (My.Detection_Black && My.Detection_White)
                    {
                        HOperatorSet.Threshold(ho_ImageReduced, out ho_Edges, (new HTuple(0)).TupleConcat(hv_grayWhite), hv_grayBlack.TupleConcat(255));
                    }
                    else if (My.Detection_Black && !My.Detection_White)
                    {
                        HOperatorSet.Threshold(ho_ImageReduced, out ho_Edges, 0, hv_grayBlack);
                    }
                    else if (!My.Detection_Black && My.Detection_White)
                    {
                        HOperatorSet.Threshold(ho_ImageReduced, out ho_Edges, hv_grayWhite, 255);
                    }
                    else
                    {
                        MessageBox.Show("黑白至少要檢測一項");
                        return;
                    }
                    //填滿縫隙
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_Edges, out ho_RegionFillUp);
                    //將相鄰的面積相連
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingCircle(ho_RegionFillUp, out ho_RegionClosing, 3.5);
                    //分割
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
                    //開放設置去掉過小面積
                    hv_UnderSizeArea = dUnderSizeArea;
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                        "and", hv_UnderSizeArea, 99999999);
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion);
                    //找出與固定環區域的交集,避免360度整個填滿的情況
                    ho_RegionIntersection.Dispose();
                    HOperatorSet.Intersection(ho_RegionUnion, ho_RegionDifference, out ho_RegionIntersection);
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                    HOperatorSet.DispObj(ho_RegionIntersection, hv_ExpDefaultWinHandle);
                }
                else if (My.MethodChoice == 2)
                {

                }
            }
            catch
            {
            }
        }

        private void btnSlopeSetSave_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            My.dInSlope = dInSlope;
            My.dOutSlope = dOutSlope;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "InSlope", My.dInSlope.ToString(),Path);
            IniFile.Write("Setting", "OutSlope", My.dOutSlope.ToString(),Path);
        }

        private void btnSpillSetSave_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            My.dSpilledUnderSizeArea = dSpilledUnderSizeArea;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "SpilledUnderSizeArea", My.dSpilledUnderSizeArea.ToString(),Path);
        }

        public void MarkSet(HWindow Window, HTuple rect2_len1Lower, HTuple rect2_len1Upper, HTuple rect2_len2Lower, HTuple rect2_len2Upper)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle7);
            HOperatorSet.GenEmptyObj(out ho_Circle8);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference4);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced4);
            HOperatorSet.GenEmptyObj(out ho_Region4);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions4);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions4);
            if (My.ho_Image == null)
                return;
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = Window;
            //畫檢視範圍
            Window.ClearWindow();
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);

            ho_Circle7.Dispose();
            HOperatorSet.GenCircle(out ho_Circle7, hv_ResultRow, hv_ResultColumn, My.dInSlope3);
            ho_Circle8.Dispose();
            HOperatorSet.GenCircle(out ho_Circle8, hv_ResultRow, hv_ResultColumn, My.dOutSlope3);
            ho_RegionDifference4.Dispose();
            HOperatorSet.Difference(ho_Circle8, ho_Circle7, out ho_RegionDifference4);
            ho_ImageReduced4.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference4, out ho_ImageReduced4
                );
            ho_Region4.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced4, out ho_Region4, 0, My.dGraythreshold3);
            ho_ConnectedRegions4.Dispose();
            HOperatorSet.Connection(ho_Region4, out ho_ConnectedRegions4);
            //外切矩形的長寬
            ho_SelectedRegions4.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions4, out ho_SelectedRegions4, (new HTuple("rect2_len1")).TupleConcat(
                "rect2_len2"), "and", (new HTuple(rect2_len1Lower)).TupleConcat(rect2_len2Lower), (new HTuple(rect2_len1Upper)).TupleConcat(
                rect2_len2Upper));
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "blue");
            HOperatorSet.DispObj(ho_SelectedRegions4, hv_ExpDefaultWinHandle);
        }

        private void btnCircleCenter1_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image);

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

        private void btnCircleCenter2_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image);

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

        private void btnCircleCenter3_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image);

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

        private void btnCircleCenter4_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image);

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
        private void btnCircleCenter5_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image);

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

        private void btnCenterCircle_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow; 
            if (My.ho_Image == null)
                return;
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            //畫檢視範圍
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);

            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
            //找出初始半徑
            HOperatorSet.DrawCircle(hv_ExpDefaultWinHandle, out dFirstCircleRow, out dFirstCircleColumn,
                out dCenterRadius);
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

        private void cbTestPlatform_CheckedChanged(object sender, EventArgs e)
        {
            My.TestPlatform = (cbTestPlatform.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "TestPlatform", My.TestPlatform.ToString(), Path);
        }

        private void btnCircleCenter6_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image);

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

        private void tbPFOutRange_ValueChanged(object sender, EventArgs e)
        {
            nudPFOutRange.Value = tbPFOutRange.Value;
        }

        private void nudPFOutRange_ValueChanged(object sender, EventArgs e)
        {
            dOutRangePF = tbPFOutRange.Value = Convert.ToInt32(nudPFOutRange.Value);

            try
            {
                //這段要另外用的
                FixationRing(hWindowControl1.HalconWindow,dInRangePF,dOutRangePF);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void tbPFInRange_ValueChanged(object sender, EventArgs e)
        {
            nudPFInRange.Value = tbPFInRange.Value;
        }

        private void nudPFInRange_ValueChanged(object sender, EventArgs e)
        {
            dInRangePF = tbPFInRange.Value = Convert.ToInt32(nudPFInRange.Value);

            try
            {
                //這段要另外用的
                FixationRing(hWindowControl1.HalconWindow,dInRangePF,dOutRangePF);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void btnPFSetSave_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            My.dInRangePF = dInRangePF;
            My.dOutRangePF = dOutRangePF;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "InRangePF", My.dInRangePF.ToString(), Path);
            IniFile.Write("Setting", "OutRangePF", My.dOutRangePF.ToString(), Path);
        }
        private void nudAngleSetPF_ValueChanged(object sender, EventArgs e)
        {
            dAngleSetPF = Convert.ToInt32(nudAngleSetPF.Value);
            My.dAngleSetPF = dAngleSetPF;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "AngleSetPF", My.dAngleSetPF.ToString(), Path);
        }

        private void btnPFSave_Click(object sender, EventArgs e)
        {
            My.dGraythresholdBlackPF = dGraythresholdBlackPF;
            My.dGraythresholdWhitePF = dGraythresholdWhitePF;
            My.dUnderSizeAreaPF = dUnderSizeAreaPF;
            My.dAngleSetPF = dAngleSetPF;
            My.dLackMaxAngleSetPF = dLackMaxAngleSetPF;

            My.iDynthresholdDarkPF2 = iDynthresholdDarkPF2;
            My.iDynthresholdLightPF2 = iDynthresholdLightPF2;
            My.iGraythresholdBlackPF2 = iGraythresholdBlackPF2;
            My.iGraythresholdWhitePF2 = iGraythresholdWhitePF2;
            My.iCloseWidthPF2 = iCloseWidthPF2;
            My.iCloseHeightPF2 = iCloseHeightPF2;
            My.iOpenWidthPF2 = iOpenWidthPF2;
            My.iOpenHeightPF2 = iOpenHeightPF2;
            My.iUnderSizeAreaPF2 = iUnderSizeAreaPF2;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "GraythresholdBlackPF", My.dGraythresholdBlackPF.ToString(), Path);
            IniFile.Write("Setting", "GraythresholdWhitePF", My.dGraythresholdWhitePF.ToString(), Path);
            IniFile.Write("Setting", "UnderSizeAreaPF", My.dUnderSizeAreaPF.ToString(), Path);
            IniFile.Write("Setting", "AngleSetPF", My.dAngleSetPF.ToString(), Path);
            IniFile.Write("Setting", "LackMaxAngleSetPF", My.dLackMaxAngleSetPF.ToString(), Path);

            IniFile.Write("Setting", "DynthresholdDarkPF2", My.iDynthresholdDarkPF2.ToString(), Path);
            IniFile.Write("Setting", "DynthresholdLightPF2", My.iDynthresholdLightPF2.ToString(), Path);
            IniFile.Write("Setting", "GraythresholdBlackPF2", My.iGraythresholdBlackPF2.ToString(), Path);
            IniFile.Write("Setting", "GraythresholdWhitePF2", My.iGraythresholdWhitePF2.ToString(), Path);
            IniFile.Write("Setting", "CloseWidthPF2", My.iCloseWidthPF2.ToString(), Path);
            IniFile.Write("Setting", "CloseHeightPF2", My.iCloseHeightPF2.ToString(), Path);
            IniFile.Write("Setting", "OpenWidthPF2", My.iOpenWidthPF2.ToString(), Path);
            IniFile.Write("Setting", "OpenHeightPF2", My.iOpenHeightPF2.ToString(), Path);
            IniFile.Write("Setting", "UnderSizeAreaPF2", My.iUnderSizeAreaPF2.ToString(), Path);
        }

        private void btnDrawMark_Click(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            try
            {
                //自己創建模板找出mark點
                HOperatorSet.DrawRectangle2(hv_ExpDefaultWinHandle, out hv_Row10, out hv_Column10,out hv_Phi10, out hv_Length10, out hv_Length20);
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row10, hv_Column10, hv_Phi10,hv_Length10, hv_Length20);
                HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
                HOperatorSet.DispObj(ho_Rectangle, hWindowControl1.HalconWindow);
            }
            catch
            {
            }
        }

        private void tbMarkGraythreshold_ValueChanged(object sender, EventArgs e)
        {
            nudMarkGraythreshold.Value = tbMarkGraythreshold.Value;
        }

        private void nudMarkGraythreshold_ValueChanged(object sender, EventArgs e)
        {
            dMarkGraythreshold = tbMarkGraythreshold.Value = Convert.ToInt32(nudMarkGraythreshold.Value);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced00);
            HOperatorSet.GenEmptyObj(out ho_Region00);
            try
            {
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                ho_ImageReduced00.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced00);
                hv_gray00 = dMarkGraythreshold;
                ho_Region00.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced00, out ho_Region00,0 ,hv_gray00);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                HOperatorSet.DispObj(ho_Region00, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
        }

        private void btnCreateMarkModule_Click(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_ImageReduced00);
            HOperatorSet.GenEmptyObj(out ho_Region00);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing00);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions00);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions00);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation00);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            try
            {
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);

                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row10, hv_Column10, hv_Phi10,
                    hv_Length10, hv_Length20);
                ho_ImageReduced00.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced00);
                hv_gray00 = dMarkGraythreshold;
                hv_GMax = hv_gray00 + 1;
                hv_GMin = hv_gray00.Clone();
                hv_Mult = 255 / (hv_GMax - hv_GMin);
                hv_add = (-hv_Mult) * hv_GMin;
                ho_ImageScaled.Dispose();
                HOperatorSet.ScaleImage(ho_ImageReduced00, out ho_ImageScaled, hv_Mult, hv_add);

                ho_Region00.Dispose();
                HOperatorSet.Threshold(ho_ImageScaled, out ho_Region00, 0, 1);
                ho_RegionClosing00.Dispose();
                HOperatorSet.ClosingCircle(ho_Region00, out ho_RegionClosing00, 3.5);
                ho_ConnectedRegions00.Dispose();
                HOperatorSet.Connection(ho_RegionClosing00, out ho_ConnectedRegions00);
                HOperatorSet.AreaCenter(ho_ConnectedRegions00, out hv_Area00, out hv_Row00,
                    out hv_Column00);
                ho_SelectedRegions00.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions00, out ho_SelectedRegions00, "area",
                    "and", hv_Area00.TupleMax(), 999999);
                ho_RegionDilation00.Dispose();
                HOperatorSet.DilationCircle(ho_SelectedRegions00, out ho_RegionDilation00,
                    20);
                ho_ImageReduced00.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageScaled, ho_RegionDilation00, out ho_ImageReduced00
                    );
                HOperatorSet.CreateNccModel(ho_ImageReduced00, 3, (new HTuple(0)).TupleRad()
                    , (new HTuple(360)).TupleRad(), "auto", "use_polarity", out hv_ModelID00);
                HOperatorSet.AreaCenter(ho_RegionDilation00, out hv_ModelRegionArea, out hv_RefRow,
                    out hv_RefColumn);
                ho_RegionDilation00.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_RegionDilation00, hv_RefRow, hv_RefColumn,
                    20, 0.0);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_ImageScaled, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_RegionDilation00, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_RegionDilation00, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
        }
      
        private void nudMarkGrade_ValueChanged(object sender, EventArgs e)
        {
            dMarkGrade = (double)nudMarkGrade.Value;
        }

        

        private void btnSaveModule_Click(object sender, EventArgs e)
        {
            try
            {
                My.dMarkGraythreshold = dMarkGraythreshold;
                My.dMarkGrade = dMarkGrade;
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
                IniFile.Write("DirectionSetting", "MarkGraythreshold", My.dMarkGraythreshold.ToString(), Path);
                IniFile.Write("DirectionSetting", "MarkGrade", My.dMarkGrade.ToString(), Path);

                if (!Directory.Exists(Sys.ModulePath))
                {
                    Directory.CreateDirectory(Sys.ModulePath);
                }
                HOperatorSet.WriteNccModel(hv_ModelID00, Sys.ModulePath + "\\" + Sys.Function + "_" + Production.CurProduction + "Module");
                disp_message(hv_ExpDefaultWinHandle, "儲存成功", "window", 24, 24, "black", "true");
            }
            catch
            {
                disp_message(hv_ExpDefaultWinHandle, "請勿重複儲存", "window", 24, 24, "black", "true");
            }
        }

        private void cbTestQuadrant1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTestQuadrant1.Checked)
            {   
                My.iTestQuadrant = 1;
                cbTestQuadrant2.Checked = false;
                cbTestQuadrant3.Checked = false;
                cbTestQuadrant4.Checked = false;
                cbTestQuadrant5.Checked = false;
                cbTestQuadrant6.Checked = false;
                cbTestQuadrant7.Checked = false;
                cbTestQuadrant8.Checked = false;
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
                IniFile.Write("DirectionSetting", "TestQuadrant", My.iTestQuadrant.ToString(), Path);
                HOperatorSet.GenEmptyObj(out ho_Sector);
                try
                {
                    if (My.ho_Image == null)
                        return;
                    ho_Image.Dispose();
                    HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                    ho_Sector.Dispose();
                    gen_sector(out ho_Sector, My.dResultRow, My.dResultColumn, dMarkID, dMarkTD, "Up");

                    hWindowControl1.HalconWindow.ClearWindow();
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.DispObj(ho_Sector, hv_ExpDefaultWinHandle);
                }
                catch
                {
                }
            }
            
        }

        private void cbTestQuadrant2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTestQuadrant2.Checked)
            {
                My.iTestQuadrant = 2;
                cbTestQuadrant1.Checked = false;
                cbTestQuadrant3.Checked = false;
                cbTestQuadrant4.Checked = false;
                cbTestQuadrant5.Checked = false;
                cbTestQuadrant6.Checked = false;
                cbTestQuadrant7.Checked = false;
                cbTestQuadrant8.Checked = false;
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
                IniFile.Write("DirectionSetting", "TestQuadrant", My.iTestQuadrant.ToString(), Path);
                HOperatorSet.GenEmptyObj(out ho_Sector);
                try
                {
                    if (My.ho_Image == null)
                        return;
                    ho_Image.Dispose();
                    HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                    ho_Sector.Dispose();
                    gen_sector(out ho_Sector, My.dResultRow, My.dResultColumn, dMarkID, dMarkTD, "Left");

                    hWindowControl1.HalconWindow.ClearWindow();
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.DispObj(ho_Sector, hv_ExpDefaultWinHandle);
                }
                catch
                {
                }
            }
        }

        private void cbTestQuadrant3_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTestQuadrant3.Checked)
            {
                My.iTestQuadrant = 3;
                cbTestQuadrant1.Checked = false;
                cbTestQuadrant2.Checked = false;
                cbTestQuadrant4.Checked = false;
                cbTestQuadrant5.Checked = false;
                cbTestQuadrant6.Checked = false;
                cbTestQuadrant7.Checked = false;
                cbTestQuadrant8.Checked = false;
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
                IniFile.Write("DirectionSetting", "TestQuadrant", My.iTestQuadrant.ToString(), Path);
                HOperatorSet.GenEmptyObj(out ho_Sector);
                try
                {
                    if (My.ho_Image == null)
                        return;
                    ho_Image.Dispose();
                    HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                    ho_Sector.Dispose();
                    gen_sector(out ho_Sector, My.dResultRow, My.dResultColumn, dMarkID, dMarkTD, "Right");

                    hWindowControl1.HalconWindow.ClearWindow();
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.DispObj(ho_Sector, hv_ExpDefaultWinHandle);
                }
                catch
                {
                }
            }
        }

        private void cbTestQuadrant4_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTestQuadrant4.Checked)
            {
                My.iTestQuadrant = 4;
                cbTestQuadrant1.Checked = false;
                cbTestQuadrant2.Checked = false;
                cbTestQuadrant3.Checked = false;
                cbTestQuadrant5.Checked = false;
                cbTestQuadrant6.Checked = false;
                cbTestQuadrant7.Checked = false;
                cbTestQuadrant8.Checked = false;
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
                IniFile.Write("DirectionSetting", "TestQuadrant", My.iTestQuadrant.ToString(), Path);
                HOperatorSet.GenEmptyObj(out ho_Sector);
                try
                {
                    if (My.ho_Image == null)
                        return;
                    ho_Image.Dispose();
                    HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                    ho_Sector.Dispose();
                    gen_sector(out ho_Sector, My.dResultRow, My.dResultColumn, dMarkID, dMarkTD, "Down");

                    hWindowControl1.HalconWindow.ClearWindow();
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.DispObj(ho_Sector, hv_ExpDefaultWinHandle);
                }
                catch
                {
                }
            }
        }

        

        private void btnFindDirection_Click(object sender, EventArgs e)
        {
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
            Vision.VisionResult[Tray.n] = "OK";
            FindDirection(hWindowControl1.HalconWindow, ho_Image, Tray.n);
        }

        public void FindDirection(HWindow hWindowControl,HObject theImage,int n)
        {
            if (Vision.VisionResult[n] == "OK")
            {
                HOperatorSet.SetSystem("global_mem_cache", "idle");
                ho_Image = theImage.CopyObj(1,-1);
                hv_ExpDefaultWinHandle = hWindowControl;
                //畫檢視範圍

                ////設定檢測第幾象限
                //switch (My.iTestQuadrant)
                //{
                //    case 1:
                //        {
                //            ho_Sector.Dispose();
                //            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "Up");
                //            break;
                //        }
                //    case 2:
                //        {
                //            ho_Sector.Dispose();
                //            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "Left");
                //            break;
                //        }
                //    case 3:
                //        {
                //            ho_Sector.Dispose();
                //            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "Right");
                //            break;
                //        }
                //    case 4:
                //        {
                //            ho_Sector.Dispose();
                //            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "Down");
                //            break;
                //        }
                //    case 5:
                //        {
                //            ho_Sector.Dispose();
                //            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "UpperLeft");
                //            break;
                //        }
                //    case 6:
                //        {
                //            ho_Sector.Dispose();
                //            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "UpperRight");
                //            break;
                //        }
                //    case 7:
                //        {
                //            ho_Sector.Dispose();
                //            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "LowerLeft");
                //            break;
                //        }
                //    case 8:
                //        {
                //            ho_Sector.Dispose();
                //            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "LowerRight");
                //            break;
                //        }
                //}
                //ho_ImageReduced00.Dispose();
                //HOperatorSet.ReduceDomain(ho_Image, ho_Sector, out ho_ImageReduced00);
                #region 方法一
                if (My.TestDirectionChoice==0)
                {
                    try
                    {
                        
                        hv_GMax = dMarkGraythreshold + 1;
                        hv_GMin = dMarkGraythreshold;
                        hv_Mult = 255 / (hv_GMax - hv_GMin);
                        hv_add = (-hv_Mult) * hv_GMin;
                        ho_ImageScaled.Dispose();
                        HOperatorSet.ScaleImage(ho_ImageReduced00, out ho_ImageScaled, hv_Mult, hv_add);
                        //如果有儲存模板
                        
                        HOperatorSet.FindNccModel(ho_ImageScaled, hv_ModelID00, (new HTuple(0)).TupleRad()
                            , (new HTuple(360)).TupleRad(), My.dMarkGrade/100, 0, 0, "true", 0, out hv_Row, out hv_Column,
                            out hv_Angle, out hv_Score);
                        ho_TransContours.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_TransContours, hv_Row.TupleSelect(0),
                            hv_Column.TupleSelect(0), 40, hv_Angle);
                        //HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "white");
                        //HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                        HOperatorSet.DispObj(ho_TransContours, hv_ExpDefaultWinHandle);
                        set_display_font(hv_ExpDefaultWinHandle, 20, "mono", "true", "false");
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 400, 0);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("匹配分數:" + ((double)hv_Score * 100).ToString("f1")));

                    }
                    catch
                    {
                        set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                        HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 1300);
                        HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("方向NG"));
                        Vision.VisionResult[n] = "NG3";
                        LogResult = "NG3";
                    }
                #endregion
                }
                else if(My.TestDirectionChoice==1)
                {
                    #region 方法二
                    
                    try
                    {
                    ho_ObjectSelected_UpperLeft.Dispose();ho_ObjectSelected_UpperRight.Dispose();ho_ObjectSelected_LowerLeft.Dispose();ho_ObjectSelected_LowerRight.Dispose();
                    find_target_in_four_circle(ho_Image, out ho_ObjectSelected_UpperLeft, out ho_ObjectSelected_UpperRight, 
                        out ho_ObjectSelected_LowerLeft, out ho_ObjectSelected_LowerRight,hv_ModelID2, hv_ModelID3, "UpperLeft", out hv_Result, out hv_Score);
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 200, 0);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Score:"+hv_Score);
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 100);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, hv_Result);
                    }
                    catch
                    {
                    }
                    #endregion
                }
                else if (My.TestDirectionChoice == 2)
                {
                    #region 方法三
                    try
                    {
                        Method3(hWindowControl, ho_Image, n);
                    }
                    catch
                    {
                    }
                    #endregion

                }

                ho_Image.Dispose();
                HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
               
            }
        }

        private void cbTestDirection_CheckedChanged(object sender, EventArgs e)
        {
            My.TestDirection = (cbTestDirection.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "TestDirection", My.TestDirection.ToString(), Path);
        }

        private void btnCircleCenter7_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image);

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

        private void tbMarkTD_ValueChanged(object sender, EventArgs e)
        {
            nudMarkTD.Value = tbMarkTD.Value;
        }

        private void nudMarkTD_ValueChanged(object sender, EventArgs e)
        {
            dMarkTD = tbMarkTD.Value = Convert.ToInt32(nudMarkTD.Value);
            try
            {
                //這段要另外用的
                FixationRing(hWindowControl1.HalconWindow, dMarkID, dMarkTD);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void tbMarkID_ValueChanged(object sender, EventArgs e)
        {
            nudMarkID.Value = tbMarkID.Value;
        }

        private void nudMarkID_ValueChanged(object sender, EventArgs e)
        {
            dMarkID = tbMarkID.Value = Convert.ToInt32(nudMarkID.Value);
            try
            {
                //這段要另外用的
                FixationRing(hWindowControl1.HalconWindow, dMarkID, dMarkTD);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void btnMarkRangeSave_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            My.dMarkTD = dMarkTD;
            My.dMarkID = dMarkID;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("DirectionSetting", "MarkTD", My.dMarkTD.ToString(), Path);
            IniFile.Write("DirectionSetting", "MarkID", My.dMarkID.ToString(), Path);
        }

        private void cbTestQuadrant5_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTestQuadrant5.Checked)
            {
                My.iTestQuadrant = 5;
                cbTestQuadrant1.Checked = false;
                cbTestQuadrant2.Checked = false;
                cbTestQuadrant3.Checked = false;
                cbTestQuadrant4.Checked = false;
                cbTestQuadrant6.Checked = false;
                cbTestQuadrant7.Checked = false;
                cbTestQuadrant8.Checked = false;
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
                IniFile.Write("DirectionSetting", "TestQuadrant", My.iTestQuadrant.ToString(), Path);
                HOperatorSet.GenEmptyObj(out ho_Sector);
                try
                {
                    if (My.ho_Image == null)
                        return;
                    ho_Image.Dispose();
                    HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                    ho_Sector.Dispose();
                    gen_sector(out ho_Sector, My.dResultRow, My.dResultColumn, dMarkID, dMarkTD, "UpperLeft");

                    hWindowControl1.HalconWindow.ClearWindow();
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.DispObj(ho_Sector, hv_ExpDefaultWinHandle);
                }
                catch
                {
                }
            }
        }

        private void cbTestQuadrant6_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTestQuadrant6.Checked)
            {
                My.iTestQuadrant = 6;
                cbTestQuadrant1.Checked = false;
                cbTestQuadrant2.Checked = false;
                cbTestQuadrant3.Checked = false;
                cbTestQuadrant4.Checked = false;
                cbTestQuadrant5.Checked = false;
                cbTestQuadrant7.Checked = false;
                cbTestQuadrant8.Checked = false;
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
                IniFile.Write("DirectionSetting", "TestQuadrant", My.iTestQuadrant.ToString(), Path);
                HOperatorSet.GenEmptyObj(out ho_Sector);
                try
                {
                    if (My.ho_Image == null)
                        return;
                    ho_Image.Dispose();
                    HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                    ho_Sector.Dispose();
                    gen_sector(out ho_Sector, My.dResultRow, My.dResultColumn, dMarkID, dMarkTD, "UpperRight");

                    hWindowControl1.HalconWindow.ClearWindow();
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.DispObj(ho_Sector, hv_ExpDefaultWinHandle);
                }
                catch
                {
                }
            }
        }

        private void cbTestQuadrant7_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTestQuadrant7.Checked)
            {
                My.iTestQuadrant = 7;
                cbTestQuadrant1.Checked = false;
                cbTestQuadrant2.Checked = false;
                cbTestQuadrant3.Checked = false;
                cbTestQuadrant4.Checked = false;
                cbTestQuadrant5.Checked = false;
                cbTestQuadrant6.Checked = false;
                cbTestQuadrant8.Checked = false;
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
                IniFile.Write("DirectionSetting", "TestQuadrant", My.iTestQuadrant.ToString(), Path);
                HOperatorSet.GenEmptyObj(out ho_Sector);
                try
                {
                    if (My.ho_Image == null)
                        return;
                    ho_Image.Dispose();
                    HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                    ho_Sector.Dispose();
                    gen_sector(out ho_Sector, My.dResultRow, My.dResultColumn, dMarkID, dMarkTD, "LowerLeft");

                    hWindowControl1.HalconWindow.ClearWindow();
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.DispObj(ho_Sector, hv_ExpDefaultWinHandle);
                }
                catch
                {
                }
            }
        }

        private void cbTestQuadrant8_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTestQuadrant8.Checked)
            {
                My.iTestQuadrant = 8;
                cbTestQuadrant1.Checked = false;
                cbTestQuadrant2.Checked = false;
                cbTestQuadrant3.Checked = false;
                cbTestQuadrant4.Checked = false;
                cbTestQuadrant5.Checked = false;
                cbTestQuadrant6.Checked = false;
                cbTestQuadrant7.Checked = false;
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
                IniFile.Write("DirectionSetting", "TestQuadrant", My.iTestQuadrant.ToString(), Path);
                HOperatorSet.GenEmptyObj(out ho_Sector);
                try
                {
                    if (My.ho_Image == null)
                        return;
                    ho_Image.Dispose();
                    HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                    ho_Sector.Dispose();
                    gen_sector(out ho_Sector, My.dResultRow, My.dResultColumn, dMarkID, dMarkTD, "LowerRight");

                    hWindowControl1.HalconWindow.ClearWindow();
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.DispObj(ho_Sector, hv_ExpDefaultWinHandle);
                }
                catch
                {
                }
            }
        }

        private void btnDrawInOutSideCircle_Click(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle_ModelTD2);
            HOperatorSet.GenEmptyObj(out ho_Circle_ModelID2);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference_Model2);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced_Model2);
            HOperatorSet.GenEmptyObj(out ho_Region_Model2);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp_Model2);
            HOperatorSet.GenEmptyObj(out ho_Contours_Model2);
            HOperatorSet.GenEmptyObj(out ho_Circle_ModelID2);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference_Model2);
            try
            {
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);

                HOperatorSet.DrawCircle(hv_ExpDefaultWinHandle, out hv_Row_ModelTD2, out hv_Column_ModelTD2,out hv_Radius_ModelTD2);
                HOperatorSet.DrawCircle(hv_ExpDefaultWinHandle, out hv_Row_ModelID2, out hv_Column_ModelID2, out hv_Radius_ModelID2);
                ho_Circle_ModelTD2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_ModelTD2, hv_Row_ModelTD2, hv_Column_ModelTD2, hv_Radius_ModelTD2);
                ho_Circle_ModelID2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_ModelID2, hv_Row_ModelID2, hv_Column_ModelID2, hv_Radius_ModelID2);
                ho_RegionDifference_Model2.Dispose();
                HOperatorSet.Difference(ho_Circle_ModelTD2, ho_Circle_ModelID2, out ho_RegionDifference_Model2);
                ho_ImageReduced_Model2.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference_Model2, out ho_ImageReduced_Model2);
                ho_Region_Model2.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced_Model2, out ho_Region_Model2, 0,80);
                ho_RegionFillUp_Model2.Dispose();
                HOperatorSet.FillUp(ho_Region_Model2, out ho_RegionFillUp_Model2);
                ho_Contours_Model2.Dispose();
                HOperatorSet.GenContourRegionXld(ho_RegionFillUp_Model2, out ho_Contours_Model2, "border");
                HOperatorSet.FitCircleContourXld(ho_Contours_Model2, "ahuber", -1, 0, 0,
                    3, 2, out hv_Row_Model2, out hv_Column_Model2, out hv_Radius_Model2, out hv_StartPhi2, out hv_EndPhi2, out hv_PointOrder2);

                HOperatorSet.CreateShapeModelXld(ho_Contours_Model2, "auto", (new HTuple(0)).TupleRad()
                    , (new HTuple(360)).TupleRad(), "auto", "auto", "ignore_local_polarity",5, out hv_ModelID2);
                HOperatorSet.WriteShapeModel(hv_ModelID2, Sys.ModulePath + "\\" + Sys.Function + "_" + Production.CurProduction + "Module2");
                MessageBox.Show("模組創建成功");
            }
            catch
            {
            }
        }

        private void tbMarkGraythreshold_2_ValueChanged(object sender, EventArgs e)
        {
            nudModuleGraythreshold_2.Value = tbModuleGraythreshold_2.Value;
        }

        private void nudMarkGraythreshold_2_ValueChanged(object sender, EventArgs e)
        {
            dModuleGraythreshold_2 = tbModuleGraythreshold_2.Value = Convert.ToInt32(nudModuleGraythreshold_2.Value);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced00);
            HOperatorSet.GenEmptyObj(out ho_Region00);
            try
            {
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
 
                hv_gray00 = dModuleGraythreshold_2;
                ho_Region00.Dispose();
                HOperatorSet.Threshold(ho_Image, out ho_Region00, 0, hv_gray00);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                HOperatorSet.DispObj(ho_Region00, hv_ExpDefaultWinHandle);
                string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
                IniFile.Write("DirectionSetting", "ModuleGraythreshold_2", My.dModuleGraythreshold_2.ToString(), Path);
            }
            catch
            {
                MessageBox.Show("請開啟圖片或拍照");
            }
        }

        private void nudModuleGrade_2_ValueChanged(object sender, EventArgs e)
        {
            dModuleGrade_2 = (double)nudModuleGrade_2.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("DirectionSetting", "ModuleGrade_2", My.dModuleGrade_2.ToString(), Path);
        }

        private void btnCreateModule_3_Click(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_PolygonRegion_model3);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp_model3);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced_model3);
            try
            {
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);

                ho_PolygonRegion_model3.Dispose();
                HOperatorSet.DrawPolygon(out ho_PolygonRegion_model3, hv_ExpDefaultWinHandle);
                ho_RegionFillUp_model3.Dispose();
                HOperatorSet.FillUp(ho_PolygonRegion_model3, out ho_RegionFillUp_model3);
                ho_ImageReduced_model3.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionFillUp_model3, out ho_ImageReduced_model3);
                HOperatorSet.CreateNccModel(ho_ImageReduced_model3, "auto", (new HTuple(0)).TupleRad()
                    , (new HTuple(360)).TupleRad(), "auto", "ignore_global_polarity", out hv_ModelID3);
                HOperatorSet.DispObj(ho_RegionFillUp_model3, hv_ExpDefaultWinHandle);
                HOperatorSet.WriteNccModel(hv_ModelID3, Sys.ModulePath + "\\" + Sys.Function + "_" + Production.CurProduction + "Module3");
                MessageBox.Show("模組創建成功");
            }
            catch
            {
            }
        }

        private void cbTestDirectionChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTestDirectionChoice.SelectedIndex < 0)
                return;
            string result = (string)cbTestDirectionChoice.SelectedItem;
            int index = 0;
            switch (result)
            {
                case "方法一": index = 0; break;
                case "方法二": index = 1; break;
                case "方法三": index = 2; break;
            }
            cbTestDirectionChoice.SelectedIndex = index;
            My.TestDirectionChoice = index;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "TestDirectionChoice", result, Path);
            tabTestDirectionChoice.SelectedTab = tabTestDirectionChoice.TabPages[My.TestDirectionChoice];
        }

        private void nudModuleGrade_3_ValueChanged(object sender, EventArgs e)
        {
            dModuleGrade_3 = (double)nudModuleGrade_3.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("DirectionSetting", "ModuleGrade_3", My.dModuleGrade_3.ToString(), Path);
        }

        private void cbDarkLightChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDarkLightChoice.SelectedIndex < 0)
                return;
            My.DarkLightChoice = cbDarkLightChoice.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "DarkLightChoice", My.DarkLightChoice.ToString(), Path);
        }

        private void cbDetection_Black_CheckedChanged(object sender, EventArgs e)
        {
            My.Detection_Black = (cbDetection_Black.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "Detection_Black", My.Detection_Black.ToString(), Path);
        }

        private void cbDetection_White_CheckedChanged(object sender, EventArgs e)
        {
            My.Detection_White = (cbDetection_White.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "Detection_White", My.Detection_White.ToString(), Path);
        }

        private void nudGlueAngleSet_ValueChanged(object sender, EventArgs e)
        {
            My.iGlueAngleSet = iGlueAngleSet = Convert.ToInt32(nudGlueAngleSet.Value);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "GlueAngleSet", My.iGlueAngleSet.ToString(), Path);
        }

        private void nudGlueRatioSet_ValueChanged(object sender, EventArgs e)
        {
            My.iGlueRatioSet = iGlueRatioSet = Convert.ToInt32(nudGlueRatioSet.Value);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "GlueRatioSet", My.iGlueRatioSet.ToString(), Path);
        }

        private void nudGlueAngleSetPF_ValueChanged(object sender, EventArgs e)
        {
            My.iGlueAngleSetPF = iGlueAngleSetPF = Convert.ToInt32(nudGlueAngleSetPF.Value);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "GlueAngleSetPF", My.iGlueAngleSetPF.ToString(), Path);
        }

        private void nudGlueRatioSetPF_ValueChanged(object sender, EventArgs e)
        {
            My.iGlueRatioSetPF = iGlueRatioSetPF = Convert.ToInt32(nudGlueRatioSetPF.Value);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "GlueRatioSetPF", My.iGlueRatioSetPF.ToString(), Path);
        }

        public void Clear_Temple()
        {
            try
            {
                HOperatorSet.ClearNccModel(hv_ModelID00);
            }
            catch
            {

            }
        }
        public void Read_Temple()
        {
            if (My.TestDirection)
            {
                if (My.TestDirectionChoice == 0)
                {
                    try
                    {
                        HOperatorSet.ReadNccModel(Sys.ModulePath + "\\" + Sys.Function + "_" + Production.CurProduction + "Module", out hv_ModelID00);
                    }
                    catch
                    {
                        MessageBox.Show("請建立方向模組一");
                    }
                }
                else if (My.TestDirectionChoice == 1)
                {
                    try
                    {
                        HOperatorSet.ReadShapeModel(Sys.ModulePath + "\\" + Sys.Function + "_" + Production.CurProduction + "Module2", out hv_ModelID2);
                    }
                    catch
                    {
                        MessageBox.Show("請建立方向模組二");
                    }
                    try
                    {
                        HOperatorSet.ReadNccModel(Sys.ModulePath + "\\" + Sys.Function + "_" + Production.CurProduction + "Module3", out hv_ModelID3);
                    }
                    catch
                    {
                        MessageBox.Show("請建立方向模組三");
                    }
                }
            }
        }
        private void cbGlueLightDarkChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbGlueLightDarkChoice.SelectedIndex < 0)
                return;
            My.GlueLightDarkChoice = cbGlueLightDarkChoice.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "GlueLightDarkChoice", My.GlueLightDarkChoice.ToString(), Path);
        }

        private void nudDynThresholdSet_ValueChanged(object sender, EventArgs e)
        {
            dDynThresholdSet = nudDynThresholdSet.Value;
            if (My.ho_Image == null)
                return;
            Method2();
        }

        private void cbMethodChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            My.MethodChoice = cbMethodChoice.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "MethodChoice", My.MethodChoice.ToString(), Path);
            tabMethodChoice.SelectedTab = tabMethodChoice.TabPages[My.MethodChoice];
        }

        private void tbUnderSizeArea2_ValueChanged(object sender, EventArgs e)
        {
            nudUnderSizeArea2.Value = tbUnderSizeArea2.Value;
        }

        private void nudUnderSizeArea2_ValueChanged(object sender, EventArgs e)
        {
            iUnderSizeArea2 = tbUnderSizeArea2.Value = (int)nudUnderSizeArea2.Value;
            if (My.ho_Image == null)
                return;
            Method2();
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

        private void cbContrastSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            My.ContrastSet = cbContrastSet.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "ContrastSet", My.ContrastSet.ToString(), Path);
        }
        
        private void tbArea_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudArea_Upper.Value = tbArea_Upper.Value;
        }
        private void nudArea_Upper_ValueChanged(object sender, EventArgs e)
        {
            Area_Upper = tbArea_Upper.Value = (int)nudArea_Upper.Value;
            if (My.ho_Image == null)
                return;
            if (hv_ResultRow.Length == 0)
                MessageBox.Show("請先求圓心!");
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.DispObj(My.ho_Image, hv_ExpDefaultWinHandle);
            Method3(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);
        }

        private void tbArea_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudArea_Lower.Value = tbArea_Lower.Value;
        }

        private void nudArea_Lower_ValueChanged(object sender, EventArgs e)
        {
            Area_Lower = tbArea_Lower.Value = (int)nudArea_Lower.Value;
            if (My.ho_Image == null)
                return;
            if (hv_ResultRow.Length == 0)
                MessageBox.Show("請先求圓心!");
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.DispObj(My.ho_Image, hv_ExpDefaultWinHandle);
            Method3(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);
        }

        private void tbRoundness_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudRoundness_Upper.Value = tbRoundness_Upper.Value;
        }

        private void nudRoundness_Upper_ValueChanged(object sender, EventArgs e)
        {
            Roundness_Upper = tbRoundness_Upper.Value = (int)nudRoundness_Upper.Value;
            if (My.ho_Image == null)
                return;
            if (hv_ResultRow.Length == 0)
                MessageBox.Show("請先求圓心!");
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.DispObj(My.ho_Image, hv_ExpDefaultWinHandle);
            Method3(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);
        }

        private void tbRoundness_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudRoundness_Lower.Value = tbRoundness_Lower.Value;
        }

        private void nudRoundness_Lower_ValueChanged(object sender, EventArgs e)
        {
            Roundness_Lower = tbRoundness_Lower.Value = (int)nudRoundness_Lower.Value;
            if (My.ho_Image == null)
                return;
            if (hv_ResultRow.Length == 0)
                MessageBox.Show("請先求圓心!");
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.DispObj(My.ho_Image, hv_ExpDefaultWinHandle);
            Method3(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);
        }

        private void tbRectangularity_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudRectangularity_Upper.Value = tbRectangularity_Upper.Value;
        }

        private void nudRectangularity_Upper_ValueChanged(object sender, EventArgs e)
        {
            Rectangularity_Upper = tbRectangularity_Upper.Value = (int)nudRectangularity_Upper.Value;
            if (My.ho_Image == null)
                return;
            if (hv_ResultRow.Length == 0)
                MessageBox.Show("請先求圓心!");
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.DispObj(My.ho_Image, hv_ExpDefaultWinHandle);
            Method3(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);
        }

        private void tbRectangularity_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudRectangularity_Lower.Value = tbRectangularity_Lower.Value;
        }

        private void nudRectangularity_Lower_ValueChanged(object sender, EventArgs e)
        {
            Rectangularity_Lower = tbRectangularity_Lower.Value = (int)nudRectangularity_Lower.Value;
            if (My.ho_Image == null)
                return;
            if (hv_ResultRow.Length == 0)
                MessageBox.Show("請先求圓心!");
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.DispObj(My.ho_Image, hv_ExpDefaultWinHandle);
            Method3(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);
        }

        private void btnMethodSave_3_Click(object sender, EventArgs e)
        {
            My.Area_Upper = Area_Upper;
            My.Area_Lower = Area_Lower;
            My.Roundness_Upper = Roundness_Upper;
            My.Roundness_Lower = Roundness_Lower;
            My.Rectangularity_Upper = Rectangularity_Upper;
            My.Rectangularity_Lower = Rectangularity_Lower;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "GraySet", My.GraySet.ToString(), Path);
            IniFile.Write("Setting", "Area_Upper", My.Area_Upper.ToString(), Path);
            IniFile.Write("Setting", "Area_Lower", My.Area_Lower.ToString(), Path);
            IniFile.Write("Setting", "Rect2_Len1_Lower", My.Rect2_Len1_Lower.ToString(), Path);
            IniFile.Write("Setting", "Rect2_Len1_Upper", My.Rect2_Len1_Upper.ToString(), Path);
            IniFile.Write("Setting", "Rect2_Len2_Lower", My.Rect2_Len2_Lower.ToString(), Path);
            IniFile.Write("Setting", "Rect2_Len2_Upper", My.Rect2_Len2_Upper.ToString(), Path);
            IniFile.Write("Setting", "Roundness_Upper", My.Roundness_Upper.ToString(), Path);
            IniFile.Write("Setting", "Roundness_Lower", My.Roundness_Lower.ToString(), Path);
            IniFile.Write("Setting", "Rectangularity_Upper", My.Rectangularity_Upper.ToString(), Path);
            IniFile.Write("Setting", "Rectangularity_Lower", My.Rectangularity_Lower.ToString(), Path);
            IniFile.Write("Setting", "ChoiceMax", My.bChoiceMax.ToString(), Path);
        }

        public void Method3(HWindow hWindowControl,HObject theImage,int n)
        {
            HObject ho_Image = new HObject();
            HObject ho_SelectedRegions5 = new HObject();
            HObject ho_Sector = new HObject();
            hv_Area_Upper = Area_Upper;
            hv_Area_Lower = Area_Lower;
            hv_Roundness_Upper = Roundness_Upper*0.01;
            hv_Roundness_Lower = Roundness_Lower*0.01;
            hv_Rectangularity_Upper = Rectangularity_Upper*0.01;
            hv_Rectangularity_Lower = Rectangularity_Lower*0.01;
            try
            {
                if (theImage == null)
                    return;

                HOperatorSet.CopyImage(theImage, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl;
                HObject ho_ImageMedian = new HObject(), ho_ImageEmphasize = new HObject();
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, dMarkID);
                //外圈
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, dMarkTD);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);

                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianRect(ho_ImageReduced, out ho_ImageMedian, 10, 10);
                ho_ImageEmphasize.Dispose();
                HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);

                //ho_ImageMean2.Dispose();
                //HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean2, 3, 3);
                //ho_ImageMean3.Dispose();
                //HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean3, 30, 30);
                //ho_RegionDynThresh1.Dispose();
                //HOperatorSet.DynThreshold(ho_ImageMean2, ho_ImageMean3, out ho_RegionDynThresh1, hv_Contrast_Value, hv_contrast_Set);
                //{
                //    HObject ExpTmpOutVar_0;
                //    HOperatorSet.Connection(ho_RegionDynThresh1, out ExpTmpOutVar_0);
                //    ho_RegionDynThresh1.Dispose();
                //    ho_RegionDynThresh1 = ExpTmpOutVar_0;
                //}
                if (My.ContrastSet == 0)
                    HOperatorSet.Threshold(ho_ImageEmphasize, out ho_RegionDynThresh1, My.GraySet, 255);
                else
                    HOperatorSet.Threshold(ho_ImageEmphasize, out ho_RegionDynThresh1, 0, My.GraySet);
                ho_ConnectedRegions5.Dispose();
                HOperatorSet.Connection(ho_RegionDynThresh1, out ho_ConnectedRegions5);
                ho_RegionFillUp7.Dispose();
                HOperatorSet.FillUp(ho_ConnectedRegions5, out ho_RegionFillUp7);
                ho_SelectedRegions6.Dispose();
                ho_SelectedRegions6 = ho_RegionFillUp7.CopyObj(1, -1);
                if (My.DetectionArea)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions6, out ExpTmpOutVar_0, "area", "and", hv_Area_Lower, hv_Area_Upper);
                    ho_SelectedRegions6.Dispose();
                    ho_SelectedRegions6 = ExpTmpOutVar_0;
                }
                if (My.DetectionRect2_Len1)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions6, out ExpTmpOutVar_0, "rect2_len1", "and", My.Rect2_Len1_Lower, My.Rect2_Len1_Upper);
                    ho_SelectedRegions6.Dispose();
                    ho_SelectedRegions6 = ExpTmpOutVar_0;
                }
                if (My.DetectionRect2_Len2)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions6, out ExpTmpOutVar_0, "rect2_len2", "and", My.Rect2_Len2_Lower, My.Rect2_Len2_Upper);
                    ho_SelectedRegions6.Dispose();
                    ho_SelectedRegions6 = ExpTmpOutVar_0;
                }
                if (My.DetectionRoundness)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions6, out ExpTmpOutVar_0, "roundness", "and", hv_Roundness_Lower, hv_Roundness_Upper);
                    ho_SelectedRegions6.Dispose();
                    ho_SelectedRegions6 = ExpTmpOutVar_0;
                }
                if (My.DetectionRectangularity)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions6, out ExpTmpOutVar_0, "rectangularity", "and", hv_Rectangularity_Lower, hv_Rectangularity_Upper);
                    ho_SelectedRegions6.Dispose();
                    ho_SelectedRegions6 = ExpTmpOutVar_0;
                }
                HOperatorSet.AreaCenter(ho_SelectedRegions6, out hv_Area, out hv_Row, out hv_Column);
                if (My.bChoiceMax)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions6, out ExpTmpOutVar_0, "area", "and", hv_Area.TupleMax(), hv_Area.TupleMax() + 1);
                    ho_SelectedRegions6.Dispose();
                    ho_SelectedRegions6 = ExpTmpOutVar_0;
                }
                else
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Union1(ho_SelectedRegions6, out ExpTmpOutVar_0);
                        ho_SelectedRegions6.Dispose();
                    ho_SelectedRegions6 = ExpTmpOutVar_0;
                }
                //設定檢測第幾象限
                switch (My.iTestQuadrant)
                {
                    case 1:
                        {
                            ho_Sector.Dispose();
                            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "Up");
                            break;
                        }
                    case 2:
                        {
                            ho_Sector.Dispose();
                            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "Left");
                            break;
                        }
                    case 3:
                        {
                            ho_Sector.Dispose();
                            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "Right");
                            break;
                        }
                    case 4:
                        {
                            ho_Sector.Dispose();
                            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "Down");
                            break;
                        }
                    case 5:
                        {
                            ho_Sector.Dispose();
                            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "UpperLeft");
                            break;
                        }
                    case 6:
                        {
                            ho_Sector.Dispose();
                            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "UpperRight");
                            break;
                        }
                    case 7:
                        {
                            ho_Sector.Dispose();
                            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "LowerLeft");
                            break;
                        }
                    case 8:
                        {
                            ho_Sector.Dispose();
                            gen_sector(out ho_Sector, hv_ResultRow, hv_ResultColumn, dMarkID, dMarkTD, "LowerRight");
                            break;
                        }
                }
                //查看交集
                ho_SelectedRegions5.Dispose();
                HOperatorSet.Intersection(ho_SelectedRegions6, ho_Sector, out ho_SelectedRegions5);
                HOperatorSet.AreaCenter(ho_SelectedRegions5, out hv_Area0, out hv_Row, out hv_Column);
                set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                if (hv_Area0.D > 0)
                {
                    //HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.SetColor(hWindowControl, "green");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, hv_Height - 500, 200);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("方向OK"));
                    Vision.VisionResult[n] = "OK";
                    LogResult = "OK";
                    HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                    HOperatorSet.DispObj(ho_SelectedRegions5, hv_ExpDefaultWinHandle);
                }
                else
                {
                    //HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.SetColor(hWindowControl, "red");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, hv_Height - 500, 200);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("方向NG"));
                    Vision.VisionResult[n] = "NG3";
                    LogResult = "NG3";
                    HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                    if (hv_Area.D > 0)
                        HOperatorSet.DispObj(ho_SelectedRegions6, hv_ExpDefaultWinHandle);
                }
            }
            catch
            {
                //HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hWindowControl, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 200);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("方向NG"));
                Vision.VisionResult[n] = "NG3";
                LogResult = "NG3";
            }
            finally
            {
                ho_Image.Dispose();
                ho_Circle1.Dispose();
                ho_Circle2.Dispose();
                ho_RegionDifference.Dispose();
                ho_Circle.Dispose();
                ho_ReducedImage.Dispose();
                ho_RegionDynThresh1.Dispose();
                ho_RegionFillUp7.Dispose();
                ho_SelectedRegions6.Dispose();
                ho_Sector.Dispose();
                ho_SelectedRegions5.Dispose();
            }
        }

        private void cbDetectionArea_CheckedChanged(object sender, EventArgs e)
        {
            My.DetectionArea = cbDetectionArea.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "DetectionArea", My.DetectionArea.ToString(), Path);
        }

        private void cbDetectionRoundness_CheckedChanged(object sender, EventArgs e)
        {
            My.DetectionRoundness = cbDetectionRoundness.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "DetectionRoundness", My.DetectionRoundness.ToString(), Path);
        }

        private void cbDetectionRectangularity_CheckedChanged(object sender, EventArgs e)
        {
            My.DetectionRectangularity = cbDetectionRectangularity.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "DetectionRectangularity", My.DetectionRectangularity.ToString(), Path);
        }

        private void nudMeanSet_1_ValueChanged(object sender, EventArgs e)
        {
            hv_MeanWidth_1 = dMeanWidth_1 = (double)nudMeanWidth_1.Value;
            hv_MeanHeight_1 = dMeanHeight_1 = (double)nudMeanHeight_1.Value;
            if (My.ho_Image == null)
                return;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            try
            {
                hv_CircleRadius1 = dRingInRange;
                hv_CircleRadius2 = dRingOutRange;

                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadius1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadius2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                ho_PolarTransImage.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImage, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadius1, hv_CircleRadius2,
            hv_CircleRadius2 * 2 * Math.PI, hv_CircleRadius2 - hv_CircleRadius1, "nearest_neighbor");
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImage, out ho_ImageMean1, hv_MeanWidth_1, hv_MeanHeight_1);
                ho_XYTransRegion.Dispose();
                HOperatorSet.PolarTransImageInv(ho_ImageMean1, out ho_XYTransRegion, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadius1, hv_CircleRadius2, hv_Width,
            hv_Height, "nearest_neighbor");
                HOperatorSet.DispObj(ho_XYTransRegion, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
            finally
            {
                ho_Image.Dispose();
                ho_Circle1.Dispose();
                ho_Circle2.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_PolarTransImage.Dispose();
                ho_ImageMean1.Dispose();
                ho_XYTransRegion.Dispose();
            }
        }

        private void nudMeanSet_2_ValueChanged(object sender, EventArgs e)
        {
            hv_MeanWidth_2 = dMeanWidth_2 = (double)nudMeanWidth_2.Value;
            hv_MeanHeight_2 = dMeanHeight_2 = (double)nudMeanHeight_2.Value;
            if (My.ho_Image == null)
                return;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image,out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            try
            {
                hv_CircleRadius1 = dRingInRange;
                hv_CircleRadius2 = dRingOutRange;

                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadius1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadius2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                ho_PolarTransImage.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImage, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadius1, hv_CircleRadius2,
            hv_CircleRadius2, hv_CircleRadius2 - hv_CircleRadius1, "nearest_neighbor");
                ho_ImageMean2.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImage, out ho_ImageMean2, hv_MeanWidth_2, hv_MeanHeight_2);
                ho_XYTransRegion.Dispose();
                HOperatorSet.PolarTransImageInv(ho_ImageMean2, out ho_XYTransRegion, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadius1, hv_CircleRadius2, hv_Width,
            hv_Height, "nearest_neighbor");
                HOperatorSet.DispObj(ho_XYTransRegion, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
            finally
            {
                ho_Image.Dispose();
                ho_Circle1.Dispose();
                ho_Circle2.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_PolarTransImage.Dispose();
                ho_ImageMean1.Dispose();
                ho_XYTransRegion.Dispose();
            }
        }

        private void nudCloseOpenSet_ValueChanged(object sender, EventArgs e)
        {
            dCloseWidthValue = (double)nudCloseWidth.Value;
            dCloseHeightValue = (double)nudCloseHeight.Value;
            dOpenWidthValue = (double)nudOpenWidth.Value;
            dOpenHeightValue = (double)nudOpenHeight.Value;
            if (My.ho_Image == null)
                return;
            Method2();
        }

        private void cbClosing_CheckedChanged(object sender, EventArgs e)
        {
            My.Closing = cbClosing.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "Closing", My.Closing.ToString(), Path);
        }

        private void cbOpening_CheckedChanged(object sender, EventArgs e)
        {
            My.Opening = cbOpening.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "Opening", My.Opening.ToString(), Path);
        }

        private void cbFilterWidth_CheckedChanged(object sender, EventArgs e)
        {
            My.FilterWidth = cbFilterWidth.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "FilterWidth", My.FilterWidth.ToString(), Path);
        }

        private void cbFilterHeight_CheckedChanged(object sender, EventArgs e)
        {
            My.FilterHeight = cbFilterHeight.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "FilterHeight", My.FilterHeight.ToString(), Path);
        }

        private void tbFilterWidth_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudFilterWidth_Upper.Value = tbFilterWidth_Upper.Value;
        }

        private void nudFilterWidth_Upper_ValueChanged(object sender, EventArgs e)
        {
            dFilterWidth_Upper = tbFilterWidth_Upper.Value = (int)nudFilterWidth_Upper.Value;
            Method2();
        }

        private void tbFilterWidth_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudFilterWidth_Lower.Value = tbFilterWidth_Lower.Value;
        }

        private void nudFilterWidth_Lower_ValueChanged(object sender, EventArgs e)
        {
            dFilterWidth_Lower = tbFilterWidth_Lower.Value = (int)nudFilterWidth_Lower.Value;
            Method2();
        }

        private void tbFilterHeight_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudFilterHeight_Upper.Value = tbFilterHeight_Upper.Value;
        }

        private void nudFilterHeight_Upper_ValueChanged(object sender, EventArgs e)
        {
            dFilterHeight_Upper = tbFilterHeight_Upper.Value = (int)nudFilterHeight_Upper.Value;
            Method2();
        }

        private void tbFilterHeight_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudFilterHeight_Lower.Value = tbFilterHeight_Lower.Value;
        }

        private void nudFilterHeight_Lower_ValueChanged(object sender, EventArgs e)
        {
            dFilterHeight_Lower = tbFilterHeight_Lower.Value = (int)nudFilterHeight_Lower.Value;
            Method2();
        }

        public void Method2()
        {
            if(My.ho_Image==null)
                return;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image,out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            try
            {
                hv_CircleRadius1 = dRingInRange;
                hv_CircleRadius2 = dRingOutRange;

                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadius1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadius2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                ho_PolarTransImage.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImage, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadius1, hv_CircleRadius2,
                    hv_CircleRadius2*2*Math.PI, hv_CircleRadius2 - hv_CircleRadius1, "nearest_neighbor");

                hv_MeanWidth_1 = (HTuple)dMeanWidth_1;
                hv_MeanHeight_1 = (HTuple)dMeanHeight_1;
                hv_MeanWidth_2 = (HTuple)dMeanWidth_2;
                hv_MeanHeight_2 = (HTuple)dMeanHeight_2;
                if (My.GlueLightDarkChoice == 0)
                    hv_GlueLightDarkChoice = "light";
                else
                    hv_GlueLightDarkChoice = "dark";
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImage, out ho_ImageMean1, hv_MeanWidth_1, hv_MeanHeight_1);
                ho_ImageMean2.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImage, out ho_ImageMean2, hv_MeanWidth_2, hv_MeanHeight_2);

                //dyn_threshold (ImageMean1, ImageMean, RegionDynThresh, 5, GlueLightDarkChoice)
                ho_RegionDynThresh.Dispose();
                HOperatorSet.DynThreshold(ho_ImageMean1, ho_ImageMean2, out ho_RegionDynThresh, (HTuple)dDynThresholdSet, hv_GlueLightDarkChoice);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.FillUp(ho_RegionDynThresh, out ExpTmpOutVar_0);
                    ho_RegionDynThresh.Dispose();
                    ho_RegionDynThresh = ExpTmpOutVar_0;
                }
                hv_closeWidthValue = (HTuple)dCloseWidthValue;
                hv_closeHeightValue = (HTuple)dCloseHeightValue;
                if (My.Closing)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ClosingRectangle1(ho_RegionDynThresh, out ExpTmpOutVar_0, hv_closeWidthValue, hv_closeHeightValue);
                        ho_RegionDynThresh.Dispose();
                        ho_RegionDynThresh = ExpTmpOutVar_0;
                    }
                }

                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.FillUp(ho_RegionDynThresh, out ExpTmpOutVar_0);
                    ho_RegionDynThresh.Dispose();
                    ho_RegionDynThresh = ExpTmpOutVar_0;
                }
                hv_OpenHeightValue = (HTuple)dOpenHeightValue;
                hv_OpenWidthValue = (HTuple)dOpenWidthValue;
                if (My.Opening)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.OpeningRectangle1(ho_RegionDynThresh, out ExpTmpOutVar_0, hv_OpenWidthValue, hv_OpenHeightValue);
                        ho_RegionDynThresh.Dispose();
                        ho_RegionDynThresh = ExpTmpOutVar_0;
                    }
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_RegionDynThresh, out ExpTmpOutVar_0);
                    ho_RegionDynThresh.Dispose();
                    ho_RegionDynThresh = ExpTmpOutVar_0;
                }
                hv_UnderSizeArea2 = iUnderSizeArea2;
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_RegionDynThresh, out ExpTmpOutVar_0, "area", "and", hv_UnderSizeArea2, 99999999);
                    ho_RegionDynThresh.Dispose();
                    ho_RegionDynThresh = ExpTmpOutVar_0;
                }
                if (My.FilterHeight)
                {
                    hv_Height_Upper = (HTuple)dFilterHeight_Upper;
                    hv_Height_Lower = (HTuple)dFilterHeight_Lower;
                    ho_SelectedRegionsA.Dispose();
                    HOperatorSet.SelectShape(ho_RegionDynThresh, out ho_SelectedRegionsA, "height", "and", hv_Height_Lower, hv_Height_Upper);
                    ho_RegionDynThresh.Dispose();
                    ho_RegionDynThresh = ho_SelectedRegionsA.CopyObj(1, -1);
                }
                if (My.FilterWidth)
                {
                    hv_Width_Upper = (HTuple)dFilterWidth_Upper;
                    hv_Width_Lower = (HTuple)dFilterWidth_Lower;
                    ho_SelectedRegionsB.Dispose();
                    HOperatorSet.SelectShape(ho_RegionDynThresh, out ho_SelectedRegionsB, "width", "and", hv_Width_Lower, hv_Width_Upper);
                    ho_RegionDynThresh.Dispose();
                    ho_RegionDynThresh = ho_SelectedRegionsB.CopyObj(1, -1);
                }
                if (My.FilterWidth && My.FilterHeight)
                {
                    ho_RegionDynThresh.Dispose();
                    HOperatorSet.Union2(ho_SelectedRegionsA, ho_SelectedRegionsB, out ho_RegionDynThresh);
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Union1(ho_RegionDynThresh, out ExpTmpOutVar_0);
                    ho_RegionDynThresh.Dispose();
                    ho_RegionDynThresh = ExpTmpOutVar_0;
                }
                //polar_trans_region_inv (RegionDynThresh, XYTransRegion, ResultRow, ResultColumn, 0, 6.28319, CircleRadius1, CircleRadius2, 512, 512, Width, Height, 'nearest_neighbor')
                ho_XYTransRegion.Dispose();
                HOperatorSet.PolarTransRegionInv(ho_RegionDynThresh, out ho_XYTransRegion, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadius1, hv_CircleRadius2,
                    hv_CircleRadius2 * 2 * Math.PI, hv_CircleRadius2 - hv_CircleRadius1, hv_Width, hv_Height, "nearest_neighbor");
                
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");   
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_XYTransRegion, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
            finally
            {

            }
        }

        private void cbDecisionMethodChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            My.DecisionMethodChoice = cbDecisionMethodChoice.SelectedIndex;
            tabDecisionMethodChoice.SelectedTab = tabDecisionMethodChoice.TabPages[My.DecisionMethodChoice];
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "DecisionMethodChoice", My.DecisionMethodChoice.ToString(), Path);
        }

        private void nudRegionDistance_ValueChanged(object sender, EventArgs e)
        {
            My.dRegionDistance = (double)nudRegionDistance.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "DecisionMethodChoice", My.DecisionMethodChoice.ToString(), Path);
        }

        private void nudGlueCountSet_ValueChanged(object sender, EventArgs e)
        {
            My.iGlueCount = (int)nudGlueCountSet.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "GlueCount", My.iGlueCount.ToString(), Path);
        }

        private void cbMethodChoice2_SelectedIndexChanged(object sender, EventArgs e)
        {
            My.MethodChoice2 = cbMethodChoice2.SelectedIndex;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "MethodChoice2", My.MethodChoice2.ToString(), Path);
            tabMethodChoice2.SelectedTab = tabMethodChoice2.TabPages[My.MethodChoice2];
        }

        private void cbDetectionPF_Dark2_CheckedChanged(object sender, EventArgs e)
        {
            My.DetectionPF_Dark2 = (cbDetectionPF_Dark2.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "DetectionPF_Dark2", My.DetectionPF_Dark2.ToString(), Path);
        }

        private void cbDetectionPF_Light2_CheckedChanged(object sender, EventArgs e)
        {
            My.DetectionPF_Light2 = (cbDetectionPF_Light2.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "DetectionPF_Light2", My.DetectionPF_Light2.ToString(), Path);
        }

        private void nudDynthresholdDarkPF2_ValueChanged(object sender, EventArgs e)
        {
            iDynthresholdDarkPF2 = (int)nudDynthresholdDarkPF2.Value;
            if (My.ho_Image == null)
                return;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;

            try
            {
                if (My.DetectionPF2_Black || My.DetectionPF2_White)
                {
                    if (My.dOutRangePF2 > My.dOutRangePF)
                        hv_CircleRadiusPF2 = My.dOutRangePF2 + 2;
                    else
                        hv_CircleRadiusPF2 = My.dOutRangePF + 2;
                    if (My.dInRangePF2 > My.dInRangePF)
                        hv_CircleRadiusPF1 = My.dInRangePF - 2;
                    else
                        hv_CircleRadiusPF1 = My.dInRangePF2 - 2;
                }
                else
                {
                    hv_CircleRadiusPF2 = My.dOutRangePF + 2;
                    hv_CircleRadiusPF1 = My.dInRangePF - 2;
                }
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                //dev_set_draw ('fill')
                //方法二
                ho_PolarTransImagePF.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImagePF, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, "nearest_neighbor");
               
                hv_MeanWidth_1 = 1;
                hv_MeanHeight_1 = 1;
                hv_MeanWidth_2 = 1;
                hv_MeanHeight_2 = (hv_CircleRadiusPF2 - hv_CircleRadiusPF1)/2;
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.MedianRect(ho_PolarTransImagePF, out ExpTmpOutVar_0, 50, 2);
                    ho_PolarTransImagePF.Dispose();
                    ho_PolarTransImagePF = ExpTmpOutVar_0;
                }
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF1, hv_MeanWidth_1, hv_MeanHeight_1);
                ho_ImageMean2.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF2, hv_MeanWidth_2, hv_MeanHeight_2);
                if (My.DetectionPF_Light2 && My.DetectionPF_Dark2)
                {
                    ho_RegionDynThreshPF2.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF2, iDynthresholdLightPF2, "light");
                    ho_RegionDynThreshPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF1, iDynthresholdDarkPF2, "dark");
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.Union2(ho_RegionDynThreshPF1, ho_RegionDynThreshPF2, out ho_RegionUnionPF1);
                }
                else if (My.DetectionPF_Dark2)
                {
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, iDynthresholdDarkPF2, "dark");
                }
                else if (My.DetectionPF_Light2)
                {
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, iDynthresholdLightPF2, "light");
                }
                ho_RectanglePF2.Dispose();
                HOperatorSet.GenRectangle1(out ho_RectanglePF2, 2, 0, dOutRangePF - dInRangePF + 2, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                ho_RegionInterSectionPF2.Dispose();
                HOperatorSet.Intersection(ho_RegionUnionPF1, ho_RectanglePF2, out ho_RegionInterSectionPF2);
                       
                ho_XYTransRegionPF2.Dispose();
                HOperatorSet.PolarTransRegionInv(ho_RegionInterSectionPF2, out ho_XYTransRegionPF2, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_XYTransRegionPF2, hv_ExpDefaultWinHandle);

            }
            catch
            {
            }
            finally
            {
                ho_Circle1.Dispose();
                ho_Circle2.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_PolarTransImagePF.Dispose();
                ho_ImageMean1.Dispose();
                ho_ImageMean2.Dispose();
                ho_RegionDynThreshPF2.Dispose();
                ho_RegionDynThreshPF1.Dispose();
                ho_RegionUnionPF1.Dispose();
                ho_XYTransRegionPF2.Dispose();
            }

        }

        private void nudDynthresholdLightPF2_ValueChanged(object sender, EventArgs e)
        {
             iDynthresholdLightPF2 = (int)nudDynthresholdLightPF2.Value;
            if (My.ho_Image == null)
                return;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;

            try
            {
                if (My.DetectionPF2_Black || My.DetectionPF2_White)
                {
                    if (My.dOutRangePF2 > My.dOutRangePF)
                        hv_CircleRadiusPF2 = My.dOutRangePF2 + 2;
                    else
                        hv_CircleRadiusPF2 = My.dOutRangePF + 2;
                    if (My.dInRangePF2 > My.dInRangePF)
                        hv_CircleRadiusPF1 = My.dInRangePF - 2;
                    else
                        hv_CircleRadiusPF1 = My.dInRangePF2 - 2;
                }
                else
                {
                    hv_CircleRadiusPF2 = My.dOutRangePF + 2;
                    hv_CircleRadiusPF1 = My.dInRangePF - 2;
                }
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                //dev_set_draw ('fill')
                //方法二
                ho_PolarTransImagePF.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImagePF, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, "nearest_neighbor");
                hv_MeanWidth_1 = 1;
                hv_MeanHeight_1 = 1;
                hv_MeanWidth_2 = 1;
                hv_MeanHeight_2 = (hv_CircleRadiusPF2 - hv_CircleRadiusPF1)/2;
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.MedianRect(ho_PolarTransImagePF, out ExpTmpOutVar_0, 50, 2);
                    ho_PolarTransImagePF.Dispose();
                    ho_PolarTransImagePF = ExpTmpOutVar_0;
                }
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF1, hv_MeanWidth_1, hv_MeanHeight_1);
                ho_ImageMean2.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF2, hv_MeanWidth_2, hv_MeanHeight_2);
                if (My.DetectionPF_Light2 && My.DetectionPF_Dark2)
                {
                    ho_RegionDynThreshPF2.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF2, iDynthresholdLightPF2, "light");
                    ho_RegionDynThreshPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF1, iDynthresholdDarkPF2, "dark");
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.Union2(ho_RegionDynThreshPF1, ho_RegionDynThreshPF2, out ho_RegionUnionPF1);
                }
                else if (My.DetectionPF_Dark2)
                {
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, iDynthresholdDarkPF2, "dark");
                }
                else if (My.DetectionPF_Light2)
                {
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, iDynthresholdLightPF2, "light");
                }
                ho_RectanglePF2.Dispose();
                HOperatorSet.GenRectangle1(out ho_RectanglePF2, 2, 0, dOutRangePF - dInRangePF + 2, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                ho_RegionInterSectionPF2.Dispose();
                HOperatorSet.Intersection(ho_RegionUnionPF1, ho_RectanglePF2, out ho_RegionInterSectionPF2);

                ho_XYTransRegionPF2.Dispose();
                HOperatorSet.PolarTransRegionInv(ho_RegionInterSectionPF2, out ho_XYTransRegionPF2, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor");
                
                 HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_XYTransRegionPF2, hv_ExpDefaultWinHandle);

            }
            catch
            {
            }
            finally
            {
                ho_Circle1.Dispose();
                ho_Circle2.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_PolarTransImagePF.Dispose();
                ho_ImageMean1.Dispose();
                ho_ImageMean2.Dispose();
                ho_RegionDynThreshPF2.Dispose();
                ho_RegionDynThreshPF1.Dispose();
                ho_RegionUnionPF1.Dispose();
                ho_XYTransRegionPF2.Dispose();
            }
        }

        private void tbGraythresholdBlackPF2_ValueChanged(object sender, EventArgs e)
        {
            nudGraythresholdBlackPF2.Value = tbGraythresholdBlackPF2.Value;
        }

        private void nudGraythresholdBlackPF2_ValueChanged(object sender, EventArgs e)
        {
            iGraythresholdBlackPF2 = tbGraythresholdBlackPF2.Value = (int)nudGraythresholdBlackPF2.Value;

            if (My.ho_Image == null)
                return;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;

            try
            {
                if (My.DetectionPF2_Black || My.DetectionPF2_White)
                {
                    if (My.dOutRangePF2 > My.dOutRangePF)
                        hv_CircleRadiusPF2 = My.dOutRangePF2 + 2;
                    else
                        hv_CircleRadiusPF2 = My.dOutRangePF + 2;
                    if (My.dInRangePF2 > My.dInRangePF)
                        hv_CircleRadiusPF1 = My.dInRangePF - 2;
                    else
                        hv_CircleRadiusPF1 = My.dInRangePF2 - 2;
                }
                else
                {
                    hv_CircleRadiusPF2 = My.dOutRangePF + 2;
                    hv_CircleRadiusPF1 = My.dInRangePF - 2;
                }
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                //dev_set_draw ('fill')
                //方法二
                ho_PolarTransImagePF.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImagePF, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, "nearest_neighbor");
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.MedianRect(ho_PolarTransImagePF, out ExpTmpOutVar_0, 50, 2);
                    ho_PolarTransImagePF.Dispose();
                    ho_PolarTransImagePF = ExpTmpOutVar_0;
                }
                hv_grayBlackPF = iGraythresholdBlackPF2;
                hv_grayWhitePF = iGraythresholdWhitePF2;
                if (My.DetectionPF_Black && My.DetectionPF_White)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2,(new HTuple(0)).TupleConcat(hv_grayWhitePF), hv_grayBlackPF.TupleConcat(255));
                }
                else if (My.DetectionPF_Black)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, 0, hv_grayBlackPF);
                }
                else if (My.DetectionPF_White)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, hv_grayWhitePF, 255);
                }
                ho_RectanglePF2.Dispose();
                HOperatorSet.GenRectangle1(out ho_RectanglePF2, 2, 0, dOutRangePF - dInRangePF + 2, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                ho_RegionInterSectionPF2.Dispose();
                HOperatorSet.Intersection(ho_RegionUnionPF2, ho_RectanglePF2, out ho_RegionInterSectionPF2);

                ho_XYTransRegionPF2.Dispose();
                HOperatorSet.PolarTransRegionInv(ho_RegionInterSectionPF2, out ho_XYTransRegionPF2, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor");
                
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_XYTransRegionPF2, hv_ExpDefaultWinHandle);

            }
            catch
            {
            }
        }

        private void tbGraythresholdWhitePF2_ValueChanged(object sender, EventArgs e)
        {
            nudGraythresholdWhitePF2.Value = tbGraythresholdWhitePF2.Value;
        }

        private void nudGraythresholdWhitePF2_ValueChanged(object sender, EventArgs e)
        {
            iGraythresholdWhitePF2 = tbGraythresholdWhitePF2.Value = (int)nudGraythresholdWhitePF2.Value;
            if (My.ho_Image == null)
                return;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;

            try
            {
                if (My.DetectionPF2_Black || My.DetectionPF2_White)
                {
                    if (My.dOutRangePF2 > My.dOutRangePF)
                        hv_CircleRadiusPF2 = My.dOutRangePF2 + 2;
                    else
                        hv_CircleRadiusPF2 = My.dOutRangePF + 2;
                    if (My.dInRangePF2 > My.dInRangePF)
                        hv_CircleRadiusPF1 = My.dInRangePF - 2;
                    else
                        hv_CircleRadiusPF1 = My.dInRangePF2 - 2;
                }
                else
                {
                    hv_CircleRadiusPF2 = My.dOutRangePF + 2;
                    hv_CircleRadiusPF1 = My.dInRangePF - 2;
                }
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                //dev_set_draw ('fill')
                //方法二
                ho_PolarTransImagePF.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImagePF, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, "nearest_neighbor");
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.MedianRect(ho_PolarTransImagePF, out ExpTmpOutVar_0, 50, 2);
                    ho_PolarTransImagePF.Dispose();
                    ho_PolarTransImagePF = ExpTmpOutVar_0;
                }
                hv_grayBlackPF = iGraythresholdBlackPF2;
                hv_grayWhitePF = iGraythresholdWhitePF2;
                if (My.DetectionPF_Black && My.DetectionPF_White)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, (new HTuple(0)).TupleConcat(hv_grayWhitePF), hv_grayBlackPF.TupleConcat(255));
                }
                else if (My.DetectionPF_Black)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, 0, hv_grayBlackPF);
                }
                else if (My.DetectionPF_White)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, hv_grayWhitePF, 255);
                }
                ho_RectanglePF2.Dispose();
                HOperatorSet.GenRectangle1(out ho_RectanglePF2, 2, 0, dOutRangePF - dInRangePF + 2, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                ho_RegionInterSectionPF2.Dispose();
                HOperatorSet.Intersection(ho_RegionUnionPF2, ho_RectanglePF2, out ho_RegionInterSectionPF2);
       
                ho_XYTransRegionPF2.Dispose();
                HOperatorSet.PolarTransRegionInv(ho_RegionInterSectionPF2, out ho_XYTransRegionPF2, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor"); HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_XYTransRegionPF2, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
        }

        private void cbClosingPF2_CheckedChanged(object sender, EventArgs e)
        {
            My.ClosingPF2 = cbClosingPF2.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "ClosingPF2", My.ClosingPF2.ToString(), Path);
        }

        private void cbOpeningPF2_CheckedChanged(object sender, EventArgs e)
        {
            My.OpeningPF2 = cbOpeningPF2.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "OpeningPF2", My.OpeningPF2.ToString(), Path);
        }

        private void nudCloseWidthPF2_ValueChanged(object sender, EventArgs e)
        {
            iCloseWidthPF2 = (int)nudCloseWidthPF2.Value;
            MethodPF2();
        }

        private void nudCloseHeightPF2_ValueChanged(object sender, EventArgs e)
        {
            iCloseHeightPF2 = (int)nudCloseHeightPF2.Value;
            MethodPF2();
        }


        private void nudOpenWidthPF2_ValueChanged(object sender, EventArgs e)
        {
            iOpenWidthPF2 = (int)nudOpenWidthPF2.Value;
            MethodPF2();
        }

        private void nudOpenHeightPF2_ValueChanged(object sender, EventArgs e)
        {
            iOpenHeightPF2 = (int)nudOpenHeightPF2.Value;
            MethodPF2();
        }

        private void tbUnderSizeAreaPF2_ValueChanged(object sender, EventArgs e)
        {
            nudUnderSizeAreaPF2.Value = tbUnderSizeAreaPF2.Value;
        }

        private void nudUnderSizeAreaPF2_ValueChanged(object sender, EventArgs e)
        {
            iUnderSizeAreaPF2 = tbUnderSizeAreaPF2.Value = (int)nudUnderSizeAreaPF2.Value;
            MethodPF2();

        }

        public void MethodPF2()
        {
            try
            {
                if (My.ho_Image == null)
                    return;
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                if (My.DetectionPF2_Black || My.DetectionPF2_White)
                {
                    if (My.dOutRangePF2 > My.dOutRangePF)
                        hv_CircleRadiusPF2 = My.dOutRangePF2 + 2;
                    else
                        hv_CircleRadiusPF2 = My.dOutRangePF + 2;
                    if (My.dInRangePF2 > My.dInRangePF)
                        hv_CircleRadiusPF1 = My.dInRangePF - 2;
                    else
                        hv_CircleRadiusPF1 = My.dInRangePF2 - 2;
                }
                else
                {
                    hv_CircleRadiusPF2 = My.dOutRangePF + 2;
                    hv_CircleRadiusPF1 = My.dInRangePF - 2;
                }
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                //dev_set_draw ('fill')
                //方法二
                ho_PolarTransImagePF.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImagePF, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, "nearest_neighbor");
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.MedianRect(ho_PolarTransImagePF, out ExpTmpOutVar_0, 50, 2);
                    ho_PolarTransImagePF.Dispose();
                    ho_PolarTransImagePF = ExpTmpOutVar_0;
                }
                hv_MeanWidth_1 = 1;
                hv_MeanHeight_1 = 1;
                hv_MeanWidth_2 = 1;
                hv_MeanHeight_2 = (hv_CircleRadiusPF2 - hv_CircleRadiusPF1)/2;

                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF1, hv_MeanWidth_1, hv_MeanHeight_1);
                ho_ImageMean2.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF2, hv_MeanWidth_2, hv_MeanHeight_2);
                if (My.DetectionPF_Light2 && My.DetectionPF_Dark2)
                {
                    ho_RegionDynThreshPF2.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF2, iDynthresholdLightPF2, "light");
                    ho_RegionDynThreshPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF1, iDynthresholdDarkPF2, "dark");
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.Union2(ho_RegionDynThreshPF1, ho_RegionDynThreshPF2, out ho_RegionUnionPF1);
                }
                else if (My.DetectionPF_Dark2)
                {
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, iDynthresholdDarkPF2, "dark");
                }
                else if (My.DetectionPF_Light2)
                {
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, iDynthresholdLightPF2, "light");
                }

                hv_grayBlackPF = iGraythresholdBlackPF2;
                hv_grayWhitePF = iGraythresholdWhitePF2;
                if (My.DetectionPF_Black && My.DetectionPF_White)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, (new HTuple(0)).TupleConcat(hv_grayWhitePF), hv_grayBlackPF.TupleConcat(255));
                }
                else if (My.DetectionPF_Black)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, 0, hv_grayBlackPF);
                }
                else if (My.DetectionPF_White)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, hv_grayWhitePF, 255);
                }
                ho_RegionDynThreshPF2.Dispose();
                if ((My.DetectionPF_Light2 || My.DetectionPF_Dark2) && (My.DetectionPF_White || My.DetectionPF_Black))
                    HOperatorSet.Intersection(ho_RegionUnionPF1, ho_RegionUnionPF2, out ho_RegionDynThreshPF2);
                else if (!My.DetectionPF_White && !My.DetectionPF_Black)
                    HOperatorSet.Union1(ho_RegionUnionPF1, out ho_RegionDynThreshPF2);
                else if (!My.DetectionPF_Dark2 && !My.DetectionPF_Light2)
                    HOperatorSet.Union1(ho_RegionUnionPF2, out ho_RegionDynThreshPF2);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.FillUp(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                    ho_RegionDynThreshPF2.Dispose();
                    ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                }
                if (My.ClosingPF2)
                {
                    hv_closeWidthValue = iCloseWidthPF2;
                    hv_closeHeightValue = iCloseHeightPF2;
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ClosingRectangle1(ho_RegionDynThreshPF2, out ExpTmpOutVar_0,hv_closeWidthValue, hv_closeHeightValue);
                        ho_RegionDynThreshPF2.Dispose();
                        ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                    }
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.FillUp(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                    ho_RegionDynThreshPF2.Dispose();
                    ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                }
                if (My.OpeningPF2)
                {
                    hv_OpenHeightValue = iOpenHeightPF2;
                    hv_OpenWidthValue = iOpenWidthPF2;
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.OpeningRectangle1(ho_RegionDynThreshPF2, out ExpTmpOutVar_0,hv_OpenWidthValue, hv_OpenHeightValue);
                        ho_RegionDynThreshPF2.Dispose();
                        ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                    }
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                    ho_RegionDynThreshPF2.Dispose();
                    ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                }
                hv_UnderSizeArea2 = iUnderSizeAreaPF2;
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_RegionDynThreshPF2, out ExpTmpOutVar_0, "area","and", hv_UnderSizeArea2, 99999999);
                    ho_RegionDynThreshPF2.Dispose();
                    ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                }

                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Union1(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                    ho_RegionDynThreshPF2.Dispose();
                    ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                }
                ho_RectanglePF2.Dispose();
                HOperatorSet.GenRectangle1(out ho_RectanglePF2, 2, 0, dOutRangePF - dInRangePF + 2, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                ho_RegionInterSectionPF2.Dispose();
                HOperatorSet.Intersection(ho_RegionDynThreshPF2, ho_RectanglePF2, out ho_RegionInterSectionPF2);

                ho_XYTransRegionPF2.Dispose();
                HOperatorSet.PolarTransRegionInv(ho_RegionInterSectionPF2, out ho_XYTransRegionPF2, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_XYTransRegionPF2, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
            finally
            {
                ho_Circle1.Dispose();
                ho_Circle2.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_PolarTransImage.Dispose();
                ho_ImageMean1.Dispose();
                ho_ImageMean2.Dispose();
                ho_RegionDynThreshPF2.Dispose();
                ho_RegionDynThreshPF1.Dispose();
                ho_RegionUnionPF1.Dispose();
                ho_RegionUnionPF2.Dispose();
                ho_XYTransRegionPF2.Dispose();
            }

        }

        private void cbDetectionPF_Black2_CheckedChanged(object sender, EventArgs e)
        {
            My.DetectionPF_Black = (cbDetectionPF_Black2.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "DetectionPF_Black", My.DetectionPF_Black.ToString(), Path);
        }

        private void cbDetectionPF_White2_CheckedChanged(object sender, EventArgs e)
        {
            My.DetectionPF_White = (cbDetectionPF_White2.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "DetectionPF_White", My.DetectionPF_White.ToString(), Path);
        }

        private void cbTestFixedCollar_CheckedChanged(object sender, EventArgs e)
        {
            My.TestFixedCollar = (cbTestFixedCollar.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "TestFixedCollar", My.TestFixedCollar.ToString(), Path);
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

        private void cbTransformOpen_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void nudLackMaxAngleSetPF_ValueChanged(object sender, EventArgs e)
        {
            dLackMaxAngleSetPF = Convert.ToInt32(nudLackMaxAngleSetPF.Value);
            My.dLackMaxAngleSetPF = dLackMaxAngleSetPF;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "LackMaxAngleSetPF", My.dLackMaxAngleSetPF.ToString(), Path);
        }

        private void nudLackMaxAngleSet_ValueChanged(object sender, EventArgs e)
        {
            dLackMaxAngleSet = Convert.ToInt32(nudLackMaxAngleSet.Value);
            My.dLackMaxAngleSet = dLackMaxAngleSet;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "LackMaxAngleSet", My.dLackMaxAngleSet.ToString(), Path);
        }

        private void ucPF2OutRange_ValueChanged(int CurrentValue)
        {
            My.dOutRangePF2 = ucPF2OutRange.Value;

            try
            {
                //這段要另外用的
                FixationRing(hWindowControl1.HalconWindow, My.dOutRangePF2, My.dInRangePF2);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void ucPF2InRange_ValueChanged(int CurrentValue)
        {
            My.dInRangePF2 = ucPF2InRange.Value;

            try
            {
                //這段要另外用的
                FixationRing(hWindowControl1.HalconWindow, My.dOutRangePF2, My.dInRangePF2);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void ucGraythresholdBlackPF3_ValueChanged(int CurrentValue)
        {
            My.dGraythresholdBlackPF3 = ucGraythresholdBlackPF3.Value;

            if (My.ho_Image == null)
                return;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;

            try
            {
                if (My.dOutRangePF2 > My.dOutRangePF)
                    hv_CircleRadiusPF2 = My.dOutRangePF2 + 2;
                else
                    hv_CircleRadiusPF2 = My.dOutRangePF + 2;
                if (My.dInRangePF2 > My.dInRangePF)
                    hv_CircleRadiusPF1 = My.dInRangePF - 2;
                else
                    hv_CircleRadiusPF1 = My.dInRangePF2 - 2;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                //dev_set_draw ('fill')
                //方法二
                ho_PolarTransImagePF.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImagePF, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, "nearest_neighbor");
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.MedianRect(ho_PolarTransImagePF, out ExpTmpOutVar_0, 50, 2);
                    ho_PolarTransImagePF.Dispose();
                    ho_PolarTransImagePF = ExpTmpOutVar_0;
                }
                hv_grayBlackPF = My.dGraythresholdBlackPF3;
                hv_grayWhitePF = My.dGraythresholdWhitePF3;
                if (My.DetectionPF2_Black && My.DetectionPF2_White)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, (new HTuple(0)).TupleConcat(hv_grayWhitePF), hv_grayBlackPF.TupleConcat(255));
                }
                else if (My.DetectionPF2_Black)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, 0, hv_grayBlackPF);
                }
                else if (My.DetectionPF2_White)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, hv_grayWhitePF, 255);
                }
                ho_RectanglePF3.Dispose();
                HOperatorSet.GenRectangle1(out ho_RectanglePF3, My.dInRangePF2 - hv_CircleRadiusPF1, 0, My.dOutRangePF2 - hv_CircleRadiusPF1, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                ho_RegionInterSectionPF3.Dispose();
                HOperatorSet.Intersection(ho_RegionUnionPF2, ho_RectanglePF3, out ho_RegionInterSectionPF3);
                ho_XYTransRegionPF2.Dispose();
                HOperatorSet.PolarTransRegionInv(ho_RegionInterSectionPF3, out ho_XYTransRegionPF2, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_XYTransRegionPF2, hv_ExpDefaultWinHandle);

            }
            catch
            {
            }
        }

        private void cbDetectionPF2_Black2_CheckedChanged(object sender, EventArgs e)
        {
            My.DetectionPF2_Black = (cbDetectionPF2_Black2.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "DetectionPF2_Black", My.DetectionPF2_Black.ToString(), Path);
        }

        private void cbDetectionPF2_White2_CheckedChanged(object sender, EventArgs e)
        {
            My.DetectionPF2_White = (cbDetectionPF2_White2.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "DetectionPF2_White", My.DetectionPF2_White.ToString(), Path);
        }

        private void ucGraythresholdWhitePF3_ValueChanged(int CurrentValue)
        {
            My.dGraythresholdWhitePF3 = ucGraythresholdWhitePF3.Value;

            if (My.ho_Image == null)
                return;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;

            try
            {
                if (My.dOutRangePF2 > My.dOutRangePF)
                    hv_CircleRadiusPF2 = My.dOutRangePF2 + 2;
                else
                    hv_CircleRadiusPF2 = My.dOutRangePF + 2;
                if (My.dInRangePF2 > My.dInRangePF)
                    hv_CircleRadiusPF1 = My.dInRangePF - 2;
                else
                    hv_CircleRadiusPF1 = My.dInRangePF2 - 2;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                //dev_set_draw ('fill')
                //方法二
                ho_PolarTransImagePF.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImagePF, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, "nearest_neighbor");
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.MedianRect(ho_PolarTransImagePF, out ExpTmpOutVar_0, 50, 2);
                    ho_PolarTransImagePF.Dispose();
                    ho_PolarTransImagePF = ExpTmpOutVar_0;
                }
                hv_grayBlackPF = My.dGraythresholdBlackPF3;
                hv_grayWhitePF = My.dGraythresholdWhitePF3;
                if (My.DetectionPF2_Black && My.DetectionPF2_White)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, (new HTuple(0)).TupleConcat(hv_grayWhitePF), hv_grayBlackPF.TupleConcat(255));
                }
                else if (My.DetectionPF2_Black)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, 0, hv_grayBlackPF);
                }
                else if (My.DetectionPF2_White)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, hv_grayWhitePF, 255);
                }
                ho_RectanglePF3.Dispose();
                HOperatorSet.GenRectangle1(out ho_RectanglePF3, My.dInRangePF2 - hv_CircleRadiusPF1, 0, My.dOutRangePF2 - hv_CircleRadiusPF1, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                ho_RegionInterSectionPF2.Dispose();
                HOperatorSet.Intersection(ho_RegionUnionPF2, ho_RectanglePF2, out ho_RegionInterSectionPF2);

                ho_XYTransRegionPF2.Dispose();
                HOperatorSet.PolarTransRegionInv(ho_RegionInterSectionPF2, out ho_XYTransRegionPF2, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor");
                
                 HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_XYTransRegionPF2, hv_ExpDefaultWinHandle);

            }
            catch
            {
            }
        }

        private void btnPF2SetSave_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "InRangePF2", My.dInRangePF2.ToString(), Path);
            IniFile.Write("Setting", "OutRangePF2", My.dOutRangePF2.ToString(), Path);
            IniFile.Write("Setting", "GraythresholdBlackPF3", My.dGraythresholdBlackPF3.ToString(), Path);
            IniFile.Write("Setting", "GraythresholdWhitePF3", My.dGraythresholdWhitePF3.ToString(), Path);
        }

        private void btnCircleCenter8_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                CatchCenter(hWindowControl1.HalconWindow, My.ho_Image);

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

        private void cbDetectionRect2_Len1_CheckedChanged(object sender, EventArgs e)
        {
            My.DetectionRect2_Len1 = cbDetectionRect2_Len1.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "DetectionRect2_Len1", My.DetectionRect2_Len1.ToString(), Path);
        }

        private void cbDetectionRect2_Len2_CheckedChanged(object sender, EventArgs e)
        {
            My.DetectionRect2_Len2 = cbDetectionRect2_Len2.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "DetectionRect2_Len2", My.DetectionRect2_Len2.ToString(), Path);
        }

        private void ucRect2_Len1_Upper_ValueChanged(int CurrentValue)
        {
            My.Rect2_Len1_Upper = ucRect2_Len1_Upper.Value;
            if (My.ho_Image == null)
                return;
            if (hv_ResultRow.Length == 0)
                MessageBox.Show("請先求圓心!");
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.DispObj(My.ho_Image, hv_ExpDefaultWinHandle);
            Method3(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);
        }

        private void ucRect2_Len1_Lower_ValueChanged(int CurrentValue)
        {
            My.Rect2_Len1_Lower = ucRect2_Len1_Lower.Value;
            if (My.ho_Image == null)
                return;
            if (hv_ResultRow.Length == 0)
                MessageBox.Show("請先求圓心!");
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.DispObj(My.ho_Image, hv_ExpDefaultWinHandle);
            Method3(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);
        }

        private void ucRect2_Len2_Upper_ValueChanged(int CurrentValue)
        {
            My.Rect2_Len2_Upper = ucRect2_Len2_Upper.Value;
            if (My.ho_Image == null)
                return;
            if (hv_ResultRow.Length == 0)
                MessageBox.Show("請先求圓心!");
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.DispObj(My.ho_Image, hv_ExpDefaultWinHandle);
            Method3(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);
        }

        private void ucRect2_Len2_Lower_ValueChanged(int CurrentValue)
        {
            My.Rect2_Len2_Lower = ucRect2_Len2_Lower.Value;
            if (My.ho_Image == null)
                return;
            if (hv_ResultRow.Length == 0)
                MessageBox.Show("請先求圓心!");
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.DispObj(My.ho_Image, hv_ExpDefaultWinHandle);
            Method3(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);
        }

        private void ucGraySet_ValueChanged(int CurrentValue)
        {
            My.GraySet = ucGraySet.Value;
            try
            {
                if (My.ho_Image == null)
                    return;
                if (hv_ResultRow.Length == 0)
                    MessageBox.Show("請先求圓心!");
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                HObject ho_ImageMedian = new HObject(), ho_ImageEmphasize = new HObject();
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, dMarkID);
                //外圈
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, dMarkTD);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);

                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianRect(ho_ImageReduced, out ho_ImageMedian, 10, 10);
                ho_ImageEmphasize.Dispose();
                HOperatorSet.Emphasize(ho_ImageMedian, out ho_ImageEmphasize, 50, 50, 1);
                if (My.ContrastSet == 0)
                    HOperatorSet.Threshold(ho_ImageEmphasize, out ho_RegionDynThresh1, My.GraySet, 255);
                else
                    HOperatorSet.Threshold(ho_ImageEmphasize, out ho_RegionDynThresh1, 0, My.GraySet);
                My.ho_Image.DispObj(hWindowControl1.HalconWindow);
                ho_RegionDynThresh1.DispObj(hWindowControl1.HalconWindow);
            }
            catch
            {
            }
        }

        private void cbChoiceMax_CheckedChanged(object sender, EventArgs e)
        {
            My.bChoiceMax = cbChoiceMax.Checked;
        }

        private void ucOuterRadius_TestDefeat_ValueChanged(int CurrentValue)
        {
            My.OuterRadius_TestDefeat = ucOuterRadius_TestDefeat.Value;

            try
            {
                //這段要另外用的
                FixationRing(hWindowControl1.HalconWindow, My.InnerRadius_TestDefeat, My.OuterRadius_TestDefeat);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }
        private void ucInnerRadius_TestDefeat_ValueChanged(int CurrentValue)
        {
            My.InnerRadius_TestDefeat = ucInnerRadius_TestDefeat.Value;

            try
            {
                //這段要另外用的
                FixationRing(hWindowControl1.HalconWindow, My.InnerRadius_TestDefeat, My.OuterRadius_TestDefeat);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void cbDetection_Dark_TestDefeat_CheckedChanged(object sender, EventArgs e)
        {
            My.Detection_Dark_TestDefeat = (cbDetection_Dark_TestDefeat.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "Detection_Dark_TestDefeat", My.Detection_Dark_TestDefeat.ToString(), Path);
        }

        private void cbDetection_Light_TestDefeat_CheckedChanged(object sender, EventArgs e)
        {
            My.Detection_Light_TestDefeat = (cbDetection_Light_TestDefeat.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "Detection_Light_TestDefeat", My.Detection_Light_TestDefeat.ToString(), Path);
        }

        private void nudDynthresholdDark_TestDefeat_ValueChanged(object sender, EventArgs e)
        {
            My.DynthresholdDark_TestDefeat = (int)nudDynthresholdDark_TestDefeat.Value;
            if (My.ho_Image == null)
                return;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;

            try
            {
                hv_CircleRadiusPF2 = My.OuterRadius_TestDefeat + 2;
                hv_CircleRadiusPF1 = My.InnerRadius_TestDefeat - 2;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                //dev_set_draw ('fill')
                //方法二
                ho_PolarTransImagePF.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImagePF, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, "nearest_neighbor");

                hv_MeanWidth_1 = 1;
                hv_MeanHeight_1 = 1;
                hv_MeanWidth_2 = 1;
                hv_MeanHeight_2 = (hv_CircleRadiusPF2 - hv_CircleRadiusPF1) / 2;
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.MedianRect(ho_PolarTransImagePF, out ExpTmpOutVar_0, 10, 2);
                    ho_PolarTransImagePF.Dispose();
                    ho_PolarTransImagePF = ExpTmpOutVar_0;
                }
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF1, hv_MeanWidth_1, hv_MeanHeight_1);
                ho_ImageMean2.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF2, hv_MeanWidth_2, hv_MeanHeight_2);
                if (My.Detection_Light_TestDefeat && My.Detection_Dark_TestDefeat)
                {
                    ho_RegionDynThreshPF2.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF2, iDynthresholdLightPF2, "light");
                    ho_RegionDynThreshPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF1, iDynthresholdDarkPF2, "dark");
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.Union2(ho_RegionDynThreshPF1, ho_RegionDynThreshPF2, out ho_RegionUnionPF1);
                }
                else if (My.Detection_Dark_TestDefeat)
                {
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, iDynthresholdDarkPF2, "dark");
                }
                else if (My.Detection_Light_TestDefeat)
                {
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, iDynthresholdLightPF2, "light");
                }
                ho_RectanglePF2.Dispose();
                HOperatorSet.GenRectangle1(out ho_RectanglePF2, 2, 0, dOutRangePF - dInRangePF + 2, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                ho_RegionInterSectionPF2.Dispose();
                HOperatorSet.Intersection(ho_RegionUnionPF1, ho_RectanglePF2, out ho_RegionInterSectionPF2);

                ho_XYTransRegionPF2.Dispose();
                HOperatorSet.PolarTransRegionInv(ho_RegionInterSectionPF2, out ho_XYTransRegionPF2, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_XYTransRegionPF2, hv_ExpDefaultWinHandle);

            }
            catch
            {
            }
            finally
            {
                ho_Circle1.Dispose();
                ho_Circle2.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_PolarTransImagePF.Dispose();
                ho_ImageMean1.Dispose();
                ho_ImageMean2.Dispose();
                ho_RegionDynThreshPF2.Dispose();
                ho_RegionDynThreshPF1.Dispose();
                ho_RegionUnionPF1.Dispose();
                ho_XYTransRegionPF2.Dispose();
            }
        }

        private void nudDynthresholdLight_TestDefeat_ValueChanged(object sender, EventArgs e)
        {
            My.DynthresholdLight_TestDefeat = (int)nudDynthresholdLight_TestDefeat.Value;
            if (My.ho_Image == null)
                return;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;

            try
            {
                hv_CircleRadiusPF2 = My.OuterRadius_TestDefeat + 2;
                hv_CircleRadiusPF1 = My.InnerRadius_TestDefeat - 2;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                //dev_set_draw ('fill')
                //方法二
                ho_PolarTransImagePF.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImagePF, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, "nearest_neighbor");

                hv_MeanWidth_1 = 1;
                hv_MeanHeight_1 = 1;
                hv_MeanWidth_2 = 1;
                hv_MeanHeight_2 = (hv_CircleRadiusPF2 - hv_CircleRadiusPF1) / 2;
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.MedianRect(ho_PolarTransImagePF, out ExpTmpOutVar_0, 10, 2);
                    ho_PolarTransImagePF.Dispose();
                    ho_PolarTransImagePF = ExpTmpOutVar_0;
                }
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF1, hv_MeanWidth_1, hv_MeanHeight_1);
                ho_ImageMean2.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF2, hv_MeanWidth_2, hv_MeanHeight_2);
                if (My.Detection_Light_TestDefeat && My.Detection_Dark_TestDefeat)
                {
                    ho_RegionDynThreshPF2.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF2, My.DynthresholdLight_TestDefeat, "light");
                    ho_RegionDynThreshPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF1, My.DynthresholdDark_TestDefeat, "dark");
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.Union2(ho_RegionDynThreshPF1, ho_RegionDynThreshPF2, out ho_RegionUnionPF1);
                }
                else if (My.Detection_Dark_TestDefeat)
                {
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, My.DynthresholdDark_TestDefeat, "dark");
                }
                else if (My.Detection_Light_TestDefeat)
                {
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, My.DynthresholdLight_TestDefeat, "light");
                }
                ho_RectanglePF2.Dispose();
                HOperatorSet.GenRectangle1(out ho_RectanglePF2, 2, 0, hv_CircleRadiusPF2 - hv_CircleRadiusPF1 + 2, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                ho_RegionInterSectionPF2.Dispose();
                HOperatorSet.Intersection(ho_RegionUnionPF1, ho_RectanglePF2, out ho_RegionInterSectionPF2);

                ho_XYTransRegionPF2.Dispose();
                HOperatorSet.PolarTransRegionInv(ho_RegionInterSectionPF2, out ho_XYTransRegionPF2, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_XYTransRegionPF2, hv_ExpDefaultWinHandle);

            }
            catch
            {
            }
            finally
            {
                ho_Circle1.Dispose();
                ho_Circle2.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_PolarTransImagePF.Dispose();
                ho_ImageMean1.Dispose();
                ho_ImageMean2.Dispose();
                ho_RegionDynThreshPF2.Dispose();
                ho_RegionDynThreshPF1.Dispose();
                ho_RegionUnionPF1.Dispose();
                ho_XYTransRegionPF2.Dispose();
            }
        }

        private void ucGraythresholdBlack_TestDefeat_ValueChanged(int CurrentValue)
        {
            My.GraythresholdBlack_TestDefeat = (int)ucGraythresholdBlack_TestDefeat.Value;
            if (My.ho_Image == null)
                return;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;

            try
            {
                hv_CircleRadiusPF2 = My.OuterRadius_TestDefeat + 2;
                hv_CircleRadiusPF1 = My.InnerRadius_TestDefeat - 2;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                //dev_set_draw ('fill')
                //方法二
                ho_PolarTransImagePF.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImagePF, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, "nearest_neighbor");
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.MedianRect(ho_PolarTransImagePF, out ExpTmpOutVar_0, 10, 2);
                    ho_PolarTransImagePF.Dispose();
                    ho_PolarTransImagePF = ExpTmpOutVar_0;
                }
                hv_grayBlackPF = My.GraythresholdBlack_TestDefeat;
                hv_grayWhitePF = My.GraythresholdWhite_TestDefeat;
                if (My.Detection_Black_TestDefeat && My.Detection_White_TestDefeat)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, (new HTuple(0)).TupleConcat(hv_grayWhitePF), hv_grayBlackPF.TupleConcat(255));
                }
                else if (My.Detection_Black_TestDefeat)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, 0, hv_grayBlackPF);
                }
                else if (My.Detection_White_TestDefeat)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, hv_grayWhitePF, 255);
                }
                ho_RectanglePF2.Dispose();
                HOperatorSet.GenRectangle1(out ho_RectanglePF2, 2, 0, hv_CircleRadiusPF2 - hv_CircleRadiusPF1 + 2, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                ho_RegionInterSectionPF2.Dispose();
                HOperatorSet.Intersection(ho_RegionUnionPF2, ho_RectanglePF2, out ho_RegionInterSectionPF2);

                ho_XYTransRegionPF2.Dispose();
                HOperatorSet.PolarTransRegionInv(ho_RegionInterSectionPF2, out ho_XYTransRegionPF2, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_XYTransRegionPF2, hv_ExpDefaultWinHandle);

            }
            catch
            {
            }
            finally
            {
                ho_Circle1.Dispose();
                ho_Circle2.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_PolarTransImagePF.Dispose();
                ho_ImageMean1.Dispose();
                ho_ImageMean2.Dispose();
                ho_RegionDynThreshPF2.Dispose();
                ho_RegionDynThreshPF1.Dispose();
                ho_RegionUnionPF1.Dispose();
                ho_XYTransRegionPF2.Dispose();
            }
        }

        private void ucGraythresholdWhite_TestDefeat_ValueChanged(int CurrentValue)
        {
            My.GraythresholdWhite_TestDefeat = (int)ucGraythresholdWhite_TestDefeat.Value;
            if (My.ho_Image == null)
                return;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;

            try
            {
                hv_CircleRadiusPF2 = My.OuterRadius_TestDefeat + 2;
                hv_CircleRadiusPF1 = My.InnerRadius_TestDefeat - 2;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                //dev_set_draw ('fill')
                //方法二
                ho_PolarTransImagePF.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImagePF, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, "nearest_neighbor");
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.MedianRect(ho_PolarTransImagePF, out ExpTmpOutVar_0, 10, 2);
                    ho_PolarTransImagePF.Dispose();
                    ho_PolarTransImagePF = ExpTmpOutVar_0;
                }
                hv_grayBlackPF = My.GraythresholdBlack_TestDefeat;
                hv_grayWhitePF = My.GraythresholdWhite_TestDefeat;
                if (My.Detection_Black_TestDefeat && My.Detection_White_TestDefeat)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, (new HTuple(0)).TupleConcat(hv_grayWhitePF), hv_grayBlackPF.TupleConcat(255));
                }
                else if (My.Detection_Black_TestDefeat)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, 0, hv_grayBlackPF);
                }
                else if (My.Detection_White_TestDefeat)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, hv_grayWhitePF, 255);
                }
                ho_RectanglePF2.Dispose();
                HOperatorSet.GenRectangle1(out ho_RectanglePF2, 2, 0, hv_CircleRadiusPF2 - hv_CircleRadiusPF1 + 2, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                ho_RegionInterSectionPF2.Dispose();
                HOperatorSet.Intersection(ho_RegionUnionPF2, ho_RectanglePF2, out ho_RegionInterSectionPF2);

                ho_XYTransRegionPF2.Dispose();
                HOperatorSet.PolarTransRegionInv(ho_RegionInterSectionPF2, out ho_XYTransRegionPF2, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_XYTransRegionPF2, hv_ExpDefaultWinHandle);

            }
            catch
            {
            }
            finally
            {
                ho_Circle1.Dispose();
                ho_Circle2.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_PolarTransImagePF.Dispose();
                ho_ImageMean1.Dispose();
                ho_ImageMean2.Dispose();
                ho_RegionDynThreshPF2.Dispose();
                ho_RegionDynThreshPF1.Dispose();
                ho_RegionUnionPF1.Dispose();
                ho_XYTransRegionPF2.Dispose();
            }
        }

        private void cbDetection_Black_TestDefeat_CheckedChanged(object sender, EventArgs e)
        {
            My.Detection_Black_TestDefeat = (cbDetection_Black_TestDefeat.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "Detection_Black_TestDefeat", My.Detection_Black_TestDefeat.ToString(), Path);
        }

        private void cbDetection_White_TestDefeat_CheckedChanged(object sender, EventArgs e)
        {
            My.Detection_White_TestDefeat = (cbDetection_White_TestDefeat.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "Detection_White_TestDefeat", My.Detection_White_TestDefeat.ToString(), Path);
        }

        private void cbClosing_TestDefeat_CheckedChanged(object sender, EventArgs e)
        {
            My.Closing_TestDefeat = cbClosing_TestDefeat.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "Closing_TestDefeat", My.Closing_TestDefeat.ToString(), Path);
        }

        private void cbOpening_TestDefeat_CheckedChanged(object sender, EventArgs e)
        {
            My.Opening_TestDefeat = cbOpening_TestDefeat.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "Opening_TestDefeat", My.Opening_TestDefeat.ToString(), Path);
        }

        private void nudCloseWidth_TestDefeat_ValueChanged(object sender, EventArgs e)
        {
            My.CloseWidth_TestDefeat = (int)nudCloseWidth_TestDefeat.Value;
            DetectionDefeat();
        }

        private void nudCloseHeight_TestDefeat_ValueChanged(object sender, EventArgs e)
        {
            My.CloseHeight_TestDefeat = (int)nudCloseHeight_TestDefeat.Value;
            DetectionDefeat();
        }

        private void nudOpenWidth_TestDefeat_ValueChanged(object sender, EventArgs e)
        {
            My.OpenWidth_TestDefeat = (int)nudOpenWidth_TestDefeat.Value;
            DetectionDefeat();
        }

        private void nudOpenHeight_TestDefeat_ValueChanged(object sender, EventArgs e)
        {
            My.OpenHeight_TestDefeat = (int)nudOpenHeight_TestDefeat.Value;
            DetectionDefeat();
        }

        private void ucUnderSizeArea_TestDefeat_ValueChanged(int CurrentValue)
        {
            My.UnderSizeArea_TestDefeat = (int)ucUnderSizeArea_TestDefeat.Value;
            DetectionDefeat();
        }

        public void DetectionDefeat()
        {
            try
            {
                if (My.ho_Image == null)
                    return;
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hv_CircleRadiusPF2 = My.OuterRadius_TestDefeat + 2;
                hv_CircleRadiusPF1 = My.InnerRadius_TestDefeat - 2;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadiusPF2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                //dev_set_draw ('fill')
                //方法二
                ho_PolarTransImagePF.Dispose();
                HOperatorSet.PolarTransImageExt(ho_ImageReduced, out ho_PolarTransImagePF, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, "nearest_neighbor");

                hv_MeanWidth_1 = 1;
                hv_MeanHeight_1 = 1;
                hv_MeanWidth_2 = 1;
                hv_MeanHeight_2 = (hv_CircleRadiusPF2 - hv_CircleRadiusPF1) / 2;
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.MedianRect(ho_PolarTransImagePF, out ExpTmpOutVar_0, 10, 2);
                    ho_PolarTransImagePF.Dispose();
                    ho_PolarTransImagePF = ExpTmpOutVar_0;
                }
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF1, hv_MeanWidth_1, hv_MeanHeight_1);
                ho_ImageMean2.Dispose();
                HOperatorSet.MeanImage(ho_PolarTransImagePF, out ho_ImageMeanPF2, hv_MeanWidth_2, hv_MeanHeight_2);
                if (My.Detection_Light_TestDefeat && My.Detection_Dark_TestDefeat)
                {
                    ho_RegionDynThreshPF2.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF2, My.DynthresholdLight_TestDefeat, "light");
                    ho_RegionDynThreshPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionDynThreshPF1, My.DynthresholdDark_TestDefeat, "dark");
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.Union2(ho_RegionDynThreshPF1, ho_RegionDynThreshPF2, out ho_RegionUnionPF1);
                }
                else if (My.Detection_Dark_TestDefeat)
                {
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, My.DynthresholdDark_TestDefeat, "dark");
                }
                else if (My.Detection_Light_TestDefeat)
                {
                    ho_RegionUnionPF1.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMeanPF1, ho_ImageMeanPF2, out ho_RegionUnionPF1, My.DynthresholdLight_TestDefeat, "light");
                }
                hv_grayBlackPF = My.GraythresholdBlack_TestDefeat;
                hv_grayWhitePF = My.GraythresholdWhite_TestDefeat;
                if (My.Detection_Black_TestDefeat && My.Detection_White_TestDefeat)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, (new HTuple(0)).TupleConcat(hv_grayWhitePF), hv_grayBlackPF.TupleConcat(255));
                }
                else if (My.Detection_Black_TestDefeat)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, 0, hv_grayBlackPF);
                }
                else if (My.Detection_White_TestDefeat)
                {
                    ho_RegionUnionPF2.Dispose();
                    HOperatorSet.Threshold(ho_PolarTransImagePF, out ho_RegionUnionPF2, hv_grayWhitePF, 255);
                }
                if ((My.Detection_Light_TestDefeat || My.Detection_Dark_TestDefeat) && (My.Detection_White_TestDefeat || My.Detection_Black_TestDefeat))
                    HOperatorSet.Intersection(ho_RegionUnionPF1, ho_RegionUnionPF2, out ho_RegionDynThreshPF2);
                else if (!My.Detection_White_TestDefeat && !My.Detection_Black_TestDefeat)
                    HOperatorSet.Union1(ho_RegionUnionPF1, out ho_RegionDynThreshPF2);
                else if (!My.Detection_Dark_TestDefeat && !My.Detection_Light_TestDefeat)
                    HOperatorSet.Union1(ho_RegionUnionPF2, out ho_RegionDynThreshPF2);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.FillUp(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                    ho_RegionDynThreshPF2.Dispose();
                    ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                }
                if (My.Closing_TestDefeat)
                {
                    hv_closeWidthValue = My.CloseWidth_TestDefeat;
                    hv_closeHeightValue = My.CloseHeight_TestDefeat;
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ClosingRectangle1(ho_RegionDynThreshPF2, out ExpTmpOutVar_0, hv_closeWidthValue, hv_closeHeightValue);
                        ho_RegionDynThreshPF2.Dispose();
                        ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                    }
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.FillUp(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                    ho_RegionDynThreshPF2.Dispose();
                    ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                }
                if (My.Opening_TestDefeat)
                {
                    hv_OpenHeightValue = My.OpenHeight_TestDefeat;
                    hv_OpenWidthValue = My.OpenWidth_TestDefeat;
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.OpeningRectangle1(ho_RegionDynThreshPF2, out ExpTmpOutVar_0, hv_OpenWidthValue, hv_OpenHeightValue);
                        ho_RegionDynThreshPF2.Dispose();
                        ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                    }
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                    ho_RegionDynThreshPF2.Dispose();
                    ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                }
                hv_UnderSizeArea2 = My.UnderSizeArea_TestDefeat;
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_RegionDynThreshPF2, out ExpTmpOutVar_0, "area", "and", hv_UnderSizeArea2, 99999999);
                    ho_RegionDynThreshPF2.Dispose();
                    ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                }

                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Union1(ho_RegionDynThreshPF2, out ExpTmpOutVar_0);
                    ho_RegionDynThreshPF2.Dispose();
                    ho_RegionDynThreshPF2 = ExpTmpOutVar_0;
                }
                ho_RectanglePF2.Dispose();
                HOperatorSet.GenRectangle1(out ho_RectanglePF2, 2, 0, hv_CircleRadiusPF2 - hv_CircleRadiusPF1 + 2, (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * ((new HTuple(180)).TupleRad()));
                ho_RegionInterSectionPF2.Dispose();
                HOperatorSet.Intersection(ho_RegionDynThreshPF2, ho_RectanglePF2, out ho_RegionInterSectionPF2);

                ho_XYTransRegionPF2.Dispose();
                HOperatorSet.PolarTransRegionInv(ho_RegionInterSectionPF2, out ho_XYTransRegionPF2, hv_ResultRow, hv_ResultColumn, 0, 6.28319, hv_CircleRadiusPF1, hv_CircleRadiusPF2,
                    (hv_CircleRadiusPF2 + hv_CircleRadiusPF1) * Math.PI, hv_CircleRadiusPF2 - hv_CircleRadiusPF1, hv_Width, hv_Height, "nearest_neighbor");
                HTuple hv_ResultArea_TestDefeat = new HTuple();
                HOperatorSet.RegionFeatures(ho_XYTransRegionPF2, "area", out hv_ResultArea_TestDefeat);

                set_display_font(hv_ExpDefaultWinHandle, 20, "mono", "true", "false");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_XYTransRegionPF2, hv_ExpDefaultWinHandle);
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, ("AefeatArea:" + Math.Round(hv_ResultArea_TestDefeat.D)) + " Pixel");
            }
            catch
            {
            }
            finally
            {
                ho_Circle1.Dispose();
                ho_Circle2.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_PolarTransImage.Dispose();
                ho_ImageMean1.Dispose();
                ho_ImageMean2.Dispose();
                ho_RegionDynThreshPF2.Dispose();
                ho_RegionDynThreshPF1.Dispose();
                ho_RegionUnionPF1.Dispose();
                ho_RegionUnionPF2.Dispose();
                ho_XYTransRegionPF2.Dispose();
            }

        }

        private void cbTestDefeat_CheckedChanged(object sender, EventArgs e)
        {
            My.TestDefeat = (cbTestDefeat.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "TestDefeat", My.TestDefeat.ToString(), Path);
        }

        private void btnSave_TestDefeat_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "OuterRadius_TestDefeat", My.OuterRadius_TestDefeat.ToString(), Path);
            IniFile.Write("Setting", "InnerRadius_TestDefeat", My.InnerRadius_TestDefeat.ToString(), Path);
            IniFile.Write("Setting", "UnderSizeArea_TestDefeat", My.UnderSizeArea_TestDefeat.ToString(), Path);
            IniFile.Write("Setting", "DetectionArea_TestDefeat", My.DetectionArea_TestDefeat.ToString(), Path);

            IniFile.Write("Setting", "DynthresholdDark_TestDefeat", My.DynthresholdDark_TestDefeat.ToString(), Path);
            IniFile.Write("Setting", "DynthresholdLight_TestDefeat", My.DynthresholdLight_TestDefeat.ToString(), Path);
            IniFile.Write("Setting", "GraythresholdBlack_TestDefeat", My.GraythresholdBlack_TestDefeat.ToString(), Path);
            IniFile.Write("Setting", "GraythresholdWhite_TestDefeat", My.GraythresholdWhite_TestDefeat.ToString(), Path);
            IniFile.Write("Setting", "CloseWidth_TestDefeat", My.CloseWidth_TestDefeat.ToString(), Path);
            IniFile.Write("Setting", "CloseHeight_TestDefeat", My.CloseHeight_TestDefeat.ToString(), Path);
            IniFile.Write("Setting", "OpenWidth_TestDefeat", My.OpenWidth_TestDefeat.ToString(), Path);
            IniFile.Write("Setting", "OpenHeight_TestDefeat", My.OpenHeight_TestDefeat.ToString(), Path);
        }

        private void ucDetectionArea_TestDefeat_ValueChanged(int CurrentValue)
        {
            My.DetectionArea_TestDefeat = ucDetectionArea_TestDefeat.Value;
        }

        private void cbTestNoGlue_CheckedChanged(object sender, EventArgs e)
        {
            My.TestNoGlue = (cbTestNoGlue.Checked ? true : false);
           
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "TestNoGlue", My.TestNoGlue.ToString(), Path);
        }

        private void ucOuterRadius_TestNoGlue_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            My.OuterRadius_TestNoGlue = ucOuterRadius_TestNoGlue.Value;
            try
            {
                //這段要另外用的
                FixationRing(hWindowControl1.HalconWindow, My.InnerRadius_TestNoGlue, My.OuterRadius_TestNoGlue);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void ucInnerRadius_TestNoGlue_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            My.InnerRadius_TestNoGlue = ucInnerRadius_TestNoGlue.Value;
            try
            {
                //這段要另外用的
                FixationRing(hWindowControl1.HalconWindow, My.InnerRadius_TestNoGlue, My.OuterRadius_TestNoGlue);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void cbDetection_Black_TestNoGlue_CheckedChanged(object sender, EventArgs e)
        {
            My.Detection_Black_TestNoGlue = (cbDetection_Black_TestNoGlue.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "Detection_Black_TestNoGlue", My.Detection_Black_TestNoGlue.ToString(), Path);
        }

        private void cbDetection_White_TestNoGlue_CheckedChanged(object sender, EventArgs e)
        {
            My.Detection_White_TestNoGlue = (cbDetection_White_TestNoGlue.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "Detection_White_TestNoGlue", My.Detection_White_TestNoGlue.ToString(), Path);
        }

        

        public void TestNoGlue(HObject TheImage,HTuple hv_ResultRow,HTuple hv_ResultColumn,out HObject ho_RegionIntersection,out HTuple hv_Area)
        {

            hv_Area = 0;
            HObject ho_Circle1 = new HObject(), ho_Circle2 = new HObject(), ho_RegionDifference = new HObject();
            HObject ho_ImageReduced = new HObject(), ho_Edges = new HObject(), ho_RegionFillUp = new HObject();
            HObject ho_RegionClosing = new HObject(), ho_ConnectedRegions = new HObject(), ho_SelectedRegions = new HObject();
            HObject ho_RegionUnion = new HObject();

            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            try
            {
                if (TheImage == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(TheImage, out ho_Image);
                //畫出固定環區域
                hv_CircleRadius1 = My.InnerRadius_TestNoGlue;
                hv_CircleRadius2 = My.OuterRadius_TestNoGlue;
                //平滑
                //ho_ImageSmooth.Dispose();
                //HOperatorSet.SmoothImage(ho_Image, out ho_ImageSmooth, "deriche2", 0.3);
                ho_Circle1.Dispose();
                HOperatorSet.GenCircle(out ho_Circle1, hv_ResultRow, hv_ResultColumn, hv_CircleRadius1);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_ResultRow, hv_ResultColumn, hv_CircleRadius2);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle2, ho_Circle1, out ho_RegionDifference);
                //分割出固定環
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                //灰度設定
                hv_grayBlack = My.GraythresholdBlack_TestNoGlue;
                hv_grayWhite = My.GraythresholdWhite_TestNoGlue;
                //膠水為黑色白色設定
                ho_Edges.Dispose();
                if (My.Detection_Black_TestNoGlue && My.Detection_White_TestNoGlue)
                {
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Edges, (new HTuple(0)).TupleConcat(hv_grayWhite), hv_grayBlack.TupleConcat(255));
                }
                else if (My.Detection_Black_TestNoGlue && !My.Detection_White_TestNoGlue)
                {
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Edges, 0, hv_grayBlack);
                }
                else if (!My.Detection_Black_TestNoGlue && My.Detection_White_TestNoGlue)
                {
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Edges, hv_grayWhite, 255);
                }
                else
                {
                    MessageBox.Show("黑白至少要檢測一項");
                    return;
                }
                //填滿縫隙
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_Edges, out ho_RegionFillUp);
                //將相鄰的面積相連
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_RegionFillUp, out ho_RegionClosing, 3.5);
                //分割
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
                //開放設置去掉過小面積
                hv_UnderSizeArea = dUnderSizeArea;
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", My.UnderSizeArea_TestNoGlue, 99999999);
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion);
                //找出與固定環區域的交集,避免360度整個填滿的情況
                ho_RegionIntersection.Dispose();
                HOperatorSet.Intersection(ho_RegionUnion, ho_RegionDifference, out ho_RegionIntersection);
                HTuple hv_Row = new HTuple(),hv_Column = new HTuple();
                HOperatorSet.AreaCenter(ho_RegionIntersection, out hv_Area, out hv_Row, out hv_Column);
                if (hv_Area.Length == 0)
                    hv_Area = 0;
                //HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                //HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                //HOperatorSet.DispObj(ho_RegionIntersection, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
        }


        private void ucGraythresholdBlack_TestNoGlue_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            My.GraythresholdBlack_TestNoGlue = ucGraythresholdBlack_TestNoGlue.Value;
            if (My.ho_Image == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_RegionIntersection = new HObject();
            HTuple Area = new HTuple();
            ho_RegionIntersection.Dispose();
            TestNoGlue(My.ho_Image, hv_ResultRow, hv_ResultColumn, out ho_RegionIntersection, out Area);

            My.ho_Image.DispObj(Window);
            Window.SetColor("green");
            Window.SetDraw("margin");
            ho_RegionIntersection.DispObj(Window);


        }
        private void ucGraythresholdWhite_TestNoGlue_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            My.GraythresholdWhite_TestNoGlue = ucGraythresholdWhite_TestNoGlue.Value;
            if (My.ho_Image == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_RegionIntersection = new HObject();
            HTuple Area = new HTuple();
            ho_RegionIntersection.Dispose();
            TestNoGlue(My.ho_Image, hv_ResultRow, hv_ResultColumn, out ho_RegionIntersection, out Area);

            My.ho_Image.DispObj(Window);
            Window.SetColor("green");
            Window.SetDraw("margin");
            ho_RegionIntersection.DispObj(Window);
        }

        private void ucUnderSizeArea_TestNoGlue_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            My.UnderSizeArea_TestNoGlue = ucUnderSizeArea_TestNoGlue.Value;
            if (My.ho_Image == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_RegionIntersection = new HObject();
            HTuple Area = new HTuple();
            ho_RegionIntersection.Dispose();
            TestNoGlue(My.ho_Image, hv_ResultRow, hv_ResultColumn, out ho_RegionIntersection, out Area);

            My.ho_Image.DispObj(Window);
            Window.SetColor("green");
            Window.SetDraw("margin");
            ho_RegionIntersection.DispObj(Window);
        }

        private void ucAreaUpper_TestNoGlue_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            My.AreaUpper_TestNoGlue = ucAreaUpper_TestNoGlue.Value;
            if (My.ho_Image == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_RegionIntersection = new HObject();
            HTuple Area = new HTuple();
            ho_RegionIntersection.Dispose();
            TestNoGlue(My.ho_Image, hv_ResultRow, hv_ResultColumn, out ho_RegionIntersection, out Area);

            My.ho_Image.DispObj(Window);
            Window.SetColor("green");
            Window.SetDraw("margin");
            ho_RegionIntersection.DispObj(Window);

            set_display_font(Window, 40, "mono", "true", "false");
            disp_message(Window, "面積:" + Area.D, "", 0, 0, "green", "false");

            if (My.AreaUpper_TestNoGlue < Area || My.AreaLower_TestNoGlue > Area)
            {
                disp_message(Window, "NG", "", 200, 0, "red", "false");
            }
            else
            {
                disp_message(Window, "OK", "", 200, 0, "green", "false");
            }
        }

        private void ucAreaLower_TestNoGlue_ValueChanged(object sender, EventArgs e)
        {
            if (bReadingPara)
                return;
            My.AreaLower_TestNoGlue = ucAreaLower_TestNoGlue.Value;
            if (My.ho_Image == null)
                return;
            HWindow Window = hWindowControl1.HalconWindow;
            HObject ho_RegionIntersection = new HObject();
            HTuple Area = new HTuple();
            ho_RegionIntersection.Dispose();
            TestNoGlue(My.ho_Image, hv_ResultRow, hv_ResultColumn, out ho_RegionIntersection, out Area);

            My.ho_Image.DispObj(Window);
            Window.SetColor("green");
            Window.SetDraw("margin");
            ho_RegionIntersection.DispObj(Window);

            set_display_font(Window, 40, "mono", "true", "false");
            disp_message(Window, "面積:" + Area.D, "", 0, 0, "green", "false");

            if (My.AreaUpper_TestNoGlue < Area || My.AreaLower_TestNoGlue > Area)
            {
                disp_message(Window, "NG", "", 200, 0, "red", "false");
            }
            else
            {
                disp_message(Window, "OK", "", 200, 0, "green", "false");
            }
        }

        private void btn_Save_TestNoGlue_Click(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "OuterRadius_TestNoGlue", My.OuterRadius_TestNoGlue.ToString(), Path);
            IniFile.Write("Setting", "InnerRadius_TestNoGlue", My.InnerRadius_TestNoGlue.ToString(), Path);
            IniFile.Write("Setting", "GraythresholdBlack_TestNoGlue", My.GraythresholdBlack_TestNoGlue.ToString(), Path);
            IniFile.Write("Setting", "GraythresholdWhite_TestNoGlue", My.GraythresholdWhite_TestNoGlue.ToString(), Path);
            IniFile.Write("Setting", "UnderSizeArea_TestNoGlue", My.UnderSizeArea_TestNoGlue.ToString(), Path);
            IniFile.Write("Setting", "AreaUpper_TestNoGlue", My.AreaUpper_TestNoGlue.ToString(), Path);
            IniFile.Write("Setting", "AreaLower_TestNoGlue", My.AreaLower_TestNoGlue.ToString(), Path);
        }

     
    }
}
