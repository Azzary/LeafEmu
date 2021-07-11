using System.Collections.Generic;

namespace LeafEmu.World.Game.Map
{
    public class InteractiveDoors
    {
        public int DoorMapID { get; }
        public int RequiedCellsMapID { get; }
        public List<int> DoorCellID { get; }
        public List<int> RequiedCells { get; }

        public InteractiveDoors(int Dmapid, int CmapID)
        {
            DoorMapID = Dmapid;
            RequiedCellsMapID = CmapID;
            DoorCellID = new List<int>();
            RequiedCells = new List<int>();
        }

    }
}
