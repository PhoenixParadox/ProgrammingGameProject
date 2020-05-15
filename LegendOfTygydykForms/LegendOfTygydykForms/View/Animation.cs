using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Model
{
    public class Animation
    {
        public Bitmap[] frames;
        public double speed; //sec
        private int _currentFrame;
        public int _timer;

        public Bitmap CurrentFrame { get { return frames[_currentFrame]; } }

        public Animation(Bitmap[] f, double s) 
        {
            frames = f;
            speed = s;
        }

        public void Update() 
        {
            _currentFrame = (_currentFrame + 1) % frames.Length;
        }
    }
}
