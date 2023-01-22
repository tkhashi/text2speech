using System;
using System.IO;
using Google.Cloud.TextToSpeech.V1;
using Reactive.Bindings;
using TextToSpeech.AudioConfig;
using TextToSpeech.SynthesisInputConfig;
using TextToSpeech.VoiceSelection;

namespace TextToSpeechWPF
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
                var credentias = @"./credentials.json";
                Environment.SetEnvironmentVariable(gcEnv, credentias);

                var client = TextToSpeechClient.Create();
                var input = SynthesisInputFactory.Create().SetText(Text.Value);
                var voiceSelection = VoiceSelectionFactory.Create();
                var audioConfig = AudioConfigFactory.Create().SetAudioConfig(Rate.Value, Pitch.Value);

                SynthesizeSpeechResponse response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

                var outputDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                using var output = File.Create($"{outputDir}/sample.mp3");
                response.AudioContent.WriteTo(output);
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
