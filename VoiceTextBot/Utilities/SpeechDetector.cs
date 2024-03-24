using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VoiceTextBot.Extensions;
using VoiceTextBot.Extentions;
using Vosk;

namespace VoiceTextBot.Utilities
{
    internal static class SpeechDetector
    {
        public static string DetectSpeech(string audioPath, float inputBitrate, string languageCode)
        {
            Vosk.Vosk.SetLogLevel(0);
            var modelPath = Path.Combine(
                DirectoryExtension.GetSolutionRoot(), 
                "Speech-models", 
                $"vosk-model-small-{languageCode.ToLower()}");

            Model model = new( modelPath );

            return GetWords(model, audioPath, inputBitrate);
        }

        private static string GetWords(Model model, string audioPath, float inputBitrate)
        {
            VoskRecognizer recognizer = new(model, inputBitrate);
            recognizer.SetMaxAlternatives(0);
            recognizer.SetWords(true);

            StringBuilder textBuffer = new();

            using(Stream source = File.OpenRead(audioPath))
            {
                byte[] buffer = new byte[4096];
                int bytesRead = 0;

                while((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if(recognizer.AcceptWaveform(buffer, bytesRead))
                    {
                        var sentenceJson = recognizer.Result();
                        JObject sentenceObj = JObject.Parse(sentenceJson);
                        string sentence = (string)sentenceObj["text"];
                        textBuffer.Append($"{StringExtension.UpperCaseFirst(sentence)}. ");
                    }
                }
            }

            var finalSentence = recognizer.FinalResult();

            JObject finalSentensObj = JObject.Parse(finalSentence);
            
            textBuffer.Append((string)finalSentensObj["text"]);

            return textBuffer.ToString();
        }
    }
}
