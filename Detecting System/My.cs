 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;
using System.Net;
using System.IO.Ports;
using System.Drawing;

namespace Detecting_System
{
    public class My
    {
        public static bool ContinueShot = false;
        public static bool bCurrention = false;
        public static HObject ho_Image;
        public static HObject[] My_image; 
        public static double dFirstCircleRadius = 1;
        public static double dReduceRadius = 1;
        public static double dGraythreshold = 1;
        public static double dCenterRadius = 1;
        public static double dLength = 1;
        public static string sGenParamValue = "negative";
        public static double dMeasureThreshold = 1;

        public static double dResultRow = 1;
        public static double dResultColumn = 1;
        public static double dRingOutRange = 1;
        public static double dRingInRange = 1;
        public static double dGraythresholdBlack = 1;
        public static double dGraythresholdWhite = 1;
        public static double dUnderSizeArea = 1;
        //固定環判定方法選擇
        public static int DecisionMethodChoice = 0;
        //固定環方法二距離
        public static double dRegionDistance = 0;
        //固定環方法二數量
        public static int iGlueCount = 0;

        //固定環膠水角度
        public static int iGlueAngleSet = 1;
        //固定環膠水閥值
        public static int iGlueRatioSet = 1;
        public static double dAngleSet = 1;
        public static double dLackMaxAngleSet = 1;
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
        //方法二連接
        public static bool Closing = false;
        //方法二連接寬
        public static double dCloseWidthValue = 1;
        //方法二連接高
        public static double dCloseHeightValue = 1;
        //方法二斷開
        public static bool Opening = false;
        //方法二斷開寬
        public static double dOpenWidthValue = 1;
        //方法二斷開高
        public static double dOpenHeightValue = 1;
        //方法二過濾寬度
        public static bool FilterWidth = false;
        //方法二過濾寬度上限
        public static double dFilterWidth_Upper = 1;
        //方法二過濾寬度下限
        public static double dFilterWidth_Lower = 1;
        //方法二過濾高度
        public static bool FilterHeight = false;
        //方法二過濾高度上限
        public static double dFilterHeight_Upper = 1;
        //方法二過濾高度下限
        public static double dFilterHeight_Lower = 1;
        
        //方法選擇
        public static int MethodChoice = 0;
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
        public static double dOutRangePF = 1;
        public static double dInRangePF = 1;
        public static double dGraythresholdBlackPF = 1;
        public static double dGraythresholdWhitePF = 1;
        public static double dUnderSizeAreaPF = 1;
        //固定環膠水角度
        public static int iGlueAngleSetPF = 1;
        //固定環膠水閥值
        public static int iGlueRatioSetPF = 1;
        public static double dAngleSetPF = 1;
        public static double dLackMaxAngleSetPF = 1;

        public static double dOutSlope = 1;
        public static double dInSlope = 1;
        public static double dSpilledGrayThreshold = 1;
        public static double dSpilledUnderSizeArea = 1;
        public static int dGraythresholdBlackPF2 = 1;
        public static int dGraythresholdWhitePF2 = 1;
        public static int dGraythresholdBlackPF3 = 1;
        public static int dGraythresholdWhitePF3 = 1;
        //檢測是否有膠水
        public static bool TestNoGlue = false;
        //有無膠內外徑
        public static int OuterRadius_TestNoGlue = 0;
        public static int InnerRadius_TestNoGlue = 0;
        //黑或白檢測
        public static bool Detection_Black_TestNoGlue = false;
        public static bool Detection_White_TestNoGlue = false;
        //灰度值
        public static int GraythresholdBlack_TestNoGlue = 0;
        public static int GraythresholdWhite_TestNoGlue = 0;
        //過濾小面積
        public static int UnderSizeArea_TestNoGlue = 0;
        //閥值上下限
        public static int AreaUpper_TestNoGlue = 0;
        public static int AreaLower_TestNoGlue = 0;


        //表面檢測外圍
        public static double dOutSlope3 = 1;
        //表面檢測內圍
        public static double dInSlope3 = 1;
        //表面灰度值
        public static double dGraythreshold3 = 1;
        //表面角度
        public static double dAngleSet2 = 1;
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
        //縫隙加強區域
        public static int dOutRangePF2 = 1;
        public static int dInRangePF2 = 1;
        public static DateTime dt;
        //檢測功能開關
        public static bool TestFixedCollar = false;
        public static bool TestGap = false;
        public static bool TestSlope = false;
        public static bool TestInside = false;
        public static bool TestPlatform = false;
        public static bool TestDirection = false;
        //檢測方向方法選擇
        public static int TestDirectionChoice = 0;
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
        //檢測第幾象限
        public static int iTestQuadrant = 1;
        public static bool bChoiceMax = false;
        public static int DarkLightChoice = 0;
        //固定環膠水檢測黑/白
        public static bool Detection_Black = true;
        public static bool Detection_White = true;
        //小平台膠水檢測黑/白
        //小平台方法
        public static int MethodChoice2 = 0;
        public static bool DetectionPF_Black = true;
        public static bool DetectionPF_White = true;
        public static bool DetectionPF_Dark2 = true;
        public static bool DetectionPF_Light2 = true;
        //小台階方法二差異閥值
        public static int iDynthresholdDarkPF2 = 1;
        public static int iDynthresholdLightPF2 = 1;
        //小台階方法二灰度閥值
        public static int iGraythresholdBlackPF2 = 1;
        public static int iGraythresholdWhitePF2 = 1;
        //小台階方法二連接/斷開/過濾小面積
        public static int iUnderSizeAreaPF2 = 1;
        public static int iCloseWidthPF2 = 1;
        public static int iCloseHeightPF2 = 1;
        public static int iOpenWidthPF2 = 1;
        public static int iOpenHeightPF2 = 1;
        public static bool ClosingPF2 = false;
        public static bool OpeningPF2 = false;
        public static bool DetectionPF2_Black = true;
        public static bool DetectionPF2_White = true;

