using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VoiceTextBot.Extentions
{
    internal static class DirectoryExtension
    {
        private static string GetSettingsFile(string path)
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
                return "";
            }
        }

        public static string GetSolutionRoot()
        {
            return Path.GetDirectoryName(GetSettingsFile(Assembly.GetExecutingAssembly().Location));
        }
    }
}
