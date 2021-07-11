using LeafEmu.World.PacketGestion;
using System;
using System.Linq;

namespace LeafEmu.World.Game.Tchat
{
    class TchatMessage
    {
        [PacketAttribute("BM")]
        public void TchatChanel(Network.listenClient prmClient, string prmPacket)
        {
            string[] var = prmPacket.Substring(2).Split("|");
            string type = var[0];
            string message = var[1];
            string packet;
            if (message.Substring(0, 1) == ".")
            {
                Command.Command.GetCommand(prmClient, message);
                return;
            }
            if (type == "*")
            {
                Map.Map map = prmClient.account.character.Map;
                prmClient.account.character.Map.SendToAllMap($"cMK|{prmClient.account.character.id}|{prmClient.account.character.speudo}|{message}");
            }
            else if (new string[] { ":", "?" }.Contains<string>(type))
            {
                //: commerce
                //? recrutement

                packet = $"cMK{type}|{prmClient.account.character.id}|{prmClient.account.character.speudo}|{message}";
                for (int i = 0; i < prmClient.CharacterInWorld.Count; i++)
                {
                    prmClient.CharacterInWorld[i].send(packet);
                }
            }
            else
            {
                for (int i = 0; i < prmClient.CharacterInWorld.Count; i++)
                {
                    if (prmClient.CharacterInWorld[i].account.character.speudo == type)
                    {
                        prmClient.CharacterInWorld[i].send($"cMKF{prmClient.account.character.id}|{prmClient.account.character.speudo}|{message}");
                        prmClient.send($"cMKT{prmClient.account.character.id}|{prmClient.account.character.speudo}|{message}");
                        return;
                    }

                }
                prmClient.send($"cMEf{type}");

            }
        }

    }
}
