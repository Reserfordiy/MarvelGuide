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
        public static FrameworkElement HidingUIElement(FrameworkElement element)
        {
            element.Visibility = Visibility.Hidden;
            element.Height = 0;
            element.Margin = new Thickness(0);

            return element;
        }
    }
}
