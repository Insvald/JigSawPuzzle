using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JigSawPuzzle;

namespace JigSawPuzzleTest
{
    [TestClass]
    public class PuzzleTest
    {
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void BadArgumentsTest()
        {
            Puzzle puzzle = new Puzzle(4, 0);
        }

        [TestMethod]
        public void EdgeTest()
        {
            //test that all edges in prepared solution are matched to each other, and border edges are flat
            int width = 6;
            int height = 3;
            Puzzle puzzle = new Puzzle(width, height);

            for (int x = 0; x < width; x++)
            {
                //top row
                Assert.AreEqual(0, puzzle.Solution[x, 0].Edge[Tile.Top]);
                //bottom row
                Assert.AreEqual(0, puzzle.Solution[x, height - 1].Edge[Tile.Bottom]);
            }

            for (int y = 0; y < height; y++)
            {
                //left row
                Assert.AreEqual(0, puzzle.Solution[0, y].Edge[Tile.Left]);
                //right row
                Assert.AreEqual(0, puzzle.Solution[width-1, y].Edge[Tile.Right]);
            }

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (x != width - 1)
                        Assert.AreEqual(0, puzzle.Solution[x, y].Edge[Tile.Right] + puzzle.Solution[x + 1, y].Edge[Tile.Left]);
                    if (y != height - 1)
                        Assert.AreEqual(0, puzzle.Solution[x, y].Edge[Tile.Bottom] + puzzle.Solution[x, y + 1].Edge[Tile.Top]);
                }
        }

        [TestMethod]
        public void ShuffleTest()
        {
            int width = 4;
            int height = 6;
            Puzzle puzzle = new Puzzle(width, height);
            Tile[] tiles = puzzle.Shuffle();
            //test for array size
            Assert.AreEqual(width * height, tiles.Length);
            //test for not null values
            for (int n = 0; n < tiles.Length; n++)
                Assert.IsNotNull(tiles[n]);
        }

        [TestMethod]
        public void TileMatchTest()
        {
            Tile T1 = new Tile();
            Tile T2 = new Tile();
            T1.Edge[0] = 27;
            T2.Edge[2] = -27;
            Assert.IsTrue(T1.IsMatch(0, T2));

            T1.Edge[0] = 29;
            Assert.IsFalse(T1.IsMatch(0, T2));
        }

        [TestMethod]
        public void SolutionTest()
        {
            int w = 5;
            int h = 4;
            Puzzle puzzle = new Puzzle(w, h);
            Tile[,] solved = puzzle.Solve(puzzle.Shuffle());

            //we need to know new sizes of the field          
            for (w = 1; solved[w - 1, 0].Edge[Tile.Right] != 0; w++) ;
            for (h = 1; solved[0, h - 1].Edge[Tile.Bottom] != 0; h++) ; 

            //check borders for zeros
            for (int x = 0; x < w; x++)
            {
                Assert.AreEqual(0, solved[x, 0].Edge[Tile.Top]);
                Assert.AreEqual(0, solved[x, h - 1].Edge[Tile.Bottom]);
            }

            for (int y = 0; y < h; y++)
            {
                Assert.AreEqual(0, solved[0, y].Edge[Tile.Left]);
                Assert.AreEqual(0, solved[w - 1, y].Edge[Tile.Right]);
            }

            //check connecting edges for zero sum
            for (int x = 0; x < w-1; x++)
                for (int y = 0; y < h-1; y++)
                {
                    Assert.AreEqual(0, solved[x, y].Edge[Tile.Right] + solved[x + 1, y].Edge[Tile.Left]);
                    Assert.AreEqual(0, solved[x, y].Edge[Tile.Bottom] + solved[x, y+1].Edge[Tile.Top]);
                }
        }
    }
}
