namespace LeafEmu.Auth

{
    class Program
    {
        static void Main(string[] args)
        {

            Database.LoadDataBase DataBase = new Database.LoadDataBase();
            PacketGestion.PacketGestion.init();
            Network.AuthServer AuthServ = new Network.AuthServer(DataBase);
        }
    }
}
