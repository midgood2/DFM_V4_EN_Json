using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Data.OleDb;

namespace DigitalFenceMonitor
{
    public partial class FormSysConfig : Form
    {
        FormMain fm = new FormMain();

        public delegate void UpdateMsgTB(string msg);

        public FormSysConfig()
        {
            InitializeComponent();
        }

        private void SystemConfig_Load(object sender, EventArgs e)
        {
            if (FormMain.AddressList.Length>0)
            {
                for (int i = 0; i < FormMain.AddressList.Length; i++)
                    comboBox_localIP.Items.Add(FormMain.AddressList[i].ToString());
                comboBox_localIP.SelectedIndex = 0;
            }
            
        }



        public event UpdateMsgTB UpdateMsgTextBox;

        private void button_Confirm_Click(object sender, EventArgs e)
        {
            string localip = comboBox_localIP.Text;
            string localport = textBox_localPort.Text; 

            if(fm.getValidIP(localip) != null && fm.getValidPort(localport) != -1) { 
                FormMain.localIP = fm.getValidIP(localip);
                FormMain.localPort = fm.getValidPort(localport);
                UpdateMsgTextBox("Has been set->Localhost IP:Port=" + localip + ":" + localport + "    please update config");
                
                string sql = " update tb_reg set lastip ='" + localip.ToString() + "',lastport='"+localport.ToString()+"'";

                FormMain.DataModel.cmsRegister.LastIp = localip.ToString();
                FormMain.DataModel.cmsRegister.LastPort = localport.ToString();
                
                this.Close();
            }
            else
            {
                if (fm.getValidIP(localip) == null)
                    UpdateMsgTextBox("localhost IP get failed");
                else { 
                    UpdateMsgTextBox("localhost port input error");
                    UpdateMsgTextBox("Localhost->IP:Port=" + FormMain.localIP + ":" + FormMain.localPort + "");
                }
            }

        }
    }
}
