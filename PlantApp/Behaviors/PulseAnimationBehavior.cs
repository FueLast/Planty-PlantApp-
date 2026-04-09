using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace PlantApp.Behaviors
{
    public class PulseAnimationBehavior : Behavior<VisualElement>
    {
        private bool _isRunning;

        protected override void OnAttachedTo(VisualElement bindable)
        {
            base.OnAttachedTo(bindable);

            _isRunning = true;
            StartAnimation(bindable);
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            _isRunning = false;
            base.OnDetachingFrom(bindable);
        }

        private async void StartAnimation(VisualElement view)
        {
            while (_isRunning)
            {
                await view.FadeTo(0.5, 800);
                await view.FadeTo(1, 800);
            }
        }
    }
}