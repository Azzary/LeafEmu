
using LeafEmu.World.Game.Spells.SpellsEffect;
using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;

namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect
{
    public class CaracEffect
    {
        public static void PA(Entity.Entity entity, int pa)
        {
            entity.CurrentFight.SendToAllFight($"GA;102;{entity.ID_InFight};{entity.ID_InFight},{pa}");
        }

        public static void PM(Entity.Entity entity, short pm)
        {
            entity.CurrentFight.SendToAllFight($"GA;128;{entity.ID_InFight};{entity.ID_InFight},{pm}");
        }



        [EffectAttribute(new int[7] { 123, 119, 126, 118, 111, 120, 128 })]
        public void AddCarac(Entity.Entity entity, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {

            var target = entity.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (target == null)
                return;

            int jet = Util.rng.Next(spells.JetMin, spells.JetMax);
            switch ((enumSpellsEffects)spells.effectID)
            {
                case enumSpellsEffects.AddAgilite:
                    Buff.Buff.AddBuff(entity, target, spells, jet, "Agi");
                    break;
                case enumSpellsEffects.AddForce:
                    Buff.Buff.AddBuff(entity, target, spells, jet, "Force");
                    break;
                case enumSpellsEffects.AddChance:
                    Buff.Buff.AddBuff(entity, target, spells, jet, "Chance");
                    break;
                case enumSpellsEffects.AddIntelligence:
                    Buff.Buff.AddBuff(entity, target, spells, jet, "Intell");
                    break;
                case enumSpellsEffects.AddPABis:
                    Buff.Buff.AddBuff(entity, target, spells, jet, "PA");
                    break;
                case enumSpellsEffects.AddPA:
                    Buff.Buff.AddBuff(entity, target, spells, jet, "PA");
                    break;
                case enumSpellsEffects.AddPM:
                    Buff.Buff.AddBuff(entity, target, spells, jet, "PM");
                    break;
            }
        }


    }
}
