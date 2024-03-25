using HomeworkBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkBot.Services.Interfaces
{
    internal interface IStorage
    {
        Session GetSession(long chatId);
    }
}
