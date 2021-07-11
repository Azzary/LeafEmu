using System.Reflection;



namespace LeafEmu.World.Game.Spells.SpellsEffect.Gestion
{
    public class EffectDatas
    {
        public object instance { get; set; }
        public int[] EffectID { get; set; }
        public MethodInfo information { get; set; }

        public EffectDatas(object _instance, int[] _EffectID, MethodInfo _information)
        {
            EffectID = _EffectID;
            instance = _instance;
            information = _information;
        }
    }
}