using LeafEmu.World.Game.Command.Gestion;

namespace LeafEmu.World.Game.Command.Admin
{
    public class CharacterCommand
    {

        [CommandAttribute(3, "size", 1, "{size} new size (100 normal)")]
        public void size(Network.listenClient client, string command)
        {
            string[] InfoTP = command.Split(' ');
            if (int.TryParse(InfoTP[1], out int size))
            {
                client.account.character.Scale = size;
                client.SendToAllMap(Game.Map.MapGestion.CreatePacketCharacterGM(client.account.character));
            }
        }

        [CommandAttribute(3, "gfx", 1, "{new gfxid}")]
        public void gfx(Network.listenClient client, string command)
        {
            string[] InfoTP = command.Split(' ');
            if (int.TryParse(InfoTP[1], out int gfxid))
            {
                client.account.character.gfxID = gfxid;
                client.SendToAllMap($"GM|-{client.account.character.id}\0" + Map.MapGestion.CreatePacketCharacterGM(client.account.character));
            }
        }
    }
}
