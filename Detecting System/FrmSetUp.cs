using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace Detecting_System
{
    public partial class FrmSetUp : Form
    {
        FrmParent parent;

        public FrmSetUp(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            this.Dock = DockStyle.Top;

            InitializeComponent();
        }

        private void FrmSetUp_Load(object sender, EventArgs e)
        {
            LoadPara();
            cbFunctionChoice.SelectedIndex = Sys.Function;//功能
            cbFactoryChoice.SelectedIndex = Sys.Factory;//廠區
            txtMachineID.Text = Sys.MachineID;//MachineID
            cbOriginal.Checked = Convert.ToBoolean(Sys.OptionOriginal);
            cbOK.Checked = Convert.ToBoolean(Sys.OptionOK);
            cbNG.Checked = Convert.ToBoolean(Sys.OptionNG);
            cbTrayMessage.Checked = Convert.ToBoolean(Sys.TrayMessage);
            cbLastResult.Checked = Convert.ToBoolean(Sys.LastResult);
            cbManualClear.Checked = Convert.ToBoolean(Sys.ManualClear);
            //Production參數
            for (int i = 0; i < Production.TotalProductionCount ; ++i )
            {
                string name = IniFile.Read("Total Production", "Production" + i, "NULL", Production.ProductionPath);
                cmbProduction.Items.Add(name);
            }
            cmbProduction.Text = IniFile.Read("Current Production", "Current Production", "NULL", Production.ProductionPath);
            cbReadBarcodeLog.Checked = Convert.ToBoolean(Sys.ReadBarcodeLog);
            cbVisitWebService.Checked = Convert.ToBoolean(Sys.VisitWebService);
            cmbCCDChoice.SelectedIndex = CCD.CCDBrand;

            cbThrow_OK.Checked = Sys.bThrow_OK;
            cbThrow_NG.Checked = Sys.bThrow_NG;
            cbThrow_NG2.Checked = Sys.bThrow_NG2;
            cbThrow_NG3.Checked = Sys.bThrow_NG3;
            cbThrow_NG4.Checked = Sys.bThrow_NG4;
            cbThrow_NG5.Checked = Sys.bThrow_NG5;
            cbThrow_Miss.Checked = Sys.bThrow_Miss;
        }

        private void btnMachineIDSave_Click(object sender, EventArgs e)
        {
            parent.lblMachineID.Text = Sys.MachineID = txtMachineID.Text;

            IniFile.Write("System", "MachineID", Sys.MachineID, Sys.SysPath);
        }

        private void btnProductionAdd_Click(object sender, EventArgs e)
        {
            string name = (string)cmbProduction.Text.Trim();
            //if (cmbProduction.SelectedIndex < 0)
            //    return;
            
            if (name == "")
            {
                MessageBox.Show("机种名称不能为空!");
                cmbProduction.Focus();
                return;
            }
            if (cmbProduction.Items.Contains(name))
            {
                MessageBox.Show("机种名称重复!");
                cmbProduction.Focus();
                return;
            }
            try
            {
                cmbProduction.Items.Add(name);
                //全部共有幾種機種
                IniFile.Write("Total Production", "Total Count", cmbProduction.Items.Count.ToString(), Production.ProductionPath);
                Production.TotalProductionCount = cmbProduction.Items.Count;
                //將所有機種寫入ini
                for (int i = 0; i < cmbProduction.Items.Count; ++i)
                {
                    IniFile.Write("Total Production", "Production" + i.ToString(), (string)cmbProduction.Items[i], Production.ProductionPath);
                }
                //正在使用的機種寫入ini
                IniFile.Write("Current Production", "Current Production", name, Production.ProductionPath);
                parent.lblProduction.Text = Production.CurProduction = (string)cmbProduction.SelectedItem;
                MessageBox.Show("添加成功");
            }
            catch
            {
            }
        }

        private void cmbProduction_SelectedIndexChanged(object sender, EventArgs e)
        {
            parent.lblProduction.Text = Production.CurProduction = (string)cmbProduction.SelectedItem;
            IniFile.Write("Current Production", "Current Production", cmbProduction.SelectedItem.ToString(), Production.ProductionPath);
        }

        private void btnProductionDelect_Click(object sender, EventArgs e)
        {
            try
            {
                //获取cmbProduction所选项的索引
                int selectIndex = cmbProduction.SelectedIndex;
                //移除所选项
                cmbProduction.Items.RemoveAt(selectIndex);

                string name = (string)cmbProduction.Text.Trim();
                //全部共有幾種機種
                IniFile.Write("Total Production", "Total Count", cmbProduction.Items.Count.ToString(), Production.ProductionPath);
                Production.TotalProductionCount = cmbProduction.Items.Count;
                //將所有機種寫入ini
                for (int i = 0; i < cmbProduction.Items.Count; ++i)
                {
                    IniFile.Write("Total Production", "Production" + i.ToString(), (string)cmbProduction.Items[i], Production.ProductionPath);
                }
                MessageBox.Show("刪除成功,請選擇機種");
            }
            catch
            {
            }
            ////正在使用的機種寫入ini
            //IniFile.Write("Current Production", "Current Production", name, Production.ProductionPath);
            //Production.CurProduction = (string)cmbProduction.SelectedItem;
        }

        private void btnImageSave_Click(object sender, EventArgs e)
        {
            Sys.OptionOriginal = (cbOriginal.Checked ? true : false);
            IniFile.Write("System", "OriginalSave", Sys.OptionOriginal.ToString(), Sys.SysPath);
            Sys.OptionOK = (cbOK.Checked ? true : false);
            IniFile.Write("System", "OkSave", Sys.OptionOK.ToString(), Sys.SysPath);
            Sys.OptionNG = (cbNG.Checked ? true : false);
            IniFile.Write("System", "NgSave", Sys.OptionNG.ToString(), Sys.SysPath);
            MessageBox.Show("保存OK!");
        }

        private void btnFactoryChoiceSave_Click(object sender, EventArgs e)
        {
            if (cbFactoryChoice.SelectedIndex < 0)
                return;
            string result = parent.lblFactory.Text = (string)cbFactoryChoice.SelectedItem;
            int index = 0;
            switch (result)
            {
                case "XM": index = 1; break;
                case "JM": index = 2; break;
                case "測試": index = 3; break;
            }
            cbFactoryChoice.SelectedIndex = index;
            Sys.Factory = index;
            IniFile.Write("Addition", "FactoryChoice", result, Sys.SysPath);
            MessageBox.Show("保存OK!");
        }
     
        private void btnLigSave_Click(object sender, EventArgs e)
        {
            if (cmbLigPort.SelectedIndex >= 0)
            {
                IniFile.Write("Light", "Port", (string)cmbLigPort.SelectedItem, Sys.SysPath);
            }
            if (cmbLigBaudrate.SelectedIndex >= 0)
            {
                IniFile.Write("Light", "Baudrate", (string)cmbLigBaudrate.SelectedItem, Sys.SysPath);
            }
            if (cmbLigDataBit.SelectedIndex >= 0)
            {
                IniFile.Write("Light", "DataBit", (string)cmbLigDataBit.SelectedItem, Sys.SysPath);
            }
            if (cmbLigParity.SelectedIndex >= 0)
            {
                IniFile.Write("Light", "Parity", (string)cmbLigParity.SelectedItem, Sys.SysPath);
            }
            if (cmbLigStopBit.SelectedIndex >= 0)
            {
                IniFile.Write("Light", "StopBit", (string)cmbLigStopBit.SelectedItem, Sys.SysPath);
            }
            MessageBox.Show("保存OK");
        }

        public void LoadPara()
        {
            try
            {
                txtCCDIp.Text = CCD.ip.ToString();
                txtCCDPort.Text = CCD.Port.ToString();

                txtPlcIp.Text = Plc.ip.ToString();
                txtPlcPort.Text = Plc.Port.ToString();

                //Light
                cmbLigPort.Text = Light.Com.PortName;
                cmbLigBaudrate.Text = Light.Com.BaudRate.ToString();
                cmbLigDataBit.Text = Light.Com.DataBits.ToString();
                cmbLigParity.Text = Light.Com.Parity.ToString();
                cmbLigStopBit.Text = Light.Com.StopBits.ToString();

                //Reader 
                cmbReaderPort.Text = Reader.Com.PortName;
                cmbReaderBaudrate.Text = Reader.Com.BaudRate.ToString();
                cmbReaderDataBit.Text = Reader.Com.DataBits.ToString();
                cmbReaderParity.Text = Reader.Com.Parity.ToString();
                cmbReaderStopBit.Text = Reader.Com.StopBits.ToString();
            }
            catch
            {
            }
        }

        private void btnCCDSave_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Any;
            int port = 0;
            if (!IPAddress.TryParse(txtCCDIp.Text, out ip))
            {
                MessageBox.Show("Ip地址格式不正确!");
                txtCCDIp.Focus();
                return;
            }
            if (!int.TryParse(txtCCDPort.Text, out port) && port > 0)
            {
                MessageBox.Show("端口设置不正确!");
                txtCCDPort.Focus();
                return;
            }
            IniFile.Write("CCD", "Ip", txtCCDIp.Text, Sys.SysPath);
            IniFile.Write("CCD", "Port", txtCCDPort.Text, Sys.SysPath);
            CCD.ip = ip;
            CCD.Port = port;
            MessageBox.Show("保存OK");
        }

        private void btnPlcSave_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Any;
            int port = 0;
            if (!IPAddress.TryParse(txtPlcIp.Text, out ip))
            {
                MessageBox.Show("Ip地址格式不正确!");
                txtPlcIp.Focus();
                return;
            }
            if (!int.TryParse(txtPlcPort.Text, out port) && port > 0)
            {
                MessageBox.Show("端口设置不正确!");
                txtPlcPort.Focus();
                return;
            }
            IniFile.Write("PLC", "Ip", txtPlcIp.Text, Sys.SysPath);
            IniFile.Write("PLC", "Port", txtPlcPort.Text, Sys.SysPath);
            Plc.ip = ip;
            Plc.Port = port;
            MessageBox.Show("保存OK");
        }

        private void btnFunctionChoiceSave_Click(object sender, EventArgs e)
        {
            if (cbFunctionChoice.SelectedIndex < 0)
                return;
            string result = parent.lblFunction.Text = (string)cbFunctionChoice.SelectedItem;
            int index = 0;
            switch (result)
            {
                case "VDI識別": index = 0; Sys.FunctionString = "VDI"; break;
                case "點膠識別": index = 1; Sys.FunctionString = "Dispensing"; break;
                case "Soma偏識別": index = 2; Sys.FunctionString = "SOMA"; break;
                case "Soma正反缺陷識別": index = 3; Sys.FunctionString = "SOMA Defect"; break;
                case "VDI鍍膜識別": index = 4; Sys.FunctionString = "VDI Coating"; break;
                case "NIR識別": index = 5; Sys.FunctionString = "NIR"; break;
                case "塗墨識別": index = 6; Sys.FunctionString = "Inkiness"; break;
                case "分類方向識別": index = 7; Sys.FunctionString = "Classifier"; break;
                case "BarcodeReader": index = 8; Sys.FunctionString = "BarcodeReader"; break;
                case "BarcodeReaderPlus": index = 9; Sys.FunctionString = "BarcodeReaderPlus"; break;
                case "鏡裂檢測": index = 10; Sys.FunctionString = "LensCrack_AVI"; break;
                case "模别模穴识别": index = 11; Sys.FunctionString = "Lens_Mold_Cave"; break;
                case "鏡片搬運": index = 12; Sys.FunctionString = "LensCarry"; break;
            }
            cbFunctionChoice.SelectedIndex = index;
            Sys.Function = index;
            IniFile.Write("Addition", "Function", result, Sys.SysPath);
            MessageBox.Show("保存OK!");
        }
        private void btnTrayMessageSave_Click(object sender, EventArgs e)
        {
            Sys.TrayMessage = (cbTrayMessage.Checked ? true : false);
            IniFile.Write("System", "TrayMessage", Sys.TrayMessage.ToString(), Sys.SysPath);
        }

        private void btnLastResultSave_Click(object sender, EventArgs e)
        {
            Sys.LastResult = (cbLastResult.Checked ? true : false);
            IniFile.Write("System", "LastResult", Sys.LastResult.ToString(), Sys.SysPath);
        }

        private void btnReaderSave_Click(object sender, EventArgs e)
        {
            if (cmbReaderPort.SelectedIndex >= 0)
            {
                IniFile.Write("Reader", "Port", (string)cmbReaderPort.SelectedItem, Sys.SysPath);
            }
            if (cmbReaderBaudrate.SelectedIndex >= 0)
            {
                IniFile.Write("Reader", "Baudrate", (string)cmbReaderBaudrate.SelectedItem, Sys.SysPath);
            }
            if (cmbReaderDataBit.SelectedIndex >= 0)
            {
                IniFile.Write("Reader", "DataBit", (string)cmbReaderDataBit.SelectedItem, Sys.SysPath);
            }
            if (cmbReaderParity.SelectedIndex >= 0)
            {
                IniFile.Write("Reader", "Parity", (string)cmbReaderParity.SelectedItem, Sys.SysPath);
            }
            if (cmbReaderStopBit.SelectedIndex >= 0)
            {
                IniFile.Write("Reader", "StopBit", (string)cmbReaderStopBit.SelectedItem, Sys.SysPath);
            }
            MessageBox.Show("保存OK");
        }

        private void btnReadBarcodeLogSave_Click(object sender, EventArgs e)
        {
            Sys.ReadBarcodeLog = (cbReadBarcodeLog.Checked ? true : false);
            IniFile.Write("System", "ReadBarcodeLog", Sys.ReadBarcodeLog.ToString(), Sys.SysPath);
        }

        private void btnVisitWebServiceSave_Click(object sender, EventArgs e)
        {
            Sys.VisitWebService = (cbVisitWebService.Checked ? true : false);
            IniFile.Write("System", "VisitWebService", Sys.VisitWebService.ToString(), Sys.SysPath);
        }

        private void btnManualClearSave_Click(object sender, EventArgs e)
        {
            Sys.ManualClear = (cbManualClear.Checked ? true : false);
            IniFile.Write("System", "ManualClear", Sys.ManualClear.ToString(), Sys.SysPath);
            
        }

        private void cmbCCDChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            CCD.CCDBrand = cmbCCDChoice.SelectedIndex;
            IniFile.Write("CCD", "CCDChoice", CCD.CCDBrand.ToString(), Sys.SysPath);
        }

        private void cbThrow_OK_CheckedChanged(object sender, EventArgs e)
        {
            Sys.bThrow_OK = cbThrow_OK.Checked;
            IniFile.Write("System", "Throw_OK", Sys.bThrow_OK.ToString(), Sys.SysPath);
        }

        private void cbThrow_NG_CheckedChanged(object sender, EventArgs e)
        {
            Sys.bThrow_NG = cbThrow_NG.Checked;
            IniFile.Write("System", "Throw_NG", Sys.bThrow_NG.ToString(), Sys.SysPath);
        }

        private void cbThrow_NG2_CheckedChanged(object sender, EventArgs e)
        {
            Sys.bThrow_NG2 = cbThrow_NG2.Checked;
            IniFile.Write("System", "Throw_NG2", Sys.bThrow_NG2.ToString(), Sys.SysPath);
        }

        private void cbThrow_NG3_CheckedChanged(object sender, EventArgs e)
        {
            Sys.bThrow_NG3 = cbThrow_NG3.Checked;
            IniFile.Write("System", "Throw_NG3", Sys.bThrow_NG3.ToString(), Sys.SysPath);
        }

        private void cbThrow_NG4_CheckedChanged(object sender, EventArgs e)
        {
            Sys.bThrow_NG4 = cbThrow_NG4.Checked;
            IniFile.Write("System", "Throw_NG4", Sys.bThrow_NG4.ToString(), Sys.SysPath);
        }

        private void cbThrow_NG5_CheckedChanged(object sender, EventArgs e)
        {
            Sys.bThrow_NG5 = cbThrow_NG5.Checked;
            IniFile.Write("System", "Throw_NG5", Sys.bThrow_NG5.ToString(), Sys.SysPath);
        }

        private void cbThrow_Miss_CheckedChanged(object sender, EventArgs e)
        {
            Sys.bThrow_Miss = cbThrow_Miss.Checked;
            IniFile.Write("System", "Throw_Miss", Sys.bThrow_Miss.ToString(), Sys.SysPath);
        }

    

       
    }
}
