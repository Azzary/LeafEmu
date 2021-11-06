using LeafEmu.World.Game.Quest;
using LeafEmu.World.PacketGestion;
using LeafEmu.World.World;
using System;
using System.Text;

namespace LeafEmu.World.Game.Entity.Npc
{
    public class NpcPacket
    {

        [PacketAttribute("DC")]
        public void SendDialogQuestion(Network.listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.State != EnumClientState.None && prmClient.account.character.State != EnumClientState.OnSwithMove)
                return;
            if (int.TryParse(prmPacket.Substring(2), out int id))
            {
                var npc = prmClient.account.character.Map.Npcs.Find(x => x.TempID == id);
                if (npc == null)
                    return;
                int idDialog = npc.Template.InitQuestion.Count == 1 ? npc.Template.InitQuestion[0] :
                    npc.Template.InitQuestion.ContainsKey(prmClient.account.character.mapID) ? npc.Template.InitQuestion[prmClient.account.character.mapID] : -1;
                if (!Database.table.Npc.InitQuestion.ContainsKey(idDialog)) 
                    return;
                var dialog = GetGoodDialog(prmClient, idDialog);
                if (dialog != null)
                {
                    prmClient.account.character.IdCurrentTalkingNpc = npc.Template.id;
                    prmClient.send("DCK" + id);
                    prmClient.send(GetPacketDialog(prmClient.account.character, dialog));
                }
                else
                {
                    ExitDialog(prmClient, string.Empty);
                }
            }
        }

