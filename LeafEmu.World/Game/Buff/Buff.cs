using LeafEmu.World.Game.Spells.SpellsEffect;
using System;
using System.Reflection;

namespace LeafEmu.World.Game.Buff
{
    public class Buff
    {
        public int SpellID { get; }
        public int EffectID { get; }
        private string varEffectName { get; set; }
        public int jet { get; set; }
        public int LifeTime { get; set; }
        public Entity.Entity laucher { get; set; }


        public static void SendBoost(Entity.Entity laucher, Entity.Entity _CharacterTarget, Spells.SpellsEffect.Spells spells, int Boost)
        {
            int ActionGA = GetEffectToCaracID(spells.effectID, Boost);
            Boost = ActionGA != spells.effectID ? Math.Abs(Boost) : Boost;
            string packet = $"GA;{ActionGA};{laucher.ID_InFight};{laucher.ID_InFight},{Boost},{spells.turns}";
            string BuffPacket = $"GIE{spells.effectID};{_CharacterTarget.id};{Boost};;;;{spells.turns};{spells.SpellsID}";
            laucher.CurrentFight.SendToAllFight($"{BuffPacket}\0" + packet);
        }

        private Buff(int _SpellID, int _EffectID, int _LifeTime, string _varEffectName, int _jet, Entity.Entity _laucher)
        {
            laucher = _laucher;
            jet = _jet;
            varEffectName = _varEffectName;
            EffectID = _EffectID;
            SpellID = _SpellID;
            LifeTime = _LifeTime;
        }

        /// <summary>
        /// Use System.Reflection to add buff to target and create Buff object
        /// You juste need to give a variable name (varName) who exist in Entity.
        /// Null varName for poison...
        /// </summary>
        /// <param name="Laucher"></param>
        /// <param name="target"></param>
        /// <param name="spells"></param>
        /// <param name="jet"></param>
        /// <param name="varName"></param>
        public static bool AddBuff(Entity.Entity laucher, Entity.Entity target, Spells.SpellsEffect.Spells spells, int jet, string varName)
        {
            lock (target.Buffs)
            {
                target.Buffs.Add(new Buff(spells.effectID, spells.effectID, spells.turns + target.CurrentFight.nbTour, varName, jet, laucher));
                if (!SetValue(varName, jet, target, typeof(Entity.Character)))
                {
                    Logger.Logger.Log(($"[Buff Error] {varName}  " + (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name));
                    return false;
                }
            }
            SendBoost(laucher, target, spells, jet);
            return true;
        }

        public static void GestionBuff(Entity.Entity target)
        {
            if (target.CurrentFight == null)
            {
                return;
            }
            lock (target.Buffs)
            {
                for (int i = 0; i < target.Buffs.Count; i++)
                {
                    Buff buff = target.Buffs[i];
                    if (buff.LifeTime == target.CurrentFight.nbTour)
                    {
                        if (target.Buffs[i].EffectID == (int)enumSpellsEffects.PreventSpellLauch)
                            target.CurrentFight.PreventsSpells.Remove(target.Buffs[i].SpellID);
                        SetValue(target.Buffs[i].varEffectName, -target.Buffs[i].jet, target, typeof(Entity.Character));
                        target.Buffs.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        switch (buff.EffectID)
                        {
                            case 131:
                                //[(Intelligence + % domms) / 100 + 1] x nombre de PA utilisés
                                int dmg = ((buff.laucher.Intell + buff.laucher.DamagePercent) / 100 + 1) * target.TotalPA - target.PA;
                                Spells.SpellsEffects.Effect.Damage.DmgLife(buff.laucher, dmg, target, false);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        //Metre dans utils
        public static bool SetValue(string varName, int boost, object target, Type t)
        {
            if (varName == null)
                return true;

            foreach (var member in t.GetMembers(BindingFlags.Instance | BindingFlags.Public))
            {
                if (member.Name == varName)
                {
                    PropertyInfo fi = (PropertyInfo)member;
                    fi.SetValue(target, (int)fi.GetValue(target) + boost);
                    return true;
                }
            }
            return false;
        }

        private static int GetEffectToCaracID(int BuffEffectID, int effect)
        {
            enumSpellsEffects CaracID = enumSpellsEffects.None;

            switch ((enumSpellsEffects)BuffEffectID)
            {
                case enumSpellsEffects.VolPO:
                    CaracID = effect > 0 ? enumSpellsEffects.AddPO : enumSpellsEffects.SubPO;
                    break;
                case enumSpellsEffects.VolAgi:
                    CaracID = effect > 0 ? enumSpellsEffects.AddAgilite : enumSpellsEffects.SubAgilite;
                    break;
                case enumSpellsEffects.VolForce:
                    CaracID = effect > 0 ? enumSpellsEffects.AddForce : enumSpellsEffects.SubForce;
                    break;
                case enumSpellsEffects.VolIntell:
                    CaracID = effect > 0 ? enumSpellsEffects.AddIntelligence : enumSpellsEffects.SubIntelligence;
                    break;
                case enumSpellsEffects.VolChance:
                    CaracID = effect > 0 ? enumSpellsEffects.AddIntelligence : enumSpellsEffects.SubIntelligence;
                    break;
                    //case enumSpellsEffects.SubPAEsquive:
                    //case enumSpellsEffects.VolPA:
                    //    CaracID =  effect > 0 ? enumSpellsEffects.AddPA : enumSpellsEffects.SubPA;
                    //    break;
                    //case enumSpellsEffects.SubPMEsquive:
                    //case enumSpellsEffects.VolPM:
                    //    CaracID =  effect > 0 ? enumSpellsEffects.AddPM : enumSpellsEffects.SubPM;
                    //    break;

            }

            return CaracID == enumSpellsEffects.None ? BuffEffectID : (int)CaracID;
        }
    }
}
