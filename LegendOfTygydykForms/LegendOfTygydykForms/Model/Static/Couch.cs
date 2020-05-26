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
        public override Sprite Sprite { get; set; }
        private bool _hasCat;
        public bool HasCat 
        {
            get { return _hasCat; }
            set 
            {
                _hasCat = value;
                if (_hasCat)
                    Sprite.PlayAnimation("hasCat");
                else
                    Sprite.PlayAnimation("empty");
            }
        }
        public Couch(Sprite s, Point pos, ObstacleOrientation or, int width = 384, int height = 128) 
        {
            Position = pos;
            Orientation = or;
            //couchSize = new Size(width, height);
            
            Sprite = s;
            Sprite.Position = pos;
            switch (or) 
            {
                case (ObstacleOrientation.FrontLeft):
                    Sprite.RotateSprite(SpriteRotate.Rotate90);
                    break;
                case (ObstacleOrientation.FrontUp):
                    Sprite.RotateSprite(SpriteRotate.Rotate180);
                    break;
                case (ObstacleOrientation.FrontRight):
                    Sprite.RotateSprite(SpriteRotate.Rotate270);
                    break;
            }

            Frame = Sprite.Frame;
        }
    }
}
