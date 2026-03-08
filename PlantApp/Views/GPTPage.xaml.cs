using System.ComponentModel;
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using PlantApp.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantApp.Services;
using PlantApp.Data;
using SQLite; 

namespace PlantApp.Views;

public partial class GPTPage : ContentPage
{
    public GPTPage()
    {
        InitializeComponent(); 
    }
}

