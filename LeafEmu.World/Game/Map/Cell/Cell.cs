namespace LeafEmu.World.Game.Map.Cell
{
    public class Cell
    {
        public int ID;
        public int Gfx1;
        public int Gfx2;
        public int Gfx3;
        private bool isWalkable;
        public bool Path;
        public bool IsSightBlocker;
        public bool Paddock;
        public bool TriggerCell;
        public bool Door;
        public bool IO;
        public bool isActive;
        public int FightCell;
        public bool FlipGfx1;
        public bool FlipGfx2;
        public bool FlipGfx3;
        public MovementEnum cellType;
        public InteractiveObject interactiveObject { get; private set; }
        public int RotaGfx1;
        public int RotaGfx2;
        public int InclineSol;
        public int NivSol;
        public bool Trigger;
        public string TriggerName;
        public int obj;
        public int movement;
        string CellData;
        public int x;
        public int y;

        public Cell(int _ID, string _cellData, int mapWidth)
        {
            int loc5 = _ID / ((mapWidth * 2) - 1);
            int loc6 = _ID - (loc5 * ((mapWidth * 2) - 1));
            int loc7 = loc6 % mapWidth;
            y = loc5 - loc7;
            x = (_ID - ((mapWidth - 1) * y)) / mapWidth;

            ID = _ID;
            CellData = _cellData;
            UncompressCell();
        }


        public Cell(int _ID, bool walkable, bool los, int _obj, int mapWidth)
        {
            int loc5 = _ID / ((mapWidth * 2) - 1);
            int loc6 = _ID - (loc5 * ((mapWidth * 2) - 1));
            int loc7 = loc6 % mapWidth;
            y = loc5 - loc7;
            x = (_ID - ((mapWidth - 1) * y)) / mapWidth;

            isWalkable = walkable;
            IsSightBlocker = !los;
            ID = _ID;
            obj = _obj;
        }
        public Cell(int _ID)
        {
            ID = _ID;

        }
        public Cell(int _ID, int Gfx)
        {
            ID = _ID;
            Gfx1 = Gfx;
        }



        private void UncompressCell()
        {
            IsSightBlocker = (CellData[0] & 1) != 0;

            Gfx1 = ((CellData[0] & 24) << 6) + ((CellData[2] & 7) << 6) + CellData[3];
            RotaGfx1 = (CellData[1] & 48) >> 4;
            FlipGfx1 = ((CellData[4] & 2) >> 1) != 0;

            Gfx2 = ((CellData[0] & 4) << 11) + ((CellData[4] & 1) << 12) + (CellData[5] << 6) + CellData[6];
            RotaGfx2 = (CellData[7] & 48) >> 4;
            FlipGfx2 = ((CellData[7] & 8) >> 3) != 0;

            Gfx3 = ((CellData[0] & 2) << 12) + ((CellData[7] & 1) << 12) + (CellData[8] << 6) + CellData[9];
            FlipGfx3 = ((CellData[7] & 4) >> 2) != 0;


            Type((CellData[2] & 56) >> 3 & -1025);
            //isActive = true;//(CellData[0] & 32) >> 5 != 0;

            //cellType = (MovementEnum)((CellData[2] & 56) >> 3);
            //UnWalkable = IsWalkable();
            NivSol = CellData[1] & 15;

            IO = ((CellData[7] & 2) >> 1) != 0;
            obj = (IO ? Gfx3 : -1);

            InclineSol = (CellData[4] & 60) >> 2;
            movement = (CellData[2] & 56) >> 3;
        }

        public bool IsWalkable()
        {
            return isWalkable || TriggerCell;
        }

        public object Type(int _id = -1)
        {
            switch (_id)
            {
                case -1:
                    if (this.isWalkable)
                        return (object)MovementEnum.NOT_WALKABLE;
                    if (this.Paddock)
                        return (object)MovementEnum.PADDOCK;
                    if (this.Path)
                        return (object)MovementEnum.TELEPORT_CELL;
                    if (this.Door)
                        return (object)MovementEnum.INTERACTIVE_OBJECT;
                    return this.Trigger ? (object)MovementEnum.TELEPORT_CELL : (object)MovementEnum.WALKABLE;
                case 0:
                    this.isWalkable = true;
                    break;
                default:
                    this.isWalkable = false;
                    if (_id == 5)
                        this.Paddock = true;
                    if (_id == 7)
                        this.Path = true;
                    if (_id == 1)
                        this.Door = true;
                    if (_id == 2)
                    {
                        this.TriggerCell = true;
                        break;
                    }
                    break;
            }
            return (object)true;
        }
    }
}
