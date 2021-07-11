using LeafEmu.World.Game.Spells;
using System;
using System.Collections.Generic;
using System.Threading;

namespace LeafEmu.World.Game.Entity.AI
{
    public abstract class AI
    {
        public Entity monster;
        public bool FirstTurn = true;
        public AI(Entity mob)
        {
            monster = mob;
        }

        public virtual void PlayAI()
        {
            try
            {
                List<int> NextMove = new List<int>();
                NextMove = MoveNear();
                Move(NextMove);
            }
            catch (Exception)
            {
            }
        }
        public List<int> MoveUntilCanHit()
        {
            List<int> moves = new List<int>();
            List<int> closedList = new List<int>();
            Entity nearestFighter = AIUtil.GetNearestFighter(monster);
            if (nearestFighter == null)
            {
                return null;
            }
            int mp = monster.PM;
            int baseCell = monster.FightInfo.FightCell;
            int timeout = 0;
            while (mp != 0)
            {
                closedList.Add(baseCell);
                if (AIUtil.GetDistanceBetween(baseCell, nearestFighter.FightInfo.FightCell, monster.CurrentFight.map) == 1)
                    break;
                int nextCell = AIUtil.GetNearestCellForGoingToFighter(nearestFighter, baseCell, closedList);
                if (nextCell != -1)
                {
                    moves.Add(nextCell);
                    baseCell = nextCell;
                    if (AIUtil.CanHit(baseCell, monster))
                    {
                        break;
                    }
                }
                mp--;
                timeout++;
                if (timeout > 1000)
                    break;
            }
            return moves;
        }
        public void AttackNeightboor()
        {
            try
            {
                int timeout = 0;
            reCast:
                if (timeout > 100)
                    return;
                Entity nearestEntity = AIUtil.GetNearestFighter(monster);
                if (nearestEntity == null)
                {
                    return;
                }

                foreach (SpellsEntity ownSpell in monster.Spells)
                {
                    if (Database.table.Spells.SpellsList[ownSpell.id].TypeOfSpell != SpellsEnumType.attaque)
                        continue;

                    SpellsStats currentSpells = Database.table.Spells.SpellsList[ownSpell.id].SpellsStats[ownSpell.level - 1];

                    if (currentSpells.GetmaxPO(monster.PO) >= AIUtil.GetDistanceBetween(monster.FightInfo.FightCell, nearestEntity.FightInfo.FightCell, nearestEntity.CurrentFight.map))
                    {
                        if (monster.PA >= currentSpells.PACost)
                        {
                            if (SpellsManagement.CheckLaunchSpells(monster.FightInfo.FightCell, nearestEntity.FightInfo.FightCell,
                                currentSpells, monster, out string msg))
                            {

                                SpellsManagement.LaunchSpells(currentSpells, monster, nearestEntity.FightInfo.FightCell);
                                Thread.Sleep(700);
                                timeout++;
                                goto reCast;
                            }
                        }
                    }
                    timeout++;
                    if (timeout > 200)
                        break;
                }

            }
            catch (Exception)
            {

            }
        }
        public void Move(List<int> path)
        {
            if (path == null || path.Count == 0)
            {
                return;
            }
            Map.Map map = monster.CurrentFight.map;
            string remakePath = Pathfinding.Pathfinding.CreateStringPath(monster.FightInfo.FightCell, 1, path, map);
            Pathfinding.PathfindingUtil pathfinding = new Pathfinding.PathfindingUtil(remakePath, map, monster.FightInfo.FightCell, 0);
            remakePath = pathfinding.GetStartPath + pathfinding.RemakePath();

            string chemain = ($"GA0;1;{monster.ID_InFight};{remakePath}\0GA;129;{monster.ID_InFight};{monster.id},-{path.Count}");
            monster.PM -= path.Count;
            monster.FightInfo.FightCell = path[path.Count - 1];
            monster.CurrentFight.SendToAllFight(chemain);
            Thread.Sleep(500 * path.Count);
        }
        public void BoostMe()
        {
            foreach (SpellsEntity ownSpell in monster.Spells)
            {
                if (Database.table.Spells.SpellsList[ownSpell.id].TypeOfSpell != SpellsEnumType.attaque)
                    continue;
                SpellsStats currentSpells = Database.table.Spells.SpellsList[ownSpell.id].SpellsStats[ownSpell.level - 1];

                if (monster.PA >= currentSpells.PACost)
                {
                    if (SpellsManagement.CheckLaunchSpells(monster.FightInfo.FightCell, monster.FightInfo.FightCell,
                            currentSpells, monster, out string msg))

                        SpellsManagement.LaunchSpells(currentSpells, monster, monster.FightInfo.FightCell);
                    Thread.Sleep(500);
                }
            }
        }
        public List<int> MoveFar()
        {
            List<int> moves = new List<int>();
            List<int> closedList = new List<int>();
            Entity nearestEntity = AIUtil.GetNearestFighter(monster);
            if (nearestEntity == null)
            {
                return null;
            }
            int mp = monster.PM;
            int baseCell = monster.FightInfo.FightCell;
            int timeout = 0;
            while (mp != 0)
            {
                closedList.Add(baseCell);
                int nextCell = AIUtil.GetFarestCellForGoingToFighter(monster, nearestEntity, baseCell, closedList);
                if (nextCell != -1)
                {
                    moves.Add(nextCell);
                    baseCell = nextCell;
                }
                mp--;
                timeout++;
                if (timeout > 100)
                    break;
            }
            return moves;
        }
        public List<int> MoveNear(bool far = false)
        {
            if (100 * monster.Vie / monster.TotalVie <= 10)
            {
                return MoveFar();
            }
            List<int> moves = new List<int>();
            Entity nearestEntity = AIUtil.GetNearestFighter(monster);
            if (nearestEntity == null)
            {
                return null;
            }
            int mp = monster.PM;
            int baseCell = monster.FightInfo.FightCell;
            var pathEngine = new Pathfinding.PathfindingV2(monster.CurrentFight.map);
            var path = pathEngine.FindShortestPath(baseCell, nearestEntity.FightInfo.FightCell, this.GetDynObs(nearestEntity.FightInfo.FightCell));
            foreach (var cell in path)
            {
                if (cell.cell.ID != nearestEntity.FightInfo.FightCell)
                {
                    moves.Add(cell.cell.ID);
                    mp--;
                    if (mp == 0) break;
                }
                else
                {
                    break;
                }
            }
            return moves;
        }
        private List<int> GetDynObs(int removeCell = -1)
        {
            var obs = new List<int>();
            foreach (var f in monster.CurrentFight.AllEntityInFight.FindAll(x => !x.FightInfo.isDead))
            {
                obs.Add(f.FightInfo.FightCell);
            }
            obs.Remove(monster.FightInfo.FightCell);
            if (removeCell != -1) obs.Remove(removeCell);
            return obs;
        }

    }
}
