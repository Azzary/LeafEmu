using System;
using System.Collections.Generic;

//@Author NightWolf
//This is a file from Project $safeprojectname$

namespace LeafEmu.World.Game.Pathfinding
{
    public class PathfindingUtil
    {
        private string _strPath;
        private int _startCell;
        private int _startDir;

        public int Destination;
        public int NewDirection;

        private Map.Map _map;

        public static int[] InLineDirPossible = new int[4] { 1, 3, 5, 7 };

        public PathfindingUtil(string path, Map.Map map, int startCell, int startDir)
        {
            _strPath = path;
            _map = map;
            _startCell = startCell;
            _startDir = startDir;
            this.init();
        }

        private void init()
        {
            for (int i = 0; i <= _strPath.Length - 1; i += 3)
            {
                try
                {
                    string littlePath = _strPath.Substring(i, 3);
                    this.Destination = GetCellNum(littlePath.Substring(1, 2));
                }
                catch (Exception e)
                {

                }
            }
        }

        public string GetStartPath
        {
            get
            {
                return GetDirChar(_startDir) + GetCellChars(_startCell);
            }
        }

        public int GetCaseIDFromDirection(int caseID, char direction, bool fight)
        {
            switch (direction)
            {
                case 'a':
                    return fight ? -1 : caseID + 1;
                case 'b':
                    return caseID + _map.Width;
                case 'c':
                    return fight ? -1 : caseID + (_map.Width * 2 - 1);
                case 'd':
                    return caseID + (_map.Width - 1);
                case 'e':
                    return fight ? -1 : caseID - 1;
                case 'f':
                    return caseID - _map.Width;
                case 'g':
                    return fight ? -1 : caseID - (_map.Width * 2 - 1);
                case 'h':
                    return caseID - _map.Width + 1;
            }
            return -1;
        }

        public static int GetCellNum(string CellChars)
        {

            string hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

            int NumChar1 = hash.IndexOf(CellChars[0]) * hash.Length;
            int NumChar2 = hash.IndexOf(CellChars[1]);

            return NumChar1 + NumChar2;

        }

        public static string GetCellChars(int CellNum)
        {

            string hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

            int CharCode2 = (CellNum % hash.Length);
            int CharCode1 = (CellNum - CharCode2) / hash.Length;

            return hash[CharCode1].ToString() + hash[CharCode2].ToString();

        }

        public static string GetDirChar(int DirNum)
        {

            string hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            if (DirNum >= hash.Length)
                return "";
            return hash[DirNum].ToString();

        }



        public bool InLine(int cell1, int cell2)
        {
            bool isX = GetCellXCoord(cell1, _map.Width) == GetCellXCoord(cell2, _map.Width);
            bool isY = GetCellYCoord(cell1, _map.Width) == GetCellYCoord(cell2, _map.Width);
            return isX || isY;
        }

        public int NextCell(int cell, int dir)
        {
            switch (dir)
            {
                case 0:
                    return cell + 1;

                case 1:
                    return cell + _map.Width;

                case 2:
                    return cell + (_map.Width * 2) - 1;

                case 3:
                    return cell + _map.Width - 1;

                case 4:
                    return cell - 1;

                case 5:
                    return cell - _map.Width;

                case 6:
                    return cell - (_map.Width * 2) + 1;

                case 7:
                    return cell - _map.Width + 1;

            }
            return -1;
        }

        public static int NextCell(Map.Map map, int cell, int dir)
        {
            switch (dir)
            {
                case 0:
                    return cell + 1;

                case 1:
                    return cell + map.Width;

                case 2:
                    return cell + (map.Width * 2) - 1;

                case 3:
                    return cell + map.Width - 1;

                case 4:
                    return cell - 1;

                case 5:
                    return cell - map.Width;

                case 6:
                    return cell - (map.Width * 2) + 1;

                case 7:
                    return cell - map.Width + 1;

            }

            return -1;
        }

        public int FreeCellNeightboor(int cell)
        {
            var cells = new List<int>();
            var free = -1;
            cells.Add(NextCell(cell, 1));
            cells.Add(NextCell(cell, 3));
            cells.Add(NextCell(cell, 5));
            cells.Add(NextCell(cell, 7));
            foreach (var c in cells)
            {
                if (_map.Cells[c].isActive)
                {
                    free = c;
                    break;
                }
            }
            cells.Clear();
            return free;
        }

        public static int sNextCell(int cell, int dir, Map.Map _map)
        {
            switch (dir)
            {
                case 0:
                    return cell + 1;

                case 1:
                    return cell + _map.Width;

                case 2:
                    return cell + (_map.Width * 2) - 1;

                case 3:
                    return cell + _map.Width - 1;

                case 4:
                    return cell - 1;

                case 5:
                    return cell - _map.Width;

                case 6:
                    return cell - (_map.Width * 2) + 1;

                case 7:
                    return cell - _map.Width + 1;

            }
            return -1;
        }

        public static int RandomJoinCell(int cell, Map.Map map)
        {
            return sNextCell(cell, InLineDirPossible[Util.rng.Next(0, 3)], map);
        }

