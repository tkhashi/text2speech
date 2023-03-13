using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using TextToSpeechWPF.Model;

namespace TextToSpeechWPF
{
    public class AudioOperationViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly AudioOperationModel _model;

        // tempフォルダからユーザーが触れるフォルダーへ移動する
        public ReactiveCommand SaveCommand { get; } = new();
        public ReactiveCommand DeleteCommand { get; }=new();
        public ReactiveCommand PlayCommand { get; } = new ();
        public ReactiveCommand StopCommand { get; } = new ();
        public ReactiveCommand<double> ValueChangedCommnad { get; } = new ();

        public ReactivePropertySlim<string> Name { get; } 
        public ReactivePropertySlim<bool> IsPlaying { get; } = new ();

        // 表示用プロパティ
        public ReactiveProperty<string> CurrentTime { get; }
        public ReactiveProperty<string> AudioTimeRange { get; }
        public ReactiveProperty<string> RemainTime { get; }
        // Sliderコントロールへ渡す用プロパティ
        public ReactiveProperty<double> CurrentSliderTime { get; }
        public ReactiveProperty<double> AudioSliderTimeRange { get; }
        public ReactiveProperty<double> RemainSliderTime { get; }

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
                    // 通常再生時もストップしてしまう。Slider thumb操作時のみ来るようにする。
                    //IsPlaying.Value = false;
                    //_model.Stop();

                    var time = TimeSpan.FromMilliseconds(x);
                    _model.ChangePosition(time);
                })
                .AddTo(_disposables);

            Name = new ReactivePropertySlim<string> (_model.FileName);
            CurrentTime = _model.CurrentTime.Select(x => x.ToString(@"mm\:ss")).ToReactiveProperty();
            AudioTimeRange = _model.AudioTimeRange.Select(x => x.ToString(@"mm\:ss")).ToReactiveProperty();
            RemainTime = _model.AudioTimeRange.Select(x => x.ToString(@"mm\:ss")).ToReactiveProperty();

            CurrentSliderTime = _model.CurrentTime.Select(x => x.TotalMilliseconds).ToReactiveProperty();
            AudioSliderTimeRange = _model.AudioTimeRange.Select(x => x.TotalMilliseconds).ToReactiveProperty();
            RemainSliderTime = _model.AudioTimeRange.Select(x => x.TotalMilliseconds).ToReactiveProperty();
        }

        public void AddCurrentTime(double millisecond)
        {
            _model.ChangePosition(TimeSpan.FromMilliseconds(millisecond));
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
