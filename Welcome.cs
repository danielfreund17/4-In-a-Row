using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace B16_Ex06_1
{
    public delegate void OnExitPressing();

    public partial class Welcome : Form
    {
        private Label label1;
        private Label label2;
        private Timer tm;
        private Label label3;
        private PictureBox pictureBox1;

        public Welcome()
        {
            InitializeComponent();
        }

        private void tm_Tick(object sender, EventArgs e)
        {
            if(this.Height <= 40)
            {
                this.tm.Enabled = false;
                this.Close();
            }
            else
            {
                this.Height -= 15;
            }
        }

        private void timerToDisplay_Tick(object sender, EventArgs e)
        {
            close();
        }

        private void close()
        {
            timerToDisplay.Enabled = false;
            this.tm.Enabled = true;
        }
    }
}
