using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Model
{
    public enum ObstacleOrientation 
    {
        FrontDown,
        FrontUp,
        FrontRight,
        FrontLeft
    }
    public abstract class Obstacle
    {
        public Rectangle Frame;
        public ObstacleOrientation Orientation;
    }
}