        public string RemakeLine(int lastCell, string cell, int finalCell)
        {
            int direction = Util.GetDirNum(cell[0].ToString());
            int toCell = GetCellNum(cell.Substring(1));
            int lenght = 0;
            if (InLine(lastCell, toCell))
            {
                lenght = GetEstimateDistanceBetween(lastCell, toCell);
            }
            else
            {
                lenght = int.Parse(Math.Truncate((GetEstimateDistanceBetween(lastCell, toCell) / 1.4)).ToString());
            }
            int backCell = lastCell;
            int actuelCell = lastCell;
            for (int i = 1; i <= lenght; i++)
            {
                actuelCell = NextCell(actuelCell, direction);
                backCell = actuelCell;
            }
            return cell + ",1";
        }

        public string RemakePath()
        {
            string newPath = "";
            int newCell = GetCellNum(_strPath.Substring(_strPath.Length - 2, 2));
            int lastCell = _startCell;
            for (int i = 0; i <= _strPath.Length - 1; i += 3)
            {
                string actualCell = _strPath.Substring(i, 3);
                string[] lineData = RemakeLine(lastCell, actualCell, newCell).Split(',');
                newPath += lineData[0];
                if (lineData[1] == null)
                    return newPath;
                lastCell = GetCellNum(actualCell.Substring(1));
            }
            Destination = GetCellNum(_strPath.Substring(_strPath.Length - 2, 2));
            NewDirection = Util.GetDirNum(_strPath.Substring(_strPath.Length - 3, 1));
            return newPath;
        }

        public int GetDistanceBetween(int id1, int id2)
        {
            if (id1 == id2) return 0;
            if (_map == null) return 0;
            int diffX = Math.Abs(GetCellXCoord(id1, _map.Width) - GetCellXCoord(id2, _map.Width));
            int diffY = Math.Abs(GetCellYCoord(id1, _map.Width) - GetCellYCoord(id2, _map.Width));
            return (diffX + diffY);
        }

        public int GetEstimateDistanceBetween(int id1, int id2)
        {
            if (id1 == id2) return 0;
            if (_map == null) return 0;
            int diffX = Math.Abs(GetCellXCoord(id1, _map.Width) - GetCellXCoord(id2, _map.Width));
            int diffY = Math.Abs(GetCellYCoord(id1, _map.Width) - GetCellYCoord(id2, _map.Width));
            return int.Parse(Math.Truncate(Math.Sqrt(Math.Pow(diffX, 2) + Math.Pow(diffY, 2))).ToString());
        }

        public static int GetCellXCoord(int cellid, int width)
        {
            int w = width;
            return ((cellid - (w - 1) * GetCellYCoord(cellid, width)) / w);
        }

        public static int GetCellYCoord(int cellid, int width)
        {
            int w = width;
            int loc5 = (int)(cellid / ((w * 2) - 1));
            int loc6 = cellid - loc5 * ((w * 2) - 1);
            int loc7 = loc6 % w;
            return (loc5 - loc7);
        }

        public int GetDirection(int Cell, int Cell2)
        {

            int MapWidth = _map.Width;

            int[] ListChange = {
                    1,
                    MapWidth,
                    MapWidth * 2 - 1,
                    MapWidth - 1,
                    -1,
                    -MapWidth,
                    -MapWidth * 2 + 1,
                    -(MapWidth - 1)
                                };

            dynamic Result = Cell2 - Cell;

            for (int i = 7; i >= 0; i += -1)
            {
                if (Result == ListChange[i])
                    return i;
            }

            int ResultX = GetCellXCoord(Cell2, _map.Width) - GetCellXCoord(Cell, _map.Width);
            int ResultY = GetCellYCoord(Cell2, _map.Width) - GetCellYCoord(Cell, _map.Width);

            if (ResultX == 0)
            {
                if (ResultY > 0)
                    return 3;
                return 7;
            }
            else if (ResultX > 0)
            {
                return 1;
            }
            else
            {
                return 5;
            }
        }

        public static int GetDirection(int Cell, int Cell2, Map.Map _map)
        {

            int MapWidth = _map.Width;

            int[] ListChange = {
                    1,
                    MapWidth,
                    MapWidth * 2 - 1,
                    MapWidth - 1,
                    -1,
                    -MapWidth,
                    -MapWidth * 2 + 1,
                    -(MapWidth - 1)
                                };

            dynamic Result = Cell2 - Cell;

            for (int i = 7; i >= 0; i += -1)
            {
                if (Result == ListChange[i])
                    return i;
            }

            int ResultX = GetCellXCoord(Cell2, _map.Width) - GetCellXCoord(Cell, _map.Width);
            int ResultY = GetCellYCoord(Cell2, _map.Width) - GetCellYCoord(Cell, _map.Width);

            if (ResultX == 0)
            {
                if (ResultY > 0)
                    return 3;
                return 7;
            }
            else if (ResultX > 0)
            {
                return 1;
            }
            else
            {
                return 5;
            }
        }

