using MarvelGuide.Core;
using MarvelGuide.Core.Helpers;
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
        private const string defaultDocumentDate = "Пример: 25.01.2018";

        private const string basicVersion = "Исходная версия";
        private const string regularVersion = "Версия от ";
        private const string currentVersion = "  (тек.)";

        private const string foundationStringDate = "12.05.2010";


        IStorage _storage;

        Document _document;
        User _user;


        bool _showingVersion = false;

        DocumentVersion _versionForShowing = null;



        public EditDocumentWindow(Document document, User user)
        {
            _document = document;
            _user = user;

            _storage = Factory.Instance.GetStorage();

            InitializeComponent();

            WindowState = WindowState.Maximized;

            NameTextBox.Width = MaxWidth / 3 + 83;
            DateTextBox.Width = MaxWidth / 3 - 4;

            FormingTheContent();
        }


        private void FormingTheContent()
        {
            if (_document.Id != -1)
            {
                FormingTheEdittingData();
            }
            else
            {
                FormingEmptyFields();
            }
        }        

        private void FormingTheEdittingData()
        {            
            NameTextBox.Text = _document.Name;
            NameTextBox.Foreground = Brushes.Black;

            DateTextBox.Text = _document.Versions[0].Date.ToString("d");
            DateTextBox.Foreground = Brushes.Black;

            if (_document.IsPublic) { PublicDocumentRadioButton.IsChecked = true; }
            else { HiddenDocumentRadioButton.IsChecked = true; }

            VersionsListBox.ItemsSource = _document.Versions;

            ContentTextBox.Text = _document.Versions[_document.Versions.Count - 1].Text;
            ContentTextBox.Foreground = Brushes.Black;            
        }

        private void FormingEmptyFields()
        {
            VersionsTextBlock.Visibility = Visibility.Collapsed;
            VersionsListBox.Visibility = Visibility.Collapsed;
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_showingVersion)
            {
                DocumentWindow documentWindow = new DocumentWindow(_document, _versionForShowing, true, true, _user);

                documentWindow.Show();
            }
            else
            {
                AllDocumentsWindow allDocumentsWindow = new AllDocumentsWindow(_user);

                allDocumentsWindow.Show();
            }
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

        private void DateTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DateTextBox.Text == defaultDocumentDate)
            {
                DateTextBox.Text = "";
                DateTextBox.Foreground = Brushes.Black;
            }
        }

        private void DateTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DateTextBox.Text == "")
            {
                DateTextBox.Text = defaultDocumentDate;
                DateTextBox.Foreground = Brushes.Gray;
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
                NameTextBox.Width = Width / 3 + 83;
                DateTextBox.Width = Width / 3 - 4;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                NameTextBox.Width = MaxWidth / 3 + 83;
                DateTextBox.Width = MaxWidth / 3 - 4;
                SaveDataButton.Margin = new Thickness(SaveDataButton.Margin.Left, SaveDataButton.Margin.Top, SaveDataButton.Margin.Right, 100);

            }
            else if (WindowState == WindowState.Normal)
            {
                NameTextBox.Width = Width / 3 + 83;
                DateTextBox.Width = Width / 3 - 4;
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
            if (DateTextBox.Text == defaultDocumentDate)
            {
                MessageBox.Show("Укажите дату создания документа в формате ДД.ММ.ГГГГ либо нажмите на кнопку 'Cегодня', чтобы задать в качестве даты создания сегодняшний день.", "Ошибка");

                DateTextBox.Focus();

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
            if (!HelpingMethods.TryParsingTheDate(DateTextBox.Text))
            {
                MessageBox.Show("Дата в полях должна задаваться в формате ДД.ММ.ГГГГ — например: 25.05.2017 . Оформите дату создания документа корректно либо воспользуйтесь кнопкой 'Сегодня', чтобы быстро указать сегодняшний день.", "Ошибка");

                DateTextBox.Text = "";
                DateTextBox.Focus();

                return false;
            }
            if (DateTime.Parse(DateTextBox.Text) > DateTime.Now)
            {
                MessageBox.Show("Некорректная дата. Этот день еще не наступил.", "Ошибка");

                DateTextBox.Text = "";
                DateTextBox.Focus();

                return false;
            }
            if (DateTime.Parse(DateTextBox.Text) < DateTime.Parse(foundationStringDate))
            {
                MessageBox.Show("Некорректная дата. В это время нашей компании еще не существовало.", "Ошибка");

                DateTextBox.Text = "";
                DateTextBox.Focus();

                return false;
            }
            if (_document.Id != -1 && _document.Versions.Count > 1 && DateTime.Parse(DateTextBox.Text) > _document.Versions[1].Date)
            {
                MessageBox.Show("Некорректная дата создания документа. К моменту наступления этого дня, в системе уже были зарегестрированы новые версии документа.", "Ошибка");

                DateTextBox.Text = "";
                DateTextBox.Focus();

                return false;
            }

            return true;
        }


        private void FixingDataAboutDocument()
        {
            if (_document.Id == -1)
            {
                SaveNewDocument();
            }
            else
            {
                SaveEdittedDocument();
            }
        }

        private void SaveEdittedDocument()
        {
            _document.Name = NameTextBox.Text;

            _document.Versions[0].Date = DateTime.Parse(DateTextBox.Text);

            if (PublicDocumentRadioButton.IsChecked == true) { _document.IsPublic = true; }
            else { _document.IsPublic = false; }

            if (ContentTextBox.Text != _document.Versions[_document.Versions.Count - 1].Text)
            {
                _document.Versions.Add(new DocumentVersion
                {
                    Date = DateTime.Now,
                    Text = ContentTextBox.Text
                });
            }
        }

        private void SaveNewDocument()
        {
            _storage.Documents.Add(_document);

            _document.Id = _storage.Documents.Items.Max(doc => doc.Id) + 1;

            _document.Name = NameTextBox.Text;

            if (PublicDocumentRadioButton.IsChecked == true) { _document.IsPublic = true; }
            else { _document.IsPublic = false; }

            _document.Versions = new List<DocumentVersion>
            {
                new DocumentVersion
                {
                    Date = DateTime.Parse(DateTextBox.Text),
                    Text = ContentTextBox.Text
                }
            };
        }



        private void TodayButton_Click(object sender, RoutedEventArgs e)
        {
            DateTextBox.Text = DateTime.Now.ToString("d");
            DateTextBox.Foreground = Brushes.Black;
        }



        private void VersionNameTextBlock_Initialized(object sender, EventArgs e)
        {
            TextBlock VersionNameTextBlock = sender as TextBlock;

            DocumentVersion version = VersionNameTextBlock.DataContext as DocumentVersion;

            if (_document.Versions.IndexOf(version) == 0)
            {
                VersionNameTextBlock.Text = basicVersion;
            }
            else if (_document.Versions.IndexOf(version) == _document.Versions.Count - 1)
            {
                VersionNameTextBlock.Text = regularVersion + version.Date.ToString("d") + currentVersion;
            }
            else
            {
                VersionNameTextBlock.Text = regularVersion + version.Date.ToString("d");
            }
        }

        

        private void ShowVersionButton_Click(object sender, RoutedEventArgs e)
        {
            Button ShowVersionButton = sender as Button;

            DocumentVersion version = ShowVersionButton.DataContext as DocumentVersion;

            _showingVersion = true;

            _versionForShowing = version;

            Close();
        }


        private void DeleteVersionButton_Click(object sender, RoutedEventArgs e)
        {
            Button DeleteButton = sender as Button;

            DocumentVersion version = DeleteButton.DataContext as DocumentVersion;

            if (_document.Versions.Count == 1)
            {
                if (MessageBox.Show($"Данное действие удалит весь документ целиком. Восстановить его будет невозможно. Вы уверены, что хотите продолжить?", "Предупреждение", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                {
                    _storage.Documents.Remove(_document);

                    _storage.Documents.Save();

                    Close();
                }
            }
            else if (_document.Versions.IndexOf(version) == 0)
            {
                if (MessageBox.Show($"Вы уверены, что хотите удалить исходную версию документа? Отменить это действие будет невозможно, и исходной версией будет назначена следующая за ней версия!", "Предупреждение", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                {
                    DeletingTheVersion(version);

                    DateTextBox.Text = _document.Versions[0].Date.ToString("d");
                }
            }
            else if (_document.Versions.IndexOf(version) == _document.Versions.Count - 1)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить текущую версию документа? Все изменения, внесенные в предпоследнюю версию, будут безвозвратно утрачены.", "Предупреждение", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                {
                    DeletingTheVersion(version);

                    ContentTextBox.Text = _document.Versions[_document.Versions.Count - 1].Text;
                }
            }
            else
            {
                if (MessageBox.Show($"Вы уверены, что хотите удалить версию от {version.Date.ToString("d")}? Отменить это действие будет невозможно!", "Предупреждение", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                {
                    DeletingTheVersion(version);
                }
            }
        }

        private void DeletingTheVersion(DocumentVersion version)
        {
            _document.Versions.Remove(version);

            _storage.Documents.Save();

            VersionsListBox.ItemsSource = null;
            VersionsListBox.ItemsSource = _document.Versions;
        }
    }
}
