using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Library
{
    public class DatabaseDeleter : DatabaseHandler
    {
        public static void DeleteFromBooks(string nameBook)
        {
            string query = "Delete from Books where nameBook = '" + nameBook + "'";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            command.ExecuteNonQuery();

            command.Dispose();
            Connection.Close();
        }
    }
}
