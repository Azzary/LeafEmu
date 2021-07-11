using System;
using System.Collections.Generic;
using System.Reflection;

namespace LeafEmu.World.Game.Entity
{
    public class Mob : Entity
    {
        public int Grade { get; set; }
        public EnumTypeAI TypeAI { get; set; }
        public int MinKamas { get; set; }
        public int MaxKamas { get; set; }
        public int IAType { get; set; }
        public SByte aggroDistance { get; set; }
        public bool IsCapturable { get; set; }
        public int XP_Value { get; set; }

        public AI.AI AI { get; set; }

        public static List<Mob> GetAllMobsGrade(int _id, string _name, int _gfxID, string _grades, string _stats, string _statsInfos, string _Spells, string _pdvs, string _points, string _init,
            int _minKamas, int _maxKamas, string _color, SByte _agroDistance, string _exps, int _IA_type, int _capturable, string name)
        {

            List<Mob> ListGradeMobs = new List<Mob>();
            string[] Split = _grades.Split("|");
            int nbBoucle = Split.Length - 1;

            for (int n = 0; n < nbBoucle; n++)
            {
                Mob Temp = new Mob();
                if (_IA_type == 2)
                {
                    Temp.TypeAI = EnumTypeAI.Fearful;
                    Temp.AI = new AI.FearfulAI(null);
                }
                else
                {
                    Temp.TypeAI = EnumTypeAI.Cac;
                    Temp.AI = new AI.CacAI(null);
                }
                Temp.speudo = name;
                Temp.Grade = n;
                Temp.id = _id;
                Temp.gfxID = _gfxID;
                Temp.align = 1;
                string[] colorado = _color.Split(',');
                for (int i = 0; i < 3; i++)
                {
                    int color = -1;
                    if (int.TryParse(colorado[i], out color))
                    { }
                    else
                        color = Convert.ToInt32(colorado[i], 16);// int.Parse(, System.Globalization.NumberStyles.HexNumber);
                    Buff.Buff.SetValue($"couleur{i + 1}", color, Temp, typeof(Mob));
                }

                Temp.MinKamas = _minKamas;
                Temp.MaxKamas = _maxKamas;
                Temp.IAType = _IA_type;
                Temp.IsCapturable = Convert.ToBoolean(_capturable);
                Temp.aggroDistance = _agroDistance;
                //Grades

                string grade = Split[n];
                string[] infos = grade.Split("@");
                Temp.level = Convert.ToInt16(infos[0]);

                string[] resist = infos.Length == 1 ? new string[7] { "0", "0", "0", "0", "0", "0", "0" } : infos[1].Split('|')[0].Split(';');

                string[] temps = _stats.Split("|");
                string[] stat = n > temps.Length - 1 ? temps[temps.Length - 1].Split(',') : temps[n].Split(',');

                temps = _statsInfos.Split("|");
                string[] statInfos = n > temps.Length - 1 ? _statsInfos.Split("|")[temps.Length - 1].Split(';') : _statsInfos.Split("|")[n].Split(';');



                if (stat[0].Equals(string.Empty))
                {
                    Logger.Logger.Log($"Mob {_id} don't have stats for grade {n}", 15);
                    continue;
                }

                Temp.P_TotalResNeutre = Convert.ToInt32(resist[0]);
                Temp.P_TotalResForce = Convert.ToInt32(resist[1]);
                Temp.P_TotalResIntell = Convert.ToInt32(resist[2]);
                Temp.P_TotalResEau = Convert.ToInt32(resist[3]);
                Temp.P_TotalResAgi = Convert.ToInt32(resist[4]);
                Temp.ResMagic = Convert.ToInt32(resist[5]);
                Temp.ResPhysic = Convert.ToInt32(resist[6]);

                Temp.CaracForce = Convert.ToInt32(stat[0]);
                Temp.CaracSagesse = Convert.ToInt32(stat[1]);
                Temp.CaracIntell = Convert.ToInt32(stat[2]);
                Temp.CaracChance = Convert.ToInt32(stat[3]);
                Temp.CaracAgi = Convert.ToInt32(stat[4].Split('|')[0]);
                Temp.Damage = Convert.ToInt32(statInfos[0]);
                Temp.HealBonus = Convert.ToInt32(statInfos[2]);
                Temp.invo = Convert.ToInt32(statInfos[3]);


                //Spells
                string allSpells = "";
                if (!_Spells.Equals("||||", StringComparison.InvariantCultureIgnoreCase)
                        && !_Spells.Equals("", StringComparison.InvariantCultureIgnoreCase)
                        && !_Spells.Equals("-1", StringComparison.InvariantCultureIgnoreCase))
                {
                    temps = _Spells.Split("|");
                    allSpells = n > temps.Length - 1 ? _Spells.Split("|")[temps.Length - 1] : allSpells = _Spells.Split("|")[n];

                    if (allSpells.Equals("-1"))
                        allSpells = "";
                }

                if (!allSpells.Equals(string.Empty))
                {
                    String[] spells = allSpells.Split(";");

                    foreach (string str in spells)
                    {
                        if (str.Equals("")) continue;
                        String[] spellInfo = str.Split("@");
                        int id, lvl;
                        id = Convert.ToInt32(spellInfo[0]);
                        lvl = Convert.ToInt32(spellInfo[1]);
                        if (id == 0 || lvl == 0 || !Database.table.Spells.SpellsList.ContainsKey(id)) continue;
                        Temp.Spells.Add(new Spells.SpellsEntity(id, lvl));
                    }
                }
                temps = _pdvs.Split("|");
                Temp.TotalVie = n > temps.Length - 1 ? Convert.ToInt32(temps[temps.Length - 1]) : Convert.ToInt32(temps[n]);
                Temp.Vie = Temp.TotalVie;
                temps = _init.Split("|");
                Temp.initiative = n > temps.Length - 1 ? Convert.ToInt32(temps[temps.Length - 1]) : Convert.ToInt32(temps[n]);
                temps = _points.Split("|");
                string[] pts = n > temps.Length - 1 ? _points.Split("|")[temps.Length - 1].Split(";") : _points.Split("|")[n].Split(";");
                Temp.PA = Convert.ToInt32(pts[0]);
                Temp.PM = Convert.ToInt32(pts[1]);
                Temp.TotalPA = Temp.PA;
                Temp.TotalPM = Temp.PM;

                temps = _exps.Split("|");
                Temp.XP_Value = n > temps.Length - 1 ? Convert.ToInt32(_exps.Split("|")[temps.Length - 1]) : Convert.ToInt32(_exps.Split("|")[n]);

                ListGradeMobs.Add(Temp);


            }
            return ListGradeMobs;
        }


        public static Mob Copy(Mob actual, int ID_InFight)
        {
            Mob Copy = new Mob();
            foreach (var property in actual.GetType().GetProperties())
            {
                PropertyInfo propertyS = Copy.GetType().GetProperty(property.Name);
                propertyS.SetValue(Copy, property.GetValue(actual));
            }
            Copy.ID_InFight = ID_InFight;
            Copy.Spells = actual.Spells;
            return Copy;
        }

        public int GetKamasWin()
        {
            return Util.rng.Next(MinKamas, MaxKamas + 1);
        }
    }
}
