using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Helpers
{
    public class NavigationHelper //этот хелпер на будущее, в случае разрастания проекта, он пока не нужен, но на будущее сделан
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationHelper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateToPageAsync<TPage, TViewModel>(Func<TViewModel, Task> init)
            where TPage : Page
            where TViewModel : class
        {
            var page = _serviceProvider.GetRequiredService<TPage>();

            if (page.BindingContext is TViewModel vm)
                await init(vm);

            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}
