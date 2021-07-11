using LeafEmu.World.Game.Entity;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace LeafEmu.World.Database.table
{
    public class Donjons
    {
        public static Dictionary<int, Game.Entity.MobGroup> Donjon = new Dictionary<int, Game.Entity.MobGroup>();

        MySqlConnection conn;
        public Donjons(MySqlConnection _conn)
        {
            conn = _conn;
            MySqlCommand Create_table = new MySqlCommand(@"
              CREATE TABLE IF NOT EXISTS `mobgroups_fix` (
              `mapid` int(11) NOT NULL,
              `cellid` int(11) NOT NULL,
              `groupData` varchar(200) NOT NULL,
              `Donjon` varchar(200) NOT NULL,
              `Salle` varchar(200) NOT NULL,
              `Timer` int(200) NOT NULL DEFAULT 30000)", conn);

            Create_table.ExecuteNonQuery();
            LoadDonjons();
        }

        private void LoadDonjons()
        {
            string r = "SELECT mapid, cellid, groupData, Donjon, Salle, Timer from mobgroups_fix;";

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
                        if (!Donjon.ContainsKey((int)dr["mapid"]))
                        {
                            Donjon.Add((int)dr["mapid"], MobGroup.CreateMobGroupe(-i, dr["groupData"].ToString(), (int)dr["cellid"], ';'));
                        }
                    }

                }
                Reader.Close();
            }
        }

    }
}
