using Microsoft.Win32;
using NAudio.Wave;
using Reactive.Bindings;
using System;
using System.IO;

namespace TextToSpeechWPF
{
    public class AudioOperationModel
    {
        private readonly string _path;
        private WaveOutEvent _outputDevice;

        public string FileName { get; private set; } 

        public ReactivePropertySlim<TimeSpan> CurrentTime { get; } = new ();
        public ReactivePropertySlim<TimeSpan> AudioTimeRange { get; } = new ();
        public ReactivePropertySlim<TimeSpan> RemainTime { get; } = new ();

        public AudioOperationModel(string path)
        {
            _path = path;
            _outputDevice = new WaveOutEvent();
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

            sfd.ShowDialog();
        }

        public void DeleteMp4()
        {
        }
        public void Play()
        {
            using var afr = new AudioFileReader(_path);
            _outputDevice.Init(afr);
            _outputDevice.Play();
        }

        public void Stop()
        {
        }

    }
}