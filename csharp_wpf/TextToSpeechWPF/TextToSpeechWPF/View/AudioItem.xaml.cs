using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TextToSpeechWPF.View
{
    /// <summary>
    /// AudioItem.xaml の相互作用ロジック
    /// </summary>
    public partial class AudioItem : UserControl
    {
        private AudioOperationViewModel _vm;
        private bool _isDragging;

        public AudioItem()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = (AudioOperationViewModel) DataContext;
        }

        // Thumbドラッグの検知
        private void Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _isDragging = true;
        }

        // Track部クリックの検知
        private void Slider_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
        }

        // UI操作時のみVMのValueChangedCommandを発火
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_isDragging) return;
            if (!_vm.ValueChangedCommnad.CanExecute()) return;
            _vm.ValueChangedCommnad.Execute(e.NewValue);

            _isDragging = false;
        }
    }
}
