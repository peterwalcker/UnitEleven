using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace HomeworkBot.Controllers.Classes
{
    internal abstract class MessageController(ITelegramBotClient botClient)
    {
        protected readonly ITelegramBotClient _botClient = botClient;
    }
}
