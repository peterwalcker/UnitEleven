using HomeworkBot.Controllers.Interfaces;
using HomeworkBot.Services.Interfaces;
using HomeworkBot.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HomeworkBot.Controllers.Classes
{
    internal class TextMessageController(ITelegramBotClient botClient, IStorage sessionStorage) : MessageController(botClient), IController<Message>
    {
        private readonly IStorage _sessionStorage = sessionStorage;

        public async Task Handle(Message message, CancellationToken cancellationToken)
        {
            Console.WriteLine("Got text message");

            switch (message.Text)
            {
                case "/start":
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Сумма чисел", "sum"),
                        InlineKeyboardButton.WithCallbackData("Количесво символов", "count")
                    });

                    await _botClient.SendTextMessageAsync(
                        message.Chat.Id, 
                        $"<b>Выберите желаемый результат:</b>" +
                        $"{Environment.NewLine}" +
                        $"{Environment.NewLine}" +
                        $"Посчитать сумму чисел или узнать количество символов в строке?", 
                        cancellationToken: cancellationToken, 
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                        replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;

                default:

                    string result = string.Empty;
                    int res = 0;
                    try
                    {
                        switch (_sessionStorage.GetSession(message.Chat.Id).ActionType)
                        {
                            case "sum":
                                res = Calculate.Sum(message.Text);
                                result = $"Сумма чисел равна {res}";
                                break;
                            case "count":
                                res = Count.GetCount(message.Text);
                                result = $"В сообщении {res} символов";
                                break;
                            default:
                                result = "Необходимо выбрать режим в главном меню.";
                                break;
                        }
                    }
                    catch(Exception ex)
                    {
                        result = ex.Message;
                    }
                    

                    await _botClient.SendTextMessageAsync(message.Chat.Id, result, cancellationToken: cancellationToken);
                    break;
            }
        }
    }
}
