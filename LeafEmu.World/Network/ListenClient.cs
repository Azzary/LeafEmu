using System.Collections.Generic;
using System.Net.Sockets;

namespace LeafEmu.World.Network
{
    public class listenClient
    {

        //public event Action<string> packetReceivedEvent;
        public List<listenClient> CharacterInWorld;

        public Database.LoadDataBase database;
        public Socket ClientSocket { get; set; }
        public SocketAsyncEventArgs SocketAsyncEvent { get; set; }
        public LinkServer linkServer;
        public bool isCo = true;


        public Game.account.Account account = new Game.account.Account();


        private List<listenClient> queue;

        public listenClient(Socket _ClientSocket, Database.LoadDataBase _database, List<listenClient> _queue, LinkServer _linkServer, List<listenClient> _CharacterInWorld)
        {
            CharacterInWorld = _CharacterInWorld;
            linkServer = _linkServer;
            queue = _queue;
            database = _database;
            ClientSocket = _ClientSocket;

        }

        public void sendDebugMsg(string msg)
        {
            send($"Im00{msg}");
        }

        public void send(string packet)
        {
            WorldServer.Server.Send(ClientSocket, packet + "\0");

        }
        public void SendToAllFight(string packet)
        {
            if (account.character.CurrentFight != null)
            {
                account.character.CurrentFight.SendToAllFight(packet);
            }
        }

        public void SendToAllMap(string packet, bool SendToAllFight = false)
        {
            account.character.Map.SendToAllMap(packet, SendToAllFight);
        }

        public bool DbLoad = false;
        public bool IsLoadDb = false;

        public void remove(listenClient prmClient)
        {
            linkServer.RemoveAccount(account.ID, account.GUID);
            prmClient.isCo = false;

            if (account.character != null && account.character.Map != null)
            {
                if (prmClient.account.character.CurrentFight != null)
                {
                    prmClient.account.character.CurrentFight.QuitBattle(prmClient, false, true);
                }
                prmClient.account.character.Map.Remove(prmClient);

            }
        }

    }
}
