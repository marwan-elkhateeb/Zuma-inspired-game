using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectGraphics1
{
    public partial class Form1 : Form
    {
        public float xs, ys, xe, ye, dx, dy, m, invM, currX, currY, xM, yM;
        int Speed = 15;
        float PX = 0, PY = 0;

        public class Cball
        {
            public float x, y;
            public float tm;
            public Color BC;
        }
        public class LineSeg
        {
            public PointF PS, PE;
            public Color C;
            public void RotateYourSelf(float Xr, float Yr, float th)
            {
                PS.X -= Xr;
                PS.Y -= Yr;
                double nX, nY;
                nX = PS.X * Math.Cos(th) - PS.Y * Math.Sin(th);
                nY = PS.X * Math.Sin(th) + PS.Y * Math.Cos(th);
                PS.X = (float)nX + Xr;
                PS.Y = (float)nY + Yr;
                PE.X -= Xr;
                PE.Y -= Yr;
                nX = PE.X * Math.Cos(th) - PE.Y * Math.Sin(th);
                nY = PE.X * Math.Sin(th) + PE.Y * Math.Cos(th);
                PE.X = (float)nX + Xr;
                PE.Y = (float)nY + Yr;

            }
            public void DrawYourSelf(Graphics g)
            {
                Pen Pn = new Pen(Color.Brown, 3);
                g.DrawLine(Pn, PS, PE);
                g.FillEllipse(Brushes.Red, PS.X - 5, PS.Y - 5, 10, 10);
                g.FillEllipse(Brushes.Red, PE.X - 5, PE.Y - 5, 10, 10);
            }

        }
        Line lne = new Line();
        Bitmap off;
        BezCurve crv = new BezCurve();
        public float x;
        List<Cball> BL = new List<Cball>();
        List<LineSeg> LS = new List<LineSeg>();
        List<LineSeg> Lns = new List<LineSeg>();
        int ctTick = 0;
        float th, th1;
        float xmid;
        float ymid;
        int f = 0;
        bool B = false;
        bool fw = false;
        bool idl = true;
        bool hitted = false;
        Color[] AColors = new Color[3] { Color.Red, Color.Blue, Color.Yellow };


        Timer tt = new Timer();
        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Load += new EventHandler(Form1_Load);
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.MouseMove += Form1_MouseMove;
            this.MouseDown += Form1_MouseDown;
            tt.Tick += Tt_Tick;
            tt.Start();
        }

        public void Gravity(int index, float tm)
        {
            for (int i = index; i >= 0; i--)
            {
                BL[i].tm -= tm;
            }
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            if (f == 1)
            {
                float t = 0;
                PointF P = new PointF();
                P = crv.CalcCurvePointAtTime(t);
                float tempX = P.X;
                float tempY = P.Y;
                Random R = new Random();
                if (ctTick % 5 == 0)
                {
                    Cball pnn = new Cball();
                    pnn.x = tempX;
                    pnn.y = tempY;
                    pnn.tm = 0;
                    pnn.BC = AColors[R.Next(0, 3)];
                    BL.Add(pnn);
                }
                for (int i = 0; i < BL.Count; i++)
                {
                    if (BL[i].tm < 1)
                    {
                        PointF NP = new PointF();
                        NP = crv.CalcCurvePointAtTime(BL[i].tm);
                        BL[i].x = NP.X;
                        BL[i].y = NP.Y;
                        BL[i].tm += 0.005f;
                    }
                    else
                    {
                        tt.Stop();
                    }
                }
                if (B == true)
                {
                    LineSeg pnn = new LineSeg();
                    pnn.PE.X = Lns[0].PE.X;
                    pnn.PE.Y = Lns[0].PE.Y;
                    pnn.C = AColors[R.Next(0, 3)];
                    LS.Add(pnn);
                    B = false;
                }
                if (fw == true)
                {
                    MoveStep();
                }
                if (LS[0].PE.X > ClientSize.Width || LS[0].PE.X < 0 || LS[0].PE.Y > ClientSize.Height || LS[0].PE.Y < 0)
                {
                    LS.Clear();
                    B = true;
                    fw = false;
                    idl = true;
                }
                if (!hitted)
                {
                    //isHit();
                }



                for (int i = 0; i < BL.Count; i++)
                {

                    if (i > 2 && LS.Count > 0)
                    {
                        if (BL[i].BC == LS[0].C && BL[i - 1].BC == LS[0].C && BL[i - 2].BC == LS[0].C && BL[i - 3].BC == LS[0].C && BL[i - 4].BC == LS[0].C && idl == false)
                        {
                            if (LS[0].PE.X >= BL[i].x - 30 && LS[0].PE.X <= BL[i].x + 30 &&
                               LS[0].PE.Y >= BL[i].y - 30 && LS[0].PE.Y <= BL[i].y + 30)
                            {
                                BL.RemoveAt(i);
                                BL.RemoveAt(i - 1);
                                BL.RemoveAt(i - 2);
                                BL.RemoveAt(i - 3);
                                BL.RemoveAt(i - 4);
                                LS.Clear();
                                Gravity(i - 2, 0.08f);
                                B = true;
                                fw = false;
                                idl = true;
                                break;
                            }
                        }

                        if (BL[i].BC == LS[0].C && BL[i - 1].BC == LS[0].C && BL[i - 2].BC == LS[0].C && BL[i - 3].BC == LS[0].C && idl == false)
                        {
                            if (LS[0].PE.X >= BL[i].x - 30 && LS[0].PE.X <= BL[i].x + 30 &&
                                LS[0].PE.Y >= BL[i].y - 30 && LS[0].PE.Y <= BL[i].y + 30)
                            {

                                BL.RemoveAt(i);
                                BL.RemoveAt(i - 1);
                                BL.RemoveAt(i - 2);
                                BL.RemoveAt(i - 3);
                                LS.Clear();
                                Gravity(i - 2, 0.08f);
                                B = true;
                                fw = false;
                                idl = true;
                                break;

                            }

                            /////////////////////////
                            if (LS[0].PE.X >= BL[i - 2].x - 30 && LS[0].PE.X <= BL[i - 2].x + 30 &&
                                LS[0].PE.Y >= BL[i - 2].y - 30 && LS[0].PE.Y <= BL[i - 2].y + 30)
                            {
                                BL.RemoveAt(i);
                                BL.RemoveAt(i - 1);
                                BL.RemoveAt(i - 2);
                                BL.RemoveAt(i - 3);
                                LS.Clear();
                                Gravity(i - 2, 0.08f);
                                B = true;
                                fw = false;
                                idl = true;
                                break;
                            }

                            if (LS[0].PE.X >= BL[i - 1].x - 30 && LS[0].PE.X <= BL[i - 1].x + 30 &&
                                LS[0].PE.Y >= BL[i - 1].y - 30 && LS[0].PE.Y <= BL[i - 1].y + 30)
                            {
                                BL.RemoveAt(i);
                                BL.RemoveAt(i - 1);
                                BL.RemoveAt(i - 2);
                                BL.RemoveAt(i - 3);
                                LS.Clear();
                                Gravity(i - 2, 0.08f);
                                B = true;
                                fw = false;
                                idl = true;
                                break;
                            }
                        }

                        if (BL[i].BC == LS[0].C && BL[i - 1].BC == LS[0].C && BL[i - 2].BC == LS[0].C && idl == false)
                        {
                            if (LS[0].PE.X >= BL[i].x - 30 && LS[0].PE.X <= BL[i].x + 30 &&
                                LS[0].PE.Y >= BL[i].y - 30 && LS[0].PE.Y <= BL[i].y + 30)
                            {

                                BL.RemoveAt(i);
                                BL.RemoveAt(i - 1);
                                BL.RemoveAt(i - 2);
                                LS.Clear();
                                //  Gravity(i - 2, 0.08f);
                                B = true;
                                fw = false;
                                idl = true;
                                break;

                            }

                            if (LS[0].PE.X >= BL[i - 1].x - 30 && LS[0].PE.X <= BL[i - 1].x + 30 &&
                              LS[0].PE.Y >= BL[i - 1].y - 30 && LS[0].PE.Y <= BL[i - 1].y + 30)
                            {

                                BL.RemoveAt(i);
                                BL.RemoveAt(i - 1);
                                BL.RemoveAt(i - 2);
                                LS.Clear();
                                //  Gravity(i - 2, 0.08f);
                                B = true;
                                fw = false;
                                idl = true;
                                break;
                            }

                            if (LS[0].PE.X >= BL[i - 2].x - 30 && LS[0].PE.X <= BL[i - 2].x + 30 &&
                               LS[0].PE.Y >= BL[i - 2].y - 30 && LS[0].PE.Y <= BL[i - 2].y + 30)
                            {

                                BL.RemoveAt(i);
                                BL.RemoveAt(i - 1);
                                BL.RemoveAt(i - 2);
                                LS.Clear();
                                //Gravity(i - 2, 0.08f);
                                B = true;
                                fw = false;
                                idl = true;
                                break;
                            }
                        }

                        if (BL[i].BC == LS[0].C && BL[i - 1].BC == LS[0].C && idl == false)
                        {
                            if (LS[0].PE.X >= BL[i - 1].x - 30 && LS[0].PE.X <= BL[i - 1].x + 30 &&
                                LS[0].PE.Y >= BL[i - 1].y - 30 && LS[0].PE.Y <= BL[i - 1].y + 30)
                            {
                                BL.RemoveAt(i);
                                BL.RemoveAt(i - 1);

                                LS.Clear();

                                B = true;
                                fw = false;
                                idl = true;

                                //Gravity(i - 2, 0.08f);
                                break;
                            }

                            if (LS[0].PE.X >= BL[i].x - 30 && LS[0].PE.X <= BL[i].x + 30 &&
                                LS[0].PE.Y >= BL[i].y - 30 && LS[0].PE.Y <= BL[i].y + 30)
                            {

                                BL.RemoveAt(i);
                                BL.RemoveAt(i - 1);

                                LS.Clear();
                                B = true;
                                fw = false;
                                idl = true;
                                //Gravity(i - 2, 0.08f);

                                break;
                            }
                        }
                        // +++++++
                        /*
                                                if (i < BL.Count - 1)
                                                {
                                                    if (BL[i].BC == LS[0].C && BL[i + 1].BC == LS[0].C && idl == false)
                                                    {
                                                        if (LS[0].PE.X >= BL[i + 1].x - 30 && LS[0].PE.X <= BL[i + 1].x + 30 &&
                                                       LS[0].PE.Y >= BL[i + 1].y - 30 && LS[0].PE.Y <= BL[i + 1].y + 30)
                                                        {

                                                            BL.RemoveAt(i);
                                                            BL.RemoveAt(i + 1);

                                                            LS.Clear();
                                                            B = true;
                                                            fw = false;
                                                            idl = true;
                                                            break;
                                                        }
                                                    }
                                                }
                        */
                    }

                }
                for (int i = 0; i < BL.Count; i++)
                {
                    if (i > 2 && LS.Count > 0)
                    {

                        if (LS[0].PE.X >= BL[i].x - 30 && LS[0].PE.X <= BL[i].x + 30 &&
                            LS[0].PE.Y >= BL[i].y - 30 && LS[0].PE.Y <= BL[i].y + 30)
                        {
                            Color temp = LS[0].C;
                            Color swap;

                            for (int j = i; j < BL.Count; j++)
                            {
                                swap = temp;
                                temp = BL[j].BC;
                                BL[j].BC = swap;
                            }

                            LS.Clear();
                            B = true;
                            fw = false;
                            idl = true;
                            break;
                        }

                    }
                }


            }
            ctTick++;
            DrawDubb(this.CreateGraphics());
        }


        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            xM = e.X;
            yM = e.Y;

            double dx = e.X - Lns[0].PE.X;
            double dy = e.Y - Lns[0].PE.Y;
            th = (float)Math.Atan2(dy, dx);
            double dx1 = Lns[0].PS.X - Lns[0].PE.X;
            double dy1 = Lns[0].PS.Y - Lns[0].PE.Y;
            th1 = (float)Math.Atan2(dy1, dx1);
            float fth = th - th1;
            for (int i = 0; i < Lns.Count; i++)
            {
                Lns[i].RotateYourSelf(xmid, ymid, fth);
            }

            if (idl == true)
            {
                for (int i = 0; i < LS.Count; i++)
                {
                    LS[i].RotateYourSelf(xmid, ymid, fth);
                }
                lne.SetVals(Lns[0].PS.X, Lns[0].PS.Y, e.X, e.Y);
            }

        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (f == 0)
            {
                float x = e.X, y = e.Y;
                crv.LCtrPts.Add(new PointF(x, y));
            }
            if (e.Button == MouseButtons.Right && idl == true)
            {
                fw = true;
                PX = e.X;
                PY = e.Y;
                xs = LS[0].PE.X;
                ys = LS[0].PE.Y;
                dx = PX - LS[0].PE.X;
                dy = PY - LS[0].PE.Y;
                m = dy / dx;
                invM = dx / dy;
                idl = false;
            }


        }
        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        f = 1;
                        B = true;
                    }
                    break;
                case Keys.C:
                    {
                        Random RC = new Random();
                        LS[0].C = AColors[RC.Next(0, 3)];
                    }
                    break;
            }

            ctTick++;

        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            CreateSegs();
            xmid = (int)(Lns[4].PE.X + Lns[2].PE.X) / 2;
            ymid = (int)(Lns[4].PE.Y + Lns[2].PE.Y) / 2;
            /////////


        }

        void CreateSegs()
        {
            float ax = ClientSize.Width / 2 - 50;
            float ay = ClientSize.Height / 2 - 50;


            LineSeg pnn = new LineSeg();
            pnn.PS = new PointF(ax, ay);
            pnn.PE = new PointF(ax, ay + 50);
            Lns.Add(pnn);
            //
            pnn = new LineSeg();
            float tempY = ay + 50;
            pnn.PS = new PointF(ax, tempY);
            pnn.PE = new PointF(ax - 50, tempY + 50);
            Lns.Add(pnn);
            tempY = ay + 100;
            ///
            pnn = new LineSeg();
            float tempX = ax - 50;
            pnn.PS = new PointF(tempX, tempY);
            pnn.PE = new PointF(tempX, tempY + 50);
            Lns.Add(pnn);
            ///
            pnn = new LineSeg();
            tempY += 50;
            pnn.PS = new PointF(tempX, tempY);
            pnn.PE = new PointF(tempX + 100, tempY);
            Lns.Add(pnn);
            /////
            tempX += 100;
            pnn = new LineSeg();
            pnn.PS = new PointF(tempX, tempY);
            pnn.PE = new PointF(tempX, tempY - 50);
            Lns.Add(pnn);
            ////
            tempY -= 50;
            pnn = new LineSeg();
            pnn.PS = new PointF(tempX, tempY);
            pnn.PE = new PointF(tempX - 50, tempY - 50);
            Lns.Add(pnn);
            tempY = ay + 150;
        }

        void DrawScene(Graphics g)
        {
            g.Clear(Color.Black);
            crv.DrawYourSelf(g);

            for (int i = 0; i < Lns.Count; i++)
            {
                Lns[i].DrawYourSelf(g);
            }
            for (int i = 0; i < BL.Count; i++)
            {
                SolidBrush bb = new SolidBrush(BL[i].BC);
                g.FillEllipse(bb, BL[i].x - 15, BL[i].y - 15, 30, 30);


            }
            for (int i = 0; i < LS.Count; i++)
            {
                SolidBrush bb = new SolidBrush(LS[i].C);
                g.FillEllipse(bb, LS[i].PE.X - 15, LS[i].PE.Y - 15, 30, 30);
            }
            g.DrawLine(Pens.Yellow, Lns[0].PS.X, Lns[0].PS.Y, xM, yM);
        }
        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
        public void MoveStep()
        {
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (xs < PX)
                {
                    LS[0].PE.X += Speed;
                    LS[0].PE.Y += lne.m * Speed;
                }
                else
                {
                    LS[0].PE.X -= Speed;
                    LS[0].PE.Y -= lne.m * Speed;
                }
            }
            else
            {
                if (ys < PY)
                {
                    LS[0].PE.Y += Speed;
                    LS[0].PE.X += lne.invM * Speed;
                }
                else
                {
                    LS[0].PE.Y -= Speed;
                    LS[0].PE.X -= lne.invM * Speed;
                }
            }
        }
    }
}


