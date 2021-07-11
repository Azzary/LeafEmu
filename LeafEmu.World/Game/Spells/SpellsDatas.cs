using System;

namespace LeafEmu.World.Game.Spells
{
    public class SpellsDatas
    {
        public int id { get; set; }
        public SpellsEnumType TypeOfSpell;
        public SpellsStats[] SpellsStats;

        public SpellsDatas(int _id, string _level1, string _level2, string _level3, string _level4, string _level5, string _level6, int type)
        {

            TypeOfSpell = type == 0 ? SpellsEnumType.attaque : SpellsEnumType.buff;

            id = _id;
            SpellsStats = new SpellsStats[6] {
                                            parseSortStats(id, 1, _level1),
                                            parseSortStats(id, 2, _level2),
                                            parseSortStats(id, 3, _level3),
                                            parseSortStats(id, 4, _level4),
                                            parseSortStats(id, 5, _level5),
                                            parseSortStats(id, 6, _level6),
                                            };

        }

        private SpellsStats parseSortStats(int id, int lvl, String str)
        {
            if (str == "-1") return null;

            //try
            //{
            string[] stat = str.Split(",");
            string effets = stat[0], CCeffets = stat[1];
            int PACOST = 6;

            try
            {
                PACOST = Convert.ToInt32(stat[2].Trim());
            }
            catch (Exception) { }

            int POm = Convert.ToInt32(stat[3].Trim());
            int POM = Convert.ToInt32(stat[4].Trim());
            int TCC = Convert.ToInt32(stat[5].Trim());
            int TEC = Convert.ToInt32(stat[6].Trim());

            bool line = stat[7].Trim().Equals("true");
            bool LDV = stat[8].Trim().Equals("true");
            bool emptyCell = stat[9].Trim().Equals("true");
            bool MODPO = stat[10].Trim().Equals("true");

            int MaxByTurn = Convert.ToInt32(stat[12].Trim());
            int MaxByTarget = Convert.ToInt32(stat[13].Trim());
            int CoolDown = Convert.ToInt32(stat[14].Trim());

            string type = stat[15].Trim();

            int level = Convert.ToInt32(stat[stat.Length - 2].Trim());
            bool endTurn = stat[19].Trim().Equals("true");

            return new SpellsStats(id, lvl, PACOST, POm, POM, TCC, TEC, line, LDV, emptyCell, MODPO, MaxByTurn, MaxByTarget, CoolDown, level, endTurn, effets, CCeffets, type);
            //}
            //catch (Exception e)
            //{
            //     Logger.Logger.Log("             Error Spells Can't be add: \n" + e);
            //    return null;
            //}
        }
    }
}
