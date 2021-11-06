using LeafEmu.World.Game;
using LeafEmu.World.Game.Quest;
using LeafEmu.World.World;


namespace LeafEmu.World
{
    public class ConditionParser
    {
        public static bool validConditions(Game.Entity.Character perso, String req)
        {
            if (req == null || req.Equals(string.Empty))
                return true;
            if (req.Contains("BI"))
                return false;
            if (perso == null)
                return false;
            req = req.Replace("&", "&&").Replace("=", "==").Replace("|", "||").Replace("!", "!=").Replace("~", "==");
            if (req.Contains("Sc"))
                return true;
            if (req.Contains("Pg")) // C'est les dons que l'on gagne lors des qu�tes d'alignement, connaissance des potions etc ... ce n'est pas encore cod� !
                return false;
            if (req.Contains("RA"))
                return haveRA(req, perso);
            if (req.Contains("RO"))
            {
                //return haveRO(req, prmClient);
            }
            if (req.Contains("Mph"))
                return haveMorph(req, perso);
            if (req.Contains("PO"))
                req = havePO(req, perso);
            if (req.Contains("PN"))
                req = canPN(req, perso);
            if (req.Contains("PJ"))
                req = canPJ(req, perso);
            if (req.Contains("JOB"))
                req = haveJOB(req, perso);
            if (req.Contains("NPC"))
                return haveNPC(req, perso);
            if (req.Contains("QEt"))
                return haveQEt(req, perso);
            if (req.Contains("QE"))
                return haveQE(req, perso);
            if (req.Contains("QT"))
                return haveQT(req, perso);
            if (req.Contains("Ce"))
                return haveCe(req, perso);
            if (req.Contains("TiT"))
                return haveTiT(req, perso);
            if (req.Contains("Ti"))
                return haveTi(req, perso);
            if (req.Contains("Qa"))
                return haveQa(req, perso);
            if (req.Contains("Pj"))
                return havePj(req, perso);
            if (req.Contains("AM"))
                return haveMetier(req, perso);
            return false;
        }
        private static bool haveMorph(String c, Game.Entity.Character p)
        {
            if (c.Equals(string.Empty))
                return false;
            int morph = -1;
            try
            {
                morph = int.Parse((c.Contains("==") ? c.Split("==")[1] : c.Split("!=")[1]));
            }
            catch (Exception e)
            {
                Logger.Logger.Warning(e.ToString());
            }
            if (p.gfxID == morph)
                return c.Contains("==");
            else
                return !c.Contains("==");
        }

        private static bool haveMetier(String c, Game.Entity.Character p)
        {
            //if (p.getMetiers() == null || p.getMetiers().isEmpty())
            //    return false;
            //for (Entry<Integer, JobStat> entry : p.getMetiers().entrySet())
            //{
            //    if (entry.getValue() != null)
            //        return true;
            //}
            return false;
        }

        private static bool havePj(String c, Game.Entity.Character p)
        {
            if (c.Equals(string.Empty))
                return false;
            foreach (String s in c.Split("\\|\\|"))
            {
                String[] k = s.Split("==");
                int id;
                try
                {
                    id = int.Parse(k[1]);
                }
                catch (Exception e)
                {
                    Logger.Logger.Warning(e.ToString());
                    continue;
                }
                if (p.getMetierByID(id) != null)
                    return true;
            }
            return false;
        }

        //Avoir la qu�te en cours
        private static bool haveQa(String req, Game.Entity.Character perso)
        {
            int id = int.Parse((req.Contains("==") ? req.Split("==")[1] : req.Split("!=")[1]));
            Quest q = Quest.questDataList[id];
            if (q == null)
                return (!req.Contains("=="));

            QuestPlayer qp = perso.getQuestPersoByQuest(q);
            if (qp == null)
                return (!req.Contains("=="));

            return !qp.finish|| (!req.Contains("=="));

        }

        // �tre � l'�tape id. Elle ne doit pas �tre valid� et celle d'avant doivent l'�tre.
        private static bool haveQEt(String req, Game.Entity.Character perso)
        {
            int id = int.Parse((req.Contains("==") ? req.Split("==")[1] : req.Split("!=")[1]));
            QuestEtape qe = QuestEtape.getQuestEtapeById(id);
            if (qe != null)
            {
                Quest q = qe.quest;
                if (q != null)
                {
                    QuestPlayer qp = perso.getQuestPersoByQuest(q);
                    if (qp != null)
                    {
                        QuestEtape current = q.getQuestEtapeCurrent(qp);
                        if (current == null)
                            return false;
                        if (current.id == qe.id)
                            return (req.Contains("=="));
                    }
                }
            }
            return false;
        }

