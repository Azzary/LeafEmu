using LeafEmu.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafEmu.World.World
{
    public static class Constant
    {
        //DEBUG
        public const int DEBUG_MAP_LIMIT = 30000;
        //Fight
        public const int TIME_START_FIGHT = 45000;
        public const int TIME_BY_TURN = 30000;
        //Phoenix
        public const string ALL_PHOENIX = "-11;-54|2;-12|-41;-17|5;-9|25;-4|36;5|12;12|10;19|-10;13|-14;31|-43;0|-60;-3|-58;18|24;-43|27;-33";
    //ETAT
        public const int ETAT_NEUTRE = 0;
        public const int ETAT_SAOUL = 1;
        public const int ETAT_CAPT_AME = 2;
        public const int ETAT_PORTEUR = 3;
        public const int ETAT_PEUREUX = 4;
        public const int ETAT_DESORIENTE = 5;
        public const int ETAT_ENRACINE = 6;
        public const int ETAT_PESANTEUR = 7;
        public const int ETAT_PORTE = 8;
        public const int ETAT_MOTIV_SYLVESTRE = 9;
        public const int ETAT_APPRIVOISEMENT = 10;
        public const int ETAT_CHEVAUCHANT = 11;
        public const int FIGHT_TYPE_CHALLENGE = 0;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //D�fies
        public const int FIGHT_TYPE_AGRESSION = 1;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Aggros
        public const int FIGHT_TYPE_CONQUETE = 2;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Conquete
        public const int FIGHT_TYPE_DOPEUL = 3;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Dopeuls de temple
        public const int FIGHT_TYPE_PVM = 4;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //PvM
        public const int FIGHT_TYPE_PVT = 5;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Percepteur
        public const int FIGHT_STATE_INIT = 1;
        public const int FIGHT_STATE_PLACE = 2;
        public const int FIGHT_STATE_ACTIVE = 3;
        public const int FIGHT_STATE_FINISHED = 4;
        //Items
        //Positions
        public const int ITEM_POS_NO_EQUIPED = -1;
        public const int ITEM_POS_AMULETTE = 0;
        public const int ITEM_POS_ARME = 1;
        public const int ITEM_POS_ANNEAU1 = 2;
        public const int ITEM_POS_CEINTURE = 3;
        public const int ITEM_POS_ANNEAU2 = 4;
        public const int ITEM_POS_BOTTES = 5;
        public const int ITEM_POS_COIFFE = 6;
        public const int ITEM_POS_CAPE = 7;
        public const int ITEM_POS_FAMILIER = 8;
        public const int ITEM_POS_DOFUS1 = 9;
        public const int ITEM_POS_DOFUS2 = 10;
        public const int ITEM_POS_DOFUS3 = 11;
        public const int ITEM_POS_DOFUS4 = 12;
        public const int ITEM_POS_DOFUS5 = 13;
        public const int ITEM_POS_DOFUS6 = 14;
        public const int ITEM_POS_BOUCLIER = 15;
        public const int ITEM_POS_DRAGODINDE = 16;
        //Objets dons, mutations, mal�diction, ..
        public const int ITEM_POS_MUTATION = 20;
        public const int ITEM_POS_ROLEPLAY_BUFF = 21;
        public const int ITEM_POS_PNJ_SUIVEUR = 24;
        public const int ITEM_POS_BENEDICTION = 23;
        public const int ITEM_POS_MALEDICTION = 22;
        public const int ITEM_POS_BONBON = 25;
        //Types
        public const int ITEM_TYPE_AMULETTE = 1;
        public const int ITEM_TYPE_ARC = 2;
        public const int ITEM_TYPE_BAGUETTE = 3;
        public const int ITEM_TYPE_BATON = 4;
        public const int ITEM_TYPE_DAGUES = 5;
        public const int ITEM_TYPE_EPEE = 6;
        public const int ITEM_TYPE_MARTEAU = 7;
        public const int ITEM_TYPE_PELLE = 8;
        public const int ITEM_TYPE_ANNEAU = 9;
        public const int ITEM_TYPE_CEINTURE = 10;
        public const int ITEM_TYPE_BOTTES = 11;
        public const int ITEM_TYPE_POTION = 12;
        public const int ITEM_TYPE_PARCHO_EXP = 13;
        public const int ITEM_TYPE_DONS = 14;
        public const int ITEM_TYPE_RESSOURCE = 15;
        public const int ITEM_TYPE_COIFFE = 16;
        public const int ITEM_TYPE_CAPE = 17;
        public const int ITEM_TYPE_FAMILIER = 18;
        public const int ITEM_TYPE_HACHE = 19;
        public const int ITEM_TYPE_OUTIL = 20;
        public const int ITEM_TYPE_PIOCHE = 21;
        public const int ITEM_TYPE_FAUX = 22;
        public const int ITEM_TYPE_DOFUS = 23;
        public const int ITEM_TYPE_QUETES = 24;
        public const int ITEM_TYPE_DOCUMENT = 25;
        public const int ITEM_TYPE_FM_POTION = 26;
        public const int ITEM_TYPE_TRANSFORM = 27;
        public const int ITEM_TYPE_BOOST_FOOD = 28;
        public const int ITEM_TYPE_BENEDICTION = 29;
        public const int ITEM_TYPE_MALEDICTION = 30;
        public const int ITEM_TYPE_RP_BUFF = 31;
        public const int ITEM_TYPE_PERSO_SUIVEUR = 32;
        public const int ITEM_TYPE_PAIN = 33;
        public const int ITEM_TYPE_CEREALE = 34;
        public const int ITEM_TYPE_FLEUR = 35;
        public const int ITEM_TYPE_PLANTE = 36;
        public const int ITEM_TYPE_BIERE = 37;
        public const int ITEM_TYPE_BOIS = 38;
        public const int ITEM_TYPE_MINERAIS = 39;
        public const int ITEM_TYPE_ALLIAGE = 40;
        public const int ITEM_TYPE_POISSON = 41;
        public const int ITEM_TYPE_BONBON = 42;
        public const int ITEM_TYPE_POTION_OUBLIE = 43;
        public const int ITEM_TYPE_POTION_METIER = 44;
        public const int ITEM_TYPE_POTION_SORT = 45;
        public const int ITEM_TYPE_FRUIT = 46;
        public const int ITEM_TYPE_OS = 47;
        public const int ITEM_TYPE_POUDRE = 48;
        public const int ITEM_TYPE_COMESTI_POISSON = 49;
        public const int ITEM_TYPE_PIERRE_PRECIEUSE = 50;
        public const int ITEM_TYPE_PIERRE_BRUTE = 51;
        public const int ITEM_TYPE_FARINE = 52;
        public const int ITEM_TYPE_PLUME = 53;
        public const int ITEM_TYPE_POIL = 54;
        public const int ITEM_TYPE_ETOFFE = 55;
        public const int ITEM_TYPE_CUIR = 56;
        public const int ITEM_TYPE_LAINE = 57;
        public const int ITEM_TYPE_GRAINE = 58;
        public const int ITEM_TYPE_PEAU = 59;
        public const int ITEM_TYPE_HUILE = 60;
        public const int ITEM_TYPE_PELUCHE = 61;
        public const int ITEM_TYPE_POISSON_VIDE = 62;
        public const int ITEM_TYPE_VIANDE = 63;
        public const int ITEM_TYPE_VIANDE_CONSERVEE = 64;
        public const int ITEM_TYPE_QUEUE = 65;
        public const int ITEM_TYPE_METARIA = 66;
        public const int ITEM_TYPE_LEGUME = 68;
        public const int ITEM_TYPE_VIANDE_COMESTIBLE = 69;
        public const int ITEM_TYPE_TEINTURE = 70;
        public const int ITEM_TYPE_EQUIP_ALCHIMIE = 71;
        public const int ITEM_TYPE_OEUF_FAMILIER = 72;
        public const int ITEM_TYPE_MAITRISE = 73;
        public const int ITEM_TYPE_FEE_ARTIFICE = 74;
        public const int ITEM_TYPE_PARCHEMIN_SORT = 75;
        public const int ITEM_TYPE_PARCHEMIN_CARAC = 76;
        public const int ITEM_TYPE_CERTIFICAT_CHANIL = 77;
        public const int ITEM_TYPE_RUNE_FORGEMAGIE = 78;
        public const int ITEM_TYPE_BOISSON = 79;
        public const int ITEM_TYPE_OBJET_MISSION = 80;
        public const int ITEM_TYPE_SAC_DOS = 81;
        public const int ITEM_TYPE_BOUCLIER = 82;
        public const int ITEM_TYPE_PIERRE_AME = 83;
        public const int ITEM_TYPE_CLEFS = 84;
        public const int ITEM_TYPE_PIERRE_AME_PLEINE = 85;
        public const int ITEM_TYPE_POPO_OUBLI_PERCEP = 86;
        public const int ITEM_TYPE_PARCHO_RECHERCHE = 87;
        public const int ITEM_TYPE_PIERRE_MAGIQUE = 88;
        public const int ITEM_TYPE_CADEAUX = 89;
        public const int ITEM_TYPE_FANTOME_FAMILIER = 90;
        public const int ITEM_TYPE_DRAGODINDE = 91;
        public const int ITEM_TYPE_BOUFTOU = 92;
        public const int ITEM_TYPE_OBJET_ELEVAGE = 93;
        public const int ITEM_TYPE_OBJET_UTILISABLE = 94;
        public const int ITEM_TYPE_PLANCHE = 95;
        public const int ITEM_TYPE_ECORCE = 96;
        public const int ITEM_TYPE_CERTIF_MONTURE = 97;
        public const int ITEM_TYPE_RACINE = 98;
        public const int ITEM_TYPE_FILET_CAPTURE = 99;
        public const int ITEM_TYPE_SAC_RESSOURCE = 100;
        public const int ITEM_TYPE_ARBALETE = 102;
        public const int ITEM_TYPE_PATTE = 103;
        public const int ITEM_TYPE_AILE = 104;
        public const int ITEM_TYPE_OEUF = 105;
        public const int ITEM_TYPE_OREILLE = 106;
        public const int ITEM_TYPE_CARAPACE = 107;
        public const int ITEM_TYPE_BOURGEON = 108;
        public const int ITEM_TYPE_OEIL = 109;
        public const int ITEM_TYPE_GELEE = 110;
        public const int ITEM_TYPE_COQUILLE = 111;
        public const int ITEM_TYPE_PRISME = 112;
        public const int ITEM_TYPE_OBJET_VIVANT = 113;
        public const int ITEM_TYPE_ARME_MAGIQUE = 114;
        public const int ITEM_TYPE_FRAGM_AME_SHUSHU = 115;
        public const int ITEM_TYPE_POTION_FAMILIER = 116;
        //Alignement
        public const int ALIGNEMENT_NEUTRE = -1;
        public const int ALIGNEMENT_BONTARIEN = 1;
        public const int ALIGNEMENT_BRAKMARIEN = 2;
        public const int ALIGNEMENT_MERCENAIRE = 3;
        //Elements
        public const int ELEMENT_NULL = -1;
        public const int ELEMENT_NEUTRE = 0;
        public const int ELEMENT_TERRE = 1;
        public const int ELEMENT_EAU = 2;
        public const int ELEMENT_FEU = 3;
        public const int ELEMENT_AIR = 4;
        //Classes
        public const int CLASS_FECA = 1;
        public const int CLASS_OSAMODAS = 2;
        public const int CLASS_ENUTROF = 3;
        public const int CLASS_SRAM = 4;
        public const int CLASS_XELOR = 5;
        public const int CLASS_ECAFLIP = 6;
        public const int CLASS_ENIRIPSA = 7;
        public const int CLASS_IOP = 8;
        public const int CLASS_CRA = 9;
        public const int CLASS_SADIDA = 10;
        public const int CLASS_SACRIEUR = 11;
        public const int CLASS_PANDAWA = 12;
        //Sexes
        public const int SEX_MALE = 0;
        public const int SEX_FEMALE = 1;
        //GamePlay
        public const int MAX_EFFECTS_ID = 1500;
        //Buff a v�rifier en d�but de tour
        public static readonly int[] BEGIN_TURN_BUFF = { 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 108 };
        //Buff des Armes
        public static readonly int[] ARMES_EFFECT_IDS = { 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 108 };
        //Buff a ne pas booster en cas de CC
        public static readonly int[] NO_BOOST_CC_IDS = { 101 };
        //Invocation Statiques
        public static readonly int[] STATIC_INVOCATIONS = { 282, 556, 2750, 7000 };                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    //Arbre et Cawotte s'tout :p
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    //Verif d'Etat au lancement d'un sort {spellID,stateID}, � completer avant d'activer
        public static readonly int[][] STATE_REQUIRED = { new int[] { 699, Constant.ETAT_SAOUL }, new int[] { 690, Constant.ETAT_SAOUL } };
        //Buff d�clench� en cas de frappe
        public static readonly int[] ON_HIT_BUFFS = { 9, 79, 107, 788, 606, 607, 608, 609, 611 };
        //Effects
        public const int STATS_ADD_PM2 = 78;
        public const int STATS_REM_PA = 101;
        public const int STATS_ADD_VIE = 110;
        public const int STATS_ADD_PA = 111;
        public const int STATS_MULTIPLY_DOMMAGE = 114;
        public const int STATS_ADD_CC = 115;
        public const int STATS_REM_PO = 116;
        public const int STATS_ADD_PO = 117;
        public const int STATS_ADD_FORC = 118;
        public const int STATS_ADD_AGIL = 119;
        public const int STATS_ADD_PA2 = 120;
        public const int STATS_ADD_DOMA = 121;
        public const int STATS_ADD_EC = 122;
        public const int STATS_ADD_CHAN = 123;
        public const int STATS_ADD_SAGE = 124;
        public const int STATS_ADD_VITA = 125;
        public const int STATS_ADD_INTE = 126;
        public const int STATS_REM_PM = 127;
        public const int STATS_ADD_PM = 128;
        public const int STATS_ADD_PERDOM = 138;
        public const int STATS_ADD_PDOM = 142;
        public const int STATS_REM_DOMA = 145;
        public const int STATS_REM_CHAN = 152;
        public const int STATS_REM_VITA = 153;
        public const int STATS_REM_AGIL = 154;
        public const int STATS_REM_INTE = 155;
        public const int STATS_REM_SAGE = 156;
        public const int STATS_REM_FORC = 157;
        public const int STATS_ADD_PODS = 158;
        public const int STATS_REM_PODS = 159;
        public const int STATS_ADD_AFLEE = 160;
        public const int STATS_ADD_MFLEE = 161;
        public const int STATS_REM_AFLEE = 162;
        public const int STATS_REM_MFLEE = 163;
        public const int STATS_ADD_MAITRISE = 165;
        public const int STATS_REM_PA2 = 168;
        public const int STATS_REM_PM2 = 169;
        public const int STATS_REM_CC = 171;
        public const int STATS_ADD_INIT = 174;
        public const int STATS_REM_INIT = 175;
        public const int STATS_ADD_PROS = 176;
        public const int STATS_REM_PROS = 177;
        public const int STATS_ADD_SOIN = 178;
        public const int STATS_REM_SOIN = 179;
        public const int STATS_CREATURE = 182;
        public const int STATS_ADD_RP_TER = 210;
        public const int STATS_ADD_RP_EAU = 211;
        public const int STATS_ADD_RP_AIR = 212;
        public const int STATS_ADD_RP_FEU = 213;
        public const int STATS_ADD_RP_NEU = 214;
        public const int STATS_REM_RP_TER = 215;
        public const int STATS_REM_RP_EAU = 216;
        public const int STATS_REM_RP_AIR = 217;
        public const int STATS_REM_RP_FEU = 218;
        public const int STATS_REM_RP_NEU = 219;
        public const int STATS_RETDOM = 220;
        public const int STATS_TRAPDOM = 225;
        public const int STATS_TRAPPER = 226;
        public const int STATS_ADD_R_FEU = 240;
        public const int STATS_ADD_R_NEU = 241;
        public const int STATS_ADD_R_TER = 242;
        public const int STATS_ADD_R_EAU = 243;
        public const int STATS_ADD_R_AIR = 244;
        public const int STATS_REM_R_FEU = 245;
        public const int STATS_REM_R_NEU = 246;
        public const int STATS_REM_R_TER = 247;
        public const int STATS_REM_R_EAU = 248;
        public const int STATS_REM_R_AIR = 249;
        public const int STATS_ADD_RP_PVP_TER = 250;
        public const int STATS_ADD_RP_PVP_EAU = 251;
        public const int STATS_ADD_RP_PVP_AIR = 252;
        public const int STATS_ADD_RP_PVP_FEU = 253;
        public const int STATS_ADD_RP_PVP_NEU = 254;
        public const int STATS_REM_RP_PVP_TER = 255;
        public const int STATS_REM_RP_PVP_EAU = 256;
        public const int STATS_REM_RP_PVP_AIR = 257;
        public const int STATS_REM_RP_PVP_FEU = 258;
        public const int STATS_REM_RP_PVP_NEU = 259;
        public const int STATS_ADD_R_PVP_TER = 260;
        public const int STATS_ADD_R_PVP_EAU = 261;
        public const int STATS_ADD_R_PVP_AIR = 262;
        public const int STATS_ADD_R_PVP_FEU = 263;
        public const int STATS_ADD_R_PVP_NEU = 264;
        //Effets ID & Buffs
        public const int EFFECT_PASS_TURN = 140;
        //Capture
        public const int CAPTURE_MONSTRE = 623;
        //Familier
        public const int STATS_PETS_PDV = 800;
        public const int STATS_PETS_POIDS = 806;
        public const int STATS_PETS_REPAS = 807;
        public const int STATS_PETS_DATE = 808;
        public const int STATS_PETS_EPO = 940;
        public const int STATS_PETS_SOUL = 717;
        // Objet d'�levage
        public const int STATS_RESIST = 812;
        // Other
        public const int STATS_TURN = 811;
        public const int STATS_EXCHANGE_IN = 983;
        public const int STATS_CHANGE_BY = 985;
        public const int STATS_BUILD_BY = 988;
        public const int STATS_NAME_TRAQUE = 989;
        public const int STATS_GRADE_TRAQUE = 961;
        public const int STATS_ALIGNEMENT_TRAQUE = 960;
        public const int STATS_NIVEAU_TRAQUE = 962;

        public const int STATS_DATE = 805;
        public const int STATS_NIVEAU = 962;
        public const int STATS_NAME_DJ = 814;
        public const int STATS_OWNER_1 = 987;//#4
        public const int STATS_SIGNATURE = 988;
        public const int ERR_STATS_XP = 1000;
        
        //Valeur des droits de guilde
        public static int G_BOOST = 2;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //G�rer les boost
        public static int G_RIGHT = 4;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //G�rer les droits
        public static int G_INVITE = 8;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Inviter de nouveaux membres
        public static int G_BAN = 16;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Bannir
        public static int G_ALLXP = 32;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //G�rer les r�partitions d'xp
        public static int G_HISXP = 256;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //G�rer sa r�partition d'xp
        public static int G_RANK = 64;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //G�rer les rangs
        public static int G_POSPERCO = 128;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Poser un percepteur
        public static int G_COLLPERCO = 512;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Collecter les percepteurs
        public static int G_USEENCLOS = 4096;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Utiliser les enclos
        public static int G_AMENCLOS = 8192;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Am�nager les enclos
        public static int G_OTHDINDE = 16384;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        //G�rer les montures des autres membres
        public static int H_GBLASON = 2;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Afficher blason pour membre de la guilde
        public static int H_OBLASON = 4;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Afficher blason pour les autres
        public static int H_GNOCODE = 8;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Entrer sans code pour la guilde
        public static int H_OCANTOPEN = 16;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Entrer impossible pour les non-guildeux
        public static int C_GNOCODE = 32;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Coffre sans code pour la guilde
        public static int C_OCANTOPEN = 64;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Coffre impossible pour les non-guildeux
        public static int H_GREPOS = 256;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Guilde droit au repos
        public static int H_GTELE = 128;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //Guilde droit a la TP
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    // Nom des documents (swfs) : Documents d'avis de recherche
        public static string HUNT_DETAILS_DOC = "71_0706251229";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                // PanMap d'explications
        public static string HUNT_FRAKACIA_DOC = "63_0706251124";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                // Frakacia Leukocythine
        public static string HUNT_AERMYNE_DOC = "100_0706251214";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            // Aermyne 'Braco' Scalptaras
        public static string HUNT_MARZWEL_DOC = "96_0706251201";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                // Marzwel le Gobelin
        public static string HUNT_BRUMEN_DOC = "68_0706251126";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                // Brumen Tinctorias
        public static string HUNT_MUSHA_DOC = "94_0706251138";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                // Musha l'Oni
        public static string HUNT_OGIVOL_DOC = "69_0706251058";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                // Ogivol Scarlacin
        public static string HUNT_PADGREF_DOC = "61_0802081743";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                // Padgref Demoel
        public static string HUNT_QILBIL_DOC = "67_0706251223";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                // Qil Bil
        public static string HUNT_ROK_DOC = "93_0706251135";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                // Rok Gnorok
        public static string HUNT_ZATOISHWAN_DOC = "98_0706251211";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                // Zatoïshwan
        public static string HUNT_LETHALINE_DOC = "65_0706251123";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                // Léthaline Sigisbul
        public static string HUNT_FOUDUGLEN_DOC = "70_0706251122";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                // Fouduglen l'�cureuil

        // {(int)BorneId, (int)CellId, (str)SwfDocName, (int)MobId, (int)ItemFollow, (int)QuestId, (int)reponseID
        public static readonly String[][] HUNTING_QUESTS = { new string[]{ "1988", "234", HUNT_DETAILS_DOC, "-1", "-1", "-1", "-1" },
            new string[]{ "1986", "161", HUNT_LETHALINE_DOC, "-1", "-1", "-1", "-1" },
            new string[]{ "1985", "119", HUNT_MARZWEL_DOC, "554", "7353", "117", "2552" },
            new string[]{ "1986", "120", HUNT_PADGREF_DOC, "459", "6870", "29", "2108" },
            new string[]{ "1985", "149", HUNT_FRAKACIA_DOC, "460", "6871", "30", "2109" },
            new string[]{ "1986", "150", HUNT_QILBIL_DOC, "481", "6873", "32", "2111" },
            new string[]{ "1986", "179", HUNT_BRUMEN_DOC, "464", "6874", "33", "2112" },
            new string[]{ "1986", "180", HUNT_OGIVOL_DOC, "462", "6876", "35", "2114" },
            new string[]{ "1985", "269", HUNT_MUSHA_DOC, "552", "7352", "116", "2551" },
            new string[]{ "1986", "270", HUNT_FOUDUGLEN_DOC, "463", "6875", "34", "2113" },
            new string[]{ "1985", "299", HUNT_ROK_DOC, "550", "7351", "115", "2550" },
            new string[]{ "1986", "300", HUNT_AERMYNE_DOC, "446", "7350", "119", "2554" },
            new string[]{ "1985", "329", HUNT_ZATOISHWAN_DOC, "555", "7354", "118", "2553" }, };

        public static int getQuestByMobSkin(int mobSkin)
        {
            for (int v = 0; v < HUNTING_QUESTS.Length; v++)
                if (Database.table.Monster.Mobs.ContainsKey(int.Parse(HUNTING_QUESTS[v][3]))
                        && Database.table.Monster.Mobs[(int.Parse(HUNTING_QUESTS[v][3]))][0].gfxID == mobSkin)
                    return int.Parse(HUNTING_QUESTS[v][5]);
            return -1;
        }

        public static int getSkinByHuntMob(int mobId)
        {
            for (int v = 0; v < HUNTING_QUESTS.Length; v++)
                if (int.Parse(HUNTING_QUESTS[v][3]) == mobId)
                    return Database.table.Monster.Mobs[(mobId)][0].gfxID;
            return -1;
        }

        public static int getItemByHuntMob(int mobId)
        {
            for (int v = 0; v < HUNTING_QUESTS.Length; v++)
                if (int.Parse(HUNTING_QUESTS[v][3]) == mobId)
                    return int.Parse(HUNTING_QUESTS[v][4]);
            return -1;
        }

        public static int getItemByMobSkin(int mobSkin)
        {
            for (int v = 0; v < HUNTING_QUESTS.Length; v++)
                if (Database.table.Monster.Mobs.ContainsKey(int.Parse(HUNTING_QUESTS[v][3]))
                        && Database.table.Monster.Mobs[(int.Parse(HUNTING_QUESTS[v][3]))][0].gfxID == mobSkin)
                    return int.Parse(HUNTING_QUESTS[v][4]);
            return -1;
        }

        public static string getDocNameByBornePos(int borneId, int cellid)
        {
            for (int v = 0; v < HUNTING_QUESTS.Length; v++)
                if (int.Parse(HUNTING_QUESTS[v][0]) == borneId
                        && int.Parse(HUNTING_QUESTS[v][1]) == cellid)
                    return HUNTING_QUESTS[v][2];
            return string.Empty;
        }

        public static short getClassStatueMap(int classID)
        {
            short pos = 10298;
            switch (classID)
            {
                case 1:
                    return 7398;
                case 2:
                    return 7545;
                case 3:
                    return 7442;
                case 4:
                    return 7392;
                case 5:
                    return 7332;
                case 6:
                    return 7446;
                case 7:
                    return 7361;
                case 8:
                    return 7427;
                case 9:
                    return 7378;
                case 10:
                    return 7395;
                case 11:
                    return 7336;
                case 12:
                    return 8035;
                case 13:
                    return 7427;
            }
            return pos;
        }
        public static int getClassStatueCell(int classID)
        {
            int pos = 314;
            switch (classID)
            {
                case 1:
                    return 299;
                case 2:
                    return 311;
                case 3:
                    return 255;
                case 4:
                    return 282;
                case 5:
                    return 326;
                case 6:
                    return 300;
                case 7:
                    return 207;
                case 8:
                    return 282;
                case 9:
                    return 368;
                case 10:
                    return 370;
                case 11:
                    return 197;
                case 12:
                    return 384;
                case 13:
                    return 282;
            }
            return pos;
        }
        public static short getStartMap(int classID)
        {
            short pos = 10298;
            switch (classID)
            {
                case Constant.CLASS_FECA:
                    pos = 10300;
                    break;
                case Constant.CLASS_OSAMODAS:
                    pos = 10284;
                    break;
                case Constant.CLASS_ENUTROF:
                    pos = 10299;
                    break;
                case Constant.CLASS_SRAM:
                    pos = 10285;
                    break;
                case Constant.CLASS_XELOR:
                    pos = 10298;
                    break;
                case Constant.CLASS_ECAFLIP:
                    pos = 10276;
                    break;
                case Constant.CLASS_ENIRIPSA:
                    pos = 10283;
                    break;
                case Constant.CLASS_IOP:
                    pos = 10294;
                    break;
                case Constant.CLASS_CRA:
                    pos = 10292;
                    break;
                case Constant.CLASS_SADIDA:
                    pos = 10279;
                    break;
                case Constant.CLASS_SACRIEUR:
                    pos = 10296;
                    break;
                case Constant.CLASS_PANDAWA:
                    pos = 10289;
                    break;
            }

            return pos;
        }
        public static int getStartCell(int classID)
        {
            int pos = 314;
            switch (classID)
            {
                case Constant.CLASS_FECA:
                    pos = 323;
                    break;
                case Constant.CLASS_OSAMODAS:
                    pos = 372;
                    break;
                case Constant.CLASS_ENUTROF:
                    pos = 271;
                    break;
                case Constant.CLASS_SRAM:
                    pos = 263;
                    break;
                case Constant.CLASS_XELOR:
                    pos = 300;
                    break;
                case Constant.CLASS_ECAFLIP:
                    pos = 296;
                    break;
                case Constant.CLASS_ENIRIPSA:
                    pos = 299;
                    break;
                case Constant.CLASS_IOP:
                    pos = 280;
                    break;
                case Constant.CLASS_CRA:
                    pos = 284;
                    break;
                case Constant.CLASS_SADIDA:
                    pos = 254;
                    break;
                case Constant.CLASS_SACRIEUR:
                    pos = 243;
                    break;
                case Constant.CLASS_PANDAWA:
                    pos = 236;
                    break;
            }
            return pos;
        }

        public static Dictionary<int, char> getStartSortsPlaces(int classID)
        {
            Dictionary<int, char> start = new Dictionary<int, char>();
            switch (classID)
            {
                case CLASS_FECA:
                    start.Add(3, 'b');//Attaque Naturelle
                    start.Add(6, 'c');//Armure Terrestre
                    start.Add(17, 'd');//Glyphe Agressif
                    break;
                case CLASS_SRAM:
                    start.Add(61, 'b');//Sournoiserie
                    start.Add(72, 'c');//Invisibilit�
                    start.Add(65, 'd');//Piege sournois
                    break;
                case CLASS_ENIRIPSA:
                    start.Add(125, 'b');//Mot Interdit
                    start.Add(128, 'c');//Mot de Frayeur
                    start.Add(121, 'd');//Mot Curatif
                    break;
                case CLASS_ECAFLIP:
                    start.Add(102, 'b');//Pile ou Face
                    start.Add(103, 'c');//Chance d'ecaflip
                    start.Add(105, 'd');//Bond du felin
                    break;
                case CLASS_CRA:
                    start.Add(161, 'b');//Fleche Magique
                    start.Add(169, 'c');//Fleche de Recul
                    start.Add(164, 'd');//Fleche Empoisonn�e(ex Fleche chercheuse)
                    break;
                case CLASS_IOP:
                    start.Add(143, 'b');//Intimidation
                    start.Add(141, 'c');//Pression
                    start.Add(142, 'd');//Bond
                    break;
                case CLASS_SADIDA:
                    start.Add(183, 'b');//Ronce
                    start.Add(200, 'c');//Poison Paralysant
                    start.Add(193, 'd');//La bloqueuse
                    break;
                case CLASS_OSAMODAS:
                    start.Add(34, 'b');//Invocation de tofu
                    start.Add(21, 'c');//Griffe Spectrale
                    start.Add(23, 'd');//Cri de l'ours
                    break;
                case CLASS_XELOR:
                    start.Add(82, 'b');//Contre
                    start.Add(81, 'c');//Ralentissement
                    start.Add(83, 'd');//Aiguille
                    break;
                case CLASS_PANDAWA:
                    start.Add(686, 'b');//Picole
                    start.Add(692, 'c');//Gueule de bois
                    start.Add(687, 'd');//Poing enflamm�
                    break;
                case CLASS_ENUTROF:
                    start.Add(51, 'b');//Lancer de Piece
                    start.Add(43, 'c');//Lancer de Pelle
                    start.Add(41, 'd');//Sac anim�
                    break;
                case CLASS_SACRIEUR:
                    start.Add(432, 'b');//Pied du Sacrieur
                    start.Add(431, 'c');//Chatiment Os�
                    start.Add(434, 'd');//Attirance
                    break;
            }
            return start;
        }

        public static int getReqPtsToBoostStatsByClass(int classID, int statID,
                                                       int val)
        {
            switch (statID)
            {
                case 11://Vita
                    return 1;
                case 12://Sage
                    return 3;
                case 10://Force
                    switch (classID)
                    {
                        case CLASS_SACRIEUR:
                            return 3;

                        case CLASS_FECA:
                            if (val < 50)
                                return 2;
                            if (val < 150)
                                return 3;
                            if (val < 250)
                                return 4;
                            return 5;

                        case CLASS_XELOR:
                            if (val < 50)
                                return 2;
                            if (val < 150)
                                return 3;
                            if (val < 250)
                                return 4;
                            return 5;

                        case CLASS_SRAM:
                            if (val < 100)
                                return 1;
                            if (val < 200)
                                return 2;
                            if (val < 300)
                                return 3;
                            if (val < 400)
                                return 4;
                            return 5;

                        case CLASS_OSAMODAS:
                            if (val < 50)
                                return 2;
                            if (val < 150)
                                return 3;
                            if (val < 250)
                                return 4;
                            return 5;

                        case CLASS_ENIRIPSA:
                            if (val < 50)
                                return 2;
                            if (val < 150)
                                return 3;
                            if (val < 250)
                                return 4;
                            return 5;

                        case CLASS_PANDAWA:
                            if (val < 50)
                                return 1;
                            if (val < 200)
                                return 2;
                            return 3;

                        case CLASS_SADIDA:
                            if (val < 50)
                                return 1;
                            if (val < 250)
                                return 2;
                            if (val < 300)
                                return 3;
                            if (val < 400)
                                return 4;
                            return 5;

                        case CLASS_CRA:
                            if (val < 50)
                                return 1;
                            if (val < 150)
                                return 2;
                            if (val < 250)
                                return 3;
                            if (val < 350)
                                return 4;
                            return 5;

                        case CLASS_ENUTROF:
                            if (val < 50)
                                return 1;
                            if (val < 150)
                                return 2;
                            if (val < 250)
                                return 3;
                            if (val < 350)
                                return 4;
                            return 5;

                        case CLASS_ECAFLIP:
                            if (val < 100)
                                return 1;
                            if (val < 200)
                                return 2;
                            if (val < 300)
                                return 3;
                            if (val < 400)
                                return 4;
                            return 5;

                        case CLASS_IOP:
                            if (val < 100)
                                return 1;
                            if (val < 200)
                                return 2;
                            if (val < 300)
                                return 3;
                            if (val < 400)
                                return 4;
                            return 5;
                    }
                    break;
                case 13://Chance
                    switch (classID)
                    {
                        case CLASS_FECA:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;

                        case CLASS_XELOR:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;

                        case CLASS_SACRIEUR:
                            return 3;

                        case CLASS_SRAM:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;

                        case CLASS_SADIDA:
                            if (val < 100)
                                return 1;
                            if (val < 200)
                                return 2;
                            if (val < 300)
                                return 3;
                            if (val < 400)
                                return 4;
                            return 5;

                        case CLASS_PANDAWA:
                            if (val < 50)
                                return 1;
                            if (val < 200)
                                return 2;
                            return 3;

                        case CLASS_IOP:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;

                        case CLASS_ENUTROF:
                            if (val < 100)
                                return 1;
                            if (val < 150)
                                return 2;
                            if (val < 230)
                                return 3;
                            if (val < 330)
                                return 4;
                            return 5;

                        case CLASS_OSAMODAS:
                            if (val < 100)
                                return 1;
                            if (val < 200)
                                return 2;
                            if (val < 300)
                                return 3;
                            if (val < 400)
                                return 4;
                            return 5;

                        case CLASS_ECAFLIP:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;

                        case CLASS_ENIRIPSA:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;

                        case CLASS_CRA:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;
                    }
                    break;
                case 14://Agilit�
                    switch (classID)
                    {
                        case CLASS_FECA:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;

                        case CLASS_XELOR:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;

                        case CLASS_SACRIEUR:
                            return 3;

                        case CLASS_SRAM:
                            if (val < 100)
                                return 1;
                            if (val < 200)
                                return 2;
                            if (val < 300)
                                return 3;
                            if (val < 400)
                                return 4;
                            return 5;

                        case CLASS_SADIDA:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;

                        case CLASS_PANDAWA:
                            if (val < 50)
                                return 1;
                            if (val < 200)
                                return 2;
                            return 3;

                        case CLASS_ENIRIPSA:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;

                        case CLASS_IOP:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;

                        case CLASS_ENUTROF:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;

                        case CLASS_ECAFLIP:
                            if (val < 50)
                                return 1;
                            if (val < 100)
                                return 2;
                            if (val < 150)
                                return 3;
                            if (val < 200)
                                return 4;
                            return 5;

                        case CLASS_CRA:
                            if (val < 50)
                                return 1;
                            if (val < 100)
                                return 2;
                            if (val < 150)
                                return 3;
                            if (val < 200)
                                return 4;
                            return 5;

                        case CLASS_OSAMODAS:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;
                    }
                    break;
                case 15://Intelligence
                    switch (classID)
                    {
                        case CLASS_XELOR:
                            if (val < 100)
                                return 1;
                            if (val < 200)
                                return 2;
                            if (val < 300)
                                return 3;
                            if (val < 400)
                                return 4;
                            return 5;

                        case CLASS_FECA:
                            if (val < 100)
                                return 1;
                            if (val < 200)
                                return 2;
                            if (val < 300)
                                return 3;
                            if (val < 400)
                                return 4;
                            return 5;

                        case CLASS_SACRIEUR:
                            return 3;

                        case CLASS_SRAM:
                            if (val < 50)
                                return 2;
                            if (val < 150)
                                return 3;
                            if (val < 250)
                                return 4;
                            return 5;

                        case CLASS_SADIDA:
                            if (val < 100)
                                return 1;
                            if (val < 200)
                                return 2;
                            if (val < 300)
                                return 3;
                            if (val < 400)
                                return 4;
                            return 5;

                        case CLASS_ENUTROF:
                            if (val < 20)
                                return 1;
                            if (val < 60)
                                return 2;
                            if (val < 100)
                                return 3;
                            if (val < 140)
                                return 4;
                            return 5;

                        case CLASS_PANDAWA:
                            if (val < 50)
                                return 1;
                            if (val < 200)
                                return 2;
                            return 3;

                        case CLASS_IOP:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;

                        case CLASS_ENIRIPSA:
                            if (val < 100)
                                return 1;
                            if (val < 200)
                                return 2;
                            if (val < 300)
                                return 3;
                            if (val < 400)
                                return 4;
                            return 5;

                        case CLASS_CRA:
                            if (val < 50)
                                return 1;
                            if (val < 150)
                                return 2;
                            if (val < 250)
                                return 3;
                            if (val < 350)
                                return 4;
                            return 5;

                        case CLASS_OSAMODAS:
                            if (val < 100)
                                return 1;
                            if (val < 200)
                                return 2;
                            if (val < 300)
                                return 3;
                            if (val < 400)
                                return 4;
                            return 5;

                        case CLASS_ECAFLIP:
                            if (val < 20)
                                return 1;
                            if (val < 40)
                                return 2;
                            if (val < 60)
                                return 3;
                            if (val < 80)
                                return 4;
                            return 5;
                    }
                    break;
            }
            return 5;
        }

        public static int getGlyphColor(int spell)
        {
            switch (spell)
            {
                case 10://Enflamm�
                case 2033://Dopeul
                    return 4;//Rouge
                case 12://Aveuglement
                case 2034://Dopeul
                    return 3;
                case 13://Silence
                case 2035://Dopeul
                    return 6;//Bleu
                case 15://Immobilisation
                case 2036://Dopeul
                    return 5;//Vert
                case 17://Aggressif
                case 2037://Dopeul
                    return 2;
                case 949://Karkargo
                    return 0;//Blanc
                             //case 476://Blop
                default:
                    return 4;
            }
        }

        public static int getTrapsColor(int spell)
        {
            switch (spell)
            {
                case 65://Sournois
                    return 7;
                case 69://Immobilisation
                    return 10;
                case 71://Empoisonn�e
                case 2068://Dopeul
                    return 9;
                case 73://Repulsif
                    return 12;
                case 77://Silence
                case 2071://Dopeul
                    return 11;
                case 79://Masse
                case 2072://Dopeul
                    return 8;
                case 80://Mortel
                    return 13;
                default:
                    return 7;
            }
        }

        //public static Stats getMountStats(int color, int lvl)
        //{
        //    Stats stats = new Stats();
        //    switch (color)
        //    {
        //        //Amande sauvage
        //        case 1:
        //            break;
        //        //Ebene
        //        case 3:
        //            stats.addOneStat(STATS_ADD_VITA, lvl / 2);
        //            stats.addOneStat(STATS_ADD_AGIL, (int)(lvl / 1.25));//100/1.25 = 80
        //            break;
        //        //Rousse |
        //        case 10:
        //            stats.addOneStat(STATS_ADD_VITA, lvl); //100/1 = 100
        //            break;
        //        //Amande
        //        case 20:
        //            stats.addOneStat(STATS_ADD_INIT, lvl * 10); // 100*10 = 1000
        //            break;
        //        //Dor�e
        //        case 18:
        //            stats.addOneStat(STATS_ADD_VITA, lvl / 2);
        //            stats.addOneStat(STATS_ADD_SAGE, (int)(lvl / 2.50)); // 100/2.50 = 40
        //            break;
        //        //Rousse-Amande
        //        case 38:
        //            stats.addOneStat(STATS_ADD_INIT, lvl * 5); // 100*5 = 500
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_CREATURE, lvl / 50); // 100/50 = 2
        //            break;
        //        //Rousse-Dor�e
        //        case 46:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_SAGE, lvl / 4); //100/4 = 25
        //            break;
        //        //Amande-Dor�e
        //        case 33:
        //            stats.addOneStat(STATS_ADD_INIT, lvl * 5);
        //            stats.addOneStat(STATS_ADD_SAGE, lvl / 4);
        //            stats.addOneStat(STATS_ADD_VITA, lvl / 2);
        //            stats.addOneStat(STATS_CREATURE, lvl / 100); // 100/100 = 1
        //            break;
        //        //Indigo |
        //        case 17:
        //            stats.addOneStat(STATS_ADD_CHAN, (int)(lvl / 1.25));
        //            stats.addOneStat(STATS_ADD_VITA, lvl / 2);
        //            break;
        //        //Rousse-Indigo
        //        case 62:
        //            stats.addOneStat(STATS_ADD_VITA, (int)(lvl * 1.50)); // 100*1.50 = 150
        //            stats.addOneStat(STATS_ADD_CHAN, (int)(lvl / 1.65));
        //            break;
        //        //Rousse-Eb�ne
        //        case 12:
        //            stats.addOneStat(STATS_ADD_VITA, (int)(lvl * 1.50));
        //            stats.addOneStat(STATS_ADD_AGIL, (int)(lvl / 1.65));
        //            break;
        //        //Amande-Indigo
        //        case 36:
        //            stats.addOneStat(STATS_ADD_INIT, lvl * 5);
        //            stats.addOneStat(STATS_ADD_VITA, lvl / 2);
        //            stats.addOneStat(STATS_ADD_CHAN, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_CREATURE, lvl / 100);
        //            break;
        //        //Pourpre | Stade 4
        //        case 19:
        //            stats.addOneStat(STATS_ADD_FORC, (int)(lvl / 1.25));
        //            stats.addOneStat(STATS_ADD_VITA, lvl / 2);
        //            break;
        //        //Orchid�e
        //        case 22:
        //            stats.addOneStat(STATS_ADD_INTE, (int)(lvl / 1.25));
        //            stats.addOneStat(STATS_ADD_VITA, lvl / 2);
        //            break;
        //        //Dor�e-Orchid�e |
        //        case 48:
        //            stats.addOneStat(STATS_ADD_VITA, (lvl));
        //            stats.addOneStat(STATS_ADD_SAGE, lvl / 4);
        //            stats.addOneStat(STATS_ADD_INTE, (int)(lvl / 1.65));
        //            break;
        //        //Indigo-Pourpre
        //        case 65:
        //            stats.addOneStat(STATS_ADD_VITA, (lvl));
        //            stats.addOneStat(STATS_ADD_CHAN, lvl / 2);
        //            stats.addOneStat(STATS_ADD_FORC, lvl / 2);
        //            break;
        //        //Ivoire-Orchid�e
        //        case 67:
        //            stats.addOneStat(STATS_ADD_VITA, (lvl));
        //            stats.addOneStat(STATS_ADD_PERDOM, lvl / 2);
        //            stats.addOneStat(STATS_ADD_INTE, lvl / 2);
        //            break;
        //        //Eb�ne-Pourpre
        //        case 54:
        //            stats.addOneStat(STATS_ADD_VITA, (lvl));
        //            stats.addOneStat(STATS_ADD_FORC, lvl / 2);
        //            stats.addOneStat(STATS_ADD_AGIL, lvl / 2);
        //            break;
        //        //Eb�ne-Orchid�e
        //        case 53:
        //            stats.addOneStat(STATS_ADD_VITA, (lvl));
        //            stats.addOneStat(STATS_ADD_AGIL, lvl / 2);
        //            stats.addOneStat(STATS_ADD_INTE, lvl / 2);
        //            break;
        //        //Pourpre-Orchid�e
        //        case 76:
        //            stats.addOneStat(STATS_ADD_VITA, (lvl));
        //            stats.addOneStat(STATS_ADD_INTE, lvl / 2);
        //            stats.addOneStat(STATS_ADD_FORC, lvl / 2);
        //            break;
        //        // Amande-Ebene	| Nami-start
        //        case 37:
        //            stats.addOneStat(STATS_ADD_INIT, lvl * 5);
        //            stats.addOneStat(STATS_ADD_VITA, lvl / 2);
        //            stats.addOneStat(STATS_ADD_AGIL, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_CREATURE, lvl / 100);
        //            break;
        //        // Amande-Rousse
        //        case 44:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_SAGE, lvl / 4);
        //            stats.addOneStat(STATS_ADD_CHAN, (int)(lvl / 1.65));
        //            break;
        //        // Dor�e-Eb�ne
        //        case 42:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_SAGE, lvl / 4);
        //            stats.addOneStat(STATS_ADD_AGIL, (int)(lvl / 1.65));
        //            break;
        //        // Indigo-Eb�ne
        //        case 51:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_CHAN, lvl / 2);
        //            stats.addOneStat(STATS_ADD_AGIL, lvl / 2);
        //            break;
        //        // Rousse-Pourpre
        //        case 71:
        //            stats.addOneStat(STATS_ADD_VITA, (int)(lvl * 1.5));
        //            stats.addOneStat(STATS_ADD_FORC, (int)(lvl / 1.65));
        //            break;
        //        // Rousse-Orchid�e
        //        case 70:
        //            stats.addOneStat(STATS_ADD_VITA, (int)(lvl * 1.5));
        //            stats.addOneStat(STATS_ADD_INTE, (int)(lvl / 1.65));
        //            break;
        //        // Amande-Pourpre
        //        case 41:
        //            stats.addOneStat(STATS_ADD_INIT, lvl * 5);
        //            stats.addOneStat(STATS_ADD_VITA, lvl / 2);
        //            stats.addOneStat(STATS_ADD_FORC, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_CREATURE, lvl / 100);
        //            break;
        //        // Amande-Orchid�e
        //        case 40:
        //            stats.addOneStat(STATS_ADD_INIT, lvl * 5);
        //            stats.addOneStat(STATS_ADD_VITA, lvl / 2);
        //            stats.addOneStat(STATS_ADD_INTE, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_CREATURE, lvl / 100);
        //            break;
        //        // Dor�e-Pourpre
        //        case 49:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_SAGE, lvl / 4);
        //            stats.addOneStat(STATS_ADD_FORC, (int)(lvl / 1.65));
        //            break;
        //        // Ivoire
        //        case 16:
        //            stats.addOneStat(STATS_ADD_VITA, lvl / 2);
        //            stats.addOneStat(STATS_ADD_PERDOM, lvl / 2);
        //            break;
        //        // Turquoise
        //        case 15:
        //            stats.addOneStat(STATS_ADD_VITA, lvl / 2);
        //            stats.addOneStat(STATS_ADD_PROS, (int)(lvl / 1.25));
        //            break;
        //        //Rousse-Ivoire
        //        case 11:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 2); // 100*2 = 200
        //            stats.addOneStat(STATS_ADD_PERDOM, (int)(lvl / 2.5)); // = 40
        //            break;
        //        //Rousse-Turquoise
        //        case 69:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 2);
        //            stats.addOneStat(STATS_ADD_PROS, (int)(lvl / 2.50));
        //            break;
        //        //Amande-Turquoise
        //        case 39:
        //            stats.addOneStat(STATS_ADD_INIT, lvl * 5);
        //            stats.addOneStat(STATS_ADD_VITA, lvl / 2);
        //            stats.addOneStat(STATS_ADD_PROS, (int)(lvl / 2.50));
        //            stats.addOneStat(STATS_CREATURE, lvl / 100);
        //            break;
        //        //Dor�e-Ivoire
        //        case 45:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_PERDOM, (int)(lvl / 2.5));
        //            stats.addOneStat(STATS_ADD_SAGE, lvl / 4);
        //            break;
        //        //Dor�e-Turquoise
        //        case 47:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_PROS, (int)(lvl / 2.50));
        //            stats.addOneStat(STATS_ADD_SAGE, lvl / 4);
        //            break;
        //        //Indigo-Ivoire
        //        case 61:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_CHAN, (int)(lvl / 2.50));
        //            stats.addOneStat(STATS_ADD_PERDOM, (int)(lvl / 2.5));
        //            break;
        //        //Indigo-Turquoise
        //        case 63:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_CHAN, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_ADD_PROS, (int)(lvl / 2.5));
        //            break;
        //        //Eb�ne-Ivoire
        //        case 9:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_AGIL, (int)(lvl / 2.50));
        //            stats.addOneStat(STATS_ADD_PERDOM, (int)(lvl / 2.5));
        //            break;
        //        //Eb�ne-Turquoise
        //        case 52:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_AGIL, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_ADD_PROS, (int)(lvl / 2.50));
        //            break;
        //        //Pourpre-Ivoire
        //        case 68:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_FORC, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_ADD_PERDOM, (int)(lvl / 2.5));
        //            break;
        //        //Pourpre-Turquoise
        //        case 73:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_FORC, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_ADD_PROS, (int)(lvl / 2.50));
        //            break;
        //        //Orchid�e-Turquoise
        //        case 72:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_INTE, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_ADD_PERDOM, (int)(lvl / 2.5));
        //            break;
        //        //Ivoire-Turquoise
        //        case 66:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_PERDOM, (int)(lvl / 2.5));
        //            stats.addOneStat(STATS_ADD_PROS, (int)(lvl / 2.50));
        //            break;
        //        // Emeraude
        //        case 21:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 2);
        //            stats.addOneStat(STATS_ADD_PM, lvl / 100);
        //            break;
        //        // Prune
        //        case 23:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 2); // 100*2 = 200
        //            stats.addOneStat(STATS_ADD_PO, lvl / 50);
        //            break;
        //        //Emeraude-Rousse
        //        case 57:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 3); // 100*3 = 300
        //            stats.addOneStat(STATS_ADD_PM, lvl / 100);
        //            break;
        //        //Rousse-Prune
        //        case 84:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 3);
        //            stats.addOneStat(STATS_ADD_PO, lvl / 100);
        //            break;
        //        //Amande-Emeraude
        //        case 35:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_PM, lvl / 100);
        //            stats.addOneStat(STATS_CREATURE, lvl / 100);
        //            stats.addOneStat(STATS_ADD_INIT, lvl * 5);
        //            break;
        //        //Amande-Prune
        //        case 77:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 2);
        //            stats.addOneStat(STATS_ADD_INIT, lvl * 5);
        //            stats.addOneStat(STATS_ADD_PO, lvl / 100);
        //            stats.addOneStat(STATS_CREATURE, lvl / 100);
        //            break;
        //        //Dor�e-Emeraude
        //        case 43:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_SAGE, lvl / 4);
        //            stats.addOneStat(STATS_ADD_PM, lvl / 100);
        //            break;
        //        //Dor�e-Prune
        //        case 78:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 2);
        //            stats.addOneStat(STATS_ADD_SAGE, lvl / 4);
        //            stats.addOneStat(STATS_ADD_PO, lvl / 100);
        //            break;
        //        //Indigo-Emeraude
        //        case 55:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_CHAN, (int)(lvl / 3.33));
        //            stats.addOneStat(STATS_ADD_PM, lvl / 100);
        //            break;
        //        //Indigo-Prune
        //        case 82:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 2);
        //            stats.addOneStat(STATS_ADD_CHAN, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_ADD_PO, lvl / 100);
        //            break;
        //        //Eb�ne-Emeraude
        //        case 50:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_AGIL, (int)(lvl / 3.33));
        //            stats.addOneStat(STATS_ADD_PM, lvl / 100);
        //            break;
        //        //Eb�ne-Prune
        //        case 79:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 2);
        //            stats.addOneStat(STATS_ADD_AGIL, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_ADD_PO, lvl / 100);
        //            break;
        //        //Pourpre-Emeraude
        //        case 60:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_FORC, (int)(lvl / 3.33));
        //            stats.addOneStat(STATS_ADD_PM, lvl / 100);
        //            break;
        //        //Pourpre-Prune
        //        case 87:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 2);
        //            stats.addOneStat(STATS_ADD_FORC, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_ADD_PO, lvl / 100);
        //            break;
        //        //Orchid�e-Emeraude
        //        case 59:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_INTE, (int)(lvl / 3.33));
        //            stats.addOneStat(STATS_ADD_PM, lvl / 100);
        //            break;
        //        //Orchid�e-Prune
        //        case 86:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 2);
        //            stats.addOneStat(STATS_ADD_INTE, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_ADD_PO, lvl / 100);
        //            break;
        //        //Ivoire-Emeraude
        //        case 56:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_PERDOM, (int)(lvl / 3.33));
        //            stats.addOneStat(STATS_ADD_PM, lvl / 100);
        //            break;
        //        //Ivoire-Prune
        //        case 83:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 2);
        //            stats.addOneStat(STATS_ADD_PERDOM, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_ADD_PO, lvl / 100);
        //            break;
        //        //Turquoise-Emeraude
        //        case 58:
        //            stats.addOneStat(STATS_ADD_VITA, lvl);
        //            stats.addOneStat(STATS_ADD_PROS, (int)(lvl / 3.33));
        //            stats.addOneStat(STATS_ADD_PM, lvl / 100);
        //            break;
        //        //Turquoise-Prune
        //        case 85:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 2);
        //            stats.addOneStat(STATS_ADD_PROS, (int)(lvl / 1.65));
        //            stats.addOneStat(STATS_ADD_PO, lvl / 100);
        //            break;
        //        //Emeraude-Prune
        //        case 80:
        //            stats.addOneStat(STATS_ADD_VITA, lvl * 2);
        //            stats.addOneStat(STATS_ADD_PM, lvl / 100);
        //            stats.addOneStat(STATS_ADD_PO, lvl / 100);
        //            break;
        //        //Armure
        //        case 88:
        //            stats.addOneStat(STATS_ADD_PERDOM, lvl / 2);
        //            stats.addOneStat(STATS_ADD_RP_AIR, lvl / 20);
        //            stats.addOneStat(STATS_ADD_RP_EAU, lvl / 20);
        //            stats.addOneStat(STATS_ADD_RP_TER, lvl / 20);
        //            stats.addOneStat(STATS_ADD_RP_FEU, lvl / 20);
        //            stats.addOneStat(STATS_ADD_RP_NEU, lvl / 20);
        //            break;
        //        case 75:
        //            stats.addOneStat(STATS_ADD_PERDOM, lvl / 2);
        //            stats.addOneStat(STATS_ADD_RP_AIR, lvl / 20);
        //            stats.addOneStat(STATS_ADD_RP_EAU, lvl / 20);
        //            stats.addOneStat(STATS_ADD_RP_TER, lvl / 20);
        //            stats.addOneStat(STATS_ADD_RP_FEU, lvl / 20);
        //            stats.addOneStat(STATS_ADD_RP_NEU, lvl / 20);
        //            break;
        //    }
        //    return stats;
        //}

        //public static ObjectTemplate getParchoTemplateByMountColor(int color)
        //{
        //    switch (color)
        //    {
        //        //Ammande sauvage
        //        case 2:
        //            return World.world.getObjTemplate(7807);
        //        //Ebene | Page 1
        //        case 3:
        //            return World.world.getObjTemplate(7808);
        //        //Rousse sauvage
        //        case 4:
        //            return World.world.getObjTemplate(7809);
        //        //Ebene-ivoire
        //        case 9:
        //            return World.world.getObjTemplate(7810);
        //        //Rousse
        //        case 10:
        //            return World.world.getObjTemplate(7811);
        //        //Ivoire-Rousse
        //        case 11:
        //            return World.world.getObjTemplate(7812);
        //        //Ebene-rousse
        //        case 12:
        //            return World.world.getObjTemplate(7813);
        //        //Turquoise
        //        case 15:
        //            return World.world.getObjTemplate(7814);
        //        //Ivoire
        //        case 16:
        //            return World.world.getObjTemplate(7815);
        //        //Indigo
        //        case 17:
        //            return World.world.getObjTemplate(7816);
        //        //Dor�e
        //        case 18:
        //            return World.world.getObjTemplate(7817);
        //        //Pourpre
        //        case 19:
        //            return World.world.getObjTemplate(7818);
        //        //Amande
        //        case 20:
        //            return World.world.getObjTemplate(7819);
        //        //Emeraude
        //        case 21:
        //            return World.world.getObjTemplate(7820);
        //        //Orchid�e
        //        case 22:
        //            return World.world.getObjTemplate(7821);
        //        //Prune
        //        case 23:
        //            return World.world.getObjTemplate(7822);
        //        //Amande-Dor�e
        //        case 33:
        //            return World.world.getObjTemplate(7823);
        //        //Amande-Ebene
        //        case 34:
        //            return World.world.getObjTemplate(7824);
        //        //Amande-Emeraude
        //        case 35:
        //            return World.world.getObjTemplate(7825);
        //        //Amande-Indigo
        //        case 36:
        //            return World.world.getObjTemplate(7826);
        //        //Amande-Ivoire
        //        case 37:
        //            return World.world.getObjTemplate(7827);
        //        //Amande-Rousse
        //        case 38:
        //            return World.world.getObjTemplate(7828);
        //        //Amande-Turquoise
        //        case 39:
        //            return World.world.getObjTemplate(7829);
        //        //Amande-Orchid�e
        //        case 40:
        //            return World.world.getObjTemplate(7830);
        //        //Amande-Pourpre
        //        case 41:
        //            return World.world.getObjTemplate(7831);
        //        //Dor�e-Eb�ne
        //        case 42:
        //            return World.world.getObjTemplate(7832);
        //        //Dor�e-Emeraude
        //        case 43:
        //            return World.world.getObjTemplate(7833);
        //        //Dor�e-Indigo
        //        case 44:
        //            return World.world.getObjTemplate(7834);
        //        //Dor�e-Ivoire
        //        case 45:
        //            return World.world.getObjTemplate(7835);
        //        //Dor�e-Rousse | Page 2
        //        case 46:
        //            return World.world.getObjTemplate(7836);
        //        //Dor�e-Turquoise
        //        case 47:
        //            return World.world.getObjTemplate(7837);
        //        //Dor�e-Orchid�e
        //        case 48:
        //            return World.world.getObjTemplate(7838);
        //        //Dor�e-Pourpre
        //        case 49:
        //            return World.world.getObjTemplate(7839);
        //        //Eb�ne-Emeraude
        //        case 50:
        //            return World.world.getObjTemplate(7840);
        //        //Eb�ne-Indigo
        //        case 51:
        //            return World.world.getObjTemplate(7841);
        //        //Eb�ne-Turquoise
        //        case 52:
        //            return World.world.getObjTemplate(7842);
        //        //Eb�ne-Orchid�e
        //        case 53:
        //            return World.world.getObjTemplate(7843);
        //        //Eb�ne-Pourpre
        //        case 54:
        //            return World.world.getObjTemplate(7844);
        //        //Emeraude-Indigo
        //        case 55:
        //            return World.world.getObjTemplate(7845);
        //        //Emeraude-Ivoire
        //        case 56:
        //            return World.world.getObjTemplate(7846);
        //        //Emeraude-Rousse
        //        case 57:
        //            return World.world.getObjTemplate(7847);
        //        //Emeraude-Turquoise
        //        case 58:
        //            return World.world.getObjTemplate(7848);
        //        //Emeraude-Orchid�e
        //        case 59:
        //            return World.world.getObjTemplate(7849);
        //        //Emeraude-Pourpre
        //        case 60:
        //            return World.world.getObjTemplate(7850);
        //        //Indigo-Ivoire
        //        case 61:
        //            return World.world.getObjTemplate(7851);
        //        //Indigo-Rousse
        //        case 62:
        //            return World.world.getObjTemplate(7852);
        //        //Indigo-Turquoise
        //        case 63:
        //            return World.world.getObjTemplate(7853);
        //        //Indigo-Orchid�e
        //        case 64:
        //            return World.world.getObjTemplate(7854);
        //        //Indigo-Pourpre
        //        case 65:
        //            return World.world.getObjTemplate(7855);
        //        //Ivoire-Turquoise
        //        case 66:
        //            return World.world.getObjTemplate(7856);
        //        //Ivoire-Ochid�e
        //        case 67:
        //            return World.world.getObjTemplate(7857);
        //        //Ivoire-Pourpre
        //        case 68:
        //            return World.world.getObjTemplate(7858);
        //        //Turquoise-Rousse
        //        case 69:
        //            return World.world.getObjTemplate(7859);
        //        //Ochid�e-Rousse
        //        case 70:
        //            return World.world.getObjTemplate(7860);
        //        //Pourpre-Rousse
        //        case 71:
        //            return World.world.getObjTemplate(7861);
        //        //Turquoise-Orchid�e
        //        case 72:
        //            return World.world.getObjTemplate(7862);
        //        //Turquoise-Pourpre
        //        case 73:
        //            return World.world.getObjTemplate(7863);
        //        //Dor�e sauvage
        //        case 74:
        //            return World.world.getObjTemplate(7864);
        //        //Squelette
        //        case 75:
        //            return World.world.getObjTemplate(7865);
        //        //Orchid�e-Pourpre
        //        case 76:
        //            return World.world.getObjTemplate(7866);
        //        //Prune-Amande
        //        case 77:
        //            return World.world.getObjTemplate(7867);
        //        //Prune-Dor�e
        //        case 78:
        //            return World.world.getObjTemplate(7868);
        //        //Prune-Eb�ne
        //        case 79:
        //            return World.world.getObjTemplate(7869);
        //        //Prune-Emeraude
        //        case 80:
        //            return World.world.getObjTemplate(7870);
        //        //Prune et Indigo
        //        case 82:
        //            return World.world.getObjTemplate(7871);
        //        //Prune-Ivoire
        //        case 83:
        //            return World.world.getObjTemplate(7872);
        //        //Prune-Rousse
        //        case 84:
        //            return World.world.getObjTemplate(7873);
        //        //Prune-Turquoise
        //        case 85:
        //            return World.world.getObjTemplate(7874);
        //        //Prune-Orchid�e
        //        case 86:
        //            return World.world.getObjTemplate(7875);
        //        //Prune-Pourpre
        //        case 87:
        //            return World.world.getObjTemplate(7876);
        //        //Armure
        //        case 88:
        //            return World.world.getObjTemplate(9582);
        //    }
        //    return null;
        //}

        //public static int getMountColorByParchoTemplate(int tID)
        //{
        //    for (int a = 1; a < 100; a++)
        //        if (getParchoTemplateByMountColor(a) != null)
        //            if (getParchoTemplateByMountColor(a).ID == tID)
        //                return a;
        //    return -1;
        //}


        public static bool isValidPlaceForItem(Game.Item.Item template, int place)
        {
            if (template.Type == 41 && place == ITEM_POS_DRAGODINDE)
                return true;

            switch (template.Type)
            {
                case ITEM_TYPE_AMULETTE:
                    if (place == ITEM_POS_AMULETTE)
                        return true;
                    break;
                case 113:
                    if ((template.ID == 9233) && (place == 7))
                        return true;
                    if ((template.ID == 9234) && (place == 6))
                        return true;
                    if ((template.ID == 9255) && (place == 0))
                        return true;
                    if ((template.ID == 9256)
                            && ((place == 2) || (place == 4)))
                        return true;
                    break;
                case 114: // tourmenteurs
                    if (place == 1) // CaC
                        return true;
                    break;
                case ITEM_TYPE_ARC:
                case ITEM_TYPE_BAGUETTE:
                case ITEM_TYPE_BATON:
                case ITEM_TYPE_DAGUES:
                case ITEM_TYPE_EPEE:
                case ITEM_TYPE_MARTEAU:
                case ITEM_TYPE_PELLE:
                case ITEM_TYPE_HACHE:
                case ITEM_TYPE_OUTIL:
                case ITEM_TYPE_PIOCHE:
                case ITEM_TYPE_FAUX:
                case ITEM_TYPE_PIERRE_AME:
                case ITEM_TYPE_FILET_CAPTURE:
                    if (place == ITEM_POS_ARME)
                        return true;
                    break;

                case ITEM_TYPE_ANNEAU:
                    if (place == ITEM_POS_ANNEAU1 || place == ITEM_POS_ANNEAU2)
                        return true;
                    break;

                case ITEM_TYPE_CEINTURE:
                    if (place == ITEM_POS_CEINTURE)
                        return true;
                    break;

                case ITEM_TYPE_BOTTES:
                    if (place == ITEM_POS_BOTTES)
                        return true;
                    break;

                case ITEM_TYPE_COIFFE:
                    if (place == ITEM_POS_COIFFE)
                        return true;
                    break;

                case ITEM_TYPE_CAPE:
                case ITEM_TYPE_SAC_DOS:
                    if (place == ITEM_POS_CAPE)
                        return true;
                    break;

                case ITEM_TYPE_FAMILIER:
                    if (place == ITEM_POS_FAMILIER)
                        return true;
                    break;

                case ITEM_TYPE_DOFUS:
                    if (place == ITEM_POS_DOFUS1 || place == ITEM_POS_DOFUS2
                            || place == ITEM_POS_DOFUS3 || place == ITEM_POS_DOFUS4
                            || place == ITEM_POS_DOFUS5 || place == ITEM_POS_DOFUS6)
                        return true;
                    break;

                case ITEM_TYPE_BOUCLIER:
                    if (place == ITEM_POS_BOUCLIER)
                        return true;
                    break;

                //Barre d'objets : Normalement le client bloque les items interdits
                case ITEM_TYPE_POTION:
                case ITEM_TYPE_PARCHO_EXP:
                case ITEM_TYPE_BOOST_FOOD:
                case ITEM_TYPE_PAIN:
                case ITEM_TYPE_BIERE:
                case ITEM_TYPE_POISSON:
                case ITEM_TYPE_BONBON:
                case ITEM_TYPE_COMESTI_POISSON:
                case ITEM_TYPE_VIANDE:
                case ITEM_TYPE_VIANDE_CONSERVEE:
                case ITEM_TYPE_VIANDE_COMESTIBLE:
                case ITEM_TYPE_TEINTURE:
                case ITEM_TYPE_MAITRISE:
                case ITEM_TYPE_BOISSON:
                case ITEM_TYPE_PIERRE_AME_PLEINE:
                case ITEM_TYPE_PARCHO_RECHERCHE:
                case ITEM_TYPE_CADEAUX:
                case ITEM_TYPE_OBJET_ELEVAGE:
                case ITEM_TYPE_OBJET_UTILISABLE:
                case ITEM_TYPE_PRISME:
                case ITEM_TYPE_FEE_ARTIFICE:
                case ITEM_TYPE_DONS:
                    if (place >= 35 && place <= 48)
                        return true;
                    break;
            }
            return false;
        }

        /*
         * public static bool feedMount(int type) { for (int feed :
         * Main.itemFeedMount) { if (type == feed) return true; } return false; }
         */

        public static void tpCim(int idArea, Network.listenClient prmClient)
        {
            switch (idArea)
            {
                case 45:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 10342, 222);
                    break;
                case 0:
                case 5:
                case 29:
                case 39:
                case 40:
                case 43:
                case 44:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 1174, 279);
                    break;

                case 3:
                case 4:
                case 6:
                case 18:
                case 25:
                case 27:
                case 41:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 8534, 196);
                    break;

                case 2:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 420, 408);
                    break;

                case 1:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 844, 370);
                    break;

                case 7:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 4285, 572);
                    break;

                case 8:
                case 14:
                case 15:
                case 16:
                case 32:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 4748, 133);
                    break;

                case 11:
                case 12:
                case 13:
                case 33:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 5719, 196);
                    break;

                case 19:
                case 22:
                case 23:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 7910, 381);
                    break;

                case 20:
                case 21:
                case 24:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 8054, 115);
                    break;

                case 28:
                case 34:
                case 35:
                case 36:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 9231, 257);
                    break;
                case 30:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 9539, 128);
                    break;
                case 31:
                    if (prmClient.account.character.isGhost)
                        Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 9558, 268);
                    else
                        Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 9558, 224);
                    break;
                case 37:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 7796, 433);
                    break;
                case 42:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 8534, 196);
                    break;
                case 46:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 10422, 327);
                    break;
                case 47:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 10590, 302);
                    break;
                case 26:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 9398, 268);
                    break;
                default:
                    Game.Map.Mouvement.MapMouvement.SwitchMap(prmClient, 8534, 196);
                    break;
            }
        }

        public static bool isTaverne(Game.Map.Map map)
        {
            switch (map.Id)
            {
                case 7573:
                case 7572:
                case 7574:
                case 465:
                case 463:
                case 6064:
                case 461:
                case 462:
                case 5867:
                case 6197:
                case 6021:
                case 6044:
                case 8196:
                case 6055:
                case 8195:
                case 1905:
                case 1907:
                case 6049:
                    return true;
            }
            return false;
        }

        public static int getLevelForChevalier(Character target)
        {
            int lvl = target.level;
            if (lvl <= 50)
                return 50;
            if ((lvl <= 80) && (lvl > 50))
                return 80;
            if ((lvl <= 110) && (lvl > 80))
                return 110;
            if ((lvl <= 140) && (lvl > 110))
                return 140;
            if ((lvl <= 170) && (lvl > 140))
                return 170;
            if ((lvl <= 500) && (lvl > 170))
                return 200;
            return 200;
        }

        //public static string getStatsOfCandy(int id, int turn)
        //{
            
        //    String a = Database.table.Item.model_item.AllItems[id].ToString .getStrTemplate();
        //    a += ",32b#64#0#" + turn.ToString("X") + "#0d0+1;";
        //    return a;
        //}

        public static string getStatsOfMascotte()
        {
            String a = 148.ToString("X") + "#0#0#0#0d0+1,";
            a += "32b#64#0#" + 1.ToString("X") + "#0d0+1;";
            return a;
        }


        public static string getStringColorDragodinde(int color)
        {
            switch (color)
            {
                case 1: // Dragodinde Amande Sauvage
                    return "16772045,-1,16772045";
                case 3: // Dragodinde Ebène
                    return "1245184,393216,1245184";
                case 6: // Dragodinde Rousse Sauvage
                    return "16747520,-1,16747520";
                case 9: // Dragodinde Ebène et Ivoire
                    return "1182992,16777200,16777200";
                case 10: // Dragodinde Rousse
                    return "16747520,-1,16747520";
                case 11: // Dragodinde Ivoire et Rousse
                    return "16747520,16777200,16777200";
                case 12: // Dragodinde Ebène et Rousse
                    return "16747520,1703936,1774084";
                case 15: // Dragodinde Turquoise
                    return "4251856,-1,4251856";
                case 16: // Dragodinde Ivoire
                    return "16777200,16777200,16777200";
                case 17: // Dragodinde Indigo
                    return "4915330,-1,4915330";
                case 18: // Dragodinde Dorée
                    return "16766720,16766720,16766720";
                case 19: // Dragodinde Pourpre
                    return "14423100,-1,14423100";
                case 20: // Dragodinde Amande
                    return "16772045,-1,16772045";
                case 21: // Dragodinde Emeraude
                    return "3329330,-1,3329330";
                case 22: // Dragodinde Orchidée
                    return "15859954,16777200,15859954";
                case 23: // Dragodinde Prune
                    return "14524637,-1,14524637";
                case 33: // Dragodinde Amande et Dorée
                    return "16772045,16766720,16766720";
                case 34: // Dragodinde Amande et Ebène
                    return "16772045,1245184,1245184";
                case 35: // Dragodinde Amande et Emeraude
                    return "16772045,3329330,3329330";
                case 36: // Dragodinde Amande et Indigo
                    return "16772045,4915330,4915330";
                case 37: // Dragodinde Amande et Ivoire
                    return "16772045,16777200,16777200";
                case 38: // Dragodinde Amande et Rousse
                    return "16772045,16747520,16747520";
                case 39: // Dragodinde Amande et Turquoise
                    return "16772045,4251856,4251856";
                case 40: // Dragodinde Amande et Orchidée
                    return "16772045,15859954,15859954";
                case 41: // Dragodinde Amande et Pourpre
                    return "16772045,14423100,14423100";
                case 42: // Dragodinde Dorée et Ebène
                    return "1245184,16766720,16766720";
                case 43: // Dragodinde Dorée et Emeraude
                    return "16766720,3329330,3329330";
                case 44: // Dragodinde Dorée et Indigo
                    return "16766720,4915330,4915330";
                case 45: // Dragodinde Dorée et Ivoire
                    return "16766720,16777200,16777200";
                case 46: // Dragodinde Dorée et Rousse
                    return "16766720,16747520,16747520";
                case 47: // Dragodinde Dorée et Turquoise
                    return "16766720,4251856,4251856";
                case 48: // Dragodinde Dorée et Orchidée
                    return "16766720,15859954,15859954";
                case 49: // Dragodinde Dorée et Pourpre
                    return "16766720,14423100,14423100";
                case 50: // Dragodinde Ebène et Emeraude
                    return "1245184,3329330,3329330";
                case 51: // Dragodinde Ebène et Indigo
                    return "4915330,4915330,1245184";
                case 52: // Dragodinde Ebène et Turquoise
                    return "1245184,4251856,4251856";
                case 53: // Dragodinde Ebène et Orchidée
                    return "15859954,0,0";
                case 54: // Dragodinde Ebène et Pourpre
                    return "14423100,14423100,1245184";
                case 55: // Dragodinde Emeraude et Indigo
                    return "3329330,4915330,4915330";
                case 56: // Dragodinde Emeraude et Ivoire
                    return "3329330,16777200,16777200";
                case 57: // Dragodinde Emeraude et Rousse
                    return "3329330,16747520,16747520";
                case 58: // Dragodinde Emeraude et Turquoise
                    return "3329330,4251856,4251856";
                case 59: // Dragodinde Emeraude et Orchidée
                    return "3329330,15859954,15859954";
                case 60: // Dragodinde Emeraude et Pourpre
                    return "3329330,14423100,14423100";
                case 61: // Dragodinde Indigo et Ivoire
                    return "4915330,16777200,16777200";
                case 62: // Dragodinde Indigo et Rousse
                    return "4915330,16747520,16747520";
                case 63: // Dragodinde Indigo et Turquoise
                    return "4915330,4251856,4251856";
                case 64: // Dragodinde Indigo et Orchidée
                    return "4915330,15859954,15859954";
                case 65: // Dragodinde Indigo et Pourpre
                    return "14423100,4915330,4915330";
                case 66: // Dragodinde Ivoire et Turquoise
                    return "16777200,4251856,4251856";
                case 67: // Dragodinde Ivoire et Orchidée
                    return "16777200,16731355,16711910";
                case 68: // Dragodinde Ivoire et Pourpre
                    return "14423100,16777200,16777200";
                case 69: // Dragodinde Ivoire et Rousse
                    return "4251856,16747520,16747520";
                case 70: // Dragodinde Orchidée et Rousse
                    return "14315734,16747520,16747520";
                case 71: // Dragodinde Pourpre et Rousse
                    return "14423100,16747520,16747520";
                case 72: // Dragodinde Turquoise et Orchidée
                    return "15859954,4251856,4251856";
                case 73: // Dragodinde Turquoise et Pourpre
                    return "14423100,4251856,4251856";
                case 74: // Dragodinde Dorée et Rousse
                    return "16766720,16766720,16766720";
                case 76: // Dragodinde Orchidée et Pourpre
                    return "14315734,14423100,14423100";
                case 77: // Dragodinde Prune et Amande
                    return "14524637,16772045,16772045";
                case 78: // Dragodinde Prune et Dorée
                    return "14524637,16766720,16766720";
                case 79: // Dragodinde Prune et Ebène
                    return "14524637,1245184,1245184";
                case 80: // Dragodinde Prune et Emeraude
                    return "14524637,3329330,3329330";
                case 82: // Dragodinde Prune et Indigo
                    return "14524637,4915330,4915330";
                case 83: // Dragodinde Prune et Ivoire
                    return "14524637,16777200,16777200";
                case 84: // Dragodinde Prune et Rousse
                    return "14524637,16747520,16747520";
                case 85: // Dragodinde Prune et Turquoise
                    return "14524637,4251856,4251856";
                case 86: // Dragodinde Prune et Orchidée
                    return "14524637,15859954,15859954";
                case 87: // Dragodinde Prune et Pourpre
                    return "14524637,14423100,14423100";
                default:
                    return "-1,-1,-1";
            }
        }

        public static int getGeneration(int color)
        {
            switch (color)
            {
                case 10: // Rousse
                case 18: // Dorée
                case 20: // Amande
                    return 1;
                case 33: // Amande - Dorée
                case 38: // Amande - Rousse
                case 46: // Dorée - Rousse
                    return 2;
                case 3: // Ebène
                case 17: // Indigo
                    return 3;
                case 62: // Indigo - Rousse
                case 12: // Ebène - Rousse
                case 36: // Amande - Indigo
                case 34: // Amande - Ebène
                case 44: // Dorée - Indigo
                case 42: // Dorée - Ebène
                case 51: // Ebène - Indigo
                    return 4;
                case 19: // Purpre
                case 22: // Orchidée
                    return 5;
                case 71: // Purpre - Rousse
                case 70: // Orchidée - Rousse
                case 41: // Amande - Purpre
                case 40: // Amande - Orchidée
                case 49: // Dorée - Purpre
                case 48: // Dorée - Orchidée
                case 65: // Indigo - Purpre
                case 64: // Indigo - Orchidée
                case 54: // Ebène - Purpre
                case 53: // Ebène - Orchidée
                case 76: // Orchidée - Purpre
                    return 6;
                case 15: // Turquoise
                case 16: // Ivoire
                    return 7;
                case 11: // Ivoire - Rousse
                case 69: // Turquoise - Rousse
                case 37: // Amande - Ivoire
                case 39: // Amande - Turquoise
                case 45: // Dorée - Ivoire
                case 47: // Dorée - Turquoise
                case 61: // Indigo - Ivoire
                case 63: // Indigo - Turquoise
                case 9: // Ebène - Ivoire
                case 52: // Ebène - Turquoise
                case 68: // Ivoire - Purpre
                case 73: // Turquoise - Purpre
                case 67: // Ivoire - Orchidée
                case 72: // Orchidée - Turquoise
                case 66: // Ivoire - Turquoise
                    return 8;
                case 21: // Emeraude
                case 23: // Prune
                    return 9;
                case 57:// Emeraude - Rousse
                case 35: // Amande - Emeraude
                case 43: // Dorée - Emeraude
                case 50: // Ebène - Emeraude
                case 55: // Emeraude - Indigo
                case 56: // Emeraude - Ivoire
                case 58: // Emeraude - Turquoise
                case 59: // Emeraude - Orchidée
                case 60: // Emeraude - Purpre
                case 77: // Prune - Amande
                case 78: // Prune - Dorée
                case 79: // Prune - Ebène
                case 80: // Prune - Emeraude
                case 82: // Prune - Indigo
                case 83: // Prune - Ivoire
                case 84: // Prune - Rousse
                case 85: // Prune - Turquoise
                case 86: // Prune - Orchidée
                    return 10;
                default:
                    return 1;
            }
        }

        //public static int colorToEtable(Character player, Mount mother, Mount father)
        //{
        //    int color1, color2;
        //    int A = 0, B = 0, C = 0;

        //    String[] splitM = mother.getAncestors().split(","), splitF = father.getAncestors().split(",");
        //    RandomStats<int> random = new RandomStats<>();

        //    short i = 0;
        //    for (String str : splitM)
        //    {
        //        i++;
        //        if (str.equals("?")) continue;

        //        int pct = 1;

        //        switch (i)
        //        {
        //            case 1: case 2: pct = 25; break;
        //            case 3: case 4: case 5: case 6: pct = 10;
        //        }

        //        random.add(pct, int.Parse(str));
        //    }

        //    random.add(random.size() == 0 ? 100 : 33, mother.getColor());
        //    color1 = random.get();

        //    random = new RandomStats<>();
        //    i = 0;
        //    for (String str : splitF)
        //    {
        //        i++;
        //        if (str.equals("?")) continue;

        //        int pct = 1;

        //        switch (i)
        //        {
        //            case 1: case 2: pct = 25; break;
        //            case 3: case 4: case 5: case 6: pct = 10;
        //        }

        //        random.add(pct, int.Parse(str));
        //    }

        //    random.add(random.size() == 0 ? 100 : 33, father.getColor());
        //    color2 = random.get();

        //    if (color1 == 75)
        //        color1 = 10;
        //    if (color2 == 75)
        //        color2 = 10;

        //    if (color1 > color2)
        //    {
        //        A = color2;// moins
        //        B = color1;// supérieur
        //    }
        //    else if (color1 <= color2)
        //    {
        //        A = color1;// moins
        //        B = color2;// supérieur
        //    }
        //    if (A == 10 && B == 18)
        //        C = 46; // Rousse y Dorée
        //    else if (A == 10 && B == 20)
        //        C = 38; // Rousse y Amande
        //    else if (A == 18 && B == 20)
        //        C = 33; // Amande y Dorée
        //    else if (A == 33 && B == 38)
        //        C = 17; // Indigo
        //    else if (A == 33 && B == 46)
        //        C = 3;// Ebène
        //    else if (A == 10 && B == 17)
        //        C = 62; // Rousse e Indigo
        //    else if (A == 10 && B == 3)
        //        C = 12; // Ebène y Rousse
        //    else if (A == 17 && B == 20)
        //        C = 36; // Amande - Indigo
        //    else if (A == 3 && B == 20)
        //        C = 34; // Amande - Ebène
        //    else if (A == 17 && B == 18)
        //        C = 44; // Dorée - Indigo
        //    else if (A == 3 && B == 18)
        //        C = 42; // Dorée - Ebène
        //    else if (A == 3 && B == 17)
        //        C = 51; // Ebène - Indigo
        //    else if (A == 38 && B == 51)
        //        C = 19; // Purpre
        //    else if (A == 46 && B == 51)
        //        C = 22; // Orchidée
        //    else if (A == 10 && B == 19)
        //        C = 71; // Purpre - Rousse
        //    else if (A == 10 && B == 22)
        //        C = 70; // Orchidée - Rousse
        //    else if (A == 19 && B == 20)
        //        C = 41; // Amande - Purpre
        //    else if (A == 20 && B == 22)
        //        C = 40; // Amande - Orchidée
        //    else if (A == 18 && B == 19)
        //        C = 49; // Dorée - Purpre
        //    else if (A == 18 && B == 22)
        //        C = 48; // Dorée - Orchidée
        //    else if (A == 17 && B == 19)
        //        C = 65; // Indigo - Purpre
        //    else if (A == 17 && B == 22)
        //        C = 64; // Indigo - Orchidée
        //    else if (A == 3 && B == 19)
        //        C = 54; // Ebène - Purpre
        //    else if (A == 3 && B == 22)
        //        C = 53; // Ebène - Orchidée
        //    else if (A == 19 && B == 22)
        //        C = 76; // Orchidée - Purpre
        //    else if (A == 53 && B == 76)
        //        C = 15; // Turquoise
        //    else if (A == 65 && B == 76)
        //        C = 16; // Ivoire
        //    else if (A == 10 && B == 16)
        //        C = 11; // Ivoire - Rousse
        //    else if (A == 10 && B == 15)
        //        C = 69; // Turquoise - Rousse
        //    else if (A == 16 && B == 20)
        //        C = 37; // Amande - Ivoire
        //    else if (A == 15 && B == 20)
        //        C = 39; // Amande - Turquoise
        //    else if (A == 16 && B == 18)
        //        C = 45; // Dorée - Ivoire
        //    else if (A == 15 && B == 18)
        //        C = 47; // Dorée - Turquoise
        //    else if (A == 16 && B == 17)
        //        C = 61; // Indigo - Ivoire
        //    else if (A == 15 && B == 17)
        //        C = 63; // Indigo - Turquoise
        //    else if (A == 3 && B == 16)
        //        C = 9; // Ebène - Ivoire
        //    else if (A == 3 && B == 15)
        //        C = 52; // Ebène - Turquoise
        //    else if (A == 16 && B == 19)
        //        C = 68; // Ivoire - Purpre
        //    else if (A == 15 && B == 19)
        //        C = 73; // Turquoise - Purpre
        //    else if (A == 16 && B == 22)
        //        C = 67; // Ivoire - Orchidée
        //    else if (A == 15 && B == 22)
        //        C = 72; // Orchidée - Turquoise
        //    else if (A == 15 && B == 16)
        //        C = 66; // Ivoire - Turquoise
        //    else if (A == 66 && B == 68)
        //        C = 21; // Emeraude
        //    else if (A == 66 && B == 72)
        //        C = 23; // Prune
        //    else if (A == 10 && B == 21)
        //        C = 57;// Emeraude - Rousse
        //    else if (A == 20 && B == 21)
        //        C = 35; // Amande - Emeraude
        //    else if (A == 18 && B == 21)
        //        C = 43; // Dorée - Emeraude
        //    else if (A == 3 && B == 21)
        //        C = 50; // Ebène - Emeraude
        //    else if (A == 17 && B == 21)
        //        C = 55; // Emeraude - Indigo
        //    else if (A == 16 && B == 21)
        //        C = 56; // Emeraude - Ivoire
        //    else if (A == 15 && B == 21)
        //        C = 58; // Emeraude - Turquoise
        //    else if (A == 21 && B == 22)
        //        C = 59; // Emeraude - Orchidée
        //    else if (A == 19 && B == 21)
        //        C = 60; // Emeraude - Purpre
        //    else if (A == 20 && B == 23)
        //        C = 77; // Prune - Amande
        //    else if (A == 18 && B == 23)
        //        C = 78; // Prune - Dorée
        //    else if (A == 3 && B == 23)
        //        C = 79; // Prune - Ebène
        //    else if (A == 21 && B == 23)
        //        C = 80; // Prune - Emeraude
        //    else if (A == 17 && B == 23)
        //        C = 82; // Prune - Indigo
        //    else if (A == 16 && B == 23)
        //        C = 83; // Prune - Ivoire
        //    else if (A == 10 && B == 23)
        //        C = 84; // Prune - Rousse
        //    else if (A == 15 && B == 23)
        //        C = 85; // Prune - Turquoise
        //    else if (A == 22 && B == 23)
        //        C = 86; // Prune - Orchidée
        //    else if (A == 19 && B == 23)
        //        C = 87; // Prune - Purpre
        //    else if (A == B)
        //        C = A = B;

        //    if (C == 0)
        //    {

        //        random = new RandomStats<>();
        //        i = 0;
        //        for (String str : splitF)
        //        {
        //            i++;
        //            if (str.equals("?")) continue;

        //            int pct = 1;

        //            switch (i)
        //            {
        //                case 1: case 2: pct = 25; break;
        //                case 3: case 4: case 5: case 6: pct = 10;
        //            }

        //            random.add(pct, int.Parse(str));
        //        }
        //        i = 0;
        //        for (String str : splitM)
        //        {
        //            i++;
        //            if (str.equals("?")) continue;

        //            int pct = 1;

        //            switch (i)
        //            {
        //                case 1: case 2: pct = 25; break;
        //                case 3: case 4: case 5: case 6: pct = 10;
        //            }

        //            random.add(pct, int.Parse(str));
        //        }
        //        C = random.get();
        //        player.sendMessage("Merci de crier auprès de Locos que C = 0, A = " + A + ", et B = " + B + ". Valeur conste : " + C + ". Message bien évidement sérieux.");

        //        return C;
        //    }
        //    random = new RandomStats<>();
        //    random.add(33, A);
        //    random.add(33, B);
        //    random.add(33, C);
        //    return random.get();
        //}

        public static int getParchoByIdPets(int id)
        {
            switch (id)
            {
                case 10802:
                    return 10806;
                case 10107:
                    return 10135;
                case 10106:
                    return 10134;
                case 9795:
                    return 9810;
                case 9624:
                    return 9685;
                case 9623:
                    return 9684;
                case 9620:
                    return 9683;
                case 9619:
                    return 9682;
                case 9617:
                    return 9675;
                case 9594:
                    return 9598;
                case 8693:
                    return 8707;
                case 8677:
                    return 8684;
                case 8561:
                    return 8564;
                case 8211:
                    return 8544;
                case 8155:
                    return 8179;
                case 8154:
                    return 8178;
                case 8153:
                    return 8175;
                case 8151:
                    return 8176;
                case 8000:
                    return 8180;
                case 7911:
                    return 8526;
                case 7892:
                    return 7896;
                case 7891:
                    return 7895;
                case 7714:
                    return 8708;
                case 7713:
                    return 9681;
                case 7712:
                    return 9680;
                case 7711:
                    return 9679;
                case 7710:
                    return 9678;
                case 7709:
                    return 9677;
                case 7708:
                    return 9676;
                case 7707:
                    return 9674;
                case 7706:
                    return 8685;
                case 7705:
                    return 8889;
                case 7704:
                    return 8888;
                case 7703:
                    return 8421;
                case 7524:
                    return 8887;
                case 7522:
                    return 7535;
                case 7520:
                    return 7533;
                case 7519:
                    return 7534;
                case 7518:
                    return 7532;
                case 7415:
                    return 7419;
                case 7414:
                    return 7418;
                case 6978:
                    return 7417;
                case 6716:
                    return 7420;
                case 2077:
                    return 2098;
                case 2076:
                    return 2101;
                case 2075:
                    return 2100;
                case 2074:
                    return 2099;
                case 1748:
                    return 2102;
                case 1728:
                    return 1735;
            }
            return -1;
        }

        public static int getPetsByIdParcho(int id)
        {
            switch (id)
            {
                case 10806:
                    return 10802;
                case 10135:
                    return 10107;
                case 10134:
                    return 10106;
                case 9810:
                    return 9795;
                case 9685:
                    return 9624;
                case 9684:
                    return 9623;
                case 9683:
                    return 9620;
                case 9682:
                    return 9619;
                case 9675:
                    return 9617;
                case 9598:
                    return 9594;
                case 8707:
                    return 8693;
                case 8684:
                    return 8677;
                case 8564:
                    return 8561;
                case 8544:
                    return 8211;
                case 8179:
                    return 8155;
                case 8178:
                    return 8154;
                case 8175:
                    return 8153;
                case 8176:
                    return 8151;
                case 8180:
                    return 8000;
                case 8526:
                    return 7911;
                case 7896:
                    return 7892;
                case 7895:
                    return 7891;
                case 8708:
                    return 7714;
                case 9681:
                    return 7713;
                case 9680:
                    return 7712;
                case 9679:
                    return 7711;
                case 9678:
                    return 7710;
                case 9677:
                    return 7709;
                case 9676:
                    return 7708;
                case 9674:
                    return 7707;
                case 8685:
                    return 7706;
                case 8889:
                    return 7705;
                case 8888:
                    return 7704;
                case 8421:
                    return 7703;
                case 8887:
                    return 7524;
                case 7535:
                    return 7522;
                case 7533:
                    return 7520;
                case 7534:
                    return 7519;
                case 7532:
                    return 7518;
                case 7419:
                    return 7415;
                case 7418:
                    return 7414;
                case 7417:
                    return 6978;
                case 7420:
                    return 6716;
                case 2098:
                    return 2077;
                case 2101:
                    return 2076;
                case 2100:
                    return 2075;
                case 2099:
                    return 2074;
                case 2102:
                    return 1748;
                case 1735:
                    return 1728;
            }
            return -1;
        }

        public static int getDoplonDopeul(int IDmob)
        {
            switch (IDmob)
            {
                case 168:
                    return 10302;
                case 165:
                    return 10303;
                case 166:
                    return 10304;
                case 162:
                    return 10305;
                case 160:
                    return 10306;
                case 167:
                    return 10307;
                case 161:
                    return 10308;
                case 2691:
                    return 10309;
                case 455:
                    return 10310;
                case 169:
                    return 10311;
                case 163:
                    return 10312;
                case 164:
                    return 10313;
            }
            return -1;
        }

        public static int getIDdoplonByMapID(int IDmap)
        {
            switch (IDmap)
            {
                case 6926: //Sram
                    return 10312;
                case 1470: //Enutrof
                    return 10305;
                case 1461: //Ecaflip (map de dessous, puisque l'autre n'est pas dans l'emu)
                    return 10303;
                case 6949: //Sacrieur
                    return 10310;
                case 1556: //Cra (map en bas dans la maison, celle dans haut n'est pas dans l'emu)
                    return 10302;
                case 1549: //Iop
                    return 10307;
                case 1469: //Xel
                    return 10313;
                case 487: //Eniripsa (dehors, puisque l'int�rieur n'est pas pr�sent dans l'emu)
                    return 10304;
                case 490: //Osamodas (idem qu'eniripsa)
                    return 10308;
                case 177: //Feca (idem ...)
                    return 10306;
                case 1466: //Sadida
                    return 10311;
                case 8207: //Panda (idem que nini ...)
                    return 10309;
            }
            return -1;
        }

        public static int getStarAlea()
        {
            int rand = Util.rng.Next(1, 100);
            if (rand > 54 && rand < 90)
            {
                int i = Util.rng.Next(1, 15);
                switch (i)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        return 15;
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        return 30;
                    case 10:
                    case 11:
                    case 12:
                        return 45;
                    case 13:
                    case 14:
                        return 60;
                    case 15:
                        return 75;
                }
            }
            if (rand >= 90)
            {
                int i = Util.rng.Next(1, 15);
                switch (i)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        return 90;
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        return 105;
                    case 10:
                    case 11:
                    case 12:
                        return 120;
                    case 13:
                    case 14:
                        return 135;
                    case 15:
                        return 150;
                }
            }
            return 0;
        }

        public static int getArmeSoin(int idArme)
        {
            switch (idArme)
            {
                case 7172:
                    return 100;
                case 7156:
                    return 80;
                case 1355:
                    return 42;
                case 7182:
                    return 100;
                case 7040:
                    return 10;
                case 6539:
                    return 80;
                case 6519:
                    return 23;
                case 8118:
                    return 30;
                default:
                    return -1;
            }
        }

        public static int getSectionByDopeuls(int id)
        {
            switch (id)
            {
                case 160:
                    return 1;
                case 161:
                    return 2;
                case 162:
                    return 3;
                case 163:
                    return 4;
                case 164:
                    return 5;
                case 165:
                    return 6;
                case 166:
                    return 7;
                case 167:
                    return 8;
                case 168:
                    return 9;
                case 169:
                    return 10;
                case 455:
                    return 11;
                case 2691:
                    return 12;
            }
            return -1;
        }

        public static int getCertificatByDopeuls(int id)
        {
            switch (id)
            {
                case 160:
                    return 10293;
                case 161:
                    return 10295;
                case 162:
                    return 10292;
                case 163:
                    return 10299;
                case 164:
                    return 10300;
                case 165:
                    return 10290;
                case 166:
                    return 10291;
                case 167:
                    return 10294;
                case 168:
                    return 10289;
                case 169:
                    return 10298;
                case 455:
                    return 10297;
                case 2691:
                    return 10296;
            }
            return -1;
        }

        public static bool isCertificatDopeuls(int id)
        {
            switch (id)
            {
                case 10293:
                case 10295:
                case 10292:
                case 10299:
                case 10300:
                case 10290:
                case 10291:
                case 10294:
                case 10289:
                case 10298:
                case 10297:
                case 10296:
                    return true;
            }
            return false;
        }

        public static int getItemIdByMascotteId(int id)
        {
            switch (id)
            {
                case 10118:
                    return 1498;//Croc blanc
                case 10078:
                    return 70;//Eni Hoube
                case 10077:
                    return -1;//Terra Cogita
                case 10009:
                    return 90;//Xephir�s
                case 9993:
                    return 71;//Sabine
                case 9096:
                    return 30;//Tacticien
                case 9061:
                    return 40;//Exoram
                case 8563:
                    return 1076;//Titi gobelait
                case 7425:
                    return 1588;//Petite Larve Dor�e
                case 7354:
                    return 1264;//Zato�shwan
                case 7353:
                    return 1076;//Marzwell Le Gobelin
                case 7352:
                    return 1153;//Musha l'Oni
                case 7351:
                    return 1248;//Rok Gnorok
                case 7350:
                    return 1228;//Aermyne Braco Scalptaras
                case 7062:
                    return 9001;//Poochan
                case 6876:
                    return 1245;//Ogivol Scarlarcin
                case 6875:
                    return 1249;//Fouduglen
                case 6874:
                    return 70;//Brumen Tinctorias
                case 6873:
                    return 1243;//Qil Bil
                case 6872:
                    return 50;//Nervoes Brakdoun
                case 6871:
                    return 1247;//Frakacia Leukocytine
                case 6870:
                    return 1246;//Padgref Demo�l
                case 6869:
                    return 9043;//Pleur Nycheuz
                case 6832:
                    return -1;//Livreur de Bi�re
                case 6768:
                    return 9001;//Soki
                case 2272:
                    return 1577;//Larve Dor�e
                case 2169:
                    return 1205;//Raaga
                case 2152:
                    return 1001;//Colonel Lyeno
                case 2134:
                    return 1205;//Trof Hapyus
                case 2132:
                    return 9004;//Hou� Dapyus
                case 2130:
                    return 1001;//Colonel Lyeno
                case 2082:
                    return 1208;//Marcassin
            }
            return -1;
        }

        public static bool isIncarnationWeapon(int id)
        {
            switch (id)
            {
                case 9544:
                case 9545:
                case 9546:
                case 9547:
                case 9548:
                case 10133:
                case 10127:
                case 10126:
                case 10125:
                    return true;
            }
            return false;
        }

        public static bool isTourmenteurWeapon(int id)
        {
            switch (id)
            {
                case 9544:
                case 9545:
                case 9546:
                case 9547:
                case 9548:
                    return true;
            }
            return false;
        }

        public static bool isBanditsWeapon(int id)
        {
            switch (id)
            {
                case 10133:
                case 10127:
                case 10126:
                case 10125:
                    return true;
            }
            return false;
        }

        public static int getSpecialSpellByClasse(int classe)
        {
            switch (classe)
            {
                case Constant.CLASS_FECA:
                    return 422;
                case Constant.CLASS_OSAMODAS:
                    return 420;
                case Constant.CLASS_ENUTROF:
                    return 425;
                case Constant.CLASS_SRAM:
                    return 416;
                case Constant.CLASS_XELOR:
                    return 424;
                case Constant.CLASS_ECAFLIP:
                    return 412;
                case Constant.CLASS_ENIRIPSA:
                    return 427;
                case Constant.CLASS_IOP:
                    return 410;
                case Constant.CLASS_CRA:
                    return 418;
                case Constant.CLASS_SADIDA:
                    return 426;
                case Constant.CLASS_SACRIEUR:
                    return 421;
                case Constant.CLASS_PANDAWA:
                    return 423;
            }
            return 0;
        }

        public static bool isFlacGelee(int id)
        {
            switch (id)
            {
                case 2430:
                case 2431:
                case 2432:
                case 2433:
                    return true;
            }
            return false;
        }

        public static bool isDoplon(int id)
        {
            switch (id)
            {
                case 10302:
                case 10303:
                case 10304:
                case 10305:
                case 10306:
                case 10307:
                case 10308:
                case 10309:
                case 10310:
                case 10311:
                case 10312:
                case 10313:
                    return true;
            }
            return false;
        }

        public static bool isInMorphDonjon(int id)
        {
            switch (id)
            {
                case 8716:
                case 8718:
                case 8719:
                case 9121:
                case 9122:
                case 9123:
                case 8979:
                case 8980:
                case 8981:
                case 8982:
                case 8983:
                case 8984:
                case 9716:
                    return true;
            }
            return false;
        }

        public static int[] getOppositeStats(int statsId)
        {
            if (statsId == 217)
                return new int[] { 210, 211, 213, 214 };
            else if (statsId == 216)
                return new int[] { 210, 212, 213, 214 };
            else if (statsId == 218)
                return new int[] { 210, 211, 212, 214 };
            else if (statsId == 219)
                return new int[] { 210, 211, 212, 214 };
            else if (statsId == 215)
                return new int[] { 211, 212, 213, 214 };
            return null;
        }
    }
}
