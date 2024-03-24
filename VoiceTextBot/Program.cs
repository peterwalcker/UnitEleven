using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using VoiceTextBot.Configuration;
using VoiceTextBot.Controllers.Classes;
using VoiceTextBot.Services.Classes;
using VoiceTextBot.Services.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using VoiceTextBot.Core;

namespace VoiceTextBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Service started!");
            await host.RunAsync();
            Console.WriteLine("Service stopped!");
        }

        //temporary gag until i realize how to do it right way
        static AppSettings? BuildAppSettings()
        {
            AppSettings settings = new();

            string configFile = Path.Combine(VoiceTextBot.Extentions.DirectoryExtension.GetSolutionRoot(), "appsettings.json");
            try
            {
                using (StreamReader reader = File.OpenText(configFile))
                {
                    settings = (Configuration.AppSettings)new JsonSerializer().Deserialize(reader, typeof(AppSettings));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                settings.Token = "";
            }

            return settings;
        }

        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(appSettings);
            
            services.AddTransient<DefaultMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<VoiceMessageController>();
            services.AddTransient<InlineKeyboardController>();

            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddSingleton<IFileHandler, AudioFileHandler>();

            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.Token));
            services.AddHostedService<Bot>();
        }
    }
}
