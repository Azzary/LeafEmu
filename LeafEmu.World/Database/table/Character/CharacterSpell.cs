using LeafEmu.World.Network;
using MySql.Data.MySqlClient;

namespace LeafEmu.World.Database.table.Character
{
    public class CharacterSpells
    {

        MySqlConnection conn;

        public CharacterSpells(MySqlConnection _conn)
        {
            conn = _conn;
            MySqlCommand Create_table = new MySqlCommand(
           @"CREATE TABLE IF NOT EXISTS `characterSpells` (
          `characterID` int(11) NOT NULL,
          `level` int(11) NOT NULL,
          `pos` int(11) NOT NULL,
          `id` int(11) NOT NULL)", conn);

            Create_table.ExecuteNonQuery();
        }

        public void LoadSpells(listenClient prmClient)
        {
            foreach (Game.Entity.Character character in prmClient.account.ListCharacter)
            {
                string r = "SELECT id, level, pos from characterSpells where characterID='" + character.id + "'";
                using (MySqlCommand commande = new MySqlCommand(r, conn))
                {
                    MySqlDataReader Reader = commande.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            character.Spells.Add(new Game.Spells.SpellsEntity((int)Reader["id"], (int)Reader["level"], (int)Reader["pos"]));
                        }
                    }
                    Reader.Close();
                }
            }
        }

    }
}
