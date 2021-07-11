using LeafEmu.World.Game.Spells.SpellsEffect;
using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;

namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect.Steal
{
    class Steal
    {

        //VolEau = 91,
        //VolTerre = 92,
        //VolAir = 93,
        //VolFeu = 94,
        //VolNeutre = 95,


        [EffectAttribute(new int[5] { 91, 92, 93, 94, 95 })]
        public void StealLife(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var Target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (Target != null)
            {
                Damage.SwitchDmgElem(spells.effectID + 5, laucher, Target, spells, true);
            }
        }

        [EffectAttribute(new int[10] { 266, 267, 268, 269, 270, 271, 320, 77, 82, 84 })]
        public void StealCarac(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var Target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);

            if (Target != null)
            {
                Game.Entity.Entity Laucher = laucher;
                int jet = Util.rng.Next(spells.JetMin, spells.JetMax);

                switch ((enumSpellsEffects)spells.effectID)
                {
                    case enumSpellsEffects.VolChance:
                        Buff.Buff.AddBuff(laucher, Laucher, spells, jet, "Chance");
                        Buff.Buff.AddBuff(laucher, Target, spells, -jet, "Chance");
                        break;
                    case enumSpellsEffects.VolForce:
                        Buff.Buff.AddBuff(laucher, Laucher, spells, jet, "Force");
                        Buff.Buff.AddBuff(laucher, Target, spells, -jet, "Force");
                        break;
                    case enumSpellsEffects.VolAgi:
                        Buff.Buff.AddBuff(laucher, Laucher, spells, jet, "Agi");
                        Buff.Buff.AddBuff(laucher, Target, spells, -jet, "Agi");
                        break;
                    case enumSpellsEffects.VolIntell:
                        Buff.Buff.AddBuff(laucher, Laucher, spells, jet, "Intell");
                        Buff.Buff.AddBuff(laucher, Target, spells, -jet, "Intell");
                        break;
                    case enumSpellsEffects.VolSagesse:
                        Buff.Buff.AddBuff(laucher, Laucher, spells, jet, "Sagesse");
                        Buff.Buff.AddBuff(laucher, Target, spells, -jet, "Sagesse");
                        break;
                    case enumSpellsEffects.VolNeutreDmg:
                        return;
                        Buff.Buff.AddBuff(laucher, Laucher, spells, jet, "DamageNeutre");
                        Buff.Buff.AddBuff(laucher, Target, spells, -jet, "DamageNeutre");
                        break;
                    case enumSpellsEffects.VolPO:
                        Buff.Buff.AddBuff(laucher, Laucher, spells, jet, "PO");
                        Buff.Buff.AddBuff(laucher, Target, spells, -jet, "PO");
                        break;
                    case enumSpellsEffects.VolPA:
                        Buff.Buff.AddBuff(laucher, Laucher, spells, jet, "PA");
                        Buff.Buff.AddBuff(laucher, Target, spells, -jet, "PA");
                        break;
                    case enumSpellsEffects.VolPM:
                        Buff.Buff.AddBuff(laucher, Laucher, spells, jet, "PM");
                        Buff.Buff.AddBuff(laucher, Target, spells, -jet, "PM");
                        break;
                    case enumSpellsEffects.VolVie:
                        Buff.Buff.AddBuff(laucher, Laucher, spells, jet, "Vie");
                        Buff.Buff.AddBuff(laucher, Target, spells, -jet, "Vie");
                        break;



                }
            }

        }

    }
}
