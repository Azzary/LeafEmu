using System;
using System.Collections.Generic;

namespace LeafEmu.World.Game.Spells
{
    public class SpellsStats
    {
        public int SpellsID;
        public int level;
        public int PACost;
        public int minPO;
        public int maxPO;
        public int TauxCC;
        public int TauxEC;
        public bool isLineLaunch;
        public bool hasLDV;
        public bool isEmptyCell;
        public bool isModifPO;
        public int maxLaunchbyTurn;
        public int maxLaunchbyByTarget;
        public int coolDown;
        public int reqLevel;
        public bool isEcEndTurn;
        public List<SpellsEffect.Spells> effects;
        public List<SpellsEffect.Spells> CCeffects;
        public string porteeType;

        public SpellsStats(int ASpellsID, int Alevel, int cost, int minPO,
                         int maxPO, int tauxCC, int tauxEC, bool isLineLaunch,
                         bool hasLDV, bool isEmptyCell, bool isModifPO,
                         int maxLaunchbyTurn, int maxLaunchbyByTarget, int coolDown,
                         int reqLevel, bool isEcEndTurn, String effects,
                         String ceffects, String typePortee)
        {
            //effets, effetsCC, PaCost, PO Min, PO Max, Taux CC, Taux EC, line, LDV, emptyCell, PO Modif, maxByTurn, maxByTarget, Cooldown, type, level, endTurn
            this.SpellsID = ASpellsID;
            this.level = Alevel;
            this.PACost = cost;
            this.minPO = minPO;
            this.maxPO = maxPO;
            this.TauxCC = tauxCC;
            this.TauxEC = tauxEC;
            this.isLineLaunch = isLineLaunch;
            this.hasLDV = hasLDV;
            this.isEmptyCell = isEmptyCell;
            this.isModifPO = isModifPO;
            this.maxLaunchbyTurn = maxLaunchbyTurn;
            this.maxLaunchbyByTarget = maxLaunchbyByTarget;
            this.coolDown = coolDown;
            this.reqLevel = reqLevel;
            this.isEcEndTurn = isEcEndTurn;
            this.effects = parseEffect(effects);
            this.CCeffects = parseEffect(ceffects);
            this.porteeType = typePortee;
        }

        public int GetmaxPO(int poEntity)
        {
            return isModifPO ? maxPO : maxPO + poEntity;
        }
        private List<SpellsEffect.Spells> parseEffect(string e)
        {
            List<SpellsEffect.Spells> effets = new List<SpellsEffect.Spells>();
            string[] splt = e.Split("|");
            foreach (string a in splt)
            {
                //try
                //{
                if (e.Equals("-1") || a.Equals(string.Empty))
                    continue;
                int id = Convert.ToInt32(a.Split(";", 2)[0]);
                string args = a.Split(";", 2)[1];
                effets.Add(new SpellsEffect.Spells(id, args, SpellsID, level));
                //}
                //catch (Exception f)
                //{
                //     Logger.Logger.Log("parseEffect Spells");
                //}
            }
            return effets;
        }

    }
}
