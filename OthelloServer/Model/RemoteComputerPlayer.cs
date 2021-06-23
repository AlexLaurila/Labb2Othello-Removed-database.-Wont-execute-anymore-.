using System;
using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OthelloServer
{
    /// <summary>
    /// Contains information about a remote computer player.
    /// Lets the player choose next move when requested by Controller.
    /// Creates a new thread every time this class is instantiated.
    /// </summary>
    [Serializable]
    class RemoteComputerPlayer : Player
    {
        //Attribut
        private AutoResetEvent senderResetEvent = new AutoResetEvent(false);
        private AutoResetEvent receiverResetEvent = new AutoResetEvent(false);
        private List<int[]> legalMoves;
        private int[] clientAnswer;

        //Konstruktor
        public RemoteComputerPlayer(string playerName, int playerId, TcpListener server)
            : base(playerName, playerId)
        {
            InitializeRemoteComputerPlayer(server);
        }

        //Metoder
        /// <summary>
        /// Updates legalMoves and waits for a event before returning with clientAnswer.
        /// </summary>
        /// <param name="legalMoves">Contains all available legal moves.</param>
        /// <returns>Returns coordinates for the chosen move</returns>
        public override int[] NextMove(List<int[]> legalMoves)
        {
            this.legalMoves = legalMoves;
            senderResetEvent.Set();
            receiverResetEvent.WaitOne();
            return clientAnswer;
        }

        /// <summary>
        /// Establishes connection with a client in a new thread.
        /// Sends all legal moves to client and lets client choose a move.
        /// </summary>
        private void InitializeRemoteComputerPlayer(TcpListener server)
        {
            new Thread(() =>
            {
                BinaryFormatter formatter = new BinaryFormatter();
                TcpClient client = server.AcceptTcpClient();
                IPAddress clientAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                NetworkStream stream = client.GetStream();

                //While game is not over, send legalmoves to client and receive answer from client.
                while (!gameOver)
                {
                    if (senderResetEvent.WaitOne(1000))
                    {
                        try
                        {
                            formatter.Serialize(stream, legalMoves);
                            clientAnswer = (int[])formatter.Deserialize(stream);
                            receiverResetEvent.Set();
                        }
                        catch (IOException)
                        {
                            gameOver = true;
                            break;
                        }
                    }
                }
            }).Start();
        }
    }
}
