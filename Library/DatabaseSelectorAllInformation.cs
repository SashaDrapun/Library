using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Library
{
    public class DatabaseSelectorAllInformation : DatabaseHandler
    {
        public static List<BookDelivery> GetAllBookDelivery(List<QuerySettings> searchSettings)
        {
            string query = "select dateOfIssue,returnDate,fioReader,nameBook,fioLibrarian,BookDelivery.idInstance" +
            " from BookDelivery inner join Books inner join Readers" +
            " inner join Librarians inner join Instances" +
            " on BookDelivery.idReader = Readers.idReader" +
            " and BookDelivery.idInstance = Instances.idInstance" +
            " and BookDelivery.idLibrarian = Librarians.idLibrarian" +
            " and Books.idBook = Instances.idBook ";

            bool wasWhereWritten = false;

            for (int i = 0; i < searchSettings.Count; i++)
            {
                if(!wasWhereWritten)
                {
                    query += " where " + searchSettings[i].Column + " Like '" + searchSettings[i].Value + "%'";
                    wasWhereWritten = true;
                }
                else
                {
                    query += " and " + searchSettings[i].Column + " Like '" + searchSettings[i].Value + "%'";
                }
                
            }

            List<BookDelivery> result = new List<BookDelivery>();

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {

                result.Add(new BookDelivery(DateParser.FromStringToDate(reader[0].ToString()),
                DateParser.FromStringToDate(reader[1].ToString()),
                reader[2].ToString(),
                reader[3].ToString(), reader[4].ToString(), Convert.ToInt32(reader[5])));
            }

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static List<BookDelivery> GetAllBookDelivery(string fioReader)
        {
            string query = "select dateOfIssue,returnDate,fioReader,nameBook,fioLibrarian,BookDelivery.idInstance" +
            " from BookDelivery inner join Books inner join Readers" +
            " inner join Librarians inner join Instances" +
            " on BookDelivery.idReader = Readers.idReader" +
            " and BookDelivery.idInstance = Instances.idInstance" +
            " and BookDelivery.idLibrarian = Librarians.idLibrarian" +
            " and Books.idBook = Instances.idBook "+
            " where fioReader = '"+fioReader + "'" +
            " and returnDate is null";

            List<BookDelivery> result = new List<BookDelivery>();

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {

                result.Add(new BookDelivery(DateParser.FromStringToDate(reader[0].ToString()),
                DateParser.FromStringToDate(reader[1].ToString()),
                reader[2].ToString(),
                reader[3].ToString(), reader[4].ToString(), Convert.ToInt32(reader[5])));
            }

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static List<Librarian> GetAllLibrarians()
        {
            string query = "select fioLibrarian,contactNumber,email,passwordLibrarian from Librarians";

            List<Librarian> result = new List<Librarian>();

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Librarian(reader[0].ToString(), reader[1].ToString(),
                    reader[2].ToString(), reader[3].ToString()));
            }

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static List<Librarian> GetAllLibrarians(List<QuerySettings> searchSettings)
        {
            string query = "select fioLibrarian,contactNumber,email,passwordLibrarian from Librarians";

            bool wasWhereWritten = false;

            for (int i = 0; i < searchSettings.Count; i++)
            {
                if (!wasWhereWritten)
                {
                    query += " where " + searchSettings[i].Column + " Like '" + searchSettings[i].Value + "%'";
                    wasWhereWritten = true;
                }
                else
                {
                    query += " and " + searchSettings[i].Column + " Like '" + searchSettings[i].Value + "%'";
                }

            }

            List<Librarian> result = new List<Librarian>();

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Librarian(reader[0].ToString(), reader[1].ToString(),
                    reader[2].ToString(), reader[3].ToString()));
            }

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static List<Reader> GetAllReaders()
        {
            string query = "select fioReader,contactNumber,email from Readers";

            List<Reader> result = new List<Reader>();

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Reader(reader[0].ToString(),reader[1].ToString(),reader[2].ToString()));
            }

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static List<Reader> GetAllReaders(List<QuerySettings> searchSettings)
        {
            string query = "select fioReader,contactNumber,email from Readers";

            bool wasWhereWritten = false;

            for (int i = 0; i < searchSettings.Count; i++)
            {
                if (!wasWhereWritten)
                {
                    query += " where " + searchSettings[i].Column + " Like '" + searchSettings[i].Value + "%'";
                    wasWhereWritten = true;
                }
                else
                {
                    query += " and " + searchSettings[i].Column + " Like '" + searchSettings[i].Value + "%'";
                }

            }

            List<Reader> result = new List<Reader>();

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Reader(reader[0].ToString(), reader[1].ToString(), reader[2].ToString()));
            }


            command.Dispose();
            Connection.Close();
            return result;
        }

        public static List<Autor> GetAllAutors()
        {
            string query = "select fioAutor,biography from Autors";

            List<Autor> result = new List<Autor>();

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Autor(reader[0].ToString(),reader[1].ToString()));
            }

            Connection.Close();
            command.Dispose();
            return result;
        }

        public static List<Autor> GetAllAutors(List<QuerySettings> searchSettings)
        {
            string query = "select fioAutor,biography from Autors";

            bool wasWhereWritten = false;

            for (int i = 0; i < searchSettings.Count; i++)
            {
                if (!wasWhereWritten)
                {
                    query += " where " + searchSettings[i].Column + " Like '" + searchSettings[i].Value + "%'";
                    wasWhereWritten = true;
                }
                else
                {
                    query += " and " + searchSettings[i].Column + " Like '" + searchSettings[i].Value + "%'";
                }

            }

            List<Autor> result = new List<Autor>();

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Autor(reader[0].ToString(), reader[1].ToString()));
            }

            Connection.Close();
            command.Dispose();
            return result;
        }

        public static List<Book> GetAllBooks()
        {
            string query = "select nameBook,fioAutor,countInStock,category,picture,yearOfIssue" +
            " from Books inner join Autors" +
            " on Books.idAutor = Autors.idAutor";

            List<Book> result = new List<Book>();

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Book(reader[0].ToString(), reader[1].ToString(),
                    Convert.ToInt32(reader[2]), reader[3].ToString(), reader[4].ToString(),
                 Convert.ToInt32(reader[5])));
            }

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static List<Book> GetAllBooks(List<QuerySettings> searchSettings)
        {
            string query = "select nameBook,fioAutor,countInStock,category,picture,yearOfIssue"+
            " from Books inner join Autors"+
            " on Books.idAutor = Autors.idAutor";

            bool wasWhereWritten = false;

            for (int i = 0; i < searchSettings.Count; i++)
            {
                if (!wasWhereWritten)
                {
                    query += " where " + searchSettings[i].Column + " Like '" + searchSettings[i].Value + "%'";
                    wasWhereWritten = true;
                }
                else
                {
                    query += " and " + searchSettings[i].Column + " Like '" + searchSettings[i].Value + "%'";
                }

            }

            List<Book> result = new List<Book>();

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Book(reader[0].ToString(),reader[1].ToString(),
                    Convert.ToInt32(reader[2]),reader[3].ToString(),reader[4].ToString(),
                     Convert.ToInt32(reader[5])));
            }

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static List<Instance> GetAllInstances(List<QuerySettings> searchSettings)
        {
            string query = "select idInstance,nameBook "+
            " from Instances inner join Books"+
            " on Instances.idBook = Books.idBook";

            bool wasWhereWritten = false;

            for (int i = 0; i < searchSettings.Count; i++)
            {
                if (!wasWhereWritten)
                {
                    query += " where " + searchSettings[i].Column + " Like '" + searchSettings[i].Value + "%'";
                    wasWhereWritten = true;
                }
                else
                {
                    query += " and " + searchSettings[i].Column + " Like '" + searchSettings[i].Value + "%'";
                }
            }

            List<Instance> result = new List<Instance>();

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Instance(Convert.ToInt32(reader[0]), reader[1].ToString()));
            }

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static List<Instance> GetAllInstances()
        {
            string query = "select idInstance,nameBook " +
            " from Instances inner join Books" +
            " on Instances.idBook = Books.idBook";

            List<Instance> result = new List<Instance>();

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Instance(Convert.ToInt32(reader[0]), reader[1].ToString()));
            }

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static List<int> GetInstancesInStock(string bookName)
        {
            string query = "select idInstance,nameBook " +
           " from Instances inner join Books" +
           " on Instances.idBook = Books.idBook" +
           " where not exists(Select * from BookDelivery where BookDelivery.idInstance = Instances.idInstance"+
           " and returnDate is null)"+
           " and nameBook = '" + bookName + "'";

            List<int> result = new List<int>();

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(Convert.ToInt32(reader[0]));
            }

            command.Dispose();
            Connection.Close();
            return result;
        }

        public static List<string> GetFioReadersWhoHaveABookOnHand()
        {
            string query = "select fioReader from Readers"+
            " where exists(Select * from BookDelivery where BookDelivery.idReader = Readers.idReader" +
            " and returnDate is null)";

            List<string> result = new List<string>();

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(reader[0].ToString());
            }

            command.Dispose();
            Connection.Close();
            return result;
        }
    }
}
