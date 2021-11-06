using LeafEmu.World.Network;
using LeafEmu.World.PacketGestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeafEmu.World.Game.Map.Mouvement
{


    class MapMouvement
    {
        //horizontal,vertical,diagonal
        public static double[] WALK_SPEEDS = { 0.75, 0.5, 0.5 };
        public static double[] RUN_SPEEDS = { 0.3, 0.2, 0.2 };
        //public static double[] MOUNT_SPEEDS = { 2.300000E-001, 2.000000E-001, 2.000000E-001, 2.000000E-001, 2.300000E-001, 2.000000E-001, 2.000000E-001, 2.000000E-001 };
        static int[] ListDir = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
        List<int> ListDirFight = new List<int> { 1, 3, 5, 7 };

        public static List<int> GetPathBetwennToCell(Map map, int cellStart, int cellTarget, List<int> obstacle, bool isFight = false)
        {
            List<int> ListCell = new List<int>() { cellStart };
            var pathEngine = new Game.Pathfinding.PathfindingV2(map, isFight);
            var path = pathEngine.FindShortestPath(cellStart, cellTarget, obstacle);
            path.ForEach(x => ListCell.Add(x.cell.ID));

            if (map.HaveZaap && ListCell[ListCell.Count - 1] == Database.table.Zaap.zaaps[map.Id].CellZaap)
                ListCell.RemoveAt(ListCell.Count - 1);
            if (map.HaveZaapi && ListCell[ListCell.Count - 1] == Database.table.Zaap.zaapis[map.Id].CellZaapi)
                ListCell.RemoveAt(ListCell.Count - 1);
            return ListCell;
        }


        public static bool CanMove(listenClient prmClient)
        {
            return (prmClient.account.character.State == EnumClientState.None || prmClient.account.character.State == EnumClientState.OnSwithMove)
                && prmClient.account.character.FightInfo.InFight == 0
                && (DateTimeOffset.Now.ToUnixTimeSeconds() > prmClient.account.character.WaitMoving || prmClient.account.character.State == EnumClientState.OnSwithMove);
        }

        [PacketAttribute("GA001")]
        public void ConfirmMove(listenClient prmClient, string prmPacket)
        {
            var character = prmClient.account.character;
            Map map = prmClient.account.character.Map;
            prmClient.account.character.UseInteractifAtEndOfMove = -1;
            Logger.Logger.Log(map.CharactersOnMap.Count);
            prmPacket = prmPacket.Substring(5).Split('\0')[0];
            if (!CanMove(prmClient) && !FightMouvement.CanMouvInFight(prmClient, prmPacket))
                return;
            
            int startCell = character.FightInfo.InFight != 2 ? character.cellID : character.FightInfo.FightCell;
            var targetCell = Util.CharToCell(prmPacket.Substring(prmPacket.Length - 2, 2));
            List<int> ListCell = character.FightInfo.InFight == 2 ? FightMouvement.mouvInFight(prmClient, prmPacket) :
                                                                    GetPathBetwennToCell(map, startCell, targetCell, new List<int>());
            if (ListCell.Count == 0 || ListCell.Count == 1 && startCell == ListCell[0])
            {
                prmClient.send("GA;0");
                return;
            }
            string chemain = ($"GA0;1;{prmClient.account.character.id};{getStringPath(startCell, ListCell, map)}"); 
            prmClient.account.character.ChangeCell(ListCell[ListCell.Count - 1]);
            prmClient.account.character.WaitMoving = DateTimeOffset.Now.ToUnixTimeSeconds() + timing(ListCell, map);
            prmClient.account.character.ListCellMove = ListCell;
            prmClient.account.character.State = EnumClientState.OnMove;

            if (character.FightInfo.InFight == 2) prmClient.SendToAllFight(chemain); else prmClient.SendToAllMap(chemain);
        }


        public static string getStringPath(int startCell, List<int> ListCell, Map map)
        {
            string remakePath = Game.Pathfinding.Pathfinding.CreateStringPath(startCell, 1, ListCell, map);
            Game.Pathfinding.PathfindingUtil pathfinding = new Game.Pathfinding.PathfindingUtil(remakePath, map, startCell, 0);
            return pathfinding.GetStartPath + pathfinding.RemakePath();
        }


        private async void setDoor(Map map, InteractiveDoors interDoors, listenClient PrmClient)
        {
            setState(map, true, interDoors, PrmClient);
            await WorldServer.MethodSleep(30000*100);
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
            if (prmClient.account.character.State == EnumClientState.OnSwithMove)
            {
                Logger.Logger.Warning("Is a cheater ????");
                return;
            }
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

            if (DateTimeOffset.Now.ToUnixTimeSeconds() + 5 < prmClient.account.character.WaitMoving)
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
                    if (true || item.RequiedCells.Intersect(cellid).Count() == item.RequiedCells.Count)
                    {
                        setDoor(map, item, prmClient);
                    }

                }
            }
            if (!MapInteraction.DoInteractionInMap(prmClient))
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
            if (Database.table.Zaap.zaaps.ContainsKey(NewMap.Id) && !prmClient.account.character.Zaaps.Contains(NewMap.Id))
            {
                prmClient.account.character.Zaaps.Add(NewMap.Id);
                prmClient.send("Im024");
            }
                
        }

        [PacketAttribute("GKE0")]
        public void SwitchMoving(Network.listenClient prmClient, string prmPacket)
        {
            prmPacket = prmPacket.Substring(5).Split("\0")[0];
            if (int.TryParse(prmPacket, out int CurrentPlayerCell))
            {
                int indexOfCell = prmClient.account.character.ListCellMove.IndexOf(CurrentPlayerCell);
                if (indexOfCell != -1)
                {
                    double time = prmClient.account.character.WaitMoving -
                       timing(prmClient.account.character.ListCellMove.GetRange(0, indexOfCell), prmClient.account.character.Map);
                    if (DateTimeOffset.Now.ToUnixTimeSeconds() + 5 >= time)
                    {
                        prmClient.account.character.cellID = CurrentPlayerCell;
                        prmClient.account.character.State = EnumClientState.OnSwithMove;               
                        prmClient.send("BN");
                    }
                }
            }
        }
        public static double timing(List<int> path, Map map)
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

        private static int find_direction(Map map, int startCell, int endCell)
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
