using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace PlantApp.Selectors;

public class PopularTemplateSelector : DataTemplateSelector
{
    public DataTemplate PlantTemplate { get; set; }
    public DataTemplate SeeAllTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (item is string)
            return SeeAllTemplate;

        return PlantTemplate;
    }
}