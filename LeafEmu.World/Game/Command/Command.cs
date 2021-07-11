namespace LeafEmu.World.Game.Command
{
    public class Command
    {
        public static void init()
        {
            Gestion.CommandGestion.init();
        }

        public static void GetCommand(Network.listenClient client, string command)
        {
            Gestion.CommandGestion.Gestion(client, command);
        }

    }
}
