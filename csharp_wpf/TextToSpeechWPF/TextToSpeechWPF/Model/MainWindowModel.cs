using System;
using System.IO;
using Google.Cloud.TextToSpeech.V1;
using Reactive.Bindings;
using SpeechLib;
using TextToSpeech.AudioConfig;
using TextToSpeech.SynthesisInputConfig;
using TextToSpeech.VoiceSelection;

namespace TextToSpeechWPF.Model;

internal class MainWindowModel
{
    private const double DefaultRate = 1;
    private const double DefaultPitch = 0;
    public ReactivePropertySlim<bool> IsGenerating { get; } = new();
    public ReactivePropertySlim<string> Text { get; } = new("");
    public ReactivePropertySlim<double> Rate { get; } = new(DefaultRate);
    public ReactivePropertySlim<double> Pitch { get; } = new();

    /// <summary>
    ///     テキストから音声を生成
    /// </summary>
    /// <returns>音声ファイル保存パス</returns>
    public string Speech()
    {
        IsGenerating.Value = true;
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var fileName = Path.ChangeExtension(Text.Value[..10], "mp3");
        var savePath = Path.Combine(localAppData, "Text2Speech", fileName);
        try
        {
            var credentials = @"./credentials.json";
            if (File.Exists(credentials))
            {
                var gcEnv = "GOOGLE_APPLICATION_CREDENTIALS";
                Environment.SetEnvironmentVariable(gcEnv, credentials);

                var client = TextToSpeechClient.Create();
                var input = SynthesisInputFactory.Create().SetText(Text.Value);
                var voiceSelection = VoiceSelectionFactory.Create();
                var audioConfig = AudioConfigFactory.Create().SetAudioConfig(Rate.Value, Pitch.Value);

                var response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

                using var fs = File.Create(savePath);
                response.AudioContent.WriteTo(fs);
            }
            else
            {
                var voice = new SpVoice();
                var stream = new SpFileStream();
                stream.Format.Type = SpeechAudioFormatType.SAFT44kHz16BitMono;
                stream.Open(savePath, SpeechStreamFileMode.SSFMCreateForWrite, true);
                voice.AudioOutputStream = stream;
                voice.Rate = (int)Rate.Value;
                voice.Speak(Text.Value);
                stream.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            IsGenerating.Value = false;
        }

        Text.Value = "";
        Rate.Value = DefaultRate;
        Pitch.Value = DefaultPitch;
        return savePath;
    }
}