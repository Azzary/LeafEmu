using LeafEmu.Auth.Util;
using MySql.Data.MySqlClient;

namespace LeafEmu.Auth.Database.table
{
    class account
    {
        /// <summary>
        /// Create account test 
        /// </summary>
        public void createaccount(int nbCompte)
        {
            for (int i = 0; i < nbCompte; i++)
            {
                Logger.Logger.Log(i);
                string cmd = "INSERT INTO account SET " +
                    $"username = 'test{i}'," +
                    $"password = 'test'," +
                    $"speudo = 'test'";
                using (MySqlCommand commande = new MySqlCommand(cmd, conn))
                {
                    MySqlDataReader Reader = commande.ExecuteReader();
                    Reader.Close();
                }
            }


            return;
        }
        MySqlConnection conn;
        public account(MySqlConnection _conn)
        {
            conn = _conn;

            MySqlCommand Create_table = new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS `account` 
                    (id INT NOT NULL AUTO_INCREMENT,
                    username VARCHAR(20) NOT NULL,
                    password VARCHAR(20) NOT NULL,
                    speudo TEXT(20) NOT NULL,
                    isBan BOOL DEFAULT 0,
                    role INT DEFAULT 0,
                    Qsecrete VARCHAR(20) DEFAULT 'dummy?',
                    Rsecrete VARCHAR(20) DEFAULT 'dummy',
                    PRIMARY KEY (id)) ", conn);
            Create_table.ExecuteNonQuery();
        }

        public bool VerifAccount(Network.listenClient prmClient)
        {

            string r = "SELECT id, username, password, speudo," +
                " isBan, role, Qsecrete, Rsecrete from account where" +
                " username='" + prmClient.account.UserName + "';";
            using (MySqlCommand commande = new MySqlCommand(r, conn))
            {
                MySqlDataReader Reader = commande.ExecuteReader();
                Reader.Read();
                if (Reader.HasRows)
                {
                    if ((bool)Reader["isBan"])
                    {
                        prmClient.send("AlEb");
                    }
                    else if (prmClient.account.HashPass == "#1" + hash.CryptPassword(prmClient.account.Key, (string)Reader["password"]))
                    {
                        if (prmClient.linkServer.ListIDAccount.Contains((int)Reader["id"]))
                        {
                            prmClient.send("AlEa");
                        }
                        else
                        {
                            prmClient.linkServer.ListIDAccount.Add((int)Reader["id"]);
                            prmClient.account.ID = (int)Reader["id"];
                            prmClient.account.role = (int)Reader["role"];
                            prmClient.account.Qsecret = (string)Reader["Qsecrete"];
                            prmClient.account.Rsecret = (string)Reader["Rsecrete"];
                            Reader.Close();
                            return true;
                        }

                    }
                    else
                    {
                        prmClient.send("AlEf");
                    }

                }
                Reader.Close();
                return false;
            }
        }

    }


}
