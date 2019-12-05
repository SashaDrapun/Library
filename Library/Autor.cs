using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Autor
    {
        public Autor(string fioAutor, string biography)
        {
            this.fioAutor = fioAutor;
            this.biography = biography;
        }

        public string FioAutor { get => fioAutor; set => fioAutor = value; }
        public string Biography { get => biography; set => biography = value; }

        private string fioAutor;
        private string biography;
    }
}
