﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public partial class FormMain : Form
    {
        public delegate void SomeChange();

        public event SomeChange BookDeliveryChange;
        public event SomeChange BooksChange;
        public event SomeChange InstancesChange;
        public event SomeChange ReadersChange;
        public event SomeChange AutorsChange;
        public event SomeChange LibrariansChange;

        public FormMain()
        {
            InitializeComponent();
            BookDeliveryChange += LoadBookDeliveryMainContent;
            BookDeliveryChange += LoadBookDeliverySideContent;
            BookDeliveryChange?.Invoke();

            BooksChange += LoadBookMainContent;
            BooksChange += LoadInstancesSideContent;
            BooksChange += LoadBookDeliverySideContent;
            BooksChange += LoadInstancesMainContent;
            BooksChange?.Invoke();

            InstancesChange += LoadInstancesMainContent;
            InstancesChange += LoadBookDeliverySideContent;
            InstancesChange?.Invoke();

            ReadersChange += LoadReadersMainContent;
            ReadersChange += LoadBookDeliverySideContent;
            ReadersChange?.Invoke();

            AutorsChange += LoadAutorsMainContent;
            AutorsChange += LoadBooksSideContent;
            AutorsChange?.Invoke();

            LibrariansChange += LoadLibrariansMainContent;
            LibrariansChange += LoadAutorization;

            LibrariansChange?.Invoke();
        }

        private Book AddedBook = null;
        private Librarian AutorizationLibrarian = null;
        private List<Book> BooksForDisplay = new List<Book>();
        private int CurrentBookNumber;
        private int ConfirmationCode = 0;
        private static Random random = new Random();

        #region Authorization
        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.PasswordChar = checkBoxShowPassword.Checked ? char.MinValue : '*';
            checkBoxShowPassword.Text = checkBoxShowPassword.Checked ? "Cкрыть пароль" : "Показать пароль";
        }

        private void LoadAutorization()
        {
            List<Librarian> librarians = DatabaseSelectorAllInformation.GetAllLibrarians();

            for (int i = 0; i < librarians.Count; i++)
            {
                comboBoxNameLibrarian.Items.Add(librarians[i].FioLibrarian);
            }
        }
        #endregion

        private void tabPageBookingDelivery_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DateTime.Now.ToString());
        }

        #region BookDeliveries

        private void TextBoxesBookDeliverySeacrh_TextChanged(object sender, EventArgs e)
        {
            LoadBookDeliveryMainContent();
        }

        private void LoadBookDeliveryMainContent()
        {
            List<BookDelivery> bookDeliveries = DatabaseSelectorAllInformation.GetAllBookDelivery(GetSeacrhSettingsBookDelivery(),
                checkBoxOnlyDebtorsBookDelivery.Checked);
            dataGridViewBookDelivery.Rows.Clear();

            for (int i = 0; i < bookDeliveries.Count; i++)
            {
                dataGridViewBookDelivery.Rows.Add(new object[]
                {
                    bookDeliveries[i].FioReader,
                    bookDeliveries[i].NameBook,
                    bookDeliveries[i].FioLibrarian,
                    bookDeliveries[i].DateOfIssue,
                    bookDeliveries[i].ReturnDate,
                    bookDeliveries[i].IdInstances,
                    string.IsNullOrEmpty(bookDeliveries[i].ReturnDate)?bookDeliveries[i].NumberOfDaysAfterIssue.ToString() : ""
                }) ;
            }
            ColorDatagridViews();
        }

        #region LoadBookDeliverySideContent
        private void LoadBookDeliverySideContent()
        {
            LoadReadersInComboboxBookDelivery();
            LoadBooksInComboboxBookDelivery();
            LoadReadersInComboboxReturnBooks();
        }

        private void LoadReadersInComboboxReturnBooks()
        {
            List<string> fiosReaders = DatabaseSelectorAllInformation.GetFioReadersWhoHaveABookOnHand();
            comboBoxFioReaderReturnsBooks.Items.Clear();
            comboBoxFioReaderReturnsBooks.Text = "";

            for (int i = 0; i < fiosReaders.Count; i++)
            {
                comboBoxFioReaderReturnsBooks.Items.Add(fiosReaders[i]); 
            }
        }

        private void LoadReadersInComboboxBookDelivery()
        {
            List<Reader> readers = DatabaseSelectorAllInformation.GetAllReaders();

            comboBoxFIOReader.Items.Clear();
            comboBoxFIOReader.Text = "";
            for (int i = 0; i < readers.Count; i++)
            {
                comboBoxFIOReader.Items.Add(readers[i].FioReader);
            }
        }

        private void LoadBooksInComboboxBookDelivery()
        {
            List<Book> books = DatabaseSelectorAllInformation.GetAllBooks();

            comboBoxNameBook.Items.Clear();
            comboBoxNameBook.Text = "";
            comboBoxIdInstance.Text = "";
            comboBoxIdInstance.Items.Clear();
            for (int i = 0; i < books.Count; i++)
            {
                if(books[i].CountInStock != 0)
                {
                    comboBoxNameBook.Items.Add(books[i].NameBook);
                }
            }
        }
        #endregion

        private List<QuerySettings> GetSeacrhSettingsBookDelivery()
        {
            List<QuerySettings> searchSettings = new List<QuerySettings>();

            if(!string.IsNullOrEmpty(textBoxFioReaderBookDelivery.Text))
            {
                searchSettings.Add(new QuerySettings("fioReader", textBoxFioReaderBookDelivery.Text));
            }
            if(!string.IsNullOrEmpty(textBoxNameBookBookDelivery.Text))
            {
                searchSettings.Add(new QuerySettings("nameBook", textBoxNameBookBookDelivery.Text));
            }
            if(!string.IsNullOrEmpty(textBoxFioLibrarianBookDelivery.Text))
            {
                searchSettings.Add(new QuerySettings("fioLibrarian", textBoxFioLibrarianBookDelivery.Text));
            }
            if (!string.IsNullOrEmpty(textBoxIdInstanceBookDelivery.Text))
            {
                searchSettings.Add(new QuerySettings("BookDelivery.idInstance", textBoxIdInstanceBookDelivery.Text));
            }
            return searchSettings;
        }

        private void comboBoxIdInstance_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxNameBook.Text))
            {
                MessageBox.Show("Сперва выберите книгу");
            }
        }

        private void comboBoxNameBook_SelectedValueChanged(object sender, EventArgs e)
        {
            List<int> instances = DatabaseSelectorAllInformation.GetInstancesInStock(comboBoxNameBook.Text);
            comboBoxIdInstance.Items.Clear();
            comboBoxIdInstance.Text = "";

            for (int i = 0; i < instances.Count; i++)
            {
                comboBoxIdInstance.Items.Add(instances[i]);
            }
        }

        private void buttonBookIssuance_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxFIOReader.Text) || !DatabaseSelectorSomeInformation.IsReaderExists(comboBoxFIOReader.Text))
            {
                MessageBox.Show("Выберите читателя из списка");
                return;
            }
            if (string.IsNullOrEmpty(comboBoxNameBook.Text) || !DatabaseSelectorSomeInformation.IsBookExists(comboBoxNameBook.Text))
            {
                MessageBox.Show("Выберите наименование книги и номер экземпляра из соответствующих списков");
                return;
            }
            if (string.IsNullOrEmpty(comboBoxIdInstance.Text) || 
                !DatabaseSelectorSomeInformation.IsInstancesExists(Convert.ToInt32(comboBoxIdInstance.Text)))
            {
                MessageBox.Show("Выберите номер экземпляра из списка");
                return;
            }
            DatabaseInserter.InsertIntoBookDelivery(new BookDelivery(
                DateParser.FromNormalTimeToBdTime(dateTimePicker1.Text), comboBoxFIOReader.Text,
                comboBoxNameBook.Text, "Драпун Александр Игоревич", Convert.ToInt32(comboBoxIdInstance.Text)));

            BookDeliveryChange?.Invoke();
            MessageBox.Show("Выдача книги произошла успешно");
        }

        private void comboBoxFioReaderReturnsBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<BookDelivery> bookDeliveries = DatabaseSelectorAllInformation.GetAllBookDelivery(comboBoxFioReaderReturnsBooks.Text);
            dataGridViewBookDeliveryReturnBooks.Rows.Clear();
            for (int i = 0; i < bookDeliveries.Count; i++)
            {
                dataGridViewBookDeliveryReturnBooks.Rows.Add(new object[]
               {
                    bookDeliveries[i].FioReader,
                    bookDeliveries[i].NameBook,
                    bookDeliveries[i].FioLibrarian,
                    bookDeliveries[i].DateOfIssue,
                    bookDeliveries[i].ReturnDate,
                    bookDeliveries[i].IdInstances,
                    string.IsNullOrEmpty(bookDeliveries[i].ReturnDate)?bookDeliveries[i].NumberOfDaysAfterIssue.ToString() : ""
               });
            }
            ColorDatagridViews();
        }

        private void ColorDatagridViews()
        {
            for (int i = 0; i < dataGridViewBookDeliveryReturnBooks.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dataGridViewBookDeliveryReturnBooks[6, i].Value.ToString()))
                {
                    int value = Convert.ToInt32(dataGridViewBookDeliveryReturnBooks[6, i].Value.ToString());

                    if (value > 30)
                    {
                        dataGridViewBookDeliveryReturnBooks[6, i].Style.BackColor = Color.Red;
                    }
                }
            }

            for (int i = 0; i < dataGridViewBookDelivery.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dataGridViewBookDelivery[6, i].Value.ToString()))
                {
                    int value = Convert.ToInt32(dataGridViewBookDelivery[6, i].Value.ToString());

                    if (value > 30)
                    {
                        dataGridViewBookDelivery[6, i].Style.BackColor = Color.Red;
                    }
                }
            }
        }

        private void dataGridViewBookDeliveryReturnBooks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxFioReaderReturnsBooks.Text) || 
                !DatabaseSelectorSomeInformation.IsReaderExists(comboBoxFioReaderReturnsBooks.Text))
            {
                PrintMessage.WarningMessage("Сперва выберите ФИО читателя");
            }
            else
            {
                if (e.RowIndex == -1 || e.RowIndex == dataGridViewBookDeliveryReturnBooks.Rows.Count-1) 
                {
                    return;
                }
                BookDelivery bookDelivery = new BookDelivery(
                    dataGridViewBookDeliveryReturnBooks[3, e.RowIndex].Value.ToString(),
                    dataGridViewBookDeliveryReturnBooks[0, e.RowIndex].Value.ToString(),
                    dataGridViewBookDeliveryReturnBooks[1, e.RowIndex].Value.ToString(),
                    dataGridViewBookDeliveryReturnBooks[2, e.RowIndex].Value.ToString(),
                    Convert.ToInt32(dataGridViewBookDeliveryReturnBooks[5, e.RowIndex].Value)
                    );

                string message = "Вы уверены, что хотите оформить возврат книги " +
                  bookDelivery.NameBook + " читателем " + bookDelivery.FioReader;
                string caption = "Оформление возврата";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string bdDate = DateParser.FromNormalTimeToBdTime(DateTime.Now.ToString());

                    DatabaseUpdater.UpdateBookDelivery(bookDelivery.IdInstances, bdDate);
                    BookDeliveryChange?.Invoke();
                    dataGridViewBookDeliveryReturnBooks.Rows.Clear();
                    MessageBox.Show("Возврат книги произошел успешно");
                }
            }
        }

        #endregion


        #region Books
        private void LoadBookMainContent()
        {
            List<Book> books = DatabaseSelectorAllInformation.GetAllBooks(GetSearchSettingsBooks(), checkBoxOnlyInStock.Checked);
            BooksForDisplay = books;

            CurrentBookNumber = 0;

            if(books.Count == 0)
            {
                HideBooks();
                
            }
            else
            {
                ShowBook(0);
            }
        }

        private void HideBooks()
        {
            for (int i = 0; i < tabControlMain.TabPages["tabPageBooks"].Controls.Count; i++)
            {
                Control control = tabControlMain.TabPages["tabPageBooks"].Controls[i];
                if (control.Tag != null && control.Tag.ToString() == "Hidden")
                {
                    control.Visible = false;
                }
            }
            labelNotFound.Visible = true;
        }

        private void ShowBooks()
        {
            for (int i = 0; i < tabControlMain.TabPages["tabPageBooks"].Controls.Count; i++)
            {
                Control control = tabControlMain.TabPages["tabPageBooks"].Controls[i];
                if (control.Tag != null && control.Tag.ToString() == "Hidden")
                {
                    control.Visible = true;
                }
            }
            labelNotFound.Visible = false;
        }


        private void ShowBook(int numberBook)
        {
            ShowBooks();
            textBoxNameBookMainValue.Text = BooksForDisplay[numberBook].NameBook;
            textBoxFioAutorMainValue.Text = BooksForDisplay[numberBook].FioAutor;
            textBoxYearOfIssueMainValue.Text = BooksForDisplay[numberBook].YearOfIssue.ToString();
            textBoxCategoryMainValue.Text = BooksForDisplay[numberBook].Category;
            textBoxCountInStockMainValue.Text = BooksForDisplay[numberBook].CountInStock.ToString();
            textBoxCountOfReaders.Text = BooksForDisplay[numberBook].CountOnHand.ToString();
            pictureBoxMainValue.Load(BooksForDisplay[numberBook].Picture);
            labelNumberCurrentBook.Text = "Книга " + (numberBook+1) + " из " + BooksForDisplay.Count;
        }

        private void LoadBooksSideContent()
        {
            LoadAutors();
        }

        private void LoadAutors()
        {
            List<Autor> autors = DatabaseSelectorAllInformation.GetAllAutors();
            comboBoxFioAutor.Items.Clear();
            comboBoxFioAutor.Text = "";

            for (int i = 0; i < autors.Count; i++)
            {
                comboBoxFioAutor.Items.Add(autors[i].FioAutor);
            }

            for (int i = 0; i < autors.Count; i++)
            {
                comboBoxFioAutorEditNewValue.Items.Add(autors[i].FioAutor);
            }
        }

        private List<QuerySettings> GetSearchSettingsBooks()
        {
            List<QuerySettings> searchSettings = new List<QuerySettings>();

            if (!string.IsNullOrEmpty(textBoxNameBook.Text))
            {
                searchSettings.Add(new QuerySettings("nameBook", textBoxNameBook.Text));
            }
            if (!string.IsNullOrEmpty(textBoxAuthor.Text))
            {
                searchSettings.Add(new QuerySettings("fioAutor", textBoxAuthor.Text));
            }
            if (!string.IsNullOrEmpty(textBoxCategory.Text))
            {
                searchSettings.Add(new QuerySettings("category", textBoxCategory.Text));
            }
            if (!string.IsNullOrEmpty(textBoxYearOfIssue.Text))
            {
                searchSettings.Add(new QuerySettings("yearOfIssue", textBoxYearOfIssue.Text));
            }
            return searchSettings;
        }

        private void TextBoxesBookChanged(object sender, EventArgs e)
        {
            LoadBookMainContent();
        }
        #endregion


        #region Instances
        private void LoadInstancesMainContent()
        {
            List<Instance> instances = DatabaseSelectorAllInformation.GetAllInstances(GetSearchSettingsInstances());
            dataGridViewInstances.Rows.Clear();

            for (int i = 0; i < instances.Count; i++)
            {
                dataGridViewInstances.Rows.Add(new object[]
                {
                     instances[i].NameBook,
                    instances[i].IdInstance

                });
            }
        }

        private List<QuerySettings> GetSearchSettingsInstances()
        {
            List<QuerySettings> searchSettings = new List<QuerySettings>();

            if (!string.IsNullOrEmpty(textBoxNameBookInstancesSearch.Text))
            {
                searchSettings.Add(new QuerySettings("nameBook", textBoxNameBookInstancesSearch.Text));
            }
            if (!string.IsNullOrEmpty(textBoxIdInstanceInstancesSearch.Text))
            {
                searchSettings.Add(new QuerySettings("idInstance", textBoxIdInstanceInstancesSearch.Text));
            }
            return searchSettings;
        }

        private void LoadInstancesSideContent()
        {
            LoadNamesBooks();
        }

        private void LoadNamesBooks()
        {
            List<Book> books = DatabaseSelectorAllInformation.GetAllBooks();
            comboBoxNameBookInstances.Items.Clear();
            comboBoxNameBookInstances.Text = "";

            for (int i = 0; i < books.Count; i++)
            {
                comboBoxNameBookInstances.Items.Add(books[i].NameBook);
            }
        }

        private void buttonAddInstances_Click(object sender, EventArgs e)
        {
            try
            {
                int idInstances = Convert.ToInt32(textBoxIdInstancesInstances.Text);

                if (DatabaseSelectorSomeInformation.IsInstancesExists(idInstances))
                {
                    MessageBox.Show("Такой номер экземпляра уже существует");
                    return;
                }
                else
                {
                    if (string.IsNullOrEmpty(comboBoxNameBookInstances.Text))
                    {
                        MessageBox.Show("Выберите имя книги из списка");
                        return;
                    }
                    else
                    {
                        DatabaseInserter.InsertIntoInstances(new Instance(idInstances, comboBoxNameBookInstances.Text));
                        MessageBox.Show("Добавление экземпляра произошло успешно");
                        InstancesChange?.Invoke();

                        if (AddedBook != null)
                        {
                            AddedBook.CountInStock--;
                            if (AddedBook.CountInStock == 0)
                            {
                                MessageBox.Show("Добавление книги " + AddedBook.NameBook + " и всех её экземпляров произошло успешно");
                                AddedBook = null;
                                comboBoxNameBookInstances.Enabled = true;
                                labelNumberOfCopiesLeftToAdd.Visible = false;
                                InstancesChange?.Invoke();
                            }
                            else
                            {
                                labelNumberOfCopiesLeftToAdd.Text = "Осталось добавить " + AddedBook.CountInStock +
                                    " экземпляров книги " + AddedBook.NameBook;
                            }
                        }

                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Номер экземпляра должен быть целым числом");
            }
        }

        private void TextBoxesInstancesChange(object sender, EventArgs e)
        {
            LoadInstancesMainContent();
        }
        #endregion

        #region Readers

        private void LoadReadersMainContent()
        {
            List<Reader> readers = DatabaseSelectorAllInformation.GetAllReaders(GetSearchSettingsReaders(), checkBoxOnlyDebtors.Checked);
            dataGridViewReaders.Rows.Clear();

            for (int i = 0; i < readers.Count; i++)
            {
                dataGridViewReaders.Rows.Add(new object[]
                {
                    readers[i].FioReader,
                    readers[i].ContactNumber,
                    readers[i].Email
                });
            }
        }
        private List<QuerySettings> GetSearchSettingsReaders()
        {
            List<QuerySettings> searchSettings = new List<QuerySettings>();

            if (!string.IsNullOrEmpty(textBoxFioReaderReadersSearch.Text))
            {
                searchSettings.Add(new QuerySettings("fioReader", textBoxFioReaderReadersSearch.Text));
            }
            if (!string.IsNullOrEmpty(textBoxContactNumberReadersSearch.Text))
            {
                searchSettings.Add(new QuerySettings("contactNumber", textBoxContactNumberReadersSearch.Text));
            }
            if (!string.IsNullOrEmpty(textBoxMailReadersSearch.Text))
            {
                searchSettings.Add(new QuerySettings("email", textBoxMailReadersSearch.Text));
            }

            return searchSettings;
        }

        private void TextBoxesReadersChange(object sender, EventArgs e)
        {
            LoadReadersMainContent();
        }
        #endregion


        #region Librarians

        private void LoadLibrariansMainContent()
        {
            List<Librarian> librarians = DatabaseSelectorAllInformation.GetAllLibrarians(GetSearchSettingsLibrarians());

            dataGridViewLibrarians.Rows.Clear();
            for (int i = 0; i < librarians.Count; i++)
            {
                dataGridViewLibrarians.Rows.Add(new object[]
                {
                    librarians[i].FioLibrarian,
                    librarians[i].ContactNumber,
                    librarians[i].Email
                });
            }
        }
        private List<QuerySettings> GetSearchSettingsLibrarians()
        {
            List<QuerySettings> searchSettings = new List<QuerySettings>();

            if (!string.IsNullOrEmpty(textBoxFioLibrarianLibrariansSearch.Text))
            {
                searchSettings.Add(new QuerySettings("fioLibrarian", textBoxFioLibrarianLibrariansSearch.Text));
            }
            if (!string.IsNullOrEmpty(textBoxContactNumberLibrariansSearch.Text))
            {
                searchSettings.Add(new QuerySettings("contactNumber", textBoxContactNumberLibrariansSearch.Text));
            }
            if (!string.IsNullOrEmpty(textBoxMailLibrariansSearch.Text))
            {
                searchSettings.Add(new QuerySettings("email", textBoxMailLibrariansSearch.Text));
            }

            return searchSettings;
        }

        private void TextBoxesLibrariansChange(object sender, EventArgs e)
        {
            LoadLibrariansMainContent();
        }
        #endregion


        #region Autors

        private void LoadAutorsMainContent()
        {
            List<Autor> autors = DatabaseSelectorAllInformation.GetAllAutors(GetSearchSettingsAutors());

            dataGridViewAutors.Rows.Clear();
            for (int i = 0; i < autors.Count; i++)
            {
                dataGridViewAutors.Rows.Add(new object[]
                {
                    autors[i].FioAutor,
                    autors[i].Biography
                });
            }
        }
        private List<QuerySettings> GetSearchSettingsAutors()
        {
            List<QuerySettings> searchSettings = new List<QuerySettings>();

            if (!string.IsNullOrEmpty(textBoxFioAutorAutorsSearch.Text))
            {
                searchSettings.Add(new QuerySettings("fioAutor", textBoxFioAutorAutorsSearch.Text));
            }

            return searchSettings;
        }

        private void TextBoxesAutorsChange(object sender, EventArgs e)
        {
            LoadAutorsMainContent();
        }
        #endregion

        private void tabPageBooks_Click(object sender, EventArgs e)
        {
            MessageBox.Show(tabControlBooks.TabPages["tabPageEditBook"].Controls.Count.ToString());
        }



        private void OnAllTextBoxDoubleClick(object sender, EventArgs e)
        {
            (sender as TextBox).Text = null;
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            tabControlMain.SelectedTab = tabControlMain.TabPages["tabPageReaders"];
            tabControlReaders.SelectedTab = tabControlReaders.TabPages["tabPageAddaReader"];
        } 

        private void buttonLoadPicture_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            
            string fullFileName = openFileDialog.FileName;
            string fileName = Path.GetFileName(fullFileName);


            if (File.Exists(fileName))
            {
                string newFileName = fileName;

                while (File.Exists(newFileName))
                {
                    newFileName = random.Next(1, 1000000).ToString();
                }
                newFileName += Path.GetExtension(fileName);

                string fullNewFileName = Path.GetDirectoryName(fullFileName) + newFileName;
                File.Move(fullFileName, fullNewFileName);
                File.Copy(fullNewFileName, newFileName);
                File.Move(fullNewFileName, fullFileName);
                pictureBoxAdd.Load(newFileName);
                textBoxPicture.Text = newFileName;
            }
            else
            {
                File.Copy(fullFileName, fileName);
                pictureBoxAdd.Load(fileName);
                textBoxPicture.Text = fileName;
            }
        }

        private void buttonBooksAdd_Click(object sender, EventArgs e)
        {
            if (CheckInformationAndPrintMessage.IsBookCorrect(textBoxNameBookBooks.Text, comboBoxFioAutor.Text,
                    textBoxCategoryBook.Text, textBoxPicture.Text))
            {
                SuccessfulAdditionToBooks();
            }
        }

        private void SuccessfulAdditionToBooks()
        {
            AddedBook = new Book(textBoxNameBookBooks.Text, comboBoxFioAutor.Text,
               Convert.ToInt32(textBoxCountInStock.Value), textBoxCategoryBook.Text, textBoxPicture.Text,
               Convert.ToInt32(textBoxYearOfIssueAdd.Value));

            DatabaseInserter.InsertIntoBooks(AddedBook);
           
            BooksChange?.Invoke();

            labelNumberOfCopiesLeftToAdd.Text = "Осталось добавить " + AddedBook.CountInStock + " экземпляров книги " +
                AddedBook.NameBook;
            labelNumberOfCopiesLeftToAdd.Visible = true;
            comboBoxNameBookInstances.Enabled = false;
            comboBoxNameBookInstances.Items.Clear();

            comboBoxNameBookInstances.Text = AddedBook.NameBook;

            PrintMessage.InformationMessage("Добавление книги " + AddedBook.NameBook + " произошло успешно, теперь необходимо добавить " +
                AddedBook.CountInStock + " экземпляров этой книги","Успешное добавление");
  
            tabControlMain.SelectedTab = tabControlMain.TabPages["tabPageInstances"];
            tabControlInstances.SelectedTab = tabControlInstances.TabPages["tabPageAddInstances"];
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(AddedBook != null)
            {
                string message = "Вам осталось добавить " + AddedBook.CountInStock + " экземпляров книги " +
                                    AddedBook.NameBook + ", если вы сейчас покинете эту вкладку добавление книги" +
                                    " и всех её экземпляров будет отменено!!!";
                string caption = "Отмена действия";

                var result = MessageBox.Show(message, caption,
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);


                if (result == DialogResult.Yes)
                {
                    CancelAddBook();
                }
                else
                {
                    e.Cancel = true;
                }  
            }
        }

        private void CancelAddBook()
        {
            DatabaseDeleter.DeleteFromBooks(AddedBook.NameBook);
            BooksChange?.Invoke();
            AddedBook = null;
        }

        private void tabControlMain_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (tabControlMain.SelectedTab == tabControlMain.TabPages["tabPageInstances"])
            {
                if (AddedBook != null)
                {
                    string message = "Вам осталось добавить " + AddedBook.CountInStock + " экземпляров книги " +
                        AddedBook.NameBook + ", если вы сейчас покинете эту вкладку добавление книги и всех её экземпляров будет " +
                        "отменено!!!";
                    string caption = "Вы уверены, что хотите прекратить добавление?";
                    var result = MessageBox.Show(message, caption,
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        CancelAddBook();
                    }
                    else
                    {
                        //tabControlMain.SelectedTab = tabControlMain.TabPages["tabPageReaders"];
                        tabControlInstances.SelectedTab = tabControlInstances.TabPages["tabPageAddInstances"];
                        e.Cancel = true;
                    }
                }
            }

            if(tabControlMain.SelectedTab == tabControlMain.TabPages["tabPageAutorization"] && AutorizationLibrarian == null)
            {
                PrintMessage.WarningMessage("Чтобы приступить к работе необходимо авторизоваться");
                e.Cancel = true;
            }
        }

        private void buttonReadersAdd_Click(object sender, EventArgs e)
        {
            string fioReader = textBoxSurnameReader.Text + " " + textBoxNameReader.Text + " " + textBoxPatronymicReader.Text;
            if (CheckInformationAndPrintMessage.IsReaderCorrect(fioReader, textBoxContactNumberReader.Text,textBoxMailReader.Text))
            {
                Reader reader = new Reader(fioReader, textBoxContactNumberReader.Text, textBoxMailReader.Text);
                DatabaseInserter.InsertIntoReaders(reader);
                ReadersChange?.Invoke();
                MessageBox.Show("Добавление читателя произошло успешно");
            }
        }

        private void buttonAutorAdd_Click(object sender, EventArgs e)
        {
            if(CheckInformationAndPrintMessage.IsAutorCorrect(textBoxSurnameAutor.Text,textBoxNameAutor.Text,
                textBoxPatronymicAutor.Text))
            {
                string fioAutor = textBoxSurnameAutor.Text + " " + textBoxNameAutor.Text
                 + " " + textBoxPatronymicAutor.Text;
                DatabaseInserter.InsertIntoAutors(new Autor(fioAutor, textBoxBiographyAutor.Text));
                AutorsChange?.Invoke();
            }
        }

        private void buttonLibrariansAdd_Click(object sender, EventArgs e)
        {
            string fioLibrarian = textBoxSurnameLibrarian.Text + " " + textBoxNameLibrarian.Text +
                    " " + textBoxPatronymicLibrarian.Text;
            if (CheckInformationAndPrintMessage.IsLibrarianCorrect(fioLibrarian, textBoxContactNumberLibrarian.Text,
                textBoxMailLibrarian.Text, textBoxPasswordLibrarian.Text, textBoxPasswordConfirmation.Text))
            {
                DatabaseInserter.InsertIntoLibrarians(new Librarian(fioLibrarian, textBoxContactNumberLibrarian.Text,
                    textBoxMailLibrarian.Text, textBoxPassword.Text));
                LibrariansChange?.Invoke();
            }
        }

        private void buttonPictureEditLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            string fullFileName = openFileDialog.FileName;
            string fileName = Path.GetFileName(fullFileName);


            if (File.Exists(fileName))
            {
                string newFileName = fileName;

                while (File.Exists(newFileName))
                {
                    newFileName = random.Next(1, 1000000).ToString();
                }
                newFileName += Path.GetExtension(fileName);

                string fullNewFileName = Path.GetDirectoryName(fullFileName) + newFileName;
                File.Move(fullFileName, fullNewFileName);
                File.Copy(fullNewFileName, newFileName);
                File.Move(fullNewFileName, fullFileName);
                textBoxPictureEdit.Text = newFileName;
            }
            else
            {
                File.Copy(fullFileName, fileName);
                textBoxPictureEdit.Text = fileName;
            }
        }

        private void buttonBooksEditing_Click(object sender, EventArgs e)
        {
            List<QuerySettings> querySettings = CheckInformationAndPrintMessage.GetBooksQuerySettingsOrPrintErrorMessage(
                textBoxNameBookNewValue.Text, comboBoxFioAutorEditNewValue.Text, textBoxCategoryBookNewValue.Text,
                textBoxPictureEdit.Text, checkBoxIsNeedToEditPicture.Checked,
                Convert.ToInt32(textBoxYearOfIssueNewValue.Value), checkBoxIsNeedToEditYearOfIssue.Checked);
            if (querySettings != null )
            {
               if(querySettings.Count != 0)
                {
                    DatabaseUpdater.UpdateBooks(querySettings, textBoxNameBookEditOldValue.Text);
                    BooksChange?.Invoke();
                    HideBooksEditingControls();
                }
                else
                {
                    MessageBox.Show("Измените хотя бы 1 поле");
                }
            }
        }

        private void HideBooksEditingControls()
        {
            for (int i = 0; i <tabControlBooks.TabPages["tabPageEditBook"].Controls.Count; i++)
            {
                Control control = tabControlBooks.TabPages["tabPageEditBook"].Controls[i];
                if(control.Tag != null && control.Tag.ToString() == "Hidden")
                {
                    control.Visible = false;
                }
            }
            labelHelpBooksEditing.Visible = true;
        }

        private void ShowBooksEditingControls()
        {
            for (int i = 0; i < tabControlBooks.TabPages["tabPageEditBook"].Controls.Count; i++)
            {
                Control control = tabControlBooks.TabPages["tabPageEditBook"].Controls[i];
                if (control.Tag != null && control.Tag.ToString() == "Hidden")
                {
                    control.Visible = true;
                }
            }
            labelHelpBooksEditing.Visible = false;
        }

        private void HideLibrariansEditingControls()
        {
            for (int i = 0; i < tabControlLibrarians.TabPages["tabPageEditLibrarian"].Controls.Count; i++)
            {
                Control control = tabControlLibrarians.TabPages["tabPageEditLibrarian"].Controls[i];
                if (control.Tag != null && control.Tag.ToString() == "Hidden")
                {
                    control.Visible = false;
                }
            }
            labelHelpBooksEditing.Visible = true;
        }

        private void ShowLibrariansEditingControls()
        {
            for (int i = 0; i < tabControlLibrarians.TabPages["tabPageEditLibrarian"].Controls.Count; i++)
            {
                Control control = tabControlLibrarians.TabPages["tabPageEditLibrarian"].Controls[i];
                if (control.Tag != null && control.Tag.ToString() == "Hidden")
                {
                    control.Visible = true;
                }
            }
            labelHelpLibrariansEditing.Visible = false;
        }

        private void HideReadersEditingControls()
        {
            for (int i = 0; i < tabControlReaders.TabPages["tabPageEditReader"].Controls.Count; i++)
            {
                Control control = tabControlReaders.TabPages["tabPageEditReader"].Controls[i];
                if (control.Tag != null && control.Tag.ToString() == "Hidden")
                {
                    control.Visible = false;
                }
            }
            labelHelpLibrariansEditing.Visible = true;
        }

        private void ShowReadersEditingControls()
        {
            for (int i = 0; i < tabControlReaders.TabPages["tabPageEditReader"].Controls.Count; i++)
            {
                Control control = tabControlReaders.TabPages["tabPageEditReader"].Controls[i];
                if (control.Tag != null && control.Tag.ToString() == "Hidden")
                {
                    control.Visible = true;
                }
            }
            labelHelpReadersEditing.Visible = false;
        }

        private void ShowAutorsEditingControls()
        {
            for (int i = 0; i < tabControlAutors.TabPages["tabPageAutorEdit"].Controls.Count; i++)
            {
                Control control = tabControlAutors.TabPages["tabPageAutorEdit"].Controls[i];
                if (control.Tag != null && control.Tag.ToString() == "Hidden")
                {
                    control.Visible = true;
                }
            }
            labelHelpAutorsEditing.Visible = false;
        }

        private void HideAutorsEditingControls()
        {
            for (int i = 0; i < tabControlAutors.TabPages["tabPageAutorEdit"].Controls.Count; i++)
            {
                Control control = tabControlAutors.TabPages["tabPageAutorEdit"].Controls[i];
                if (control.Tag != null && control.Tag.ToString() == "Hidden")
                {
                    control.Visible = false;
                }
            }
            labelHelpAutorsEditing.Visible = true;
        }

        private void dataGridViewReaders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                string fio = dataGridViewReaders[0, e.RowIndex].Value.ToString();

                string[] partsOfFio = fio.Split(new char[] { ' ' });

                string surname = partsOfFio[0];
                string name = partsOfFio[1];
                string patronymic = partsOfFio[2];

                textBoxSurnameReaderOldValue.Text = surname;
                textBoxNameReaderOldValue.Text = name;
                textBoxPatronymicReaderOldValue.Text = patronymic;

                textBoxContactNumberOldValue.Text = dataGridViewReaders[1, e.RowIndex].Value.ToString();
                textBoxMailReaderOldValue.Text = dataGridViewReaders[2, e.RowIndex].Value.ToString();
                tabControlReaders.SelectedTab = tabControlReaders.TabPages["tabPageEditReader"];
                ShowReadersEditingControls();
            }
        }

        private void buttonReadersEditing_Click(object sender, EventArgs e)
        {
            List<QuerySettings> searchSettings = new List<QuerySettings>();

            string surname = textBoxSurnameReaderOldValue.Text;
            string name = textBoxNameReaderOldValue.Text;
            string patronymic = textBoxPatronymicReaderOldValue.Text;
            bool isFioChanged = false;
            bool isAllCorrect = true;
            string errorMessage = "";

            if (!string.IsNullOrEmpty(textBoxSurnameReaderNewValue.Text))
            {
                surname = textBoxSurnameReaderNewValue.Text;
                if (!DataValidation.IsPathOfFioCorrect(surname))
                {
                    errorMessage += "\n" + "Фамилия читателя не должна содержать цифр и состоять как минимум из 2 букв";
                }
                isFioChanged = true;
            }
            if (!string.IsNullOrEmpty(textBoxNameReaderNewValue.Text))
            {
                name = textBoxNameReaderNewValue.Text;
                if (!DataValidation.IsPathOfFioCorrect(name))
                {
                    errorMessage += "\n" + "Имя читателя не должно содержать цифр и состоять как минимум из 2 букв";
                }
                isFioChanged = true;
            }
            if (!string.IsNullOrEmpty(textBoxPatronymicReaderNewValue.Text))
            {
                patronymic = textBoxPatronymicReaderNewValue.Text;
                if (!DataValidation.IsPathOfFioCorrect(patronymic))
                {
                    errorMessage += "\n" + "Отчество читателя не должно содержать цифр и состоять как минимум из 2 букв";
                }
                isFioChanged = true;
            }

            string fio = surname + " " + name + " " + patronymic;

            if (isFioChanged && DatabaseSelectorSomeInformation.IsReaderExists(fio))
            {
                errorMessage += "\n" +  "Читатель с таким ФИО уже существует";
            }
            else
            {
                searchSettings.Add(new QuerySettings("fioReader", fio));
            }

            if(DataValidation.IsContactNumberCorrect(textBoxContactNumberReaderNewValue.Text))
            {
                searchSettings.Add(new QuerySettings("contactNumber", textBoxContactNumberReaderNewValue.Text));
            }          

            if (!string.IsNullOrEmpty(textBoxMailReaderNewValue.Text))
            {
                if (!DataValidation.IsEmailCorrect(textBoxMailReaderNewValue.Text))
                {
                    errorMessage += "\n" + "Заполните поле почта правильно." +
                        " Если вы уверены что ваша почта заполнена правильно, обратитесь к разработчику!";
                }
                searchSettings.Add(new QuerySettings("email", textBoxMailReaderNewValue.Text));
            }

            if(searchSettings.Count != 0)
            {
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    MessageBox.Show(errorMessage, "Перед редактированием устраните следующие ошибки:", MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                }
                else
                {
                    string oldFioReader = textBoxSurnameReaderOldValue.Text + " " + textBoxNameReaderOldValue.Text + " " +
                                        textBoxPatronymicReaderOldValue.Text;
                    DatabaseUpdater.UpdateReaders(searchSettings, oldFioReader);
                    HideReadersEditingControls();
                    ReadersChange?.Invoke();
                }              
            }
            else
            {
                MessageBox.Show("Измените хотя бы 1 поле");
            }
        }

        private void dataGridViewAutors_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                tabControlAutors.SelectedTab = tabControlAutors.TabPages["tabPageAutorEdit"];

                string fio = dataGridViewAutors[0, e.RowIndex].Value.ToString();

                textBoxBiographyAutorOldValue.Text = dataGridViewAutors[1, e.RowIndex].Value.ToString();

                string[] partsOfFio = fio.Split(new char[] { ' ' });

                textBoxSurnameAutorOldValue.Text = partsOfFio[0];
                textBoxNameAutorOldValue.Text = partsOfFio[1];
                textBoxPatronymicAutorOldValue.Text = partsOfFio[2];
                ShowAutorsEditingControls();
            }
        }

        private void buttonAutorsEditing_Click(object sender, EventArgs e)
        {
            List<QuerySettings> querySettings = new List<QuerySettings>();

            string surname = textBoxSurnameAutorOldValue.Text;
            string name = textBoxNameAutorOldValue.Text;
            string patronymic = textBoxPatronymicAutorOldValue.Text;
            bool isFioChanged = false;
            string errorMessage = "";

            if (!string.IsNullOrEmpty(textBoxSurnameAutorNewValue.Text))
            {
                
                surname = textBoxSurnameAutorNewValue.Text;
                if (!DataValidation.IsPathOfFioCorrect(surname))
                {
                    errorMessage += "\n" + "Фамилия автора не должна содержать цифр и состоять как минимум из 2 букв";
                }
                isFioChanged = true;
            }
            if (!string.IsNullOrEmpty(textBoxNameAutorNewValue.Text))
            {
                name = textBoxNameAutorNewValue.Text;
                if (!DataValidation.IsPathOfFioCorrect(name))
                {
                    errorMessage += "\n" + "Имя автора не должно содержать цифр и состоять как минимум из 2 букв";
                }
                isFioChanged = true;
            }
            if (!string.IsNullOrEmpty(textBoxPatronymicAutorNewValue.Text))
            {
                patronymic = textBoxPatronymicAutorNewValue.Text;
                if (!DataValidation.IsPathOfFioCorrect(patronymic))
                {
                    errorMessage += "\n" + "Отчество автора не должно содержать цифр и состоять как минимум из 2 букв";
                }
                isFioChanged = true;
            }

            string fio = surname + " " + name + " " + patronymic;

            if (isFioChanged && DatabaseSelectorSomeInformation.IsAutorExists(fio))
            {
                errorMessage += "\n" +  "Автор с таким ФИО уже существует";
            }
            else
            {
                querySettings.Add(new QuerySettings("fioAutor", fio));
            }

            if (!string.IsNullOrEmpty(textBoxBiographyAutorNewValue.Text))
            {
                querySettings.Add(new QuerySettings("biography", textBoxBiographyAutorNewValue.Text));
            }

            if(querySettings.Count != 0)
            {
                if (string.IsNullOrEmpty(errorMessage))
                {
                    string oldFioAutor = textBoxSurnameAutorOldValue.Text + " " + textBoxNameAutorOldValue.Text + " " +
                   textBoxPatronymicAutorOldValue.Text;
                    DatabaseUpdater.UpdateAutors(querySettings, oldFioAutor);
                    AutorsChange?.Invoke();
                    HideAutorsEditingControls();
                }
                else
                {
                    PrintMessage.WarningMessage(errorMessage, "Перед редактированием устраните следующие ошибки:");
                }
               
            }
            else
            {
                MessageBox.Show("Измените хотя бы 1 поле");
            }
        }

        private void buttonGoToReturnBook_Click(object sender, EventArgs e)
        {
            tabControlBookDelivery.SelectedTab = tabControlBookDelivery.TabPages["tabPageBookReturn"];
        }

        private void buttonGoToAddBook_Click(object sender, EventArgs e)
        {
            tabControlMain.SelectedTab = tabControlMain.TabPages["tabPageBooks"];
            tabControlBooks.SelectedTab = tabControlBooks.TabPages["tabPageAddBook"];
        }

        private void buttonGoToAddInstances_Click(object sender, EventArgs e)
        {
            tabControlMain.SelectedTab = tabControlMain.TabPages["tabPageInstances"];
            tabControlInstances.SelectedTab = tabControlInstances.TabPages["tabPageAddInstances"];
        }
        private void dataGridViewLibrarians_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                tabControlLibrarians.SelectedTab = tabControlLibrarians.TabPages["tabPageEditLibrarian"];

                string fio = dataGridViewLibrarians[0, e.RowIndex].Value.ToString();

                textBoxContactNumberLibrarianOldValue.Text = dataGridViewLibrarians[1, e.RowIndex].Value.ToString();
                textBoxEmailLibrarianOldValue.Text = dataGridViewLibrarians[2, e.RowIndex].Value.ToString();

                string[] partsOfFio = fio.Split(new char[] { ' ' });

                textBoxSurnameLibrarianOldValue.Text = partsOfFio[0];
                textBoxNameLibrarianOldValue.Text = partsOfFio[1];
                textBoxPatronymicLibrarianOldValue.Text = partsOfFio[2];
                ShowLibrariansEditingControls();
            }
        }

        private void buttonLibrariansEdit_Click(object sender, EventArgs e)
        {
            List<QuerySettings> searchSettings = new List<QuerySettings>();

            string surname = textBoxSurnameReaderOldValue.Text;
            string name = textBoxNameReaderOldValue.Text;
            string patronymic = textBoxPatronymicReaderOldValue.Text;
            bool isFioChanged = false;
            string errorMessage = "";

            if (!string.IsNullOrEmpty(textBoxSurnameLibrarianNewValue.Text))
            {
                surname = textBoxSurnameLibrarianNewValue.Text;
                if (!DataValidation.IsPathOfFioCorrect(surname))
                {
                    errorMessage += "\n" + "Фамилия библиотекаря не должна содержать цифр и состоять как минимум из 2 букв";
                }
                isFioChanged = true;
            }
            if (!string.IsNullOrEmpty(textBoxNameLibrarianNewValue.Text))
            {
                name = textBoxNameLibrarianNewValue.Text;
                if (!DataValidation.IsPathOfFioCorrect(name))
                {
                    errorMessage += "\n" + "Имя библиотекаря не должно содержать цифр и состоять как минимум из 2 букв";
                }
                isFioChanged = true;
            }
            if (!string.IsNullOrEmpty(textBoxPatronymicLibrarianNewValue.Text))
            {
                patronymic = textBoxPatronymicLibrarianNewValue.Text;
                if (!DataValidation.IsPathOfFioCorrect(patronymic))
                {
                    errorMessage += "\n" + "Отчество библиотекаря не должно содержать цифр и состоять как минимум из 2 букв";
                }
                isFioChanged = true;
            }

            string fio = surname + " " + name + " " + patronymic;

            if (isFioChanged && DatabaseSelectorSomeInformation.IsLibrarianExists(fio))
            {
                errorMessage += "\n" + "Библиотекарь с таким ФИО уже существует";
            }
            else if(isFioChanged)
            {
                searchSettings.Add(new QuerySettings("fioLibrarian", fio));
            }

            if (DataValidation.IsContactNumberCorrect(textBoxContactNumberLibrarianNewValue.Text))
            {
                searchSettings.Add(new QuerySettings("contactNumber", textBoxContactNumberLibrarianNewValue.Text));
            }

            if (!string.IsNullOrEmpty(textBoxEmailLibrarianNewValue.Text))
            {
                if (!DataValidation.IsEmailCorrect(textBoxEmailLibrarianNewValue.Text))
                {
                    errorMessage += "\n" + "Заполните поле почта правильно." +
                        " Если вы уверены что ваша почта заполнена правильно, обратитесь к разработчику!";
                }
                searchSettings.Add(new QuerySettings("email", textBoxMailReaderNewValue.Text));
            }

            if (searchSettings.Count != 0)
            {
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    MessageBox.Show(errorMessage, "Перед редактированием устраните следующие ошибки:", MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                }
                else
                {
                    string oldFioLibrarian = textBoxSurnameLibrarianOldValue.Text + " " + textBoxNameLibrarianOldValue.Text + " " +
                                        textBoxPatronymicLibrarianOldValue.Text;
                    DatabaseUpdater.UpdateLibrarians(searchSettings, oldFioLibrarian);
                    HideLibrariansEditingControls();
                    LibrariansChange?.Invoke();
                }
            }
            else
            {
                MessageBox.Show("Измените хотя бы 1 поле");
            }
        }

        private void buttonPreviousBook_Click(object sender, EventArgs e)
        {
            if (CurrentBookNumber != 0)
            {
                ShowBook(--CurrentBookNumber);
            }
        }

        private void buttonNextBook_Click(object sender, EventArgs e)
        {
            if(CurrentBookNumber != BooksForDisplay.Count-1)
            {
                ShowBook(++CurrentBookNumber);
            }
        }

        private void buttonBooksEdit_Click(object sender, EventArgs e)
        {
            ShowBooksEditingControls();
            textBoxNameBookEditOldValue.Text = textBoxNameBookMainValue.Text;
            textBoxFioAutorEditOldValue.Text = textBoxFioAutorMainValue.Text;
            textBoxCategoryEditOldValue.Text = textBoxCategoryMainValue.Text;
            textBoxYearOfIssueEditOldValue.Text = textBoxYearOfIssueMainValue.Text;
            tabControlBooks.SelectedTab = tabControlBooks.TabPages["tabPageEditBook"];
        }

        private void buttonPasswordRecovery_Click(object sender, EventArgs e)
        {
            if(comboBoxNameLibrarian.Text == "")
            {
                PrintMessage.WarningMessage("Выберите ФИО библиотекаря", "Внимание");
                return;
            }
            ConfirmationCode = random.Next(1, 1000000);

            string message = "<h1>" + "Вы пытаетесь сменить пароль в приложении Библиотека" + "</h1>"+
                "<p>" + "Ваш код подтверждения - "+ ConfirmationCode + "</p>";
            string email = DatabaseSelectorSomeInformation.GetLibrarianEmail(comboBoxNameLibrarian.Text);
          
            PrintMessage.InformationMessage("Мы выслали на вашу почту " + email + " код подтверждения, введите его для смены пароля",
                "Код подтверждения");
            ChangeVisibleConfirmationCode();
            ChangeVisibleAutorization();
            MessageToEmail.SendMessageAsync(email, message);
        }

        private void ChangeVisibleAutorization()
        {
            for (int i = 0; i < tabControlMain.TabPages["tabPageAutorization"].Controls.Count; i++)
            {
                Control control = tabControlMain.TabPages["tabPageAutorization"].Controls[i];
                if (control.Tag != null && control.Tag.ToString() == "Autorization")
                {
                    control.Visible = control.Visible ? false :true;
                }
            }
        }

        private void ChangeVisibleConfirmationCode()
        {
            for (int i = 0; i < tabControlMain.TabPages["tabPageAutorization"].Controls.Count; i++)
            {
                Control control = tabControlMain.TabPages["tabPageAutorization"].Controls[i];
                if (control.Tag != null && control.Tag.ToString() == "ConfirmationCode")
                {
                    control.Visible = control.Visible ? false : true;
                }
            }
        }

        private void ChangeVisibleNewPassword()
        {
            for (int i = 0; i < tabControlMain.TabPages["tabPageAutorization"].Controls.Count; i++)
            {
                Control control = tabControlMain.TabPages["tabPageAutorization"].Controls[i];
                if (control.Tag != null && control.Tag.ToString() == "NewPassword")
                {
                    control.Visible = control.Visible ? false : true;
                }
            }
        }

        private void buttonCodeConfirmation_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConfirmationCode == Convert.ToInt32(textBoxCodeConfirmation.Text))
                {
                    PrintMessage.InformationMessage("Подтверждение кода выполнено успешно", "Код подтверждения");
                    ChangeVisibleConfirmationCode();
                    ChangeVisibleNewPassword();
                }
                else
                {
                    PrintMessage.WarningMessage("Код подтверждения введен неверно", "Код подтверждения");
                }
            }
            catch (FormatException)
            {
                PrintMessage.WarningMessage("Код подтверждения введен неверно", "Код подтверждения");
            }
        }

        private void buttonCancelChangePassword_Click(object sender, EventArgs e)
        {
            ChangeVisibleConfirmationCode();
            ChangeVisibleAutorization();
        }

        private void buttonCancelChangePassword2_Click(object sender, EventArgs e)
        {
            ChangeVisibleNewPassword();
            ChangeVisibleAutorization();
        }

        private void checkBoxShowPasswordConfirmationPassword_CheckedChanged(object sender, EventArgs e)
        {
            textBoxConfirmationNewPassword.PasswordChar = checkBoxShowPasswordConfirmationPassword.Checked ? char.MinValue : '*';
            checkBoxShowPasswordConfirmationPassword.Text = checkBoxShowPasswordConfirmationPassword.Checked ?
                "Cкрыть пароль" : "Показать пароль";
        }

        private void checkBoxShowPasswordNewPassword_CheckedChanged(object sender, EventArgs e)
        {
            textBoxNewPassword.PasswordChar = checkBoxShowPasswordNewPassword.Checked ? char.MinValue : '*';
            checkBoxShowPasswordNewPassword.Text = checkBoxShowPasswordNewPassword.Checked ?
                "Cкрыть пароль" : "Показать пароль";
        }

        private void buttonConfirmNewPassword_Click(object sender, EventArgs e)
        {
            if(textBoxNewPassword.Text != textBoxConfirmationNewPassword.Text)
            {
                PrintMessage.WarningMessage("Пароли не совпадают");
            }
            else
            {
                DatabaseUpdater.UpdatePasswordLibrarian(comboBoxNameLibrarian.Text, textBoxNewPassword.Text);
                ChangeVisibleNewPassword();
                ChangeVisibleAutorization();
                PrintMessage.InformationMessage("Смена пароля прошла успешно");
            }
        }

        private void checkBoxOnlyDebtors_CheckedChanged(object sender, EventArgs e)
        {
            LoadBookDeliveryMainContent();
        }

        private void checkBoxOnlyDebtors_CheckedChanged_1(object sender, EventArgs e)
        {
            LoadReadersMainContent();
        }

        private void checkBoxOnlyInStock_CheckedChanged(object sender, EventArgs e)
        {
            LoadBookMainContent();
        }

        private void comboBoxFIOReader_TextChanged(object sender, EventArgs e)
        {
            if (!DatabaseSelectorSomeInformation.IsReaderExists(comboBoxFIOReader.Text))
            {
                comboBoxFIOReader.Items.Clear();

                List<Reader> readers = DatabaseSelectorAllInformation.GetAllReaders(comboBoxFIOReader.Text);

                for (int i = 0; i < readers.Count; i++)
                {
                    comboBoxFIOReader.Items.Add(readers[i].FioReader);
                }

                comboBoxFIOReader.SelectionStart = comboBoxFIOReader.Text.Length;
            }
        }

        private void comboBoxNameBook_TextChanged(object sender, EventArgs e)
        {
            if (!DatabaseSelectorSomeInformation.IsBookExists(comboBoxNameBook.Text))
            {
                comboBoxNameBook.Items.Clear();

                List<string> namesBooks = DatabaseSelectorAllInformation.GetNamesOfBooks(comboBoxNameBook.Text);

                for (int i = 0; i < namesBooks.Count; i++)
                {
                    comboBoxNameBook.Items.Add(namesBooks[i]);
                }

                comboBoxNameBook.SelectionStart = comboBoxNameBook.Text.Length;
            }
        }

        private void comboBoxFioAutor_TextChanged(object sender, EventArgs e)
        {
            if (!DatabaseSelectorSomeInformation.IsAutorExists(comboBoxFioAutor.Text))
            {
                comboBoxFioAutor.Items.Clear();

                List<Autor> autors = DatabaseSelectorAllInformation.GetAllAutors(comboBoxFioAutor.Text);

                for (int i = 0; i < autors.Count; i++)
                {
                    comboBoxFioAutor.Items.Add(autors[i].FioAutor);
                }

                comboBoxFioAutor.SelectionStart = comboBoxFioAutor.Text.Length;
            }
        }

        private void comboBoxFioAutorEditNewValue_TextChanged(object sender, EventArgs e)
        {
            if (!DatabaseSelectorSomeInformation.IsAutorExists(comboBoxFioAutorEditNewValue.Text))
            {
                comboBoxFioAutorEditNewValue.Items.Clear();

                List<Autor> autors = DatabaseSelectorAllInformation.GetAllAutors(comboBoxFioAutorEditNewValue.Text);

                for (int i = 0; i < autors.Count; i++)
                {
                    comboBoxFioAutorEditNewValue.Items.Add(autors[i].FioAutor);
                }

                comboBoxFioAutorEditNewValue.SelectionStart = comboBoxFioAutorEditNewValue.Text.Length;
            }
        }

        private void comboBoxFioReaderReturnsBooks_TextChanged(object sender, EventArgs e)
        {
            if (!DatabaseSelectorSomeInformation.IsReaderExists(comboBoxFioReaderReturnsBooks.Text))
            {
                comboBoxFioReaderReturnsBooks.Items.Clear();

                List<string> values = DatabaseSelectorAllInformation.GetFiosReader(comboBoxFioReaderReturnsBooks.Text);

                for (int i = 0; i < values.Count; i++)
                {
                    comboBoxFioReaderReturnsBooks.Items.Add(values[i]);
                }

                comboBoxFioReaderReturnsBooks.SelectionStart = comboBoxFioReaderReturnsBooks.Text.Length;
            }
        }

        private void comboBoxNameBookInstances_TextChanged(object sender, EventArgs e)
        {
            if (!DatabaseSelectorSomeInformation.IsBookExists(comboBoxNameBookInstances.Text))
            {
                comboBoxNameBookInstances.Items.Clear();

                List<string> namesBooks = DatabaseSelectorAllInformation.GetNamesOfBooks(comboBoxNameBookInstances.Text);

                for (int i = 0; i < namesBooks.Count; i++)
                {
                    comboBoxNameBookInstances.Items.Add(namesBooks[i]);
                }

                comboBoxNameBookInstances.SelectionStart = comboBoxNameBookInstances.Text.Length;
            }
        }

        private void buttonSendMail_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxFioReaderReturnsBooks.Text) ||
               !DatabaseSelectorSomeInformation.IsReaderExists(comboBoxFioReaderReturnsBooks.Text))
            {
                PrintMessage.WarningMessage("Сперва выберите ФИО читателя");
            }
            else
            {
                string fioReader = comboBoxFioReaderReturnsBooks.Text;
                string messageBox = "Вы уверены, что хотите отправить сообщение читателю " +
                    fioReader + " о текующих выданных ему книгах?";
                string caption = "Отправка сообщения";
                var result = MessageBox.Show(messageBox, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    List<BookDelivery> bookDeliveries = DatabaseSelectorAllInformation.GetAllBookDelivery(comboBoxFioReaderReturnsBooks.Text);
                    string emailReader = DatabaseSelectorSomeInformation.GetReaderEmail(fioReader);

                    string message = "<h1>" + "Здравствуйте " + fioReader + " вас приветствует библиотека" + "</h1>" +
                        "<h2>" + "Хотим напомнить вам, что вы взяли в библиотеке книги:" + "</h2>";

                    for (int i = 0; i < bookDeliveries.Count; i++)
                    {
                        message += "<p>" + (i+1)+") "+ bookDeliveries[i].NameBook + " количество дней со дня выдачи - "+
                           bookDeliveries[i].NumberOfDaysAfterIssue + "</p>";
                    }

                    message += "<p><strong>" + "Напоминаем, что наша библиотека выдаёт книги на 30 дней, просим в ближайшее" +
                        "время вернуть книги, которые находятся у вас более 30 дней" + "</strong></p>";
                    message += "<p><strong>" + "С уважением, ваша библиотека" + "</strong></p>";

                    MessageToEmail.SendMessageAsync(emailReader, message);
                    PrintMessage.InformationMessage("Отправка сообщения прошла успешно");
                }
            }
        }

        private void buttonAutorization_Click(object sender, EventArgs e)
        {
            if(!DatabaseSelectorSomeInformation.IsLibrarianExists(comboBoxNameLibrarian.Text))
            {
                PrintMessage.InformationMessage("Выбирите библиотекаря из списка", "Библиотекарь не выбран");
            }
            else
            {
                string password = DatabaseSelectorSomeInformation.GetLibrarianPassword(comboBoxNameLibrarian.Text);

                if(password != textBoxPassword.Text)
                {
                    PrintMessage.WarningMessage("Пароль введен неверно");
                }
                else
                {
                    string email = DatabaseSelectorSomeInformation.GetLibrarianEmail(comboBoxNameLibrarian.Text);
                    AutorizationLibrarian = new Librarian(comboBoxNameLibrarian.Text, email, password);
                    tabControlMain.TabPages["tabPageAutorization"].Text = "Выйти";
                    labelNowLibrarian.Text = "Текующий библиотекарь - ";
                    comboBoxNameLibrarian.Text = "";
                    textBoxPassword.Text = "";
                    PrintMessage.InformationMessage("Авторизация прошла успешно");
                }
            }
        }

        private void tabControlMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (tabControlMain.SelectedTab == tabControlMain.TabPages["tabPageAutorization"])
            {
                string messageBox = "Вы уверены, что хотите cменить библиотекаря?";
                string caption = "Смена библиотекаря";
                var result = MessageBox.Show(messageBox, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    AutorizationLibrarian = null;
                    tabControlMain.TabPages["tabPageAutorization"].Text = "Авторизация";
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
