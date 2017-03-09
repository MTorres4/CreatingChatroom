using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Chatroom
{
    class Server : IUser
    {
        public Queue<string> messages = new Queue<string>();
        public string serverIP;
        ILoggable log;
        public TcpListener listen = new TcpListener(IPAddress.Any, 2007);
        List<TcpClient> clientList = new List<TcpClient>();



        public string username;
        private Dictionary<string, string> users = new Dictionary <string, string>();
        public Server(ILoggable log)
        {
            this.log = log;
        }

        public void AcceptClient()
        {
            // Retrieve the Name of HOST
            string hostName = Dns.GetHostName();

            // Get the IP
            serverIP = Dns.GetHostEntry(hostName).AddressList[1].ToString();

            //Listens for client to connect
            IPAddress ipAddress = IPAddress.Parse(serverIP);
            TcpListener listen = new TcpListener(IPAddress.Any, 2007);


            //accepts data from client


        }

        public void ListenForClient()
        {
            TcpClient client;
            //   Console.WriteLine("[Listening...]");
            listen.Start();
            client = listen.AcceptTcpClient();
            clientList.Add(client);
            Console.WriteLine("[Client connected]");
            Task.Run(() => clientbob(client));
            ListenForClient();
        }

        public void clientbob(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            //JustJoined();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int data = stream.Read(buffer, 0, client.ReceiveBufferSize);
            string ch = Encoding.Unicode.GetString(buffer, 0, data);
            ch.ToCharArray();
            if (ch[ch.Length - 1] == '3' && ch[ch.Length - 2] == '2' && ch[ch.Length - 3] == '8' && ch[ch.Length - 4] == '9')
            {
                Console.WriteLine("EQUALS USERNAME");
                string un = string.Concat(ch);
                users.Add(un, un);

            }
            else
            {
                string dude = string.Concat(ch);
                dude = "\n" + dude;
                AddToQueue(dude);
                SendMessage(stream);/*
                Console.WriteLine($" Message Received: {ch}");
                if (client == null)
                {
                    return;
                }*/
            }
            clientbob(client);

            // Console.ReadKey();
        }



        public void JoinChatroom()
        {
            //users.Add(user);
        }

        public void JustJoined()
        {
            NotifyUsers();
        }

        public void NotifyUsers()
        {
            foreach(KeyValuePair<string, string> kvp in users)
            {
                //user.Notify(user);
                //may not need above
                //CWL($"{user.Username} has joined the chatroom"); Need method to send for display
            }
        }

        public void WriteToLog(string receivedMessage)
        {
            log.WriteTo(receivedMessage);
        }

        public void AddToQueue(string receivedMessage)
        {
            messages.Enqueue(receivedMessage);
        }

        public void DisplayMessageInQueue()
        {
            messages.Dequeue();
        }

        public void SendMessage(NetworkStream stream)
        {
            byte[] messagee = Encoding.Unicode.GetBytes(messages.Dequeue());
            for (int i = 0; i < clientList.Count(); i++)
            {
                NetworkStream streamz = clientList[i].GetStream();
                streamz.Write(messagee, 0, messagee.Length);
            }
        }
    }
}
