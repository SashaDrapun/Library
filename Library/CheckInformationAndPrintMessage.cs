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
        public static bool IsBookCorrect(string nameBook, string fioAutor, string category,string picture)
        {
            string errorMessage = "";

            if (string.IsNullOrEmpty(nameBook))
            {
                errorMessage += "\n" + "Заполните поле имя книги";
            }
            if (string.IsNullOrEmpty(fioAutor))
            {
                errorMessage += "\n" + "Заполните поле ФИО автора";
            }
            if (string.IsNullOrEmpty(category))
            {
                errorMessage += "\n" + "Заполните поле категория";
            }
            if (string.IsNullOrEmpty(picture))
            {
                errorMessage += "\n" + "Выберите фотографию";
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
                PrintMessage.WarningMessage(errorMessage,"Перед добавлением устраните следующие ошибки:");
                return false;
            }
        }

        public static bool IsReaderCorrect(string fio,string contactNumber,string email)
        {
            string[] partsOfFio = fio.Split(new char[] { ' ' });

            string surname = partsOfFio[0];
            string name = partsOfFio[1];
            string patronymic = partsOfFio[2];
            string errorMessage = "";

            if (string.IsNullOrEmpty(surname))
            {
                errorMessage += "\n" + "Заполните поле фамилия читателя";
            }
            else
            {
                if (!DataValidation.IsPathOfFioCorrect(surname))
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
                if (!DataValidation.IsPathOfFioCorrect(name))
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
                if (!DataValidation.IsPathOfFioCorrect(patronymic))
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
                if (!DataValidation.IsContactNumberCorrect(contactNumber))
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
                if (!DataValidation.IsEmailCorrect(email))
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
                PrintMessage.WarningMessage(errorMessage, "Перед добавлением устраните следующие ошибки:");
                return false;
            }
        }

        public static bool IsLibrarianCorrect(string fio, string contactNumber, string email, string password,
            string passwordRepeat)
        {
            string[] partsOfFio = fio.Split(new char[] { ' ' });

            string surname = partsOfFio[0];
            string name = partsOfFio[1];
            string patronymic = partsOfFio[2];
            string errorMessage = "";


            if (string.IsNullOrEmpty(surname))
            {
                errorMessage += "\n" + "Заполните поле фамилия библиотекаря";
            }
            else
            {
                if (!DataValidation.IsPathOfFioCorrect(surname))
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
                if (!DataValidation.IsPathOfFioCorrect(name))
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
                if (!DataValidation.IsPathOfFioCorrect(patronymic))
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
                if (!DataValidation.IsContactNumberCorrect(contactNumber))
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
                if (!DataValidation.IsEmailCorrect(email))
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
                PrintMessage.WarningMessage(errorMessage, "Перед добавлением устраните следующие ошибки:");
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
                if (!DataValidation.IsPathOfFioCorrect(surname))
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
                if (!DataValidation.IsPathOfFioCorrect(name))
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
                if (!DataValidation.IsPathOfFioCorrect(patronymic))
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
                PrintMessage.WarningMessage(errorMessage, "Перед добавлением устраните следующие ошибки:");
                return false;
            }
        }

        public static List<QuerySettings> GetBooksQuerySettingsOrPrintErrorMessage(string bookName,string fioAutor, string category,
            string picture, bool isPictureNeedToEdit, int yearOfIssue, bool isYearOfIssueNeedToEdit)
        {
            List<QuerySettings> querySettings = new List<QuerySettings>();
            string errorMessage = "";

            if (!string.IsNullOrEmpty(bookName))
            {
                if (DatabaseSelectorSomeInformation.IsBookExists(bookName))
                {
                    errorMessage += "\n" + "Книга с таким названием уже существует";
                }
                querySettings.Add(new QuerySettings("nameBook", bookName));
            }
            if (!string.IsNullOrEmpty(fioAutor))
            {
                int idAutor = DatabaseSelectorSomeInformation.GetIdAutor(fioAutor);

                querySettings.Add(new QuerySettings("idAutor", idAutor.ToString()));
            }
            if (!string.IsNullOrEmpty(category))
            {
                querySettings.Add(new QuerySettings("category", category));
            }
            if (!string.IsNullOrEmpty(picture) && isPictureNeedToEdit)
            {
                querySettings.Add(new QuerySettings("picture", picture));
            }
            if (isYearOfIssueNeedToEdit)
            {
                querySettings.Add(new QuerySettings("yearOfIssue", yearOfIssue.ToString()));
            }
            if (!string.IsNullOrEmpty(errorMessage))
            {
                PrintMessage.WarningMessage(errorMessage, "Перед редактированием устраните следующие ошибки:");
                return null;
            }
            else
            {
                return querySettings;
            }
                
        }

    }
}
