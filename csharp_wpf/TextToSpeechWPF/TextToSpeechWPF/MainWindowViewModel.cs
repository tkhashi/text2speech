using System;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Reactive.Disposables;
using System.Windows.Input;
using System.IO;
using System.Linq;

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
        public ReactiveCommand SpeechCommand { get; } = new();

        public ReactiveCollection<AudioOperationViewModel> AudioOperations { get; } = new();

        public MainWindowViewModel()
        {
            _model = new MainWindowModel();
            IsGenerating = _model.IsGenerating;
            InputText = _model.Text;
            SpeakingRate = _model.Rate;
            Pitch = _model.Pitch;
            SpeechCommand = IsGenerating.Inverse().ToReactiveCommand();
            SpeechCommand.Subscribe(Speech).AddTo(_disposables);

            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var sourceDir = Path.Combine(localAppData, "Text2Speech");
            Directory.CreateDirectory(sourceDir);

            Directory.GetFiles(sourceDir, "*.mp3", SearchOption.TopDirectoryOnly)
                .ToList()
                .ForEach(x => AddFilePath(x));

            // Mock
            AddFilePath("test1");
            AddFilePath("test2");
            AddFilePath("test3");
        }

        private void Speech()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                var savePath = _model.Speech();
                AddFilePath(savePath);
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

        private void AddFilePath(string path)
        {
            AudioOperations.Add(new AudioOperationViewModel(path));
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
