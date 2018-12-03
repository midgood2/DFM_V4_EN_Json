﻿using System;
using System.Data;
using System.Windows.Forms;
using System.Net;
using System.Collections.Generic;

namespace DigitalFenceMonitor
{
    public partial class FormStaSet : Form
    {
        DataTable datatable = null;
        BindingSource bindingsrc = null;
        FormMain fm = new FormMain();

        int DataLength;
        public FormStaSet()
        {
            InitializeComponent();
            this.ControlBox = false;
        }
        
        private void FormStaSet_Load(object sender, EventArgs e)
        {
            if (FormMain.NowUser == "admin")
            {
                but_Del.Enabled = true;
                btn_DelAll.Enabled = true;
                btn_Add.Enabled = true;
            }
            datatable =  JsonHelper.ToDataTable(FormMain.DataModel.StationSet);
            bindingsrc = new BindingSource();
            dataGridView.DataSource = datatable;
            bindingsrc.DataSource = datatable;

            DataLength = FormMain.DataModel.StationSet.Count;
            label_all.Text = DataLength.ToString();

            update_Data("init");

            Console.WriteLine(dataGridView.Columns);

            dataGridView.Columns["IP"].HeaderText = "Base station IP";
            dataGridView.Columns["StaName"].HeaderText = "Base station name";
            dataGridView.Columns["Port"].HeaderText = "Port number";
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public delegate void GetDBData();
        public event GetDBData GetDatabaseData;

        public delegate void Load_TreeView();
        public event Load_TreeView LoadTreeView;

        private void myGetDBData()
        {
            GetDatabaseData();
        }
        private void myLoadTreeView()
        {
            LoadTreeView();
        }
        private void btn_Add_Click(object sender, EventArgs e)
        {
            string str1 = textBox_IP.Text;
            string str2 = textBox_SatName.Text;
            string str3 = textBox_Port.Text;
            if (str1 == "" || str2 == "" || str3 == "" ) {
                MessageBox.Show("Please complete the filling", "WARN");
            }
            else
            {
                if (fm.getValidIP(str1) !=null && fm.getValidPort(str3) !=-1) { 
                    string insertstr = "insert into tb_StaSet(IP,StaName,Port,IPandPort) values('";
                    insertstr += str1 + "', '";
                    insertstr += str2 + "', '";
                    insertstr += str3 + "', '";
                    insertstr += str1 + ":" + str3 + "')";

                    cmsStationSet cmsSS = new cmsStationSet();
                    cmsSS.IP = str1;
                    cmsSS.StaName = str2;
                    cmsSS.Port = str3;
                    cmsSS.IPandPort = str1 + ":" + str3;
                    FormMain.DataModel.StationSet.Add(cmsSS);

                    DataLength++;
                    label_all.Text = DataLength.ToString();

                    update_Data(insertstr);
                    GetDatabaseData();
                }
                else
                {
                    if (fm.getValidIP(str1) == null && fm.getValidPort(str3) != -1)
                    { MessageBox.Show("  IP  format error", "WARN"); }
                    else if (fm.getValidIP(str1) != null && fm.getValidPort(str3) == -1)
                    { MessageBox.Show("  Port  format error", "WARN"); }
                    else
                    { MessageBox.Show("IP and Port formats error", "WARN"); }
                }
            }

        }

        private void update_Data(string cmd)
        {
            if (cmd != "init") { 
                try
                {
                    // comm.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show(" IP : Port Repeat","WARN");
                }
            }

            updata_datagridView();
        }
        private void updata_datagridView()
        {
            DataTable Dt = (DataTable)dataGridView.DataSource;
            Dt.Rows.Clear();
            DataTable newDt = JsonHelper.ToDataTable(FormMain.DataModel.StationSet);
            dataGridView.DataSource = newDt;

            // dataGridView.Columns[0].Visible = false;
            // dataGridView.Columns[4].Visible = false;
        }
        public delegate void UpdateAA(string str);
        public event UpdateAA UpdateAlertArea;

        private void but_Del_Click(object sender, EventArgs e)
        {
            if (DataLength > 0 && int.Parse(label_select.Text)<DataLength)
            {
                int Selected = int.Parse(label_select.Text);
                string deleIP = dataGridView.Rows[Selected].Cells[1].Value.ToString();
                string delePort = dataGridView.Rows[Selected].Cells[2].Value.ToString();
                string delestr = deleIP + ":" + delePort;

                string mapstr = "delete from tb_Map where MapInfo like '"+"%"+ delestr + "%"+"'";
                UpdateAlertArea("dele");
                foreach (cmsMap cmsM in FormMain.DataModel.Map)
                {
                    if (JsonHelper.IsRexMatched(cmsM.MapInfo,".*"+delestr+".*"))
                    {
                        FormMain.DataModel.Map.Remove(cmsM);
                    }
                }

                string deletestr = "delete from tb_StaSet where IPandPort = '" + delestr + "'";
                List<cmsStationSet> RemoveListSS = new List<cmsStationSet>();
                foreach (cmsStationSet cmsSS in FormMain.DataModel.StationSet)
                {
                    if ( cmsSS.IPandPort == delestr )
                    {
                        RemoveListSS.Add(cmsSS);
                    }
                }

                foreach (cmsStationSet rmSS in RemoveListSS)
                {
                    FormMain.DataModel.StationSet.Remove(rmSS);
                }

                DataLength--;
                label_all.Text = DataLength.ToString();
                update_Data(deletestr);

                deletestr = "delete from tb_AreaSet where IPandPort = '" + delestr + "'";
                List<Object> RemoveList = new List<Object>();
                foreach (cmsAreaSet cmsAS in FormMain.DataModel.AreaSet)
                {
                    if ( cmsAS.IPport == delestr )
                    {
                        RemoveList.Add(cmsAS);
                    }
                }
                foreach (object rmAS in RemoveList )
                {
                    FormMain.DataModel.AreaSet.Remove((cmsAreaSet)rmAS);
                }
                update_Data(deletestr);
                LoadTreeView();
            }

        }



        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                int rows = dataGridView.CurrentRow.Index;
                label_select.Text = rows.ToString();
            }
            catch { Console.WriteLine("no data"); }
        }

        private void btn_DelAll_Click(object sender, EventArgs e)
        {
            if (DataLength != 0)
            {
                string mapstr = "delete from tb_Map";
                UpdateAlertArea("dele");

                string deleall = "delete from tb_StaSet";

                DataLength = 0;
                label_all.Text = DataLength.ToString();

                update_Data(deleall);

                deleall = "delete from tb_AreaSet ";
                update_Data(deleall);

                EndPoint[] temp = new EndPoint[1000];
                FormMain.RemotePointArr = temp;
                FormMain.RemotePointArrCount = 0;
                LoadTreeView();

            }
        }

        static public string[] forChange = new string[3];
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Selected = int.Parse(label_select.Text);
            forChange[0] = dataGridView.Rows[Selected].Cells[1].Value.ToString();
            forChange[1] = dataGridView.Rows[Selected].Cells[2].Value.ToString();
            forChange[2] = dataGridView.Rows[Selected].Cells[3].Value.ToString();

            FormStaChange changedatabase = new FormStaChange();
            changedatabase.StartPosition = FormStartPosition.CenterScreen;
            changedatabase.update_datagridView += updata_datagridView;
            changedatabase.GetDatabaseData += myGetDBData;
            changedatabase.LoadTreeView += myLoadTreeView;
            changedatabase.ShowDialog();
        }
    }
}
