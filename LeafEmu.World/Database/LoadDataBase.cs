using MySql.Data.MySqlClient;


namespace LeafEmu.World.Database
{
    public class LoadDataBase
    {
        public table.Character.Character tablecharacter;
        public table.Spells tableSpells;
        public table.Map tablemap;
        public table.Monster tableMonster;
        public table.Experience experience;
        public table.Item.model_item model_item;
        public table.Drops TableDrops;
        public table.Npc tableNpc;
        public table.Donjons tableDonjons;
        public table.Item.items_personnage tableCharacterItems;
        public table.Character.CharacterSpells tableCharacterSpells;
        private static int UIDPersonnage = 1;
        private static int UIDItem = 1;
        public LoadDataBase()
        {
            MySqlConnection conn = LeafEmu.DataBase.GetMySqlConnection.GetWorldMySqlConnection();
            conn.Open();
            Logger.Logger.Log("load Table:");
            Logger.Logger.Log("Character...", 10);
            tablecharacter = new table.Character.Character(conn);
            GetId(conn);
            tableCharacterSpells = new table.Character.CharacterSpells(conn);
            Logger.Logger.Log("Spells...", 10);
            tableSpells = new table.Spells(conn);
            Logger.Logger.Log("Monster...", 10);
            tableMonster = new table.Monster(conn);
            Logger.Logger.Log("Map...", 10);
            tablemap = new table.Map(conn);
            Logger.Logger.Log("Npc...", 10);
            tableNpc = new table.Npc(conn);
            Logger.Logger.Log("Donjons...", 10);
            tableDonjons = new table.Donjons(conn);
            Logger.Logger.Log("Items...", 10);
            model_item = new table.Item.model_item(conn);
            Logger.Logger.Log("Drops...", 10);
            TableDrops = new table.Drops(conn);
            tableCharacterItems = new table.Item.items_personnage(conn);
            Logger.Logger.Log("Experience...", 10);
            experience = new table.Experience(conn);
        }
        public static int GetNewUIDCharacter()
        {
            UIDPersonnage++;
            return UIDPersonnage;
        }
        public static int GetNewUIDItem()
        {
            UIDItem++;
            return UIDItem;
        }

        private void GetId(MySqlConnection conn)
        {
            string strSQL = "Select id from personnage";
            using (MySqlCommand commande = new MySqlCommand(strSQL, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        if ((int)Reader["id"] > UIDPersonnage)
                            UIDPersonnage = (int)Reader["id"];
                    }
                }
                Reader.Close();
            }
            strSQL = "Select uid from items_personnage";
            using (MySqlCommand commande = new MySqlCommand(strSQL, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        if ((int)Reader["uid"] > UIDPersonnage)
                            UIDPersonnage = (int)Reader["uid"];
                    }
                }
                Reader.Close();
            }
        }
    }

}
