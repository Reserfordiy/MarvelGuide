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
        IStorage _storage;


        public AllDocumentsWindow()
        {
            _storage = Factory.Instance.GetStorage();

            InitializeComponent();

            PublishingTheDocuments();
        }


        private void PublishingTheDocuments()
        {
            AllDocumentsListBox.ItemsSource = _storage.Documents.Items;
        }







        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();

            mainWindow.Show();

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

            Document document = ReadButton.DataContext as Document;

            DocumentWindow documentWindow = new DocumentWindow(document);

            documentWindow.Show();

            Close();
        }
    }
}
