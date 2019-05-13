using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MarvelGuide.GUI.Helpers
{
    public class UIElementsMethods
    {
        public static UIElement GetUIElementChildByNumberFromTemplatedListBox(ListBox listBox, int listBoxIndex, int transitionGridNumber, int childNumber)
        {
            ListBoxItem itemContainer = listBox.ItemContainerGenerator.ContainerFromIndex(listBoxIndex) as ListBoxItem;

            Border itemContainerBorder = VisualTreeHelper.GetChild(itemContainer, 0) as Border;
            ContentPresenter itemContainerContentPresenter = VisualTreeHelper.GetChild(itemContainerBorder, 0) as ContentPresenter;
            Grid itemUIParent = VisualTreeHelper.GetChild(itemContainerContentPresenter, 0) as Grid;
            Grid transitionGrid = itemUIParent.Children[transitionGridNumber] as Grid;

            return transitionGrid.Children[childNumber];
        }



        public static bool CheckingWhetherComboBoxHasDefaultValueInTheTemplatedListBox(ListBox listBox, int transitionGridNumber, int childNumber, string wantedStringValue, string messageBoxText)
        {
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                ComboBox ComboBox = GetUIElementChildByNumberFromTemplatedListBox(listBox, i, transitionGridNumber, childNumber) as ComboBox;

                if (ComboBox.Text.ToUpperInvariant() == wantedStringValue.ToUpperInvariant())
                {
                    MessageBox.Show(messageBoxText, "Ошибка!");

                    ComboBox.Focus();

                    return true;
                }
            }

            return false;
        }


        public static bool FindTextOrNonNeutralsInTextBoxesOfTheTemplatedListBox(ListBox listBox, int transitionGridNumber, int childNumber, string wantedStringValue, bool searchingText, string messageBoxText)
        {
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                TextBox TextBox = GetUIElementChildByNumberFromTemplatedListBox(listBox, i, transitionGridNumber, childNumber) as TextBox;

                if (searchingText && TextBox.Text.ToUpperInvariant() == wantedStringValue.ToUpperInvariant() 
                    || !searchingText && (!int.TryParse(TextBox.Text, out int r) || int.Parse(TextBox.Text) <= 0))
                {                    
                    MessageBox.Show(messageBoxText, "Ошибка!");

                    TextBox.Focus();

                    return true;                    
                }
            }

            return false;
        }

        public static bool FindTextOrNonNeutralsInTextBoxesOfTheTemplatedListBox(ListBox listBox, int transitionGridNumber, int childNumber, string messageBoxText)
        {
            return FindTextOrNonNeutralsInTextBoxesOfTheTemplatedListBox(listBox, transitionGridNumber, childNumber, null, false, messageBoxText);
        }



        public static bool CheckingSpecialIntegerConditions(ListBox listBox, int transitionGridNumber, int childNumber, Func<List<int>, bool> specialCondition)
        {
            List<int> list = new List<int>();

            for (int i = 0; i < listBox.Items.Count; i++)
            {
                TextBox TextBox = GetUIElementChildByNumberFromTemplatedListBox(listBox, i, transitionGridNumber, childNumber) as TextBox;

                list.Add(int.Parse(TextBox.Text));
            }

            return specialCondition(list);
        }
    }
}
