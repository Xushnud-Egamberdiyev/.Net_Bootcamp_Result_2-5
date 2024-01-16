using Telegram.Bot;

namespace Telegram_Bot_2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            const string link =  ("6370570532:AAEFO9eKaIeyaZJW2Sn7oQfLSD41LpkMPBA");

            System_bot system_Bot = new System_bot(link);

            await system_Bot.BotHandle();

            try
            {
                await system_Bot.BotHandle();

            }
            catch (Exception ex)
            {
                throw new Exception("Xatoku!");
            }

        }
    }
}
