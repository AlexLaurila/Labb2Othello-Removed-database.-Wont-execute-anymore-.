using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OthelloServer
{
    /// <summary>
    /// Is the main window containing an instance of GameBoard.
    /// Contains buttons for starting a new game or exiting the program.
    /// </summary>
    public partial class GameFrame : Window
    {
        //Attributer
        private GameManager manager;
        private GameBoard board;

        //Konstruktor
        public GameFrame(GameManager manager)
        {
            InitializeComponent();
            board = new GameBoard();
            this.manager = manager;
        }

        //Metoder
        /// <summary>
        /// Exits the program
        /// </summary>
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Creates two instances of SetUpGameDialog and sends the information to GameManager to initialize the players.
        /// </summary>
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            SetUpGameDialog player = new SetUpGameDialog();
            player.ShowDialog();
            string player1Name = player.GetPlayerName();
            string player1Type = player.GetPlayerType();

            SetUpGameDialog player2 = new SetUpGameDialog();
            player2.ShowDialog();
            string player2Name = player2.GetPlayerName();
            string player2Type = player2.GetPlayerType();

            manager.InitializePlayer(player1Name, player1Type, player2Name, player2Type);
        }

        /// <summary>
        /// Sends matrix containing boardstate to GameBoard.
        /// Paints the new GameBoard after it finishes updating.
        /// </summary>
        /// <param name="matrix">Matrix containing current boardstate</param>
        public void UpdateBoard(int[,] matrix)
        {
            board.UpdateBoard(matrix, this);
            Grid.SetColumn(board, 1);
            gfxGrid.Children.RemoveAt(2);
            gfxGrid.Children.Add(board);
        }

        /// <summary>
        /// Forwards information from GameBoard to GameManager
        /// </summary>
        /// <param name="graphicChosenCell"></param>
        public void GraphicalInput(int[] graphicChosenCell)
        {
            manager.GraphicalInput(graphicChosenCell);
        }

        /// <summary>
        /// Registers when button E is pressed on keyboard and calls another method for tracking mouse position.
        /// </summary>
        private void GfxGrid_KeyDown_E(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.E)
                board.GetMousePosition(this);
        }
    }
}
