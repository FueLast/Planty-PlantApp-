using CommunityToolkit.Mvvm.ComponentModel;
using PlantApp.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.ViewModels;

public partial class CalendarViewModel : ObservableObject
{
    // выбранная дата
    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    // текущий месяц
    [ObservableProperty]
    private DateTime currentMonth = DateTime.Today;

    // все события
    public ObservableCollection<PlantReminder> AllEvents { get; set; }

    // события выбранного дня
    public ObservableCollection<PlantReminder> SelectedDayEvents { get; set; }

    public CalendarViewModel()
    {
        // временные тестовые данные
        AllEvents = new ObservableCollection<PlantReminder>
        {
            new PlantReminder { Title = "полив фикуса", Date = DateTime.Today },
            new PlantReminder { Title = "удобрение кактуса", Date = DateTime.Today.AddDays(1) },
            new PlantReminder { Title = "полив монстеры", Date = DateTime.Today }
        };

        SelectedDayEvents = new ObservableCollection<PlantReminder>();

        // обновляем список при старте
        UpdateEvents();
    }

    partial void OnSelectedDateChanged(DateTime value)
    {
        // обновляем список при выборе даты
        UpdateEvents();
    }

    private void UpdateEvents()
    {
        SelectedDayEvents.Clear();

        foreach (var item in AllEvents)
        {
            if (item.Date.Date == SelectedDate.Date)
                SelectedDayEvents.Add(item);
        }
    }
}