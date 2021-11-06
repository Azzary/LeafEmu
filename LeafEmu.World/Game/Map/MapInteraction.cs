using LeafEmu.World.Network;
using LeafEmu.World.PacketGestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafEmu.World.Game.Map
{
    class MapInteraction
    {
        [PacketAttribute("GA500")]
        public void Interaction(listenClient prmClient, string prmPacket)
        {
            prmPacket = prmPacket.Substring(5);
            if (Int32.TryParse(prmPacket.Split(';')[0], out int cell) && Int32.TryParse(prmPacket.Split(';')[1], out int action))
            {
                if (prmClient.account.character.Map.HaveZaap && Database.table.Zaap.zaaps[prmClient.account.character.Map.Id].CellZaap == cell
                    || prmClient.account.character.Map.HaveZaapi && Database.table.Zaap.zaapis[prmClient.account.character.Map.Id].CellZaapi == cell)
                {
                    int startcell = prmClient.account.character.ListCellMove.Count == 0 ? prmClient.account.character.cellID :
                    prmClient.account.character.ListCellMove[0];
                    var ListCell = Mouvement.MapMouvement.GetPathBetwennToCell(prmClient.account.character.Map, startcell,
                        cell, new List<int>(), false);
                    prmClient.account.character.UseInteractifAtEndOfMove = action;
                    if ((ListCell.Count == 1 && ListCell[0] == cell
                        || Game.Pathfinding.Pathfinding.GetJoinCell(cell, prmClient.account.character.Map, false).Contains(startcell)))
                        DoInteractionInMap(prmClient);             
                }
                if (prmClient.account.character.Map.HaveZaapi)
                {
                    Logger.Logger.Warning($"Zaap map:{prmClient.account.character.Map.Id} is in cell: {cell}");
                }
            }
        }

        public static bool DoInteractionInMap(listenClient prmClient)
        {
            if (prmClient.account.character.UseInteractifAtEndOfMove != -1)
            {
                var map = prmClient.account.character.Map;
                switch (prmClient.account.character.UseInteractifAtEndOfMove)
                {
                    case 157:
                        if (checkZaapi(prmClient))
                        {
                            prmClient.account.character.State = EnumClientState.OnRequestZaapi;
                            prmClient.send($"GDF|{Database.table.Zaap.zaapis[map.Id].CellZaapi};3;1\0{MapInteraction.ZaapiPacket(prmClient)}");
                        }
                        break;
                    case 114:
                        //use zaap
                        if (checkZaap(prmClient))
                        {
                            prmClient.account.character.State = EnumClientState.OnRequestZaap;
                            prmClient.send($"GDF|{Database.table.Zaap.zaaps[map.Id].CellZaap};3;1\0{MapInteraction.ZaapPacket(prmClient)}");
                        }

                        break;
                        //save zaap
                    case 44:
                        if (checkZaap(prmClient))
                        {
                            prmClient.account.character.State = prmClient.account.character.State == EnumClientState.OnMove ? EnumClientState.None: 
                                prmClient.account.character.State;
                            if (prmClient.account.character.MapSpawnPoint != prmClient.account.character.Map.Id)
                            {
                                prmClient.account.character.MapSpawnPoint = prmClient.account.character.Map.Id;
                                prmClient.account.character.CellSpawnPoint = Database.table.Zaap.zaaps[prmClient.account.character.Map.Id].CellTp;
                                prmClient.send("Im06");
                            }
                        }
                        break;
                }
                prmClient.account.character.UseInteractifAtEndOfMove = -1;
                return true;
            }
            return false;
        }

        private static bool checkZaap(listenClient prmClient)
        {
            return prmClient.account.character.Map.HaveZaap &&
            Game.Pathfinding.Pathfinding.GetJoinCell(prmClient.account.character.cellID, prmClient.account.character.Map, false)
            .Contains(Database.table.Zaap.zaaps[prmClient.account.character.Map.Id].CellZaap);
        }

        private static bool checkZaapi(listenClient prmClient)
        {
            return prmClient.account.character.Map.HaveZaapi &&
            Game.Pathfinding.Pathfinding.GetJoinCell(prmClient.account.character.cellID, prmClient.account.character.Map, false)
            .Contains(Database.table.Zaap.zaapis[prmClient.account.character.Map.Id].CellZaapi);
        }

        private void quitInteractif(listenClient prmClient, string quitPacket)
        {
            prmClient.account.character.State = EnumClientState.None;
            prmClient.send(quitPacket);
        }

        [PacketAttribute("WV")]
        public void QuitZaap(listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.State == EnumClientState.OnRequestZaap)
                quitInteractif(prmClient, "WV");
        }

        [PacketAttribute("Wv")]
        public void QuitZaapi(listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.State == EnumClientState.OnRequestZaapi)
                quitInteractif(prmClient, "Wv");
        }


        [PacketAttribute("Wu")]
        public void UseZaapi(listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.State == EnumClientState.OnRequestZaapi
                && Int32.TryParse(prmPacket.Split("Wu")[1], out int mapid))
            {
                if (Database.table.Zaap.zaapis.ContainsKey(mapid) 
                   && Database.table.Zaap.zaapis[mapid].align == Database.table.Zaap.zaapis[prmClient.account.character.Map.Id].align)
                    changeMapZaap(prmClient, mapid, Database.table.Zaap.zaapis[mapid].CellTp, 20);
                prmClient.send("Wv");
            }
        }

        [PacketAttribute("WU")]
        public void UseZaap(listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.State == EnumClientState.OnRequestZaap
                && Int32.TryParse(prmPacket.Split("WU")[1], out int mapid))
            {
                if (Database.table.Zaap.zaaps.ContainsKey(mapid)
                && prmClient.account.character.Zaaps.Contains(mapid))
                {
                    changeMapZaap(prmClient, mapid, Database.table.Zaap.zaaps[mapid].CellTp);
                }
                prmClient.send("WV");
            }
        }

        private static void changeMapZaap(listenClient prmClient, int mapid, int cellTP, int price = -1)
        {
            var startMap = Database.table.Map.Maps[prmClient.account.character.MapSpawnPoint];
            var targetMap = Database.table.Map.Maps[mapid];
            price = price == -1 ? Util.GetPriceOfZaap(startMap, targetMap) : price;
            if (prmClient.account.character.kamas >= price)
            {
                prmClient.account.character.kamas -= price;
                Mouvement.MapMouvement.SwitchMap(prmClient, mapid, cellTP);
                prmClient.account.character.State = EnumClientState.None;
                prmClient.send(Character.GestionCharacter.createAsPacket(prmClient.account.character));
            }
        }

        public static string ZaapiPacket(listenClient prmClient)
        {
            StringBuilder packet = new StringBuilder($"Wc{prmClient.account.character.Map.Id}");
            foreach (var item in Database.table.Zaap.zaapis)
            {
                if (Database.table.Zaap.zaapis[prmClient.account.character.Map.Id].align == item.Value.align
                    && item.Key != prmClient.account.character.Map.Id)
                    packet.Append($"|{item.Key};20");
            }
            return packet.ToString();
        }


        public static string ZaapPacket(listenClient prmClient)
        {
            StringBuilder packet = new StringBuilder($"WC{prmClient.account.character.MapSpawnPoint}");
            var startMap = Database.table.Map.Maps[prmClient.account.character.Map.Id];
            foreach (var item in prmClient.account.character.Zaaps)
            {
                var targetMap = Database.table.Map.Maps[item];
                //|mapid;CostKamas
                packet.Append($"|{item};{Util.GetPriceOfZaap(startMap, targetMap)}");
            }
            return packet.ToString();
        }
    }
}
