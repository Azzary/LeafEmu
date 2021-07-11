using LeafEmu.World.PacketGestion;
using System;

namespace LeafEmu.World.Game.Spells.LaunchSpells
{
    public class LaunchSpells
    {
        [PacketAttribute("GA300")]
        public void PacketLaunchSpells(Network.listenClient prmClient, string prmPacket)
        {
            if (!prmClient.account.character.FightInfo.YourTurn || prmClient.account.character.CurrentFight == null)
                return;
            string[] data = prmPacket.Substring(5).Split(';');
            int ID = Convert.ToInt32(data[0]);
            SpellsEntity SpellsLauch = prmClient.account.character.Spells.Find(x => x.id == ID);
            if (SpellsLauch == null)
                return;
            int CellTarget = Convert.ToInt32(data[1]);
            SpellsStats StatsSpells = Database.table.Spells.SpellsList[SpellsLauch.id].SpellsStats[SpellsLauch.level - 1];
            int MaxPO = StatsSpells.GetmaxPO(prmClient.account.character.PO);
            if (SpellsManagement.CheckLaunchSpells(prmClient.account.character.FightInfo.FightCell, CellTarget, StatsSpells, prmClient.account.character, out string msg))
            {
                SpellsManagement.LaunchSpells(StatsSpells, prmClient.account.character, CellTarget);
            }
            else
            {
                prmClient.send($"Im{msg}");
                prmClient.SendToAllFight($"GA;102;{prmClient.account.character.id};{prmClient.account.character.id},-0");
            }
        }
    }
}
