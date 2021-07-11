using System;

namespace LeafEmu.World.Game.Spells.SpellsEffect
{
    public class Spells
    {
        public int durationFixed;
        public int effectID;
        public int turns = 0;
        string jet = "1d0+0";
        public int JetMin = 0;
        public int JetMax = 0;
        public int chance = 100;
        public string args;
        public short ChanceOfLineCast;
        public int value = 0;
        public Network.listenClient caster = null;
        public int SpellsID = 0;
        public int SpellsLvl = 1;
        public bool debuffable = true;
        public int duration = 0;
        public Map.Cell.Cell cell = null;

        public Spells(int turn)
        {
            SpellsID = 666;
            effectID = 666;
            this.turns = turn;
        }

        public Spells(int aID, string aArgs, int aSpells, int aSpellsLevel)
        {
            effectID = aID;
            args = aArgs;
            SpellsID = aSpells;
            SpellsLvl = aSpellsLevel;
            durationFixed = 0;

            string[] temArd = args.Split(";");
            value = Convert.ToInt32(temArd[0]);
            turns = Convert.ToInt32(temArd[3]);

            jet = temArd.Length > 5 ? temArd[5] : "";
            if (jet.Length < 4 || !jet.Contains('+'))
            {
                return;
            }
            ChanceOfLineCast = Convert.ToInt16(temArd[4]);
            int jetToAdd = Convert.ToInt32(jet.Split('+')[1].Split(',')[0].Split('|')[0].Split(';')[0]);
            JetMin = Convert.ToInt32(temArd[0]);
            JetMax = Math.Max(Convert.ToInt32(temArd[1]), Convert.ToInt32(jet.Split('d')[1].Split('+')[0]) + jetToAdd);

        }

        public Spells(int id, int value2, int aduration, int turns2, bool debuff, Network.listenClient aCaster, String args2, int aSpells)
        {
            effectID = id;
            value = value2;
            turns = turns2;
            debuffable = debuff;
            caster = aCaster;
            duration = aduration;
            this.durationFixed = duration;
            args = args2;
            SpellsID = aSpells;
            try
            {
                jet = args.Split(";")[5];
            }
            catch (Exception)
            {
            }
        }

        //public static Spells_Effect
    }
}
