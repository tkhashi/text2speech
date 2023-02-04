using Google.Cloud.TextToSpeech.V1;
using TextToSpeech.AudioConfig;
using TextToSpeech.SynthesisInputConfig;
using TextToSpeech.VoiceSelection;

var gcEnv = "GOOGLE_APPLICATION_CREDENTIALS";
var credentias = @"./credentials.json";
Environment.SetEnvironmentVariable(gcEnv, credentias);

var client = TextToSpeechClient.Create();
var input = SynthesisInputFactory.Create();
var voiceSelection = VoiceSelectionFactory.Create();
var audioConfig = AudioConfigFactory.Create();

SynthesizeSpeechResponse response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

var outputDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
using var output = File.Create($"{outputDir}/sample.mp3");
response.AudioContent.WriteTo(output);

