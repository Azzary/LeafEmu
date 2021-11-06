using LeafEmu.World.Game.Command.Gestion;
using System;

namespace LeafEmu.World.Game.Command.Admin
{
    public class Tp
    {

        [CommandAttribute(3, "tpXY", 2, "{x} {y} Tp to a map x,y ")]
        public void TpMapWithX_Y(Network.listenClient client, string command)
        {
            string[] InfoTP = command.Split(' ');
            foreach (var item in Database.table.Map.Maps)
            {
                if (item.Value.X.ToString() == InfoTP[1] && item.Value.Y.ToString() == InfoTP[2])
                {
                    Map.Cell.Cell cellTemp = Database.table.Map.Maps[item.Value.Id].Cells.Find(x => x.IsWalkable);
                    if (cellTemp != null)
                    {
                        Map.Mouvement.MapMouvement.SwitchMap(client, item.Value.Id, cellTemp.ID);
                    }
                    return;
                }

            }
        }

        [CommandAttribute(3, "tpTo", 1, "{MapID/Player} Tp to a map or a player")]
        public void TpMap(Network.listenClient client, string command)
        {
            string InfoTP = command.Split(' ')[1];
            int MapID = -1;
            int cellID = 0;
            if (Int32.TryParse(InfoTP, out MapID))
            {
                if (Database.table.Map.Maps.ContainsKey(MapID))
                {
                    Map.Cell.Cell cellTemp = Database.table.Map.Maps[MapID].Cells.Find(x => x.IsWalkable);
                    if (cellTemp == null)
                    {
                        return;
                    }
                    cellID = cellTemp.ID;
                }
                else
                    return;
            }
            else
            {
                Network.listenClient TpTo = client.CharacterInWorld.Find(x => x.account.character.speudo == InfoTP);
                if (TpTo == null)
                    return;

                MapID = TpTo.account.character.Map.Id;
                cellID = TpTo.account.character.cellID;
            }

            Map.Mouvement.MapMouvement.SwitchMap(client, MapID, cellID);
        }


        [CommandAttribute(3, "tpHere", 1, "{Target} Tp a target")]
        public void TpPlayer(Network.listenClient client, string command)
        {
            string speudo = command.Split(' ')[1];
            Network.listenClient TpTo = client.CharacterInWorld.Find(x => x.account.character.speudo == speudo);
            if (TpTo != null)
                Map.Mouvement.MapMouvement.SwitchMap(TpTo, client.account.character.Map.Id, client.account.character.cellID);
        }
    }
}
