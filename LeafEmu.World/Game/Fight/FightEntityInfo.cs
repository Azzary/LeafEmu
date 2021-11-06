using LeafEmu.World.Network;
using System.Collections.Generic;
using System.Text;

namespace LeafEmu.World.Game.Fight
{
    public class FightEntityInfo
    {
        public int FightID { get; set; }
        public int FightCell { get; set; }
        public bool YourTurn { get; set; }
        public bool isDead { get; set; }
        public List<Entity.Entity> EntityInvocation { get; set; }
        public Network.listenClient RequestDuel { get; set; }

        public bool IsReady { get; set; }
        public Dictionary<int, int> Drops { get; set; }
        public bool IsLauncherDuel { get; set; }
        public int NbInvo { get; set; }
        //0 = not in fight 1 = In pre Fight 2 = in fight
        public int InFight { get; set; }

        public int PosFight { get; set; }
        public short equipeID { get; set; }

        public List<int> ListCellIDPlacement = new List<int>();

        public bool IsSpeactator { get; set; }



        public FightEntityInfo(int fightid, short _equipe = 1, bool isSpeac = false)
        {
            EntityInvocation = new List<Entity.Entity>();
            Drops = new Dictionary<int, int>();
            IsSpeactator = isSpeac;
            FightID = fightid;
            equipeID = _equipe;
            YourTurn = false;
            IsReady = false;
            InFight = 0;
            IsLauncherDuel = false;
            if (IsSpeactator)
            {
                isDead = true;
                equipeID = -1;
                FightID = -1;
            }
        }

        public bool IsFriendly(Entity.Entity entity)
        {
            return equipeID == entity.FightInfo.equipeID;
        }

        public void AddDrop(int itemID, int quantity)
        {
            if (quantity == 0)
                return;

            if (this.Drops.ContainsKey(itemID))
            {
                this.Drops[itemID]++;
            }
            else
            {
                this.Drops.Add(itemID, 1);
            }
        }


        public string ParseDrop
        {
            get
            {
                StringBuilder packet = new StringBuilder();
                foreach (var drop in Drops)
                {
                    if (packet.ToString().Length > 0) packet.Append(",");
                    packet.Append(drop.Key);
                    packet.Append("~");
                    packet.Append(drop.Value);
                }
                return packet.ToString();
            }
        }


        public void AtributePosFight(listenClient prmClient, Entity.Entity entity)
        {
            prmClient.account.character.FightInfo.FightCell = -100;
            string cells = prmClient.account.character.Map.PosFight.Split("|")[equipeID];
            for (int i = 0; i < cells.Length; i += 2)
            {
                ListCellIDPlacement.Add(Util.CharToCell(cells.Substring(i, 2)));
            }
            foreach (int cell in ListCellIDPlacement)
            {
                if (!prmClient.account.character.CurrentFight.AllEntityInFight.Exists(x => x.FightInfo != null && x.FightInfo.FightCell == cell))
                {
                    entity.FightInfo.InFight = 1;
                    entity.FightInfo.FightCell = cell;
                    break;
                }
            }
            if (entity.FightInfo.FightCell == -100)
            {
                Logger.Logger.Log("PB need to remove of the fight");
            }
        }

        public void GenerateInInventory(Network.listenClient PrmClient)
        {
            foreach (var drop in this.Drops)
            {
                PrmClient.account.character.Inventaire.GenerateItemInInv(PrmClient, drop.Key, drop.Value, false);
            }
        }
    }
}
