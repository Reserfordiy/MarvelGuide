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
    /// Логика взаимодействия для DocumentWindow.xaml
    /// </summary>
    public partial class DocumentWindow : Window
    {
        private const string versionText = "Версия от ";


        Document _document;
        DocumentVersion _version;

        DocumentVersion _followingVersion = null;


        bool _readingInTheDeveloperMode = false;
        bool _readingFromEditDocumentWindow = false;

        bool _goingToAnotherVersion = false;

        User _userWhoReads = null;

        

        public DocumentWindow(Document document, DocumentVersion version, bool readingInTheDeveloperMode, bool readingFromEditDocumentWindow, User userWhoReads)
        {
            _document = document;
            _version = version;

            _readingInTheDeveloperMode = readingInTheDeveloperMode;
            _readingFromEditDocumentWindow = readingFromEditDocumentWindow;

            _userWhoReads = userWhoReads;

            InitializeComponent();

            WindowState = WindowState.Maximized;

            FormingTheVisualData();            
        }

        public DocumentWindow(Document document) : this(document, document.Versions[document.Versions.Count - 1], false, false, null) { }
        public DocumentWindow(Document document, User userWhoReads) : this(document, document.Versions[document.Versions.Count - 1], true, false, userWhoReads) { }


        private void FormingTheVisualData()
        {
            DocumentNameTextBlock.Text = _document.Name;
            DocumentContentTextBlock.Text = _version.Text;
            VersionDateTextBlock.Text = versionText + _version.Date.ToString("d");

            if (_document.Versions.Count == 1)
            {
                VersionDateTextBlock.Margin = new Thickness(0, VersionDateTextBlock.Margin.Top, 0, VersionsButtonsGrid.Margin.Bottom - 2);
                VersionsButtonsGrid.Visibility = Visibility.Collapsed;               
            }
            else if (_document.Versions.IndexOf(_version) == 0)
            {
                PreviousVersionButton.Visibility = Visibility.Collapsed;
            }
            else if (_document.Versions.IndexOf(_version) == _document.Versions.Count - 1)
            {
                NextVersionButton.Visibility = Visibility.Collapsed;
            }
        }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_goingToAnotherVersion)
            {
                DocumentWindow documentWindow = new DocumentWindow(_document, _followingVersion, _readingInTheDeveloperMode, _readingFromEditDocumentWindow, _userWhoReads);

                documentWindow.Show();
            }
            else if (_readingFromEditDocumentWindow && _readingInTheDeveloperMode)
            {
                EditDocumentWindow editDocumentWindow = new EditDocumentWindow(_document, _userWhoReads);

                editDocumentWindow.Show();
            }
            else if (_readingInTheDeveloperMode)
            {
                AllDocumentsWindow allDocumentsWindow = new AllDocumentsWindow(_userWhoReads);

                allDocumentsWindow.Show();
            }
            else
            {
                AllDocumentsWindow allDocumentsWindow = new AllDocumentsWindow();

                allDocumentsWindow.Show();
            }
            
        }



        private void PreviousVersionButton_Click(object sender, RoutedEventArgs e)
        {
            _goingToAnotherVersion = true;
            _followingVersion = _document.Versions[_document.Versions.IndexOf(_version) - 1];

            Close();
        }

        private void NextVersionButton_Click(object sender, RoutedEventArgs e)
        {
            _goingToAnotherVersion = true;
            _followingVersion = _document.Versions[_document.Versions.IndexOf(_version) + 1];

            Close();
        }
    }
}
