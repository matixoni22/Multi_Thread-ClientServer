using System;
using System.Text;
using System.Linq;
using MySql.Data.MySqlClient;


namespace server.DatabaseServices
{
    public class DbFunctions : DbConnector
    {
        public bool LoginValidation(string userName, string password)
        {
            ConnectToDatabase();
            if (OpenConnection() == true)
            {
                try{
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText =   "SELECT Login" +
                        "FROM LoginValidation" +
                        "WHERE Password = @password and login = @userName;";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@userName", userName);

                    MySqlDataReader reader =  cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        return true;
                    }
                    return false;

                }
                catch(MySqlException ex){
                    Console.WriteLine(ex.ToString());
                    return false;
                }


            }
            else
            {
                return false;
            }
        }

    }
}

