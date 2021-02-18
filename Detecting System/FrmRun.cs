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
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace Detecting_System
{
    public partial class FrmRun : Form
    {
        FrmParent parent;
        FrmVisionSet VisionSet;
        FrmVDI_NIR VDI_NIR;
        public List<Label> Labels_1 = new List<Label>();
        public List<Label> Labels_2 = new List<Label>();
        public List<Label> Labels_Last = new List<Label>();
        public List<Label> Labels_Now = new List<Label>();
        public static string[][] ArrResult;
        public static bool error = false;
        public static bool status = false;
        //釋放內存參數
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        public FrmRun(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;
            InitializeComponent();
        }

        #region //Halcon原碼
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

        public HTuple hv_ExpDefaultWinHandle;

        HObject ho_Image, ho_SymbolXLDs,ho_Region,ho_ImageReduced;

        // Local control variables 

        HTuple hv_Width = null, hv_Height = null, hv_WindowHandle = new HTuple();
        HTuple hv_DataCodeHandle = null, hv_ResultHandles = null;
        HTuple hv_DecodedDataStrings = null;
        #endregion

        private void FrmRun_Load(object sender, EventArgs e)
        {
           
            if (Sys.ReadBarcodeLog)
            {
                //连接  加@是為了讓\是正常的\
                try
                {
                    CallWithTimeout(DataBase, 20000);
                }
                catch
                {
                    DataBank.IsConnected = false;
                }

                if (status)
                    DataBank.IsConnected = true;
                DataBankReconnect.RunWorkerAsync();
            }
        }

        /// </summary>
        /// <param name="path">远程共享文件夹的路径</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        public static bool connectState(string path, string userName, string passWord)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = "net use " + path + " " + passWord + " /user:" + userName;
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                DataBank.IsConnected = false;
                //將之前已連接的帳號斷線
                string dosLine = "net use * /del /y";
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }

        void DataBase()
        {
            status = connectState(@"\\192.168.0.160\Disk1\BarcodeReader\" + DateTime.Now.ToString("yyyyMMdd"), Sys.Accounts, Sys.PassWord);
        }

        //超時跳出設置
        static void CallWithTimeout(Action action, int timeoutMilliseconds)
        {
            Thread threadToKill = null;
            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                action();
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                throw new TimeoutException();
            }
        }

        public void ReadBarcodeReaderLog(string filePath, string rowColumn, string resultColumn,out int TotalCount_Barcode)
        {
                DataBank.ResultBarcode = new string[15, 15];
            
            StreamReader reader0 = new StreamReader(filePath, System.Text.Encoding.Default, false);
            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.Default, false);
            try
            {
                //m0为从第几行开始读取；rowcolumn为要读取数据在第几列；m为文件共有几行
                int m0 = 0, rowcolumn = 0, resultcolumn = 0, m = 0;
                System.Data.DataColumn column0;
                column0 = new System.Data.DataColumn();
                while (!reader0.EndOfStream)
                {
                    m0 = m0 + 1;
                    string str0 = reader0.ReadLine();
                    string[] split0 = str0.Split('\t');
                    int count = split0.Length;
                    bool OK = false;
                    for (int i = 0; i <= count; i++)
                        if (split0[i] == rowColumn)
                        {
                            OK = true;
                            break;
                        }
                    if (OK)
                        break;
                }
                while (!reader.EndOfStream)
                {
                    m = m + 1;
                    //读取一行
                    string str = reader.ReadLine();
                    //读到的行拆成数组
                    string[] split = str.Split('\t');
                    //读取数据在哪一列
                    if (m == m0)
                    {
                        for (int c = 0; c < split.Length; c++)
                        {
                            if (split[c] == rowColumn)
                            {
                                rowcolumn = c;
                            }
                            if (split[c] == resultColumn)
                            {
                                resultcolumn = c;
                            }
                        }
                    }
                    //填入ResultBarcode二維數組
                    if (m > m0)
                    {
                        string xyStr = split[rowcolumn];
                        //处理目标数据（例 读出是1.5，1为Y，5为X）
                        string[] xy = xyStr.Split(new char[] { '.' });

                        DataBank.ResultBarcode[int.Parse(xy[0]) - 1, int.Parse(xy[1]) - 1] = split[resultcolumn];
                    }
                }
                TotalCount_Barcode = m - m0;
                reader.Close();
                reader0.Close();
            }
            catch (Exception ex)
            {
                TotalCount_Barcode = 0;
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 讀取MTF Log到視窗
        /// </summary>
        /// <param name="dst_MTF">MTF Log 路徑</param>
        /// <param name="ChuanPiao">傳票名</param>
        public void ReadLogToWindow(string dst_Barcode, string ChuanPiao)  //dst：远程服务器路径,ChuanPiao:傳票
        {
            int BarcodeCount = 0;
            int TotalCount_Barcode = 0;
            DataBank.Name_Barcode = "";
            DataTable dt_Barcode = new DataTable();
            #region 讀NIR檢測數據

            List<string> list = new List<string>();
            List<Int32> listMax = new List<Int32>();
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(dst_Barcode);
            System.IO.FileInfo[] files = dir.GetFiles(); // 获取所有文件信息。。
            Console.WriteLine("{0} 该目录下的文件有: ", dst_Barcode);
            string Name = "";
            foreach (System.IO.FileInfo file in files)
            {
                //CSVName = 找到符合傳票的文件名
                string Vicefilename = ".txt";

                //必須符合傳票以及副檔名
                if (file.Name.Contains(ChuanPiao) && file.Name.Contains(Vicefilename))
                {
                    list.Add(file.Name); //添加文件名
                }
            }
            string ResultList = "";
            if (list.Count == 0)
            {
                MessageBox.Show("找不到符合TrayBarcode的BarcodeReader檢測紀錄");
                error = true;
                return;
            }
            else
            {
                try
                {
                    int MaxList = 0;
                    int MaxList_ = 0;
                    string List = "";
                    
                    //转换成数字
                    for (int i = 0; i < list.Count; i++)
                    {
                        List = list[i];
                        if (List.Substring(0, 5) != "Done_")
                            continue;
                        string[] Timer0 = List.Split(new char[] { '_' });
                        MaxList_ = int.Parse(Timer0[2] + Timer0[3] + Timer0[4]);
                        if (MaxList < MaxList_)
                        {
                            MaxList = MaxList_;
                            ResultList = List;
                        }
                        //listMax.Add(Convert.ToInt32(list[i].Substring(list[i].Length - 8, 4))); //提取ECC后6位数字
                    }
                }
                catch { }

                //var maxValue = listMax.Max(); //获取最大值
                //for (int i = 0; i < list.Count; i++) //找到最大值的索引
                //{
                //    if (listMax[i] == maxValue)
                //        BarcodeCount = i;
                //}
                dst_Barcode = dst_Barcode + ResultList; //生成路径
                DataBank.Name_Barcode = dst_Barcode;
                listMax.Clear();
                list.Clear();
            }

            ReadBarcodeReaderLog(dst_Barcode, "Tray No.", "Barrel Bacode NO.",out TotalCount_Barcode);

            #endregion
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (Plc.Status == 1)
            {
                MessageBox.Show("運行中，請等待流程完畢。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Sys.TrayMessage)
            {
                if (txtTrayBarcode_1.Text == "" && txtTrayBarcode_2.Text== "")
                {
                    MessageBox.Show("Tray1或Tray2 Barcode不可為空!");
                    return;
                }
                if (txtClass.Text == "")
                {
                    MessageBox.Show("班別不可為空!");
                    return;
                }
                if (txtJobNumber.Text == "")
                {
                    MessageBox.Show("人員工號不可為空!");
                    return;
                }
            }
            //if (txtTrayBarcode_1.Text == "")
            //{
            //    MessageBox.Show("Tray1 Barcode為空!");
            //    return;
            //}
            //if (txtTrayBarcode_2.Text == "")
            //{
            //    MessageBox.Show("Tray2 Barcode為空!");
            //    return;
            //}

            if (!Plc.IsConnected)
            {
                MessageBox.Show("PLC未連接，请重啟!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtClass.Text == "")
            {
                MessageBox.Show("班別不可為空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtJobNumber.Text == "")
            {
                MessageBox.Show("工號不可為空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            #region 模别模穴判断赋值
            if (Sys.Function == 11&&txtTray1Mold.Text.Trim()=="")
            {
                MessageBox.Show("Tray1模别不可为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
 
            }
            if (Sys.Function == 11 && txtTray1Cave.Text.Trim() == "")
            {
                MessageBox.Show("Tray1模具不可为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            //if (Sys.Function == 11 && txtTray2Mold.Text.Trim() == "")
            //{
            //    MessageBox.Show("Tray2模别不可为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;

            //}
            //if (Sys.Function == 11 && txtTray1Cave.Text.Trim() == "")
            //{
            //    MessageBox.Show("Tray2模具不可为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;

            //}
            if (Sys.Function == 11)
            {
                MyLens_Mold_Cave.sTray1Cave = "";
                MyLens_Mold_Cave.sTray1Mold = "";
                MyLens_Mold_Cave.sTray2Cave = "";
                MyLens_Mold_Cave.sTray2Mold = "";
                MyLens_Mold_Cave.sTray1Cave = txtTray1Cave.Text.Trim();
                MyLens_Mold_Cave.sTray1Mold = txtTray1Mold.Text.Trim().ToUpper();
                MyLens_Mold_Cave.sTray2Cave = txtTray2Cave.Text.Trim();
                MyLens_Mold_Cave.sTray2Mold = txtTray2Mold.Text.Trim().ToUpper();
            }
            #endregion

            if (Sys.ReadBarcodeLog)
            {
                if (!DataBank.IsConnected)
                {
                    MessageBox.Show("資料庫連接失敗");
                    return;
                }
                if (status)
                {
                    DirectoryInfo theFolder_Barcode = new DirectoryInfo(@"\\192.168.0.160\Disk1\BarcodeReader\" + DateTime.Now.ToString("yyyyMMdd") + @"\");
                    DataBank.filename_Barcode = theFolder_Barcode.ToString();
                    string TrayBarcode = "";
                    if (txtTrayBarcode_2.Text != "")
                        TrayBarcode = txtTrayBarcode_2.Text;
                    if (txtTrayBarcode_1.Text != "")
                        TrayBarcode = txtTrayBarcode_1.Text;
                    if (TrayBarcode == "")
                    {
                        MessageBox.Show("TrayBarcode不可為空!");
                        return;
                    }

                    //执行方法
                    //TransportLocalToRemote(@"D:\readme1.txt", filename, "readme1.txt");  //实现将本地文件写入到远程服务器
                    ReadLogToWindow(DataBank.filename_Barcode, TrayBarcode);    //实现将Pro5文件讀出來
                    if (error)
                    {
                        error = false;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("資料庫未能連接！");
                }

            }
            //創建WebService交互數組
            
                WebService.ArrResult = new string[Tray.Rows_1][];
                for (int i = 0; i < Tray.Rows_1; i++)
                {
                    WebService.ArrResult[i] = new string[Tray.Columns_1] ;
                }
            
            Tray.Barcode_1 = txtTrayBarcode_1.Text;
            Tray.Barcode_2 = txtTrayBarcode_2.Text;
            if (Tray.Barcode_1 == "")
                Tray.Barcode_1 = "NA";
            if(Tray.Barcode_2 == "")
                Tray.Barcode_2 = "NA";
            lblTrayBarcode_1.Text = txtTrayBarcode_1.Text;
            lblTrayBarcode_2.Text = txtTrayBarcode_2.Text;
            txtTrayBarcode_1.Text = "";
            txtTrayBarcode_2.Text = "";
            if (txtClass.Text == "")
                txtClass.Text = "";
            int n = Tray.n;
            Vision.Images_Last.Clear();
            //是否顯示上一盤
            if (Sys.LastResult)
            {
                Vision.Images_Last.Clear();
                    HObject LastImage;
                    HOperatorSet.GenEmptyObj(out LastImage);
                    int TotalCount = Tray.Rows_1*Tray.Columns_1;
                    
                    if (Vision.Images_Now.Count != 0)
                    {
                        for (int i = 0; i <= TotalCount-1; i++)
                        {
                             try
                            {
                                if (!Vision.Images_Now.ContainsKey(i))
                                    continue;
                                HOperatorSet.CopyImage(Vision.Images_Now[i], out LastImage);
                                Vision.Images_Last[i] = LastImage;
                            }
                             catch
                             {
                             }
                        }
                    }
                    Labels_Last = Labels_2;
                    Labels_Now = Labels_1;
                    CopyLabels(Labels_2, Labels_1,panelResult_2,panelResult_1, Tray.Rows_1, Tray.Columns_1, Tray.CurrentRow, Tray.CurrentColumn);
                
            }
            else
            {
                //Label1,2復位
                if (Labels_1.Count != (Tray.Rows_1 * Tray.Columns_1))
                {
                    ReleaseLabels_1();
                    GenerateLabels_1();
                }
                else
                {
                    InitialLabels_1();
                }
                if (Labels_2.Count != (Tray.Rows_2 * Tray.Columns_2))
                {
                    ReleaseLabels_2();
                    GenerateLabels_2();
                }
                else
                {
                    InitialLabels_2();
                }
            }
            foreach (var Key in Vision.Images_1)
            {
                if (Vision.Images_1.ContainsKey(Key.Key))    //如果存在
                    Vision.Images_1[Key.Key].Dispose();
            }
            foreach (var Key in Vision.Images_2)
            {
                if (Vision.Images_1.ContainsKey(Key.Key))    //如果存在
                    Vision.Images_1[Key.Key].Dispose();
            }
            foreach (var Key in Vision.Images_Now)
            {
                if (Vision.Images_1.ContainsKey(Key.Key))    //如果存在
                    Vision.Images_1[Key.Key].Dispose();
            }
            foreach (var Key in Vision.ImagesOriginal_1)
            {
                if (Vision.Images_1.ContainsKey(Key.Key))    //如果存在
                    Vision.Images_1[Key.Key].Dispose();
            }
            foreach (var Key in Vision.ImagesOriginal_2)
            {
                if (Vision.Images_1.ContainsKey(Key.Key))    //如果存在
                    Vision.Images_1[Key.Key].Dispose();
            }
            
            Vision.Images_1.Clear();
            Vision.Images_2.Clear();
            Vision.Images_Now.Clear();
            Vision.VisionResult.Clear();
            Vision.ProcessingTime.Clear();
            Vision.ImagesOriginal_1.Clear();
            Vision.ImagesOriginal_2.Clear();
            Vision.VisionBarcodeResult.Clear();
            
            Tray.Class = txtClass.Text;
            Tray.OperatorID = txtJobNumber.Text;
            Protocol.bPCOK = true;
            Tray.OpDateTime = DateTime.Now;
            //parent.Clear_Read_Temple();
            Protocol.bVisionOK = false;
            Plc.bPlateful_1 = false;
            Plc.bPlateful_2 = false;
            Vision.FolderName = Sys.LogPath + "\\" + Tray.OpDateTime.ToString("yyyyMMdd") + "\\";
            Vision.FileName = Production.CurProduction + "-" + Sys.MachineID +
                             Tray.OpDateTime.ToString("-yyyyMMdd_HH_mm_ss_") +
                             Sys.Codes + "-" +
                             Tray.Barcode_1 + "-" + Tray.Barcode_2 + ".txt";
        }

        void GenerateLabels_1()//生成Labels（映射Tray上面的Lens）
        {
            int rows = Tray.Rows_1;
            int cols = Tray.Columns_1;
            lblCntRow.Text = Tray.Rows_1.ToString();
            lblCntCol.Text = Tray.Columns_1.ToString();
            lblCurrentRow.Text = Tray.CurrentRow.ToString();
            lblCurrentCol.Text = Tray.CurrentColumn.ToString();
            int height = (panelResult_1.Height) / rows;
            int width = (panelResult_1.Width) / cols;
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    if (Plc.TrayMode_1 == 0 || Plc.TrayMode_1 == 1)
                    {
                        Label lbl = new Label();
                        lbl.AutoSize = false;
                        lbl.BackColor = Color.Gray;
                        lbl.Size = new System.Drawing.Size(width - (int)(width * 0.95), height - (int)(height * 0.95));
                        lbl.Location = new Point(width * j, height * i);
                        lbl.ForeColor = Color.White;
                        lbl.TextAlign = ContentAlignment.MiddleCenter;
                        lbl.Font = new Font("宋体", 6, FontStyle.Bold);
                        lbl.Click += LabelClick_1;
                        panelResult_1.Controls.Add(lbl);
                        Labels_1.Add(lbl);
                    }
                    else if(Plc.TrayMode_1==2)
                    {
                        if (i % 2 == 0)
                        {
                            Label lbl = new Label();
                            lbl.AutoSize = false;
                            lbl.BackColor = Color.Gray;
                            lbl.Size = new System.Drawing.Size(width - (int)(width * 0.95), height - (int)(height * 0.95));
                            lbl.Location = new Point(width * j, height * i);
                            lbl.ForeColor = Color.White;
                            lbl.TextAlign = ContentAlignment.MiddleCenter;
                            lbl.Font = new Font("宋体", 6, FontStyle.Bold);
                            lbl.Click += LabelClick_1;
                            panelResult_1.Controls.Add(lbl);
                            Labels_1.Add(lbl);
                        }
                        else
                        {
                            Label lbl = new Label();
                            lbl.AutoSize = false;
                            lbl.BackColor = Color.Gray;
                            lbl.Size = new System.Drawing.Size(width - (int)(width * 0.95), height - (int)(height * 0.95));
                            lbl.Location = new Point(width * j + width/2, height * i);
                            lbl.ForeColor = Color.White;
                            lbl.TextAlign = ContentAlignment.MiddleCenter;
                            lbl.Font = new Font("宋体", 6, FontStyle.Bold);
                            if (j == cols - 1)
                                lbl.Visible = false;
                            lbl.Click += LabelClick_1;
                            panelResult_1.Controls.Add(lbl);
                            Labels_1.Add(lbl);
                        }
                    }
                    else if (Plc.TrayMode_1 == 3)
                    {
                        if (i % 2 == 1)
                        {
                            Label lbl = new Label();
                            lbl.AutoSize = false;
                            lbl.BackColor = Color.Gray;
                            lbl.Size = new System.Drawing.Size(width - (int)(width * 0.95), height - (int)(height * 0.95));
                            lbl.Location = new Point(width * j + width/2, height * i);
                            lbl.ForeColor = Color.White;
                            lbl.TextAlign = ContentAlignment.MiddleCenter;
                            lbl.Font = new Font("宋体", 6, FontStyle.Bold);
                            if (j == cols - 1)
                                lbl.Visible = false;
                            lbl.Click += LabelClick_1;
                            panelResult_1.Controls.Add(lbl);
                            Labels_1.Add(lbl);
                        }
                        else
                        {
                            Label lbl = new Label();
                            lbl.AutoSize = false;
                            lbl.BackColor = Color.Gray;
                            lbl.Size = new System.Drawing.Size(width - (int)(width * 0.95), height - (int)(height * 0.95));
                            lbl.Location = new Point(width * j, height * i);
                            lbl.ForeColor = Color.White;
                            lbl.TextAlign = ContentAlignment.MiddleCenter;
                            lbl.Font = new Font("宋体", 6, FontStyle.Bold);
                            lbl.Click += LabelClick_1;
                            panelResult_1.Controls.Add(lbl);
                            Labels_1.Add(lbl);
                        }
                    }
                }
            }
        }

        void GenerateLabels_2()//生成Labels（映射Tray上面的Lens）
        {
            int rows = Tray.Rows_2;
            int cols = Tray.Columns_2;
            lblCntRow.Text = Tray.Rows_2.ToString();
            lblCntCol.Text = Tray.Columns_2.ToString();
            lblCurrentRow.Text = Tray.CurrentRow.ToString();
            lblCurrentCol.Text = Tray.CurrentColumn.ToString();
            int height = (panelResult_2.Height) / rows;
            int width = (panelResult_2.Width) / cols;
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    if (Plc.TrayMode_2 == 0 || Plc.TrayMode_2 == 1)
                    {
                        Label lbl = new Label();
                        lbl.AutoSize = false;
                        lbl.BackColor = Color.Gray;
                        lbl.Size = new System.Drawing.Size(width - (int)(width * 0.05), height - (int)(height * 0.05));
                        lbl.Location = new Point(width * j, height * i);
                        lbl.ForeColor = Color.White;
                        lbl.TextAlign = ContentAlignment.MiddleCenter;
                        lbl.Font = new Font("宋体", 8, FontStyle.Bold);
                        lbl.Click += LabelClick_2;
                        panelResult_2.Controls.Add(lbl);
                        Labels_2.Add(lbl);
                    }
                    else if (Plc.TrayMode_2 == 2)
                    {
                        if (i % 2 == 0)
                        {
                            Label lbl = new Label();
                            lbl.AutoSize = false;
                            lbl.BackColor = Color.Gray;
                            lbl.Size = new System.Drawing.Size(width - (int)(width * 0.05), height - (int)(height * 0.05));
                            lbl.Location = new Point(width * j, height * i);
                            lbl.ForeColor = Color.White;
                            lbl.TextAlign = ContentAlignment.MiddleCenter;
                            lbl.Font = new Font("宋体", 8, FontStyle.Bold);
                            lbl.Click += LabelClick_2;
                            panelResult_2.Controls.Add(lbl);
                            Labels_2.Add(lbl);
                        }
                        else
                        {
                            Label lbl = new Label();
                            lbl.AutoSize = false;
                            lbl.BackColor = Color.Gray;
                            lbl.Size = new System.Drawing.Size(width - (int)(width * 0.05), height - (int)(height * 0.05));
                            lbl.Location = new Point(width * (int)(j+0.5), height * i);
                            lbl.ForeColor = Color.White;
                            lbl.TextAlign = ContentAlignment.MiddleCenter;
                            lbl.Font = new Font("宋体", 8, FontStyle.Bold);
                            lbl.Click += LabelClick_2;
                            panelResult_2.Controls.Add(lbl);
                            Labels_2.Add(lbl);
                        }
                    }
                    else if (Plc.TrayMode_2 == 3)
                    {
                        if (i % 2 == 1)
                        {
                            Label lbl = new Label();
                            lbl.AutoSize = false;
                            lbl.BackColor = Color.Gray;
                            lbl.Size = new System.Drawing.Size(width - (int)(width * 0.05), height - (int)(height * 0.05));
                            lbl.Location = new Point(width * j, height* i);
                            lbl.ForeColor = Color.White;
                            lbl.TextAlign = ContentAlignment.MiddleCenter;
                            lbl.Font = new Font("宋体", 8, FontStyle.Bold);
                            lbl.Click += LabelClick_2;
                            panelResult_2.Controls.Add(lbl);
                            Labels_2.Add(lbl);
                        }
                        else
                        {
                            Label lbl = new Label();
                            lbl.AutoSize = false;
                            lbl.BackColor = Color.Gray;
                            lbl.Size = new System.Drawing.Size(width - (int)(width * 0.05), height - (int)(height * 0.05));
                            lbl.Location = new Point(width * (int)(j + 0.5), height * i);
                            lbl.ForeColor = Color.White;
                            lbl.TextAlign = ContentAlignment.MiddleCenter;
                            lbl.Font = new Font("宋体", 8, FontStyle.Bold);
                            lbl.Click += LabelClick_2;
                            panelResult_2.Controls.Add(lbl);
                            Labels_2.Add(lbl);
                        }
                    }
                }
            }
        }

        void CopyLabels(List<Label> LabelLast, List<Label> LabelNow,Panel PanelLast,Panel PanelNow, int Rows,int Cols,int CurrentRow,int CurrentCol)
        {
            for (int i = 0; i < LabelLast.Count; ++i)
            {
                //清除上一個紀錄
                LabelLast[i].Dispose();
            }
            LabelLast.Clear();
            //創建上一個
            lblCntRow.Text = Rows.ToString();
            lblCntCol.Text = Cols.ToString();
            lblCurrentRow.Text = CurrentRow.ToString();
            lblCurrentCol.Text = CurrentCol.ToString();
            int height = (PanelLast.Height) / Rows;
            int width = (PanelLast.Width) / Cols;
            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Cols; ++j)
                {
                    if (Plc.TrayMode_1 == 0 || Plc.TrayMode_1 == 1)
                    {
                        Label lbl2 = new Label();
                        lbl2.AutoSize = false;
                        lbl2.BackColor = Color.Gray;
                        lbl2.Size = new System.Drawing.Size(width - 1, height - 1);
                        lbl2.Location = new Point(width * j, height * i);
                        lbl2.ForeColor = Color.White;
                        lbl2.TextAlign = ContentAlignment.MiddleCenter;
                        lbl2.Font = new Font("宋体", 6, FontStyle.Bold);
                        lbl2.Click += LabelClick_Last;
                        PanelLast.Controls.Add(lbl2);
                        LabelLast.Add(lbl2);
                    }
                    else if (Plc.TrayMode_1 == 2)
                    {
                        if (i % 2 == 0)
                        {
                            Label lbl2 = new Label();
                            lbl2.AutoSize = false;
                            lbl2.BackColor = Color.Gray;
                            lbl2.Size = new System.Drawing.Size(width - 1, height - 1);
                            lbl2.Location = new Point(width * j, height * i);
                            lbl2.ForeColor = Color.White;
                            lbl2.TextAlign = ContentAlignment.MiddleCenter;
                            lbl2.Font = new Font("宋体", 6, FontStyle.Bold);
                            lbl2.Click += LabelClick_Last;
                            PanelLast.Controls.Add(lbl2);
                            LabelLast.Add(lbl2);
                        }
                        else
                        {
                            Label lbl2 = new Label();
                            lbl2.AutoSize = false;
                            lbl2.BackColor = Color.Gray;
                            lbl2.Size = new System.Drawing.Size(width - 1, height - 1);
                            lbl2.Location = new Point(width * j + width / 2, height * i);
                            lbl2.ForeColor = Color.White;
                            lbl2.TextAlign = ContentAlignment.MiddleCenter;
                            lbl2.Font = new Font("宋体", 6, FontStyle.Bold);
                            if (j == Cols - 1)
                                lbl2.Visible = false;
                            lbl2.Click += LabelClick_Last;
                            PanelLast.Controls.Add(lbl2);
                            LabelLast.Add(lbl2);
                        }
                    }
                    else if (Plc.TrayMode_1 == 3)
                    {
                        if (i % 2 == 0)
                        {
                            Label lbl2 = new Label();
                            lbl2.AutoSize = false;
                            lbl2.BackColor = Color.Gray;
                            lbl2.Size = new System.Drawing.Size(width - 1, height - 1);
                            lbl2.Location = new Point(width * j + width / 2, height * i);
                            lbl2.ForeColor = Color.White;
                            lbl2.TextAlign = ContentAlignment.MiddleCenter;
                            lbl2.Font = new Font("宋体", 6, FontStyle.Bold);
                            if (j == Cols - 1)
                                lbl2.Visible = false;
                            lbl2.Click += LabelClick_Last;
                            PanelLast.Controls.Add(lbl2);
                            LabelLast.Add(lbl2);
                        }
                        else
                        {
                            Label lbl2 = new Label();
                            lbl2.AutoSize = false;
                            lbl2.BackColor = Color.Gray;
                            lbl2.Size = new System.Drawing.Size(width - 1, height - 1);
                            lbl2.Location = new Point(width * j, height * i);
                            lbl2.ForeColor = Color.White;
                            lbl2.TextAlign = ContentAlignment.MiddleCenter;
                            lbl2.Font = new Font("宋体", 6, FontStyle.Bold);
                            lbl2.Click += LabelClick_Last;
                            PanelLast.Controls.Add(lbl2);
                            LabelLast.Add(lbl2);
                        }
                    }
                }
            }
            if (LabelNow.Count != 0)
            {
                for (int i = 0; i < LabelNow.Count; ++i)
                {
                    LabelLast[i].BackColor = LabelNow[i].BackColor;
                    LabelLast[i].Font = new Font("宋体", 7, FontStyle.Bold);
                    LabelLast[i].ForeColor = LabelNow[i].ForeColor;
                    LabelLast[i].Text = LabelNow[i].Text;
                    LabelNow[i].Dispose();
                }
                LabelNow.Clear();
            }
            //創建新的
            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Cols; ++j)
                {
                    if (Plc.TrayMode_1 == 0 || Plc.TrayMode_1 == 1)
                    {
                        Label lbl = new Label();
                        lbl.AutoSize = false;
                        lbl.BackColor = Color.Gray;
                        lbl.Size = new System.Drawing.Size(width - (int)(width * 0.05), height - (int)(height * 0.05));
                        lbl.Location = new Point(width * j, height * i);
                        lbl.ForeColor = Color.White;
                        lbl.TextAlign = ContentAlignment.MiddleCenter;
                        lbl.Font = new Font("宋体", 6, FontStyle.Bold);
                        lbl.Click += LabelClick_Now;
                        PanelNow.Controls.Add(lbl);
                        LabelNow.Add(lbl);
                    }
                    else if (Plc.TrayMode_1 == 2)
                    {
                        if (i % 2 == 0)
                        {
                            Label lbl = new Label();
                            lbl.AutoSize = false;
                            lbl.BackColor = Color.Gray;
                            lbl.Size = new System.Drawing.Size(width - (int)(width * 0.05), height - (int)(height * 0.05));
                            lbl.Location = new Point(width * j, height * i);
                            lbl.ForeColor = Color.White;
                            lbl.TextAlign = ContentAlignment.MiddleCenter;
                            lbl.Font = new Font("宋体", 6, FontStyle.Bold);
                            lbl.Click += LabelClick_Now;
                            PanelNow.Controls.Add(lbl);
                            LabelNow.Add(lbl);
                        }
                        else
                        {
                            Label lbl = new Label();
                            lbl.AutoSize = false;
                            lbl.BackColor = Color.Gray;
                            lbl.Size = new System.Drawing.Size(width - (int)(width * 0.05), height - (int)(height * 0.05));
                            lbl.Location = new Point(width * j + width / 2, height * i);
                            lbl.ForeColor = Color.White;
                            lbl.TextAlign = ContentAlignment.MiddleCenter;
                            lbl.Font = new Font("宋体", 6, FontStyle.Bold);
                            if (j == Cols - 1)
                                lbl.Visible = false;
                            lbl.Click += LabelClick_Now;
                            PanelNow.Controls.Add(lbl);
                            LabelNow.Add(lbl);
                        }
                    }
                    else if (Plc.TrayMode_1 == 3)
                    {
                        if (i % 2 == 0)
                        {
                            Label lbl = new Label();
                            lbl.AutoSize = false;
                            lbl.BackColor = Color.Gray;
                            lbl.Size = new System.Drawing.Size(width - (int)(width * 0.05), height - (int)(height * 0.05));
                            lbl.Location = new Point(width * j + width / 2, height * i);
                            lbl.ForeColor = Color.White;
                            lbl.TextAlign = ContentAlignment.MiddleCenter;
                            lbl.Font = new Font("宋体", 6, FontStyle.Bold);
                            lbl.Click += LabelClick_Now;
                            if (j == Cols - 1)
                                lbl.Visible = false;
                            PanelNow.Controls.Add(lbl);
                            LabelNow.Add(lbl);
                        }
                        else
                        {
                            Label lbl = new Label();
                            lbl.AutoSize = false;
                            lbl.BackColor = Color.Gray;
                            lbl.Size = new System.Drawing.Size(width - (int)(width * 0.05), height - (int)(height * 0.05));
                            lbl.Location = new Point(width * j, height * i);
                            lbl.ForeColor = Color.White;
                            lbl.TextAlign = ContentAlignment.MiddleCenter;
                            lbl.Font = new Font("宋体", 6, FontStyle.Bold);
                            lbl.Click += LabelClick_Now;
                            PanelNow.Controls.Add(lbl);
                            LabelNow.Add(lbl);
                        }
                    }
                }
            }
        }

        void ReleaseLabels_1()//释放Labels
        {
            for (int i = 0; i < Labels_1.Count; ++i)
            {
                Labels_1[i].Dispose();
            }
            Labels_1.Clear();
        }

        void ReleaseLabels_2()//释放Labels
        {
            for (int i = 0; i < Labels_2.Count; ++i)
            {
                Labels_2[i].Dispose();
            }
            Labels_2.Clear();
        }

        void InitialLabels_1()//复位Labels属性
        {
            for (int i = 0; i < Labels_1.Count; i++)
            {
                Labels_1[i].BackColor = Color.Gray;
                Labels_1[i].Text = "";
                //txtResult.Text = "";
            }
            //picImgResult.Image = null;
        }

        void InitialLabels_2()//复位Labels属性
        {
            for (int i = 0; i < Labels_2.Count; i++)
            {
                Labels_2[i].BackColor = Color.Gray;
                Labels_2[i].Text = "";
                //txtResult.Text = "";
            }
            //picImgResult.Image = null;
        }
        void LabelClick_1(object obj, EventArgs e)//调出对应Lens的視覺辨識结果
        {
            Label lbl = obj as Label;
            int n = Labels_1.IndexOf(lbl);

            if (Vision.Images_1.ContainsKey(n))
            {
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GetImageSize(Vision.Images_1[n], out hv_Width, out hv_Height);
                HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, hv_Height, hv_Width);
                HOperatorSet.DispObj(Vision.Images_1[n], hWindowControl1.HalconWindow);

            }
            else
            {
                hWindowControl1.HalconWindow.ClearWindow();
            }
        }

        void LabelClick_2(object obj, EventArgs e)//调出对应Lens的視覺辨識结果
        {
            Label lbl = obj as Label;
            int n = Labels_2.IndexOf(lbl);

            if (Vision.Images_2.ContainsKey(n))
            {
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GetImageSize(Vision.Images_2[n], out hv_Width, out hv_Height);
                HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, hv_Height, hv_Width);
                HOperatorSet.DispObj(Vision.Images_2[n], hWindowControl1.HalconWindow);

            }
            else
            {
                hWindowControl1.HalconWindow.ClearWindow();
            }
        }

        void LabelClick_Last(object obj, EventArgs e)//调出对应Lens的視覺辨識结果
        {
            Label lbl = obj as Label;
            
                Labels_Last = Labels_2;
            
            int n = Labels_Last.IndexOf(lbl);

            if (Vision.Images_Last.ContainsKey(n))
            {
                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.GetImageSize(Vision.Images_Last[n], out hv_Width, out hv_Height);
                HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, hv_Height, hv_Width);
                HOperatorSet.DispObj(Vision.Images_Last[n], hWindowControl1.HalconWindow);

            }
            else
            {
                hWindowControl1.HalconWindow.ClearWindow();
            }
        }

        void LabelClick_Now(object obj, EventArgs e)//调出对应Lens的視覺辨識结果
        {
            Label lbl = obj as Label;
            
                Labels_Now = Labels_1;
           
            int n = Labels_Now.IndexOf(lbl);

            try
            {
                if (Vision.Images_Now.ContainsKey(n))
                {
                    hWindowControl1.HalconWindow.ClearWindow();
                    HOperatorSet.GetImageSize(Vision.Images_Now[n], out hv_Width, out hv_Height);
                    HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, hv_Height, hv_Width);
                    HOperatorSet.DispObj(Vision.Images_Now[n], hWindowControl1.HalconWindow);

                }
                else
                {
                    hWindowControl1.HalconWindow.ClearWindow();
                }
            }
            catch
            { }
        }

        private void timerUI_Tick(object sender, EventArgs e)
        {
            if (Plc.IsConnected)
            {
                picPlcConnect.BackColor = Color.Green;
            }
            else
            {
                picPlcConnect.BackColor = Color.Red;
            }
            if (CCD.CCDBrand == 0)
            {
                CCD.IsConnected = parent.myBasler.IsConnected;
                picCCDConnect.BackColor = CCD.IsConnected ? Color.Green : Color.Red;
            }
            else
            {
                CCD.IsConnected = parent.myHikvision.IsConnected;
                picCCDConnect.BackColor = CCD.IsConnected ? Color.Green : Color.Red;
            }
            if (CCD.IsConnected)
            {
                parent.GetParam();
            }
            if (Reader.IsConnected)
            {
                picReaderConnect.BackColor = Color.Green;
            }
            else
            {
                picReaderConnect.BackColor = Color.Red;
            }
            if (Light.IsConnected)
            {
                picLightConnect.BackColor = Color.Green;
            }
            else
            {
                picLightConnect.BackColor = Color.Red;
            }
            if (DataBank.IsConnected)
            {
                picDataBankConnect.BackColor = Color.Green;
            }
            else
            {
                picDataBankConnect.BackColor = Color.Red;
            }
            if (!Sys.ManualClear)
            {
                lblCountMessage.Text = "自動模式:每日8,20時自動歸零";
            }
            else
            {
                lblCountMessage.Text = "手動模式:需自行清零";
            }
            if (Sys.Function == 5)
            {
                lblBarrelBarcode1.Visible = true;
                lblBarrelBarcode2.Visible = true;
                if (My.NIR.ReadBarrelBarcode)
                {
                    lblBarrelBarcode2.ForeColor = Color.Green;
                    lblBarrelBarcode2.Text = "Open";
                }
                else
                {
                    lblBarrelBarcode2.ForeColor = Color.Red;
                    lblBarrelBarcode2.Text = "Close";
                }
            }
            if (Sys.Function == 7)
            {
                lblProduction.Visible = true;
                lblProduction2.Visible = true;
                switch (My.Classifier.iProductionSet)
                {
                    case 0:
                        {
                            lblProduction2.Text = "DZ";
                            break;
                        }
                    case 1:
                        {
                            lblProduction2.Text = "EZ";
                            break;
                        }
                    case 2:
                        {
                            lblProduction2.Text = "HZ";
                            break;
                        }
                }
                
            }
            switch(Plc.RunMode)
            {
                case 1://僅盤一
                {
                    lblTray1.Enabled = true;
                    lblTray2.Enabled = false;
                    txtTrayBarcode_1.Enabled = true;
                    txtTrayBarcode_2.Enabled = false;
                    break;
                }
                case 2://僅盤二
                {
                    lblTray1.Enabled = false;
                    lblTray2.Enabled = true;
                    txtTrayBarcode_1.Enabled = false;
                    txtTrayBarcode_2.Enabled = true;
                    break;
                }
                default://其他
                {
                    lblTray1.Enabled = true;
                    lblTray2.Enabled = true;
                    txtTrayBarcode_1.Enabled = true;
                    txtTrayBarcode_2.Enabled = true;
                    break;
                }

            }
            // 模穴识别群组框不可见
            #region
            if (Sys.Function == 11)
            {
                gBMoldCaveSetUp.Visible = true;
                if (Plc.RunMode == 1 && Plc.Status == 0)
                {
                    txtTray1Mold.Enabled = true;
                    txtTray1Cave.Enabled = true;
                    txtTray2Mold.Enabled = false;
                    txtTray2Cave.Enabled = false;
                }
                else if (Plc.RunMode == 2 && Plc.Status == 0)
                {
                    txtTray1Mold.Enabled = false;
                    txtTray1Cave.Enabled = false;
                    txtTray2Mold.Enabled = true;
                    txtTray2Cave.Enabled = true;

                }
                else if (Plc.Status == 1)
                {
                    txtTray1Mold.Enabled = false;
                    txtTray1Cave.Enabled = false;
                    txtTray2Mold.Enabled = false;
                    txtTray2Cave.Enabled = false;

                }

                else
                {
                    txtTray1Mold.Enabled = false;
                    txtTray1Cave.Enabled = false;
                    txtTray2Mold.Enabled = true;
                    txtTray2Cave.Enabled = true;
                }
        

            }
            else
            {
                gBMoldCaveSetUp.Visible = false;

            }
            #endregion



            if (Plc.Status == 1 && Plc.Status_Last == 0)
            {
                Plc.Status_Last = 1;
            }
            if (Plc.Status == 0 && Plc.Status_Last == 1)
            {
                Plc.Status_Last = 0;
                if (Sys.Function != 9)
                {
                    string FileName = Vision.FolderName + Vision.FileName;
                    string newFileName = Vision.FolderName + "Done_" + Vision.FileName;
                    if (File.Exists(FileName))// && Transmission.code != ""
                    {
                        if (!ModifyFilename(FileName, newFileName))
                        {
                            try
                            {
                                if (File.Exists(FileName))
                                {
                                    FileInfo fi = new FileInfo(FileName);
                                    fi.MoveTo(newFileName);
                                }
                            }
                            catch
                            {
                                //
                            }
                        }
                    }
                }
                if (Sys.VisitWebService)
                {
                    if (Sys.Function == 5) 
                    {
                        if (Sys.Codes == "M")
                        {
                            bool End = false;

                            End = Plc.bPlateful_WS;

                            //如果沒接收滿盤信號,跳出彈窗確認是否出站
                            if (!End)
                            {
                                DialogResult dr = MessageBox.Show("確定要出站嗎?若出站后就无法重复检测，直接流到MTF站", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);

                                if (dr != DialogResult.OK)
                                {
                                    Plc.bPlateful_WS = false;
                                    return;
                                }
                                else
                                {
                                    Plc.bPlateful_WS = false;
                                }
                            }
                            else
                            {
                                Plc.bPlateful_WS = false;
                            }
                            try
                            {
                                CallWithTimeout(DataBase_WS_End, 10000);
                            }
                            catch
                            {
                                //My.sResult = "N";
                                Protocol.Result_BarcodeOK = 2;
                                MessageBox.Show("訪問WebService超時10秒異常,請檢查網路連線,若網路無異常請聯絡資訊 葉奇彬*3641處理", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            }
                            if (!WebService.NIRResult_End)
                            {
                                Protocol.Result_BarcodeOK = 2;
                                MessageBox.Show(WebService.NIRMsg_End, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            }
                            if (Plc.Plateful_1 == 1)//通知歸零
                            {
                                Protocol.PlatefulClear_1 = true;
                            }
                            if (Plc.Plateful_2 == 1)
                            {
                                Protocol.PlatefulClear_2 = true;
                            }
                        }
                    }
                   
                }
                //清除暫存
                GC.Collect();
                //GC.WaitForPendingFinalizers();
                //if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                //{
                //    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                //}
            }
            Count.iTotal = Count.iOK + Count.iNG + Count.iNG2 + Count.iNG3 + Count.iNG4;
            lblNowTray.Text = 1.ToString();
            lblCntRow.Text = Tray.Rows_1.ToString();
            lblCntCol.Text = Tray.Columns_1.ToString();
            lblCurrentRow.Text = Tray.CurrentRow.ToString();
            lblCurrentCol.Text = Tray.CurrentColumn.ToString();
            lblCntRow2.Text = Protocol.BarcodeReaderPlus_MaxRow.ToString();
            lblCntCol2.Text = Protocol.BarcodeReaderPlus_MaxColumn.ToString();
            lblCurrentRow2.Text = Protocol.BarcodeReaderPlus_NowRow.ToString();
            lblCurrentCol2.Text = Protocol.BarcodeReaderPlus_NowColumn.ToString();
           
            lblTotalCount.Text = Count.iTotal.ToString();
            lblOKCount.Text = Count.iOK.ToString();
            lblNGCount.Text = Count.iNG.ToString();
            lblNG2Count.Text = Count.iNG2.ToString();
            lblNG3Count.Text = Count.iNG3.ToString();
            lblNG4Count.Text = Count.iNG4.ToString();
            lblNG5Count.Text = Count.iNG5.ToString();
            lblTestCount.Text = Count.iTest.ToString();
            lblMissCount.Text = Count.iMiss.ToString();
            lblTestTotal.Text = Count.iTestTotal.ToString();
            if (Count.iTotal != 0)
            {
                Count.dTotalRatio = Math.Round(Count.iTotal / Count.iTotal * 100, 1);
                Count.dOKRatio = Math.Round(Count.iOK / Count.iTotal * 100, 1);
                Count.dNGRatio = Math.Round(Count.iNG / Count.iTotal * 100, 1);
                Count.dNG2Ratio = Math.Round(Count.iNG2 / Count.iTotal * 100, 1);
                Count.dNG3Ratio = Math.Round(Count.iNG3 / Count.iTotal * 100, 1);
                Count.dNG4Ratio = Math.Round(Count.iNG4 / Count.iTotal * 100, 1);
                Count.dNG5Ratio = Math.Round(Count.iNG5 / Count.iTotal * 100, 1);
                if (Count.iTestTotal!=0)
                Count.dTestRatio = Math.Round(Count.iTest / Count.iTestTotal * 100, 1);
                lblTotalRatio.Text = Count.dTotalRatio.ToString() + "%";
                lblOKRatio.Text = Count.dOKRatio.ToString() + "%";
                lblNGRatio.Text = Count.dNGRatio.ToString() + "%";
                lblNG2Ratio.Text = Count.dNG2Ratio.ToString() + "%";
                lblNG3Ratio.Text = Count.dNG3Ratio.ToString() + "%";
                lblNG4Ratio.Text = Count.dNG4Ratio.ToString() + "%";
                lblNG5Ratio.Text = Count.dNG5Ratio.ToString() + "%";
                lblTestRatio.Text = Count.dTestRatio.ToString() + "%";
            }
            else
            {
                lblTotalRatio.Text = "0%";
                lblOKRatio.Text = "0%";
                lblNGRatio.Text = "0%";
                lblNG2Ratio.Text ="0%";
                lblNG3Ratio.Text = "0%";
                lblNG4Ratio.Text = "0%";
                lblNG5Ratio.Text = "0%";
            }
        }

        public bool ModifyFilename(string Filename, string newFilename)
        {
            try
            {
                if (File.Exists(Filename))
                {
                    FileInfo fi = new FileInfo(Filename);
                    fi.MoveTo(newFilename);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            parent.WriteCountLog();
            Count.iOK = 0;
            Count.iNG = 0;
            Count.iNG2 = 0;
            Count.iNG3 = 0;
            Count.iNG4 = 0;
            Count.iNG5 = 0;
            Count.iMiss = 0;
        }
        public void ReadBarcode(HWindow hWindowControl)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            try
            {
                ho_Image = My.ho_Image;
                hv_ExpDefaultWinHandle = hWindowControl;
                HOperatorSet.SetLineWidth(hv_ExpDefaultWinHandle, 2);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.CreateDataCode2dModel("Data Matrix ECC 200", "default_parameters",
                    "enhanced_recognition", out hv_DataCodeHandle);
                HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "polarity", "light_on_dark");
                HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "symbol_rows_max", 12);
                HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "symbol_rows_max", 14);
                HOperatorSet.SetDataCode2dParam(hv_DataCodeHandle, "persistence", 1);
                ho_SymbolXLDs.Dispose();
                HOperatorSet.FindDataCode2d(ho_Image, out ho_SymbolXLDs, hv_DataCodeHandle,
                    "train", "all", out hv_ResultHandles, out hv_DecodedDataStrings);
                set_display_font(hv_ExpDefaultWinHandle, 20, "mono", "true", "false");
                HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, 0, 0);
                HOperatorSet.DumpWindowImage(out ho_Image, hv_ExpDefaultWinHandle);
                if ((int)(new HTuple((new HTuple(hv_DecodedDataStrings.TupleLength())).TupleEqual(
                    0))) != 0)
                {
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "NA");
                    
                        txtTrayBarcode_1.Text = Tray.Barcode_1 = "NA";
                        Vision.BarcodeResult_1 = "NG";
                        if (Labels_1.Count != (Tray.Rows_1 * Tray.Columns_1))
                        {
                            ReleaseLabels_1();
                            GenerateLabels_1();
                        }
                        else
                        {
                            InitialLabels_1();
                        }
                        Vision.Images_1.Clear();
                        Protocol.bPCOK = true;
                        Tray.OpDateTime = DateTime.Now;
                    
                }
                else
                {
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "green");
                    HOperatorSet.DispObj(ho_SymbolXLDs, hv_ExpDefaultWinHandle);
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, hv_DecodedDataStrings);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_SymbolXLDs, out ho_Region, "filled");
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_Region, out ho_ImageReduced);
                    
                        txtTrayBarcode_1.Text = Tray.Barcode_1 = hv_DecodedDataStrings;
                        Vision.BarcodeResult_1 = "OK";
                        if (Labels_1.Count != (Tray.Rows_1 * Tray.Columns_1))
                        {
                            ReleaseLabels_1();
                            GenerateLabels_1();
                        }
                        else
                        {
                            InitialLabels_1();
                        }
                        Vision.Images_1.Clear();
                        Protocol.bPCOK = true;
                        Tray.OpDateTime = DateTime.Now;
                   
                }
            }
            catch
            {
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                HOperatorSet.WriteString(hv_ExpDefaultWinHandle, "Null");
                
                    txtTrayBarcode_1.Text = Tray.Barcode_1 = "Null";
                    Vision.BarcodeResult_1 = "NG";
                    if (Labels_1.Count != (Tray.Rows_1 * Tray.Columns_1))
                    {
                        ReleaseLabels_1();
                        GenerateLabels_1();
                    }
                    else
                    {
                        InitialLabels_1();
                    }
                    Vision.Images_1.Clear();
                    Protocol.bPCOK = true;
                    Tray.OpDateTime = DateTime.Now;
                
            }
            HOperatorSet.ClearDataCode2dModel(hv_DataCodeHandle);
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string TypeIndex = cbType.SelectedIndex.ToString();
            switch (cbType.SelectedIndex)
            {
                case 0: Sys.Type = "Semi-finished"; break;
                case 1: Sys.Type = "finished"; break;
            }
            IniFile.Write("Addition", "Type", TypeIndex, Sys.SysPath);
        }

        private void cbCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //补充Codes和产品类型
            string CodesIndex = cbCode.SelectedIndex.ToString();
            switch (cbCode.SelectedIndex)
            {
                case 0: Sys.Codes = "M"; break;
                case 1: Sys.Codes = "T"; break;
                case 2: Sys.Codes = "F"; break;
                case 3: Sys.Codes = "C"; break;
                case 4: Sys.Codes = "M"; break;
                case 5: Sys.Codes = "K"; break;
                case 6: Sys.Codes = "Q"; break;
                case 7: Sys.Codes = "H"; break;
                case 8: Sys.Codes = "E"; break;
                case 9: Sys.Codes = "R"; break;
                case 10: Sys.Codes = ""; break;
                case 11: Sys.Codes = "S"; break;
                case 12: Sys.Codes = "N"; break;
                case 13: Sys.Codes = "Other"; break;
            }
            if (Sys.Function == 5 && Sys.Codes != "M")
            {
                MessageBox.Show("進出站管控已關閉!");
            }
            else if ((Sys.Function == 5 && Sys.Codes == "M"))
            {
                MessageBox.Show("進出站管控已開啟!");
            }
            IniFile.Write("Addition", "Codes", CodesIndex, Sys.SysPath);
        }

        private void DataBankReconnect_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(3000);
            while (!DataBankReconnect.CancellationPending)
            {
                if (!DataBank.IsConnected)
                {
                    //连接  加@是為了讓\是正常的\
                    try
                    {
                        CallWithTimeout(DataBase, 20000);
                    }
                    catch
                    {
                        DataBank.IsConnected = false;
                    }

                    if (status)
                        DataBank.IsConnected = true;
                }
                Thread.Sleep(1);
            }
        }
        

        private void cbReadLensBarcode_CheckedChanged(object sender, EventArgs e)
        {
            My.NIR.ReadBarrelBarcode = (cbReadBarrelBarcode.Checked ? true : false);
            string Path = Sys.IniPath + "\\" + Sys.Function + "_" + Production.CurProduction + "_SetReport.ini";

            IniFile.Write("Setting", "ReadBarrelBarcode", My.NIR.ReadBarrelBarcode.ToString(), Path);
        }

        void DataBase_WS_End()
        {
            string TrayBarcode = "";
            string sMsg = "";
                TrayBarcode = Tray.Barcode_1;
           
           
                for (int i = 0; i < Tray.Rows_1; i++)
                {
                    for (int j = 0; j < Tray.Columns_1; j++)
                    {
                        if (WebService.ArrResult[i][j] == null)
                        {
                            WebService.ArrResult[i][j] = "NA";
                        }
                    }
                }
            
            if (parent.lblFactory.Text == "XM")
            {
                WS_Eqp_NIR_XM.Eqp_NIR Eqp_NIR_XM = new WS_Eqp_NIR_XM.Eqp_NIR();
                WebService.NIRResult_End = Eqp_NIR_XM.DoFinish(TrayBarcode, Sys.MachineID, WebService.ArrResult, DateTime.Now, out WebService.NIRMsg_End);
            }
            else if (parent.lblFactory.Text == "JM")
            {
                WS_Eqp_NIR_JM.Eqp_NIR Eqp_NIR_JM = new WS_Eqp_NIR_JM.Eqp_NIR();
                WebService.NIRResult_End = Eqp_NIR_JM.DoFinish(TrayBarcode, Sys.MachineID, WebService.ArrResult, DateTime.Now, out WebService.NIRMsg_End);
            }
        }

        private void hWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            //parent.hWindowControl1_HMouseDown(hWindowControl1.HalconWindow);
        }

        private void hWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {
            //parent.hWindowControl1_HMouseUp(hWindowControl1.HalconWindow);
        }

        private void hWindowControl1_HMouseWheel(object sender, HMouseEventArgs e)
        {
            //parent.hWindowControl1_HMouseWheel(hWindowControl1.HalconWindow, e);
        }

        private void btnShowOriginalImage_Click(object sender, EventArgs e)
        {

        }

        private void btnTestClear_Click(object sender, EventArgs e)
        {
            parent.WriteCountLog();
            Count.iTest = 0;
            Count.iTestTotal = 0;
        }
        #region 文本框限制
        private void txtTray1Cave_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextNumber(sender, e);
        }

        private void txtTray1Mold_KeyPress(object sender, KeyPressEventArgs e)
        {
            Textletter(sender, e);
        }
        private void txtTray2Cave_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextNumber(sender, e);
        }

        private void txtTray2Mold_KeyPress(object sender, KeyPressEventArgs e)
        {
            Textletter(sender, e);
        }

         public void TextNumber(object sender, KeyPressEventArgs e) //文本框格式设定
         {
             if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8)
                 e.Handled = true;

         }
         public void Textletter(object sender, KeyPressEventArgs e) //文本框格式设定
         {

             if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || e.KeyChar == 8)
             {
                 e.Handled = false;
             }
             else
             {
                 if ((e.KeyChar >= 'a' && e.KeyChar <= 'z') || e.KeyChar == 8)
                 {
                     e.Handled = false;
                 }
                 else e.Handled = true;

             }

         }
        #endregion

         private void txtClass_TextChanged(object sender, EventArgs e)
         {
             string s = this.txtClass.Text;
             foreach (char c in s)
             {
                 int i = (int)c;
                 if (i > 0x4e00 && i < 0x9fa5)
                 {
                     txtClass.Text = "";
                     MessageBox.Show("禁止輸入漢字");
                 }
             }
         }

         private void txtJobNumber_TextChanged(object sender, EventArgs e)
         {
             string s = this.txtClass.Text;
             foreach (char c in s)
             {
                 int i = (int)c;
                 if (i > 0x4e00 && i < 0x9fa5)
                 {
                     txtClass.Text = "";
                     MessageBox.Show("禁止輸入漢字");
                 }
             }
         }


    }

   
}