        private static bool haveTiT(String req, Game.Entity.Character perso)
        {
            if (req.Contains("=="))
            {
                String Split = req.Split("==")[1];
                if (Split.Contains("&&"))
                {
                    int item = int.Parse(Split.Split("&&")[0]);
                    int time = int.Parse(Split.Split("&&")[1]);
                    int item2 = int.Parse(Split.Split("&&")[2]);
                    if (perso.Inventaire.HasItemTemplate(item2, 1)
                            && perso.Inventaire.HasItemTemplate(item, 1))
                    {
                        long timeStamp = long.Parse(perso.Inventaire.GetItem(item).Effects.Find(x => x.ID == Constant.STATS_DATE).ToString());
                        if (Util.GetUnixTime - timeStamp <= time)
                            return true;
                    }
                }
            }
            return false;
        }

        private static bool haveTi(String req, Game.Entity.Character perso)
        {
            if (req.Contains("=="))
            {
                String Split = req.Split("==")[1];
                if (Split.Contains(","))
                {
                    String[] Split2 = Split.Split(",");
                    int item = int.Parse(Split2[0]);
                    int time = int.Parse(Split2[1]) * 60 * 1000;
                    if (perso.Inventaire.HasItemTemplate(item, 1))
                    {
                        long timeStamp = long.Parse(perso.Inventaire.GetItem(item).Effects.Find(x => x.ID == Constant.STATS_DATE).ToString());
                        if (Util.GetUnixTime - timeStamp <= time)
                            return true;
                    }
                }
            }
            return false;
        }

        private static bool haveCe(String req, Game.Entity.Character perso)
        {
            var dopeuls = QuestAction.getDopeul();
            var map = perso.Map;
            if (dopeuls.ContainsKey(map.Id))
            {
                var couple = dopeuls[map.Id];
                if (couple == null)
                    return false;

                int IDmob = couple[0];
                int certificat = Constant.getCertificatByDopeuls(IDmob);

                if (certificat == -1)
                    return false;

                if (perso.Inventaire.HasItemTemplate(certificat, 1))
                {
                    String txt = perso.Inventaire.GetItem(certificat, 1).Effects.Find(x => x.ID == Constant.STATS_DATE).ToString();
                    if (txt.Contains("#"))
                        txt = txt.Split("#")[3];
                    long timeStamp = long.Parse(txt);
                    return Util.GetUnixTime - timeStamp > 86400000;
                }
                else
                    return true;
            }
            return false;
        }

        // Avoir la qu�te en cours.
        private static bool haveQE(String req, Game.Entity.Character perso)
        {
            if (perso == null)
                return false;
            int id = int.Parse((req.Contains("==") ? req.Split("==")[1] : req.Split("!=")[1]));
            QuestPlayer qp = perso.questList.ContainsKey(id) ? perso.questList[id] : null;
            if (req.Contains("=="))
            {
                return qp != null && !qp.finish;
            }
            else
            {
                return qp == null || qp.finish;
            }
        }

        private static bool haveQT(String req, Game.Entity.Character perso)
        {
            int id = int.Parse((req.Contains("==") ? req.Split("==")[1] : req.Split("!=")[1]));
            QuestPlayer quest = perso.getQuestPersoByQuestId(id);
            if (req.Contains("=="))
                return (quest != null && quest.finish);
            else
                return (quest == null || !quest.finish);
        }

        private static bool haveNPC(String req, Game.Entity.Character perso)
        {
            switch (perso.mapID)
            {
                case 9052:
                    if (perso.cellID == 268
                            && perso.Orientation == 7)//TODO
                        return true;
                    break;
                case 8905:
                    List<int> cell = new List<int>();
                    foreach (String i in "168,197,212,227,242,183,213,214,229,244,245,259".Split("\\,"))
                        cell.Add(int.Parse(i));
                    if (cell.Contains(perso.cellID))
                        return true;
                    break;
            }
            return false;
        }

