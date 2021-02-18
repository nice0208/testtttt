using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalconDotNet;
using System.Threading;

namespace Detecting_System
{
    public class MyHalcon
    {
        public static HTuple hv_ExpDefaultWinHandle;


        public static HTuple hv_WindowHandle = new HTuple(), hv_Width = null;
        public static HTuple hv_Height = null;

        public static HTuple CenterRaw, CenterColumn;


        public static void ReadPicture(HWindow window, string ImagePath,out HObject ho_Image)
        {
            // 得到图片显示的窗口句柄
            hv_ExpDefaultWinHandle = window; //从上个函数传进来的窗口句柄
            HOperatorSet.GenEmptyObj(out ho_Image);
            ho_Image.Dispose();
            HOperatorSet.ReadImage(out ho_Image, ImagePath); //从这个路径读取图片
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height); //得到他的大小
            HOperatorSet.SetWindowAttr("background_color", "black");
            //调整窗口显示大小以适应图片，这句一定要加上，不然图片显示的不全
            HOperatorSet.SetPart(hv_ExpDefaultWinHandle, 0, 0, hv_Height - 1, hv_Width - 1);
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle); //将图像在该窗口进行显示
        }
        //劃出檢視範圍
        public static void GenDetectionRadius(HWindow Window, HObject ho_Image, HTuple dReduceRadius, out HObject ho_ReducedImage)
        {
            try
            {
                HObject ho_Circle;

                ho_Image = My.ho_Image;
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
                Window.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, dReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                HOperatorSet.DispObj(ho_ReducedImage, Window);
                Thread.Sleep(1);
            }
            catch
            {
                ho_ReducedImage = My.ho_Image;
            }
        }

        //抓出灰度閥值
        public static void GrayThreshold1(HWindow Window, HObject ho_Image, HTuple dReduceRadius,HTuple Graythreshold, out HObject ho_ReducedImage)
        {
            try
            {
                HObject ho_Circle,ho_Region;
                hv_ExpDefaultWinHandle = Window;

                ho_Image = My.ho_Image;
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
                HOperatorSet.GenEmptyObj(out ho_Region);

                Window.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, dReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                HOperatorSet.DispObj(ho_ReducedImage, Window);
                ho_Image = ho_ReducedImage;

                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_Image, out ho_Region, Graythreshold, 255);
                HOperatorSet.DispObj(ho_Region, hv_ExpDefaultWinHandle);
                
                Thread.Sleep(1);
            }
            catch
            {
                ho_ReducedImage = My.ho_Image;
            }
        }

        //抓圓
        public static void CatchCenter(HWindow Window, HObject ho_Image, HTuple dReduceRadius, HTuple dGraythreshold,
            HTuple dFirstCircleRadius, HTuple sGenParamValue, HTuple dLength, HTuple dWidth, HTuple dMeasureThreshold)
        {
            hv_ExpDefaultWinHandle = Window;
            HObject ho_Circle, ho_ReducedImage, ho_Region, ho_Connection;
            HObject ho_SelectedRegions0,ho_ModelContour,ho_MeasureContour,ho_Contour;
            HObject ho_CrossCenter, ho_Contours, ho_Cross, ho_UsedEdges, ho_ResultContours;
            
            HTuple hv_Radious0, hv_GraySetting, hv_FirstRadius, hv_Area0, hv_Row0, hv_Column0;
            HTuple hv_Area1, hv_Row1, hv_Column1, hv_MetrologyHandle, hv_GenParamValue;
            HTuple hv_circleIndices, hv_Row, hv_Column, hv_Length1, hv_Width1, hv_Measure_Threshold;
            HTuple hv_circleParameter, hv_UsedRow, hv_UsedColumn, hv_ResultRow,hv_ResultColumn;
            HTuple hv_ResultRadius, hv_StartPhi, hv_EndPhi, hv_PointOrder;
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
            try
            {
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = Window;
                //畫檢視範圍
                Window.ClearWindow();
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                ho_Circle.Dispose();
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                hv_GraySetting = dGraythreshold;
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_Region, hv_GraySetting, 255);
                ho_Connection.Dispose();
                HOperatorSet.Connection(ho_Region, out ho_Connection);
                hv_FirstRadius = dFirstCircleRadius;
                ho_SelectedRegions0.Dispose();
                HOperatorSet.SelectShape(ho_Connection, out ho_SelectedRegions0, (new HTuple("outer_radius")).TupleConcat(
                    "roundness"), "and", (new HTuple(0)).TupleConcat(0.8), ((hv_FirstRadius + 50)).TupleConcat(
                    1));
                HOperatorSet.AreaCenter(ho_SelectedRegions0, out hv_Area0, out hv_Row0, out hv_Column0);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions0, out ExpTmpOutVar_0, "area", "and",
                        hv_Area0.TupleMax(), 999999);
                    ho_SelectedRegions0.Dispose();
                    ho_SelectedRegions0 = ExpTmpOutVar_0;
                }
                HOperatorSet.AreaCenter(ho_SelectedRegions0, out hv_Area1, out hv_Row1, out hv_Column1);
                //找出圓心
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.AddMetrologyObjectGeneric(hv_MetrologyHandle, "circle", ((hv_Row1.TupleConcat(
                    hv_Column1))).TupleConcat(hv_FirstRadius), 25, 5, 1, 30, new HTuple(),
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
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                HOperatorSet.DispObj(ho_UsedEdges, hv_ExpDefaultWinHandle);
                ho_ResultContours.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_ResultContours, hv_MetrologyHandle,
                    "all", "all", 1.5);
                HOperatorSet.DispObj(ho_ResultContours, hv_ExpDefaultWinHandle);
                HOperatorSet.FitCircleContourXld(ho_ResultContours, "algebraic", -1, 0, 0,
                    3, 2, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius, out hv_StartPhi,
                    out hv_EndPhi, out hv_PointOrder);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
            }
            catch
            {
            }
        }

        //抓圓並顯示
        public static void CatchCenter2(HWindow Window, HObject ho_Image, HTuple dReduceRadius, HTuple dGraythreshold,
            HTuple dFirstCircleRadius, HTuple sGenParamValue, HTuple dLength, HTuple dWidth, HTuple dMeasureThreshold)
        {
            hv_ExpDefaultWinHandle = Window;
            HObject ho_Circle, ho_ReducedImage, ho_Region, ho_Connection;
            HObject ho_SelectedRegions0, ho_ModelContour, ho_MeasureContour, ho_Contour;
            HObject ho_CrossCenter, ho_Contours, ho_Cross, ho_UsedEdges, ho_ResultContours;

            HTuple hv_Radious0, hv_GraySetting, hv_FirstRadius, hv_Area0, hv_Row0, hv_Column0;
            HTuple hv_Area1, hv_Row1, hv_Column1, hv_MetrologyHandle, hv_GenParamValue;
            HTuple hv_circleIndices, hv_Row, hv_Column, hv_Length1, hv_Width1, hv_Measure_Threshold;
            HTuple hv_circleParameter, hv_UsedRow, hv_UsedColumn, hv_ResultRow, hv_ResultColumn;
            HTuple hv_ResultRadius, hv_StartPhi, hv_EndPhi, hv_PointOrder;
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
            try
            {
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = Window;
                //畫檢視範圍
                Window.ClearWindow();
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                ho_Circle.Dispose();
                hv_Radious0 = dReduceRadius;
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, hv_Radious0);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                hv_GraySetting = dGraythreshold;
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_Region, hv_GraySetting, 255);
                ho_Connection.Dispose();
                HOperatorSet.Connection(ho_Region, out ho_Connection);
                hv_FirstRadius = dFirstCircleRadius;
                ho_SelectedRegions0.Dispose();
                HOperatorSet.SelectShape(ho_Connection, out ho_SelectedRegions0, (new HTuple("outer_radius")).TupleConcat(
                    "roundness"), "and", (new HTuple(0)).TupleConcat(0.8), ((hv_FirstRadius + 50)).TupleConcat(
                    1));
                HOperatorSet.AreaCenter(ho_SelectedRegions0, out hv_Area0, out hv_Row0, out hv_Column0);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions0, out ExpTmpOutVar_0, "area", "and",
                        hv_Area0.TupleMax(), 999999);
                    ho_SelectedRegions0.Dispose();
                    ho_SelectedRegions0 = ExpTmpOutVar_0;
                }
                HOperatorSet.AreaCenter(ho_SelectedRegions0, out hv_Area1, out hv_Row1, out hv_Column1);
                //找出圓心
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.AddMetrologyObjectGeneric(hv_MetrologyHandle, "circle", ((hv_Row1.TupleConcat(
                    hv_Column1))).TupleConcat(hv_FirstRadius), 25, 5, 1, 30, new HTuple(),
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
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                HOperatorSet.DispObj(ho_UsedEdges, hv_ExpDefaultWinHandle);
                ho_ResultContours.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_ResultContours, hv_MetrologyHandle,
                    "all", "all", 1.5);
                HOperatorSet.DispObj(ho_ResultContours, hv_ExpDefaultWinHandle);
                HOperatorSet.FitCircleContourXld(ho_ResultContours, "algebraic", -1, 0, 0,
                    3, 2, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius, out hv_StartPhi,
                    out hv_EndPhi, out hv_PointOrder);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);

                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_ResultRow, hv_ResultColumn, hv_ResultRadius);
                Window.ClearWindow();

                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_ResultRow, hv_ResultColumn, 16,
                        0.785398);
                HOperatorSet.DispObj(My.ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_Circle, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_Cross, hv_ExpDefaultWinHandle);
                CenterRaw = hv_circleParameter.TupleSelect(0);
                CenterColumn = hv_circleParameter.TupleSelect(1);

            }
            catch
            {
            }
        }

        //SOMA辨識範圍
        public static void GenSomaDetectionRadius(HWindow Window, HObject ho_Image, HTuple dSomaReduceRadius, out HObject ho_SomaReducedImage)
        {
            try
            {
                HObject ho_Circle;
                hv_ExpDefaultWinHandle = Window;
                ho_Image = My.ho_Image;
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_SomaReducedImage);
                Window.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, CenterRaw, CenterColumn, dSomaReduceRadius);
                ho_SomaReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_SomaReducedImage);
                HOperatorSet.DispObj(ho_SomaReducedImage, Window);
                Thread.Sleep(1);
            }
            catch
            {
                ho_SomaReducedImage = My.ho_Image;
            }
        }

        //抓出SOMA灰度閥值
        public static void GrayThreshold2(HWindow Window, HObject ho_Image, HTuple dSomaReduceRadius, HTuple SomaGraythreshold, out HObject ho_SomaReducedImage)
        {
            try
            {
                HObject ho_Circle, ho_Region2, ho_RegionClosing2, ho_RegionFillUp2, ho_ConnectedRegions2, ho_SelectedRegions2;
                HTuple hv_Area2, hv_Row2, hv_Column2;
                ho_Image = My.ho_Image;
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_SomaReducedImage);
                HOperatorSet.GenEmptyObj(out ho_Region2);
                HOperatorSet.GenEmptyObj(out ho_RegionClosing2);
                HOperatorSet.GenEmptyObj(out ho_RegionFillUp2);
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
                HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);

                Window.ClearWindow();
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_SomaReducedImage);
                Window.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, CenterRaw, CenterColumn, dSomaReduceRadius);
                ho_SomaReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_SomaReducedImage);
                HOperatorSet.DispObj(ho_SomaReducedImage, Window);
                Thread.Sleep(1);
                //ho_Image = ho_ReducedImage;

                ho_Region2.Dispose();
                HOperatorSet.Threshold(ho_SomaReducedImage, out ho_Region2, 0, SomaGraythreshold);
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
                //顯示SOMA範圍
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                HOperatorSet.DispObj(ho_SelectedRegions2, hv_ExpDefaultWinHandle);

                Thread.Sleep(1);
                               

            }
            catch
            {
                ho_SomaReducedImage = My.ho_Image;
            }
        }

    }

}
