using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using ATP.MazeGenerators;
using ATP.Search;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly Random getrandom = new Random();

        /// <summary>
        /// Check if returns null when the x,y,z are less than two.
        /// </summary>
        [TestMethod]
        public void generatemazenegativeint()
        {

            int x = getrandom.Next(-100, 2);
            int y = getrandom.Next(-100, 2);
            int z = getrandom.Next(-100, 2);
            ArrayList s = new ArrayList();
            s.Add(x);
            s.Add(y);
            s.Add(z);
            MyMaze3dGenerator mg = new MyMaze3dGenerator();
            AMaze maze = mg.generate(s);
            Assert.IsNull(maze);
        }

        /// <summary>
        /// Check if returns null when get array list that is null.
        /// </summary>
        [TestMethod]
        public void generatemazewithnull()
        {
            ArrayList s = new ArrayList();
            s = null;
            MyMaze3dGenerator mg = new MyMaze3dGenerator();
            AMaze maze = mg.generate(s);
            Assert.IsNull(maze);
        }

        /// <summary>
        /// Generate 100 mazes and check if not null each time.
        /// </summary>
        [TestMethod]
        public void generatemaze100times()
        {
            for (int i = 0; i < 100; i++)
            {
                int x = getrandom.Next(3, 50);
                int y = getrandom.Next(3, 50);
                int z = getrandom.Next(3, 50);
                ArrayList s = new ArrayList();
                s.Add(x);
                s.Add(y);
                s.Add(z);
                MyMaze3dGenerator mg = new MyMaze3dGenerator();
                AMaze maze = mg.generate(s);
                Assert.IsNotNull(maze);
            }

        }

        /// <summary>
        /// Generate 10 large mazes and check if not null each time.
        /// </summary>
        [TestMethod]
        public void generatelargemaze()
        {
            for (int i = 0; i < 10; i++)
            {
                int x = getrandom.Next(50, 100);
                int y = getrandom.Next(50, 100);
                int z = getrandom.Next(50, 100);
                ArrayList s = new ArrayList();
                s.Add(x);
                s.Add(y);
                s.Add(z);
                MyMaze3dGenerator mg = new MyMaze3dGenerator();
                AMaze maze = mg.generate(s);
                Assert.IsNotNull(maze);
            }

        }

        /// <summary>
        /// Check if maze has a solution - 50 times bfs and 50 times with dfs.
        /// </summary>
        [TestMethod]
        public void ismazesolveable()
        {
            for (int i = 0; i < 50; i++)
            {
                int x = getrandom.Next(3, 50);
                int y = getrandom.Next(3, 50);
                int z = getrandom.Next(3, 50);
                ArrayList s = new ArrayList();
                s.Add(x);
                s.Add(y);
                s.Add(z);
                MyMaze3dGenerator mg = new MyMaze3dGenerator();
                AMaze maze = mg.generate(s);
                SearchableMaze3d sm = new SearchableMaze3d(maze as Maze3d);
                Solution sol;
                DepthFirstSearch ds = new DepthFirstSearch();
                sol = ds.Solve(sm);
                Assert.IsNotNull(sol);
            }
            for (int i = 0; i < 50; i++)
            {
                int x = getrandom.Next(3, 50);
                int y = getrandom.Next(3, 50);
                int z = getrandom.Next(3, 50);
                ArrayList s = new ArrayList();
                s.Add(x);
                s.Add(y);
                s.Add(z);
                MyMaze3dGenerator mg = new MyMaze3dGenerator();
                AMaze maze = mg.generate(s);
                SearchableMaze3d sm = new SearchableMaze3d(maze as Maze3d);
                Solution sol;
                BreadthFirstSearch bs = new BreadthFirstSearch();
                sol = bs.Solve(sm);
                Assert.IsNotNull(sol);
            }
        }
    }
}
