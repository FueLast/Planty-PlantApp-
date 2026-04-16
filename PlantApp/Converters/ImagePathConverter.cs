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
                return ImageSource.FromFile("background_listik_profile.png");

            try
            {
                if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    return ImageSource.FromUri(new Uri(path));

                if (File.Exists(path))
                    return ImageSource.FromFile(path);
            }
            catch
            {
                // игнор, упадём в placeholder
            }

            return ImageSource.FromFile("background_listik_profile.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
