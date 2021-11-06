using LeafEmu.World.Game.Character;
using LeafEmu.World.Game.Fight;
using LeafEmu.World.Game.Item;
using LeafEmu.World.Game.Quest;
using LeafEmu.World.World;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeafEmu.World.Game.Entity
{
    public class Character : Entity
    {
        public EnumClientState State { get; set; }
        public Dictionary<int, Quest.QuestPlayer> questList { get; set; }
        public int NewCell { get; set; }
        public List<int> ListCellMove { get; set; }
        public int UseInteractifAtEndOfMove { get; set; }
        public Map.Map Map { get; set; }
        public bool newCharac = false;
        public int mapID { get; set; }
        public int MapSpawnPoint { get; set; }
        public int CellSpawnPoint { get; set; }
        public int cellID { get; set; }
        public int subArea { get; set; }
        public List<int> Zaaps;
        public int pods { get; set; }
        public int podsMax { get; set; }
        public double WaitMoving { get; set; }

        public Character(int _id, string _speudo, short _level, int _isDead, int _gfxID, int _cellID, int _mapID, int _couleur1, int _couleur2, int _couleur3, int _sexe, byte _classe, int _pods, int _podsMax,
             Int64 _XP, int _kamas, int _capital, int _PSorts, int _vie, int _energie, int _PA, int _PM, int _force, int _sagesse, int _chance, int _agi, int _intell, bool _newCharac, string PointSpawn)
        {
            questList = new Dictionary<int, Quest.QuestPlayer>();
            UseInteractifAtEndOfMove = -1;
            int[] infoSpawn = Array.ConvertAll(PointSpawn.Split(','), int.Parse);
            MapSpawnPoint = infoSpawn[0];
            CellSpawnPoint = infoSpawn[1];
            IsHuman = true;
            Inventaire = new Inventaire.Inventaire();
            ListCellMove = new List<int>();
            Zaaps = new List<int>();
            FightInfo = new FightEntityInfo(20);
            State = EnumClientState.None;
            NewCell = -1;
            id = _id;
            ID_InFight = id;
            speudo = _speudo;
            level = _level;
            mapID = _mapID;
            cellID = _cellID;
            isDead = _isDead;
            gfxID = _gfxID;
            couleur1 = _couleur1;
            couleur2 = _couleur2;
            couleur3 = _couleur3;
            classe = _classe;
            sexe = _sexe;
            pods = _pods;
            podsMax = _podsMax;
            XP = _XP;
            kamas = _kamas;
            capital = _capital;
            PSorts = _PSorts;
            energie = _energie;
            PA = _PA;
            PM = _PM;
            CaracForce = _force;
            CaracSagesse = _sagesse;
            CaracChance = _chance;
            CaracAgi = _agi;
            CaracIntell = _intell;
            newCharac = _newCharac;
            CaracVie = _vie;
            UpdateStat();
        }

        internal QuestPlayer getQuestPersoByQuestId(int questId)
        {
            return questList.Values.ToList().Find(x => x.quest.id == questId);
        }

        public void ChangeCell(int newCell)
        {
            if (FightInfo.InFight == 0)
                cellID = newCell;
            else
                FightInfo.FightCell = newCell;
        }

        public void AddSpells(int id, int level = 1)
        {
            Spells.Add(new Spells.SpellsEntity(id, level, Game.Spells.SpellsManagement.GetNextPosDispo(this)));

        }

        public void AddXp(Int64 XpToAdd, Network.listenClient PrmClient)
        {
            if (PrmClient == null)
                return;
            XP += XpToAdd;
            short LastLvl = level;
            for (short i = (short)(1 + level); i <= 200; i++)
            {
                if (Database.table.Experience.ExperienceStat[i][0] > XP)
                {
                    break;
                }

                else
                {
                    level = i;
                    Game.Spells.SpellsManagement.AddSpells(this);
                    PrmClient.send("AN" + this.level);
                }
            }
            if (LastLvl < level)
            {
                capital += 5 * (level - LastLvl);
                PSorts += level - LastLvl;
                UpdateStat();
                Vie = TotalVie = TotalVie + capital;
                PrmClient.send(GestionCharacter.createAsPacket(PrmClient.account.character));
            }
            PrmClient.GAME_SEND_Im_PACKET("08;" + XpToAdd);
        }

        internal void teleport(Network.listenClient prmClient, int _mapID, int _cellID)
        {
             Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, _mapID, _cellID);
        }

        internal object getMetierByID(int id)
        {
            return null;
        }

        public void UpdateEquipentStats()
        {
            if (FightInfo.InFight == 1 || FightInfo.InFight == 2)
            {
                return;
            }
            resCaract();
            EquipementVie = 0;
            EquipementPA = 0;
            EquipementPM = 0;
            EquipementIntell = 0;
            EquipementAgi = 0;
            EquipementChance = 0;
            EquipementSagesse = 0;
            EquipementForce = 0;
            EquipementPO = 0;
            EquipementDommages = 0;
            EquipementDommagesPieges = 0;
            EquipementCoupsCritique = 0;
            EquipementInitiative = 0;

            P_EquipementResistanceForce = 0;
            P_EquipementResistanceIntell = 0;
            P_EquipementResistanceEau = 0;
            P_EquipementResistanceAgi = 0;

            F_EquipementResistanceForce = 0;
            F_EquipementResistanceIntell = 0;
            F_EquipementResistanceEau = 0;
            F_EquipementResistanceAgi = 0;

            List<List<Effect>> stats = new List<List<Effect>>();
            

            foreach (var posID in new List<int>() { 1, 6, 7, 8, 15 })
            {
                var item = Inventaire.getItemByPos(posID);
                if (item != null)
                    stats.Add(item.Effects);
            }

            foreach (List<Effect> effect in stats)
            {
                foreach (Effect oneStat in effect)
                {
                    int valeur = oneStat.CurrentJet;
                    switch ((EffectEnum)oneStat.ID)
                    {
                        case EffectEnum.AddPA:
                            EquipementPA += valeur;
                            break;
                        case EffectEnum.AddDamageCritic:
                            EquipementCoupsCritique += valeur;
                            break;
                        case EffectEnum.AddInitiative:
                            EquipementInitiative += valeur;
                            break;
                        case EffectEnum.AddVitalite:
                            EquipementVie += valeur;
                            Vie += valeur;
                            break;
                        case EffectEnum.AddDamage:
                            EquipementDommages += valeur;
                            break;
                        case EffectEnum.AddPO:
                            EquipementPO += valeur;
                            break;
                        case EffectEnum.AddPM:
                            EquipementPM += valeur;
                            break;
                        case EffectEnum.AddForce://force
                            EquipementForce += valeur;
                            break;
                        case EffectEnum.AddAgilite://agi
                            EquipementAgi += valeur;
                            break;
                        case EffectEnum.AddChance://chance
                            EquipementChance += valeur;
                            break;
                        case EffectEnum.AddSagesse://sagesse
                            EquipementSagesse += valeur;
                            break;
                        case EffectEnum.AddIntelligence://intell
                            EquipementIntell += valeur;
                            break;
                        case EffectEnum.AddDamagePiege://DmgPiege
                            EquipementDommagesPieges += valeur;
                            break;
                        default:
                            break;
                    }
                }
            }
            UpdateStat();
        }

        public QuestPlayer getQuestPersoByQuest(Quest.Quest q)
        {
            return questList.ContainsKey(q.id) ? questList[q.id] : null;
        }

        public void resCaract()
        {
            if (level > 99)
            {
                PA = 7;
            }
            else
                PA = 6;
            PM = 3;
            TotalPA = PA + EquipementPA;
            TotalPM = PM + EquipementPM;
            TotalIntell = CaracIntell + EquipementIntell;
            TotalAgi = CaracAgi + EquipementAgi;
            TotalForce = CaracForce + EquipementForce;
            TotalSagesse = CaracSagesse + EquipementSagesse;
            TotalChance = CaracChance + EquipementChance;
            TotalVie = 45 + level * 5 + CaracVie + EquipementVie;
            Vie = TotalVie;
            Sagesse = TotalSagesse;
            Chance = TotalChance;
            Force = TotalForce;
            Agi = TotalAgi;
            Intell = TotalIntell;

            P_TotalResAgi = P_EquipementResistanceAgi;
            P_TotalResIntell = P_EquipementResistanceIntell;
            P_TotalResForce = P_EquipementResistanceForce;
            P_TotalResEau = P_EquipementResistanceEau;
            P_TotalResAgi = P_EquipementResistanceAgi;

            F_TotalResAgi = F_EquipementResistanceAgi;
            F_TotalResIntell = F_EquipementResistanceIntell;
            F_TotalResForce = F_EquipementResistanceForce;
            F_TotalResEau = F_EquipementResistanceEau;
            F_TotalResAgi = F_EquipementResistanceAgi;
        }

        public void addQuestPerso(QuestPlayer qPerso)
        {
            questList.Add(qPerso.id, qPerso);
        }

        internal void SetNewSpawn(int _mapID, int _cellID)
        {
            MapSpawnPoint = _mapID;
            CellSpawnPoint = _cellID;
        }

        internal bool isGhost => isDead == 0 && energie <= 0;

        public int DialogID { get; set; }
        public int IdCurrentTalkingNpc { get; set; }

        internal void SetKamas(Network.listenClient prmClient, long kamas)
        {
            prmClient.account.character.kamas = kamas;
            prmClient.send(GestionCharacter.createAsPacket(prmClient.account.character));
            prmClient.GAME_SEND_Im_PACKET("045;" + kamas);
        }
    }


}
