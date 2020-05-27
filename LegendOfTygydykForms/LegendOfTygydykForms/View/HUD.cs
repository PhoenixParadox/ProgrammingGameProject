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
        private Point _levelNamePosition;
        private Point _pausePosition;
        private Point _fishesPosition;
        private Point _menuFishesPosition;
        private Font hudFont;
        private Font menuFont;
        private Form1 Form;

        public HUD(Game g, Form1 form) 
        {
            Form = form;
            _pointsPosition = new Point(468, 16);
            _bestScorePosition = new Point(268, 16);
            _livesPosition = new Point(32, 16);
            _ledearBoardPosition = new Point(128, 128);
            _titlePosition = new Point(728, 128);
            _levelNamePosition = new Point(_titlePosition.X + 30, _titlePosition.Y + 100);
            _fishesPosition = new Point(550, 16);
            _menuFishesPosition = new Point(256, 512);
            _currentGame = g;
            hudFont = new Font("Microsoft Sans", 16f);
            menuFont = new Font("Microsoft Sans", 24f);
        }

        public void DrawHUD(Graphics g)
        {
            if (_currentGame.State == GameState.Playing || _currentGame.State == GameState.Pause)
            {
                g.DrawImage(Assets.fishIcon, _fishesPosition);
                g.DrawString(_currentGame._gameData.Fishes.ToString(), hudFont, Brushes.White, new Point(_fishesPosition.X + 64, _fishesPosition.Y));
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
                if (_currentGame.State == GameState.Pause)
                {
                    _pausePosition = new Point(_currentGame.DesiredWindowSize.Width / 2, _currentGame.DesiredWindowSize.Height / 2);
                    _pausePosition.X -= Assets.pauseIcon.Width / 2;
                    _pausePosition.Y -= Assets.pauseIcon.Height / 2;
                    g.DrawImage(Assets.pauseIcon, _pausePosition);
                }
            }
            else if (_currentGame.State == GameState.Menu)
            {
                var pos = _ledearBoardPosition;
                g.DrawImage(Assets.fishIcon, _menuFishesPosition);
                g.DrawString("YOU HAVE:", menuFont, Brushes.White, new Point(_menuFishesPosition.X - 200, _menuFishesPosition.Y));
                g.DrawString("PRESS S TO VISIT SHOP", menuFont, Brushes.White, new Point(_menuFishesPosition.X - 200, _menuFishesPosition.Y + 64));
                g.DrawString(_currentGame._gameData.Fishes.ToString(), menuFont, Brushes.White, new Point(_menuFishesPosition.X + 64, _menuFishesPosition.Y));
                g.DrawString("Ledearboard:", menuFont, Brushes.White, new Point(pos.X - 60, pos.Y - 40));
                g.DrawString("Press Enter to start", menuFont, Brushes.White, _titlePosition);
                g.DrawString("Use arrows to choose level", menuFont, Brushes.White, new Point(_titlePosition.X - 60, _titlePosition.Y + 145));
                g.DrawString(_currentGame.CurrentWorldName, menuFont, Brushes.White, _levelNamePosition);
                g.DrawString("<", menuFont, Brushes.White, new Point(_levelNamePosition.X - 60, _levelNamePosition.Y));
                g.DrawString(">", menuFont, Brushes.White, new Point(_levelNamePosition.X + 260, _levelNamePosition.Y));
                foreach (var d in _currentGame.CurrentTop.OrderByDescending(d => d.Score))
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
