using HomeworkBot.Controllers.Classes;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using BotTypes = Telegram.Bot.Types;

namespace HomeworkBot.Core
{
    internal class Bot : BackgroundService
    {
        private ITelegramBotClient _botClient;

        private DefaultMessageController _messageController;
        private TextMessageController _textMessageController;
        private InlineKeyboardController _keyboardController;

        public Bot( ITelegramBotClient botClient, 
                    DefaultMessageController messageController, 
                    TextMessageController textMessageController, 
                    InlineKeyboardController keyboardController)
        {
            _botClient = botClient;
            _messageController = messageController;
            _textMessageController = textMessageController;
            _keyboardController = keyboardController;
        }

        async Task HandlerUpdateAsync(ITelegramBotClient botClient, BotTypes.Update update, CancellationToken cancellationToken)
        {
            if(update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                await _keyboardController.Handle(update.CallbackQuery, cancellationToken);
            }

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                switch (update.Message!.Type)
                {
                    case BotTypes.Enums.MessageType:
                        await _textMessageController.Handle(update.Message, cancellationToken);
                        break;
                    
                    default:
                        await _messageController.Handle(update.Message, cancellationToken); 
                        break;
                }
            }
        }

        Task HandlerErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine($"Error: {errorMessage}");

            Console.WriteLine("Waiting for 10 seconds for reconnect.");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _botClient.StartReceiving(
                HandlerUpdateAsync,
                HandlerErrorAsync,
                new Telegram.Bot.Polling.ReceiverOptions() { AllowedUpdates = { } },
                cancellationToken: cancellationToken);

            Console.WriteLine("Bot started!");
        }
    }
}
