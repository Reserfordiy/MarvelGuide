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
    /// Логика взаимодействия для UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        IStorage _storage;


        User _userWhoWatches;



        public UserDetailsWindow(User userWhoWatches)
        {
            _storage = Factory.Instance.GetStorage();

            _userWhoWatches = userWhoWatches;

            InitializeComponent();

            EditorsDocumentsListBox.ItemsSource = new string[] { "1", "2", "3" };
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ProfileWindow profileWindow = new ProfileWindow(_userWhoWatches);

            profileWindow.Show();
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void EditorsDocumentTextBlock_Initialized(object sender, EventArgs e)
        {

        }



        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {

        }



        private void ListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Scroll.ScrollToVerticalOffset(Scroll.VerticalOffset - (double)e.Delta * 5 / 12);
        }
    }
}
