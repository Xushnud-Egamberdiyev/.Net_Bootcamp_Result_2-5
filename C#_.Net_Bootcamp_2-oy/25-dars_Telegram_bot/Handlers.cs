using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBot
{
    internal class Handlers
    {
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message!),
                _ => UnknownUpdateHandlerAsync(botClient, update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            Console.WriteLine($"Receive message type: {message.Type}");

            if (message.Type == MessageType.Contact && message.Contact != null)
            {
                // TODO: save the number...
                Console.WriteLine($"Phone number: {message.Contact.PhoneNumber}");
            }

            if (message.Type != MessageType.Text)
                return;

            var action = message.Text!.Split(' ')[0] switch
            {
                "/myNumber" => RequestContactAndLocation(botClient, message),
                _ => Usage(botClient, message)
            };
            Message sentMessage = await action;
            Console.WriteLine($"The message was sent with id: {sentMessage.MessageId}");


            static async Task<Message> RequestContactAndLocation(ITelegramBotClient botClient, Message message)
            {
                ReplyKeyboardMarkup requestReplyKeyboard = new(
                    new[]
                    {
                // KeyboardButton.WithRequestLocation("Location"), // this for the location if you need it
                KeyboardButton.WithRequestContact("Send my phone Number"),
                    });

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Could you please send your phone number?",
                                                            replyMarkup: requestReplyKeyboard);
            }

            static async Task<Message> Usage(ITelegramBotClient botClient, Message message)
            {
                const string usage = "/myNumber  - to send your phone number";

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: usage,
                                                            replyMarkup: new ReplyKeyboardRemove());
            }
        }

        private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }
    }
}
