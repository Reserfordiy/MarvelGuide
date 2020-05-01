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
using System.IO;

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

        private const string deputy = "Заместитель";

        private const string directorsEmployeesFirst = "Работают под подчинением";        
        private const string directorsEmployeesSecond = "Среди них:";

        private const string allEmployees = "Работают под руководством";

        private const string employer = "Непосредственный начальник";
        private const string employerForOneHead = " (руководство отделом)";
        private const string employerForSeveralHeads = " (руководство отделами)";
        private const string employerForManager = " (аппаратный офис)";
        private const string employerForMarketer = " (отдел маркетинга)";
        private const string employerForAgent = " (отдел поддержки)";
        private const string employerForEditor = " (отдел редакции)";
        private const string employerForModerator = " (отдел модерации)";
        private const string employerForSpecial = " (отдел спецпроектов)";

        private const string directorsEmployeeMarketers = "Маркетологов";
        private const string directorsEmployeeEditors = "Редакторов";
        private const string directorsEmployeeAgents = "Агентов поддержки";
        private const string directorsEmployeeModerators = "Модераторов";
        private const string directorsEmployeeSpecials = "Спецредакторов";
        //private const string directorsEmployeeTechnician = "Техников";

        private const string ending1 = "";
        private const string ending234 = "а";
        private const string ending5 = "ов";

        private const string employees = " сотрудник";
        private const string employeeHeads1 = " руководитель отдела";
        private const string employeeHeads2 = " руководителя отделов";
        private const string employeeHeads5 = " руководителей отделов";
        private const string employeeManagers = " менеджер";
        private const string employeeMarketers = " маркетолог";
        private const string employeeEditors = " редактор";
        private const string employeeAgents = " агент";
        private const string employeeModerators = " модератор";
        private const string employeeSpecials = " спецредактор";
        //private const string employeeTechnicians = " техник";

        private const string directorsJob = "Полная должность директора";

        private const string managersJob = "Менеджерская должность";        

        private const string editorsRubric = "Редакторская рубрика";
        private const string editorsFrequency = "Частота размещения";

        private const string specialsProject = "Спецпроект";

        private const string agentsNumber = "Агентский номер";
        private const string agentsFirstWords = "Приветствие агента";
        private const string agentsLastWords = "Подпись агента";        


        private const string showDetailsButton = "Показать подробности";
        private const string hideDetailsButton = "Скрыть подробности";

        private const string exitOwnProfile = "Выйти";
        private const string exitForeignProfile = "Назад";

        private const string updatePassword = "Обновить пароль";
        private const string oldPassword = "Старый пароль";

        private const string personalPageTitle = "Ваш профиль";
        private const string watchingForeignPageTitle = "Профиль ";
        private const string addingNewUserTitle = "Добавление нового сотрудника";
        private const string editingUserTitle = "Изменение профиля ";
        private const string editingYourProfileTitle = "Изменение Вашего профиля";


        private const string defaultName = "Пример: Иван";
        private const string defaultSurname = "Пример: Иванов";
        private const string defaultLogin = "Пример: ivani";
        private const string defaultStartWorkingDate = "Пример: 18.08.2018";
        private const string defaultEndWorkingDate = "Пример: 01.09.2018";
        private const string defaultDirectorsPosition = "Пример: Генеральный директор";
        private const string defaultManagersPosition = "Пример: Менеджер по кадрам";
        private const string defaultEditorsRubric = "Выберите рубрику";
        private const string defaultSpecialProject = "Выберите спецпроект";
        private const string defaultEditorsFrequency = "Пример: 3";
        private const string defaultAgentsNumber = "Пример: 14";
        private const string defaultAgentsFirstWords = "Пример: Здравствуйте!";
        private const string defaultAgentsLastWords = "Пример: С любовью";

        private const string defaultImageSource = "default.jpg";

        private const string imageFolder = "../MarvelGuide.Core/Avatars";


        private const string foundationStringDate = "12.05.2010";


        private const double leftAmplitude = 23;
        private const double rightAmplitude = 40;


        private const int minimalEditorsFrequency = 5;



        readonly IStorage _storage;

        readonly User _user;
        readonly User _userWhoWatches;

        Picture _picture;

        List<string> _personalData;

        List<EditorsPublication> _editorsPublications;
        List<EditorsPublication> _specialsProjects;

        Rubric _unselectedRubric;
        Rubric _unselectedProject;

        int _amountOfRegularJobs;
        int _amountOfHeadJobs;

        int _additionalData = 0;
        bool _detailsShown = false;

        bool _goingToTheTeamWindow = false;
        bool _goingToTheDeveloperMode = false;
        bool _goingToEditDocuments = false;
        bool _goingToEditRubrics = false;
        bool _goingToTheUserDetailsWindow = false;

        readonly bool _personalPage = true;
        readonly bool _editingPage = false;

        readonly bool _fullVersionOfTheTeamWasShown = false;

        bool _savingCompleted = false;
        

        bool _newImagesWereUploaded = false;
        Picture _lastUploadedImage = null;

        bool _programSwitch = true;


        ComboBox CapturedComboBox = null;



        public ProfileWindow(User user)
        {
            _storage = Factory.Instance.GetStorage();

            _user = user;

            InitializeComponent();

            CheckingWhetherWeEditPage();
        }

        public ProfileWindow(User user, User userWhoWatches, bool editingPage, bool fullVersionOfTheTeamWasShown)
        {
            _personalPage = false;

            _user = user;
            _userWhoWatches = userWhoWatches;

            _fullVersionOfTheTeamWasShown = fullVersionOfTheTeamWasShown;
            _editingPage = editingPage;

            _storage = Factory.Instance.GetStorage();

            InitializeComponent();           

            CheckingWhetherWeEditPage();

            _programSwitch = false;
        }

        public ProfileWindow(User user, User userWhoWatches, bool fullVersionOfTheTeamWasShown) : this (user, userWhoWatches, false, fullVersionOfTheTeamWasShown) { }



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

            if (_user.HeadOfManagers) { _amountOfHeadJobs++; }
            if (_user.HeadOfMarketers) { _amountOfHeadJobs++; }
            if (_user.HeadOfEditors) { _amountOfHeadJobs++; }
            if (_user.HeadOfAgents) { _amountOfHeadJobs++; }
            if (_user.HeadOfModerators) { _amountOfHeadJobs++; }
            if (_user.HeadOfSpecials) { _amountOfHeadJobs++; }
            if (_user.HeadOfTechnicians) { _amountOfHeadJobs++; }

            if (_user.Manager) { _amountOfRegularJobs++; }
            if (_user.Marketer) { _amountOfRegularJobs++; }
            if (_user.Editor) { _amountOfRegularJobs++; }
            if (_user.Agent) { _amountOfRegularJobs++; }
            if (_user.Moderator) { _amountOfRegularJobs++; }
            if (_user.Special) { _amountOfRegularJobs++; }
            if (_user.Technician) { _amountOfRegularJobs++; }

            PersonalDataListBox.ItemsSource = _personalData;
        }

        private void FormingTheEdittingData()
        {
            _unselectedRubric = new Rubric
            {
                Id = -1,
                Name = defaultEditorsRubric
            };

            _unselectedProject = new Rubric
            {
                Id = -1,
                Name = defaultSpecialProject,
                SpecialProject = true
            };

            _editorsPublications = new List<EditorsPublication>
            {
                new EditorsPublication()
                {
                    Frequency = -1,
                    Rubric = null,
                    RubricID = -1
                }
            };

            _specialsProjects = new List<EditorsPublication>
            {
                new EditorsPublication()
                {
                    Frequency = -1,
                    Rubric = null,
                    RubricID = -1
                }
            };

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
                
                MainPasswordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordTextBlock.Visibility = Visibility.Collapsed;

                RepeatPasswordBox.Visibility = Visibility.Collapsed;
                RepeatPasswordTextBox.Visibility = Visibility.Collapsed;
                RepeatPasswordTextBlock.Visibility = Visibility.Collapsed;

                PasswordButton.Content = updatePassword;

                if (!_user.WorkingNow)
                {
                    StillWorkingCheckBox.IsChecked = false;

                    EndWorkingDateTextBox.Text = _user.LostTheJob.ToString("d");
                    EndWorkingDateTextBox.Foreground = Brushes.Black;
                }
                else
                {
                    StartWorkingDateTextBox.Margin = new Thickness(StartWorkingDateTextBox.Margin.Left, StartWorkingDateTextBox.Margin.Top, StartWorkingDateTextBox.Margin.Right, 11);
                }

                StartWorkingDateTextBox.Text = _user.GotAJob.ToString("d");
                StartWorkingDateTextBox.Foreground = Brushes.Black;

                Today1Button.Visibility = Visibility.Collapsed;

                if (_user.GeneralDirector) { GeneralDirectorCheckBox.IsChecked = true; }
                if (_user.Director)
                {
                    DirectorCheckBox.IsChecked = true;

                    DirectorsPositionTextBox.Text = _user.DirectorsPosition;
                    DirectorsPositionTextBox.Foreground = Brushes.Black;
                }
                if (_user.DeputyGeneralDirector) { DeputyGeneralDirectorCheckBox.IsChecked = true; }
                if (_user.HeadOfManagers) { HeadOfManagersCheckBox.IsChecked = true; }
                if (_user.HeadOfMarketers) { HeadOfMarketersCheckBox.IsChecked = true; }
                if (_user.HeadOfEditors) { HeadOfEditorsCheckBox.IsChecked = true; }
                if (_user.HeadOfAgents) { HeadOfAgentsCheckBox.IsChecked = true; }
                if (_user.HeadOfModerators) { HeadOfModeratorsCheckBox.IsChecked = true; }
                if (_user.HeadOfSpecials) { HeadOfSpecialsCheckBox.IsChecked = true; }
                if (_user.HeadOfTechnicians) { HeadOfTechniciansCheckBox.IsChecked = true; }
                if (_user.Manager)
                {
                    ManagerCheckBox.IsChecked = true;

                    ManagersPositionTextBox.Text = _user.ManagersPosition;
                    ManagersPositionTextBox.Foreground = Brushes.Black;
                }
                if (_user.Marketer) { MarketerCheckBox.IsChecked = true; }
                if (_user.Editor)
                {
                    EditorCheckBox.IsChecked = true;

                    _editorsPublications = _user.EditorsRubrics.Select(publication => new EditorsPublication { Rubric = publication.Rubric, RubricID = publication.RubricID, Frequency = publication.Frequency }).ToList();

                    EditorsInformationListBox.ItemsSource = _editorsPublications;
                }
                if (_user.Special)
                {
                    SpecialCheckBox.IsChecked = true;

                    _specialsProjects = _user.SpecialsProjects.Select(publication => new EditorsPublication { Rubric = publication.Rubric, RubricID = publication.RubricID, Frequency = -1 }).ToList();

                    SpecialsInformationListBox.ItemsSource = _specialsProjects;
                }
                if (_user.Agent)
                {
                    AgentCheckBox.IsChecked = true;

                    AgentsNumberTextBox.Text = _user.AgentsNumber.ToString();
                    AgentsNumberTextBox.Foreground = Brushes.Black;

                    AgentsFirstWordsTextBox.Text = _user.AgentsFirstWords;
                    AgentsFirstWordsTextBox.Foreground = Brushes.Black;

                    AgentsLastWordsTextBox.Text = _user.AgentsLastWords;
                    AgentsLastWordsTextBox.Foreground = Brushes.Black;
                }
                if (_user.Moderator) { ModeratorCheckBox.IsChecked = true; }

                if (_user.IsDeveloper)
                {
                    IsDeveloperCheckBox.IsChecked = true;

                    if (_user.HighDeveloper || _user.SuperDeveloper) { HighDeveloperRadioButton.IsChecked = true; }
                    else if (_user.MediumDeveloper) { MediumDeveloperRadioButton.IsChecked = true; }
                    else
                    {
                        LightDeveloperRadioButton.IsChecked = true;

                        if (_user.LightDeveloperGeneralDirector) { GeneralDirectorDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperDirector) { DirectorDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperDeputyGeneralDirector) { DeputyGeneralDirectorDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperHeadOfManagers) { HeadOfManagersDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperHeadOfMarketers) { HeadOfMarketersDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperHeadOfEditors) { HeadOfEditorsDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperHeadOfAgents) { HeadOfAgentsDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperHeadOfModerators) { HeadOfModeratorsDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperHeadOfSpecials) { HeadOfSpecialsDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperHeadOfTechnicians) { HeadOfTechniciansDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperManager) { ManagerDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperMarketer) { MarketerDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperEditor) { EditorDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperAgent) { AgentDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperModerator) { ModeratorDeveloperCheckBox.IsChecked = true; }
                        if (_user.LightDeveloperSpecial) { SpecialDeveloperCheckBox.IsChecked = true; }
                    }
                }
            }

            else
            {
                RepeatPasswordTextBox.Margin = new Thickness(RepeatPasswordTextBox.Margin.Left, RepeatPasswordTextBox.Margin.Top, RepeatPasswordTextBox.Margin.Right, SexGrid.Margin.Bottom);
                RepeatPasswordBox.Margin = RepeatPasswordTextBox.Margin;

                PasswordButton.Visibility = Visibility.Collapsed;

                Today1Button.Margin = new Thickness(Today1Button.Margin.Left, Today1Button.Margin.Top, Today1Button.Margin.Right, 13);
            }

            EditorsInformationListBox.ItemsSource = _editorsPublications;
            SpecialsInformationListBox.ItemsSource = _specialsProjects;
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
            if (_goingToTheUserDetailsWindow && _personalPage)
            {
                UserDetailsWindow userDetailsWindow = new UserDetailsWindow(_user);

                userDetailsWindow.Show();
            }

            else if (_goingToTheUserDetailsWindow && !_personalPage)
            {
                UserDetailsWindow userDetailsWindow = new UserDetailsWindow(_user, _userWhoWatches);

                userDetailsWindow.Show();
            }

            else if (_goingToEditRubrics)
            {
                AllRubricsWindow allRubricsWindow = new AllRubricsWindow(_user);

                allRubricsWindow.Show();
            }

            else if (_goingToEditDocuments)
            {
                AllDocumentsWindow allDocumentsWindow = new AllDocumentsWindow(_user);

                allDocumentsWindow.Show();
            }

            else if (!_goingToTheTeamWindow && !_goingToTheDeveloperMode)
            {
                if (_personalPage)
                {
                    MainWindow mainWindow = new MainWindow();

                    mainWindow.Show();
                }
                else
                {
                    if (_newImagesWereUploaded)
                    {
                        DeleteImage(_lastUploadedImage.ImageSource);
                    }

                    ProfileWindow profileWindow = new ProfileWindow(_userWhoWatches);

                    profileWindow.Show();
                }
            }

            else if (_goingToTheDeveloperMode)
            {
                TheTeamWindow theTeamWindow;

                if (_editingPage)
                {
                    if (_newImagesWereUploaded && !_savingCompleted)
                    {
                        DeleteImage(_lastUploadedImage.ImageSource);
                    }

                    theTeamWindow = new TheTeamWindow(_userWhoWatches, _user, true, true);
                }
                else
                {
                    if (_personalPage)
                    {
                        theTeamWindow = new TheTeamWindow(_user, true, true);
                    }
                    else
                    {
                        theTeamWindow = new TheTeamWindow(_userWhoWatches, true, true);
                    }
                }

                theTeamWindow.Show();
            }

            else
            {
                if (_personalPage)
                {
                    TheTeamWindow theTeamWindow = new TheTeamWindow(_user, false);

                    theTeamWindow.Show();
                }
                else
                {
                    TheTeamWindow theTeamWindow = new TheTeamWindow(_userWhoWatches, _user, _fullVersionOfTheTeamWasShown);

                    theTeamWindow.Show();
                }                
            }
        }



        private void AvatarImage_Initialized(object sender, EventArgs e)
        {
            try
            {
                AvatarImage.Source = UIElementsMethods.InitializingBitmapImage(_user.Avatar.ImageSource, imageFolder);
            }
            catch
            {
                AvatarImage.Source = UIElementsMethods.InitializingBitmapImage(defaultImageSource, imageFolder);
            }
        }

        

        private void ShowDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_detailsShown)
            {
                _detailsShown = true;

                if (_user.GeneralDirector) { GeneralDirectorsDetails(); }
                if (_user.Director) { DirectorsDetails(); }
                if (_user.DeputyGeneralDirector) { DeputyGeneralDirectorsDetails(); }
                if (_user.IsHead) { HeadsEmloyerDetails(); }
                if (_user.HeadOfManagers) { HeadOfManagersDetails(); }
                if (_user.HeadOfMarketers) { HeadOfMarketersDetails(); }
                if (_user.HeadOfEditors) { HeadOfEditorsDetails(); }
                if (_user.HeadOfSpecials) { HeadOfSpecialsDetails(); }
                if (_user.HeadOfAgents) { HeadOfAgentsDetails(); }
                if (_user.HeadOfModerators) { HeadOfModeratorsDetails(); }
                if (_user.HeadOfTechnicians) { HeadOfTechniciansDetails(); }
                if (_user.Manager) { ManagersDetails(); }
                if (_user.Marketer) { MarketersDetails(); }
                if (_user.Editor) { EditorsDetails(); }
                if (_user.Special) { SpecialsDetails(); }
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


        private void GeneralDirectorsDetails()
        {
            var numberOfEmployees = _storage.Users.Items.Count(u => u.WorkingNow && !u.Director && !u.GeneralDirector);

            if (_user.WorkingNow)
            {
                if (_storage.Users.Items.FirstOrDefault(u => u.DeputyGeneralDirector) is User deputyGeneralDirector)
                {
                    _personalData.Add(deputy + adding + deputyGeneralDirector.Name + " " + deputyGeneralDirector.Surname);

                    _additionalData++;
                }

                _personalData.Add(directorsEmployeesFirst + adding + numberOfEmployees.ToString() + employees + HelpingMethods.ChoosingTheCorrespondingEnding(ending1, ending234, ending5, numberOfEmployees));
                _personalData.Add(directorsEmployeesSecond);
              //_personalData.Add(directorsEmployeeManagers + adding + _storage.Users.Items.Count(u => u.Manager && u.WorkingNow && !u.Director && !u.GeneralDirector).ToString());
                _personalData.Add(directorsEmployeeMarketers + adding + _storage.Users.Items.Count(u => u.Marketer && u.WorkingNow && !u.Director && !u.GeneralDirector).ToString());
                _personalData.Add(directorsEmployeeEditors + adding + _storage.Users.Items.Count(u => u.Editor && u.WorkingNow && !u.Director && !u.GeneralDirector).ToString());
                _personalData.Add(directorsEmployeeSpecials + adding + _storage.Users.Items.Count(u => u.Special && u.WorkingNow && !u.Director && !u.GeneralDirector).ToString());
                _personalData.Add(directorsEmployeeAgents + adding + _storage.Users.Items.Count(u => u.Agent && u.WorkingNow && !u.Director && !u.GeneralDirector).ToString());
                _personalData.Add(directorsEmployeeModerators + adding + _storage.Users.Items.Count(u => u.Moderator && u.WorkingNow && !u.Director && !u.GeneralDirector).ToString());

                _additionalData += 7;
            }
        }

        private void DirectorsDetails()
        {
            var numberOfEmployees = _storage.Users.Items.Count(u => u.WorkingNow && !u.Director && !u.GeneralDirector);

            _personalData.Add(directorsJob + adding + _user.DirectorsPosition);

            _additionalData++;

            if (_user.WorkingNow)
            {
                _personalData.Add(directorsEmployeesFirst + adding + numberOfEmployees.ToString() + employees + HelpingMethods.ChoosingTheCorrespondingEnding(ending1, ending234, ending5, numberOfEmployees));
                _personalData.Add(directorsEmployeesSecond);
              //_personalData.Add(directorsEmployeeManagers + adding + _storage.Users.Items.Count(u => u.Manager && u.WorkingNow && !u.Director && !u.GeneralDirector).ToString());
                _personalData.Add(directorsEmployeeMarketers + adding + _storage.Users.Items.Count(u => u.Marketer && u.WorkingNow && !u.Director && !u.GeneralDirector).ToString());
                _personalData.Add(directorsEmployeeEditors + adding + _storage.Users.Items.Count(u => u.Editor && u.WorkingNow && !u.Director && !u.GeneralDirector).ToString());
                _personalData.Add(directorsEmployeeSpecials + adding + _storage.Users.Items.Count(u => u.Special && u.WorkingNow && !u.Director && !u.GeneralDirector).ToString());
                _personalData.Add(directorsEmployeeAgents + adding + _storage.Users.Items.Count(u => u.Agent && u.WorkingNow && !u.Director && !u.GeneralDirector).ToString());
                _personalData.Add(directorsEmployeeModerators + adding + _storage.Users.Items.Count(u => u.Moderator && u.WorkingNow && !u.Director && !u.GeneralDirector).ToString());

                _additionalData += 7;
            }
        }

        private void DeputyGeneralDirectorsDetails()
        {
            if (_user.WorkingNow)
            {
                User generalDirector = _storage.Users.Items.FirstOrDefault(u => u.GeneralDirector && u.WorkingNow);

                if (generalDirector != null)
                {
                    _personalData.Add(employer + adding + generalDirector.Name + " " + generalDirector.Surname);

                    _additionalData++;
                }

                var numberOfHeads = _storage.Users.Items.Count(u => u.IsHead && u.WorkingNow && !u.Director && !u.GeneralDirector && !u.DeputyGeneralDirector);

                _personalData.Add(allEmployees + adding + numberOfHeads.ToString() + HelpingMethods.ChoosingTheCorrespondingEnding(employeeHeads1, employeeHeads2, employeeHeads5, numberOfHeads));

                _additionalData++;
            }
        }

        private void HeadsEmloyerDetails()
        {
            if (_user.WorkingNow && !_user.GeneralDirector && !_user.Director && !_user.DeputyGeneralDirector)
            {
                User deputyGeneralDirector = _storage.Users.Items.FirstOrDefault(u => u.DeputyGeneralDirector && u.WorkingNow);

                if (deputyGeneralDirector != null)
                {
                    if (_amountOfRegularJobs == 0)
                    {
                        _personalData.Add(employer + adding + deputyGeneralDirector.Name + " " + deputyGeneralDirector.Surname);
                    }
                    else if (_amountOfHeadJobs == 1)
                    {
                        _personalData.Add(employer + employerForOneHead + adding + deputyGeneralDirector.Name + " " + deputyGeneralDirector.Surname);
                    }
                    else
                    {
                        _personalData.Add(employer + employerForSeveralHeads + adding + deputyGeneralDirector.Name + " " + deputyGeneralDirector.Surname);
                    }

                    _additionalData++;
                }
            }
        }

        private void HeadOfManagersDetails()
        {
            if (_user.WorkingNow)
            {
                var numberOfManagers = _storage.Users.Items.Count(u => u.Manager && u.WorkingNow && !u.GeneralDirector && !u.Director && !u.DeputyGeneralDirector && !u.HeadOfManagers);

                _personalData.Add(allEmployees + adding + numberOfManagers.ToString() + employeeManagers + HelpingMethods.ChoosingTheCorrespondingEnding(ending1, ending234, ending5, numberOfManagers));

                _additionalData++;
            }
        }

        private void HeadOfMarketersDetails()
        {
            if (_user.WorkingNow)
            {
                var numberOfMarketers = _storage.Users.Items.Count(u => u.Marketer && u.WorkingNow && !u.GeneralDirector && !u.Director && !u.DeputyGeneralDirector && !u.HeadOfMarketers);

                _personalData.Add(allEmployees + adding + numberOfMarketers.ToString() + employeeMarketers + HelpingMethods.ChoosingTheCorrespondingEnding(ending1, ending234, ending5, numberOfMarketers));

                _additionalData++;
            }
        }

        private void HeadOfEditorsDetails()
        {
            if (_user.WorkingNow)
            {
                var numberOfEditors = _storage.Users.Items.Count(u => u.Editor && u.WorkingNow && !u.GeneralDirector && !u.Director && !u.DeputyGeneralDirector && !u.HeadOfEditors);

                _personalData.Add(allEmployees + adding + numberOfEditors.ToString() + employeeEditors + HelpingMethods.ChoosingTheCorrespondingEnding(ending1, ending234, ending5, numberOfEditors));

                _additionalData++;
            }
        }

        private void HeadOfAgentsDetails()
        {
            if (_user.WorkingNow)
            {
                var numberOfAgents = _storage.Users.Items.Count(u => u.Agent && u.WorkingNow && !u.GeneralDirector && !u.Director && !u.DeputyGeneralDirector && !u.HeadOfAgents);

                _personalData.Add(allEmployees + adding + numberOfAgents.ToString() + employeeAgents + HelpingMethods.ChoosingTheCorrespondingEnding(ending1, ending234, ending5, numberOfAgents));

                _additionalData++;
            }
        }

        private void HeadOfModeratorsDetails()
        {
            if (_user.WorkingNow)
            {
                var numberOfModerators = _storage.Users.Items.Count(u => u.Moderator && u.WorkingNow && !u.GeneralDirector && !u.Director && !u.DeputyGeneralDirector && !u.HeadOfModerators);

                _personalData.Add(allEmployees + adding + numberOfModerators.ToString() + employeeModerators + HelpingMethods.ChoosingTheCorrespondingEnding(ending1, ending234, ending5, numberOfModerators));

                _additionalData++;
            }
        }

        private void HeadOfSpecialsDetails()
        {
            if (_user.WorkingNow)
            {
                var numberOfSpecials = _storage.Users.Items.Count(u => u.Special && u.WorkingNow && !u.GeneralDirector && !u.Director && !u.DeputyGeneralDirector && !u.HeadOfSpecials);

                _personalData.Add(allEmployees + adding + numberOfSpecials.ToString() + employeeSpecials + HelpingMethods.ChoosingTheCorrespondingEnding(ending1, ending234, ending5, numberOfSpecials));

                _additionalData++;
            }
        }

        private void HeadOfTechniciansDetails()
        {

        }

        private void ManagersDetails()
        {
            _personalData.Add(managersJob + adding + _user.ManagersPosition);

            _additionalData++;

            if (_user.WorkingNow && !_user.GeneralDirector && !_user.Director && !_user.DeputyGeneralDirector && !_user.HeadOfManagers)
            {
                if (_storage.Users.Items.FirstOrDefault(u => u.HeadOfManagers && u.WorkingNow) is User headOfManagers)
                {
                    if (_amountOfRegularJobs + _amountOfHeadJobs == 1)
                    {
                        _personalData.Add(employer + adding + headOfManagers.Name + " " + headOfManagers.Surname);
                    }
                    else
                    {
                        _personalData.Add(employer + employerForManager + adding + headOfManagers.Name + " " + headOfManagers.Surname);
                    }

                    _additionalData++;
                }
            }
        }

        private void MarketersDetails()
        {
            if (_user.WorkingNow && !_user.GeneralDirector && !_user.Director && !_user.DeputyGeneralDirector && !_user.HeadOfMarketers)
            {
                if (_storage.Users.Items.FirstOrDefault(u => u.HeadOfMarketers && u.WorkingNow) is User headOfMarketers)
                {
                    if (_amountOfRegularJobs + _amountOfHeadJobs == 1)
                    {
                        _personalData.Add(employer + adding + headOfMarketers.Name + " " + headOfMarketers.Surname);
                    }
                    else
                    {
                        _personalData.Add(employer + employerForMarketer + adding + headOfMarketers.Name + " " + headOfMarketers.Surname);
                    }

                    _additionalData++;
                }
            }
        }

        private void EditorsDetails()
        {
            foreach (var publication in _user.EditorsRubrics)
            {
                _personalData.Add(editorsRubric + adding + publication.Rubric.Name);

                if (_user.WorkingNow)
                {
                    _personalData.Add(editorsFrequency + adding + publication.StringFrequency());

                    _additionalData++;
                }

                _additionalData++;
            }

            if (_user.WorkingNow && !_user.GeneralDirector && !_user.Director && !_user.DeputyGeneralDirector && !_user.HeadOfEditors)
            {
                if (_storage.Users.Items.FirstOrDefault(u => u.HeadOfEditors && u.WorkingNow) is User headOfEditors)
                {
                    if (_amountOfRegularJobs + _amountOfHeadJobs == 1)
                    {
                        _personalData.Add(employer + adding + headOfEditors.Name + " " + headOfEditors.Surname);
                    }
                    else
                    {
                        _personalData.Add(employer + employerForEditor + adding + headOfEditors.Name + " " + headOfEditors.Surname);
                    }

                    _additionalData++;
                }
            }
        }

        private void SpecialsDetails()
        {
            foreach (var publication in _user.SpecialsProjects)
            {
                _personalData.Add(specialsProject + adding + publication.Rubric.Name);

                _additionalData++;
            }

            if (_user.WorkingNow && !_user.GeneralDirector && !_user.Director && !_user.DeputyGeneralDirector && !_user.HeadOfSpecials)
            {
                if (_storage.Users.Items.FirstOrDefault(u => u.HeadOfSpecials && u.WorkingNow) is User headOfSpecials)
                {
                    if (_amountOfRegularJobs + _amountOfHeadJobs == 1)
                    {
                        _personalData.Add(employer + adding + headOfSpecials.Name + " " + headOfSpecials.Surname);
                    }
                    else
                    {
                        _personalData.Add(employer + employerForSpecial + adding + headOfSpecials.Name + " " + headOfSpecials.Surname);
                    }

                    _additionalData++;
                }
            }
        }

        private void AgentsDetails()
        {
            _personalData.Add(agentsNumber + adding + _user.AgentsNumber.ToString());
            _personalData.Add(agentsFirstWords + adding + _user.AgentsFirstWords);
            _personalData.Add(agentsLastWords + adding + _user.AgentsLastWords);

            _additionalData += 3;

            if (_user.WorkingNow && !_user.GeneralDirector && !_user.Director && !_user.DeputyGeneralDirector && !_user.HeadOfAgents)
            {
                if (_storage.Users.Items.FirstOrDefault(u => u.HeadOfAgents && u.WorkingNow) is User headOfAgents)
                {
                    if (_amountOfRegularJobs + _amountOfHeadJobs == 1)
                    {
                        _personalData.Add(employer + adding + headOfAgents.Name + " " + headOfAgents.Surname);
                    }
                    else
                    {
                        _personalData.Add(employer + employerForAgent + adding + headOfAgents.Name + " " + headOfAgents.Surname);
                    }

                    _additionalData++;
                }
            }
        }

        private void ModeratorsDetails()
        {
            if (_user.WorkingNow && !_user.GeneralDirector && !_user.Director && !_user.DeputyGeneralDirector && !_user.HeadOfModerators)
            {
                if (_storage.Users.Items.FirstOrDefault(u => u.HeadOfModerators && u.WorkingNow) is User headOfModerators)
                {
                    if (_amountOfRegularJobs + _amountOfHeadJobs == 1)
                    {
                        _personalData.Add(employer + adding + headOfModerators.Name + " " + headOfModerators.Surname);
                    }
                    else
                    {
                        _personalData.Add(employer + employerForModerator + adding + headOfModerators.Name + " " + headOfModerators.Surname);
                    }

                    _additionalData++;
                }
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

        private void UserDetailsButton_Initialized(object sender, EventArgs e)
        {
            if (!CheckingWhetherUserDetailsButtonIsShown())
            {
                UserDetailsButton.Visibility = Visibility.Collapsed;
            }
        }

        private void DeveloperModeButton_Initialized(object sender, EventArgs e)
        {
            if (!(_user.SuperDeveloper || _user.HighDeveloper || _user.MediumDeveloper) || !_personalPage)
            {
                DeveloperModeButton.Visibility = Visibility.Collapsed;
            }
        }

        private void EditDocumentsButton_Initialized(object sender, EventArgs e)
        {
            if (!(_user.SuperDeveloper || _user.HighDeveloper || _user.MediumDeveloper) || !_personalPage)
            {
                EditDocumentsButton.Visibility = Visibility.Collapsed;
            }
        }

        private void EditRubricsButton_Initialized(object sender, EventArgs e)
        {
            if (!(_user.SuperDeveloper || _user.HighDeveloper || _user.MediumDeveloper) || !_personalPage)
            {
                EditRubricsButton.Visibility = Visibility.Collapsed;
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
            if (!_editingPage || !_user.IsDeveloper)
            {
                DevelopersLevelGrid.Visibility = Visibility.Collapsed;
            }
        }



        private void UploadAvatarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WorkWithImages imageUploadingProcess = new WorkWithImages();

                imageUploadingProcess.UploadImageAndSave(imageFolder);

                _picture = imageUploadingProcess.Picture;

                AvatarImage.Source = UIElementsMethods.InitializingBitmapImage(_picture.ImageSource, imageFolder);

                if (_newImagesWereUploaded)
                {
                    DeleteImage(_lastUploadedImage.ImageSource);
                }
                else
                {
                    _newImagesWereUploaded = true;
                }

                _lastUploadedImage = _picture;
            }
            catch { }
        }

        private void DeleteImage(string imageSource)
        {
            WorkWithImages deletingProcess = new WorkWithImages();

            deletingProcess.DeleteImage(imageFolder, imageSource);
        }


        private void ShowTheTeamButton_Click(object sender, RoutedEventArgs e)
        {
            _goingToTheTeamWindow = true;

            Close();
        }

        private void UserDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            _goingToTheUserDetailsWindow = true;

            Close();
        }        

        private void DeveloperModeButton_Click(object sender, RoutedEventArgs e)
        {
            _goingToTheDeveloperMode = true;

            Close();
        }

        private void EditDocumentsButton_Click(object sender, RoutedEventArgs e)
        {
            _goingToEditDocuments = true;

            Close();
        }

        private void EditRubricsButton_Click(object sender, RoutedEventArgs e)
        {
            _goingToEditRubrics = true;

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

            if (PasswordTextBlock.Visibility == Visibility.Visible)
            {
                _user.Password = User.GetHash(MainPasswordBox.Password);
            }

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

            if (GeneralDirectorCheckBox.IsChecked == true) { _user.GeneralDirector = true; }
            else { _user.GeneralDirector = false; }
            if (DirectorCheckBox.IsChecked == true)
            {
                _user.Director = true;
                _user.DirectorsPosition = DirectorsPositionTextBox.Text;
            }
            else
            {
                _user.Director = false;
                _user.DirectorsPosition = null;
            }
            if (DeputyGeneralDirectorCheckBox.IsChecked == true) { _user.DeputyGeneralDirector = true; }
            else { _user.DeputyGeneralDirector = false; }
            if (HeadOfManagersCheckBox.IsChecked == true) { _user.HeadOfManagers = true; }
            else { _user.HeadOfManagers = false; }
            if (HeadOfMarketersCheckBox.IsChecked == true) { _user.HeadOfMarketers = true; }
            else { _user.HeadOfMarketers = false; }
            if (HeadOfEditorsCheckBox.IsChecked == true) { _user.HeadOfEditors = true; }
            else { _user.HeadOfEditors = false; }
            if (HeadOfAgentsCheckBox.IsChecked == true) { _user.HeadOfAgents = true; }
            else { _user.HeadOfAgents = false; }
            if (HeadOfModeratorsCheckBox.IsChecked == true) { _user.HeadOfModerators = true; }
            else { _user.HeadOfModerators = false; }
            if (HeadOfSpecialsCheckBox.IsChecked == true) { _user.HeadOfSpecials = true; }
            else { _user.HeadOfSpecials = false; }
            if (HeadOfTechniciansCheckBox.IsChecked == true) { _user.HeadOfTechnicians = true; }
            else { _user.HeadOfTechnicians = false; }
            if (ManagerCheckBox.IsChecked == true)
            {
                _user.Manager = true;
                _user.ManagersPosition = ManagersPositionTextBox.Text;
            }
            else
            {
                _user.Manager = false;
                _user.ManagersPosition = null;
            }
            if (MarketerCheckBox.IsChecked == true) { _user.Marketer = true; }
            else { _user.Marketer = false; }
            if (EditorCheckBox.IsChecked == true)
            {
                _user.Editor = true;
                FixingEditorsData();
            }
            else
            {
                _user.Editor = false;
                _user.EditorsRubrics = new List<EditorsPublication>();
            }
            if (SpecialCheckBox.IsChecked == true)
            {
                _user.Special = true;
                FixingSpecialsData();
            }
            else
            {
                _user.Special = false;
                _user.SpecialsProjects = new List<EditorsPublication>();
            }
            if (AgentCheckBox.IsChecked == true)
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
            if (ModeratorCheckBox.IsChecked == true) { _user.Moderator = true; }
            else { _user.Moderator = false; }

            if (!_user.SuperDeveloper)
            {
                if (IsDeveloperCheckBox.IsChecked == true)
                {
                    if (HighDeveloperRadioButton.IsChecked == true)
                    {
                        _user.NotADeveloper();
                        _user.HighDeveloper = true;
                    }
                    else if (MediumDeveloperRadioButton.IsChecked == true)
                    {
                        _user.NotADeveloper();
                        _user.MediumDeveloper = true;
                    }
                    else
                    {
                        _user.NotADeveloper();

                        if (GeneralDirectorDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperGeneralDirector = true; }
                        else { _user.LightDeveloperGeneralDirector = false; }
                        if (DirectorDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperDirector = true; }
                        else { _user.LightDeveloperDirector = false; }
                        if (DeputyGeneralDirectorDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperDeputyGeneralDirector = true; }
                        else { _user.LightDeveloperDeputyGeneralDirector = false; }
                        if (HeadOfManagersDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperHeadOfManagers = true; }
                        else { _user.LightDeveloperHeadOfManagers = false; }
                        if (HeadOfMarketersDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperHeadOfMarketers = true; }
                        else { _user.LightDeveloperHeadOfMarketers = false; }
                        if (HeadOfEditorsDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperHeadOfEditors = true; }
                        else { _user.LightDeveloperHeadOfEditors = false; }
                        if (HeadOfAgentsDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperHeadOfAgents = true; }
                        else { _user.LightDeveloperHeadOfAgents = false; }
                        if (HeadOfModeratorsDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperHeadOfModerators = true; }
                        else { _user.LightDeveloperHeadOfModerators = false; }
                        if (HeadOfSpecialsDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperHeadOfSpecials = true; }
                        else { _user.LightDeveloperHeadOfSpecials = false; }
                        if (HeadOfTechniciansDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperHeadOfTechnicians = true; }
                        else { _user.LightDeveloperHeadOfTechnicians = false; }
                        if (ManagerDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperManager = true; }
                        else { _user.LightDeveloperManager = false; }
                        if (MarketerDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperMarketer = true; }
                        else { _user.LightDeveloperMarketer = false; }
                        if (EditorDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperEditor = true; }
                        else { _user.LightDeveloperEditor = false; }
                        if (AgentDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperAgent = true; }
                        else { _user.LightDeveloperAgent = false; }
                        if (ModeratorDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperModerator = true; }
                        else { _user.LightDeveloperModerator = false; }
                        if (SpecialDeveloperCheckBox.IsChecked == true) { _user.LightDeveloperSpecial = true; }
                        else { _user.LightDeveloperSpecial = false; }
                    }
                }
                else { _user.NotADeveloper(); }
            }

            if (_newImagesWereUploaded)
            {
                try
                {
                    DeleteImage(_user.Avatar.ImageSource);
                }
                catch { }

                _user.Avatar = _picture;
            }

            _savingCompleted = true;
        }


        private void FixingEditorsData()
        {
            var editorsPublications = new List<EditorsPublication>();

            for (int i = 0; i < EditorsInformationListBox.Items.Count; i++)
            {
                var EditorsRubricComboBox = UIElementsMethods.GetUIElementChildByNumberFromTemplatedListBox(EditorsInformationListBox, i, 1, 0) as ComboBox;
                var EditorsFrequencyTextBox = UIElementsMethods.GetUIElementChildByNumberFromTemplatedListBox(EditorsInformationListBox, i, 1, 1) as TextBox;

                var rubric = EditorsRubricComboBox.SelectedItem as Rubric;
                var frequency = int.Parse(EditorsFrequencyTextBox.Text);

                editorsPublications.Add(new EditorsPublication
                {
                    Rubric = rubric,
                    RubricID = rubric.Id,
                    Frequency = frequency
                });
            }

            _user.EditorsRubrics = editorsPublications;
        }

        private void FixingSpecialsData()
        {
            var specialsProjects = new List<EditorsPublication>();

            for (int i = 0; i < SpecialsInformationListBox.Items.Count; i++)
            {
                var SpecialProjectComboBox = UIElementsMethods.GetUIElementChildByNumberFromTemplatedListBox(SpecialsInformationListBox, i, 1, 0) as ComboBox;

                var project = SpecialProjectComboBox.SelectedItem as Rubric;

                specialsProjects.Add(new EditorsPublication
                {
                    Rubric = project,
                    RubricID = project.Id,
                    Frequency = -1
                });
            }

            _user.SpecialsProjects = specialsProjects;
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


        private void DirectorsRoleGrid_Initialized(object sender, EventArgs e)
        {
            if (!_user.Director) { DirectorsRoleGrid.Visibility = Visibility.Collapsed; }
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

        private void SpecialsRoleGrid_Initialized(object sender, EventArgs e)
        {
            if (!_user.Special) { SpecialsRoleGrid.Visibility = Visibility.Collapsed; }
        }


        private void DeveloperCheckBoxGrid_Initialized(object sender, EventArgs e)
        {
            if (!_user.IsLightDeveloper)
            {
                DeveloperCheckBoxGrid.Visibility = Visibility.Collapsed;
            }
        }


        private void EditorsRubricComboBox_Initialized(object sender, EventArgs e)
        {
            var startRubrics = new List<Rubric> { _unselectedRubric };
            var usedRubrics = _editorsPublications.Select(publ => publ.Rubric);

            ComboBox EditorsRubricComboBox = sender as ComboBox;

            EditorsPublication publication = EditorsRubricComboBox.DataContext as EditorsPublication;
            
            if (publication.Rubric != null)
            {
                startRubrics = startRubrics.Concat(new List<Rubric> { publication.Rubric }).ToList();
            }

            EditorsRubricComboBox.ItemsSource = startRubrics
                .Concat(_storage.Rubrics.Items
                    .Except(usedRubrics)
                    .Where(rubr => (rubr.Actual || StillWorkingCheckBox.IsChecked == false) && !rubr.SpecialProject)
                    .OrderByDescending(rubr => rubr.Actual)
                    .ThenBy(rubr => _storage.Users.Items.Count(u => u.Editor && u.EditorsRubrics.Exists(edPub => edPub.Rubric == rubr) && u.WorkingNow))
                    .ThenByDescending(rubr => _storage.Users.Items.Count(u => u.Editor && u.EditorsRubrics.Exists(edPub => edPub.Rubric == rubr)))
                    .ThenBy(rubr => rubr.Name));

            if (publication.Rubric == null) { EditorsRubricComboBox.SelectedIndex = 0; }
            else { EditorsRubricComboBox.SelectedIndex = 1; }
        }

        private void SpecialsProjectComboBox_Initialized(object sender, EventArgs e)
        {
            var startProjects = new List<Rubric> { _unselectedProject };
            var usedProjects = _specialsProjects.Select(publ => publ.Rubric);

            ComboBox SpecialsProjectComboBox = sender as ComboBox;

            EditorsPublication publication = SpecialsProjectComboBox.DataContext as EditorsPublication;

            if (publication.Rubric != null)
            {
                startProjects = startProjects.Concat(new List<Rubric> { publication.Rubric }).ToList();
            }

            SpecialsProjectComboBox.ItemsSource = startProjects
                .Concat(_storage.Rubrics.Items
                    .Except(usedProjects)
                    .Where(proj => (proj.Actual || StillWorkingCheckBox.IsChecked == false) && proj.SpecialProject)
                    .OrderByDescending(proj => proj.Actual)
                    .ThenBy(proj => _storage.Users.Items.Count(u => u.Special && u.SpecialsProjects.Exists(edPub => edPub.Rubric == proj) && u.WorkingNow))
                    .ThenByDescending(proj => _storage.Users.Items.Count(u => u.Special && u.SpecialsProjects.Exists(edPub => edPub.Rubric == proj)))
                    .ThenBy(proj => proj.Name));

            if (publication.Rubric == null) { SpecialsProjectComboBox.SelectedIndex = 0; }
            else { SpecialsProjectComboBox.SelectedIndex = 1; }
        }

        private void EditorsFrequencyTextBox_Initialized(object sender, EventArgs e)
        {
            TextBox EditorsFrequencyTextBox = sender as TextBox;

            EditorsPublication publication = EditorsFrequencyTextBox.DataContext as EditorsPublication;

            if (publication.Frequency != -1)
            {
                EditorsFrequencyTextBox.Text = publication.Frequency.ToString();
                EditorsFrequencyTextBox.Foreground = Brushes.Black;
            }
            else
            {
                EditorsFrequencyTextBox.Text = defaultEditorsFrequency;
                EditorsFrequencyTextBox.Foreground = Brushes.Gray;
            }
        }


        private void EndWorkingDateTextBox_Initialized(object sender, EventArgs e)
        {
            if (_user.Id == -1 || _user.WorkingNow)
            {
                StartWorkingDateTextBox.Margin = EndWorkingDateTextBox.Margin;
                EndWorkingDateTextBox.Visibility = Visibility.Collapsed;
            }
        }

        private void EndWorkingDateTextBlock_Initialized(object sender, EventArgs e)
        {
            if (_user.Id == -1 || _user.WorkingNow) { EndWorkingDateTextBlock.Visibility = Visibility.Collapsed; }
        }

        private void Today2Button_Initialized(object sender, EventArgs e)
        {
            Today2Button.Visibility = Visibility.Collapsed;
        }



        private void AddRubricButton_Click(object sender, RoutedEventArgs e)
        {
            if (StillWorkingCheckBox.IsChecked == true && _editorsPublications.Count == _storage.Rubrics.Items.Where(rubr => rubr.Actual && !rubr.SpecialProject).Count() ||
                StillWorkingCheckBox.IsChecked == false && _editorsPublications.Count == _storage.Rubrics.Items.Where(rubr => !rubr.SpecialProject).Count())
            {
                MessageBox.Show("К сожалению, другие свободные рубрики в системе отсутствуют. Добавьте требуемую рубрику в систему через меню редактирования рубрик и повторите попытку.", "Ошибка");
            }

            else
            {
                _editorsPublications.Add(new EditorsPublication()
                {
                    Frequency = -1,
                    Rubric = null,
                    RubricID = -1
                });

                EditorsInformationListBox.ItemsSource = null;
                EditorsInformationListBox.ItemsSource = _editorsPublications;
            }
        }

        private void AddSpecialProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (StillWorkingCheckBox.IsChecked == true && _specialsProjects.Count == _storage.Rubrics.Items.Where(rubr => rubr.Actual && rubr.SpecialProject).Count() ||
                StillWorkingCheckBox.IsChecked == false && _editorsPublications.Count == _storage.Rubrics.Items.Where(rubr => rubr.SpecialProject).Count())
            {
                MessageBox.Show("К сожалению, другие спецпроекты в системе отсутствуют. Добавьте нужный спецпроект в систему через меню редактирования рубрик и повторите попытку.", "Ошибка");
            }

            else
            {
                _specialsProjects.Add(new EditorsPublication()
                {
                    Frequency = -1,
                    Rubric = null,
                    RubricID = -1
                });

                SpecialsInformationListBox.ItemsSource = null;
                SpecialsInformationListBox.ItemsSource = _specialsProjects;
            }
        }


        private void EditorsDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var DeleteButton = sender as Button;

            var publication = DeleteButton.DataContext as EditorsPublication;

            _editorsPublications.Remove(publication);

            EditorsInformationListBox.ItemsSource = null;
            EditorsInformationListBox.ItemsSource = _editorsPublications;
        }

        private void SpecialsDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var DeleteButton = sender as Button;

            var publication = DeleteButton.DataContext as EditorsPublication;

            _specialsProjects.Remove(publication);

            SpecialsInformationListBox.ItemsSource = null;
            SpecialsInformationListBox.ItemsSource = _specialsProjects;
        }



        private void DirectorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DirectorsRoleGrid.Visibility = Visibility.Visible;
        }

        private void DirectorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DirectorsRoleGrid.Visibility = Visibility.Collapsed;
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

        private void AgentCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            AgentsRoleGrid.Visibility = Visibility.Visible;
        }

        private void AgentCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            AgentsRoleGrid.Visibility = Visibility.Collapsed;
        }

        private void SpecialСheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SpecialsRoleGrid.Visibility = Visibility.Visible;
        }

        private void SpecialСheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SpecialsRoleGrid.Visibility = Visibility.Collapsed;
        }


        private void LightDeveloperRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (_user != _userWhoWatches && !_userWhoWatches.MediumDeveloper && !_user.SuperDeveloper)
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
            else if (_user.SuperDeveloper)
            {
                _programSwitch = true;

                LightDeveloperRadioButton.IsChecked = false;
                MediumDeveloperRadioButton.IsChecked = false;
                HighDeveloperRadioButton.IsChecked = true;

                MessageBox.Show("Отредактировать настройки разработчика для этого сотрудника нельзя.", "Ошибка");

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
            else if (_user.SuperDeveloper && !_programSwitch)
            {
                IsDeveloperCheckBox.IsChecked = true;

                MessageBox.Show("Отредактировать настройки разработчика для этого сотрудника нельзя.", "Ошибка");
            }
            else
            {
                DevelopersLevelGrid.Visibility = Visibility.Collapsed;
            }
        }


        private void StillWorkingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (EndWorkingDateTextBlock != null && EndWorkingDateTextBox != null)
            {
                if (_user.Id == -1)
                {
                    Today1Button.Visibility = Visibility.Visible;
                    Today1Button.Margin = new Thickness(Today1Button.Margin.Left, Today1Button.Margin.Top, Today1Button.Margin.Bottom, 13);
                }
                else
                {
                    StartWorkingDateTextBox.Margin = new Thickness(StartWorkingDateTextBox.Margin.Left, StartWorkingDateTextBox.Margin.Top, StartWorkingDateTextBox.Margin.Right, 11);
                }

                EndWorkingDateTextBlock.Visibility = Visibility.Collapsed;
                EndWorkingDateTextBox.Visibility = Visibility.Collapsed;
                Today2Button.Visibility = Visibility.Collapsed;
            }
        }

        private void StillWorkingCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_user.Id == -1 || !_user.WorkingNow)
            {
                Today1Button.Visibility = Visibility.Collapsed;

                EndWorkingDateTextBox.Margin = new Thickness(EndWorkingDateTextBox.Margin.Left, EndWorkingDateTextBox.Margin.Top, EndWorkingDateTextBox.Margin.Right, 11);
            }
            else if (_user.WorkingNow)
            {
                Today2Button.Visibility = Visibility.Visible;
            }

            StartWorkingDateTextBox.Margin = LoginTextBox.Margin;

            EndWorkingDateTextBox.Visibility = Visibility.Visible;
            EndWorkingDateTextBlock.Visibility = Visibility.Visible;
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
            PasswordTextBox.Visibility = Visibility.Hidden;
            MainPasswordBox.Visibility = Visibility.Visible;

            MainPasswordBox.Focus();
        }

        private void RepeatPasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            RepeatPasswordTextBox.Visibility = Visibility.Hidden;
            RepeatPasswordBox.Visibility = Visibility.Visible;

            RepeatPasswordBox.Focus();
        }

        private void MainPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MainPasswordBox.Password == "")
            {
                PasswordTextBox.Visibility = Visibility.Visible;
                MainPasswordBox.Visibility = Visibility.Hidden;
            }
        }

        private void RepeatPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (RepeatPasswordBox.Password == "")
            {
                RepeatPasswordTextBox.Visibility = Visibility.Visible;
                RepeatPasswordBox.Visibility = Visibility.Hidden;
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
            if (StartWorkingDateTextBox.Text == "" && !Today1Button.IsMouseOver)
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
            if (EndWorkingDateTextBox.Text == "" && !Today2Button.IsMouseOver)
            {
                EndWorkingDateTextBox.Text = defaultEndWorkingDate;
                EndWorkingDateTextBox.Foreground = Brushes.Gray;
            }
        }

        private void DirectorsPositionTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DirectorsPositionTextBox.Text == defaultDirectorsPosition)
            {
                DirectorsPositionTextBox.Text = "";
                DirectorsPositionTextBox.Foreground = Brushes.Black;
            }
        }

        private void DirectorsPositionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DirectorsPositionTextBox.Text == "")
            {
                DirectorsPositionTextBox.Text = defaultDirectorsPosition;
                DirectorsPositionTextBox.Foreground = Brushes.Gray;
            }
        }

        private void ManagersPositionTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ManagersPositionTextBox.Text == defaultManagersPosition)
            {
                ManagersPositionTextBox.Text = "";
                ManagersPositionTextBox.Foreground = Brushes.Black;
            }
        }

        private void ManagersPositionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ManagersPositionTextBox.Text == "")
            {
                ManagersPositionTextBox.Text = defaultManagersPosition;
                ManagersPositionTextBox.Foreground = Brushes.Gray;
            }
        }

        private void EditorsFrequencyTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox EditorsFrequencyTextBox = sender as TextBox;

            if (EditorsFrequencyTextBox.Text == defaultEditorsFrequency)
            {
                EditorsFrequencyTextBox.Text = "";
                EditorsFrequencyTextBox.Foreground = Brushes.Black;
            }
        }

        private void EditorsFrequencyTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox EditorsFrequencyTextBox = sender as TextBox;

            EditorsPublication publication = EditorsFrequencyTextBox.DataContext as EditorsPublication;

            int frequency = -1;

            if (int.TryParse(EditorsFrequencyTextBox.Text, out _))
            {
                frequency = int.Parse(EditorsFrequencyTextBox.Text);
            }

            publication.Frequency = frequency;

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



        private void PasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBlock.Visibility == Visibility.Collapsed)
            { 
                PasswordTextBlock.Visibility = Visibility.Visible;
                RepeatPasswordTextBlock.Visibility = Visibility.Visible;

                if (MainPasswordBox.Password == "")
                {
                    PasswordTextBox.Visibility = Visibility.Visible;
                    MainPasswordBox.Visibility = Visibility.Hidden;
                }
                else
                {
                    PasswordTextBox.Visibility = Visibility.Hidden;
                    MainPasswordBox.Visibility = Visibility.Visible;
                }                

                if (RepeatPasswordBox.Password == "")
                {
                    RepeatPasswordTextBox.Visibility = Visibility.Visible;
                    RepeatPasswordBox.Visibility = Visibility.Hidden;
                }
                else
                {
                    RepeatPasswordTextBox.Visibility = Visibility.Hidden;
                    RepeatPasswordBox.Visibility = Visibility.Visible;
                }

                PasswordButton.Content = oldPassword;
            }

            else
            {
                PasswordTextBlock.Visibility = Visibility.Collapsed;
                PasswordTextBox.Visibility = Visibility.Collapsed;
                MainPasswordBox.Visibility = Visibility.Collapsed;

                RepeatPasswordTextBlock.Visibility = Visibility.Collapsed;
                RepeatPasswordTextBox.Visibility = Visibility.Collapsed;
                RepeatPasswordBox.Visibility = Visibility.Collapsed;

                PasswordButton.Content = updatePassword;
            }
        }


        private void Today1Button_Click(object sender, RoutedEventArgs e)
        {
            StartWorkingDateTextBox.Text = DateTime.Now.ToString("d");
            StartWorkingDateTextBox.Foreground = Brushes.Black;
        }

        private void Today2Button_Click(object sender, RoutedEventArgs e)
        {
            EndWorkingDateTextBox.Text = DateTime.Now.ToString("d");
            EndWorkingDateTextBox.Foreground = Brushes.Black;
        }



        private bool CheckingWhetherAllFieldsFilledCorrectly()
        {
            LoginTextBox.Text = LoginTextBox.Text.TrimEnd();

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
            if (PasswordTextBlock.Visibility == Visibility.Visible && (MainPasswordBox.Password == "" || RepeatPasswordBox.Password == ""))
            {
                MessageBox.Show("Укажите пароль для сотрудника, а затем воспроизведите его.", "Ошибка");

                MainPasswordBox.Password = "";
                RepeatPasswordBox.Password = "";

                MainPasswordBox.Visibility = Visibility.Visible;
                PasswordTextBox.Visibility = Visibility.Hidden;

                RepeatPasswordBox.Visibility = Visibility.Hidden;
                RepeatPasswordTextBox.Visibility = Visibility.Visible;

                MainPasswordBox.Focus();

                return false;
            }
            if (StartWorkingDateTextBox.Text == defaultStartWorkingDate || StartWorkingDateTextBox.Text == "")
            {
                MessageBox.Show("Укажите дату, когда сотрудник приступил к исполнению своих обязанностей.", "Ошибка");

                StartWorkingDateTextBox.Focus();

                return false;
            }
            if (StillWorkingCheckBox.IsChecked == false && (EndWorkingDateTextBox.Text == defaultEndWorkingDate || EndWorkingDateTextBox.Text == ""))
            {
                MessageBox.Show("Укажите дату, когда сотрудник покинул свою должность.", "Ошибка");

                EndWorkingDateTextBox.Focus();

                return false;
            }
            if (GeneralDirectorCheckBox.IsChecked == false && DirectorCheckBox.IsChecked == false && DeputyGeneralDirectorCheckBox.IsChecked == false && 
                HeadOfManagersCheckBox.IsChecked == false && HeadOfMarketersCheckBox.IsChecked == false && HeadOfEditorsCheckBox.IsChecked == false && 
                HeadOfAgentsCheckBox.IsChecked == false && HeadOfModeratorsCheckBox.IsChecked == false && HeadOfSpecialsCheckBox.IsChecked == false && 
                HeadOfTechniciansCheckBox.IsChecked == false && ManagerCheckBox.IsChecked == false && MarketerCheckBox.IsChecked == false && 
                EditorCheckBox.IsChecked == false && AgentCheckBox.IsChecked == false && ModeratorCheckBox.IsChecked == false && SpecialCheckBox.IsChecked == false)
            {
                MessageBox.Show("Укажите хотя бы одну должность для сотрудника.", "Ошибка");

                EditorCheckBox.Focus();

                return false;
            }
            if (DirectorCheckBox.IsChecked == true && DirectorsPositionTextBox.Text == defaultDirectorsPosition)
            {
                MessageBox.Show("Укажите полную должность директора.", "Ошибка");

                DirectorsPositionTextBox.Focus();

                return false;
            }
            if (ManagerCheckBox.IsChecked == true && ManagersPositionTextBox.Text == defaultManagersPosition)
            {
                MessageBox.Show("Укажите расширенную менеджерскую должность сотрудника.", "Ошибка");

                ManagersPositionTextBox.Focus();

                return false;
            }
            if (EditorCheckBox.IsChecked == true && 
                (UIElementsMethods.CheckingWhetherComboBoxHasDefaultValueInTheTemplatedListBox(EditorsInformationListBox, 1, 0, defaultEditorsRubric, "Укажите все недостающие рубрики либо удалите ненужные.") ||
                UIElementsMethods.FindTextOrNonNeutralsInTextBoxesOfTheTemplatedListBox(EditorsInformationListBox, 1, 1, defaultEditorsFrequency, true, "Для каждой рубрики должна быть указана её частота размещения. Заполните все недостающие поля либо удалите ненужные рубрики.")))
            {
                return false;
            }
            if (SpecialCheckBox.IsChecked == true &&
                (UIElementsMethods.CheckingWhetherComboBoxHasDefaultValueInTheTemplatedListBox(SpecialsInformationListBox, 1, 0, defaultSpecialProject, "Укажите все недостающие спецпроекты либо удалите ненужные.")))
            {
                return false;
            }
            if (AgentCheckBox.IsChecked == true && AgentsNumberTextBox.Text == defaultAgentsNumber)
            {
                MessageBox.Show("Укажите агентский номер сотрудника.", "Ошибка");

                AgentsNumberTextBox.Focus();

                return false;
            }
            if (AgentCheckBox.IsChecked == true && AgentsFirstWordsTextBox.Text == defaultAgentsFirstWords)
            {
                MessageBox.Show("Укажите приветствие сотрудника (как агента Поддержки).", "Ошибка");

                AgentsFirstWordsTextBox.Focus();

                return false;
            }
            if (AgentCheckBox.IsChecked == true && AgentsLastWordsTextBox.Text == defaultAgentsLastWords)
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
            if (IsDeveloperCheckBox.IsChecked == true && LightDeveloperRadioButton.IsChecked == true && 
                GeneralDirectorDeveloperCheckBox.IsChecked == false && DirectorDeveloperCheckBox.IsChecked == false && DeputyGeneralDirectorDeveloperCheckBox.IsChecked == false && 
                HeadOfManagersDeveloperCheckBox.IsChecked == false && HeadOfMarketersDeveloperCheckBox.IsChecked == false && HeadOfEditorsDeveloperCheckBox.IsChecked == false && 
                HeadOfAgentsDeveloperCheckBox.IsChecked == false && HeadOfModeratorsDeveloperCheckBox.IsChecked == false && HeadOfSpecialsDeveloperCheckBox.IsChecked == false && 
                HeadOfTechniciansDeveloperCheckBox.IsChecked == false && ManagerDeveloperCheckBox.IsChecked == false && MarketerDeveloperCheckBox.IsChecked == false && 
                EditorDeveloperCheckBox.IsChecked == false && AgentDeveloperCheckBox.IsChecked == false && ModeratorDeveloperCheckBox.IsChecked == false && SpecialDeveloperCheckBox.IsChecked == false)
            {
                MessageBox.Show("Выберите те категории управленческих должностей для сотрудника, расширенная информация по которым будет ему доступна как разработчику с базовым уровнем.", "Ошибка");

                EditorDeveloperCheckBox.Focus();

                return false;
            }

            return true;
        }


        private bool CheckingIfAllValuesAreValid()
        {
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
            if (PasswordTextBlock.Visibility == Visibility.Visible && (MainPasswordBox.Password.Length != 6 && MainPasswordBox.Password.Length != 2 || !int.TryParse(MainPasswordBox.Password, out int result)))
            {
                MessageBox.Show("Пароль обязательно должен быть шестизначным числом. Пожалуйста, измените пароль и воспроизведите его в поле ниже.", "Ошибка");

                MainPasswordBox.Password = "";
                RepeatPasswordBox.Password = "";

                RepeatPasswordBox.Visibility = Visibility.Hidden;
                RepeatPasswordTextBox.Visibility = Visibility.Visible;

                MainPasswordBox.Focus();

                return false;
            }
            if (PasswordTextBlock.Visibility == Visibility.Visible && MainPasswordBox.Password != RepeatPasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка");

                MainPasswordBox.Password = "";
                RepeatPasswordBox.Password = "";

                RepeatPasswordBox.Visibility = Visibility.Hidden;
                RepeatPasswordTextBox.Visibility = Visibility.Visible;

                MainPasswordBox.Focus();

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
            if (DateTime.Parse(StartWorkingDateTextBox.Text) > DateTime.Now)
            {
                MessageBox.Show("Некорректная дата. Этот день еще не наступил.", "Ошибка");

                StartWorkingDateTextBox.Text = "";
                StartWorkingDateTextBox.Focus();

                return false;
            }
            if (DateTime.Parse(StartWorkingDateTextBox.Text) < DateTime.Parse(foundationStringDate))
            {
                MessageBox.Show("Некорректная дата. В это время нашей компании еще не существовало.", "Ошибка");

                StartWorkingDateTextBox.Text = "";
                StartWorkingDateTextBox.Focus();

                return false;
            }
            if (StillWorkingCheckBox.IsChecked == false && DateTime.Parse(EndWorkingDateTextBox.Text) > DateTime.Now)
            {
                MessageBox.Show("Некорректная дата. Этот день еще не наступил.", "Ошибка");

                EndWorkingDateTextBox.Text = "";
                EndWorkingDateTextBox.Focus();

                return false;
            }
            if (StillWorkingCheckBox.IsChecked == false && DateTime.Parse(EndWorkingDateTextBox.Text) <= DateTime.Parse(StartWorkingDateTextBox.Text))
            {
                MessageBox.Show("Некорректные даты начала и окончания работы сотрудника. Вторая дата должна идти строго позднее первой.");

                StartWorkingDateTextBox.Focus();

                return false;
            }
            if (DirectorCheckBox.IsChecked == true && 
                _storage.Users.Items.Count(u => u != _user && u.WorkingNow && u.DirectorsPosition == DirectorsPositionTextBox.Text) != 0)
            {
                MessageBox.Show("Указанная директорская должность уже занята другим директором. Пожалуйста, проверьте корректность названия должности.", "Ошибка");

                DirectorsPositionTextBox.Text = "";
                DirectorsPositionTextBox.Focus();

                return false;
            }
            if (EditorCheckBox.IsChecked == true)
            { 
                if (_editorsPublications.Count() == 0)
                {
                    MessageBox.Show("Если сотрудник — редактор, то у него должна быть хотя бы одна рубрика.", "Ошибка");

                    return false;
                }
                if (UIElementsMethods.FindTextOrNonNeutralsInTextBoxesOfTheTemplatedListBox(EditorsInformationListBox, 1, 1, "Частота рубрики — это положительное число, обозначающее количество дней, которое даётся редактору для создания одного поста. Измените все невалидные значения."))
                {
                    return false;
                }
                if (!UIElementsMethods.CheckingSpecialIntegerConditions(EditorsInformationListBox, 1, 1, list => list.Count(num => num <= minimalEditorsFrequency) >= 1))
                {
                    if (MessageBox.Show($"В соответствии с действующими правилами, у каждого редактора хотя бы по одной из рубрик частота должна быть не меньше {minimalEditorsFrequency.ToString()}. Вы всё равно желаете продолжить?", "Предупреждение", MessageBoxButton.YesNoCancel) != MessageBoxResult.Yes)
                    {
                        UIElementsMethods.GetUIElementChildByNumberFromTemplatedListBox(EditorsInformationListBox, 0, 1, 1).Focus();

                        return false;
                    }                    
                }
            }
            if (SpecialCheckBox.IsChecked == true && _specialsProjects.Count() == 0)
            {                
                MessageBox.Show("Если сотрудник — спецредактор, то у него должен быть хотя бы один спецпроект.", "Ошибка");

                return false;                
            }
            if (AgentCheckBox.IsChecked == true && (!int.TryParse(AgentsNumberTextBox.Text, out result) || result <= 0))
            {
                MessageBox.Show("Номер Агента Поддержки должен всегда быть целым числом, большим 0.", "Ошибка");

                AgentsNumberTextBox.Text = "";
                AgentsNumberTextBox.Focus();

                return false;
            }
            if (AgentCheckBox.IsChecked == true && !(_storage.Users.Items.FirstOrDefault(u => u.AgentsNumber.ToString() == AgentsNumberTextBox.Text) == null ||
                _storage.Users.Items.FirstOrDefault(u => u.AgentsNumber.ToString() == AgentsNumberTextBox.Text) == _user))
            {
                MessageBox.Show("Вам нужен другой номер для Агента Поддержки, посколько указанный сейчас уже используется или использовался другим сотрудником.", "Ошибка");

                AgentsNumberTextBox.Text = "";
                AgentsNumberTextBox.Focus();

                return false;
            }
            if (StillWorkingCheckBox.IsChecked == false && IsDeveloperCheckBox.IsChecked == true && (MediumDeveloperRadioButton.IsChecked == true || HighDeveloperRadioButton.IsChecked == true))
            {
                MessageBox.Show("Неработающий сотрудник не может быть разработчиком с уровнем доступа выше базового.", "Ошибка");

                return false;
            }

            return true;
        }

        

        private void ListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (CapturedComboBox == null || CapturedComboBox.IsMouseDirectlyOver)
            {
                if (CapturedComboBox != null)
                {
                    CapturedComboBox.IsDropDownOpen = false;
                }

                Scroll.ScrollToVerticalOffset(Scroll.VerticalOffset - (double)e.Delta * 5 / 12);
            }
        }


        private void EditorsRubricComboBox_DropDownOpened(object sender, EventArgs e)
        {
            CapturedComboBox = sender as ComboBox;
        }

        private void EditorsRubricComboBox_DropDownClosed(object sender, EventArgs e)
        {
            CapturedComboBox = null;
        }

        private void SpecialsProjectComboBox_DropDownOpened(object sender, EventArgs e)
        {
            CapturedComboBox = sender as ComboBox;
        }

        private void SpecialsProjectComboBox_DropDownClosed(object sender, EventArgs e)
        {
            CapturedComboBox = null;
        }



        private void EditorsRubricComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count == 1)
            {
                ComboBox EditorsRubricComboBox = sender as ComboBox;

                EditorsPublication publication = EditorsRubricComboBox.DataContext as EditorsPublication;

                var rubric = EditorsRubricComboBox.SelectedItem as Rubric;

                publication.Rubric = rubric;
                publication.RubricID = rubric.Id;

                if (rubric.Id == -1) { publication.Rubric = null; }

                EditorsInformationListBox.ItemsSource = null;
                EditorsInformationListBox.ItemsSource = _editorsPublications;

                if (!rubric.Actual && rubric.Id != -1)
                {
                    MessageBox.Show("Рубрика является неактивной, и на данный момент отсутствует.", "Предупреждение");
                }
            }
        }

        private void SpecialsProjectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count == 1)
            {
                ComboBox SpecialsProjectComboBox = sender as ComboBox;

                EditorsPublication publication = SpecialsProjectComboBox.DataContext as EditorsPublication;

                var project = SpecialsProjectComboBox.SelectedItem as Rubric;

                publication.Rubric = project;
                publication.RubricID = project.Id;

                if (project.Id == -1) { publication.Rubric = null; }

                SpecialsInformationListBox.ItemsSource = null;
                SpecialsInformationListBox.ItemsSource = _specialsProjects;

                if (!project.Actual && project.Id != -1)
                {
                    MessageBox.Show("Спецпроект является на данный момент закрытым.", "Предупреждение");
                }
            }
        }



        private void HeadOfManagersCheckBox_Initialized(object sender, EventArgs e)
        {
            ManagersSpecification(sender);
        }

        private void ManagerCheckBox_Initialized(object sender, EventArgs e)
        {
            ManagersSpecification(sender);
        }

        private void HeadOfManagersDeveloperCheckBox_Initialized(object sender, EventArgs e)
        {
            ManagersSpecification(sender);
        }

        private void ManagerDeveloperCheckBox_Initialized(object sender, EventArgs e)
        {
            ManagersSpecification(sender);
        }

        private void ManagersSpecification(object sender)
        {
            if (_user.WorkingNow || _user.Id == -1) 
            {
                CheckBox checkBox = sender as CheckBox;

                checkBox.Visibility = Visibility.Collapsed;
            }
        }



        private bool CheckingWhetherUserDetailsButtonIsShown()
        {
            if (_personalPage && _user.CheckingWhichJobsDetailsCanBeDisplayedInUserDetailsWindow(null, out _))
            { 
                return true; 
            }

            else if (!_personalPage && _user.CheckingWhichJobsDetailsCanBeDisplayedInUserDetailsWindow(_userWhoWatches, out _))
            {
                return true;
            }

            return false;
        }
    }
}
