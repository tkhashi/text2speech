using System;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Reactive.Disposables;
using System.Windows.Input;

namespace TextToSpeechWPF
{
    class MainWindowViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly MainWindowModel _model;

        public ReactivePropertySlim<bool> IsGenerating { get; }
        public ReactivePropertySlim<string> InputText { get; }
        public ReactivePropertySlim<double> SpeakingRate { get; } 
        public ReactivePropertySlim<double> Pitch { get; } 
        public ReactiveCommand SpeechCommand { get; } = new ();

        public ReactiveCollection<FileOperationViewModel> FileOperations { get; } = new ();

        public MainWindowViewModel()
        {
            _model = new MainWindowModel();
            IsGenerating = _model.IsGenerating;
            InputText = _model.Text;
            SpeakingRate = _model.Rate;
            Pitch = _model.Pitch;
            SpeechCommand = IsGenerating.Inverse().ToReactiveCommand();
            SpeechCommand.Subscribe(Speech).AddTo(_disposables);

            // Mock
            FileOperations.Add(new FileOperationViewModel());
            FileOperations.Add(new FileOperationViewModel());
            FileOperations.Add(new FileOperationViewModel());
        }

        private void Speech()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                _model.Speech();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                Mouse.OverrideCursor = default;
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
