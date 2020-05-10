using MarvelGuide.Core;
using MarvelGuide.Core.Intefraces;
using MarvelGuide.Core.Models;
using MarvelGuide.Core.Helpers;
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
using MarvelGuide.GUI.Helpers;

namespace MarvelGuide.GUI
{
    /// <summary>
    /// Логика взаимодействия для TheTeamWindow.xaml
    /// </summary>
    public partial class TheTeamWindow : Window
    {
        private const string director = "Директор";
        private const string manager = "Менеджер";

        private const string defaultImageSource = "default.jpg";
        private const string noneImageSource = "none.jpg";

        private const string imageFolder = "../MarvelGuide.Core/Avatars";

        private const string noneText = "none";

        private const string defaultButtonContent = "Открыть профиль";
        private const string yourProfileButtonContent = "В свой профиль";
        private const string editProfileButtonContent = "Редактировать";
        private const string addProfileButtonContent = "Добавить нового сотрудника";

        private const string normalTitle = "Наша команда:";
        private const string developerTitle = "Отредактируйте данные о сотрудниках";


        private const int amountForTheCollapsibleListOfPeople = 5;


        IStorage _storage;

        User _user;

        bool _lookingInTheDeveloperMode = false;

        List<User> _theTeam;
        List<User> _unsortedTeam;
        List<User> _leftPeople;
        List<User> _unsortedLeftPeople;

        bool _goingBackToProfile = true;
        bool _goingToEditPage = false;

        bool _fullVersionShouldBeShown = false;

        User _visitedUser = null;
        User _focusingUser = null;


        public TheTeamWindow(User user, User focusingUser, bool lookingInTheDeveloperMode, bool fullVersionShouldBeShown)
        {
            _storage = Factory.Instance.GetStorage();

            _user = user;
            _focusingUser = focusingUser;

            _lookingInTheDeveloperMode = lookingInTheDeveloperMode;
            _fullVersionShouldBeShown = fullVersionShouldBeShown;

            _theTeam = new List<User>();
            _leftPeople = new List<User>();
            _unsortedTeam = _storage.Users.Items
                .Where(u => u.WorkingNow)
                .OrderBy(u => u.GotAJob)
                .ThenBy(u => u.Name).ToList();
            _unsortedLeftPeople = _storage.Users.Items
                .Where(u => !u.WorkingNow)
                .OrderByDescending(u => u.LostTheJob)
                .ThenBy(u => u.Name).ToList();

            InitializeComponent();

            FormingTheTeam();

            ShowingEmployers();

            FocusUser();

            Window_StateChanged();
        }

        public TheTeamWindow(User user, bool fullVersionShouldBeShown) : this (user, null, false, fullVersionShouldBeShown) { }
        public TheTeamWindow(User user, User focusingUser, bool fullVersionShouldBeShown) : this (user, focusingUser, false, fullVersionShouldBeShown) { }
        public TheTeamWindow(User user, bool lookingInTheDeveloperMode, bool fullVersionShouldBeShown) : this (user, null, lookingInTheDeveloperMode, fullVersionShouldBeShown) { }



        private void ShowingEmployers()
        {
            TheTeamListBox.ItemsSource = _theTeam;

            if (_fullVersionShouldBeShown)
            {
                LeftPeopleListBox.ItemsSource = _leftPeople;
                ShowDetailsButton.Visibility = Visibility.Hidden;
            }
            else
            {
                LeftPeopleListBox.ItemsSource = _leftPeople.Take(amountForTheCollapsibleListOfPeople);
            }
        }



        private void SwitchingTheMembers(Func<User, bool> condition)
        {
            var usersForRemovingFromUnsortedTeam = new List<User>();
            var usersForRemovingFromUnsortedLeftPeople = new List<User>();

            foreach (var user in _unsortedTeam)
            {
                if (condition(user))
                {
                    usersForRemovingFromUnsortedTeam.Add(user);
                    _theTeam.Add(user);
                }
            }

            foreach (var user in usersForRemovingFromUnsortedTeam)
            {
                _unsortedTeam.Remove(user);
            }

            foreach (var user in _unsortedLeftPeople)
            {
                if (condition(user))
                {
                    usersForRemovingFromUnsortedLeftPeople.Add(user);
                    _leftPeople.Add(user);
                }
            }

            foreach (var user in usersForRemovingFromUnsortedLeftPeople)
            {
                _unsortedLeftPeople.Remove(user);
            }
        }

