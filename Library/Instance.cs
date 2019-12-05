using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Instance
    {
        public Instance(int idInstance, string nameBook)
        {
            this.idInstance = idInstance;
            this.nameBook = nameBook;
        }

        public int IdInstance { get => idInstance; set => idInstance = value; }
        public string NameBook { get => nameBook; set => nameBook = value; }

        private int idInstance;
        private string nameBook;
    }
}
