using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceTextBot.Models;
using VoiceTextBot.Services.Interfaces;

namespace VoiceTextBot.Services.Classes
{
    internal class MemoryStorage : IStorage
    {
        private readonly ConcurrentDictionary<long, Session> _sessions;

        public MemoryStorage()
        {
            _sessions = new ConcurrentDictionary<long, Session>();
        }
        
        public Session GetSession(long chatId)
        {
            if (_sessions.ContainsKey(chatId)) 
                return _sessions[chatId];

            var newSession = new Session() { LanguageCode = "ru" };
            _sessions.TryAdd(chatId, newSession);
            return newSession;
        }
    }
}
