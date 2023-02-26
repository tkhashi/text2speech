using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

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
        public ReactiveCommand<string> ValueChangedCommnad { get; } = new ();

        public ReactivePropertySlim<string> Name { get; } 
        public ReactivePropertySlim<bool> IsPlaying { get; } = new ();
        public ReactiveProperty<string> CurrentTime { get; }
        public ReactiveProperty<string> AudioTimeRange { get; }
        public ReactiveProperty<string> RemainTime { get; }

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

            ValueChangedCommnad
                .Subscribe(x =>
                {
                    IsPlaying.Value = false;
                    _model.Stop();

                    var canParse = TimeSpan.TryParse(x, out var time);
                    if (!canParse) return;
                    _model.ChangePosition(time);
                })
                .AddTo(_disposables);

            Name = new ReactivePropertySlim<string> (_model.FileName);
            CurrentTime = _model.CurrentTime.Select(x => x.ToString(@"mm\:ss")).ToReactiveProperty();
            AudioTimeRange = _model.AudioTimeRange.Select(x => x.ToString(@"mm\:ss")).ToReactiveProperty();
            RemainTime = _model.AudioTimeRange.Select(x => x.ToString(@"mm\:ss")).ToReactiveProperty();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
