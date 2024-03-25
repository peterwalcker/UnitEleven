using HomeworkBot.Configuration;
using HomeworkBot.Controllers.Classes;
using HomeworkBot.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Newtonsoft.Json;
using System.Reflection;
using HomeworkBot.Services.Classes;
using HomeworkBot.Services.Interfaces;

namespace HomeworkBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Bot starting!");

            await host.RunAsync();

            Console.WriteLine("Bot stopped!");
        }

        static AppSettings? BuildAppSettings()
        {
            AppSettings settings = new();

            string configFile = HomeworkBot.Extensions.DirectoryExtension.GetSettingsFile(Assembly.GetExecutingAssembly().Location);
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

            services.AddTransient<DefaultMessageController> ();
            services.AddTransient<TextMessageController> ();
            services.AddTransient<InlineKeyboardController> ();

            services.AddSingleton<IStorage, SessionStorage> ();

            services.AddSingleton<ITelegramBotClient> (provider => new TelegramBotClient(appSettings.Token));
            services.AddHostedService<Bot>();
        }
    }
}
