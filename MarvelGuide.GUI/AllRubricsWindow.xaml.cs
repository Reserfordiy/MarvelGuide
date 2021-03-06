﻿using MarvelGuide.Core;
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
    /// Логика взаимодействия для AllRubricsWindow.xaml
    /// </summary>
    public partial class AllRubricsWindow : Window
    {
        private const string defaultImageSource = "default.jpg";
        private const string defaultDarkImageSource = "default_d.jpg";

        private const string imageFolder = "../MarvelGuide.Core/Rubrics";


        IStorage _storage;


        User _userWhoWatches;

        List<Rubric> _rubrics1;
        List<Rubric> _rubrics2;

        List<Rubric> _specialProjects1;
        List<Rubric> _specialProjects2;


        bool _showingTheRubric = false;

        Rubric _rubricShown = null;        



        public AllRubricsWindow(User userWhoWatches)
        {
            _storage = Factory.Instance.GetStorage();

            _userWhoWatches = userWhoWatches;

            InitializeComponent();

            var ListBoxes = new ListBox[]
            {
                RubricsListBox1,
                RubricsListBox2,
                OldRubricsListBox1,
                OldRubricsListBox2
            };                

            for (int i = 0; i < ListBoxes.Length; i+=2)
            {
                _rubrics1 = new List<Rubric>();
                _rubrics2 = new List<Rubric>();

                if (i == 0) { FormingTheRubricsListBoxSource(ListBoxes[i], ListBoxes[i + 1], rubr => rubr.Actual); }
                else { FormingTheRubricsListBoxSource(ListBoxes[i], ListBoxes[i + 1], rubr => !rubr.Actual); }
            }

            _specialProjects1 = new List<Rubric>();
            _specialProjects2 = new List<Rubric>();

            FormingTheSpecialProjectsListBoxSource();

            WindowState = WindowState.Maximized;
        }


        private void FormingTheRubricsListBoxSource(ListBox listBox1, ListBox listBox2, Func<Rubric, bool> func)
        {
            int i = 0;

            foreach (var rubric in _storage.Rubrics.Items
                .Where(rubr => func(rubr) && !rubr.SpecialProject)
                .OrderByDescending(rubr => _storage.Users.Items.Count(u => u.Editor && u.EditorsRubrics.Exists(edPub => edPub.Rubric == rubr) && u.WorkingNow))
                .ThenByDescending(rubr => _storage.Users.Items.Count(u => u.Editor && u.EditorsRubrics.Exists(edPub => edPub.Rubric == rubr)))
                .ThenBy(rubr => rubr.Name))
            {
                if (i % 2 == 0) { _rubrics1.Add(rubric); }
                else { _rubrics2.Add(rubric); }

                i += 1;
            }

            listBox1.ItemsSource = _rubrics1;
            listBox2.ItemsSource = _rubrics2;
        }

        private void FormingTheSpecialProjectsListBoxSource()
        {
            int i = 0;

            foreach (var rubric in _storage.Rubrics.Items
                .Where(rubr => rubr.SpecialProject))
            {
                if (i % 2 == 0) { _specialProjects1.Add(rubric); }
                else { _specialProjects2.Add(rubric); }

                i += 1;
            }

            SpecialProjectsListBox1.ItemsSource = _specialProjects1;
            SpecialProjectsListBox2.ItemsSource = _specialProjects2;
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_showingTheRubric)
            {
                RubricWindow rubricWindow = new RubricWindow(_rubricShown, _userWhoWatches);

                rubricWindow.Show();
            }
            else
            {
                ProfileWindow profileWindow = new ProfileWindow(_userWhoWatches);

                profileWindow.Show();
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                RubricsListBox1.Margin = new Thickness(0, 0, 50 * Width * Width / (1200 * 1200), RubricsListBox1.Margin.Bottom);
                RubricsListBox2.Margin = new Thickness(50 * Width * Width / (1200 * 1200), 0, 0, RubricsListBox2.Margin.Bottom);
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                RubricsListBox1.Margin = new Thickness(0, 0, 50 * MaxWidth * MaxWidth / (1200 * 1200), 55);
                RubricsListBox2.Margin = new Thickness(50 * MaxWidth * MaxWidth / (1200 * 1200), 0, 0, 55);
            }
            else
            {
                RubricsListBox1.Margin = new Thickness(0, 0, 50 * MaxWidth * MaxWidth / (1200 * 1200), 90);
                RubricsListBox2.Margin = new Thickness(50 * MaxWidth * MaxWidth / (1200 * 1200), 0, 0, 90);
            }
        }



        private void RubricNameTextBlock_Initialized(object sender, EventArgs e)
        {
            TextBlock RubricNameTextBlock = sender as TextBlock;

            Rubric rubric = RubricNameTextBlock.DataContext as Rubric;

            RubricNameTextBlock.Text = rubric.Name;

            if (rubric.Name.Split().Length > 1)
            {
                RubricNameTextBlock.FontSize = 26;
            }
        }


        private void PictureImage_Initialized(object sender, EventArgs e)
        {
            Image PictureImage = sender as Image;

            Rubric rubric = PictureImage.DataContext as Rubric;

            InitializingTheImageSource((rubric.PictureDark ?? new Picture()).ImageSource, defaultDarkImageSource, imageFolder, PictureImage);      
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid Grid = sender as Grid;
            Image PictureImage = (Grid.Children[0] as Border).Child as Image;

            Rubric rubric = Grid.DataContext as Rubric;

            InitializingTheImageSource((rubric.Picture ?? new Picture()).ImageSource, defaultImageSource, imageFolder, PictureImage);

            Cursor = Cursors.Hand;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid Grid = sender as Grid;
            Image PictureImage = (Grid.Children[0] as Border).Child as Image;

            Rubric rubric = Grid.DataContext as Rubric;

            InitializingTheImageSource((rubric.PictureDark ?? new Picture()).ImageSource, defaultDarkImageSource, imageFolder, PictureImage);

            Cursor = Cursors.Arrow;
        }

        private void InitializingTheImageSource(string regularSource, string defaultSource, string folder, Image PictureImage)
        {
            try
            {
                PictureImage.Source = UIElementsMethods.InitializingBitmapImage(regularSource, folder);
            }
            catch
            {
                try
                {
                    PictureImage.Source = UIElementsMethods.InitializingBitmapImage(defaultSource, folder);
                }
                catch { }
            }
        }



        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid Grid = sender as Grid;

            Rubric rubric = Grid.DataContext as Rubric;

            _showingTheRubric = true;

            _rubricShown = rubric;

            Close();
        }

        private void AddRubricButton_Click(object sender, RoutedEventArgs e)
        {
            _showingTheRubric = true;
            
            _rubricShown = new Rubric { Id = -1 };

            Close();
        }



        private void ListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Scroll.ScrollToVerticalOffset(Scroll.VerticalOffset - (double)e.Delta * 5 / 12);
        }
    }
}
