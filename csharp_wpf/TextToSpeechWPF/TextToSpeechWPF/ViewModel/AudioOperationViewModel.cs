using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Reactive.Disposables;
using TextToSpeechWPF.Model;

namespace TextToSpeechWPF
{
    public class AudioOperationViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new ();
        private readonly AudioOperationModel _model;

        // tempフォルダからユーザーが触れるフォルダーへ移動する
        public ReactiveCommand SaveCommand { get; } = new();
        public ReactiveCommand DeleteCommand { get; }=new();
        public ReactiveCommand PlayCommand { get; } = new ();
        public ReactiveCommand StopCommand { get; } = new ();

        public ReactivePropertySlim<string> Name { get; } 
        public ReactivePropertySlim<bool> IsPlaying { get; } = new ();
        public ReactivePropertySlim<string> CurrentTime { get; } = new ();
        public ReactivePropertySlim<double> AudioTimeRange { get; } = new ();
        public ReactivePropertySlim<string> RemainTime { get; } = new ();

        public AudioOperationViewModel(string path)
        {
            _model = new AudioOperationModel(path);
            SaveCommand
                .Subscribe(() => _model.MoveMp4(_model.FileName))
                .AddTo(_disposables);

            DeleteCommand
                .Subscribe(() => _model.DeleteMp4())
                .AddTo(_disposables);

            PlayCommand
                .Subscribe(() => 
                {
                    IsPlaying.Value = true;
                    _model.Play();
                })
                .AddTo(_disposables);

            StopCommand
                .Subscribe(() =>
                {
                    IsPlaying.Value = false;
                    _model.Stop();
                })
                .AddTo(_disposables);

            Name = new ReactivePropertySlim<string> (_model.FileName);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
