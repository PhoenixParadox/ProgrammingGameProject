
using LegendOfTygydykForms.Model.Moving;
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

    public class Cat : GameObject
    {
        private CatState _state;
        private Dir _dir;

        public Sprite sprite { get; private set; }

        #region exp
        public Rectangle Frame { get { return new Rectangle(CornerX, CornerY, Width, Height); } }

        // coordinates of upper left corner
        private int CornerX { get { return Position.X - Offset.X; } }
        private int CornerY { get { return Position.Y - Offset.Y; } }
        public Point Offset { get { return new Point(Width / 2, Height / 2); } }
        public int Width { get { return sprite.Width - (speed); } }
        public int Height { get { return sprite.Height - (speed); } }
        #endregion

        //public Rectangle Frame { get { return new Rectangle(sprite.Frame.X + speed, sprite.Frame.Y + speed, sprite.Frame.Width - speed * 2, sprite.Frame.Height - speed * 2); } }

        //public Rectangle Frame { get { return sprite.Frame; } }
        public Point Position { get { return sprite.Position; } }
        public int speed { get; private set; } // px/tick
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

        public Cat(Sprite s)
        {
            sprite = s;
            speed = 7;
            State = CatState.Idle;
        }

        public void Stop() 
        {
            sprite.StopAnimation();
        }

        public void UpdatePosition(Point pos)
        {
            sprite.Position = pos;
        }

        public void UpdatePosition(int dx, int dy) 
        {
            sprite.Position.X += dx;
            sprite.Position.Y += dy;
        }
    }
}
