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


        User _user;

        List<string> _personalData;

        string _job;


        public ProfileWindow(User user)
        {
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

        
    }
}