        public static bool DetectionRect2_Len2 = false;
        public static bool DetectionRect2_Len1 = false;
        public static int Rect2_Len1_Upper = 0;
        public static int Rect2_Len1_Lower = 0;
        public static int Rect2_Len2_Upper = 0;
        public static int Rect2_Len2_Lower = 0;
        public static int GraySet = 0;

        //檢測缺陷開關
        public static bool TestDefeat = false;
        //檢測缺陷差異閥值
        public static bool Detection_Black_TestDefeat = true;
        public static bool Detection_White_TestDefeat = true;
        public static bool Detection_Dark_TestDefeat = true;
        public static bool Detection_Light_TestDefeat = true;
        public static int OuterRadius_TestDefeat = 1;
        public static int InnerRadius_TestDefeat = 1;
        public static int DynthresholdDark_TestDefeat = 1;
        public static int DynthresholdLight_TestDefeat = 1;
        //檢測缺陷灰度閥值
        public static int GraythresholdBlack_TestDefeat = 1;
        public static int GraythresholdWhite_TestDefeat = 1;
        //檢測缺陷連接/斷開/過濾小面積
        public static int UnderSizeArea_TestDefeat = 1;
        public static int CloseWidth_TestDefeat = 1;
        public static int CloseHeight_TestDefeat = 1;
        public static int OpenWidth_TestDefeat = 1;
        public static int OpenHeight_TestDefeat = 1;
        public static bool Closing_TestDefeat = false;
        public static bool Opening_TestDefeat = false;
        public static int DetectionArea_TestDefeat = 1;
        public class VDI
        {
            public static int DarkLightChoice = 0;
            public static double AimCirR = 1;
            public static double mmpixel = 0.04;
            public static double dCoatRMin = 1;
            public static double dCoatRMax = 1;
            public static double dNegativeOffSet = 1;
            public static double dPositiveOffSet = 1;
            public static double DiamMin = 1;
            public static double DiamMinSet = 1;

            public static double dFirstCircleRadius2 = 1;
            public static double dLength2 = 1;
            public static string sGenParamValue2 = "negative";
            public static double dMeasureThreshold2 = 1;

            public static double dResultRow;
            public static double dResultColumn;

            public static double dResultRow2;
            public static double dResultColumn2;
            public static double dResultRadius2;
            public static double dResultDiam = 1;

            public static double dMissRadius = 1;
            public static double dMissGray = 1;
            public static double dMissArea = 1;
        }

        public class Soma
        {
            //是否檢測缺陷
            public static bool TestDefect = false;

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
            
        }

        public class VDICoating
        {
            public static double mmpixel = 0.0044;
            public static bool TestDefect = false;
            public static bool IgnoreOpen = false;
            public static string PointChoice = "first";
            public static double dRadius_ID = 1;
            public static double dRadius_TD = 1;

            public static double dFirstCircleRadius = 1;
            public static double dReduceRadius = 1;
            public static double dGraythreshold = 1;
            public static double dLength = 1;
            public static double dMeasureThreshold = 1;
            public static string sGenParamValue = "negative";

            public static double dDefect_ID = 1;
            public static double dDefect_TD = 1;
            public static double dIgnore_ID = 1;
            public static double dIgnore_TD = 1;

            public static bool DetectionColor = false;
            public static bool DetectionColor_2 = false;
            public static bool DetectionG = false;
            public static bool DetectionR = false;
            public static bool Detection1 = false;
            public static bool Detection2 = false;
            public static bool Detection3 = false;
            public static bool DetectionA = false;
            public static bool DetectionB = false;

            public static double dDefectGraySet = 1;
            public static double dFilterArea = 1;
            public static double dScratchLength = 1;
            public static double dLargeArea = 1;
            public static double dNumber = 1;

            public static double dMissGray = 1;
            public static double dMissArea = 1;
            public static double dMissOuterRadius = 1;

            public static double dUpperLimit_A = 1;
            public static double dLowerLimit_A = 1;
            public static double dUpperLimit_B = 1;
            public static double dLowerLimit_B = 1;

            public static double dRangeRadius = 1;
            public static double dCenterDistance = 1;

            public static int ColorRangeChoice = 1;
            public static int DarkLightChoice = 0;
            public static double AimCirR = 0;
            public static int iNoInkAreaSet = 0;
        }

        public class VDI_Ink
        {
            public static int iEmptyGraySet = 1;
            public static HTuple dEmptyCircleRadius = 0;
            //抓圓心的圓半徑
            public static HTuple dCenterRadius = 1;
            public static string PointChoice = "first";

            public static int iNoInkAreaSet = 0;
            public static int iReduceRadius = 1;
            //擬合圓線條長度
            public static int iLength = 1;
            //擬合圓灰度差異
            public static int iMeasureThreshold = 20;
            //擬合圓白找黑或黑找白
            public static string sGenParamValue = "negative";

            public static double pixel2um = 4.4;
            public static int iOutsideDiam_Upper = 1;
            public static int iOutsideDiam_Lower = 1;
            public static int iInsideDiam_Upper = 1;
            public static int iInsideDiam_Lower = 1;

            public static double dAngleRange_Empty = 1;
            public static double dScoreSet_Empty = 1;
            public static double dAngleRange_NoInk = 1;
            public static double dScoreSet_NoInk = 1;

            public static int iUnderSizeArea = 1;

            public HTuple hv_ExpDefaultWinHandle;
            public static bool CreateModel_Empty = false;
            public static bool CreateModel_NoInk = false;
            public static int iEccentricitySet = 0;
            public static int iGraySet = 1;
            public static int iAimCirR = 1;

        }

        public class NIR
        {
            public static bool ReadBarrelBarcode = false;
            public static double BarcodeRange_Row1 = 0;
            public static double BarcodeRange_Column1 = 0;
            public static double BarcodeRange_Row2 = 0;
            public static double BarcodeRange_Column2 = 0;
            public static int BarcodePosition = 0;

