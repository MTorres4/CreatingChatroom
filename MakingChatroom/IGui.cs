using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom
{
    public interface IGui
    {
        string Username { get; }
        void DisplayNotification();
        string MessageLog { get; }
    }
}
