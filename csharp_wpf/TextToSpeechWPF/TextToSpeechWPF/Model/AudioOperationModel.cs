using System;
using System.IO;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using NAudio.Wave;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace TextToSpeechWPF.Model;

public class AudioOperationModel : IDisposable
{
    private readonly CompositeDisposable _disposables = new();
    private readonly WaveOutEvent _outputDevice;
    private readonly string _path;
    private readonly AudioFileReader _reader;

    public AudioOperationModel(string path)
    {
        _path = path;
        _outputDevice = new WaveOutEvent().AddTo(_disposables);
        _reader = new AudioFileReader(path).AddTo(_disposables);

        FileName = Path.GetFileNameWithoutExtension(path);
        CurrentTime = new ReactivePropertySlim<TimeSpan>(TimeSpan.Zero);
        AudioTimeRange = new ReactivePropertySlim<TimeSpan>(_reader.TotalTime);
        RemainTime = new ReactivePropertySlim<TimeSpan>(TimeSpan.Zero);
    }

    public string FileName { get; private set; }

    public ReactivePropertySlim<TimeSpan> CurrentTime { get; }
    public ReactivePropertySlim<TimeSpan> AudioTimeRange { get; }
    public ReactivePropertySlim<TimeSpan> RemainTime { get; }

    public void Dispose()
    {
        _disposables.Dispose();
    }

    public void ChangeFileName(string fileName)
    {
        FileName = fileName;
    }

    public void MoveMp4(string fileName)
    {
        var sfd = new SaveFileDialog
        {
            FileName = Path.ChangeExtension(fileName, "mp3"),
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Title = "保存先のファイルを選択してください",
            RestoreDirectory = true,
            OverwritePrompt = true,
            CheckPathExists = true
        };

        if (sfd.ShowDialog() ?? false)
        {
            File.Copy(_path, sfd.FileName, sfd.OverwritePrompt);
            MessageBox.Show("ファイルを保存しました。", "保存", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public void DeleteMp4()
    {
    }

    public void Play()
    {
        // 停止時・再生時は初期化しない
        if (_outputDevice.PlaybackState is PlaybackState.Stopped) _outputDevice.Init(_reader);

        _outputDevice.Play();

        Task.Run(() =>
        {
            while (_outputDevice.PlaybackState is PlaybackState.Playing)
            {
                CurrentTime.Value = _reader.CurrentTime;
                RemainTime.Value = AudioTimeRange.Value - CurrentTime.Value;
            }

            // 最後まで再生したら戻す
            _reader.Position = 0;
            if (_outputDevice.PlaybackState is PlaybackState.Stopped)
            {
                _reader.Position = 0;
                CurrentTime.Value = TimeSpan.Zero;
            }
        });
    }

    public void Stop()
    {
        _outputDevice.Pause();
    }

    public void ChangePosition(TimeSpan time)
    {
        _reader.CurrentTime = time;
        CurrentTime.Value = _reader.CurrentTime;
    }
}