            //找方形參數
            public static HTuple Length1_RectangleCenter = 1;
            public static HTuple Length2_RectangleCenter = 1;
            public static HTuple Phi_RectangleCenter = 1;
            public static HTuple Length_RectangleCenter = 1;
            public static HTuple MeasureSelect_RectangleCenter = 0;
            public static HTuple MeasureThreshold_RectangleCenter = 0;
            public static HTuple MeasureTransition_RectangleCenter = "negative";

            public static HTuple hv_ResultRow_CenterRectangle = 0;
            public static HTuple hv_ResultColumn_CenterRectangle = 0;

            //找圓參數
            public static HTuple Radius_Center = 1;
            public static HTuple Length_Center = 1;
            public static HTuple MeasureSelect_Center = 0;
            public static HTuple MeasureThreshold_Center = 0;
            public static HTuple MeasureTransition_Center = "negative";

            public static HTuple hv_CenterRow = 0;
            public static HTuple hv_CenterColumn = 0;

            //固定環半徑
            public static HTuple expected_holder_radius = 0;

            //溢膠檢測
            public static bool Check_ExcessiveGlue;

            public static int Mode_ExcessiveGlue;

            public static double OuterRadius_ExcessiveGlue = 0;
            public static double InnerRadius_ExcessiveGlue = 0;
            public static int DarkLightChoice_ExcessiveGlue = 0;
            public static int OffSet_ExcessiveGlue = 0;
            //Particle檢測
            public static double OuterRadius_Paricle = 0;
            public static double InnerRadius_Paricle = 0;
            //缺陷檢測
            public static int DarkLightChoice_Scar = 0;

            public static double mm_2_pixel = 360.0;
        }

        public class Classifier
        {
            //機種參數
            public static int iProductionSet = 0;
            public static string sProductionSet = "";
            public static int TotalProductionCount = 0;
            public static List<string> Productions = new List<string>();//所有的机种
            public static List<string> NotchCounts = new List<string>();//所有的剪口
            public static List<string> MarkLocations = new List<string>();//所有的Mark位置

            //無料參數
            public static int iNullthreshold = 1;
            public static int iNullArea_Upper = 1;
            public static int iNullArea_Lower = 1;
            //矩形參數
            public static int iRectangleSmooth = 1;
            public static int iRectangleLightDark = 0;
            public static int iRectangleOffSet = 1;
            public static int iRectangleDilation = 1;
            public static bool RectangleAreaSet = false;
            public static bool RectangleRoundnessSet = false;
            public static bool RectangleRectangularitySet = false;
            public static int iRectangleArea_Upper = 1;
            public static int iRectangleArea_Lower = 1;
            public static int iRectangleRoundness_Upper = 1;
            public static int iRectangleRoundness_Lower = 1;
            public static int iRectangleRectangularity_Upper = 1;
            public static int iRectangleRectangularity_Lower = 1;
            public static double dDrawRectangle2Length1 = 1;
            public static double dDrawRectangle2Length2 = 1;
            public static int iRectangleLightDark2 = 0;
            public static int iRectanglePointChoice = 0;
            public static int iRectangleLength = 1;
            public static int iRectangleMeasureThreshold = 1;
            //圓參數
            public static int iCircleSmooth = 1;
            public static int iCircleLightDark = 0;
            public static int iCircleOffSet = 1;
            public static int iCircleClosing = 1;
            public static bool CircleAreaSet = false;
            public static bool CircleRoundnessSet = false;
            public static bool CircleRectangularitySet = false;
            public static int iCircleArea_Upper = 1;
            public static int iCircleArea_Lower = 1;
            public static int iCircleRoundness_Upper = 1;
            public static int iCircleRoundness_Lower = 1;
            public static int iCircleRectangularity_Upper = 1;
            public static int iCircleRectangularity_Lower = 1;
            public static double dDrawCircleRadius = 1;
            public static int iCircleLightDark2 = 0;
            public static int iCirclePointChoice = 0;
            public static int iCircleLength = 1;
            public static int iCircleMeasureThreshold = 1;
            //剪口參數
            public static int iNotchArea_Upper = 1;
            public static int iNotchArea_Lower = 1;
            //Mark參數
            public static int iMarkSmooth = 1;
            public static int iMarkLightDark = 0;
            public static int iMarkOffSet = 1;
            public static bool MarkAreaSet = false;
            public static bool MarkRoundnessSet = false;
            public static bool MarkRectangularitySet = false;
            public static int iMarkArea_Upper = 1;
            public static int iMarkArea_Lower = 1;
            public static int iMarkRoundness_Upper = 1;
            public static int iMarkRoundness_Lower = 1;
            public static int iMarkRectangularity_Upper = 1;
            public static int iMarkRectangularity_Lower = 1;

        }

        public class LensCarry
        {
            public struct _Result//專門儲存結果
            {
                public HTuple hv_FirstCenterRow;
                public HTuple hv_FirstCenterColumn;
                public HTuple hv_CenterRow;
                public HTuple hv_CenterColumn;
                public HTuple hv_CenterPhi;
                public HTuple hv_AngleCenterRow;
                public HTuple hv_AngleCenterColumn;
                public HTuple hv_ResultAngle;
                public HTuple hv_ResultPhi;

                public HObject ho_Image_Befort;
                public HTuple hv_CenterRow_Befort;
                public HTuple hv_CenterColumn_Befort;
                public HTuple hv_ResultAngle_Befort;
                public HTuple hv_ResultPhi_Befort;

                public string DetectionResult;
            }
            public _Result m_Result = new _Result();

