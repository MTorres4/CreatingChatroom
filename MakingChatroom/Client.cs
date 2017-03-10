using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Chatroom
{
    class Client
    {
        public string myIP;
        public string username;
        public NetworkStream connection;
        TcpClient client;

        public Client()
        {
            this.username = PromptingForUserName();
        }


        public void GettingIpAddress()
        {
            // Retrive the Name of HOST
            string hostName = Dns.GetHostName();
            // Get the IP
            myIP = Dns.GetHostEntry(hostName).AddressList[2].ToString();
           // 192.168.0.128
        }
        public void ConnectingToServer()
        {
            //establishes connection with server
            client = new TcpClient(myIP, 2007);
            Console.WriteLine("[Trying to connect to server...]");
            //sends data to server
            connection = client.GetStream();
            Console.WriteLine("[Connected]");
        }

        public string PromptingForUserName()
        {
            Console.Write("             Welcome to Chatroom!   "
                + " \n To get started, enter a username and begin chatting!"
                + " \n To leave the chatroom, type EXIT in all caps."
                + " \n Please enter your username: ");
            string username = Console.ReadLine();
            Console.Clear();
            username += "9823";
            return (username);
        }

        public void EnteringMessage()
        {
            Task.Run(() => ReceivingMessage());
            Console.Write("Enter message here: ");
            string input = Console.ReadLine();
            byte[] message = Encoding.Unicode.GetBytes(input);
            connection.Write(message, 0, message.Length);
            string entry = Encoding.Unicode.GetString(message, 0, message.Length);
            if(entry == "EXIT")
            {
                Environment.Exit(0);
            }
            EnteringMessage();
            Console.ReadKey();
        }

        public void Sending(string generic)
        {
            byte[] message = Encoding.Unicode.GetBytes(generic);
            connection.Write(message, 0, message.Length);
        }

        public void ReceivingMessage()
        {
            try
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int data = connection.Read(buffer, 0, client.ReceiveBufferSize);
                string message = Encoding.Unicode.GetString(buffer, 0, data);
                Console.WriteLine(message);
                ReceivingMessage();
            }
            catch(Exception)
            {
                return;
            }
        }
    }
}
