using Microsoft.EntityFrameworkCore;
using PlantApp.Data;
using PlantApp.Services;
using PlantApp.ViewModels;

namespace PlantApp.Views.AdditionalViews;

public partial class PlantDetailsPage : ContentPage
{
    public PlantDetailsPage(PlantDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}