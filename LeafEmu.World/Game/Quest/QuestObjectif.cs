using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafEmu.World.Game.Quest
{
    public class QuestObjectif
    {
        public static Dictionary<int, QuestObjectif> questObjectifList => Database.table.Quest.QuestsObjectifs;
        public int id;
        /* Butin */
        public int xp;
        public int kamas;
        public Dictionary<int, int> items = new Dictionary<int, int>();
        public List<QuestAction> actionList = new List<QuestAction>();
        public List<QuestEtape> questEtape = new List<QuestEtape>();

        public QuestObjectif(int aId, int aXp, int aKamas, String aItems,
                              String aAction)
        {
            this.id = aId;
            this.xp = aXp;
            this.kamas = aKamas;
            if (!aItems.Equals(string.Empty))
            {
                String[] split = aItems.Split(";");
                if (split != null && split.Length > 0)
                {
                    foreach (String loc1 in split)
                    {
                        if (loc1.Equals(string.Empty))
                            continue;
                        if (loc1.Contains(","))
                        {
                            String[] loc2 = loc1.Split(",");
                            this.items.Add(int.Parse(loc2[0]), int.Parse(loc2[1]));
                        }
                        else
                        {
                            this.items.Add(int.Parse(loc1), 1);
                        }
                    }
                }
            }
            if (aAction != null && !aAction.Equals(string.Empty))
            {
                String[] split = aAction.Split(";");
                if (split != null & split.Length > 0)
                {
                    foreach (string loc1 in split.Where(x => x != string.Empty))
                    {
                        string[] loc2 = loc1.Split("|");
                        int actionId = int.Parse(loc2[0]);
                        string args = loc2[1];
                        QuestAction action = new QuestAction(actionId, args, "-1", null);
                        actionList.Add(action);
                    }
                }
            }

        }


        public static Dictionary<int, QuestObjectif> getQuestObjectifList()
        {
            return questObjectifList;
        }

        public static void setQuest_Objectif(QuestObjectif qObjectif)
        {
            if (!questObjectifList.ContainsKey(qObjectif.id)
                    && !questObjectifList.ContainsValue(qObjectif))
                questObjectifList.Add(qObjectif.id, qObjectif);
        }

        public int getSizeUnique()
        {
            int cpt = 0;
            List<int> id = new List<int>();
            foreach (QuestEtape qe in questEtape)
            {
                if (!id.Contains(qe.id))
                {
                    id.Add(qe.id);
                    cpt++;
                }
            }
            return cpt;
        }

        public void setEtape(QuestEtape qEtape)
        {
            if (!questEtape.Contains(qEtape))
                questEtape.Add(qEtape);
        }
    }
}
