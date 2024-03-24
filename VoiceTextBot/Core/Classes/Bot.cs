using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Hosting;

namespace VoiceTextBot.Core.Classes
{
    class Bot : BackgroundService
    {
        private ITelegramBotClient _telegramBotClient;

        public Bot(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        async Task HandlerUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                                "you pushed the button",
                                                                cancellationToken: cancellationToken);
                return;
            }

            if (update.Type == UpdateType.Message)
            {
                Console.WriteLine($"Got new message: {update.Message.Text}");

                await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                                "you sent text message",
                                                                cancellationToken: cancellationToken);
                return;
            }
        }

        Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramBotClient.StartReceiving(
                HandlerUpdateAsync,
                HandleErrorAsync,
                new Telegram.Bot.Polling.ReceiverOptions() { AllowedUpdates = { } },
                cancellationToken: stoppingToken);

            Console.WriteLine("Bot started!");
        }
    }
}
