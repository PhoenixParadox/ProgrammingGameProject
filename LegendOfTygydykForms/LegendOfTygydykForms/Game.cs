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
        Menu,
        Pause,
        Shop
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
                    CurrentWorld = new World(Worlds[currentWorld], this);
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
        private Form3 shopForm;
        private GameState _state;
        public ShopController _shopController;
        private double _shopRegisterTimer;
        private const double _shopRegisterRate = 0.2;

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
                    CurrentWorld = new World(Worlds[curWrldInd], this);
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
                },
                new WorldConfig()
                {
                    Name = "\"MAZE\"",
                    Size = new Size(15, 10),
                    TileWidth = 64,
                    Lives = 4,
                    Couches = new List<ObstacleConfig>(),
                    Walls = new List<ObstacleConfig>()
                    {
                        new ObstacleConfig(){ Position = new Point(3, 3)},
                        new ObstacleConfig(){ Position = new Point(3, 2)},
                        new ObstacleConfig(){ Position = new Point(2, 3)},
                        new ObstacleConfig(){ Position = new Point(2, 7)},
                        new ObstacleConfig(){ Position = new Point(3, 7)},
                        new ObstacleConfig(){ Position = new Point(2, 8)},
                        new ObstacleConfig(){ Position = new Point(5, 5)},
                        new ObstacleConfig(){ Position = new Point(5, 6)},
                        new ObstacleConfig(){ Position = new Point(5, 7)},
                        new ObstacleConfig(){ Position = new Point(5, 8)},
                        new ObstacleConfig(){ Position = new Point(6, 5)},
                        new ObstacleConfig(){ Position = new Point(6, 1)},
                        new ObstacleConfig(){ Position = new Point(7, 5)},
                        new ObstacleConfig(){ Position = new Point(8, 5)},
                        new ObstacleConfig(){ Position = new Point(9, 5)},
                        new ObstacleConfig(){ Position = new Point(10, 5)},
                        new ObstacleConfig(){ Position = new Point(7, 8)},
                        new ObstacleConfig(){ Position = new Point(8, 7)},
                        new ObstacleConfig(){ Position = new Point(8, 8)},
                        new ObstacleConfig(){ Position = new Point(9, 8)},
                        new ObstacleConfig(){ Position = new Point(10, 8)},
                        new ObstacleConfig(){ Position = new Point(5, 3)},
                        new ObstacleConfig(){ Position = new Point(6, 3)},
                        new ObstacleConfig(){ Position = new Point(7, 3)},
                        new ObstacleConfig(){ Position = new Point(12, 4)},
                        new ObstacleConfig(){ Position = new Point(12, 1)},
                        new ObstacleConfig(){ Position = new Point(12, 3)},
                        new ObstacleConfig(){ Position = new Point(13, 9)},
                        new ObstacleConfig(){ Position = new Point(13, 8)}
                    }
                },
                new WorldConfig()
                {
                    Name = "\"TWO ROOMS\"",
                    Size = new Size(13, 13),
                    TileWidth = 64,
                    Lives = 4,
                    Couches = new List<ObstacleConfig>()
                    {
                        new ObstacleConfig
                        {
                            Position = new Point(2, 4),
                            Orientation = ObstacleOrientation.FrontRight
                        },
                        new ObstacleConfig
                        {
                            Position = new Point(12, 10),
                            Orientation = ObstacleOrientation.FrontLeft
                        }
                    },
                    Walls = new List<ObstacleConfig>()
                    {
                        new ObstacleConfig(){ Position = new Point(6, 1)},
                        new ObstacleConfig(){ Position = new Point(6, 2)},
                        new ObstacleConfig(){ Position = new Point(6, 3)},
                        new ObstacleConfig(){ Position = new Point(6, 4)},
                        new ObstacleConfig(){ Position = new Point(6, 9)},
                        new ObstacleConfig(){ Position = new Point(6, 10)},
                        new ObstacleConfig(){ Position = new Point(6, 11)},
                        new ObstacleConfig(){ Position = new Point(6, 12)},
                        new ObstacleConfig(){ Position = new Point(7, 1)},
                        new ObstacleConfig(){ Position = new Point(7, 2)},
                        new ObstacleConfig(){ Position = new Point(7, 3)},
                        new ObstacleConfig(){ Position = new Point(7, 4)},
                        new ObstacleConfig(){ Position = new Point(7, 9)},
                        new ObstacleConfig(){ Position = new Point(7, 10)},
                        new ObstacleConfig(){ Position = new Point(7, 11)},
                        new ObstacleConfig(){ Position = new Point(7, 12)}
                    }
                },
                new WorldConfig()
                {
                    Name = "\"BOX IN A BOX\"",
                    Size = new Size(16, 15),
                    TileWidth = 64,
                    Lives = 4,
                    Couches = new List<ObstacleConfig>()
                    {
                        new ObstacleConfig
                        {
                            Position = new Point(6, 8),
                            Orientation = ObstacleOrientation.FrontRight
                        },
                        new ObstacleConfig
                        {
                            Position = new Point(11, 8),
                            Orientation = ObstacleOrientation.FrontLeft
                        }
                    },
                    Walls = new List<ObstacleConfig>()
                    {
                        new ObstacleConfig(){ Position = new Point(4, 9)},
                        new ObstacleConfig(){ Position = new Point(4, 10)},
                        new ObstacleConfig(){ Position = new Point(4, 11)},
                        new ObstacleConfig(){ Position = new Point(4, 8)},
                        new ObstacleConfig(){ Position = new Point(4, 7)},
                        new ObstacleConfig(){ Position = new Point(4, 6)},
                        new ObstacleConfig(){ Position = new Point(4, 5)},
                        new ObstacleConfig(){ Position = new Point(4, 4)},
                        new ObstacleConfig(){ Position = new Point(5, 4)},
                        new ObstacleConfig(){ Position = new Point(6, 4)},
                        new ObstacleConfig(){ Position = new Point(10, 4)},
                        new ObstacleConfig(){ Position = new Point(11, 4)},
                        new ObstacleConfig(){ Position = new Point(12, 4)},
                        new ObstacleConfig(){ Position = new Point(12, 5)},
                        new ObstacleConfig(){ Position = new Point(12, 6)},
                        new ObstacleConfig(){ Position = new Point(12, 7)},
                        new ObstacleConfig(){ Position = new Point(12, 8)},
                        new ObstacleConfig(){ Position = new Point(12, 9)},
                        new ObstacleConfig(){ Position = new Point(12, 10)},
                        new ObstacleConfig(){ Position = new Point(12, 11)},
                        new ObstacleConfig(){ Position = new Point(5, 11)},
                        new ObstacleConfig(){ Position = new Point(6, 11)},
                        new ObstacleConfig(){ Position = new Point(10, 11)},
                        new ObstacleConfig(){ Position = new Point(11, 11)}
                    }
                }
            };

            #region game data
            _gameData = new GameData();
            _dataManager = new DataManager(this);
            if (_gameData.TopPlayers != null && _gameData.TopPlayers.GetLength(0) != Worlds.Count)
            {
                _gameData.TopPlayers = null;
                _gameData.LedearboardIndex = 0;
            }
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
            #endregion


            form = f;
            State = GameState.Menu;
            VisualData.Load();
            currentWorld = 1;
            _currentWorld = new World(Worlds[currentWorld], this);
            controller = new Controller(_currentWorld, this);
            _shopController = new ShopController(this);
            PlayerName = "";
            gameHUD = new HUD(this, form);



            GameLost += ShowGameLostDialog;
        }

        public void VisitShop() 
        {
            shopForm = new Form3(this);
            State = GameState.Shop;
            //shopForm.game = this;
            shopForm.FormClosed += ReturnToMenu;
            shopForm.Focus();
            shopForm.ShowDialog();
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
                if (_gameData.LedearboardIndex == _gameData.TopPlayers[currentWorld].Length) 
                {
                    _gameData.LedearboardIndex = 0;
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
            CurrentWorld = new World(Worlds[currentWorld], this);
        }
        public void Update(int dt) 
        {
            _shopRegisterTimer += dt;
            switch (State) 
            {
                case (GameState.Pause):
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
                    else if (keyDown == Keys.S) 
                    {
                        keyDown = default(Keys);
                        if (_shopRegisterTimer >= _shopRegisterRate) 
                        {
                            _shopRegisterTimer = 0;
                            VisitShop();
                        }
                        break;
                    }
                    break;
            }
        }
        public void Pause() 
        {
            State = GameState.Pause;
        }
        public void Unpause() 
        {
            State = GameState.Playing;
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
