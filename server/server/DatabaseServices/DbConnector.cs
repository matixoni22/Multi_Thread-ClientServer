using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace server.DatabaseServices
{
    public class DbConnector
    {
        private string connectionString;
        protected MySqlConnection connection;

        protected void ConnectToDatabase()
        {
            this.connectionString = ConfigurationManager.AppSettings["DbConnection"];  
        }
        protected bool OpenConnection()
        {
            try{
                connection = new MySqlConnection(connectionString);
                connection.Open();
                return true;
            }
            catch(MySqlException ex){
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        protected bool ExitConnection()
        {
            try{
                connection.Close();
                return true;
            }
            catch(MySqlException ex){
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

    }
}

