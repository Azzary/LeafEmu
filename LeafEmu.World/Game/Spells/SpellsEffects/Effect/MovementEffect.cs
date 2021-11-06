using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;

namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect
{
    class MovementEffect
    {

        public static void MouvPlayer(Entity.Entity laucher, Entity.Entity target, int ID_InFight_InFightLauncher, int EffectID_InFight_InFight)
        {
            laucher.CurrentFight.SendToAllFight($"GA;{EffectID_InFight_InFight};{ID_InFight_InFightLauncher};{target.ID_InFight},{target.FightInfo.FightCell}\0");
        }

        [EffectAttribute(new int[1] { 4 })]
        public void Tp(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var temp = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);

            if (temp == null)
            {
                Map.Cell.Cell cell = laucher.CurrentFight.map.Cells[TargetCell];
                if (cell.IsWalkable)
                {
                    laucher.FightInfo.FightCell = TargetCell;
                    MouvPlayer(laucher, laucher, laucher.ID_InFight, 4);
                }
            }

        }

        [EffectAttribute(new int[2] { 5, 6 })]
        public void PushBack(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (target != null)
            {
                int CellBasePush = BaseCellTarget == TargetCell ? laucher.FightInfo.FightCell : BaseCellTarget;

                int dir = Map.Pathfinding.GetDirection(laucher.CurrentFight.map,
                                                CellBasePush,
                                                target.FightInfo.FightCell);

                dir = spells.effectID == 5 ? dir : Map.Pathfinding.OppositeDirection(dir);
                int NewCell = target.FightInfo.FightCell;

                int i;
                for (i = 0; i < spells.value; i++)
                {
                    NewCell = Map.Pathfinding.NextCell(laucher.CurrentFight.map, NewCell, dir);
                    if (!laucher.CurrentFight.map.Cells[NewCell].IsWalkable || laucher.CurrentFight.PlayerInFight.Exists(x => x.account.character.FightInfo.FightCell == NewCell))
                    {
                        break;
                    }
                    else
                        target.FightInfo.FightCell = NewCell;
                }
                if (spells.effectID == 5 && i < spells.value - 1)
                {
                    Damage.DmgPouse(laucher, target, spells.value - i);
                }
                MouvPlayer(laucher, target, laucher.ID_InFight, 5);
            }
        }


    }
}
