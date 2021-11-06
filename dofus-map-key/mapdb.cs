using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace DofusKeyFinder
{
    public class mapdb
    {
        public static List<mapStruc> allMap = new List<mapStruc>();
        static MySqlConnection conn;


        public static void databaseChangeValue(mapStruc map)
        {
            string r1 = $"UPDATE `maptemplate` SET `DataKey` = '{map.DataKey}' WHERE `id` = {map.Id};";
            using (MySqlCommand commande2 = new MySqlCommand(r1, conn))
            {
                MySqlDataReader Reader2 = commande2.ExecuteReader();
                Reader2.Close();
            }
        }

        public static void databaseCreateMap(Dictionary<string,string> map
            ,string datakey, string posFight = "|", string mobs =",", int x = 100000, int y = 100000, int subarrea = 100000,
            int nbgroup = 3, int minsize = 1, int fixesize =-1, int maxsize = 8, string forbidden = "0;0;0;0;0;0;0",
            int sniffed = 0, int minRespawnTime = 12000, int maxRespawnTime = 30000)
        {
            mapStruc.testFonction(map["mapdata"], datakey);
            return;
            posFight = posFight == string.Empty ? "|" : posFight;
            mobs = mobs == string.Empty ? "," : mobs;
            string dataMap = "";
            string r1 = "INSERT INTO `maptemplate` " +
                $"VALUES ({map["id"]}," +
                $" '{map["dateTime"]}'," +
                $" {map["width"]}," +
                $" {map["height"]}," +
                $" '{posFight}'," +
                $" '{datakey}'," +
                $" '{dataMap}'," +
                $" ''{mobs}" +
                $" '{x},{y},{subarrea}'," +
                $" {nbgroup}," +
                $" {minsize}," +
                $" {fixesize}," +
                $" {maxsize}," +
                $" '{forbidden}'," +
                $" {sniffed}," +
                $" {minRespawnTime}," +
                $" {maxRespawnTime});";
            using (MySqlCommand commande2 = new MySqlCommand(r1, conn))
            {
                MySqlDataReader Reader2 = commande2.ExecuteReader();
                Reader2.Close();
            }
        }


        public static void loadMap(MySqlConnection _conn)
        {
            conn = _conn;
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
                        int mapId = (int)dr["id"];
                        string X = dr["mappos"].ToString().Split(',')[0];
                        string Y = dr["mappos"].ToString().Split(',')[1];
                        string subar = dr["mappos"].ToString().Split(',')[2];

                        allMap.Add(new mapStruc(mapId, dr["CreateTime"].ToString(), dr["Data"].ToString(), dr["DataKey"].ToString(),
                                        dr["mobs"].ToString(), dr["Width"].ToString(), dr["Height"].ToString(), X,
                                        Y, dr["fixSize"].ToString(), subar, (int)dr["maxMobs"], (string)dr["PosFight"], (int)dr["numgroup"]));
                    }
                }
                Reader.Close();
            }
        }

        public static void ChangeKey(Dictionary<string,string> data, string key)
        {
            var res = allMap.Find(x => x.Id == int.Parse(data["id"]));
            if (res != null)
            {
                return;
                res.DataKey = key;
                res.CreateTime = data["dateTime"];
                databaseChangeValue(res);
            }
            else
            {
                databaseCreateMap(data, key);
                Logger.Log("No row for " + data["id"]);
            }
        }
    }
}