        private void FormingTheTeam()
        {
            if (_lookingInTheDeveloperMode)
            {
                _theTeam.Add(new User { Id = -1 });
            }

            SwitchingTheMembers(u => u.GeneralDirector);
            SwitchingTheMembers(u => u.Director);
            SwitchingTheMembers(u => u.DeputyGeneralDirector);
            SwitchingTheMembers(u => u.HeadOfManagers);
            SwitchingTheMembers(u => u.HeadOfExperts);
            SwitchingTheMembers(u => u.HeadOfMarketers);
            SwitchingTheMembers(u => u.HeadOfEditors);
            SwitchingTheMembers(u => u.HeadOfSpecials);
            SwitchingTheMembers(u => u.HeadOfAgents);
            SwitchingTheMembers(u => u.HeadOfModerators);
            SwitchingTheMembers(u => u.HeadOfTechnicians);
            SwitchingTheMembers(u => u.Manager);
            SwitchingTheMembers(u => u.Expert);
            SwitchingTheMembers(u => u.Marketer);
            SwitchingTheMembers(u => u.Editor);
            SwitchingTheMembers(u => u.Special);
            SwitchingTheMembers(u => u.Agent);
            SwitchingTheMembers(u => u.Moderator);
            SwitchingTheMembers(u => u.Technician);
        }



        private void FocusUser()
        {
            if (FocusAppropriateUser(TheTeamListBox) == false)
            {
                FocusAppropriateUser(LeftPeopleListBox);
            }
        }

        private bool FocusAppropriateUser(ListBox UserListBox)
        {
            if (_focusingUser != null)
            {
                UserListBox.UpdateLayout();

                for (int i = 0; i < UserListBox.Items.Count; i++)
                {
                    if (UserListBox.Items[i] as User == _focusingUser)
                    {
                        if ( i + 2 < UserListBox.Items.Count)
                        {
                            UserListBox.ScrollIntoView(UserListBox.Items[i + 2]);
                        }
                        else
                        {
                            UserListBox.ScrollIntoView(UserListBox.Items[UserListBox.Items.Count - 1]);
                        }
                         
                        return true;
                    }
                }
            }

            return false;
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
            else if (_goingToEditPage)
            {
                ProfileWindow profileWindow = new ProfileWindow(_visitedUser, _user, true, _fullVersionShouldBeShown);

                profileWindow.Show();
            }
            else
            {
                ProfileWindow profileWindow = new ProfileWindow(_visitedUser, _user, _fullVersionShouldBeShown);

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

                if (job.IndexOf(director) != -1)
                {
                    job = job.Substring(0, job.IndexOf(director)) + user.DirectorsPosition + job.Substring(job.IndexOf(director) + director.Length);
                }
                if (job.IndexOf(manager) != -1)
                {
                    job = job.Substring(0, job.IndexOf(manager)) + user.ManagersPosition + job.Substring(job.IndexOf(manager) + manager.Length);
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
                    AvatarImage.Source = UIElementsMethods.InitializingBitmapImage(user.Avatar.ImageSource, imageFolder);
                }
                catch
                {
                    try
                    {
                        AvatarImage.Source = UIElementsMethods.InitializingBitmapImage(defaultImageSource, imageFolder);
                    }
                    catch { }
                }
            }
            else
            {
                try
                {
                    AvatarImage.Source = UIElementsMethods.InitializingBitmapImage(noneImageSource, imageFolder);
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
            Button ReadButton = sender as Button;

            User visitedUser = ReadButton.DataContext as User;

            if (!_lookingInTheDeveloperMode)
            {
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
                _visitedUser = visitedUser;

                _goingBackToProfile = false;
                _goingToEditPage = true;

                Close();                
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



        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (_fullVersionShouldBeShown)
            {
                if (WindowState == WindowState.Maximized)
                {
                    LeftPeopleListBox.Margin = new Thickness(75, 10, 75, -60);
                }
                else if (WindowState == WindowState.Normal)
                {
                    LeftPeopleListBox.Margin = new Thickness(75, 10, 75, -77);
                }
            }
            else
            {
                if (WindowState == WindowState.Maximized)
                {
                    LeftPeopleListBox.Margin = new Thickness(75, 10, 75, 0);
                    ShowDetailsButton.Margin = new Thickness(80, 0, 125, 90);
                }
                else if (WindowState == WindowState.Normal)
                {
                    LeftPeopleListBox.Margin = new Thickness(75, 10, 75, 0);
                    ShowDetailsButton.Margin = new Thickness(80, 0, 125, 58);
                }
            }
        }

        private void Window_StateChanged() { Window_StateChanged(null, null); }



        private void ShowDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            _fullVersionShouldBeShown = true;
            ShowingEmployers();
            Window_StateChanged();
        }

        

        private void ListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Scroll.ScrollToVerticalOffset(Scroll.VerticalOffset - (double)e.Delta * 5 / 12);
        }
    }
}
