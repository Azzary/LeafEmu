using LeafEmu.World.Game.Command.Gestion;
using System.Text;

namespace LeafEmu.World.Game.Command.Player
{
    class Help
    {
        [CommandAttribute(0, "help", 0, "Help")]
        public void Help_Command(Network.listenClient Prmclient, string command)
        {
            StringBuilder msg = new StringBuilder();
            foreach (CommandDatas Command in CommandGestion.metodos)
            {
                if (Prmclient.account.Role >= Command.RoleNeeded)
                    msg.Append($".{Command.name_command}  " + Command.CommandInfo + "\n");


            }
            Prmclient.sendDebugMsg(msg.ToString());
        }

    }
}
