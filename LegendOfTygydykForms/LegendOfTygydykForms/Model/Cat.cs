using LegendOfTygydykForms.Model.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Model
{
    public enum CatState 
    {
        Idle,
        Hidden,
        Invincible
    }

    public class Cat
    {
        //public Circle solidFrame { get { return new Circle(sprite.Position, (sprite.Width - 2) / 2); } }

        public Sprite sprite;
        public Rectangle Frame { get { return sprite.Frame; } }
        public Point Position { get { return sprite.Position; } }
        private CatState _state;
        public CatState State
        {
            get { return _state; }
            set 
            {
                _state = value;
                if (_state == CatState.Invincible)
                    sprite.PlayAnimation("invincible");
            }
        }
        public bool isHidden;
        private Dir _dir;
        public Dir Direction
        {
            get { return _dir; }
            set
            {
                _dir = value;
                if (State != CatState.Invincible) 
                {
                    switch (_dir)
                    {
                        case (Dir.Down):
                            sprite.StopAnimation();
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
        }
        public int speed; // px/tick

        public Cat(Sprite s)
        {
            sprite = s;
            speed = 5;
            State = CatState.Idle;
            //frame = new Rectangle(sprite.CorenrX, sprite.CornerY, sprite.Width, sprite.Height);
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
