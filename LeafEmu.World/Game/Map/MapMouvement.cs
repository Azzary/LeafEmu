using LeafEmu.World.Network;
using LeafEmu.World.PacketGestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeafEmu.World.Game.Map
{


    class MapMouvement
    {
        //horizontal,vertical,diagonal
        public static double[] WALK_SPEEDS = { 0.75, 0.5, 0.5 };
        public static double[] RUN_SPEEDS = { 0.3, 0.2, 0.2 };
        //public static double[] MOUNT_SPEEDS = { 2.300000E-001, 2.000000E-001, 2.000000E-001, 2.000000E-001, 2.300000E-001, 2.000000E-001, 2.000000E-001, 2.000000E-001 };
        int[] ListDir = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
        List<int> ListDirFight = new List<int> { 1, 3, 5, 7 };

        [PacketAttribute("GA001")]
        public void ConfirmMove(Network.listenClient prmClient, string prmPacket)
        {

            if (DateTimeOffset.Now.ToUnixTimeSeconds() < prmClient.account.character.WaitMoving || prmClient.account.character.FightInfo.InFight != 0 && !prmClient.account.character.FightInfo.YourTurn)
            {
                return;
            }
            prmPacket = prmPacket.Substring(5).Split('\0')[0];
            if (!prmClient.account.character.FightInfo.YourTurn && prmClient.account.character.FightInfo.InFight == 2 || !(prmClient.account.character.State == EnumClientState.None) && prmClient.account.character.FightInfo.InFight != 2
                || prmClient.account.character.FightInfo.InFight == 2 && prmPacket.Length / 3 > prmClient.account.character.TotalPM)
            {
                return;
            }
            int Cell = prmClient.account.character.FightInfo.InFight != 2 ?
            prmClient.account.character.cellID:
            prmClient.account.character.FightInfo.FightCell;
            List<int> ListCell = new List<int>();
            Map map = prmClient.account.character.Map;
            var pathEngine = new Game.Pathfinding.PathfindingV2(map, prmClient.account.character.FightInfo.InFight == 2);
            var path = pathEngine.FindShortestPath(Cell, Util.CharToCell(prmPacket.Substring(prmPacket.Length - 2, 2)), new List<int>());
            if (path.Count == 0)
                return;
            path.ForEach(x => ListCell.Add(x.cell.ID));
            string remakePath = Game.Pathfinding.Pathfinding.CreateStringPath(Cell, 1, ListCell, map);
            Game.Pathfinding.PathfindingUtil pathfinding = new Game.Pathfinding.PathfindingUtil(remakePath, map, Cell, 0);
            string chemain = pathfinding.GetStartPath + pathfinding.RemakePath();
            List<listenClient> CharactersOnMap;
            if (prmClient.account.character.FightInfo.InFight == 2)
            {
                if (ListCell.Count > prmClient.account.character.PM)
                    return;
                for (int i = 0; i < ListCell.Count; i++)
                {
                    var traps = prmClient.account.character.CurrentFight.glyphAndTrapsOnMap.FindAll(x => x.IsTrap && x.ListCell.Contains(ListCell[i]));
                    if (traps.Count != 0)
                    {
                        ListCell.RemoveRange(i, ListCell.Count - i);
                        foreach (var trap in traps)
                        {
                            trap.ActionGliphEntity(prmClient.account.character);
                            trap.Remove();
                        }
                        break;
                    }
                }

                foreach (var item in prmClient.account.character.CurrentFight.glyphAndTrapsOnMap)
                {
                    if (item.IsTrap)
                    {
                        foreach (int cell in item.ListCell)
                        {
                            if (true)
                            {

                            }
                        }
                    }
                }
                
                chemain = ($"GA0;1;{prmClient.account.character.id};b{Util.CellToChar(prmClient.account.character.FightInfo.FightCell)}{chemain}");
                prmClient.account.character.PM -= ListCell.Count;
                prmClient.account.character.FightInfo.FightCell = Util.CharToCell(chemain.Substring(chemain.Length - 2));
                chemain += $"\0GA;129;{prmClient.account.character.id};{prmClient.account.character.id},-{ListCell.Count}";
                CharactersOnMap = prmClient.account.character.CurrentFight.PlayerInFight;

            }
            else
            {
                chemain = ($"GA0;1;{prmClient.account.character.id};{chemain}");
                CharactersOnMap = map.CharactersOnMap;
                prmClient.account.character.cellID = Util.CharToCell(chemain.Substring(chemain.Length - 2));
                prmClient.account.character.WaitMoving = DateTimeOffset.Now.ToUnixTimeSeconds() + timing(ListCell, map);
            }
            prmClient.account.character.ListCellMove = ListCell;
            prmClient.account.character.State = EnumClientState.OnMove;
            lock (CharactersOnMap)
            {
                for (int i = 0; i < CharactersOnMap.Count; i++)
                {
                    CharactersOnMap[i].send(chemain);
                }
            }


        }

        private async void setDoor(Map map, InteractiveDoors interDoors, listenClient PrmClient)
        {
            setState(map, true, interDoors, PrmClient);
            await Task.Delay(30000);
            setState(map, false, interDoors, PrmClient);
        }

        private void setState(Map map, bool active, InteractiveDoors interDoors, listenClient PrmClient)
        {
            string packet = "GDF";
            Entity.Character player = PrmClient.account.character;

            interDoors.DoorCellID.ForEach(x =>
            {
                packet += setStateDoor(x, active, player != null);
                setStateCell(map, map.Cells[x], active, PrmClient);
            });

            PrmClient.SendToAllMap(packet);
        }

        private void setStateCell(Game.Map.Map map, Cell.Cell cell, bool active, listenClient PrmClient)
        {
            String packet = "GDC" + cell.ID;
            if (active)
            {
                map.CellForceDisable.Remove(cell.ID);
                if (!map.CellForceEnable.Contains(cell.ID))
                {
                    map.CellForceEnable.Add(cell.ID);
                }
                packet += ";aaGaaaaaaa801;1";
            }
            else
            {
                map.CellForceEnable.Remove(cell.ID);
                if (!map.CellForceDisable.Contains(cell.ID))
                {
                    map.CellForceDisable.Add(cell.ID);
                }
                packet += ";aaaaaaaaaa801;1";
            }
            PrmClient.SendToAllMap(packet);
        }

        private static string setStateDoor(int cell, bool active, bool fast)
        {
            return "|" + cell + (!fast ? (active ? ";2" : ";4") : (active ? ";3" : ";1"));
        }


        [PacketAttribute("GKK0")]
        public void ActionEnd(listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.FightInfo.InFight != 0 || prmClient.account.character.ListCellMove.Count == 0)
            {
                if (prmClient.account.character.FightInfo.InFight == 2)
                {
                    prmClient.account.character.State = EnumClientState.None;
                    prmClient.SendToAllFight("GAF2|" + prmClient.account.character.ID_InFight);
                    prmClient.send("BN");
                }
                return;
            }

            if (DateTimeOffset.Now.ToUnixTimeSeconds() < prmClient.account.character.WaitMoving)
            {
                prmClient.account.character.WaitMoving = DateTimeOffset.Now.ToUnixTimeSeconds() + 10000000;
                return;
            }
            if (prmClient.account.character.Map.CellTp.ContainsKey(prmClient.account.character.cellID))
            {
                int[] arg = prmClient.account.character.Map.CellTp[prmClient.account.character.cellID];
                SwitchMap(prmClient, arg[0], arg[1]);
            }
            else
            {
                if (Database.table.Donjons.Donjon.ContainsKey(prmClient.account.character.mapID))
                {
                    if (Database.table.Donjons.Donjon.ContainsKey(prmClient.account.character.mapID) &&
                        Database.table.Donjons.Donjon[prmClient.account.character.mapID].cellID == prmClient.account.character.cellID)
                    {
                        Fight.GestionFight.LauchFight(prmClient, Database.table.Donjons.Donjon[prmClient.account.character.mapID], Fight.FightTypeEnum.PvM, true);
                    }
                }
                else
                {
                    object GroupMob = prmClient.account.character.Map.MobInMap.Find(x => x.cellID == prmClient.account.character.cellID);
                    if (GroupMob != null)
                    {
                        Fight.GestionFight.LauchFight(prmClient, GroupMob, Fight.FightTypeEnum.PvM);
                    }
                }
            }
            Map map = prmClient.account.character.Map;
            foreach (var item in map.InteractiveDoors)
            {
                if (item.RequiedCells.Contains(prmClient.account.character.cellID))
                {
                    List<int> cellid = new List<int>();
                    map.CharactersOnMap.ForEach(x => cellid.Add(x.account.character.cellID));
                    if (item.RequiedCells.Intersect(cellid).Count() == item.RequiedCells.Count)
                    {
                        setDoor(map, item, prmClient);
                    }

                }
            }
            prmClient.account.character.State = EnumClientState.None;
            prmClient.account.character.ListCellMove.Clear();
        }

        public static void SwitchMap(listenClient prmClient, int mapID, int CellID)
        {
            Map NewMap = Database.table.Map.Maps[mapID];
            lock (NewMap)
            {
                prmClient.account.character.Map.CharactersOnMap.Remove(prmClient);
                NewMap.CharactersOnMap.Add(prmClient);
                Map LastMap = prmClient.account.character.Map;
                prmClient.account.character.Map.SendToAllMap($"GM|-{prmClient.account.character.id}");
                prmClient.account.character.Map = NewMap;
                prmClient.account.character.mapID = NewMap.Id;
                prmClient.account.character.cellID = CellID;
                prmClient.send($"GDM|{NewMap.Id}|{NewMap.CreateTime}|{NewMap.DataKey}");
            }
        }

        [PacketAttribute("GKE0")]
        public void SwitchMoving(Network.listenClient prmClient, string prmPacket)
        {
            prmPacket = prmPacket.Substring(5).Split("\0")[0];
            if (int.TryParse(prmPacket, out int var))
            {
                List<int> pathTemp = prmClient.account.character.ListCellMove;
                for (int i = 0; i < pathTemp.Count - 1; i++)
                {
                    if (var == pathTemp[i])
                    {
                        prmClient.account.character.cellID = pathTemp[i];
                        if ( DateTimeOffset.Now.ToUnixTimeSeconds() == prmClient.account.character.WaitMoving - timing(pathTemp.GetRange(i, pathTemp.Count -1), prmClient.account.character.Map))
                        {
                            prmClient.account.character.WaitMoving = DateTimeOffset.Now.ToUnixTimeSeconds() - 1;
                            prmClient.account.character.State = EnumClientState.None;
                            prmClient.account.character.Map.SendToAllMap($"GA0;1;{prmClient.account.character.id};b{Util.CellToChar(prmClient.account.character.cellID)}");
                            return;
                        }
                    }

                }

            }
        }
        private double timing(List<int> path, Map map)
        {
            double run_time = 0;
            double[] time;
            if (path.Count > 2)
            {
                time = RUN_SPEEDS;
            }
            else
            {
                time = WALK_SPEEDS;
            }
            for (int i = 0; i < path.Count - 1; i++)
            {
                int dir = find_direction(map, path[i], path[i + 1]);
                if (dir == 2 || dir == 6)
                {
                    run_time += time[1];
                }
                else if (dir == 0 || dir == 4)
                {
                    run_time += time[0];
                }
                else
                {
                    run_time += time[2];
                }

            }

            return run_time - 1;
        }

        private int find_direction(Map map, int startCell, int endCell)
        {
            foreach (int dir in ListDir)
            {
                int Cell = Pathfinding.NextCell(map, startCell, dir);
                if (Cell == endCell)
                {
                    return dir;
                }
            }
            return 0;
        }
    }


}
