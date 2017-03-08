using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Chatroom
{
    class Client : IGui
    {
        public string myIP;
        private string username;
        private string messagelog;
        public string Username
        {
            get
            {
                return username;
            }
        }

        public string MessageLog
        {
            get
            {
                return messagelog;
            }
        }

        public Client()
        {

        }


        public void GetIpAddress()
        {
            // Retrive the Name of HOST
            string hostName = Dns.GetHostName();
            // Get the IP
            myIP = Dns.GetHostEntry(hostName).AddressList[1].ToString();
        }
        public void ConnectToServer()
        {
            //establishes connection with server
            TcpClient client = new TcpClient("192.168.0.128", 2007);
            Console.WriteLine("[Trying to connect to server...]");
            //sends data to server
            NetworkStream n = client.GetStream();
            Console.WriteLine("[Connected]");
            send(n, "Lao");
            Console.WriteLine("--------------------sent---------------");
            client.Close();
            Console.ReadKey();
        }

        public void send(NetworkStream n, string generic)
        {
            byte[] message = Encoding.Unicode.GetBytes(generic);
            n.Write(message, 0, message.Length);
        }

        public void DisplayNotification()
        {
            Console.WriteLine($"{Username} has joined the chatroom...");
        }
    }
}
