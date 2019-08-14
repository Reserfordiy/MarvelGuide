using MarvelGuide.Core;
using MarvelGuide.Core.Helpers;
using MarvelGuide.Core.Intefraces;
using MarvelGuide.Core.Models;
using MarvelGuide.GUI.Helpers;
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
    /// Логика взаимодействия для RubricWindow.xaml
    /// </summary>
    public partial class RubricWindow : Window
    {
        private const string defaultImageSource = "default.jpg";
        private const string defaultDarkImageSource = "default_d.jpg";

        private const string imageFolder = "../MarvelGuide.Core/Rubrics";

        private const string defaultName = "Пример: Фильмарты";
        private const string defaultDocument = "Выберите документ";


        IStorage _storage;
        
        Rubric _rubric;

        User _userWhoWatches;

        Picture _picture = new Picture();
        Picture _pictureDark = new Picture();

        Document _unselectedDocument;


        bool _closingBySaveButton = false;

        bool _ifDarkThemeIsSwitchedOn = false;

        bool _newImagesWereUploaded = false;



        public RubricWindow(Rubric rubric, User userWhoWatches)
        {
            _storage = Factory.Instance.GetStorage();

            _rubric = rubric;
            _userWhoWatches = userWhoWatches;

            if (_rubric.Id != -1)
            {
                _picture = _rubric.Picture ?? new Picture();
                _pictureDark = _rubric.PictureDark ?? new Picture();
            }

            _unselectedDocument = new Document
            {
                Id = -1,
                Name = defaultDocument
            };

            InitializeComponent();

            FormingTheEdittingData();

            WindowState = WindowState.Maximized;
        }


        private void FormingTheEdittingData()
        {
            if (_rubric.Id != -1)
            {
                NameTextBox.Text = _rubric.Name;
                NameTextBox.Foreground = Brushes.Black;
            }
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_closingBySaveButton)
            {
                if (_rubric.Id == -1)
                {
                    if (_newImagesWereUploaded)
                    {
                        if (MessageBox.Show("Все изменения будут потеряны. Вы уверены, что хотите выйти?", "Предупреждение", MessageBoxButton.YesNoCancel) != MessageBoxResult.Yes)
                        {
                            e.Cancel = true;

                            return;
                        }
                        else
                        {
                            try { DeleteImage(_picture.ImageSource); } catch { }
                            try { DeleteImage(_pictureDark.ImageSource); } catch { }
                        }
                    }
                }

                else
                {
                    if (CheckingImagedData())
                    {
                        FixingImagedDataAboutUser();

                        _storage.Rubrics.Save();
                    }
                    else
                    {
                        e.Cancel = true;

                        return;
                    }
                }
            }

            AllRubricsWindow allRubricsWindow = new AllRubricsWindow(_userWhoWatches);

            allRubricsWindow.Show();
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void RubricNameTextBlock_Initialized(object sender, EventArgs e)
        {
            RubricNameTextBlock.Text = _rubric.Name;

            RubricNameTextBlock.Visibility = Visibility.Hidden;
        }


        private void PictureImage_Initialized(object sender, EventArgs e)
        {
            InitializingTheImageSource(_picture.ImageSource, defaultImageSource, imageFolder, PictureImage);
        }


        private void SwitchTheDesignButton_Click(object sender, RoutedEventArgs e)
        {
            if (_ifDarkThemeIsSwitchedOn)
            {
                InitializingTheImageSource(_picture.ImageSource, defaultImageSource, imageFolder, PictureImage);

                RubricNameTextBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                InitializingTheImageSource(_pictureDark.ImageSource, defaultDarkImageSource, imageFolder, PictureImage);

                RubricNameTextBlock.Visibility = Visibility.Visible;
            }

            _ifDarkThemeIsSwitchedOn = !_ifDarkThemeIsSwitchedOn;
        }


        private void InitializingTheImageSource(string regularSource, string defaultSource, string folder, Image Image)
        {
            try
            {
                Image.Source = UIElementsMethods.InitializingBitmapImage(regularSource, folder);
            }
            catch
            {
                try
                {
                    Image.Source = UIElementsMethods.InitializingBitmapImage(defaultSource, folder);
                }
                catch { }
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



        private void UploadPictureImage_Initialized(object sender, EventArgs e)
        {
            InitializingTheImageSource(_picture.ImageSource, defaultImageSource, imageFolder, UploadPictureImage);
        }

        private void UploadDarkPictureImage_Initialized(object sender, EventArgs e)
        {
            InitializingTheImageSource(_pictureDark.ImageSource, defaultDarkImageSource, imageFolder, UploadDarkPictureImage);
        }



        private void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            Picture picture = UploadingImage(UploadPictureImage);

            if (picture != null)
            {
                _newImagesWereUploaded = true;

                Picture pictureForDeleting = _picture;

                _picture = picture;

                if (!_ifDarkThemeIsSwitchedOn)
                {
                    InitializingTheImageSource(_picture.ImageSource, defaultImageSource, imageFolder, PictureImage);
                }

                try
                {
                    DeleteImage(pictureForDeleting.ImageSource);
                }
                catch { }
            }            
        }

        private void UploadDarkImageButton_Click(object sender, RoutedEventArgs e)
        {
            Picture picture = UploadingImage(UploadDarkPictureImage);

            if (picture != null)
            {
                _newImagesWereUploaded = true;

                Picture pictureForDeleting = _pictureDark;

                _pictureDark = picture;

                if (_ifDarkThemeIsSwitchedOn)
                {
                    InitializingTheImageSource(_pictureDark.ImageSource, defaultDarkImageSource, imageFolder, PictureImage);
                }

                try
                {
                    DeleteImage(pictureForDeleting.ImageSource);
                }
                catch { }
            }
        }


        private Picture UploadingImage(Image Image)
        {
            try
            {
                WorkWithImages imageUploadingProcess = new WorkWithImages();

                imageUploadingProcess.UploadImageAndSave(imageFolder);

                Picture picture = imageUploadingProcess.Picture;

                Image.Source = UIElementsMethods.InitializingBitmapImage(picture.ImageSource, imageFolder);

                return picture;
            }
            catch { return null; }
        }

        private void DeleteImage(string imageSource)
        {
            WorkWithImages deletingProcess = new WorkWithImages();

            deletingProcess.DeleteImage(imageFolder, imageSource);
        }



        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameTextBox.Text == defaultName)
            {
                RubricNameTextBlock.Text = _rubric.Name;
            }
            else
            {
                RubricNameTextBlock.Text = NameTextBox.Text;
            }
        }



        private void SaveDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckingWhetherNonImagedInformationIsFilledCorrectly() && CheckingImagedData())
            {
                FixingNonImagedDataAboutUser();
                FixingImagedDataAboutUser();

                _storage.Rubrics.Save();

                _closingBySaveButton = true;

                Close();
            }
        }


        private void FixingNonImagedDataAboutUser()
        {
            if (_rubric.Id == -1)
            {
                _storage.Rubrics.Add(_rubric);

                int id = _storage.Rubrics.Items.Max(rubr => rubr.Id);

                _rubric.Id = id + 1;
            }

            _rubric.Name = NameTextBox.Text;
        }

        private void FixingImagedDataAboutUser()
        {
            _rubric.Picture = _picture;
            _rubric.PictureDark = _pictureDark;
        }


        private bool CheckingWhetherNonImagedInformationIsFilledCorrectly()
        {
            if (NameTextBox.Text == defaultName)
            {
                MessageBox.Show("Укажите название для рубрики.", "Ошибка");

                NameTextBox.Focus();

                return false;
            }
            if (_storage.Rubrics.Items.Count(rubr => rubr != _rubric && rubr.Name == NameTextBox.Text) > 0)
            {
                MessageBox.Show("В системе уже существует рубрика с таким названием.", "Ошибка");

                NameTextBox.Text = "";
                NameTextBox.Focus();

                return false;
            }            

            return true;
        }

        private bool CheckingImagedData()
        {
            if (_picture.ImageSource == null && _pictureDark.ImageSource == null)
            {
                if (MessageBox.Show("Изображения для рубрики не загружены. Вы уверены, что хотите продолжить?", "Предупреждение", MessageBoxButton.YesNoCancel) != MessageBoxResult.Yes)
                {
                    return false;
                }
            }
            else if (_pictureDark.ImageSource == null)
            {
                if (MessageBox.Show("Затемненное изображение не загружено. Вы уверены, что хотите продолжить?", "Предупреждение", MessageBoxButton.YesNoCancel) != MessageBoxResult.Yes)
                {
                    return false;
                }
            }
            else if (_picture.ImageSource == null)
            {
                if (MessageBox.Show("Осветленное изображение не загружено. Вы уверены, что хотите продолжить?", "Предупреждение", MessageBoxButton.YesNoCancel) != MessageBoxResult.Yes)
                {
                    return false;
                }
            }

            return true;
        }



        private void DocumentComboBox_Initialized(object sender, EventArgs e)
        {
            var startDocuments = new List<Document> { _unselectedDocument };

            if (_rubric.Document != null)
            {
                startDocuments = startDocuments.Concat(new List<Document> { _rubric.Document }).ToList();
            }

            DocumentComboBox.ItemsSource = startDocuments
                .Concat(_storage.Documents.Items
                    .Where(doc => doc.Id != _rubric.DocumentId)
                    .OrderByDescending(doc => doc.Versions[doc.Versions.Count - 1].Date));

            if (_rubric.Document == null) { DocumentComboBox.SelectedIndex = 0; }
            else { DocumentComboBox.SelectedIndex = 1; }
        }
    }
}
