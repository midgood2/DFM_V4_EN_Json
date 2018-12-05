using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace DigitalFenceMonitor
{
    public partial class FormDigitalMap : Form
    {
        static public string MapPath = "";

        public FormDigitalMap()
        {
            InitializeComponent();
            this.ControlBox = false;
        }


        private void btn_Done_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Brows_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "Please select the file";
            fileDialog.Filter = "jpgImage|*.jpg|Image File|*.jpg;*.png;*.jpeg;*.bmp|All Files|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = fileDialog.FileName;
                textBox_MapPath.Text = file;
            }
        }

        public delegate void ChangeMP(string path);
        public event ChangeMP ChangeMapPath;
        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if(textBox_MapPath.Text!="")
                if( DialogResult.OK ==MessageBox.Show("The file selected:" + textBox_MapPath.Text, "Selecting file hint", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                {
                    MapPath = textBox_MapPath.Text;
                    ChangeMapPath(MapPath);
                    string userDocument = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\hoyoCMS";
                    if (!Directory.Exists(@userDocument))//如果不存在就创建file文件夹
                        Directory.CreateDirectory(@userDocument);
                    userDocument += "\\map.jpg";
                    File.Copy(@MapPath, userDocument, true);
                }
                else
                {
                    textBox_MapPath.Text = "";
                }
            else
                MessageBox.Show("the selection can't be NOTHING", "WARN");
        }

        private void FormDigitalMap_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < FormMain.RemotePointArrCount; i++)
            {
                string ipport = FormMain.RemotePointArr[i].ToString();
                this.comboBox_StaName.Items.Add(FormMain.StaBuff[ipport] + "-" + ipport);
            }

        }


        private void comboBox_StaName_TextChanged(object sender, EventArgs e)
        {
            string str = comboBox_StaName.Text.Split('-')[1];
            
            string sql = "select AreaNum from tb_AreaSet where IPandPort = '" + str + "'";

            comboBox_AreaNum.Items.Clear();
            comboBox_AreaNum.Text = "";

            if (FormMain.DataModel.AreaSet.Count !=0)
            {
                for (int i = 0; i < FormMain.DataModel.AreaSet.Count; i++)
                {
                    if (i != 0)
                    {
                        int x = Convert.ToInt16(FormMain.DataModel.AreaSet[i].AreaNum);
                        int y = Convert.ToInt16(FormMain.DataModel.AreaSet[i-1].AreaNum);
                        if (x == y) ;
                        else comboBox_AreaNum.Items.Add(FormMain.DataModel.AreaSet[i].AreaNum);
                    }
                    else
                    {
                        comboBox_AreaNum.Items.Add(FormMain.DataModel.AreaSet[i].AreaNum);
                    }
                }

                comboBox_AreaNum.SelectedIndex = 0;
            }
            
        }

        private void comboBox_AreaNum_TextChanged(object sender, EventArgs e)
        {
            string str1 = comboBox_StaName.Text.Split('-')[1];
            string str2 = comboBox_AreaNum.Text;

            string sql = "select Descripe from tb_AreaSet where IPandPort = '" + str1 + "' and AreaNum = '" + str2 + "'";

            comboBox_Position.Items.Clear();
            comboBox_Position.Text = "";

            if (FormMain.DataModel.AreaSet.Count != 0)
            {      
                for (int i = 0; i < FormMain.DataModel.AreaSet.Count; i++)
                {
                    comboBox_Position.Items.Add(FormMain.DataModel.AreaSet[i].Describe);
                }
                comboBox_Position.SelectedIndex = 0;
            }
            
        }

        private void btn_Set_Click(object sender, EventArgs e)
        {
            if (comboBox_StaName.Text != "" && comboBox_AreaNum.Text != "" && comboBox_Position.Text != "")
            {
                MessageBox.Show("Set" + (checkBox.CheckState.ToString() == "Checked" ? "" : "No") + "Linked");
            }
            else
            {
                MessageBox.Show("Please complete the filling", "WARN");
            }
        }
    }
}
