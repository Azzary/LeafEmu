using LeafEmu.World.Game.Command.Gestion;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafEmu.World.Game.Command.Admin
{


    public class debug
    {
        [CommandAttribute(3, "mapkey", 0, "chang the mapkey")]
        public void size(Network.listenClient client, string command)
        {
            Database.table.Map.ChangeKey(client, client.account.character.Map.Id);
        }
    }
}
