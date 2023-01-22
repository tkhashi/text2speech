using Google.Cloud.TextToSpeech.V1;

namespace TextToSpeech.SynthesisInputConfig
{
    public static class SynthesisInputFactory
    {
        public static SynthesisInput Create()
        {
            var input = new SynthesisInput
            {
                /// <summary>
                /// The raw text to be synthesized.
                /// </summary>
                Text = "This is a demonstration of the Google Cloud Text-to-Speech API",

                /// <summary>
                /// The SSML document to be synthesized. The SSML document must be valid
                /// and well-formed. Otherwise the RPC will fail and return
                /// [google.rpc.Code.INVALID_ARGUMENT][google.rpc.Code.INVALID_ARGUMENT]. For
                /// more information, see
                /// [SSML](https://cloud.google.com/text-to-speech/docs/ssml).
                /// </summary>
                //Ssml = "",
            };

            return input;
        }
    }
}
