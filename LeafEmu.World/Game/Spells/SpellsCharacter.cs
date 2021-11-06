namespace LeafEmu.World.Game.Spells
{
    public class SpellsEntity
    {
        public int id;
        public int level;
        public int pos;

        public SpellsEntity(int _id, int _level, int pos)
        {
            id = _id;
            level = _level;
            this.pos = pos;
        }

    }
}
