using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Book
    {
        public string NameBook { get => nameBook; set => nameBook = value; }
        public string FioAutor { get => fioAutor; set => fioAutor = value; }
        public int CountInStock { get => countInStock; set => countInStock = value; }
        public string Category { get => category; set => category = value; }
        public string Picture { get => picture; set => picture = value; }
        public int YearOfIssue { get => yearOfIssue; set => yearOfIssue = value; }

        public Book(string nameBook, string fioAutor, int countInStock, string category, string picture, int yearOfIssue)
        {
            this.nameBook = nameBook;
            this.fioAutor = fioAutor;
            this.countInStock = countInStock;
            this.category = category;
            this.picture = picture;
            this.yearOfIssue = yearOfIssue;
        }

        private string nameBook;
        private string fioAutor;
        private int countInStock;
        private string category;
        private string picture;
        private int yearOfIssue;
    }
}
