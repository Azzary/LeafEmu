using System;

namespace LeafEmu.World.Game.Spells.SpellsEffect.Gestion
{
    class EffectAttribute : Attribute
    {

        public int[] EffectID;

        public EffectAttribute(int[] _EffectID) => EffectID = _EffectID;


    }
}
