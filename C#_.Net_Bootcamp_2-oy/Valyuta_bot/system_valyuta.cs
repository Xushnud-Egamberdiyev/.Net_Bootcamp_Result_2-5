using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Valyuta_bot
{
    public class System_valyuta
    {
        public string Token { get; set; }
        public System_valyuta(string token)
        {
            this.Token = token;
        }

        public async Task BotHandle()
        {
            var botClient = new TelegramBotClient($"{this.Token}");

            using CancellationTokenSource cts = new();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();

        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;



            var chatId = message.Chat.Id;


            Console.WriteLine($"Received a '{messageText}' message in chat {chatId} --> {message.Chat.Username}.");

            // Echo received message text
            if (messageText.ToUpper() == "/START")
            {
                Message sentMessage4 = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Savollaringiz bolsa bersaz boladi!  \n",

                cancellationToken: cancellationToken);
            }

            if (messageText.ToUpper() != "/START")
            {
                

                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Savolingiz qabul qilingi \n",

                    cancellationToken: cancellationToken);

                Message sentMessage2 = await botClient.SendStickerAsync(
                    chatId: chatId,
                    sticker: InputFile.FromUri("https://raw.githubusercontent.com/TelegramBots/book/master/src/docs/sticker-fred.webp"),

                    cancellationToken: cancellationToken);

                Message sentMessage3 = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: message.From.LastName + " " + message.From.FirstName + "\nSizning savolingiz korib chiqiladi admin tarafdan",

                    cancellationToken: cancellationToken);
            }


        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
