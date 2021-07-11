
using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect
{
    class Boost
    {

        [EffectAttribute(new int[1] { 10100 })]
        public Byte Roulette(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            SpellsStats StatsSpells = Database.table.Spells.SpellsList[6666].SpellsStats[spells.SpellsLvl - 1];
            int RouletteEffect = Util.rng.Next(0, StatsSpells.effects.Count - 1);
            List<Map.Cell.Cell> TargetsCells = laucher.CurrentFight.PlayerInFight.Select(x => laucher.CurrentFight.map.Cells[x.account.character.FightInfo.FightCell]).ToList();
            StatsSpells.effects[RouletteEffect].ChanceOfLineCast = 0;
            EffectGestion.Gestion(laucher, StatsSpells.effects[RouletteEffect], laucher.FightInfo.FightCell, TargetsCells);
            TargetsCells.Clear();
            TargetsCells.Add(laucher.CurrentFight.map.Cells[laucher.FightInfo.FightCell]);
            EffectGestion.Gestion(laucher, StatsSpells.effects[StatsSpells.effects.Count - 1], BaseCellTarget, TargetsCells);
            return 0;
        }

        [EffectAttribute(new int[4] { 138, 112, 142, 143 })]
        public void DmgBoost(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            var Target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (Target != null)
            {
                Entity.Entity Laucher = laucher;
                int jet = Util.rng.Next(spells.JetMin, spells.JetMax);


                switch (spells.effectID)
                {
                    case 138:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "DamagePercent");
                        break;
                    case 112:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "Damage");
                        break;
                    case 142:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "DamagePhysic");
                        break;
                    case 143:
                        Buff.Buff.AddBuff(laucher, Target, spells, jet, "DamageMagic");
                        break;
                }
            }
        }

    }
}
