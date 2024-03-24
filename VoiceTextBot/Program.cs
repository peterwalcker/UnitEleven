using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Telegram.Bot;
using VoiceTextBot.Configuration;
using VoiceTextBot.Controllers.Classes;
using VoiceTextBot.Core.Classes;
using VoiceTextBot.Services.Classes;
using VoiceTextBot.Services.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace VoiceTextBot
{
    internal class Program
    {
        private readonly IConfiguration _configuration;
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
        static AppSettings BuildAppSettings()
        {
            AppSettings settings = new AppSettings();

            string configFile = GetSettingsFile(Assembly.GetExecutingAssembly().Location);

            using (StreamReader reader = File.OpenText(configFile))
            {
                settings = (Configuration.AppSettings)new Newtonsoft.Json.JsonSerializer().Deserialize(reader, typeof(AppSettings));
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

            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.Token));
            services.AddHostedService<Bot>();
        }

        static string GetSettingsFile(string path)
        {
            try
            {
                FileInfo info = new(path);
                DirectoryInfo dir = info.Directory;
                string searchPattern = Path.Combine(path, "appsettings.json");

                if (!File.Exists(searchPattern))
                {
                    searchPattern = GetSettingsFile(dir.Parent.FullName);
                }
                return searchPattern;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
