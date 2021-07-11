
using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;

namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect
{
    class BaseEffect
    {


        [EffectAttribute(new int[1] { 950 })]
        public void PreventSpell(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            laucher.CurrentFight.PreventsSpells.Add(spells.SpellsID);
            Buff.Buff.AddBuff(laucher, laucher, spells, 0, null);
        }

        [EffectAttribute(new int[1] { 79 })]
        public void Chance(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (target == null)
                return;
            if (Util.rng.Next(0, 2) == 0)//Soin
            {
                Buff.Buff.AddBuff(laucher, target, spells, 1, "ChanceEca");
            }
            else//Dmg * 2
            {
                Buff.Buff.AddBuff(laucher, target, spells, 1, "BadChanceEca");
            }
        }

    }
}
