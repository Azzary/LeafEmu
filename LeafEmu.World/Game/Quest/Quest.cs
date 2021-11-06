using LeafEmu.World.Game.Entity.Npc;
using LeafEmu.World.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafEmu.World.Game.Quest
{
    public class Quest
    {

        /* Static List */
        public static Dictionary<int, Quest> questDataList => Database.table.Quest.Quests;

        public int id;
        public List<QuestEtape> questEtapeList = new List<QuestEtape>();
        public List<QuestObjectif> questObjectifList = new List<QuestObjectif>();
        public NpcTemplate npc = null;
        public List<QuestAction> actions = new List<QuestAction>();
        public bool delete;
        public int[] condition = null;

        public Quest(int aId, String questEtape, String aObjectif, int aNpc,
                     String action, String args, bool delete, String condition)
        {
            this.id = aId;
            this.delete = delete;
            if (!questEtape.Equals(string.Empty))
            {
                String[] split = questEtape.Split(";");

                if (split != null && split.Length > 0)
                {
                    foreach (string qEtape in split)
                    {
                        QuestEtape q_Etape = QuestEtape.getQuestEtapeById(int.Parse(qEtape));
                        q_Etape.quest = this;
                        questEtapeList.Add(q_Etape);
                    }
                }
            }
            if (!aObjectif.Equals(string.Empty))
            {
                String[] Split = aObjectif.Split(";");

                if (Split != null && Split.Length > 0)
                {
                    foreach (String qObjectif in Split)
                    {
                        questObjectifList.Add(QuestObjectif.questObjectifList[int.Parse(qObjectif)]);
                    }
                }
            }
            if (!condition.Equals(string.Empty))
            {
                String[] Split = condition.Split(":");
                if (Split != null && Split.Length > 0)
                {
                    this.condition = new int[2] { int.Parse(Split[0]), int.Parse(Split[1]) };
                }

            }
            if (Database.table.Npc.NpcTemplate.ContainsKey(aNpc))
            {
                this.npc = Database.table.Npc.NpcTemplate[aNpc];
                npc.Quest = this;
            }
                

           
                if (!action.Equals(string.Empty) && !args.Equals(string.Empty))
                {
                    string[] arguments = args.Split(";");
                    int nbr = 0;
                    foreach (string loc0 in action.Split(","))
                    {
                        int actionId = int.Parse(loc0);
                        string arg = arguments[nbr];
                        actions.Add(new QuestAction(actionId, arg, -1 + string.Empty, null));
                        nbr++;
                    }
                }
            //Logger.Logger.Warning("Erreur avec l action et les args de la quete " + this.id + ".");
        }

        public bool haveRespectCondition(QuestPlayer qPerso, QuestEtape qEtape)
        {
            switch (qEtape.condition)
            {
                case "1": //Valider les etapes d'avant
                    bool loc2 = true;
                    foreach (QuestEtape aEtape in questEtapeList)
                    {
                        if (aEtape == null)
                            continue;
                        if (aEtape.id == qEtape.id)
                            continue;
                        if (!qPerso.isQuestEtapeIsValidate(aEtape))
                            loc2 = false;
                    }
                    return loc2;

                case "0":
                    return true;
            }
            return false;
        }

        public String getGmQuestDataPacket(Entity.Character perso, int _id)
        {
            QuestPlayer qPerso = perso.questList[_id];
            int loc1 = getObjectifCurrent(qPerso);
            int loc2 = getObjectifPrevious(qPerso);
            int loc3 = getNextObjectif(QuestObjectif.questObjectifList[getObjectifCurrent(qPerso)]);
            StringBuilder str = new StringBuilder();
            str.Append(id).Append("|");
            str.Append(loc1 > 0 ? loc1 : string.Empty);
            str.Append("|");

            StringBuilder str_prev = new StringBuilder();
            bool loc4 = true;
            // Il y a une exeption dans le code ici pour la seconde �tape de papotage
            foreach (QuestEtape qEtape in questEtapeList)
            {
                if (qEtape.objectif != loc1)
                    continue;
                if (!haveRespectCondition(qPerso, qEtape))
                    continue;
                /*
                 * if(!loc4 && (getNextObjectif(QuestObjectif.getQuestObjectifById(
                 * getObjectifCurrent(qPerso))) == qEtape.getObjectif() ||
                 * questObjectifList.Count < 2)) { if(qEtape.getType() == 10 &&
                 * qEtape.id == getQuestEtapeCurrent(qPerso).id && !loc4)
                 * str_prev.Append(";"); else if(qEtape.getType() != 10)
                 * str_prev.Append(";"); } if(qEtape.getType() == 10 &&
                 * getQuestEtapeCurrent(qPerso).id == qEtape.id) //On
                 * efface toute les questEtape pass� avant str_prev.delete(0,
                 * str_prev.Length());
                 * str_prev.Append(qEtape.id).Append(",").Append
                 * (qPerso.isQuestEtapeIsValidate(qEtape) ? 1 : 0); if(loc4) loc4 =
                 * false;
                 */
                if (!loc4)
                    str_prev.Append(";");
                str_prev.Append(qEtape.id);
                str_prev.Append(",");
                str_prev.Append(qPerso.isQuestEtapeIsValidate(qEtape) ? 1 : 0);
                loc4 = false;
            }
            str.Append(str_prev);
            str.Append("|");
            str.Append(loc2 > 0 ? loc2 : string.Empty).Append("|");
            str.Append(loc3 > 0 ? loc3 : string.Empty);
            if (npc != null)
            {
                str.Append("|");
                if (qPerso.quest.npc != null && Database.table.Npc.InitQuestion.ContainsKey(qPerso.quest.npc.id))
                    str.Append(Database.table.Npc.InitQuestion[qPerso.quest.npc.id].ID);
                str.Append("|");
            }
            return str.ToString();
        }

        public QuestEtape getQuestEtapeCurrent(QuestPlayer qPerso)
        {
            foreach (QuestEtape qEtape in questEtapeList)
            {
                if (!qPerso.isQuestEtapeIsValidate(qEtape))
                    return qEtape;
            }
            return null;
        }

        public int getObjectifCurrent(QuestPlayer qPerso)
        {
            foreach (QuestEtape qEtape in questEtapeList)
            {
                if (qPerso.isQuestEtapeIsValidate(qEtape))
                    continue;
                return qEtape.objectif;
            }
            return 0;
        }

        public int getObjectifPrevious(QuestPlayer qPerso)
        {
            if (questObjectifList.Count == 1)
                return 0;
            else
            {
                int previousqObjectif = 0;
                foreach (QuestObjectif qObjectif in questObjectifList)
                {
                    if (qObjectif.id == getObjectifCurrent(qPerso))
                        return previousqObjectif;
                    else
                        previousqObjectif = qObjectif.id;
                }
            }
            return 0;
        }

        public int getNextObjectif(QuestObjectif qO)
        {
            if (qO == null)
                return 0;
            foreach (QuestObjectif qObjectif in questObjectifList)
            {
                if (qObjectif.id == qO.id)
                {
                    int index = questObjectifList.IndexOf(qObjectif);
                    if (questObjectifList.Count <= index + 1)
                        return 0;
                    return questObjectifList[(index + 1)].id;
                }
            }
            return 0;
        }

        public void applyQuest(Network.listenClient prmClient)
        {
            var perso = prmClient.account.character;
            if (this.condition != null)
            {
                switch (this.condition[0])
                {
                    case 1: // Niveau
                        if (perso.level < this.condition[1])
                        {
                            prmClient.SendMessageToPlayer("Votre niveau est insuffisant pour apprendre la quête.");
                            return;
                        }
                        break;
                }
            }
            QuestPlayer qPerso = new QuestPlayer(Database.table.Quest.GetNewID(), id, false, perso, string.Empty, false);
            perso.addQuestPerso(qPerso);
            prmClient.GAME_SEND_Im_PACKET("054;" + id);
            Map.MapGestion.ShowNpcsOnMap(prmClient);
            if (actions.Count != 0)
                foreach (QuestAction aAction in actions)
                    aAction.apply(prmClient, perso, -1, -1);
        }

        public void updateQuestData(Network.listenClient prmClient, bool validation, int type)
        {
            var perso = prmClient.account.character;
            QuestPlayer qPerso = perso.getQuestPersoByQuest(this);
            foreach (QuestEtape qEtape in questEtapeList)
            {
                if (qEtape.validationType != type)
                    continue;

                bool refresh = false;
                if (qPerso.isQuestEtapeIsValidate(qEtape)) //On a d�j� valid� la questEtape on passe
                    continue;

                if (qEtape.objectif != getObjectifCurrent(qPerso))
                    continue;

                if (!haveRespectCondition(qPerso, qEtape))
                    continue;

                if (validation)
                    refresh = true;
                switch (qEtape.type)
                {

                    case 3://Donner item
                        if (perso.ExchangeEntity != null && perso.State == EnumClientState.OnExchangePnj
                            && perso.ExchangeEntity.id == qEtape.npc.id)
                        {
                            foreach (var entry in qEtape.itemNecessary)
                            {
                                if (perso.Inventaire.HasItemTemplate(entry.Key, entry.Value))
                                { //Il a l'item et la quantit�
                                    perso.Inventaire.RemoveItem(prmClient, entry.Key, entry.Value); //On supprime donc
                                    refresh = true;
                                }
                            }
                        }
                        break;

                    case 0:
                    case 1://Aller voir %
                    case 9://Retourner voir %
                        if (qEtape.condition.Equals("1"))
                        { //Valider les questEtape avant
                            if (perso.ExchangeEntity != null && perso.State == EnumClientState.OnDialog 
                                && perso.ExchangeEntity.id == qEtape.npc.id)
                            {
                                if (haveRespectCondition(qPerso, qEtape))
                                {
                                    refresh = true;
                                }
                            }
                        }
                        else
                        {
                            if (perso.ExchangeEntity != null && perso.State == EnumClientState.OnDialog 
                                && perso.ExchangeEntity.id == qEtape.npc.id)
                                refresh = true;
                        }
                        break;

                    case 6: // monstres
                        foreach (var entry in qPerso.monsterKill)
                            if (entry.Key == qEtape.monsterId && entry.Value >= qEtape.qua)
                                refresh = true;
                        break;

                    case 10://Ramener prisonnier
                        if (perso.ExchangeEntity != null && perso.State == EnumClientState.OnDialog 
                            && perso.ExchangeEntity.id == qEtape.npc.id)
                        {
                            var follower = perso.Inventaire.getItemByPos(Constant.ITEM_POS_PNJ_SUIVEUR);
                            if (follower != null)
                            {
                                foreach (var entry2 in qEtape.itemNecessary)
                                {
                                    if (entry2.Key == follower.Template.ID)
                                    {
                                        refresh = true;
                                        //perso.setMascotte(0);
                                    }
                                }
                            }
                        }
                        break;
                }

                if (refresh)
                {
                    QuestObjectif ansObjectif = QuestObjectif.questObjectifList[getObjectifCurrent(qPerso)];
                    qPerso.setQuestEtapeValidate(qEtape);
                    prmClient.GAME_SEND_Im_PACKET("055;" + id);
                    if (haveFinish(qPerso, ansObjectif))
                    {
                        prmClient.GAME_SEND_Im_PACKET("056;" + id);
                        applyButinOfQuest(prmClient, qPerso, ansObjectif);
                        qPerso.setFinish(true);
                    }
                    else
                    {
                        if (getNextObjectif(ansObjectif) != 0)
                        {
                            if (qPerso.overQuestEtape(ansObjectif))
                                applyButinOfQuest(prmClient, qPerso, ansObjectif);
                        }
                    }
                }
            }
        }

        public bool haveFinish(QuestPlayer qPerso, QuestObjectif qO)
        {
            return qPerso.overQuestEtape(qO) && getNextObjectif(qO) == 0;
        }

        public void applyButinOfQuest(Network.listenClient prmClient, QuestPlayer qPerso,
                                      QuestObjectif ansObjectif)
        {
            var perso = prmClient.account.character;
            long aXp = 0;

            if ((aXp = ansObjectif.xp) > 0)
            { //Xp a donner
                perso.AddXp(aXp, prmClient);

            }
            if (ansObjectif.items.Count > 0)
            { //Item a donner
                foreach (var entry in ansObjectif.items)
                {
                    perso.Inventaire.GenerateItemInInv(prmClient, entry.Key, entry.Value);
                }
            }

            int aKamas = 0;
            if ((aKamas = ansObjectif.kamas) > 0)
            { //Kams a donner
                perso.SetKamas(prmClient, perso.kamas + (long)aKamas);
            }

            if (getNextObjectif(ansObjectif) != ansObjectif.id)
            { //On passe au nouveau objectif on applique les actions
                foreach (QuestAction a in ansObjectif.actionList)
                {
                    a.apply(prmClient, null, 0, 0);
                }
            }

        }

        public int getQuestEtapeByObjectif(QuestObjectif qObjectif)
        {
            int nbr = 0;
            foreach (QuestEtape qEtape in questEtapeList)
            {
                if (qEtape.objectif == qObjectif.id)
                    nbr++;
            }
            return nbr;
        }


    }
}
