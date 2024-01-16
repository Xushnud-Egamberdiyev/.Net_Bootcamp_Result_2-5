using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static System.Net.WebRequestMethods;

namespace Telegram_Bot_2
{
    public class System_bot
    {
        public string Token { get; set; }
        public System_bot(string token)
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

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            var handlar = update.Type switch
            {
                UpdateType.Message => HandlaMessageAsync(botClient, update, cancellationToken),
                UpdateType.EditedMessage => HandleVideoMessageAync2(botClient, update, cancellationToken),
                _ => HandlaUnkowMessageAsync(botClient, update, cancellationToken)
            };

            try
            {
                await handlar;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Chiqdi! {ex.Message}");
            }
        }

        private Task HandleVideoMessageAync2(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        async Task HandlaMessageAsync(ITelegramBotClient? botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            var handlar = message.Type switch
            {
                MessageType.Sticker => HandLeSticerMessageAsync(botClient, update, cancellationToken),
                MessageType.Text => HandlaTextMessageAsync(botClient, update, cancellationToken),
                MessageType.Video => HandleVideoMessageAync(botClient, update, cancellationToken),
                MessageType.Voice => HandleAudioMessageAsync(botClient, update, cancellationToken),
                _ => HandlaUnkowMessageAsync(botClient, update, cancellationToken)
            };
        }
        private async Task HandLeSticerMessageAsync(ITelegramBotClient? botClient, Update update, CancellationToken cancellationToken)
        {
            Message sentMessage2 = await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Sizning Stikeringiz qabul qilindi!  \n",

                    cancellationToken: cancellationToken);

            Message sentMessage = await botClient.SendStickerAsync(
                chatId: update.Message.Chat.Id,
                sticker: InputFile.FromUri("https://raw.githubusercontent.com/TelegramBots/book/master/src/docs/sticker-fred.webp"),
                     cancellationToken: cancellationToken);
        }

        private Task HandlaUnkowMessageAsync(ITelegramBotClient? botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        async Task HandleAudioMessageAsync(ITelegramBotClient? botClient, Update update, CancellationToken cancellationToken)
        {

            Message sentMessage2 = await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Sizning Audio habaringiz qabul qilindi!  \n",
                    cancellationToken: cancellationToken);

            Message sentMessage4 = await botClient.SendVoiceAsync(
                chatId: update.Message.Chat.Id,
                voice: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/audio-guitar.mp3"),
                cancellationToken: cancellationToken);
        }

        async Task HandleVideoMessageAync(ITelegramBotClient? botClient, Update update, CancellationToken cancellationToken)
        {
            Message sentMessage2 = await botClient.SendTextMessageAsync(
                   chatId: update.Message.Chat.Id,
                   text: "Sizning Videongiz qabul qilindi!  \n",
                   cancellationToken: cancellationToken);

            Message message = await botClient.SendVideoAsync(
                chatId: update.Message.Chat.Id,
                video: InputFile.FromUri("https://t.me/c/2106620367/444"),
                thumbnail: InputFile.FromUri("https://t.me/c/2106620367/444"),
                supportsStreaming: true,
                         cancellationToken: cancellationToken);
        }
        async Task HandlaTextMessageAsync(ITelegramBotClient? botClient, Update update, CancellationToken cancellationToken)
        {
            Message sentMessage2 = await botClient.SendTextMessageAsync(
                   chatId: update.Message.Chat.Id,
                   text: "Sizning Messegangiz qabul qilindi!  \n",
                   cancellationToken: cancellationToken);

            Message sentMessage4 = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Nima gapla\n" + update.Message.Text,


                cancellationToken: cancellationToken);
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