        public static bool haveRO(String condition, Network.listenClient prmClient)
        {
            var perso = prmClient.account.character;
            try
            {
                foreach (String cond in condition.Split("&&"))
                {
                    String[] Split = cond.Split("==")[1].Split(",");
                    int id = int.Parse(Split[0]), qua = int.Parse(Split[1]);

                    if (perso.Inventaire.HasItemTemplate(id, qua))
                    {
                        perso.Inventaire.RemoveItem(prmClient, id, qua);
                        return true;
                    }
                    else
                    {
                        prmClient.GAME_SEND_Im_PACKET("14");
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Logger.Warning(e.ToString());
            }
            return false;
        }

        public static bool haveRA(String condition, Game.Entity.Character perso)
        {
            try
            {
                foreach (String cond in condition.Split("&&"))
                {
                    String[] Split = cond.Split("==")[1].Split(",");
                    int id = int.Parse(Split[0]), qua = int.Parse(Split[1]);

                    if (!perso.Inventaire.HasItemTemplate(id, qua))
                        return false;
                }
            }
            catch (Exception e)
            {
                Logger.Logger.Warning(e.ToString());
            }
            return true;
        }

        public static String havePO(String cond, Game.Entity.Character perso)//On remplace les PO par leurs valeurs si possession de l'item
        {
            bool Jump = false;
            bool ContainsPO = false;
            bool CutFinalLenght = true;
            String copyCond = string.Empty;
            int finalLength = 0;

            if (cond.Contains("&&"))
            {
                foreach (String cur in cond.Split("&&"))
                {
                    if (cond.Contains("=="))
                    {
                        foreach (String cur2 in cur.Split("=="))
                        {
                            if (cur2.Contains("PO"))
                            {
                                ContainsPO = true;
                                continue;
                            }
                            if (Jump)
                            {
                                copyCond += cur2;
                                Jump = false;
                                continue;
                            }
                            if (!cur2.Contains("PO") && !ContainsPO)
                            {
                                copyCond += cur2 + "==";
                                Jump = true;
                                continue;
                            }
                            if (cur2.Contains("!="))
                                continue;
                            ContainsPO = false;
                            if (perso.Inventaire.HasItemTemplate(int.Parse(cur2), 1))
                            {
                                copyCond += int.Parse(cur2) + "=="
                                        + int.Parse(cur2);
                            }
                            else
                            {
                                copyCond += int.Parse(cur2) + "==" + 0;
                            }
                        }
                    }
                    if (cond.Contains("!="))
                    {
                        foreach (String cur2 in cur.Split("!="))
                        {
                            if (cur2.Contains("PO"))
                            {
                                ContainsPO = true;
                                continue;
                            }
                            if (Jump)
                            {
                                copyCond += cur2;
                                Jump = false;
                                continue;
                            }
                            if (!cur2.Contains("PO") && !ContainsPO)
                            {
                                copyCond += cur2 + "!=";
                                Jump = true;
                                continue;
                            }
                            if (cur2.Contains("=="))
                                continue;
                            ContainsPO = false;
                            if (perso.Inventaire.HasItemTemplate(int.Parse(cur2), 1))
                            {
                                copyCond += int.Parse(cur2) + "!="
                                        + int.Parse(cur2);
                            }
                            else
                            {
                                copyCond += int.Parse(cur2) + "!=" + 0;
                            }
                        }
                    }
                    copyCond += "&&";
                }
            }
            else if (cond.Contains("||"))
            {
                foreach (String cur in cond.Split("\\|\\|"))
                {
                    if (cond.Contains("=="))
                    {
                        foreach (String cur2 in cur.Split("=="))
                        {
                            if (cur2.Contains("PO"))
                            {
                                ContainsPO = true;
                                continue;
                            }
                            if (Jump)
                            {
                                copyCond += cur2;
                                Jump = false;
                                continue;
                            }
                            if (!cur2.Contains("PO") && !ContainsPO)
                            {
                                copyCond += cur2 + "==";
                                Jump = true;
                                continue;
                            }
                            if (cur2.Contains("!="))
                                continue;
                            ContainsPO = false;
                            if (perso.Inventaire.HasItemTemplate(int.Parse(cur2), 1))
                            {
                                copyCond += int.Parse(cur2) + "=="
                                        + int.Parse(cur2);
                            }
                            else
                            {
                                
                                copyCond += int.Parse(cur2) + "==" + 0;
                            }
                        }
                    }
                    if (cond.Contains("!="))
                    {
                        foreach (String cur2 in cur.Split("!="))
                        {
                            if (cur2.Contains("PO"))
                            {
                                ContainsPO = true;
                                continue;
                            }
                            if (Jump)
                            {
                                copyCond += cur2;
                                Jump = false;
                                continue;
                            }
                            if (!cur2.Contains("PO") && !ContainsPO)
                            {
                                copyCond += cur2 + "!=";
                                Jump = true;
                                continue;
                            }
                            if (cur2.Contains("=="))
                                continue;
                            ContainsPO = false;
                            if (perso.Inventaire.HasItemTemplate(int.Parse(cur2), 1))
                            {
                                copyCond += int.Parse(cur2) + "!="
                                        + int.Parse(cur2);
                            }
                            else
                            {
                                copyCond += int.Parse(cur2) + "!=" + 0;
                            }
                        }
                    }
                    copyCond += "||";
                }
            }
            else
            {
                CutFinalLenght = false;
                if (cond.Contains("=="))
                {
                    foreach (String cur in cond.Split("=="))
                    {
                        if (cur.Contains("PO"))
                            continue;
                        if (cur.Contains("!="))
                            continue;
                        if (perso.Inventaire.HasItemTemplate(int.Parse(cur), 1))
                            copyCond += int.Parse(cur) + "=="
                                    + int.Parse(cur);
                        else
                            copyCond += int.Parse(cur) + "==" + 0;
                    }
                }
                if (cond.Contains("!="))
                {
                    foreach (String cur in cond.Split("!="))
                    {
                        if (cur.Contains("PO"))
                            continue;
                        if (cur.Contains("=="))
                            continue;
                        if (perso.Inventaire.HasItemTemplate(int.Parse(cur), 1))
                            copyCond += int.Parse(cur) + "!="
                                    + int.Parse(cur);
                        else
                            copyCond += int.Parse(cur) + "!=" + 0;
                    }
                }
            }
            if (CutFinalLenght)
            {
                finalLength = (copyCond.Count() - 2);//On retire les deux derniers carract�res (|| ou &&)
                copyCond = copyCond.Substring(0, finalLength);
            }
            return copyCond;
        }

        public static String canPN(String cond, Game.Entity.Character perso)//On remplace le PN par 1 et si le nom correspond == 1 sinon == 0
        {
            String copyCond = string.Empty;
            foreach (String cur in cond.Split("=="))
            {
                if (cur.Contains("PN"))
                {
                    copyCond += "1==";
                    continue;
                }
                if (perso.speudo.ToLower().CompareTo(cur) == 0)
                    copyCond += "1";
                else
                    copyCond += "0";
            }
            return copyCond;
        }

        public static String canPJ(String cond, Game.Entity.Character perso)//On remplace le PJ par 1 et si le metier correspond == 1 sinon == 0
        {
            String copyCond = string.Empty;
            if (cond.Contains("=="))
            {
                String[] cur = cond.Split("==");
                if (perso.getMetierByID(int.Parse(cur[1])) != null)
                    copyCond = "1==1";
                else
                    copyCond = "1==0";
            }
            else if (cond.Contains(">"))
            {
                if (cond.Contains("||"))
                {
                    foreach (String cur in cond.Split("\\|\\|"))
                    {
                        if (!cur.Contains(">"))
                            continue;
                        String[] _cur = cur.Split(">");
                        if (!_cur[1].Contains(","))
                            continue;
                        String[] m = _cur[1].Split(",");
                        var jobs = perso.getMetierByID(int.Parse(m[0]));
                        if (!copyCond.Equals(string.Empty))
                            copyCond += "||";
                        //if (jobs != null)
                        //    copyCond += jobs.get_lvl() + ">" + m[1];
                        //else
                        //    copyCond += "1==0";
                    }
                }
                else
                {
                    //String[] cur = cond.Split(">");
                    //String[] m = cur[1].Split(",");
                    //JobStat js = perso.getMetierByID(int.Parse(m[0]));
                    //if (js != null)
                    //    copyCond = js.get_lvl() + ">" + m[1];
                    //else
                    //    copyCond = "1==0";
                }
            }
            return copyCond;
        }

        public static String haveJOB(String cond, Game.Entity.Character perso)
        {
            String copyCond = string.Empty;
            if (perso.getMetierByID(int.Parse(cond.Split("==")[1])) != null)
                copyCond = "1==1";
            else
                copyCond = "0==1";
            return copyCond;
        }

        public static bool stackIfSimilar(Game.Item.Stuff stuff, Game.Item.Stuff newStuff, bool stackIfSimilar)
        {
            switch (stuff.Template.ID)
            {
                case 10275:
                case 8378:
                    if (stuff.Template.ID == newStuff.Template.ID)
                        return false;
                    break;
            }
            return stuff.Template.ID == newStuff.Template.ID && stackIfSimilar && Game.Item.Stuff.ItemHaveSameEffect(newStuff, stuff) && !Constant.isIncarnationWeapon(newStuff.Template.ID)
                    && newStuff.Template.Type != Constant.ITEM_TYPE_CERTIFICAT_CHANIL
                    && newStuff.Template.Type != Constant.ITEM_TYPE_FAMILIER
                    && newStuff.Template.Type != Constant.ITEM_TYPE_PIERRE_AME_PLEINE
                    && newStuff.Template.Type != Constant.ITEM_TYPE_OBJET_ELEVAGE
                    && newStuff.Template.Type != Constant.ITEM_TYPE_CERTIF_MONTURE
                    && newStuff.Template.Type != Constant.ITEM_TYPE_OBJET_VIVANT
                    && (newStuff.Template.Type != Constant.ITEM_TYPE_QUETES || Constant.isFlacGelee(stuff.Template.ID) || Constant.isDoplon(stuff.Template.ID))
                    && stuff.Position == Constant.ITEM_POS_NO_EQUIPED;
        }
    }
}
