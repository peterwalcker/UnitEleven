using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using VoiceTextBot.Configuration;
using VoiceTextBot.Services.Interfaces;
using VoiceTextBot.Utilities;

namespace VoiceTextBot.Services.Classes
{
    internal class AudioFileHandler(AppSettings appSettings, ITelegramBotClient telegramBotClient) : IFileHandler
    {
        private readonly AppSettings _appSettings = appSettings;
        private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

        public async Task Download(string fileId, CancellationToken ct)
        {
            string inputAudioFilePath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}.{_appSettings.InputAudioFormat}");

            using(FileStream destinationStream = File.Create(inputAudioFilePath))
            {
                var file = await _telegramBotClient.GetFileAsync(fileId, ct);
                if (file.FilePath == null)
                    return;

                await _telegramBotClient.DownloadFileAsync(file.FilePath, destinationStream, ct);
            }
        }

        public string Process(string languageCode)
        {
            string inputAudioPath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}.{_appSettings.InputAudioFormat}");
            string outputAudioPath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}.{_appSettings.OutputAudioFormat}");

            Console.WriteLine("Converting...");
            AudioConverter.TryConvert(inputAudioPath, outputAudioPath);
            Console.WriteLine("Converted");

            Console.WriteLine("Recognizing...");
            var speechText = SpeechDetector.DetectSpeech(outputAudioPath, _appSettings.InputBitrate, languageCode);
            Console.WriteLine("Recognized.");

            return speechText;
        }
    }
}
