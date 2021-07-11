using LeafEmu.World.Game.Command.Gestion;

namespace LeafEmu.World.Game.Command.Admin
{
    class ItemCommand
    {

        [CommandAttribute(3, "additem", 2, "{itemid/item Name} {1 = perfectjet}")]
        public void additem(Network.listenClient client, string command)
        {
            string[] InfoItem = command.Split(' ');
            int id;
            if (int.TryParse(InfoItem[1], out id) && Database.table.Item.model_item.AllItems.ContainsKey(id))
            {
                client.account.character.Invertaire.GenerateItemInInv(client, id, InfoItem[2] == "1");
                Item.Stuff.RangerItem(ref client.account.character.Invertaire.Stuff);
                Game.Character.GestionCharacter.CreateASKPacket(client.account.character);
            }
        }
    }
}
