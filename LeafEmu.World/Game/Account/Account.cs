using LeafEmu.World.Network;
using LeafEmu.World.PacketGestion;
using System.Collections.Generic;


namespace LeafEmu.World.Game.account
{
    public class Account
    {
        public int statue = 0;

        public short Role { get; set; }
        public int ID { get; set; }
        public List<Game.Entity.Character> ListCharacter = new List<Game.Entity.Character>();

        public List<Game.Entity.Character> ListRemoveCharacter = new List<Game.Entity.Character>();

        public Game.Entity.Character character;
        public string GUID { get; set; }


        [PacketAttribute("AT")]
        public void LoginInWorld(Network.listenClient prmClient, string prmPacket)
        {
            prmClient.account.GUID = prmPacket.Substring(2, 7);
            string[] InfoAcc = null;
            if (prmClient.linkServer.ListOfGUID.Count != 0)
            {
                lock (prmClient.linkServer.ListOfGUID)
                {
                    InfoAcc = prmClient.linkServer.ListOfGUID.Find(x => x != null && x[1] == prmClient.account.GUID);
                    prmClient.linkServer.ListOfGUID.RemoveAll(x => x[1] == prmClient.account.GUID);
                }
            }
            if (InfoAcc == null)
            {
                prmClient.ClientSocket.Close();
                prmClient.isCo = false;
                return;
            }
            prmClient.account.ID = int.Parse(InfoAcc[0]);
            prmClient.account.Role = short.Parse(InfoAcc[2]);
            prmClient.linkServer.addAccount(prmClient.account.ID);
            WorldServer.AddToQueue(prmClient);



        }

        public Account()
        {
            ID = -1;
        }
    }
}
