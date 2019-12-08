using System;
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
            LibrariansChange?.Invoke();

        }




        #region Authorization
        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.PasswordChar = checkBoxShowPassword.Checked ? char.MinValue : '*';
            checkBoxShowPassword.Text = checkBoxShowPassword.Checked ? "Cкрыть пароль" : "Показать пароль";
        }

        private void buttonAutorization_Click(object sender, EventArgs e)
        {
            tabControlMain.Visible = true;
        }
        #endregion

        private void tabPageBookingDelivery_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DateTime.Now.ToString());
        }

        #region PageBookDeliveries

        private void TextBoxesBookDeliverySeacrh_TextChanged(object sender, EventArgs e)
        {
            LoadBookDeliveryMainContent();
        }

        private void LoadBookDeliveryMainContent()
        {
            List<BookDelivery> bookDeliveries = DatabaseSelectorAllInformation.GetAllBookDelivery(GetSeacrhSettingsBookDelivery());
            dataGridViewBookDelivery.Rows.Clear();

            Image ig = Image.FromFile("1.jpg");

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
                    ig
                });
            }
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

        private List<SearchSettings> GetSeacrhSettingsBookDelivery()
        {
            List<SearchSettings> searchSettings = new List<SearchSettings>();

            if(!string.IsNullOrEmpty(textBoxFioReaderBookDelivery.Text))
            {
                searchSettings.Add(new SearchSettings("fioReader", textBoxFioReaderBookDelivery.Text));
            }
            if(!string.IsNullOrEmpty(textBoxNameBookBookDelivery.Text))
            {
                searchSettings.Add(new SearchSettings("nameBook", textBoxNameBookBookDelivery.Text));
            }
            if(!string.IsNullOrEmpty(textBoxFioLibrarianBookDelivery.Text))
            {
                searchSettings.Add(new SearchSettings("fioLibrarian", textBoxFioLibrarianBookDelivery.Text));
            }
            if (!string.IsNullOrEmpty(textBoxIdInstanceBookDelivery.Text))
            {
                searchSettings.Add(new SearchSettings("BookDelivery.idInstance", textBoxIdInstanceBookDelivery.Text));
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
            if (string.IsNullOrEmpty(comboBoxFIOReader.Text))
            {
                MessageBox.Show("Выберите читателя из списка");
                return;
            }
            if (string.IsNullOrEmpty(comboBoxNameBook.Text))
            {
                MessageBox.Show("Выберите наименование книги и номер экземпляра из соответствующих списков");
                return;
            }
            if (string.IsNullOrEmpty(comboBoxIdInstance.Text))
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
            Image ig = Image.FromFile("1.jpg");
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
                    ig
               });
            }
        }

        private void dataGridViewBookDeliveryReturnBooks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxFioReaderReturnsBooks.Text))
            {
                MessageBox.Show("Сперва выберите ФИО читателя");
            }
            else
            {
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
            List<Book> books = DatabaseSelectorAllInformation.GetAllBooks(GetSearchSettingsBooks());
            dataGridViewBooks.Rows.Clear();

           

            for (int i = 0; i < books.Count; i++)
            {
                Image picture = Image.FromFile(books[i].Picture);
                dataGridViewBooks.Rows.Add(new object[]
                {
                    books[i].NameBook,
                    books[i].FioAutor,
                    books[i].CountInStock,
                    books[i].Category,
                    books[i].YearOfIssue,
                    picture
                });
            }
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

        private List<SearchSettings> GetSearchSettingsBooks()
        {
            List<SearchSettings> searchSettings = new List<SearchSettings>();

            if (!string.IsNullOrEmpty(textBoxNameBook.Text))
            {
                searchSettings.Add(new SearchSettings("nameBook", textBoxNameBook.Text));
            }
            if (!string.IsNullOrEmpty(textBoxAuthor.Text))
            {
                searchSettings.Add(new SearchSettings("fioAutor", textBoxAuthor.Text));
            }
            if (!string.IsNullOrEmpty(textBoxCategory.Text))
            {
                searchSettings.Add(new SearchSettings("category", textBoxCategory.Text));
            }
            if (!string.IsNullOrEmpty(textBoxYearOfIssue.Text))
            {
                searchSettings.Add(new SearchSettings("yearOfIssue", textBoxYearOfIssue.Text));
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

        private List<SearchSettings> GetSearchSettingsInstances()
        {
            List<SearchSettings> searchSettings = new List<SearchSettings>();

            if (!string.IsNullOrEmpty(textBoxNameBookInstancesSearch.Text))
            {
                searchSettings.Add(new SearchSettings("nameBook", textBoxNameBookInstancesSearch.Text));
            }
            if (!string.IsNullOrEmpty(textBoxIdInstanceInstancesSearch.Text))
            {
                searchSettings.Add(new SearchSettings("idInstance", textBoxIdInstanceInstancesSearch.Text));
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
        #endregion

        #region Readers

        private void LoadReadersMainContent()
        {
            List<Reader> readers = DatabaseSelectorAllInformation.GetAllReaders(GetSearchSettingsReaders());
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
        private List<SearchSettings> GetSearchSettingsReaders()
        {
            List<SearchSettings> searchSettings = new List<SearchSettings>();

            if (!string.IsNullOrEmpty(textBoxFioReaderReadersSearch.Text))
            {
                searchSettings.Add(new SearchSettings("fioReader", textBoxFioReaderReadersSearch.Text));
            }
            if (!string.IsNullOrEmpty(textBoxContactNumberReadersSearch.Text))
            {
                searchSettings.Add(new SearchSettings("contactNumber", textBoxContactNumberReadersSearch.Text));
            }
            if (!string.IsNullOrEmpty(textBoxMailReadersSearch.Text))
            {
                searchSettings.Add(new SearchSettings("email", textBoxMailReadersSearch.Text));
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
        private List<SearchSettings> GetSearchSettingsLibrarians()
        {
            List<SearchSettings> searchSettings = new List<SearchSettings>();

            if (!string.IsNullOrEmpty(textBoxFioLibrarianLibrariansSearch.Text))
            {
                searchSettings.Add(new SearchSettings("fioLibrarian", textBoxFioLibrarianLibrariansSearch.Text));
            }
            if (!string.IsNullOrEmpty(textBoxContactNumberLibrariansSearch.Text))
            {
                searchSettings.Add(new SearchSettings("contactNumber", textBoxContactNumberLibrariansSearch.Text));
            }
            if (!string.IsNullOrEmpty(textBoxMailLibrariansSearch.Text))
            {
                searchSettings.Add(new SearchSettings("email", textBoxMailLibrariansSearch.Text));
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
        private List<SearchSettings> GetSearchSettingsAutors()
        {
            List<SearchSettings> searchSettings = new List<SearchSettings>();

            if (!string.IsNullOrEmpty(textBoxFioAutorAutorsSearch.Text))
            {
                searchSettings.Add(new SearchSettings("fioAutor", textBoxFioAutorAutorsSearch.Text));
            }

            return searchSettings;
        }

        private void TextBoxesAutorsChange(object sender, EventArgs e)
        {
            LoadAutorsMainContent();
        }
        #endregion

        private void dataGridViewBookDelivery_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            
        }

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
        }

       

        private void label15_Click(object sender, EventArgs e)
        {

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
                            if(AddedBook.CountInStock == 0)
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

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

        private void TextBoxesInstancesChange(object sender, EventArgs e)
        {
            LoadInstancesMainContent();
        }

        private static Random random = new Random();

        private void buttonBooksAdd_Click(object sender, EventArgs e)
        {
            if (CheckInformationAndPrintMessage.IsBookCorrect(textBoxNameBookBooks.Text, comboBoxFioAutor.Text,
                textBoxCountInStock.Text, textBoxCategoryBook.Text, textBoxPicture.Text, textBoxYearOfIssueAdd.Text))
                {
                Book newBook = new Book(textBoxNameBookBooks.Text, comboBoxFioAutor.Text,
                Convert.ToInt32(textBoxCountInStock.Text), textBoxCategoryBook.Text, textBoxPicture.Text,
                Convert.ToInt32(textBoxYearOfIssueAdd.Text));
                DatabaseInserter.InsertIntoBooks(newBook);
                AddedBook = newBook;

                BooksChange?.Invoke();

                labelNumberOfCopiesLeftToAdd.Text = "Осталось добавить " + newBook.CountInStock + " экземпляров книги " +
                    newBook.NameBook;
                labelNumberOfCopiesLeftToAdd.Visible = true;
                comboBoxNameBookInstances.Enabled = false;
                comboBoxNameBookInstances.Items.Clear();
                comboBoxNameBookInstances.Text = AddedBook.NameBook;
                MessageBox.Show("Добавление книги " + newBook.NameBook + " произошло успешно, теперь необходимо добавить " +
                    newBook.CountInStock + " экземпляров этой книги");
                tabControlMain.SelectedTab = tabControlMain.TabPages["tabPageInstances"];
                tabControlInstances.SelectedTab = tabControlInstances.TabPages["tabPageAddInstances"];
            }
        }

        private void tabPageInstances_Leave(object sender, EventArgs e)
        {
            
        }

        private Book AddedBook = null;

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

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void tabPageInstances_Enter(object sender, EventArgs e)
        {

        }

        private void tabControlMain_Deselected(object sender, TabControlEventArgs e)
        {
            
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
        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonReadersAdd_Click(object sender, EventArgs e)
        {
            if(CheckInformationAndPrintMessage.IsReaderCorrect(textBoxSurnameReader.Text,
                textBoxNameReader.Text,textBoxPatronymicReader.Text,textBoxContactNumberReader.Text,
                textBoxMailReader.Text))
            {
                string fioReader = textBoxSurnameReader.Text + " " + textBoxNameReader.Text + " " + textBoxPatronymicReader.Text;
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

        private void label53_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonLibrariansAdd_Click(object sender, EventArgs e)
        {
            if(CheckInformationAndPrintMessage.IsLibrarianCorrect(textBoxSurnameLibrarian.Text,
                textBoxNameLibrarian.Text,textBoxPatronymicLibrarian.Text,textBoxContactNumberLibrarian.Text,
                textBoxMailLibrarian.Text, textBoxPasswordLibrarian.Text, textBoxPasswordConfirmation.Text))
            {
                string fioLibrarian = textBoxSurnameLibrarian.Text + " " + textBoxNameLibrarian.Text +
                    " " + textBoxPatronymicLibrarian.Text;
                DatabaseInserter.InsertIntoLibrarians(new Librarian(fioLibrarian, textBoxContactNumberLibrarian.Text,
                    textBoxMailLibrarian.Text, textBoxPassword.Text));
                LibrariansChange?.Invoke();
            }
        }

        private void dataGridViewBooks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                ShowBooksEditingControls();
                textBoxNameBookEditOldValue.Text = dataGridViewBooks[0, e.RowIndex].Value.ToString();
                textBoxFioAutorEditOldValue.Text = dataGridViewBooks[1, e.RowIndex].Value.ToString();
                textBoxCategoryEditOldValue.Text = dataGridViewBooks[3, e.RowIndex].Value.ToString();
                textBoxYearOfIssueEditOldValue.Text = dataGridViewBooks[4, e.RowIndex].Value.ToString();
                Image image = (Image)dataGridViewBooks[5, e.RowIndex].Value;
                pictureBoxBookEditOldValue.Image = image;
                tabControlBooks.SelectedTab = tabControlBooks.TabPages["tabPageEditBook"];
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
                pictureBoxEdit.Load(newFileName);
                textBoxPictureEdit.Text = newFileName;
            }
            else
            {
                File.Copy(fullFileName, fileName);
                pictureBoxEdit.Load(fileName);
                textBoxPictureEdit.Text = fileName;
            }
        }

        private void buttonBooksEditing_Click(object sender, EventArgs e)
        {
            List<SearchSettings> searchSettings = new List<SearchSettings>();

            if (!string.IsNullOrEmpty(textBoxNameBookNewValue.Text))
            {
                if (DatabaseSelectorSomeInformation.IsBookExists(textBoxNameBookNewValue.Text))
                {
                    MessageBox.Show("Книга с таким названием уже существует");
                    return;
                }
                searchSettings.Add(new SearchSettings("nameBook", textBoxNameBookNewValue.Text));
            }
            if (!string.IsNullOrEmpty(comboBoxFioAutorEditNewValue.Text))
            {
                int idAutor = DatabaseSelectorSomeInformation.GetIdAutor(comboBoxFioAutorEditNewValue.Text);

                searchSettings.Add(new SearchSettings("idAutor", idAutor.ToString()));
            }
            if (!string.IsNullOrEmpty(textBoxCategoryBookNewValue.Text))
            {
                searchSettings.Add(new SearchSettings("category", textBoxCategoryBookNewValue.Text));
            }
            if (!string.IsNullOrEmpty(textBoxPictureEdit.Text))
            {
                searchSettings.Add(new SearchSettings("picture", textBoxPictureEdit.Text));
            }
            if (!string.IsNullOrEmpty(textBoxYearOfIssueNewValue.Text))
            {
                searchSettings.Add(new SearchSettings("yearOfIssue", textBoxYearOfIssueNewValue.Text));
            }
            if (searchSettings.Count != 0)
            {
                DatabaseUpdater.UpdateBooks(searchSettings,textBoxNameBookEditOldValue.Text);
                BooksChange?.Invoke();
                HideBooksEditingControls();
            }
            else
            {
                MessageBox.Show("Измените хотя бы 1 поле");
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
        }

        private void HideReadersEditingControls()
        {
            for (int i = 0; i < tabControlReaders.TabPages["tabPageEdit"].Controls.Count; i++)
            {
                Control control = tabControlReaders.TabPages["tabPageEdit"].Controls[i];
                if (control.Tag != null && control.Tag.ToString() == "Hidden")
                {
                    control.Visible = false;
                }
            }
        }

        private void ShowReadersEditingControls()
        {
            for (int i = 0; i < tabControlReaders.TabPages["tabPageEdit"].Controls.Count; i++)
            {
                Control control = tabControlReaders.TabPages["tabPageEdit"].Controls[i];
                if (control.Tag != null && control.Tag.ToString() == "Hidden")
                {
                    control.Visible = true;
                }
            }
        }

        private void pictureBoxEdit_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewReaders_DoubleClick(object sender, EventArgs e)
        {
           
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
                tabControlReaders.SelectedTab = tabControlReaders.TabPages["tabPageEdit"];
                ShowReadersEditingControls();
            }
        }

        private void buttonReadersEditing_Click(object sender, EventArgs e)
        {
            List<SearchSettings> searchSettings = new List<SearchSettings>();

            string surname = textBoxSurnameReaderOldValue.Text;
            string name = textBoxNameReaderOldValue.Text;
            string patronymic = textBoxPatronymicReaderOldValue.Text;
            if (!string.IsNullOrEmpty(textBoxSurnameReaderNewValue.Text))
            {
                surname = textBoxSurnameReaderNewValue.Text;
            }
            if (!string.IsNullOrEmpty(textBoxNameReaderNewValue.Text))
            {
                name = textBoxNameReaderNewValue.Text;
            }
            if (!string.IsNullOrEmpty(textBoxPatronymicReaderNewValue.Text))
            {
                patronymic = textBoxPatronymicReaderNewValue.Text;
            }
            string fio = surname + " " + name + " " + patronymic;

            if (DatabaseSelectorSomeInformation.IsReaderExists(fio))
            {
                MessageBox.Show("Читатель с таким ФИО уже существует");
                return;
            }
            else
            {
                searchSettings.Add(new SearchSettings("fioReader", fio));
            }

            if(CheckInformationAndPrintMessage.IsContactNumberCorrect(textBoxContactNumberReaderNewValue.Text))
            {
                searchSettings.Add(new SearchSettings("contactNumber", textBoxContactNumberReaderNewValue.Text));
            }          

            if (!string.IsNullOrEmpty(textBoxMailReaderNewValue.Text))
            {
                searchSettings.Add(new SearchSettings("email", textBoxMailReaderNewValue.Text));
            }

            if(searchSettings.Count != 0)
            {
                string oldFioReader = textBoxSurnameReaderOldValue.Text + " " + textBoxNameReaderOldValue.Text + " " +
                    textBoxPatronymicReaderOldValue.Text;
                DatabaseUpdater.UpdateReaders(searchSettings, oldFioReader);
                HideReadersEditingControls();
                ReadersChange?.Invoke();
            }
            else
            {
                MessageBox.Show("Измените хотя бы 1 поле");
            }
        }
    }
}
