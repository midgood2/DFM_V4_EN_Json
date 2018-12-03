using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Net;
using System.Collections.Generic;

namespace DigitalFenceMonitor
{
    public partial class FormSingleSet : Form
    {
        OleDbConnection conn = FormMain.conn;
        
        public FormSingleSet()
        {
            InitializeComponent();
        }

        public delegate void SendDt(byte[] data, EndPoint RemotePoin);
        public event SendDt SendData;

        public delegate void TStart();
        public event TStart TimerStart;
        public delegate void TStop();
        public event TStop TimerStop;


        private void SingleSet_Load(object sender, EventArgs e)
        {
            SingleSetInit();
        }
        private void SingleSetInit()
        {
            
            for (int i = 0; i < FormMain.RemotePointArrCount; i++)
            {
                string ipport = FormMain.RemotePointArr[i].ToString();
                this.comboBox_StaName.Items.Add(FormMain.StaBuff[ipport]);
            }
                

            this.comboBox_BuCheFang.Items.Add("Disarm");
            this.comboBox_BuCheFang.Items.Add("Arm");
            comboBox_BuCheFang.SelectedIndex = 0;

            this.comboBox_Beep.Items.Add("turn ON buzzer");
            this.comboBox_Beep.Items.Add("turn OFF buzzer");
            comboBox_Beep.SelectedIndex = 0;

            this.comboBox_Mode.Items.Add("Pulse mode");
            this.comboBox_Mode.Items.Add("Smart mode");
            this.comboBox_Mode.Items.Add("Electrostatic mode");
            comboBox_Mode.SelectedIndex = 0;

            this.comboBox_JingDian.Items.Add("Accuracy grade 1");
            this.comboBox_JingDian.Items.Add("Accuracy grade 2");
            this.comboBox_JingDian.Items.Add("Accuracy grade 3");
            this.comboBox_JingDian.Items.Add("Accuracy grade 4");
            comboBox_JingDian.SelectedIndex = 0;

            for (int i = 0; i < 10; i++)
                this.comboBox_Voltage.Items.Add(i.ToString());
            comboBox_Voltage.SelectedIndex = 0;


        }
        private void but_confirm0_Click(object sender, EventArgs e)
        {

            if (comboBox_StaName.Text!="" && comboBox_DeviceAdd.Text!="")
            {
                string temp = FormMain.IPBuff[ comboBox_StaName.Text ];
                IPAddress IPadr = IPAddress.Parse(temp.Split(':')[0]);//先把string类型转换成IPAddress类型
                IPEndPoint EndPoint = new IPEndPoint(IPadr, int.Parse(temp.Split(':')[1]));//传递IPAddress和Port

                byte[] data = new byte[7];
                data[0] = 0xAA;
                data[1] = Convert.ToByte(comboBox_DeviceAdd.Text.ToString());
                data[2] = 0xF8;
                data[3] = 0x07;
                data[5] = Convert.ToByte(comboBox_Voltage.Text);
                data[6] = 0;
                
                byte num3 = (byte)comboBox_JingDian.SelectedIndex;
                byte num2 = (byte)comboBox_Mode.SelectedIndex;
                byte num1 = (byte)comboBox_Beep.SelectedIndex;
                byte num0 = (byte)comboBox_BuCheFang.SelectedIndex;

                data[4] = (byte)((byte)num3 * 64 + (byte)num2 * 16 + (byte)num1 * 8 + (byte)num0 * 4 + (byte)0 * 2 + (byte)1 * 1);
                InfoUpdate(temp, comboBox_DeviceAdd.Text, comboBox_BuCheFang.Text + ", 电压" + comboBox_Voltage.Text + ".6Kv");
                TimerStop();
                SendData(data, EndPoint);
                TimerStart();
            }
            else
            {
                MessageBox.Show("Please complete the filling");
            }            



        }
        private string[] AlertInfoBuff = new string[7] { "NowDate", "NowTime", "StationIP", "StationName", "AreaNum", "AlertType", "Descripe" };
        private void InfoUpdate(string StaIP, string AreaNum, string AlertType)
        {
            string datetime = DateTime.Now.ToString();
            string[] alertinfobuff = new string[7];
            alertinfobuff[0] = datetime.Split(' ')[0];
            alertinfobuff[1] = datetime.Split(' ')[1];
            alertinfobuff[2] = StaIP;
            alertinfobuff[3] = FormMain.StaBuff[StaIP];
            alertinfobuff[4] = AreaNum;
            alertinfobuff[5] = AlertType;
            alertinfobuff[6] = "ALL";

            string cmd = "insert into tb_Content (NowDate,NowTime,StationIP,StationName,AreaNum," +
                "AlertType,Descripe,OperationInfo,Operator,OperationResult) values(";
            for (int i = 0; i < AlertInfoBuff.Length; i++)
                cmd += "'" + alertinfobuff[i] + "',";
            cmd += "'" + "none" + "',";
            cmd += "'" + "admin" + "',";
            cmd += "'" + "has been sent" + "'";
            cmd += ")";

            OleDbCommand comm = new OleDbCommand(cmd, conn);
            try { comm.ExecuteNonQuery(); }
            catch {; }
        }
        private void but_confirm1_Click(object sender, EventArgs e)
        {
            if (rbtn_daynight_on.Checked)
            {
                if (comboBox_StaName.Text != "" && comboBox_DeviceAdd.Text != "" && textBox_dayTime.Text != "" && textBox_nightTime.Text != "")
                {
                    try
                    {
                        string dt0, dt1;
                        if (checkDateString())
                        {
                            dt0 = "20000101" + textBox_dayTime.Text.Remove(2, 1) + "00";
                            dt1 = "20000101" + textBox_nightTime.Text.Remove(2, 1) + "00";
                            DateTime day = DateTime.ParseExact(dt0, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                            DateTime night = DateTime.ParseExact(dt1, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                            int dayvol = comboBox_dayVol.SelectedIndex;
                            int nightvol = comboBox_nightVol.SelectedIndex;

                            FormMain.DayNightBuff[lb_thisip.Text + "," + comboBox_DeviceAdd.Text] = new FormMain.DayNightBuffCLS(day, night, dayvol, nightvol);
                            string cmd = "insert into tb_DayNightAlert (IpPortNum,Status) values(";
                            cmd += "'" + lb_thisip.Text + "," + comboBox_DeviceAdd.Text + "',";
                            cmd += "'" + textBox_dayTime.Text + "-" + dayvol + "-" + textBox_nightTime.Text + "-" + nightvol + "'";
                            cmd += ")";
                            OleDbCommand comm = new OleDbCommand(cmd, conn);
                            comm.ExecuteNonQuery();
                        }
                        else
                            MessageBox.Show("date format error", "WARN");
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine(exp.Message.Length);
                        if (exp.Message.Length == 30 || exp.Message.Length == 64 )
                            MessageBox.Show("date format error", "WARN");
                        else
                        {
                            string cmd = "update tb_DayNightAlert set Status = ";
                            cmd += "'" + textBox_dayTime.Text + "-" + comboBox_dayVol.SelectedIndex + "-" + textBox_nightTime.Text + "-" + comboBox_nightVol.SelectedIndex + "'";
                            cmd += " where IpPortNum = ";
                            cmd += "'" + lb_thisip.Text + "," + comboBox_DeviceAdd.Text + "'";
                            OleDbCommand comm = new OleDbCommand(cmd, conn);
                            try
                            {
                                comm.ExecuteNonQuery();
                            }
                            catch
                            {
                                MessageBox.Show("setting errer，try again", "WARN");
                            }
                            
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please complete the filling");
                }
            }
            else
            {
                if (comboBox_StaName.Text != "" && comboBox_DeviceAdd.Text != "")
                {
                    try
                    {
                        FormMain.DayNightBuff.Remove(lb_thisip.Text + "," + comboBox_DeviceAdd.Text);
                        string cmd = "delete * from tb_DayNightAlert Where IpPortNum =";
                        cmd += "'" + lb_thisip.Text + "," + comboBox_DeviceAdd.Text + "'";
                        OleDbCommand comm = new OleDbCommand(cmd, conn);
                        comm.ExecuteNonQuery();
                    }
                    catch { MessageBox.Show("delete failed"); }
                }
                else
                {
                    MessageBox.Show("Please complete the filling");
                }
            }
        }

        private bool checkDateString()
        {
            bool res = true;
            string str0 = textBox_dayTime.Text;
            string str1 = textBox_nightTime.Text;
            char[] date0 = str0.Remove(2, 1).ToCharArray();
            char[] date1 = str1.Remove(2, 1).ToCharArray();
            if (date0.Length == 4 && date1.Length == 4 && str0.Substring(2,1)==":" && str1.Substring(2,1)==":")
            {
                for (int i = 0; i < 4; ++i)
                    if (
                        date0[i] >= '0' && date0[i] <= '9' && date1[i] >= '0' && date1[i] <= '9'
                        )
                        continue;
                    else { res = false; break; }
            }
            else
                res = false;
            return res;
        }
        private void DataBaseSelect(string str)
        {
            lb_thisip.Text = FormMain.IPBuff[str] ;
            string sql = "select AreaNum from tb_AreaSet where IPandPort = '"+ lb_thisip.Text + "'";
            DataTable tempdt = new DataTable();
            OleDbDataAdapter tempda = new OleDbDataAdapter(sql, conn);
            tempda.Fill(tempdt);

            comboBox_DeviceAdd.Items.Clear();
            comboBox_DeviceAdd.Text = "";

            for (int i = 0; i < tempdt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    int x = Convert.ToInt16(tempdt.Rows[i]["AreaNum"]);
                    int y = Convert.ToInt16(tempdt.Rows[i - 1]["AreaNum"]);
                    if (x != y) comboBox_DeviceAdd.Items.Add(tempdt.Rows[i]["AreaNum"]);
                }
                else
                {
                    comboBox_DeviceAdd.Items.Add(tempdt.Rows[i]["AreaNum"]);
                }
            }
            
        }

        private void comboBox_StaName_TextChanged(object sender, EventArgs e)
        {
            DataBaseSelect(comboBox_StaName.Text);
        }

        private void rbtn_daynight_off_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_daynight_off.Checked)
            {
                rbtn_daynight_on.Checked = false;
            }
            else
            {
                rbtn_daynight_on.Checked = true;
            }
        }

        private void rbtn_daynight_on_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_daynight_on.Checked)
            {
                rbtn_daynight_off.Checked = false;

                lab_dayTime.Enabled = true;
                lab_dayVol.Enabled = true;
                lab_nightTime.Enabled = true;
                lab_nightVol.Enabled = true;

                textBox_dayTime.Enabled = true;
                comboBox_dayVol.Enabled = true;
                textBox_nightTime.Enabled = true;
                comboBox_nightVol.Enabled = true;

                for (int i = 0; i < 10; i++)
                {
                    comboBox_dayVol.Items.Add(i.ToString());
                    comboBox_nightVol.Items.Add(i.ToString());
                }
                comboBox_dayVol.SelectedIndex = 0;
                comboBox_nightVol.SelectedIndex = 5;

            }
            else
            {
                rbtn_daynight_off.Checked = true;

                comboBox_dayVol.Text = "";
                comboBox_nightVol.Text = "";
                comboBox_dayVol.Items.Clear();
                comboBox_nightVol.Items.Clear();

                lab_dayTime.Enabled = false;
                lab_dayVol.Enabled = false;
                lab_nightTime.Enabled = false;
                lab_nightVol.Enabled = false;

                textBox_dayTime.Enabled = false;
                comboBox_dayVol.Enabled = false;
                textBox_nightTime.Enabled = false;
                comboBox_nightVol.Enabled = false;
            }
        }


    }
}
