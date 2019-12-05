using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class BookDelivery
    {
        public string DateOfIssue { get => dateOfIssue; set => dateOfIssue = value; }
        public string ReturnDate { get => returnDate; set => returnDate = value; }
        public string FioReader { get => fioReader; set => fioReader = value; }
        public string FioLibrarian { get => fioLibrarian; set => fioLibrarian = value; }
        public int IdInstances { get => idInstances; set => idInstances = value; }
        public string NameBook { get => nameBook; set => nameBook = value; }

        public BookDelivery(string dateOfIssue, string returnDate, string fioReader, string nameBook, string fioLibrarian, int idInstances)
        {
            this.dateOfIssue = dateOfIssue;
            this.returnDate = returnDate;
            this.fioReader = fioReader;
            this.nameBook = nameBook;
            this.fioLibrarian = fioLibrarian;
            this.idInstances = idInstances;
        }

        public BookDelivery(string dateOfIssue, string fioReader, string nameBook, string fioLibrarian, int idInstances)
        {
            this.dateOfIssue = dateOfIssue;
            this.fioReader = fioReader;
            this.nameBook = nameBook;
            this.fioLibrarian = fioLibrarian;
            this.idInstances = idInstances;
        }

        private string dateOfIssue;
        private string returnDate;
        private string fioReader;
        private string nameBook;
        private string fioLibrarian;
        private int idInstances;

    }
}
