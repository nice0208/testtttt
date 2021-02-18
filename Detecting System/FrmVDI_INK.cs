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
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Detecting_System
{
    public partial class FrmVDI_INK : Form
    {
        FrmParent parent;
        FrmRun Run;

        public FrmVDI_INK(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;
            InitializeComponent();
        }
        public static int iEmptyGraySet = 1;
        public HTuple dFirstCircleRow = 0;
        public HTuple dFirstCircleColumn = 0;
        public HTuple dEmptyCircleRow = 0;
        public HTuple dEmptyCircleColumn = 0;
        public HTuple dEmptyCircleRadius = 0;
        public HTuple dEmptyCircleArea = 0;
        public static int iNoInkAreaSet = 0;
        //抓圓心的圓半徑
        public HTuple dCenterRadius = 1;
        public static string PointChoice = "first";

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
        public static bool CreateModel_Empty = false;
        public static bool CreateModel_NoInk = false;
        public static int iEccentricitySet = 0;
        public static int iGraySet = 1;
        #region halcon參數1
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
            HOperatorSet.DispText(hv_WindowHandle, hv_String, hv_CoordSystem_COPY_INP_TMP,
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
                //HOperatorSet.DispObj(ho_Contour, Window);
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
                //HOperatorSet.SetColor(Window, "blue");
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row1, hv_Column1, 6, 0.785398);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "used_edges",
                    "row", out hv_UsedRow);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "used_edges",
                    "column", out hv_UsedColumn);
                ho_UsedEdges.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_UsedEdges, hv_UsedRow, hv_UsedColumn,
                    10, (new HTuple(45)).TupleRad());
                //HOperatorSet.DispObj(ho_UsedEdges, Window);
                ho_ResultContours.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_ResultContours, hv_MetrologyHandle,
                    "all", "all", 1.5);
                //HOperatorSet.DispObj(ho_ResultContours, Window);
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

        // Local procedures 
        public void Mean_offset(HTuple hv_rows, HTuple hv_cols, HTuple hv_center_row,
            HTuple hv_center_col, HTuple hv_radius, out HTuple hv_distance)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_point_length = null, hv_top_1_percent = null;
            HTuple hv_rows_offset = null, hv_cols_offset = null, hv_rows_offset_power2 = null;
            HTuple hv_cols_offset_power2 = null, hv_offset = null;
            // Initialize local and output iconic variables 
            hv_point_length = new HTuple(hv_rows.TupleLength());
            HOperatorSet.TupleInt(hv_point_length * 0.01, out hv_top_1_percent);
            hv_rows_offset = hv_rows - hv_center_row;
            hv_cols_offset = hv_cols - hv_center_col;
            HOperatorSet.TupleMult(hv_rows_offset, hv_rows_offset, out hv_rows_offset_power2);
            HOperatorSet.TupleMult(hv_cols_offset, hv_cols_offset, out hv_cols_offset_power2);
            HOperatorSet.TupleSqrt(hv_rows_offset_power2 + hv_cols_offset_power2, out hv_offset);
            HOperatorSet.TupleAbs(hv_offset - hv_radius, out hv_offset);
            HOperatorSet.TupleSort(-hv_offset, out hv_offset);
            HOperatorSet.TupleMean(hv_offset.TupleSelectRange(0, hv_top_1_percent), out hv_distance);
            hv_distance = -hv_distance;

            return;
        }
        #endregion
        #region halcon參數2
        // Stack for temporary objects 
        HObject[] OTemp = new HObject[20];

        // Local iconic variables 
        HObject ho_Image = null;
        HObject ho_Circle = null, ho_ReducedImage = null;


        HObject ho_srcImage, ho_DetectionRange = null;
        HObject ho_DetectionImage = null, ho_light_region = null, ho_light_region_Empty = null;
        HObject ho_light_region_result = null, ho_UsedEdges = null;
        HObject ho_ResultContours = null, ho_CrossCenter = null, ho_LensCircle = null;
        HObject ho_LensImage = null, ho_LensDark_region = null, ho_LensDark_region_ = null;
        HObject ho_Ink_region = null, ho_Ink_region_FillUp = null, ho_InkHole_region = null;
        HObject ho_OutsideCircle_Upper = null, ho_OutsideCircle_Lower = null;
        HObject ho_InsideCircle_Upper = null, ho_InsideCircle_Lower = null;
        HObject ho_Circle_Miss = null, ho_ExcessInkRegion_Outer = null;
        HObject ho_ExcessInkRegion_Inner = null, ho_MissInkRegion = null;
        HObject ho_FirstCross = null, ho_Circle_Empty = null, ho_InkCross = null;
        HObject ho_LensCross = null, ho_RegionFillUp = null, ho_light_region_Empty_ = null, ho_ImageReduced_Lens = null;

        // Local control variables 
        HTuple hv_ImageFiles = null, hv_pixel2mm = null;
        HTuple hv_OutsideDiam_Upper = null, hv_OutsideDiam_Lower = null;
        HTuple hv_InsideDiam_Upper = null, hv_InsideDiam_Lower = null;
        HTuple hv_CreateEmptyModel = null, hv_CreateNoInkModel = null;
        HTuple hv_minCircularity = null, hv_WindowHandle = new HTuple();
        HTuple hv_imageIndex = null, hv_ResultCode = new HTuple();
        HTuple hv_srcWidth = new HTuple(), hv_srcHeight = new HTuple();
        HTuple hv_EmptyThreshold = new HTuple(), hv_Area = new HTuple();
        HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
        HTuple hv_EmptyRadiusSet = new HTuple(), hv_Area_Empty = new HTuple();
        HTuple hv_Row_Empty = new HTuple(), hv_Column_Empty = new HTuple();
        HTuple hv_Number_Empty = new HTuple(), hv_light_region_Area = new HTuple();
        HTuple hv_light_region_Row = new HTuple(), hv_light_region_Column = new HTuple();
        HTuple hv_NoInkAreaSet = new HTuple(), hv_Area0 = new HTuple();
        HTuple hv_Row0 = new HTuple(), hv_Column0 = new HTuple();
        HTuple hv_Area1 = new HTuple(), hv_InitialRow = new HTuple();
        HTuple hv_InitialColumn = new HTuple(), hv_FirstRadius = new HTuple();
        HTuple hv_Length = new HTuple(), hv_Measure_Threshold = new HTuple();
        HTuple hv_GenParamValue = new HTuple(), hv_PointChoice = new HTuple();
        HTuple hv_ResultRow = new HTuple(), hv_ResultColumn = new HTuple();
        HTuple hv_ResultRadius = new HTuple(), hv_InkThreshold = new HTuple();
        HTuple hv_count = new HTuple(), hv_LensDark_Area2 = new HTuple();
        HTuple hv_LensDark_Row2 = new HTuple(), hv_LensDark_Column2 = new HTuple();
        HTuple hv_LensDark_Area = new HTuple(), hv_LensDark_Row = new HTuple();
        HTuple hv_LensDark_Column = new HTuple(), hv_InkHole_Area = new HTuple();
        HTuple hv_InkHole_Row = new HTuple(), hv_InkHole_Column = new HTuple();
        HTuple hv_Distance = new HTuple(), hv_mmDistance = new HTuple();
        HTuple hv_ExcessInkArea_Outer = new HTuple(), hv_ExcessInkRow_Outer = new HTuple();
        HTuple hv_ExcessInkColumn_Outer = new HTuple(), hv_ExcessInkArea_Inner = new HTuple();
        HTuple hv_ExcessInkRow_Ineer = new HTuple(), hv_ExcessInkColumn_Inner = new HTuple();
        HTuple hv_MissInkArea = new HTuple(), hv_MissInkRow = new HTuple();
        HTuple hv_MissInkColumn = new HTuple(), hv_Number_NoInk = new HTuple();
        HTuple hv_Ink_region_Radius = new HTuple(), hv_InkHole_Outer_Radius = new HTuple();

        HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
        #endregion

        private void FrmVDI_INK_Load(object sender, EventArgs e)
        {
            ReadPara();
            LoadSettingLight();
        }

        public void ReadPara()
        {

            //HOperatorSet.GenEmptyObj(out ho_Circle_Empty);
            //HOperatorSet.GenEmptyObj(out ho_ImageReduced_Empty);
            //HOperatorSet.GenEmptyObj(out ho_ModelContours_Empty);
            //HOperatorSet.GenEmptyObj(out ho_Circle_NoInk);
            //HOperatorSet.GenEmptyObj(out ho_ImageReduced_NoInk);
            //HOperatorSet.GenEmptyObj(out ho_ModelContours_NoInk);
            HOperatorSet.GenEmptyObj(out ho_FirstCross);
            HOperatorSet.GenEmptyObj(out ho_LensCross);
            HOperatorSet.GenEmptyObj(out ho_InkCross);
            HOperatorSet.GenEmptyObj(out ho_ReducedImage);
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_DetectionRange);
            HOperatorSet.GenEmptyObj(out ho_DetectionImage);
            HOperatorSet.GenEmptyObj(out ho_light_region);
            HOperatorSet.GenEmptyObj(out ho_light_region_Empty);
            HOperatorSet.GenEmptyObj(out ho_light_region_result);
            HOperatorSet.GenEmptyObj(out ho_UsedEdges);
            HOperatorSet.GenEmptyObj(out ho_ResultContours);
            HOperatorSet.GenEmptyObj(out ho_CrossCenter);
            HOperatorSet.GenEmptyObj(out ho_LensCircle);
            HOperatorSet.GenEmptyObj(out ho_LensImage);
            HOperatorSet.GenEmptyObj(out ho_LensDark_region);
            HOperatorSet.GenEmptyObj(out ho_LensDark_region_);
            HOperatorSet.GenEmptyObj(out ho_Ink_region);
            HOperatorSet.GenEmptyObj(out ho_Ink_region_FillUp);
            HOperatorSet.GenEmptyObj(out ho_InkHole_region);
            HOperatorSet.GenEmptyObj(out ho_OutsideCircle_Upper);
            HOperatorSet.GenEmptyObj(out ho_OutsideCircle_Lower);
            HOperatorSet.GenEmptyObj(out ho_InsideCircle_Upper);
            HOperatorSet.GenEmptyObj(out ho_InsideCircle_Lower);
            HOperatorSet.GenEmptyObj(out ho_Circle_Miss);
            HOperatorSet.GenEmptyObj(out ho_ExcessInkRegion_Outer);
            HOperatorSet.GenEmptyObj(out ho_ExcessInkRegion_Inner);
            HOperatorSet.GenEmptyObj(out ho_MissInkRegion);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_light_region_Empty_);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced_Lens);

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            dEmptyCircleRadius = My.VDI_Ink.dEmptyCircleRadius;
            iEmptyGraySet = My.VDI_Ink.iEmptyGraySet;
            dEmptyCircleRadius = My.VDI_Ink.dEmptyCircleRadius;
            tbLength.Value = My.VDI_Ink.iLength;
            dCenterRadius = My.VDI_Ink.dCenterRadius;
            PointChoice = My.VDI_Ink.PointChoice;
            tbDetectionRadius.Value = My.VDI_Ink.iReduceRadius;
            tbOutsideDiam_Upper.Value = My.VDI_Ink.iOutsideDiam_Upper;
            tbOutsideDiam_Lower.Value = My.VDI_Ink.iOutsideDiam_Lower;
            tbInsideDiam_Upper.Value = My.VDI_Ink.iInsideDiam_Upper;
            tbInsideDiam_Lower.Value = My.VDI_Ink.iInsideDiam_Lower;
            tbUnderSizeArea.Value = My.VDI_Ink.iUnderSizeArea;
            tbGraySet.Value = My.VDI_Ink.iGraySet;
            tbEmptyGraySet.Value = My.VDI_Ink.iEmptyGraySet;
            tbNoInkAreaSet.Value = My.VDI_Ink.iNoInkAreaSet;
            if (My.VDI_Ink.PointChoice == "first")
                cbPointChoice.SelectedIndex = 0;
            else
                cbPointChoice.SelectedIndex = 1;
            if (My.VDI_Ink.sGenParamValue == "negative")
                tbWhiteToBlack.Value = My.VDI_Ink.iMeasureThreshold;
            else
                tbBlackToWhite.Value = My.VDI_Ink.iMeasureThreshold;

            nudAngleRange_Empty.Value = (decimal)My.VDI_Ink.dAngleRange_Empty;
            nudScoreSet_Empty.Value = (decimal)My.VDI_Ink.dScoreSet_Empty;
            nudAngleRange_NoInk.Value = (decimal)My.VDI_Ink.dAngleRange_NoInk;
            nudScoreSet_NoInk.Value = (decimal)My.VDI_Ink.dScoreSet_NoInk;
            nudEccentricitySet.Value = (decimal)My.VDI_Ink.iEccentricitySet;
            nudAimCirR.Value = (decimal)My.VDI_Ink.iAimCirR;
            pixel2um = My.VDI_Ink.pixel2um;
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

        private void btnImageProPlus_Click(object sender, EventArgs e)
        {
            ImageProPlus(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);
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

        public HObject ReadPicture(HWindow window, string ImagePath)
        {
            // 得到图片显示的窗口句柄
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
            HOperatorSet.SetPart(window, 0, 0, hv_Height - 1, hv_Width - 1);
            HOperatorSet.DispObj(ho_Image, window); //将图像在该窗口进行显示
            return ho_Image; //返回这个图像
        }

        public void ImageProPlus(HWindow Window, HObject theImage, int n)
        {
            if (theImage == null)
                return;
            double OuterDiameter = 0;
            double InnerDiameter = 0;
            hv_OutsideDiam_Upper = (iOutsideDiam_Upper / pixel2um) / 2;
            hv_OutsideDiam_Lower = (iOutsideDiam_Lower / pixel2um) / 2;
            hv_InsideDiam_Upper = (iInsideDiam_Upper / pixel2um) / 2;
            hv_InsideDiam_Lower = (iInsideDiam_Lower / pixel2um) / 2;
            hv_pixel2mm = pixel2um;
            ho_Image = theImage;
            Window.ClearWindow();
            HOperatorSet.DispObj(ho_Image, Window);
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            try
            {
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, iReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_light_region.Dispose();
                hv_EmptyThreshold = iEmptyGraySet;
                ho_light_region.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_light_region, hv_EmptyThreshold, 255);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.RemoveNoiseRegion(ho_light_region, out ExpTmpOutVar_0, "n_48");
                    ho_light_region.Dispose();
                    ho_light_region = ExpTmpOutVar_0;
                }
                //判斷是否無料(外徑小於鏡片外徑)
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_light_region, out ho_RegionFillUp);
                ho_light_region_Empty.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_light_region_Empty);
                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_Area, out hv_Row, out hv_Column);
                ho_light_region_Empty_.Dispose();
                HOperatorSet.SelectShape(ho_light_region_Empty, out ho_light_region_Empty_, "area", "and", hv_Area.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_light_region_Empty_, out hv_Area, out hv_Row, out hv_Column);
                hv_EmptyRadiusSet = dEmptyCircleRadius;
                ho_light_region_Empty.Dispose();
                HOperatorSet.SelectShape(ho_light_region_Empty_, out ho_light_region_Empty, "outer_radius", "and", 0, hv_EmptyRadiusSet);
                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_Area_Empty, out hv_Row_Empty, out hv_Column_Empty);
                HOperatorSet.TupleLength(hv_Area_Empty, out hv_Number_Empty);

                set_display_font(Window, 30, "mono", "true", "false");
                if ((int)(new HTuple(hv_Number_Empty.TupleGreater(0))) != 0)
                {
                    HOperatorSet.DispObj(ho_Image, Window);
                    hv_ResultCode = 0;
                    HOperatorSet.SetColor(Window, "red");
                    HOperatorSet.SetTposition(Window, 1500, 100);
                    HOperatorSet.WriteString(Window, "Miss");
                    Vision.VisionResult[n] = "Miss";
                    hv_mmDistance = 0;
                    HOperatorSet.DumpWindowImage(out ho_Image, Window);
                    
                        Vision.Images_1[n] = ho_Image;
                        Vision.Images_Now[n] = Vision.Images_1[n];
                        Vision.ImagesOriginal_1[n] = theImage;
                    
                    WriteLog(n, Vision.VisionResult[n], hv_mmDistance, 0, 0);
                    return;
                }
                //切出鏡片區域,判斷是否沒墨
                ho_ImageReduced_Lens.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_light_region_Empty_, out ho_ImageReduced_Lens);
                ho_light_region_Empty.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced_Lens, out ho_light_region_Empty, hv_EmptyThreshold, 255);

                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_light_region_Area, out hv_light_region_Row, out hv_light_region_Column);

                hv_NoInkAreaSet = iNoInkAreaSet;
                if (hv_light_region_Area > hv_NoInkAreaSet)
                {
                    hv_ResultCode = -2;
                    HOperatorSet.DispObj(ho_Image, Window);
                    HOperatorSet.SetColor(Window, "red");
                    HOperatorSet.SetTposition(Window, 1500, 100);
                    HOperatorSet.WriteString(Window, "無塗墨");
                    Vision.VisionResult[n] = "NG3";
                    hv_mmDistance = 0;
                    HOperatorSet.DumpWindowImage(out ho_Image, Window);
                    
                        Vision.Images_1[n] = ho_Image;
                        Vision.Images_Now[n] = Vision.Images_1[n];
                        Vision.ImagesOriginal_1[n] = theImage;
                    
                    WriteLog(n, Vision.VisionResult[n], hv_mmDistance, 0, 0);
                    return;
                }

                HOperatorSet.AreaCenter(ho_light_region_Empty_, out hv_Area1, out hv_InitialRow,
                    out hv_InitialColumn);
                //(2)找鏡片
                hv_FirstRadius = dCenterRadius;
                hv_Length = iLength;
                hv_Measure_Threshold = iMeasureThreshold;
                hv_GenParamValue = sGenParamValue;
                hv_PointChoice = PointChoice;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_Image, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, hv_FirstRadius,
                    hv_Length, hv_Measure_Threshold, hv_GenParamValue, hv_PointChoice, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);

                //(3)找墨環

                ho_LensCircle.Dispose();
                HOperatorSet.GenCircle(out ho_LensCircle, hv_ResultRow, hv_ResultColumn, hv_ResultRadius);
                ho_LensImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_LensCircle, out ho_LensImage);
                ho_LensDark_region.Dispose();
                HOperatorSet.Threshold(ho_LensImage, out ho_LensDark_region, 0, iGraySet);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ClosingCircle(ho_LensDark_region, out ExpTmpOutVar_0, 3.5);
                    ho_LensDark_region.Dispose();
                    ho_LensDark_region = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.RemoveNoiseRegion(ho_LensDark_region, out ExpTmpOutVar_0, "n_48");
                    ho_LensDark_region.Dispose();
                    ho_LensDark_region = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_LensDark_region, out ExpTmpOutVar_0);
                    ho_LensDark_region.Dispose();
                    ho_LensDark_region = ExpTmpOutVar_0;
                }
                ho_LensDark_region_.Dispose();
                HOperatorSet.SelectShape(ho_LensDark_region, out ho_LensDark_region_, "outer_radius", "and", hv_InsideDiam_Lower, hv_OutsideDiam_Upper);
                HOperatorSet.CountObj(ho_LensDark_region_, out hv_count);
                //如果溢膠過於嚴重
                if ((int)(new HTuple(hv_count.TupleEqual(0))) != 0)
                {
                    hv_ResultCode = -1;
                    HOperatorSet.AreaCenter(ho_LensDark_region, out hv_LensDark_Area2, out hv_LensDark_Row2, out hv_LensDark_Column2);
                    ho_LensDark_region_.Dispose();
                    HOperatorSet.SelectShape(ho_LensDark_region, out ho_LensDark_region_, "area", "and", hv_LensDark_Area2.TupleMax(), 9999999);
                }

                HOperatorSet.AreaCenter(ho_LensDark_region_, out hv_LensDark_Area, out hv_LensDark_Row, out hv_LensDark_Column);
                ho_Ink_region.Dispose();
                HOperatorSet.SelectShape(ho_LensDark_region_, out ho_Ink_region, "area", "and", hv_LensDark_Area.TupleMax(), 9999999);
                //求墨環外半徑
                HOperatorSet.RegionFeatures(ho_Ink_region, "outer_radius", out hv_Ink_region_Radius);
                ho_Ink_region_FillUp.Dispose();
                HOperatorSet.FillUp(ho_Ink_region, out ho_Ink_region_FillUp);

                ho_InkHole_region.Dispose();
                HOperatorSet.Difference(ho_Ink_region_FillUp, ho_Ink_region, out ho_InkHole_region);
                HOperatorSet.AreaCenter(ho_InkHole_region, out hv_InkHole_Area, out hv_InkHole_Row, out hv_InkHole_Column);
                //求墨環內半徑
                HOperatorSet.RegionFeatures(ho_InkHole_region, "outer_radius", out hv_InkHole_Outer_Radius);
                //計算偏心
                HOperatorSet.DistancePp(hv_InkHole_Row, hv_InkHole_Column, hv_ResultRow, hv_ResultColumn, out hv_Distance);
                hv_mmDistance = ((((hv_Distance * hv_pixel2mm)).TupleString(".0f"))).TupleNumber();

                ho_OutsideCircle_Upper.Dispose();
                HOperatorSet.GenCircle(out ho_OutsideCircle_Upper, hv_ResultRow, hv_ResultColumn, hv_OutsideDiam_Upper);
                ho_OutsideCircle_Lower.Dispose();
                HOperatorSet.GenCircle(out ho_OutsideCircle_Lower, hv_ResultRow, hv_ResultColumn, hv_OutsideDiam_Lower);
                ho_InsideCircle_Upper.Dispose();
                HOperatorSet.GenCircle(out ho_InsideCircle_Upper, hv_ResultRow, hv_ResultColumn, hv_InsideDiam_Upper);
                ho_InsideCircle_Lower.Dispose();
                HOperatorSet.GenCircle(out ho_InsideCircle_Lower, hv_ResultRow, hv_ResultColumn, hv_InsideDiam_Lower);
                //缺膠範圍
                ho_Circle_Miss.Dispose();
                HOperatorSet.Difference(ho_OutsideCircle_Lower, ho_InsideCircle_Upper, out ho_Circle_Miss);

                //外溢膠
                ho_ExcessInkRegion_Outer.Dispose();
                HOperatorSet.Difference(ho_Ink_region, ho_OutsideCircle_Upper, out ho_ExcessInkRegion_Outer);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ExcessInkRegion_Outer, out ExpTmpOutVar_0, "area", "and", (HTuple)iUnderSizeArea, 9999999);

                    ho_ExcessInkRegion_Outer.Dispose();
                    ho_ExcessInkRegion_Outer = ExpTmpOutVar_0;
                }
                //內溢膠
                ho_ExcessInkRegion_Inner.Dispose();
                HOperatorSet.Difference(ho_InsideCircle_Lower, ho_Ink_region, out ho_ExcessInkRegion_Inner);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(ho_InsideCircle_Lower, ho_ExcessInkRegion_Inner, out ExpTmpOutVar_0);
                    ho_ExcessInkRegion_Inner.Dispose();
                    ho_ExcessInkRegion_Inner = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ErosionCircle(ho_ExcessInkRegion_Inner, out ExpTmpOutVar_0, 2);
                    ho_ExcessInkRegion_Inner.Dispose();
                    ho_ExcessInkRegion_Inner = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_ExcessInkRegion_Inner, out ExpTmpOutVar_0, "area", "and", (HTuple)iUnderSizeArea, 9999999);

                    ho_ExcessInkRegion_Inner.Dispose();
                    ho_ExcessInkRegion_Inner = ExpTmpOutVar_0;
                }
                //內部缺膠
                ho_MissInkRegion.Dispose();
                HOperatorSet.Difference(ho_Circle_Miss, ho_Ink_region, out ho_MissInkRegion);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_MissInkRegion, out ExpTmpOutVar_0, "area", "and", (HTuple)iUnderSizeArea, 9999999);

                    ho_MissInkRegion.Dispose();
                    ho_MissInkRegion = ExpTmpOutVar_0;
                }
                HOperatorSet.AreaCenter(ho_ExcessInkRegion_Outer, out hv_ExcessInkArea_Outer, out hv_ExcessInkRow_Outer, out hv_ExcessInkColumn_Outer);
                HOperatorSet.AreaCenter(ho_ExcessInkRegion_Inner, out hv_ExcessInkArea_Inner, out hv_ExcessInkRow_Ineer, out hv_ExcessInkColumn_Inner);
                HOperatorSet.AreaCenter(ho_MissInkRegion, out hv_MissInkArea, out hv_MissInkRow, out hv_MissInkColumn);

                HOperatorSet.GenCrossContourXld(out ho_LensCross, hv_ResultRow, hv_ResultColumn, 40, (new HTuple(45)).TupleRad());
                HOperatorSet.GenCrossContourXld(out ho_InkCross, hv_InkHole_Row, hv_InkHole_Column, 40, (new HTuple(45)).TupleRad());
                HOperatorSet.SetDraw(Window, "margin");
                HOperatorSet.SetColor(Window, "green");
                HOperatorSet.DispObj(ho_Image, Window);
                HOperatorSet.DispObj(ho_LensCircle, Window);
                HOperatorSet.DispObj(ho_LensCross, Window);
                HOperatorSet.SetColor(Window, "blue");
                HOperatorSet.DispObj(ho_InkCross, Window);
                set_display_font(Window, 20, "mono", "true", "false");
                HOperatorSet.SetColor(Window, "green");
                HOperatorSet.SetTposition(Window, 0, 0);
                HOperatorSet.WriteString(Window, ("內徑偏心:" + hv_mmDistance + "um"));
                HOperatorSet.SetTposition(Window, 100, 0);
                InnerDiameter = Math.Round((double)hv_InkHole_Outer_Radius * 2 * pixel2um, 3);
                HOperatorSet.WriteString(Window, ("內徑直徑:" + InnerDiameter + "mm"));
                HOperatorSet.SetTposition(Window, 200, 0);
                OuterDiameter = Math.Round((double)hv_Ink_region_Radius * 2 * pixel2um, 3);
                HOperatorSet.WriteString(Window, ("外徑直徑:" + OuterDiameter + "mm"));
                HOperatorSet.TupleLength(hv_ExcessInkArea_Outer, out hv_ExcessInkArea_Outer);
                HOperatorSet.TupleLength(hv_ExcessInkArea_Inner, out hv_ExcessInkArea_Inner);
                HOperatorSet.TupleLength(hv_MissInkArea, out hv_MissInkArea);
                set_display_font(Window, 30, "mono", "true", "false");
                if (hv_ExcessInkArea_Outer == 0 && hv_ExcessInkArea_Inner == 0 && hv_MissInkArea == 0 && hv_mmDistance <= iEccentricitySet)
                {
                    HOperatorSet.SetColor(Window, "green");

                    HOperatorSet.SetTposition(Window, 1500, 100);
                    HOperatorSet.WriteString(Window, "Pass");
                    Vision.VisionResult[n] = "OK";
                }
                else
                {
                    HOperatorSet.SetColor(Window, "red");
                    HOperatorSet.SetTposition(Window, 1500, 100);
                    HOperatorSet.WriteString(Window, "NG");
                    //顯示region
                    if (hv_ExcessInkArea_Outer > 0 || hv_ExcessInkArea_Inner > 0)
                    {
                        HOperatorSet.SetColor(Window, "red");
                        HOperatorSet.DispObj(ho_ExcessInkRegion_Outer, Window);
                        HOperatorSet.DispObj(ho_ExcessInkRegion_Inner, Window);
                    }
                    if (hv_MissInkArea > 0)
                    {
                        HOperatorSet.SetColor(Window, "orange red");
                        HOperatorSet.DispObj(ho_MissInkRegion, Window);
                    }
                    //顯示結果提示
                    if (hv_ExcessInkArea_Outer > 0 || hv_ExcessInkArea_Inner > 0)
                    {
                        HOperatorSet.SetColor(Window, "red");
                        HOperatorSet.DispObj(ho_ExcessInkRegion_Outer, Window);
                        HOperatorSet.DispObj(ho_ExcessInkRegion_Inner, Window);
                        HOperatorSet.SetTposition(Window, 1600, 100);
                        HOperatorSet.WriteString(Window, "溢墨");
                        hv_ResultCode = -1;
                        Vision.VisionResult[n] = "NG";
                    }
                    else if (hv_MissInkArea > 0)
                    {
                        HOperatorSet.SetColor(Window, "orange red");
                        HOperatorSet.DispObj(ho_MissInkRegion, Window);
                        HOperatorSet.SetTposition(Window, 1700, 100);
                        HOperatorSet.WriteString(Window, "缺墨");
                        hv_ResultCode = -3;
                        Vision.VisionResult[n] = "NG";
                    }
                    else if ((int)(new HTuple(hv_mmDistance.TupleGreater(iEccentricitySet))) != 0)
                    {
                        HOperatorSet.SetTposition(Window, 1900, 100);
                        HOperatorSet.WriteString(Window, "偏心");
                        hv_ResultCode = -4;
                        Vision.VisionResult[n] = "NG2";
                    }
                }
            }
            catch
            {
                HOperatorSet.DispObj(ho_Image, Window);
                hv_ResultCode = 0;
                HOperatorSet.SetColor(Window, "red");
                HOperatorSet.SetTposition(Window, 1500, 100);
                HOperatorSet.WriteString(Window, "Miss");
                Vision.VisionResult[n] = "Miss";
                hv_mmDistance = 0;
                HOperatorSet.DumpWindowImage(out ho_Image, Window);
                
                    Vision.Images_1[n] = ho_Image;
                    Vision.Images_Now[n] = Vision.Images_1[n];
                    Vision.ImagesOriginal_1[n] = theImage;
                
                WriteLog(n, Vision.VisionResult[n], hv_mmDistance, 0, 0);
                return;
            }


            HOperatorSet.DumpWindowImage(out ho_Image, Window);
            
                Vision.Images_1[n] = ho_Image;
                Vision.Images_Now[n] = Vision.Images_1[n];
                Vision.ImagesOriginal_1[n] = theImage;
            
            WriteLog(n, Vision.VisionResult[n], hv_mmDistance, InnerDiameter, OuterDiameter);
        }

        public void WriteLog(int n, string ResultOK, int EccentricitySet, double InnerDiameter, double OuterDiameter)
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
                            "\tOperatorID\tMachine No.\tTime\tCT\tResult\tEccentricity\tInnerDiameter\tOuterDiameter" +
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
                                     EccentricitySet.ToString("f0") + "\t" +
                                     InnerDiameter + "\t" +
                                     OuterDiameter);
                    }
                }
                catch
                {
                }

            }
        }

        private void tbDetectionRadius_ValueChanged(object sender, EventArgs e)
        {
            nudDetectionRadius.Value = tbDetectionRadius.Value;
        }

        private void nudDetectionRadius_ValueChanged(object sender, EventArgs e)
        {
            iReduceRadius = tbDetectionRadius.Value = Convert.ToInt32(nudDetectionRadius.Value);
            try
            {
                ho_Image = My.ho_Image;
                HWindow Window = hWindowControl1.HalconWindow;
                if (ho_Image == null)
                    return;
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, iReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                HOperatorSet.DispObj(ho_ReducedImage, hWindowControl1.HalconWindow);
                Thread.Sleep(1);
            }
            catch
            {
            }
        }

        private void btnFirstCircleCenter_Click(object sender, EventArgs e)
        {
            try
            {
                hv_OutsideDiam_Lower = iReduceRadius; 
                HWindow Window = hWindowControl1.HalconWindow;
                ho_Image = My.ho_Image;
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                set_display_font(Window, 40, "mono", "true", "false");
                if (ho_Image == null)
                    return;
                //劃出檢測區域減少干擾
                HOperatorSet.DispObj(ho_Image, Window);

                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, iReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_light_region.Dispose();
                hv_EmptyThreshold = iEmptyGraySet;
                ho_light_region.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_light_region, hv_EmptyThreshold, 255);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.RemoveNoiseRegion(ho_light_region, out ExpTmpOutVar_0, "n_48");
                    ho_light_region.Dispose();
                    ho_light_region = ExpTmpOutVar_0;
                }
                //判斷是否無料(外徑小於鏡片外徑)
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_light_region, out ho_RegionFillUp);
                ho_light_region_Empty.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_light_region_Empty);
                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_Area, out hv_Row, out hv_Column);
                ho_light_region_Empty_.Dispose();
                HOperatorSet.SelectShape(ho_light_region_Empty, out ho_light_region_Empty_, "area", "and", hv_Area.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_light_region_Empty_, out hv_Area, out hv_Row, out hv_Column);
                hv_EmptyRadiusSet = dEmptyCircleRadius;
                ho_light_region_Empty.Dispose();
                HOperatorSet.SelectShape(ho_light_region_Empty_, out ho_light_region_Empty, "outer_radius", "and", 0, hv_EmptyRadiusSet);
                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_Area_Empty, out hv_Row_Empty, out hv_Column_Empty);
                HOperatorSet.TupleLength(hv_Area_Empty, out hv_Number_Empty);
                if ((int)(new HTuple(hv_Number_Empty.TupleGreater(0))) != 0)
                {
                    HOperatorSet.DispObj(ho_Image, Window);
                    HOperatorSet.DispObj(ho_light_region_Empty, Window);
                    hv_ResultCode = 0;
                    HOperatorSet.SetColor(Window, "red");
                    HOperatorSet.SetTposition(Window, 1500, 100);
                    HOperatorSet.WriteString(Window, "Miss");

                    hv_mmDistance = 0;
                }
                //切出鏡片區域,判斷是否沒墨
                ho_ImageReduced_Lens.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_light_region_Empty_, out ho_ImageReduced_Lens);
                ho_light_region_Empty.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced_Lens, out ho_light_region_Empty, hv_EmptyThreshold, 255);

                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_light_region_Area, out hv_light_region_Row, out hv_light_region_Column);

                hv_NoInkAreaSet = iNoInkAreaSet;
                if (hv_light_region_Area > hv_NoInkAreaSet)
                {
                    hv_ResultCode = -2;
                    HOperatorSet.DispObj(ho_Image, Window);
                    HOperatorSet.WriteString(Window, "無塗墨");
                    HOperatorSet.SetColor(Window, "orange");
                    hv_mmDistance = 0;
                    return;
                }

                HOperatorSet.DispObj(ho_Image, Window);
                HOperatorSet.AreaCenter(ho_light_region_Empty_, out hv_Area1, out hv_InitialRow,
                    out hv_InitialColumn);
                HOperatorSet.GenCrossContourXld(out ho_FirstCross, hv_InitialRow, hv_InitialColumn, 50, (new HTuple(45)).TupleRad());
                HOperatorSet.SetColor(Window, "red");
                HOperatorSet.DispObj(ho_FirstCross, Window);
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
            HWindow Window = hWindowControl1.HalconWindow;
            ho_Image = My.ho_Image;
            if (ho_Image == null)
                return;
            //畫檢視範圍
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.DispObj(ho_Image, Window);

            HOperatorSet.SetColor(Window, "blue");
            //找出初始半徑
            HOperatorSet.DrawCircle(Window, out dFirstCircleRow, out dFirstCircleColumn, out dCenterRadius);
        }

        private void tbLength_ValueChanged(object sender, EventArgs e)
        {
            nudLength.Value = tbLength.Value;
        }

        private void nudLength_ValueChanged(object sender, EventArgs e)
        {
            iLength = tbLength.Value = Convert.ToInt32(nudLength.Value);
            try
            {
                HWindow Window = hWindowControl1.HalconWindow;
                ho_Image = My.ho_Image;
                if (ho_Image == null)
                    return;
                //畫檢視範圍
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, Window);

                hv_FirstRadius = dCenterRadius;
                hv_Length = iLength;
                hv_Measure_Threshold = iMeasureThreshold;
                hv_GenParamValue = sGenParamValue;
                hv_PointChoice = PointChoice;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_Image, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, hv_FirstRadius,
                    hv_Length, hv_Measure_Threshold, hv_GenParamValue, hv_PointChoice, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);
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
            iMeasureThreshold = tbBlackToWhite.Value = Convert.ToInt32(nudBlackToWhite.Value);
            sGenParamValue = "positive";
            try
            {
                HWindow Window = hWindowControl1.HalconWindow;
                ho_Image = My.ho_Image;
                if (ho_Image == null)
                    return;
                //畫檢視範圍
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, Window);

                hv_FirstRadius = dCenterRadius;
                hv_Length = iLength;
                hv_Measure_Threshold = iMeasureThreshold;
                hv_GenParamValue = sGenParamValue;
                hv_PointChoice = PointChoice;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_Image, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, hv_FirstRadius,
                    hv_Length, hv_Measure_Threshold, hv_GenParamValue, hv_PointChoice, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);
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
            iMeasureThreshold = tbWhiteToBlack.Value = Convert.ToInt32(nudWhiteToBlack.Value);
            sGenParamValue = "negative";
            try
            {
                HWindow Window = hWindowControl1.HalconWindow;
                ho_Image = My.ho_Image;
                if (ho_Image == null)
                    return;
                //畫檢視範圍
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, Window);

                hv_FirstRadius = dCenterRadius;
                hv_Length = iLength;
                hv_Measure_Threshold = iMeasureThreshold;
                hv_GenParamValue = sGenParamValue;
                hv_PointChoice = PointChoice;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_Image, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, hv_FirstRadius,
                    hv_Length, hv_Measure_Threshold, hv_GenParamValue, hv_PointChoice, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void cbPointChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPointChoice.SelectedIndex == 0)
                PointChoice = "first";
            else
                PointChoice = "last";
            try
            {
                HWindow Window = hWindowControl1.HalconWindow;
                ho_Image = My.ho_Image;
                if (ho_Image == null)
                    return;
                //畫檢視範圍
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, Window);

                hv_FirstRadius = dCenterRadius;
                hv_Length = iLength;
                hv_Measure_Threshold = iMeasureThreshold;
                hv_GenParamValue = sGenParamValue;
                hv_PointChoice = PointChoice;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_srcImage, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, hv_FirstRadius,
                    hv_Length, hv_Measure_Threshold, hv_GenParamValue, hv_PointChoice, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);

                HOperatorSet.DispObj(ho_Image, Window);
                HOperatorSet.SetColor(Window, "blue");
                HOperatorSet.DispObj(ho_UsedEdges, Window);
                HOperatorSet.SetColor(Window, "green");
                HOperatorSet.DispObj(ho_ResultContours, Window);
            }
            catch
            {
                //MessageBox.Show("error");
            }
        }

        private void btnCircleCenter_Click(object sender, EventArgs e)
        {
            try
            {
                HWindow Window = hWindowControl1.HalconWindow;
                ho_Image = My.ho_Image;
                if (ho_Image == null)
                    return;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                set_display_font(Window, 40, "mono", "true", "false");

                //劃出檢測區域減少干擾
                HOperatorSet.DispObj(ho_Image, Window);

                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, iReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_light_region.Dispose();
                hv_EmptyThreshold = iEmptyGraySet;
                ho_light_region.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_light_region, hv_EmptyThreshold, 255);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.RemoveNoiseRegion(ho_light_region, out ExpTmpOutVar_0, "n_48");
                    ho_light_region.Dispose();
                    ho_light_region = ExpTmpOutVar_0;
                }
                //判斷是否無料(外徑小於鏡片外徑)
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_light_region, out ho_RegionFillUp);
                ho_light_region_Empty.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_light_region_Empty);
                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_Area, out hv_Row, out hv_Column);
                ho_light_region_Empty_.Dispose();
                HOperatorSet.SelectShape(ho_light_region_Empty, out ho_light_region_Empty_, "area", "and", hv_Area.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_light_region_Empty_, out hv_Area, out hv_Row, out hv_Column);
                hv_EmptyRadiusSet = dEmptyCircleRadius;
                ho_light_region_Empty.Dispose();
                HOperatorSet.SelectShape(ho_light_region_Empty_, out ho_light_region_Empty, "outer_radius", "and", 0, hv_EmptyRadiusSet);
                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_Area_Empty, out hv_Row_Empty, out hv_Column_Empty);
                HOperatorSet.TupleLength(hv_Area_Empty, out hv_Number_Empty);
                if ((int)(new HTuple(hv_Number_Empty.TupleGreater(0))) != 0)
                {
                    HOperatorSet.DispObj(ho_Image, Window);
                    HOperatorSet.DispObj(ho_light_region_Empty, Window);
                    hv_ResultCode = 0;
                    HOperatorSet.SetColor(Window, "red");
                    HOperatorSet.SetTposition(Window, 1500, 100);
                    HOperatorSet.WriteString(Window, "Miss");

                    hv_mmDistance = 0;
                }
                //切出鏡片區域,判斷是否沒墨
                ho_ImageReduced_Lens.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_light_region_Empty_, out ho_ImageReduced_Lens);
                ho_light_region_Empty.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced_Lens, out ho_light_region_Empty, hv_EmptyThreshold, 255);

                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_light_region_Area, out hv_light_region_Row, out hv_light_region_Column);

                hv_NoInkAreaSet = iNoInkAreaSet;
                if (hv_light_region_Area > hv_NoInkAreaSet)
                {
                    hv_ResultCode = -2;
                    HOperatorSet.DispObj(ho_Image, Window);
                    HOperatorSet.WriteString(Window, "無塗墨");
                    HOperatorSet.SetColor(Window, "orange");
                    hv_mmDistance = 0;
                    return;
                }

                HOperatorSet.DispObj(ho_Image, Window);
                HOperatorSet.AreaCenter(ho_light_region_Empty_, out hv_Area1, out hv_InitialRow,
                    out hv_InitialColumn);

                hv_FirstRadius = dCenterRadius;
                hv_Length = iLength;
                hv_Measure_Threshold = iMeasureThreshold;
                hv_GenParamValue = sGenParamValue;
                hv_PointChoice = PointChoice;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_Image, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, hv_FirstRadius,
                    hv_Length, hv_Measure_Threshold, hv_GenParamValue, hv_PointChoice, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);

                HOperatorSet.DispObj(ho_Image, Window);
                HOperatorSet.SetColor(Window, "green");
                HOperatorSet.DispObj(ho_ResultContours, Window);
            }
            catch
            {
            }
        }

        private void btnCenterSave_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            My.VDI_Ink.iReduceRadius = iReduceRadius;
            My.VDI_Ink.dCenterRadius = dCenterRadius;
            My.VDI_Ink.iLength = iLength;
            My.VDI_Ink.iMeasureThreshold = iMeasureThreshold;
            My.VDI_Ink.sGenParamValue = sGenParamValue;
            My.VDI_Ink.PointChoice = PointChoice;
            My.VDI_Ink.dEmptyCircleRadius = dEmptyCircleRadius;
            My.VDI_Ink.iEmptyGraySet = iEmptyGraySet;
            My.VDI_Ink.iNoInkAreaSet = iNoInkAreaSet;

            IniFile.Write("Setting", "ReduceRadius", iReduceRadius.ToString(), Path);
            IniFile.Write("Setting", "CenterRadius", dCenterRadius.ToString(), Path);
            IniFile.Write("Setting", "Length", iLength.ToString(), Path);
            IniFile.Write("Setting", "MeasureThreshold", iMeasureThreshold.ToString(), Path);
            IniFile.Write("Setting", "GenParamValue", sGenParamValue.ToString(), Path);
            IniFile.Write("Setting", "PointChoice", PointChoice, Path);
            IniFile.Write("Setting", "NoInkAreaSet", iNoInkAreaSet.ToString(), Path);
            IniFile.Write("Setting", "EmptyCircleRadius", dEmptyCircleRadius.ToString(), Path);
            IniFile.Write("Setting", "EmptyGraySet", iEmptyGraySet.ToString(), Path);
            //lblCenterSave.Text = "儲存成功";
            //Thread.Sleep(1000);
            //lblCenterSave.Text = "";
        }

        private void tbOutsideDiam_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudOutsideDiam_Upper.Value = tbOutsideDiam_Upper.Value;
        }

        private void nudOutsideDiam_Upper_ValueChanged(object sender, EventArgs e)
        {
            tbOutsideDiam_Upper.Value = Convert.ToInt32(nudOutsideDiam_Upper.Value);
            iOutsideDiam_Upper = tbOutsideDiam_Upper.Value;

            hv_OutsideDiam_Upper = (HTuple)iOutsideDiam_Upper / pixel2um / 2;
            hv_InsideDiam_Lower = (HTuple)iInsideDiam_Lower / pixel2um / 2;
            HWindow Window = hWindowControl1.HalconWindow;

            ho_Image = My.ho_Image;
            if (ho_Image == null)
                return;
            //畫檢視範圍
            try
            {
                Window.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, Window);

                ho_OutsideCircle_Upper.Dispose();
                HOperatorSet.GenCircle(out ho_OutsideCircle_Upper, hv_ResultRow, hv_ResultColumn, hv_OutsideDiam_Upper);
                ho_OutsideCircle_Lower.Dispose();
                HOperatorSet.GenCircle(out ho_InsideCircle_Lower, hv_ResultRow, hv_ResultColumn, hv_InsideDiam_Lower);
                HOperatorSet.SetDraw(Window, "margin");
                HOperatorSet.SetColor(Window, "blue");
                HOperatorSet.DispObj(ho_OutsideCircle_Upper, Window);
                HOperatorSet.SetColor(Window, "orange");
                HOperatorSet.DispObj(ho_InsideCircle_Lower, Window);
                HTuple area = new HTuple(), row = new HTuple(), col = new HTuple();

                HOperatorSet.AreaCenter(ho_InsideCircle_Lower, out area, out row, out col);
            }
            catch
            {
            }
        }

        private void tbdOutsideDiam_Lower_ValueChanged(object sender, EventArgs e)
        {
            nud_OutsideDiam_Upper.Value = tbOutsideDiam_Lower.Value;
        }

        private void nudOutsideDiam_Lower_ValueChanged(object sender, EventArgs e)
        {
            tbOutsideDiam_Lower.Value = Convert.ToInt32(nud_OutsideDiam_Upper.Value);

            iOutsideDiam_Lower = tbOutsideDiam_Lower.Value;

            hv_InsideDiam_Upper = (HTuple)iInsideDiam_Upper / pixel2um / 2;
            hv_OutsideDiam_Lower = (HTuple)iOutsideDiam_Lower / pixel2um / 2;
            HWindow Window = hWindowControl1.HalconWindow;
            ho_Image = My.ho_Image;
            if (ho_Image == null)
                return;
            //畫檢視範圍
            try
            {
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, Window);

                ho_OutsideCircle_Upper.Dispose();
                HOperatorSet.GenCircle(out ho_InsideCircle_Upper, hv_ResultRow, hv_ResultColumn, hv_InsideDiam_Upper);
                ho_OutsideCircle_Lower.Dispose();
                HOperatorSet.GenCircle(out ho_OutsideCircle_Lower, hv_ResultRow, hv_ResultColumn, hv_OutsideDiam_Lower);
                HOperatorSet.SetDraw(Window, "margin");
                HOperatorSet.SetColor(Window, "blue");
                HOperatorSet.DispObj(ho_OutsideCircle_Lower, Window);
                HOperatorSet.SetColor(Window, "orange");
                HOperatorSet.DispObj(ho_InsideCircle_Upper, Window);
            }
            catch
            {
            }
        }

        private void btnOutsideDiamSave_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            My.VDI_Ink.iOutsideDiam_Upper = iOutsideDiam_Upper;
            My.VDI_Ink.iInsideDiam_Lower = iInsideDiam_Lower;
            IniFile.Write("Setting", "OutsideDiam_Upper", iOutsideDiam_Upper.ToString(), Path);
            IniFile.Write("Setting", "InsideDiam_Lower", iInsideDiam_Lower.ToString(), Path);

            //lblOutsideDiamSave.Text = "儲存成功";
            //Thread.Sleep(500);
            //lblOutsideDiamSave.Text = "";
        }

        private void tbInsideDiam_Upper_ValueChanged(object sender, EventArgs e)
        {
            nudInsideDiam_Upper.Value = tbInsideDiam_Upper.Value;
        }

        private void nudInsideDiam_Upper_ValueChanged(object sender, EventArgs e)
        {
            tbInsideDiam_Upper.Value = Convert.ToInt32(nudInsideDiam_Upper.Value);

            iInsideDiam_Upper = tbInsideDiam_Upper.Value;

            hv_InsideDiam_Upper = (HTuple)iInsideDiam_Upper / pixel2um / 2;
            hv_OutsideDiam_Lower = (HTuple)iOutsideDiam_Lower / pixel2um / 2;
            HWindow Window = hWindowControl1.HalconWindow;
            ho_Image = My.ho_Image;
            if (ho_Image == null)
                return;
            //畫檢視範圍
            try
            {
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, Window);

                ho_OutsideCircle_Upper.Dispose();
                HOperatorSet.GenCircle(out ho_InsideCircle_Upper, hv_ResultRow, hv_ResultColumn, hv_InsideDiam_Upper);
                ho_OutsideCircle_Lower.Dispose();
                HOperatorSet.GenCircle(out ho_OutsideCircle_Lower, hv_ResultRow, hv_ResultColumn, hv_OutsideDiam_Lower);
                HOperatorSet.SetDraw(Window, "margin");
                HOperatorSet.SetColor(Window, "blue");
                HOperatorSet.DispObj(ho_OutsideCircle_Lower, Window);
                HOperatorSet.SetColor(Window, "orange");
                HOperatorSet.DispObj(ho_InsideCircle_Upper, Window);
            }
            catch
            {
            }
        }

        private void tbInsideDiam_Lower_ValueChanged(object sender, EventArgs e)
        {
            nudInsideDiam_Lower.Value = tbInsideDiam_Lower.Value;
        }

        private void nudInsideDiam_Lower_ValueChanged(object sender, EventArgs e)
        {
            tbInsideDiam_Lower.Value = Convert.ToInt32(nudInsideDiam_Lower.Value);

            iInsideDiam_Lower = tbInsideDiam_Lower.Value;

            hv_OutsideDiam_Upper = (HTuple)iOutsideDiam_Upper / pixel2um / 2;
            hv_InsideDiam_Lower = (HTuple)iInsideDiam_Lower / pixel2um / 2;
            HWindow Window = hWindowControl1.HalconWindow;

            ho_Image = My.ho_Image;
            if (ho_Image == null)
                return;
            //畫檢視範圍
            try
            {
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.DispObj(ho_Image, Window);

                ho_OutsideCircle_Upper.Dispose();
                HOperatorSet.GenCircle(out ho_OutsideCircle_Upper, hv_ResultRow, hv_ResultColumn, hv_OutsideDiam_Upper);
                ho_OutsideCircle_Lower.Dispose();
                HOperatorSet.GenCircle(out ho_InsideCircle_Lower, hv_ResultRow, hv_ResultColumn, hv_InsideDiam_Lower);
                HOperatorSet.SetDraw(Window, "margin");
                HOperatorSet.SetColor(Window, "blue");
                HOperatorSet.DispObj(ho_OutsideCircle_Upper, Window);
                HOperatorSet.SetColor(Window, "orange");
                HOperatorSet.DispObj(ho_InsideCircle_Lower, Window);
            }
            catch
            {
            }
        }

        private void btnInsideDiamSave_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            My.VDI_Ink.iInsideDiam_Upper = iInsideDiam_Upper;
            My.VDI_Ink.iOutsideDiam_Lower = iOutsideDiam_Lower;
            IniFile.Write("Setting", "InsideDiam_Upper", iInsideDiam_Upper.ToString(), Path);
            IniFile.Write("Setting", "OutsideDiam_Lower", iOutsideDiam_Lower.ToString(), Path);

            //lblInsideDiamSave.Text = "儲存成功";
            //Thread.Sleep(500);
            //lblInsideDiamSave.Text = "";
        }



        private void nudAngleRange_Empty_ValueChanged(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            dAngleRange_Empty = (double)nudAngleRange_Empty.Value;
            IniFile.Write("Setting", "AngleRange_Empty", dAngleRange_Empty.ToString(), Path);
        }

        private void nudScoreSet_Empty_ValueChanged(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            dScoreSet_Empty = (double)nudScoreSet_Empty.Value;
            IniFile.Write("Setting", "ScoreSet_Empty", dScoreSet_Empty.ToString(), Path);
        }

        private void nudAngleRange_NoInk_ValueChanged(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            dAngleRange_NoInk = (double)nudAngleRange_NoInk.Value;
            IniFile.Write("Setting", "AngleRange_NoInk", dAngleRange_NoInk.ToString(), Path);
        }

        private void nudScoreSet_NoInk_ValueChanged(object sender, EventArgs e)
        {
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            dScoreSet_NoInk = (double)nudScoreSet_NoInk.Value;
            IniFile.Write("Setting", "ScoreSet_NoInk", dScoreSet_NoInk.ToString(), Path);
        }

        private void btnCircleCenter2_Click(object sender, EventArgs e)
        {
            try
            {
                HWindow Window = hWindowControl1.HalconWindow;
                ho_Image = My.ho_Image;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                set_display_font(Window, 40, "mono", "true", "false");
                if (ho_Image == null)
                    return;
                //劃出檢測區域減少干擾
                HOperatorSet.DispObj(ho_Image, Window);

                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, iReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_light_region.Dispose();
                hv_EmptyThreshold = iEmptyGraySet;
                ho_light_region.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_light_region, hv_EmptyThreshold, 255);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.RemoveNoiseRegion(ho_light_region, out ExpTmpOutVar_0, "n_48");
                    ho_light_region.Dispose();
                    ho_light_region = ExpTmpOutVar_0;
                }
                //判斷是否無料(外徑小於鏡片外徑)
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_light_region, out ho_RegionFillUp);
                ho_light_region_Empty.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_light_region_Empty);
                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_Area, out hv_Row, out hv_Column);
                ho_light_region_Empty_.Dispose();
                HOperatorSet.SelectShape(ho_light_region_Empty, out ho_light_region_Empty_, "area", "and", hv_Area.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_light_region_Empty_, out hv_Area, out hv_Row, out hv_Column);
                hv_EmptyRadiusSet = dEmptyCircleRadius;
                ho_light_region_Empty.Dispose();
                HOperatorSet.SelectShape(ho_light_region_Empty_, out ho_light_region_Empty, "outer_radius", "and", 0, hv_EmptyRadiusSet);
                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_Area_Empty, out hv_Row_Empty, out hv_Column_Empty);
                HOperatorSet.TupleLength(hv_Area_Empty, out hv_Number_Empty);
                if ((int)(new HTuple(hv_Number_Empty.TupleGreater(0))) != 0)
                {
                    HOperatorSet.DispObj(ho_Image, Window);
                    HOperatorSet.DispObj(ho_light_region_Empty, Window);
                    hv_ResultCode = 0;
                    HOperatorSet.SetColor(Window, "red");
                    HOperatorSet.SetTposition(Window, 1500, 100);
                    HOperatorSet.WriteString(Window, "Miss");

                    hv_mmDistance = 0;
                }
                //切出鏡片區域,判斷是否沒墨
                ho_ImageReduced_Lens.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_light_region_Empty_, out ho_ImageReduced_Lens);
                ho_light_region_Empty.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced_Lens, out ho_light_region_Empty, hv_EmptyThreshold, 255);

                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_light_region_Area, out hv_light_region_Row, out hv_light_region_Column);

                hv_NoInkAreaSet = iNoInkAreaSet;
                if (hv_light_region_Area > hv_NoInkAreaSet)
                {
                    hv_ResultCode = -2;
                    HOperatorSet.DispObj(ho_Image, Window);
                    HOperatorSet.WriteString(Window, "無塗墨");
                    HOperatorSet.SetColor(Window, "orange");
                    hv_mmDistance = 0;
                    return;
                }

                HOperatorSet.DispObj(ho_Image, Window);
                HOperatorSet.AreaCenter(ho_light_region_Empty_, out hv_Area1, out hv_InitialRow,
                    out hv_InitialColumn);

                hv_FirstRadius = dCenterRadius;
                hv_Length = iLength;
                hv_Measure_Threshold = iMeasureThreshold;
                hv_GenParamValue = sGenParamValue;
                hv_PointChoice = PointChoice;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_Image, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, hv_FirstRadius,
                    hv_Length, hv_Measure_Threshold, hv_GenParamValue, hv_PointChoice, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);

                HOperatorSet.DispObj(ho_Image, Window);
                HOperatorSet.SetColor(Window, "green");
                HOperatorSet.DispObj(ho_ResultContours, Window);
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
            iGraySet = tbGraySet.Value = Convert.ToInt32(nudGraySet.Value);
            ho_Image = My.ho_Image;
            HWindow Window = hWindowControl1.HalconWindow;
            if (ho_Image == null)
                return;
            try
            {
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.SetColor(Window, "yellow");
                HOperatorSet.SetDraw(Window, "fill");
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                HOperatorSet.DispObj(ho_Image, Window);
                ho_LensDark_region.Dispose();
                HOperatorSet.Threshold(ho_Image, out ho_LensDark_region, 0, iGraySet);

                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.RemoveNoiseRegion(ho_LensDark_region, out ExpTmpOutVar_0,
                        "n_48");
                    ho_LensDark_region.Dispose();
                    ho_LensDark_region = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_LensDark_region, out ExpTmpOutVar_0);
                    ho_LensDark_region.Dispose();
                    ho_LensDark_region = ExpTmpOutVar_0;
                }
                HOperatorSet.DispObj(ho_LensDark_region, Window);
            }
            catch
            {
            }
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "GraySet", iGraySet.ToString(), Path);

            lblSave2.Text = "儲存成功";
            Thread.Sleep(500);
            lblSave2.Text = "";
        }

        private void nudEccentricitySet_ValueChanged(object sender, EventArgs e)
        {
            iEccentricitySet = (int)nudEccentricitySet.Value;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            IniFile.Write("Setting", "EccentricitySet", iEccentricitySet.ToString(), Path);
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
                    HOperatorSet.WriteImage(ho_Image, "png", 0,Resultpath + "\\" + Namepath + "_OK_" + Time);
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
                    bool A = ImageProcess.GetPicThumbnail(Resultpath + "\\" + Namepath + "_NG_" + Time+ ".png", pathNG + "\\" + Namepath + "_NG_" + Time, 360, 270, 100);
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

        private void nudAimCirR_ValueChanged(object sender, EventArgs e)
        {
            My.VDI_Ink.iAimCirR = (int)nudAimCirR.Value;
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "AimCirR", My.VDI_Ink.iAimCirR.ToString(), Path);
        }

        private void btnCircleCenter3_Click(object sender, EventArgs e)
        {
            try
            {
                HWindow Window = hWindowControl1.HalconWindow;
                ho_Image = My.ho_Image;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                set_display_font(Window, 40, "mono", "true", "false");
                if (ho_Image == null)
                    return;
                //劃出檢測區域減少干擾
                HOperatorSet.DispObj(ho_Image, Window);

                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, iReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_light_region.Dispose();
                hv_EmptyThreshold = iEmptyGraySet;
                ho_light_region.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_light_region, hv_EmptyThreshold, 255);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.RemoveNoiseRegion(ho_light_region, out ExpTmpOutVar_0, "n_48");
                    ho_light_region.Dispose();
                    ho_light_region = ExpTmpOutVar_0;
                }
                //判斷是否無料(外徑小於鏡片外徑)
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_light_region, out ho_RegionFillUp);
                ho_light_region_Empty.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_light_region_Empty);
                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_Area, out hv_Row, out hv_Column);
                ho_light_region_Empty_.Dispose();
                HOperatorSet.SelectShape(ho_light_region_Empty, out ho_light_region_Empty_, "area", "and", hv_Area.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_light_region_Empty_, out hv_Area, out hv_Row, out hv_Column);
                hv_EmptyRadiusSet = dEmptyCircleRadius;
                ho_light_region_Empty.Dispose();
                HOperatorSet.SelectShape(ho_light_region_Empty_, out ho_light_region_Empty, "outer_radius", "and", 0, hv_EmptyRadiusSet);
                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_Area_Empty, out hv_Row_Empty, out hv_Column_Empty);
                HOperatorSet.TupleLength(hv_Area_Empty, out hv_Number_Empty);
                if ((int)(new HTuple(hv_Number_Empty.TupleGreater(0))) != 0)
                {
                    HOperatorSet.DispObj(ho_Image, Window);
                    HOperatorSet.DispObj(ho_light_region_Empty, Window);
                    hv_ResultCode = 0;
                    HOperatorSet.SetColor(Window, "red");
                    HOperatorSet.SetTposition(Window, 1500, 100);
                    HOperatorSet.WriteString(Window, "Miss");

                    hv_mmDistance = 0;
                }
                //切出鏡片區域,判斷是否沒墨
                ho_ImageReduced_Lens.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_light_region_Empty_, out ho_ImageReduced_Lens);
                ho_light_region_Empty.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced_Lens, out ho_light_region_Empty, hv_EmptyThreshold, 255);

                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_light_region_Area, out hv_light_region_Row, out hv_light_region_Column);

                hv_NoInkAreaSet = iNoInkAreaSet;
                if (hv_light_region_Area > hv_NoInkAreaSet)
                {
                    hv_ResultCode = -2;
                    HOperatorSet.DispObj(ho_Image, Window);
                    HOperatorSet.WriteString(Window, "無塗墨");
                    HOperatorSet.SetColor(Window, "orange");
                    hv_mmDistance = 0;
                    return;
                }

                HOperatorSet.DispObj(ho_Image, Window);
                HOperatorSet.AreaCenter(ho_light_region_Empty_, out hv_Area1, out hv_InitialRow,
                    out hv_InitialColumn);

                hv_FirstRadius = dCenterRadius;
                hv_Length = iLength;
                hv_Measure_Threshold = iMeasureThreshold;
                hv_GenParamValue = sGenParamValue;
                hv_PointChoice = PointChoice;
                ho_UsedEdges.Dispose(); ho_ResultContours.Dispose(); ho_CrossCenter.Dispose();
                gen_circle_center(ho_Image, out ho_UsedEdges, out ho_ResultContours, out ho_CrossCenter, hv_InitialRow, hv_InitialColumn, hv_FirstRadius,
                    hv_Length, hv_Measure_Threshold, hv_GenParamValue, hv_PointChoice, out hv_ResultRow, out hv_ResultColumn, out hv_ResultRadius);

                HOperatorSet.DispObj(ho_Image, Window);
                HOperatorSet.SetColor(Window, "green");
                HOperatorSet.DispObj(ho_ResultContours, Window);
            }
            catch
            {
            }
        }

        private void btnRevise_Click(object sender, EventArgs e)
        {
            pixel2um = My.VDI_Ink.pixel2um = My.VDI_Ink.iAimCirR / hv_ResultRadius / 2;
            txtpixel2um.Text = pixel2um.ToString();
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "pixel2um ", pixel2um.ToString(), Path);
        }

        private void btnEmptyRadius_Click(object sender, EventArgs e)
        {
            if (My.ContinueShot)
            {
                MessageBox.Show("請先停止預覽");
                return;
            }
            HWindow Window = hWindowControl1.HalconWindow;
            ho_Image = My.ho_Image;
            if (ho_Image == null)
                return;
            //畫檢視範圍
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.DispObj(ho_Image, Window);

            HOperatorSet.SetColor(Window, "blue");
            //找出初始半徑
            HOperatorSet.DrawCircle(Window, out dEmptyCircleRow, out dEmptyCircleColumn, out dEmptyCircleRadius);
            HOperatorSet.GenCircle(out ho_Circle_Empty, dEmptyCircleRow, dEmptyCircleColumn, dEmptyCircleRadius);

            HOperatorSet.DispObj(ho_Circle_Empty, Window);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "EmptyCircleRadius", dEmptyCircleRadius.ToString(), Path);
        }

        private void tbEmptyGraySet_ValueChanged(object sender, EventArgs e)
        {
            nudEmptyGraySet.Value = tbEmptyGraySet.Value;
        }

        private void nudEmptyGraySet_ValueChanged(object sender, EventArgs e)
        {
            iEmptyGraySet = tbEmptyGraySet.Value = Convert.ToInt32(nudEmptyGraySet.Value);
            ho_Image = My.ho_Image;

            HWindow Window = hWindowControl1.HalconWindow;
            if (ho_Image == null)
                return;
            try
            {
                hWindowControl1.HalconWindow.ClearWindow();

                HOperatorSet.SetColor(Window, "yellow");
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                HOperatorSet.DispObj(ho_Image, Window);
                ho_LensDark_region.Dispose();
                HOperatorSet.Threshold(ho_Image, out ho_LensDark_region, iEmptyGraySet, 255);

                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.RemoveNoiseRegion(ho_LensDark_region, out ExpTmpOutVar_0,
                        "n_48");
                    ho_LensDark_region.Dispose();
                    ho_LensDark_region = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_LensDark_region, out ExpTmpOutVar_0);
                    ho_LensDark_region.Dispose();
                    ho_LensDark_region = ExpTmpOutVar_0;
                }
                HOperatorSet.DispObj(ho_LensDark_region, Window);
            }
            catch
            {
            }
        }

        private void tbNoInkAreaSet_ValueChanged(object sender, EventArgs e)
        {
            nudNoInkAreaSet.Value = tbNoInkAreaSet.Value;
        }

        private void nudNoInkAreaSet_ValueChanged(object sender, EventArgs e)
        {
            iNoInkAreaSet = tbNoInkAreaSet.Value = (int)nudNoInkAreaSet.Value;

            ho_Image = My.ho_Image;

            HWindow Window = hWindowControl1.HalconWindow;
            if (ho_Image == null)
                return;
            double dEmptyCircleRadius_ = dEmptyCircleRadius;
            if (dEmptyCircleRadius_ == 0)
            {
                MessageBox.Show("請先設定Tray孔洞大小");
                return;
            }
            try
            {
                hWindowControl1.HalconWindow.ClearWindow();

                HOperatorSet.SetColor(Window, "yellow");
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                HOperatorSet.DispObj(ho_Image, Window);

                HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, iReduceRadius);
                ho_ReducedImage.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ReducedImage);
                ho_light_region.Dispose();
                hv_EmptyThreshold = iEmptyGraySet;
                ho_light_region.Dispose();
                HOperatorSet.Threshold(ho_ReducedImage, out ho_light_region, hv_EmptyThreshold, 255);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.RemoveNoiseRegion(ho_light_region, out ExpTmpOutVar_0, "n_48");
                    ho_light_region.Dispose();
                    ho_light_region = ExpTmpOutVar_0;
                }
                //判斷是否無料(外徑小於鏡片外徑)
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_light_region, out ho_RegionFillUp);
                ho_light_region_Empty.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_light_region_Empty);
                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_Area, out hv_Row, out hv_Column);
                ho_light_region_Empty_.Dispose();
                HOperatorSet.SelectShape(ho_light_region_Empty, out ho_light_region_Empty_, "area", "and", hv_Area.TupleMax(), 9999999);
                HOperatorSet.AreaCenter(ho_light_region_Empty_, out hv_Area, out hv_Row, out hv_Column);
                hv_EmptyRadiusSet = dEmptyCircleRadius;
                ho_light_region_Empty.Dispose();
                HOperatorSet.SelectShape(ho_light_region_Empty_, out ho_light_region_Empty, "outer_radius", "and", 0, hv_EmptyRadiusSet);
                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_Area_Empty, out hv_Row_Empty, out hv_Column_Empty);
                HOperatorSet.TupleLength(hv_Area_Empty, out hv_Number_Empty);
                if ((int)(new HTuple(hv_Number_Empty.TupleGreater(0))) != 0)
                {
                    HOperatorSet.DispObj(ho_Image, Window);
                    HOperatorSet.DispObj(ho_light_region_Empty, Window);
                    hv_ResultCode = 0;
                    HOperatorSet.SetColor(Window, "red");
                    HOperatorSet.SetTposition(Window, 1500, 100);
                    HOperatorSet.WriteString(Window, "Miss");

                    hv_mmDistance = 0;
                }
                //切出鏡片區域,判斷是否沒墨
                ho_ImageReduced_Lens.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_light_region_Empty_, out ho_ImageReduced_Lens);
                ho_light_region_Empty.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced_Lens, out ho_light_region_Empty, hv_EmptyThreshold, 255);

                HOperatorSet.AreaCenter(ho_light_region_Empty, out hv_light_region_Area, out hv_light_region_Row, out hv_light_region_Column);
                HOperatorSet.SetDraw(Window, "fill");
                hv_NoInkAreaSet = iNoInkAreaSet;
                if (hv_light_region_Area > hv_NoInkAreaSet)
                {
                    hv_ResultCode = -2;
                    HOperatorSet.DispObj(ho_Image, Window);
                    HOperatorSet.SetColor(Window, "yellow");
                    HOperatorSet.DispObj(ho_light_region_Empty, Window);
                    set_display_font(Window, 20, "mono", "true", "false");
                    HOperatorSet.SetColor(Window, "blue");
                    HOperatorSet.SetTposition(Window, hv_light_region_Row, hv_light_region_Column);
                    HOperatorSet.WriteString(Window, hv_light_region_Area);
                    set_display_font(Window, 30, "mono", "true", "false");
                    HOperatorSet.SetColor(Window, "red");
                    HOperatorSet.SetTposition(Window, 1500, 100);
                    HOperatorSet.WriteString(Window, "無塗墨");
                    hv_mmDistance = 0;
                    return;
                }
                else
                {
                    set_display_font(Window, 20, "mono", "true", "false");
                    HOperatorSet.SetColor(Window, "yellow");
                    HOperatorSet.DispObj(ho_light_region_Empty, Window);
                    HOperatorSet.SetColor(Window, "blue");
                    HOperatorSet.SetTposition(Window, hv_light_region_Row, hv_light_region_Column);
                    HOperatorSet.WriteString(Window, hv_light_region_Area);
                }

            }
            catch
            {
            }
        }

        private void tbUnderSizeArea_ValueChanged(object sender, EventArgs e)
        {
            nudUnderSizeArea.Value = tbUnderSizeArea.Value;
        }

        private void nudUnderSizeArea_ValueChanged(object sender, EventArgs e)
        {
            iUnderSizeArea = tbUnderSizeArea.Value = Convert.ToInt32(nudUnderSizeArea.Value);
            ImageProPlus(hWindowControl1.HalconWindow, My.ho_Image, Tray.n);
        }

        private void btnUnderSizeAreaSave_Click(object sender, EventArgs e)
        {
            My.ContinueShot = false;

            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";
            My.VDI_Ink.iUnderSizeArea = iUnderSizeArea;
            IniFile.Write("Setting", "UnderSizeArea", iUnderSizeArea.ToString(), Path);
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
