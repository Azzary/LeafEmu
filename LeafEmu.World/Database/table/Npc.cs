using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace LeafEmu.World.Database.table
{
    public class Npc
    {

        MySqlConnection conn;
        public static Dictionary<int, Game.Entity.Npc.NpcTemplate> NpcTemplate = new Dictionary<int, Game.Entity.Npc.NpcTemplate>();
        public static Dictionary<int, Game.Entity.Npc.Dialog> InitQuestion = new Dictionary<int, Game.Entity.Npc.Dialog>();
        public static Dictionary<int, Game.Entity.Npc.ReponceDialog> ReponceDialog = new Dictionary<int, Game.Entity.Npc.ReponceDialog>();

        public Npc(MySqlConnection _conn)
        {
            conn = _conn;
            MySqlCommand Create_table = new MySqlCommand(@"
            CREATE TABLE IF NOT EXISTS `npc_template` (
              `id` int(11) NOT NULL,
              `bonusValue` int(11) NOT NULL,
              `gfxID` int(11) NOT NULL,
              `scaleX` int(11) NOT NULL,
              `scaleY` int(11) NOT NULL,
              `sex` int(11) NOT NULL,
              `color1` int(11) NOT NULL,
              `color2` int(11) NOT NULL,
              `color3` int(11) NOT NULL,
              `accessories` varchar(30) NOT NULL DEFAULT '0,0,0,0',
              `extraClip` int(11) NOT NULL DEFAULT -1,
              `customArtWork` int(11) NOT NULL DEFAULT 0,
              `initQuestion` text NOT NULL,
              `ventes` text NOT NULL,
              `quests` text NOT NULL,
              `exchanges` text NOT NULL,
              `path` varchar(255) NOT NULL DEFAULT '',
              `informations` int(11) NOT NULL DEFAULT 0,
              PRIMARY KEY (`id`) USING BTREE,
              INDEX `id`(`id`) USING BTREE
            )", conn);

            Create_table.ExecuteNonQuery();
            LoadNpc_template();
            LoadNpc();
            LoadNpc_Dialog();
        }

        private void LoadNpc()
        {
            string r = "SELECT mapid, npcid, cellid, orientation, isMovable from npcs;";

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
                        if (Map.Maps.ContainsKey((int)dr["mapid"]) &&
                            NpcTemplate.ContainsKey((int)dr["npcid"]))
                        {
                            Map.Maps[(int)dr["mapid"]].Npcs.Add(new Game.Entity.Npc.Npc((int)dr["cellid"], (int)dr["orientation"], -i * 100, NpcTemplate[(int)dr["npcid"]]));
                        }
                    }

                }
                Reader.Close();
            }

        }
        private void LoadNpc_template()
        {
            string r = "SELECT id, bonusValue, gfxID, scaleX, scaleY, sex, color1, color2, color3," +
                       "accessories, extraClip, customArtWork, initQuestion, ventes, quests, exchanges," +
                       "path, informations from npc_template;";

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
                        NpcTemplate.Add((int)dr["id"], new Game.Entity.Npc.NpcTemplate((int)dr["id"], (int)dr["bonusValue"], (int)dr["gfxID"], (int)dr["scaleX"],
                            (int)dr["scaleY"], (int)dr["sex"], (int)dr["color1"], (int)dr["color2"], (int)dr["color3"], (string)dr["accessories"],
                            (int)dr["extraClip"], (int)dr["customArtWork"], (string)dr["initQuestion"], (string)dr["ventes"], (string)dr["quests"],
                            (string)dr["exchanges"], (string)dr["path"], (int)dr["informations"]));
                    }

                }
                Reader.Close();
            }
        }
        private void LoadNpc_Dialog()
        {
            string r = "SELECT ID, type, args, nom from npc_reponses_actions;";

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
                        if (!ReponceDialog.ContainsKey((int)dr["ID"]))
                            ReponceDialog.Add((int)dr["ID"], new Game.Entity.Npc.ReponceDialog((int)dr["ID"], (int)dr["type"], (string)dr["args"], (string)dr["nom"]));
                    }

                }
                Reader.Close();
            }

            r = "SELECT ID, responses, params, cond, ifFalse, description from npc_questions;";

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
                        InitQuestion.Add((int)dr["ID"], new Game.Entity.Npc.Dialog((int)dr["ID"], (string)dr["responses"], (string)dr["params"], (string)dr["cond"],
                            (string)dr["ifFalse"], (string)dr["description"]));
                    }

                }
                Reader.Close();
            }
        }
    }
}
