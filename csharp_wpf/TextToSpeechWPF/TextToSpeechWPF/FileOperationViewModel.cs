using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Reactive.Disposables;

namespace TextToSpeechWPF
{
    public class FileOperationViewModel
    {
        private readonly CompositeDisposable _disposables = new ();
        private readonly FileOperationModel _model;

        public ReactivePropertySlim<string> Name { get; } = new("test1");
        // tempフォルダからユーザーが触れるフォルダーへ移動する
        public ReactiveCommand SaveCommand { get; } = new();
        public ReactiveCommand DeleteCommand { get; }=new();

        public FileOperationViewModel()
        {
            _model = new FileOperationModel(Name.Value);
            SaveCommand
                .Subscribe(() => _model.MoveMp4())
                .AddTo(_disposables);
            DeleteCommand
                .Subscribe(() => _model.DeleteMp4())
                .AddTo(_disposables);
        }
    }
}
