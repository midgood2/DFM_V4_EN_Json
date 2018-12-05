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
    public partial class FormDefenceAreaChange : Form
    {
        public FormDefenceAreaChange()
        {
            InitializeComponent();
        }

        string ipPort, name, addr, posi, type;

        private void FormDefenceAreaChange_Load(object sender, EventArgs e)
        {
            ipPort = FormDenfenAreaSet.forChange[0].Trim();
            name = FormMain.StaBuff[ipPort].Trim();
            addr = FormDenfenAreaSet.forChange[1];
            posi = FormDenfenAreaSet.forChange[2];
            type = FormDenfenAreaSet.forChange[3];

            textBox1.Text = ipPort;
            textBox2.Text = name;
            textBox_IP.Text = textBox_IPn.Text = addr;
            textBox_Port.Text = textBox_Portn.Text = type;
            textBox_SatName.Text = textBox_SatNamen.Text = posi;
        }


        public delegate void GetDBData();
        public event GetDBData GetDatabaseData;

        public delegate void Load_TreeView();
        public event Load_TreeView LoadTreeView;

        public delegate void updategridView();
        public event updategridView update_datagridView;

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            string addressOld = textBox_IP.Text.Trim();
            string address = textBox_IPn.Text.Trim();
            string position = textBox_SatNamen.Text.Trim();
            string type = textBox_Portn.Text.Trim();

            string sql = null;

            try
            {
                sql = " update tb_AreaSet set ";
                sql += "    AreaNum = '" + address;
                sql += "' , Descripe = '" + position;
                sql += "' , DeviceType = '" + type;
                sql += "' , Map = replace( Map , '," + addressOld + "-' , '," + address + "-' )";
                sql += " Where IPandPort = '" + ipPort + "' and AreaNum = '" + addr +"' and Descripe = '" + posi + "'";
                for(int i = 0; i < FormMain.DataModel.AreaSet.Count; i++)
                {
                    cmsAreaSet tmpAS = FormMain.DataModel.AreaSet[i];
                    if ( tmpAS.IPport == ipPort
                        && tmpAS.AreaNum == addr 
                        && tmpAS.Describe == posi
                        )
                    {
                        tmpAS.AreaNum = address;
                        tmpAS.Describe = position;
                        tmpAS.DeviceType = type;
                        tmpAS.Map = tmpAS.Map.Replace(addressOld+"-", address+"-");
                        FormMain.DataModel.AreaSet[i] = tmpAS;

                    }
                }

                sql = " update tb_AreaSet set ";
                sql += "    AreaNum = '" + address;
                sql += "' , DeviceType = '" + type;
                sql += "' , Map = replace( Map , '," + addressOld + "-' , '," + address + "-' )";
                sql += " Where IPandPort = '" + ipPort + "' and AreaNum = '" + addr + "'";
                for (int i = 0; i < FormMain.DataModel.AreaSet.Count; i++)
                {
                    cmsAreaSet tmpAS = FormMain.DataModel.AreaSet[i];
                    if (tmpAS.IPport == ipPort
                        && tmpAS.AreaNum == addr
                        )
                    {
                        tmpAS.AreaNum = address;
                        tmpAS.DeviceType = type;
                        tmpAS.Map = tmpAS.Map.Replace(addressOld + "-", address + "-");
                        FormMain.DataModel.AreaSet[i] = tmpAS;
                    }
                }

                update_datagridView();
            }

            catch
            {
                MessageBox.Show("Zone change failed");
            }



            try
            {
                sql = " update tb_Map set ";
                sql += " MapInfo = '" + ipPort + "," + address + "," + position + "'";
                sql += " Where MapInfo = '" + ipPort + "," + addr + "," + posi + "'";
                for (int i = 0; i < FormMain.DataModel.Map.Count; i++)
                {
                    cmsMap tmpMap = FormMain.DataModel.Map[i];
                    if (tmpMap.MapInfo == ipPort + "," + addr + "," + posi )
                    {
                        tmpMap.MapInfo = ipPort + "," + address + "," + position;
                        FormMain.DataModel.Map[i] = tmpMap;
                    }
                }

                sql = " update tb_Map set ";
                sql += " MapInfo = replace(MapInfo,',"+ addr + ",',',"+ address + ",'"+")";
                for (int i = 0; i < FormMain.DataModel.Map.Count; i++)
                {
                    cmsMap tmpMap = FormMain.DataModel.Map[i];
                    tmpMap.MapInfo = tmpMap.MapInfo.Replace(addr,address);
                    FormMain.DataModel.Map[i] = tmpMap;
                }
            }

            catch
            {
                MessageBox.Show("map change failed");
            }


            GetDatabaseData();
            LoadTreeView();
            this.Close();
        }
    }
}
