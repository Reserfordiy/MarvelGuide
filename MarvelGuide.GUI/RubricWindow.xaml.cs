using MarvelGuide.Core;
using MarvelGuide.Core.Helpers;
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
    /// Логика взаимодействия для RubricWindow.xaml
    /// </summary>
    public partial class RubricWindow : Window
    {
        private const string defaultImageSource = "default.jpg";
        private const string defaultDarkImageSource = "default_d.jpg";

        private const string defaultName = "Пример: Фильмарты";


        IStorage _storage;


        Rubric _rubric;

        User _userWhoWatches;


        Picture _picture = new Picture();
        Picture _pictureDark = new Picture();


        bool _ifDarkThemeIsSwitchedOn = false;



        public RubricWindow(Rubric rubric, User userWhoWatches)
        {
            _storage = Factory.Instance.GetStorage();

            _rubric = rubric;
            _userWhoWatches = userWhoWatches;

            if (_rubric.Id != -1)
            {
                _picture = _rubric.Picture;
                _pictureDark = _rubric.PictureDark;
            }
            
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
            InitializingTheImageSource(_picture.ImageSource, defaultImageSource, "../MarvelGuide.Core/Rubrics", PictureImage);
        }


        private void SwitchTheDesignButton_Click(object sender, RoutedEventArgs e)
        {
            if (_ifDarkThemeIsSwitchedOn)
            {
                InitializingTheImageSource(_picture.ImageSource, defaultImageSource, "../MarvelGuide.Core/Rubrics", PictureImage);

                RubricNameTextBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                InitializingTheImageSource(_pictureDark.ImageSource, defaultDarkImageSource, "../MarvelGuide.Core/Rubrics", PictureImage);

                RubricNameTextBlock.Visibility = Visibility.Visible;
            }

            _ifDarkThemeIsSwitchedOn = !_ifDarkThemeIsSwitchedOn;
        }


        private void InitializingTheImageSource(string regularSource, string defaultSource, string folder, Image Image)
        {
            try
            {
                Image.Source = new BitmapImage(new Uri(WorkWithImages.GetDestinationPath(regularSource, folder)));
            }
            catch
            {
                try
                {
                    Image.Source = new BitmapImage(new Uri(WorkWithImages.GetDestinationPath(defaultSource, folder)));
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
            InitializingTheImageSource(_picture.ImageSource, defaultImageSource, "../MarvelGuide.Core/Rubrics", UploadPictureImage);
        }

        private void UploadDarkPictureImage_Initialized(object sender, EventArgs e)
        {
            InitializingTheImageSource(_pictureDark.ImageSource, defaultDarkImageSource, "../MarvelGuide.Core/Rubrics", UploadDarkPictureImage);
        }



        private void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            Picture picture = UploadingImage(UploadPictureImage);

            if (picture != null)
            {
                _picture = picture;

                if (!_ifDarkThemeIsSwitchedOn)
                {
                    InitializingTheImageSource(_picture.ImageSource, defaultImageSource, "../MarvelGuide.Core/Rubrics", PictureImage);
                }
            }            
        }

        private void UploadDarkImageButton_Click(object sender, RoutedEventArgs e)
        {
            Picture picture = UploadingImage(UploadDarkPictureImage);

            if (picture != null)
            {
                _pictureDark = picture;

                if (_ifDarkThemeIsSwitchedOn)
                {
                    InitializingTheImageSource(_pictureDark.ImageSource, defaultDarkImageSource, "../MarvelGuide.Core/Rubrics", PictureImage);
                }
            }
        }


        private Picture UploadingImage(Image Image)
        {
            try
            {
                WorkWithImages imageUploadingProcess = new WorkWithImages();

                imageUploadingProcess.UploadImageAndSave("../MarvelGuide.Core/Rubrics");

                Picture picture = imageUploadingProcess.Picture;

                Image.Source = new BitmapImage(new Uri(WorkWithImages.GetDestinationPath(picture.ImageSource, "../MarvelGuide.Core/Rubrics")));

                return picture;
            }
            catch { return null; }
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
            if (CheckingWhetherAllFieldsFilledCorrectly())
            {
                FixingDataAboutUser();

                _storage.Rubrics.Save();

                Close();
            }
        }

        private void FixingDataAboutUser()
        {
            if (_rubric.Id == -1)
            {
                _storage.Rubrics.Add(_rubric);

                int id = _storage.Rubrics.Items.Max(rubr => rubr.Id);

                _rubric.Id = id + 1;
            }

            _rubric.Name = NameTextBox.Text;

            _rubric.Picture = _picture;
            _rubric.PictureDark = _pictureDark;
        }

        private bool CheckingWhetherAllFieldsFilledCorrectly()
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
            if (_picture.ImageSource == null && _pictureDark.ImageSource == null)
            {
                if (MessageBox.Show("Изображения для рубрики не загружены. Вы уверены, что хотите продолжить?", "Предупреждение", MessageBoxButton.YesNoCancel) != MessageBoxResult.Yes)
                {
                    return false;
                }
            }
            else if (_pictureDark.ImageSource == null)
            {
                MessageBox.Show("В системе не может быть одинарных версий изображений. Загрузите затемненное изображение либо удалите основное.", "Ошибка");

                return false;
            }
            else if (_picture.ImageSource == null)
            {
                MessageBox.Show("В системе не может быть одинарных версий изображений. Загрузите основное изображение либо удалите затемненное.", "Ошибка");

                return false;
            }

            return true;
        }
    }
}
