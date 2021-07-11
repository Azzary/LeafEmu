
using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;

namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect
{
    class Resistance
    {

        [EffectAttribute(new int[2] { 183, 184 })]
        public void ResPhysic_Magic(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var Target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (Target != null)
            {
                int jet = Util.rng.Next(spells.JetMin, spells.JetMax);

                switch (spells.effectID)
                {
                    case 183:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "ResMagic");
                        break;
                    case 184:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "ResPhysic");
                        break;
                }
            }
        }
    }
}