            public struct _CircleCenter
            {
                public int FirstRadius;
                public int ModelMode;
                public int ModelGrade;
                public int Radius;
                public string Measure_Transition;
                public string Measure_Select;
                public int Num_Measures;
                public int Measure_Length1;
                public int Measure_Length2;
                public int Measure_Threshold;
            }
            public _CircleCenter m_CircleCenter = new _CircleCenter();

            public struct _Correction
            {
                //Correction校正
                //初始座標
                public HTuple hv_FirstRow;
                public HTuple hv_FirstColumn;
                public HTuple hv_FirstRadius;
                //結果座標
                public HTuple hv_FirstResultRow;
                public HTuple hv_FirstResultColumn;
                //校正座標
                public HTuple hv_Row1;
                public HTuple hv_Column1;
                public HTuple hv_Row2;
                public HTuple hv_Column2;
                public HTuple hv_PlcY1;
                public HTuple hv_PlcX1;
                public HTuple hv_PlcY2;
                public HTuple hv_PlcX2;

                public HTuple Angle;
                public HTuple Pixelmm;

                public HTuple Origin_PlcX;
                public HTuple Origin_PlcY;
                
                public HTuple Gray;
                public int DarkLightChoice;
                public HTuple MeasureSelect;
                public HTuple Radius;
                public HTuple MeasureLength;
                public HTuple MeasureTransition;
                public HTuple MeasureThreshold;

                public double Position_CCDDifference_X;//CCDX軸影像偏差
                public double Position_CCDDifference_Y;//CCDY軸影像偏差

            }
            public _Correction m_Correction = new _Correction();
            public struct _Angle1//角度1參數
            {
                public HTuple OuterRadius;
                public HTuple InnerRadius;
                //檢測區域與方形相交
                public bool InterSectingRectangle;
                public int DarkLightChoice;
                public int Length1;
                public int Length2;
                public string Measure_Transition;
                public string Measure_Select;
                public int Num_Measures;
                public int Measure_Length1;
                public int Measure_Length2;
                public int Measure_Threshold;

                public int ContrastSet;
                public HTuple Gray;

                public bool Closing;
                public HTuple ClosingValue;
                public bool Opening;
                public HTuple OpeningValue;
                public HTuple Rect2_Len1_Upper;
                public HTuple Rect2_Len1_Lower;
                public HTuple Rect2_Len2_Upper;
                public HTuple Rect2_Len2_Lower;
                public bool SelectAreaMaximum;
                public int StandardAngle;
            }
            public _Angle1 m_Angle1 = new _Angle1();
        }

    }
    //相機參數
    public class CCD
    {
        public static int CCDBrand = 0;
        public static bool IsConnected = false;
        public static IPAddress ip;
        public static int Port = 8001;
        public static double GainMaximum = 1000;
        public static double GainMinimum = 0;
        public static double Gain = 0;
        public static double ExposureTimeMaximum = 1000000;
        public static double ExposureTimeMinimum = 1000000;
        public static double ExposureTime = 35000;
    }
    public class Sys
    {
        public static int Function = 0;//功能選擇 1.點膠識別 2.Soma偏識別
        public static int Factory = 0;//廠區選擇

        //补充FunctionString、code、type
        public static string FunctionString = "VDI"; //与Function对应；分别记名为VDI、Dispensing、SOMA、SOMA Defect、VDI Coating、NIR、Inkiness
        public static string Codes = "M";  //对应Run.cbCodes索引（0-13）；分别记名为M、T、F、C、M、K、Q、H、E、R、P、S、N、Other
        public static string Type = "Semi-finished"; //对应Run.cbType索引（0-1）;分别记名为Semi-finished、finished

        public static string MachineID = "";

        public static bool OptionOriginal = false;
        public static bool OptionOK = false;
        public static bool OptionNG = false;
        public static bool TrayMessage = false;
        public static bool LastResult = false;
        public static bool ReadBarcodeLog = false;
        public static bool VisitWebService = false;
        public static bool ManualClear = false;
        public static string IniPath = Application.StartupPath + "\\Ini";
        public static string SysPath = Application.StartupPath + "\\Ini\\System.ini";
        public static string ImageSavePath = "E:\\Visual Inspection System Image";
        public static string ImageSavePath0 = Application.StartupPath + "\\Image";
        public static string LogPath = "C:\\Visual Inspection System Log";
        public static string ConditionPath = "C:\\Visual Inspection System Log\\Condition Log";
        public static string AlarmPath = Application.StartupPath + "\\Log\\Alarm";
        public static string ModulePath = Application.StartupPath + "\\Module";
        public static string Accounts = "AutoDept";
        public static string PassWord = "Auto.123";
        public static string TrayPath = Application.StartupPath + "\\Ini\\Tray";

        public static bool bThrow_OK = false;
        public static bool bThrow_NG = false;
        public static bool bThrow_NG2 = false;
        public static bool bThrow_NG3 = false;
        public static bool bThrow_NG4 = false;
        public static bool bThrow_NG5 = false;
        public static bool bThrow_Miss = false;
        
    }
    public class Production
    {
        public static int TotalProductionCount = 0;
        public static string CurProduction = "";
        public static string ProductionPath = Application.StartupPath + "\\Ini\\Production.ini";
    }
    public class User
    {
        //映射：用户名+密码
        public static Dictionary<string, string> Total = new Dictionary<string, string>();
        public static string CurrentUser = "";
    }
    public class Light
    {
        public static int LightSet_1 = 0;
        public static int LightSet_2 = 0;
        public static int LightSet_3 = 0;
        public static int LightSet_4 = 0;

