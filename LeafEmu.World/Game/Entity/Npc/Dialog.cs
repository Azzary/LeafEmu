namespace LeafEmu.World.Game.Entity.Npc
{
    public class Dialog
    {
        public int ID { get; }
        public string responses { get; }
        public string parametre { get; }
        public string cond { get; }
        public int ifFalse { get; }
        public string description { get; }

        public Dialog(int id, string rep, string param, string _cond, string _ifFalse, string desc)
        {
            ID = id;
            responses = rep;
            parametre = param;
            cond = _cond;
            ifFalse = _ifFalse == string.Empty ? 0 : int.Parse(_ifFalse);
            description = desc;
        }

        public static bool NpcTalkCondition(Character character, string cond)
        {
            if (cond.Contains("QE") && !cond.Contains(":"))
            {
                var id = int.Parse(cond.Substring(3));
                var quest = character.questList.Values.ToList().Find(x => x.quest.id == id);
                bool test = false;
                // ! = quete pas apris
                // + = quete fini
                // = = quete apris mais pas fini
                switch (cond.Substring(2, 1))
                {
                    case "!":
                        return quest == null;
                    case "=":
                        return quest != null && !quest.finish;
                    case "+":
                        return !(quest != null && quest.finish);
                }
            }
            else if (cond.Contains("QT"))
            {
                int id = int.Parse(cond.Contains("=") ? cond.Split("=")[1] : cond.Split("!")[1]);
                var quest = character.getQuestPersoByQuestId(id);
                if (cond.Contains("="))
                {
                    return quest != null && quest.finish;
                }
                else
                {
                    return quest == null || !quest.finish;
                }
            }
            return true;
        }
    }

    public class ReponceDialog
    {
        public int ID { get; }
        public int type { get; }
        public string args { get; }
        public string nom { get; }


        public ReponceDialog(int id, int _type, string _args, string _nom)
        {
            ID = id;
            type = _type;
            args = _args;
            nom = _nom;
        }
    }
}
