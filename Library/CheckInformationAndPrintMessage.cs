using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Library
{
    public static class CheckInformationAndPrintMessage
    {
        public static bool IsBookCorrect(string nameBook, string fioAutor, string countInStock, string category,string picture,string yearOfIssue)
        {
            int countInStockNormal = 0;
            int yearOfIssueNormal = 0;
            string errorMessage = "";

            try
            {
                countInStockNormal = Convert.ToInt32(countInStock);

            }
            catch (FormatException)
            {
                errorMessage += "\n" + "Количество в наличии должно быть целочисленным";
            }
            try
            {
                yearOfIssueNormal = Convert.ToInt32(yearOfIssue);
            }
            catch (FormatException)
            {
                errorMessage += "\n" + "Год издания должен быть целочисленным";
            }
            if(countInStockNormal <= 0)
            {
                errorMessage += "\n" + "Количество в наличии должно быть натуральным числом";
            }
            if (string.IsNullOrEmpty(nameBook))
            {
                errorMessage += "\n" + "Заполните поле имя книги";
            }
            if (string.IsNullOrEmpty(fioAutor))
            {
                errorMessage += "\n" + "Заполните поле имя автора";
            }
            if (string.IsNullOrEmpty(countInStock))
            {
                errorMessage += "\n" + "Заполните поле количество экземпляров";
            }
            if (string.IsNullOrEmpty(category))
            {
                errorMessage += "\n" + "Заполните поле категория";
            }
            if (string.IsNullOrEmpty(picture))
            {
                errorMessage += "\n" + "Выберите фотографию";
            }
            if (string.IsNullOrEmpty(picture)) 
            {
                errorMessage += "\n" + "Заполните поле год издания";
            }
            if (DatabaseSelectorSomeInformation.IsBookExists(nameBook))
            {
                errorMessage += "\n" + "Книга с таким названием уже существует";
            }


            if (string.IsNullOrEmpty(errorMessage))
            {
                return true;
            }
            
            else
            {
                MessageBox.Show(errorMessage,"Перед добавлением устраните следующие ошибки:",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return false;
            }
        }

        public static bool IsReaderCorrect(string surname,string name,string patronymic,string contactNumber,string email)
        {
            string errorMessage = "";
          

            if (string.IsNullOrEmpty(surname))
            {
                errorMessage += "\n" + "Заполните поле фамилия читателя";
            }
            else
            {
                if (!IsPathOfFioCorrect(surname))
                {
                    errorMessage += "\n" + "Фамилия читателя не должна содержать цифр и состоять как минимум из 2 букв";
                }
            }

            if (string.IsNullOrEmpty(name))
            {
                errorMessage += "\n" + "Заполните поле имя читателя";
            }
            else
            {
                if (!IsPathOfFioCorrect(name))
                {
                    errorMessage += "\n" + "Имя читателя не должно содержать цифр и состоять как минимум из 2 букв";
                }
                
            }

            if (string.IsNullOrEmpty(patronymic))
            {
                errorMessage += "\n" + "Заполните поле отчество читателя";
            }
            else
            {
                if (!IsPathOfFioCorrect(patronymic))
                {
                    errorMessage += "\n" + "Отчество читателя не должно содержать цифр и состоять как минимум из 2 букв";
                }
            }

            if (string.IsNullOrEmpty(contactNumber))
            {
                errorMessage += "\n" + "Заполните поле контактный номер";
            }
            else
            {
                if (!IsContactNumberCorrect(contactNumber))
                {
                    errorMessage += "\n" + "Заполните номер телефона по шаблону";
                }
            }

            if (string.IsNullOrEmpty(email))
            {
                errorMessage += "\n" + "Заполните поле почта";
            }
            else
            {
                if (!IsEmailCorrect(email))
                {
                    errorMessage += "\n" + "Заполните поле почта правильно." +
                        " Если вы уверены что ваша почта заполнена правильно, обратитесь к разработчику!";
                }
            }

            string fioReader = surname + " " + name + " " + patronymic;
                
            if (DatabaseSelectorSomeInformation.IsReaderExists(fioReader))
            {
                errorMessage += "\n" + "Читатель с таким ФИО уже существует";
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                return true;
            }
            else
            {
                MessageBox.Show(errorMessage, "Перед добавлением устраните следующие ошибки:", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }
        }

        public static bool IsLibrarianCorrect(string surname, string name, string patronymic, string contactNumber, string email, string password,
            string passwordRepeat)
        {
            string errorMessage = "";


            if (string.IsNullOrEmpty(surname))
            {
                errorMessage += "\n" + "Заполните поле фамилия библиотекаря";
            }
            else
            {
                if (!IsPathOfFioCorrect(surname))
                {
                    errorMessage += "\n" + "Фамилия библиотекаря не должна содержать цифр и состоять как минимум из 2 букв";
                }
            }

            if (string.IsNullOrEmpty(name))
            {
                errorMessage += "\n" + "Заполните поле имя библиотекаря";
            }
            else
            {
                if (!IsPathOfFioCorrect(name))
                {
                    errorMessage += "\n" + "Имя библиотекаря не должно содержать цифр и состоять как минимум из 2 букв";
                }

            }

            if (string.IsNullOrEmpty(patronymic))
            {
                errorMessage += "\n" + "Заполните поле отчество библиотекаря";
            }
            else
            {
                if (!IsPathOfFioCorrect(patronymic))
                {
                    errorMessage += "\n" + "Отчество библиотекаря не должно содержать цифр и состоять как минимум из 2 букв";
                }
            }

            if (string.IsNullOrEmpty(contactNumber))
            {
                errorMessage += "\n" + "Заполните поле контактный номер";
            }
            else
            {
                if (!IsContactNumberCorrect(contactNumber))
                {
                    errorMessage += "\n" + "Заполните номер телефона по шаблону";
                }
            }




            if (string.IsNullOrEmpty(email))
            {
                errorMessage += "\n" + "Заполните поле почта";
            }
            else
            {
                if (!IsEmailCorrect(email))
                {
                    errorMessage += "\n" + "Заполните поле почта правильно." +
                        " Если вы уверены что ваша почта заполнена правильно, обратитесь к разработчику!";
                }
            }

            if (string.IsNullOrEmpty(password))
            {
                errorMessage += "\n" + "Пароль должен состоять как минимум из одного символа";
            }

            if(password != passwordRepeat)
            {
                errorMessage += "\n" + "Пароли не совпадают";
            }

            string fioLibrarian = surname + " " + name + " " + patronymic;

            if (DatabaseSelectorSomeInformation.IsLibrarianExists(fioLibrarian))
            {
                errorMessage += "\n" + "Библиотекарь с таким ФИО существует";
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                return true;
            }
            else
            {
                MessageBox.Show(errorMessage, "Перед добавлением устраните следующие ошибки:", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }
        }

        public static bool IsAutorCorrect(string surname, string name, string patronymic)
        {
            string errorMessage = "";


            if (string.IsNullOrEmpty(surname))
            {
                errorMessage += "\n" + "Заполните поле фамилия автора";
            }
            else
            {
                if (!IsPathOfFioCorrect(surname))
                {
                    errorMessage += "\n" + "Фамилия автора не должна содержать цифр и состоять как минимум из 2 букв";
                }
            }

            if (string.IsNullOrEmpty(name))
            {
                errorMessage += "\n" + "Заполните поле имя автора";
            }
            else
            {
                if (!IsPathOfFioCorrect(name))
                {
                    errorMessage += "\n" + "Имя автора не должно содержать цифр и состоять как минимум из 2 букв";
                }

            }

            if (string.IsNullOrEmpty(patronymic))
            {
                errorMessage += "\n" + "Заполните поле отчество автора";
            }
            else
            {
                if (!IsPathOfFioCorrect(patronymic))
                {
                    errorMessage += "\n" + "Отчество автора не должно содержать цифр и состоять как минимум из 2 букв";
                }
            }

            string fioAutor = surname + " " + name + " " + patronymic;

            if (DatabaseSelectorSomeInformation.IsAutorExists(fioAutor))
            {
                errorMessage += "\n" + "Автор с таким ФИО уже существует";
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                return true;
            }
            else
            {
                MessageBox.Show(errorMessage, "Перед добавлением устраните следующие ошибки:", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }
        }

        private static bool IsEmailCorrect(string email)
        {
            Regex checkFIO = new Regex(@"[0-9a-z_.]+@[0-9a-z_^.]+.[a-z]{2,3}");

            return checkFIO.IsMatch(email) ? true : false;
        }

        private static bool IsPathOfFioCorrect(string partOfFio)
        {
            Regex checkFIO = new Regex(@"(\D{2})+");

            return checkFIO.IsMatch(partOfFio) ? true : false;
        }

        private static bool IsContactNumberCorrect(string phoneNumber)
        {
            Regex checkFIO = new Regex(@"[0-9]{2}.[0-9]{3}.[0-9]{2}.[0-9]{2}");

            return checkFIO.IsMatch(phoneNumber) ? true : false;
        }
    }
}
