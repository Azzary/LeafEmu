using LeafEmu.World.Game.Quest;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace LeafEmu.World.Database.table
{
    public class Quest
    {
        public readonly static Dictionary<int, QuestEtape> QuestsEtapes = new Dictionary<int, QuestEtape>();
        public readonly static Dictionary<int, QuestObjectif> QuestsObjectifs = new Dictionary<int, QuestObjectif>();
        public readonly static Dictionary<int, Game.Quest.Quest> Quests = new Dictionary<int, Game.Quest.Quest>();
        private static MySqlConnection conn;
        
        public Quest(MySqlConnection _conn)
        {
            conn = _conn;

            MySqlCommand Create_table = new MySqlCommand(@"
                CREATE TABLE IF NOT EXISTS `quests_characters`  (
                `id` int(11) NOT NULL,
                `id_quest` int(11) NOT NULL,
                `finish` int(11) NOT NULL,
                `id_character` int(11) NOT NULL,
                `stepsValidation` varchar(255) DEFAULT NULL,
                 PRIMARY KEY (id));
            ", conn);
            GetUid();
            Create_table.ExecuteNonQuery();
            LoadQuestsObjectifs();
            LoadQuestEtapes();
            LoadQuestsData();
        }

        private void LoadQuestsData()
        {
            string r = "SELECT id, etapes, objectif, npc, action" +
                ",args, condi, deleteFinish  from quest_data;";

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
                        Quests.Add((int)dr["id"], new Game.Quest.Quest((int)dr["id"], (string)dr["etapes"], (string)dr["objectif"], (int)dr["npc"],
                            dr["action"].ToString(), dr["args"].ToString(), Convert.ToBoolean(dr["deleteFinish"]), dr["condi"].ToString()));
                    }
                }
                Reader.Close();
            }
        }

        private void LoadQuestsObjectifs()
        {
            string r = "SELECT id, xp, kamas, item, action" +
                " from quest_objectifs;";

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
                        QuestsObjectifs.Add((int)dr["id"], new Game.Quest.QuestObjectif((int)dr["id"], (int)dr["xp"], (int)dr["kamas"], dr["item"].ToString(),
                            (string)dr["action"].ToString()));
                    }
                }
                Reader.Close();
            }
        }

        private void LoadQuestEtapes()
        {
            string r = "SELECT id, type, objectif, item, npc," +
                "monster, conditions, validationType from quest_etapes;";

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
                        QuestsEtapes.Add((int)dr["id"], new Game.Quest.QuestEtape((int)dr["id"], (int)dr["type"], (int)dr["objectif"], (string)dr["item"],
                            (int)dr["npc"], (string)dr["monster"], (string)dr["conditions"], (int)dr["validationType"]));
                    }
                }
                Reader.Close();
            }
        }


        public static void LoadQuestPlayer(Network.listenClient prmClient)
        {
            foreach (var character in prmClient.account.ListCharacter)
            {
                // "from personnage where accountid='" + prmClient.account.ID + "';";
                string r = "SELECT id, id_quest, finish, id_character, stepsValidation" +
                    $" from quests_characters where id_character = {character.id};";
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
                            character.questList.Add((int)dr["id"], new QuestPlayer((int)dr["id"], (int)dr["id_quest"], Convert.ToBoolean(dr["finish"]), character, dr["stepsValidation"].ToString(), true));
                        }
                    }
                    Reader.Close();
                }
            }
        }

        
        public static void RemoveQuestPlayer(Network.listenClient prmClient)
        {
            var character = prmClient.account.character;
            string r = $"DELETE FROM quests_characters WHERE id_character={character.id};";
            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                Reader.Close();
            }
        }

        public static int PrimarayID { get; set; }
        public static int GetNewID()
        {
            PrimarayID++;
            return PrimarayID;
        }

        private void GetUid()
        {
            string strSQL = "Select id from quests_characters";
            using (MySqlCommand commande = new MySqlCommand(strSQL, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        if ((int)Reader["id"] > PrimarayID)
                           PrimarayID = (int)Reader["id"];
                    }
                }
                Reader.Close();
            }
        }

        public static void UpdateQuestPlayer(Game.Entity.Character character)
        {
            if (character.questList.Count == 0)
                return;
            string r = string.Empty;
            foreach (var questPlayer in character.questList.Values)
            {
                if (questPlayer.quest.delete && questPlayer.finish && questPlayer.isAddInDataBase)
                    r = $"DELETE FROM quests_characters WHERE id={questPlayer.id};";
                else if (questPlayer.isAddInDataBase)
                    r += $"UPDATE `quests_characters` SET `finish` = {Convert.ToInt32(questPlayer.finish)}, `stepsValidation` = '{questPlayer.getQuestEtapeString()}' WHERE `id` = {questPlayer.id};";
                else
                    r += $"INSERT INTO `quests_characters` VALUES ({questPlayer.id}, {questPlayer.quest.id}, {Convert.ToInt32(questPlayer.finish)}, {character.id}, '{questPlayer.getQuestEtapeString()}');";
            }
            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                Reader.Close();
            }
        }
    }
}
