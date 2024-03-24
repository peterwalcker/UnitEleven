using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTextBot.Controllers.Interfaces;
using VoiceTextBot.Configuration;
using Telegram.Bot.Types.ReplyMarkups;

namespace VoiceTextBot.Controllers.Classes
{
    internal class TextMessageController(ITelegramBotClient client) : MessageController(client), IMessageController<Message>
    {
        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"🇷🇺 Русский", $"ru"),
                        InlineKeyboardButton.WithCallbackData($"🇬🇧 English", $"en"),
                        InlineKeyboardButton.WithCallbackData($"🇯🇵 日本語", $"ja")
                    });

                    await _telegramBotClient.SendTextMessageAsync(
                        message.Chat.Id,
                        $"<b>This bot cast your spell to the text.</b> {Environment.NewLine}" +
                        $"You cat record your message and resend for your friend if you don't want to type it.",
                        cancellationToken: ct,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                        replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
                
                default:
                    await _telegramBotClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Send voice message to transform it to text.",
                        cancellationToken: ct);
                    break;
            }
        }
    }
}
