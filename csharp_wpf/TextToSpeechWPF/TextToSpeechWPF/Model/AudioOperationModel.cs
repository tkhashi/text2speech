using Microsoft.Win32;
using NAudio.Wave;
using Reactive.Bindings;
using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using System.Windows;

namespace TextToSpeechWPF.Model
{
    public class AudioOperationModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly string _path;
        private readonly WaveOutEvent _outputDevice;
        private AudioFileReader _reader;

        public string FileName { get; private set; }

        public ReactivePropertySlim<TimeSpan> CurrentTime { get; } = new();
        public ReactivePropertySlim<TimeSpan> AudioTimeRange { get; } = new();
        public ReactivePropertySlim<TimeSpan> RemainTime { get; } = new();

        public AudioOperationModel(string path)
        {
            _path = path;
            _outputDevice = new WaveOutEvent();
            _disposables.Add(_outputDevice);
            _reader = new AudioFileReader(path);
            _disposables.Add(_reader);

            FileName = Path.GetFileNameWithoutExtension(path);
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
                File.Copy(_path, sfd.FileName, overwrite: sfd.OverwritePrompt);
                MessageBox.Show("ファイルを保存しました。", "保存", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void DeleteMp4()
        {
        }
        public void Play()
        {
            // 停止時・再生時は初期化しない
            if (_outputDevice.PlaybackState is not (PlaybackState.Paused or PlaybackState.Playing))
            {
                _outputDevice.Init(_reader);
                _reader.Position = 0;
            }
            _outputDevice.Play();
        }

        public void Stop()
        {
            _outputDevice.Pause();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}