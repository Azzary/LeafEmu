using LeafEmu.World.Game.Item;
using System.Collections.Generic;

namespace LeafEmu.World.Game.Inventaire
{

    public class Inventaire
    {
        public List<Item.Stuff> Stuff = new List<Item.Stuff>();

        internal void addItem(int uid, int position, int quantier, List<Effect> stats, Item.Item item)
        {

            Stuff.Add(new Item.Stuff(uid,
                                     position,
                                     stats,
                                     item,
                                     quantier));
        }

        public void GenerateItemInInv(Network.listenClient PrmClient, int id, bool perfect = false)
        {
            Item.Stuff stuff = Item.Stuff.GenerateItem(id, perfect);
            PrmClient.account.character.Invertaire.Stuff.Add(stuff);
            PrmClient.send("OAKO" + stuff.DisplayItem + ";");
        }

    }
}
