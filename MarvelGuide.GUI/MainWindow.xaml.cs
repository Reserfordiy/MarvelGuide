using MarvelGuide.Core;
using MarvelGuide.Core.Intefraces;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarvelGuide.GUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IStorage _storage;


        public MainWindow()
        {
            _storage = Factory.Instance.GetStorage();

            InitializeComponent();
        }


        private void ShowDocumentsButton_Click(object sender, RoutedEventArgs e)
        {
            AllDocumentsWindow allDocumentsWindow = new AllDocumentsWindow();

            allDocumentsWindow.Show();

            Close();
        }



        private void LoginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UnexpectedFunctional();
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UnexpectedFunctional();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            UnexpectedFunctional();
        }


        private void UnexpectedFunctional()
        {
            MessageBox.Show("Извините, функционал личного кабинета будет доступен в следующей версии приложения!", "Ошибка!");

            ShowDocumentsButton.Focus();
        }
    }
}
