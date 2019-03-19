using MarvelGuide.Core;
using MarvelGuide.Core.Intefraces;
using MarvelGuide.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MarvelGuide.GUI
{
    /// <summary>
    /// Логика взаимодействия для EditDocumentWindow.xaml
    /// </summary>
    public partial class EditDocumentWindow : Window
    {
        private const string defaultDocumentName = "Пример: Инструкции для агентов";
        private const string defaultDocumentText = "Пример: Все сотрудники должны выполнять правила. Если правила выполняться не будут, зачем тогда они вообще нужны?";


        IStorage _storage;

        Document _document;
        User _user;        



        public EditDocumentWindow(Document document, User user)
        {
            _document = document;
            _user = user;

            _storage = Factory.Instance.GetStorage();

            InitializeComponent();

            WindowState = WindowState.Maximized;
            NameTextBox.Width = MaxWidth / 3 + 50;

            FormingTheEdittingData();
        }


        private void FormingTheEdittingData()
        {
            if (_document.Id != -1)
            {
                NameTextBox.Text = _document.Name;
                NameTextBox.Foreground = Brushes.Black;

                if (_document.IsPublic) { PublicDocumentRadioButton.IsChecked = true; }
                else { HiddenDocumentRadioButton.IsChecked = true; }

                ContentTextBox.Text = _document.Text;
                ContentTextBox.Foreground = Brushes.Black;
            }
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AllDocumentsWindow allDocumentsWindow = new AllDocumentsWindow(_user);

            allDocumentsWindow.Show();
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void NameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == defaultDocumentName)
            {
                NameTextBox.Text = "";
                NameTextBox.Foreground = Brushes.Black;
            }
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == "")
            {
                NameTextBox.Text = defaultDocumentName;
                NameTextBox.Foreground = Brushes.Gray;
            }
        }

        private void ContentTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ContentTextBox.Text == defaultDocumentText)
            {
                ContentTextBox.Text = "";
                ContentTextBox.Foreground = Brushes.Black;
            }
        }

        private void ContentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ContentTextBox.Text == "")
            {
                ContentTextBox.Text = defaultDocumentText;
                ContentTextBox.Foreground = Brushes.Gray;
            }
        }



        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                NameTextBox.Width = Width / 3 + 50;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                NameTextBox.Width = MaxWidth / 3 + 50;
                SaveDataButton.Margin = new Thickness(SaveDataButton.Margin.Left, SaveDataButton.Margin.Top, SaveDataButton.Margin.Right, 100);

            }
            else if (WindowState == WindowState.Normal)
            {
                NameTextBox.Width = Width / 3 + 50;
                SaveDataButton.Margin = new Thickness(SaveDataButton.Margin.Left, SaveDataButton.Margin.Top, SaveDataButton.Margin.Right, 65);
            }
        }



        private void SaveDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckingWhetherAllFieldsFilledCorrectly())
            {
                FixingDataAboutDocument();

                _storage.Documents.Save();

                Close();
            }
        }


        private bool CheckingWhetherAllFieldsFilledCorrectly()
        {
            if (NameTextBox.Text == defaultDocumentName)
            {
                MessageBox.Show("Укажите название документа.", "Ошибка");

                NameTextBox.Focus();

                return false;
            }
            if (PublicDocumentRadioButton.IsChecked == false && HiddenDocumentRadioButton.IsChecked == false)
            {
                MessageBox.Show("Укажите уровень доступности документа.", "Ошибка");

                PublicDocumentRadioButton.Focus();

                return false;
            }
            if (ContentTextBox.Text == defaultDocumentText)
            {
                MessageBox.Show("Документ не может быть пустым.", "Ошибка");

                ContentTextBox.Focus();

                return false;
            }
            if (_storage.Documents.Items.Count(doc => doc.Name == NameTextBox.Text && doc != _document) > 0)
            {
                MessageBox.Show("В системе уже существует документ с таким названием.", "Ошибка");

                NameTextBox.Text = "";
                NameTextBox.Focus();

                return false;
            }

            return true;
        }


        private void FixingDataAboutDocument()
        {
            if (_document.Id == -1)
            {
                _storage.Documents.Add(_document);

                int id = _storage.Documents.Items.Max(doc => doc.Id);

                _document.Id = id + 1;
            }

            _document.Name = NameTextBox.Text;

            if (PublicDocumentRadioButton.IsChecked == true) { _document.IsPublic = true; }
            else { _document.IsPublic = false; }

            _document.Text = ContentTextBox.Text;
        }
    }
}
