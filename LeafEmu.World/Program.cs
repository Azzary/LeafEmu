using System;
using System.IO;

namespace LeafEmu.World
{
    class Program
    {
        static Network.WorldServer WorldServ;
        static Database.LoadDataBase database;
        static void Main(string[] args)
        {
            Util.PrintLogoLeafColor();
            database = new Database.LoadDataBase();


            //SynMap(database);
            PacketGestion.PacketGestion.init();
            Game.Command.Command.init();
            Game.Spells.SpellsEffect.Gestion.EffectGestion.init();
            WorldServ = new Network.WorldServer(database);
        }

        public static void SynMap(Database.LoadDataBase database)
        {
            DirectoryInfo d = new DirectoryInfo(@"C:\wamp64\www\dofus\maps");
            FileInfo[] Files = d.GetFiles("*.swf"); //Getting Text files
            foreach (FileInfo file in Files)
            {

                if (Int32.TryParse(file.Name.Split("_")[0], out int res))
                {

                    if (Database.table.Map.Maps.ContainsKey(res))
                    {
                        if (Database.table.Map.Maps[res].CreateTime + ".swf" != file.Name.Split("_")[1] && file.Name.Substring(file.Name.Length - 5) == "X.swf")
                        {
                            bool x = File.Exists(file.Directory + $"\\{res}_{Database.table.Map.Maps[res].CreateTime}X.swf");
                            if (x == false)
                            {

                                try
                                {
                                    File.Move(file.FullName, file.Directory + $"\\{res}_{Database.table.Map.Maps[res].CreateTime}X.swf");
                                }
                                catch (Exception)
                                {
                                    Logger.Logger.Log(file.Name);
                                }

                            }

                        }


                    }
                }
            }
            Logger.Logger.Log("Check Map End");
            Console.ReadKey();
            return;
        }


    }
}


