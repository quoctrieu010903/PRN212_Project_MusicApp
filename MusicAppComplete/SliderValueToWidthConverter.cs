using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MusicAppComplete
{
    public class SliderValueToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double sliderValue && parameter is Slider slider)
            {
                // Subtract thumb width (16px as per the style)
                double trackWidth = slider.ActualWidth - 16;
                double max = slider.Maximum;

                // Avoid division by zero
                if (max == 0 || trackWidth <= 0)
                    return 0.0;

                // Calculate proportional width
                return (sliderValue / max) * trackWidth;
            }
            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack is not supported.");
        }
    }
}
