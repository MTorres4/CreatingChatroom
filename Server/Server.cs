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
        private Dictionary<TcpClient, string> users = new Dictionary<TcpClient, string>();
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
            Console.WriteLine("[Client connected]");
            Task.Run(() => clientbob(client));
            ListenForClient();
        }

        public void clientbob(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            try
            {
                int data = stream.Read(buffer, 0, client.ReceiveBufferSize);
                string ch = Encoding.Unicode.GetString(buffer, 0, data);
                ch.ToCharArray();
                if (ch[ch.Length - 1] == '3' && ch[ch.Length - 2] == '2' && ch[ch.Length - 3] == '8' && ch[ch.Length - 4] == '9')
                {
                    Console.WriteLine("EQUALS USERNAME");
                    string un = string.Concat(ch);
                    users.Add(client, un);
                    JustJoined(un, stream, client);
                }
                else if (ch[ch.Length - 1] == 'T' && ch[ch.Length - 2] == 'I' && ch[ch.Length - 3] == 'X' && ch[ch.Length - 4] == 'E')
                {
                    string un = string.Concat(ch);
                    ExitChatRoom(un, stream, client);
                }
                else
                {
                    string dude = string.Concat(ch);
                    for (int i = 0; i < users.Count(); i++)
                    {
                        if (users.ElementAt(i).Key == client)
                        {
                            dude = ($"\n{users.ElementAt(i).Value} says: {dude}");
                        }
                    }
                    log.WriteTo(dude);
                    AddToQueue(dude);
                    SendMessage(stream);
                }
                clientbob(client);
            }
            catch(Exception)
            {
                //Console.WriteLine("User not online!");
                return;
            }
        }

        public void JoinChatroom()
        {
            //users.Add(user);
        }
        
        public void ExitChatRoom(string un, NetworkStream stream, TcpClient client)
        {
            NotifyUsers(un, stream,2, client);

        }

        public void JustJoined(string un, NetworkStream stream, TcpClient client)
        {
            NotifyUsers(un, stream,1,client);
        }

        public void NotifyUsers(string un, NetworkStream stream, int notification, TcpClient client)
        {
            string notify= "";
            if(notification == 1)
            {
                notify = ($" \n{un} has joined the chatroom");
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
            WriteToLog(notify);
            AddToQueue(notify);
            SendMessage(stream);
                //Need method to send for display
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
            for (int i = 0; i < users.Count(); i++)
            {
                NetworkStream streamz = users.ElementAt(i).Key.GetStream();
                try
                {
                    streamz.Write(messagee, 0, messagee.Length);
                }
                catch
                {
                    Console.WriteLine($" {users.ElementAt(i).Value} is no longer online.");
                    continue;
                }
            }
        }
    }
}
