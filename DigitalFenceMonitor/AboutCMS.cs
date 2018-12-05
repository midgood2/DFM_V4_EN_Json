﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Data.OleDb;

namespace DigitalFenceMonitor
{
    public partial class AboutCMS : Form
    {
        public List<string> macs = new List<string>();
        string dis = "";
        string disr = "";

        public AboutCMS()
        {
            InitializeComponent();
        }
        //本机的MAC地址列表
       
        private void AboutCMS_Load(object sender, EventArgs e)
        {
            disr = FormMain.DataModel.cmsRegister.HoyoSerial;

                

            GetMacByWMI();         
            string[] mac = macs[0].Split(':');

            for(int i=0;i<mac.Length;i++)
            {
                byte b0, b1;
                b0 = (byte)((((int)Convert.ToInt16(mac[i],16) << 4) & 0xF0) + 0);
                b1 = (byte)((((int)Convert.ToInt16(mac[i],16) >> 4) & 0x0F) + 1);
                dis += (b0 | b1).ToString("x").PadLeft(2, '0');
            }

            textBox_ser.ReadOnly = true;
            textBox_ser.Text = dis;

            if (disr != "null")
            {
                btn_reg.Enabled = false;
                btn_reg.Text = "Registration success";
                textBox_reg.Text = disr;
            }
        }
        public List<string> GetMacByWMI()
        {

            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        mac = mo["MacAddress"].ToString();
                        macs.Add(mac);
                    }
                }
                moc = null;
                mc = null;
            }
            catch
            {
            }

            return macs;
        }

        private void btn_reg_Click(object sender, EventArgs e)
        {
            disr = textBox_reg.Text;
            if (disr.Length==12)
            {
                string[] mac = macs[0].Split(':');
                string[] macr = new string[6];
                for (int i = 0; i < mac.Length; i++)
                {
                    byte b0, b1;
                    b0 = (byte)((((int)Convert.ToInt16(mac[i], 16) << 4) & 0xF0) + 0);
                    b1 = (byte)((((int)Convert.ToInt16(mac[i], 16) >> 4) & 0x0F) + 1);
                    if (i % 2 == 0)
                        macr[i] = (((b0 | b1) >> 1) & 0xFF).ToString("x").PadLeft(2, '0');
                    else
                        macr[i] = (((b0 | b1) << 1) & 0xFF).ToString("x").PadLeft(2, '0');
                }

                if (disr==(macr[1] + macr[0] + macr[3] + macr[2] + macr[5] + macr[4]))
                {
                    string sql = " update tb_reg set HoyoSerial ='" + disr + "'";
                    FormMain.DataModel.cmsRegister.HoyoSerial = disr;
                    btn_reg.Enabled = false;
                    btn_reg.Text = "Registration success";
                }
                
            }
            else
            {
                MessageBox.Show("Registration failed");
            }
        }
    }
}
