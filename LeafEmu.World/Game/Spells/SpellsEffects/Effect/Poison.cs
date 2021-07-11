using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;

namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect
{
    public class Poison
    {
        [EffectAttribute(new int[1] { 131 })]
        public void poison(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (target == null)
                return;
            Buff.Buff.AddBuff(laucher, target, spells, spells.JetMax, null);
        }
    }
}
