using HomeworkBot.Models;
using HomeworkBot.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkBot.Services.Classes
{
    internal class SessionStorage : IStorage
    {
        private readonly ConcurrentDictionary<long, Session> _sessions = new();

        public Session GetSession(long chatId)
        {
            if(_sessions.ContainsKey(chatId))
                return _sessions[chatId];

            var newSession = new Session() { ActionType = "" };
            _sessions.TryAdd(chatId, newSession);
            return newSession;
        }
    }
}
