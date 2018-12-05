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
using System.IO.Ports;

namespace DigitalFenceMonitor
{
    public partial class FormLinkage : Form
    {
        public FormLinkage()
        {
            InitializeComponent();
        }

        private void btn_Done_Click(object sender, EventArgs e)
        {
            if (FormMain.LinkageSelect == 1)
            {

                if (textBox_Node.Text != "" && 
                    comboBox_StaName.Text!="" && 
                    comboBox_SerialNum.Text!="" &&
                    comboBox_DeviceAdd.Text!="" &&
                    textBox_port.Text != "")
                {
                    string msg = "";
                    int all = textBox_Node.Text.Split(',').Count();
                    for (int i = 0; i < textBox_Node.Text.Split(',').Count(); i++)
                    {
                        if (textBox_Node.Text.Split(',')[i] == "") all--;
                        else
                        {
                            try { byte num = Convert.ToByte(textBox_Node.Text.Split(',')[i]); msg += num.ToString() + " "; }
                            catch { all--; }
                        }

                    }
                    
                    string LinkageNodes = msg.Trim().Replace(' ', ',');
                    string LinkageConnect = comboBox_SerialNum.Text.Trim()+":"+textBox_port.Text;
                    Console.WriteLine(LinkageConnect);
                    int area = 1;
                    if (comboBox_Position.Items.Count==2) { area = comboBox_Position.SelectedIndex + 1; }

                    FormMain.LinkageBuff[lb_thisip.Text + "-" + comboBox_DeviceAdd.Text + "-" + area.ToString()] = LinkageConnect + "-" + LinkageNodes;
                        
                    string cmd = "insert into tb_LinkageAlert (IpPortNodes,LinkInfo) values(";
                    cmd += "'" + LinkageConnect + "-" + LinkageNodes + "',";
                    cmd += "'" + lb_thisip.Text + "-" + comboBox_DeviceAdd.Text + "-" + area.ToString() + "'";
                    cmd += ")";
                    bool isUpdate = false;
                    for (int i=0;i<FormMain.DataModel.LinkageAlert.Count;i++)
                    {
                        cmsLinkageAlert updLA = FormMain.DataModel.LinkageAlert[i];
                        if (updLA.LinkInfo == lb_thisip.Text + "-" + comboBox_DeviceAdd.Text + "-" + area.ToString())
                        {
                            updLA.IpPortNodes = LinkageConnect + "-" + LinkageNodes;
                            FormMain.DataModel.LinkageAlert[i] = updLA;
                            isUpdate = true;
                            break;
                        }
                    }
                    if (!isUpdate)
                    {
                        cmsLinkageAlert addLA = new cmsLinkageAlert();
                        addLA.IpPortNodes = LinkageConnect + "-" + LinkageNodes;
                        addLA.LinkInfo = lb_thisip.Text + "-" + comboBox_DeviceAdd.Text + "-" + area.ToString();
                        FormMain.DataModel.LinkageAlert.Add(addLA);
                    }

                    MessageBox.Show("Total" + all.ToString() + "node(s) are set\r\n" + LinkageNodes + "");
                }
                else
                {
                    MessageBox.Show("Please complete the filling", "Linkage settings");
                }

            }
            else if (FormMain.LinkageSelect == 2)
            {

            }


        }

        private void FormLinkage_Load(object sender, EventArgs e)
        {
            if (FormMain.LinkageSelect==1)
            {
                for (int i = 0; i < FormMain.RemotePointArrCount; i++)
                {
                    string ipport = FormMain.RemotePointArr[i].ToString();
                    this.comboBox_StaName.Items.Add(FormMain.StaBuff[ipport]);
                }
            }
            else if (FormMain.LinkageSelect == 2)
            {
                this.Text = "Electronic map settings";

                lab_LinkNode.Visible = false;
                textBox_Node.Visible = false;

                lab_alert0.Visible = false;
                lab_alert1.Visible = false;
            }
            

            rbtn_net.Checked = true;
            rbtn_serial.Checked = false;

        }

        
        

        private void rbtn_net_CheckedChanged(object sender, EventArgs e)
        {
            lab_SerialNum.Text = "Net Addr";
            comboBox_SerialNum.Text = "";
        }

        private void rbtn_serial_CheckedChanged(object sender, EventArgs e)
        {
            lab_SerialNum.Text = "Serial Num";
            comboBox_SerialNum.Items.Clear() ;
            string[] PortName = SerialPort.GetPortNames();
            if (PortName.Length !=0)
            {
                for (int i = 0; i < PortName.Length; i++)
                {
                    this.comboBox_SerialNum.Items.Add(PortName[i]);
                }
                comboBox_SerialNum.SelectedIndex = 0;
            }
            else
            {
                comboBox_SerialNum.Text = "no valid serial port";
            }
        }

        private void FormLinkage_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormMain.DataModel.Write(JsonHelper.SerializeObject(FormMain.DataModel));
            FormMain.LinkageSelect = -1;
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            if (FormMain.LinkageSelect == 1)
            {
                FormMain.LinkageBuff.Clear();
                string cmd = "delete * from tb_LinkageAlert";
                FormMain.DataModel.LinkageAlert.Clear();
            }
        }

        private void comboBox_StaName_TextChanged(object sender, EventArgs e)
        {
            lb_thisip.Text = FormMain.IPBuff[comboBox_StaName.Text];
            string sql = "select AreaNum from tb_AreaSet where IPandPort = '" + lb_thisip.Text + "'";
            


            comboBox_DeviceAdd.Items.Clear();
            comboBox_DeviceAdd.Text = "";
            for (int i = 0;i < FormMain.DataModel.AreaSet.Count;i++)
            {
                
                if (i != 0)
                {
                    int x = Convert.ToInt16(FormMain.DataModel.AreaSet[i].AreaNum);
                    int y = Convert.ToInt16(FormMain.DataModel.AreaSet[i-1].AreaNum);
                    if (x != y) comboBox_DeviceAdd.Items.Add(FormMain.DataModel.AreaSet[i].AreaNum);
                }
                else
                {
                    comboBox_DeviceAdd.Items.Add(FormMain.DataModel.AreaSet[i].AreaNum);
                }
            }
        }

        private void comboBox_DeviceAdd_TextChanged(object sender, EventArgs e)
        {
            string str1 = lb_thisip.Text;
            string str2 = comboBox_DeviceAdd.Text;

            string sql = "select Descripe from tb_AreaSet where IPandPort = '" + str1 + "' and AreaNum = '" + str2 + "'";

            comboBox_Position.Items.Clear();
            comboBox_Position.Text = "";


            if ( FormMain.DataModel.AreaSet.Count != 0)
            {
                for (int i = 0; i < FormMain.DataModel.AreaSet.Count; i++)
                {
                    comboBox_Position.Items.Add(FormMain.DataModel.AreaSet[i].Describe);
                }
                comboBox_Position.SelectedIndex = 0;
            }
        }
    }
}
