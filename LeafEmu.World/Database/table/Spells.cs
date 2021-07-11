using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace LeafEmu.World.Database.table
{
    public class Spells
    {

        MySqlConnection conn;

        public static Dictionary<int, Game.Spells.SpellsDatas> SpellsList = new Dictionary<int, Game.Spells.SpellsDatas>();

        public Spells(MySqlConnection _conn)
        {
            conn = _conn;
            MySqlCommand Create_table = new MySqlCommand(@"
            CREATE TABLE IF NOT EXISTS `sorts` (
          `id` int(11) NOT NULL,
          `nom` varchar(100)  NOT NULL,
          `sprite` int(11) NOT NULL,
          `spriteinfos` text NOT NULL,
          `lvl1` text  NOT NULL,
          `lvl2` text  NOT NULL,
          `lvl3` text  NOT NULL,
          `lvl4` text  NOT NULL,
          `lvl5` text  NOT NULL,
          `lvl6` text  NOT NULL,
          `effectTarget` text NOT NULL,
          `type` text NOT NULL,
          `duration` int(11) NOT NULL,
           PRIMARY KEY (`id`))", conn);

            Create_table.ExecuteNonQuery();
            LoadSpells();
        }

        private void LoadSpells()
        {
            string r = "SELECT id, lvl1, lvl2, lvl3," +
                "lvl4, lvl5, lvl6, type from sorts;";


            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                if (Reader.HasRows)
                {
                    int i = 0;

                    DataTable dt = new DataTable();
                    dt.Load(Reader);
                    int numRows = dt.Rows.Count;

                    foreach (DataRow dr in dt.Rows)
                    {
                        i++;
                        Util.Turn(i, numRows);
                        SpellsList.Add((int)dr["id"], new Game.Spells.SpellsDatas((int)dr["id"], (string)dr["lvl1"], (string)dr["lvl2"], (string)dr["lvl3"],
                            (string)dr["lvl4"], (string)dr["lvl5"], (string)dr["lvl6"], (int)dr["type"]));
                    }

                }
                Reader.Close();
            }
        }

    }
}
