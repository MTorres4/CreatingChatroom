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
        public NetworkStream n;
        TcpClient client;
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
            this.username = PromptUserName();
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
            client = new TcpClient(myIP, 2007);
            Console.WriteLine("[Trying to connect to server...]");
            //sends data to server
            n = client.GetStream();
            Console.WriteLine("[Connected]");
        }

        public string PromptUserName()
        {
            Console.Write("Please enter your username: ");
            string username = Console.ReadLine();
            username += "9823";
            return (username);
        }

        public void EnterMessage()
        {
            Console.Write("Enter message here: ");
            string dog = Console.ReadLine();
            byte[] message = Encoding.Unicode.GetBytes(dog);
            n.Write(message, 0, message.Length);
            Task.Run(() => ReceiveMessage());
            //send(n, "Lao");
            EnterMessage();
            //client.Close();
            Console.ReadKey();
        }

        public void send(string generic)
        {
            byte[] message = Encoding.Unicode.GetBytes(generic);
            n.Write(message, 0, message.Length);
        }

        public void SendReceive()
        {
            EnterMessage();
        }

        public void ReceiveMessage()
        {

            byte[] buffer = new byte[client.ReceiveBufferSize];
            int data = n.Read(buffer, 0, client.ReceiveBufferSize);
            string ch = Encoding.Unicode.GetString(buffer, 0, data);
            Console.WriteLine($"message received: {ch}");
            ReceiveMessage();
        }

        public void DisplayNotification()
        {
            Console.WriteLine($"{Username} has joined the chatroom...");
        }
    }
}
