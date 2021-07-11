using LeafEmu.World.Game.Spells.SpellsEffects.Effect;
using LeafEmu.World.PacketGestion;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeafEmu.World.Game.Spells
{
    class SpellsManagement
    {
        [PacketAttribute("SB")]
        public void UpSpells(Network.listenClient prmClient, string prmPacket)
        {
            if (int.TryParse(prmPacket.Split("\0")[0].Substring(2), out int SpellsID))
            {
                foreach (SpellsEntity Spells in prmClient.account.character.Spells)
                {

                    if (Spells.id == SpellsID)
                    {
                        if (prmClient.account.character.PSorts < Spells.level || Spells.level == 6)
                            return;
                        Game.Entity.Character character = prmClient.account.character;
                        character.PSorts -= Spells.level;
                        Spells.level++;
                        prmClient.send($"SUK{Spells.id}~{Spells.level}");
                        prmClient.send($"Ak{Database.table.Experience.ExperienceStat[character.level][0]},{character.XP},{Database.table.Experience.ExperienceStat[character.level + 1][0]}" +
                            $"|0|{character.capital}|{character.PSorts}|0~0,0,1,0,0,0|{character.TotalVie},{character.TotalVie}|{character.energie},10000|\0Ab");

                    }
                }
            }

        }

        public static string CreatesSpellsPacket(List<SpellsEntity> listSpells)
        {
            byte i = 0;
            StringBuilder packet = new StringBuilder();
            foreach (SpellsEntity Spells in listSpells)
            {
                packet.Append(Spells.id);
                packet.Append('~');
                packet.Append(Spells.level);
                packet.Append('~');
                packet.Append(i);
                packet.Append(';');
                i++;
            }
            return packet.ToString();
        }

        public static void AddSpells(Game.Entity.Character Character)
        {
            byte breed = Character.classe;
            int level = Character.level;
            switch (breed)
            {
                case 1:
                    if (level == 1)
                    {
                        Character.AddSpells(3);
                        Character.AddSpells(6);
                        Character.AddSpells(17);
                        break;
                    }
                    if (level == 3)
                        Character.AddSpells(4);//Renvoie de sort
                    if (level == 6)
                        Character.AddSpells(2);//Aveuglement
                    if (level == 9)
                        Character.AddSpells(1);//Armure Incandescente
                    if (level == 13)
                        Character.AddSpells(9);//Attaque nuageuse
                    if (level == 17)
                        Character.AddSpells(18);//Armure Aqueuse
                    if (level == 21)
                        Character.AddSpells(20);//Immunit�
                    if (level == 26)
                        Character.AddSpells(14);//Armure Venteuse
                    if (level == 31)
                        Character.AddSpells(19);//Bulle
                    if (level == 36)
                        Character.AddSpells(5);//Tr�ve
                    if (level == 42)
                        Character.AddSpells(16);//Science du b�ton
                    if (level == 48)
                        Character.AddSpells(8);//Retour du b�ton
                    if (level == 54)
                        Character.AddSpells(12);//glyphe d'Aveuglement
                    if (level == 60)
                        Character.AddSpells(11);//T�l�portation
                    if (level == 70)
                        Character.AddSpells(10);//Glyphe Enflamm�
                    if (level == 80)
                        Character.AddSpells(7);//Bouclier F�ca
                    if (level == 90)
                        Character.AddSpells(15);//Glyphe d'Immobilisation
                    if (level == 100)
                        Character.AddSpells(13);//Glyphe de Silence
                    if (level == 200)
                        Character.AddSpells(1901);//Invocation de Dopeul F�ca
                    break;

                case 2:
                    if (level == 1)
                    {
                        Character.AddSpells(34);
                        Character.AddSpells(21);
                        Character.AddSpells(23);
                        break;
                    }
                    if (level == 3)
                        Character.AddSpells(26);//B�n�diction Animale
                    if (level == 6)
                        Character.AddSpells(22);//D�placement F�lin
                    if (level == 9)
                        Character.AddSpells(35);//Invocation de Bouftou
                    if (level == 13)
                        Character.AddSpells(28);//Crapaud
                    if (level == 17)
                        Character.AddSpells(37);//Invocation de Prespic
                    if (level == 21)
                        Character.AddSpells(30);//Fouet
                    if (level == 26)
                        Character.AddSpells(27);//Piq�re Motivante
                    if (level == 31)
                        Character.AddSpells(24);//Corbeau
                    if (level == 36)
                        Character.AddSpells(33);//Griffe Cinglante
                    if (level == 42)
                        Character.AddSpells(25);//Soin Animal
                    if (level == 48)
                        Character.AddSpells(38);//Invocation de Sanglier
                    if (level == 54)
                        Character.AddSpells(36);//Frappe du Craqueleur
                    if (level == 60)
                        Character.AddSpells(32);//R�sistance Naturelle
                    if (level == 70)
                        Character.AddSpells(29);//Crocs du Mulou
                    if (level == 80)
                        Character.AddSpells(39);//Invocation de Bwork Mage
                    if (level == 90)
                        Character.AddSpells(40);//Invocation de Craqueleur
                    if (level == 100)
                        Character.AddSpells(31);//Invocation de Dragonnet Rouge
                    if (level == 200)
                        Character.AddSpells(1902);//Invocation de Dopeul Osamodas
                    break;

                case 3:
                    if (level == 1)
                    {
                        Character.AddSpells(51);
                        Character.AddSpells(43);
                        Character.AddSpells(41);
                        break;
                    }
                    if (level == 3)
                        Character.AddSpells(49);//Pelle Fantomatique
                    if (level == 6)
                        Character.AddSpells(42);//Chance
                    if (level == 9)
                        Character.AddSpells(47);//Bo�te de Pandore
                    if (level == 13)
                        Character.AddSpells(48);//Remblai
                    if (level == 17)
                        Character.AddSpells(45);//Cl� R�ductrice
                    if (level == 21)
                        Character.AddSpells(53);//Force de l'Age
                    if (level == 26)
                        Character.AddSpells(46);//D�sinvocation
                    if (level == 31)
                        Character.AddSpells(52);//Cupidit�
                    if (level == 36)
                        Character.AddSpells(44);//Roulage de Pelle
                    if (level == 42)
                        Character.AddSpells(50);//Maladresse
                    if (level == 48)
                        Character.AddSpells(54);//Maladresse de Masse
                    if (level == 54)
                        Character.AddSpells(55);//Acc�l�ration
                    if (level == 60)
                        Character.AddSpells(56);//Pelle du Jugement
                    if (level == 70)
                        Character.AddSpells(58);//Pelle Massacrante
                    if (level == 80)
                        Character.AddSpells(59);//Corruption
                    if (level == 90)
                        Character.AddSpells(57);//Pelle Anim�e
                    if (level == 100)
                        Character.AddSpells(60);//Coffre Anim�
                    if (level == 200)
                        Character.AddSpells(1903);//Invocation de Dopeul Enutrof
                    break;

                case 4:
                    if (level == 1)
                    {
                        Character.AddSpells(61);
                        Character.AddSpells(72);
                        Character.AddSpells(65);
                        break;
                    }
                    if (level == 3)
                        Character.AddSpells(66);//Poison insidieux
                    if (level == 6)
                        Character.AddSpells(68);//Fourvoiement
                    if (level == 9)
                        Character.AddSpells(63);//Coup Sournois
                    if (level == 13)
                        Character.AddSpells(74);//Double
                    if (level == 17)
                        Character.AddSpells(64);//Rep�rage
                    if (level == 21)
                        Character.AddSpells(79);//Pi�ge de Masse
                    if (level == 26)
                        Character.AddSpells(78);//Invisibilit� d'Autrui
                    if (level == 31)
                        Character.AddSpells(71);//Pi�ge Empoisonn�
                    if (level == 36)
                        Character.AddSpells(62);//Concentration de Chakra
                    if (level == 42)
                        Character.AddSpells(69);//Pi�ge d'Immobilisation
                    if (level == 48)
                        Character.AddSpells(77);//Pi�ge de Silence
                    if (level == 54)
                        Character.AddSpells(73);//Pi�ge r�pulsif
                    if (level == 60)
                        Character.AddSpells(67);//Peur
                    if (level == 70)
                        Character.AddSpells(70);//Arnaque
                    if (level == 80)
                        Character.AddSpells(75);//Pulsion de Chakra
                    if (level == 90)
                        Character.AddSpells(76);//Attaque Mortelle
                    if (level == 100)
                        Character.AddSpells(80);//Pi�ge Mortel
                    if (level == 200)
                        Character.AddSpells(1904);//Invocation de Dopeul Sram
                    break;

                case 5:
                    if (level == 1)
                    {
                        Character.AddSpells(82);
                        Character.AddSpells(81);
                        Character.AddSpells(83);
                        break;
                    }
                    if (level == 3)
                        Character.AddSpells(84);//Gelure
                    if (level == 6)
                        Character.AddSpells(100);//Sablier de X�lor
                    if (level == 9)
                        Character.AddSpells(92);//Rayon Obscur
                    if (level == 13)
                        Character.AddSpells(88);//T�l�portation
                    if (level == 17)
                        Character.AddSpells(93);//Fl�trissement
                    if (level == 21)
                        Character.AddSpells(85);//Flou
                    if (level == 26)
                        Character.AddSpells(96);//Poussi�re Temporelle
                    if (level == 31)
                        Character.AddSpells(98);//Vol du Temps
                    if (level == 36)
                        Character.AddSpells(86);//Aiguille Chercheuse
                    if (level == 42)
                        Character.AddSpells(89);//D�vouement
                    if (level == 48)
                        Character.AddSpells(90);//Fuite
                    if (level == 54)
                        Character.AddSpells(87);//D�motivation
                    if (level == 60)
                        Character.AddSpells(94);//Protection Aveuglante
                    if (level == 70)
                        Character.AddSpells(99);//Momification
                    if (level == 80)
                        Character.AddSpells(95);//Horloge
                    if (level == 90)
                        Character.AddSpells(91);//Frappe de X�lor
                    if (level == 100)
                        Character.AddSpells(97);//Cadran de X�lor
                    if (level == 200)
                        Character.AddSpells(1905);//Invocation de Dopeul X�lor
                    break;

                case 6:
                    if (level == 1)
                    {
                        Character.AddSpells(102);
                        Character.AddSpells(103);
                        Character.AddSpells(105);
                        break;
                    }
                    if (level == 3)
                        Character.AddSpells(109);//Bluff
                    if (level == 6)
                        Character.AddSpells(113);//Perception
                    if (level == 9)
                        Character.AddSpells(111);//Contrecoup
                    if (level == 13)
                        Character.AddSpells(104);//Tr�fle
                    if (level == 17)
                        Character.AddSpells(119);//Tout ou rien
                    if (level == 21)
                        Character.AddSpells(101);//Roulette
                    if (level == 26)
                        Character.AddSpells(107);//Topkaj
                    if (level == 31)
                        Character.AddSpells(116);//Langue R�peuse
                    if (level == 36)
                        Character.AddSpells(106);//Roue de la Fortune
                    if (level == 42)
                        Character.AddSpells(117);//Griffe Invocatrice
                    if (level == 48)
                        Character.AddSpells(108);//Esprit F�lin
                    if (level == 54)
                        Character.AddSpells(115);//Odorat
                    if (level == 60)
                        Character.AddSpells(118);//R�flexes
                    if (level == 70)
                        Character.AddSpells(110);//Griffe Joueuse
                    if (level == 80)
                        Character.AddSpells(112);//Griffe de Ceangal
                    if (level == 90)
                        Character.AddSpells(114);//Rekop
                    if (level == 100)
                        Character.AddSpells(120);//Destin d'Ecaflip
                    if (level == 200)
                        Character.AddSpells(1906);//Invocation de Dopeul Ecaflip
                    break;

                case 7:
                    if (level == 1)
                    {
                        Character.AddSpells(125);
                        Character.AddSpells(128);
                        Character.AddSpells(121);
                        break;
                    }
                    if (level == 3)
                        Character.AddSpells(124);//Mot Soignant
                    if (level == 6)
                        Character.AddSpells(122);//Mot Blessant
                    if (level == 9)
                        Character.AddSpells(126);//Mot Stimulant
                    if (level == 13)
                        Character.AddSpells(127);//Mot de Pr�vention
                    if (level == 17)
                        Character.AddSpells(123);//Mot Drainant
                    if (level == 21)
                        Character.AddSpells(130);//Mot Revitalisant
                    if (level == 26)
                        Character.AddSpells(131);//Mot de R�g�n�ration
                    if (level == 31)
                        Character.AddSpells(132);//Mot d'Epine
                    if (level == 36)
                        Character.AddSpells(133);//Mot de Jouvence
                    if (level == 42)
                        Character.AddSpells(134);//Mot Vampirique
                    if (level == 48)
                        Character.AddSpells(135);//Mot de Sacrifice
                    if (level == 54)
                        Character.AddSpells(129);//Mot d'Amiti�
                    if (level == 60)
                        Character.AddSpells(136);//Mot d'Immobilisation
                    if (level == 70)
                        Character.AddSpells(137);//Mot d'Envol
                    if (level == 80)
                        Character.AddSpells(138);//Mot de Silence
                    if (level == 90)
                        Character.AddSpells(139);//Mot d'Altruisme
                    if (level == 100)
                        Character.AddSpells(140);//Mot de Reconstitution
                    if (level == 200)
                        Character.AddSpells(1907);//Invocation de Dopeul Eniripsa
                    break;

                case 8:
                    if (level == 1)
                    {
                        Character.AddSpells(143);
                        Character.AddSpells(141);
                        Character.AddSpells(142);
                        break;
                    }
                    if (level == 3)
                        Character.AddSpells(144);//Compulsion
                    if (level == 6)
                        Character.AddSpells(145);//Ep�e Divine
                    if (level == 9)
                        Character.AddSpells(146);//Ep�e du Destin
                    if (level == 13)
                        Character.AddSpells(147);//Guide de Bravoure
                    if (level == 17)
                        Character.AddSpells(148);//Amplification
                    if (level == 21)
                        Character.AddSpells(154);//Ep�e Destructrice
                    if (level == 26)
                        Character.AddSpells(150);//Couper
                    if (level == 31)
                        Character.AddSpells(151);//Souffle
                    if (level == 36)
                        Character.AddSpells(155);//Vitalit�
                    if (level == 42)
                        Character.AddSpells(152);//Ep�e du Jugement
                    if (level == 48)
                        Character.AddSpells(153);//Puissance
                    if (level == 54)
                        Character.AddSpells(149);//Mutilation
                    if (level == 60)
                        Character.AddSpells(156);//Temp�te de Puissance
                    if (level == 70)
                        Character.AddSpells(157);//Ep�e C�leste
                    if (level == 80)
                        Character.AddSpells(158);//Concentration
                    if (level == 90)
                        Character.AddSpells(160);//Ep�e de Iop
                    if (level == 100)
                        Character.AddSpells(159);//Col�re de Iop
                    if (level == 200)
                        Character.AddSpells(1908);//Invocation de Dopeul Iop
                    break;

                case 9:
                    if (level == 1)
                    {
                        Character.AddSpells(161);
                        Character.AddSpells(169);
                        Character.AddSpells(164);
                        break;
                    }
                    if (level == 3)
                        Character.AddSpells(163);//Fl�che Glac�e
                    if (level == 6)
                        Character.AddSpells(165);//Fl�che enflamm�e
                    if (level == 9)
                        Character.AddSpells(172);//Tir Eloign�
                    if (level == 13)
                        Character.AddSpells(167);//Fl�che d'Expiation
                    if (level == 17)
                        Character.AddSpells(168);//Oeil de Taupe
                    if (level == 21)
                        Character.AddSpells(162);//Tir Critique
                    if (level == 26)
                        Character.AddSpells(170);//Fl�che d'Immobilisation
                    if (level == 31)
                        Character.AddSpells(171);//Fl�che Punitive
                    if (level == 36)
                        Character.AddSpells(166);//Tir Puissant
                    if (level == 42)
                        Character.AddSpells(173);//Fl�che Harcelante
                    if (level == 48)
                        Character.AddSpells(174);//Fl�che Cinglante
                    if (level == 54)
                        Character.AddSpells(176);//Fl�che Pers�cutrice
                    if (level == 60)
                        Character.AddSpells(175);//Fl�che Destructrice
                    if (level == 70)
                        Character.AddSpells(178);//Fl�che Absorbante
                    if (level == 80)
                        Character.AddSpells(177);//Fl�che Ralentissante
                    if (level == 90)
                        Character.AddSpells(179);//Fl�che Explosive
                    if (level == 100)
                        Character.AddSpells(180);//Ma�trise de l'Arc
                    if (level == 200)
                        Character.AddSpells(1909);//Invocation de Dopeul Cra
                    break;

                case 10:
                    if (level == 1)
                    {
                        Character.AddSpells(183);
                        Character.AddSpells(200);
                        Character.AddSpells(193);
                        break;
                    }
                    if (level == 3)
                        Character.AddSpells(198);//Sacrifice Poupesque
                    if (level == 6)
                        Character.AddSpells(195);//Larme
                    if (level == 9)
                        Character.AddSpells(182);//Invocation de la Folle
                    if (level == 13)
                        Character.AddSpells(192);//Ronce Apaisante
                    if (level == 17)
                        Character.AddSpells(197);//Puissance Sylvestre
                    if (level == 21)
                        Character.AddSpells(189);//Invocation de la Sacrifi�e
                    if (level == 26)
                        Character.AddSpells(181);//Tremblement
                    if (level == 31)
                        Character.AddSpells(199);//Connaissance des Poup�es
                    if (level == 36)
                        Character.AddSpells(191);//Ronce Multiples
                    if (level == 42)
                        Character.AddSpells(186);//Arbre
                    if (level == 48)
                        Character.AddSpells(196);//Vent Empoisonn�
                    if (level == 54)
                        Character.AddSpells(190);//Invocation de la Gonflable
                    if (level == 60)
                        Character.AddSpells(194);//Ronces Agressives
                    if (level == 70)
                        Character.AddSpells(185);//Herbe Folle
                    if (level == 80)
                        Character.AddSpells(184);//Feu de Brousse
                    if (level == 90)
                        Character.AddSpells(188);//Ronce Insolente
                    if (level == 100)
                        Character.AddSpells(187);//Invocation de la Surpuissante
                    if (level == 200)
                        Character.AddSpells(1910);//Invocation de Dopeul Sadida
                    break;

                case 11:
                    if (level == 1)
                    {
                        Character.AddSpells(432);
                        Character.AddSpells(431);
                        Character.AddSpells(434);
                        break;
                    }
                    if (level == 3)
                        Character.AddSpells(444);//D�robade
                    if (level == 6)
                        Character.AddSpells(449);//D�tour
                    if (level == 9)
                        Character.AddSpells(436);//Assaut
                    if (level == 13)
                        Character.AddSpells(437);//Ch�timent Agile
                    if (level == 17)
                        Character.AddSpells(439);//Dissolution
                    if (level == 21)
                        Character.AddSpells(433);//Ch�timent Os�
                    if (level == 26)
                        Character.AddSpells(443);//Ch�timent Spirituel
                    if (level == 31)
                        Character.AddSpells(440);//Sacrifice
                    if (level == 36)
                        Character.AddSpells(442);//Absorption
                    if (level == 42)
                        Character.AddSpells(441);//Ch�timent Vilatesque
                    if (level == 48)
                        Character.AddSpells(445);//Coop�ration
                    if (level == 54)
                        Character.AddSpells(438);//Transposition
                    if (level == 60)
                        Character.AddSpells(446);//Punition
                    if (level == 70)
                        Character.AddSpells(447);//Furie
                    if (level == 80)
                        Character.AddSpells(448);//Ep�e Volante
                    if (level == 90)
                        Character.AddSpells(435);//Tansfert de Vie
                    if (level == 100)
                        Character.AddSpells(450);//Folie Sanguinaire
                    if (level == 200)
                        Character.AddSpells(1911);//Invocation de Dopeul Sacrieur
                    break;

                case 12:
                    if (level == 1)
                    {
                        Character.AddSpells(686);
                        Character.AddSpells(692);
                        Character.AddSpells(687);
                        break;
                    }
                    if (level == 3)
                        Character.AddSpells(689);//Epouvante
                    if (level == 6)
                        Character.AddSpells(690);//Souffle Alcoolis�
                    if (level == 9)
                        Character.AddSpells(691);//Vuln�rabilit� Aqueuse
                    if (level == 13)
                        Character.AddSpells(688);//Vuln�rabilit� Incandescente
                    if (level == 17)
                        Character.AddSpells(693);//Karcham
                    if (level == 21)
                        Character.AddSpells(694);//Vuln�rabilit� Venteuse
                    if (level == 26)
                        Character.AddSpells(695);//Stabilisation
                    if (level == 31)
                        Character.AddSpells(696);//Chamrak
                    if (level == 36)
                        Character.AddSpells(697);//Vuln�rabilit� Terrestre
                    if (level == 42)
                        Character.AddSpells(698);//Souillure
                    if (level == 48)
                        Character.AddSpells(699);//Lait de Bambou
                    if (level == 54)
                        Character.AddSpells(700);//Vague � Lame
                    if (level == 60)
                        Character.AddSpells(701);//Col�re de Zato�shwan
                    if (level == 70)
                        Character.AddSpells(702);//Flasque Explosive
                    if (level == 80)
                        Character.AddSpells(703);//Pandatak
                    if (level == 90)
                        Character.AddSpells(704);//Pandanlku
                    if (level == 100)
                        Character.AddSpells(705);//Lien Spiritueux
                    if (level == 200)
                        Character.AddSpells(1912);//Invocation de Dopeul Pandawa
                    break;
            }
        }

        public static void LaunchSpells(SpellsStats StatsSpells, Entity.Entity entity, int cellsSpellsTarget)
        {
            if (StatsSpells.TauxEC != 0 && Util.rng.Next(0, StatsSpells.TauxEC) == 0) // Echec 
            {
                if (entity.IsHuman)
                {
                    entity.CurrentFight.StopTurn.Cancel();
                }
                entity.CurrentFight.SendToAllFight($"GA;305;{entity.id}");
                return;
            }
            entity.CurrentFight.SendToAllFight("GAS" + entity.id + $"\0GA;300;{entity.id};" +
                $"{StatsSpells.SpellsID},{cellsSpellsTarget},0,1,0,0,1");

            List<Map.Cell.Cell> cellsTargets = Pathfinding.Pathfinding.getCellListFromAreaString(entity, cellsSpellsTarget, entity.FightInfo.FightCell, StatsSpells.porteeType, 0, false);
            double chanceCC = entity.TotalCC * Math.E * 1.1 / Math.Log(entity.Agi + 12);
            chanceCC = chanceCC >= 2 ? chanceCC : 2;

            List<SpellsEffect.Spells> StatsSpells_Effect = StatsSpells.CCeffects.Count != 0 && Util.rng.Next(1, (int)Math.Floor(chanceCC) + 1) == 1 ?
                                                            StatsSpells.CCeffects : StatsSpells.effects;
            CaracEffect.PA(entity, -StatsSpells.PACost);
            entity.CurrentFight.SendToAllFight($"GAF0|" + entity.id);
            foreach (SpellsEffect.Spells Spell in StatsSpells_Effect)
            {
                SpellsEffect.Gestion.EffectGestion.Gestion(entity, Spell, cellsSpellsTarget, cellsTargets);
            }
        }

        public static bool CheckLaunchSpells(int CellStart, int CellTarget, SpellsStats StatsSpells, Entity.Entity Launcher, out string msg)
        {

            if (Launcher.CurrentFight.PreventsSpells.Exists(x => x == StatsSpells.SpellsID))
            {
                msg = "00;Impossible de lancer ce sort car il a déjà été lancer et est soumit une restriction";
                return false;
            }

            int MaxPO = StatsSpells.isModifPO ?
                            StatsSpells.maxPO + Launcher.PO :
                            StatsSpells.maxPO;

            int DistanceCells = Pathfinding.Pathfinding.getDistanceBetween(Launcher.CurrentFight.map, CellStart, CellTarget);

            //Check Pa, if TargetSpells est inferieur MaxPO and minimal distance respected
            if (Launcher.PA >= StatsSpells.PACost)
            {
                if (StatsSpells.minPO <= DistanceCells && MaxPO >= DistanceCells)
                {
                    //If Spells have LOS and there is a obstacle can't cast Spells 
                    if (StatsSpells.isEmptyCell && !Launcher.CurrentFight.map.Cells[CellTarget].IsWalkable() || StatsSpells.hasLDV && World.Los.GetLos(Launcher.CurrentFight.map, Launcher.CurrentFight.map.Cells[CellStart], Launcher.CurrentFight.map.Cells[CellTarget], new List<int>()))//!Pathfinding.checkLoS(Launcher.Map, CellStart, CellTarget, Launcher))
                    {
                        msg = $"1171";
                        return false;
                    }
                    msg = "";
                    Launcher.PA -= StatsSpells.PACost;
                    return true;
                }
                else
                    msg = $"1171;{StatsSpells.minPO}~{MaxPO}~{DistanceCells}";
            }
            else
                msg = $"1170;{Launcher.PA}~{StatsSpells.PACost}";
            return false;

        }





    }
}
