using PlantApp.Data;
using System.Globalization;

namespace PlantApp.Helpers;

public class AvatarHelper : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not UserProfile profile)
            return "avatar_1.png";

        if (!string.IsNullOrWhiteSpace(profile.AvatarUrl))
            return profile.AvatarUrl;

        if (profile.AvatarId <= 0)
            return "avatar_1.png";

        return $"avatar_{profile.AvatarId}.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}