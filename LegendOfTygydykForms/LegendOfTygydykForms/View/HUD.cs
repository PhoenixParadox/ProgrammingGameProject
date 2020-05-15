using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.View
{
    public class HUD
    {
        public Game _currentGame;
        private Point _pointsPosition;
        private Point _livesPosition;
        private Font hudFont;

        public HUD(Game g) 
        {
            _pointsPosition = new Point(768, 32);
            _livesPosition = new Point(32, 16);
            _currentGame = g;
            hudFont = SystemFonts.CaptionFont;
        }

        public void DrawHUD(Graphics g)
        {
            g.DrawString(_currentGame._points.ToString(), hudFont, Brushes.White, _pointsPosition);
            if (_currentGame._lives > 0) 
            {
                var pos = _livesPosition;
                for (int i = _livesPosition.X; i <= _currentGame._lives * Assets.CatSiluete.Width; i += (Assets.CatSiluete.Width))
                {
                    g.DrawImage(Assets.CatSiluete, pos);
                    pos.X += Assets.CatSiluete.Width;
                }
            }
        }
    }
}
