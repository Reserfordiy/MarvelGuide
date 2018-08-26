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
    /// Логика взаимодействия для ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        private const string adding = ":  ";

        private const string login = "Логин в системе";
        private const string name = "Ф.И. сотрудника";
        private const string job = "Должность";

        private const string securityManagerRole = "Менеджер по безопасности";

        private const string creatorsEmployeesFirst = "Работают под подчинением";        
        private const string creatorsEmployeesSecond = "Среди них:";

        private const string allEmployees = "Работают под руководством";

        private const string employer = "Непосредственный начальник";
        private const string employerForManager = " (менеджерство)";
        private const string employerForAgent = " (Поддержка)";
        private const string employerForEditor = " (редакторство)";
        private const string employerForModerator = " (модераторство)";

        private const string creatorsEmployeeManagers = "Менеджеров";
        private const string creatorsEmployeeEditors = "Редакторов";
        private const string creatorsEmployeeAgents = "Агентов Поддержки";
        private const string creatorsEmployeeModerators = "Модераторов";

        private const string ending1 = "";
        private const string ending234 = "а";
        private const string ending5 = "ов";

        private const string employees = " сотрудник";
        private const string employeeManagers = " менеджер";
        private const string employeeEditors = " редактор";
        private const string employeeAgents = " агент";
        private const string employeeModerators = " модератор";

        private const string managerJob = "Менеджерская должность";

        private const string editorsRubric = "Редакторская рубрика";
        private const string editorsFrequencyFractionStart = "Частота размещения постов:  1/";
        private const string editorsFrequencyIntegerStart = "Частота размещения постов:  ";
        private const string editorsFrequencyFractionEnd = " поста в сутки";
        private const string editorsFrequencyInteger1End = " пост в сутки";
        private const string editorsFrequencyInteger234End = " поста в сутки";
        private const string editorsFrequencyInteger5End = " постов в сутки";

        private const string agentsNumber = "Агентский номер";
        private const string agentsFirstWords = "Приветствие агента";
        private const string agentsLastWords = "Подпись агента";


        private const string showDetailsButton = "Показать подробности";
        private const string hideDetailsButton = "Скрыть подробности";

        private const string exitOwnProfile = "Выйти";
        private const string exitForeignProfile = "Назад";

        private const string personalPageTitle = "Личный кабинет";
        private const string watchingForeignPageTitle = "Профиль ";
        private const string addingNewUserTitle = "Добавление нового сотрудника";
        private const string editingUserTitle = "Изменение профиля ";


        private const string defaultName = "Пример: Иван";
        private const string defaultSurname = "Пример: Иванов";
        private const string defaultLogin = "Пример: ivani";
        private const string defaultPassword = "Пример: 123456";
        private const string defaultManagerRole = "Пример: Менеджер по кадрам";
        private const string defaultEditorsRubric = "Пример: Старс";
        private const string defaultEditorsFrequency = "Пример: 3";
        private const string defaultAgentsNumber = "Пример: 14";
        private const string defaultAgentsFirstWords = "Пример: Здравствуйте!";
        private const string defaultAgentsLastWords = "Пример: С любовью";

        private const string defaultImageSource = "default.jpg";


        IStorage _storage;

        User _user;
        User _userWhoWatches;

        List<string> _personalData;

        int _amountOfRegularJobs;

        int _additionalData = 0;
        bool _detailsShown = false;
        bool _goingToTheTeamWindow = false;
        bool _goingToTheDeveloperMode = false;

        bool _personalPage = true;
        bool _editingPage = false;



        public ProfileWindow(User user)
        {
            _storage = Factory.Instance.GetStorage();

            _user = user;

            InitializeComponent();

            CheckingWhetherWeEditPage();
        }

        public ProfileWindow(User user, User userWhoWatches, bool editingPage)
        {
            _personalPage = false;

            _user = user;
            _userWhoWatches = userWhoWatches;
            _editingPage = editingPage;

            _storage = Factory.Instance.GetStorage();

            InitializeComponent();

            CheckingWhetherWeEditPage();
        }

        public ProfileWindow(User user, User userWhoWatches) : this (user, userWhoWatches, false) { }



        private void FormingPersonalData()
        { 
            _personalData = new List<string>
            {
                login + adding + _user.Login,
                name + adding + _user.Surname + " " + _user.Name,
                job + adding + _user.Job()
            };

            if (_user.Manager) { _amountOfRegularJobs++; }
            if (_user.Editor) { _amountOfRegularJobs++; }
            if (_user.Agent) { _amountOfRegularJobs++; }
            if (_user.Moderator) { _amountOfRegularJobs++; }

            PersonalDataListBox.ItemsSource = _personalData;
        }

        private void FormingTheEdittingData()
        {
            if (_user.Id != -1)
            {

                NameTextBox.Text = _user.Name;
                NameTextBox.Foreground = Brushes.Black;

                SurnameTextBox.Text = _user.Surname;
                SurnameTextBox.Foreground = Brushes.Black;

                if (_user.Male) { MaleRadioButton.IsChecked = true; }
                else { FemaleRadioButton.IsChecked = true; }

                LoginTextBox.Text = _user.Login;
                LoginTextBox.Foreground = Brushes.Black;

                PasswordTextBox.Text = _user.Password;
                PasswordTextBox.Foreground = Brushes.Black;

                RepeatPasswordTextBox.Text = _user.Password;
                RepeatPasswordTextBox.Foreground = Brushes.Black;

                if (_user.Creator) { CreatorCheckBox.IsChecked = true; }
                if (_user.SuperAdmin) { SuperAdminCheckBox.IsChecked = true; }
                if (_user.AdminEditor) { AdminEditorCheckBox.IsChecked = true; }
                if (_user.AdminAgent) { AdminAgentCheckBox.IsChecked = true; }
                if (_user.Manager)
                {
                    ManagerCheckBox.IsChecked = true;

                    ManagersRoleTextBox.Text = _user.ManagersRole;
                    ManagersRoleTextBox.Foreground = Brushes.Black;
                }
                if (_user.Editor)
                {
                    EditorCheckBox.IsChecked = true;

                    EditorsRubricTextBox.Text = _user.EditorsRubric;
                    EditorsRubricTextBox.Foreground = Brushes.Black;

                    EditorsFrequencyTextBox.Text = _user.EditorsFrequency.ToString();
                    EditorsFrequencyTextBox.Foreground = Brushes.Black;
                }
                if (_user.Agent)
                {
                    AgentChecBox.IsChecked = true;

                    AgentsNumberTextBox.Text = _user.AgentsNumber.ToString();
                    AgentsNumberTextBox.Foreground = Brushes.Black;

                    AgentsFirstWordsTextBox.Text = _user.AgentsFirstWords;
                    AgentsFirstWordsTextBox.Foreground = Brushes.Black;

                    AgentsLastWordsTextBox.Text = _user.AgentsLastWords;
                    AgentsLastWordsTextBox.Foreground = Brushes.Black;
                }
                if (_user.Moderator) { ModeratorcheckBox.IsChecked = true; }
            }
        }

        private void CheckingWhetherWeEditPage()
        {
            if (!_editingPage)
            {
                MaxHeight = 700;

                DeveloperModeGrid.Visibility = Visibility.Hidden;
                DeveloperModeGrid.Height = 0;

                FormingPersonalData();
            }
            else
            {
                WindowState = WindowState.Maximized;

                NormalModeGrid.Visibility = Visibility.Hidden;
                NormalModeGrid.Height = 0;

                FormingTheEdittingData();
            }
        }



        private void CharacteristicTextBlock_Initialized(object sender, EventArgs e)
        {
            TextBlock CharacteristicTextBlock = sender as TextBlock;

            string characteristic = CharacteristicTextBlock.DataContext as string;

            CharacteristicTextBlock.Text = characteristic;
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_goingToTheTeamWindow && !_goingToTheDeveloperMode)
            {
                if (_personalPage)
                {
                    MainWindow mainWindow = new MainWindow();

                    mainWindow.Show();
                }
                else
                {
                    ProfileWindow profileWindow = new ProfileWindow(_userWhoWatches);

                    profileWindow.Show();
                }
            }

            else if (_goingToTheDeveloperMode && _personalPage)
            {
                TheTeamWindow theTeamWindow = new TheTeamWindow(_user, true);

                theTeamWindow.Show();
            }

            else if (_goingToTheDeveloperMode)
            {
                TheTeamWindow theTeamWindow = new TheTeamWindow(_userWhoWatches, true);

                theTeamWindow.Show();
            }

            else
            {
                if (_personalPage)
                {
                    TheTeamWindow theTeamWindow = new TheTeamWindow(_user);

                    theTeamWindow.Show();
                }
                else
                {
                    TheTeamWindow theTeamWindow = new TheTeamWindow(_userWhoWatches);

                    theTeamWindow.Show();
                }                
            }
        }



        private void AvatarImage_Initialized(object sender, EventArgs e)
        {
            try
            {
                AvatarImage.Source = new BitmapImage(new Uri(WorkWithImages.GetDestinationPath(_user.Avatar.ImageSource, "../MarvelGuide.Core/Avatars")));
            }
            catch
            {
                AvatarImage.Source = new BitmapImage(new Uri(WorkWithImages.GetDestinationPath(defaultImageSource, "../MarvelGuide.Core/Avatars")));
            }
        }



        private void ShowDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_detailsShown)
            {
                _detailsShown = true;

                if (_user.Creator) { CreatorsDetails(); }
                if (_user.SuperAdmin) { SuperAdminsDetails(); }
                if (_user.AdminEditor) { AdminEditorDetails(); }
                if (_user.AdminAgent) { AdminAgentDetails(); }
                if (_user.Manager) { ManagersDetails(); }
                if (_user.Editor) { EditorsDetails(); }
                if (_user.Agent) { AgentsDetails(); }
                if (_user.Moderator) { ModeratorsDetails(); }

                ShowDetailsButton.Content = hideDetailsButton;
            }

            else
            {
                _detailsShown = false;

                _personalData = _personalData.Take(_personalData.Count - _additionalData).ToList();

                ShowDetailsButton.Content = showDetailsButton;

                _additionalData = 0;
            }

            PersonalDataListBox.ItemsSource = null;
            PersonalDataListBox.ItemsSource = _personalData;
        }

        
        private void CreatorsDetails()
        {
            _personalData.Add(creatorsEmployeesFirst + adding + _storage.Users.Items.Count().ToString() + employees + HelpingMethods.ChoosingTheCorrespondingEnding(ending1, ending234, ending5, _storage.Users.Items.Count()));
            _personalData.Add(creatorsEmployeesSecond);
            _personalData.Add(creatorsEmployeeManagers + adding + _storage.Users.Items.Count(u => u.Manager).ToString());
            _personalData.Add(creatorsEmployeeEditors + adding + _storage.Users.Items.Count(u => u.Editor).ToString());
            _personalData.Add(creatorsEmployeeAgents + adding + _storage.Users.Items.Count(u => u.Agent).ToString());
            _personalData.Add(creatorsEmployeeModerators + adding + _storage.Users.Items.Count(u => u.Moderator).ToString());

            _additionalData += 6;
        }

        private void SuperAdminsDetails()
        {
            _personalData.Add(allEmployees + adding + _storage.Users.Items.Count(u => u.Manager).ToString() + employeeManagers + HelpingMethods.ChoosingTheCorrespondingEnding(ending1, ending234, ending5, _storage.Users.Items.Count(u => u.Manager)));

            _additionalData++;
        }

        private void AdminEditorDetails()
        {
            _personalData.Add(allEmployees + adding + _storage.Users.Items.Count(u => u.Editor).ToString() + employeeEditors + HelpingMethods.ChoosingTheCorrespondingEnding(ending1, ending234, ending5, _storage.Users.Items.Count(u => u.Editor)));

            _additionalData++;
        }

        private void AdminAgentDetails()
        {
            _personalData.Add(allEmployees + adding + _storage.Users.Items.Count(u => u.Agent).ToString() + employeeAgents + HelpingMethods.ChoosingTheCorrespondingEnding(ending1, ending234, ending5, _storage.Users.Items.Count(u => u.Agent)));

            _additionalData++;
        }

        private void ManagersDetails()
        {
            _personalData.Add(managerJob + adding + _user.ManagersRole);

            _additionalData++;

            if (!_user.Creator && !_user.SuperAdmin && !_user.AdminAgent && !_user.AdminEditor)
            {
                if (_amountOfRegularJobs == 1)
                {
                    _personalData.Add(employer + adding + _storage.Users.Items.FirstOrDefault(u => u.SuperAdmin).Name + " " + _storage.Users.Items.FirstOrDefault(u => u.SuperAdmin).Surname);
                }
                else
                {
                    _personalData.Add(employer + employerForManager + adding + _storage.Users.Items.FirstOrDefault(u => u.SuperAdmin).Name + " " + _storage.Users.Items.FirstOrDefault(u => u.SuperAdmin).Surname);
                }

                _additionalData++;
            }

            if (_user.ManagersRole.IndexOf(securityManagerRole) != -1)
            {
                _personalData.Add(allEmployees + adding + _storage.Users.Items.Count(u => u.Moderator).ToString() + employeeModerators + HelpingMethods.ChoosingTheCorrespondingEnding(ending1, ending234, ending5, _storage.Users.Items.Count(u => u.Moderator)));

                _additionalData++;
            }
        }

        private void EditorsDetails()
        {
            _personalData.Add(editorsRubric + adding + _user.EditorsRubric);

            if (_user.EditorsFrequency > 1)
            {
                _personalData.Add(editorsFrequencyFractionStart + _user.EditorsFrequency.ToString() + editorsFrequencyFractionEnd);
            }
            else
            {
                _personalData.Add(editorsFrequencyIntegerStart + _user.EditorsFrequency.ToString() + editorsFrequencyInteger1End);
            }

            _additionalData += 2;

            if (!_user.Creator && !_user.SuperAdmin && !_user.AdminAgent && !_user.AdminEditor)
            {
                if (_amountOfRegularJobs == 1)
                {
                    _personalData.Add(employer + adding + _storage.Users.Items.FirstOrDefault(u => u.AdminEditor).Name + " " + _storage.Users.Items.FirstOrDefault(u => u.AdminEditor).Surname);
                }
                else
                {
                    _personalData.Add(employer + employerForEditor + adding + _storage.Users.Items.FirstOrDefault(u => u.AdminEditor).Name + " " + _storage.Users.Items.FirstOrDefault(u => u.AdminEditor).Surname);
                }

                _additionalData++;
            }
        }

        private void AgentsDetails()
        {
            _personalData.Add(agentsNumber + adding + _user.AgentsNumber.ToString());
            _personalData.Add(agentsFirstWords + adding + _user.AgentsFirstWords);
            _personalData.Add(agentsLastWords + adding + _user.AgentsLastWords);

            _additionalData += 3;

            if (!_user.Creator && !_user.SuperAdmin && !_user.AdminAgent && !_user.AdminEditor)
            {
                if (_amountOfRegularJobs == 1)
                {
                    _personalData.Add(employer + adding + _storage.Users.Items.FirstOrDefault(u => u.AdminAgent).Name + " " + _storage.Users.Items.FirstOrDefault(u => u.AdminAgent).Surname);
                }
                else
                {
                    _personalData.Add(employer + employerForAgent + adding + _storage.Users.Items.FirstOrDefault(u => u.AdminAgent).Name + " " + _storage.Users.Items.FirstOrDefault(u => u.AdminAgent).Surname);
                }

                _additionalData++;
            }
        }

        private void ModeratorsDetails()
        {
            User securityManager = _storage.Users.Items.FirstOrDefault(u => u.Manager && u.ManagersRole.IndexOf(securityManagerRole) != -1);

            if (securityManager != null && !_user.Creator && !_user.SuperAdmin && !_user.AdminAgent && !_user.AdminEditor)
            {
                if (_amountOfRegularJobs == 1)
                {
                    _personalData.Add(employer + adding + securityManager.Name + " " + securityManager.Surname);
                }
                else
                {
                    _personalData.Add(employer + employerForModerator + adding + securityManager.Name + " " + securityManager.Surname);
                }

                _additionalData++;
            }
        }



        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }



        private void ProfileTitleTextBlock_Initialized(object sender, EventArgs e)
        {
            if (_editingPage)
            {
                if (_user.Id == -1)
                {
                    ProfileTitleTextBlock.Text = addingNewUserTitle;
                }
                else
                {
                    ProfileTitleTextBlock.Text = editingUserTitle + _user.GenitiveName();
                }
            }
            else if (!_personalPage)
            {
                ProfileTitleTextBlock.Text = watchingForeignPageTitle + _user.GenitiveName();
            }
            else
            {
                ProfileTitleTextBlock.Text = personalPageTitle;
            }            
        }

        private void ExitProfileButton_Initialized(object sender, EventArgs e)
        {
            if (_personalPage)
            {
                ExitProfileButton.Content = exitOwnProfile;
            }
            else
            {
                ExitProfileButton.Content = exitForeignProfile;
            }
        }


        private void ExitProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (_editingPage)
            {
                _goingToTheDeveloperMode = true;
            }
            else if (!_personalPage)
            {
                _goingToTheTeamWindow = true;
            }

            Close();
        }

        

        private void UploadAvatarButton_Initialized(object sender, EventArgs e)
        {
            if (!_editingPage)
            {
                UploadAvatarButton = UIElementsMethods.HidingUIElement(UploadAvatarButton) as Button;
            }
        }

        private void ShowTheTeamButton_Initialized(object sender, EventArgs e)
        {
            if (!_personalPage)
            {
                ShowTheTeamButton = UIElementsMethods.HidingUIElement(ShowTheTeamButton) as Button;
            }
        }

        private void DeveloperModeButton_Initialized(object sender, EventArgs e)
        {
            if (!_user.SuperDeveloper || !_personalPage)
            {
                DeveloperModeButton = UIElementsMethods.HidingUIElement(DeveloperModeButton) as Button;
            }
        }
        
        private void SaveDataButton_Initialized(object sender, EventArgs e)
        {
            if (!_editingPage)
            {
                SaveDataButton = UIElementsMethods.HidingUIElement(SaveDataButton) as Button;
            }
        }




        private void UploadAvatarButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Извините, этого функционала еще нет!", "Ошибка!");
        }

        private void ShowTheTeamButton_Click(object sender, RoutedEventArgs e)
        {
            _goingToTheTeamWindow = true;

            Close();
        }

        private void DeveloperModeButton_Click(object sender, RoutedEventArgs e)
        {
            _goingToTheDeveloperMode = true;

            Close();
        }

        private void SaveDataButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Извините, сотрудников пока нельзя сохранить!", "Ошибка!");
        }



        private void Window_StateChanged(object sender, EventArgs e)
        {
            if ( _editingPage && WindowState == WindowState.Maximized)
            {
                SaveDataButton.Margin = new Thickness(0, 35, 0, 120);
            }
            else if (_editingPage && WindowState == WindowState.Normal)
            {
                SaveDataButton.Margin = new Thickness(0, 35, 0, 85);
            }
        }



        private void ManagersRoleGrid_Initialized(object sender, EventArgs e)
        {
            if (!_user.Manager) { ManagersRoleGrid.Visibility = Visibility.Collapsed; }
        }

        private void EditorsRoleGrid_Initialized(object sender, EventArgs e)
        {
            if (!_user.Editor) { EditorsRoleGrid.Visibility = Visibility.Collapsed; }
        }

        private void AgentsRoleGrid_Initialized(object sender, EventArgs e)
        {
            if (!_user.Agent) { AgentsRoleGrid.Visibility = Visibility.Collapsed; }
        }

        private void ManagerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ManagersRoleGrid.Visibility = Visibility.Visible;
        }

        private void ManagerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ManagersRoleGrid.Visibility = Visibility.Collapsed;
        }

        private void EditorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            EditorsRoleGrid.Visibility = Visibility.Visible;
        }

        private void EditorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EditorsRoleGrid.Visibility = Visibility.Collapsed;
        }

        private void AgentChecBox_Checked(object sender, RoutedEventArgs e)
        {
            AgentsRoleGrid.Visibility = Visibility.Visible;
        }

        private void AgentChecBox_Unchecked(object sender, RoutedEventArgs e)
        {
            AgentsRoleGrid.Visibility = Visibility.Collapsed;
        }



        private void NameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == defaultName)
            {
                NameTextBox.Text = "";
                NameTextBox.Foreground = Brushes.Black;
            }
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == "")
            {
                NameTextBox.Text = defaultName;
                NameTextBox.Foreground = Brushes.Gray;
            }
        }

        private void SurnameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SurnameTextBox.Text == defaultSurname)
            {
                SurnameTextBox.Text = "";
                SurnameTextBox.Foreground = Brushes.Black;
            }
        }

        private void SurnameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SurnameTextBox.Text == "")
            {
                SurnameTextBox.Text = defaultSurname;
                SurnameTextBox.Foreground = Brushes.Gray;
            }
        }

        private void LoginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == defaultLogin)
            {
                LoginTextBox.Text = "";
                LoginTextBox.Foreground = Brushes.Black;
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

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Text == defaultPassword)
            {
                PasswordTextBox.Text = "";
                PasswordTextBox.Foreground = Brushes.Black;
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

        private void RepeatPasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (RepeatPasswordTextBox.Text == defaultPassword)
            {
                RepeatPasswordTextBox.Text = "";
                RepeatPasswordTextBox.Foreground = Brushes.Black;
            }
        }

        private void RepeatPasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (RepeatPasswordTextBox.Text == "")
            {
                RepeatPasswordTextBox.Text = defaultPassword;
                RepeatPasswordTextBox.Foreground = Brushes.Gray;
            }
        }

        private void ManagersRoleTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ManagersRoleTextBox.Text == defaultManagerRole)
            {
                ManagersRoleTextBox.Text = "";
                ManagersRoleTextBox.Foreground = Brushes.Black;
            }
        }

        private void ManagersRoleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ManagersRoleTextBox.Text == "")
            {
                ManagersRoleTextBox.Text = defaultManagerRole;
                ManagersRoleTextBox.Foreground = Brushes.Gray;
            }
        }

        private void EditorsRubricTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EditorsRubricTextBox.Text == defaultEditorsRubric)
            {
                EditorsRubricTextBox.Text = "";
                EditorsRubricTextBox.Foreground = Brushes.Black;
            }
        }

        private void EditorsRubricTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (EditorsRubricTextBox.Text == "")
            {
                EditorsRubricTextBox.Text = defaultEditorsRubric;
                EditorsRubricTextBox.Foreground = Brushes.Gray;
            }
        }

        private void EditorsFrequencyTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EditorsFrequencyTextBox.Text == defaultEditorsFrequency)
            {
                EditorsFrequencyTextBox.Text = "";
                EditorsFrequencyTextBox.Foreground = Brushes.Black;
            }
        }

        private void EditorsFrequencyTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (EditorsFrequencyTextBox.Text == "")
            {
                EditorsFrequencyTextBox.Text = defaultEditorsFrequency;
                EditorsFrequencyTextBox.Foreground = Brushes.Gray;
            }
        }

        private void AgentsNumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (AgentsNumberTextBox.Text == defaultAgentsNumber)
            {
                AgentsNumberTextBox.Text = "";
                AgentsNumberTextBox.Foreground = Brushes.Black;
            }
        }

        private void AgentsNumberTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AgentsNumberTextBox.Text == "")
            {
                AgentsNumberTextBox.Text = defaultAgentsNumber;
                AgentsNumberTextBox.Foreground = Brushes.Gray;
            }
        }

        private void AgentsFirstWordsTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (AgentsFirstWordsTextBox.Text == defaultAgentsFirstWords)
            {
                AgentsFirstWordsTextBox.Text = "";
                AgentsFirstWordsTextBox.Foreground = Brushes.Black;
            }
        }

        private void AgentsFirstWordsTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AgentsFirstWordsTextBox.Text == "")
            {
                AgentsFirstWordsTextBox.Text = defaultAgentsFirstWords;
                AgentsFirstWordsTextBox.Foreground = Brushes.Gray;
            }
        }

        private void AgentsLastWordsTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (AgentsLastWordsTextBox.Text == defaultAgentsLastWords)
            {
                AgentsLastWordsTextBox.Text = "";
                AgentsLastWordsTextBox.Foreground = Brushes.Black;
            }
        }

        private void AgentsLastWordsTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AgentsLastWordsTextBox.Text == "")
            {
                AgentsLastWordsTextBox.Text = defaultAgentsLastWords;
                AgentsLastWordsTextBox.Foreground = Brushes.Gray;
            }
        }
    }
}
