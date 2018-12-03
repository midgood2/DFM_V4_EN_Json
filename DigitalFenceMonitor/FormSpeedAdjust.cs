using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DigitalFenceMonitor
{
    public partial class FormSpeedAdjust : Form
    {
        public const int maxSpeed = 10000;
        public const int minSpeed = 40;

        public FormSpeedAdjust()
        {
            InitializeComponent();
        }

        private void FormSpeedAdjust_Load(object sender, EventArgs e)
        {
            showNowSpeed();
        }

        private void btn_Up_Click(object sender, EventArgs e)
        {
            FormMain.speedAdjustment -= 5;
            if (FormMain.speedAdjustment < minSpeed)
            {
                FormMain.speedAdjustment = minSpeed;
                MessageBox.Show("Maxium speed not more than " + minSpeed.ToString() + "ms per time", "WARN");
            }
            showNowSpeed();
            setSpeedAdjustment();
        }
        public delegate void setSpeed();
        public event setSpeed setSpeedAdjustment;
        private void btn_Down_Click(object sender, EventArgs e)
        {
            FormMain.speedAdjustment += 5;
            if (FormMain.speedAdjustment > maxSpeed)
            {
                FormMain.speedAdjustment = maxSpeed;
                MessageBox.Show("Minium speed not lower than " + maxSpeed.ToString() + "ms per time", "WARN");
            }
            showNowSpeed();
            setSpeedAdjustment();
        }

        private void showNowSpeed()
        {
            lab_speedAdj.Text = FormMain.speedAdjustment.ToString() + "ms/time";
        }
    }
}
