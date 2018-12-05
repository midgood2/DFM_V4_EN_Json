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
    public partial class FormStaChange : Form
    {
        public FormStaChange()
        {
            InitializeComponent();
        }

        string ip, name, port;
        private void ChangeDatabase_Load(object sender, EventArgs e)
        {
            ip = FormStaSet.forChange[0];
            port = FormStaSet.forChange[1];
            name = FormStaSet.forChange[2];

            textBox_IP.Text = textBox_IPn.Text = ip;
            textBox_Port.Text = textBox_Portn.Text = port;
            textBox_SatName.Text = textBox_SatNamen.Text = name;
        }

        public delegate void updategridView();
        public event updategridView update_datagridView;

        public delegate void GetDBData();
        public event GetDBData GetDatabaseData;

        public delegate void Load_TreeView();
        public event Load_TreeView LoadTreeView;

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            string ipn = textBox_IPn.Text.Trim();
            string namen = textBox_SatNamen.Text.Trim();
            string portn = textBox_Portn.Text.Trim();
            if (true)
            {
                string sql = null;
                
                try
                {
                    sql = " update tb_StaSet set";
                    sql += "    IP= '" + ipn;
                    sql += "' , Port= '" + portn;
                    sql += "' , StaName= '" + namen;
                    sql += "' , IPandPort= '" + ipn + ":" + portn;
                    sql += " 'Where IPandPort = '" + ip + ":" + port + "'";
                    for (int i=0;i<FormMain.DataModel.StationSet.Count;i++)
                    {
                        cmsStationSet udSS = FormMain.DataModel.StationSet[i];
                        if (udSS.IPandPort == (ip + ":" + port))
                        {
                            udSS.IP = ipn;
                            udSS.Port = portn;
                            udSS.StaName = namen;
                            udSS.IPandPort = ipn + ":" + portn;
                            FormMain.DataModel.StationSet[i] = udSS;
                        }
                    }

                    update_datagridView();
                }
                catch
                {
                    MessageBox.Show("change failed");
                }

                try
                {
                    sql = " update tb_AreaSet set ";
                    sql += " IPandPort= '" + ipn + ":" + portn;
                    sql += "' , Map = replace( Map , '" + ip + ":" + port + "' , '" + ipn + ":" + portn + "' )";
                    sql += " Where IPandPort = '" + ip + ":" + port + "'";
                    for (int i = 0; i < FormMain.DataModel.AreaSet.Count; i++)
                    {
                        cmsAreaSet udAS = FormMain.DataModel.AreaSet[i];
                        if (udAS.IPport == (ip + ":" + port))
                        {
                            udAS.IPport = ipn + ":" + portn;
                            udAS.Map = udAS.Map.Replace(ip + ":" + port, ipn + ":" + portn);
                            FormMain.DataModel.AreaSet[i] = udAS;
                        }
                        
                    }
                }
                catch
                {
                    MessageBox.Show("change failed");
                }

                try
                {
                    sql = " update tb_Map set ";
                    sql += " MapInfo = replace( MapInfo , '" + ip + ":" + port + "' , '" + ipn + ":" + portn + "' )";
                    foreach (cmsMap udMap in FormMain.DataModel.Map)
                    {
                        udMap.MapInfo = udMap.MapInfo.Replace(ip + ":" + port, ipn + ":" + portn);
                    }
                }
                catch
                {
                    MessageBox.Show("change failed");
                }


                GetDatabaseData();
                LoadTreeView();
                this.Close();
            }
            else
            {
                MessageBox.Show("Fill in non-standard");
            }
                    
        }
    }
}
