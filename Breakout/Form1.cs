using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Breakout
{
    public partial class Form1 : Form
    {
        Vector ballPos; //位置(Vector：2D空間における変位を表す)
        Vector ballSpeed;
        int ballRadius; //半径
        Rectangle paddlePos; //パドル位置(Rectangle:四角形を作成)
        List<Rectangle> blockPos; //ブロックの位置(リスト化)
        Timer timer = new Timer(); //ボール移動タイマー
        Timer timerCount = new Timer(); //カウントダウンタイマー
        int countSize = 200; //カウント文字のサイズ
        int countNo = 3; //カウント数
        double accelSpeed = 1; //加速速度

        public static int blockNum { get; set; } // ブロック数
        public static int blockNumMax { get; set; } // ブロック数最大値

        public static Stopwatch keikaTime = new Stopwatch(); //経過時間


        public Form1()
        {
            InitializeComponent(); //設定したハンドラ等の初期設定

            this.ballSpeed = new Vector(Form2.x/3, Form2.y/3); //Form2で設定した値を代入

            this.ballPos = new Vector(200, 200);
            this.ballRadius = 10;
            this.paddlePos = new Rectangle(100, this.Height - 50, 100, 5); //(位置横縦,サイズ横縦)
            this.blockPos = new List<Rectangle>();

            blockNum = 0;

            for (int x = 0; x <= this.Height; x += 100)
            {
                for (int y = 0; y <= 150; y += 40)
                {
                    this.blockPos.Add(new Rectangle(25 + x, y, 80, 25));

                    blockNum++;
                }
            }
            blockNumMax = blockNum;

            label1.Font = new Font(label1.Font.OriginalFontName, countSize + 1); //カウントダウン初期表示サイズ指定
            label1.Left = (this.Width / 2) - (countSize / 2);
            label1.Top = (this.Height /2) - (countSize / 2);
            label1.BackColor = Color.Transparent; //背景透明化

            label2.BackColor = Color.Transparent; //背景透明化

            timer.Interval = 100;
            timerCount.Tick += new EventHandler(countDown); //timer.Trik：Timer有効時に呼ばれる
            timerCount.Start();
        }

        private void countDown(object sender, EventArgs e)
        {
            //debug用
            //if (countSize == 200 && countNo == 3)
            //{
            //    System.Threading.Thread.Sleep(8000);
            //}

            if (countSize == 0)
            {
                countNo--;
                label1.Text = countNo.ToString();
                countSize = 200;
            }

            if (countSize == 20 && countNo == 1)
            {
                timerCount.Stop();

                //ゲームスタート
                countDownAfter();
            }
            countSize -= 20;
            label1.Font = new Font(label1.Font.OriginalFontName, countSize +1);
            label1.Left = (this.Width / 2) - (countSize / 2);
            label1.Top = (this.Height / 2) - (countSize / 2);
        }

        private void countDownAfter()
        {
            //ゲームタイマー
            timer.Interval = 10;
            timer.Tick += new EventHandler(Update); //timer.Trik：Timer有効時に呼ばれる
            timer.Start();

            //経過時間スタート
            keikaTime.Restart();
        }

        private void realTime() //リアルタイム表示
        {
            label2.Text = keikaTime.Elapsed.ToString().Substring(6, 6);
        }

        /// <summary>
        /// 内積計算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        double DotProduct(Vector a, Vector b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        /// <summary>
        /// 当たり判定
        /// </summary>
        /// <param name="p1">パドル左端座標</param>
        /// <param name="p2">パドル右端座標</param>
        /// <param name="center">ボール中心</param>
        /// <param name="radius">ボール半径</param>
        /// <returns></returns>
        bool LineVsCircle(Vector p1, Vector p2, Vector center, float radius)
        {
            Vector lineDir = (p2 - p1); //パドルのベクトル(パドルの長さ)
            Vector n = new Vector(lineDir.Y, -lineDir.X); //パドルの法線
            n.Normalize();

            Vector dir1 = center - p1;
            Vector dir2 = center - p2;

            double dist = Math.Abs(DotProduct(dir1, n));
            double a1 = DotProduct(dir1, lineDir);
            double a2 = DotProduct(dir2, lineDir);

            return (a1 * a2 < 0 && dist < radius) ? true : false;
        }

        int BlockVsCircle(Rectangle block, Vector ball)
        {
            if (LineVsCircle(new Vector(block.Left, block.Top),
                new Vector(block.Right, block.Top), ball, ballRadius))
                return 1;

            if (LineVsCircle(new Vector(block.Left, block.Bottom),
                new Vector(block.Right, block.Bottom), ball, ballRadius))
                return 2;

            if (LineVsCircle(new Vector(block.Right, block.Top),
                new Vector(block.Right, block.Bottom), ball, ballRadius))
                return 3;

            if (LineVsCircle(new Vector(block.Left, block.Top),
                new Vector(block.Left, block.Bottom), ball, ballRadius))
                return 4;

            return -1;
        }

        private void Update(object sender, EventArgs e)
        {
            //リアルタイム
            realTime();

            //ボールの移動
            //加速判定
            int x = int.Parse(keikaTime.Elapsed.ToString().Substring(9, 3)); //リアルタイムのコンマ3桁を取得
            if (Form4.accelFrag == true && x/100 == 1)
            {
                ballSpeed.X *= accelSpeed;
                ballSpeed.Y *= accelSpeed;
                ballPos += ballSpeed;
                accelSpeed += 0.001;
            }
            else
            {
                ballPos += ballSpeed;
            }
            

            //左右の壁でのバウンド
            if (ballPos.X + ballRadius * 2 > this.Bounds.Width || ballPos.X - ballRadius < 0)
            {
                ballSpeed.X *= -1;
            }

            //上の壁でバウンド
            if (ballPos.Y - ballRadius < 0)
            {
                ballSpeed.Y *= -1;
            }

            //パドルの当たり判定
            if (LineVsCircle(new Vector(this.paddlePos.Left, this.paddlePos.Top),
                             new Vector(this.paddlePos.Right, this.paddlePos.Top),
                             ballPos, ballRadius)
                )
            {
                if (ballPos.Y  > paddlePos.Top)
                {
                    ballSpeed.Y *= -1;
                }
            }

            // ブロックとのあたり判定
            for (int i = 0; i < this.blockPos.Count; i++)
            {
                int collision = BlockVsCircle(blockPos[i], ballPos);
                if (collision == 1 || collision == 2)
                {
                    ballSpeed.Y *= -1;
                    this.blockPos.Remove(blockPos[i]);
                    blockNum--;
                }
                else if (collision == 3 || collision == 4)
                {
                    ballSpeed.X *= -1;
                    this.blockPos.Remove(blockPos[i]);
                    blockNum--;
                }
            }

            //失敗時・成功時
            if (ballPos.Y > this.Height || blockNum == 0)
            {
                //画面閉じてリザルト表示
                keikaTime.Stop();
                timer.Stop();
                this.Close();
                this.Hide();
                Form3 form3 = new Form3();
                form3.ShowDialog();
            }

            //画面再描画
            Invalidate();
        }

        private void Draw(object sender, PaintEventArgs e) //Draw意味:描画する
        {
            SolidBrush pinkBrush = new SolidBrush(Color.HotPink); //SolidBrush(ブラシ)は.Netのクラスで図形を塗り潰す
            SolidBrush grayBrush = new SolidBrush(Color.DimGray);
            SolidBrush blueBrush = new SolidBrush(Color.LightBlue);

            //左上からの位置を設定
            float px = (float)this.ballPos.X - ballRadius; //マイナス半径とすることで円の中心になる
            float py = (float)this.ballPos.Y - ballRadius;

            //e.描画.円(色, 横, 縦, 物質幅, 物質高さ)
            e.Graphics.FillEllipse(pinkBrush, px, py, this.ballRadius * 2, this.ballRadius * 2);
            //e.描画.長方形(色, 長方形)
            e.Graphics.FillRectangle(grayBrush, paddlePos);
            //ブロック描画
            for (int i = 0; i < this.blockPos.Count; i++)
            {
                e.Graphics.FillRectangle(blueBrush, blockPos[i]);
            }
        }

        private void KeyPressed(object sender, KeyPressEventArgs e) //押下毎
        {
            if (!timerCount.Enabled)
            {

                if (Form4.paddleMove == 0) //設定行っていない場合
                {
                    Form4.paddleMove = 20;
                }
                if (e.KeyChar == 'a' && paddlePos.Left > 0) //A押下時
                {
                    this.paddlePos.X -= Form4.paddleMove;
                }
                else if (e.KeyChar == 's' && paddlePos.Right < this.Width) //S押下時
                {
                    this.paddlePos.X += Form4.paddleMove;
                }
            }
        }

        private void form1_Closing(object sender, FormClosingEventArgs e) //×ボタン押下時
        {
            keikaTime.Stop();
            timer.Stop();
            timerCount.Stop();
            this.Close();
            this.Hide();
        }
    }
}