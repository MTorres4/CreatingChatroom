using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Chatroom
{
    class Server
    {
        public Queue<string> messages = new Queue<string>();
        ILoggable log;
        public TcpListener listen = new TcpListener(IPAddress.Any, 2007);
        private Dictionary<TcpClient, string> users = new Dictionary<TcpClient, string>();
        public Server(ILoggable log)
        {
            this.log = log;
        }

        public void ListeningForClient()
        {
            TcpClient client;
            listen.Start();
            client = listen.AcceptTcpClient();
            Console.WriteLine("[Client connected]");
            Task.Run(() => ConvertingData(client));
            ListeningForClient();
        }

        public void ConvertingData(TcpClient client)
        {
            string message = "";
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int data = stream.Read(buffer, 0, client.ReceiveBufferSize);
                string convertedmessage = Encoding.Unicode.GetString(buffer, 0, data);
                convertedmessage.ToCharArray();
                if (convertedmessage[convertedmessage.Length - 1] == '3' && convertedmessage[convertedmessage.Length - 2] == '2' && convertedmessage[convertedmessage.Length - 3] == '8' && convertedmessage[convertedmessage.Length - 4] == '9')
                {
                    JoiningChatroom(convertedmessage, client);
                }
                else if (convertedmessage[convertedmessage.Length - 1] == 'T' && convertedmessage[convertedmessage.Length - 2] == 'I' && convertedmessage[convertedmessage.Length - 3] == 'X' && convertedmessage[convertedmessage.Length - 4] == 'E')
                {
                    ExitingChatroom(client);
                }
                else
                {
                    message = string.Concat(convertedmessage);
                    for (int i = 0; i < users.Count(); i++)
                    {
                        if (users.ElementAt(i).Key == client)
                        {
                            message = ($"\n{users.ElementAt(i).Value} says: {message}");
                        }
                    }
                }
            }

            catch (Exception)
            {
                if (stream == null)
                {
                    return;
                }
            }
            WritingToLog(message);
            AddingToQueue(message);
            SendingMessage();
            ConvertingData(client);
        }

        public void JoiningChatroom(string convertedmessage, TcpClient client)
        {
            Console.WriteLine("Username saved");
            string username = string.Concat(convertedmessage);
            users.Add(client, username);
            JustJoiningChatroom(username, client);
        }

        public void JustJoiningChatroom(string username, TcpClient client)
        {
            NotifyingUsers(username, 1, client);
        }

        public void ExitingChatroom(TcpClient client)
        {
            NotifyingUsers("", 2, client);
        }

        private void NotifyingUsers(string username, int notification, TcpClient client)
        {
            string notify = "";
            if(notification == 1)
            {
                notify = ($" \n{username} has joined the chatroom");
            }
            else
            {
                for (int i = 0; i < users.Count(); i++)
                {
                    if (users.ElementAt(i).Key == client)
                    {
                        notify = ($" \n{users.ElementAt(i).Value} has left the chatroom");
                    }
                }
            }
            WritingToLog(notify);
            AddingToQueue(notify);
            SendingMessage();
        }

        public void WritingToLog(string receivedMessage)
        {
            log.WriteTo(receivedMessage);
        }

        public void AddingToQueue(string receivedMessage)
        {
            messages.Enqueue(receivedMessage);
        }

        public string DisplayingMessageInQueue()
        {
            return messages.Dequeue();
        }

        public void SendingMessage()
        {
            byte[] message = Encoding.Unicode.GetBytes(DisplayingMessageInQueue());
            for (int i = 0; i < users.Count(); i++)
            {
                try
                {
                    NetworkStream stream = users.ElementAt(i).Key.GetStream();
                    stream.Write(message, 0, message.Length);
                }
                catch
                {
                    Console.WriteLine($" {users.ElementAt(i).Value} is no longer online.");
                    users.Remove(users.ElementAt(i).Key);
                        continue;
                }
            }
        }
    }
}
