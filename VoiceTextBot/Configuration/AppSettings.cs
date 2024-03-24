using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceTextBot.Configuration
{
    internal class AppSettings
    {
        public string Token { get; set; }

        public string DownloadsFolder { get; set; }

        public string AudioFileName { get; set; }

        public string InputAudioFormat { get; set; }
        
        public string OutputAudioFormat { get; set; }

        public float InputBitrate { get; set; }
    }
}
