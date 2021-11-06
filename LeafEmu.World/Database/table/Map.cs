using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace LeafEmu.World.Database.table
{
    public class Map
    {
        public static Dictionary<int, Game.Map.Map> Maps = new Dictionary<int, Game.Map.Map>();
        public static List<object[]> Maps2 = new List<object[]>();

        static MySqlConnection conn;

        public Map(MySqlConnection _conn)
        {
            conn = _conn;
            MySqlCommand Create_table = new MySqlCommand(@"
            CREATE TABLE IF NOT EXISTS `maptemplate` (
            `id` int(11) NOT NULL,
            `CreateTime` varchar(50) NOT NULL,
            `Width` int(2) NOT NULL DEFAULT -1,
            `Height` int(2) NOT NULL DEFAULT -1,
            `PosFight` varchar(300) NOT NULL DEFAULT '|',
            `DataKey` text  NOT NULL,
            `Data` text NOT NULL,
            `cells` text NOT NULL,
            `mobs` text NOT NULL,
            `X` int(3) NOT NULL,
            `Y` int(3) NOT NULL,
            `subArea` int(3) NOT NULL,
            `numgroup` int(2) NOT NULL DEFAULT 5,
            `maxMobs` int(2) NOT NULL DEFAULT 8,
            `Capaciter` int(5) NOT NULL DEFAULT 0,
            `descripcion` int(5) NOT NULL DEFAULT 0,
            PRIMARY KEY (`id`))", conn);
            Create_table.ExecuteNonQuery();
            get_map();
            fusion();
            LoadInteractiveDoors();
            EndFightAction();
        }

        public static void ChangeKey(Network.listenClient prmClient, int mapID)
        {
            var map2 = Maps2.Find(x => (int)x[0] == mapID);
            string r1 = $"UPDATE `maptemplate` SET `DataKey` = '{map2[1]}' WHERE `id` = {map2[0]};";
            using (MySqlCommand commande2 = new MySqlCommand(r1, conn))
            {
                MySqlDataReader Reader2 = commande2.ExecuteReader();
                Reader2.Close();
            }
            Maps[mapID].DataKey = map2[1].ToString();
            prmClient.SendMessageToPlayer($"Map: {mapID} change key...");
            Logger.Logger.Log($"Map: {mapID} change key...");
        }

        private void EndFightAction()
        {
            string r = "SELECT map, fighttype, action, args, cond from endfight_action;";
            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        int mapid = (int)Reader["Map"];
                        if (Maps.ContainsKey(mapid))
                        {
                            string[] tpInfo = Reader["args"].ToString().Split(',');

                            if (int.TryParse(tpInfo[0], out int map) && int.TryParse(tpInfo[1], out int cell))
                            {
                                Maps[mapid].EndFightActions =
                            new Game.Map.EndFightAction((int)Reader["fighttype"], (int)Reader["action"], map, cell, Reader["cond"].ToString());
                            }
                        }
                    }
                }
                Reader.Close();
            }
        }

        private void fusion()
        {
            string r = "SELECT id, DataKey from maptemplate_last;";

            string r1 = string.Empty;
            string r2 = string.Empty;

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
                        Maps2.Add(new object[2] { (int)dr["id"], dr["DataKey"] });
                    }
                    Reader.Close();
                }
                return;
                int ddi = 0;

                foreach (object[] map2 in Maps2)
                {
                    if (Maps.ContainsKey(Convert.ToInt32(map2[0])))
                    {
                        ddi++;
                        Util.Turn(ddi, Maps2.Count);
                        Game.Map.Map map = Maps[Convert.ToInt32(map2[0])];

                        r1 += $"UPDATE `maptemplate` SET `DataKey` = '{map2[1]}' WHERE `id` = {map2[0]};";
                        //r1 = "DELETE from maptemplate WHERE" +
                        //    " Id=" + map.Id + ";";

                        //r2 = "INSERT INTO maptemplate SET Id=" + map.Id + "," +
                        //"CreateTime='" + map.CreateTime + "X'," +
                        //"Width=" + map.Width + "," +
                        //"Height=" + map.Height + "," +
                        //"PosFight='" + map.PosFight + "'," +
                        //"DataKey='" + map.DataKey + "'," +
                        //"Data='" + map.Data + "'," +
                        //"cells= ''," +
                        //"mobs='" + map.stringmobs + "'," +
                        //"X=" + map.X + "," +
                        //"Y=" + map.Y + "," +
                        //"subArea=" + map.SubArea + "," +
                        //"nroGrupo=" + map.NbGroups + "," +
                        //"maxMobs=" + map.MaxMobs + "," +
                        //"Capaciter=" + map.Capabilities + "," +
                        //"descripcion= 0 ;";

                        //using (MySqlCommand commande2 = new MySqlCommand(r2, conn))
                        //{

                        //    MySqlDataReader Reader2 = commande2.ExecuteReader();
                        //    Reader2.Close();
                        //}
                    }
                }
                using (MySqlCommand commande2 = new MySqlCommand(r1, conn))
                {
                    MySqlDataReader Reader2 = commande2.ExecuteReader();
                    Reader2.Close();
                }
            }
            Logger.Logger.Log("fini");
        }
        private void get_map()
        {
    //        string r = "SELECT id, CreateTime, Data, DataKey," +
    //"mobs, Width, Height, mappos, Capaciter, maxMobs, PosFight, nroGrupo," +
    //"subArea from maptemplate;";
            string r = "SELECT id, CreateTime, Data, DataKey," +
                "mobs, Width, Height, mappos, fixSize, maxMobs, PosFight, numgroup" +
                " from maptemplate;";

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
                        int mapId = (int)dr["id"];
                        string X = dr["mappos"].ToString().Split(',')[0];
                        string Y = dr["mappos"].ToString().Split(',')[1];
                        string subar = dr["mappos"].ToString().Split(',')[2];
                        Maps.Add(mapId, new Game.Map.Map(mapId, dr["CreateTime"].ToString(), dr["Data"].ToString(), dr["DataKey"].ToString(),
                                                         dr["mobs"].ToString(), dr["Width"].ToString(), dr["Height"].ToString(), X,
                                                         Y, dr["fixSize"].ToString(), subar, (int)dr["maxMobs"], (string)dr["PosFight"], (int)dr["numgroup"]));

                    }
                }
                Reader.Close();
            }


            r = "SELECT MapID, CellID, ActionsArgs from scripted_cells;";
            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                if (Reader.HasRows)
                {

                    while (Reader.Read())
                    {
                        int mapid = (int)Reader["MapID"];
                        if (Maps.ContainsKey(mapid))
                        {
                            int cellID = (int)Reader["CellID"];
                            if (!Maps[mapid].CellTp.ContainsKey(cellID))
                            {
                                string[] strArgs = Reader["ActionsArgs"].ToString().Split(',');
                                if (strArgs.Length < 2)
                                    continue;

                                int[] intArgs = new int[2] { Convert.ToInt32(strArgs[0]), Convert.ToInt32(strArgs[1]) };
                                Maps[mapid].CellTp.Add(cellID, intArgs);
                            }
                        }
                    }
                }
                Reader.Close();
            }
        }

        private void LoadInteractiveDoors()
        {
            string r = "SELECT maps, doorsEnable, doorsDisable, cellsEnable, " +
                "cellsDisable, requiredCells, button, time from interactive_doors;";

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
                        foreach (var item in dr["maps"].ToString().Split(','))
                        {
                            int mapid = Convert.ToInt32(item);
                            if (Map.Maps.ContainsKey(mapid))
                            {
                                try
                                {
                                    string[] strDoorInfo = dr["doorsEnable"].ToString().Split(':');
                                    string[] strCellRequiredInfo = dr["requiredCells"].ToString().Split(':');
                                    int[] CellDoors = Array.ConvertAll(strDoorInfo[1].Split(','), int.Parse);
                                    int[] requiredCells = Array.ConvertAll(strCellRequiredInfo[1].Split(';'), int.Parse);
                                    var temp = new Game.Map.InteractiveDoors(Convert.ToInt32(strDoorInfo[0]), Convert.ToInt32(strCellRequiredInfo[0]));
                                    temp.DoorCellID.AddRange(CellDoors);
                                    temp.RequiedCells.AddRange(requiredCells);
                                    Map.Maps[mapid].CellForceDisable.AddRange(CellDoors);
                                    Map.Maps[mapid].InteractiveDoors.Add(temp);
                                }
                                catch (Exception)
                                {
                                    Logger.Logger.Log("Error to add Interactive Doors on map " + mapid, 15);
                                }
                            }
                        }
                    }

                }
                Reader.Close();
            }

        }

    }


}


