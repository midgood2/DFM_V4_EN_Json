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

    public partial class FormDenfenAreaSet : Form
    {
        DataTable datatable = null;
        BindingSource bindingsrc = null;

        static public Dictionary<string, string> StaBuff = new Dictionary<string, string>();

        static public int DataLength;
        public FormDenfenAreaSet()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        private void FormDenfenAreaSet_Load(object sender, EventArgs e)
        {
            if (FormMain.NowUser == "admin")
            {
                but_Del.Enabled = true;
                btn_DelAll.Enabled = true;
                btn_Add.Enabled = true;
            }

            string sql = "select * from tb_AreaSet";
            datatable = JsonHelper.ToDataTable(FormMain.DataModel.AreaSet);
            bindingsrc = new BindingSource();
            dataGridView.DataSource = datatable;
            bindingsrc.DataSource = datatable;
            DataLength = FormMain.DataModel.AreaSet.Count;

            StaBuff = FormMain.StaBuff;

            update_Data("init");

            dataGridView.Columns.Add("StaName", "Base station Name");
            dataGridView.Columns["StaName"].DisplayIndex = 2;
            for(int i = 0; i < DataLength; i++)
            {
                string s = dataGridView.Rows[i].Cells["IPport"].Value.ToString();
                dataGridView.Rows[i].Cells["StaName"].Value = StaBuff[s.Trim()];
            }

            dataGridView.Columns["IPport"].HeaderText = "Base station";
            dataGridView.Columns["AreaNum"].HeaderText = "Device address";
            dataGridView.Columns["Status"].HeaderText = "Zone status";
            dataGridView.Columns["Describe"].HeaderText = "Zone location";
            dataGridView.Columns["DeviceType"].HeaderText = "Device type";
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            FormMain.DataModel.Write(JsonHelper.SerializeObject(FormMain.DataModel));
            this.Close();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            FormDefenAreaAdd defenareaadd = new FormDefenAreaAdd();
            defenareaadd.StartPosition = FormStartPosition.CenterScreen;
            defenareaadd.UpdateDataBase += update_Data;
            defenareaadd.LoadTreeView += myLoadTreeView;
            defenareaadd.Show();
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
                    MessageBox.Show(" IP : Port Repeat", "WARN");
                    DataLength--;
                }
            }

            DataTable Dt = (DataTable)dataGridView.DataSource;
            Dt.Rows.Clear();
            DataTable newDt = JsonHelper.ToDataTable(FormMain.DataModel.AreaSet);
            dataGridView.DataSource = newDt;

            for (int i = 0; i < DataLength; i++)
            {
                string s = dataGridView.Rows[i].Cells["IPport"].Value.ToString();
                try
                {
                    dataGridView.Rows[i].Cells["StaName"].Value = StaBuff[s];
                }
                catch
                {
                    ;
                }
            }
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[5].Visible = false;

            dataGridView.Columns[1].Width = 150;
        }

        private void btn_DelAll_Click(object sender, EventArgs e)
        {
            if (DataLength != 0)
            {
                string mapstr = "delete from tb_Map";
                FormMain.DataModel.Map.Clear();
                UpdateAlertArea("dele");

                string deleall = "delete from tb_AreaSet";
                FormMain.DataModel.AreaSet.Clear();
                DataLength = 0;
                update_Data(deleall);
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
            catch
            {
                Console.WriteLine("no data");
            }
        }
        public delegate void UpdateAA(string str);
        public event UpdateAA UpdateAlertArea;
        private void but_Del_Click(object sender, EventArgs e)
        {
            if(label_select != null && label_select.Text!="")
            if (DataLength > 0 && int.Parse(label_select.Text) < DataLength )
            {
                int Selected = int.Parse(label_select.Text);
                string str = dataGridView.Rows[Selected].Cells["Map"].Value.ToString().Split('-')[1];

                string s1 = dataGridView.Rows[Selected].Cells["IPport"].Value.ToString();
                string s2 = dataGridView.Rows[Selected].Cells["AreaNum"].Value.ToString();
                string mapstr = "delete from tb_Map where MapInfo like '%" + s1 + "," + s2 + "," + "%'";
                List<cmsMap> RemoveListMap = new List<cmsMap>();
                foreach (cmsMap cmsM in FormMain.DataModel.Map)
                {
                    if (JsonHelper.IsRexMatched(cmsM.MapInfo, ".*" + s1 + "," + s2 + ".*"))
                    {
                        RemoveListMap.Add(cmsM);
                    }
                }
                foreach (cmsMap rmMap in RemoveListMap)
                {
                    FormMain.DataModel.Map.Remove(rmMap);
                }
                UpdateAlertArea("dele");

                if (str == "0")
                { 
                    string map = dataGridView.Rows[int.Parse(label_select.Text)].Cells["Map"].Value.ToString().Split('-')[0] + "-0";
                    string deletestr = "delete from tb_AreaSet where map = '" + map + "'";
                    List<cmsAreaSet> RemoveListAS = new List<cmsAreaSet>();
                    foreach (cmsAreaSet cmsAS in FormMain.DataModel.AreaSet)
                    {
                        if ( cmsAS.Map == map )
                        {
                                RemoveListAS.Add(cmsAS);
                        }
                    }
                    foreach (cmsAreaSet rmAS in RemoveListAS)
                    {
                        FormMain.DataModel.AreaSet.Remove(rmAS);
                    }

                    DataLength--;
                    update_Data(deletestr);
                }
                else
                {
                    string map = dataGridView.Rows[Selected].Cells["Map"].Value.ToString().Split('-')[0];
                    for (int j = 1; j < 3; j++)
                    {
                        string deletestr = "delete from tb_AreaSet where map = '" + map + "-" + j.ToString() + "'";
                        List<cmsAreaSet> RemoveListAS = new List<cmsAreaSet>();
                        foreach (cmsAreaSet cmsAS in FormMain.DataModel.AreaSet)
                        {
                            if (cmsAS.Map == map + "-" + j.ToString())
                            {
                                RemoveListAS.Add(cmsAS);
                            }
                        }
                        foreach (cmsAreaSet rmAS in RemoveListAS)
                        {
                            FormMain.DataModel.AreaSet.Remove(rmAS);
                        }
                        DataLength--;
                        update_Data(deletestr);
                    }
                    
                }
                
            }
            LoadTreeView();
        }

        private void dataGridView_Sorted(object sender, EventArgs e)
        {
            for (int i = 0; i < DataLength; i++)
            {
                string s = dataGridView.Rows[i].Cells["IPport"].Value.ToString();
                dataGridView.Rows[i].Cells["StaName"].Value = StaBuff[s];
            }
        }
        private void updata_datagridView()
        {
            DataTable Dt = (DataTable)dataGridView.DataSource;
            Dt.Rows.Clear();
            DataTable newDt = JsonHelper.ToDataTable(FormMain.DataModel.AreaSet);
            dataGridView.DataSource = newDt;

            for (int i = 0; i < DataLength; i++)
            {
                string s = dataGridView.Rows[i].Cells["IPport"].Value.ToString();
                try
                {
                    dataGridView.Rows[i].Cells["StaName"].Value = StaBuff[s];
                }
                catch
                {
                    ;
                }
            }
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[5].Visible = false;

            dataGridView.Columns[1].Width = 150;
        }

        static public string[] forChange = new string[4];
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Selected = int.Parse(label_select.Text);
            forChange[0] = dataGridView.Rows[Selected].Cells[1].Value.ToString();
            forChange[1] = dataGridView.Rows[Selected].Cells[2].Value.ToString();
            forChange[2] = dataGridView.Rows[Selected].Cells["Descripe"].Value.ToString();
            forChange[3] = dataGridView.Rows[Selected].Cells["DeviceType"].Value.ToString();

            FormDefenceAreaChange formdefencechange = new FormDefenceAreaChange();
            formdefencechange.StartPosition = FormStartPosition.CenterScreen;
            formdefencechange.GetDatabaseData += myGetDBData;
            formdefencechange.LoadTreeView += myLoadTreeView;
            formdefencechange.update_datagridView += updata_datagridView;
            formdefencechange.ShowDialog();
        }
    }
}
