using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Model
{
    public class Couch : Obstacle
    {
        public Point Position;
        //public ObstacleOrientation orientation;
        //private readonly Size couchSize;
        public Sprite sprite;
        private bool _hasCat;
        public bool HasCat 
        {
            get { return _hasCat; }
            set 
            {
                _hasCat = value;
                if (_hasCat)
                    sprite.PlayAnimation("hasCat");
                else
                    sprite.PlayAnimation("empty");
            }
        }
        public Couch(Sprite s, Point pos, ObstacleOrientation or, int width = 384, int height = 128) 
        {
            Position = pos;
            Orientation = or;
            //couchSize = new Size(width, height);
            
            sprite = s;
            sprite.Position = pos;
            switch (or) 
            {
                case (ObstacleOrientation.FrontLeft):
                    sprite.RotateSprite(SpriteRotate.Rotate90);
                    break;
                case (ObstacleOrientation.FrontUp):
                    sprite.RotateSprite(SpriteRotate.Rotate180);
                    break;
                case (ObstacleOrientation.FrontRight):
                    sprite.RotateSprite(SpriteRotate.Rotate270);
                    break;
            }

            Frame = sprite.Frame;
        }
    }
}
