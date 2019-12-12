﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Library
{
    public class DatabaseUpdater : DatabaseHandler
    {
        public static void UpdateBookDelivery(int idInstace, string bdDate)
        {
            string query = "update BookDelivery set returnDate = '" + bdDate + "' where returnDate is null and idInstance = " + idInstace;

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            command.ExecuteNonQuery();

            command.Dispose();
            Connection.Close();
        }

        public static void UpdateBooks(List<QuerySettings> searchSettings, string bookName)
        {
            string query = "update Books set ";

            for (int i = 0; i < searchSettings.Count; i++)
            {
                if (i == searchSettings.Count - 1)
                {
                    query += searchSettings[i].Column + "='" + searchSettings[i].Value + "'";
                }
                else
                {
                    query += searchSettings[i].Column + "='" + searchSettings[i].Value + "',";
                }
            }

            query += " where nameBook = '" + bookName + "'";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            command.ExecuteNonQuery();

            command.Dispose();
            Connection.Close();
        }

        public static void UpdateReaders(List<QuerySettings> searchSettings, string fioReader)
        {
            string query = "update Readers set ";

            for (int i = 0; i < searchSettings.Count; i++)
            {
                if (i == searchSettings.Count - 1)
                {
                    query += searchSettings[i].Column + "='" + searchSettings[i].Value + "'";
                }
                else
                {
                    query += searchSettings[i].Column + "='" + searchSettings[i].Value + "',";
                }
            }

            query += " where fioReader = '" + fioReader + "'";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            command.ExecuteNonQuery();

            command.Dispose();
            Connection.Close();
        }

        public static void UpdateAutors(List<QuerySettings> searchSettings, string fioAutor)
        {
            string query = "update Autors set ";

            for (int i = 0; i < searchSettings.Count; i++)
            {
                if (i == searchSettings.Count - 1)
                {
                    query += searchSettings[i].Column + "='" + searchSettings[i].Value + "'";
                }
                else
                {
                    query += searchSettings[i].Column + "='" + searchSettings[i].Value + "',";
                }
            }

            query += " where fioAutor = '" + fioAutor + "'";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            command.ExecuteNonQuery();

            command.Dispose();
            Connection.Close();
        }

        public static void UpdateLibrarians(List<QuerySettings> searchSettings, string fioLibrarian)
        {
            string query = "update Librarians set ";

            for (int i = 0; i < searchSettings.Count; i++)
            {
                if (i == searchSettings.Count - 1)
                {
                    query += searchSettings[i].Column + "='" + searchSettings[i].Value + "'";
                }
                else
                {
                    query += searchSettings[i].Column + "='" + searchSettings[i].Value + "',";
                }
            }

            query += " where fioLibrarian = '" + fioLibrarian + "'";

            Connection.Open();

            MySqlCommand command = new MySqlCommand(query, Connection);

            command.ExecuteNonQuery();

            command.Dispose();
            Connection.Close();
        }
    }
}
