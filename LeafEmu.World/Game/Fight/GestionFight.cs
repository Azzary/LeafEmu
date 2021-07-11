using LeafEmu.World.Network;
using LeafEmu.World.PacketGestion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LeafEmu.World.Game.Fight
{
    class GestionFight
    {

        [PacketAttribute("Gp")]
        public void MovingplacementCell(Network.listenClient prmClient, string prmPacket)
        {

            if (prmClient.account.character.FightInfo.InFight != 1)
            {
                return;
            }

            if (int.TryParse(prmPacket.Split("\0")[0].Substring(2), out int cellID))
            {
                if (prmClient.account.character.FightInfo.ListCellIDPlacement.Contains(cellID))
                {
                    for (int i = 0; i < prmClient.account.character.CurrentFight.PlayerInFight.Count; i++)
                    {
                        prmClient.account.character.CurrentFight.PlayerInFight[i].send($"GIC|{prmClient.account.character.id};{cellID}");
                        prmClient.account.character.FightInfo.FightCell = cellID;
                    }
                }
            }
        }

        [PacketAttribute("Gt")]
        public void PassTurn(listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.FightInfo.YourTurn)
                prmClient.account.character.CurrentFight.StopTurn.Cancel();
        }


        [PacketAttribute("GR1")]
        public void IsReady(Network.listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.FightInfo.InFight != 1 || prmClient.account.character.FightInfo.IsReady)
            {
                return;
            }
            prmClient.account.character.FightInfo.IsReady = true;
            prmClient.SendToAllFight($"GR1{prmClient.account.character.id}");
            bool allReady = !prmClient.account.character.CurrentFight.AllEntityInFight.Exists(x => x.IsHuman && !x.FightInfo.IsReady);
            if (allReady)
            {
                new Thread(prmClient.account.character.CurrentFight.start).Start();
            }

        }

        [PacketAttribute("GR0")]
        public void IsNotReady(listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.FightInfo.InFight != 1)
            {
                return;
            }
            prmClient.account.character.FightInfo.IsReady = false;
            for (int i = 0; i < prmClient.account.character.CurrentFight.PlayerInFight.Count; i++)
            {
                listenClient entity = prmClient.account.character.CurrentFight.PlayerInFight[i];
                entity.send($"GR0{prmClient.account.character.id}");
            }

        }

        [PacketAttribute("GA903")]
        public void JoinFight(listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.State == EnumClientState.None)
            {
                prmPacket = prmPacket.Substring(5);
                string test1 = prmPacket.Split("\0")[0].Split(';')[1].Substring(1);
                int.TryParse(test1, out int id);
                id = -id;
                string test2 = prmPacket.Split(";")[0].Substring(1);
                int.TryParse(test2, out int FightID);
                FightID = -FightID;
                Fight fight = prmClient.account.character.Map.FightInMap.Find(x => x.FightID == FightID);
                if (fight == null)
                {
                    return;
                }
                foreach (var entity in fight.PlayerInFight)
                {
                    if (entity.account.character.CurrentFight.InfoJoinConbat[0] == id)
                    {
                        prmClient.account.character.State = entity.account.character.State;
                        prmClient.account.character.FightInfo = new FightEntityInfo(entity.account.character.FightInfo.FightID, entity.account.character.FightInfo.equipeID);
                        prmClient.account.character.FightInfo.InFight = 1;
                        prmClient.account.character.CurrentFight = entity.account.character.CurrentFight;
                        prmClient.account.character.CurrentFight.Equipe[entity.account.character.FightInfo.equipeID].Add(prmClient.account.character);
                        prmClient.account.character.CurrentFight.PlayerInFight.Add(prmClient);
                        prmClient.SendToAllMap($"GM|-{prmClient.account.character.id}\0");
                        prmClient.account.character.FightInfo.AtributePosFight(prmClient, prmClient.account.character);
                        prmClient.send("fC1");
                        prmClient.send($"GJK2|1|1|0|0|0\0GP{prmClient.account.character.Map.PosFight}|{prmClient.account.character.FightInfo.equipeID}\0ILF0");
                        prmClient.send($"Gc-{prmClient.account.character.FightInfo.FightID}");

                        foreach (var item in prmClient.account.character.CurrentFight.AllEntityInFight)
                        {
                            prmClient.send("GM" + Map.MapGestion.GmPacket(item));
                        }
                        prmClient.account.character.CurrentFight.AllEntityInFight.Add(prmClient.account.character);
                        prmClient.SendToAllFight($"GM{Map.MapGestion.GmPacket(prmClient.account.character)}");
                        return;
                    }
                }
            }
        }

        public static void RemoveInvo(Entity.Entity entity)
        {
            foreach (var item in entity.FightInfo.EntityInvocation)
            {
                entity.CurrentFight.SendToAllFight("GM|-" + item.ID_InFight);
                entity.CurrentFight.AllEntityInFight.Remove(item);
            }
        }

        public static int GetId(List<Fight> FightInMap)
        {
            int keys = -1;
            for (int i = 0; i < FightInMap.Count; i++)
            {
                if (FightInMap[i].FightID == keys)
                {
                    keys--;
                    i = -1;
                }
            }
            return keys;
        }

        public static void LauchFight(listenClient prmClient, object _enemie, FightTypeEnum fightType, bool IsDonjon = false)
        {
            prmClient.account.character.CurrentFight = new Fight(new List<listenClient>() { prmClient }, fightType);
            lock (prmClient.account.character.CurrentFight)
            {
                prmClient.account.character.CurrentFight = new Fight(new List<listenClient>() { prmClient }, fightType);
                Fight fight = prmClient.account.character.CurrentFight;
                int keys = GestionFight.GetId(prmClient.account.character.Map.FightInMap);
                prmClient.account.character.CurrentFight.FightID = keys;
                if (prmClient.account.character.Map.FightInMap.Contains(prmClient.account.character.CurrentFight))
                    return;

                prmClient.account.character.Map.FightInMap.Add(prmClient.account.character.CurrentFight);
                prmClient.account.character.FightInfo = new FightEntityInfo(keys, 0);
                switch (fightType)
                {
                    case FightTypeEnum.Challenge:
                    case FightTypeEnum.Agression:
                        break;
                    case FightTypeEnum.PvM:
                        Entity.MobGroup Mobs = (Entity.MobGroup)_enemie;
                        if (IsDonjon)
                        {
                            Database.table.Donjons.Donjon[prmClient.account.character.mapID] = Entity.MobGroup.CreateMobGroupe(Mobs.ID, Mobs.GroupData,
                                Entity.MobGroup.GetRngCellWalkable(prmClient.account.character.Map.Cells), ';');
                        }
                        else
                        {
                            prmClient.account.character.Map.MobInMap.Remove(Mobs);
                            prmClient.account.character.Map.MobInMap.Add(Entity.MobGroup.CreateMobGroupe(Mobs.ID, Mobs.GroupData,
                                Entity.MobGroup.GetRngCellWalkable(prmClient.account.character.Map.Cells), '|'));
                        }
                        prmClient.account.character.CurrentFight.Stars = Mobs.Stars;
                        Mobs.Mobs.ForEach(x =>
                        {
                            x.AI = x.TypeAI == Entity.EnumTypeAI.Cac ? new Entity.AI.CacAI(x) : new Entity.AI.FearfulAI(x);
                            x.FightInfo = new FightEntityInfo(keys, 1);
                            prmClient.account.character.CurrentFight.AllEntityInFight.Add(x);
                        });
                        fight.InfoJoinConbat = new int[] { -prmClient.account.character.id, prmClient.account.character.cellID, keys, keys, keys };
                        prmClient.SendToAllMap($"GM|-{prmClient.account.character.id}\0" +
                        $"GM|-{Mobs.ID}\0" +
                        $"Gc+{fight.InfoJoinConbat[4]};0|{fight.InfoJoinConbat[0]};{fight.InfoJoinConbat[1]};0;-1|{fight.InfoJoinConbat[2]};{fight.InfoJoinConbat[3]};0;-1\0");

                        break;
                    default:
                        return;
                }
                prmClient.account.character.CurrentFight.AllEntityInFight.Add(prmClient.account.character);
                StringBuilder str = new StringBuilder();
                StringBuilder ItemPacket = new StringBuilder();
                StringBuilder strGmPacket = new StringBuilder("GM");
                foreach (Entity.Entity entity in prmClient.account.character.CurrentFight.AllEntityInFight)
                {
                    entity.CurrentFight = prmClient.account.character.CurrentFight;
                    prmClient.account.character.CurrentFight.Equipe[entity.FightInfo.equipeID].Add(entity);
                    entity.FightInfo.AtributePosFight(prmClient, entity);
                    str.Append($"Gt{entity.id}|+{-5};{entity.speudo};{entity.level}\0");
                    strGmPacket.Append(Map.MapGestion.GmPacket(entity));
                    ItemPacket.Append($"{Item.MoveItem.GetItemsPos(entity, true)}\0");
                }
                prmClient.send("fC1");
                prmClient.SendToAllFight(str.ToString() + "\0" +
                    $"GJK2|1|1|0|0|0\0GP{prmClient.account.character.Map.PosFight}|{prmClient.account.character.FightInfo.equipeID}\0ILF0\0"
                    + strGmPacket.ToString() + "\0" + ItemPacket.ToString());


            }
        }


        [PacketAttribute("GQ")]
        public void GiveUp(listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.CurrentFight == null)
            {
                return;
            }
            prmClient.account.character.CurrentFight.QuitBattle(prmClient, true, true);

        }


        public static void EndFightResult(Fight CurrentFight)
        {
            StringBuilder packet = new StringBuilder();
            packet.Append("GE");
            long elapsedTimeFight = Environment.TickCount - 1;
            packet.Append(elapsedTimeFight + "|" + CurrentFight.FightID + "|" + (CurrentFight.FightType == FightTypeEnum.Agression ? "1" : "0"));
            CurrentFight.GenerateDrops();
            foreach (Entity.Entity infoEntity in CurrentFight.AllEntityInFight)
            {
                bool winner = infoEntity.CurrentFight.WinerTeam.Contains(infoEntity);

                packet.Append("|" + (winner ? "2" : "0"));
                packet.Append(";" + infoEntity.id);
                packet.Append(";" + infoEntity.speudo);
                packet.Append(";" + infoEntity.level);
                packet.Append(";" + (infoEntity.FightInfo.isDead ? "1" : "0"));

                switch (CurrentFight.FightType)
                {
                    #region Challenge

                    case FightTypeEnum.Challenge:
                        int earnedExperience = 0;
                        int earnedKamas = 0;
                        var playerBis = (Entity.Character)infoEntity;
                        packet.Append(";" + Database.table.Experience.ExperienceStat[playerBis.level][0]);
                        packet.Append(";" + playerBis.XP);
                        packet.Append(";-1");

                        packet.Append(";" + earnedExperience);
                        packet.Append(";");
                        packet.Append(";");
                        packet.Append(";");
                        packet.Append(";" + earnedKamas);
                        break;

                    #endregion

                    #region PVM

                    case FightTypeEnum.PvM:
                        Int64 earnedPvmExperience = 0;
                        int earnedPvmKamas = 0;

                        if (winner)
                        {
                            foreach (Entity.Entity entity in CurrentFight.LoosersTeam)
                            {
                                if (!entity.IsHuman)
                                {
                                    Entity.Mob mob = (Entity.Mob)entity;
                                    earnedPvmKamas += mob.GetKamasWin();
                                }
                            }
                            if (infoEntity.IsHuman)
                            {
                                var player = (Entity.Character)infoEntity;
                                player.kamas += earnedPvmKamas;
                                earnedPvmExperience = FormuleExpPvm(player);
                                player.AddXp(earnedPvmExperience, CurrentFight.PlayerInFight.Find(x => x.account.character.id == player.id));
                            }

                        }

                        if (infoEntity.IsHuman)
                        {
                            var player = (Entity.Character)infoEntity;
                            packet.Append(";" + Database.table.Experience.ExperienceStat[player.level][0]);
                            packet.Append(";" + player.XP);
                            if (Database.table.Experience.ExperienceStat.ContainsKey(player.level))
                            {
                                packet.Append(";" + Database.table.Experience.ExperienceStat[player.level][0]);
                            }
                            else
                            {
                                packet.Append(";-1");
                            }
                            packet.Append(";" + earnedPvmExperience);
                        }
                        else
                        {
                            packet.Append(";;;;");
                        }
                        packet.Append(";");
                        packet.Append(";");
                        packet.Append(";" + infoEntity.FightInfo.ParseDrop);
                        packet.Append(";" + earnedPvmKamas);
                        break;

                    #endregion

                    #region Aggresion

                    case FightTypeEnum.Agression:
                        //int earnedHonor = (Utilities.ConfigurationManager.GetIntValue("RateHonor") * Utilities.ConfigurationManager.GetIntValue("BaseCoefHonor"))
                        //    * LoosersTeam.Fighters.FindAll(x => !x.IsInvoc).Count;

                        //if (WinTeam.IsFriendly(player))
                        //{
                        //    player.Character.Faction.AddExp(earnedHonor);
                        //}
                        //else
                        //{
                        //    player.Character.Faction.RemoveExp(earnedHonor);
                        //}

                        //packet.Append(";" + player.Character.Faction.Floor.Pvp);
                        //packet.Append(";" + player.Character.Faction.Honor);
                        //packet.Append(";" + (Helper.ExpFloorHelper.GetNextCharactersLevelFloor(player.Character.Faction.Power) != null ?
                        //    Helper.ExpFloorHelper.GetNextCharactersLevelFloor(player.Character.Faction.Power).Pvp.ToString() : "-1"));
                        //if (WinTeam.IsFriendly(player))
                        //{
                        //    packet.Append(";" + earnedHonor);
                        //}
                        //else
                        //{
                        //    packet.Append(";" + (-(earnedHonor)));
                        //}
                        //packet.Append(";" + player.Character.Faction.Power);
                        break;

                        #endregion
                }
            }

            CurrentFight.SendToAllFight(packet.ToString());



            /* //////// Ending build end packet //////// */

        }

        public static Int64 FormuleExpPvm(Entity.Entity player)
        {
            //https://www.dofus.com/fr/forum/1003-divers/1644516-formule-xp
            //Expérience gagnée par le joueur =
            //(1 + ((sagesse + (étoiles x 20) +bonus de challenges)/ 100)) 
            //x(coefficient multiplicateur + multiplicateur de niveau de groupe) 
            //x(expérience de groupe / nombre de joueurs dans le groupe)
            Int64 GroupExp = 0;
            int nbInPlayerTeam = player.CurrentFight.WinerTeam.Count;
            int nbInMobTeam = player.CurrentFight.LoosersTeam.Count;
            foreach (var item in player.CurrentFight.LoosersTeam.FindAll(x => !x.IsHuman && !x.IsInvocation))
            {
                Entity.Mob mob = (Entity.Mob)item;
                GroupExp += mob.XP_Value;
            }

            return Convert.ToInt64((1 + ((player.TotalSagesse + (player.CurrentFight.Stars * 20) + 0) / 100))
                * (GetCoefficientMultiplicateur(nbInPlayerTeam) + GetMultiplicateurDeNiveauDuGroupe(nbInPlayerTeam, nbInMobTeam))
                * (GroupExp / nbInPlayerTeam));
        }



        private static float GetCoefficientMultiplicateur(int NbPlyeur)
        {
            float coefMult = 0.5f;
            switch (NbPlyeur)
            {
                case 0:
                    coefMult = 0.5f;
                    break;
                case 1:
                    coefMult = 1;
                    break;
                case 2:
                    coefMult = 1.1f;
                    break;
                case 3:
                    coefMult = 1.5f;
                    break;
                case 4:
                    coefMult = 2.3f;
                    break;
                case 5:
                    coefMult = 3.1f;
                    break;
                case 6:
                    coefMult = 3.6f;
                    break;
                case 7:
                    coefMult = 4.2f;
                    break;
                case 8:
                default:
                    coefMult = 4.7f;
                    break;
            }
            return coefMult;
        }
        private static float GetMultiplicateurDeNiveauDuGroupe(int PlayerGroup, int MobGroup)
        {
            return PlayerGroup >= MobGroup * 0.9 && PlayerGroup <= MobGroup - 1.1 ? 1 : PlayerGroup / (MobGroup > PlayerGroup ? MobGroup : PlayerGroup + MobGroup);
        }


    }
}
