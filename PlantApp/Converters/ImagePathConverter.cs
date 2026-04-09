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
                return "plant_placeholder.png";

            // если это локальный файл
            if (File.Exists(path))
                return ImageSource.FromFile(path);

            // если это ссылка
            if (path.StartsWith("http"))
                return ImageSource.FromUri(new Uri(path));

            return "plant_placeholder.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
