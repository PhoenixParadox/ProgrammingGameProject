using LegendOfTygydykForms.Model;
using LegendOfTygydykForms.Model.Geometry;
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

        #region timers
        private double _robotUpdateTimer; //ms
        private double _robotChaseUpdateTimer; // ms
        private double _spaceRegisterTimer; //ms
        private double _catInvincibilityTimer; // ms
        private double _mouseMovementTimer;
        private double _robotUpdateRate; //sec       
        private double _robotChaseUpdateRate;//sec
        private double _spaceRegisterRate; //sec
        private double _catInvincibilityLength; // sec
        private double _mouseMovementRate;
        #endregion


        public Controller(World w, Game g) 
        {
            game = g;
            world = w;
            world.MouseSpawner = new MouseSpawner(world, 4);
            world.MouseSpawner.SpawnMouse();
            rnd = new Random();
            _robotUpdateRate = 2;
            _robotChaseUpdateRate = 0.6;
            _mouseMovementRate = 0.5;
            _spaceRegisterRate = 0.2;
            _catInvincibilityLength = w.invincibilityLength;

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
            _catInvincibilityLength = w.invincibilityLength;

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

            //var temp = FreePoints();
            #region timers
            _spaceRegisterTimer += dt;
            _robotUpdateTimer += dt;
            _robotChaseUpdateTimer += dt;
            if (world.cat.State == CatState.Invincible)
            {
                _catInvincibilityTimer += dt;
            }
            #endregion

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
                            world.jumpingPoints[world.AbsPositionToRelaive(world.cat.Position)].HasCat = false;
                            toDraw.Add(world.cat.sprite);
                        }
                        else if (world.jumpingPoints.ContainsKey(world.AbsPositionToRelaive(world.cat.Position)))
                        {
                            world.jumpingPoints[world.AbsPositionToRelaive(world.cat.Position)].HasCat = true;
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
                case (default(Keys)):
                    world.cat.Direction = Dir.None;
                    break;
            }
            #endregion

            if (world.cat.State != CatState.Hidden)
                Update(world.cat, dt);
            world.UpdateTrail();

            #region mouse
            _mouseMovementTimer += dt;
            world.MouseSpawner.Update(dt);
            if (world.cat.State != CatState.Hidden && world.Mouse.Frame.IntersectsWith(world.cat.Frame))
            {
                if (toDraw.Contains(world.Mouse.sprite)) 
                {
                    toDraw.Remove(world.Mouse.sprite);
                }
                world.MouseSpawner.SpawnMouse();
                world.MouseSpawner._timer = 0;
                world.Points += 100;
                world._pointsDelta += 100;
                toDraw.Add(world.Mouse.sprite);
            }
            //else 
            //{
            //    if (MsToSec(world.MouseSpawner._timer) >= world.MouseSpawner._respawnRate) 
            //    {
            //        if (toDraw.Contains(world.Mouse.sprite))
            //        {
            //            toDraw.Remove(world.Mouse.sprite);
            //        }
            //        world.MouseSpawner.SpawnMouse();
            //        world.MouseSpawner._timer = 0;
            //        toDraw.Add(world.Mouse.sprite);
            //    }
            //}
            if (MsToSec(_mouseMovementTimer) >= _mouseMovementRate) 
            {
                world.Mouse.Direction = (Dir)rnd.Next(0, 3);
                _mouseMovementTimer = 0;
            }
            Update(world.Mouse, dt);
            #endregion

            #region robots
            var robotUpdateDirection = MsToSec(_robotUpdateTimer) >= _robotUpdateRate;
            if (robotUpdateDirection) _robotUpdateTimer = 0;
            var robotUpdateChase = MsToSec(_robotChaseUpdateTimer) >= _robotChaseUpdateRate;
            if (robotUpdateChase) _robotChaseUpdateTimer = 0;

                foreach (var r in world.robots)
                {
                    if (world.cat.State == CatState.Idle && DistBetween(world.AbsPositionToRelaive(r.sprite.Position), world.AbsPositionToRelaive(world.cat.sprite.Position)) <= 5)
                        r.State = RobotState.Chasing;
                    else
                        r.State = RobotState.Idle;
                }

            foreach (var r in world.robots) 
            {
                if (world.cat.State != CatState.Hidden &&  robotUpdateChase)
                    r.Direction = ChooseRobotDir(r);
                else if(robotUpdateDirection)
                {
                    r.Direction = (Dir)rnd.Next(0, 3);
                }
                Update(r, dt);
            }
            #endregion

            foreach (var spawner in world.spawners)
            {
                spawner.Update(dt);
            }
            var toRemove = new List<Goldfish>();
            foreach (var f in world.fishes)
            {
                if (world.cat.State != CatState.Hidden && f.Frame.IntersectsWith(world.cat.Frame))
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

            if (world.SquishedMice.Count > 5) 
            {
                toDraw.Remove(world.SquishedMice.Dequeue());
            }
        }

        public void Update(Cat c, int dt) 
        {
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
                if(IntersectsWithRobots(newFrame))
                {
                    Game.InvokeGotHit();
                    world.lives -= 1;
                    if (world.lives == 0)
                        game.Restart();
                    c.State = CatState.Invincible;
                }
            }
            if (IntersectsWithObstacles(newFrame))
                return;

            c.UpdatePosition(new Point(newFrame.X + c.sprite.Offset.X, newFrame.Y + c.sprite.Offset.Y));
        }

        private void Update(Robot r, int dt) 
        {
            if (!toDraw.Contains(r.sprite)) toDraw.Add(r.sprite);
            r.sprite.Update(dt);
            var delta = DirToDelta(r.Direction);
            delta.X *= r.speed;
            delta.Y *= r.speed;
            var newFrame = new Rectangle(r.Frame.X + delta.X, r.Frame.Y + delta.Y, r.Frame.Width, r.Frame.Height);
            foreach (var o in world.obstacles)
            {
                if (o.Frame.IntersectsWith(newFrame))
                {
                    return;
                }
            }
            //foreach (var r1 in world.robots)
            //{
            //    if (r1 != r && r1.Frame.IntersectsWith(r.Frame))
            //    {
            //        newFrame.X -= 2 * delta.X;
            //        newFrame.Y -= 2 * delta.Y;
            //        break;
            //    }
            //}
            r.UpdatePosition(new Point(newFrame.X + r.sprite.Offset.X, newFrame.Y + r.sprite.Offset.Y));
        }

        public void Update(Mouse m, int dt)
        {
            m.sprite.Update(dt);
            var delta = DirToDelta(m.Direction);
            delta.X *= m.speed;
            delta.Y *= m.speed;
            var newFrame = new Rectangle(m.Frame.X + delta.X, m.Frame.Y + delta.Y, m.Frame.Width, m.Frame.Height);
            if (IntersectsWithRobots(m.Frame))
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

            if (IntersectsWithWalls(newFrame))
                return;

            m.UpdatePosition(new Point(newFrame.X + m.sprite.Offset.X, newFrame.Y + m.sprite.Offset.Y));
            if (IntersectsWithCouches(m.Frame))
            {
                toDraw.Remove(m.sprite);
            }
            else if (!toDraw.Contains(m.sprite))
            {
                toDraw.Add(m.sprite);
            }
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

        private bool IntersectsWithObstacles(Rectangle frame) 
        {
            foreach (var o in world.obstacles) 
            {
                if (frame.IntersectsWith(o.Frame)) return true;
            }
            return false;
        }

        private bool IntersectsWithWalls(Rectangle frame) 
        {
            foreach (var o in world.obstacles)
            {
                if ((o is Wall) && frame.IntersectsWith(o.Frame)) return true;
            }
            return false;
        }
        private bool IntersectsWithCouches(Rectangle frame)
        {
            foreach (var o in world.obstacles)
            {
                if ((o is Couch) && o.Frame.Contains(frame)) return true;
            }
            return false;
        }
        private bool IntersectsWithRobots(Rectangle frame)
        {
            foreach (var r in world.robots)
            {
                if (frame.IntersectsWith(r.Frame)) return true;
            }
            return false;
        }

        public static int DistBetween(Point p1, Point p2)
        {
            var dx = p1.X - p2.X;
            var dy = p1.Y - p2.Y;
            return (int)Math.Sqrt(dx * dx + dy * dy);
        }

        private Dir ChooseRobotDir(Robot r) 
        {
            var rp = world.AbsPositionToRelaive(r.sprite.Position);
            var p1 = world.trail.Where(t => t != rp).OrderBy(t => DistBetween(t, rp)).First();
            var p2 = GetNighborhood(rp).OrderBy(p => DistBetween(p, p1)).First();
            return DeltaToDir(p2.X - rp.X, p2.Y - rp.Y);
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
