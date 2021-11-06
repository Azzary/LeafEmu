using LeafEmu.World.Game.Map;
using LeafEmu.World.Network;
using LeafEmu.World.PacketGestion;
using System.Text;

namespace LeafEmu.World.Game.Character
{
    class GestionCharacter
    {

        [PacketAttribute("AA")]
        public void CreateCharacter(listenClient prmClient, string prmPacket)
        {
            //AA Tifehucu|2|0|-1|-1|-1
            if (prmClient.account.ListCharacter.Count <= 5)
            {
                List<string> infoStr;
                List<int> infoInt;
                string name = string.Empty;
                try
                { 
                    infoStr = prmPacket.Substring(2).Split("|").ToList();
                    name = infoStr[0];
                    infoStr.RemoveAt(0);
                    infoInt = infoStr.ConvertAll(x => int.Parse(x));
                }
                catch (Exception)
                {
                    return;
                }
                if (infoInt.Count < 5 || (infoInt[0] < 1 && infoInt[0] > 12))
                    return;

                int gfxID = infoInt[0] * 10;
                gfxID = infoInt[1] == 1 ? gfxID + 1: gfxID;
                byte classe = (byte)infoInt[0];
                int[] StartPos = World.WorldConfig.GetPosStart(classe);

                Entity.Character character = new Entity.Character(Database.LoadDataBase.GetNewUIDCharacter(), name, 1, 0, gfxID, StartPos[0], StartPos[1], infoInt[2],
                    infoInt[3], infoInt[4], infoInt[1], classe, 0, 1000, 0, 0, 0, 0, 0, 10000, 6, 3, 0, 0, 0, 0, 0, true, $"{StartPos[1]},{StartPos[0]}");
                Spells.SpellsManagement.AddSpells(character);
                prmClient.account.ListCharacter.Add(character);
              
            }
            prmClient.send(Get_Packet_character(prmClient));
            //Cinema ???
            prmClient.send("TB");
        }


        [PacketAttribute("AD")]
        public void DelCharacter(listenClient prmClient, string prmPacket)
        {
            string[] infoReck = prmPacket.Substring(2).Split('|');
            if (infoReck.Length == 0)
                return;
            string id = infoReck[0];
            for (int i = 0; i < prmClient.account.ListCharacter.Count; i++)
            {
                if (id == prmClient.account.ListCharacter[i].id.ToString())
                {
                    if (prmClient.account.ListCharacter[i].level < 20 
                    || (infoReck.Length == 2 && prmClient.account.rSecret == infoReck[1]))
                    {
                        prmClient.account.ListRemoveCharacter.Add(prmClient.account.ListCharacter[i]);
                        prmClient.account.ListCharacter.RemoveAt(i);
                    }
                    break;
                }
            }
            prmClient.send(Get_Packet_character(prmClient));

        }

        [PacketAttribute("Ak0")]
        public void ListCharacter(Network.listenClient prmClient, string prmPacket)
        {
            prmClient.send("Aq1");
            prmClient.send(Get_Packet_character(prmClient));
        }


        [PacketAttribute("GC1")]
        public static void GetInfoCharacter(Network.listenClient prmClient, string prmPacket)
        {
            if (prmClient.account.character.FightInfo.InFight != 2)
            {
                MapGestion.SetCharacterInMap(prmClient);
            }
            //prmClient.send(createAsPacket(prmClient) + "\0ILS2000");
            //Map.MapGestion.CreateMapPacketInfo(prmClient);
        }


