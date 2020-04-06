﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Breakout
{
    public partial class Form2 : Form
    {
        public static int x { get; set; }
        public static int y { get; set; }


        public Form2()
        {
            InitializeComponent();

        }

        private void start_Click(object sender, EventArgs e)
        {
            Console.WriteLine(x);
            // Form1のインスタンスを生成
            Form1 form1 = new Form1();
            // form1を表示
            form1.ShowDialog();
           
        }

        public void mode_Select(object sender, EventArgs e)
        {
            int selectedMode = comboBox1.SelectedIndex;
            switch (selectedMode)
            {
                case 0:
                    //Easy
                    x = -2;
                    y = -4;
                    break;
                case 1:
                    //Normal
                    x = -3;
                    y = -6;
                    break;
                case 2:
                    //Hard
                    x = -5;
                    y = -10;
                    break;
                case 3:
                    //Expert
                    x = -8;
                    y = -16;
                    break;
            }
        }
    }
}
