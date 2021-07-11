using LeafEmu.World.PacketGestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LeafEmu.World.Game.Spells.SpellsEffect.Gestion
{
    class EffectGestion
    {
        public static readonly List<EffectDatas> metodos = new List<EffectDatas>();


        public static void init()
        {
            Assembly asm = typeof(Frame).GetTypeInfo().Assembly;

            foreach (MethodInfo type in asm.GetTypes().SelectMany(x => x.GetMethods()).Where(m => m.GetCustomAttributes(typeof(EffectAttribute), false).Length > 0))
            {
                EffectAttribute attribute = type.GetCustomAttributes(typeof(EffectAttribute), true)[0] as EffectAttribute;
                Type type_string = Type.GetType(type.DeclaringType.FullName);

                object instance = Activator.CreateInstance(type_string, null);
                metodos.Add(new EffectDatas(instance, attribute.EffectID, type));
            }
        }


        public static void Gestion(Entity.Entity entity, Spells spells, int BaseCellTarget, List<Map.Cell.Cell> CellsTargets)
        {
            EffectDatas method = metodos.Find(m => m.EffectID.FirstOrDefault(x => x == spells.effectID) != 0);
            try
            {
                if (method != null)
                {
                    for (int i = 0; i < CellsTargets.Count; i++)
                    {
                        if (entity.CurrentFight != null)
                        {
                            var res = method.information.Invoke(method.instance, new object[5] { entity, spells, BaseCellTarget, CellsTargets[i].ID, i == 0 });
                            if (res != null && (byte)res != 1)
                                return;
                        }
                        else
                            return;
                    }
                }
                else
                    Logger.Logger.Error("No effect for Spell :" + spells.effectID);

            }
            catch (Exception ex)
            {
                Logger.Logger.Log(ex);
            }
        }


    }
}
