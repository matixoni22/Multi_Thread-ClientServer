using System;
using MySql.Data.MySqlClient;

namespace DatabaseServices
{
    public class DbConnector
    {
        public MySqlDataReader reader;
        public MySqlCommand command;

        private MySqlConnection connection;
        private string connectionString;

        public void ConnectToDatabase()
        {
            connectionString = "server=localhost;uid=root;pwd=123mati123;database=Server_database;";

            try
            {
                connection =  new MySqlConnection(connectionString);
                connection.Open();

            }
            catch(MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

