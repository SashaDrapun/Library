using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Library
{
    public class DatabaseUpdater : DatabaseHandler
    {
        public static void UpdateBookDelivery(int idInstace,string bdDate)
        {
            string query = "update BookDelivery set returnDate = '" + bdDate + "' where returnDate is null and idInstance = " + idInstace;

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            command.ExecuteNonQuery();

            command.Dispose();
            Connection.Close();
        }
    }
}
