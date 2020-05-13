using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Breakout
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private double score()
        {
            //倍率設定
            double bairitsu = ((Form2.mode + 1) /10 * 2) + 1;
            //加速ONで倍率2倍
            bairitsu *= 2;

            //基準点300*ブロック数*モード倍率
            return 300 * (Form1.blockNumMax - Form1.blockNum) * bairitsu;
        }

        private void replay_Click(object sender, EventArgs e)
        {
            //画面を閉じ、プレイ画面を開く
            this.Close();
            this.Hide();
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }

        private void backHome_Click(object sender, EventArgs e)
        {
            //画面を閉じる
            this.Close();
        }

        private void form3_Load(object sender, EventArgs e)
        {
            label2.Text = score().ToString();

            //経過時間例 00:01:03.1235785 → 03.123
            label4.Text = Form1.keikaTime.Elapsed.ToString().Substring(6, 6);

            label6.Text = Form2.modeText;

            if (Form1.blockNum > 0)
            {
                label7.Text = "NotGood..";
                label7.ForeColor = Color.Black;
            }
        }
    }
}