        [PacketAttribute("DR")]
        public void DialogReponce(Network.listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.State == EnumClientState.OnDialog)
            {
                string[] data = prmPacket.Substring(2).Split('|');
                if (int.TryParse(data[0], out int dialogID) && int.TryParse(data[1], out int responceID))
                {
                    if (prmClient.account.character.DialogID == dialogID && Database.table.Npc.ReponceDialog.ContainsKey(responceID))
                        this.DoActionWithType(prmClient, responceID);
                }

            }
        }

        private Dialog GetGoodDialog(Network.listenClient prmClient, int idDialog)
        {
            if (!Database.table.Npc.InitQuestion.ContainsKey(idDialog))
                return null;

            retry:
            Dialog dialog = Database.table.Npc.InitQuestion[idDialog];
            prmClient.account.character.State = EnumClientState.OnDialog;
            if (Dialog.NpcTalkCondition(prmClient.account.character, dialog.cond))
            {
                prmClient.account.character.DialogID = idDialog;
                return dialog;
            }
            else if (dialog.ifFalse != 0)
            {
                idDialog = dialog.ifFalse;
                goto retry;
            }

            return null;
        }

        [PacketAttribute("DV")]
        public static void ExitDialog(Network.listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.State == EnumClientState.OnDialog)
            {
                prmClient.account.character.IdCurrentTalkingNpc = -1;
                prmClient.account.character.DialogID = -1;
                prmClient.account.character.State = EnumClientState.None;
                prmClient.send("DV");
            }
        }

        private static string GetPacketDialog(Character character, Dialog dialog)
        {
            string responses = GetOnlyReponceDispo(character, dialog);
            return "DQ" + dialog.ID + (responses != string.Empty ? "|" + responses : string.Empty);
        }

        private static string GetOnlyReponceDispo(Character character, Dialog dialog)
        {
            var reponce = new StringBuilder(dialog.responses);
            //"R=0:QE!|";
            if (dialog.cond.Contains("R="))
            {
                reponce.Clear();
                var allReponce = dialog.responses.Split(';');
                var allCond = dialog.cond.Split('|');
                for (int i = 0; i < allCond.Length; i++)
                {
                    if (Dialog.NpcTalkCondition(character, allCond[i].Split(':')[1]))
                    {
                        if (reponce.Length != 0)
                            reponce.Append(";");

                        reponce.Append(allReponce[i]);
                    }
                }
            }

            return reponce.ToString();
        }

        private void GetNewDialog(Network.listenClient prmClient, int id)
        {
            var rep = Database.table.Npc.ReponceDialog[id];
            if (rep.args != "DV"
            && int.TryParse(Database.table.Npc.ReponceDialog[id].args, out int newDialogID))
            {
                prmClient.account.character.State = EnumClientState.OnDialog;
                Dialog dialog = GetGoodDialog(prmClient, newDialogID);
                if (dialog != null && ResponceCondition(prmClient, id))
                {
                    prmClient.send(GetPacketDialog(prmClient.account.character, dialog));
                    return;
                }
            }

            ExitDialog(prmClient, string.Empty);
        }

        private bool ResponceCondition(Network.listenClient prmClient, int idResponce)
        {
            var character = prmClient.account.character;
            bool continuDialog = true;
            foreach (QuestPlayer questPlayer in character.questList.Values)
            {
                if (questPlayer.finish || questPlayer.quest == null
                || (questPlayer.quest.npc != null && questPlayer.quest.npc.id != character.IdCurrentTalkingNpc))
                    continue;
                foreach (QuestEtape questEtape in questPlayer.quest.questEtapeList)
                {
                    if (questPlayer.isQuestEtapeIsValidate(questEtape))
                        continue;
                    if (questEtape.validationType == idResponce)
                    {
                        continuDialog = false;
                        switch (questEtape.type)
                        {
                            case 3: // Si on doit donner des items
                                foreach (var item in questEtape.itemNecessary)
                                {
                                    if (!questPlayer.isQuestEtapeIsValidate(questEtape)
                                    && character.Inventaire.HasItemTemplate(item.Key, item.Value))
                                    {
                                        character.Inventaire.RemoveItem(prmClient, item.Key, item.Value, false);
                                        questPlayer.setQuestEtapeValidate(questEtape);
                                    }
                                }

                                break;
                        }
                    }
                    else
                    {
                        continuDialog = true;
                        continue;
                    }
                }

                if (questPlayer.questEtapeListValidate.Count == questPlayer.quest.questEtapeList.Count)
                {
                    questPlayer.finish = true;
                    continuDialog = true;
                }
            }

            return continuDialog;
        }

        private void DoActionWithType(Network.listenClient prmClient, int id)
        {
            var rep = Database.table.Npc.ReponceDialog[id];
            switch (rep.type)
            {
                case 1:
                    //GetNewDialog(prmClient, id);
                    break;
                case 2://tp
                    Map.Mouvement.MapMouvement.SwitchMap(prmClient, 7411, 295);
                    break;
                case 15://dj
                    Dungeon.Dungeon.DialogEnter(prmClient, id);
                    break;
                case 40:// quete
                    QuestManagement.GiveQuest(prmClient, Database.table.Npc.ReponceDialog[id].args);
                    break;
                case 43:// tp quete
                    QuestManagement.TpQuest(prmClient, Database.table.Npc.ReponceDialog[id].args);
                    break;
                case 229: //Quit Incarnam
                    short map = Constant.getClassStatueMap(prmClient.account.character.classe);
                    int cell = Constant.getClassStatueCell(prmClient.account.character.classe);
                    Map.Mouvement.MapMouvement.SwitchMap(prmClient, map, cell);
                    prmClient.account.character.SetNewSpawn(map, cell);
                    prmClient.GAME_SEND_Im_PACKET("06");
                    break;
                case 983:
                    FightBouftouQuestIncarnam(prmClient);
                    break;
            }
            GetNewDialog(prmClient, id);
        }

        private void FightBouftouQuestIncarnam(Network.listenClient prmClient)
        {
            var player = prmClient.account.character;
            var quest = Database.table.Quest.Quests[193];
            if (quest == null)
                return;
            Map.Map curMap = player.Map;
            if (curMap.Id != (short)10332)
                return;
            if (player.getQuestPersoByQuest(quest) == null)
                player.addQuestPerso(new QuestPlayer(Database.table.Quest.GetNewID(), quest.id, false, player, string.Empty, false));
            else if (quest.getQuestEtapeCurrent(player.getQuestPersoByQuest(quest)).quest.id != 793)
                return;
            var groupMob = new MobGroup(-100, new List<Mob>(), -1, string.Empty);
            groupMob.Mobs.Add(Mob.Copy(Database.table.Monster.Mobs[984][0], -9));
            Fight.GestionFight.LauchFight(prmClient, groupMob, Fight.FightTypeEnum.PvM);
        }
    }
}
