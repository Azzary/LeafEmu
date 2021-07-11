namespace LeafEmu.World.Game.Map
{
    public class EndFightAction
    {
        public int FightType;
        public int Action;
        public int map;
        public int cell;
        public string Cond;

        public EndFightAction(int fighttype, int action, int map, int cell, string cond)
        {
            this.FightType = fighttype;
            this.Action = action;
            this.map = map;
            this.cell = cell;
            this.Cond = cond;
        }
    }
}
