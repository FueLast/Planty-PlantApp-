using PlantApp.Data;
using System.Globalization;

namespace PlantApp.Helpers;

public class AvatarHelper : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var profile = value as UserProfile;

        if (profile == null)
            return "avatar_1.png";

        if (!string.IsNullOrWhiteSpace(profile.AvatarUrl))
            return profile.AvatarUrl;

        return $"avatar_{profile.AvatarId}.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}