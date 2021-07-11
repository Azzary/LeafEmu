
using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;
using System;


namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect.LifeEffect
{
    class Heal
    {
        public int FormuleHeal(int jet, Game.Entity.Entity Laucher)
        {
            return (int)Math.Floor((double)(jet * (100 + Laucher.Intell) / 100 + Laucher.HealBonus));
        }

        public void Life(Entity.Entity laucher, Game.Entity.Entity target, int lifeChange)
        {
            string packet = $"GA;100;{laucher.ID_InFight};{target.ID_InFight},+{lifeChange},2";
            laucher.CurrentFight.SendToAllFight(packet);
        }

        public void GiveLife(Entity.Entity laucher, Game.Entity.Entity target, int jet)
        {
            jet = jet + target.Vie <= target.TotalVie ? jet : target.TotalVie - target.Vie;
            target.Vie += jet;
            Life(laucher, target, FormuleHeal(jet, laucher));
        }

        [EffectAttribute(new int[2] { 125, 108 })]
        public void Heals(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (target != null)
            {
                int jet = Util.rng.Next(spells.JetMin, spells.JetMax + 1);
                switch (spells.effectID)
                {
                    case 125:
                        Buff.Buff.AddBuff(laucher, target, spells, jet, "Vie");
                        break;
                    case 108: //buff
                        GiveLife(laucher, target, jet);
                        break;
                }
            }
        }

    }
}
