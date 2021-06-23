using System;
using System.Collections.Generic;
using System.Linq;

namespace OthelloServer
{
    /// <summary>
    /// Handles all local computer player logic.
    /// </summary>
    public class LocalComputerPlayer : Player
    {
        public LocalComputerPlayer(string playerName, int playerId)
            : base(playerName, playerId)
        {

        }

        /// <summary>
        /// Picks a random cell from legalMoves
        /// </summary>
        /// <param name="legalMoves">List containing all the legal moves</param>
        /// <returns>Returns a cell from legalMoves</returns>
        public override int[] NextMove(List<int[]> legalMoves)
        {
            Random numberGenerator = new Random();
            int randomNumber = numberGenerator.Next(0, legalMoves.Count());
            return legalMoves[randomNumber];
        }
    }
}
