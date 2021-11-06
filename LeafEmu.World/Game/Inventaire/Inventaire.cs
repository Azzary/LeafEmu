using LeafEmu.World.Game.Item;
using LeafEmu.World.World;
using System.Collections.Generic;
using System.Text;

namespace LeafEmu.World.Game.Inventaire
{

    public class Inventaire
    {
        public List<Item.Stuff> Stuffs { get; set; }
        public Inventaire()
        {
            Stuffs = new List<Stuff>();
        }

        internal void addItem(Network.listenClient PrmClient, int uid, int position, int quantier, List<Effect> stats, Item.Item item)
        {
            add(PrmClient, new Stuff(uid,
                          position,
                          stats,
                          item,
                          quantier));
        }

        public string CreateStuffPacketOM(Network.listenClient prmClient)
        {
            StringBuilder packet = new StringBuilder("OS+5|");
            foreach (var item in Stuffs)
            {
                if (item.Position != -1)
                {
                    packet.Append($"{item.Template.ID}|");
                }
            }
            return packet.ToString();
        }

        public void GenerateItemInInv(Network.listenClient PrmClient, int id, int Quantity, bool perfect = false)
        {
            Stuff stuff = Stuff.GenerateItem(id, perfect);
            stuff.Quantity = Quantity;
            add(PrmClient, stuff);
        }

        public void SendInventory(Network.listenClient PrmClient)
        {
            if (PrmClient.account.character != null)
            {
                Stuffs = Stuff.RangerItem(Stuffs);
                PrmClient.send(CreateASKPacket(PrmClient.account.character));
            }
        }

        public void add(Network.listenClient PrmClient, Stuff stuff)
        {
            var itemAllReadyIn = Stuffs.Find(item => item.Template.ID == stuff.Template.ID && stuff.Position == -1 && Stuff.ItemHaveSameEffect(stuff, item));
            if (itemAllReadyIn != null)
            {
                itemAllReadyIn.Quantity += stuff.Quantity;
                PrmClient.send("OQ" + itemAllReadyIn.UID + "|" + itemAllReadyIn.Quantity);
            }
            else
            {
                Stuffs.Add(stuff);
                PrmClient.send("OAKO" + stuff.DisplayItem + ";");
            }

            //PrmClient.GAME_SEND_Im_PACKET("021;" + stuff.Quantity + "~" + stuff.Template.ID);
    
            //SendInventory(PrmClient);
        }

        internal void RemoveItem(Network.listenClient PrmClient, Stuff stuff)
        {
            Stuffs.Remove(stuff);
            SendInventory(PrmClient);
        }

        /// <summary>
        /// Inventer
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string CreateASKPacket(Entity.Character character)
        {
            StringBuilder selectPacket = new StringBuilder("ASK|");
            selectPacket.Append(character.id).Append("|").Append(character.speudo).Append("|").Append(character.level).Append("|")
                .Append(character.classe).Append("|").Append("0|").Append(character.gfxID).Append("|").Append("-1")
                .Append("|").Append("-1").Append("|").Append("-1").Append("|");
            character.Inventaire.Stuffs.ForEach(x => selectPacket.Append(x.DisplayItem + ";"));
            selectPacket.Append("|");
            return selectPacket.ToString();
        }

        internal void RemoveItem(Network.listenClient PrmClient, int id, int qte, bool isUID = true)
        {
            var item = isUID ? Stuffs.Find(x => x.UID == id) : Stuffs.Find(x => x.Template.ID == id);
            if (item != null)
            {
                if (item.Quantity - qte <= 0)
                    Stuffs.Remove(item);
                else
                    item.Quantity -= qte;
            }
            SendInventory(PrmClient);
        }

        internal Stuff getItemByPos(int pos)
        {
            if (pos == Constant.ITEM_POS_NO_EQUIPED)
                return null;
            return Stuffs.Find(x => x.Position == pos);
        }

        public bool HasItemTemplate(int id, int qte = 1)
        {
            return Stuffs.Exists(x => x.Position == Constant.ITEM_POS_NO_EQUIPED && x.Template.ID == id && x.Quantity >= qte);
        }

        internal Stuff GetItem(int id, int qte = 1)
        {
            return Stuffs.Find(x => x.Position == Constant.ITEM_POS_NO_EQUIPED && x.Template.ID == id && x.Quantity >= qte);
        }
    }
}
