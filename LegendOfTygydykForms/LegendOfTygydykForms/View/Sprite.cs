using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Model
{
    public enum SpriteRotate 
    {
        Rotate90,
        Rotate180,
        Rotate270,
    }

    public class Sprite
    {
        private Dictionary<string, Animation> animations;
        private Bitmap texture;

        public Animation currentAnimation;
        /// <summary>
        /// Position of the center of the sprite.
        /// </summary>
        public Point Position;
        public double Layer;

        public Rectangle Frame { get { return new Rectangle(CornerX, CornerY, Width, Height); } }

        // coordinates of upper left corner
        private int CornerX { get { return Position.X - Offset.X; } }
        private int CornerY { get { return Position.Y - Offset.Y; } }
        public Point Offset { get { return new Point(Width / 2, Height / 2); } }
        public int Width { get { return texture.Width; } }
        public int Height { get { return texture.Height; } }

        public Bitmap currentFrame 
        {
            get 
            {
                if (currentAnimation == null)
                    return texture;
                return currentAnimation.CurrentFrame;
            }
        }

        public Sprite(Bitmap b, double layer = 0.0) 
        {
            texture = b;
            Layer = layer;
        }

        public Sprite(Bitmap b, Point position, double layer = 0.0) 
        {
            texture = b;
            Position = new Point(position.X + Offset.X, position.Y + Offset.Y);
            Layer = layer;
        }

        public Sprite(Dictionary<string, Animation> d, Bitmap b, double layer = 0.0) 
        {
            animations = d;
            texture = b;
            Layer = layer;
        }

        public void RotateSprite(SpriteRotate flip) 
        {
            switch (flip) 
            {
                case (SpriteRotate.Rotate90):
                    texture.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    foreach (var a in animations.Values)
                        foreach (var f in a.frames)
                            f.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case (SpriteRotate.Rotate180):
                    texture.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    foreach (var a in animations.Values)
                        foreach (var f in a.frames)
                            f.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case (SpriteRotate.Rotate270):
                    texture.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    foreach (var a in animations.Values)
                        foreach (var f in a.frames)
                            f.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }
        }

        public void PlayAnimation(string animationName) 
        {
            try
            {
                currentAnimation = animations[animationName];
            }
            catch 
            {
                return;
            }
        }

        public void StopAnimation() 
        {
            currentAnimation = null;
        }

        public void Update(int dt) 
        {
            if (currentAnimation != null) 
            {
                currentAnimation._timer += dt;
                if (currentAnimation._timer / 1000.0 >= currentAnimation.speed) 
                {
                    currentAnimation.Update();
                    currentAnimation._timer = 0;
                }
            }
        }
    }
}
