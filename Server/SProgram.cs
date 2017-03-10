using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom
{
    class SProgram
    {
        static void Main(string[] args)
        {
            Log log = new Log();
            Server server = new Server(log);
            server.ListeningForClient();
        }
    }
}
