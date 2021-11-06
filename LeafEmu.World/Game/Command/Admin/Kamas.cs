using LeafEmu.World.Game.Command.Gestion;
using System;

namespace LeafEmu.World.Game.Command.Admin
{
    class Kamas
    {

        [CommandAttribute(3, "kamas", 1, "{Kamas to add} {Target} Add xp to the target")]
        public void AddKamas(Network.listenClient prmClient, string command)
        {
            string[] InfoKamas = command.Split(' ');
            if (Int64.TryParse(InfoKamas[1], out Int64 kamas))
            {
                prmClient = InfoKamas.Length > 2 ? prmClient.CharacterInWorld.Find(x => x.account.character.speudo == InfoKamas[2]) : prmClient;
                if (prmClient == null)
                    return;
                prmClient.account.character.SetKamas(prmClient,prmClient.account.character.kamas + kamas);
            }

        }

    }
}
