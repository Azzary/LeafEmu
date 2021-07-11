using LeafEmu.World.Network;
using LeafEmu.World.PacketGestion;
using System.Collections.Generic;
using System.Text;

namespace LeafEmu.World.Game.Fight
{
    class duel
    {

        [PacketAttribute("GA900")]
        public void RequetDuel(Network.listenClient prmClient, string prmPacket)
        {
            if (!(prmClient.account.character.State == EnumClientState.None) || prmClient.account.character.Map.PosFight == "")
            {
                return;
            }
            if (int.TryParse(prmPacket.Substring(5), out int id))
            {
                Map.Map map = prmClient.account.character.Map;
                for (int i = 0; i < map.CharactersOnMap.Count; i++)
                {
                    if (map.CharactersOnMap[i].account.character.id == id)
                    {
                        if (map.CharactersOnMap[i].account.character.State == EnumClientState.None)
                        {
                            prmClient.account.character.State = EnumClientState.OnRequestChallenge;
                            map.CharactersOnMap[i].account.character.State = EnumClientState.OnRequestChallenge;
                            prmClient.account.character.FightInfo.RequestDuel = map.CharactersOnMap[i];
                            map.CharactersOnMap[i].account.character.FightInfo.RequestDuel = prmClient;
                            prmClient.send($"GA;900;{prmClient.account.character.id};{id}");
                            map.CharactersOnMap[i].send($"GA;900;{prmClient.account.character.id};{id}");
                        }
                        return;
                    }
                }
            }
        }


        [PacketAttribute("GA902")]
        public void CancelRequetDuel(Network.listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.FightInfo.RequestDuel == null)
            {
                return;
            }

            Network.listenClient temp = prmClient.account.character.FightInfo.RequestDuel;

            prmClient.send($"GA;902;{prmClient.account.character.id};{temp.account.character.id}");
            temp.send($"GA;902;{temp.account.character.id};{prmClient.account.character.id}");

            prmClient.account.character.FightInfo.IsLauncherDuel = false;
            prmClient.account.character.State = EnumClientState.None;
            temp.account.character.State = EnumClientState.None;
            prmClient.account.character.FightInfo.RequestDuel = null;
            temp.account.character.FightInfo.RequestDuel = null;
        }

        [PacketAttribute("GA901")]
        public void AcceptRequetDuel(Network.listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.FightInfo.RequestDuel == null || prmClient.account.character.FightInfo.IsLauncherDuel)
            {
                return;
            }
            prmClient.account.character.FightInfo.IsLauncherDuel = true;
            Network.listenClient ennemie = prmClient.account.character.FightInfo.RequestDuel;
            ennemie.account.character.FightInfo.InFight = 1;
            prmClient.account.character.FightInfo.InFight = 1;
            string packet = $"GA;901;";
            ennemie.send($"{packet}{ennemie.account.character.id};{prmClient.account.character.id}");
            prmClient.send($"{packet}{ennemie.account.character.id};{prmClient.account.character.id}");

            LauchDuel(prmClient, ennemie);

        }





        //refonte in GestionFight.cs
        public static void LauchDuel(listenClient prmClient, listenClient ennemie, Fight JoinFight = null)
        {
            List<listenClient> PlayersList;
            //Entity.Entity ennemie;

            return;

            if (ennemie != null)
            {
                PlayersList = new List<listenClient>() { ennemie, prmClient };
                Fight fight = new Fight(PlayersList, FightTypeEnum.Challenge);
                prmClient.account.character.CurrentFight = fight;
                ennemie.account.character.CurrentFight = fight;
                prmClient.account.character.FightInfo.equipeID = 0;
                ennemie.account.character.FightInfo.equipeID = 1;

                int keys = GestionFight.GetId(prmClient.account.character.Map.FightInMap);
                prmClient.account.character.Map.FightInMap.Add(fight);
                prmClient.account.character.FightInfo.FightID = ennemie.account.character.FightInfo.FightID = keys;
                ennemie.account.character.FightInfo.AtributePosFight(ennemie, ennemie.account.character);
            }
            else
            {
                JoinFight.PlayerInFight.Add(prmClient);
                prmClient.account.character.CurrentFight = JoinFight;
            }


            PlayersList = prmClient.account.character.CurrentFight.PlayerInFight;

            for (int i = 0; i < prmClient.account.character.Map.CharactersOnMap.Count; i++)
            {
                prmClient.account.character.Map.CharactersOnMap[i].send($"GM|-{prmClient.account.character.id}");
                if (ennemie != null)
                {
                    prmClient.account.character.Map.CharactersOnMap[i].send($"GM|-{ennemie.account.character.id}");
                    prmClient.account.character.Map.CharactersOnMap[i].send($"Gc+{prmClient.account.character.FightInfo.FightID};0|{prmClient.account.character.id};{prmClient.account.character.cellID};0;-1|{ennemie.account.character.id};{ennemie.account.character.cellID};0;-1");
                }
            }
            int y = prmClient.account.character.FightInfo.FightID;
            for (int x = 0; x < prmClient.account.character.Map.FightInMap[y].PlayerInFight.Count; x++)
            {
                Entity.Character character = prmClient.account.character.Map.FightInMap[y].PlayerInFight[x].account.character;
                prmClient.account.character.Map.FightInMap[y].PlayerInFight[x].send($"Gt{prmClient.account.character.id}|+{prmClient.account.character.id};{prmClient.account.character.speudo};{prmClient.account.character.level}");
                prmClient.send($"Gt{character.id}|+{character.id};{character.speudo};{character.level}");
            }
            prmClient.send("fC1");

            StringBuilder str = new StringBuilder();
            prmClient.account.character.FightInfo.AtributePosFight(prmClient, prmClient.account.character);

            foreach (listenClient Entity in PlayersList)
            {
                Entity.Character character = Entity.account.character;
                Logger.Logger.Log(character.FightInfo.FightCell);
                character.UpdateEquipentStats();
                Map.MapGestion.GmPacket(character);
            }

            foreach (listenClient Entity in PlayersList)
            {
                Entity.send($"GJK2|1|1|0|0|0\0GP{prmClient.account.character.Map.PosFight}|{Entity.account.character.FightInfo.equipeID}\0ILF0\0GM" + str);
                //foreach (listenClient Entity2 in PlayersList)
                //{
                //    Entity.send(Item.MoveItem.GetItemsPos(Entity2.account.character));
                //}
            }

        }


    }


}