        public static bool IsConnected = false;
        public static byte[] ReadCh1 = new byte[] { 0x86, 0x55, 0xAA, 0x21, 0x00, 0xA6 };
        public static List<DateTime> OpTime = new List<DateTime>();
        public static string CurrentBarcode;
        public static DateTime CurrentOpTime;
        public static int SerialNumber = 0;
        public static SerialPort Com = new SerialPort("COM1", 19200, Parity.None, 8, StopBits.One);

    }
    public class Plc
    {
        public static bool IsConnected = false;
        public static bool CloseNow = false;//离开程式，关闭PLC通讯端口
        public static int Status = 0;//是否读吗流程运转中
        public static int Status_Last = 0;//上一個PLC運行狀態
        public static int Trigger = 0;//trigger信号的值
        public static int Trigger_Last = 0;//trigger信号的值
        public static int CurrentPointIndex = 0;//当前的点位
        public static int LDirection = 1;//D11 L掃碼方向
        public static int BarcodeTrigger = 0;
        // 补充D13-15
        public static int RDirection = 1;//D13:R跑盘方向1左上角2右上角3左下角4右下角
        public static int TriggerMode = 2; //D14:触发模式 1软体2硬件
        public static int TriggerMode_Last = 0;
        public static int RunMode = 1; //D15:跑盘模式 1.仅盘1 2.仅盘2 3.盘1-盘2 4.盘2-盘1
        public static int TrayMode_1 = 1; //D15:跑盘模式 1.仅盘1 2.仅盘2 3.盘1-盘2 4.盘2-盘1
        public static int TrayMode_2 = 1; //D15:跑盘模式 1.仅盘1 2.仅盘2 3.盘1-盘2 4.盘2-盘1
        public static int Plateful_1 = 0;//D18盤一滿盤 = 1
        public static bool bPlateful_1 = false;
        public static int Plateful_2 = 0;//D19盤二滿盤 = 1
        public static bool bPlateful_2 = false;
        public static bool bPlateful_WS = false;
        public static bool VisualComplete = false;
        public static IPAddress ip;
        public static int Port;
    }
    public class Protocol
    {
        //Read D0~D19
        public static string ReadD0toD19 = "01FF000A4420000000001400";
        //Read D30~D45
        public static string ReadD30toD45 = "01FF000A44200000001E1000";
        //Write trigger signal D10
        public static string Trigger = "03FF000A44200000000A0100";
        public static int Result_Trigger = 0;
        //Write trigger signal D12
        public static string BarcodeTrigger = "03FF000A44200000000C0100";
        public static int Result_BarcodeTrigger = 0;
        //Write PC IsConnect D20
        public static string PCIsConnect = "03FF000A4420000000140100";
        public static int Result_PCIsConnect = 1;
        //Write Vision Result D21
        public static string Vision = "03FF000A4420000000150100";
        public static int Result_Vision = 0;
        //Write PC IsConnect D22
        public static string PCOK = "03FF000A4420000000160100";
        public static int Result_PCOK = 1;
        //Write PC IsConnect D22
        public static string BarcodeOK = "03FF000A4420000000170100";
        public static int Result_BarcodeOK = 0;
        //Write trigger signal D18
        public static string PlatefulClearOK_1 = "03FF000A4420000000120100";
        public static int Result_PlatefulClearOK_1 = 0;
        //Write trigger signal D19
        public static string PlatefulClearOK_2 = "03FF000A4420000000130100";
        public static int Result_PlatefulClearOK_2 = 0;
        //Write D3500-3501 吸嘴CCD軸距離/3502-3503吸嘴CCD軸距離
        public static string Position_Distance = "03FF000A442000000DAC0400";
        public static double Position_Distance_X = 0;
        public static double Position_Distance_Y = 0;

        //Read D2112 单颗周期（两位小数点）
        public static string SingleCycle = "01FF000A4420000008400100";
        public static double Result_Cycle = 0.00;
        //D2122 盘1周期（一位小数点）
        public static string P1Cycle = "01FF000A44200000084A0100";
        public static double Result_P1Cycle = 0.0;
        //D2132 盘2周期（一位小数点）
        public static string P2Cycle = "01FF000A4420000008540100";
        public static double Result_P2Cycle = 0.0;
        //D8340 X轴当前坐标（两位小数点）
        public static string XCurrent = "01FF000A4420000020940100";
        public static double AxisCoordinate_X = 0.00;
        //D8350 Y轴当前坐标（两位小数点）
        public static string YCurrent = "01FF000A44200000209E0100";
        public static double AxisCoordinate_Y = 0.00;
       

        ////D500~D900 分料結果
        //public static string Result_D500to = "";
        //public static string Result_D520to = "";
        //public static string Result_D540to = "";
        //public static string Result_D560to = "";
        //public static string Result_D580to = "";
        //public static string Result_D600to = "";
        //public static string Result_D620to = "";
        //public static string Result_D640to = "";
        //public static string Result_D660to = "";
        //public static string Result_D680to = "";
        //public static string Result_D700to = "";
        //public static string Result_D720to = "";
        //public static string Result_D740to = "";
        //public static string Result_D760to = "";
        //public static string Result_D780to = "";
        //public static string Result_D800to = "";
        //public static string Result_D820to = "";
        //public static string Result_D840to = "";
        //public static string Result_D860to = "";
        //public static string Result_D880to = "";
    
        ////PC ready D7
        //public static string TrayBarCodeComplete = "03FF000A4420000000070100";
        //public static int Result_TrayBarCodeComplete = 0;
        ////Clear coordinate D8
        //public static string Clear = "03FF000A4420000000080100";
        //public static int Result_Clear = 0;
        ////AB tray  D100
        //public static string SelectTrayType = "03FF000A4420000000640100";
        //public static int Result_TrayType = 0;

