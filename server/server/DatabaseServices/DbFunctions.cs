using System;

namespace server.DatabaseServices
{
    public class DbFunctions : DbConnector
    {
        public DbFunctions()
        {
            base.ConnectToDatabase();
        }
        public void LoginValidation(string userName, string password)
        {
            
        }

    }
}

