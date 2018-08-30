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
        private const string isWorking = "Работает в настоящее время";

        private const string yes = "да";
        private const string no = "нет";

        private const string firstDateOfWork = "Дата начала работы";
        private const string lastDateOfWork = "Дата завершения работы";

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
        private const string editingYourProfileTitle = "Изменение Вашего профиля";


        private const string defaultName = "Пример: Иван";
        private const string defaultSurname = "Пример: Иванов";
        private const string defaultLogin = "Пример: ivani";
        private const string defaultPassword = "Пример: 123456";
        private const string defaultStartWorkingDate = "Пример: 18.08.2018";
        private const string defaultEndWorkingDate = "Пример: 01.09.2018";
        private const string defaultManagerRole = "Пример: Менеджер по кадрам";
        private const string defaultEditorsRubric = "Пример: Старс";
        private const string defaultEditorsFrequency = "Пример: 3";
        private const string defaultAgentsNumber = "Пример: 14";
        private const string defaultAgentsFirstWords = "Пример: Здравствуйте!";
        private const string defaultAgentsLastWords = "Пример: С любовью";

        private const string defaultImageSource = "default.jpg";


        private const double leftAmplitude = 23;
        private const double rightAmplitude = 40;


        IStorage _storage;

        User _user;
        User _userWhoWatches;

        Picture _picture;

        List<string> _personalData;

        int _amountOfRegularJobs;

        int _additionalData = 0;
        bool _detailsShown = false;
        bool _goingToTheTeamWindow = false;
        bool _goingToTheDeveloperMode = false;

        bool _personalPage = true;
        bool _editingPage = false;

        bool _programSwitch = true;



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

            _programSwitch = false;
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

            if (_user.WorkingNow) { _personalData.Add(isWorking + adding + yes); }
            else { _personalData.Add(isWorking + adding + no); }

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
                _picture = _user.Avatar;

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

                if (!_user.WorkingNow)
                {
                    StillWorkingCheckBox.IsChecked = false;

                    EndWorkingDateTextBox.Text = _user.LostTheJob.ToString("d");
                    EndWorkingDateTextBox.Foreground = Brushes.Black;
                }

                StartWorkingDateTextBox.Text = _user.GotAJob.ToString("d");
                StartWorkingDateTextBox.Foreground = Brushes.Black;

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

                if (_user.IsDeveloper())
                {
                    IsDeveloperCheckBox.IsChecked = true;

                    if (_user.HighDeveloper || _user.SuperDeveloper) { HighDeveloperRadioButton.IsChecked = true; }
                    else if (_user.MediumDeveloper) { MediumDeveloperRadioButton.IsChecked = true; }
                    else
                    {
                        LightDeveloperRadioButton.IsChecked = true;

                        if (_user.LightDeveloperCreator) { CreatorDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperSuperAdmin) { SuperAdminDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperAdminEditor) { AdminEditoDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperAdminAgent) { AdminAgentDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperManager) { ManagerDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperEditor) { EditorDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperAgent) { AgentDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperModerator) { ModeratorDeveloperCheckBox.IsChecked = true; }
                    }
                }
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

                _personalData.Add(firstDateOfWork + adding + _user.GotAJob.ToString("d"));
                _additionalData++;

                if (!_user.WorkingNow)
                {
                    _personalData.Add(lastDateOfWork + adding + _user.LostTheJob.ToString("d"));
                    _additionalData++;
                }

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
                else if (_user == _userWhoWatches)
                {
                    ProfileTitleTextBlock.Text = editingYourProfileTitle;
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
                UploadAvatarButton.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowTheTeamButton_Initialized(object sender, EventArgs e)
        {
            if (!_personalPage)
            {
                ShowTheTeamButton.Visibility = Visibility.Collapsed;
            }
        }

        private void DeveloperModeButton_Initialized(object sender, EventArgs e)
        {
            if (!(_user.SuperDeveloper || _user.HighDeveloper || _user.MediumDeveloper) || !_personalPage)
            {
                DeveloperModeButton.Visibility = Visibility.Collapsed;
            }
        }
        
        private void SaveDataButton_Initialized(object sender, EventArgs e)
        {
            if (!_editingPage)
            {
                SaveDataButton.Visibility = Visibility.Collapsed;
            }
        }

        private void DeveloperModePropertiesGrid_Initialized(object sender, EventArgs e)
        {
            if (!_editingPage)
            {
                DeveloperModePropertiesGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void DevelopersLevelGrid_Initialized(object sender, EventArgs e)
        {
            if (!_editingPage || !(_user.SuperDeveloper || _user.HighDeveloper || _user.MediumDeveloper || _user.LightDeveloperAdminAgent || 
                _user.LightDeveloperAdminEditor || _user.LightDeveloperAgent || _user.LightDeveloperCreator || _user.LightDeveloperEditor ||
                _user.LightDeveloperManager || _user.LightDeveloperModerator || _user.LightDeveloperSuperAdmin))
            {
                DevelopersLevelGrid.Visibility = Visibility.Collapsed;
            }
        }



        private void UploadAvatarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WorkWithImages imageUploadingProcess = new WorkWithImages();

                imageUploadingProcess.UploadImageAndSave();

                _picture = imageUploadingProcess.Picture;

                AvatarImage.Source = new BitmapImage(new Uri(WorkWithImages.GetDestinationPath(_picture.ImageSource, "../MarvelGuide.Core/Avatars")));
            }
            catch { }
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
            if (CheckingWhetherAllFieldsFilledCorrectly())
            {
                FixingDataAboutUser();

                _storage.Users.Save();

                _goingToTheDeveloperMode = true;

                Close();
            }
        }


        private void FixingDataAboutUser()
        {
            if (_user.Id == -1)
            {
                _storage.Users.Add(_user);

                int id = _storage.Users.Items.Max(u => u.Id);

                _user.Id = id + 1;
            }

            _user.Name = NameTextBox.Text;
            _user.Surname = SurnameTextBox.Text;
            
            if (MaleRadioButton.IsChecked == true) { _user.Male = true; }
            else { _user.Male = false; }

            _user.Login = LoginTextBox.Text;
            _user.Password = PasswordTextBox.Text;

            _user.GotAJob = DateTime.Parse(StartWorkingDateTextBox.Text);
            if (StillWorkingCheckBox.IsChecked == true)
            {
                _user.WorkingNow = true;
            }
            else
            {
                _user.WorkingNow = false;
                _user.LostTheJob = DateTime.Parse(EndWorkingDateTextBox.Text);
            }

            if (CreatorCheckBox.IsChecked == true) { _user.Creator = true; }
            else { _user.Creator = false; }
            if (SuperAdminCheckBox.IsChecked == true) { _user.SuperAdmin = true; }
            else { _user.SuperAdmin = false; }
            if (AdminEditorCheckBox.IsChecked == true) { _user.AdminEditor = true; }
            else { _user.AdminEditor = false; }
            if (AdminAgentCheckBox.IsChecked == true) { _user.AdminAgent = true; }
            else { _user.AdminAgent = false; }
            if (ManagerCheckBox.IsChecked == true)
            {
                _user.Manager = true;
                _user.ManagersRole = ManagersRoleTextBox.Text;
            }
            else
            {
                _user.Manager = false;
                _user.ManagersRole = null;
            }
            if (EditorCheckBox.IsChecked == true)
            {
                _user.Editor = true;
                _user.EditorsRubric = EditorsRubricTextBox.Text;
                _user.EditorsFrequency = int.Parse(EditorsFrequencyTextBox.Text);
            }
            else
            {
                _user.Editor = false;
                _user.EditorsRubric = null;
                _user.EditorsFrequency = 0;
            }
            if (AgentChecBox.IsChecked == true)
            {
                _user.Agent = true;
                _user.AgentsNumber = int.Parse(AgentsNumberTextBox.Text);
                _user.AgentsFirstWords = AgentsFirstWordsTextBox.Text;
                _user.AgentsLastWords = AgentsLastWordsTextBox.Text;
            }
            else
            {
                _user.Agent = false;
                _user.AgentsNumber = 0;
                _user.AgentsFirstWords = null;
                _user.AgentsLastWords = null;
            }
            if (ModeratorcheckBox.IsChecked == true) { _user.Moderator = true; }
            else { _user.Moderator = false; }

            if (IsDeveloperCheckBox.IsChecked == true)
            {
                if (HighDeveloperRadioButton.IsChecked == true) { _user.HighDeveloper = true; }
                else
                {
                    _user.SuperDeveloper = false;
                    _user.HighDeveloper = false;
                    if (MediumDeveloperRadioButton.IsChecked == true) { _user.MediumDeveloper = true; }
                    else
                    {
                        _user.MediumDeveloper = false;

                        if (CreatorDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperCreator = true; }
                        else { _user.LightDeveloperCreator = false; }
                        if (SuperAdminDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperSuperAdmin = true; }
                        else { _user.LightDeveloperSuperAdmin = false; }
                        if (AdminEditoDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperAdminEditor = true; }
                        else { _user.LightDeveloperAdminEditor = false; }
                        if (AdminAgentDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperAdminAgent = true; }
                        else { _user.LightDeveloperAdminAgent = false; }
                        if (ManagerDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperManager = true; }
                        else { _user.LightDeveloperManager = false; }
                        if (EditorDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperEditor = true; }
                        else { _user.LightDeveloperEditor = false; }
                        if (AgentDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperAgent = true; }
                        else { _user.LightDeveloperAgent = false; }
                        if (ModeratorDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperModerator = true; }
                        else { _user.LightDeveloperModerator = false; }
                    }
                }
            }

            _user.Avatar = _picture;
        }



        private void Window_StateChanged(object sender, EventArgs e)
        {
            if ( _editingPage && WindowState == WindowState.Maximized)
            {
                SaveDataButton.Margin = new Thickness(0, 35, 0, 120);

                DeveloperModePropertiesGrid.Margin = new Thickness(-1*leftAmplitude, 40, -1*rightAmplitude, 0);
                DevelopersLevelGrid.Margin = new Thickness(-1 * leftAmplitude, 0, -1 * rightAmplitude, 0);
            }
            else if (_editingPage && WindowState == WindowState.Normal)
            {
                SaveDataButton.Margin = new Thickness(0, 35, 0, 85);

                ChangingTheSizeOfDevelopersGrids();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_editingPage && WindowState != WindowState.Maximized)
            {
                ChangingTheSizeOfDevelopersGrids();
            }
        }

        private void ChangingTheSizeOfDevelopersGrids()
        {
            double amplitude = MaxWidth - MinWidth;
            double currentDifference = Width - MinWidth;
            double ratio = currentDifference / amplitude;

            DeveloperModePropertiesGrid.Margin = new Thickness(-1 * leftAmplitude * ratio, 40, -1 * rightAmplitude * ratio, 0);
            DevelopersLevelGrid.Margin = new Thickness(-1 * leftAmplitude * ratio, 0, -1 * rightAmplitude * ratio, 0);
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


        private void DeveloperCheckBoxGrid_Initialized(object sender, EventArgs e)
        {
            if (!(_user.LightDeveloperAdminAgent || _user.LightDeveloperAdminEditor || _user.LightDeveloperAgent 
                || _user.LightDeveloperCreator || _user.LightDeveloperEditor || _user.LightDeveloperManager || _user.LightDeveloperModerator || _user.LightDeveloperSuperAdmin))
            {
                DeveloperCheckBoxGrid.Visibility = Visibility.Collapsed;
            }
        }


        private void EndWorkingDateTextBox_Initialized(object sender, EventArgs e)
        {
            if (_user.Id == -1 || _user.WorkingNow) { EndWorkingDateTextBox.Visibility = Visibility.Collapsed; }
        }

        private void EndWorkingDateTextBlock_Initialized(object sender, EventArgs e)
        {
            if (_user.Id == -1 || _user.WorkingNow) { EndWorkingDateTextBlock.Visibility = Visibility.Collapsed; }
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


        private void LightDeveloperRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (_user != _userWhoWatches && !_userWhoWatches.MediumDeveloper)
            {
                DeveloperCheckBoxGrid.Visibility = Visibility.Visible;
            }
        }

        private void LightDeveloperRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_userWhoWatches.MediumDeveloper && !_programSwitch)
            {
                _programSwitch = true;

                MediumDeveloperRadioButton.IsChecked = false;
                HighDeveloperRadioButton.IsChecked = false;
                LightDeveloperRadioButton.IsChecked = true;

                MessageBox.Show("У Вас недостаточно прав для совершения данного действия. По всем вопросам можно обратиться к разработчикам приложения.", "Ошибка");

                _programSwitch = false;
            }
            else
            {
                DeveloperCheckBoxGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void MediumDeveloperRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_userWhoWatches.MediumDeveloper && !_programSwitch)
            {
                _programSwitch = true;

                LightDeveloperRadioButton.IsChecked = false;
                HighDeveloperRadioButton.IsChecked = false;
                MediumDeveloperRadioButton.IsChecked = true;

                MessageBox.Show("У Вас недостаточно прав для совершения данного действия. По всем вопросам можно обратиться к разработчикам приложения.", "Ошибка");

                _programSwitch = false;
            }
            else if (_user == _userWhoWatches && !_programSwitch)
            {
                _programSwitch = true;

                LightDeveloperRadioButton.IsChecked = false;
                HighDeveloperRadioButton.IsChecked = false;
                MediumDeveloperRadioButton.IsChecked = true;

                MessageBox.Show("Вы не можете сами изменять собственные настройки разработчика.", "Ошибка");

                _programSwitch = false;
            }
        }

        private void HighDeveloperRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_userWhoWatches.MediumDeveloper && !_programSwitch)
            {
                _programSwitch = true;

                LightDeveloperRadioButton.IsChecked = false;
                MediumDeveloperRadioButton.IsChecked = false;
                HighDeveloperRadioButton.IsChecked = true;

                MessageBox.Show("У Вас недостаточно прав для совершения данного действия. По всем вопросам можно обратиться к разработчикам приложения.", "Ошибка");

                _programSwitch = false;
            }
            else if (_user == _userWhoWatches && !_programSwitch)
            {
                _programSwitch = true;

                LightDeveloperRadioButton.IsChecked = false;
                MediumDeveloperRadioButton.IsChecked = false;
                HighDeveloperRadioButton.IsChecked = true;

                MessageBox.Show("Вы не можете сами изменять собственные настройки разработчика.", "Ошибка");

                _programSwitch = false;
            }
        }


        private void DeveloperCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!_programSwitch && _userWhoWatches.MediumDeveloper)
            {
                _programSwitch = true;

                CheckBox DeveloperCheckBox = sender as CheckBox;

                DeveloperCheckBox.IsChecked = false;

                MessageBox.Show("У Вас недостаточно прав для совершения данного действия. По всем вопросам можно обратиться к разработчикам приложения.", "Ошибка");

                _programSwitch = false;
            }
        }

        private void DeveloperCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!_programSwitch && _userWhoWatches.MediumDeveloper)
            {
                _programSwitch = true;

                CheckBox DeveloperCheckBox = sender as CheckBox;

                DeveloperCheckBox.IsChecked = true;

                MessageBox.Show("У Вас недостаточно прав для совершения данного действия. По всем вопросам можно обратиться к разработчикам приложения.", "Ошибка");

                _programSwitch = false;
            }
        }


        private void StillWorkingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (EndWorkingDateTextBlock != null && EndWorkingDateTextBox != null)
            {
                EndWorkingDateTextBlock.Visibility = Visibility.Collapsed;
                EndWorkingDateTextBox.Visibility = Visibility.Collapsed;
            }
        }

        private void StillWorkingCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EndWorkingDateTextBox.Visibility = Visibility.Visible;
            EndWorkingDateTextBlock.Visibility = Visibility.Visible;
        }

        private void IsDeveloperCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!_userWhoWatches.MediumDeveloper)
            {
                DevelopersLevelGrid.Visibility = Visibility.Visible;
            }
            else if (!_programSwitch)
            {
                _programSwitch = true;

                IsDeveloperCheckBox.IsChecked = false;

                MessageBox.Show("У Вас недостаточно прав для совершения данного действия. По всем вопросам можно обратиться к разработчикам приложения.", "Ошибка");

                _programSwitch = false;
            }
        }

        private void IsDeveloperCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_userWhoWatches.MediumDeveloper && !_programSwitch)
            {
                _programSwitch = true;

                IsDeveloperCheckBox.IsChecked = true;

                MessageBox.Show("У Вас недостаточно прав для совершения данного действия. По всем вопросам можно обратиться к разработчикам приложения.", "Ошибка");

                _programSwitch = false;
            }
            else if (_user == _userWhoWatches)
            {
                IsDeveloperCheckBox.IsChecked = true;

                MessageBox.Show("Вы не можете сами изменять собственные настройки разработчика.", "Ошибка");
            }
            else
            {
                DevelopersLevelGrid.Visibility = Visibility.Collapsed;
            }
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

        private void StartWorkingDateTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (StartWorkingDateTextBox.Text == defaultStartWorkingDate)
            {
                StartWorkingDateTextBox.Text = "";
                StartWorkingDateTextBox.Foreground = Brushes.Black;
            }
        }

        private void StartWorkingDateTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (StartWorkingDateTextBox.Text == "")
            {
                StartWorkingDateTextBox.Text = defaultStartWorkingDate;
                StartWorkingDateTextBox.Foreground = Brushes.Gray;
            }
        }

        private void EndWorkingDateTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EndWorkingDateTextBox.Text == defaultEndWorkingDate)
            {
                EndWorkingDateTextBox.Text = "";
                EndWorkingDateTextBox.Foreground = Brushes.Black;
            }
        }

        private void EndWorkingDateTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (EndWorkingDateTextBox.Text == "")
            {
                EndWorkingDateTextBox.Text = defaultEndWorkingDate;
                EndWorkingDateTextBox.Foreground = Brushes.Gray;
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



        private bool CheckingWhetherAllFieldsFilledCorrectly()
        {
            if (!CheckingIfAllValuesAreNotDefault())
            {
                return false;
            }
            if (!CheckingIfAllValuesAreValid())
            {
                return false;
            }

            return true;
        }


        private bool CheckingIfAllValuesAreNotDefault()
        {
            if (NameTextBox.Text == defaultName)
            {
                MessageBox.Show("Укажите имя сотрудника.", "Ошибка");

                NameTextBox.Focus();

                return false;
            }
            if (SurnameTextBox.Text == defaultSurname)
            {
                MessageBox.Show("Укажите фамилию сотрудника.", "Ошибка");

                SurnameTextBox.Focus();

                return false;
            }
            if (MaleRadioButton.IsChecked == false && FemaleRadioButton.IsChecked == false)
            {
                MessageBox.Show("Укажите пол сотрудника.", "Ошибка");

                MaleRadioButton.Focus();

                return false;
            }
            if (LoginTextBox.Text == defaultLogin)
            {
                MessageBox.Show("Укажите логин для сотрудника.", "Ошибка");

                LoginTextBox.Focus();

                return false;
            }
            if (PasswordTextBox.Text == defaultPassword || RepeatPasswordTextBox.Text == defaultPassword)
            {
                MessageBox.Show("Укажите пароль для сотрудника, а затем воспроизведите его.", "Ошибка");

                PasswordTextBox.Text = "";
                PasswordTextBox.Foreground = Brushes.Black;
                RepeatPasswordTextBox.Text = defaultPassword;
                RepeatPasswordTextBox.Foreground = Brushes.Gray;

                PasswordTextBox.Focus();

                return false;
            }
            if (StartWorkingDateTextBox.Text == defaultStartWorkingDate)
            {
                MessageBox.Show("Укажите дату, когда сотрудник приступил к исполнению своих обязанностей.", "Ошибка");

                StartWorkingDateTextBox.Focus();

                return false;
            }
            if (StillWorkingCheckBox.IsChecked == false && EndWorkingDateTextBox.Text == defaultEndWorkingDate)
            {
                MessageBox.Show("Укажите дату, когда сотрудник покинул свою должность.", "Ошибка");

                EndWorkingDateTextBox.Focus();

                return false;
            }
            if (CreatorCheckBox.IsChecked == false && SuperAdminCheckBox.IsChecked == false && AdminEditorCheckBox.IsChecked == false && AdminAgentCheckBox.IsChecked == false && ManagerCheckBox.IsChecked == false && EditorCheckBox.IsChecked == false && AgentChecBox.IsChecked == false && ModeratorcheckBox.IsChecked == false)
            {
                MessageBox.Show("Укажите хотя бы одну должность из списка для сотрудника.", "Ошибка");

                EditorCheckBox.Focus();

                return false;
            }
            if (ManagerCheckBox.IsChecked == true && ManagersRoleTextBox.Text == defaultManagerRole)
            {
                MessageBox.Show("Укажите расширенную менеджерскую должность сотрудника.", "Ошибка");

                ManagersRoleTextBox.Focus();

                return false;
            }
            if (EditorCheckBox.IsChecked == true && EditorsRubricTextBox.Text == defaultEditorsRubric)
            {
                MessageBox.Show("Укажите редакторскую рубрику сотрудника.", "Ошибка");

                EditorsRubricTextBox.Focus();

                return false;
            }
            if (EditorCheckBox.IsChecked == true && EditorsFrequencyTextBox.Text == defaultEditorsFrequency)
            {
                MessageBox.Show("Укажите частоту размещения постов для сотрудника.", "Ошибка");

                EditorsFrequencyTextBox.Focus();

                return false;
            }
            if (AgentChecBox.IsChecked == true && AgentsNumberTextBox.Text == defaultAgentsNumber)
            {
                MessageBox.Show("Укажите агентский номер сотрудника.", "Ошибка");

                AgentsNumberTextBox.Focus();

                return false;
            }
            if (AgentChecBox.IsChecked == true && AgentsFirstWordsTextBox.Text == defaultAgentsFirstWords)
            {
                MessageBox.Show("Укажите приветствие сотрудника (как агента Поддержки).", "Ошибка");

                AgentsFirstWordsTextBox.Focus();

                return false;
            }
            if (AgentChecBox.IsChecked == true && AgentsLastWordsTextBox.Text == defaultAgentsLastWords)
            {
                MessageBox.Show("Укажите подпись сотрудника (как агента Поддержки).", "Ошибка");

                AgentsLastWordsTextBox.Focus();

                return false;
            }
            if (IsDeveloperCheckBox.IsChecked == true && LightDeveloperRadioButton.IsChecked == false && MediumDeveloperRadioButton.IsChecked == false && HighDeveloperRadioButton.IsChecked == false)
            {
                MessageBox.Show("Выберите для сотрудника уровень доступа разработчика из предложенных вариантов. За подробностями по каждому из уровней доступа обращайтесь к разработчикам приложения.", "Ошибка");

                MediumDeveloperRadioButton.Focus();

                return false;
            }
            if (IsDeveloperCheckBox.IsChecked == true && LightDeveloperRadioButton.IsChecked == true && CreatorDeveloperCheckBox.IsChecked == false && SuperAdminDeveloperCheckBox.IsChecked == false && AdminEditoDeveloperCheckBox.IsChecked == false && AdminAgentDeveloperCheckBox.IsChecked == false && ManagerDeveloperCheckBox.IsChecked == false && EditorDeveloperCheckBox.IsChecked == false && AgentDeveloperCheckBox.IsChecked == false && ModeratorDeveloperCheckBox.IsChecked == false)
            {
                MessageBox.Show("Выберите те категории управленческих должностей для сотрудника, расширенная информация по которым будет ему доступна как разработчику с базовым уровнем.", "Ошибка");

                EditorDeveloperCheckBox.Focus();

                return false;
            }

            return true;
        }


        private bool CheckingIfAllValuesAreValid()
        {
            int result;

            if (LoginTextBox.Text.IndexOf(' ') != -1)
            {
                MessageBox.Show("Пробелы не допускаются в логине.", "Ошибка");

                LoginTextBox.Text = "";
                LoginTextBox.Focus();

                return false;
            }
            if (!(_storage.Users.Items.FirstOrDefault(u => u.Login == LoginTextBox.Text) == null ||
                _storage.Users.Items.FirstOrDefault(u => u.Login == LoginTextBox.Text) == _user))
            {
                MessageBox.Show("Этот логин уже используется другим сотрудником, придумайте другой.", "Ошибка");

                LoginTextBox.Text = "";
                LoginTextBox.Focus();

                return false;
            }
            if (PasswordTextBox.Text.Length != 6 && PasswordTextBox.Text.Length != 2 || !int.TryParse(PasswordTextBox.Text, out result))
            {
                MessageBox.Show("Пароль обязательно должен быть шестизначным числом. Пожалуйста, измените пароль и воспроизведите его в поле ниже.", "Ошибка");

                PasswordTextBox.Text = "";
                RepeatPasswordTextBox.Text = defaultPassword;
                RepeatPasswordTextBox.Foreground = Brushes.Gray;

                PasswordTextBox.Focus();

                return false;
            }
            if (PasswordTextBox.Text != RepeatPasswordTextBox.Text)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка");

                PasswordTextBox.Text = "";
                RepeatPasswordTextBox.Text = defaultPassword;
                RepeatPasswordTextBox.Foreground = Brushes.Gray;

                PasswordTextBox.Focus();

                return false;
            }
            if (!HelpingMethods.TryParsingTheDate(StartWorkingDateTextBox.Text))
            {
                MessageBox.Show("Дата в полях должна задаваться в формате ДД.ММ.ГГГГ — например: 09.05.1999 . Оформите дату, когда работник приступил к своим обязанностям, корректно.", "Ошибка");

                StartWorkingDateTextBox.Text = "";
                StartWorkingDateTextBox.Focus();

                return false;
            }
            if (StillWorkingCheckBox.IsChecked == false && !HelpingMethods.TryParsingTheDate(EndWorkingDateTextBox.Text))
            {
                MessageBox.Show("Дата в полях должна задаваться в формате ДД.ММ.ГГГГ — например: 09.05.1999 . Оформите дату, когда работник ушел со своей должности, корректно.", "Ошибка");

                EndWorkingDateTextBox.Text = "";
                EndWorkingDateTextBox.Focus();

                return false;
            }
            if (EditorCheckBox.IsChecked == true && !int.TryParse(EditorsFrequencyTextBox.Text, out result))
            {
                MessageBox.Show("Частота размещения постов указана неправильно. Она должна выражаться целым числом. Например, если сотрудник размещает посты раз в 3 дня, следует в этом поле указывать число 3.", "Ошибка");

                EditorsFrequencyTextBox.Text = "";
                EditorsFrequencyTextBox.Focus();

                return false;
            }
            if (AgentChecBox.IsChecked == true && (!int.TryParse(AgentsNumberTextBox.Text, out result) || result <= 0))
            {
                MessageBox.Show("Номер Агента Поддержки должен всегда быть целым числом, большим 0.", "Ошибка");

                AgentsNumberTextBox.Text = "";
                AgentsNumberTextBox.Focus();

                return false;
            }
            if (!(_storage.Users.Items.FirstOrDefault(u => u.AgentsNumber.ToString() == AgentsNumberTextBox.Text) == null ||
                _storage.Users.Items.FirstOrDefault(u => u.AgentsNumber.ToString() == AgentsNumberTextBox.Text) == _user))
            {
                MessageBox.Show("Вам нужен другой номер для Агента Поддержки, посколько указанный сейчас уже используется или использовался другим сотрудником.", "Ошибка");

                AgentsNumberTextBox.Text = "";
                AgentsNumberTextBox.Focus();

                return false;
            }

            return true;
        }



        private void CreatorCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CreatorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void SuperAdminCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SuperAdminCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void AdminEditorCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void AdminEditorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void AdminAgentCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }        

        private void AdminAgentCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }        
    }
}
