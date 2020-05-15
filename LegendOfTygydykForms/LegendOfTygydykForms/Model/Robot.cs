using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Model
{
    public enum RobotState 
    {
        Idle,
        Chasing
    }

    public class Robot
    {
        private RobotState _state;
        public RobotState State 
        {
            get { return _state; }
            set 
            {
                //if (_state == RobotState.Chasing && value == RobotState.Idle)
                 //   Direction = Dir.Down;
                _state = value;
            }
        }
        public Sprite sprite;
        public Rectangle Frame { get { return sprite.Frame; } }

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
                        sprite.PlayAnimation("idle");
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

        public Robot(Sprite s)
        {
            sprite = s;
            speed = 5;
            State = RobotState.Idle;
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
