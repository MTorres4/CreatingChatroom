﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom
{
    public class CProgram
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            client.GetIpAddress();
            client.ConnectToServer();
            client.send(client.Username);
            client.EnterMessage();

        }

    }
}
