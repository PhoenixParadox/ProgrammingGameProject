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
        private Point _bestScorePosition;
        private Point _ledearBoardPosition;
        private Point _titlePosition;
        private Font hudFont;
        private Font menuFont;

        public HUD(Game g) 
        {
            _pointsPosition = new Point(468, 16);
            _bestScorePosition = new Point(268, 16);
            _livesPosition = new Point(32, 16);
            _ledearBoardPosition = new Point(128, 128);
            _titlePosition = new Point(728, 128);
            _currentGame = g;
            hudFont = new Font("Microsoft Sans", 16f);
            menuFont = new Font("Microsoft Sans", 24f);
        }

        public void DrawHUD(Graphics g)
        {
            if (_currentGame.State == GameState.Playing)
            {
                g.DrawString(_currentGame._points.ToString(), hudFont, Brushes.White, _pointsPosition);
                g.DrawString("Best score: " + _currentGame.BestScore, hudFont, Brushes.White, _bestScorePosition);
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
            else if (_currentGame.State == GameState.Menu) 
            {
                var pos = _ledearBoardPosition;
                g.DrawString("Ledearboard:", menuFont, Brushes.White, new Point(pos.X - 60, pos.Y - 40));
                g.DrawString("Press Enter to start", menuFont, Brushes.White, _titlePosition);
                foreach (var d in _currentGame._gameData.TopPlayers.OrderByDescending(d => d.Score)) 
                {
                    g.DrawString(d.Score.ToString(), menuFont, Brushes.White, new Point(pos.X - 90, pos.Y));
                    g.DrawString(":", menuFont, Brushes.White, pos);
                    g.DrawString(d.Name, menuFont, Brushes.White, new Point(pos.X + 40, pos.Y));
                    pos.Y += 50;
                }
            }

        }
    }
}
