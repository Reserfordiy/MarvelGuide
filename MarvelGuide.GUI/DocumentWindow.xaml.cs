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
        Document _document;

        public DocumentWindow(Document document)
        {
            _document = document;

            InitializeComponent();

            DocumentNameTextBlock.Text = document.Name;
            DocumentContentTextBlock.Text = document.Text;
        }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AllDocumentsWindow allDocumentsWindow = new AllDocumentsWindow();

            allDocumentsWindow.Show();

            Close();
        }
    }
}
