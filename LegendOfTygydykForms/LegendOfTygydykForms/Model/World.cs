using LegendOfTygydykForms.Control;
using LegendOfTygydykForms.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LegendOfTygydykForms.Model
{
    public enum Dir
    {
        Up,
        Right,
        Down,
        Left,
        None
    }

    public class World
    {
        public Cat cat;
        private int _latestTrail;
        private int _trailLength;
        public Queue<Point> trail;

        public Mouse Mouse;
        public MouseSpawner MouseSpawner;
        public Queue<Sprite> SquishedMice;

        public List<Robot> robots;
        public List<Point> robotPositions { get { return robots.Select(r => AbsPositionToRelaive(new Point(r.Frame.X, r.Frame.Y))).ToList(); } }
        public int mapSize; // in tiles
        public List<Point> mapPoints 
        {
            get 
            {
                var res = new List<Point>();
                for (int i = 1; i < mapSize; i++) 
                {
                    for (int j = 1; j < mapSize; j++) 
                    {
                        res.Add(new Point(i, j));
                    }
                }
                return res;
            }
        }
        public int tileWidth;
        public Point robotSpawn;

        public List<Point> AccessiblePoints { get { return mapPoints.Except(robotPositions).Except(obstaclesPositions).ToList(); } }
        public List<Obstacle> obstacles;
        public List<Rectangle> obstaclesFrames { get { return obstacles.Where(o => o is Couch).Select(o => o.Frame).ToList(); } }
        public List<Point> obstaclesPositions 
        {
            get 
            {
                var res = new List<Point>();
                for (int i = tileWidth; i < mapSize * tileWidth; i += tileWidth)
                {
                    for (int j = tileWidth; j < mapSize * tileWidth; j += tileWidth)
                    {
                        var r = new Rectangle(i, j, tileWidth, tileWidth);
                        var flag = false;
                        foreach (var o in obstaclesFrames) 
                        {
                            if (o.IntersectsWith(r)) 
                            {
                                flag = true;
                                break;
                            }
                        }
                        if(flag) res.Add(AbsPositionToRelaive(new Point(i, j)));
                    }
                }
                return res;
            }
        }
        public Dictionary<Point, Couch> jumpingPoints;
        public List<FishSpawner> spawners;
        public List<Goldfish> fishes;

        private int _points;
        public int Points 
        {
            get { return _points; }
            set 
            {
                _points = value;
                Game.InvokePointsIncreased();
            }
        }
        public int lives;
        public int invincibilityLength; //sec

        public World(int ms, int mw) 
        {
            mapSize = ms;
            tileWidth = mw;
            robotSpawn = new Point(200, 200);
            jumpingPoints = new Dictionary<Point, Couch>();
            lives = 5;
            invincibilityLength = 2;

            SquishedMice = new Queue<Sprite>();

            #region cat
            cat = new Cat(new Sprite(VisualData._catAnimations, Assets.catFront));
            cat.sprite.Position = new Point(100, 100);
            trail = new Queue<Point>();
            _trailLength = 5;
            trail.Enqueue(new Point(cat.Position.X / tileWidth, cat.Position.Y / tileWidth));
            #endregion

            #region robots
            robots = new List<Robot>();
            robots.Add(new Robot(new Sprite(VisualData._robotAnimations, Assets.robotUp) { Position = robotSpawn }));
            //robots.Add(new Robot(new Sprite(VisualData._robotAnimations, Assets.robotUp) { Position = new Point(512, 128) }));
            #endregion

            #region obstacles
            obstacles = new List<Obstacle>();
            obstacles.Add(new Couch(new Sprite(VisualData._couchTextures, Assets.emptyCouch), new Point(448, 320), ObstacleOrientation.FrontDown));
            obstacles.Add(new Couch(new Sprite(VisualData._couchTextures, Assets.emptyCouch), RelativePositionToAbs(new Point(7, 9)), ObstacleOrientation.FrontUp));
            obstacles.Add(new Wall(new Rectangle(0, 0, mapSize * tileWidth, tileWidth)));
            obstacles.Add(new Wall(new Rectangle(0, 0, tileWidth, mapSize * tileWidth)));
            obstacles.Add(new Wall(new Rectangle(mapSize * tileWidth, 0, tileWidth, mapSize * tileWidth)));
            obstacles.Add(new Wall(new Rectangle(0, mapSize * tileWidth, mapSize * tileWidth, tileWidth)));
            #endregion
            CreateJumpingPoints();

            #region spawners
            fishes = new List<Goldfish>();
            spawners = new List<FishSpawner>();
            var dict = new Dictionary<string, Animation>();
            dict["idle"] = new Animation(new[] { Assets.GoldCoin0, Assets.GoldCoin1 }, 0.3);
            spawners.Add(new FishSpawner(this, Assets.GoldCoin0, 10));
            #endregion
        }

        public Point AbsPositionToRelaive(Point p) 
        {
            return new Point(p.X / tileWidth, p.Y / tileWidth);
        }
        public Point RelativePositionToAbs(Point p) 
        {
            return new Point(p.X * tileWidth, p.Y * tileWidth);
        }

        public void UpdateTrail() 
        {
            var curPos = AbsPositionToRelaive(cat.Position);
            if (curPos != trail.Last()) 
            {
                trail.Enqueue(curPos);
                if (trail.Count > _trailLength) trail.Dequeue();
            }
        }

        private void CreateJumpingPoints() 
        {
            foreach (var o in obstacles) 
            {
                if (o is Couch) 
                {
                    var c = o as Couch;
                    var pos = new Point(c.Frame.X / tileWidth, c.Frame.Y / tileWidth);
                    switch (c.orientation) 
                    {
                        case (ObstacleOrientation.FrontDown):
                            jumpingPoints.Add(new Point(pos.X + 1, pos.Y + 2), c);
                            jumpingPoints.Add(new Point(pos.X + 2, pos.Y + 2), c);
                            jumpingPoints.Add(new Point(pos.X + 3, pos.Y + 2), c);
                            jumpingPoints.Add(new Point(pos.X + 4, pos.Y + 2), c);
                            break;
                        case (ObstacleOrientation.FrontUp):
                            jumpingPoints.Add(new Point(pos.X + 1, pos.Y - 1), c);
                            jumpingPoints.Add(new Point(pos.X + 2, pos.Y - 1), c);
                            jumpingPoints.Add(new Point(pos.X + 3, pos.Y - 1), c);
                            jumpingPoints.Add(new Point(pos.X + 4, pos.Y - 1), c);
                            break;
                        case (ObstacleOrientation.FrontRight):
                            jumpingPoints.Add(new Point(pos.X + 2, pos.Y + 1), c);
                            jumpingPoints.Add(new Point(pos.X + 2, pos.Y + 2), c);
                            jumpingPoints.Add(new Point(pos.X + 2, pos.Y + 3), c);
                            jumpingPoints.Add(new Point(pos.X + 2, pos.Y + 4), c);
                            break;
                        case (ObstacleOrientation.FrontLeft):
                            jumpingPoints.Add(new Point(pos.X - 1, pos.Y + 1), c);
                            jumpingPoints.Add(new Point(pos.X - 1, pos.Y + 2), c);
                            jumpingPoints.Add(new Point(pos.X - 1, pos.Y + 3), c);
                            jumpingPoints.Add(new Point(pos.X - 1, pos.Y + 4), c);
                            break;
                    }
                }
            }
        }

        public bool IsAccessible(int X, int Y) 
        {
            return IsAccessible(new Point(X, Y));
        }

        public bool IsAccessible(Point p) 
        {
            return mapPoints.Except(robotPositions).Except(obstaclesPositions).ToList().Contains(p);
        }
    }
}
