using LeafEmu.World.Game.Item;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LeafEmu.World.Database.table.Item
{
    public class items_personnage
    {
        MySqlConnection conn;
        public items_personnage(MySqlConnection _conn)
        {
            conn = _conn;
            MySqlCommand Create_table = new MySqlCommand(@"
            CREATE TABLE IF NOT EXISTS `items_personnage`(
            `uid` int (11) NOT NULL AUTO_INCREMENT,
            `modelid` int (11) NOT NULL,
            `personnageid` int (11) NOT NULL,
            `position` int (11) NOT NULL,
            `quantity` int NOT NULL,
            `stats` text NOT NULL,
            `objectif` int (11) NOT NULL DEFAULT 0,
            `price` bigint(31) NOT NULL DEFAULT 0,
            PRIMARY KEY(`uid`))", conn);

            Create_table.ExecuteNonQuery();
        }

        public void LoadItems(Network.listenClient prmClient)
        {
            foreach (Game.Entity.Character character in prmClient.account.ListCharacter)
            {
                string r = $"SELECT uid, modelid, stats, position, quantity from items_personnage where personnageid = {character.id};";


                using (MySqlCommand commande = new MySqlCommand(r, conn))
                {
                    MySqlDataReader Reader = commande.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            if (model_item.AllItems.ContainsKey((int)Reader["modelid"]))
                            {
                                Game.Item.Item item = model_item.AllItems[(int)Reader["modelid"]];
                                if (item.Type < 0)
                                    return;
                                List<Effect> effects = new List<Effect>();
                                foreach (var ItemEffect in ((string)Reader["stats"]).Split(";"))
                                {
                                    if (ItemEffect == "")
                                    {
                                        continue;
                                    }
                                    string[] InfoEffect = ItemEffect.Split(',');
                                    Effect temp = new Effect();
                                    temp.ID = Convert.ToInt32(InfoEffect[0]);
                                    temp.Fix = Convert.ToInt32(InfoEffect[1]);
                                    temp.Max = Convert.ToInt32(InfoEffect[2]);
                                    temp.Max = Convert.ToInt32(InfoEffect[3]);
                                    temp.CurrentJet = Convert.ToInt32(InfoEffect[4]);
                                    effects.Add(temp);
                                }
                                character.Invertaire.addItem((int)Reader["uid"],
                                    (int)Reader["position"], (int)Reader["Quantity"], effects.ConvertAll(x => x), item);
                                effects.Clear();
                            }


                        }
                    }
                    Reader.Close();
                }
            }
        }

    }
}