        public static bool bTrigger = false;//复位PLC trigger信号
        public static bool bBarcodeTrigger = false;//复位PLC Barcodetrigger信号
        public static bool bPCOK = false;
        public static bool bPCIsConnect = false;
        public static bool bVisionOK = false;//影像處理完成，通知PLC
        public static bool bBarcodeOK = false;//Barcode完成，通知PLC
        public static bool PlatefulClear_1 = false;//已接收滿盤信號歸零
        public static bool PlatefulClear_2 = false;//已接收滿盤信號歸零
        public static bool bPositionDistance_SuctionNozzleCCD = false;//寫入吸嘴CCD距離
        //D30結果
        public static int Result_XY_Opposite = 0;//分料結果XY相反
        public static int Separator_Open = 0;//是否開啟分料 D31
        public static int BarcodeReaderPlus_n = 0;//是否開啟分料 D32
        public static int BarcodeReaderPlus_MaxRow = 0;//定點掃碼總行數
        public static int BarcodeReaderPlus_MaxColumn = 0;//定點掃碼總行數
        public static int BarcodeReaderPlus_NowRow = 0;//定點掃碼總行數
        public static int BarcodeReaderPlus_NowColumn = 0;//定點掃碼總行數
        public static int BarcodeReaderPlus_Open = 0;//定點掃碼總行數

        public static int PlcStatus = 0;//機台狀態 1-RUN 2-Down 3-IDLE 4-PAUSE
        public static int PlcStatus_Befort = 0;//機台狀態 1-RUN 2-Down 3-IDLE 4-PAUSE

        public static string Versions_HMI = "20200101";//D41-42 HMI版本
        public static string Versions_PLC = "20200101";//D43-44 PLC版本
        public static string Versions_PC = "20201208";//PC版本
        public static int Mode_Light = 0;//光源切換模式 0不開啟 1開啟

        //public static bool bTrayBarCodeComplete = false;//PC端Barcode信息输入完成，通知PLC
        //public static bool bClear = false;//清除点位
        //public static bool bTrayType = false;//写ABC版类型
    }

    public class AlarmMessage
    {
        public string AlarmMessageEn = "";
        public string AlarmMessageCH = "";

        public bool A0000 = false;//d9123
        public bool A0001 = false;
        public bool A0002 = false;
        public bool A0003 = false;
        public bool A0004 = false;
        public bool A0005 = false;
        public bool A0006 = false;
        public bool A0007 = false;
        public bool A0008 = false;
        public bool A0009 = false;
        public bool A0010 = false;
        public bool A0011 = false;
        public bool A0012 = false;
        public bool A0013 = false;
        public bool A0014 = false;
        public bool A0015 = false;
    }

    public class Tray
    {
        public static int n = 1;//第幾顆
        public static int Rows_1 = 15;//行
        public static int Columns_1 = 15;//列
        public static int Rows_2 = 9;//行
        public static int Columns_2 = 9;//列
        public static int NowTray = 1;
        public static int CurrentRow = 1;//当前行
        public static int CurrentColumn = 1;//当前列
        //public static string ChuanPiao = "";//传票
        //public static string OperatorID = "";//操作员ID
        public static DateTime OpDateTime = DateTime.Now;//操作时间
        //public static string MtfBarcode = "";//mtf tray barcode
        //public static string MtfProductViaMtfBarcode = "";//通过tray盘获取的机种名称

        public static string Barcode_1 = "";
        public static string Barcode_2 = "";
        public static string Class = "";
        public static string OperatorID = "";

    }

    public class Vision
    {
        public static string FolderName = "";
        public static string FileName = "";
        //視覺辨識结果
        public static Dictionary<int, string> VisionResult = new Dictionary<int, string>();
        public static Dictionary<int, string> ProcessingTime = new Dictionary<int, string>();
        public static Dictionary<int[,], string> VisionResult2 = new Dictionary<int[,], string>();
        //public static Dictionary<int, string> VisionBarcodeResult = new Dictionary<int, string>();
        public static Dictionary<int, int> VisionBarcodeRotate = new Dictionary<int, int>();
        public static Dictionary<int, BarcodeResult> VisionBarcodeResult = new Dictionary<int, BarcodeResult>();
        public static Dictionary<int, double> VisionOffsetCenterRow = new Dictionary<int, double>();
        public static Dictionary<int, double> VisionOffsetCenterColumn = new Dictionary<int, double>();
        //图片
        public static Dictionary<int, HObject> Images_1 = new Dictionary<int, HObject>();
        public static Dictionary<int, HObject> Images_2 = new Dictionary<int, HObject>();
        public static Dictionary<int, HObject> Images_Last = new Dictionary<int, HObject>();
        public static Dictionary<int, HObject> Images_Now = new Dictionary<int, HObject>();
        public static Dictionary<int, HObject> ImagesOriginal_1 = new Dictionary<int, HObject>();
        public static Dictionary<int, HObject> ImagesOriginal_2 = new Dictionary<int, HObject>();
        //Barcode圖片
        public static string BarcodeResult_1 = "";
        public static string BarcodeResult_2 = "";

        public class BarcodeResult
        {
            public string Barcode = "NA";
            public double MeanLight = 0;
            public double BarcodeAngle = 0;//Barcode角度(依照產品旋轉角度)
            public HTuple hv_DecodedOrientation = 0;//Barcode角度(初始代出角度)
            public HTuple hv_DecodedMirrored = "no";//"yes"/"no" 是否鏡像
            public object[] Overall_Quality;
            public object[] Cell_Contrast;
            public object[] Print_Growth;
            public object[] Unused_Error_Correction;
            public object[] Cell_Modulation;
            public object[] Fixed_Pattern_Damage;
            public object[] Grid_Nonuniformity;
            public object[] Decode;
            public BarcodeResult()
            {
                Overall_Quality = new object[2] { "F", 0};
                Cell_Contrast = new object[2] { "F", 0};
                Print_Growth = new object[2] { "F", 0};
                Unused_Error_Correction = new object[2] { "F", 0};
                Cell_Modulation = new object[2] { "F", 0};
                Fixed_Pattern_Damage = new object[2] { "F", 0};
                Grid_Nonuniformity = new object[2] { "F", 0};
                Decode = new object[2] { "F", 0};
            }

        }
    }
    public class My8982Soma
    {
        public static HObject Myho_Image;
        public static double dFirstCircleRadius = 1;
        public static double dReduceRadius = 1;
        public static double dGraythreshold = 1;
        public static double dLength = 1;
        public static double dWidth = 1;
        public static string sGenParamValue = "negative";
        public static double dMeasureThreshold = 1;

