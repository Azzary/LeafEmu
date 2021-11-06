using System;
using System.Collections.Generic;

namespace LeafEmu.World.Game.Entity.AI
{
    public static class AIUtil
    {



        public static Entity GetNearestFighter(Entity entity)
        {
            if (entity.CurrentFight == null)
                return null;
            Entity nearest = null;
            int dist = 1000;
            int timeout = 0;
            foreach (Entity fighter in entity.CurrentFight.AllEntityInFight)
            {
                if (!fighter.FightInfo.isDead && !fighter.FightInfo.IsFriendly(entity))
                {
                    int fDist = GetDistanceBetween(entity.FightInfo.FightCell, fighter.FightInfo.FightCell, entity.CurrentFight.map);
                    if (fDist < dist)
                    {
                        dist = fDist;
                        nearest = fighter;
                    }
                }
                timeout += 1;
                if (timeout > 100)
                    break;
            }
            return nearest;
        }

        public static int GetDistanceBetween(int id1, int id2, Map.Map _map)
        {
            if (id1 == id2) return 0;
            if (_map == null) return 0;
            int diffX = Math.Abs(Pathfinding.Pathfinding.GetCellXCoord(id1, _map.Width) - Pathfinding.Pathfinding.GetCellXCoord(id2, _map.Width));
            int diffY = Math.Abs(Pathfinding.Pathfinding.GetCellYCoord(id1, _map.Width) - Pathfinding.Pathfinding.GetCellYCoord(id2, _map.Width));
            return (diffX + diffY);
        }

        public static int GetFarestCellForGoingToFighter(Entity currentEntity, Entity target, int baseCell, List<int> closedList)
        {
            int cell = -1;
            List<int> adjaCells = Pathfinding.Pathfinding.GetJoinCell(baseCell, currentEntity.CurrentFight.map);
            int dist = 0;
            int timeout = 0;

            foreach (int aCell in adjaCells)
            {
                if (currentEntity.CurrentFight.map.Cells[aCell].IsWalkable
                    && currentEntity.CurrentFight.AllEntityInFight.Exists(x => !x.FightInfo.isDead && x.FightInfo.FightCell != aCell)
                    && !closedList.Contains(aCell))
                {
                    int fDist = AIUtil.GetDistanceBetween(aCell, target.FightInfo.FightCell, currentEntity.CurrentFight.map);
                    if (fDist > dist)
                    {
                        cell = aCell;
                        dist = fDist;
                    }
                }
                timeout += 1;
                if (timeout > 100)
                    break;
            }
            return cell;
        }

        public static int GetNearestCellForGoingToFighter(Entity target, int baseCell, List<int> closedList)
        {
            int cell = -1;
            List<int> adjaCells = Pathfinding.Pathfinding.GetJoinCell(baseCell, target.CurrentFight.map);
            int dist = 1000;
            int timeout = 0;
            foreach (int aCell in adjaCells)
            {
                if (target.CurrentFight.map.Cells[aCell].IsWalkable
                    && target.CurrentFight.AllEntityInFight.Exists(x => !x.FightInfo.isDead && x.FightInfo.FightCell != aCell)
                    && !closedList.Contains(aCell))
                {
                    int fDist = AIUtil.GetDistanceBetween(aCell, target.FightInfo.FightCell, target.CurrentFight.map);
                    if (fDist < dist)
                    {
                        cell = aCell;
                        dist = fDist;
                    }
                }
                timeout += 1;
                if (timeout > 100)
                    break;
            }
            if (cell == -1)
                return baseCell;
            return cell;
        }

        public static bool CanHit(int cellID, Entity monster)
        {
            Entity nearestFighter = AIUtil.GetNearestFighter(monster);
            if (nearestFighter == null)
            {
                return false;
            }
            int timeout = 100;
            foreach (Spells.SpellsEntity ownSpell in monster.Spells)
            {
                Spells.SpellsStats currentSpells = Database.table.Spells.SpellsList[ownSpell.id].SpellsStats[ownSpell.level - 1];
                if (currentSpells.GetmaxPO(monster.PO) >= GetDistanceBetween(cellID, nearestFighter.FightInfo.FightCell, monster.CurrentFight.map))
                {
                    return true;
                }
                timeout++;
                if (timeout > 100)
                    break;
            }
            return false;
        }
    }
}
