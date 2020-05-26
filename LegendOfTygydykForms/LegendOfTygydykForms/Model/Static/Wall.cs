using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Model
{
    public class Wall : Obstacle
    {
        public override Sprite Sprite { get; set; }
        public Wall(Rectangle frame) 
        {
            this.Frame = frame;
        }
        public Wall(Rectangle frame, Sprite sprite)
        {
            this.Frame = frame;
            Sprite = sprite;
        }
    }
}
