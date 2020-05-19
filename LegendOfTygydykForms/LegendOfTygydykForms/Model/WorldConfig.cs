using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Model
{
    public struct ObstacleConfig 
    {
        public Point Position;
        public Rectangle Frame;
        public ObstacleOrientation Orientation;
    }
    public class WorldConfig
    {
        public string Name;
        public Size Size;
        public int TileWidth;
        public int Lives;

        public List<ObstacleConfig> Walls;
        public List<ObstacleConfig> Couches;
    }
}
