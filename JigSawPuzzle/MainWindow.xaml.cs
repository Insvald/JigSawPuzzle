using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JigSawPuzzle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            console.Items.Clear();
            int w = 6;
            int h = 3;
            int x = 0;
            int y = 0;
            Puzzle puzzle = new Puzzle(w, h);
            //test initial solution
            for (y = 0; y < h; y++)
            {
                string S = "";
                for (x = 0; x < w; x++)
                    S += String.Format("   {0}   ", puzzle.Solution[x, y].Edge[Tile.Top]);
                console.Items.Add(S);
                S = "";
                for (x = 0; x < w; x++)
                    S += String.Format("{0}     {1}", puzzle.Solution[x, y].Edge[Tile.Left], puzzle.Solution[x, y].Edge[Tile.Right]);
                console.Items.Add(S);
                S = "";
                for (x = 0; x < w; x++)
                    S += String.Format("   {0}   ", puzzle.Solution[x, y].Edge[Tile.Bottom]);
                console.Items.Add(S);
            }
            //test shuffling
            console.Items.Add("shuffling...");
            Tile[] tiles = puzzle.Shuffle();
            
            console.Items.Add("solving...");
            Tile[,] solvedTiles = puzzle.Solve(tiles);
            
            //we need to know new sizes of the field          
            for (w = 1; solvedTiles[w - 1, 0].Edge[Tile.Right] != 0; w++);
            for (h = 1; solvedTiles[0, h - 1].Edge[Tile.Bottom] != 0; h++); 

            //test final solution            
            for (y = 0; y < h; y++)
            {
                string S = "";
                for (x = 0; x < w; x++)
                    S += String.Format("   {0}   ", solvedTiles[x, y].Edge[Tile.Top]);
                console.Items.Add(S);
                S = "";
                for (x = 0; x < w; x++)
                    S += String.Format("{0}     {1}", solvedTiles[x, y].Edge[Tile.Left], solvedTiles[x, y].Edge[Tile.Right]);
                console.Items.Add(S);
                S = "";
                for (x = 0; x < w; x++)
                    S += String.Format("   {0}   ", solvedTiles[x, y].Edge[Tile.Bottom]);
                console.Items.Add(S);
            }

        }
    }
}
