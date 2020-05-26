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
    public class MouseSpawner
    {
        private Random rnd;
        private Point[] spawnPoints;
        private World _currentWorld;
        public double _timer; //ms
        public double _respawnRate; //sec

        public MouseSpawner(World w, double respawnRate) 
        {
            _currentWorld = w;
            rnd = new Random();
            spawnPoints = new Point[4];
            var list = _currentWorld.AccessiblePoints.Except(_currentWorld.AccessiblePoints.Where(p => p.X == 1 || p.X == _currentWorld.worldSize.Width - 1 || p.Y == 1 || p.Y == _currentWorld.worldSize.Height - 1)).ToList();
            var temp = _currentWorld.AccessiblePoints.Select(ap => _currentWorld.RelativePositionToAbs(ap)).Select(ap => new Point(ap.X + _currentWorld.tileWidth / 2, ap.Y + _currentWorld.tileWidth / 2)).ToList();
            for (int i = 0; i < 4; i++) 
            {
                spawnPoints[i] = temp[rnd.Next(0, list.Count)];
            }
            _respawnRate = respawnRate;
        }

        public void SpawnMouse() 
        {
            _currentWorld.Mouse = new Mouse(new Sprite(VisualData._mouseAnimations, Assets.mouseUp) { Position = spawnPoints[rnd.Next(0, 3)]});
        }

        public void Update(int dt)
        {
            if (_currentWorld.Mouse == null) 
            {
                SpawnMouse();
                _timer = 0;
            }
            _timer += dt;
        }

    }
}
