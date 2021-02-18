using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Detecting_System
{
    public partial class FrmNumeric : Form
    {
        TextBox txt;
        string buff = "";
        decimal buff2 = 0;
        public FrmNumeric(Form child, TextBox txt)
        {
            Point p = new Point(500, 500);
            this.txt = txt;
            buff = txt.Text;
            //txt.Clear();
            this.Location = p;
            InitializeComponent();
        }

        private void FrmNumeric_Load(object sender, EventArgs e)
        {
            foreach (Control ctr in this.Controls)
            {
                if (ctr.Name.Contains("btn"))
                {
                    ctr.Click += BtnClick;
                }
            }
        }
        void BtnClick(object sender, EventArgs e)
        {
            char input = 'A';
            if (sender == btnZero) input = '0';
            else if (sender == btnOne) input = '1';
            else if (sender == btnTwo) input = '2';
            else if (sender == btnThree) input = '3';
            else if (sender == btnFour) input = '4';
            else if (sender == btnFive) input = '5';
            else if (sender == btnSix) input = '6';
            else if (sender == btnSeven) input = '7';
            else if (sender == btnEight) input = '8';
            else if (sender == btnNine) input = '9';
            else if (sender == btnPoint) input = '.';
            else if (sender == btnNeg) input = '-';
            else if (sender == btnBackSpace) input = 'B';
            else if (sender == btnClear) input = 'C';
            else if (sender == btnEnter) input = 'E';

            if (input == 'B')
            {
                if (txt.Text.Length <= 0)
                    return;
                else
                {
                    txt.Text = txt.Text.Substring(0, txt.Text.Length - 1);
                    return;
                }
            }
            else if (input == 'C')
            {
                txt.Clear();
                buff = "";
                return;
            }
            else if (input == 'E')
            {
                this.Close();
                return;
            }
            else
            {
                if (input != 'A')
                    txt.AppendText(input.ToString());
            }
        }

        private void FrmNumeric_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (txt.Text == "")
                txt.Text = buff;
        }

    }
}
