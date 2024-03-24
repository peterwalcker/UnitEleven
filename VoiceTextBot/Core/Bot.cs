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
using VoiceTextBot.Controllers.Classes;

namespace VoiceTextBot.Core
{
    class Bot : BackgroundService
    {
        private ITelegramBotClient _telegramBotClient;

        private DefaultMessageController _defaultMessageController;
        private TextMessageController _textMessageController;
        private VoiceMessageController _voiceMessageController;
        private InlineKeyboardController _inlineKeyboardController;

        public Bot(
            ITelegramBotClient telegramBotClient,
            DefaultMessageController defaultMessageController,
            TextMessageController textMessageController,
            VoiceMessageController voiceMessageController,
            InlineKeyboardController inlineKeyboardController)
        {
            _telegramBotClient = telegramBotClient;
            _defaultMessageController = defaultMessageController;
            _textMessageController = textMessageController;
            _voiceMessageController = voiceMessageController;
            _inlineKeyboardController = inlineKeyboardController;
        }

        async Task HandlerUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }

            if (update.Type == UpdateType.Message)
            {
                switch (update.Message!.Type)
                {
                    case MessageType.Text:
                        await _textMessageController.Handle(update.Message, cancellationToken);
                        return;
                    case MessageType.Voice:
                        await _voiceMessageController.Handle(update.Message, cancellationToken);
                        return;
                    default:
                        await _defaultMessageController.Handle(update.Message, cancellationToken);
                        return;
                }
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
