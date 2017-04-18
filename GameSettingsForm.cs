using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace B16_Ex06_1
{
    public partial class GameSettingsForm : Form
    {
        private Button buttonStart;
        private Label label1, label2, label3, label4, label5, label6;
        private TextBox firstPlayer, secondPlayer;
        private NumericUpDown rows, cols;

        public GameSettingsForm()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>

        #endregion

        public string PlayerOneName
        {
            get { return firstPlayer.Text; }
        }

        public string PlayerTwoName
        {
            get { return secondPlayer.Text; }
        }

        public bool IsNamesEntered()
        {
            bool res = true;
            if (firstPlayer.Text == string.Empty || secondPlayer.Text == string.Empty)
            {
                res = false;
            }

            return res;
        }

        public int GetRows
        {
            get { return (int)rows.Value; }
        }

        public int GetCols
        {
            get { return (int)cols.Value; }
        }

        public void SetStartButtonMethod(EventHandler i_EventHandler)
        {
            buttonStart.Click += i_EventHandler;
        }

        private void start_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            buttonStart.BackColor = Color.LightSlateGray;
            buttonStart.ForeColor = Color.White;
        }

        private void start_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            buttonStart.BackColor = System.Drawing.SystemColors.Highlight;
            buttonStart.ForeColor = Color.Black;
        }

        private void player_TextChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
    }
}
