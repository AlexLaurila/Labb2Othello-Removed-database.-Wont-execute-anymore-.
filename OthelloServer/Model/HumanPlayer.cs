using System.Collections.Generic;

namespace OthelloServer
{
    /// <summary>
    /// Handles all human player logic.
    /// Lets the player choose next move when requested.
    /// </summary>
    public class HumanPlayer : Player
    {
        //Attributer
        private GameManager manager;

        //Konstruktor
        public HumanPlayer(string playerName, int playerId, GameManager manager)
            :base (playerName, playerId)
        {
            this.manager = manager;
        }

        //Metoder
        /// <summary>
        /// Lets player choose a cell from Graphical User Interface
        /// </summary>
        /// <param name="legalMoves">Contains all available legal moves. Not currently used by this class</param>
        /// <returns>Returns coordinates for the chosen move</returns>
        public override int[] NextMove(List<int[]> legalMoves)
        {
            int[] chosenMove = manager.RequestGraphicalInput();
            return chosenMove;
        }
    }
}
