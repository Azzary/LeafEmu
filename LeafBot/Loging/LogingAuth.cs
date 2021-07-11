using LeafBot.PacketGestion;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace LeafBot.Loging
{
    class LogingAuth
    {
        [PacketAttribute("HC")]
        public void SendHelloMessage(Client.Client client, string Packet, List<string> LastPacket)
        {

            string key = Packet.Substring(2, 32);
            client.send(LeafConfig.ClientVersion);
            client.send($"test{client.ID}\n{Hash.Crypt_Password("test", key)}\nAf");
        }

        [PacketAttribute("Ad")]
        public void Af(Client.Client client, string Packet, List<string> LastPacket)
        {
            client.send("Ax");
        }

        [PacketAttribute("AxK")]
        public void ListServer(Client.Client client, string Packet, List<string> LastPacket)
        {
            Logger.Log(client.ID);
            client.send("AX" + LeafConfig.ServerID);
        }

        [PacketAttribute("AXK")]
        public void PrepareSwitchToWorld(Client.Client client, string Packet, List<string> LastPacket)
        {
            Packet = Packet.Substring(3);
            string RealmIp = Hash.Decrypt_IP(Packet.Substring(0, 8));
            int RealmPort = Hash.Decrypt_Port(Packet.Substring(8, 3).ToCharArray());
            client.GUID = Packet.Substring(11, 7);
            client.socket.Close();
            client.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.socket.Connect(new IPEndPoint(IPAddress.Parse(RealmIp), RealmPort));
        }

    }
}
