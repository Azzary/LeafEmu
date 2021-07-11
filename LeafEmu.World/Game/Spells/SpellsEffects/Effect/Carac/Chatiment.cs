using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;

namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect.Carac
{
    class Chatiment
    {

        [EffectAttribute(new int[1] { 788 })]
        public void ChatimentTest(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var Target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (Target != null)
            {
                Game.Entity.Entity Laucher = laucher;
                int jet = Util.rng.Next(spells.JetMin, spells.JetMax);


                switch (spells.effectID)
                {
                    case 788:
                        //Buff.Buff.AddBuff(prmClient, Target, spells, jet, "Chatiment");
                        break;
                }
            }

        }

    }
}
