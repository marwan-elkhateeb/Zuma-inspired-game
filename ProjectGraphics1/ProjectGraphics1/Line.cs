using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ProjectGraphics1
{
    class Line
    {
        public float xs, ys, xe, ye, dx, dy, m, invM, currX, currY;
        int Speed = 5;

        public void SetVals(float a, float b, float c, float d)
        {
            xs = a;
            ys = b;
            xe = c;
            ye = d;
            //////////////////
            dx = xe - xs;
            dy = ye - ys;
            m = dy / dx;
            invM = dx / dy;
            /////////////////
            currX = xs;
            currY = ys;
        }


        public void MoveStep()
        {
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (xs < xe)
                {
                    currX += Speed;
                    currY += m * Speed;
                }
                else
                {
                    currX -= Speed;
                    currY -= m * Speed;
                }
            }
            else
            {
                if (ys < ye)
                {
                    currY += Speed;
                    currX += invM * Speed;
                }
                else
                {
                    currY -= Speed;
                    currX -= invM * Speed;
                }
            }
        }

        public void DrawYourCurrPos(Graphics g)
        {
            g.FillEllipse(Brushes.Yellow, currX - 5, currY - 5, 10, 10);
            g.DrawLine(Pens.Yellow, xs, ys, xe, ye);
        }
    }
}

