using LeafEmu.World.Game.Entity;
using LeafEmu.World.Network;
using System;
using System.Collections.Generic;


namespace LeafEmu.World.Game.Map
{
    public class Map
    {
        public Dictionary<int, int[]> CellTp;
        public List<Cell.Cell> Cells;
        public EndFightAction EndFightActions { get; set; }
        public List<int> CellForceDisable = new List<int>();
        public List<int> CellForceEnable = new List<int>();
        public List<MobGroup> MobInMap = new List<MobGroup>();
        public List<Fight.Fight> FightInMap = new List<Fight.Fight>();
        public List<Entity.Npc.Npc> Npcs = new List<Entity.Npc.Npc>();
        public List<InteractiveDoors> InteractiveDoors = new List<InteractiveDoors>();
        public List<listenClient> CharactersOnMap;
        public int Id { get; set; }
        public string CreateTime { get; set; }
        public string Data { get; set; }
        public string DataKey { get; set; }
        public List<int[]> mobsIDs { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string X { get; set; }
        public string stringmobs { get; set; }
        public string Y { get; set; }
        public string Capabilities { get; set; }
        public string SubArea { get; set; }
        public int MaxMobs { get; set; }
        public int NbGroups { get; set; }
        public string PosFight { get; set; }

        public Map(int _id, string _createTime, string _data, string _datakey, string _mobs, string _width, string _height,
            string _x, string _y, string _capabilities, string _subAreaId, int _maxMobs, string _PosFight, int _nbGroup)
        {
            stringmobs = _mobs;
            PosFight = _PosFight;
            CellTp = new Dictionary<int, int[]>();
            CharactersOnMap = new List<Network.listenClient>();
            Id = _id;
            CreateTime = _createTime.Split('X')[0];
            DataKey = _datakey;
            Data = _data;
            mobsIDs = new List<int[]>();
            Width = int.Parse(_width);
            Height = int.Parse(_height);
            X = _x;
            Y = _y;
            Capabilities = _capabilities;
            SubArea = _subAreaId;
            MaxMobs = _maxMobs;
            NbGroups = NbGroups > 3 ? 3 : _nbGroup;
            Cells = new List<Cell.Cell>();
            UncompressDatas(0);
            Create_Groupe_monster(_mobs);
        }

        private void Create_Groupe_monster(string _mobs)
        {
            if (_mobs == "")
                return;
            if (PosFight == "")
            {
                Logger.Logger.Warning($"No pos for map {Id} but need to spawn mobs", 15);
                return;
            }
            short size = 8;
            for (int i = 0; i < NbGroups; i++)
            {
                MobInMap.Add(MobGroup.CreateMobGroupe(i, _mobs, MobGroup.GetRngCellWalkable(Cells), '|', size, false));
                size = (short)(size - 3);
            }

        }

        public void Remove(listenClient prmClient)
        {
            if (System.Threading.Monitor.TryEnter(prmClient.account.character.Map.CharactersOnMap, new TimeSpan(0, 0, 2)))
            {
                try
                {
                    prmClient.account.character.Map.CharactersOnMap.Remove(prmClient);
                    prmClient.account.character.Map.SendToAllMap($"GM|-{prmClient.account.character.id}");
                }
                finally
                {
                    System.Threading.Monitor.Exit(prmClient.account.character.Map.CharactersOnMap);
                }
            }
        }

        public void SendToAllMap(string packet, bool SendToFight = false)
        {
            foreach (var item in CharactersOnMap)
            {
                if (SendToFight || item.account.character.FightInfo.InFight == 0)
                    item.send(packet);
            }
        }

        //public void UncompressDatas()
        //{
        //    string data = Data;
        //    for (int i = 0; i < data.Length; i += 10)
        //    {
        //        string CurrentCellData = data.Substring(i, 10);
        //        Cells.Add(new Cell.Cell(i/10, CurrentCellData, Width));
        //    }

        //}   
        public void UncompressDatas(byte sniffed)
        {
            string data = Data;
            List<Cell.Cell> cells = new List<Cell.Cell>();
            for (int f = 0; f < data.Length; f += 10)
            {
                string mapData = data.Substring(f, 10);
                List<byte> cellInfos = new List<byte>();

                for (int i = 0; i < mapData.Length; i++)
                    cellInfos.Add((byte)getIntByHashedValue(mapData.ToCharArray()[i]));

                int walkable = ((cellInfos[2] & 56) >> 3);
                bool los = (cellInfos[0] & 1) != 0;

                int layerObject2 = ((cellInfos[0] & 2) << 12) + ((cellInfos[7] & 1) << 12) + (cellInfos[8] << 6) + cellInfos[9];
                bool layerObject2Interactive = ((cellInfos[7] & 2) >> 1) != 0;
                int obj = (layerObject2Interactive && sniffed == 0 ? layerObject2 : -1);

                cells.Add(new Cell.Cell((short)(f / 10), (walkable != 0 && !mapData.Equals("bhGaeaaaaa") && !mapData.Equals("Hhaaeaaaaa")), los, obj, Width));
            }

            Cells = new List<Cell.Cell>(cells);
        }

        public static int getIntByHashedValue(char c)
        {
            for (int a = 0; a < Util.HASH.Count; a++)
                if (Util.HASH[a] == c)
                    return a;
            return -1;
        }

        public static int[] CellIDToPos(int cell_id, int width)
        {
            int posY = (cell_id / (((width) * 2) - 1)) * 2;
            int posX = 0;

            if ((cell_id % (((width) * 2) - 1)) >= width)
                posY++;
            if (cell_id > (width - 1) * 2)
                posX = (cell_id + (posY / 2)) % (width) + 1;
            else
                posX = cell_id % (width) + 1;

            return new int[] { posX, posY };
        }

        public static int PosToCellID(int x, int y, int width)
        {
            if (y % 2 != 0)
            {
                return (y * width) + (x) - (y / 2) - 1;
            }
            return (y * width) + (x) - (y / 2);
        }

    }
}


