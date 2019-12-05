using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class SearchSettings
    {
        public SearchSettings(string column, string value)
        {
            this.column = column;
            this.value = value;
        }

        public string Column { get => column; set => column = value; }
        public string Value { get => value; set => this.value = value; }

        private string column;
        private string value;
    }
}
