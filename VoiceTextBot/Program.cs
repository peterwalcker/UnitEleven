using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using VoiceTextBot.Core.Classes;

namespace VoiceTextBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string token = String.Empty;
            try
            {
                token = args[0];
            }
            catch
            {
                while (String.IsNullOrEmpty(token))
                {
                    Console.Write("Enter bot token: ");
                    token = Console.ReadLine();
                }
            }
            

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services, token))
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Service started!");
            await host.RunAsync();
            Console.WriteLine("Service stopped!");
        }

        static void ConfigureServices(IServiceCollection services, string token)
        {
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(token));
            services.AddHostedService<Bot>();
        }
    }
}
