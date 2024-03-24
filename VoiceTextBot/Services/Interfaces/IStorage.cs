using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceTextBot.Models;

namespace VoiceTextBot.Services.Interfaces
{
    internal interface IStorage
    {
        Session GetSession(long chatId);
    }
}
