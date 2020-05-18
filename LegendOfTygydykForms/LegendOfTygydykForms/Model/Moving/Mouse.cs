using LegendOfTygydykForms.Model.Moving;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Model
{
    public class Mouse : GameObject
    {
        public Sprite sprite { get; }
        public Rectangle Frame { get { return sprite.Frame; } }
        public Point Position { get { return sprite.Position; } }

        public bool isHidden;
        private Dir _dir;
        public Dir Direction
        {
            get { return _dir; }
            set
            {
                _dir = value;
                switch (_dir)
                {
                        case (Dir.Down):
                            sprite.PlayAnimation("moveDown");
                            break;
                        case (Dir.Left):
                            sprite.PlayAnimation("moveLeft");
                            break;
                        case (Dir.None):
                            sprite.StopAnimation();
                            break;
                        case (Dir.Right):
                            sprite.PlayAnimation("moveRight");
                            break;
                        case (Dir.Up):
                            sprite.PlayAnimation("moveUp");
                            break;
                    }
            }
        }
        public int speed; // px/tick

        public Mouse(Sprite s)
        {
            sprite = s;
            speed = 5;
        }

        public void Stop()
        {
            sprite.StopAnimation();
        }

        public void UpdatePosition(Point pos)
        {
            sprite.Position = pos;
        }
    }
}

