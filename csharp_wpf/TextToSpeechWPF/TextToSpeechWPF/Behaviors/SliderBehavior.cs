using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace TextToSpeechWPF.Behaviors
{
    class SliderBehavior : Behavior<Slider>
    {
        public double Value { get; set; }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(double),
            typeof(SliderBehavior),
            new PropertyMetadata());

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.ValueChanged += ChangeValue;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.ValueChanged -= ChangeValue;

        }

        private void ChangeValue(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.OldValue == e.NewValue) return;
            Value = e.NewValue;
        }


    }
}
