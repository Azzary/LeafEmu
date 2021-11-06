using LeafEmu.World.PacketGestion;
using System.Linq;
using System.Text;

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
            if (!prmClient.account.character.Inventaire.Stuffs.Any(x => x.UID == UID))
                return;

            Stuff item = prmClient.account.character.Inventaire.Stuffs.First(x => x.UID == UID);
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

                if (prmClient.account.character.Inventaire.Stuffs.Any(x => x.Position == pos))
                {
                    Stuff lastItem = prmClient.account.character.Inventaire.Stuffs.First(x => x.Position == pos);
                    lastItem.Position = -1;

                }
                string info = $"{prmPacket}|{pos}\0";
                item.Position = pos;
            }
            UptadeStuff(prmClient, prmPacket + "\0");

        }

        public static void UptadeStuff(Network.listenClient prmClient, string info = "")
        {
            string packet = prmClient.account.character.Inventaire.CreateStuffPacketOM(prmClient);
            prmClient.send(info + packet);
            prmClient.send(Game.Character.GestionCharacter.createAsPacket(prmClient.account.character));
            prmClient.account.character.Map.SendToAllMap(GetItemsPos(prmClient.account.character));
        }


        public static string GetItemsPos(Game.Entity.Entity character, bool InFight = false)
        {
            var packet = new StringBuilder($"Oa" + (InFight ? character.ID_InFight : character.id) + "|");
            Inventaire.Inventaire inventaire = character.Inventaire;
            foreach (var posID in new List<int>() { 1, 6, 7, 8, 15 })
            {
                var item = character.Inventaire.getItemByPos(posID);
                if (item != null)
                    packet.Append(item.Template.ID.ToString("X"));
                packet.Append(",");
            }

            return packet.ToString();
        }

    }
}
