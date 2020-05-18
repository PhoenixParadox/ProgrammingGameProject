using LegendOfTygydykForms.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Control
{
    public class FishSpawner
    {
        public World _currentWorld;
        public Bitmap _fishTxtr;
        private Random rnd;
        private int Points;
        private int _timer;
        private int speed; // sec

        public FishSpawner(World w, Bitmap b, int p) 
        {
            _fishTxtr = b;
            Points = p;
            rnd = new Random();
            speed = 5;
            _currentWorld = w;
        }

        public void MakeFish() 
        {
            var list = _currentWorld.AccessiblePoints.Where(p => p.X != 1 && p.X != _currentWorld.worldSize.Width - 1 && p.Y != 1 && p.Y != _currentWorld.worldSize.Height - 1).ToList();
            var dict = new Dictionary<string, Animation>();
            dict["idle"] = new Animation(new[] { Assets.GoldCoin0, Assets.GoldCoin1 }, 0.3);
            var sprite = new Sprite(dict, _fishTxtr) { Position = _currentWorld.RelativePositionToAbs(list[rnd.Next(0, list.Count)]) };
            _currentWorld.fishes.Add( new Goldfish(sprite, Points));
        }

        public void Update(int dt) 
        {
            _timer += dt;
            if (_timer / 1000.0 >= speed) 
            {
                MakeFish();
                _timer = 0;
            }
        }
    }
}
