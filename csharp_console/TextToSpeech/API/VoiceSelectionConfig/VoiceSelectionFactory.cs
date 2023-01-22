using Google.Cloud.TextToSpeech.V1;

namespace TextToSpeech.VoiceSelection
{
    public static class VoiceSelectionFactory
    {
        public static VoiceSelectionParams Create()
        {
            var voiceSelectionParams = new VoiceSelectionParams
            {
                /// <summary>
                /// Required. The language (and potentially also the region) of the voice
                /// expressed as a [BCP-47](https://www.rfc-editor.org/rfc/bcp/bcp47.txt)
                /// language tag, e.g. "en-US". This should not include a script tag (e.g. use
                /// "cmn-cn" rather than "cmn-Hant-cn"), because the script will be inferred
                /// from the input provided in the SynthesisInput.  The TTS service
                /// will use this parameter to help choose an appropriate voice.  Note that
                /// the TTS service may choose a voice with a slightly different language code
                /// than the one selected; it may substitute a different region
                /// (e.g. using en-US rather than en-CA if there isn't a Canadian voice
                /// available), or even a different language, e.g. using "nb" (Norwegian
                /// Bokmal) instead of "no" (Norwegian)".
                /// </summary>
                LanguageCode = "en-US",

                /// <summary>
                /// The name of the voice. If not set, the service will choose a
                /// voice based on the other parameters such as language_code and gender.
                /// </summary>
                //Name = "",

                /// <summary>
                /// The gender of this voice.
                /// </summary>
                SsmlGender = SsmlVoiceGender.Female,

                /// <summary>
                /// https://cloud.google.com/text-to-speech/custom-voice/docs
                /// The configuration for a custom voice. If [CustomVoiceParams.model] is set,
                /// the service will choose the custom voice matching the specified
                /// configuration.
                /// </summary>
                //CustomVoice = new CustomVoiceParams();
            };

            return voiceSelectionParams;
        }
    }
}
