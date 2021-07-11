using LeafEmu.Auth.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LeafEmu.Auth.Network
{
    class AuthServer
    {
        List<int> ListIDAccount = new List<int>();
        List<listenClient> queue = new List<listenClient>();
        LoadDataBase DataBase;
        LinkServer linkServer;
        public AuthServer(Database.LoadDataBase _database)
        {
            Logger.Logger.Log("Connection To World...");
            linkServer = new LinkServer(ListIDAccount);
            DataBase = _database;
            Logger.Logger.Log("Opening Socket...");
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            server.Bind(new IPEndPoint(IPAddress.Parse(AuthConfig.IP), AuthConfig.PORT));
            server.Listen(5);
            new Thread(wait_queue).Start();
            Logger.Logger.Log($"Waiting Connection. {AuthConfig.IP}:{AuthConfig.PORT}");
            //new Thread(AcceptConnection).Start(server);
            AcceptConnection(server);

        }

        private void AcceptConnection(object o)
        {
            Socket server = (Socket)o;

            while (true)
            {
                try
                {
                    Socket socketClient = server.Accept();
                    listenClient li = new listenClient(socketClient, DataBase, queue, linkServer);
                    new Thread(ThreadlistenClient).Start(li);
                }
                catch (Exception)
                {
                    return;
                }

            }

        }
        private void wait_queue()
        {
            bool PacketAxSend;
            while (true)
            {
                if (queue.Count > 0)
                {
                    PacketAxSend = false;
                    listenClient var = queue[0];
                    new connection.Authentication().CheckLogin(var);
                    if (var != null && !var.isCo)
                    {
                        var.ClientSocket.Close();
                        queue.Remove(var);
                        continue;
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        if (queue.Count == 0 || queue[0] != var)
                        {
                            PacketAxSend = true;
                            break;
                        }
                        Thread.Sleep(80);
                    }
                    if (!PacketAxSend)
                    {
                        queue.Remove(var);
                    }
                }
            }
        }

        private void ThreadlistenClient(object o)
        {
            listenClient li = (listenClient)o;
            try
            {
                li.startlisten();
            }
            catch (Exception ex)
            {
                if (li.account.ID != -1)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(ex);
                    string exeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string path = exeDir + "\\log\\" + li.account.ID + "_log.txt";
                    using (StreamWriter sw = new StreamWriter(path, true))
                    {
                        sw.WriteLine(ex + "\n");
                    }


                    File.AppendAllText(path, sb.ToString());
                    sb.Clear();
                }
            }
            Logger.Logger.Log("Client Deconnected");
            li.linkServer.RemoveAccount(li.account.ID);
            queue.Remove(li);

        }



    }

}

