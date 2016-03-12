using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApolloniusCirclesDraw
{
    public partial class Form1 : Form
    {
        Bitmap bMapCanvas;
        Graphics g;
        System.Drawing.Point sp1;
        System.Drawing.Point cp1;

        System.Drawing.Point sp2;
        System.Drawing.Point cp2;

        System.Drawing.Point sp;
        System.Drawing.Point cp;

        Pen p;
        Pen p2;
        int clickCounter;

        float x1g;
        float y1g;
        float r1g;
        float x2g;
        float y2g;
        float r2g;
        float x3g;
        float y3g;
        float r3g;

        double distance1;
        double distance2;
        float distance;
        
        public Form1()
        {
            InitializeComponent();
            bMapCanvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bMapCanvas);
            clickCounter = 0;
            g.Dispose();
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            bMapCanvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bMapCanvas);
            clickCounter = 0;
            g.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            g = Graphics.FromImage(bMapCanvas);
            g.FillRectangle(Brushes.White, g.VisibleClipBounds);
            pictureBox1.Image = bMapCanvas;
            clickCounter = 0;
            g.Dispose();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //マウスが押されたとき
            sp1 = System.Windows.Forms.Cursor.Position;
            cp1 = pictureBox1.PointToClient(sp1);

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            g = Graphics.FromImage(bMapCanvas);
            //マウス釦が離されたとき
            sp2 = System.Windows.Forms.Cursor.Position;
            cp2 = pictureBox1.PointToClient(sp2);

            distance1 = Math.Pow((cp2.X - cp1.X), 2);
            distance2 = Math.Pow((cp2.Y - cp1.Y), 2);

            distance = float.Parse(Math.Sqrt(distance1 + distance2).ToString());

            clickCounter++;
            DrawFirstCircles(clickCounter);

            if (clickCounter == 3)
            {
                int calcCounter = 0;

                while (calcCounter <= 8)
                {
                    calcCounter++;
                    SolveTheApollonius(calcCounter);
                }
                clickCounter = 0;
            }
            g.Dispose();
        }

        private void DrawFirstCircles(int CCounter)
        {
            g = Graphics.FromImage(bMapCanvas);
            sp = sp1;
            cp = cp1;
            p = new Pen(Color.Red, 1);

            if (CCounter == 1)
            {
                x1g = cp.X;
                y1g = cp.Y;
                r1g = distance;
            }
            else if (CCounter == 2)
            {
                x2g = cp.X;
                y2g = cp.Y;
                r2g = distance;
            }
            else if (CCounter == 3)
            {
                x3g = cp.X;
                y3g = cp.Y;
                r3g = distance;
            }

            if (CCounter == 1)
            {
                g.DrawEllipse(p, x1g - distance, y1g - distance, distance*2, distance*2);
            }
            else if (CCounter == 2)
            {
                g.DrawEllipse(p, x2g - distance, y2g - distance, distance*2, distance*2);
            }
            else if (CCounter == 3)
            {
                g.DrawEllipse(p, x3g - distance, y3g - distance, distance*2, distance*2);
            }

            pictureBox1.Image = bMapCanvas;
            p.Dispose();
            g.Dispose();
        }

        private void SolveTheApollonius(int calcCounter)
        {
            g = Graphics.FromImage(bMapCanvas);
            float s1 = 1;
            float s2 = 1;
            float s3 = 1;

            if (calcCounter == 2)
            {
                s1 = -1;
                s2 = -1;
                s3 = -1;
            }
            else if (calcCounter == 3)
            {
                s1 = 1;
                s2 = -1;
                s3 = -1;
            }
            else if (calcCounter == 4)
            {
                s1 = -1;
                s2 = 1;
                s3 = -1;
            }
            else if (calcCounter == 5)
            {
                s1 = -1;
                s2 = -1;
                s3 = 1;
            }
            else if (calcCounter == 6)
            {
                s1 = 1;
                s2 = 1;
                s3 = -1;
            }
            else if (calcCounter == 7)
            {
                s1 = -1;
                s2 = 1;
                s3 = 1;
            }
            else if (calcCounter == 8)
            {
                s1 = 1;
                s2 = -1;
                s3 = 1;
            }

            float x1 = x1g;
            float y1 = y1g;
            float r1 = r1g;
            float x2 = x2g;
            float y2 = y2g;
            float r2 = r2g;
            float x3 = x3g;
            float y3 = y3g;
            float r3 = r3g;

            //Currently optimized for fewest multiplications. Should be optimized for
            //readability
            float v11 = 2 * x2 - 2 * x1;
            float v12 = 2 * y2 - 2 * y1;
            float v13 = x1 * x1 - x2 * x2 + y1 * y1 - y2 * y2 - r1 * r1 + r2 * r2;
            float v14 = 2 * s2 * r2 - 2 * s1 * r1;

            float v21 = 2 * x3 - 2 * x2;
            float v22 = 2 * y3 - 2 * y2;
            float v23 = x2 * x2 - x3 * x3 + y2 * y2 - y3 * y3 - r2 * r2 + r3 * r3;
            float v24 = 2 * s3 * r3 - 2 * s2 * r2;

            float w12 = v12 / v11;
            float w13 = v13 / v11;
            float w14 = v14 / v11;

            float w22 = v22 / v21 - w12;
            float w23 = v23 / v21 - w13;
            float w24 = v24 / v21 - w14;

            float P = -w23 / w22;
            float Q = w24 / w22;
            float M = -w12 * P - w13;
            float N = w14 - w12 * Q;

            float a = N * N + Q * Q - 1;
            float b = 2 * M * N - 2 * N * x1 + 2 * P * Q - 2 * Q * y1 + 2 * s1 * r1;
            float c = x1 * x1 + M * M - 2 * M * x1 + P * P + y1 * y1 - 2 * P * y1 - r1 * r1;

            // Find a root of a quadratic equation. This requires the circle centers not
            // to be e.g. colinear
            float D = b * b - 4 * a * c;
            float rs = (-b - float.Parse(Math.Sqrt(D).ToString())) / (2 * float.Parse(a.ToString()));
            float xs = M + N * rs;
            float ys = P + Q * rs;

            p2 = new Pen(Color.Black, 1);
            g.DrawEllipse(p2, xs - rs, ys - rs, rs * 2, rs * 2);
            p2.Dispose();
            g.Dispose();
        }
    }
}
