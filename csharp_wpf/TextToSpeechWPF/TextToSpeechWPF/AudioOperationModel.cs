using Reactive.Bindings;
using System;
using System.IO;

namespace TextToSpeechWPF
{
    public class AudioOperationModel
    {
        private readonly string _path;
        public string FileName { get; private set; } 

        public ReactivePropertySlim<TimeSpan> CurrentTime { get; } = new ();
        public ReactivePropertySlim<TimeSpan> AudioTimeRange { get; } = new ();
        public ReactivePropertySlim<TimeSpan> RemainTime { get; } = new ();

        public AudioOperationModel(string path)
        {
            _path = path;
            FileName = Path.GetFileNameWithoutExtension(path);
        }

        public void ChangeFileName(string fileName)
        {
            FileName = fileName;
        }

        public void MoveMp4()
        {
        }

        public void DeleteMp4()
        {
        }
        public void Play()
        {
        }

        public void Stop()
        {
        }

    }
}