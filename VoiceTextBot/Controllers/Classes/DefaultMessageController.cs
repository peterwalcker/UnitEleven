﻿using System;
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
    internal class DefaultMessageController : MessageController, IMessageController<Message>
    {
        public DefaultMessageController(ITelegramBotClient client) : base(client) { }

        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Controller {GetType().Name} got message");
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Got unknown message", cancellationToken: ct);
        }
    }
}
