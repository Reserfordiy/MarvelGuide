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
        private const string editorsDocument = "Инструкции для редакторов";

        private const string rubricName = "";
        private const string rubricFrequency = "Частота размещения: ";


        IStorage _storage;

        User _user;
        User _userWhoWatches;

        List<EditorsPublicationForVisualization> _publicationsForVisualization;


        Document _documentForReading = null;

        bool _goingToRead = false;        



        public UserDetailsWindow(User user, User userWhoWatches)
        {
            _storage = Factory.Instance.GetStorage();

            _user = user;
            _userWhoWatches = userWhoWatches;

            InitializeComponent();

            if (_user.EditorsRubrics.Count() > 1)
            {
                WindowState = WindowState.Maximized;
            }

            FormingEditorsData();            
        }

        public UserDetailsWindow(User user) : this(user, null) { }


        private void FormingEditorsData()
        {
            if (_user.Editor)
            {
                EditorsDocumentsListBox.ItemsSource = new Document[]
                {
                _storage.Documents.Items.FirstOrDefault(doc => doc.Name == editorsDocument)
                };

                _publicationsForVisualization = _user.EditorsRubrics
                    .Select(edPub => new EditorsPublicationForVisualization() { EditorsPublication = edPub, ShowDetails = false }).ToList();

                EditorsRubricsListBox.ItemsSource = _publicationsForVisualization;
            }
            else
            {
                EditorsDetailsGrid.Visibility = Visibility.Collapsed;
            }
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_goingToRead)
            {
                DocumentWindow documentWindow = new DocumentWindow(_documentForReading, false, false, true, _user, _userWhoWatches);

                documentWindow.Show();                
            }
            else
            {
                ProfileWindow profileWindow = new ProfileWindow(_user, _userWhoWatches, false);

                profileWindow.Show();
            }            
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void EditorsDocumentTextBlock_Initialized(object sender, EventArgs e)
        {
            TextBlock DocumentNameTextBlock = sender as TextBlock;

            Document document = DocumentNameTextBlock.DataContext as Document;

            DocumentNameTextBlock.Text = document.Name;
        }


        private void RubricNameTextBlock_Initialized(object sender, EventArgs e)
        {
            TextBlock RubricNameTextBlock = sender as TextBlock;

            var publicationForVisualization = RubricNameTextBlock.DataContext as EditorsPublicationForVisualization;

            RubricNameTextBlock.Text = rubricName + publicationForVisualization.EditorsPublication.Rubric.Name;
        }

        private void RubricFrequencyTextBlock_Initialized(object sender, EventArgs e)
        {
            TextBlock RubricFrequencyTextBlock = sender as TextBlock;

            var publicationForVisualization = RubricFrequencyTextBlock.DataContext as EditorsPublicationForVisualization;

            RubricFrequencyTextBlock.Text = rubricFrequency + publicationForVisualization.EditorsPublication.StringFrequency();
        }


        private void DocumentGrid_Initialized(object sender, EventArgs e)
        {
            Grid DocumentGrid = sender as Grid;

            var publicationForVisualization = DocumentGrid.DataContext as EditorsPublicationForVisualization;

            if (publicationForVisualization.EditorsPublication.Rubric.DocumentId == 0)
            {
                DocumentGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void RubricCanonTextBlock_Initialized(object sender, EventArgs e)
        {
            TextBlock RubricCanonTextBlock = sender as TextBlock;

            var publicationForVisualization = RubricCanonTextBlock.DataContext as EditorsPublicationForVisualization;

            var documentName = "<Документ отсутствует>";

            if (publicationForVisualization.EditorsPublication.Rubric.DocumentId != 0)
            {
                documentName = publicationForVisualization.EditorsPublication.Rubric.Document.Name;
            }

            RubricCanonTextBlock.Text = documentName;
        }



        private void ReadDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            Button ReadButton = sender as Button;

            _documentForReading = ReadButton.DataContext as Document;
            
            _goingToRead = true;

            Close();            
        }


        private void ReadCanonButton_Click(object sender, RoutedEventArgs e)
        {
            Button ReadButton = sender as Button;

            var publicationForVisualization = ReadButton.DataContext as EditorsPublicationForVisualization;

            _documentForReading = publicationForVisualization.EditorsPublication.Rubric.Document;

            _goingToRead = true;

            Close();
        }

        private void ShowDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("К сожалению, пока раздел детализации недоступен, однако в скором времени здесь будет статистика обо всех публикациях, включая среднее число лайков, репостов, комментов на постах, общее число постов, ссылки на наиболее популярные посты, а также многое-многое другое! Следите за обновлениями!", "Предупреждение");
        }



        private void ListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Scroll.ScrollToVerticalOffset(Scroll.VerticalOffset - (double)e.Delta * 5 / 12);
        }        
    }
}
