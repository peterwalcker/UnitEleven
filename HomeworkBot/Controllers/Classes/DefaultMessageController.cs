using HomeworkBot.Controllers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HomeworkBot.Controllers.Classes
{
    internal class DefaultMessageController(ITelegramBotClient botClient) : MessageController(botClient), IController<Message>
    {
        public async Task Handle(Message message, CancellationToken cancellationToken)
        {
            Console.WriteLine("Got default message");

            await _botClient.SendTextMessageAsync(
                message.Chat.Id, 
                "Такому меня не учили.", 
                cancellationToken: cancellationToken);
        }
    }
}
