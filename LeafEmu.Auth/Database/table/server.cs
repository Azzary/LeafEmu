﻿using LeafEmu.Auth.Network;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace LeafEmu.Auth.Database.table
{
    class server
    {
        MySqlConnection conn;
        MySqlConnection conn_world;
        public server(MySqlConnection _conn)
        {
            conn = _conn;
            conn_world = StaticDataBase.GetMySqlConnection.GetWorldMySqlConnection();
            conn_world.Open();
            MySqlCommand Create_table = new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS `server` 
                    (id INT NOT NULL AUTO_INCREMENT,
                    ip VARCHAR(20) NOT NULL,
                    port INT NOT NULL,
                    PRIMARY KEY (id)) ", conn);
            Create_table.ExecuteNonQuery();
        }

        public void getServer(listenClient prmClient)
        {
            string r = "SELECT id from server;";

            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                prmClient.account.ListServ = new List<int>();
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        int idServ = (int)Reader[0];
                        prmClient.account.ListServ.Add(idServ);
                        prmClient.account.PacketAH += $"|{idServ};1;10;1";
                    }
                }
                Reader.Close();
            }
        }


        public void getCharacOnServer(listenClient prmClient)
        {

            string r = "SELECT accountID, ID from personnage where" +
                " accountID='" + prmClient.account.ID + "';";
            foreach (int idServ in prmClient.account.ListServ)
            {
                string connStr = $"server = localhost; user = root; database = leafworld{idServ};";
                using (MySqlCommand commande = new MySqlCommand(r, conn_world))
                {

                    MySqlDataReader Reader = commande.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        int nb_perso = 0;
                        while (Reader.Read())
                        {
                            nb_perso++;
                        }

                        for (int i = 0; i < nb_perso; i++)
                        {
                            prmClient.account.PacketAx += $"|{idServ},{nb_perso}";
                        }
                    }

                    Reader.Close();
                }

            }
        }

    }
}
