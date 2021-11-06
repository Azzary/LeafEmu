using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace LeafEmu.World.Database.table
{
    public class Monster
    {
        public readonly static Dictionary<int, List<Game.Entity.Mob>> Mobs = new Dictionary<int, List<Game.Entity.Mob>>();
        MySqlConnection conn;

        public Monster(MySqlConnection _conn)
        {
            conn = _conn;
            MySqlCommand Create_table = new MySqlCommand(@"
            CREATE TABLE IF NOT EXISTS `monsters`  (
            `id` int(11) NOT NULL,
            `name` varchar(100)  NOT NULL,
            `gfxID` int(11) NOT NULL,
            `align` int(11) NOT NULL,
            `grades` text  NOT NULL,
            `colors` varchar(30)  NOT NULL DEFAULT '-1,-1,-1',
            `stats` text  NOT NULL COMMENT 'For,Sag,Int,Cha,Agi',
            `statsInfos` varchar(200)  NOT NULL DEFAULT '0;0;0;1' COMMENT 'dmg;%dmg;soins;créainv',
            `spells` text  NOT NULL,
            `pdvs` varchar(200)  NOT NULL DEFAULT '1|1|1|1|1|1|1|1|1|1',
            `points` varchar(200)  NOT NULL DEFAULT '1;1|1;1|1;1|1;1|1;1|1;1|1;1|1;1|1;1|1;1',
            `inits` varchar(200)  NOT NULL DEFAULT '1|1|1|1|1|1|1|1|1|1',
            `minKamas` int(11) NOT NULL DEFAULT 0,
            `maxKamas` int(11) NOT NULL DEFAULT 0,
            `exps` varchar(200)  NOT NULL DEFAULT '1|1|1|1|1|1|1|1|1|1',
            `AI_Type` int(11) NOT NULL DEFAULT 1 COMMENT '0: poutch 1: Agressif 2: Fuyarde 3: Soutient 4: Spécial',
            `capturable` int(11) NOT NULL DEFAULT 1,
            `type` int(11) NOT NULL DEFAULT 1 COMMENT '1 : Monster, 2 : Mascotte, 3 : Archi monster',
            `aggroDistance` tinyint(5) NULL DEFAULT 0,
            UNIQUE INDEX `id`(`id`) USING BTREE)", conn);
            Create_table.ExecuteNonQuery();
            get_monster();
        }

        private void get_monster()
        {
            string r = "SELECT Id, name, gfxID, stats, statsInfos, spells, pdvs, points," +
                " inits, minKamas, maxKamas, colors, aggroDistance, exps, grades" +
                ", AI_Type, capturable, name from monsters;";

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
                        int id = (int)dr["Id"];

                        Mobs.Add(id, Game.Entity.Mob.GetAllMobsGrade(id, (string)dr["name"], (int)dr["gfxID"], (string)dr["grades"], (string)dr["stats"],
                            (string)dr["statsInfos"], (string)dr["spells"], (string)dr["pdvs"], (string)dr["points"], (string)dr["inits"],
                            (int)dr["minKamas"], (int)dr["maxKamas"], (string)dr["colors"], (SByte)dr["aggroDistance"], (string)dr["exps"],
                            (int)dr["AI_Type"], (int)dr["capturable"], (string)dr["name"]));
                    }

                }
                Reader.Close();
            }
        }

    }
}
