using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace DigitalFenceMonitor
{
    public partial class FormMusic : Form
    {
        OleDbConnection conn = FormMain.conn;
        static public string MusicPath = "";
        public FormMusic()
        {
            InitializeComponent();
        }

        private void btn_Brows_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "Please select the file";
            fileDialog.Filter = "All Files(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = fileDialog.FileName;
                textBox_MapPath.Text = file;
            }
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("The file selected:" + textBox_MapPath.Text, "Selecting file hint", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
            {
                MusicPath = textBox_MapPath.Text;
                string destPath = Path.Combine(@".\", "alert.wav");
                File.Copy(@MusicPath, destPath, true);
                FormMain.player.Load();
            }
            else
            {
                textBox_MapPath.Text = "";
            }
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
