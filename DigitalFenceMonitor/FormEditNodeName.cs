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
using System.Net.Sockets;
using System.Net;

namespace DigitalFenceMonitor
{
    public partial class FormEditNodeName : Form
    {
        public FormEditNodeName()
        {
            InitializeComponent();
        }
        OleDbConnection conn = FormMain.conn;

        int forswitch = FormMain.Num_Node_Vol_CMD;
        private void EditNodeName_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
 
            switch (forswitch)
            {
                case 0: NodeName(); break;
                case 1: AreaNum(); break;
                case 2: WorkVoltage(); break;
                default: MessageBox.Show(forswitch.ToString()); break;
            }

        }

        private void btn_quit_Click(object sender, EventArgs e)
        {
            FormMain.Num_Node_Vol_CMD = -1;
            this.Close();
        }

        private void NodeName()
        {
            this.Text = "change name";
            if (FormMain.CurrentNode != null)
            {
                this.comboBox_new.Visible = false;
                this.tBox_new.Visible = true;
                this.lb_old.Text = "old name";
                this.lb_new.Text = "new name";
                tBox_old.Text = FormMain.CurrentNode.Split(',')[2];
                tboxoldtext = FormMain.CurrentNode;
            }
            else
            {
                FormMain.Num_Node_Vol_CMD = -1;
                this.Close();
            }
        }
        public delegate void thisMapNN(string name);
        public event thisMapNN thisMapNodeName;
        public string tboxoldtext = "";
        private void NodeNameOK()
        {
            bool flag = true;
            if (conn.State == ConnectionState.Closed) conn.Open();
            string sql = "select Descripe from tb_AreaSet";
            sql += " where IPandPort = '" + tboxoldtext.Split(',')[0] + "'";
            sql += " and AreaNum = '" + tboxoldtext.Split(',')[1] + "'";
            DataTable tempdt = new DataTable();
            OleDbDataAdapter tempda = new OleDbDataAdapter(sql, conn);
            tempda.Fill(tempdt);
            for (int i = 0; i < tempdt.Rows.Count; i++)
            {
                string s = tempdt.Rows[i]["Descripe"].ToString();
                if (s == tBox_new.Text) flag = false;
            }
            if (flag) {
                sql = "select Map from tb_AreaSet";
                sql += " where IPandPort = '" + tboxoldtext.Split(',')[0] + "'";
                sql += " and AreaNum = '" + tboxoldtext.Split(',')[1] + "'";
                sql += " and Descripe = '" + tboxoldtext.Split(',')[2] + "'";
                tempdt = new DataTable();
                tempda = new OleDbDataAdapter(sql, conn);
                tempda.Fill(tempdt);
                string mapTemp = tempdt.Rows[0]["Map"].ToString();

                string descripe = tBox_new.Text.Trim();
                sql = " update tb_AreaSet set Descripe ='" + descripe;
                sql += "' Where Descripe = '" + tboxoldtext.Split(',')[2] + "'";
                sql += " and IPandPort = '" + tboxoldtext.Split(',')[0] + "'";
                sql += " and AreaNum = '" + tboxoldtext.Split(',')[1] + "'";
                OleDbCommand comm = new OleDbCommand(sql, conn);
                comm.ExecuteNonQuery();

                sql = " update tb_Map set MapInfo ='" +
                    FormMain.CurrentNode.Split(',')[0] + "," +
                    FormMain.CurrentNode.Split(',')[1] + "," +
                    tBox_new.Text +
                    " 'Where MapInfo = '" + FormMain.CurrentNode + "'";
                comm = new OleDbCommand(sql, conn);
                comm.ExecuteNonQuery();

                FormMain.ContentBuff[mapTemp]=descripe;

                thisMapNodeName(tBox_new.Text);

                LoadTreeView();
                tBox_old.Text = tBox_new.Text;
                tBox_new.Text = "";
            }
            else
            {
                MessageBox.Show("name repeate","WARN");
            }
        }




        public delegate void SendData(byte[] data, EndPoint RemotePoint);
        public event SendData SendNumData;
        private void AreaNum()
        {
            this.comboBox_new.Visible = false;
            this.tBox_new.Visible = true;
            this.Text = "change address";
            this.lb_old.Text = "base station name";
            this.lb_new.Text = "host address";       
            for (int i = 0; i < FormMain.StaSetCount; i++)
                tBox_old.Items.Add(FormMain.StaBuff[FormMain.RemotePointArr[i].ToString()]);
        }
        private void AreaNumOK()
        {
            if (tBox_old.Text != "")
            {
                byte[] data = new byte[6];
                data[0] = 0xAA;
                data[1] = 0xAE;
                data[2] = 0xF8;
                data[3] = 0x06;
                data[4] = (byte)(Convert.ToInt32(tBox_new.Text) & 0x0FF);
                data[5] = 0x00;
                int cou = tBox_old.SelectedIndex;
                FormMain.SetAreaNum = true;
                SendNumData(data, FormMain.RemotePointArr[cou]);
            }
        }





        private void WorkVoltage()
        {
            this.Text = "Unified operating voltage";
            string[] buff = new string[7] { "lowVolt-0.6KV", "highVolt-1.6KV", "highVolt-2.6KV", "highVolt-3.6KV", "highVolt-4.6KV", "highVolt-5.6KV", "highVolt-6.6KV" };
            this.comboBox_new.Visible = true;
            this.tBox_new.Visible = false;

            this.lb_old.Text = "base station name";
            this.lb_new.Text = "new voltage";

            for (int i = 0; i < FormMain.StaSetCount; i++)
                tBox_old.Items.Add(FormMain.StaBuff[FormMain.RemotePointArr[i].ToString()]);
            for(int i=0;i<buff.Length;i++)
                comboBox_new.Items.Add(buff[i]);
        }
        private void WorkVoltageOK()
        {
            byte[] data = new byte[6];
            data[0] = 0xAA;
            data[1] = 0xAD;
            data[2] = 0xF8;
            data[3] = 0x06;
            data[4] = (byte)(comboBox_new.SelectedIndex);
            tBox_new.Text = data[4].ToString();
            data[5] = 0x00;
            int cou = tBox_old.SelectedIndex;
            SendNumData(data, FormMain.RemotePointArr[cou]);
        }




        public delegate void Load_TreeView();
        public event Load_TreeView LoadTreeView;

        private void btn_ok_Click(object sender, EventArgs e)
        {
            switch (forswitch)
            {
                case 0: NodeNameOK(); break;
                case 1: AreaNumOK(); break;
                case 2: WorkVoltageOK(); break;
                default: MessageBox.Show(forswitch.ToString()); break;
            }
            FormMain.Num_Node_Vol_CMD = -1;
            this.Close();
        }
    }
}
