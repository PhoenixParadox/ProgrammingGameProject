using LegendOfTygydykForms.Model;
using LegendOfTygydykForms.View;
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

        public FishSpawner(World w, int p) 
        {
            Points = p;
            rnd = new Random();
            speed = 5;
            _currentWorld = w;
        }

        public void MakeFish() 
        {
            var list = _currentWorld.AccessiblePoints.Where(p => p.X != 1 && p.X != _currentWorld.worldSize.Width - 1 && p.Y != 1 && p.Y != _currentWorld.worldSize.Height - 1).ToList();
            var temp = _currentWorld.AccessiblePoints.Select(ap => _currentWorld.RelativePositionToAbs(ap)).Select(ap => new Point(ap.X + _currentWorld.tileWidth / 2, ap.Y + _currentWorld.tileWidth / 2)).ToList();
            //var dict = new Dictionary<string, Animation>();
            //dict["idle"] = new Animation(new[] { Assets.GoldCoin0, Assets.GoldCoin1 }, 0.3);
            var dict = new Dictionary<string, Animation>();
            dict["idle"] = new Animation(new[] { Assets.goldFish0, Assets.goldFish1, Assets.goldFish2, Assets.goldFish3 }, 0.45);
            var sprite = new Sprite(dict, Assets.goldFish0, layer: 0.25) { Position = temp[rnd.Next(0, list.Count)] };
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
