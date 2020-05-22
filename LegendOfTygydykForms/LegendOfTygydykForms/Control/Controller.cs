using LegendOfTygydykForms.Model;
using LegendOfTygydykForms.Model.Moving;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LegendOfTygydykForms.Control
{
    public class Controller
    {
        public World world;
        public Keys keyDown;
        public List<Sprite> toDraw;

        private Game game;
        private Random rnd;
        private bool _gameOver;
        private bool isGameOver
        {
            get 
            {
                return _gameOver;
            }
            set 
            {
                if(value)
                    game.GameOver();
                _gameOver = value;
            }
        }

        #region timers
        private double _robotUpdateTimer; //ms
        private double _robotChaseUpdateTimer; // ms
        private double _spaceRegisterTimer; //ms
        private double _catInvincibilityTimer; // ms
        private double _mouseMovementTimer;
        private const double _robotUpdateRate = 2; //sec       
        private const double _robotChaseUpdateRate = 0.6;//sec
        private const double _spaceRegisterRate = 0.2; //sec
        private const double _catInvincibilityLength = 2; // sec
        private const double _mouseMovementRate = 0.5;
        #endregion


        public Controller(World w, Game g) 
        {
            game = g;
            world = w;
            world.MouseSpawner = new MouseSpawner(world, 4);
            world.MouseSpawner.SpawnMouse();
            rnd = new Random();
            keyDown = default(Keys);
            isGameOver = false;
            toDraw = new List<Sprite>();
            toDraw.Add(world.Mouse.sprite);
            foreach (var r in world.robots)
                toDraw.Add(r.sprite);
            foreach (var o in world.obstacles)
                if (o is Couch)
                    toDraw.Add((o as Couch).sprite);
            toDraw.Add(world.cat.sprite);
        }

        public void RestartWith(World w) 
        {
            world = w;
            world.MouseSpawner = new MouseSpawner(world, 4);
            world.MouseSpawner.SpawnMouse();
            rnd = new Random();
            keyDown = default(Keys);
            isGameOver = false;
            toDraw = new List<Sprite>();
            toDraw.Add(world.Mouse.sprite);
            foreach (var r in world.robots)
                toDraw.Add(r.sprite);
            foreach (var o in world.obstacles)
                if (o is Couch)
                    toDraw.Add((o as Couch).sprite);
            toDraw.Add(world.cat.sprite);
        }

        // Update
        public void InvokeGameTick(int dt) 
        {
            if (world._pointsDelta >= 500) 
            {
                world._pointsDelta = 0;
                world.AddRobot();
            }

            UpdateTimers(dt);
            UpdateKeys();

            Update(world.cat, dt);
            if (isGameOver) return;

            UpdateMouse(dt);
            UpdateRobots(dt);
            UpdateSpawners(dt);
            UpdateFishes(dt);

            if (world.SquishedMice.Count > 5) 
            {
                toDraw.Remove(world.SquishedMice.Dequeue());
            }
        }

        private void UpdateTimers(int dt) 
        {
            #region timers
            _spaceRegisterTimer += dt;
            _robotUpdateTimer += dt;
            _robotChaseUpdateTimer += dt;
            _mouseMovementTimer += dt;

            if (world.cat.State == CatState.Invincible)
            {
                _catInvincibilityTimer += dt;
            }
            #endregion
        }
        private void UpdateKeys() 
        {
            #region keys handle
            switch (keyDown)
            {
                case (Keys.Up):
                    world.cat.Direction = Dir.Up;
                    break;
                case (Keys.Down):
                    world.cat.Direction = Dir.Down;
                    break;
                case (Keys.Right):
                    world.cat.Direction = Dir.Right;
                    break;
                case (Keys.Left):
                    world.cat.Direction = Dir.Left;
                    break;
                case (Keys.Space):
                    if (MsToSec(_spaceRegisterTimer) >= _spaceRegisterRate)
                    {
                        _spaceRegisterTimer = 0;
                        if (world.cat.State == CatState.Hidden)
                        {
                            world.cat.State = CatState.Idle;
                            world.jumpingPoints[world.AbsPositionToRelative(world.cat.Position)].HasCat = false;
                            toDraw.Add(world.cat.sprite);
                        }
                        else if (world.jumpingPoints.ContainsKey(world.AbsPositionToRelative(world.cat.Position)))
                        {
                            world.jumpingPoints[world.AbsPositionToRelative(world.cat.Position)].HasCat = true;
                            world.cat.State = CatState.Hidden;
                            toDraw.Remove(world.cat.sprite);
                        }
                    }
                    break;
                case (Keys.Escape):
                    game.Close();
                    break;
                case (Keys.R):
                    game.Restart();
                    break;
                case (Keys.Q):
                    game.GameOver();
                    break;
                case (default(Keys)):
                    world.cat.Direction = Dir.None;
                    break;
            }
            #endregion
        }
        private void UpdateRobots(int dt) 
        {
            #region robots
            var robotUpdateDirection = MsToSec(_robotUpdateTimer) >= _robotUpdateRate;
            if (robotUpdateDirection) _robotUpdateTimer = 0;
            var robotUpdateChase = MsToSec(_robotChaseUpdateTimer) >= _robotChaseUpdateRate;
            if (robotUpdateChase) _robotChaseUpdateTimer = 0;

            foreach (var r in world.robots)
            {
                if (world.CatState == CatState.Idle && DistBetween(world.AbsPositionToRelative(r.Position), world.CatPosition) <= 5)
                    r.State = RobotState.Chasing;
                else
                    r.State = RobotState.Idle;
            }

            foreach (var r in world.robots)
            {
                if (world.CatState != CatState.Hidden && robotUpdateChase)
                    r.Direction = ChooseRobotDir(r);
                else if (robotUpdateDirection)
                {
                    r.Direction = (Dir)rnd.Next(0, 3);
                }
                Update(r, dt);
            }
            #endregion
        }
        private void UpdateMouse(int dt) 
        {
            #region mouse
            if (world.CatState != CatState.Hidden && world.Mouse.Frame.IntersectsWith(world.cat.Frame))
            {
                if (toDraw.Contains(world.Mouse.sprite))
                {
                    toDraw.Remove(world.Mouse.sprite);
                }
                world.MouseSpawner.SpawnMouse();
                world.Points += 100;
                world._pointsDelta += 100;
                toDraw.Add(world.Mouse.sprite);
            }
            Update(world.Mouse, dt);
            #endregion
        }
        private void UpdateSpawners(int dt) 
        {
            world.MouseSpawner.Update(dt);
            foreach (var spawner in world.spawners)
            {
                spawner.Update(dt);
            }
        }
        private void UpdateFishes(int dt)
        {
            var toRemove = new List<Goldfish>();
            foreach (var f in world.fishes)
            {
                if (world.CatState != CatState.Hidden && f.Frame.IntersectsWith(world.CatFrame))
                {
                    world.Points += f.Points;
                    world._pointsDelta += f.Points;
                    toDraw.Remove(f.Sprite);
                    toRemove.Add(f);
                    continue;
                }
                if (!toDraw.Contains(f.Sprite))
                    toDraw.Add(f.Sprite);
                f.Sprite.Update(dt);
            }
            world.fishes.RemoveAll(f => toRemove.Contains(f));
        }
        private void Update(Cat c, int dt) 
        {
            if (c.State == CatState.Hidden) return;

            if (MsToSec(_catInvincibilityTimer) >= _catInvincibilityLength) 
            {
                c.State = CatState.Idle;
                _catInvincibilityTimer = 0;
            }
            c.sprite.Update(dt);
            var delta = DirToDelta(c.Direction);
            delta.X *= c.speed;
            delta.Y *= c.speed;
            var newFrame = new Rectangle(c.Frame.X + delta.X, c.Frame.Y + delta.Y, c.Frame.Width, c.Frame.Height);

            if (c.State != CatState.Invincible)
            {
                if(IntersectsWith(newFrame, withRobots: true))
                {
                    Game.InvokeGotHit();
                    world.lives -= 1;
                    if (world.lives == 0) 
                    {
                        isGameOver = true;
                        return;
                    }
                    c.State = CatState.Invincible;
                }
            }
            if (IntersectsWith(newFrame, withCouch: true, withWalls: true))
                return;

            c.UpdatePosition(new Point(newFrame.X + c.sprite.Offset.X, newFrame.Y + c.sprite.Offset.Y));
            world.UpdateTrail();
        }

        private void Update(Mouse m, int dt)
        {
            if (MsToSec(_mouseMovementTimer) >= _mouseMovementRate)
            {
                world.Mouse.Direction = (Dir)rnd.Next(0, 3);
                _mouseMovementTimer = 0;
            }
            m.sprite.Update(dt);
            var delta = DirToDelta(m.Direction);
            delta.X *= m.speed;
            delta.Y *= m.speed;
            var newFrame = new Rectangle(m.Frame.X + delta.X, m.Frame.Y + delta.Y, m.Frame.Width, m.Frame.Height);
            if (IntersectsWith(m.Frame, withRobots: true))
            {
                toDraw.Remove(m.sprite);
                world.MouseSpawner.SpawnMouse();
                world.MouseSpawner._timer = 0;
                toDraw.Add(world.Mouse.sprite);
                var temp = new Sprite(Assets.SquishedMouse) { Position = m.Position };
                toDraw.Add(temp);
                world.SquishedMice.Enqueue(temp);
                Game.InvokeMouseSquished();
                return;
            }

            if (IntersectsWith(newFrame, withWalls: true))
                return;

            m.UpdatePosition(new Point(newFrame.X + m.sprite.Offset.X, newFrame.Y + m.sprite.Offset.Y));
            if (IntersectsWith(m.Frame, withCouch: true))
            {
                toDraw.Remove(m.sprite);
            }
            else if (!toDraw.Contains(m.sprite))
            {
                toDraw.Add(m.sprite);
            }
        }

        private void Update(Robot r, int dt) 
        {
            if (!toDraw.Contains(r.sprite)) toDraw.Add(r.sprite);
            r.sprite.Update(dt);
            var delta = DirToDelta(r.Direction);
            delta.X *= r.speed;
            delta.Y *= r.speed;
            var newFrame = new Rectangle(r.Frame.X + delta.X, r.Frame.Y + delta.Y, r.Frame.Width, r.Frame.Height);
            if (IntersectsWith(newFrame, withWalls: true, withCouch: true))
                return;
            r.UpdatePosition(new Point(newFrame.X + r.sprite.Offset.X, newFrame.Y + r.sprite.Offset.Y));
        }

        private Dir ChooseRobotDir(Robot r)
        {
            var rp = world.AbsPositionToRelative(r.sprite.Position);
            var temp = world.trail.Where(t => t != rp).OrderBy(t => DistBetween(t, rp)).ToList();
            if (temp.Count == 0) return DeltaToDir(0, 0);
            var p1 = temp.First();
            temp = GetNighborhood(rp).OrderBy(p => DistBetween(p, p1)).ToList();
            if (temp.Count == 0) return DeltaToDir(0, 0);
            var p2 = temp.First();
            return DeltaToDir(p2.X - rp.X, p2.Y - rp.Y);
        }

        private bool IntersectsWith(Rectangle frame, bool withWalls = false, bool withCouch = false, bool withRobots = false)
        {
            if (!(withWalls || withCouch || withRobots)) return false;
            var list = new List<Rectangle>();
            if (withWalls)
            {
                list = world.wallFrames;
            }
            if (withCouch)
            {
                list.AddRange(world.couchFrames);
            }
            if (withRobots)
            {
                list.AddRange(world.robotFrames);
            }
            foreach (var o in list.Where(o => o != frame))
            {
                if (o.IntersectsWith(frame))
                    return true;
            }
            return false;
        }

        private Point DirToDelta(Dir d) 
        {
            var dx = 0;
            var dy = 0;
            switch (d)
            {
                case (Dir.Up):
                    dy = -1;
                    break;
                case (Dir.Right):
                    dx = 1;
                    break;
                case (Dir.Left):
                    dx = -1;
                    break;
                case (Dir.Down):
                    dy = 1;
                    break;
            }
            return new Point(dx, dy);
        }

        private Dir DeltaToDir(int dx, int dy) 
        {
            return (dx == 0) ? ((dy > 0) ? Dir.Down : Dir.Up) : ((dx > 0) ? Dir.Right : Dir.Left);
        }

        private double MsToSec(double ms) 
        {
            return ms / 1000.0;
        }

        private static int DistBetween(Point p1, Point p2)
        {
            var dx = p1.X - p2.X;
            var dy = p1.Y - p2.Y;
            return (int)Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"> Relative position in world.</param>
        /// <returns></returns>
        private IEnumerable<Point> GetNighborhood(Point p)         
        {
            for (int dx = -1; dx < 2; dx++) 
            {
                for (int dy = -1; dy < 2; dy++) 
                {
                    if (Math.Abs(dx + dy) == 1 && world.IsAccessible(p.X + dx, p.Y + dy))
                        yield return new Point(p.X + dx, p.Y + dy);
                }
            }
        }
    }
}
