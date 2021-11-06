using LeafEmu.World.Game.Fight;

namespace LeafEmu.World.Game.Entity
{
    public class Entity //: IEquatable<Entity>, IComparable<Entity>
    {
        public Inventaire.Inventaire Inventaire = new Inventaire.Inventaire();
        public List<Spells.SpellsEntity> Spells = new List<Spells.SpellsEntity>();
        public Fight.Fight CurrentFight { get; set; }
        public FightEntityInfo FightInfo { get; set; }
        public Entity ExchangeEntity { get; set; }
        public short Orientation { get; set; }
        public Int64 XP { get; set; }
        public Int64 kamas { get; set; }
        public int capital { get; set; }
        public int PSorts { get; set; }
        public int energie { get; set; }
        public int sexe { get; set; }
        public byte classe { get; set; }
        public string speudo { get; set; }
        public int gfxID { get; set; }
        public int ID_InFight { get; set; }
        public short level { get; set; }
        public int couleur1 { get; set; }
        public int couleur2 { get; set; }
        public int couleur3 { get; set; }
        public int Imunite { get; set; }
        public int align { get; set; }
        public int Scale { get; set; }

        public bool hasBuff(int idBuff)
        {
            return Buffs.Exists(x => x.EffectID == idBuff);
        }

        public int initiative { get; set; }
        public int PO { get; set; }
        public int ChanceEca { get; set; }
        public int BadChanceEca { get; set; }
        public int invo { get; set; }
        public int isDead { get; set; }
        public bool IsHuman { get; set; }
        public List<Buff.Buff> Buffs = new List<Buff.Buff>();
        public int id { get; set; }
        public int ReverseDmg { get; set; }
        public int Vie { get; set; }
        public int DamagePercent { get; set; }
        public int DamagePercentReceived { get; set; }
        public int P_DamagePiege { get; set; }
        public int F_DamagePiege { get; set; }
        public int BuffPM => Buffs.Where(x => x.EffectID == 128 || x.EffectID == 169).Select(x => x.jet).Sum(x => x);
        public int BuffPA => Buffs.Where(x => x.EffectID == 111 || x.EffectID == 168).Select(x => x.jet).Sum(x => x);
        public int P_RetraitPM { get; set; }
        public int P_RetraitPA { get; set; }
        public int P_EsquivePA { get; set; }
        public int P_EsquivePM { get; set; }

        public int CC { get; set; }
        public int TotalCC { get; set; }
        public int EC { get; set; }
        public int TotalEC { get; set; }
        public int HealBonus { get; set; }
        public int Damage { get; set; }
        public int DamageMagic { get; set; }
        public int DamagePhysic { get; set; }

        public int DamageForce { get; set; }
        public int DamageIntell { get; set; }
        public int DamageChance { get; set; }
        public int DamageAgi { get; set; }
        public int DamageNeutre { get; set; }
        public int DamagePousser { get; set; }
        public int ResPousser { get; set; }
        public int CaracIntell { get; set; }
        public int CaracForce { get; set; }
        public int CaracAgi { get; set; }
        public int CaracChance { get; set; }
        public int CaracSagesse { get; set; }
        public int CaracVie { get; set; }
        public int Force { get; set; }
        public int Sagesse { get; set; }
        public int Chance { get; set; }
        public int Agi { get; set; }
        public int Intell { get; set; }
        public int PA { get; set; }
        public int PM { get; set; }
        public int TotalVie { get; set; }
        public int TotalPA { get; set; }
        public int TotalPM { get; set; }
        public int TotalIntell { get; set; }
        public int TotalForce { get; set; }
        public int TotalSagesse { get; set; }
        public int TotalChance { get; set; }
        public int TotalAgi { get; set; }
        public int ResMagic { get; set; }
        public int ResPhysic { get; set; }
        public int P_EquipementResistanceForce { get; set; }
        public int P_EquipementResistanceIntell { get; set; }
        public int P_EquipementResistanceEau { get; set; }
        public int P_EquipementResistanceAgi { get; set; }
        public int P_EquipementResistanceNeutre { get; set; }
        public int P_EquipementNeutre { get; set; }
        public int P_TotalResForce { get; set; }
        public int P_TotalResIntell { get; set; }
        public int P_TotalResEau { get; set; }
        public int P_TotalResAgi { get; set; }
        public int P_TotalResNeutre { get; set; }
        public int P_BoostResForce { get; set; }
        public int P_BoostResIntell { get; set; }
        public int P_BoostlResEau { get; set; }
        public int P_BoostResAgi { get; set; }
        public int P_BoostResNeutre { get; set; }
        public int EquipementVie { get; set; }
        public int EquipementPA { get; set; }
        public int EquipementPM { get; set; }
        public int EquipementIntell { get; set; }
        public int EquipementForce { get; set; }
        public int EquipementSagesse { get; set; }
        public int EquipementChance { get; set; }
        public int EquipementAgi { get; set; }
        public int EquipementPO { get; set; }
        public int EquipementDommages { get; set; }
        public int EquipementDommagesPieges { get; set; }
        public int EquipementCoupsCritique { get; set; }
        public int EquipementInitiative { get; set; }
        public int F_EquipementResistanceForce { get; set; }
        public int F_EquipementResistanceIntell { get; set; }
        public int F_EquipementResistanceEau { get; set; }
        public int F_EquipementResistanceAgi { get; set; }
        public int F_EquipementResistanceNeutre { get; set; }
        public int F_TotalResForce { get; set; }
        public int F_TotalResIntell { get; set; }
        public int F_TotalResEau { get; set; }
        public int F_TotalResAgi { get; set; }
        public int F_TotalResNeutre { get; set; }
        public int F_BoostResForce { get; set; }
        public int F_BoostResIntell { get; set; }
        public int F_BoostlResEau { get; set; }
        public int F_BoostResAgi { get; set; }
        public int F_BoostResNeutre { get; set; }

        public int RetraitPA { get; set; }
        public int RetraitPM { get; set; }
        public int EsquivePA { get; set; }
        public int EsquivePM { get; set; }
        public bool IsInvocation { get; set; }

        //public override int GetHashCode()
        //{
        //    return initiative;
        //}
        //public override bool Equals(object obj)
        //{
        //    if (obj == null) return false;
        //    Entity objAsPart = obj as Entity;
        //    if (objAsPart == null) return false;
        //    else return Equals(objAsPart);
        //}

        //public bool Equals(Entity other)
        //{
        //    if (other == null) return false;
        //    return (this.initiative.Equals(other.initiative));
        //}

        //public int CompareTo(Entity comparePart)
        //{
        //    // A null value means that this object is greater.
        //    if (comparePart == null)
        //        return 1;

        //    else
        //        return this.initiative.CompareTo(comparePart.initiative);
        //}

        public Entity()
        {
            Scale = 100;
            IsHuman = false;
            IsInvocation = false;
        }

        public void UpdateStat()
        {
            PA = TotalPA + BuffPA;
            PM = TotalPM + BuffPM;
        }


    }
}
