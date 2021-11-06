using LeafEmu.World.Game.Item;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LeafEmu.World.Database.table.Item
{
    public class items_personnage
    {
        static MySqlConnection conn;
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
                    MySqlDataReader reader = commande.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (model_item.AllItems.ContainsKey((int)reader["modelid"]))
                            {
                                Game.Item.Item item = model_item.AllItems[(int)reader["modelid"]];
                                if (item.Type < 0)
                                    return;
                                List<Effect> effects = new List<Effect>();
                                foreach (var itemEffect in ((string)reader["stats"]).Split(";"))
                                {
                                    if (itemEffect == string.Empty)
                                    {
                                        continue;
                                    }
                                    string[] infoEffect = itemEffect.Split(',');
                                    Effect temp = new Effect();
                                    temp.ID = Convert.ToInt32(infoEffect[0]);
                                    temp.Fix = Convert.ToInt32(infoEffect[1]);
                                    temp.Max = Convert.ToInt32(infoEffect[2]);
                                    temp.Max = Convert.ToInt32(infoEffect[3]);
                                    temp.CurrentJet = Convert.ToInt32(infoEffect[4]);
                                    effects.Add(temp);
                                }

                                character.Inventaire.addItem(prmClient,
                                    (int)reader["uid"],
                                    (int)reader["position"],
                                    (int)reader["Quantity"],
                                    effects.ConvertAll(x => x),
                                    item);
                                effects.Clear();
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        public static void UpdateItems(int id, List<Stuff> inv)
        {
            inv = Stuff.RangerItem(inv);
            string r = string.Empty;
            foreach (var item in inv)
            {
                r += $"INSERT INTO items_personnage SET personnageid= {id}, uid= {item.UID} ,modelid= {item.Template.ID}, stats= '{item.getStatForDb()}', objectif= 0 , price = 0 , position = {item.Position}, quantity = {item.Quantity};";
            }
            if (r == string.Empty)
                return;

            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                Reader.Close();
            }
        }


        public static void removeItems(int id)
        {
            string r = "DELETE from items_personnage WHERE" +
             " personnageid = " + id + ";";
            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader reader = commande.ExecuteReader();
                reader.Close();
            }
        }
    }
}
