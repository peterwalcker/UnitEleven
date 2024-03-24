using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace VoiceTextBot.Controllers.Classes
{
    abstract class MessageController(ITelegramBotClient telegramBotClient)
    {
        protected readonly ITelegramBotClient _telegramBotClient = telegramBotClient;
    }
}
