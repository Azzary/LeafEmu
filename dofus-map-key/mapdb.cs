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
            string r1 = "DELETE from maptemplate WHERE" +
            " Id='" + map.Id + "';";

            string r2 = "INSERT INTO maptemplate SET Id=" + map.Id + "," +
            "CreateTime='" + map.CreateTime + "'," +
            "Width=" + map.Width + "," +
            "Height=" + map.Height + "," +
            "PosFight='" + map.PosFight + "'," +
            "DataKey='" + map.DataKey + "'," +
            "Data='" + map.Data + "'," +
            "cells= '" + map.cell + "'," +
            "mobs='" + map.mob + "'," +
            "X=" + map.X + "," +
            "Y=" + map.Y + "," +
            "subArea=" + map.SubArea + "," +
            "nroGrupo=" + map.NbGroups + "," +
            "maxMobs=" + map.MaxMobs + "," +
            "Capaciter=" + map.Capabilities + "," +
            "descripcion= 0 ;";

            using (MySqlCommand commande = new MySqlCommand(r1, conn))
            {

                MySqlDataReader Reader = commande.ExecuteReader();
                Reader.Close();
            }
            using (MySqlCommand commande = new MySqlCommand(r2, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                Reader.Close();
            }
        }
        public static void loadMap(MySqlConnection _conn)
        {
            conn = _conn;
            string r = "SELECT Id, CreateTime, Data, DataKey," +
                "mobs, Width, Height, X, Y, Capaciter, cells, maxMobs, PosFight, nroGrupo," +
                "subArea from maptemplate_copy1;";

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
                        allMap.Add(new mapStruc(dr["Id"].ToString(), dr["CreateTime"].ToString(), dr["Data"].ToString(), dr["DataKey"].ToString(),
                           dr["mobs"].ToString(), dr["Width"].ToString(), dr["Height"].ToString(), dr["X"].ToString(),
                           dr["Y"].ToString(), dr["Capaciter"].ToString(), dr["subArea"].ToString(), (int)dr["maxMobs"], (string)dr["PosFight"], (int)dr["nroGrupo"], (string)dr["PosFight"]));
                    }
                }
                Reader.Close();
            }
        }

        public static void changeKey(int id, string dateTime, string key)
        {
            var res = allMap.Find(x => x.Id == id);
            if (res != null)
            {
                res.DataKey = key;
                res.CreateTime = dateTime;
                databaseChangeValue(res);
            }
            else
            {
                Logger.Log("No row for " + id);
            }
        }
    }
}
