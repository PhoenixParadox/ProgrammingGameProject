﻿using LegendOfTygydykForms.Control;
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


    public class World
    {
        private Point robotSpawn;
        private int _points;
        private Random rnd;
        public Cat cat { get; private set; }
        public Point CatPosition { get { return cat.Position; } }
        private int _trailLength;
        public Queue<Point> trail;

        public Mouse Mouse;
        public MouseSpawner MouseSpawner;
        public Queue<Sprite> SquishedMice;

        public List<Robot> robots;
        public List<Obstacle> obstacles;

        #region points and frames collections

        /// <summary>
        /// All map points.
        /// </summary>
        public List<Point> mapPoints
        {
            get
            {
                var res = new List<Point>();
                for (int i = 1; i < worldSize.Width; i++)
                {
                    for (int j = 1; j < worldSize.Height; j++)
                    {
                        res.Add(new Point(i, j));
                    }
                }
                return res;
            }
        }
        /// <summary>
        /// Points without obstacles and robots.
        /// </summary>
        public List<Point> AccessiblePoints { get { return mapPoints.Except(robotPositions).Except(obstaclesPositions).ToList(); } }
        public List<Point> robotPositions { get { return robots.Select(r => AbsPositionToRelaive(new Point(r.Frame.X, r.Frame.Y))).ToList(); } }
        public List<Point> obstaclesPositions
        {
            get
            {
                var res = new List<Point>();
                for (int i = tileWidth; i < worldSize.Width * tileWidth; i += tileWidth)
                {
                    for (int j = tileWidth; j < worldSize.Height * tileWidth; j += tileWidth)
                    {
                        var r = new Rectangle(i, j, tileWidth, tileWidth);
                        var flag = false;
                        foreach (var o in couchFrames)
                        {
                            if (o.IntersectsWith(r))
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag) res.Add(AbsPositionToRelaive(new Point(i, j)));
                    }
                }
                return res;
            }
        }
        public List<Rectangle> robotFrames { get { return robots.Select(r => r.Frame).ToList(); } }
        public List<Rectangle> couchFrames { get { return obstacles.Where(o => o is Couch).Select(o => o.Frame).ToList(); } }
        public List<Rectangle> wallFrames { get { return obstacles.Where(o => o is Wall).Select(o => o.Frame).ToList(); } }
        public List<Rectangle> obstacleFrames { get { return obstacles.Select(o => o.Frame).ToList(); } }

        #endregion

        public Size worldSize { get; }
        public int tileWidth { get; }        

        public Dictionary<Point, Couch> jumpingPoints;
        public List<FishSpawner> spawners;
        public List<Goldfish> fishes;


        public int _pointsDelta;

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

        /// <summary>
        /// Creates a new game world.
        /// </summary>
        /// <param name="mw"> Size of a tile. Tiles are squared. </param>
        /// <param name="size"> Size of the room. </param>
        public World(int mw, Size size) 
        {
            tileWidth = mw;
            worldSize = size;
            lives = 5;
            invincibilityLength = 2;
            jumpingPoints = new Dictionary<Point, Couch>();
            rnd = new Random();
            SquishedMice = new Queue<Sprite>();
            
            #region cat
            cat = new Cat(new Sprite(VisualData._catAnimations, Assets.catFront));
            trail = new Queue<Point>();
            _trailLength = 5;
            trail.Enqueue(new Point(cat.Position.X / tileWidth, cat.Position.Y / tileWidth));
            #endregion

            #region robots
            robots = new List<Robot>();
            robots.Add(new Robot(new Sprite(VisualData._robotAnimations, Assets.robotUp)));
            #endregion

            #region obstacles
            obstacles = new List<Obstacle>();
            obstacles.Add(new Couch(new Sprite(VisualData._couchTextures, Assets.emptyCouch), RelativePositionToAbs(new Point(7, 4)), ObstacleOrientation.FrontDown));
            obstacles.Add(new Couch(new Sprite(VisualData._couchTextures, Assets.emptyCouch), RelativePositionToAbs(new Point(7, 9)), ObstacleOrientation.FrontUp));
            obstacles.Add(new Wall(new Rectangle(0, 0, worldSize.Width * tileWidth, tileWidth)));
            obstacles.Add(new Wall(new Rectangle(0, 0, tileWidth, worldSize.Height * tileWidth)));
            obstacles.Add(new Wall(new Rectangle(worldSize.Width * tileWidth, 0, tileWidth, worldSize.Height * tileWidth)));
            obstacles.Add(new Wall(new Rectangle(0, worldSize.Height * tileWidth, worldSize.Width * tileWidth, tileWidth)));
            #endregion
            CreateJumpingPoints();
            var temp = AccessiblePoints.Where(p => p.X != 1 && p.X != worldSize.Width - 1 && p.Y != 1 && p.Y != worldSize.Height - 1).ToList();
            robotSpawn = RelativePositionToAbs(temp.ElementAt(rnd.Next(temp.Count - 1)));
            robots[0].UpdatePosition(robotSpawn);
            cat.UpdatePosition(RelativePositionToAbs(temp.ElementAt(rnd.Next(temp.Count - 1))));
            #region spawners
            fishes = new List<Goldfish>();
            spawners = new List<FishSpawner>();
            var dict = new Dictionary<string, Animation>();
            dict["idle"] = new Animation(new[] { Assets.GoldCoin0, Assets.GoldCoin1 }, 0.3);
            spawners.Add(new FishSpawner(this, Assets.GoldCoin0, 10));
            #endregion
        }

        public void AddRobot() 
        {
            robots.Add(new Robot(new Sprite(VisualData._robotAnimations, Assets.robotUp) { Position = robotSpawn }));
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
                    switch (c.Orientation) 
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
