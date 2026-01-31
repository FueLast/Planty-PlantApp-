using Microsoft.Maui.Hosting;
using PlantApp.Data;
using Microsoft.Maui;
using Microsoft.Maui.Storage;
using Microsoft.Extensions.DependencyInjection;
using PlantApp.Views;

namespace PlantApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // 1. Создаем окно и помещаем в него ваш AppShell
            var window = new Window(new AppShell());

            // 2. Задаем размеры (для Windows и Mac)
            window.Width = 500;
            window.Height = 1000;

            // 3. Возвращаем настроенное окно
            return window;
        }




    }
}