        /// <summary>
        /// Stats Personnage (xp/kamas...)
        /// </summary>
        /// <param name="prmClient"></param>
        /// <returns></returns>
        public static string createAsPacket(Entity.Character entity)
        {
            StringBuilder packet = new StringBuilder();
            entity.UpdateEquipentStats();

            packet.Append($"As{entity.XP},{Database.table.Experience.ExperienceStat[entity.level][0]},{Database.table.Experience.ExperienceStat[entity.level + 1][0]}|");
            packet.Append($"{entity.kamas}|");
            packet.Append($"{entity.capital}|");
            packet.Append($"{entity.PSorts}|");
            packet.Append($"0~0,0,0,0,0,0|");
            packet.Append($"{entity.Vie},{entity.TotalVie}|");
            packet.Append($"{entity.energie},10000|");
            packet.Append($"0|");
            packet.Append($"100|");
            packet.Append($"{entity.PA},{entity.EquipementPA},0,0,{entity.TotalPA}|");
                packet.Append($"{entity.PM},{entity.EquipementPM},0,0,{entity.TotalPM}|");
            packet.Append($"{entity.CaracForce},{entity.EquipementForce},0,{entity.TotalForce}|");
            packet.Append($"{entity.CaracVie},{entity.EquipementVie},0,{entity.TotalVie}|");
            packet.Append($"{entity.CaracSagesse},{entity.EquipementSagesse},0,{entity.TotalSagesse}|");
            packet.Append($"{entity.CaracChance},{entity.EquipementChance},0,{entity.TotalChance}|");
            packet.Append($"{entity.CaracAgi},{entity.EquipementAgi},0,{entity.TotalAgi}|");
            packet.Append($"{entity.CaracIntell},{entity.EquipementIntell},0,{entity.TotalIntell}|");
            packet.Append($"{entity.PO},0,0,0|");

            packet.Append($"{entity.Damage},0,0,0|");
            packet.Append($"{entity.DamagePhysic},0,0,0|");
            packet.Append($"{entity.DamageMagic},0,0,0|");
            packet.Append($"{entity.DamagePercent},0,0,0|");
            packet.Append($"{entity.F_DamagePiege},0,0,0|");
            packet.Append($"{entity.P_DamagePiege},0,0,0|");
            packet.Append($"{entity.HealBonus},0,0,0|");
            packet.Append($"0,0,0,0|");//Revoie de dmg
            packet.Append($"{entity.CC},0,0,0|");
            packet.Append($"{entity.EC},0,0,0|");
            packet.Append($"{entity.EsquivePA},0,0,0|");
            packet.Append($"{entity.EsquivePM},0,0,0|");

            packet.Append($"{entity.F_TotalResNeutre},0,0,0|");
            packet.Append($"{entity.P_TotalResNeutre},0,0,0|");

            packet.Append($"{entity.F_TotalResForce},0,0,0|");
            packet.Append($"{entity.P_TotalResForce},0,0,0|");

            packet.Append($"{entity.F_TotalResIntell},0,0,0|");
            packet.Append($"{entity.P_TotalResIntell},0,0,0|");

            packet.Append($"{entity.F_TotalResEau},0,0,0|");
            packet.Append($"{entity.P_TotalResEau},0,0,0|");

            packet.Append($"{entity.F_TotalResAgi},0,0,0|");
            packet.Append($"{entity.P_TotalResAgi},0,0,0|");

            for (int i = 0; i < 10; i++)
            {
                packet.Append("0,0,0,0,0|");
            }
            packet.Append("20");
            return packet.ToString();
        }

