using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace VoiceTextBot.Controllers.Classes
{
    abstract class MessageController
    {
        protected readonly ITelegramBotClient _telegramBotClient;

        public MessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }
    }
}
