using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafEmu.World.Database.table
{
    public class ZaapInstance
    {
        public int CellZaap { get; set; }
        public int CellTp { get; set; }

        public ZaapInstance(int tp, int zaap)
        {
            CellZaap = zaap;
            CellTp = tp;
        }
    }

    public class ZaapiInstance
    {
        public int CellZaapi { get; set; }
        public int CellTp { get; set; }
        public int align { get; set; }

        public ZaapiInstance(int tp, int zaap, int _align)
        {
            align = _align;
            CellZaapi = zaap;
            CellTp = tp;
        }
    }

    public class Zaap
    {

        static MySqlConnection conn;

        public static Dictionary<int, ZaapInstance> zaaps = new Dictionary<int, ZaapInstance>();
        public static Dictionary<int, ZaapiInstance> zaapis = new Dictionary<int, ZaapiInstance>();

        public Zaap(MySqlConnection _conn)
        {
            conn = _conn;

            MySqlCommand Create_table = new MySqlCommand(@"
            CREATE TABLE IF NOT EXISTS `zaaps`
            (`mapID` int NOT NULL,
            `cellID` int NOT NULL,
            `ZaapCellID` int NOT NULL,
            PRIMARY KEY (`mapID`))", conn);
            Create_table.ExecuteNonQuery();

            Create_table = new MySqlCommand(@"
            CREATE TABLE IF NOT EXISTS `zaapi`
            (`mapid` int NOT NULL,
            `align` int NOT NULL,
            `cellID` int NOT NULL,
            `ZaapiCellID` int NOT NULL,
            PRIMARY KEY (`mapid`))", conn);

            Create_table = new MySqlCommand(@"
            CREATE TABLE IF NOT EXISTS `zaaps_personnage`
            (`id_personnage` int NOT NULL,
            `ZaapMapID` text,
            PRIMARY KEY (`id_personnage`))", conn);
            Create_table.ExecuteNonQuery();

            LoadZaaps();
            LoadZaapi();
        }

        public static void LoadZaapsCharacter(Network.listenClient PrmClient)
        {
            foreach (var character in PrmClient.account.ListCharacter)
            {
                string r = $"SELECT ZaapMapID from zaaps_personnage where id_personnage = {character.id};";

                using (MySqlCommand commande = new MySqlCommand(r, conn))
                {
                    MySqlDataReader Reader = commande.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        Reader.Read();
                        foreach (var item in Reader["ZaapMapID"].ToString().Split(','))
                        {
                            if (Int32.TryParse(item, out int mapid))
                            {
                                character.Zaaps.Add(mapid);
                            }
                        }
                    }
                    Reader.Close();
                }
            }
        }

        public static void SetZaapsCharacter(Game.Entity.Character character)
        {
            del(character);
            add(character);
        }


        static void add(Game.Entity.Character character)
        {
            string r = string.Empty;
            character.Zaaps.ForEach(x => r += $"{x},");
            if (r == string.Empty)
                return;
            r = "INSERT INTO zaaps_personnage SET id_personnage=" + character.id + ", ZaapMapID= '" + r + "';";
            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {

                MySqlDataReader Reader = commande.ExecuteReader();
                Reader.Close();
            }
        }
        public static void del(Game.Entity.Character character)
        {
            string r = "DELETE from zaaps_personnage WHERE id_personnage = " + character.id + ";";
            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                Reader.Close();
            }
        }

        private void LoadZaaps()
        {
            string r = "SELECT mapID, cellID, ZaapCellID from zaaps;";

            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        if (Map.Maps.ContainsKey((int)Reader["mapID"]))
                        {
                            Map.Maps[(int)Reader["mapID"]].HaveZaap = true;
                            zaaps.Add((int)Reader["mapID"], new ZaapInstance((int)Reader["cellID"], (int)Reader["ZaapCellID"]));
                        }
                        else
                            Logger.Logger.Debug($"No map for Zaap in mapid={(int)Reader["mapID"]}:{(int)Reader["cellID"]}");
                    }
                }
                Reader.Close();
            }
        }

        private void LoadZaapi()
        {
            string r = "SELECT mapid, align, cellID, ZaapiCellID from zaapi;";

            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        if (Map.Maps.ContainsKey((int)Reader["mapid"]))
                        {
                            Map.Maps[(int)Reader["mapID"]].HaveZaapi = true;
                            zaapis.Add((int)Reader["mapID"], new ZaapiInstance((int)Reader["cellID"], (int)Reader["ZaapiCellID"], (int)Reader["align"]));
                        }
                        else
                            Logger.Logger.Debug($"No map for Zaap in mapid={(int)Reader["mapid"]}:{(int)Reader["cellID"]}");
                    }
                }
                Reader.Close();
            }
        }
    }
}
