using LeafEmu.World.PacketGestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafEmu.World.Game.Quest
{
    public class QuestManagement
    {

        public static void GiveQuest(Network.listenClient prmClient, string args)
        {
            int QuestID = int.Parse(args);
            bool problem = false;
            Quest quest0 = Quest.questDataList[QuestID];
            if (quest0 == null)
            {
                prmClient.SendMessageToPlayer("La quête est introuvable.");
                problem = true;
                return;
            }
            foreach (QuestPlayer qPerso in prmClient.account.character.questList.Values)
            {
                if (qPerso.quest.id == QuestID)
                {
                    prmClient.SendMessageToPlayer("Vous connaissez déjà cette quête.");
                    problem = true;
                    break;
                }
            }

            if (!problem)
                quest0.applyQuest(prmClient);
        }

        public static void TpQuest(Network.listenClient prmClient, string args)
        {
            var character = prmClient.account.character;
            String[] split = args.Split(";");
            int mapid = int.Parse(split[0].Split(",")[0]);
            int cellid = int.Parse(split[0].Split(",")[1]);
            int mapsecu = int.Parse(split[1]);
            int questId = int.Parse(split[2]);
            QuestPlayer quest = character.getQuestPersoByQuestId(questId);
            if (character.Map.Id != mapsecu || quest == null || !quest.finish)
                return;

            Map.Mouvement.MapMouvement.SwitchMap(prmClient, mapid, cellid);
        }

        [PacketAttribute("QL")]
        public void getQuestGmPacket(Network.listenClient prmClient, string prmPacket)
        {
            StringBuilder packet = new StringBuilder();
            bool flag = false;
            packet.Append("QL+");
            foreach (var qPerso in prmClient.account.character.questList.Values)
            {
                if (flag)
                    packet.Append("|");
                packet.Append(qPerso.quest.id).Append(";");
                packet.Append(qPerso.finish ? 1 : 0);
                flag = true;
            }
            prmClient.send(packet.ToString());
        }

        [PacketAttribute("QS")]
        public void getGmQuestDataPacket(Network.listenClient prmClient, string prmPacket)
        {
            if (Int32.TryParse(prmPacket.Substring(2), out int questID))
            {
                var quest = prmClient.account.character.questList.First(x => x.Value.quest.id == questID);
                if (quest.Value != null)
                    prmClient.send("QS" + quest.Value.quest.getGmQuestDataPacket(prmClient.account.character, quest.Key));
            }
           
        }
    }
}
