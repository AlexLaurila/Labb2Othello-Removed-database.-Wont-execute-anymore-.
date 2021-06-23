using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OthelloServer
{
    /// <summary>
    /// Creates and paints all necessary visuals for the game. 
    /// Updates visuals to current boardstate when requested.
    /// </summary>
    public partial class GameBoard : UserControl
    {
        //Attribut
        private int boardInitialized = 0;
        private Grid[,] innerGridList = new Grid[8,8];

        //Konstruktor
        public GameBoard()
        {
            InitializeComponent();
        }

        //Metoder
        /// <summary>
        /// Updates visuals to current boardstate.
        /// </summary>
        /// <param name="boardState">A matrix containing information about the current boardstate</param>
        /// <param name="frame">A instance of GameFrame to initialize board with clickable buttons</param>
        public void UpdateBoard(int[,] boardState, GameFrame frame)
        {
            //Board will be initialized with buttons first time this method is executed.
            if (boardInitialized == 0)
            {
                InitializeBoard(frame);
                boardInitialized++;
            }

            //Clears all current ellipses and draw new ones
            for (int i = 0; i < boardState.GetLength(0); i++)
            {
                for (int j = 0; j < boardState.GetLength(0); j++)
                {
                    Ellipse ellipse = new Ellipse();

                    if (boardState[i, j] == 0)
                        ellipse.Fill = Brushes.Green;
                    else if (boardState[i, j] == 1)
                        ellipse.Fill = Brushes.White;
                    else
                        ellipse.Fill = Brushes.Black;

                    ellipse.Height = double.NaN;
                    ellipse.Width = double.NaN;
                    ellipse.Margin = new Thickness(6, 6, 6, 6);
                    ellipse.IsHitTestVisible = false;

                    innerGridList[i, j].Children.Clear();
                    innerGridList[i, j].Children.Add(ellipse);
                }
            }
        }

        /// <summary>
        /// Initializes board with clickable buttons
        /// </summary>
        /// <param name="frame">Connecting to a instance of GameFrame to send graphical input</param>
        private void InitializeBoard(GameFrame frame)
        {
            for (int i = 0; i < innerGridList.GetLength(0); i++)
            {
                for (int j = 0; j < innerGridList.GetLength(0); j++)
                {
                    Grid innerGrid = new Grid();
                    innerGrid.Background = Brushes.Green;
                    innerGrid.MouseLeftButtonDown += (sender, e) =>
                    {
                        int[] chosenCoords = new int[2];
                        Grid button = sender as Grid;
                        int row = (int)button.GetValue(Grid.RowProperty);
                        int column = (int)button.GetValue(Grid.ColumnProperty);

                        chosenCoords[0] = row;
                        chosenCoords[1] = column;
                        frame.GraphicalInput(chosenCoords);
                    };

                    innerGrid.Margin = new Thickness(2, 2, 2, 2);
                    Grid.SetRow(innerGrid, i);
                    Grid.SetColumn(innerGrid, j);
                    innerGridList[i, j] = innerGrid;
                    gameGraphicsGrid.Children.Add(innerGrid);
                }
            }
        }

        /// <summary>
        /// Finds out which cell the mouse is currently hovering over.
        /// </summary>
        /// <param name="frame">Connecting to a instance of GameFrame to send graphical input</param>
        public void GetMousePosition(GameFrame frame)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (innerGridList[i, j].IsMouseOver)
                    {
                        int[] chosenCoords = new int[2];
                        int row = (int)innerGridList[i, j].GetValue(Grid.RowProperty);
                        int column = (int)innerGridList[i, j].GetValue(Grid.ColumnProperty);
                        chosenCoords[0] = row;
                        chosenCoords[1] = column;
                        frame.GraphicalInput(chosenCoords);
                        break;
                    }
                }
            }
        }
    }
}
