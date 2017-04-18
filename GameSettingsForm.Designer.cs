using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace B16_Ex06_1
{
    public partial class GameSettingsForm : Form
    {       
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameSettingsForm));
            this.buttonStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.firstPlayer = new System.Windows.Forms.TextBox();
            this.secondPlayer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rows = new System.Windows.Forms.NumericUpDown();
            this.cols = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)this.rows).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.cols).BeginInit();
            this.SuspendLayout();

            //// buttonStart

            this.buttonStart.BackColor = System.Drawing.SystemColors.Highlight;
            this.buttonStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonStart.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonStart.FlatAppearance.BorderSize = 2;
            this.buttonStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonStart.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.buttonStart.Location = new System.Drawing.Point(42, 221);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(198, 28);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.MouseLeave += new System.EventHandler(this.start_MouseLeave);
            this.buttonStart.MouseHover += new System.EventHandler(this.start_MouseHover);

            //// label1

            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe Print", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.label1.Location = new System.Drawing.Point(44, 9);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(89, 33);
            this.label1.TabIndex = 1;
            this.label1.Text = "Players:";

            //// label2

            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe Print", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.label2.Location = new System.Drawing.Point(61, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Player 1:";

            //// label3

            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(106, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 3;

            //// firstPlayer

            this.firstPlayer.Location = new System.Drawing.Point(140, 62);
            this.firstPlayer.Name = "firstPlayer";
            this.firstPlayer.Size = new System.Drawing.Size(100, 20);
            this.firstPlayer.TabIndex = 0;

            //// secondPlayer

            this.secondPlayer.Location = new System.Drawing.Point(140, 93);
            this.secondPlayer.Name = "secondPlayer";
            this.secondPlayer.Size = new System.Drawing.Size(100, 20);
            this.secondPlayer.TabIndex = 1;

            //// label4

            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe Print", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.label4.Location = new System.Drawing.Point(44, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 33);
            this.label4.TabIndex = 7;
            this.label4.Text = "Board Size:";

            //// rows

            this.rows.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rows.Location = new System.Drawing.Point(92, 185);
            this.rows.Maximum = new decimal(new int[]
            {
            10,
            0,
            0,
            0
            });
            this.rows.Minimum = new decimal(new int[] 
            {
            4,
            0,
            0,
            0
            });
            this.rows.Name = "rows";
            this.rows.Size = new System.Drawing.Size(41, 20);
            this.rows.TabIndex = 3;
            this.rows.Value = new decimal(new int[] 
            {
            4,
            0,
            0,
            0
            });

            //// cols

            this.cols.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cols.Location = new System.Drawing.Point(199, 185);
            this.cols.Maximum = new decimal(new int[] 
            {
            10,
            0,
            0,
            0
            });
            this.cols.Minimum = new decimal(new int[] 
            {
            4,
            0,
            0,
            0
            });
            this.cols.Name = "cols";
            this.cols.Size = new System.Drawing.Size(41, 20);
            this.cols.TabIndex = 4;
            this.cols.Value = new decimal(new int[] 
            {
            4,
            0,
            0,
            0
            });

            //// label5

            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe Print", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.label5.Location = new System.Drawing.Point(46, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 23);
            this.label5.TabIndex = 10;
            this.label5.Text = "Rows:";

            //// label6

            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe Print", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.label6.Location = new System.Drawing.Point(153, 182);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 23);
            this.label6.TabIndex = 11;
            this.label6.Text = "Cols:";

            //// label7

            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe Print", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.label7.Location = new System.Drawing.Point(61, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 23);
            this.label7.TabIndex = 12;
            this.label7.Text = "Player 2:";

            //// GameSettingsForm

            this.AcceptButton = this.buttonStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ClientSize = new System.Drawing.Size(280, 257);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cols);
            this.Controls.Add(this.rows);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.secondPlayer);
            this.Controls.Add(this.firstPlayer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game Settings";
            ((System.ComponentModel.ISupportInitialize)this.rows).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.cols).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// 
        private System.ComponentModel.IContainer components = null;
        private Label label7;
    }
}