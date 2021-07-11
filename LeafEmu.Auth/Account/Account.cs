﻿using System.Collections.Generic;

namespace LeafEmu.Auth.Account
{
    class Account
    {
        public int AccountIsVerifStade = 0;
        public List<int> ListServ { get; set; }
        public int role { get; set; }
        public int ID { get; set; }
        public string Qsecret { get; set; }
        public string Rsecret { get; set; }
        public string UserName { get; set; }
        public string HashPass { get; set; }
        public string version { get; set; }
        public string Key { get; set; }
        public string PacketAx { get; set; }
        public string PacketAH { get; set; }
        public Account(string _key)
        {
            ID = -1;
            PacketAH = "AH";
            PacketAx = "AxK10";
            ListServ = new List<int>();
            Key = _key;
        }


    }
}
