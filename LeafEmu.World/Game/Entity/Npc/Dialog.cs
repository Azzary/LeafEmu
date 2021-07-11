namespace LeafEmu.World.Game.Entity.Npc
{
    public class Dialog
    {
        public int ID { get; }
        public string responses { get; }
        public string parametre { get; }
        public string cond { get; }
        public string ifFalse { get; }
        public string description { get; }

        public Dialog(int id, string rep, string param, string _cond, string _ifFalse, string desc)
        {
            ID = id;
            responses = rep;
            parametre = param;
            cond = _cond;
            ifFalse = _ifFalse;
            description = desc;
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
