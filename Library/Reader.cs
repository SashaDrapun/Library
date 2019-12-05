using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Reader
    {
        public string FioReader { get => fioReader; set => fioReader = value; }
        public string ContactNumber { get => contactNumber; set => contactNumber = value; }
        public string Email { get => email; set => email = value; }

        public Reader(string fioReader, string contactNumber, string email)
        {
            this.FioReader = fioReader;
            this.ContactNumber = contactNumber;
            this.Email = email;
        }

        private string fioReader;
        private string contactNumber;
        private string email;
    }
}
