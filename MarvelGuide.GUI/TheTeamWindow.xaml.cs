using MarvelGuide.Core;
using MarvelGuide.Core.Intefraces;
using MarvelGuide.Core.Models;
using MarvelGuide.Core.SpecialMethods;
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
    /// Логика взаимодействия для TheTeamWindow.xaml
    /// </summary>
    public partial class TheTeamWindow : Window
    {
        private const string manager = "Менеджер";

        private const string defaultImageSource = "default.jpg";
        private const string noneImageSource = "none.jpg";

        private const string noneText = "none";

        private const string defaultButtonContent = "Открыть профиль";
        private const string yourProfileButtonContent = "В свой личный кабинет";
        private const string editProfileButtonContent = "Редактировать";
        private const string addProfileButtonContent = "Добавить нового сотрудника";

        private const string normalTitle = "Наша команда:";
        private const string developerTitle = "Отредактируйте данные о сотрудниках";


        IStorage _storage;

        User _user;

        bool _lookingInTheDeveloperMode = false;

        List<User> _theTeam;
        List<User> _unsortedTeam;

        bool _goingBackToProfile = true;

        User _visitedUser = null;


        public TheTeamWindow(User user, bool lookingInTheDeveloperMode)
        {
            _storage = Factory.Instance.GetStorage();

            _user = user;
            _lookingInTheDeveloperMode = lookingInTheDeveloperMode;

            _theTeam = new List<User>();
            _unsortedTeam = _storage.Users.Items.ToList();
            
            InitializeComponent();

            FormingTheTeam();

            TheTeamListBox.ItemsSource = _theTeam;
        }

        public TheTeamWindow(User user) : this (user, false) { }



        private void SwitchingTheMembers(Func<User, bool> condition)
        {
            var usersForRemoving = new List<User>();

            foreach (var user in _unsortedTeam)
            {
                if (condition(user))
                {
                    usersForRemoving.Add(user);
                    _theTeam.Add(user);
                }
            }

            foreach (var user in usersForRemoving)
            {
                _unsortedTeam.Remove(user);
            }
        }

        private void FormingTheTeam()
        {
            if (_lookingInTheDeveloperMode)
            {
                _theTeam.Add(new User { Id = -1 });
            }

            SwitchingTheMembers(u => u.Creator);
            SwitchingTheMembers(u => u.SuperAdmin);
            SwitchingTheMembers(u => u.AdminEditor);
            SwitchingTheMembers(u => u.AdminAgent);
            SwitchingTheMembers(u => u.Manager);
            SwitchingTheMembers(u => u.Editor);
            SwitchingTheMembers(u => u.Agent);
            SwitchingTheMembers(u => u.Moderator);
        }
        


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_goingBackToProfile)
            {
                ProfileWindow profileWindow = new ProfileWindow(_user);

                profileWindow.Show();
            }
            else
            {
                ProfileWindow profileWindow = new ProfileWindow(_visitedUser, _user);

                profileWindow.Show();
            }
        }



        private void UserNameTextBlock_Initialized(object sender, EventArgs e)
        {
            TextBlock UserNameTextBlock = sender as TextBlock;

            User user = UserNameTextBlock.DataContext as User;

            if (user.Id != -1)
            {
                UserNameTextBlock.Text = user.Name + " " + user.Surname;
            }
            else
            {
                UserNameTextBlock.Text = noneText;
            }
        }

        private void UserJobTextBlock_Initialized(object sender, EventArgs e)
        {
            TextBlock UserJobTextBlock = sender as TextBlock;

            User user = UserJobTextBlock.DataContext as User;

            if (user.Id != -1)
            {
                string job = user.Job();

                if (job.IndexOf(manager) != -1)
                {
                    job = job.Substring(0, job.IndexOf(manager)) + user.ManagersRole + job.Substring(job.IndexOf(manager) + manager.Length);
                }

                UserJobTextBlock.Text = job;
            }
            else
            {
                UserJobTextBlock.Text = noneText;
            }
        }

        private void AvatarImage_Initialized(object sender, EventArgs e)
        {
            Image AvatarImage = sender as Image;

            User user = AvatarImage.DataContext as User;

            if (user.Id != -1)
            {
                try
                {
                    AvatarImage.Source = new BitmapImage(new Uri(WorkWithImages.GetDestinationPath(user.Avatar.ImageSource, "../MarvelGuide.Core/Avatars")));
                }
                catch
                {
                    try
                    {
                        AvatarImage.Source = new BitmapImage(new Uri(WorkWithImages.GetDestinationPath(defaultImageSource, "../MarvelGuide.Core/Avatars")));
                    }
                    catch { }
                }
            }
            else
            {
                try
                {
                    AvatarImage.Source = new BitmapImage(new Uri(WorkWithImages.GetDestinationPath(noneImageSource, "../MarvelGuide.Core/Avatars")));
                }
                catch { }
            }
        }



        private void OpenButton_Initialized(object sender, EventArgs e)
        {
            Button ReadButton = sender as Button;

            User user = ReadButton.DataContext as User;

            if (!_lookingInTheDeveloperMode)
            {
                if (user == _user)
                {
                    ReadButton.Content = yourProfileButtonContent;
                }
                else
                {
                    ReadButton.Content = defaultButtonContent;
                }
            }
            else
            {
                if (user.Id != -1)
                {
                    ReadButton.Content = editProfileButtonContent;
                }
                else
                {
                    ReadButton.Content = addProfileButtonContent;
                }
            }
        }


        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_lookingInTheDeveloperMode)
            {
                Button ReadButton = sender as Button;

                User visitedUser = ReadButton.DataContext as User;

                if (visitedUser == _user)
                {
                    Close();
                }
                else
                {
                    _visitedUser = visitedUser;

                    _goingBackToProfile = false;

                    Close();
                }
            }
            else
            {
                MessageBox.Show("Извините, этого функционала еще нет.", "Ошибка!");
            }
        }



        private void TitleTextBlock_Initialized(object sender, EventArgs e)
        {
            if (_lookingInTheDeveloperMode)
            {
                TitleTextBlock.Text = developerTitle;
                TitleTextBlock.Margin = new Thickness(90, 75, 0, 10);
            }
            else
            {
                TitleTextBlock.Text = normalTitle;
            }
        }
    }
}
