namespace Valyuta_bot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            const string link = ("6722084986:AAFw2-B8a9zAURnbury3OcmZZXq4yObZv3c");

            System_valyuta system_Valyuta = new System_valyuta(link);

            await system_Valyuta.BotHandle();

            try
            {
                await system_Valyuta.BotHandle();

            }
            catch (Exception ex)
            {
                throw new Exception("Xatoku!");
            }
        }
    }
}
