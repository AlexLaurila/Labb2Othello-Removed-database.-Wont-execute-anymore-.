using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.SqlClient;
using System.IO;

namespace OthelloClient
{
    /// <summary>
    /// Main class for Client.
    /// Receives server information from database.
    /// Handles all communication with the server.
    /// </summary>
    [Serializable]
    class Client
    {
        static void Main(string[] args)
        {
            int port;
            string hostIP;

            //Opens connection with database and reads IP and Port to server.
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = @"Data Source=sqlutb2-db.hb.se,56077;Initial Catalog=oopc1905;User Id=oopc1905;Password=wp6696";
                connection.Open();

                SqlCommand myCommand = new SqlCommand("EXEC GetConnectionDetails @groupId = '1905'", connection);
                using (SqlDataReader myDataReader = myCommand.ExecuteReader())
                {
                    myDataReader.Read();
                    hostIP = myDataReader["ipAddress"].ToString();
                    port = Convert.ToInt32(myDataReader["port"].ToString());
                }
                connection.Close();
            }

            //Opens connection with server
            TcpClient socket = new TcpClient(hostIP, port);
            BinaryFormatter formatter = new BinaryFormatter();
            int[] clientAnswer = new int[2];
            List<int[]> legalMoves;
            NetworkStream stream = socket.GetStream();

            //Communication to/from server.
            while (true)
            {
                try
                {
                    legalMoves = (List<int[]>)formatter.Deserialize(stream);

                    Random numberGenerator = new Random();
                    int randomNumber = numberGenerator.Next(0, legalMoves.Count());
                    clientAnswer[0] = legalMoves[randomNumber][0];
                    clientAnswer[1] = legalMoves[randomNumber][1];

                    formatter.Serialize(stream, clientAnswer);
                }
                catch(IOException)
                {
                    break;
                }
            }
        }
    }
}
