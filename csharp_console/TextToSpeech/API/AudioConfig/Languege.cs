using TextToSpeech.AudioConfig;

namespace TextToSpeech.AudioConfig
{
    public enum Languege
    {
        English,
        Japanese,
    }
}

public static class LanguegeExtensions
{
    public static string ToRFCTags(this Languege lang)
    {
        return lang switch
        {
            Languege.English => "en-US",
            Languege.Japanese => "ja-JP",
            _ => "ja-JP",
        };
    }
}

