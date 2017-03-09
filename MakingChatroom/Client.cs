﻿using System;
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
            username +="9823";
            return (username);
        }

        public void EnterMessage()
        {
            Console.Write("Enter your message: ");
            string dog = Console.ReadLine();
            byte[] message = Encoding.Unicode.GetBytes(dog);
            n.Write(message, 0, message.Length);
            Console.Write("Enter your message 2: ");
            string doge = Console.ReadLine();
            byte[] messagee = Encoding.Unicode.GetBytes(doge);
            n.Write(messagee, 0, messagee.Length);
            //send(n, "Lao");
            Console.WriteLine("--------------------sent---------------");
            client.Close();
            Console.ReadKey();
        }

        public void send(string generic)
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
