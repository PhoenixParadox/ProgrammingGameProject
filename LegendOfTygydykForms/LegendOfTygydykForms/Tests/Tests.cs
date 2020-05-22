using LegendOfTygydykForms.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Tests
{
    [TestFixture]
    public class Tests
    {
        public WorldConfig testWorld1 = new WorldConfig()
        {
            Size = new Size(5, 5),
            TileWidth = 64,
            Lives = 1,
            Walls = new List<ObstacleConfig>(),
            Couches = new List<ObstacleConfig>()
        };
        public WorldConfig testWorld2 = new WorldConfig()
        {
            Size = new Size(15, 15),
            TileWidth = 64,
            Lives = 5,
            Couches = new List<ObstacleConfig>()
                    {
                        new ObstacleConfig { Position = new Point(8, 2), Orientation = ObstacleOrientation.FrontDown },
                        new ObstacleConfig { Position = new Point(8, 14), Orientation = ObstacleOrientation.FrontUp },
                        new ObstacleConfig { Position = new Point(2, 8), Orientation = ObstacleOrientation.FrontRight },
                        new ObstacleConfig { Position = new Point(14, 8), Orientation = ObstacleOrientation.FrontLeft }
                    },
            Walls = new List<ObstacleConfig>()
        };

        [Test]
        public void CreationTest()
        {
            var world = new World(testWorld1);
            Assert.IsNotNull(world.cat);
            Assert.IsNotNull(world.robots);
            Assert.IsNotNull(world.obstacles);
        }

        [Test]
        public void CorrectFieldSize() 
        {
            var world = new World(testWorld1);
            Assert.AreEqual(world.mapPoints.Count, 16);
        }

        [Test]
        public void CorrectObstaclesCount() 
        {
            // extra 4 is accounted for walls
            var world = new World(testWorld1);
            Assert.AreEqual(world.obstacles.Count, 4);
            world = new World(testWorld2);
            Assert.AreEqual(world.obstacles.Count, 8);
        }
    }
}
