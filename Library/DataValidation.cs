using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Library
{
    public static class DataValidation
    {
        public static bool IsEmailCorrect(string email)
        {
            Regex checkFIO = new Regex(@"[0-9a-z_.]+@[0-9a-z_^.]+.[a-z]{2,3}");

            return checkFIO.IsMatch(email) ? true : false;
        }

        public static bool IsPathOfFioCorrect(string partOfFio)
        {
            Regex checkFIO = new Regex(@"^[а-яА-Я]{2,}$");

            return checkFIO.IsMatch(partOfFio) ? true : false;
        }

        public static bool IsValueInteger(string value)
        {
            try
            {
                int integerValue = Convert.ToInt32(value);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsContactNumberCorrect(string phoneNumber)
        {
            Regex checkFIO = new Regex(@"[0-9]{2}.[0-9]{3}.[0-9]{2}.[0-9]{2}");

            return checkFIO.IsMatch(phoneNumber) ? true : false;
        }
    }
}
