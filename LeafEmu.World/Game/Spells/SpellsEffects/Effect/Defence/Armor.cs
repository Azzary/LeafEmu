
using LeafEmu.World.Game.Spells.SpellsEffect;
using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;

namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect.Defence
{
    class Armor
    {

        [EffectAttribute(new int[1] { 265 })]
        public void AddArmorBis(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {

            var Target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (Target == null)
                return;
            int intell = laucher.Intell;
            int jet = Util.rng.Next(spells.JetMin, spells.JetMax);
            switch (spells.SpellsID)
            {
                case 18://Aqueuse
                    jet += jet * (intell + laucher.Chance / 2) / 100;
                    Buff.Buff.AddBuff(laucher, Target, spells, jet, "F_TotalResEau");
                    break;
                case 1://Incandescente
                    jet += jet * (intell + laucher.Intell / 2) / 100;
                    Buff.Buff.AddBuff(laucher, Target, spells, jet, "F_TotalResIntell");
                    break;
                case 6://Terrestre
                    jet += jet * (intell + laucher.Force / 2) / 100;
                    Buff.Buff.AddBuff(laucher, Target, spells, jet, "F_TotalResForce");
                    break;
                case 14://Venteuse
                    jet += jet * (intell + laucher.Agi / 2) / 100;
                    Buff.Buff.AddBuff(laucher, Target, spells, jet, "F_TotalResAgi");
                    break;
            }

        }


        [EffectAttribute(new int[1] { 107 })]
        public void ReverseSpells(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {

            var Target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (Target == null)
                return;
            int jet = Util.rng.Next(spells.JetMin, spells.JetMax);
            switch ((enumSpellsEffects)spells.effectID)
            {
                case enumSpellsEffects.AddRenvoiDamage:
                    Buff.Buff.AddBuff(laucher, Target, spells, jet, "ReverseDmg");
                    break;

            }
            
        }

        [EffectAttribute(new int[1] { 105 })]
        public void Imunite(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var Target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (Target == null)
                return;
            int jet = Util.rng.Next(spells.JetMin, spells.JetMax);
            Buff.Buff.AddBuff(laucher, Target, spells, jet, "Imunite");
        }

        [EffectAttribute(new int[5] { 210, 211, 212, 213, 214 })]
        public void AddPourcentRes(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var Target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (Target == null)
                return;
            int jet = Util.rng.Next(spells.JetMin, spells.JetMax);
            switch ((enumSpellsEffects)spells.effectID)
            {
                case enumSpellsEffects.AddReduceDamagePourcentAir:
                    Buff.Buff.AddBuff(laucher, Target, spells, jet, "P_TotalResAgi");
                    break;
                case enumSpellsEffects.AddReduceDamagePourcentEau:
                    Buff.Buff.AddBuff(laucher, Target, spells, jet, "P_TotalResEau");
                    break;
                case enumSpellsEffects.AddReduceDamagePourcentFeu:
                    Buff.Buff.AddBuff(laucher, Target, spells, jet, "P_TotalResIntell");
                    break;
                case enumSpellsEffects.AddReduceDamagePourcentTerre:
                    Buff.Buff.AddBuff(laucher, Target, spells, jet, "P_TotalResForce");
                    break;
                case enumSpellsEffects.AddReduceDamagePourcentNeutre:
                    Buff.Buff.AddBuff(laucher, Target, spells, jet, "P_TotalResNeutre");
                    break;

            }
        }
    }
}
