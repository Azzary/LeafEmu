using LeafEmu.World.Database;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;


namespace LeafEmu.World.Network
{
    class WorldServer
    {

        public static List<listenClient> queue = new List<listenClient>();
        public static List<listenClient> CharacterInWorld = new List<listenClient>();
        List<string[]> ListOfGUID = new List<string[]>();
        public static LinkServer linkServer;
        public static AsyncSocket.IoServer Server;
        public static LoadDataBase DataBase;
        public static Dictionary<Socket, listenClient> SocketTolistenClient = new Dictionary<Socket, listenClient>();

        public WorldServer(LoadDataBase _database)
        {
            ThreadPool.SetMinThreads(8000, 8000);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(save_in_database);
            Logger.Logger.Log("Connection To Auth...");
            linkServer = new LinkServer(ListOfGUID);
            DataBase = _database;
            Start_Queue();
            Logger.Logger.Log("Opening Socket...");
            Server = new AsyncSocket.IoServer(10000, 1024);
            Logger.Logger.Log($"Waiting Connection. {World.WorldConfig.IP}:{World.WorldConfig.PORT}");
            Server.Start();
        }


        private static void save_in_database(object sender, EventArgs e)
        {
            foreach (listenClient client in CharacterInWorld)
            {
                client.database.tablecharacter.updateDatabase(client);
            }
        }


        public static void Start_Queue()
        {
            Thread WaitingQueue = new Thread(wait_queue);
            Thread SenfAF = new Thread(send_Af);

            WaitingQueue.Priority = ThreadPriority.BelowNormal;
            WaitingQueue.Start();

            SenfAF.Priority = ThreadPriority.Lowest;
            SenfAF.Start();
        }



        public static readonly EventWaitHandle waitHandle = new AutoResetEvent(false);

        /// <summary>
        /// Moche
        /// </summary>
        private static async void send_Af()
        {
            while (true)
            {
                //waitHandle.WaitOne(1000000, true);
                try
                {
                    for (int i = 0; i < queue.Count - 5; i++)
                    {
                        if (i < 0)
                            break;
                        queue[i].send($"Af{i + 1}");
                    }
                }
                catch (Exception)
                {

                }

                await Task.Delay(1000);
            }
        }
        public static void AddToQueue(listenClient prmClient)
        {
            WorldServer.queue.Add(prmClient);
            waitHandle.Set();
        }

        private static async void wait_queue()
        {
            while (true)
            {
                //waitHandle.Reset();
                if (queue.Count > 0)
                {
                    listenClient queueClient = queue[0];
                    if (queueClient != null)
                        if (queueClient.isCo)
                        {
                            DataBase.tablecharacter.LoadCharacter(queueClient);
                            queueClient.send("ATK0");
                            queueClient.send("ALK10|0");
                            queueClient.DbLoad = true;
                            var temp = (IPEndPoint)queueClient.ClientSocket.RemoteEndPoint;

                            Logger.Logger.Log($"Client {queueClient.account.ID} Connected {temp.Address}:{temp.Port}");
                        }
                        else
                        {
                            queueClient.database.tablecharacter.updateDatabase(queueClient);
                        }
                    queue.Remove(queueClient);
                }
                //waitHandle.WaitOne(10000, true);
                await Task.Delay(Math.Clamp(155 * queue.Count, 155, 700));
            }
        }

        public static void removeClient(listenClient li)
        {
            Logger.Logger.Log("Client Deconnected");
            CharacterInWorld.Remove(li);
            SocketTolistenClient.Remove(li.ClientSocket);
            queue.Remove(li);
            li.remove(li);
            AddToQueue(li);
        }
    }

}