        public static string CreateStringPath(int baseCell, int baseDir, List<int> cells, Map.Map map)
        {
            string path = GetDirChar(baseDir) + GetCellChars(baseCell);
            foreach (int cell in cells)
            {
                path += GetDirChar(GetDirection(baseCell, cell, map)) + GetCellChars(cell);
                baseCell = cell;
            }
            return path;
        }

        public static int NextCell(int cell, int dir, Map.Map _map)
        {
            switch (dir)
            {
                case 0:
                    return cell + 1;

                case 1:
                    return cell + _map.Width;

                case 2:
                    return cell + (_map.Width * 2) - 1;

                case 3:
                    return cell + _map.Width - 1;

                case 4:
                    return cell - 1;

                case 5:
                    return cell - _map.Width;

                case 6:
                    return cell - (_map.Width * 2) + 1;

                case 7:
                    return cell - _map.Width + 1;

            }
            return -1;
        }

        public static List<int> GetJoinCell(int cell, Map.Map map)
        {
            List<int> cells = new List<int>();
            cells.Add(NextCell(cell, 1, map));
            cells.Add(NextCell(cell, 3, map));
            cells.Add(NextCell(cell, 5, map));
            cells.Add(NextCell(cell, 7, map));
            return cells;
        }

        public static int GetRemoteCaseInThisDir(int dir, int distance, int cell, Map.Map map)
        {
            int lastCell = cell;
            for (int i = 0; i <= distance; i++)
            {
                lastCell = sNextCell(lastCell, dir, map);
            }
            return lastCell;
        }

        public static bool InLine(Map.Map map, int cell1, int cell2)
        {
            bool isX = GetCellXCoord(cell1, map.Width) == GetCellXCoord(cell2, map.Width);
            bool isY = GetCellYCoord(cell1, map.Width) == GetCellYCoord(cell2, map.Width);

            return isX || isY;
        }

        public static int GetCaseIDFromDirection(int caseID, char direction, Map.Map map, bool fight = false)
        {
            switch (direction)
            {
                case 'a':
                    return fight ? -1 : caseID + 1;
                case 'b':
                    return caseID + map.Width;
                case 'c':
                    return fight ? -1 : caseID + (map.Width * 2 - 1);
                case 'd':
                    return caseID + (map.Width - 1);
                case 'e':
                    return fight ? -1 : caseID - 1;
                case 'f':
                    return caseID - map.Width;
                case 'g':
                    return fight ? -1 : caseID - (map.Width * 2 - 1);
                case 'h':
                    return caseID - map.Width + 1;
            }

            return -1;
        }

        public static List<int> GetAllCellsForThisLinePath(int dir, int baseCell, int remoteCell, Map.Map map)
        {
            List<int> cells = new List<int>();
            if (!InLine(map, baseCell, remoteCell))
            {
                return cells;
            }
            bool pathFinished = false;
            int timeOut = 0;
            while (!pathFinished)
            {
                baseCell = GetCaseIDFromDirection(baseCell, char.Parse(GetDirChar(dir)), map, false);
                if (baseCell == remoteCell)
                {
                    pathFinished = true;
                }
                else
                {
                    cells.Add(baseCell);
                }
                timeOut++;
                if (timeOut >= 30) break;
            }
            return cells;
        }

        public static List<int> GetCircleZone(int baseCell, int radius, Map.Map map)
        {
            List<int> openedList = new List<int>();
            openedList.Add(baseCell);
            for (int i = 0; i <= radius - 1; i++)
            {
                foreach (int cell in openedList.ToArray())
                {
                    List<int> joinedCells = GetJoinCell(cell, map);
                    foreach (int jCell in joinedCells)
                    {
                        if (!openedList.Contains(jCell))
                        {
                            openedList.Add(jCell);
                        }
                    }
                }
            }
            return openedList;
        }

        public static List<int> GetCrossZone(int baseCell, int lenght, Map.Map map)
        {
            List<int> openedList = new List<int>();
            openedList.Add(baseCell);
            int cCell = baseCell;
            for (int i = 0; i <= lenght - 1; i++)
            {
                cCell = NextCell(map, cCell, 1);
                openedList.Add(cCell);
            }
            cCell = baseCell;
            for (int i = 0; i <= lenght - 1; i++)
            {
                cCell = NextCell(map, cCell, 3);
                openedList.Add(cCell);
            }
            cCell = baseCell;
            for (int i = 0; i <= lenght - 1; i++)
            {
                cCell = NextCell(map, cCell, 5);
                openedList.Add(cCell);
            }
            cCell = baseCell;
            for (int i = 0; i <= lenght - 1; i++)
            {
                cCell = NextCell(map, cCell, 7);
                openedList.Add(cCell);
            }
            cCell = baseCell;
            return openedList;
        }

        public static List<int> GetLineZone(int baseCell, int lenght, int dir, Map.Map map)
        {
            var cells = new List<int>();
            for (int i = 0; i <= lenght; i++)
            {

            }
            return cells;
        }
    }
}
