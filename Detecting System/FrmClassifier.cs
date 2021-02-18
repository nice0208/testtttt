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
    public partial class FrmClassifier : Form
    {
        FrmParent parent;
        FrmRun Run;
        public FrmClassifier(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;
            InitializeComponent();
        }
        #region 參數
        //無料參數
        public static int iNullthreshold = 1;
        public static int iNullArea_Upper = 1;
        public static int iNullArea_Lower = 1;
        //矩形參數
        public static int iRectangleSmooth = 1;
        public static int iRectangleLightDark = 0;
        public static int iRectangleOffSet = 1;
        public static int iRectangleDilation = 1;
        public static int iRectangleArea_Upper = 1;
        public static int iRectangleArea_Lower = 1;
        public static int iRectangleRoundness_Upper = 1;
        public static int iRectangleRoundness_Lower = 1;
        public static int iRectangleRectangularity_Upper = 1;
        public static int iRectangleRectangularity_Lower = 1;
        public static bool bDrawRectangle2 = false;
        public static HTuple dDrawRectangle2Row = 1;
        public static HTuple dDrawRectangle2Column = 1;
        public static HTuple dDrawRectangle2Phi = 1;
        public static HTuple dDrawRectangle2Length1 = 1;
        public static HTuple dDrawRectangle2Length2 = 1;
        public static int iRectangleLightDark2 = 0;
        public static int iRectanglePointChoice = 0;
        public static int iRectangleLength = 1;
        public static int iRectangleMeasureThreshold = 1;
        //圓參數
        public static int iCircleSmooth = 1;
        public static int iCircleLightDark = 0;
        public static int iCircleOffSet = 1;
        public static int iCircleClosing = 1;
        public static int iCircleArea_Upper = 1;
        public static int iCircleArea_Lower = 1;
        public static int iCircleRoundness_Upper = 1;
        public static int iCircleRoundness_Lower = 1;
        public static int iCircleRectangularity_Upper = 1;
        public static int iCircleRectangularity_Lower = 1;
        public static bool bDrawCircle = false;
        public static HTuple dDrawCircleRow = 1;
        public static HTuple dDrawCircleColumn = 1;
        public static HTuple dDrawCircleRadius = 1;
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
        public static int iMarkArea_Upper = 1;
        public static int iMarkArea_Lower = 1;
        public static int iMarkRoundness_Upper = 1;
        public static int iMarkRoundness_Lower = 1;
        public static int iMarkRectangularity_Upper = 1;
        public static int iMarkRectangularity_Lower = 1;
        #endregion
        #region Halcon參數1
        // Procedures 
        // External procedures 
        public void gen_circle_center(HObject ho_Image, out HObject ho_UsedEdges, out HObject ho_Contour,
     out HObject ho_ResultContours, out HObject ho_CrossCenter, HTuple hv_InitialRow,
     HTuple hv_InitialColumn, HTuple hv_InitialRadius, HTuple hv_Length, HTuple hv_Measure_Threshold,
     HTuple hv_GenParamValue, HTuple hv_PointChoice, out HTuple hv_ResultRow, out HTuple hv_ResultColumn,
     out HTuple hv_ResultRadius)
        {




            // Local iconic variables 

            HObject ho_ModelContour, ho_MeasureContour;
            HObject ho_Contours, ho_Cross;

            // Local control variables 

            HTuple hv_MetrologyHandle = null, hv_circleIndices = null;
            HTuple hv_Row = null, hv_Column = null, hv_circleParameter = null;
            HTuple hv_Row1 = null, hv_Column1 = null, hv_UsedRow = null;
            HTuple hv_UsedColumn = null, hv_StartPhi = null, hv_EndPhi = null;
            HTuple hv_PointOrder = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            HOperatorSet.GenEmptyObj(out ho_ModelContour);
            HOperatorSet.GenEmptyObj(out ho_MeasureContour);
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
                ho_Contours.Dispose();
                ho_Cross.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ModelContour.Dispose();
                ho_MeasureContour.Dispose();
                ho_Contours.Dispose();
                ho_Cross.Dispose();

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

        // Local procedures 
        public void gen_rectangle2_center(HObject ho_Image, out HObject ho_UsedEdges,
     out HObject ho_Contour, out HObject ho_ResultContours, out HObject ho_CrossCenter,
     HTuple hv_InitialRow, HTuple hv_InitialColumn, HTuple hv_InitialPhi, HTuple hv_InitialLength1,
     HTuple hv_InitialLength2, HTuple hv_Length, HTuple hv_Measure_Threshold, HTuple hv_GenParamValue,
     HTuple hv_PointChoice, out HTuple hv_ResultRow, out HTuple hv_ResultColumn,
     out HTuple hv_ResultPhi, out HTuple hv_ResultLength1, out HTuple hv_ResultLength2)
        {




            // Local iconic variables 

            HObject ho_ModelContour, ho_MeasureContour;
            HObject ho_Contours, ho_Cross;

            // Local control variables 

            HTuple hv_MetrologyHandle = null, hv_rectangle2Indices = null;
            HTuple hv_Row = null, hv_Column = null, hv_rectangle2Parameter = null;
            HTuple hv_Row1 = null, hv_Column1 = null, hv_UsedRow = null;
            HTuple hv_UsedColumn = null, hv_ResultPointOrder = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            HOperatorSet.GenEmptyObj(out ho_ModelContour);
            HOperatorSet.GenEmptyObj(out ho_MeasureContour);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.AddMetrologyObjectGeneric(hv_MetrologyHandle, "rectangle2", ((((((hv_InitialRow.TupleConcat(
                    hv_InitialColumn))).TupleConcat(hv_InitialPhi))).TupleConcat(hv_InitialLength1))).TupleConcat(
                    hv_InitialLength2), hv_Length, 5, 1, hv_Measure_Threshold, new HTuple(),
                    new HTuple(), out hv_rectangle2Indices);
                ho_ModelContour.Dispose();
                HOperatorSet.GetMetrologyObjectModelContour(out ho_ModelContour, hv_MetrologyHandle,
                    "all", 1.5);
                ho_MeasureContour.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureContour, hv_MetrologyHandle,
                    "all", "all", out hv_Row, out hv_Column);
                //白找黑('negative')或黑找白('positive')
                //set_metrology_object_param (MetrologyHandle, rectangle2Indices, 'measure_transition', GenParamValue)
                //第一個點或最後一個點
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_rectangle2Indices,
                    "measure_select", hv_PointChoice);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_rectangle2Indices,
                    "min_score", 0.2);
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                ho_Contour.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour, hv_MetrologyHandle,
                    "all", "positive", out hv_Row, out hv_Column);
                HOperatorSet.DispObj(ho_Contour, hv_ExpDefaultWinHandle);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_rectangle2Indices,
                    "all", "result_type", "all_param", out hv_rectangle2Parameter);
                ho_CrossCenter.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_CrossCenter, hv_rectangle2Parameter.TupleSelect(
                    0), hv_rectangle2Parameter.TupleSelect(1), 20, 0.785398);
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
                    40, (new HTuple(45)).TupleRad());
                HOperatorSet.DispObj(ho_UsedEdges, hv_ExpDefaultWinHandle);
                ho_ResultContours.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_ResultContours, hv_MetrologyHandle,
                    "all", "all", 1.5);
                HOperatorSet.DispObj(ho_ResultContours, hv_ExpDefaultWinHandle);
                HOperatorSet.FitRectangle2ContourXld(ho_ResultContours, "regression", -1, 0,
                    0, 3, 2, out hv_ResultRow, out hv_ResultColumn, out hv_ResultPhi, out hv_ResultLength1,
                    out hv_ResultLength2, out hv_ResultPointOrder);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);




                ho_ModelContour.Dispose();
                ho_MeasureContour.Dispose();
                ho_Contours.Dispose();
                ho_Cross.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ModelContour.Dispose();
                ho_MeasureContour.Dispose();
                ho_Contours.Dispose();
                ho_Cross.Dispose();

                throw HDevExpDefaultException;
            }
        }
        #endregion
        #region Halcon參數2
        HTuple hv_ExpDefaultWinHandle = new HTuple();
        // Local iconic variables 

        HObject ho_Image, ho_ImageMean1 = null, ho_RegionDynThresh = null, ho_ImageResult = null, ho_Region = null;
        HObject ho_RegionDilation = null, ho_RegionFillUp = null, ho_ConnectedRegions = null;
        HObject ho_SelectedRegions = null, ho_Contours = null, ho_RectangleUsedEdges = null;
        HObject ho_RectangleContours = null, ho_RectangleResultContour = null;
        HObject ho_RectangleCrossCenter = null, ho_Rectangle = null;
        HObject ho_ImageReduced = null, ho_RegionClosing = null, ho_CircleUsedEdges = null;
        HObject ho_CircleContours = null, ho_CircleResultContour = null;
        HObject ho_CircleCrossCenter = null, ho_Circle = null, ho_RegionDifference = null;
        HObject ho_Circle_Notch = null, ho_Mark = null;

        // Local control variables 

        HTuple hv_WindowHandle = new HTuple(), hv_Width = null;
        HTuple hv_Height = null, hv_ImageFiles = null, hv_i = null;
        HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
        HTuple hv_InitialRow = new HTuple(), hv_InitialColumn = new HTuple();
        HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
        HTuple hv_Length2 = new HTuple(), hv_PointOrder = new HTuple();
        HTuple hv_Rectangle2_Length1 = new HTuple(), hv_Rectangle2_Length2 = new HTuple();
        HTuple hv_Rectangle2_Length = new HTuple(), hv_Rectangle2_Measure_Threshold = new HTuple();
        HTuple hv_Rectangle2_GenParamValue = new HTuple(), hv_Rectangle2_PointChoice = new HTuple();
        HTuple hv_RectangleRow = new HTuple(), hv_RectangleColumn = new HTuple();
        HTuple hv_RectanglePhi = new HTuple(), hv_RectangleLength1 = new HTuple();
        HTuple hv_RectangleLength2 = new HTuple(), hv_Circle_Radius = new HTuple();
        HTuple hv_Circle_Length = new HTuple(), hv_Circle_Measure_Threshold = new HTuple();
        HTuple hv_Circle_GenParamValue = new HTuple(), hv_Circle_PointChoice = new HTuple();
        HTuple hv_CircleRow = new HTuple(), hv_CircleColumn = new HTuple();
        HTuple hv_CircleRadius = new HTuple(), hv_Number_Notch = new HTuple();
        HTuple hv_MarkRow = new HTuple(), hv_MarkColumn = new HTuple();
        HTuple hv_Direction = new HTuple(), hv_MarkDirection = new HTuple();
        HTuple hv_Produce = new HTuple();

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

        private void btnImageProPlus_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            DateTime t1 = DateTime.Now;
            
            ImageProPlus(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);
            DateTime t2 = DateTime.Now;
            TimeSpan t3 = t2.Subtract(t1);
            lblTime.Text = Math.Round(t3.TotalSeconds * 1000 + t3.TotalMilliseconds).ToString()+"ms";
        }

        private void btnImageSave_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
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
                    HOperatorSet.WriteImage(ho_Image, "png", 0, pathOK + "\\" + DateTime.Now.ToString("HH_mm_ss_f") + "OK");
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
                    HOperatorSet.WriteImage(ho_Image, "png", 0, pathNG + "\\" + DateTime.Now.ToString("HH_mm_ss_f") + "NG");
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
                    HOperatorSet.WriteImage(ho_Image, "png", 0, pathNG + "\\" + DateTime.Now.ToString("HH_mm_ss_f") + "NG2");
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
                    HOperatorSet.WriteImage(ho_Image, "png", 0, pathNG + "\\" + DateTime.Now.ToString("HH_mm_ss_f") + "Miss");
                }
            }
            if (Sys.OptionOriginal)
            {
                //儲存原始圖片
                string pathOriginal = Sys.ImageSavePath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + Sys.FunctionString + "\\" + Production.CurProduction + "\\Original";
                if (!Directory.Exists(pathOriginal))
                {
                    Directory.CreateDirectory(pathOriginal);
                }
                HOperatorSet.WriteImage(My.ho_Image, "png", 0, pathOriginal + "\\" + DateTime.Now.ToString("HH_mm_ss_f"));
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

        public void ImageProPlus(HWindow hWindowControl, HObject theImage, int n)
        {
            try
            {
                if (theImage == null)
                    return;
                //ho_Image.Dispose();
                HOperatorSet.CopyImage(theImage, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl;
                hWindowControl.ClearWindow();
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                ho_ImageResult.Dispose();
                HOperatorSet.GrayRangeRect(ho_Image, out ho_ImageResult, 10, 10);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageResult, out ho_Region, iNullthreshold, 255);
                HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row, out hv_Column);
                if (iNullArea_Lower > hv_Area || hv_Area > iNullArea_Upper)
                {
                    set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                    Vision.VisionResult[n] = "Miss";
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle,2300, 200);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                    HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                    WriteLog(n, Vision.VisionResult[n], My.Classifier.sProductionSet, "NA", "NA");
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
                    return;
                }
                set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_Image, out ho_ImageMean1, iRectangleSmooth, iRectangleSmooth);
                ho_RegionDynThresh.Dispose();
                if (iRectangleLightDark == 0)
                    HOperatorSet.DynThreshold(ho_Image, ho_ImageMean1, out ho_RegionDynThresh, iRectangleOffSet, "light");
                else
                    HOperatorSet.DynThreshold(ho_Image, ho_ImageMean1, out ho_RegionDynThresh, iRectangleOffSet, "dark");
                ho_RegionDilation.Dispose();
                HOperatorSet.DilationRectangle1(ho_RegionDynThresh, out ho_RegionDilation, iRectangleDilation, iRectangleDilation);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionDilation, out ho_RegionFillUp);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                if (My.Classifier.RectangleAreaSet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "area", "and", iRectangleArea_Lower, iRectangleArea_Upper);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                if (My.Classifier.RectangleRoundnessSet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "roundness", "and", iRectangleRoundness_Lower, iRectangleRoundness_Upper);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                if (My.Classifier.RectangleRectangularitySet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "rectangularity", "and", iRectangleRectangularity_Lower, iRectangleRectangularity_Upper);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", hv_Area.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_InitialRow, out hv_InitialColumn);
                //轉XLD
                ho_Contours.Dispose();
                HOperatorSet.GenContourRegionXld(ho_SelectedRegions, out ho_Contours, "border");
                //擬合矩形
                HOperatorSet.FitRectangle2ContourXld(ho_Contours, "regression", -1, 0, 0, 3, 2, out hv_Row, out hv_Column, out hv_Phi, out hv_Length1, out hv_Length2, out hv_PointOrder);
                hv_Rectangle2_Length1 = dDrawRectangle2Length1;
                hv_Rectangle2_Length2 = dDrawRectangle2Length2;
                hv_Rectangle2_Length = iRectangleLength;
                hv_Rectangle2_Measure_Threshold = iRectangleMeasureThreshold;
                if (iRectangleLightDark == 0)
                    hv_Rectangle2_GenParamValue = "positive";
                else
                    hv_Rectangle2_GenParamValue = "negative";
                if (iRectanglePointChoice == 0)
                    hv_Rectangle2_PointChoice = "first";
                else
                    hv_Rectangle2_PointChoice = "last";
                ho_RectangleUsedEdges.Dispose(); ho_RectangleContours.Dispose(); ho_RectangleResultContour.Dispose(); ho_RectangleCrossCenter.Dispose();
                gen_rectangle2_center(ho_Image, out ho_RectangleUsedEdges, out ho_RectangleContours,out ho_RectangleResultContour, out ho_RectangleCrossCenter, hv_InitialRow,
                    hv_InitialColumn, hv_Phi, hv_Rectangle2_Length1, hv_Rectangle2_Length2,hv_Rectangle2_Length, hv_Rectangle2_Measure_Threshold, hv_Rectangle2_GenParamValue,
                    hv_Rectangle2_PointChoice, out hv_RectangleRow, out hv_RectangleColumn,out hv_RectanglePhi, out hv_RectangleLength1, out hv_RectangleLength2);
            }
            catch
            {
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                Vision.VisionResult[n] = "NG3";
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 200);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "找不到矩形!");
                HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                WriteLog(n, Vision.VisionResult[n], My.Classifier.sProductionSet, "NA", "NA");
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
                ho_ImageResult.Dispose();
                ho_Region.Dispose();
                ho_ImageMean1.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionDilation.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Contours.Dispose();
                return;
            }
            try
            {
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
                //2.找圓形區域
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, iCircleSmooth, iCircleSmooth);
                ho_RegionDynThresh.Dispose();
                if (iCircleLightDark == 0)
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iCircleOffSet, "light");
                else
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iCircleOffSet, "dark");
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_RegionDynThresh, out ho_RegionClosing, iCircleClosing);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionClosing, out ho_RegionFillUp);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
                if (My.Classifier.CircleAreaSet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "area", "and", iCircleArea_Lower, iCircleArea_Upper);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                if (My.Classifier.CircleRoundnessSet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "roundness", "and", iCircleRoundness_Lower, iCircleRoundness_Upper);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                if (My.Classifier.CircleRectangularitySet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "rectangularity", "and", iCircleRectangularity_Lower, iCircleRectangularity_Upper);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", hv_Area.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_InitialRow, out hv_InitialColumn);
                hv_Circle_Radius = dDrawCircleRadius;
                hv_Circle_Length = iCircleLength;
                hv_Circle_Measure_Threshold = iCircleMeasureThreshold;
                if (iCircleLightDark == 0)
                    hv_Circle_GenParamValue = "positive";
                else
                    hv_Circle_GenParamValue = "negative";
                if (iCirclePointChoice == 0)
                    hv_Circle_PointChoice = "first";
                else
                    hv_Circle_PointChoice = "last";
                ho_CircleUsedEdges.Dispose(); ho_CircleContours.Dispose(); ho_CircleCrossCenter.Dispose();
                gen_circle_center(ho_ImageReduced, out ho_CircleUsedEdges, out ho_CircleContours, out ho_CircleResultContour, out ho_CircleCrossCenter, hv_InitialRow, hv_InitialColumn, hv_Circle_Radius,
                    hv_Circle_Length, hv_Circle_Measure_Threshold, hv_Circle_GenParamValue, hv_Circle_PointChoice, out hv_CircleRow, out hv_CircleColumn, out hv_CircleRadius);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_CircleRow, hv_CircleColumn, hv_CircleRadius);
                }
            catch
            {
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                HOperatorSet.DispObj(ho_RectangleResultContour, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_RectangleCrossCenter, hv_ExpDefaultWinHandle);
                Vision.VisionResult[n] = "NG3";
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 200);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "找不到圓形!");
                HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                WriteLog(n, Vision.VisionResult[n], My.Classifier.sProductionSet, "NA", "NA");
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
                ho_ImageResult.Dispose();
                ho_Region.Dispose();
                ho_ImageMean1.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionDilation.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Contours.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced.Dispose();
                ho_Circle.Dispose();
                return;
            }
            //找正反
            hv_Direction = "正";
            if ((int)(new HTuple(((hv_RectangleRow - hv_CircleRow)).TupleLess(0))) != 0)
            {
                hv_Direction = "正";
            }
            else
            {
                hv_Direction = "反";
            }
            try
            {
                //3.找出圓剪口,計算數量
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle, ho_SelectedRegions, out ho_RegionDifference);
                ho_Circle_Notch.Dispose();
                HOperatorSet.Connection(ho_RegionDifference, out ho_Circle_Notch);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_Circle_Notch, out ExpTmpOutVar_0, "area", "and", iNotchArea_Lower, iNotchArea_Upper);
                    ho_Circle_Notch.Dispose();
                    ho_Circle_Notch = ExpTmpOutVar_0;
                }
                HOperatorSet.CountObj(ho_Circle_Notch, out hv_Number_Notch);
            }
            catch
            {
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                HOperatorSet.DispObj(ho_RectangleResultContour, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_RectangleCrossCenter, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.DispObj(ho_CircleResultContour, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_CircleCrossCenter, hv_ExpDefaultWinHandle);
                Vision.VisionResult[n] = "NG3";
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 200);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "找不到剪口!");
                HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                WriteLog(n, Vision.VisionResult[n], My.Classifier.sProductionSet, "NA", hv_Direction);
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
                ho_ImageResult.Dispose();
                ho_Region.Dispose();
                ho_ImageMean1.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionDilation.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Contours.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced.Dispose();
                ho_Circle.Dispose();
                ho_RegionDifference.Dispose();
                ho_Circle_Notch.Dispose();
                return;
            }
            try
            {
                //4.找通氣孔位置(mark)
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Rectangle, ho_Circle, out ho_RegionDifference);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, iMarkSmooth, iMarkSmooth);
                ho_RegionDynThresh.Dispose();
                if (iMarkLightDark == 0)
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iMarkOffSet, "light");
                else
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iMarkOffSet, "dark");
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionDynThresh, out ho_ConnectedRegions);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_ConnectedRegions, out ho_Mark);

                if (My.Classifier.MarkAreaSet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_Mark, out ExpTmpOutVar_0, "area", "and", iMarkArea_Lower, iMarkArea_Upper);
                    ho_Mark.Dispose();
                    ho_Mark = ExpTmpOutVar_0;
                }
                if (My.Classifier.MarkRoundnessSet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_Mark, out ExpTmpOutVar_0, "roundness", "and", iMarkRoundness_Lower * 0.01, iMarkRoundness_Upper * 0.01);
                    ho_Mark.Dispose();
                    ho_Mark = ExpTmpOutVar_0;
                }
                if (My.Classifier.MarkRectangularitySet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_Mark, out ExpTmpOutVar_0, "rectangularity", "and", iMarkRectangularity_Lower * 0.01, iMarkRectangularity_Upper * 0.01);
                    ho_Mark.Dispose();
                    ho_Mark = ExpTmpOutVar_0;
                }
                HOperatorSet.AreaCenter(ho_Mark, out hv_Area, out hv_MarkRow, out hv_MarkColumn);
            }
            catch
            {
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                HOperatorSet.DispObj(ho_RectangleResultContour, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_RectangleCrossCenter, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.DispObj(ho_CircleResultContour, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_CircleCrossCenter, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_Circle_Notch, hv_ExpDefaultWinHandle);

                Vision.VisionResult[n] = "NG3";
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 200);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "找不到Mark!");
                HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                WriteLog(n, Vision.VisionResult[n], My.Classifier.sProductionSet, "NA", hv_Direction);
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
                ho_ImageResult.Dispose();
                ho_Region.Dispose();
                ho_ImageMean1.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionDilation.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Contours.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced.Dispose();
                ho_Circle.Dispose();
                ho_RegionDifference.Dispose();
                ho_Circle_Notch.Dispose();
                return;
            }
            //判斷
            //HZ=2剪口 Mark在左邊
            //DZ=1剪口 Mark在左邊
            //EZ=1剪口 Mark在右邊
            
            hv_MarkDirection = "left";
            if ((int)(new HTuple(hv_Direction.TupleEqual("正"))) != 0)
            {
                if ((int)(new HTuple(((hv_MarkColumn - hv_CircleColumn)).TupleGreater(0))) != 0)
                {
                    hv_MarkDirection = "Right";
                }
                else
                {
                    hv_MarkDirection = "Left";
                }
            }
            else
            {
                if ((int)(new HTuple(((hv_MarkColumn - hv_CircleColumn)).TupleGreater(0))) != 0)
                {
                    hv_MarkDirection = "Left";
                }
                else
                {
                    hv_MarkDirection = "Right";
                }
            }
            if ((int)(new HTuple(hv_Number_Notch.TupleEqual(1))) != 0)
            {
                hv_Produce = "DZ";
            }
            else if ((int)((new HTuple(hv_Number_Notch.TupleEqual(2))).TupleAnd(
                new HTuple(hv_MarkDirection.TupleEqual("Right")))) != 0)
            {
                hv_Produce = "EZ";
            }
            else if ((int)((new HTuple(hv_Number_Notch.TupleEqual(2))).TupleAnd(
                new HTuple(hv_MarkDirection.TupleEqual("Left")))) != 0)
            {
                hv_Produce = "HZ";
            }
            else
            {
                hv_Produce = "NG";
            }
            
            
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
            HOperatorSet.DispObj(ho_RectangleResultContour, hv_ExpDefaultWinHandle);
            HOperatorSet.DispObj(ho_RectangleCrossCenter, hv_ExpDefaultWinHandle);
            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
            HOperatorSet.DispObj(ho_CircleResultContour, hv_ExpDefaultWinHandle);
            HOperatorSet.DispObj(ho_CircleCrossCenter, hv_ExpDefaultWinHandle);
            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
            HOperatorSet.DispObj(ho_Circle_Notch, hv_ExpDefaultWinHandle);
            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
            HOperatorSet.DispObj(ho_Mark, hv_ExpDefaultWinHandle);
            set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
            HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
            HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "當前檢測機種:" + My.Classifier.sProductionSet);

            if ((int)(new HTuple(hv_Direction.TupleEqual("正"))) != 0)
            {
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
            }
            else
            {
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
            }
            HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 200);
            HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "方向:" + hv_Direction);
            if (My.Classifier.sProductionSet == hv_Produce)
            {
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
            }
            else
            {
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
            }
            HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2400, 200);
            HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "機種:" + hv_Produce);
            set_display_font(hv_ExpDefaultWinHandle, 40, "mono", "true", "false");
            if (My.Classifier.sProductionSet == hv_Produce && hv_Direction == "正")
            {
                Vision.VisionResult[n] = "OK";
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 1300);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "OK");
            }
            else if (My.Classifier.sProductionSet != hv_Produce)
            {
                Vision.VisionResult[n] = "NG";
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 1300);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "混料");
            }
            else if (hv_Direction != "正")
            {
                Vision.VisionResult[n] = "NG2";
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 1300);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "方向NG");
            }
            else
            {
                Vision.VisionResult[n] = "NG3";
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "orange");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 2300, 1300);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "未知NG");
            }
            //擷取當前畫面圖片為Image
            
            HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);

            WriteLog(n,Vision.VisionResult[n],My.Classifier.sProductionSet,hv_Produce,hv_Direction);
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
            ho_ImageResult.Dispose();
            ho_Region.Dispose();
            ho_ImageMean1.Dispose();
            ho_RegionDynThresh.Dispose();
            ho_RegionDilation.Dispose();
            ho_RegionFillUp.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_Contours.Dispose();
            ho_RectangleUsedEdges.Dispose();
            ho_RectangleContours.Dispose();
            ho_RectangleResultContour.Dispose();
            ho_RectangleCrossCenter.Dispose();
            ho_Rectangle.Dispose();
            ho_CircleUsedEdges.Dispose();
            ho_CircleContours.Dispose();
            ho_CircleResultContour.Dispose();
            ho_CircleCrossCenter.Dispose();
            ho_Circle.Dispose();
            ho_RegionDifference.Dispose();
            ho_Circle_Notch.Dispose();
            ho_Mark.Dispose();
        }

        public void WriteLog(int n, string ResultOK, string ProductionSet, string NowProduction, string Direction)
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
                        File.WriteAllText(Log, "Function\tCode\tTray Barcode_B\tTray No.\tToTal Count\tTray Barcode_A\tProgram ID\tProduct\tClass\tOperatorID\tMachine No.\tTime\tCT\tResult\tProductionSet\tNowProduction\tDirection" +
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
                                     My.Classifier.sProductionSet + "\t" +
                                     NowProduction + "\t" +
                                     Direction);
                    }
                
                }
                catch
                {
                }

            }
        }

        private void FrmClassify_Load(object sender, EventArgs e)
        {
            ReadPara();
        }
        
        public void ReadPara()
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ImageMean1);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThresh);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_RectangleUsedEdges);
            HOperatorSet.GenEmptyObj(out ho_RectangleContours);
            HOperatorSet.GenEmptyObj(out ho_RectangleResultContour);
            HOperatorSet.GenEmptyObj(out ho_RectangleCrossCenter);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_CircleUsedEdges);
            HOperatorSet.GenEmptyObj(out ho_CircleContours);
            HOperatorSet.GenEmptyObj(out ho_CircleResultContour);
            HOperatorSet.GenEmptyObj(out ho_CircleCrossCenter);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_Circle_Notch);
            HOperatorSet.GenEmptyObj(out ho_Mark);

            iNullthreshold = My.Classifier.iNullthreshold;
            iNullArea_Upper = My.Classifier.iNullArea_Upper;
            iNullArea_Lower = My.Classifier.iNullArea_Lower;
            iRectangleSmooth = My.Classifier.iRectangleSmooth;
            iRectangleLightDark = My.Classifier.iRectangleLightDark;
            iRectangleOffSet = My.Classifier.iRectangleOffSet;
            iRectangleDilation = My.Classifier.iRectangleDilation;
            iRectangleArea_Upper = My.Classifier.iRectangleArea_Upper;
            iRectangleArea_Lower = My.Classifier.iRectangleArea_Lower;
            iRectangleRoundness_Upper = My.Classifier.iRectangleRoundness_Upper;
            iRectangleRoundness_Lower = My.Classifier.iRectangleRoundness_Lower;
            iRectangleRectangularity_Upper = My.Classifier.iRectangleRectangularity_Upper;
            iRectangleRectangularity_Lower = My.Classifier.iRectangleRectangularity_Lower;
            iRectangleLightDark2 = My.Classifier.iRectangleLightDark2;
            iRectanglePointChoice = My.Classifier.iRectanglePointChoice;
            iRectangleLength = My.Classifier.iRectangleLength;
            iRectangleMeasureThreshold = My.Classifier.iRectangleMeasureThreshold;
            iCircleSmooth = My.Classifier.iCircleSmooth;
            iCircleLightDark = My.Classifier.iCircleLightDark;
            iCircleOffSet = My.Classifier.iCircleOffSet;
            iCircleClosing = My.Classifier.iCircleClosing;
            iCircleArea_Upper = My.Classifier.iCircleArea_Upper;
            iCircleArea_Lower = My.Classifier.iCircleArea_Lower;
            iCircleRoundness_Upper = My.Classifier.iCircleRoundness_Upper;
            iCircleRoundness_Lower = My.Classifier.iCircleRoundness_Lower;
            iCircleRectangularity_Upper = My.Classifier.iCircleRectangularity_Upper;
            iCircleRectangularity_Lower = My.Classifier.iCircleRectangularity_Lower;
            iCircleLightDark2 = My.Classifier.iCircleLightDark2;
            iCirclePointChoice = My.Classifier.iCirclePointChoice;
            iCircleLength = My.Classifier.iCircleLength;
            iCircleMeasureThreshold = My.Classifier.iCircleMeasureThreshold;
            iNotchArea_Upper = My.Classifier.iNotchArea_Upper;
            iNotchArea_Lower = My.Classifier.iNotchArea_Lower;
            iMarkSmooth = My.Classifier.iMarkSmooth;
            iMarkLightDark = My.Classifier.iMarkLightDark;
            iMarkOffSet = My.Classifier.iMarkOffSet;
            iMarkArea_Upper = My.Classifier.iMarkArea_Upper;
            iMarkArea_Lower = My.Classifier.iMarkArea_Lower;
            iMarkRoundness_Upper = My.Classifier.iMarkRoundness_Upper;
            iMarkRoundness_Lower = My.Classifier.iMarkRoundness_Lower;
            iMarkRectangularity_Upper = My.Classifier.iMarkRectangularity_Upper;
            iMarkRectangularity_Lower = My.Classifier.iMarkRectangularity_Lower;
            dDrawRectangle2Length1 = My.Classifier.dDrawRectangle2Length1;
            dDrawRectangle2Length2 = My.Classifier.dDrawRectangle2Length2;
            dDrawCircleRadius = My.Classifier.dDrawCircleRadius;

            cmbProduction.SelectedIndex  = My.Classifier.iProductionSet;
            switch (My.Classifier.iProductionSet)
            {
                case 0:
                    {
                        My.Classifier.sProductionSet = "DZ";
                        break;
                    }
                case 1:
                    {
                        My.Classifier.sProductionSet = "EZ";
                        break;
                    }
                case 2:
                    {
                        My.Classifier.sProductionSet = "HZ";
                        break;
                    }
            }
            nudNullthreshold.Value = (decimal)iNullthreshold;
            nudNullArea_Upper.Value = (decimal)iNullArea_Upper;
            nudNullArea_Lower.Value = (decimal)iNullArea_Lower;
            nudRectangleSmooth.Value = (decimal)iRectangleSmooth;
            cbRectangleLightDark.SelectedIndex = iRectangleLightDark;
            nudRectangleOffSet.Value = (decimal)iRectangleOffSet;
            nudRectangleDilation.Value = (decimal)iRectangleDilation;
            cbRectangleAreaSet.Checked = My.Classifier.RectangleAreaSet;
            cbRectangleRoundnessSet.Checked = My.Classifier.RectangleRoundnessSet;
            cbRectangleRectangularitySet.Checked = My.Classifier.RectangleRectangularitySet;
            nudRectangleArea_Upper.Value = (decimal)iRectangleArea_Upper;
            nudRectangleArea_Lower.Value = (decimal)iRectangleArea_Lower;
            nudRectangleRoundness_Upper.Value = (decimal)iRectangleRoundness_Upper;
            nudRectangleRoundness_Lower.Value = (decimal)iRectangleRoundness_Lower;
            nudRectangleRectangularity_Upper.Value = (decimal)iRectangleRectangularity_Upper;
            nudRectangleRectangularity_Lower.Value = (decimal)iRectangleRectangularity_Lower;
            cbRectangleLightDark2.SelectedIndex = iRectangleLightDark2;
            cbRectanglePointChoice.SelectedIndex = iRectanglePointChoice;
            nudRectangleLength.Value = (decimal)iRectangleLength;
            nudRectangleMeasureThreshold.Value = (decimal)iRectangleMeasureThreshold;
            nudCircleSmooth.Value = (decimal)iCircleSmooth;
            cbCircleLightDark.SelectedIndex = iCircleLightDark;
            nudCircleOffSet.Value = (decimal)iCircleOffSet;
            nudRectangleSmooth.Value = (decimal)iRectangleSmooth;
            nudCircleClosing.Value = (decimal)iCircleClosing;
            cbCircleAreaSet.Checked = My.Classifier.CircleAreaSet;
            cbCircleRoundnessSet.Checked = My.Classifier.CircleRoundnessSet;
            cbCircleRectangularitySet.Checked = My.Classifier.CircleRectangularitySet;
            nudCircleArea_Upper.Value = (decimal)iCircleArea_Upper;
            nudCircleArea_Lower.Value = (decimal)iCircleArea_Lower;
            nudCircleRoundness_Upper.Value = (decimal)iCircleRoundness_Upper;
            nudCircleRoundness_Lower.Value = (decimal)iCircleRoundness_Lower;
            nudCircleRectangularity_Upper.Value = (decimal)iCircleRectangularity_Upper;
            nudCircleRectangularity_Lower.Value = (decimal)iCircleRectangularity_Lower;
            cbCircleLightDark2.SelectedIndex = iCircleLightDark2;
            cbCirclePointChoice.SelectedIndex = iCirclePointChoice;
            nudCircleLength.Value = (decimal)iCircleLength;
            nudCircleMeasureThreshold.Value = (decimal)iCircleMeasureThreshold;
            nudNotchArea_Upper.Value = (decimal)iNotchArea_Upper;
            nudNotchArea_Lower.Value = (decimal)iNotchArea_Lower;
            nudMarkSmooth.Value = (decimal)iMarkSmooth;
            cbMarkLightDark.SelectedIndex = iMarkLightDark;
            nudMarkOffSet.Value = (decimal)iMarkOffSet;
            cbMarkAreaSet.Checked = My.Classifier.MarkAreaSet;
            cbMarkRoundnessSet.Checked = My.Classifier.MarkRoundnessSet;
            cbMarkRectangularitySet.Checked = My.Classifier.MarkRectangularitySet;
            nudMarkArea_Upper.Value = (decimal)iMarkArea_Upper;
            nudMarkArea_Lower.Value = (decimal)iMarkArea_Lower;
            nudMarkRoundness_Upper.Value = (decimal)iMarkRoundness_Upper;
            nudMarkRoundness_Lower.Value = (decimal)iMarkRoundness_Lower;
            nudMarkRectangularity_Upper.Value = (decimal)iMarkRectangularity_Upper;
            nudMarkRectangularity_Lower.Value = (decimal)iMarkRectangularity_Lower;
        }

        private void nudRectangleSmooth_ValueChanged(object sender, EventArgs e)
        {
            iRectangleSmooth = (int)nudRectangleSmooth.Value;

            if (My.ho_Image == null)
                return;
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_ImageResult.Dispose();
            HOperatorSet.GrayRangeRect(ho_Image, out ho_ImageResult, 10, 10);
            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_ImageResult, out ho_Region, iNullthreshold, 255);
            HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row, out hv_Column);
            HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
            HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Area:" + hv_Area);
            if (iNullArea_Lower > hv_Area || hv_Area > iNullArea_Upper)
            {
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1800, 200);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
            }
            else
            {
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_Image, out ho_ImageMean1, iRectangleSmooth, iRectangleSmooth);
                HOperatorSet.DispObj(ho_ImageMean1, hv_ExpDefaultWinHandle);
            }
            ho_Image.Dispose();
            ho_ImageResult.Dispose();
            ho_Region.Dispose();
            ho_ImageMean1.Dispose();
        }

        private void cbRectangleLightDark_SelectedIndexChanged(object sender, EventArgs e)
        {
            iRectangleLightDark = cbRectangleLightDark.SelectedIndex;
        }

        private void nudRectangleOffSet_ValueChanged(object sender, EventArgs e)
        {
            iRectangleOffSet = (int)nudRectangleOffSet.Value;

            if (My.ho_Image == null)
                return;
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_ImageResult.Dispose();
            HOperatorSet.GrayRangeRect(ho_Image, out ho_ImageResult, 10, 10);
            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_ImageResult, out ho_Region, iNullthreshold, 255);
            HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row, out hv_Column);
            HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
            HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Area:" + hv_Area);
            if (iNullArea_Lower > hv_Area || hv_Area > iNullArea_Upper)
            {
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1800, 200);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
            }
            else
            {
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_Image, out ho_ImageMean1, iRectangleSmooth, iRectangleSmooth);
                HOperatorSet.DispObj(ho_ImageMean1, hv_ExpDefaultWinHandle);

                ho_RegionDynThresh.Dispose();
                if (iRectangleLightDark == 0)
                    HOperatorSet.DynThreshold(ho_Image, ho_ImageMean1, out ho_RegionDynThresh, iRectangleOffSet, "light");
                else
                    HOperatorSet.DynThreshold(ho_Image, ho_ImageMean1, out ho_RegionDynThresh, iRectangleOffSet, "dark");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_RegionDynThresh, hv_ExpDefaultWinHandle);
            }
            ho_Image.Dispose();
            ho_ImageResult.Dispose();
            ho_Region.Dispose();
            ho_ImageMean1.Dispose();
            ho_RegionDynThresh.Dispose();
        }

        private void nudRectangleDilation_ValueChanged(object sender, EventArgs e)
        {
            iRectangleDilation = (int)nudRectangleDilation.Value;

            if (My.ho_Image == null)
                return;
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_ImageResult.Dispose();
            HOperatorSet.GrayRangeRect(ho_Image, out ho_ImageResult, 10, 10);
            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_ImageResult, out ho_Region, iNullthreshold, 255);
            HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row, out hv_Column);
            HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
            HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Area:" + hv_Area);
            if (iNullArea_Lower > hv_Area || hv_Area > iNullArea_Upper)
            {
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1800, 200);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
            }
            else
            {
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_Image, out ho_ImageMean1, iRectangleSmooth, iRectangleSmooth);
                ho_RegionDynThresh.Dispose();
                if (iRectangleLightDark == 0)
                    HOperatorSet.DynThreshold(ho_Image, ho_ImageMean1, out ho_RegionDynThresh, iRectangleOffSet, "light");
                else
                    HOperatorSet.DynThreshold(ho_Image, ho_ImageMean1, out ho_RegionDynThresh, iRectangleOffSet, "dark");
                ho_RegionDilation.Dispose();
                HOperatorSet.DilationRectangle1(ho_RegionDynThresh, out ho_RegionDilation, iRectangleDilation, iRectangleDilation);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_RegionDilation, hv_ExpDefaultWinHandle);
            }
            ho_Image.Dispose();
            ho_ImageMean1.Dispose();
            ho_RegionDynThresh.Dispose();
            ho_RegionDilation.Dispose();
        }

        private void btnRectangleSave_Click(object sender, EventArgs e)
        {
            My.Classifier.iRectangleSmooth = iRectangleSmooth;
            My.Classifier.iRectangleLightDark = iRectangleLightDark;
            My.Classifier.iRectangleOffSet = iRectangleOffSet;
            My.Classifier.iRectangleDilation = iRectangleDilation;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "RectangleSmooth", My.Classifier.iRectangleSmooth.ToString(), Path);
            IniFile.Write("Setting", "RectangleLightDark", My.Classifier.iRectangleLightDark.ToString(), Path);
            IniFile.Write("Setting", "RectangleOffSet", My.Classifier.iRectangleOffSet.ToString(), Path);
            IniFile.Write("Setting", "RectangleDilation", My.Classifier.iRectangleDilation.ToString(), Path);
        }

        private void tbRectangleArea_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudRectangleArea_Upper.Value = tbRectangleArea_Upper.Value;
        }

        private void nudRectangleArea_Upper_ValueChanged(object sender, EventArgs e)
        {
            iRectangleArea_Upper = tbRectangleArea_Upper.Value = (int)nudRectangleArea_Upper.Value;
            if (My.ho_Image == null)
                return;
            RectangleSetting();
        }

        private void tbRectangleArea_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudRectangleArea_Lower.Value = tbRectangleArea_Lower.Value;
        }

        private void nudRectangleArea_Lower_ValueChanged(object sender, EventArgs e)
        {
            iRectangleArea_Lower = tbRectangleArea_Lower.Value = (int)nudRectangleArea_Lower.Value;
            if (My.ho_Image == null)
                return;
            RectangleSetting();
        }

        private void tbRectangleRoundness_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudRectangleRoundness_Upper.Value = tbRectangleRoundness_Upper.Value;
        }

        private void nudRectangleRoundness_Upper_ValueChanged(object sender, EventArgs e)
        {
            iRectangleRoundness_Upper = tbRectangleRoundness_Upper.Value = (int)nudRectangleRoundness_Upper.Value;
            if (My.ho_Image == null)
                return;
            RectangleSetting();
        }

        private void tbRectangleRoundness_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudRectangleRoundness_Lower.Value = tbRectangleRoundness_Lower.Value;
        }

        private void nudRectangleRoundness_Lower_ValueChanged(object sender, EventArgs e)
        {
            iRectangleRoundness_Lower = tbRectangleRoundness_Lower.Value = (int)nudRectangleRoundness_Lower.Value;
            if (My.ho_Image == null)
                return;
            RectangleSetting();
        }

        private void tbRectangleRectangularity_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudRectangleRectangularity_Upper.Value = tbRectangleRectangularity_Upper.Value;
        }

        private void nudRectangleRectangularity_Upper_ValueChanged(object sender, EventArgs e)
        {
            iRectangleRectangularity_Upper = tbRectangleRectangularity_Upper.Value = (int)nudRectangleRectangularity_Upper.Value;
            if (My.ho_Image == null)
                return;
            RectangleSetting();
        }

        private void tbRectangleRectangularity_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudRectangleRectangularity_Lower.Value = tbRectangleRectangularity_Lower.Value;
        }

        private void nudRectangleRectangularity_Lower_ValueChanged(object sender, EventArgs e)
        {
            iRectangleRectangularity_Lower = tbRectangleRectangularity_Lower.Value = (int)nudRectangleRectangularity_Lower.Value;
            if (My.ho_Image == null)
                return;
            RectangleSetting();
        }

        public void RectangleSetting()
        {
            if (My.ho_Image == null)
                return;
            try
            {
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                ho_ImageResult.Dispose();
                HOperatorSet.GrayRangeRect(ho_Image, out ho_ImageResult, 10, 10);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageResult, out ho_Region, iNullthreshold, 255);
                HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row, out hv_Column);
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Area:" + hv_Area);
                if (iNullArea_Lower > hv_Area || hv_Area > iNullArea_Upper)
                {
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1800, 200);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                }
                else
                {
                    ho_ImageMean1.Dispose();
                    HOperatorSet.MeanImage(ho_Image, out ho_ImageMean1, iRectangleSmooth, iRectangleSmooth);
                    ho_RegionDynThresh.Dispose();
                    if (iRectangleLightDark == 0)
                        HOperatorSet.DynThreshold(ho_Image, ho_ImageMean1, out ho_RegionDynThresh, iRectangleOffSet, "light");
                    else
                        HOperatorSet.DynThreshold(ho_Image, ho_ImageMean1, out ho_RegionDynThresh, iRectangleOffSet, "dark");
                    ho_RegionDilation.Dispose();
                    HOperatorSet.DilationRectangle1(ho_RegionDynThresh, out ho_RegionDilation, iRectangleDilation, iRectangleDilation);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_RegionDilation, out ho_RegionFillUp);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                    if (My.Classifier.RectangleAreaSet)
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "area", "and", iRectangleArea_Lower, iRectangleArea_Upper);
                        ho_ConnectedRegions.Dispose();
                        ho_ConnectedRegions = ExpTmpOutVar_0;
                    }
                    if (My.Classifier.RectangleRoundnessSet)
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "roundness", "and", iRectangleRoundness_Lower, iRectangleRoundness_Upper);
                        ho_ConnectedRegions.Dispose();
                        ho_ConnectedRegions = ExpTmpOutVar_0;
                    }
                    if (My.Classifier.RectangleRectangularitySet)
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "rectangularity", "and", iRectangleRectangularity_Lower, iRectangleRectangularity_Upper);
                        ho_ConnectedRegions.Dispose();
                        ho_ConnectedRegions = ExpTmpOutVar_0;
                    }
                    HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", hv_Area.TupleMax(), 9999999);
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.DispObj(ho_SelectedRegions, hv_ExpDefaultWinHandle);
                }
            }
            catch
            {
            }
            finally
            {
                ho_Image.Dispose();
                ho_ImageResult.Dispose();
                ho_Region.Dispose();
                ho_ImageMean1.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionDilation.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
            }
        }

        private void cbRectangleAreaSet_CheckedChanged(object sender, EventArgs e)
        {
            My.Classifier.RectangleAreaSet = cbRectangleAreaSet.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "RectangleAreaSet", My.Classifier.RectangleAreaSet.ToString(), Path);
        }

        private void cbRectangleRoundnessSet_CheckedChanged(object sender, EventArgs e)
        {
            My.Classifier.RectangleRoundnessSet = cbRectangleRoundnessSet.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "RectangleRoundnessSet", My.Classifier.RectangleRoundnessSet.ToString(), Path);
        }

        private void cbRectangleRectangularitySet_CheckedChanged(object sender, EventArgs e)
        {
            My.Classifier.RectangleRectangularitySet = cbRectangleRectangularitySet.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "RectangleRectangularitySet", My.Classifier.RectangleRectangularitySet.ToString(), Path);
        }

        private void btnRectangleSave2_Click(object sender, EventArgs e)
        {
            My.Classifier.iRectangleArea_Upper = iRectangleArea_Upper;
            My.Classifier.iRectangleArea_Lower = iRectangleArea_Lower;
            My.Classifier.iRectangleRoundness_Upper = iRectangleRoundness_Upper;
            My.Classifier.iRectangleRoundness_Lower = iRectangleRoundness_Lower;
            My.Classifier.iRectangleRectangularity_Upper = iRectangleRectangularity_Upper;
            My.Classifier.iRectangleRectangularity_Lower = iRectangleRectangularity_Lower;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "RectangleArea_Upper", My.Classifier.iRectangleArea_Upper.ToString(), Path);
            IniFile.Write("Setting", "RectangleArea_Lower", My.Classifier.iRectangleArea_Lower.ToString(), Path);
            IniFile.Write("Setting", "RectangleRoundness_Upper", My.Classifier.iRectangleRoundness_Upper.ToString(), Path);
            IniFile.Write("Setting", "RectangleRoundness_Lower", My.Classifier.iRectangleRoundness_Lower.ToString(), Path);
            IniFile.Write("Setting", "RectangleRectangularity_Upper", My.Classifier.iRectangleRectangularity_Upper.ToString(), Path);
            IniFile.Write("Setting", "RectangleRectangularity_Lower", My.Classifier.iRectangleRectangularity_Lower.ToString(), Path);
        }

        private void btnDrawRectangle2_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            if (bDrawRectangle2)
            {
                return;
            } 
            if (My.ho_Image == null)
                return;
            bDrawRectangle2 = true;
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            //畫檢視範圍
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
            //找出初始長寬
            HOperatorSet.DrawRectangle2(hv_ExpDefaultWinHandle, out dDrawRectangle2Row, out dDrawRectangle2Column,out dDrawRectangle2Phi,out dDrawRectangle2Length1,out dDrawRectangle2Length2);
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
            HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle2(out ho_Rectangle,dDrawRectangle2Row,dDrawRectangle2Column,dDrawRectangle2Phi,dDrawRectangle2Length1,dDrawRectangle2Length2);
            HOperatorSet.DispObj(ho_Rectangle, hv_ExpDefaultWinHandle);
            if (dDrawRectangle2Length1 > dDrawRectangle2Length2)
            {
                My.Classifier.dDrawRectangle2Length1 = dDrawRectangle2Length1;
                My.Classifier.dDrawRectangle2Length2 = dDrawRectangle2Length2;
            }
            else
            {
                My.Classifier.dDrawRectangle2Length1 = dDrawRectangle2Length2;
                My.Classifier.dDrawRectangle2Length2 = dDrawRectangle2Length1;
            }
            dDrawRectangle2Length1 = My.Classifier.dDrawRectangle2Length1;
            dDrawRectangle2Length2 = My.Classifier.dDrawRectangle2Length2;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "DrawRectangle2Length1", My.Classifier.dDrawRectangle2Length1.ToString(), Path);
            IniFile.Write("Setting", "DrawRectangle2Length2", My.Classifier.dDrawRectangle2Length2.ToString(), Path);
          
            bDrawRectangle2 = false;
            ho_Image.Dispose();
            ho_Rectangle.Dispose();
        }

        private void cbRectangleLightDark2_SelectedIndexChanged(object sender, EventArgs e)
        {
            iRectangleLightDark2 = cbRectangleLightDark2.SelectedIndex;
            if (My.ho_Image == null)
                return; 
            RectangleSetting2();
        }

        private void cbRectanglePointChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            iRectanglePointChoice = cbRectanglePointChoice.SelectedIndex;
            if (My.ho_Image == null)
                return; 
            RectangleSetting2();
        }

        private void tbRectangleLength_ValueChanged(object sender, EventArgs e)
        {
            nudRectangleLength.Value = tbRectangleLength.Value;
        }

        private void nudRectangleLength_ValueChanged(object sender, EventArgs e)
        {
            iRectangleLength = tbRectangleLength.Value = (int)nudRectangleLength.Value;
            if (My.ho_Image == null)
                return;
            RectangleSetting2();
        }

        private void tbRectangleMeasureThreshold_ValueChanged(object sender, EventArgs e)
        {
            nudRectangleMeasureThreshold.Value = tbRectangleMeasureThreshold.Value;
        }

        private void nudRectangleMeasureThreshold_ValueChanged(object sender, EventArgs e)
        {
            iRectangleMeasureThreshold = tbRectangleMeasureThreshold.Value = (int)nudRectangleMeasureThreshold.Value;
            if (My.ho_Image == null)
                return;
            RectangleSetting2();
        }

        public void RectangleSetting2()
        {
            try
            {
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                ho_ImageResult.Dispose();
                HOperatorSet.GrayRangeRect(ho_Image, out ho_ImageResult, 10, 10);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageResult, out ho_Region, iNullthreshold, 255);
                HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row, out hv_Column);
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Area:" + hv_Area);
                if (iNullArea_Lower > hv_Area || hv_Area > iNullArea_Upper)
                {
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1800, 200);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                }
                else
                {
                    ho_ImageMean1.Dispose();
                    HOperatorSet.MeanImage(ho_Image, out ho_ImageMean1, iRectangleSmooth, iRectangleSmooth);
                    ho_RegionDynThresh.Dispose();
                    if (iRectangleLightDark == 0)
                        HOperatorSet.DynThreshold(ho_Image, ho_ImageMean1, out ho_RegionDynThresh, iRectangleOffSet, "light");
                    else
                        HOperatorSet.DynThreshold(ho_Image, ho_ImageMean1, out ho_RegionDynThresh, iRectangleOffSet, "dark");
                    ho_RegionDilation.Dispose();
                    HOperatorSet.DilationRectangle1(ho_RegionDynThresh, out ho_RegionDilation, iRectangleDilation, iRectangleDilation);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_RegionDilation, out ho_RegionFillUp);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                    if (My.Classifier.RectangleAreaSet)
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "area", "and", iRectangleArea_Lower, iRectangleArea_Upper);
                        ho_ConnectedRegions.Dispose();
                        ho_ConnectedRegions = ExpTmpOutVar_0;
                    }
                    if (My.Classifier.RectangleRoundnessSet)
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "roundness", "and", iRectangleRoundness_Lower * 0.01, iRectangleRoundness_Upper * 0.01);
                        ho_ConnectedRegions.Dispose();
                        ho_ConnectedRegions = ExpTmpOutVar_0;
                    }
                    if (My.Classifier.RectangleRectangularitySet)
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "rectangularity", "and", iRectangleRectangularity_Lower * 0.01, iRectangleRectangularity_Upper * 0.01);
                        ho_ConnectedRegions.Dispose();
                        ho_ConnectedRegions = ExpTmpOutVar_0;
                    }
                    HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", hv_Area.TupleMax(), 9999999);
                    HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_InitialRow, out hv_InitialColumn);
                    //轉XLD
                    ho_Contours.Dispose();
                    HOperatorSet.GenContourRegionXld(ho_SelectedRegions, out ho_Contours, "border");
                    //擬合矩形
                    HOperatorSet.FitRectangle2ContourXld(ho_Contours, "regression", -1, 0, 0, 3, 2, out hv_Row, out hv_Column, out hv_Phi, out hv_Length1, out hv_Length2, out hv_PointOrder);
                    hv_Rectangle2_Length1 = dDrawRectangle2Length1;
                    hv_Rectangle2_Length2 = dDrawRectangle2Length2;
                    hv_Rectangle2_Length = iRectangleLength;
                    hv_Rectangle2_Measure_Threshold = iRectangleMeasureThreshold;
                    if (iRectangleLightDark == 0)
                        hv_Rectangle2_GenParamValue = "positive";
                    else
                        hv_Rectangle2_GenParamValue = "negative";
                    if (iRectanglePointChoice == 0)
                        hv_Rectangle2_PointChoice = "first";
                    else
                        hv_Rectangle2_PointChoice = "last";
                    ho_RectangleUsedEdges.Dispose(); ho_RectangleContours.Dispose(); ho_RectangleResultContour.Dispose(); ho_RectangleCrossCenter.Dispose();
                    gen_rectangle2_center(ho_Image, out ho_RectangleUsedEdges, out ho_RectangleContours,
                        out ho_RectangleResultContour, out ho_RectangleCrossCenter, hv_InitialRow,
                        hv_InitialColumn, hv_Phi, hv_Rectangle2_Length1, hv_Rectangle2_Length2,
                        hv_Rectangle2_Length, hv_Rectangle2_Measure_Threshold, hv_Rectangle2_GenParamValue,
                        hv_Rectangle2_PointChoice, out hv_RectangleRow, out hv_RectangleColumn,
                        out hv_RectanglePhi, out hv_RectangleLength1, out hv_RectangleLength2);
                    HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");

                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                    HOperatorSet.DispObj(ho_RectangleUsedEdges, hv_ExpDefaultWinHandle);
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(ho_RectangleContours, hv_ExpDefaultWinHandle);
                    HOperatorSet.DispObj(ho_RectangleCrossCenter, hv_ExpDefaultWinHandle);
                }
            }
            catch
            {
            }
            finally
            {
                ho_Image.Dispose();
                ho_ImageResult.Dispose();
                ho_Region.Dispose();
                ho_ImageMean1.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionDilation.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Contours.Dispose();
                ho_RectangleUsedEdges.Dispose();
                ho_RectangleContours.Dispose();
                ho_RectangleResultContour.Dispose();
                ho_RectangleCrossCenter.Dispose();
            }
        }

        private void btnRectangleSave3_Click(object sender, EventArgs e)
        {
            My.Classifier.iRectangleLightDark2 = iRectangleLightDark2;
            My.Classifier.iRectanglePointChoice = iRectanglePointChoice;
            My.Classifier.iRectangleLength = iRectangleLength;
            My.Classifier.iRectangleMeasureThreshold = iRectangleMeasureThreshold;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "RectangleLightDark2", My.Classifier.iRectangleLightDark2.ToString(), Path);
            IniFile.Write("Setting", "RectanglePointChoice", My.Classifier.iRectanglePointChoice.ToString(), Path);
            IniFile.Write("Setting", "RectangleLength", My.Classifier.iRectangleLength.ToString(), Path);
            IniFile.Write("Setting", "RectangleMeasureThreshold", My.Classifier.iRectangleMeasureThreshold.ToString(), Path);
        }

        private void nudCircleSmooth_ValueChanged(object sender, EventArgs e)
        {
            iCircleSmooth = (int)nudCircleSmooth.Value;

            if (My.ho_Image == null)
                return;
            if (hv_RectangleRow == null)
            {
                MessageBox.Show("先求矩形區域!");
                return;
            }
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
            //2.找圓形區域
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
            ho_ImageMean1.Dispose();
            HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, iCircleSmooth, iCircleSmooth);
            HOperatorSet.DispObj(ho_ImageMean1, hv_ExpDefaultWinHandle);

            ho_Image.Dispose();
            ho_Rectangle.Dispose();
            ho_ImageReduced.Dispose();
            ho_ImageMean1.Dispose();
        }

        private void cbCircleLightDark_SelectedIndexChanged(object sender, EventArgs e)
        {
            iCircleLightDark = (int)cbCircleLightDark.SelectedIndex;

            if (My.ho_Image == null)
                return;
            if (hv_RectangleRow == null)
            {
                MessageBox.Show("先求矩形區域!");
                return;
            }
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
            //2.找圓形區域
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
            ho_ImageMean1.Dispose();
            HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, iCircleSmooth, iCircleSmooth);
            if (iCircleLightDark == 0)
                HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iCircleOffSet, "light");
            else
                HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iCircleOffSet, "dark");
            ho_RegionClosing.Dispose();
            HOperatorSet.ClosingCircle(ho_RegionDynThresh, out ho_RegionClosing, iCircleClosing);
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
            HOperatorSet.DispObj(ho_RegionClosing, hv_ExpDefaultWinHandle);

            
            
            HOperatorSet.DispObj(ho_ImageMean1, hv_ExpDefaultWinHandle);

            ho_Image.Dispose();
            ho_Rectangle.Dispose();
            ho_ImageReduced.Dispose();
            ho_ImageMean1.Dispose();
        }

        private void nudCircleOffSet_ValueChanged(object sender, EventArgs e)
        {
            iCircleOffSet = (int)nudCircleOffSet.Value;

            if (My.ho_Image == null)
                return;
            if (hv_RectangleRow == null)
            {
                MessageBox.Show("先求矩形區域!");
                return;
            }
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
            //2.找圓形區域
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
            ho_ImageMean1.Dispose();
            HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, iCircleSmooth, iCircleSmooth);
            ho_RegionDynThresh.Dispose();
            if (iCircleLightDark == 0)
                HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iCircleOffSet, "light");
            else
                HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iCircleOffSet, "dark");
            ho_RegionClosing.Dispose();
            HOperatorSet.ClosingCircle(ho_RegionDynThresh, out ho_RegionClosing, iCircleClosing);
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
            HOperatorSet.DispObj(ho_RegionClosing, hv_ExpDefaultWinHandle);

            ho_Image.Dispose();
            ho_Rectangle.Dispose();
            ho_ImageReduced.Dispose();
            ho_ImageMean1.Dispose();
            ho_RegionDynThresh.Dispose();
            ho_RegionClosing.Dispose();
        }

        private void nudCircleClosing_ValueChanged(object sender, EventArgs e)
        {
            iCircleClosing = (int)nudCircleClosing.Value;

            if (My.ho_Image == null)
                return;
            if (hv_RectangleRow == null)
            {
                MessageBox.Show("先求矩形區域!");
                return;
            }
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            hWindowControl1.HalconWindow.ClearWindow();
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
            //2.找圓形區域
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
            ho_ImageMean1.Dispose();
            HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, iCircleSmooth, iCircleSmooth);
            ho_RegionDynThresh.Dispose();
            if (iCircleLightDark == 0)
                HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iCircleOffSet, "light");
            else
                HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iCircleOffSet, "dark");
            ho_RegionClosing.Dispose();
            HOperatorSet.ClosingCircle(ho_RegionDynThresh, out ho_RegionClosing, iCircleClosing);
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
            HOperatorSet.DispObj(ho_RegionClosing, hv_ExpDefaultWinHandle);

            ho_Image.Dispose();
            ho_Rectangle.Dispose();
            ho_ImageReduced.Dispose();
            ho_ImageMean1.Dispose();
            ho_RegionDynThresh.Dispose();
            ho_RegionClosing.Dispose();
        }

        private void btnCircleSave_Click(object sender, EventArgs e)
        {
            My.Classifier.iCircleSmooth = iCircleSmooth;
            My.Classifier.iCircleLightDark = iCircleLightDark;
            My.Classifier.iCircleOffSet = iCircleOffSet;
            My.Classifier.iCircleClosing = iCircleClosing;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "CircleSmooth", My.Classifier.iCircleSmooth.ToString(), Path);
            IniFile.Write("Setting", "CircleLightDark", My.Classifier.iCircleLightDark.ToString(), Path);
            IniFile.Write("Setting", "CircleOffSet", My.Classifier.iCircleOffSet.ToString(), Path);
            IniFile.Write("Setting", "CircleClosing", My.Classifier.iCircleClosing.ToString(), Path);

        }

        private void btnSearchRectangle_Click(object sender, EventArgs e)
        {
            if (My.ho_Image == null)
                return;
            RectangleSetting2();
            try
            {
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                HOperatorSet.DispObj(ho_Rectangle, hv_ExpDefaultWinHandle);
            }
            catch
            {
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1800, 200);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "找不到矩形!");
            }
        }

        private void cbCircleAreaSet_CheckedChanged(object sender, EventArgs e)
        {
            My.Classifier.CircleAreaSet = cbCircleAreaSet.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "CircleAreaSet", My.Classifier.CircleAreaSet.ToString(), Path);
        }

        private void cbCircleRoundnessSet_CheckedChanged(object sender, EventArgs e)
        {
            My.Classifier.CircleRoundnessSet = cbCircleRoundnessSet.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "CircleRoundnessSet", My.Classifier.CircleRoundnessSet.ToString(), Path);
        }

        private void cbCircleRectangularitySet_CheckedChanged(object sender, EventArgs e)
        {
            My.Classifier.CircleRectangularitySet = cbCircleRectangularitySet.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "CircleRectangularitySet", My.Classifier.CircleRectangularitySet.ToString(), Path);
        }

        private void tbCircleArea_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudCircleArea_Upper.Value = tbCircleArea_Upper.Value;
        }

        private void nudCircleArea_Upper_ValueChanged(object sender, EventArgs e)
        {
            iCircleArea_Upper = tbCircleArea_Upper.Value = (int)nudCircleArea_Upper.Value;
            CircleSetting();
        }

        private void tbCircleArea_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudCircleArea_Upper.Value = tbCircleArea_Upper.Value;
        }

        private void nudCircleArea_Lower_ValueChanged(object sender, EventArgs e)
        {
            iCircleArea_Upper = tbCircleArea_Upper.Value = (int)nudCircleArea_Upper.Value;
            CircleSetting();
        }

        private void tbCircleRoundness_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudCircleRoundness_Upper.Value = tbCircleRoundness_Upper.Value;
        }

        private void nudCircleRoundness_Upper_ValueChanged(object sender, EventArgs e)
        {
            iCircleRoundness_Upper = tbCircleRoundness_Upper.Value = (int)nudCircleRoundness_Upper.Value;
            CircleSetting();
        }

        private void tbCircleRoundness_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudCircleRoundness_Upper.Value = tbCircleRoundness_Upper.Value;
        }

        private void nudCircleRoundness_Lower_ValueChanged(object sender, EventArgs e)
        {
            iCircleRoundness_Upper = tbCircleRoundness_Upper.Value = (int)nudCircleRoundness_Upper.Value;
            CircleSetting();
        }

        private void tbCircleRectangularity_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudCircleRectangularity_Upper.Value = tbCircleRectangularity_Upper.Value;
        }

        private void nudCircleRectangularity_Upper_ValueChanged(object sender, EventArgs e)
        {
            iCircleRectangularity_Upper = tbCircleRectangularity_Upper.Value = (int)nudCircleRectangularity_Upper.Value;
            CircleSetting();
        }

        private void tbCircleRectangularity_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudCircleRectangularity_Upper.Value = tbCircleRectangularity_Upper.Value;
        }

        private void nudCircleRectangularity_Lower_ValueChanged(object sender, EventArgs e)
        {
            iCircleRectangularity_Upper = tbCircleRectangularity_Upper.Value = (int)nudCircleRectangularity_Upper.Value;
            CircleSetting();
        }

        private void btnCircleSave2_Click(object sender, EventArgs e)
        {
            My.Classifier.iCircleArea_Upper = iCircleArea_Upper;
            My.Classifier.iCircleArea_Lower = iCircleArea_Lower;
            My.Classifier.iCircleRoundness_Upper = iCircleRoundness_Upper;
            My.Classifier.iCircleRoundness_Lower = iCircleRoundness_Lower;
            My.Classifier.iCircleRectangularity_Upper = iCircleRectangularity_Upper;
            My.Classifier.iCircleRectangularity_Lower = iCircleRectangularity_Lower;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "CircleArea_Upper", My.Classifier.iCircleArea_Upper.ToString(), Path);
            IniFile.Write("Setting", "CircleArea_Lower", My.Classifier.iCircleArea_Lower.ToString(), Path);
            IniFile.Write("Setting", "CircleRoundness_Upper", My.Classifier.iCircleRoundness_Upper.ToString(), Path);
            IniFile.Write("Setting", "CircleRoundness_Lower", My.Classifier.iCircleRoundness_Lower.ToString(), Path);
            IniFile.Write("Setting", "CircleRectangularity_Upper", My.Classifier.iCircleRectangularity_Upper.ToString(), Path);
            IniFile.Write("Setting", "CircleRectangularity_Lower", My.Classifier.iCircleRectangularity_Lower.ToString(), Path);
        }

        public void CircleSetting()
        {
            if (My.ho_Image == null)
                return;
            if (hv_RectangleRow == null)
                return;
            try
            {
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
                //2.找圓形區域
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, iCircleSmooth, iCircleSmooth);
                ho_RegionDynThresh.Dispose();
                if (iCircleLightDark == 0)
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iCircleOffSet, "light");
                else
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iCircleOffSet, "dark");
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_RegionDynThresh, out ho_RegionClosing, iCircleClosing);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionClosing, out ho_RegionFillUp);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
                if (My.Classifier.CircleAreaSet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "area", "and", iCircleArea_Lower, iCircleArea_Upper);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                if (My.Classifier.CircleRoundnessSet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "roundness", "and", iCircleRoundness_Lower, iCircleRoundness_Upper);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                if (My.Classifier.CircleRectangularitySet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "rectangularity", "and", iCircleRectangularity_Lower, iCircleRectangularity_Upper);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", hv_Area.TupleMax(), 9999999);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle,"green");
                HOperatorSet.DispObj(ho_SelectedRegions, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
            finally
            {
                ho_Image.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMean1.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
            }
        }

        private void btnDrawCircle_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            if (bDrawCircle)
                return;
            if (My.ho_Image == null)
                return;
            bDrawCircle = true;
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
            ho_Image.Dispose();
            HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
            //畫檢視範圍
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
            //找出初始長寬
            HOperatorSet.DrawCircle(hv_ExpDefaultWinHandle,out dDrawCircleRow,out dDrawCircleColumn,out dDrawCircleRadius);
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
            HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
            ho_Circle.Dispose();
            HOperatorSet.GenCircle(out ho_Circle, dDrawCircleRow, dDrawCircleColumn, dDrawCircleRadius);
            HOperatorSet.DispObj(ho_Circle, hv_ExpDefaultWinHandle);
            My.Classifier.dDrawCircleRadius = dDrawCircleRadius;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "DrawCircleRadius", My.Classifier.dDrawCircleRadius.ToString(), Path);
           
            bDrawCircle = false;
            ho_Image.Dispose();
            ho_Circle.Dispose();
        }

        private void cbCircleLightDark2_SelectedIndexChanged(object sender, EventArgs e)
        {
            iCircleLightDark2 = cbCircleLightDark2.SelectedIndex;
            CircleSetting2();
        }

        private void cbCirclePointChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            iCirclePointChoice = cbCirclePointChoice.SelectedIndex;
            CircleSetting2();
        }

        private void tbCircleLength_ValueChanged(object sender, EventArgs e)
        {
            nudCircleLength.Value = tbCircleLength.Value;
        }

        private void nudCircleLength_ValueChanged(object sender, EventArgs e)
        {
            iCircleLength = tbCircleLength.Value = (int)nudCircleLength.Value;
            CircleSetting2();
        }

        private void tbCircleMeasureThreshold_ValueChanged(object sender, EventArgs e)
        {
            nudCircleMeasureThreshold.Value = tbCircleMeasureThreshold.Value;
        }

        private void nudCircleMeasureThreshold_ValueChanged(object sender, EventArgs e)
        {
            iCircleMeasureThreshold = tbCircleMeasureThreshold.Value = (int)nudCircleMeasureThreshold.Value;
            CircleSetting2();
        }

        private void btnCircleSave3_Click(object sender, EventArgs e)
        {
            My.Classifier.iCircleLightDark2 = iCircleLightDark2;
            My.Classifier.iCirclePointChoice = iCirclePointChoice;
            My.Classifier.iCircleLength = iCircleLength;
            My.Classifier.iCircleMeasureThreshold = iCircleMeasureThreshold;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "CircleLightDark2", My.Classifier.iCircleLightDark2.ToString(), Path);
            IniFile.Write("Setting", "CirclePointChoice", My.Classifier.iCirclePointChoice.ToString(), Path);
            IniFile.Write("Setting", "CircleLength", My.Classifier.iCircleLength.ToString(), Path);
            IniFile.Write("Setting", "CircleMeasureThreshold", My.Classifier.iCircleMeasureThreshold.ToString(), Path);
        }

        public void CircleSetting2()
        {
            if (My.ho_Image == null)
                return;
            if (hv_RectangleRow == null)
                return;
            try
            {
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
                //2.找圓形區域
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, iCircleSmooth, iCircleSmooth);
                ho_RegionDynThresh.Dispose();
                if (iCircleLightDark == 0)
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iCircleOffSet, "light");
                else
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iCircleOffSet, "dark");
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_RegionDynThresh, out ho_RegionClosing, iCircleClosing);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionClosing, out ho_RegionFillUp);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
                if (My.Classifier.CircleAreaSet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "area", "and", iCircleArea_Lower, iCircleArea_Upper);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                if (My.Classifier.CircleRoundnessSet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "roundness", "and", iCircleRoundness_Lower * 0.01, iCircleRoundness_Upper * 0.01);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                if (My.Classifier.CircleRectangularitySet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "rectangularity", "and", iCircleRectangularity_Lower * 0.01, iCircleRectangularity_Upper * 0.01);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", hv_Area.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_InitialRow,out hv_InitialColumn);
                hv_Circle_Radius = dDrawCircleRadius;
                hv_Circle_Length = iCircleLength;
                hv_Circle_Measure_Threshold = iCircleMeasureThreshold;
                if (iCircleLightDark == 0)
                    hv_Circle_GenParamValue = "positive";
                else
                    hv_Circle_GenParamValue = "negative";
                if (iCirclePointChoice == 0)
                    hv_Circle_PointChoice = "first";
                else
                    hv_Circle_PointChoice = "last";
                ho_CircleUsedEdges.Dispose(); ho_CircleContours.Dispose(); ho_CircleCrossCenter.Dispose();
                gen_circle_center(ho_ImageReduced, out ho_CircleUsedEdges, out ho_CircleContours,out ho_CircleResultContour,out ho_CircleCrossCenter, hv_InitialRow, hv_InitialColumn, hv_Circle_Radius,
                    hv_Circle_Length, hv_Circle_Measure_Threshold, hv_Circle_GenParamValue,hv_Circle_PointChoice, out hv_CircleRow, out hv_CircleColumn, out hv_CircleRadius);
                ho_Circle.Dispose();
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_CircleContours, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                HOperatorSet.DispObj(ho_CircleUsedEdges, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.DispObj(ho_CircleResultContour, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_CircleCrossCenter, hv_ExpDefaultWinHandle);
            }
            catch
            {
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_CircleContours, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                HOperatorSet.DispObj(ho_CircleUsedEdges, hv_ExpDefaultWinHandle);
            }
            finally
            {
                ho_Image.Dispose();
                ho_ImageMean1.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionDilation.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Contours.Dispose();
                ho_CircleUsedEdges.Dispose();
                ho_CircleContours.Dispose();
                ho_CircleResultContour.Dispose();
                ho_CircleCrossCenter.Dispose();
            }
        }

        private void tbNotchArea_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudNotchArea_Upper.Value = tbNotchArea_Upper.Value;
        }

        private void nudNotchArea_Upper_ValueChanged(object sender, EventArgs e)
        {
            iNotchArea_Upper = tbNotchArea_Upper.Value = (int)nudNotchArea_Upper.Value;
            NotchSetting();
        }

        private void tbNotchArea_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudNotchArea_Lower.Value = tbNotchArea_Lower.Value;
        }

        private void nudNotchArea_Lower_ValueChanged(object sender, EventArgs e)
        {
            iNotchArea_Lower = tbNotchArea_Lower.Value = (int)nudNotchArea_Lower.Value;
            NotchSetting();
        }

        private void btnNotchSave_Click(object sender, EventArgs e)
        {
            My.Classifier.iNotchArea_Upper = iNotchArea_Upper;
            My.Classifier.iNotchArea_Lower = iNotchArea_Lower;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "NotchArea_Lower", My.Classifier.iNotchArea_Lower.ToString(), Path);
            IniFile.Write("Setting", "NotchArea_Upper", My.Classifier.iNotchArea_Upper.ToString(), Path);
        }

        public void NotchSetting()
        {
            if (My.ho_Image == null)
                return;
            if (hv_RectangleRow == null)
                return;
            try
            {
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
                //2.找圓形區域
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, iCircleSmooth, iCircleSmooth);
                ho_RegionDynThresh.Dispose();
                if (iCircleLightDark == 0)
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iCircleOffSet, "light");
                else
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iCircleOffSet, "dark");
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_RegionDynThresh, out ho_RegionClosing, iCircleClosing);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionClosing, out ho_RegionFillUp);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
                if (My.Classifier.CircleAreaSet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "area", "and", iCircleArea_Lower, iCircleArea_Upper);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                if (My.Classifier.CircleRoundnessSet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "roundness", "and", iCircleRoundness_Lower, iCircleRoundness_Upper);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                if (My.Classifier.CircleRectangularitySet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ExpTmpOutVar_0, "rectangularity", "and", iCircleRectangularity_Lower, iCircleRectangularity_Upper);
                    ho_ConnectedRegions.Dispose();
                    ho_ConnectedRegions = ExpTmpOutVar_0;
                }
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", hv_Area.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_InitialRow,out hv_InitialColumn);
                hv_Circle_Radius = dDrawCircleRadius;
                hv_Circle_Length = iCircleLength;
                hv_Circle_Measure_Threshold = iCircleMeasureThreshold;
                if (iCircleLightDark == 0)
                    hv_Circle_GenParamValue = "positive";
                else
                    hv_Circle_GenParamValue = "negative";
                if (iCirclePointChoice == 0)
                    hv_Circle_PointChoice = "first";
                else
                    hv_Circle_PointChoice = "last";
                ho_CircleUsedEdges.Dispose(); ho_CircleContours.Dispose(); ho_CircleCrossCenter.Dispose();
                gen_circle_center(ho_ImageReduced, out ho_CircleUsedEdges, out ho_CircleContours,out ho_CircleResultContour,out ho_CircleCrossCenter, hv_InitialRow, hv_InitialColumn, hv_Circle_Radius,
                    hv_Circle_Length, hv_Circle_Measure_Threshold, hv_Circle_GenParamValue,hv_Circle_PointChoice, out hv_CircleRow, out hv_CircleColumn, out hv_CircleRadius);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_CircleRow, hv_CircleColumn, hv_CircleRadius);
                //3.找出圓剪口,計算數量
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Circle, ho_SelectedRegions, out ho_RegionDifference);
                ho_Circle_Notch.Dispose();
                HOperatorSet.Connection(ho_RegionDifference, out ho_Circle_Notch);
                {
                HObject ExpTmpOutVar_0;
                HOperatorSet.SelectShape(ho_Circle_Notch, out ExpTmpOutVar_0, "area", "and", iNotchArea_Lower, iNotchArea_Upper);
                ho_Circle_Notch.Dispose();
                ho_Circle_Notch = ExpTmpOutVar_0;
                }
                HOperatorSet.CountObj(ho_Circle_Notch, out hv_Number_Notch);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_Circle_Notch, hv_ExpDefaultWinHandle);
                set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "圓剪口數量:" + hv_Number_Notch);
            }
            catch
            {
            }
            finally
            {
                ho_Image.Dispose();
                ho_ImageMean1.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionDilation.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Contours.Dispose();
                ho_CircleUsedEdges.Dispose();
                ho_CircleContours.Dispose();
                ho_CircleCrossCenter.Dispose();
                ho_Circle.Dispose();
                ho_RegionDifference.Dispose();
                ho_Circle_Notch.Dispose();
            }
        }

        private void btnSearchMarkRange_Click(object sender, EventArgs e)
        {
            if (My.ho_Image == null)
                return;
            try
            {
                RectangleSetting2();
                CircleSetting2();
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_CircleRow, hv_CircleColumn, hv_CircleRadius);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Rectangle, ho_Circle, out ho_RegionDifference);

                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "cyan");
                HOperatorSet.DispObj(ho_RegionDifference, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
            finally
            {
                ho_Rectangle.Dispose();
                ho_Circle.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
            }
        }

        private void nudMarkSmooth_ValueChanged(object sender, EventArgs e)
        {
            iMarkSmooth = (int)nudMarkSmooth.Value;
            if (My.ho_Image == null)
                return;
            if (hv_RectangleRow == null || hv_CircleRow == null)
                return;
            try
            {
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_CircleRow, hv_CircleColumn, hv_CircleRadius);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Rectangle, ho_Circle, out ho_RegionDifference);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, iMarkSmooth, iMarkSmooth);
                HOperatorSet.DispObj(ho_ImageMean1, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
            finally
            {
                ho_Rectangle.Dispose();
                ho_Circle.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMean1.Dispose();
            }
        }

        private void cbMarkLightDark_SelectedIndexChanged(object sender, EventArgs e)
        {
            iMarkLightDark = (int)cbMarkLightDark.SelectedIndex;
            if (My.ho_Image == null)
                return;
            if (hv_RectangleRow == null || hv_CircleRow == null)
                return;
            try
            {
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_CircleRow, hv_CircleColumn, hv_CircleRadius);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Rectangle, ho_Circle, out ho_RegionDifference);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, iMarkSmooth, iMarkSmooth);
                HOperatorSet.DispObj(ho_ImageMean1, hv_ExpDefaultWinHandle);
                ho_RegionDynThresh.Dispose();
                if (iMarkLightDark==0)
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh,iMarkOffSet, "light");
                else
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iMarkOffSet, "dark");
                HOperatorSet.DispObj(ho_ImageReduced, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_RegionDynThresh,hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
            finally
            {
                ho_Rectangle.Dispose();
                ho_Circle.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMean1.Dispose();
            }
        }

        private void nudMarkOffSet_ValueChanged(object sender, EventArgs e)
        {
            iMarkOffSet = (int)nudMarkOffSet.Value;
            if (My.ho_Image == null)
                return;
            if (hv_RectangleRow == null || hv_CircleRow == null)
                return;
            try
            {
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_CircleRow, hv_CircleColumn, hv_CircleRadius);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Rectangle, ho_Circle, out ho_RegionDifference);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, iMarkSmooth, iMarkSmooth);
                HOperatorSet.DispObj(ho_ImageMean1, hv_ExpDefaultWinHandle);
                ho_RegionDynThresh.Dispose();
                if (iMarkLightDark == 0)
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iMarkOffSet, "light");
                else
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iMarkOffSet, "dark");
                HOperatorSet.DispObj(ho_ImageReduced, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_RegionDynThresh, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
            finally
            {
                ho_Rectangle.Dispose();
                ho_Circle.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMean1.Dispose();
            }
        }

        private void btnMarkSave_Click(object sender, EventArgs e)
        {
            My.Classifier.iMarkSmooth = iMarkSmooth;
            My.Classifier.iMarkLightDark = iMarkLightDark;
            My.Classifier.iMarkOffSet = iMarkOffSet;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "MarkSmooth", My.Classifier.iMarkSmooth.ToString(), Path);
            IniFile.Write("Setting", "MarkLightDark", My.Classifier.iMarkLightDark.ToString(), Path);
            IniFile.Write("Setting", "MarkOffSet", My.Classifier.iMarkOffSet.ToString(), Path);
        }

        private void cbMarkAreaSet_CheckedChanged(object sender, EventArgs e)
        {
            My.Classifier.MarkAreaSet = cbMarkAreaSet.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "MarkAreaSet", My.Classifier.MarkAreaSet.ToString(), Path);
        }

        private void cbMarkRoundnessSet_CheckedChanged(object sender, EventArgs e)
        {
            My.Classifier.MarkRoundnessSet = cbMarkRoundnessSet.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "MarkRoundnessSet", My.Classifier.MarkRoundnessSet.ToString(), Path);
        }

        private void cbMarkRectangularitySet_CheckedChanged(object sender, EventArgs e)
        {
            My.Classifier.MarkRectangularitySet = cbMarkRectangularitySet.Checked ? true : false;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "MarkRectangularitySet", My.Classifier.MarkRectangularitySet.ToString(), Path);
        }

        private void tbMarkArea_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudMarkArea_Upper.Value = tbMarkArea_Upper.Value;
        }

        private void nudMarkArea_Upper_ValueChanged(object sender, EventArgs e)
        {
            iMarkArea_Upper = tbMarkArea_Upper.Value = (int)nudMarkArea_Upper.Value;
            MarkSetting();
        }

        private void tbMarkArea_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudMarkArea_Lower.Value = tbMarkArea_Lower.Value;
        }

        private void nudMarkArea_Lower_ValueChanged(object sender, EventArgs e)
        {
            iMarkArea_Lower = tbMarkArea_Lower.Value = (int)nudMarkArea_Lower.Value;
            MarkSetting();
        }

        private void tbMarkRoundness_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudMarkRoundness_Upper.Value = tbMarkRoundness_Upper.Value;
        }

        private void nudMarkRoundness_Upper_ValueChanged(object sender, EventArgs e)
        {
            iMarkRoundness_Upper = tbMarkRoundness_Upper.Value = (int)nudMarkRoundness_Upper.Value;
            MarkSetting();
        }

        private void tbMarkRoundness_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudMarkRoundness_Lower.Value = tbMarkRoundness_Lower.Value;
        }

        private void nudMarkRoundness_Lower_ValueChanged(object sender, EventArgs e)
        {
            iMarkRoundness_Lower = tbMarkRoundness_Lower.Value = (int)nudMarkRoundness_Lower.Value;
            MarkSetting();
        }

        private void tbMarkRectangularity_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudMarkRectangularity_Upper.Value = tbMarkRectangularity_Upper.Value;
        }

        private void nudMarkRectangularity_Upper_ValueChanged(object sender, EventArgs e)
        {
            iMarkRectangularity_Upper = tbMarkRectangularity_Upper.Value = (int)nudMarkRectangularity_Upper.Value;
            MarkSetting();
        }

        private void tbMarkRectangularity_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudMarkRectangularity_Lower.Value = tbMarkRectangularity_Lower.Value;
        }

        private void nudMarkRectangularity_Lower_ValueChanged(object sender, EventArgs e)
        {
            iMarkRectangularity_Lower = tbMarkRectangularity_Lower.Value = (int)nudMarkRectangularity_Lower.Value;
            MarkSetting();
        }

        private void btnMarkSave2_Click(object sender, EventArgs e)
        {
            My.Classifier.iMarkArea_Upper = iMarkArea_Upper;
            My.Classifier.iMarkArea_Lower = iMarkArea_Lower;
            My.Classifier.iMarkRoundness_Upper = iMarkRoundness_Upper;
            My.Classifier.iMarkRoundness_Lower = iMarkRoundness_Lower;
            My.Classifier.iMarkRectangularity_Upper = iMarkRectangularity_Upper;
            My.Classifier.iMarkRectangularity_Lower = iMarkRectangularity_Lower;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "MarkArea_Upper", My.Classifier.iMarkArea_Upper.ToString(), Path);
            IniFile.Write("Setting", "MarkArea_Lower", My.Classifier.iMarkArea_Lower.ToString(), Path);
            IniFile.Write("Setting", "MarkRoundness_Upper", My.Classifier.iMarkRoundness_Upper.ToString(), Path);
            IniFile.Write("Setting", "MarkRoundness_Lower", My.Classifier.iMarkRoundness_Lower.ToString(), Path);
            IniFile.Write("Setting", "MarkRectangularity_Upper", My.Classifier.iMarkRectangularity_Upper.ToString(), Path);
            IniFile.Write("Setting", "MarkRectangularity_Lower", My.Classifier.iMarkRectangularity_Lower.ToString(), Path);
        }

        public void MarkSetting()
        {
            if (My.ho_Image == null)
                return;
            if (hv_RectangleRow == null)
                return;
            if (hv_CircleRow == null)
                return;
            try
            {
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_RectangleRow, hv_RectangleColumn, hv_RectanglePhi, hv_RectangleLength1, hv_RectangleLength2);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_CircleRow, hv_CircleColumn, hv_CircleRadius);
                //4.找通氣孔位置(mark)
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Rectangle, ho_Circle, out ho_RegionDifference);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced);
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean1, iMarkSmooth, iMarkSmooth);
                ho_RegionDynThresh.Dispose();
                if(iMarkLightDark==0)
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh,iMarkOffSet, "light");
                else
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean1, out ho_RegionDynThresh, iMarkOffSet, "dark");
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionDynThresh, out ho_ConnectedRegions);
                ho_Mark.Dispose();
                HOperatorSet.FillUp(ho_ConnectedRegions, out ho_Mark);

                if (My.Classifier.MarkAreaSet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_Mark, out ExpTmpOutVar_0, "area", "and", iMarkArea_Lower, iMarkArea_Upper);
                    ho_Mark.Dispose();
                    ho_Mark = ExpTmpOutVar_0;
                }
                if (My.Classifier.MarkRoundnessSet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_Mark, out ExpTmpOutVar_0, "roundness", "and", iMarkRoundness_Lower * 0.01, iMarkRoundness_Upper * 0.01);
                    ho_Mark.Dispose();
                    ho_Mark = ExpTmpOutVar_0;
                }
                if (My.Classifier.MarkRectangularitySet)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_Mark, out ExpTmpOutVar_0, "rectangularity", "and", iMarkRectangularity_Lower * 0.01, iMarkRectangularity_Upper * 0.01);
                    ho_Mark.Dispose();
                    ho_Mark = ExpTmpOutVar_0;
                }
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.DispObj(ho_Mark, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
            finally
            {
                ho_Image.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMean1.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_RegionFillUp.Dispose();
                ho_Mark.Dispose();
            }
        }

        private void btnSearchCircle_Click(object sender, EventArgs e)
        {
            if (My.ho_Image == null)
                return;
            try
            {
                RectangleSetting2();
                CircleSetting2();
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_CircleRow, hv_CircleColumn, hv_CircleRadius);
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                HOperatorSet.DispObj(ho_Circle, hv_ExpDefaultWinHandle);
            }
            catch
            {
            }
            finally
            {
                ho_Circle.Dispose();
            }
        }

        private void cmbProduction_SelectedIndexChanged(object sender, EventArgs e)
        {
            My.Classifier.iProductionSet = cmbProduction.SelectedIndex;
            My.Classifier.sProductionSet = cmbProduction.SelectedItem.ToString();
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "ProductionSet", My.Classifier.iProductionSet.ToString(), Path);

        }

        private void tbNullThreshold_ValueChanged(object sender, EventArgs e)
        {
            nudNullthreshold.Value = tbNullThreshold.Value;
        }

        private void nudNullthreshold_ValueChanged(object sender, EventArgs e)
        {
            iNullthreshold = tbNullThreshold.Value = (int)nudNullthreshold.Value;
            try
            {
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                ho_ImageResult.Dispose();
                HOperatorSet.GrayRangeRect(ho_Image, out ho_ImageResult, 10, 10);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageResult, out ho_Region, iNullthreshold, 255);
                HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row, out hv_Column);
                HOperatorSet.DispObj(ho_Image,hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_ImageResult, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_Region, hv_ExpDefaultWinHandle);
                set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Area:" + hv_Area);
                if (iNullArea_Lower > hv_Area || hv_Area > iNullArea_Upper)
                {
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1700, 200);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                    ho_ImageResult.Dispose();
                    ho_Region.Dispose();
                }
            }
            catch
            {
            }
        }

        private void tbNullArea_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudNullArea_Upper.Value = tbNullArea_Upper.Value;
        }

        private void nudNullArea_Upper_ValueChanged(object sender, EventArgs e)
        {
            iNullArea_Upper = tbNullArea_Upper.Value = (int)nudNullArea_Upper.Value;
            try
            {
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                ho_ImageResult.Dispose();
                HOperatorSet.GrayRangeRect(ho_Image, out ho_ImageResult, 10, 10);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageResult, out ho_Region, iNullthreshold, 255);
                HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row, out hv_Column);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_ImageResult, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_Region, hv_ExpDefaultWinHandle);
                set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Area:" + hv_Area);
                if (iNullArea_Lower > hv_Area || hv_Area > iNullArea_Upper)
                {
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1700, 200);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                    ho_ImageResult.Dispose();
                    ho_Region.Dispose();
                }
            }
            catch
            {
            }
        }

        private void tbNullArea_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudNullArea_Lower.Value = tbNullArea_Lower.Value;
        }

        private void nudNullArea_Lower_ValueChanged(object sender, EventArgs e)
        {
            iNullArea_Lower = tbNullArea_Lower.Value = (int)nudNullArea_Lower.Value;
            try
            {
                if (My.ho_Image == null)
                    return;
                ho_Image.Dispose();
                HOperatorSet.CopyImage(My.ho_Image, out ho_Image);
                hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
                hWindowControl1.HalconWindow.ClearWindow();
                ho_ImageResult.Dispose();
                HOperatorSet.GrayRangeRect(ho_Image, out ho_ImageResult, 10, 10);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageResult, out ho_Region, iNullthreshold, 255);
                HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row, out hv_Column);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_ImageResult, hv_ExpDefaultWinHandle);
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.DispObj(ho_Region, hv_ExpDefaultWinHandle);
                set_display_font(hv_ExpDefaultWinHandle, 30, "mono", "true", "false");
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "yellow");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Area:" + hv_Area);
                if (iNullArea_Lower > hv_Area || hv_Area > iNullArea_Upper)
                {
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "blue");
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 1700, 200);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Miss");
                    ho_ImageResult.Dispose();
                    ho_Region.Dispose();
                }
            }
            catch
            {
            }
        }

        private void btnNullSave_Click(object sender, EventArgs e)
        {
            My.Classifier.iNullthreshold = iNullthreshold;
            My.Classifier.iNullArea_Upper = iNullArea_Upper;
            My.Classifier.iNullArea_Lower = iNullArea_Lower;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "Nullthreshold", My.Classifier.iNullthreshold.ToString(), Path);
            IniFile.Write("Setting", "NullArea_Upper", My.Classifier.iNullArea_Upper.ToString(), Path);
            IniFile.Write("Setting", "NullArea_Lower", My.Classifier.iNullArea_Lower.ToString(), Path);
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

        
    }
}
