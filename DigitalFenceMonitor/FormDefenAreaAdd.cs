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
    public partial class FormDefenAreaAdd : Form
    {
        string[] data = new string[5];
        static public Dictionary<string, string> StaBuff = new Dictionary<string, string>();

        public delegate void UpdateDB(string msg);
        public event UpdateDB UpdateDataBase;

        public FormDefenAreaAdd()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            FormMain.DataModel.Write(JsonHelper.SerializeObject(FormMain.DataModel));
            this.Close();
        }

        public delegate void Load_TreeView();
        public event Load_TreeView LoadTreeView;

        private void btn_Done_Click(object sender, EventArgs e)
        {
            if (textBox_AreaNum.Text != "" && textBox_Position.Text != "")
            {
                try
                {
                    data[0] = StaBuff[comboBox_StaName.Text].Trim();
                }
                catch
                {
                    data[0] = "null";
                }
                data[1] = textBox_AreaNum.Text.Trim();
                data[2] = comboBox_AreaStatus.Text.Trim();
                data[3] = textBox_Position.Text.Trim();
                data[4] = comboBox_type.Text.Split('-')[1].Trim();
                bool flag = true;

                string sql = "select AreaNum from tb_AreaSet Where IPandPort ='" + data[0] + "'";
                List<string> areaNum = new List<string>();
                foreach ( cmsAreaSet cmsAS in FormMain.DataModel.AreaSet )
                {
                    if ( cmsAS.IPport == data[0] )
                    {
                        areaNum.Add(cmsAS.AreaNum);
                    }
                }

                for (int i = 0; i < areaNum.Count; i++)
                {
                    string temp = areaNum[i];
                    if (temp == data[1])
                    {
                        flag = false;
                        break;
                    }
                }

                if(flag)
                { 
                    char CheckState = (comboBox_type.Text.Split('-')[0] == "SingleZone" ? 'N' : 'Y');

                    if (CheckState == 'N')
                    {

                        string insertstr = "insert into tb_areaset(ipandport,areanum,status,descripe,devicetype,map) values('";
                        for (int i = 0; i < 5; i++)
                            insertstr += data[i] + "', '";
                        insertstr += data[0] + "," + data[1] + "-0" + "')";
                        cmsAreaSet addAS = new cmsAreaSet();
                        addAS.IPport = data[0];
                        addAS.AreaNum = data[1];
                        addAS.Status = data[2];
                        addAS.Describe = data[3];
                        addAS.DeviceType = data[4];
                        addAS.Map = data[0] + "," + data[1] + "-0";
                        FormMain.DataModel.AreaSet.Add(addAS);

                        FormDenfenAreaSet.DataLength++;
                        UpdateDataBase(insertstr);
                    }
                    else
                    {

                        for (int j = 1; j < 3; j++)
                        {

                            string insertstr = "insert into tb_areaset(ipandport,areanum,status,descripe,devicetype,map) values('";
                            for (int i = 0; i < 3; i++)
                                insertstr += data[i] + "', '";
                            insertstr += data[3] + "_" + j.ToString() + "', '";
                            insertstr += data[4] + "', '";
                            insertstr += data[0] + "," + data[1] + "-" + j.ToString() + "')";

                            cmsAreaSet addAS = new cmsAreaSet();
                            addAS.IPport = data[0];
                            addAS.AreaNum = data[1];
                            addAS.Status = data[2];
                            addAS.Describe = data[3] + "_" + j.ToString();
                            addAS.DeviceType = data[4];
                            addAS.Map = data[0] + "," + data[1] + "-" + j.ToString();
                            FormMain.DataModel.AreaSet.Add(addAS);

                            FormDenfenAreaSet.DataLength++;
                            UpdateDataBase(insertstr);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Zone Number Is Repeated Under The Same Base Station","WARN");
                }
                
            }
            else
            {
                MessageBox.Show("Please complete the filling");
            }
            LoadTreeView();
        }
        
        private void FormDefenAreaAdd_Load(object sender, EventArgs e)
        {
            string sql = "select IPandPort,StaName from tb_StaSet";

            int len = FormMain.DataModel.StationSet.Count;
            for (int i = 0; i < len; i++)
            {
                StaBuff[FormMain.DataModel.StationSet[i].StaName] = FormMain.DataModel.StationSet[i].IPandPort;
            }

            for (int i=0;i< len; i++)
                comboBox_StaName.Items.Add(FormMain.DataModel.StationSet[i].StaName);
            if (len!=0) comboBox_StaName.SelectedIndex = 0;

            comboBox_AreaStatus.Items.Add("Arm");
            comboBox_AreaStatus.Items.Add("Disarm");
            comboBox_AreaStatus.SelectedIndex = 0;

            comboBox_type.Items.Add("SingleZone-Launch high voltage pulse host");
            comboBox_type.Items.Add("DualZone-Launch high voltage pulse host");
            comboBox_type.Items.Add("SingleZone-Tension sensor alarm host");
            comboBox_type.Items.Add("DualZone-Tension sensor alarm host");
            comboBox_type.Items.Add("SingleZone-Address code");
            comboBox_type.Items.Add("DualZone-Address code");
            comboBox_type.Items.Add("SingleZone-Touch the fence automatically alarm host");
            comboBox_type.Items.Add("DualZone-Touch the fence automatically alarm host");
            comboBox_type.Items.Add("SingleZone-viberating fiber");
            comboBox_type.Items.Add("DualZone-viberating fiber");
            comboBox_type.Items.Add("SingleZone-leaking cable");
            comboBox_type.Items.Add("SingleZone-Electrostatic induction");     
            comboBox_type.SelectedIndex = 0;
        }

        private void comboBox_type_TextChanged(object sender, EventArgs e)
        {
            char CheckState = (comboBox_type.Text.Split('-')[0]== "SingleZone" ? 'N' : 'Y');
        }
    }
}
