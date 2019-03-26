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

            //_storage.ChangingUsersModels();
            _storage.ChangingDocumentsModels();

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
            PasswordTextBox.Visibility = Visibility.Hidden;
            MainPasswordBox.Visibility = Visibility.Visible;

            MainPasswordBox.Focus();
        }

        private void LoginTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "")
            {
                LoginTextBox.Text = defaultLogin;

                LoginTextBox.Foreground = Brushes.Gray;
            }
        }

        private void MainPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MainPasswordBox.Password == "")
            {
                PasswordTextBox.Visibility = Visibility.Visible;
                MainPasswordBox.Visibility = Visibility.Hidden;
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

            else if (e.Key == Key.Tab && !LoginTextBox.IsFocused)
            {
                LoginTextBox.Focus();

                e.Handled = true;
            }
        }



        private void Authorization()
        {
            string login = LoginTextBox.Text.TrimEnd(new char[] { ' ' });
            string password = MainPasswordBox.Password;

            var user = _storage.Users.Items.FirstOrDefault(u => u.Login == login && password == u.Password);

            if (user == null)
            {
                MessageBox.Show("Введенной пары логина и пароля не найдено в базе. Попробуйте еще раз либо обратитесь к разработчикам!", "Ошибка");

                LoginTextBox.Text = "";
                LoginTextBox.Foreground = Brushes.Black;

                PasswordTextBox.Visibility = Visibility.Visible;
                MainPasswordBox.Visibility = Visibility.Hidden;

                MainPasswordBox.Password = "";

                LoginTextBox.Focus();
            }

            else
            {
                ProfileWindow profileWindow = new ProfileWindow(user);

                profileWindow.Show();

                Close();
            }
        }
    }
}
