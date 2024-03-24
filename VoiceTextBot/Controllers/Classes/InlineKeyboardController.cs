// Ignore Spelling: Inline

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
    internal class InlineKeyboardController(ITelegramBotClient client, IStorage memoryStorage) : 
        MessageController(client), 
        IMessageController<CallbackQuery>
    {
        private IStorage _memoryStorage = memoryStorage;

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if(callbackQuery.Data == null) 
                return;

            _memoryStorage.GetSession(callbackQuery.From.Id).LanguageCode = callbackQuery.Data;

            string languageText = callbackQuery.Data switch
            {
                "ru" => $"<b>Выбранный язык - русский.{Environment.NewLine}</b>" +
                        $"{Environment.NewLine} Изменить можно в главном меню.",
                "en" => $"<b>Selected language - english.{Environment.NewLine}</b>" +
                        $"{Environment.NewLine}You can change this setting in main menu.",
                "ja" => $"<b>選択した言語 - 日本語。{Environment.NewLine}</b>" +
                        $"{Environment.NewLine}メインメニューで選択を変更できます。",
                _ => String.Empty,
            }; 

            await _telegramBotClient.SendTextMessageAsync(callbackQuery.From.Id,
                languageText,
                cancellationToken: ct,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
    }
}
