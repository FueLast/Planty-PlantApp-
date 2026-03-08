// Базовые пространства имён
//using AndroidX.Lifecycle;
using CommunityToolkit.Mvvm.ComponentModel;
// MAUI / UI
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using PlantApp.Data;
using PlantApp.Services;
// ViewModel и модели
using PlantApp.ViewModels;
using PlantApp.Views.AdditionalViews;
using SQLite;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PlantApp.Views.AdditionalViews;

public partial class PlantDetailsPage : ContentPage
{

    public string Image_of_PLant { get; set; }

    public PlantDetailsPage(Plant plant)
    {
        InitializeComponent();
        BindingContext = plant; 
    }

    //private void Load(Plant plant)
    //{ 

    //    Image_of_PLant = plant.PlantImage;

    //}


    //public string Plant_Photo { get; set; }
    //private void LoadPlant(Plant plant)
    //{
    //    Plant_Photo = plant;
    //}

}
