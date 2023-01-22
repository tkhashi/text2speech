using Google.Cloud.TextToSpeech.V1;

var gcEnv = "GOOGLE_APPLICATION_CREDENTIALS";
var credentias = @"./credentials.json";
Environment.SetEnvironmentVariable(gcEnv, credentias);

var client = TextToSpeechClient.Create();
var input = new SynthesisInput
{
    Text = "This is a demonstration of the Google Cloud Text-to-Speech API"
};

var voiceSelection = new VoiceSelectionParams
{
    LanguageCode = "en-US",
    SsmlGender = SsmlVoiceGender.Female
};

var audioConfig = new AudioConfig
{
    AudioEncoding = AudioEncoding.Mp3
};

SynthesizeSpeechResponse response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

var outputDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
using var output = File.Create($"{outputDir}/sample.mp3");
response.AudioContent.WriteTo(output);
