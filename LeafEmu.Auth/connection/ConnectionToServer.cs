using LeafEmu.Auth.PacketGestion;
using System;

namespace LeafEmu.Auth.connection
{
    class ConnectionToServer
    {
        [PacketAttribute("AX")]
        public void CheckQueue(Network.listenClient prmClient, string prmPacket)
        {
            if (Int32.TryParse(prmPacket.Substring(2), out int id))
            {
                if (prmClient.account.ListServ.Contains(id))
                {
                    string packet = "AXK";
                    packet += Util.hash.CryptIP("127.0.0.1");
                    packet += "bwZ"; //port
                    string GUID = Util.hash.GenerateString(7);
                    packet += GUID;
                    prmClient.linkServer.sendConnectionToServer(prmClient.account.ID, GUID, prmClient.account.role, prmClient.account.Rsecret);
                    prmClient.send(packet);
                }
            }
            else
                prmClient.isCo = false;


        }


    }
}
