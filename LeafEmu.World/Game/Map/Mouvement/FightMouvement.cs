using LeafEmu.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafEmu.World.Game.Map.Mouvement
{
    public class FightMouvement
    {
        public static List<int> mouvInFight(listenClient prmClient, string path)
        {
            var character = prmClient.account.character;
            var ListCell = verifPath(character.Map, path, character.FightInfo.FightCell,
                                     character.PM, character.CurrentFight.AllEntityInFight.Select(x => x.FightInfo.FightCell).ToList());
            WalkInTrap(ListCell, prmClient.account.character);
            character.PM -= ListCell.Count;
            prmClient.send($"GA;129;{prmClient.account.character.id};{prmClient.account.character.id},-{ListCell.Count}");
            return ListCell;
        }

        public static bool CanMouvInFight(listenClient prmClient, string prmPacket)
        {
            return prmClient.account.character.FightInfo.InFight == 2 && prmClient.account.character.FightInfo.YourTurn;
        }

        private static List<int> verifPath(Map map, string path, int startCell, int maxPM, List<int> obstacle)
        {
            List<int> ListCell = new List<int>();
            for (int i = 0; i < path.Length; i += 3)
            {
                var targetCell = Util.CharToCell(path.Substring(i + 1, 2));
                var dir = Game.Pathfinding.PathfindingUtil.GetDirection(startCell, targetCell, map);
                var lineCell = Game.Pathfinding.PathfindingUtil.GetAllCellsForThisLinePath(dir, startCell, targetCell, map, ref maxPM);
                if (!lineCell.Contains(targetCell) || lineCell.Any(x => !map.Cells[x].IsWalkable || obstacle.Any(y => y == x)))
                {
                    ListCell.Clear();
                    break;
                }
                ListCell.AddRange(lineCell);
                startCell = targetCell;
            }
            return ListCell;
        }

        private static void WalkInTrap(List<int> ListCell, Entity.Entity entity)
        {
            for (int i = 0; i < ListCell.Count; i++)
            {
                var traps = entity.CurrentFight.glyphAndTrapsOnMap.FindAll(x => x.IsTrap && x.ListCell.Contains(ListCell[i]));
                if (traps.Count != 0)
                {
                    ListCell.RemoveRange(i, ListCell.Count - i);
                    foreach (var trap in traps)
                    {
                        trap.ActionGliphEntity(entity);
                        trap.Remove();
                    }
                    break;
                }
            }
        }

    }
}
