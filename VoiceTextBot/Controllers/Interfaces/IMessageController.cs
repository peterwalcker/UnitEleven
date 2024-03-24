using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceTextBot.Controllers.Interfaces
{
    internal interface IMessageController<T>
    {
        public abstract Task Handle(T? recievedObject, CancellationToken ct);
    }
}
