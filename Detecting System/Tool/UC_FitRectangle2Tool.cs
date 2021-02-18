using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalconDotNet;

namespace UC_FirCircle
{
    class UC_FitRectangle2Tool
    {
        public HWindow Window = new HWindow();
        public HObject ho_Image = new HObject();
        public HTuple hv_InitialRow = new HTuple();
        public HTuple hv_InitialColumn = new HTuple();
        public HTuple hv_InitialPhi = new HTuple();
        public HTuple hv_CenterRow = new HTuple();
        public HTuple hv_CenterColumn = new HTuple();
        public HTuple hv_CenterPhi = new HTuple();
        public HTuple hv_CenterLength1 = new HTuple();
        public HTuple hv_CenterLength2 = new HTuple();
    
        /// <summary>
        /// 方形長
        /// </summary>
        public  int length1 = 100;
        /// <summary>
        /// 方形寬
        /// </summary>
        public int length2 = 100;
        /// <summary>
        /// 極性設定
        /// </summary>
        public string measure_transition = "negative";
        /// <summary>
        /// 邊設定
        /// </summary>
        public string measure_select = "first";
        /// <summary>
        /// 卡尺數量設定
        /// </summary>
        public  int num_measures = 10;
        /// <summary>
        /// 卡尺高設定
        /// </summary>
        public  int measure_length1 = 30;
        /// <summary>
        /// 卡尺寬設定
        /// </summary>
        public  int measure_length2 = 5;
        /// <summary>
        /// 卡尺閥值
        /// </summary>
        public int measure_threshold = 40;


        public UC_FitRectangle2 ucFitRectangle2;


