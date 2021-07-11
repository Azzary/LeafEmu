using LeafEmu.World.Network;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LeafEmu.World.Game.Fight
{
    public class Fight
    {
        public List<listenClient> PlayerInFight;
        public List<Entity.Entity> AllEntityInFight;
        public List<Entity.Entity> LoosersTeam;
        public List<Entity.Entity> WinerTeam;
        public CancellationTokenSource StopTurn;
        public FightTypeEnum FightType { get; }
        public List<List<Entity.Entity>> Equipe;
        public int nbTour = 0;
        public int Stars { get; set; }
        public int FightID { get; set; }
        public string GTLpacket { get; set; }
        public int FightStade { get; set; }
        public List<int> PreventsSpells = new List<int>();
        public bool FightRuning { get; set; }
        public int[] InfoJoinConbat { get; set; }
        public List<Game.Spells.SpellsEffects.Effect.GlyphAndTrap> glyphAndTrapsOnMap = new List<Spells.SpellsEffects.Effect.GlyphAndTrap>();
        private Pathfinding.PathfindingV2 pathfinding;
        public Map.Map map;
        private readonly int TimeTurn = 40000;
        public Fight(List<listenClient> _PlayerInFight, FightTypeEnum _FightType)
        {
            FightStade = 0;
            LoosersTeam = WinerTeam = new List<Entity.Entity>();
            map = _PlayerInFight[0].account.character.Map;
            pathfinding = new Pathfinding.PathfindingV2(map);
            Equipe = new List<List<Entity.Entity>>(2) { new List<Entity.Entity>(), new List<Entity.Entity>() };
            AllEntityInFight = new List<Entity.Entity>();
            FightType = _FightType;
            PlayerInFight = _PlayerInFight;
            FightRuning = false;
        }

        //Don't Remove
        public Fight() { }

        public void start()
        {
            FightStade = 1;
            SetOrderFight();
            map.SendToAllMap("Gc-" + PlayerInFight[0].account.character.FightInfo.FightID);
            Entity.Entity TourEntity = AllEntityInFight[0];
            int pos = -1;
            while (FightRuning)
            {
                while (FightRuning)
                {
                    FightRuning = nbTour < 100;
                    CheckEnd();
                    if (!FightRuning)
                        break;
                    pos++;
                    if (pos > AllEntityInFight.Count - 1)
                    {
                        pos = 0;
                        nbTour++;
                    }
                    TourEntity = AllEntityInFight[pos];
                    if (TourEntity.FightInfo.InFight == 2 && !TourEntity.FightInfo.isDead)
                    {
                        break;
                    }
                }
                if (!FightRuning)
                    break;

                string packetGTF = "GTF" + TourEntity.id;
                string packetGTM = CreateGTMPacket(true);
                for (int x = 0; x < PlayerInFight.Count; x++)
                {
                    PlayerInFight[x].send(packetGTF);
                    PlayerInFight[x].send(GTLpacket);
                    PlayerInFight[x].send(packetGTM);
                    PlayerInFight[x].send($"GTS{TourEntity.ID_InFight}|{TimeTurn}\0");
                }
                //GAC\0GA;940\0As...
                TourEntity.FightInfo.YourTurn = true;
                //TourEntity.send("GAC\0GA;940\0");// + Character.GestionCharacter.createAsPacket(TourEntity));
                TourEntity.CurrentFight = this;
                TurnGestion(TourEntity);
                GlyphGestion(TourEntity);
                Buff.Buff.GestionBuff(TourEntity);
                FightRuning = AllEntityInFight.Count <= 1 ? false : true;
                TourEntity.UpdateStat();
            }
        }

        private void GlyphGestion(Entity.Entity TourEntity)
        {
            foreach (var item in glyphAndTrapsOnMap)
            {
                if (item.IsTrap)
                    continue;
                if (item.ListCell.Contains(TourEntity.FightInfo.FightCell))
                    item.ActionGliphEntity(TourEntity);
                if (item.LifeTime == nbTour)
                {
                    item.Remove();
                }
            }
            glyphAndTrapsOnMap.RemoveAll(x => !x.IsTrap && x.LifeTime == nbTour);
            
        }

        private string CreateGTMPacket(bool InFight = false)
        {
            string GTMpacket = "GTM|";
            foreach (var entity in AllEntityInFight)
            {
                if (!entity.FightInfo.isDead)
                {
                    GTMpacket += $"{entity.ID_InFight};0;{entity.Vie};{entity.PA};{entity.PM};{entity.FightInfo.FightCell};;{entity.TotalVie};0;0;{entity.P_TotalResNeutre},{entity.P_TotalResForce}," +
                    $"{entity.P_TotalResIntell},{entity.P_TotalResEau},{entity.P_TotalResAgi},8,0," +
                    $"|998;0;50;6;3;444;;50;0;0;0,0,0,0,0,0,0,;|";
                }
                //GTM|1257;0;50;6;3;368;;50;0;0;0,0,0,0,0,0,0,|998;0;50;6;3;444;;50;0;0;0,0,0,0,0,0,0,\0
            }
            return GTMpacket;
        }

        private void TurnGestion(Entity.Entity TourEntity)
        {
            if (TourEntity.IsHuman)
            {
                StopTurn = new CancellationTokenSource();
                try
                {
                    Task.Delay(TimeTurn).Wait(StopTurn.Token);
                }
                catch (OperationCanceledException)
                {
                    //Fin de Tour tout est normal
                }
                catch
                {
                    //probleme
                }
            }
            else
            {
                if (TourEntity.Vie > 0)
                {
                    try
                    {
                        Entity.Mob mob = (Entity.Mob)TourEntity;
                        mob.AI.PlayAI();
                    }
                    catch (Exception)
                    { }
                }
                else
                    TourEntity.FightInfo.isDead = true;
            }
            TourEntity.FightInfo.YourTurn = false;

        }

        public void SendToAllFight(string packet, int equipe = -1)
        {
            lock (PlayerInFight)
            {
                foreach (listenClient entity in PlayerInFight)
                {
                    if (equipe == -1 || equipe == entity.account.character.FightInfo.equipeID)
                    {
                        entity.send(packet);
                    }
                }
            }
        }

        bool ischeckEnd = false;

        public void CheckEnd()
        {
            if (ischeckEnd)
            {
                return;
            }
            ischeckEnd = true;
            lock (this)
            {

                bool endingOfFight = false;
                foreach (var fighter in AllEntityInFight)
                {
                    if (fighter.Vie <= 0)
                    {
                        fighter.FightInfo.isDead = true;
                    }
                }
                if (Equipe[0].FindAll(x => !x.FightInfo.isDead).Count <= 0)
                {
                    endingOfFight = true;
                    WinerTeam = Equipe[1];
                    LoosersTeam = Equipe[0];
                }
                else if (Equipe[1].FindAll(x => !x.FightInfo.isDead).Count <= 0)
                {
                    endingOfFight = true;
                    WinerTeam = Equipe[0];
                    LoosersTeam = Equipe[1];
                }
                else if (PlayerInFight.Count == 0)
                    endingOfFight = true;
                if (endingOfFight)
                {
                    if (FightStade == 0)
                    {
                        map.SendToAllMap($"Gc-{FightID}", true);
                    }
                    FightRuning = false;
                    map.FightInMap.Remove(this);
                    GestionFight.EndFightResult(this);
                    Thread.Sleep(1000);
                    foreach (listenClient otherFighter in PlayerInFight)
                    {
                        if (true)//FightType == FightTypeEnum.Challenge)
                        {
                            otherFighter.account.character.Vie = otherFighter.account.character.TotalVie;
                        }
                        if (otherFighter.account.character.IsHuman)
                        {
                            otherFighter.account.character.FightInfo = new FightEntityInfo(20, -1, true);
                        }
                        otherFighter.send("GV");
                        otherFighter.account.character.Buffs.ForEach(x => x.LifeTime = 0);
                        Buff.Buff.GestionBuff(otherFighter.account.character);
                        otherFighter.account.character.Buffs.Clear();
                        otherFighter.account.character.resCaract();
                        Map.MapGestion.SetCharacterInMap(otherFighter);

                    }
                    foreach (var player in PlayerInFight)
                    {
                        Item.Stuff.RangerItem(ref player.account.character.Invertaire.Stuff);
                        player.send(Character.GestionCharacter.CreateASKPacket(player.account.character));
                        if (FightType == FightTypeEnum.PvM && LoosersTeam.Contains(player.account.character))
                        {
                            Map.MapMouvement.SwitchMap(player, player.account.character.MapSpawnPoint, player.account.character.CellSpawnPoint);
                        }
                        else if (map.EndFightActions != null
                            && FightType == FightTypeEnum.PvM
                            && map.EndFightActions.Action == 0
                            && WinerTeam.Contains(player.account.character))
                        {
                            Map.MapMouvement.SwitchMap(player, map.EndFightActions.map, map.EndFightActions.cell);
                        }
                    }

                    AllEntityInFight.ForEach(x => x.CurrentFight = null);
                    Equipe[0].Clear();
                    Equipe[1].Clear();
                    AllEntityInFight.Clear();
                    PlayerInFight.Clear();
                    return;
                }

                ischeckEnd = false;


            }
        }

        private void SetOrderFight()
        {
            List<Entity.Entity> OrderFither = new List<Entity.Entity>();
            Equipe[0].Sort();
            Equipe[1].Sort();
            int idEquipe = Equipe[0][0].initiative > Equipe[1][0].initiative ? 0 : 1;
            int indiceEquipe0 = 0;
            int indiceEquipe1 = 0;
            int pos = 0;
            for (int i = 0; i < AllEntityInFight.Count; i++)
            {
                AllEntityInFight[i].FightInfo.InFight = 2;
                OrderFither.Add(Equipe[idEquipe][pos]);
                if (idEquipe == 0)
                {
                    indiceEquipe0++;
                    idEquipe = pos < Equipe[1].Count ? 1 : 0;
                }
                else
                {
                    indiceEquipe1++;
                    idEquipe = pos < Equipe[0].Count ? 0 : 1;
                }
                pos = idEquipe == 0 ? indiceEquipe0 : indiceEquipe1;
            }
            AllEntityInFight = OrderFither;

            string GICpacket = "GIC|";
            foreach (Entity.Entity entity in AllEntityInFight)
            {
                GICpacket += $"{entity.ID_InFight};{entity.FightInfo.FightCell}|";
            }
            CreateOrderPacket();
            SendToAllFight(GICpacket + "\0GS\0" + GTLpacket);
            FightRuning = true;
        }
        public void CreateOrderPacket()
        {
            GTLpacket = "GTL";
            foreach (Entity.Entity entity in AllEntityInFight)
            {
                GTLpacket += $"|{entity.ID_InFight}";
            }
        }

        public bool CanDrop(Item.Item drop, int Grade)
        {
            return Util.rng.Next(0, 100) <= drop.PercentDropeByGrade[Grade];
        }

        public void GenerateDrops()
        {
            var possibleDrops = new Dictionary<int, int>();

            /* Generate looted droppred items */
            foreach (var entity in LoosersTeam.FindAll(x => !x.IsHuman && !x.IsInvocation))
            {
                Entity.Mob monster = (Entity.Mob)entity;
                if (Database.table.Drops.DropsTables.ContainsKey(monster.id))
                {
                    foreach (var drop in Database.table.Drops.DropsTables[monster.id])
                    {

                        if (CanDrop(drop, monster.Grade))
                        {
                            if (possibleDrops.ContainsKey(drop.ID))
                            {
                                possibleDrops[drop.ID]++;
                            }
                            else
                            {
                                possibleDrops.Add(drop.ID, 1);
                            }
                        }

                    }
                }
            }

            var winHumanTeam = this.WinerTeam.FindAll(x => x.IsHuman && !x.IsInvocation);

            /* Assign drop to fighter */
            foreach (var loot in possibleDrops)
            {
                for (int i = 0; i <= loot.Value - 1; i++)
                {
                    if (winHumanTeam.Count > 0)
                    {
                        var randomFighter = winHumanTeam[Util.rng.Next(0, winHumanTeam.Count)];
                        randomFighter.FightInfo.AddDrop(loot.Key);
                    }
                }
            }

            foreach (var item in PlayerInFight)
            {
                if (WinerTeam.Contains(item.account.character))
                {
                    item.account.character.FightInfo.GenerateInInventory(item);
                }
            }
        }

        public void QuitBattle(listenClient player, bool loop = false, bool abandon = false)
        {
            SendToAllFight("GM|-" + player.account.character.ID_InFight);
            if (player.account.character.IsHuman)
            {
                if (FightType == FightTypeEnum.PvM)
                {
                    GestionFight.RemoveInvo(player.account.character);
                    player.account.character.Vie = 1;
                    Map.MapGestion.SetCharacterInMap(player);
                    PlayerInFight.Remove(player);
                    AllEntityInFight.Remove(player.account.character);
                    Map.MapMouvement.SwitchMap(player, player.account.character.MapSpawnPoint, player.account.character.CellSpawnPoint);
                    player.send("GV");
                }
            }
            CheckEnd();
            player.account.character.CurrentFight = null;

        }


    }
}
