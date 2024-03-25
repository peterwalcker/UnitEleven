using HomeworkBot.Controllers.Interfaces;
using HomeworkBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HomeworkBot.Controllers.Classes
{
    internal class InlineKeyboardController(ITelegramBotClient botClient, IStorage SessionStorage) 
        : MessageController(botClient), IController<CallbackQuery>
    {
        private IStorage _sessionStorage = SessionStorage;
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken cancellationToken)
        {
            if (callbackQuery.Data == null) 
                return;

            _sessionStorage.GetSession(callbackQuery.From.Id).ActionType = callbackQuery.Data;

            string actionType = callbackQuery.Data switch
            {
                "sum" => $"Выбран режим <b>сложения чисел</b>!",
                "count" => $"Выбран режим <b>подсчёта количества символов</b>!",
                _ => String.Empty
            };

            await _botClient.SendTextMessageAsync(
                callbackQuery.From.Id, 
                actionType, 
                cancellationToken: cancellationToken, 
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
    }
}
