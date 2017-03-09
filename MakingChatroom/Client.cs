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
        public NetworkStream n;
        TcpClient client;
        public string Username
        {
            get
            {
                return username;
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
            myIP = Dns.GetHostEntry(hostName).AddressList[2].ToString();
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
            Task.Run(() => ReceiveMessage());
            Console.Write("Enter message here: ");
            string dog = Console.ReadLine();
            byte[] message = Encoding.Unicode.GetBytes(dog);
            n.Write(message, 0, message.Length);

            string ch = Encoding.Unicode.GetString(message, 0, message.Length);
            if(ch == "EXIT")
            {
                client.Close();
                Environment.Exit(0);
            }

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
            try
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int data = n.Read(buffer, 0, client.ReceiveBufferSize);
                string ch = Encoding.Unicode.GetString(buffer, 0, data);
                Console.WriteLine(ch);
                ReceiveMessage();
            }
            catch(Exception)
            {
                //Console.WriteLine("User not online!");
                return;
            }
        }

        public void DisplayNotification()
        {
            Console.WriteLine($"{Username} has joined the chatroom...");
        }
    }
}
