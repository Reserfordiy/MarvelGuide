﻿using MarvelGuide.Core.Intefraces;
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
    /// Логика взаимодействия для ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        private const string adding = ":  ";
        private const string splitting = "; ";

        private const string login = "Логин в системе";
        private const string name = "Ф.И. сотрудника";
        private const string job = "Должность";

        private const string creator = "Создатель";
        private const string superAdmin = "Главный администратор";
        private const string adminEditor = "Исполнительный администратор";
        private const string adminAgent = "Директор Поддержки";
        private const string manager = "Менеджер";
        private const string editor = "Редактор";
        private const string agent = "Агент Поддержки";
        private const string moderator = "Модератор";

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
        private const string editorsFrequencyStart = "Частота размещения постов: 1/";
        private const string editorsFrequencyEnd = " поста в сутки";

        private const string agentsNumber = "Агентский номер";
        private const string agentsFirstWords = "Приветствие агента";
        private const string agentsLastWords = "Подпись агента";


        private const string showDetailsButton = "Показать подробности";
        private const string hideDetailsButton = "Скрыть подробности";


        private const string defaultImageSource = "default.jpg";


        IStorage _storage;
        User _user;

        List<string> _personalData;

        string _job;
        int _amountOfRegularJobs;

        bool _shown = false;
        int _additionalData = 0;

        public static string Employees => employees;

        public ProfileWindow(User user, IStorage storage)
        {
            _storage = storage;
            _user = user;

            InitializeComponent();

            FormingPersonalData();
        }


        private void FormingPersonalData()
        { 
            _personalData = new List<string>
            {
                login + adding + _user.Login,
                name + adding + _user.Surname + " " + _user.Name
            };
             
            if (_user.Creator) { _job += splitting + creator; }
            if (_user.SuperAdmin) { _job += splitting + superAdmin; }
            if (_user.AdminEditor) { _job += splitting + adminEditor; }
            if (_user.AdminAgent) { _job += splitting + adminAgent; }
            if (_user.Manager) { _job += splitting + manager; _amountOfRegularJobs++; }
            if (_user.Editor) { _job += splitting + editor; _amountOfRegularJobs++; }
            if (_user.Agent) { _job += splitting + agent; _amountOfRegularJobs++; }
            if (_user.Moderator) { _job += splitting + moderator; _amountOfRegularJobs++; }

            if (_job[0] == splitting[0])
            {
                _job = _job.Remove(0, splitting.Count());
            }

            _personalData.Add(job + adding + _job);

            PersonalDataListBox.ItemsSource = _personalData;
        }



        private void CharacteristicTextBlock_Initialized(object sender, EventArgs e)
        {
            TextBlock CharacteristicTextBlock = sender as TextBlock;

            string characteristic = CharacteristicTextBlock.DataContext as string;

            CharacteristicTextBlock.Text = characteristic;
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();

            mainWindow.Show();
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
            if (!_shown)
            {
                _shown = true;

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
                _shown = false;

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
            _personalData.Add(editorsFrequencyStart + _user.EditorsFrequency.ToString() + editorsFrequencyEnd);

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
    }
}
