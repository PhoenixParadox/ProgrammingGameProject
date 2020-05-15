using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Model
{
    public class Goldfish
    {
        public int Points;
        public Point Position { get { return Sprite.Position; } }
        public Sprite Sprite;

        public Rectangle Frame { get { return Sprite.Frame; } }

        public Goldfish(Sprite s, int p)
        {
            Sprite = s;
            Points = p;
            s.PlayAnimation("idle");
        }
    }
}
