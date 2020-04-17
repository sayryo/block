using System;
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
        //ボールの速度(x, y)
        public static int x { get; set; }
        public static int y { get; set; }
        //モード値
        public static int mode { get; set; }
        //モードテキスト
        public static string modeText { get; set; }

        public Form2()
        {
            InitializeComponent();
        }

        private void start_Click(object sender, EventArgs e) //スタートボタン
        {
            mode_Select(sender, e);

            // Form1のインスタンスを生成
            Form1 form1 = new Form1();
            // form1を表示
            form1.ShowDialog();
        }

        private void mode_Select(object sender, EventArgs e) //モードセレクト
        {
            int selectedMode = comboBox1.SelectedIndex;
            mode = selectedMode;

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
                default:
                    //未選択時はNormal
                    x = -3;
                    y = -6;
                    break;
            }

            if (comboBox1.SelectedItem ==null)
            {
                modeText = "Normal";
            } else
            {
                modeText = comboBox1.SelectedItem.ToString();
            }
        }

        private void option_Click(object sender, EventArgs e) //設定押下時
        {
            // Form4のインスタンスを生成
            Form4 form4 = new Form4();
            // form4を表示
            form4.ShowDialog();
        }
    }
}
