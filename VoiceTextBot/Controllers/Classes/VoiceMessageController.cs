using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTextBot.Controllers.Interfaces;
using VoiceTextBot.Configuration;
using VoiceTextBot.Services.Interfaces;

namespace VoiceTextBot.Controllers.Classes
{
    internal class VoiceMessageController(
        ITelegramBotClient client, 
        IFileHandler audioFileHandler,
        IStorage memoryStorage) : MessageController(client), IMessageController<Message>
    {
        private readonly IFileHandler _audioFileHandler = audioFileHandler;
        private readonly IStorage _memoryStorage = memoryStorage;
        public async Task Handle(Message message, CancellationToken ct)
        {
            var fileId = message.Voice.FileId;
            if (fileId == null)
                return;

            await _audioFileHandler.Download(fileId, ct);

            string userLangCode = _memoryStorage.GetSession(message.Chat.Id).LanguageCode;
            
            string result = _audioFileHandler.Process(userLangCode);

            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, result, cancellationToken: ct);
        }
    }
}
