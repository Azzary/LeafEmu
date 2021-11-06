using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafEmu.World.Game.Quest
{
    public class QuestPlayer
    {
        public int id { get; set; }
        public Quest quest = null;
        public bool finish;
        public bool isAddInDataBase;
        public Entity.Character character;
        public Dictionary<int, QuestEtape> questEtapeListValidate = new Dictionary<int, QuestEtape>();
        public Dictionary<int, short> monsterKill = new Dictionary<int, short>();

        public QuestPlayer(int aId, int qId, bool aFinish, Entity.Character character,
                           String qEtapeV, bool isAddInDataBase)
        {
            this.isAddInDataBase = isAddInDataBase;
            this.id = aId;
            this.quest = Quest.questDataList[qId];
            this.finish = aFinish;
            this.character = character;//Network.WorldServer.getPlayer(pId).account.character;
            try
            {
                String[] split = qEtapeV.Split(";");
                if (split != null && split.Length > 0)
                {
                    foreach (string loc1 in split)
                    {
                        if (loc1.Equals(string.Empty))
                            continue;
                        QuestEtape qEtape = QuestEtape.getQuestEtapeById(int.Parse(loc1));
                        questEtapeListValidate.Add(qEtape.id, qEtape);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Logger.Warning(e.ToString());
            }
        }

        public void setFinish(bool finish)
        {
            this.finish = finish;
        }

        public bool isQuestEtapeIsValidate(QuestEtape qEtape)
        {
            return qEtape != null && questEtapeListValidate.ContainsKey(qEtape.id);
        }

        public void setQuestEtapeValidate(QuestEtape qEtape)
        {
            if (!questEtapeListValidate.ContainsKey(qEtape.id))
                questEtapeListValidate.Add(qEtape.id, qEtape);
        }

        public string getQuestEtapeString()
        {
            StringBuilder str = new StringBuilder();
            int nb = 0;
            foreach (QuestEtape qEtape in questEtapeListValidate.Values)
            {
                nb++;
                str.Append(qEtape.id);
                if (nb < questEtapeListValidate.Count)
                    str.Append(";");
            }
            return str.ToString();
        }

        public bool overQuestEtape(QuestObjectif qObjectif)
        {
            int nbrQuest = 0;
            foreach (QuestEtape qEtape in questEtapeListValidate.Values)
            {
                if (qEtape.objectif == qObjectif.id)
                    nbrQuest++;
            }
            return qObjectif.getSizeUnique() == nbrQuest;
        }
    }
}
