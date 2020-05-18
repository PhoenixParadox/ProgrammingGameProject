using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Model.Moving
{
    public enum Dir
    {
        Up,
        Right,
        Down,
        Left,
        None
    }

    public interface GameObject
    {
        Point Position { get; }
        Sprite sprite { get; }
        Rectangle Frame { get; }
        Dir Direction { get; }
    }
}
