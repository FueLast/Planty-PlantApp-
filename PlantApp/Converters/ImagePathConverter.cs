using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace PlantApp.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;

            if (string.IsNullOrWhiteSpace(path))
                return "background_listik_profile.png";

            if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return path;

            return "background_listik_profile.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
