using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace DigitalFenceMonitor
{
    public partial class FormUserSet : Form
    {
        DataTable datatable = null;
        BindingSource bindingsrc = null;
        FormMain fm = new FormMain();
        int DataLength;
        public FormUserSet()
        {
            InitializeComponent();
        }

        private void UserSet_Load(object sender, EventArgs e)
        {
            string sql = "select username,authorition from tb_Account";

            datatable = JsonHelper.ToDataTable(FormMain.DataModel.Account);
            bindingsrc = new BindingSource();
            dataGridView.DataSource = datatable;
            bindingsrc.DataSource = datatable;

            DataLength = FormMain.DataModel.Account.Count;
            update_Data("init");
            dataGridView.Columns["UserName"].HeaderText = "username";
            dataGridView.Columns["authorition"].HeaderText = "permissions";

            comboBox_auth.Items.Add("0-administrator's permission");
            comboBox_auth.Items.Add("1-common permission");
        }
        private void update_Data(string cmd)
        {
            if (cmd != "init")
            {
                try
                {
                    // comm.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Repeat", "WARN");
                }
            }
            DataTable Dt = (DataTable)dataGridView.DataSource;
            Dt.Rows.Clear();
            DataTable newDt = JsonHelper.ToDataTable(FormMain.DataModel.Account);
            dataGridView.DataSource = newDt;
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void but_Del_Click(object sender, EventArgs e)
        {
            if (DataLength > 0 && int.Parse(label_select.Text) < DataLength)
            {
                int Selected = int.Parse(label_select.Text);
                string username = dataGridView.Rows[Selected].Cells[0].Value.ToString();

                string deletestr = "delete * from tb_Account where username = '" + username + "'";
                List<cmsAccount> RemoveListAcc = new List<cmsAccount>();
                foreach (cmsAccount cmsAcc in FormMain.DataModel.Account)
                {
                    if (cmsAcc.UserName == username)
                    {
                        RemoveListAcc.Add(cmsAcc);
                    }
                }
                foreach (cmsAccount rmAcc in RemoveListAcc)
                {
                    FormMain.DataModel.Account.Remove(rmAcc);
                }
                update_Data(deletestr);
            }
        }

        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rows = dataGridView.CurrentRow.Index;
            label_select.Text = rows.ToString();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (textBox_UserName.Text != "" && textBox_UserPSW0.Text != "" && textBox_UserPSW1.Text != "" && comboBox_auth.Text != "")
            {
                if (textBox_UserPSW0.Text == textBox_UserPSW1.Text)
                {
                    string insertstr = "insert into tb_Account(username,psw,authorition) values('";
                    insertstr += textBox_UserName.Text + "', '";
                    insertstr += textBox_UserPSW0.Text + "', '";
                    insertstr += comboBox_auth.Text + "')";

                    cmsAccount addAcc = new cmsAccount();
                    addAcc.UserName = textBox_UserName.Text;
                    addAcc.PassWord = textBox_UserPSW0.Text;
                    addAcc.authorition = comboBox_auth.Text;
                    FormMain.DataModel.Account.Add(addAcc);

                    update_Data(insertstr);
                    DataLength++;
                }
                else
                {
                    MessageBox.Show("the password is inconsistent", "WARN");
                }
            }
            else
            {
                MessageBox.Show("Incomplete information", "WARN");
            }
        }
    }
}
