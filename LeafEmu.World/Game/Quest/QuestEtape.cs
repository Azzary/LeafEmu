using LeafEmu.World.Game.Entity.Npc;


namespace LeafEmu.World.Game.Quest
{
    public class QuestEtape
    {

        /* Static function */
        public static Dictionary<int, QuestEtape> questEtapeList => Database.table.Quest.QuestsEtapes;

        public int id;
        public short type;
        public int objectif;
        public Quest quest = null;
        public Dictionary<int, int> itemNecessary = new Dictionary<int, int>(); //ItemId,Qua
        public NpcTemplate npc = null;
        public int monsterId;
        public short qua;
        public string condition = null;
        public int validationType;

        public QuestEtape(int aId, int aType, int aObjectif, string itemN,
                           int aNpc, string aMonster, string aCondition, int validationType)
        {
            this.id = aId;
            this.type = (short)aType;
            this.objectif = aObjectif;
            if (!itemN.Equals(string.Empty))
            {
                string[] split = itemN.Split(";");
                if (split != null && split.Length > 0)
                {
                    foreach (string infos in split)
                    {
                        String[] loc1 = infos.Split(",");
                        this.itemNecessary.Add(int.Parse(loc1[0]), int.Parse(loc1[1]));
                    }
                }
            }
            if (Database.table.Npc.NpcTemplate.ContainsKey(aNpc))
                npc = Database.table.Npc.NpcTemplate[aNpc];
                
            if (aMonster.Contains(",") && !aMonster.Equals(0))
            {
                String[] loc0 = aMonster.Split(",");
                monsterId = int.Parse(loc0[0]);
                qua = short.Parse(loc0[1]); // Des qu�tes avec le truc vide ! ><
            }
            this.validationType = validationType;
            this.condition = aCondition;
            
            if (QuestObjectif.questObjectifList.ContainsKey(this.objectif))
                QuestObjectif.questObjectifList[this.objectif].setEtape(this);
            //else
            //    Logger.Logger.Warning("No quest Objectif for : "+ id);
        }

        public static QuestEtape getQuestEtapeById(int id)
        {
            return questEtapeList[id];
        }
    }
}