        public static double dSomaReduceRadius = 1;
        public static double dSomaGraythreshold = 1;
            
        public static double dCircularitySet = 0.97;
    }
    public class Count
    {
        public static decimal iTotal = 0;
        public static int iOK = 0;
        public static int iNG = 0;
        public static int iNG2 = 0;
        public static int iNG3 = 0;
        public static int iNG4 = 0;
        public static int iNG5 = 0;
        public static int iMiss = 0;
        public static int iTest= 0;
        public static decimal iTestTotal = 0;
        public static decimal dTotalRatio = 0;
        public static decimal dOKRatio = 0;
        public static decimal dNGRatio = 0;
        public static decimal dNG2Ratio = 0;
        public static decimal dNG3Ratio = 0;
        public static decimal dNG4Ratio = 0;
        public static decimal dNG5Ratio = 0;
        public static decimal dTestRatio = 0;
    }
    public class Reader
    {
        public static bool IsConnected = false;
        public static List<string> Barcode = new List<string>();
        public static List<DateTime> OpTime = new List<DateTime>();
        public static string CurrentBarcode;
        public static DateTime CurrentOpTime;
        public static int SerialNumber = 0;
        public static SerialPort Com = new SerialPort("COM3", 19200, Parity.None, 8, StopBits.One);

        public static bool BarcodeRead = false;
    }
    public class DataBank
    {
        public static bool IsConnected = false;
        public static string filename_Barcode = "";
        public static string Name_Barcode = "";
        public static string[,] ResultBarcode = new string[15, 15];
    }
    public class WebService
    {
        public static string[][] ArrResult;
        public static bool NIRResult_Start = false;
        public static string NIRMsg_Start = "";
        public static bool NIRResult_End = false;
        public static string NIRMsg_End = "";
    }
    public class Test
    {
        public static bool _Test = false;
        public static int Condition = 0;
        public static string ConditionName = "";
        public static decimal Thresh_Upper = 0;
        public static decimal Thresh_Lower = 0;
        public static decimal Corner_Upper = 0;
        public static decimal Corner_Lower = 0;
        public static Dictionary<int, Boolean> Target = new Dictionary<int, Boolean>();
    }

    public class MyBarcodeReader
    {
        public static int Production = 0;
        public static bool Verification = false;
        public static HTuple RangeRow1 = 0;
        public static HTuple RangeColumn1 = 0;
        public static HTuple RangeRow2 = 0;
        public static HTuple RangeColumn2 = 0;

        public static int OkAddition = 0;

        
    }

    public class MyBarcodeReaderPlus
    {
        public static int Production = 0;
        public static int TrayRows = 0;
        public static int TrayColumns = 0;
        public static int RowCutNum = 1;
        public static int ColumnCutNum = 1;
        public static bool[] TrayPointBanned = new bool[400];
        public static HObject ho_ImagePart;
        public static int FirstRadius = 1;

        public static HTuple hv_ModelID = new HTuple();
        public static int ModelMode = 0;
        public static double ModelGrade = 0;
        //檢測區域

        public static HTuple RegionRow1 = 0;
        public static HTuple RegionColumn1 = 0;
        public static HTuple RegionRow2 = 0;
        public static HTuple RegionColumn2 = 0;
        //找圓參數
        public static HTuple Radius = 1;
        public static HTuple Length = 1;
        public static HTuple MeasureSelect = "last";
        public static HTuple MeasureThreshold = 0;
        public static HTuple MeasureTransition = "negative";

        //找方形參數
        public static HTuple Length2 = 0;
        public static HTuple Phi2 = 0;
        public static HTuple Length21 = 0;
        public static HTuple Length22 = 0;
        public static HTuple MeasureSelect2 = "last";
        public static HTuple MeasureThreshold2 = 0;
        public static HTuple MeasureTransition2 = "negative";
        public static HTuple hv_FirstRow = 0;
        public static HTuple hv_FirstColumn = 0;
        public static HTuple hv_ResultRow = 0;
        public static HTuple hv_ResultColumn = 0;
        public static HTuple hv_ResultPhi = 0;

        public static HTuple OuterRadius = 1;
        public static HTuple InnerRadius = 1;
        public static HTuple StartAngle = 0;
        public static HTuple EndAngle = 180;

        public static int RegionDarkLight = 0;
        public static int RegionThreshold = 0;
        public static HTuple RegionRect2_Len1_Upper = 0;
        public static HTuple RegionRect2_Len1_Lower = 0;
        public static HTuple RegionRect2_Len2_Upper = 0;
        public static HTuple RegionRect2_Len2_Lower = 0;
        //barcode位置
        public static HTuple RegionRect2_Row = 0;
        public static HTuple RegionRect2_Column = 0;

        public static HTuple RegionErosion = 0;

        public static HTuple RegionDistance = 0;
        public static HTuple RegionRotation = 0;
        public static HTuple RegionLength1 = 0;
        public static HTuple RegionLength2 = 0;

        public static int RegionProjectSet = 0;

        public static bool Mirrored = false;
        public static int BarcodeAngleSet = 0;
        public static double AllowableOffsetAngle_L = 0;
        public static double AllowableOffsetAngle = 0;


        //角度設定
        public static int Mode_Angle = 0;

       
        //模式1
        public static HTuple Length1_Angle1 = 0;
        public static HTuple Length2_Angle1 = 0;
        public static HTuple Phi_Angle1 = 0;
        
        public static HTuple Length_Angle1 = 0;
        public static int MeasureSelect_Angle1 = 0;
        public static HTuple MeasureThreshold_Angle1 = 0;
        public static HTuple MeasureTransition_Angle1 = "negative";


