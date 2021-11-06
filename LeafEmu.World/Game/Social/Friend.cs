//using LeafEmu.World.PacketGestion;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LeafEmu.World.Game.Social
//{
//    public class Friend
//    {
//        private int     id;
//        private string  NickName;
//        private int     Level;
//        private int     classe;
//        private int     gender;
//        private int     look;

//        public Friend(int id, string NickName, int level, int classe, int gender, int look)
//        {
//            this.id = id;
//            this.NickName = NickName;
//            this.Level = level;
//            this.classe = classe;
//            this.gender = gender;
//            this.look = look;
//        }

//        public string CharacterToFriendsListKnow
//        {
//            get
//            {
//                StringBuilder packet = new StringBuilder();
//                packet.Append(";?;");//FIXME
//                packet.Append(NickName).Append(";");
//                packet.Append(Level).Append(";");
//                packet.Append("-1;");//FIXME : Alignement
//                packet.Append(classe).Append(";");
//                packet.Append(gender).Append(";");
//                packet.Append(look).Append(";");
//                return packet.ToString();
//            }
//        }

//        public string CharacterToFriendsListUnKnow
//        {
//            get
//            {
//                StringBuilder packet = new StringBuilder();
//                packet.Append(";?;");//FIXME
//                packet.Append(NickName).Append(";");
//                packet.Append("?").Append(";");
//                packet.Append("-1;");//FIXME : Alignement
//                packet.Append(classe).Append(";");
//                packet.Append(gender).Append(";");
//                packet.Append(look).Append(";");
//                return packet.ToString();
//            }
//        }

//    }
//    class FriendPacket
//    {
//        [PacketAttribute("FL")]
//        public static void ShowFriends(Network.listenClient prmClient, string prmPacket)
//        {
//            StringBuilder friendPacket = new StringBuilder("FL");
//            foreach (var account in prmClient.account.ListFriends)
//            {
//                //Database.Records.AccountDataRecord account = Helper.AccountHelper.GetAccountData(i);
//                friendPacket.Append($"|{account.NickName}");
//                World.Network.WorldClient player = Helper.WorldHelper.GetClientByAccountNickName(account.NickName);
//                if (player != null)
//                {
//                    if (player.AccountData.FriendsIDs.Contains(client.AccountData.AccountID))
//                    {
//                        friendPacket += player.Character.Pattern.CharacterToFriendsListKnow;
//                    }
//                    else
//                    {
//                        friendPacket += player.Character.Pattern.CharacterToFriendsListUnKnow;
//                    }
//                }
//            }
//            prmClient.send(friendPacket);
//        }


//        [PacketAttribute("FA")]
//        public static void AddFriend(Network.listenClient prmClient, string prmPacket)
//        {
//            string addType = packet[2].ToString();
//            string nickname;
//            Network.WorldClient player;
//            switch (addType)
//            {
//                case "%"://Character name
//                    nickname = packet.Substring(3);
//                    player = Helper.WorldHelper.GetClientByCharacter(nickname);
//                    if (player != null)
//                    {
//                        client.AccountData.FriendsIDs.Add(player.AccountData.AccountID);
//                    }
//                    else
//                    {
//                        client.Send("cMEf" + nickname);
//                    }
//                    break;

//                case "*":
//                    nickname = packet.Substring(3);
//                    player = Helper.WorldHelper.GetClientByCharacter(nickname);
//                    if (player != null)
//                    {
//                        client.AccountData.FriendsIDs.Add(player.AccountData.AccountID);
//                    }
//                    else
//                    {
//                        client.Send("cMEf" + nickname);
//                    }
//                    break;

//                default:
//                    nickname = packet.Substring(2);
//                    player = Helper.WorldHelper.GetClientByCharacter(nickname);
//                    if (player != null)
//                    {
//                        client.AccountData.FriendsIDs.Add(player.AccountData.AccountID);
//                        client.Send("BN");
//                    }
//                    else
//                    {
//                        client.Send("cMEf" + nickname);
//                    }
//                    break;
//            }
//        }

//        [PacketAttribute("FD")]
//        public static void DeleteFriend(Network.listenClient prmClient, string prmPacket)
//        {
//            string nickname = packet.Substring(3);
//            if (Helper.AccountHelper.ExistAccountData(nickname))
//            {
//                Database.Records.AccountDataRecord account = Helper.AccountHelper.GetAccountData(nickname);
//                if (client.AccountData.FriendsIDs.Contains(account.AccountID))
//                {
//                    client.AccountData.FriendsIDs.Remove(account.AccountID);
//                    ShowFriends(client);
//                }
//            }
//        }

//        public static void WarnConnectionToFriends(Network.listenClient prmClient)
//        {
//            foreach (int i in client.AccountData.FriendsIDs)
//            {
//                if (Helper.AccountHelper.ExistAccountData(i))
//                {
//                    Database.Records.AccountDataRecord account = Helper.AccountHelper.GetAccountData(i);
//                    World.Network.WorldClient player = Helper.WorldHelper.GetClientByAccountNickName(account.NickName);
//                    if (player != null)
//                    {
//                        if (player.AccountData.FriendsIDs.Contains(client.AccountData.AccountID))
//                        {
//                            player.SendImPacket("0143", client.AccountData.NickName +
//                                        "(<b><a href='asfunction:onHref,ShowPlayerPopupMenu," +
//                                        client.Character.Nickname + "'>" + client.Character.Nickname + "</a></b>)");
//                        }
//                    }
//                }
//            }
//        }
//    }
//}
