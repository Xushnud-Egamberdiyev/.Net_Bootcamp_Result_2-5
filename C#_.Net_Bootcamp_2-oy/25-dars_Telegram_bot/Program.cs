using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TelegramBot;

public static class Program
{
    private static TelegramBotClient? bot;

    public static async Task Main()
    {

        var bot = new TelegramBotClient("6898287495:AAFhPWS3wyDVaiFBQaJcDNeSZTe681KrHak");

        User me = await bot.GetMeAsync();
        Console.Title = me.Username ?? "My awesome bot";

        using var cts = new CancellationTokenSource();

        ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
        bot.StartReceiving(Handlers.HandleUpdateAsync,
            Handlers.HandleErrorAsync,
            receiverOptions,
            cts.Token);

        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();

        cts.Cancel();
    }
}
//using Telegram.Bot;
//using Telegram.Bot.Exceptions;
//using Telegram.Bot.Polling;
//using Telegram.Bot.Types.Enums;
//using Telegram.Bot.Types;
//using static System.Net.WebRequestMethods;

//var botClient = new TelegramBotClient("6332306275:AAGI4-wfyfvaP-ldH-YWFDzSIbG1KpgZ_Ew");


//using CancellationTokenSource cts = new();

//// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
//ReceiverOptions receiverOptions = new()
//{
//    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
//};

//botClient.StartReceiving(
//    updateHandler: HandleUpdateAsync,
//    pollingErrorHandler: HandlePollingErrorAsync,
//    receiverOptions: receiverOptions,
//    cancellationToken: cts.Token
//);

//var me = await botClient.GetMeAsync();

//Console.WriteLine($"Start listening for @{me.Username}");
//Console.ReadLine();

//// Send cancellation request to stop bot
//cts.Cancel();

//async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
//{
//    // Only process Message updates: https://core.telegram.org/bots/api#message
//    if (update.Message is not { } message)
//        return;
//    // Only process text messages
//    if (message.Text is not { } messageText)
//        return;

//    var chatId = message.Chat.Id;

//    if (messageText.ToUpper() == "/START")
//    {
//        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

//        Message messages = await botClient.SendContactAsync(
//         chatId: chatId,
//         phoneNumber: "+998 93 188 5213",
//         firstName: message.From.FirstName,
//         lastName: message.From.LastName,
//         cancellationToken: cancellationToken);


//    }

//}





//Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
//{
//    var ErrorMessage = exception switch
//    {
//        ApiRequestException apiRequestException
//            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
//        _ => exception.ToString()
//    };

//    Console.WriteLine(ErrorMessage);
//    return Task.CompletedTask;
//}
