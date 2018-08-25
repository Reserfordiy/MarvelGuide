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

        private const string allEmployeesStart = "Работают под подчинением ";
        private const string creatorsEmployeesEnd = " сотрудников. Среди них:";

        private const string employer = "Непосредственный начальник";

        private const string employeeManagers = "Менеджеров";
        private const string employeeEditors = "Редакторов";
        private const string employeeAgents = "Агентов Поддержки";
        private const string employeeModerators = "Модераторов";

        private const string managerJob = "Менеджерская должность";

        private const string editorsRubric = "Редакторская рубрика";
        private const string editorsFrequencyStart = "Частота размещения постов: 1/";
        private const string editorsFrequencyEnd = " поста в сутки";

        private const string agentsNumber = "Номер агента";
        private const string agentsFirstWords = "Приветствие агента";
        private const string agentsLastWords = "Подпись агента";


        private const string showDetailsButton = "Показать подробности";
        private const string hideDetailsButton = "Скрыть подробности";


        private const string defaultImageSource = "default.jpg";


        IStorage _storage;
        User _user;

        List<string> _personalData;

        string _job;

        bool _shown = false;
        int _additionalData = 0;


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
            if (_user.Manager) { _job += splitting + manager; }
            if (_user.Editor) { _job += splitting + editor; }
            if (_user.Agent) { _job += splitting + agent; }
            if (_user.Moderator) { _job += splitting + moderator; }

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
            _personalData.Add(allEmployeesStart + _storage.Users.Items.Count().ToString() + creatorsEmployeesEnd);
            _personalData.Add(employeeManagers + adding + _storage.Users.Items.Count(u => u.Manager).ToString());
            _personalData.Add(employeeEditors + adding + _storage.Users.Items.Count(u => u.Editor).ToString());
            _personalData.Add(employeeAgents + adding + _storage.Users.Items.Count(u => u.Agent).ToString());
            _personalData.Add(employeeModerators + adding + _storage.Users.Items.Count(u => u.Moderator).ToString());

            _additionalData += 5;
        }

        private void SuperAdminsDetails()
        {
            _personalData.Add(allEmployeesStart + _storage.Users.Items.Count(u => u.Manager).ToString() + " " + employeeManagers);

            _additionalData++;
        }

        private void AdminEditorDetails()
        {
            _personalData.Add(allEmployeesStart + _storage.Users.Items.Count(u => u.Editor).ToString() + " " + employeeEditors);

            _additionalData++;
        }

        private void AdminAgentDetails()
        {
            _personalData.Add(allEmployeesStart + _storage.Users.Items.Count(u => u.Agent).ToString() + " " + employeeAgents);

            _additionalData++;
        }

        private void ManagersDetails()
        {
            _personalData.Add(managerJob + adding + _user.ManagersRole);

            _additionalData++;

            if (!_user.Creator && !_user.SuperAdmin && !_user.AdminAgent && !_user.AdminEditor)
            {
                _personalData.Add(employer + adding + _storage.Users.Items.FirstOrDefault(u => u.SuperAdmin).Name + " " + _storage.Users.Items.FirstOrDefault(u => u.SuperAdmin).Surname);

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
                _personalData.Add(employer + adding + _storage.Users.Items.FirstOrDefault(u => u.AdminEditor).Name + " " + _storage.Users.Items.FirstOrDefault(u => u.AdminEditor).Surname);

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
                _personalData.Add(employer + adding + _storage.Users.Items.FirstOrDefault(u => u.AdminAgent).Name + " " + _storage.Users.Items.FirstOrDefault(u => u.AdminAgent).Surname);

                _additionalData++;
            }
        }

        private void ModeratorsDetails()
        {
            _additionalData += 0;
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
