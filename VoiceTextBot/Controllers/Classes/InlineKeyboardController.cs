using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTextBot.Controllers.Interfaces;
using VoiceTextBot.Configuration;

namespace VoiceTextBot.Controllers.Classes
{
    internal class InlineKeyboardController : MessageController, IMessageController<CallbackQuery>
    {
        public InlineKeyboardController(ITelegramBotClient client) : base(client) { }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            Console.WriteLine($"Controller {GetType().Name} detected a button press");

            await _telegramBotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Detected a button press", cancellationToken: ct);
        }
    }
}
