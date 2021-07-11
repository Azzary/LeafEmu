
using LeafEmu.World.Game.Spells.SpellsEffect;
using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;
using System;

namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect.Retrait
{
    class Retrait
    {
        private static int RetraitFormule(Game.Entity.Entity laucher, Game.Entity.Entity target, int nbRetait, int nbStats, int totalStats, float retrait, float esquive)
        {
            int StatsLost = 0;
            for (int i = 0; i < nbRetait; i++)
            {
                //PA ou PM restants de la cible/PA ou PM totaux de la cible * Retrait du lanceur/Esquive de la cible * 0.5
                float paSTotal = (float)nbStats / (float)totalStats;
                retrait = MathF.Floor((float)laucher.Sagesse / 5) * (retrait + 1);
                esquive = MathF.Floor((float)target.Sagesse / 5) * ((esquive + 1) * 0.5f);

                int ChanceOfSum = (int)Math.Clamp((paSTotal * (retrait / (esquive + 1)) * 100) + laucher.P_RetraitPA - target.P_EsquivePA, 10, 90);
                if (Util.rng.Next(0, 101) <= ChanceOfSum)
                {
                    StatsLost++;
                    nbStats--;
                }
            }
            return StatsLost;
        }



        [EffectAttribute(new int[4] { 160, 161, 162, 163 })]
        public void AddRetraitEsquive(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var Target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            int jet = Util.rng.Next(spells.JetMin, spells.JetMax + 1);
            if (Target != null)
            {
                switch ((enumSpellsEffects)spells.effectID)
                {
                    case enumSpellsEffects.SubEsquivePA:
                        Buff.Buff.AddBuff(laucher, Target, spells, -jet, "P_EsquivePA");
                        break;
                    case enumSpellsEffects.AddEsquivePA:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "P_EsquivePA");
                        break;
                    case enumSpellsEffects.SubEsquivePM:
                        Buff.Buff.AddBuff(laucher, Target, spells, -jet, "P_EsquivePM");
                        break;
                    case enumSpellsEffects.AddEsquivePM:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "P_EsquivePM");
                        break;

                }
            }
        }

        [EffectAttribute(new int[2] { 127, 101 })]
        public void Retrait_Esquive(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var Target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (Target != null)
            {
                int nb = 0;
                switch (spells.effectID)
                {
                    case 101://pa
                        nb = -RetraitFormule(laucher, Target, spells.JetMax, Target.PA, Target.TotalPA, laucher.RetraitPA, Target.EsquivePA);
                        if (nb != 0)
                        {
                            Buff.Buff.AddBuff(laucher, Target, spells, nb, "PA");
                        }
                        break;
                    case 127://pm
                        nb = -RetraitFormule(laucher, Target, spells.JetMax, Target.PM, Target.TotalPM, laucher.RetraitPM, Target.EsquivePM);
                        if (nb != 0)
                        {
                            Buff.Buff.AddBuff(laucher, Target, spells, nb, "PM");
                        }
                        break;
                }
            }
        }

        [EffectAttribute(new int[9] { 171, 154, 152, 157, 156, 155, 168, 169, 116 })]
        public void Retrait_NotEsquive(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var Target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (Target != null)
            {
                int jet = -Util.rng.Next(spells.JetMin, spells.JetMax);
                switch ((enumSpellsEffects)spells.effectID)
                {
                    case enumSpellsEffects.SubCritic:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "CC");
                        break;
                    case enumSpellsEffects.SubAgilite:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "Agi");
                        break;
                    case enumSpellsEffects.SubChance:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "Chance");
                        break;
                    case enumSpellsEffects.SubForce:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "Force");
                        break;
                    case enumSpellsEffects.SubIntelligence:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "Intell");
                        break;
                    case enumSpellsEffects.SubSagesse:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "Sagesse");
                        break;
                    case enumSpellsEffects.SubPA:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "PA");
                        break;
                    case enumSpellsEffects.SubPM:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "PM");
                        break;
                    case enumSpellsEffects.SubPO:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "PO");
                        break;


                }
            }
        }
    }
}
