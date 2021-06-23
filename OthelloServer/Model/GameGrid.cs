using System.Collections.Generic;

namespace OthelloServer
{
    /// <summary>
    /// Contains the matrix with current boardstate.
    /// Counts all available moves for a player.
    /// Adds tokens and flips all affected tokens.
    /// </summary>
    public class GameGrid
    {
        //Attributer
        private int[,] matrix = new int[,] {
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 1, 2, 0, 0, 0},
            {0, 0, 0, 2, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0}
            };

        //Metoder
        /// <summary>
        /// Adds a token to chosen coordinates.
        /// Flips all tokens affected by the newly added token.
        /// </summary>
        /// <param name="player">White or Black player</param>
        /// <param name="xCoordinate">X coordinate for the new token</param>
        /// <param name="yCoordinate">Y coordinate for the new token</param>
        public void AddMove(int player, int xCoordinate, int yCoordinate)
        {
            List<int[]> flipTiles = IsValidMove(player, xCoordinate, yCoordinate);
            matrix[xCoordinate, yCoordinate] = player;
            for (int i = 0; i < flipTiles.Count; i++)
            {
                matrix[flipTiles[i][0], flipTiles[i][1]] = player;
            }
        }

        /// <summary>
        /// Counts all available moves for a player.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <returns>List with all available moves</returns>
        public List<int[]> GetLegalMoves(int player)
        {
            List<int[]> legalMoves = new List<int[]>();
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                for (int k = 0; k < matrix.GetLength(0); k++)
                {
                    if (IsValidMove(player, j, k).Count > 0)
                    {
                        int[] legalMove = new int[2];
                        legalMove[0] = j;
                        legalMove[1] = k;
                        legalMoves.Add(legalMove);
                    }
                }
            }
            return legalMoves;
        }

        /// <summary>
        /// Checks if a token can be added to a specific cell.
        /// Returns all tokens that would be affected by the new token.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="startX">X coordinate for the specific cell</param>
        /// <param name="startY">Y coordinate for the specific cell</param>
        /// <returns></returns>
        public List<int[]> IsValidMove(int player, int startX, int startY)
        {
            int enemyPlayer, currentX, currentY;
            List<int[]> flipTiles = new List<int[]>();

            //Lists all possible directions
            int[,] directions = new int[,] {
                {0, 1}, {1, 1}, {-1, 1}, {-1, 0}, 
                {1, 0}, {1, -1}, {0, -1}, {-1, -1} };

            if (player == 1)
                enemyPlayer = 2;
            else
                enemyPlayer = 1;

            if (CoordinatesOnBoard(startX, startY) && matrix[startX, startY] != 0)
                return flipTiles;
            else
            {
                //Test all directions from starting point
                for (int i = 0; i < directions.Length / 2; i++)
                {
                    currentX = startX + directions[i, 0];
                    currentY = startY + directions[i, 1];

                    //Keeps going in the same direction while current coordinates has an enemy token
                    while(CoordinatesOnBoard(currentX, currentY) && matrix[currentX, currentY] == enemyPlayer)
                    {
                        currentX += directions[i, 0];
                        currentY += directions[i, 1];
                    }
                    //When current coordinates has a friendly token, backtrack to starting point and save every token inbetween
                    if (CoordinatesOnBoard(currentX, currentY) && matrix[currentX, currentY] == player)
                    {
                        while(true)
                        {
                            currentX -= directions[i, 0];
                            currentY -= directions[i, 1];
                            if (currentX == startX && currentY == startY)
                                break;
                            int[] tileToFlip = new int[2];
                            tileToFlip[0] = currentX;
                            tileToFlip[1] = currentY;
                            flipTiles.Add(tileToFlip);
                        }
                    }
                }
                return flipTiles;
            }
        }

        /// <summary>
        /// Checks wether targeted cell is inside the board.
        /// </summary>
        /// <param name="x">X coordinate of cell</param>
        /// <param name="y">Y coordinate of cell</param>
        /// <returns></returns>
        private bool CoordinatesOnBoard(int x, int y)
        {
            if (x < 0 || x >= matrix.GetLength(0) || y < 0 || y >= matrix.GetLength(0))
                return false;
            return true;
        }

        public int[,] GetMatrix()
        {
            return matrix;
        }
    }
}
