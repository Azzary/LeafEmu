using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace LeafEmu.World.Database.table
{
    public class Drops
    {
        public static Dictionary<int, List<Game.Item.Item>> DropsTables = new Dictionary<int, List<Game.Item.Item>>();
        private MySqlConnection conn;

        public Drops(MySqlConnection _conn)
        {
            conn = _conn;
            MySqlCommand Create_table = new MySqlCommand(@"
            CREATE TABLE IF NOT EXISTS `drops` (
            `monsterName` varchar(255) NOT NULL DEFAULT '',
            `monsterId` int(10) UNSIGNED NOT NULL,
            `objectName` varchar(255) NOT NULL DEFAULT '',
            `objectId` int(10) UNSIGNED NOT NULL,
            `percentGrade1` decimal(6, 3) UNSIGNED NOT NULL,
            `percentGrade2` decimal(6, 3) UNSIGNED NOT NULL,
            `percentGrade3` decimal(6, 3) UNSIGNED NOT NULL,
            `percentGrade4` decimal(6, 3) UNSIGNED NOT NULL,
            `percentGrade5` decimal(6, 3) UNSIGNED NOT NULL,
            `ceil` smallint(5) UNSIGNED NOT NULL COMMENT 'Prospection ceil',
            `action` varchar(255) NOT NULL DEFAULT '1',
            `level` int(3) NOT NULL DEFAULT -1)", conn);

            Create_table.ExecuteNonQuery();
            LoadDataBaseDrops();
        }

        private void LoadDataBaseDrops()
        {
            string r = "SELECT monsterId, ceil, action, Level, objectId," +
                "percentGrade1, percentGrade2, percentGrade3, percentGrade4, percentGrade5 from drops;";

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
                        int idItem = Convert.ToInt32(dr["objectId"]);
                        if (!Item.model_item.AllItems.ContainsKey(idItem))
                            continue;
                        Game.Item.Item currItem = Item.model_item.AllItems[idItem];
                        var actionSplit = dr["action"].ToString().Split(':');
                        currItem.ActionDrops = int.Parse(actionSplit[0]);
                        currItem.ceil = (UInt16)dr["ceil"];
                        currItem.levelDrops = (int)dr["Level"];
                        if (actionSplit.Length > 1)
                        {
                            currItem.ConditionDrops = actionSplit[1];
                        }
                        currItem.ConditionDrops = dr["action"].ToString().Split(':')[0];
                        currItem.PercentDropeByGrade = new Decimal[5] { (Decimal)dr["percentGrade1"], (Decimal)dr["percentGrade2"], (Decimal)dr["percentGrade3"],
                                                                      (Decimal)dr["percentGrade4"], (Decimal)dr["percentGrade5"]};

                        int monsterid = Convert.ToInt32(dr["monsterId"]);
                        if (DropsTables.ContainsKey(monsterid))
                        {
                            DropsTables[monsterid].Add(currItem);
                        }
                        else
                            DropsTables.Add(monsterid, new List<Game.Item.Item>() { currItem });
                    }

                }
                Reader.Close();
            }

        }


    }
}
