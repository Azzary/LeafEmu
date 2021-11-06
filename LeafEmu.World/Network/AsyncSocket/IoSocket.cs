using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LeafEmu.World.Network.AsyncSocket
{

    /// <summary>
    /// Implementation of IOCP Server Based on Socket AsyncEventArgs
    /// </summary>
    internal sealed class IoServer
    {
        /// <summary>
        /// Listen on Socket for receiving client connection requests
        /// </summary>
        private Socket listenSocket;

        /// <summary>
        /// Mutual Exclusive Synchronization Objects for Server Execution
        /// </summary>
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        /// <summary>
        /// Buffer size for each I/O Socket operation
        /// </summary>
        private Int32 bufferSize;

        /// <summary>
        /// Total number of clients connected on the server
        /// </summary>
        private Int32 numConnectedSockets;

        /// <summary>
        /// Maximum number of connections that the server can accept
        /// </summary>
        private Int32 numConnections;

        /// <summary>
        /// IoContext Object Pool for Delivery on Completion Port
        /// </summary>
        private IoContextPool ioContextPool;


        /// <summary>
        /// Constructor to create an uninitialized server instance
        /// </summary>
        /// <param name="num Connections">Maximum connection data for the server </param>
        /// <param name="bufferSize"></param>
        internal IoServer(Int32 numConnections, Int32 bufferSize)
        {
            this.numConnectedSockets = 0;
            this.numConnections = numConnections;
            this.bufferSize = bufferSize;

            this.ioContextPool = new IoContextPool(numConnections);

            // Pre-assign SocketAsyncEventArgs objects for IoContextPool
            for (Int32 i = 0; i < this.numConnections; i++)
            {
                SocketAsyncEventArgs ioContext = new SocketAsyncEventArgs();
                ioContext.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
                ioContext.SetBuffer(new Byte[this.bufferSize], 0, this.bufferSize);

                // Add pre-allocated objects to the SocketAsyncEventArgs object pool
                this.ioContextPool.Add(ioContext);
            }
        }

        /// <summary>
        /// Call this function when the send or receive request on the Socket is completed
        /// </summary>
        /// <param name="sender">the object that triggers the event </param>
        /// <param name="e">SocketAsyncEventArg object </param> associated with the completion of the send or receive operation
        private void OnIOCompleted(object sender, SocketAsyncEventArgs e)
        {
            // Determine which type of operation just completed and call the associated handler.
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    this.ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    this.ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        public class test
        {
            public listenClient li;
            public string packet;
            public test(listenClient li, string packet)
            {
                this.li = li;
                this.packet = packet;
            }
        }

        public Dictionary<int, List<test>> PacketRecvFrame = new Dictionary<int, List<test>>();

        public void AddActionInQueue(listenClient li, string packets)
        {
            lock (PacketRecvFrame)
            {
                if (li.account.statue <= 0)
                {
                    if (!PacketRecvFrame.ContainsKey(-1))
                        PacketRecvFrame.Add(-1, new List<test>());
                    PacketRecvFrame[-1].Add(new test(li, packets));
                }
                else
                {
                    if (!PacketRecvFrame.ContainsKey(li.account.character.mapID))
                        PacketRecvFrame.Add(li.account.character.mapID, new List<test>());
                    PacketRecvFrame[li.account.character.mapID].Add(new test(li, packets));
                }
            }
        }

        /// <summary>
        /// Receiving Completion Processing Function
        /// </summary>
        /// <param name="e">SocketAsyncEventArg object </param> associated with the receive completion operation
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            // Check whether the remote host closes the connection
            if (e.BytesTransferred > 0)
            {
                if (e.SocketError == SocketError.Success)
                {
                    Socket s = (Socket)e.UserToken;
                    //Determine whether all data to be received has been completed
                    if (true)//s.Available == 0 || !WorldServer.SocketTolistenClient.ContainsKey(s))
                    {
                        // Setting up send data
                        //Array.Copy(e.Buffer, 0, e.Buffer, e.BytesTransferred, e.BytesTransferred);
                        //e.SetBuffer(e.Offset, e.BytesTransferred * 2);
                        string packets = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);
                        listenClient li = WorldServer.SocketTolistenClient[s];
                        li.SocketAsyncEvent = e;
                        AddActionInQueue(li, packets);
                    }
                    try
                    {
                        if (!s.ReceiveAsync(e))    //In order to receive the next piece of data and send the receiving request, this function may be completed synchronously, and then return false, without triggering the SocketAsyncEventArgs.Completed event.
                        {
                            // Processing Receive Completion Events while Synchronizing Receive
                            this.ProcessReceive(e);
                        }
                    }
                    catch (Exception)
                    {
                        this.CloseClientSocket(e);
                    }
                }
                else
                {
                    this.ProcessError(e);
                }
            }
            else
            {
                this.CloseClientSocket(e);
            }
        }

        public void Send(Socket handler, String data)
        {
            try
            {
                byte[] byteData = Encoding.ASCII.GetBytes(data);
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
            }
            catch (Exception e)
            {
                this.ProcessError(null);
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.  
            Socket handler = (Socket)ar.AsyncState;
            // Complete sending the data to the remote device.  
            int bytesSent = handler.EndSend(ar);
        }

        /// <summary>
        /// Send Completion Time Processing Function
        /// </summary>
        /// <param name="e">SocketAsyncEventArg object </param> associated with the send completion operation
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Socket s = (Socket)e.UserToken;

                //When receiving, the size of the buffer is reduced according to the number of bytes received, so the size of the buffer is restored when the receiving request is delivered.
                e.SetBuffer(0, bufferSize);
                if (!s.ReceiveAsync(e))     //Delivery and reception of requests
                {
                    // Processing Receive Completion Events while Synchronizing Receive
                    this.ProcessReceive(e);
                }
            }
            else
            {
                this.ProcessError(e);
            }
        }

        /// <summary>
        /// Handling socket errors
        /// </summary>
        /// <param name="e"></param>
        private void ProcessError(SocketAsyncEventArgs e)
        {
            if (e == null)
                return;
            Socket s = e.UserToken as Socket;
            IPEndPoint localEp = s.LocalEndPoint as IPEndPoint;

            this.CloseClientSocket(s, e);
            string outStr = String.Format("socket error {0}, IP {1}, operation {2}. ", (Int32)e.SocketError, localEp, e.LastOperation);
            Logger.Logger.Warning(outStr);
        }

        /// <summary>
        /// Close socket connection
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the completed send/receive operation.</param>
        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            Socket s = e.UserToken as Socket;
            this.CloseClientSocket(s, e);
        }

        private void CloseClientSocket(Socket s, SocketAsyncEventArgs e)
        {
            // The SocketAsyncEventArg object is released and pushed into a reusable queue.
            WorldServer.RemoveClient(WorldServer.SocketTolistenClient[s]);
            try
            {
                Interlocked.Decrement(ref this.numConnectedSockets);
                this.ioContextPool.Push(e);
                string outStr = String.Format("Client {0} Disconnected.\0{1} client connection.", s.RemoteEndPoint.ToString(), this.numConnectedSockets);
                Logger.Logger.Log(outStr);
                s.Shutdown(SocketShutdown.Send);
                //WorldServer.queue.Remove()
            }
            catch (Exception)
            {
                // Throw if client has closed, so it is not necessary to catch.
            }
            finally
            {
                s.Close();
            }
        }

        /// <summary>
        /// Callback function when accept operation is completed
        /// </summary>
        /// <param name="sender">Object who raised the event.</param>
        /// <param name="e">SocketAsyncEventArg associated with the completed accept operation.</param>
        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            this.ProcessAccept(e);
        }


        /// <summary>
        /// Monitor Socket Acceptance Processing
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the completed accept operation.</param>
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            Socket s = e.AcceptSocket;
            if (s.Connected)
            {
                try
                {
                    SocketAsyncEventArgs ioContext = this.ioContextPool.Pop();

                    if (ioContext != null)
                    {
                        listenClient li = new listenClient(s, WorldServer.DataBase, WorldServer.Queue,
                            WorldServer.LinkServer, WorldServer.CharacterInWorld);
                        WorldServer.SocketTolistenClient.Add(s, li);

                        ioContext.UserToken = s;
                        // Extracting data from accepted client connections to configure ioContext
                        Interlocked.Increment(ref this.numConnectedSockets);
                        li.send("HG");
                        if (!s.ReceiveAsync(ioContext))
                        {
                            this.ProcessReceive(ioContext);
                        }
                    }
                    else        //The maximum number of customer connections has been reached. Accept connections here, send "the maximum number of connections has been reached", and then disconnect.
                    {
                        s.Send(Encoding.Default.GetBytes("Connections have reached maximum!"));
                        string outStr = String.Format("Connection full, reject {0} Connection.", s.RemoteEndPoint);
                        Logger.Logger.Log(outStr);
                        s.Close();
                    }
                }
                catch (SocketException ex)
                {
                    Socket token = e.UserToken as Socket;
                    string outStr = String.Format("Receiving customers {0} Data error, Exception information: {1} . ", token.RemoteEndPoint, ex.ToString());

                    Logger.Logger.Log(outStr);
                }
                catch (Exception ex)
                {
                    Logger.Logger.Log("Exception:" + ex.ToString());
                }
                // Delivery of the next acceptance request
                this.StartAccept(e);
            }
        }

        /// <summary>
        /// Accept a connection operation from the client
        /// </summary>
        /// <param name="acceptEventArg">The context object to use when issuing 
        /// the accept operation on the server's listening socket.</param>
        private void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            }
            else
            {
                // Object Cleaning Before Reuse
                acceptEventArg.AcceptSocket = null;
            }

            if (!this.listenSocket.AcceptAsync(acceptEventArg))
            {
                this.ProcessAccept(acceptEventArg);
            }
        }

        /// <summary>
        /// Start the service, start listening
        /// </summary>
        /// <param name="port">Port where the server will listen for connection requests.</param>
        internal void Start()
        {
            // Create a listener socket
            this.listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.listenSocket.ReceiveBufferSize = this.bufferSize;
            this.listenSocket.SendBufferSize = this.bufferSize;

            listenSocket.Bind(new IPEndPoint(IPAddress.Parse(World.WorldConfig.IP), World.WorldConfig.PORT));

            // Start monitoring
            this.listenSocket.Listen(this.numConnections);

            // Send an acceptance request on the listening Socket.
            this.StartAccept(null);

            // Blocks the current thread to receive incoming messages.
            allDone.WaitOne();
        }

        /// <summary>
        /// Stop service
        /// </summary>
        internal void Stop()
        {
            this.listenSocket.Close();
        }

    }

}