        [PacketAttribute("AS")]
        public void LoginInWorld(Network.listenClient prmClient, string prmPacket)
        {
            if (int.TryParse(prmPacket.Substring(2).Split('\0')[0], out int id))
            {
                prmClient.account.statue = 2;
                foreach (Entity.Character character in prmClient.account.ListCharacter)
                {
                    if (id == character.id)
                    {
                        prmClient.account.character = character;
                        prmClient.CharacterInWorld.Add(prmClient);
                        prmClient.send("BN");
                        prmClient.send("Rx0");
                        prmClient.account.character.Inventaire.SendInventory(prmClient);
                        //prmClient.send(CreateASKPacket(character) + "3254786~371~1~~;");
                        prmClient.send("ZS0");
                        prmClient.send("0cC+*#$pi^");

                        //prmClient.send($"\0Os");
                        prmClient.send("al|270;0|49;1|319;0|98;0|147;0|466;2|245;0|515;2|294;0|73;1|122;2|171;2|441;0|220;0|490;2|269;0|48;1|318;0|97;0|146;0|465;0|244;0|514;1|23;2|293;0|72;2|121;2|170;2|440;0|219;0|268;0|47;1|317;0|96;0|145;0|464;2|243;0|513;1|22;2|292;0|71;2|120;2|169;2|218;0|488;2|267;0|46;1|316;2|95;0|144;0|463;0|512;1|21;0|291;0|70;1|119;2|168;2|217;0|487;0|266;0|536;0|45;1|315;2|94;0|143;2|462;0|511;2|20;0|290;0|69;1|339;0|118;2|167;2|216;0|486;2|44;1|314;2|93;2|461;2|510;2|19;0|289;0|68;1|338;0|117;2|166;2|215;0|485;2|43;1|313;0|92;0|141;0|460;0|509;2|18;0|288;0|67;1|337;0|116;2|165;2|214;0|484;2|42;0|312;0|91;2|140;0|459;0|508;2|17;0|287;0|66;1|336;0|115;2|164;2|213;0|483;2|41;0|311;0|139;0|507;2|16;0|286;0|65;1|335;0|114;2|163;2|212;0|482;2|261;0|40;0|310;0|89;0|138;0|457;2|236;0|506;2|15;0|285;0|64;1|334;2|113;2|162;2|211;0|481;2|260;0|39;0|309;0|88;0|137;0|235;2|505;2|14;0|284;0|63;1|333;0|112;2|161;2|210;0|480;2|259;0|38;0|308;0|87;2|136;0|455;2|234" +
                            ";2|504;2|13;0|62;1|332;0|111;2|209;1|479;2|258;0|37;1|307;0|86;0|135;0|454;2|233;2|503;2|12;2|61;2|331;0|110;0|159;0|208;0|478;1|257;0|306;0|85;0|134;0|453;2|232;2|502;2|11;2|281;0|60;0|330;0|109;2|158;0|207;0|477;1|256;0|35;0|84;0|133;0|182;2|452;0|231;2|501;0|10;2|280;2|59;2|329;0|108;0|157;0|206;0|476;2|255;0|34;0|304;0|83;0|132;0|181;0|451;0|230;2|500;2|9;2|279;1|328;0|107;2|156;0|205;0|254;0|33;2|303;0|82;0|131;0|180;0|450;0|229;0|499;2|8;2|278;0|57;2|327;0|106;2|155;0|204;0|474;0|253;2|32;2|302;0|81;1|130;0|179;2|449;0|228;0|498;0|7;2|277;2|56;1|326;0|105;2|154;0|203;0|473;0|252;0|31;2|301;0|80;1|129;0|178;0|448;0|227;0|497;0|6;2|276;2|55;2|325;0|153;0|202;0|472;2|251;0|30;0|300;0|79;1|128;0|177;0|447;0|226;0|496;0|5;2|275;2|54;0|324;0|103;2|152;2|201;0|471;2|250;0|29;2|299;0|78;0|127;0|446;0|225;0|495;0|4;2|274;0|53;2|323;0|102;2|151;0|200;0|470;0|249;0|28;2|298;0|77;0|126;0|175;0|445;0|224;0|494;0|3;2|273;0|322;0|101;0|150;0|469;2|248;0|27;2|297;0|76;1|125;0|174;0|444;0|223;0|493;0|2;2|272;0|51;1|321;0|100;0|149;0|468;1|247;0|26;0|296;0|75;2|124;0|173;0|443;0|222;0|492;2|1;2|271;0|50;1|320;0|99;0|148;0|467;2|246;0|25;2|295;0|74;1|123;2|442;0|221;0|491;0|0;0");
                        prmClient.send($"SLo+\0{CreateSpellsPacket(prmClient)}\0AR6bk\0Ow{character.pods}|{character.podsMax}\0FO-\0Im189\0Im0152;2021~05~10~11~32~***.***.***.91\0Im0153;***.***.***.91\0");
                        prmClient.send($"GCK|1|{prmClient.account.character.speudo}");
                        prmClient.send(createAsPacket(prmClient.account.character) + "\0ILS2000");
                        MapGestion.SetCharacterInMap(prmClient);
                        prmClient.send("BT1620646624846\0fC0\0");
                    }
                }
            }
            else
                ListCharacter(prmClient, prmPacket);

        }


       public static string CreateSpellsPacket(listenClient prmClient)
        {
            return ($"SL{Spells.SpellsManagement.CreatesSpellsPacket(prmClient.account.character.Spells)}");
        }
        [PacketAttribute("ALf")]
        public void CancelCreateCharacter(Network.listenClient prmClient, string prmPacket)
        {
            prmClient.send(Get_Packet_character(prmClient));
        }



        private string Get_Packet_character(listenClient prmClient)
        {
            string packet = string.Empty;
            foreach (Game.Entity.Character charac in prmClient.account.ListCharacter)
            {
                packet += $"|{charac.id};{charac.speudo};{charac.level};{charac.gfxID};-1;-1;-1;,,,,;{charac.isDead};{World.WorldConfig.ServerID};;;";
                //{ charac.couleur3.ToString("x")}

            }
            packet = $"ALK1{prmClient.account.ID}|{prmClient.account.ListCharacter.Count}{packet}";
            return packet;
        }
    }
}
