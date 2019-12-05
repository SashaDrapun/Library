using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Library
{
    public class DatabaseInserter : DatabaseHandler
    {
        public static void InsertIntoBookDelivery(BookDelivery bookDelivery)
        {
            int idReader = DatabaseSelectorSomeInformation.GetIdReader(bookDelivery.FioReader);
            int idLibrarian = DatabaseSelectorSomeInformation.GetIdLibrarian(bookDelivery.FioLibrarian);

            string query = "Insert into BookDelivery values(default," +
                "" + idReader + ","+
                "" + bookDelivery.IdInstances + ","+
                "" + idLibrarian + "," +
                "'" + bookDelivery.DateOfIssue + "',"+
                " null)"; 

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            command.ExecuteNonQuery();

            command.Dispose();
            Connection.Close();
        }

        public static void InsertIntoBooks(Book book)
        {
            int idAutor = DatabaseSelectorSomeInformation.GetIdAutor(book.FioAutor);

            string query = "Insert into Books values(default," +
               "'" + book.NameBook + "'," +
               "" + idAutor + ","+
               "" + "0,"+
               "'" + book.Category + "',"+
               "'" + book.Picture + "',"+
               "" + book.YearOfIssue + ")";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            command.ExecuteNonQuery();

            command.Dispose();
            Connection.Close();
        }

        public static void InsertIntoInstances(Instance instance)
        {
            int idBook = DatabaseSelectorSomeInformation.GetIdBook(instance.NameBook);

            string query = "Insert into Instances values(" +
                "" + instance.IdInstance + "," +
                "" + idBook + ")";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            command.ExecuteNonQuery();

            command.Dispose();
            Connection.Close();
        }

        public static void InsertIntoReaders(Reader reader)
        {
            string query = "Insert into Readers values(default," +
                "'" + reader.FioReader + "'," +
                 "'" + reader.ContactNumber +"'," +
                  "'" + reader.Email +"')";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            command.ExecuteNonQuery();

            command.Dispose();
            Connection.Close();
        }

        public static void InsertIntoAutors(Autor autor)
        {
            string query = "Insert into Autors values(default," +
              "'" + autor.FioAutor + "'," +
                "'" + autor.Biography + "')";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            command.ExecuteNonQuery();

            command.Dispose();
            Connection.Close();
        }

        public static void InsertIntoLibrarians(Librarian librarian)
        {
            string query = "Insert into Librarians values(default," +
              "'" + librarian.FioLibrarian + "'," +
              "'" + librarian.ContactNumber + "'," +
              "'" + librarian.Email + "'," +
                "'" + librarian.PasswordLibrarian + "')";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            command.ExecuteNonQuery();

            command.Dispose();
            Connection.Close();
        }
    }
}
