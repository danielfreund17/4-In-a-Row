using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace B16_Ex06_1
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            showInstructions();
        }

        private void showInstructions()
        {
            try
            {
              //  string[] allLines = File.ReadAllLines(@"C:/FourInARowHelp.txt");
            //    foreach (string line in allLines)
            //    {
            //        this.textInstructions.Text += line.ToString() + Environment.NewLine;
            //    }
            }
            catch(Exception)
            {
                MessageBox.Show(@"File Not Found!
Make sure that the text file is in the folder C:\ ");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
