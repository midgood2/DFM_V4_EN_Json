using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace DigitalFenceMonitor
{
    public partial class FormLogin : Form
    {
        string[] username = new string[100];
        string[] password = new string[100];
        int pswindex = -1;
        int numberofaccount = 0;
        public FormLogin()
        {
            InitializeComponent();
            this.ControlBox = false;
            
        }
        private void but_login_Click(object sender, EventArgs e)
        {
            bool IsChecked = false;
            if (-1 == pswindex)
            {
                for (int n = 0; n < numberofaccount; n++)
                {
                    if (comboBox_login_id.Text == username[n])
                    {
                        pswindex = n;
                        IsChecked = true;
                        break;
                    }
                }
                if (false == IsChecked)
                    MessageBox.Show("No such user");
            }
            else
                IsChecked = true;

            if (true == IsChecked)
                if (textBox_login_psw.Text == password[pswindex])
                {
                    FormMain.NowUser = comboBox_login_id.Text;
                    this.Owner.Show();
                    FormMain.reloadmap.Start();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("password error","WARN");
                }
            pswindex = -1;
        }
        private void but_exit_Click(object sender, EventArgs e)
        {
            this.Owner.Close();
        }
        private void login_Load(object sender, EventArgs e)
        {
            if (FormMain.DataModel.Account.Count == 0)
            {
                MessageBox.Show("Nothing!");
            }
            else
            {
                int RowsofTable = FormMain.DataModel.Account.Count;
                numberofaccount = RowsofTable;
                int n = 0;
                foreach ( cmsAccount  cmsAcc in FormMain.DataModel.Account )
                {
                    username[n] = cmsAcc.UserName;
                    password[n] = cmsAcc.PassWord;
                    this.comboBox_login_id.Items.Add(username[n]);
                }

                comboBox_login_id.SelectedIndex = 0;
            }
        }

        private void comboBox_login_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            pswindex = comboBox_login_id.Items.IndexOf(comboBox_login_id.Text);

        }
    }
}
