using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace TextToSpeechWPF.Behaviors
{
    class SliderBehavior : Behavior<Slider>
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(double),
            typeof(SliderBehavior),
            new PropertyMetadata());

        public double Value 
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value); 
        }

        public static readonly DependencyProperty ValueChangedCommandProperty = DependencyProperty.Register(
            nameof(ValueChangedCommand),
            typeof(ICommand),
            typeof(SliderBehavior),
            new PropertyMetadata());

        public ICommand ValueChangedCommand 
        { 
            get => (ICommand) GetValue(ValueChangedCommandProperty);
            set => SetValue(ValueChangedCommandProperty, value);
        }

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
            // ViewModelからの変更は受け付けない
            // View(UI側)からの変更の場合のみCommand発火
            if (Value != e.NewValue)
            {
                Value = e.NewValue;
                ValueChangedCommand.Execute(Value);
            }
        }


    }
}
