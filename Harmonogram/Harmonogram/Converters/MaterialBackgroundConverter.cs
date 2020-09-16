using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Harmonogram.Converters
{
    public class MaterialBackgroundConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string matColor = (string)values[0];
            string material = (string)values.ElementAt(1);

            if (StaticResources.Materials.Contains(material))
            {
                return Brushes.White;
            }
            else
            {
                return Brushes.Red;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
