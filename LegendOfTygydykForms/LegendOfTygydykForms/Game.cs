using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LegendOfTygydykForms.Control;
using LegendOfTygydykForms.Model;
using LegendOfTygydykForms.View;

namespace LegendOfTygydykForms
{

//    public enum WorldChangeType
//    {
//        PointsIncreased,
//        GotHit
//    }
//    public class WorldChangeArgs 
//    {
//        public WorldChangeType ChangeType;
//    }

    public class Game
    {
        private World _currentWorld;
        public World CurrentWorld 
        {
            get { return _currentWorld; }
            private set 
            {
                _currentWorld = value;
                controller.RestartWith(_currentWorld);
            }
        }

        public delegate void WorldStateChanged();
        public delegate void PointsIncreasedHandler();
        public static event PointsIncreasedHandler PointsIncreased;
        public static event WorldStateChanged GotHit;
        public static event WorldStateChanged MouseSquished;

        public World[] worlds;
        public int _points { get { return _currentWorld.Points; } }
        public int _lives { get { return _currentWorld.lives; } }
        public HUD gameHUD;

        public Keys keyDown { get { return controller.keyDown; } set { controller.keyDown = value; } }
        public List<Sprite> toDraw { get { return controller.toDraw; } }

        private Controller controller;

        private Form1 form;

        public Game(Form1 f) 
        {
            form = f;
            worlds = new World[2];
            VisualData.Load();
            _currentWorld = new World(13, 64);
            controller = new Controller(_currentWorld, this);
            gameHUD = new HUD(this);
        }

        public void Restart() 
        {
            CurrentWorld = new World(13, 64);
            //controller = new Controller(_currentWorld, this);
        }

        public void Update(int dt) 
        {
            controller.InvokeGameTick(dt);
        }

        public void Close() 
        {
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
