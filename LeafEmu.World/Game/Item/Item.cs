using System;

namespace LeafEmu.World.Game.Item
{
    public class Item
    {
        public int ID { get; set; }

        public int Niveau { get; set; }
        public int Pods { get; set; }
        public int Type { get; set; }
        public int Quantity { get; set; }
        public string Stats { get; set; }
        public string Condition { get; set; }
        public string infosWeapon { get; set; }

        #region drop
        public Decimal[] PercentDropeByGrade { get; set; }
        public UInt16 ceil { get; set; } //Prospection ceil
        public int ActionDrops { get; set; }
        public string ConditionDrops { get; set; }
        public int levelDrops { get; set; }
        #endregion
        public Item(int _ID, int _pods, int _Niveau, int _Type, string _ModelStat, string _condition, string _infosWeapon)
        {
            // set after drop table load
            ceil = 0;
            ActionDrops = -1;
            levelDrops = -1;
            // set after drop table load
            ID = _ID;
            Type = _Type;
            Pods = _pods;
            Niveau = _Niveau;
            Stats = _ModelStat;
            Condition = _condition;
            infosWeapon = _infosWeapon;
            PercentDropeByGrade = new Decimal[5] { 0, 0, 0, 0, 0 };
        }

        public string GetRandomEffect(string stats)
        {
            return stats;
        }
    }
}
