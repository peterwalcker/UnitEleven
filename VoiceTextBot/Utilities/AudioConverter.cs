using FFMpegCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceTextBot.Extentions;

namespace VoiceTextBot.Utilities
{
    internal static class AudioConverter
    {
        public static void TryConvert(string inputFile, string outputFile)
        {
            GlobalFFOptions.Configure(options => options.BinaryFolder = Path.Combine(DirectoryExtension.GetSolutionRoot(), "ffmpeg-win64", "bin"));

            FFMpegArguments.FromFileInput(inputFile)
                .OutputToFile(outputFile, true, options => options.WithFastStart())
                .ProcessSynchronously();
        }
    }
}
