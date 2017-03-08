using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom
{
    interface ILoggable
    {
        List<string> MessageLog { get; set; }
        void WriteTo(string input);
    }
}
