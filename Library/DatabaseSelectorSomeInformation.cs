using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Library
{
    public class DatabaseSelectorSomeInformation : DatabaseHandler
    {
        public static int GetIdReader(string fioReader)
        {
            string query = "select idReader from Readers where fioReader = '" + fioReader + "'";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            int result = Convert.ToInt32(command.ExecuteScalar());

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static int GetIdLibrarian(string fioLibrarian)
        {
            string query = "select idLibrarian from Librarians where fioLibrarian = '" + fioLibrarian + "'";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            int result = Convert.ToInt32(command.ExecuteScalar());

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static bool IsInstancesExists(int idInstance)
        {
            string query = "select idInstance from Instances where idInstance = " + idInstance;

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            bool result =command.ExecuteScalar() == null?false:true;

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static int GetIdBook(string nameBook)
        {
            string query = "select idBook from Books where nameBook = '" + nameBook + "'";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            int result = Convert.ToInt32(command.ExecuteScalar());

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static int GetIdAutor(string fioAutor)
        {
            string query = "select idAutor from Autors where fioAutor = '" + fioAutor + "'";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            int result = Convert.ToInt32(command.ExecuteScalar());

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static bool IsBookExists(string nameBook)
        {
            string query = "select idBook from Books where nameBook = '" + nameBook + "'";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            bool result = command.ExecuteScalar() == null ? false : true;

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static bool IsReaderExists(string fioReader)
        {
            string query = "select idReader from Readers where fioReader = '" + fioReader + "'";
            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            bool result = command.ExecuteScalar() == null ? false : true;

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static bool IsAutorExists(string fioAutor)
        {
            string query = "select idAutor from Autors where fioAutor = '" + fioAutor + "'";
            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            bool result = command.ExecuteScalar() == null ? false : true;

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static bool IsLibrarianExists(string fioLibrarian)
        {
            string query = "select idLibrarian from Librarians where fioLibrarian = '" + fioLibrarian + "'";
            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            bool result = command.ExecuteScalar() == null ? false : true;

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static string GetLibrarianEmail(string fioLibrarian)
        {
            string query = "select email from Librarians where fioLibrarian = '" + fioLibrarian + "'";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            string result = command.ExecuteScalar().ToString();

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static string GetLibrarianPassword(string fioLibrarian)
        {
            string query = "select passwordLibrarian from Librarians where fioLibrarian = '" + fioLibrarian + "'";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            string result = command.ExecuteScalar().ToString();

            command.Dispose();
            Connection.Close();
            return result;
        }


        public static string GetReaderEmail(string fioReader)
        {
            string query = "select email from Readers where fioReader = '" + fioReader + "'";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            string result = command.ExecuteScalar().ToString();

            command.Dispose();
            Connection.Close();
            return result;
        }
    }
}
