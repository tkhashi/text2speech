using Reactive.Bindings;

namespace TextToSpeechWPF
{
    class MainWindowViewModel
    {
        private readonly MainWindowModel _model;

        public ReactivePropertySlim<string> InputText { get; } = new();
        public ReactivePropertySlim<double> SpeakingRate { get; } = new();
        public ReactivePropertySlim<double> Pitch { get; } = new();
        public ReactiveCommand SpeechCommand { get; } = new ();


        public MainWindowViewModel()
        {
            _model = new MainWindowModel();
            SpeechCommand.Subscribe(Speech);
        }

        private void Speech()
        {
            _model.Speech();
        }
    }
}
