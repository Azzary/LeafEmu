﻿using LeafEmu.World.Network;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LeafEmu.World.Database.table.Character
{
    public class Character
    {
        MySqlConnection conn;
        public Character(MySqlConnection _conn)
        {
            conn = _conn;

            MySqlCommand Create_table = new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS `personnage` 
                    (id INT NOT NULL AUTO_INCREMENT,
                    accountid int NOT NULL,
                    level smallint DEFAULT 1,
                    classe int NOT NULL,
                    sexe int NOT NULL,
                    speudo TEXT(20) NOT NULL,
                    couleur1 int NOT NULL,
                    couleur2 int NOT NULL,
                    couleur3 int NOT NULL,
                    pods int NOT NULL DEFAULT 0,
                    gfxID int NOT NULL,
                    isDead int DEFAULT 0,
                    SubArea int DEFAULT 7669,
                    mapID int DEFAULT 10295,
                    cellID int DEFAULT 299,
                    XP bigint DEFAULT 0,
                    kamas int DEFAULT 1000,
                    capital int DEFAULT 0,
                    PSorts int DEFAULT 0,
                    vie int DEFAULT 50,
                    energie int DEFAULT 10000,
                    PA int DEFAULT 6,
                    PM int DEFAULT 3,
                    forcee int DEFAULT 0,
                    sagesse int DEFAULT 0,
                    chance int DEFAULT 0,
                    agi int DEFAULT 0,
                    intell int DEFAULT 0,
                    podsMax int NOT NULL DEFAULT 1000,
                    PRIMARY KEY (id)) ", conn);
            Create_table.ExecuteNonQuery();
        }

        private void CreateCharacter(int accountID, Game.Entity.Character charac)
        {
            string cmd = "INSERT INTO personnage SET ";
            if (!charac.newCharac)
            {
                cmd += "id=" + charac.id + ",";
            }

            cmd += "accountid=" + accountID + "," +
                "speudo='" + charac.speudo + "'," +
                "level='" + charac.level + "'," +
                "classe=" + charac.classe + "," +
                "sexe=" + charac.sexe + "," +
                "couleur1=" + charac.couleur1 + "," +
                "couleur2=" + charac.couleur2 + "," +
                "couleur3=" + charac.couleur3 + "," +
                "gfxID=" + charac.gfxID + "," +
                "cellID=" + charac.cellID + "," +
                "mapID=" + charac.mapID + "," +
                "pods=" + charac.pods + "," +
                "podsMax=" + charac.podsMax + "," +
                "XP=" + charac.XP + "," +
                "kamas=" + charac.kamas + "," +
                "capital=" + charac.capital + "," +
                "PSorts=" + charac.PSorts + "," +
                "vie=" + charac.CaracVie + "," +
                "PA=" + charac.PA + "," +
                "PM=" + charac.PM + "," +
                "energie=" + charac.energie + "," +
                "forcee=" + charac.Force + "," +
                "sagesse=" + charac.Sagesse + "," +
                "chance=" + charac.Chance + "," +
                "agi=" + charac.Agi + "," +
                "intell=" + charac.Intell + "," +
                $"SpawnPoint = '{charac.MapSpawnPoint},{charac.CellSpawnPoint}'";

            MySqlCommand command = new MySqlCommand(cmd, conn);
            command.ExecuteNonQuery();

        }

        public void LoadCharacter(listenClient prmClient)
        {
            string r = "SELECT id, speudo, level, gfxID, classe," +
            " couleur1, couleur2, couleur3, isDead, mapID, cellID," +
            "sexe, pods, podsMax, XP, PA,PM, kamas, capital, PSorts," +
            " vie, forcee, sagesse, chance, agi, intell, energie, SpawnPoint " +
            "from personnage where accountid='" + prmClient.account.ID + "';";

            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();

                if (Reader.HasRows)
                {
                    bool flag = false;
                    while (Reader.Read())
                    {
                        foreach (Game.Entity.Character item in prmClient.account.ListCharacter)
                        {
                            if (item.id == (int)Reader["id"])
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            Game.Entity.Character character = new Game.Entity.Character((int)Reader["id"], (string)Reader["speudo"], (short)Reader["level"],
                               (int)Reader["isDead"], (int)Reader["gfxID"], (int)Reader["cellID"], (int)Reader["mapID"], (int)Reader["couleur1"],
                               (int)Reader["couleur2"], (int)Reader["couleur3"], (int)Reader["sexe"], Convert.ToByte((int)Reader["classe"]), (int)Reader["pods"], (int)Reader["podsMax"],
                               (Int64)Reader["XP"], (int)Reader["kamas"], (int)Reader["capital"], (int)Reader["PSorts"], (int)Reader["vie"], (int)Reader["energie"], (int)Reader["PA"],
                               (int)Reader["PM"], (int)Reader["forcee"], (int)Reader["sagesse"], (int)Reader["chance"], (int)Reader["agi"], (int)Reader["intell"], false, Reader["SpawnPoint"].ToString());

                            prmClient.account.ListCharacter.Add(character);
                        }

                    }
                }

                Reader.Close();
                prmClient.database.tableCharacterSpells.LoadSpells(prmClient);
                prmClient.database.tableCharacterItems.LoadItems(prmClient);

            }
        }


        private void removeSpells(int id)
        {
            string r = "DELETE from characterSpells WHERE" +
             " characterID=" + id + ";";
            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                Reader.Close();
            }

        }

        private void AddSpells(int id, List<Game.Spells.SpellsEntity> Spells)
        {
            string r = "";
            foreach (Game.Spells.SpellsEntity item in Spells)
            {
                r += "INSERT INTO characterSpells SET characterID=" + id + "," +
                "id=" + item.id + "," +
                "level=" + item.level + ";";

            }
            if (r == "")
                return;


            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {

                MySqlDataReader Reader = commande.ExecuteReader();
                Reader.Close();
            }

        }

        public void updateDatabase(listenClient prmClient)
        {
            foreach (Game.Entity.Character item in prmClient.account.ListRemoveCharacter)
            {
                removeCharacter(item.id.ToString());
                removeSpells(item.id);
                removeItems(item.id);
            }

            foreach (Game.Entity.Character item in prmClient.account.ListCharacter)
            {
                removeCharacter(item.id.ToString());
                removeSpells(item.id);
                removeItems(item.id);
                CreateCharacter(prmClient.account.ID, item);
                AddSpells(item.id, item.Spells);
                UpdateItems(item.id, item.Invertaire.Stuff);
            }
        }

        private void removeItems(int id)
        {
            string r = "DELETE from items_personnage WHERE" +
             " personnageid = " + id + ";";
            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                Reader.Close();
            }

        }



        private void UpdateItems(int id, List<Game.Item.Stuff> Inv)
        {
            Game.Item.Stuff.RangerItem(ref Inv);
            string r = "";
            foreach (var item in Inv)
            {
                r += $"INSERT INTO items_personnage SET personnageid= {id}, uid= {item.UID} ,modelid= {item.Template.ID}, stats= '{item.getStatForDb()}', objectif= 0 , price = 0 , position = {item.Position}, quantity = {item.Quantity};";
            }
            if (r == "")
                return;


            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                Reader.Close();
            }

        }

        public void removeCharacter(string id)
        {
            string r = "DELETE from personnage WHERE" +
            " id='" + id + "';";
            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                Reader.Close();
            }
        }
    }

}
