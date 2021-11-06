namespace LeafEmu.World.Network
{
    using System.Net;
    using System.Net.Sockets;
    using LeafEmu.World.Database;
    internal class WorldServer
    {

        public static readonly List<listenClient> Queue = new List<listenClient>();
        public static readonly List<listenClient> CharacterInWorld = new List<listenClient>();
        List<string[]> ListOfGUID = new List<string[]>();
        public static LinkServer LinkServer;
        public static AsyncSocket.IoServer Server;
        public static LoadDataBase DataBase;
        public static readonly Dictionary<Socket, listenClient> SocketTolistenClient = new Dictionary<Socket, listenClient>();
        public static readonly List<Game.Fight.Fight> Fights = new List<Game.Fight.Fight>();
        public static readonly List<TaskCompletionSource<bool>> MethodToResume = new List<TaskCompletionSource<bool>>();
        private long GetCurrentTime => DateTimeOffset.Now.ToUnixTimeSeconds();
        private int GetIntervale => (int)(this.start + 40 - this.GetCurrentTime);

        public WorldServer(LoadDataBase database)
        {
            ThreadPool.SetMinThreads(8000, 8000);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(SaveInDatabase);
            Logger.Logger.Log("Connection To Auth...");
            LinkServer = new LinkServer(this.ListOfGUID);
            DataBase = database;
            StartQueue();
            Logger.Logger.Log("Opening Socket...");
            Server = new AsyncSocket.IoServer(10000, 1024);
            Logger.Logger.Log($"Waiting Connection. {World.WorldConfig.IP}:{World.WorldConfig.PORT}");
            new Thread(this.MainBoucleGame).Start();
            Server.Start();
        }

        long start;
        private async void MainBoucleGame()
        {
            List<List<AsyncSocket.IoServer.test>> copyPacketRecvFrame = new List<List<AsyncSocket.IoServer.test>>();
            //Dictionary<listenClient, string> copyPacketRecvFrame;
            while (true)
            {
                await this.GestionOfFight();
                this.GestionResumeMethode();
                if (Server.PacketRecvFrame.Count != 0)
                {
                    this.start = this.GetCurrentTime;
                    lock (Server.PacketRecvFrame)
                    {
                        foreach (var item in Server.PacketRecvFrame)
                        {
                            copyPacketRecvFrame.Add(item.Value);
                        }
                        Server.PacketRecvFrame.Clear();
                    }
                    var tasks = new List<Task>();
                    foreach (var instance in copyPacketRecvFrame)
                    {
                        tasks.Add(Task.Run(() => this.gestionPacketForInstance(instance)));
                    }
                    copyPacketRecvFrame.Clear();
                    await Task.WhenAll(tasks);
                }
                if (this.GetIntervale > 0)
                    await Task.Delay(this.GetIntervale);
            }
        }


        /// <summary>
        /// pauses the method and after {int} time set in the game loop to restart the method
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static async Task MethodSleep(int time)
        {
            TaskCompletionSource<bool> isSomethingLoading = new TaskCompletionSource<bool>();
            await Task.Delay(time);
            lock (MethodToResume)
            {
                MethodToResume.Add(isSomethingLoading);
            }
            await isSomethingLoading.Task;
        }

        private void GestionResumeMethode()
        {
            lock (MethodToResume)
            {
                foreach (var item in MethodToResume)
                {
                    item.SetResult(true);
                }
                MethodToResume.Clear();
            }
        }

        private async Task GestionOfFight()
        {
            var tasks = new List<Task>();
            List<Game.Fight.Fight> copyFights;
            lock (Fights)
            {
                copyFights = new List<Game.Fight.Fight>(Fights);
                Fights.Clear();
            }
            foreach (var fight in copyFights)
            {
                if (fight.ActionToDoAtFrame == 0)
                    tasks.Add(Task.Run(() => fight.GestionTurn()));
                else
                    tasks.Add(Task.Run(() => fight.GestionEndTurn()));
            }
            await Task.WhenAll(tasks);
        }

        private void gestionPacketForInstance(List<AsyncSocket.IoServer.test> PacketRecvFrame)
        {
            foreach (var item in PacketRecvFrame)
            {
                foreach (string packet in item.packet.Replace("\x0a", string.Empty).Split('\0').Where(x => x != string.Empty))
                {
                    // Logger.Logger.Log(packet);
                    // packetReceivedEvent?.Invoke(packet);
                    try
                    {
                        PacketGestion.PacketGestion.Gestion(item.li, packet);
                        if (GetIntervale <= 0) return;
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }
        }

        private static void SaveInDatabase(object sender, EventArgs e)
        {
            foreach (listenClient client in CharacterInWorld)
            {
                client.database.tablecharacter.updateDatabase(client);
            }
        }


        private static void StartQueue()
        {
            Thread waitingQueue = new Thread(wait_queue);
            Thread senfAF = new Thread(send_Af);

            waitingQueue.Priority = ThreadPriority.BelowNormal;
            waitingQueue.Start();

            senfAF.Priority = ThreadPriority.Lowest;
            senfAF.Start();
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
                    for (int i = 0; i < Queue.Count - 5; i++)
                    {
                        if (i < 0)
                            break;
                        Queue[i].send($"Af{i + 1}");
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
            WorldServer.Queue.Add(prmClient);
            waitHandle.Set();
        }

        private static async void wait_queue()
        {
            while (true)
            {
                // waitHandle.Reset();
                if (Queue.Count > 0)
                {
                    listenClient queueClient = Queue[0];
                    if (queueClient != null)
                    {
                        if (queueClient.isCo)
                        {
                            DataBase.tablecharacter.LoadCharacter(queueClient);
                            queueClient.send("ATK0");
                            // queueClient.send("ALK10|0");
                            queueClient.DbLoad = true;
                            var temp = (IPEndPoint)queueClient.ClientSocket.RemoteEndPoint;

                            Logger.Logger.Log($"Client {queueClient.account.ID} Connected {temp.Address}:{temp.Port}");
                        }
                        else
                        {
                            queueClient.database.tablecharacter.updateDatabase(queueClient);
                        }
                    }

                    Queue.Remove(queueClient);
                }

                // waitHandle.WaitOne(10000, true);
                await Task.Delay(0);

                // (Math.Clamp(155 * queue.Count, 155, 700));
            }
        }

        public static void RemoveClient(listenClient li)
        {
            Logger.Logger.Log("Client Deconnected");
            CharacterInWorld.Remove(li);
            SocketTolistenClient.Remove(li.ClientSocket);
            Queue.Remove(li);
            li.remove(li);
            AddToQueue(li);
        }
    }

}

