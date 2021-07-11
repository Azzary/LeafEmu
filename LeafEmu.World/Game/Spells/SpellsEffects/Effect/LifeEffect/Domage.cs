using LeafEmu.World.Game.Spells.SpellsEffect.Gestion;
using System;
using System.Text;

namespace LeafEmu.World.Game.Spells.SpellsEffects.Effect
{
    public class Damage
    {
        public static void DmgPouse(Entity.Entity laucher, Entity.Entity target, int NbCells)
        {
            DmgLife(laucher, (laucher.level / 2 + (laucher.DamagePousser - target.ResPousser) + 32) * NbCells / 4, target, false);
        }


        private static float TotalDmg(int Dmg, float DamagePercent, int Carac, int DmgBoost)
        {
            //(effectBase * (100 + stats.Total + fighter.Stats.PercentPointBonus.Total) / 100 + fighter.Stats.FixPointBonus.Total)
            return (int)Math.Ceiling((double)(Dmg * (100 + Carac + DamagePercent) / 100 + DmgBoost));
            //return (Dmg * ((Carac) / 100) + DmgCarac + Dmg) * (DamagePercent/100 + 1);
        }
        private static int TakenDmg(float _TotalDmg, int ResFix, int PercentageRes)
        {
            PercentageRes = PercentageRes == 0 ? 1 : PercentageRes;
            return (int)MathF.Ceiling(_TotalDmg - ResFix - (_TotalDmg / 100 / PercentageRes));
        }

        public static void DmgLife(Entity.Entity laucher, int LifeChange, Entity.Entity target, bool IsStilLife, bool CheckEnd = true)
        {
            LifeChange = LifeChange <= 0 ? 0 : (int)Math.Ceiling((float)(LifeChange) * ((float)(target.DamagePercentReceived) / 100 + 1)); //add % dmg sup of target
            LifeChange = Math.Clamp(LifeChange - laucher.Imunite, 0, LifeChange);  
            StringBuilder packet = new StringBuilder($"GA;100;{laucher.ID_InFight};{target.ID_InFight},-{LifeChange},2");
            if (target.ReverseDmg != 0)
            {
                int dmg = 0;
                if (LifeChange >= target.ReverseDmg)
                {
                    laucher.Vie -= target.ReverseDmg;
                    LifeChange -= target.ReverseDmg;
                    dmg = target.ReverseDmg;
                }
                else
                {
                    laucher.Vie -= LifeChange;
                    dmg = LifeChange;
                    LifeChange = 0;
                }
                packet.Append($"\0GA;107;{target.ID_InFight};{laucher.ID_InFight},{dmg},2");
                packet.Append($"\0GA;100;{target.ID_InFight};{laucher.ID_InFight},-{dmg},2");
            }
            LifeChange = target.BadChanceEca != 0 ? LifeChange * 2 : LifeChange;
            LifeChange = target.ChanceEca != 0 ? -LifeChange : LifeChange;
            target.Vie -= LifeChange;
            if (IsStilLife)
            {
                int lifeHeal = laucher.Vie + LifeChange <= laucher.TotalVie ?
                             LifeChange : laucher.TotalVie - laucher.Vie;
                laucher.Vie += lifeHeal;
                packet.Append($"\0GA;100;{target.ID_InFight};{laucher.ID_InFight},{lifeHeal},2");
            }

            if (target.Vie <= 0)
            {
                packet.Append("\0GA;103;" + target.ID_InFight + ";" + target.ID_InFight);
                laucher.CurrentFight.SendToAllFight(packet.ToString());

                target.FightInfo.isDead = true;
                target.FightInfo.FightCell = -100;
                if (CheckEnd)
                {
                    Fight.GestionFight.RemoveInvo(target);
                    target.CurrentFight.CheckEnd();
                }
            }
            else
                laucher.CurrentFight.SendToAllFight(packet.ToString());

        }

        [EffectAttribute(new int[5] { 96, 97, 98, 99, 100 })]
        public void DamageElem(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            Entity.Entity target = laucher.CurrentFight.AllEntityInFight.Find(x => !x.FightInfo.isDead && x.FightInfo.FightCell == TargetCell);
            if (target != null)
            {

                int Elem = spells.effectID;

                SwitchDmgElem(Elem, laucher, target, spells);
            }
        }

        [EffectAttribute(new int[5] { 85, 86, 87, 88, 89 })]
        public void DamagePercentageLife(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            Entity.Entity target = spells.effectID == 111 ? laucher : laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (target != null)
            {
                float LifeDmg = (float)Util.rng.Next(spells.JetMin, spells.JetMax + 1) / 100;

                DmgLife(laucher, (int)Math.Round(target.Vie * LifeDmg), target, false);
            }
        }

        /// <summary>
        /// % DMG suplementaire 
        /// </summary>
        [EffectAttribute(new int[1] { 776 })]
        public void DamageEffect(Entity.Entity laucher, SpellsEffect.Spells spells, int BaseCellTarget, int TargetCell, bool isFristTime)
        {
            Entity.Entity target = laucher.CurrentFight.AllEntityInFight.Find(x => x.FightInfo.FightCell == TargetCell);
            if (target != null)
            {
                int jet = Util.rng.Next(spells.JetMin, spells.JetMax + 1);

                Buff.Buff.AddBuff(laucher, target, spells, jet, "DamagePercentReceived");
            }
        }

        public static void SwitchDmgElem(int Elem, Entity.Entity laucher, Entity.Entity target, SpellsEffect.Spells spells, bool IsStilLife = false)
        {
            int jet = Util.rng.Next(spells.JetMin, spells.JetMax + 1);
            switch (Elem)
            {
                case 96:
                    DmgLife(laucher, TakenDmg(
                                        TotalDmg(jet, laucher.DamagePercent, laucher.Chance, laucher.DamageChance + laucher.Damage + laucher.DamageMagic),
                                        target.F_TotalResEau + target.ResMagic, target.P_TotalResEau),
                                        target,
                                        IsStilLife);
                    break;
                case 97:
                    DmgLife(laucher, TakenDmg(
                                        TotalDmg(jet, laucher.DamagePercent, laucher.Force, laucher.DamageForce + laucher.Damage + laucher.DamagePhysic),
                                        target.F_TotalResForce + target.ResPhysic, target.P_TotalResForce),
                                        target,
                                        IsStilLife);
                    break;
                case 98:
                    DmgLife(laucher, TakenDmg(
                                        TotalDmg(jet, laucher.DamagePercent, laucher.Agi, laucher.DamageAgi + laucher.Damage + laucher.DamageMagic),
                                        target.F_TotalResAgi + target.ResMagic, target.P_TotalResAgi),
                                        target,
                                        IsStilLife);
                    break;
                case 99:
                    DmgLife(laucher, TakenDmg(
                                        TotalDmg(jet, laucher.DamagePercent, laucher.Intell, laucher.DamageIntell + laucher.Damage + laucher.DamageMagic),
                                        target.F_TotalResIntell + target.ResMagic, target.P_TotalResIntell),
                                        target,
                                        IsStilLife);
                    break;
                case 100:
                    DmgLife(laucher, TakenDmg(
                                        TotalDmg(jet, laucher.DamagePercent, 0, laucher.DamageNeutre + laucher.Damage + laucher.DamagePhysic),
                                        target.F_TotalResNeutre + target.ResPhysic, target.P_TotalResNeutre),
                                        target,
                                        IsStilLife);
                    break;
            }

        }

    }
}
