using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMQSender.Services.Interfaces
{
    public interface IMQSender
    {
        void SendMessage(string message);
    }
}
