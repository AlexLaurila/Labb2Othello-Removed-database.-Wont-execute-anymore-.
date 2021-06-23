using System.Collections.Generic;
using System.Threading;

namespace OthelloServer
{
    /// <summary>
    /// Superclass for all different types of players.
    /// Contains player name and player ID for all players. 
    /// Cannot be instantiated by itself as it is an abstract class.
    /// </summary>
    public abstract class Player
    {
        //Attributer
        protected string playerName;
        protected int playerId;
        protected static bool gameOver;

        //Konstruktorer
        public Player(string playerName, int playerId)
        {
            gameOver = false;
            this.playerId = playerId;
            this.playerName = playerName;
        }

        //Metoder
        public abstract int[] NextMove(List<int[]> legalMoves);

        public int GetId()
        {
            return playerId;
        }

        public string GetName()
        {
            return playerName;
        }

        /// <summary>
        /// Informs all subclasses that the game is over.
        /// </summary>
        public void FinishGame()
        {
            gameOver = true;
        }
    }
}
