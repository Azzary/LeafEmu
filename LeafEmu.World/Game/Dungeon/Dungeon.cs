using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafEmu.World.Game.Dungeon
{
    public class Dungeon
    {
        public static void DialogEnter(Network.listenClient prmClient, int id)
        {
            int[] infoDJ = Array.ConvertAll(Database.table.Npc.ReponceDialog[id].args.Split(','), int.Parse);
            if (prmClient.account.character.mapID != infoDJ[3])
                return;
            Item.Stuff cle = prmClient.account.character.Inventaire.Stuffs.Find(x => x.Template.ID == infoDJ[2]);
            if (cle != null)
            {
                if (cle.Quantity == 1)
                    prmClient.account.character.Inventaire.RemoveItem(prmClient, cle);
                else
                    cle.Quantity -= 1;
                Map.Mouvement.MapMouvement.SwitchMap(prmClient, infoDJ[0], infoDJ[1]);
            }
        }

    }
}
