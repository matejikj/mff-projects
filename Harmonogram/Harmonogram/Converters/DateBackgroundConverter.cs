using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Harmonogram.Converters
{
    public class DateBackgroundConverter : IMultiValueConverter
    {
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[1] == null)
            {
                return null;
            }
            else
            {
                if (values[0].GetType() == typeof(DateTime) && values[1].GetType() == typeof(DateTime))
                {
                    DateTime dateTime1 = (DateTime)values[0];
                    DateTime dateTime2 = (DateTime)values.ElementAt(1);

                    if (dateTime2.Date == dateTime1.Date)
                    {
                        return null;
                    }
                    else
                    {
                        if (dateTime2.Date > dateTime1.Date)
                        {
                            return new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            return new SolidColorBrush(Colors.LimeGreen);
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
