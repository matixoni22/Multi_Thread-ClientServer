using System;
using System.Configuration;
using LinqToDB.DataProvider.MySql;

namespace server.DatabaseServices
{
    public class DbConnector
    {
        public void ConnectToDatabase()
        {
            string connectionString = ConfigurationManager.AppSettings["DbConnection"];  
        }
    }
}

