using LegendOfTygydykForms.Control;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Model.Geometry
{
    public class Circle
    {
        public Point Position { get; set; }
        public int Radius { get; set; }

        public Circle(Point pos, int rad) 
        {
            Radius = rad;
            Position = pos;
        }
        public bool IntersectsWith(Circle another) 
        {
            var centerDist = Controller.DistBetween(Position, another.Position);
            return centerDist < Radius + another.Radius;
        }

        public bool IntersectsWith(Rectangle rectangle) 
        {
            var cond1 = rectangle.Contains(Position);
            var vertices = new Point[4];
            vertices[0] = new Point(rectangle.Left, rectangle.Top);
            vertices[1] = new Point(rectangle.Right, rectangle.Top);
            vertices[2] = new Point(rectangle.Right, rectangle.Bottom);
            vertices[3] = new Point(rectangle.Left, rectangle.Bottom);
            var cond2 = false;
            cond2 = new Line(vertices[0], vertices[1]).DistTo(Position) <= Radius;
            cond2 = new Line(vertices[1], vertices[2]).DistTo(Position) <= Radius;
            cond2 = new Line(vertices[2], vertices[3]).DistTo(Position) <= Radius;
            cond2 = new Line(vertices[3], vertices[0]).DistTo(Position) <= Radius;
            return cond1 || cond2;
        }
    }
}
