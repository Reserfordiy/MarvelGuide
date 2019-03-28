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
    /// Логика взаимодействия для AllDocumentsWindow.xaml
    /// </summary>
    public partial class AllDocumentsWindow : Window
    {
        private const string publicDocumentsLabel = "Публичные документы:";



        IStorage _storage;


        bool _lookingInTheDeveloperMode = false;

        User _userWhoWatches = null;


        bool _goingToRead = false;
        bool _goingToEdit = false;

        Document _documentForReadingOrEditing = null;



        public AllDocumentsWindow(bool lookingInTheDeveloperMode, User userWhoWatches)
        {
            _storage = Factory.Instance.GetStorage();

            _lookingInTheDeveloperMode = lookingInTheDeveloperMode;

            _userWhoWatches = userWhoWatches;

            InitializeComponent();

            CheckingWhetherWeAreInDeveloperMode();

            PublishingTheDocuments();
        }

        public AllDocumentsWindow() : this(false, null) { }
        public AllDocumentsWindow(User userWhowatches) : this(true, userWhowatches) { }



        private void CheckingWhetherWeAreInDeveloperMode()
        {
            if (_lookingInTheDeveloperMode)
            {
                PublicDocumentsTextBlock.Text = publicDocumentsLabel;

                WindowState = WindowState.Maximized;
            }
            else
            {
                PublicDocumentsListBox.Margin = HiddenDocumentsListBox.Margin;
                PublicDocumentsTextBlock.Margin = new Thickness(PublicDocumentsTextBlock.Margin.Left, PublicDocumentsTextBlock.Margin.Top - 30, PublicDocumentsTextBlock.Margin.Right, PublicDocumentsTextBlock.Margin.Bottom);

                HiddenDocumentsTextBlock.Visibility = Visibility.Collapsed;
                HiddenDocumentsListBox.Visibility = Visibility.Collapsed;
                AddDocumentButton.Visibility = Visibility.Collapsed;
            }                
        }


        private void PublishingTheDocuments()
        {
            PublicDocumentsListBox.ItemsSource = _storage.Documents.Items
                .Where(doc => doc.IsPublic)
                .OrderByDescending(doc => doc.Versions[doc.Versions.Count - 1].Date);
            HiddenDocumentsListBox.ItemsSource = _storage.Documents.Items
                .Where(doc => !doc.IsPublic)
                .OrderByDescending(doc => doc.Versions[doc.Versions.Count - 1].Date);
        }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void DocumentNameTextBlock_Initialized(object sender, EventArgs e)
        {
            TextBlock DocumentNameTextBlock = sender as TextBlock;

            Document document = DocumentNameTextBlock.DataContext as Document;

            DocumentNameTextBlock.Text = document.Name;
        }



        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            Button ReadButton = sender as Button;

            _documentForReadingOrEditing = ReadButton.DataContext as Document;

            _goingToRead = true;

            Close();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button EditButton = sender as Button;

            _documentForReadingOrEditing = EditButton.DataContext as Document;

            _goingToEdit = true;

            Close();
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_goingToRead)
            {
                if (_lookingInTheDeveloperMode)
                {
                    DocumentWindow documentWindow = new DocumentWindow(_documentForReadingOrEditing, _userWhoWatches);

                    documentWindow.Show();
                }
                else
                {
                    DocumentWindow documentWindow = new DocumentWindow(_documentForReadingOrEditing);

                    documentWindow.Show();
                }
            }
            else if (_goingToEdit)
            {
                EditDocumentWindow editDocumentWindow = new EditDocumentWindow(_documentForReadingOrEditing, _userWhoWatches);

                editDocumentWindow.Show();
               
            }
            else
            {
                if (_lookingInTheDeveloperMode)
                {
                    ProfileWindow profileWindow = new ProfileWindow(_userWhoWatches);

                    profileWindow.Show();
                }
                else
                {
                    MainWindow mainWindow = new MainWindow();

                    mainWindow.Show();
                }
            }
        }



        private void ReadButton_Initialized(object sender, EventArgs e)
        {
            Button ReadButton = sender as Button;
            
            if (!_lookingInTheDeveloperMode)
            {
                ReadButton.Padding = new Thickness(ReadButton.Padding.Left + 2, ReadButton.Padding.Top, ReadButton.Padding.Right + 2, ReadButton.Padding.Bottom);
            }
        }

        private void EditButton_Initialized(object sender, EventArgs e)
        {
            Button EditButton = sender as Button;

            if (!_lookingInTheDeveloperMode)
            {
                EditButton.Visibility = Visibility.Collapsed;
            }
        }



        private void AddDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            _documentForReadingOrEditing = new Document { Id = -1 };

            _goingToEdit = true;

            Close();
        }



        private void ListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Scroll.ScrollToVerticalOffset(Scroll.VerticalOffset - (double)e.Delta * 5 / 12);
        }
    }
}
