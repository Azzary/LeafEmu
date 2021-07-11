using LeafEmu.World.PacketGestion;
using System;

namespace LeafEmu.World.Game.Entity.Npc
{
    public class NpcPacket
    {

        [PacketAttribute("DC")]
        public void SendDialogQuestion(Network.listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.State != EnumClientState.None)
                return;

            if (int.TryParse(prmPacket.Substring(2), out int id))// Database.table.Npc.InitQuestion.ContainsKey(id))
            {

                var npc = prmClient.account.character.Map.Npcs.Find(x => x.TempID == id);
                if (npc == null)
                    return;
                int idDialog = npc.Template.InitQuestion.Count == 1 ? npc.Template.InitQuestion[0] :
                    npc.Template.InitQuestion.ContainsKey(prmClient.account.character.mapID) ? npc.Template.InitQuestion[prmClient.account.character.mapID] : -1;
                if (!Database.table.Npc.InitQuestion.ContainsKey(idDialog))
                    return;
                Dialog dialog = Database.table.Npc.InitQuestion[idDialog];
                prmClient.account.character.State = EnumClientState.OnDialog;
                prmClient.send("DCK" + id);
                prmClient.send(GetPacketDialog(dialog));
            }
        }

        [PacketAttribute("DV")]
        public static void ExitDialog(Network.listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.State == EnumClientState.OnDialog)
            {
                prmClient.account.character.State = EnumClientState.None;
                prmClient.send("DV");
            }
        }

        private static string GetPacketDialog(Dialog dialog)
        {
            return "DQ" + dialog.ID + (dialog.responses != "" ? "|" + dialog.responses : "");
        }

        //"DR3850|3388"
        [PacketAttribute("DR")]
        public void DialogReponce(Network.listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.State == EnumClientState.OnDialog)
            {
                string[] data = prmPacket.Substring(2).Split('|');
                if (int.TryParse((data[1]), out int id))
                {
                    if (Database.table.Npc.ReponceDialog.ContainsKey(id))
                    {
                        //DJ
                        if (Database.table.Npc.ReponceDialog[id].type == 15)
                        {
                            int[] infoDJ = Array.ConvertAll(Database.table.Npc.ReponceDialog[id].args.Split(','), int.Parse);
                            if (prmClient.account.character.mapID != infoDJ[3])
                                return;
                            Item.Stuff cle = prmClient.account.character.Invertaire.Stuff.Find(x => x.Template.ID == infoDJ[2]);
                            if (cle != null)
                            {
                                if (cle.Quantity == 1)
                                    prmClient.account.character.Invertaire.Stuff.Remove(cle);
                                else
                                    cle.Quantity -= 1;
                                Item.Stuff.RangerItem(ref prmClient.account.character.Invertaire.Stuff);
                                Map.MapMouvement.SwitchMap(prmClient, infoDJ[0], infoDJ[1]);
                            }

                        }
                        else if (Database.table.Npc.ReponceDialog[id].type == 2)
                        {
                            int[] infoDJ = Array.ConvertAll(Database.table.Npc.ReponceDialog[id].args.Split(','), int.Parse);
                            if (prmClient.account.character.mapID != infoDJ[2])
                                return;
                            Map.MapMouvement.SwitchMap(prmClient, infoDJ[0], infoDJ[1]);
                        }
                        else if (int.TryParse(Database.table.Npc.ReponceDialog[id].args, out int newID) && Database.table.Npc.InitQuestion.ContainsKey(newID))
                        {
                            prmClient.account.character.State = EnumClientState.OnDialog;
                            Dialog dialog = Database.table.Npc.InitQuestion[newID];
                            prmClient.send(GetPacketDialog(dialog));
                            return;
                        }
                        else
                            ExitDialog(prmClient, "");
                    }
                }

            }
        }
    }
}
