using LeafEmu.World.Network;
using LeafEmu.World.World;
using System;
using System.Collections.Generic;
using System.Text;
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
        public readonly long StartTime = Util.GetUnixTime;
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
        int pos = -1;
        Entity.Entity TourEntity;
        public int ActionToDoAtFrame = 1;

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

        public async void start()
        {
            FightStade = 1;
            SetOrderFight();
            SendToAllFight(CreateGTMPacket());
            map.SendToAllMap("Gc-" + PlayerInFight[0].account.character.FightInfo.FightID);
            TourEntity = AllEntityInFight[0];
            SendToAllFight("BN");
            await WorldServer.MethodSleep(1000);
            await PlayTurn();
        }

        private async Task PlayTurn()
        {
            SendToAllFight($"{CreateGTMPacket(true)}\0GTS{TourEntity.ID_InFight}|{TimeTurn}");
            TourEntity.FightInfo.YourTurn = true;
            TourEntity.CurrentFight = this;
            if (TourEntity.IsHuman)
            {
                StopTurn = new CancellationTokenSource();
                waitPlayTurn(TimeTurn);
            }
            else
            {
                if (TourEntity.Vie > 0)
                {
                    try
                    {
                        await WorldServer.MethodSleep(1000);
                        Entity.Mob mob = (Entity.Mob)TourEntity;
                        await mob.AI.PlayAI();
                        TourEntity.FightInfo.YourTurn = false;
                        await WorldServer.MethodSleep(500);
                        addToWorldServerFightList();
                    }
                    catch (Exception)
                    { }
                }
                else
                    TourEntity.FightInfo.isDead = true;
            }
            
        }

        private void waitPlayTurn(int timeWait)
        {
            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(timeWait, StopTurn.Token);
                }
                catch (OperationCanceledException) {}
                catch {}
                TourEntity.FightInfo.YourTurn = false;
                addToWorldServerFightList();
            });
        }
        public async void GestionTurn(Entity.Entity nextEntity = null)
        {
            EndTurn();
            if (TourEntity.GetType() == typeof(Entity.Character))
                SendToPlayer((Entity.Character)TourEntity, Character.GestionCharacter.createAsPacket((Entity.Character)TourEntity));
            findNextEntity(nextEntity);
            await PlayTurn();
            //don't put code after TurnGestion for thread safe
        }

        private void addToWorldServerFightList() 
        {
            if (!FightRuning)
                return;
            ActionToDoAtFrame = ActionToDoAtFrame == 0 ? 1 : 0;
            if (!WorldServer.Fights.Contains(this))
                lock (WorldServer.Fights)
                {
                    WorldServer.Fights.Add(this);
                }
        }

        public void GestionEndTurn()
        {
            GlyphGestion(TourEntity);
            Buff.Buff.GestionBuff(TourEntity);
            FightRuning = AllEntityInFight.Count <= 1 ? false : true;
            TourEntity.UpdateStat();
            addToWorldServerFightList();
        }

        private void EndTurn()
        {
             SendToAllFight($"GTF{TourEntity.ID_InFight}\0GTR{TourEntity.ID_InFight}");
        }

        private void findNextEntity(Entity.Entity forceNextEntity = null)
        {
            if (forceNextEntity != null)
            {
                TourEntity = forceNextEntity;
                return;
            }
               
            while (FightRuning)
            {
                FightRuning = nbTour < 100;
                CheckEnd();
                if (!FightRuning)
                    return;
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
            StringBuilder GTMpacket = new StringBuilder("GTM");
            foreach (var entity in AllEntityInFight)
            {
                if (!entity.FightInfo.isDead)
                {
                    GTMpacket.Append("|").Append(entity.ID_InFight).Append(";")
                        .Append("0;")
                        .Append(entity.Vie).Append(";")
                        .Append(entity.PA).Append(";")
                        .Append(entity.PM).Append(";")
                        .Append(entity.FightInfo.FightCell).Append(";")
                        .Append(";")
                        .Append(entity.TotalVie);
                }
            }
            return GTMpacket.ToString();
        }

        private void SendToPlayer(Entity.Character character, string packet)
        {
            try
            {
                PlayerInFight.Find(x => x.account.character == character).send(packet);
            }
            catch (Exception)
            {

            }
           
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


        public static void SendEndToPlayer(Network.listenClient prmClient)
        {
            prmClient.send("GV");
            prmClient.send($"GCK|1|{prmClient.account.character.speudo}");
            prmClient.send(Character.GestionCharacter.createAsPacket(prmClient.account.character) + "ILS2000");
            Map.MapGestion.SetCharacterInMap(prmClient);
            prmClient.send("BT1633000182959\0fC0");
        }

        public async void CheckEnd()
        {
            if (ischeckEnd)
                return;

            ischeckEnd = true;
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
                FightRuning = false;
                await WorldServer.MethodSleep(2500);// Attent la fin de l'animation de mort
                if (FightStade == 0)
                    map.SendToAllMap($"Gc-{FightID}", true);
                map.FightInMap.Remove(this);
                GestionFight.EndFightResult(this);
                await WorldServer.MethodSleep(1000);//sinon le client affiche pas bien les resultat du combat
                foreach (listenClient otherFighter in PlayerInFight)
                {
                    if (FightType == FightTypeEnum.Challenge)
                    {
                        otherFighter.account.character.Vie = otherFighter.account.character.TotalVie;
                    }
                    otherFighter.account.character.FightInfo = new FightEntityInfo(20, -1, true);
                    otherFighter.account.character.State = EnumClientState.None;
                    otherFighter.account.character.Buffs.ForEach(x => x.LifeTime = 0);
                    Buff.Buff.GestionBuff(otherFighter.account.character);
                    otherFighter.account.character.Buffs.Clear();
                    otherFighter.account.character.resCaract();
                }
                foreach (var player in PlayerInFight)
                {
                    SendEndToPlayer(player);
                    if (FightType == FightTypeEnum.PvM && LoosersTeam.Contains(player.account.character))
                    {
                        Map.Mouvement.MapMouvement.SwitchMap(player, player.account.character.MapSpawnPoint, player.account.character.CellSpawnPoint);
                    }
                    else if (map.EndFightActions != null
                        && FightType == FightTypeEnum.PvM
                        && map.EndFightActions.Action == 0
                        && WinerTeam.Contains(player.account.character))
                    {
                        Map.Mouvement.MapMouvement.SwitchMap(player, map.EndFightActions.map, map.EndFightActions.cell);
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

        private void SetOrderFight()
        {
            List<Entity.Entity> OrderFither = new List<Entity.Entity>();
            Equipe[0].OrderBy(x => x.initiative);
            Equipe[1].OrderBy(x => x.initiative);
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
            CreateOrderPacketGTL();
            SendToAllFight($"GR{AllEntityInFight[0].ID_InFight}\0" + GICpacket + "\0GS\0" + GTLpacket);
            FightRuning = true;
        }
        public void CreateOrderPacketGTL()
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

        private bool CheckDropHunter(Entity.Character character, Item.Item drop)
        {
            var weapon = character.Inventaire.getItemByPos(Constant.ITEM_POS_ARME);
            return (weapon != null && weapon.Effects.Exists(x => x.ID == 795))
                && character.getMetierByID(41) != null;// && character.getMetierByID(41).get_lvl() >= drop.levelDrops;
        }

        private int CheckIfPlayerCanDropItem(Entity.Character character, Item.Item drop, int quantity)
        {
            bool itsOk = false;
            bool unique = false;
            switch (drop.ActionDrops)
            {
                case -2:
                    unique = true;
                    itsOk = true;
                    break;
                case -1:// All items without condition.
                    itsOk = true;
                    break;

                case 1:// Is meat so..
                    itsOk = CheckDropHunter(character, drop);
                    break;

                case 2:// Verification of the condition (MAP)
                    foreach (string id in drop.ConditionDrops.Split(','))
                        if (id.Equals(character.Map.Id.ToString()))
                            itsOk = true;
                    break;

                case 3:// Alignement
                    //if (this.getMapOld().getSubArea() == null)
                    //    break;
                    //switch (drop.ConditionDrops)
                    //{
                    //    case "0":
                    //        if (this.getMapOld().getSubArea().getAlignement() == 2)
                    //            itsOk = true;
                    //        break;
                    //    case "1":
                    //        if (this.getMapOld().getSubArea().getAlignement() == 1)
                    //            itsOk = true;
                    //        break;
                    //    case "2":
                    //        if (this.getMapOld().getSubArea().getAlignement() == 2)
                    //            itsOk = true;
                    //        break;
                    //    case "3":
                    //        if (this.getMapOld().getSubArea().getAlignement() == 3)
                    //            itsOk = true;
                    //        break;
                    //    default:
                    //        itsOk = true;
                    //        break;
                    //}
                    break;

                case 4: // Quete
                    if (ConditionParser.validConditions(character, "QE=" + drop.ConditionDrops))
                        itsOk = true;
                    break;

                case 5: // Dropable une seule fois
                    if (character == null) break;
                    if (character.FightInfo.Drops.ContainsKey(drop.ID)) break;
                    itsOk = true;
                    break;

                case 6: // Avoir l'objet
                    int item = int.Parse(drop.ConditionDrops);
                    if (item == 2039)
                    {
                        if (this.map.Id == 7388)
                        {
                            if (character.Inventaire.HasItemTemplate(item))
                                itsOk = true;
                        }
                        else
                            itsOk = false;
                    }
                    else if (character.Inventaire.HasItemTemplate(item))
                        itsOk = true;
                    break;

                case 7:// Verification of the condition (MAP) mais pas plusieurs fois
                    if (character.Inventaire.HasItemTemplate(drop.ID, 1))
                        break;
                    foreach(string id in drop.ConditionDrops.Split(','))
                    {
                        if (id.Equals(this.map.Id.ToString()))
                        {
                            itsOk = true;
                        }
                    }
                    break;

                case 8:// Win a specific quantity
                    string[] split = drop.ConditionDrops.Split(',');
                    quantity = Util.rng.Next(int.Parse(split[0]), int.Parse(split[1]));
                    itsOk = true;
                    break;

                case 9:// Relique minotoror
                    //if (character != null && Minotoror.isValidMap(player.getCurMap()))
                    //    itsOk = true;
                    break;

                case 999:// Drop for collector
                    itsOk = true;
                    break;

                default:
                    itsOk = true;
                    break;
            }

            

            if (!itsOk || unique)
                quantity = 0;
            
            return quantity;
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

                        randomFighter.FightInfo.AddDrop(loot.Key, CheckIfPlayerCanDropItem(
                            (Entity.Character)randomFighter, 
                            Database.table.Item.model_item.AllItems[loot.Key], 
                            loot.Value));
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
            SendEndToPlayer(player);
            if (FightType == FightTypeEnum.PvM)
            {
                Map.Mouvement.MapMouvement.SwitchMap(player, player.account.character.MapSpawnPoint, player.account.character.CellSpawnPoint);
            }
            GestionFight.RemoveInvo(player.account.character);
            player.account.character.Vie = 1;
            PlayerInFight.Remove(player);
            AllEntityInFight.Remove(player.account.character);
            CheckEnd();
            player.account.character.CurrentFight = null;
        }
    }
}
