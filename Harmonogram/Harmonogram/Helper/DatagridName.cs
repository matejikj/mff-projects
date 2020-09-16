using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Harmonogram.Helper
{
    public class DatagridName
    {
        public static string GetName(DependencyObject obj)
        {
            return (string)obj.GetValue(NameProperty);
        }

        public static void SetName(DependencyObject obj, string value)
        {
            obj.SetValue(NameProperty, value);
        }

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.RegisterAttached("Name", typeof(string), typeof(DatagridName), new UIPropertyMetadata(""));

    }
}
