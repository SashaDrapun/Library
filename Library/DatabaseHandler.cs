using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Library
{
    public abstract class DatabaseHandler
    {
        public static string ConnectionString { get => connectionString; set => connectionString = value; }
        public static MySqlConnection Connection { get => connection; set => connection = value; }

        private static string connectionString = "server=localhost; Port=3305;user=root;database=Library;password=1234;";
        private static MySqlConnection connection = new MySqlConnection(ConnectionString);
    }
}
