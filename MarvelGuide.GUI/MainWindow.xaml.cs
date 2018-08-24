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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarvelGuide.GUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string defaultLogin = "Логин";
        private const string defaultPassword = "Пароль";


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
            if (LoginTextBox.Text == defaultLogin)
            {
                LoginTextBox.Text = "";

                LoginTextBox.Foreground = Brushes.Black;
            }
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Text == defaultPassword)
            {
                PasswordTextBox.Text = "";

                PasswordTextBox.Foreground = Brushes.Black;
            }
        }

        private void LoginTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "")
            {
                LoginTextBox.Text = defaultLogin;

                LoginTextBox.Foreground = Brushes.Gray;
            }
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Text == "")
            {
                PasswordTextBox.Text = defaultPassword;

                PasswordTextBox.Foreground = Brushes.Gray;
            }
        }



        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Authorization();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Authorization();
            }
        }



        private void Authorization()
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Text;

            var user = _storage.Users.Items.FirstOrDefault(u => u.Login == login && password == u.Password);

            if (user == null)
            {
                MessageBox.Show("Введенной пары логина и пароля не найдено в базе. Попробуйте еще раз либо обратитесь к разработчикам!", "Ошибка!");

                LoginTextBox.Text = defaultLogin;
                PasswordTextBox.Text = defaultPassword;
                LoginTextBox.Foreground = Brushes.Gray;
                PasswordTextBox.Foreground = Brushes.Gray;
            }

            else
            {
                ProfileWindow profileWindow = new ProfileWindow(user, _storage);

                profileWindow.Show();

                Close();
            }
        }       
    }
}
