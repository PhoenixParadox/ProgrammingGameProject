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
                controller?.RestartWith(_currentWorld);
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
                    CurrentWorld = new World(Worlds[currentWorld]);
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

        public List<WorldConfig> Worlds;
        private int curWrldInd;
        public int currentWorld 
        {
            get
            {
                return curWrldInd;
            }
            set 
            {
                if (value != curWrldInd) 
                {
                    curWrldInd = value;
                    CurrentWorld = new World(Worlds[curWrldInd]);
                    if ( _gameData != null && _gameData.TopPlayers != null)
                    {
                        var maxInd = 0;
                        for (int i = 0; i < _gameData.LedearboardLength; i++)
                        {
                            if (_gameData.TopPlayers[currentWorld][i].Score > _gameData.TopPlayers[currentWorld][maxInd].Score)
                            {
                                maxInd = i;
                            }
                        }
                        BestScore = _gameData.TopPlayers[currentWorld][maxInd].Score;
                    }
                }
            }
        }
        public string CurrentWorldName { get { return Worlds[currentWorld].Name; } }
        public LedearboardEntry[] CurrentTop
        {
            get
            {
                if ( _gameData != null && _gameData.TopPlayers != null) 
                {
                    return _gameData.TopPlayers[currentWorld];
                }
                return new LedearboardEntry[0];
            } 
        }

        public Game(Form1 f) 
        {
            Worlds = new List<WorldConfig>()
            {
                new WorldConfig()
                { 
                    Name = "\"BOX\"", 
                    Size = new Size(15, 15),
                    TileWidth = 64,
                    Lives = 5,
                    Couches = new List<ObstacleConfig> () 
                    { 
                        new ObstacleConfig { Position = new Point(8, 2), Orientation = ObstacleOrientation.FrontDown },
                        new ObstacleConfig { Position = new Point(8, 14), Orientation = ObstacleOrientation.FrontUp },
                        new ObstacleConfig { Position = new Point(2, 8), Orientation = ObstacleOrientation.FrontRight },
                        new ObstacleConfig { Position = new Point(14, 8), Orientation = ObstacleOrientation.FrontLeft }
                    }, 
                    Walls = new List<ObstacleConfig>() 
                },
                new WorldConfig()
                { 
                    Name = "\"CORRIDOR\"",
                    Size = new Size(20, 8), 
                    TileWidth = 64,
                    Lives = 5,
                    Couches = new List<ObstacleConfig> ()
                    { 
                        new ObstacleConfig 
                        { 
                            Position = new Point(6, 4),
                            Orientation = ObstacleOrientation.FrontDown
                        },
                        new ObstacleConfig
                        { 
                            Position = new Point(15, 4), 
                            Orientation = ObstacleOrientation.FrontDown 
                        } 
                    }, 
                    Walls = new List<ObstacleConfig>()
                },
                new WorldConfig()
                {
                    Name = "\"ARENA\"",
                    Size = new Size(15, 15),
                    TileWidth = 64,
                    Lives = 3,
                    Couches = new List<ObstacleConfig> (),
                    Walls = new List<ObstacleConfig>()
                }
            };

            form = f;
            worlds = new World[2];
            State = GameState.Menu;
            VisualData.Load();
            currentWorld = 1;
            //_currentWorld = new World(64, new Size(20, 10));
            _currentWorld = new World(Worlds[currentWorld]);
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
                    if (_gameData.TopPlayers[currentWorld][i].Score > _gameData.TopPlayers[currentWorld][maxInd].Score)
                    {
                        maxInd = i;
                    }
                }
                BestScore = _gameData.TopPlayers[currentWorld][maxInd].Score;
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
                _gameData.TopPlayers = new LedearboardEntry[Worlds.Count][];
                for (int i = 0; i < Worlds.Count; i++) 
                {
                    _gameData.TopPlayers[i] = new LedearboardEntry[5];
                }
                _gameData.TopPlayers[currentWorld][_gameData.LedearboardIndex] = new LedearboardEntry() { Name = PlayerName, Score = _points };
                _gameData.LedearboardIndex++;
                return;
            }
            for(int i = 0; i < _gameData.LedearboardLength; i++) 
            {
                if (_gameData.TopPlayers[currentWorld][i].Name == PlayerName) 
                {
                    _gameData.TopPlayers[currentWorld][i].Score = Math.Max(_points, _gameData.TopPlayers[currentWorld][i].Score);
                    return;
                }
            }
            if (_gameData.LedearboardIndex == _gameData.LedearboardLength)
            {
                var minInd = 0;
                for (int i = 0; i < _gameData.LedearboardLength; i++)
                {
                    if (_gameData.TopPlayers[currentWorld][i].Score <= _gameData.TopPlayers[currentWorld][minInd].Score)
                    {
                        minInd = i;
                    }
                }
                _gameData.TopPlayers[currentWorld][minInd] = new LedearboardEntry() { Name = PlayerName, Score = _points };
            }
            else 
            {
                _gameData.TopPlayers[currentWorld][_gameData.LedearboardIndex++] = new LedearboardEntry() { Name = PlayerName, Score = _points };
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
            //CurrentWorld = new World(64, new Size(20, 10));
            CurrentWorld = new World(Worlds[currentWorld]);
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
                    else if (keyDown == Keys.Right)
                    {
                        currentWorld = (currentWorld + 1) % Worlds.Count;
                        keyDown = default(Keys);
                    }
                    else if (keyDown == Keys.Left) 
                    {
                        currentWorld = (currentWorld == 0) ? currentWorld = Worlds.Count - 1 : currentWorld - 1;
                        keyDown = default(Keys);
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
