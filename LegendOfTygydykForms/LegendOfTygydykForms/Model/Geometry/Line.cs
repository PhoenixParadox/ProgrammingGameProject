using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Model.Geometry
{
    public class Line
    {
        public double A;
        public double B;
        public double C;

        public Line(Point p1, Point p2)
        {
            if (p1.X == p2.X)
            {
                A = 1;
                B = 0;
                C = -p1.X;
            }
            else 
            {
                A = Math.Atan((p2.Y - p1.Y) / (p1.X - p2.X));
                B = -1;
                C = p1.Y - A * p1.X;
            }
        }

        public double DistTo(Point p) 
        {
            return Math.Abs(A * p.X + B * p.Y + C) / Math.Sqrt(A * A + B * B);
        }
    }
}
