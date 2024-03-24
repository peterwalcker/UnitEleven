using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using VoiceTextBot.Core.Classes;

namespace VoiceTextBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services, args[0]))
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
