using System;
using System.Data.SqlClient;
using System.Net;

namespace OthelloServer
{
    /// <summary>
    /// Updates database with server IP and port
    /// </summary>
    public class DatabaseManager
    {
        public void Run(IPAddress address, int port)
        {
            using (SqlConnection connection = new SqlConnection()) //Outdated DB info, removed. Program will not execute
            {
                connection.ConnectionString = @"";
                connection.Open();

                string sql = $""; //Outdated DB info, removed. Program will not execute
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            } 
        }
    }
}
