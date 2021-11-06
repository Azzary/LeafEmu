using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace LeafEmu.World.Network
{
    public class LinkServer
    {
        Socket connection;
        public List<string[]> ListOfGUID;
        public LinkServer(List<string[]> _ListOfGUID)
        {
            ListOfGUID = _ListOfGUID;
            connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            connection.Connect(new IPEndPoint(IPAddress.Parse(World.WorldConfig.IP), World.WorldConfig.ServerID));

            new Thread(AuthListen).Start();
        }

        private void AuthListen()
        {
            byte[] buffer;
            int len;
            string packets = string.Empty;
            connection.Send(Encoding.ASCII.GetBytes("NS" + World.WorldConfig.ServerID));
            while (true)
            {
                buffer = new byte[connection.ReceiveBufferSize];
                len = connection.Receive(buffer);

                packets = Encoding.UTF8.GetString(buffer, 0, len);
                foreach (var item in packets.Split(";"))
                {
                    if (item == string.Empty)
                        break;
                    if (item.Substring(0, 2) == "NC")
                    {
                        ListOfGUID.Add(item.Substring(2).Split("|"));
                    }
                    if (item.Substring(0, 2) == "DC")
                    {
                        ListOfGUID.Remove(item.Substring(2).Split("|"));
                    }
                }



            }
        }

        public void addAccount(int ID)
        {
            try
            {
                connection.Send(Encoding.ASCII.GetBytes($"NC{ID};"));
            }
            catch (Exception) { };
        }

        public void RemoveAccount(int ID, string GUID, string Rsecrete)
        {
            ListOfGUID.Remove($"{ID}|{GUID}|{Rsecrete}".Split('|'));
            try
            {
                connection.Send(Encoding.ASCII.GetBytes($"DC{ID};"));
            }
            catch (Exception) { };
        }
    }
}
