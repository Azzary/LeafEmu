using LeafEmu.World.PacketGestion;
using System;
using System.Text;

namespace LeafEmu.World.Game.Map
{
    class MapGestion
    {
        [PacketAttribute("BD")]
        public void SendMap(Network.listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.statue == 0)
            {
                prmClient.account.statue = 1;
            }

        }

        [PacketAttribute("GІ")]
        public void SendDate(Network.listenClient prmClient, string prmPacket)
        {

            prmClient.send("BD" + DateTime.UtcNow.Date.ToString("yyyy|MM|dd"));
            CreateMapPacketInfo(prmClient);

        }

        public static void SetCharacterInMap(Network.listenClient prmClient)
        {
            Map map = Database.table.Map.Maps[prmClient.account.character.mapID];
            lock (map.CharactersOnMap)
            {
                if (!map.CharactersOnMap.Contains(prmClient))
                    map.CharactersOnMap.Add(prmClient);
            }
            prmClient.account.character.Map = map;
            prmClient.send($"GDM|{map.Id}|{map.CreateTime}|{map.DataKey}");
        }


        public static string CreatePacketCharacterGM(Entity.Character character)
        {
            //+367;1;0;2784108;Florde;8;80^100;0;0,0,0,2784110;-1;-1;-1;,,,,;0;;;;;0;;

            return $"GM|+{character.cellID};1;0;{character.id};{character.speudo};{character.classe};{character.gfxID}^{character.Scale};{character.sexe};0,0,0,999;-1;-1;-1;,,,,;0;;;;;0;;\0{Item.MoveItem.GetItemsPos(character)}";

        }

        public static void CreateMapPacketInfo(Network.listenClient prmClient)
        {
            if (prmClient.account.character.Map != null)
            {
                Map _map = prmClient.account.character.Map;
                lock (_map)
                {
                    string packet2 = string.Empty;

                    foreach (var fight in _map.FightInMap)
                    {
                        if (fight.FightStade == 0 && fight.nbTour == 0)
                        {
                            packet2 += $"Gc+{fight.InfoJoinConbat[4]};0|{fight.InfoJoinConbat[0]};{fight.InfoJoinConbat[1]};0;-1|{fight.InfoJoinConbat[2]};{fight.InfoJoinConbat[3]};0;-1\0";
                        }
                    }
                    ShowNpcsOnMap(prmClient);
                    lock (_map.CharactersOnMap)
                    {
                        foreach (var player in _map.CharactersOnMap)
                        {
                            Entity.Character entitie = player.account.character;
                            if (entitie.FightInfo.InFight == 0)
                            {
                                prmClient.send(CreatePacketCharacterGM(entitie) + $"\0{packet2}");
                                player.send(CreatePacketCharacterGM(prmClient.account.character));
                            }
                        }
                    }
                    foreach (Entity.MobGroup groupMob in prmClient.account.character.Map.MobInMap)
                    {
                        prmClient.send("GM|" + groupMob.parseGM());
                    }
                    if (Database.table.Donjons.Donjon.ContainsKey(prmClient.account.character.mapID))
                    {
                        prmClient.send("GM|" + Database.table.Donjons.Donjon[prmClient.account.character.mapID].parseGM());
                    }
                }

            }
            prmClient.send("GDK");
            prmClient.send("EW+2784108|");
        }

        public static void ShowNpcsOnMap(Network.listenClient client)
        {
            string globalPattern = "GM";
            foreach (var npc in client.account.character.Map.Npcs)
            {
                globalPattern += npc.DisplayOnMap;
            }
            client.send(globalPattern);
        }

        [PacketAttribute("GI")]
        public void SendPong(Network.listenClient prmClient, string prmPacket)
        {
            prmClient.send("BD" + DateTime.UtcNow.Date.ToString("yyyy|MM|dd"));
            CreateMapPacketInfo(prmClient);
        }

        public static string GmPacket(Entity.Entity entity)
        {
            StringBuilder str = new StringBuilder();
            str.Append("|+");
            str.Append(entity.FightInfo.FightCell).Append(";");
            str.Append("1;0^false;");//1; = Orientation
            str.Append(entity.ID_InFight).Append(";");
            str.Append(entity.speudo).Append(";");
            str.Append(entity.classe).Append(";");
            str.Append(entity.gfxID).Append("^").Append(entity.Scale).Append(";");
            str.Append(entity.sexe).Append(";");
            str.Append(entity.level).Append(";");
            str.Append("0").Append(",");//Alignement
            str.Append("0").Append(",");
            str.Append("0").Append(",");//grade
            str.Append(entity.level + entity.level);
            str.Append("0").Append(",");
            if (false)//this.perso.is_showWings() && this.perso.getDeshonor() > 0)
            {
                str.Append(",");
                //str.Append(perso.getDeshonor() > 0 ? 1 : 0).Append(';');
            }
            else
            {
                str.Append("0;");
            }
            int color1 = entity.couleur1,
                    color2 = entity.couleur2,
                    color3 = entity.couleur2;
            str.Append((color1.ToString("x"))).Append(";");
            str.Append((color2.ToString("x"))).Append(";");
            str.Append((color3.ToString("x"))).Append(";");
            str.Append(",,,,,1,").Append(";");
            str.Append(entity.Vie).Append(";");
            str.Append(entity.PA).Append(";");
            str.Append(entity.PM).Append(";");
            str.Append(entity.P_TotalResNeutre).Append(";");
            str.Append(entity.P_TotalResForce).Append(";");
            str.Append(entity.P_TotalResIntell).Append(";");
            str.Append(entity.P_TotalResEau).Append(";");
            str.Append(entity.P_TotalResAgi).Append(";");
            str.Append("0").Append(";");
            str.Append("0").Append(";");
            str.Append(entity.FightInfo.equipeID).Append(";").Append(";");
            str.Append("0").Append(";");
            //if (this.perso.isOnMount() && this.perso.getMount() != null)
            //str.Append(this.perso.getMount().getStringColor(this.perso.parsecolortomount()));
            str.Append("0;");

            return str.ToString();
        }

    }
}

