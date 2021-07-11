using LeafEmu.World.Game.Command.Gestion;
using System;

namespace LeafEmu.World.Game.Command.Admin
{
    class Xp
    {

        [CommandAttribute(3, "xp", 1, "{XP to add} {Target} Add xp to the target")]
        public void AddPM(Network.listenClient PrmClient, string command)
        {
            string[] InfoXP = command.Split(' ');
            if (Int64.TryParse(InfoXP[1], out Int64 xp))
            {
                PrmClient = InfoXP.Length > 2 ? PrmClient.CharacterInWorld.Find(x => x.account.character.speudo == InfoXP[2]) : PrmClient;
                if (PrmClient == null)
                    return;

                PrmClient.account.character.AddXp(xp, PrmClient);

            }

        }

    }
}