        //品質檢測
        public static int Overall_Quality = 0;
        public static int Cell_Contrast = 0;
        public static int Print_Growth = 0;
        public static int Unused_Error_Correction = 0;
        public static int Cell_Modulation = 0;
        public static int Fixed_Pattern_Damage = 0;
        public static int Grid_Nonuniformity = 0;
        public static int Decode = 0;
    }

    public class MyLensCrack_AVI
    {
        public static HObject ResultImage = new HObject();
        public static int Radius_First = 1;
        public static int OuterRadius_Lens = 1;
        public static int InnerRadius_Lens = 1;
        public class m_First
        {
            //找圓參數
            public static HTuple Radius = 1;
            public static HTuple Length = 1;
            public static HTuple MeasureSelect = "last";
            public static HTuple MeasureThreshold = 1;
            public static HTuple MeasureTransition = "negative";

            public static HTuple hv_ResultRow = 0;
            public static HTuple hv_ResultColumn = 0;
        }
        public class m_Cutting
        {
            public static int Gray = 1;
            public static int Length1_Upper = 1;
            public static int Length1_Lower = 1;
            public static int Length2_Upper = 1;
            public static int Length2_Lower = 1;
            public static int CuttingDilation = 1;
            public static HObject ReionCutting = new HObject();
        }
        public class m_RegionDetection_1
        {
            public static int OuterRadous = 1;
            public static int InnerRadous = 1;
            public static int Offset_Dark = 1;
            public static int Offset_Light = 1;
            public static HObject ResultRegion = new HObject();
        }
        public class m_RegionDetection_2
        {
            public static int OuterRadous = 1;
            public static int InnerRadous = 1;
            public static int Offset_Dark = 1;
            public static int Offset_Light = 1;
            public static HObject ResultRegion = new HObject();
        }
        public class m_RegionDetection_3
        {
            public static int OuterRadous = 1;
            public static int InnerRadous = 1;
            public static int Offset_Dark = 1;
            public static int Offset_Light = 1;
            public static HObject ResultRegion = new HObject();
        }
        public class m_Filter
        {
            public static int Dilation_Width = 1;
            public static int Dilation_Height = 1;
            public static int Closing = 1;
            public static int Select_Area = 1;
            public static HObject UnionResultRegion = new HObject();
        }



    }
    public class MyLens_Mold_Cave
    {   ///Halcon信息展示
        public static bool bShowMessage = false;
        /// <summary>
        /// Tray盘1模别
        /// </summary>
        public static string sTray1Mold = "";
        /// <summary>
        /// Tray盘1模穴
        /// </summary>
        public static string sTray1Cave = "";
        /// <summary>
        /// Tray盘2模别
        /// </summary>
        public static string sTray2Mold = "";
        /// <summary>
        /// Tray盘2模穴
        /// </summary>
        public static string sTray2Cave = "";
        /// <summary>
        /// 图片处理半径
        /// </summary>
        public static int dReduceRadius = 0;
        /// <summary>
        /// 灰度上限
        /// </summary>
        public static int dGraythresholdUp = 255;
        /// <summary>
        /// 灰度下限
        /// </summary>       
        public static int dGraythresholdDown = 0;
        /// 面积上限
        /// </summary>
        public static int dAreaUp = 0;
        /// <summary>
        /// 面积下限
        /// </summary>
        public static int dAreaDown = 100000;
        /// <summary>
        /// 长度1上限
        /// </summary>
        public static int dLength1Up = 500;
        /// <summary>
        /// 长度1下限
        /// </summary>
        public static int dLength1Down = 0;
        /// <summary>
        /// 长度2上限
        /// </summary>
        public static int dLength2Up = 500;
        /// <summary>
        /// 长度2下限
        /// </summary>
        public static int dLength2Down = 0;
        /// <summary>
        /// 边阈值
        /// </summary>
        public static int dthreshold = 30;
        /// <summary>
        /// 卡尺数量
        /// </summary>
        public static int dcliperNum = 20;
        /// <summary>
        /// 卡尺长
        /// </summary>
        public static int dcliperlength = 80;
        /// <summary>
        /// 卡尺宽
        /// </summary>
        public static int dcliperwidth = 80;
        /// <summary>
        /// 极性 黑找白或白找黑
        /// </summary>
        public static string spolarity = "positive";
        public static string sedgeSelect = "first";
        /// <summary>
        /// 字符到中心距离里
        /// </summary>
        public static int dOCRCenterDistance = 850;
        /// <summary>
        /// 字符矩形长度
        /// </summary>
        public static int dOCRRectangleLength = 80;
        /// <summary>
        /// 字符矩形宽度
        /// </summary>
        public static int dOCRRectangleWidth = 80;
        /// <summary>
        /// OCR字符灰度分割上限
        /// </summary>
        public static int dOCRthresholdUp = 255;
        /// <summary>
        /// OCR字符灰度分割下限
        /// </summary>
        public static int dOCRthresholdDown = 0;
        /// <summary>
        /// OCR字符宽度上限
        /// </summary>
        public static int dOCRWidthUp = 100;
        /// <summary>
        /// OCR字符宽度下限
        /// </summary>
        public static int dOCRWidthDown = 0;
        /// <summary>
        ///  OCR字符高度上限
        /// </summary>
        public static int dOCRHeightUp = 100;
        /// <summary>
        /// OCR字符高度下限
        /// </summary>
        public static int dOCRHeightDown = 0;
        public static int OCRMethod = 0;
        public static bool binGegion = false;
        public static int dMeanfilte = 40;
        public static int iThresholdSelect = 0;   //0:dynthreshol  1:thresholdUp
        public static int dCloseWidth = 1;
        public static int dCloseHeight = 1;
        public static int dOpenWidth = 1;
        public static int dOpenHeight = 1;
        public static bool bClosing = false;
        public static bool bOpenging = false;
    }

}
