using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LegendOfTygydykForms.Control;
using LegendOfTygydykForms.Model;
using LegendOfTygydykForms.View;

namespace LegendOfTygydykForms
{
    public enum GameState 
    {
        Playing,
        Loss,
        Menu
    }

    public class Game
    {
        public GameData _gameData;
        public World CurrentWorld 
        {
            get { return _currentWorld; }
            private set 
            {
                _currentWorld = value;
                controller.RestartWith(_currentWorld);
            }
        }

        #region events
        public static event Action PointsIncreased;
        public static event Action GotHit;
        public static event Action MouseSquished;
        public static event Action GameStateChanged;
        public static event Action GameLost;
        #endregion

        public World[] worlds;
        public HUD gameHUD;
        public string PlayerName;
        public int BestScore;

        public int _points { get { return _currentWorld.Points; } }
        public int _lives { get { return _currentWorld.lives; } }
        public int _currentWorldTileSize { get { return _currentWorld.tileWidth; } }
        public Size _worldSize { get { return _currentWorld.worldSize; } }
        public Size DesiredWindowSize 
        {
            get 
            {
                switch (State) 
                {
                    
                    case (GameState.Menu):
                        return new Size(VisualData._menuBackground.Width, VisualData._menuBackground.Height);
                        break;
                    case (GameState.Playing):
                    default:
                        return new Size((_worldSize.Width + 1) * _currentWorldTileSize, (_worldSize.Height + 1) * _currentWorldTileSize);
                }
            }
        }
        public Keys keyDown 
        {
            get
            {
                switch (State) 
                {
                        
                    case (GameState.Menu):
                        return _keyDown;
                        break;
                    case (GameState.Playing):
                    default:
                        return controller.keyDown;
                        break;
                }
            }
            set 
            {
                switch (State)
                {
                    case (GameState.Menu):
                        _keyDown = value;
                        break;
                    case (GameState.Playing):
                    default:
                        controller.keyDown = value;
                        break;
                }
            }
        }
        public List<Sprite> toDraw { get { return controller.toDraw; } }
        public GameState State 
        {
            get { return _state; }
            private set
            {
                if (_state == GameState.Menu && value == GameState.Playing)
                {
                    CurrentWorld = new World(64, new Size(20, 10));
                }
                else if (_state == GameState.Playing && value == GameState.Loss) 
                {
                    GameLost?.Invoke();
                }
                _state = value;
                GameStateChanged?.Invoke();
            }
        }

        private DataManager _dataManager;
        private Controller controller;
        private World _currentWorld;
        private Keys _keyDown;
        private Form1 form;
        private Form2 dialogForm;
        private GameState _state;

        public Game(Form1 f) 
        {
            form = f;
            worlds = new World[2];
            State = GameState.Menu;
            VisualData.Load();
            _currentWorld = new World(64, new Size(20, 10));
            controller = new Controller(_currentWorld, this);
            worlds[0] = _currentWorld;
            PlayerName = "";
            gameHUD = new HUD(this);

            _gameData = new GameData();
            _dataManager = new DataManager(this);
            if (_gameData.TopPlayers != null) 
            {
                var maxInd = 0;
                for (int i = 0; i < _gameData.LedearboardLength; i++) 
                {
                    if (_gameData.TopPlayers[i].Score > _gameData.TopPlayers[maxInd].Score) 
                    {
                        maxInd = i;
                    }
                }
                BestScore = _gameData.TopPlayers[maxInd].Score;
            }

            GameLost += ShowGameLostDialog;
        }
        private void ShowGameLostDialog()
        {
            dialogForm = new Form2();
            dialogForm.game = this;
            dialogForm.FormClosed += ReturnToMenu;
            dialogForm.Show();
        }
        private void ReturnToMenu(object sender, FormClosedEventArgs e)
        {
            UpdateLedearboard();
            State = GameState.Menu;
        }
        private void UpdateLedearboard() 
        {
            if (_gameData.TopPlayers == null) 
            {
                _gameData.TopPlayers = new LedearboardEntry[5];
                _gameData.TopPlayers[_gameData.LedearboardIndex] = new LedearboardEntry() { Name = PlayerName, Score = _points };
                _gameData.LedearboardIndex++;
                return;
            }
            for(int i = 0; i < _gameData.LedearboardLength; i++) 
            {
                if (_gameData.TopPlayers[i].Name == PlayerName) 
                {
                    _gameData.TopPlayers[i].Score = _points;
                    return;
                }
            }
            if (_gameData.LedearboardIndex == 5)
            {
                var minInd = 0;
                for (int i = 0; i < _gameData.LedearboardLength; i++)
                {
                    if (_gameData.TopPlayers[i].Score <= _gameData.TopPlayers[minInd].Score)
                    {
                        minInd = i;
                    }
                }
                _gameData.TopPlayers[minInd] = new LedearboardEntry() { Name = PlayerName, Score = _points };
            }
            else 
            {
                _gameData.TopPlayers[_gameData.LedearboardIndex++] = new LedearboardEntry() { Name = PlayerName, Score = _points };
            }
        }
        public void GameOver() 
        {
            if (keyDown == Keys.Q) 
            {
                State = GameState.Menu;
                return;
            }
            State = GameState.Loss;
        }
        public void Restart() 
        {
            CurrentWorld = new World(64, new Size(20, 10));
        }
        public void Update(int dt) 
        {
            switch (State) 
            {
                case (GameState.Playing):
                        controller.InvokeGameTick(dt);
                        break;
                case (GameState.Menu):
                    if (keyDown == Keys.Enter)
                    {
                        keyDown = default(Keys);
                        State = GameState.Playing;
                    }
                    else if (keyDown == Keys.Escape) 
                    {
                        Close();
                    }
                    break;
            }
        }
        public void Close() 
        {
            _dataManager.Save();
            form.Close();
        }
        public static void InvokePointsIncreased() 
        {
            PointsIncreased?.Invoke();
        }
        public static void InvokeGotHit() 
        {
            GotHit?.Invoke();
        }
        public static void InvokeMouseSquished()
        {
            MouseSquished?.Invoke();
        }
    }
}
