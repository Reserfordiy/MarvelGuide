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


        bool _readingInTheDeveloperMode = false;

        User _userWhoReads = null;

        

        public DocumentWindow(Document document, bool readingInTheDeveloperMode, User userWhoReads)
        {
            _document = document;

            _readingInTheDeveloperMode = readingInTheDeveloperMode;

            _userWhoReads = userWhoReads;

            InitializeComponent();

            WindowState = WindowState.Maximized;

            DocumentNameTextBlock.Text = document.Name;
            DocumentContentTextBlock.Text = document.Text;
            VersionDateTextBlock.Text = versionText + document.CreationDate.ToString("d");
        }

        public DocumentWindow(Document document) : this(document, false, null) { }
        public DocumentWindow(Document document, User userWhoReads) : this(document, true, userWhoReads) { }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_readingInTheDeveloperMode)
            {
                AllDocumentsWindow allDocumentsWindow = new AllDocumentsWindow();

                allDocumentsWindow.Show();
            }
            else
            {
                AllDocumentsWindow allDocumentsWindow = new AllDocumentsWindow(_userWhoReads);

                allDocumentsWindow.Show();
            }
        }
    }
}
