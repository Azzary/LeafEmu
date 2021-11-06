using MySql.Data.MySqlClient;

namespace LeafEmu.StaticDataBase
{
    public static class GetMySqlConnection
    {
        public static MySqlConnection GetAuthMySqlConnection()
        {
            string connStr = "server = localhost; user = root; database = leafauth;";
            return new MySqlConnection(connStr);
        }

        public static MySqlConnection GetWorldMySqlConnection(int idServer = 602)
        {
            string connStr = $"server = localhost; user = root; database = leafworld{idServer};";
            return new MySqlConnection(connStr);
        }
    }
}
