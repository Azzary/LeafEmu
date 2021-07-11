using LeafEmu.World.PacketGestion;

namespace LeafEmu.World.Game.Tchat
{
    class TchatParametre
    {
        [PacketAttribute("cC")]
        public void TchatOption(Network.listenClient prmClient, string prmPacket)
        {
            prmClient.send(prmPacket);
        }

    }
}
