using LeafEmu.World.PacketGestion;
using System.Linq;

namespace LeafEmu.World.Game.Item
{
    class MoveItem
    {
        [PacketAttribute("OM")]
        public void MoveStuff(Network.listenClient prmClient, string prmPacket)
        {
            string[] Datas = prmPacket.Substring(2).Split('|');
            int quantity = 1;
            int UID = -1;
            int pos = 1;


            if (Datas.Length >= 3)
            {
                if (!int.TryParse(Datas[2], out quantity))
                    return;
            }

            if (!int.TryParse(Datas[1], out pos))
                return;

            if (!int.TryParse(Datas[0], out UID))
                return;
            if (!prmClient.account.character.Invertaire.Stuff.Any(x => x.UID == UID))
                return;

            Stuff item = prmClient.account.character.Invertaire.Stuff.First(x => x.UID == UID);
            if (quantity > 1)
            {
                return;
            }
            else if (item.Template.Niveau > prmClient.account.character.level)
            {
                prmClient.send("OAEL");
                return;
            }
            else if (item.Template.Type <= 20)
            {

                if (prmClient.account.character.Invertaire.Stuff.Any(x => x.Position == pos))
                {
                    Stuff lastItem = prmClient.account.character.Invertaire.Stuff.First(x => x.Position == pos);
                    lastItem.Position = -1;

                }
                string info = $"{prmPacket}|{pos}\0";
                item.Position = pos;
            }
            uptadeStuff(prmClient, prmPacket + "\0");

        }

        public static void uptadeStuff(Network.listenClient prmClient, string info = "")
        {
            string packet = Game.Character.GestionCharacter.CreateStuffPacketOM(prmClient);
            prmClient.send(info + packet);
            prmClient.send(Game.Character.GestionCharacter.createAsPacket(prmClient));
            prmClient.account.character.Map.SendToAllMap(GetItemsPos(prmClient.account.character));
        }


        public static string GetItemsPos(Game.Entity.Entity character, bool InFight = false)
        {
            var packet = $"Oa" + (InFight ? character.ID_InFight : character.id) + "|";
            Inventaire.Inventaire inventaire = character.Invertaire;
            if (inventaire.Stuff.Any(x => x.Position == 1))
                packet += inventaire.Stuff.First(x => x.Position == 1).Template.ID.ToString("X");

            packet += ",";

            if (inventaire.Stuff.Any(x => x.Position == 6))
                packet += inventaire.Stuff.First(x => x.Position == 6).Template.ID.ToString("X");

            packet += ",";

            if (inventaire.Stuff.Any(x => x.Position == 7))
                packet += inventaire.Stuff.First(x => x.Position == 7).Template.ID.ToString("X");

            packet += ",";

            if (inventaire.Stuff.Any(x => x.Position == 8))
                packet += inventaire.Stuff.First(x => x.Position == 8).Template.ID.ToString("X");

            packet += ",";

            if (inventaire.Stuff.Any(x => x.Position == 15))
                packet += inventaire.Stuff.First(x => x.Position == 15).Template.ID.ToString("X");

            return packet;
        }

    }
}
