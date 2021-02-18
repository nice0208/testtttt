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
    public partial class FrmLogIn : Form
    {
        FrmParent parent;
        public FrmLogIn(FrmParent parent)
        {
            this.parent = parent;
            this.MdiParent = parent;
            //  this.Dock = DockStyle.Top;
            InitializeComponent();
        }

        private void FrmLogIn_Shown(object sender, EventArgs e)
        {
            txtPassword.Focus();
        }
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("请输入密码");
                return;
            }
            if (cmbUsers.SelectedIndex < 0)
            {
                return;
            }
            if (User.Total[(string)cmbUsers.SelectedItem] == txtPassword.Text)
            {
                MessageBox.Show("登录成功");
                btnLogIn.Enabled = false;
                User.CurrentUser = (string)cmbUsers.SelectedItem;
                parent.lblUser.Text = User.CurrentUser;
                switch (FrmParent.flag)
                {
                    case 1: parent.btnToFrmSetUp.PerformClick(); break;
                    case 2: parent.btnToFrmVisionSet.PerformClick(); break;
                    
                }
                FrmParent.flag = 0;
                this.Close();
            }
            else
            {
                MessageBox.Show("密码错误,请重新输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
            }
            txtPassword.Clear();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            FrmParent.flag = 0;
            //this.Close();
            //Run = new FrmRun(this);
            //Run.Show();
            parent.btnToFrmRun.PerformClick();

        }

        private void FrmLogIn_Load(object sender, EventArgs e)
        {
            string path = Sys.IniPath + "\\Users.ini";
            string totalUsers = IniFile.Read("Users", "Total", "", path);
            User.Total.Clear();
            cmbUsers.Items.Clear();
            string[] Users = totalUsers.Split(',');
            if (Users.Length > 0)
            {
                string[] PassWord = new string[Users.Length];
                for (int i = 0; i < Users.Length; ++i)
                {
                    PassWord[i] = IniFile.Read(Users[i], "PassWord", "", path);
                    User.Total.Add(Users[i], PassWord[i]);
                }
                cmbUsers.Items.AddRange(Users);
                cmbUsers.SelectedIndex = 0;
            }
        }

        private void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogIn.PerformClick();
            }
        }

        private void txtPassword_MouseDown(object sender, MouseEventArgs e)
        {
            FrmNumeric numric = new FrmNumeric(this, txtPassword);
            numric.ShowDialog();
        }
    }
}
