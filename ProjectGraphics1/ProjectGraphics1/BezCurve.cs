﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ProjectGraphics1
{
    class BezCurve
    {
        public List<PointF> LCtrPts = new List<PointF>();


        float Fact(int v)
        {
            float F = 1;
            for (int i = 2; i <= v; i++)
            {
                F *= i;
            }
            return F;
        }
        public PointF CalcCurvePointAtTime(float t)
        {
            PointF P = new PointF();
            int n = LCtrPts.Count - 1;
            float C;
            for (int i = 0; i < LCtrPts.Count; i++)
            {
                C = Fact(n) / (Fact(i) * Fact(n - i));
                P.X += (float)(Math.Pow(t, i) * Math.Pow(1 - t, n - i) * C * LCtrPts[i].X);
                P.Y += (float)(Math.Pow(t, i) * Math.Pow(1 - t, n - i) * C * LCtrPts[i].Y);
            }

            return P;
        }
        public void DrawYourSelf(Graphics g)
        {

            for (float t = 0; t <= 1; t += 0.001f)
            {
                PointF P = CalcCurvePointAtTime(t);
                g.FillEllipse(Brushes.Red, P.X, P.Y, 3, 3);
            }
        }

    }
}

