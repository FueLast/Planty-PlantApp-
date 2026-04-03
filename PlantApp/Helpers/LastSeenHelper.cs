using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using PlantApp.Data;

namespace PlantApp.Helpers;

public class LastSeenHelper : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var profile = value as UserProfile;

        if (profile == null)
            return "";

        if (profile.LastSeen == null)
            return "давно не заходил";

        var diff = DateTime.UtcNow - profile.LastSeen.Value;



        if (diff.TotalSeconds < 30)
            return "онлайн";

        if (diff.TotalMinutes < 1)
            return "только что";

        if (diff.TotalMinutes < 60)
            return $"был {Math.Floor(diff.TotalMinutes)} мин назад";

        if (diff.TotalHours < 24)
            return $"был {Math.Floor(diff.TotalHours)} ч назад";

        return $"был {Math.Floor(diff.TotalDays)} д назад";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}