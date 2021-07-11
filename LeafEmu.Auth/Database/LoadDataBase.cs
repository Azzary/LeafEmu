
using MySql.Data.MySqlClient;


namespace LeafEmu.Auth.Database
{
    class LoadDataBase
    {
        public table.account tableaccount;
        public table.server tableserver;
        private MySqlConnection conn;

        public LoadDataBase()
        {
            conn = DataBase.GetMySqlConnection.GetAuthMySqlConnection();
            Logger.Logger.Log("Opening Connection Database");
            conn.Open();
            Logger.Logger.Log("load Table:");

            Logger.Logger.Log("           Account...");
            tableaccount = new table.account(conn);
            Logger.Logger.Log("           Server...");
            tableserver = new table.server(conn);
        }



    }
}
