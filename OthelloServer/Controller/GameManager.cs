using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Media;

namespace OthelloServer
{
    /// <summary>
    /// Controller class, handles all communcation between Model and View. 
    /// In charge of players turns and ending the game when no more moves can be made.
    /// </summary>
    public class GameManager
    {
        //Attributer
        private AutoResetEvent resetEvent = new AutoResetEvent(false);
        private GameFrame mainWindow;
        private TcpListener server;
        private int[] graphicChosenCell;

        //Konstruktor
        public GameManager()
        {
            IPAddress address = Dns.GetHostAddresses(Dns.GetHostName())[0];
            int port = 8000;
            server = new TcpListener(address, port);
            server.Start();

            DatabaseManager database = new DatabaseManager();
            database.Run(address, port);

            mainWindow = new GameFrame(this);
            mainWindow.Show();
        }

        //Metoder
        /// <summary>
        /// Swapping between player1 and player2, granting them up to 1 move per swap. 
        /// Updates GameBoard to reflect current boardstate in GameGrid.
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        private void Run(Player player1, Player player2)
        {
            GameGrid grid = new GameGrid();
            int playerTurn = -1;
            int noMoves = 0;

            do
            {
                mainWindow.Dispatcher.Invoke(() =>
                {
                    mainWindow.UpdateBoard(grid.GetMatrix());
                });

                playerTurn++;
                playerTurn %= 2;
                if (playerTurn == 0)
                    noMoves = PlayerTurnExecute(player1, grid, noMoves);
                else
                    noMoves = PlayerTurnExecute(player2, grid, noMoves);

            } while (noMoves < 2);

            CalculateWinner(grid, player1, player2);
            player1.FinishGame();
        }

        /// <summary>
        /// Instantiates a Player. Executes Run() in a new thread when two players have been created.
        /// </summary>
        /// <param name="playerName">A string containing the players name</param>
        /// <param name="playerType">A string containing information about the player type</param>
        /// <param name="playerName2">Same as above but for player 2</param>
        /// <param name="playerType2">Same as above but for player 2</param>
        public void InitializePlayer(string playerName, string playerType, string playerName2, string playerType2)
        {
            Player player1;
            Player player2;

            if (playerType == "Human")
                player1 = new HumanPlayer(playerName, 1, this);
            else if (playerType == "Local")
                player1 = new LocalComputerPlayer(playerName, 1);
            else
                player1 = new RemoteComputerPlayer(playerName, 1, server);

            if (playerType2 == "Human")
                player2 = new HumanPlayer(playerName, 2, this);
            else if (playerType2 == "Local")
                player2 = new LocalComputerPlayer(playerName, 2);
            else
                player2 = new RemoteComputerPlayer(playerName, 2, server);

            Thread t = new Thread(() => Run(player1, player2));
            t.Start();
        }

        /// <summary>
        /// Waits for graphical user input before returning coordinates.
        /// </summary>
        /// <returns>User chosen coordinates</returns>
        public int[] RequestGraphicalInput()
        {
            resetEvent.WaitOne();
            return graphicChosenCell;
        }

        /// <summary>
        /// Updates coordinates for graphicalChosenCoords.
        /// </summary>
        /// <param name="graphicChosenCell">Coordinates to be updated into graphicalChosenCell.</param>
        public void GraphicalInput(int[] graphicChosenCell)
        {
            this.graphicChosenCell = graphicChosenCell;
            resetEvent.Set();
        }

        /// <summary>
        /// Handles a single turn for a player
        /// </summary>
        /// <param name="player">Which players turn it is</param>
        /// <param name="grid">The GameGrid used by current game</param>
        /// <param name="noMoves">Variable that keeps track on how many passes have been made by players recently.</param>
        /// <returns>Updated noMoves</returns>
        private int PlayerTurnExecute(Player player, GameGrid grid, int noMoves)
        {
            int[] chosenCoords;
            List<int[]> legalMoves;

            legalMoves = grid.GetLegalMoves(player.GetId());
            if (legalMoves.Count == 0)
            {
                noMoves++;
                return noMoves;
            }

            while (true)
            {
                chosenCoords = player.NextMove(legalMoves);
                if (grid.IsValidMove(player.GetId(), chosenCoords[0], chosenCoords[1]).Count > 0)
                    break;
                SystemSounds.Beep.Play();
            }

            grid.AddMove(player.GetId(), chosenCoords[0], chosenCoords[1]);
            noMoves = 0;
            return noMoves;
        }

        /// <summary>
        /// Counts the final score and shows results in a new dialog.
        /// </summary>
        /// <param name="grid">The GameGrid used by current game</param>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        private void CalculateWinner(GameGrid grid, Player player1, Player player2)
        {
            int[,] matrix = grid.GetMatrix();
            int blackTokens = 0;
            int whiteTokens = 0;

            //Counts all black and white tokens
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    if (matrix[i, j] == 1)
                        whiteTokens++;
                    else if (matrix[i, j] == 2)
                        blackTokens++;
                }
            }

            //Presents winner in a dialog
            mainWindow.Dispatcher.Invoke(() =>
            {
                if (whiteTokens > blackTokens)
                {
                    WinnerDialog winner = new WinnerDialog(player1.GetName());
                    winner.ShowDialog();
                }
                else if (whiteTokens < blackTokens)
                {
                    WinnerDialog winner = new WinnerDialog(player2.GetName());
                    winner.ShowDialog();
                }
                else
                {
                    DrawnDialog draw = new DrawnDialog();
                    draw.ShowDialog();
                }
            });
        }
    }
}
