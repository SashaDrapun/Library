using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Librarian
    {
        public string FioLibrarian { get => fioLibrarian; set => fioLibrarian = value; }
        public string ContactNumber { get => contactNumber; set => contactNumber = value; }
        public string Email { get => email; set => email = value; }
        public string PasswordLibrarian { get => passwordLibrarian; set => passwordLibrarian = value; }

        public Librarian(string fioLibrarian, string contactNumber, string email, string passwordLibrarian)
        {
            this.fioLibrarian = fioLibrarian;
            this.contactNumber = contactNumber;
            this.email = email;
            this.passwordLibrarian = passwordLibrarian;
        }

        private string fioLibrarian;
        private string contactNumber;
        private string email;
        private string passwordLibrarian;
    }
}
