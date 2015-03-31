using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JigSawPuzzle
{

    public class Tile
    {
        public const int EdgeCount = 4; 
        public const int Top = 0;
        public const int Right = 1;
        public const int Bottom = 2;
        public const int Left = 3;        
        
        //0 - flat
        //negative - inner edge
        //positive - outer edge        
        public int[] Edge { get; set; }

        public Tile()
        {
            Edge = new int[EdgeCount];
        }
       
        public bool IsMatch(int ownEdgeToMatch, Tile tile)
        {
            bool match = false;
            int rotationCount = 0;
            while (!match && (rotationCount < Tile.EdgeCount))
            {
                if (this.Edge[ownEdgeToMatch] == -tile.Edge[(ownEdgeToMatch + 2) % Tile.EdgeCount])
                    match = true;
                else
                {
                    tile.Rotate(1);
                    rotationCount++;
                }
            }
            return match;
        }

        public void Rotate(int rotationCount)
        {
            for (int i = 0; i < rotationCount; i++)
            {
                //rotation clockwise by 90 degree
                int tempEdge = Edge[Tile.Top];
                Edge[Tile.Top] = Edge[Tile.Left];
                Edge[Tile.Left] = Edge[Tile.Bottom];
                Edge[Tile.Bottom] = Edge[Tile.Right];
                Edge[Tile.Right] = tempEdge;
            }
        }
    }
    
    public class Puzzle
    {
        //field size
        private int width;
        private int height;
       
        public Tile[,] Solution {get; set; }

        const string ArgumentShouldBePositive = "Argument should be positive!";
        
        //generate tiles
        public Puzzle(int width, int height)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width", ArgumentShouldBePositive);
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height", ArgumentShouldBePositive);

            this.width = width;
            this.height = height;
            int signatureCounter = 0;

            //we need to generate width x height tiles
            Solution = new Tile[width, height];
            
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    Solution[x, y] = new Tile();

            for (int x = 0; x < width; x++)
            {
                //top row
                Solution[x, 0].Edge[Tile.Top] = 0;
                //bottom row
                Solution[x, height - 1].Edge[Tile.Bottom] = 0;
            }

            for (int y = 0; y < height; y++)
            {
                //left row
                Solution[0, y].Edge[Tile.Left] = 0;
                //right row
                Solution[width-1, y].Edge[Tile.Right] = 0;
            }

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (x != width-1)
                    {
                        //we can add randomizer for inner/outer edge, but there is no need
                        Solution[x, y].Edge[Tile.Right] = ++signatureCounter;
                        Solution[x + 1, y].Edge[Tile.Left] = -signatureCounter;
                    }
                    if (y != height-1)
                    {
                        Solution[x, y].Edge[Tile.Bottom] = ++signatureCounter;
                        Solution[x, y + 1].Edge[Tile.Top] = -signatureCounter;
                    }
                }
        }

        //return random array of tiles
        public Tile[] Shuffle()
        {
            int tileCount = width * height;
            Tile[] result = new Tile[tileCount];
            Random randomizer = new Random();

            for (int x = 0; x < width; x++)
                for(int y = 0; y < height; y++)
                {
                    //randomize place
                    int n = randomizer.Next(tileCount);
                    //looking for a free place
                    while (result[n] != null)
                        if (++n == tileCount)
                            n = 0;
                    result[n] = Solution[x, y];
                    //randomize rotation
                    result[n].Rotate(randomizer.Next(Tile.EdgeCount));
                }
            return result;
        }

        //try to solve the puzzle
        public Tile[,] Solve(Tile[] tiles)
        { 
            int size = Math.Max(width, height);
            int notDistributedTiles = tiles.Length;
            Tile[,] field = new Tile[size, size];

            //looking for a first candidate for top-left corner - a tile with two adjacent flat edges
            for(int n = 0; field[0, 0] == null; n++)
                for(int i = 0; i < Tile.EdgeCount-1; i++)
                    if ((tiles[n].Edge[i] == 0) && (tiles[n].Edge[i < 1 ? Tile.EdgeCount-1 : i-1] == 0))
                    {
                        tiles[n].Rotate(Tile.EdgeCount - i);
                        field[0, 0] = tiles[n];
                        tiles[n] = null;
                        notDistributedTiles--;
                        break;
                    }

            for (int y = 0; notDistributedTiles > 0; y++)
            {
                for (int x = 0; field[x, y].Edge[Tile.Right] != 0; x++)
                {
                    //looking for a match to the right
                    for (int n = 0; n < tiles.Length; n++)
                        if (tiles[n] != null)
                            if (field[x, y].IsMatch(Tile.Right, tiles[n]))
                            {
                                field[x + 1, y] = tiles[n];
                                tiles[n] = null;
                                notDistributedTiles--;
                                break;
                            }
                }
                //if not the last row, go back to the start of the row and find a match to the bottom
                for (int n = 0; n < tiles.Length; n++)
                    if (tiles[n] != null)
                        if (field[0, y].IsMatch(Tile.Bottom, tiles[n]))
                        {
                            field[0, y + 1] = tiles[n];
                            tiles[n] = null;
                            notDistributedTiles--;
                            break;
                        }                    
            }

            return field;
        }
    }
}
