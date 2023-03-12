using System;
using System.IO;
using Google.Cloud.TextToSpeech.V1;
using Reactive.Bindings;
using TextToSpeech.AudioConfig;
using TextToSpeech.SynthesisInputConfig;
using TextToSpeech.VoiceSelection;

namespace TextToSpeechWPF.Model
{
    class MainWindowModel
    {
        public ReactivePropertySlim<bool> IsGenerating { get; } = new ();
        public ReactivePropertySlim<string> Text { get; } = new ("");
        public ReactivePropertySlim<double> Rate { get; } = new (1);
        public ReactivePropertySlim<double> Pitch { get; } = new (0);

        public void Speech()
        {
            IsGenerating.Value = true;
            try
            {
                var gcEnv = "GOOGLE_APPLICATION_CREDENTIALS";
                var credentials = @"./credentials.json";
                Environment.SetEnvironmentVariable(gcEnv, credentials);

                var client = TextToSpeechClient.Create();
                var input = SynthesisInputFactory.Create().SetText(Text.Value);
                var voiceSelection = VoiceSelectionFactory.Create();
                var audioConfig = AudioConfigFactory.Create().SetAudioConfig(Rate.Value, Pitch.Value);

                SynthesizeSpeechResponse response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

                var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var fileName = Path.ChangeExtension(Text.Value[..10], "mp3");
                using var fs = File.Create(Path.Combine(localAppData, "Text2Speech", fileName));
                response.AudioContent.WriteTo(fs);
            }
            catch (Exception e)
            {
                Console.WriteLine (e);
                throw;
            }
            finally
            {
                IsGenerating.Value = false;
            }
        }
    }
}
