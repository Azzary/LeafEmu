﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace LeafEmu.Auth.Network
{
    class listenClient
    {

        public event Action<string> packetReceivedEvent;

        public Database.LoadDataBase database;
        public Socket ClientSocket { get; set; }

        public bool isCo = true;

        private string key;

        public Account.Account account;
        public LinkServer linkServer;
        public List<listenClient> queue;

        public listenClient(Socket _ClientSocket, Database.LoadDataBase _database, List<listenClient> _queue, LinkServer _linkServer)
        {
            linkServer = _linkServer;
            queue = _queue;
            database = _database;
            ClientSocket = _ClientSocket;
            key = Util.hash.GenerateString(32);
            account = new Account.Account(key);

        }

        public void send(string packet)
        {
            try
            {
                ClientSocket.Send(Encoding.ASCII.GetBytes(packet + "\0"));
            }
            catch (Exception) { }

        }

        public void Recv()
        {
            if (ClientSocket.Poll(10000, SelectMode.SelectRead))
            {
                byte[] buffer = new byte[ClientSocket.ReceiveBufferSize];
                int len = ClientSocket.Receive(buffer);
                if (len == 0)
                    return;
                string Packets = Encoding.ASCII.GetString(buffer, 0, len);
                Logger.Logger.Log(Packets);
                foreach (string Packet in Packets.Split('\0'))
                {
                    PacketGestion.PacketGestion.Gestion(this, Packet);
                }

            }
        }

        public void startlisten()
        {
            send("HC" + key);
            byte[] buffer = new byte[ClientSocket.ReceiveBufferSize];

            int len = ClientSocket.Receive(buffer);
            account.version = Encoding.UTF8.GetString(buffer, 0, len).Replace("\n\0", string.Empty);

            len = ClientSocket.Receive(buffer);
            string packets = Encoding.UTF8.GetString(buffer, 0, len);

            account.UserName = packets.Split('\n')[0];
            account.HashPass = packets.Split('\n')[1];

            queue.Add(this);
            while (isCo)
            {
                len = ClientSocket.Receive(buffer);
                packets = Encoding.UTF8.GetString(buffer, 0, len);
                if (packets == string.Empty)
                    break;

                foreach (string packet in packets.Replace("\x0a", string.Empty).Split('\0').Where(x => x != string.Empty))
                {
                    packetReceivedEvent?.Invoke(packet);

                    PacketGestion.PacketGestion.Gestion(this, packet);
                }

                buffer = new byte[ClientSocket.ReceiveBufferSize];
            }

            ClientSocket.Close();
        }


    }
}