        #region halcon算子
        public void gen_rectangle2_center(HObject ho_Image, out HObject ho_UsedEdges, out HObject ho_Contour, out HObject ho_ResultContours, out HObject ho_CrossCenter, HTuple hv_InitialRow,
           HTuple hv_InitialColumn, HTuple hv_InitialPhi, HTuple hv_InitialLength1, HTuple hv_InitialLength2, HTuple hv_Num_Measure, HTuple hv_MeasureLength1, HTuple hv_MeasureLength2, HTuple hv_MeasureThreshold, HTuple hv_MeasureTransition, HTuple hv_MeasureSelect,
            out HTuple hv_ResultRow, out HTuple hv_ResultColumn, out HTuple hv_ResultPhi,out  HTuple hv_ResultLength1,out HTuple hv_ResultLength2)
        {
            // Local iconic variables 28

            HObject ho_ModelContour, ho_Contours;

            // Local control variables 

            HTuple hv_MetrologyHandle = null, hv_circleIndices = null;
            HTuple hv_Row = null, hv_Column = null;
            HTuple hv_UsedRow = null, hv_UsedColumn = null;
            HTuple hv_PointOrder = null;
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
                HOperatorSet.AddMetrologyObjectGeneric(hv_MetrologyHandle, "rectangle2", ((((((hv_InitialRow.TupleConcat(
                   hv_InitialColumn))).TupleConcat(hv_InitialPhi))).TupleConcat(hv_InitialLength1))).TupleConcat(
                   hv_InitialLength2), hv_MeasureLength1, hv_MeasureLength2, 1, hv_MeasureThreshold, new HTuple(),
                   new HTuple(), out hv_circleIndices);
                //ho_ModelContour.Dispose();
                //HOperatorSet.GetMetrologyObjectModelContour(out ho_ModelContour, hv_MetrologyHandle, "all", 1.5);
                //第一個點或最後一個點,或全部
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices, "measure_select", hv_MeasureSelect);
                //點數量
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices, "num_measures", hv_Num_Measure);
                //極性
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices, "measure_transition", hv_MeasureTransition);
                //最小分數
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_circleIndices, "min_score", 0.2);
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                //HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_circleIndices, "all", "result_type", "all_param", out hv_circleParameter);

                //白找黑('negative')或黑找白('positive')
                ho_Contour.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour, hv_MetrologyHandle, "all", hv_MeasureTransition, out hv_Row, out hv_Column);
                //ho_Contours.Dispose();
                //HOperatorSet.GetMetrologyObjectResultContour(out ho_Contours, hv_MetrologyHandle, "all", "all", 1.5);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "used_edges", "row", out hv_UsedRow);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "used_edges", "column", out hv_UsedColumn);
                ho_UsedEdges.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_UsedEdges, hv_UsedRow, hv_UsedColumn, 10, (new HTuple(45)).TupleRad());
                ho_ResultContours.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_ResultContours, hv_MetrologyHandle,"all", "all", 1.5);
                HOperatorSet.FitRectangle2ContourXld(ho_ResultContours, "regression", -1, 0,
                    0, 3, 2, out hv_ResultRow, out hv_ResultColumn, out hv_ResultPhi, out hv_ResultLength1,
                    out hv_ResultLength2, out hv_PointOrder);
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
       
        #endregion
        //public void SetValue(int Radius, int Measure_Transition, int Measure_Select, int Num_Measures, int Measure_Length1, int Measure_Length2)
        //{
        //    ucFitCircle.Radius = Radius;
        //    ucFitCircle.Measure_Transition = Measure_Transition;
        //    ucFitCircle.Measure_Select = Measure_Select;
        //    ucFitCircle.Num_Measures = Num_Measures;
        //    ucFitCircle.Measure_Length1 = Measure_Length1;
        //    ucFitCircle.Measure_Length2 = Measure_Length2;
        //}

        public UC_FitRectangle2Tool()
        {
            ucFitRectangle2 = new UC_FitRectangle2();
            ucFitRectangle2.SetChangedEvent += new UC_FitRectangle2.SetChangeHandler(Disp_FitRectangle2_1);
            ucFitRectangle2.DispResultEvent += new UC_FitRectangle2.DisplayResultHandler(Disp_FitRectangle2_2);
        }

        /// <summary>
        /// 调用委托事件
        /// </summary>
        public void Reconnect()
        {
            //con = new CogFitCircle();
            //con.setval(sigma, threshold, transition, selcet, lmeasure, hmeasure, rulenums, ruleheiht, rulewidth, check_mode);
            //ucFitCircle = new UC_FitCircle();
            ucFitRectangle2.SetChangedEvent += new UC_FitRectangle2.SetChangeHandler(Disp_FitRectangle2_1);
            ucFitRectangle2.DispResultEvent += new UC_FitRectangle2.DisplayResultHandler(Disp_FitRectangle2_2);
            //con1 = con;
        }
        /// <summary>
        /// 獲得圓心結果但不顯示結果時調用
        /// </summary>
        /// <param name="ho_ResultContours"></param>
        /// <param name="hv_CenterRow"></param>
        /// <param name="hv_CenterColumn"></param>
        /// <returns>返回抓圓心結果 1=成功 -1=失敗</returns>
        public int ImageProcess_FitRectangle2(HObject ho_Image, out HObject ho_ResultContours,out HObject ho_CrossCenter, out HTuple hv_CenterRow, out HTuple hv_CenterColumn,out HTuple hv_CenterPhi)
        {
            int iImageProcessResult = 0;
            HObject ho_UsedEdges = new HObject();
            HObject ho_Contour = new HObject();
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            hv_CenterRow = 0;
            hv_CenterColumn = 0;
            hv_CenterPhi = 0;
            length1 = ucFitRectangle2.Length1;
            length2 = ucFitRectangle2.Length2;
            num_measures = ucFitRectangle2.Num_Measures;
            measure_length1 = ucFitRectangle2.Measure_Length1;
            measure_length2 = ucFitRectangle2.Measure_Length2;
            measure_threshold = ucFitRectangle2.Measure_Threshold;
            measure_transition = ucFitRectangle2.Measure_Transition;
            measure_select = ucFitRectangle2.Measure_Select;
            try
            {
                ho_UsedEdges.Dispose(); ho_Contour.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_rectangle2_center(ho_Image, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out ho_CrossCenter,
                  hv_InitialRow, hv_InitialColumn, hv_InitialPhi, length1, length2, num_measures, measure_length1, measure_length2, measure_threshold, measure_transition, measure_select,
                  out hv_CenterRow, out hv_CenterColumn, out hv_CenterPhi, out hv_CenterLength1, out hv_CenterLength2);
            }
            catch
            {
            }
            if (hv_CenterRow != null)
                iImageProcessResult = 1;
            else
                iImageProcessResult = -1;
            return iImageProcessResult;
        }
        /// <summary>
        /// 顯示全部結果時調用
        /// </summary>
        public void Disp_FitRectangle2_1()
        {
            HObject ho_UsedEdges = new HObject();
            HObject ho_Contour = new HObject();
            HObject ho_ResultContours = new HObject();
            HObject ho_CrossCenter = new HObject();

            length1 = ucFitRectangle2.Length1;
            length2 = ucFitRectangle2.Length2;
            num_measures = ucFitRectangle2.Num_Measures;
            measure_length1 = ucFitRectangle2.Measure_Length1;
            measure_length2 = ucFitRectangle2.Measure_Length2;
            measure_threshold = ucFitRectangle2.Measure_Threshold;
            measure_transition = ucFitRectangle2.Measure_Transition;
            measure_select = ucFitRectangle2.Measure_Select;

            try
            {
                ho_UsedEdges.Dispose(); ho_Contour.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_rectangle2_center(ho_Image, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out ho_CrossCenter,
                 hv_InitialRow, hv_InitialColumn, hv_InitialPhi, length1, length2, num_measures, measure_length1, measure_length2, measure_threshold, measure_transition, measure_select,
                 out hv_CenterRow, out hv_CenterColumn, out hv_CenterPhi, out hv_CenterLength1, out hv_CenterLength2);

                Window.ClearWindow();
                ho_Image.DispObj(Window);
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
        /// <summary>
        /// 只顯示圓心時調用
        /// </summary>
        public void Disp_FitRectangle2_2()
        {
            HObject ho_UsedEdges = new HObject();
            HObject ho_Contour = new HObject();
            HObject ho_ResultContours = new HObject();
            HObject ho_CrossCenter = new HObject();

            length1 = ucFitRectangle2.Length1;
            length2 = ucFitRectangle2.Length2;
            num_measures = ucFitRectangle2.Num_Measures;
            measure_length1 = ucFitRectangle2.Measure_Length1;
            measure_length2 = ucFitRectangle2.Measure_Length2;
            measure_threshold = ucFitRectangle2.Measure_Threshold;
            measure_transition = ucFitRectangle2.Measure_Transition;
            measure_select = ucFitRectangle2.Measure_Select;
            try
            {
                ho_UsedEdges.Dispose(); ho_Contour.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_rectangle2_center(ho_Image, out ho_UsedEdges, out ho_Contour, out ho_ResultContours, out ho_CrossCenter,
                 hv_InitialRow, hv_InitialColumn, hv_InitialPhi, length1, length2, num_measures, measure_length1, measure_length2, measure_threshold, measure_transition, measure_select,
                 out hv_CenterRow, out hv_CenterColumn, out hv_CenterPhi, out hv_CenterLength1, out hv_CenterLength2);

                Window.ClearWindow();
                ho_Image.DispObj(Window);
                Window.SetDraw("margin");
                Window.SetColor("green");
                ho_ResultContours.DispObj(Window);
                ho_CrossCenter.DispObj(Window);
            }
            catch
            {
            }
        }
    }
}
