using LeafEmu.World.Game.Map.Cell;

namespace LeafEmu.World.Game.Pathfinding
{
    public class CellPathfinding
    {

        public CellPathfinding Parent { get; set; }


        public Cell cell { get; set; }
        public int f = 0;
        public int g = 0;
        public int h = 0;

        public CellPathfinding(Cell _cell)
        {
            Parent = this;
            cell = _cell;
        }

    }
}
