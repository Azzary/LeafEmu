using LeafEmu.World.Game.Map.Cell;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeafEmu.World.Game.Pathfinding
{
    class Pathfinding
    {



        public static char getDirBetweenTwoCase(int cell1ID, int cell2ID, Game.Map.Map map,
                                        bool Combat)
        {
            List<char> dirs = new List<char>();
            dirs.Add('b');
            dirs.Add('d');
            dirs.Add('f');
            dirs.Add('h');
            if (!Combat)
            {
                dirs.Add('a');
                dirs.Add('b');
                dirs.Add('c');
                dirs.Add('d');
            }
            foreach (char c in dirs)
            {
                int cell = cell1ID;
                for (int i = 0; i <= 64; i++)
                {
                    if (GetCaseIDFromDirrection(cell, c, map, Combat) == cell2ID)
                        return c;
                    cell = GetCaseIDFromDirrection(cell, c, map, Combat);
                }
            }
            return '0';
        }

        public static int GetDirNum(char DirChar)
        {
            string hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            return hash.IndexOf(DirChar);
        }


        public static string convert_astar_path_to_dofus_path(List<int> path, Map.Map map)
        {
            string le_path = "";
            int last_dir = -1;
            int last_cell;
            for (int i = 1; i < path.Count; i++)
            {
                last_cell = path[i - 1];
                int cell = path[i];
                int dir = Util.GetDirNum(getDirBetweenTwoCase(last_cell, cell, map, false).ToString());
                if (i == 1)
                {
                    last_dir = dir;
                }

                if (dir != last_dir)
                {
                    le_path += Util.GetDirChar(last_dir) + Util.CellToChar(last_cell);
                }
                last_dir = dir;
            }

            last_cell = path[path.Count - 1];

            le_path += Util.GetDirChar(last_dir) + Util.CellToChar(last_cell);
            return le_path;
        }


        public static List<int> GetJoinCell(int cell, Map.Map map, bool inFight = true)
        {
            List<int> cells = new List<int>();

            if (!inFight)
            {
                cells.Add(NextCell(cell, 6, map));//356
                cells.Add(NextCell(cell, 5, map));//370
                cells.Add(NextCell(cell, 4, map));//384
                cells.Add(NextCell(cell, 3, map));//399
                cells.Add(NextCell(cell, 2, map));//414
                cells.Add(NextCell(cell, 1, map));//400
                cells.Add(NextCell(cell, 0, map));//386
                cells.Add(NextCell(cell, 7, map));//371
            }
            else
            {
                cells.Add(NextCell(cell, 1, map));
                cells.Add(NextCell(cell, 3, map));
                cells.Add(NextCell(cell, 5, map));
                cells.Add(NextCell(cell, 7, map));
            }
            return cells;
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
            string path = Util.GetDirChar(baseDir) + Util.CellToChar(baseCell);
            foreach (int cell in cells)
            {
                path += Util.GetDirChar(GetDirection(baseCell, cell, map)) + Util.CellToChar(cell);
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

        public static int NextCell(int cell, char dir, Game.Map.Map map)
        {
            switch (dir)
            {// mag.get_w() = te da el ancho del mapa
                case 'b':
                    return cell + map.Width; // diagonal derecha abajo
                case 'd':
                    return cell + (map.Width - 1); // diagonal izquierda abajo
                case 'f':
                    return cell - map.Width; // diagonal izquierda arriba
                case 'h':
                    return cell - map.Width + 1;// diagonal derecha arriba
            }
            return -1;
        }

        public static bool InLine(int cell1, int cell2, Game.Map.Map _map)
        {
            bool isX = GetCellXCoord(cell1, _map.Width) == GetCellXCoord(cell2, _map.Width);
            bool isY = GetCellYCoord(cell1, _map.Width) == GetCellYCoord(cell2, _map.Width);
            return isX || isY;
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

        public static int getDistanceBetween(Game.Map.Map map, int id1, int id2)
        {
            if (id1 == id2)
                return 0;
            if (map == null)
                return 0;

            int diffX = Math.Abs(GetCellXCoord(id1, map.Width) - GetCellXCoord(id2, map.Width));
            int diffY = Math.Abs(GetCellYCoord(id1, map.Width) - GetCellYCoord(id2, map.Width));
            return (diffX + diffY);
        }

        public static int GetCaseIDFromDirrection(int CaseID, char Direction,
                                          Game.Map.Map map, bool Combat)
        {
            if (map == null)
                return -1;
            switch (Direction)
            {
                case 'a':
                    return Combat ? -1 : CaseID + 1;
                case 'b':
                    return CaseID + map.Width;
                case 'c':
                    return Combat ? -1 : CaseID + (map.Width * 2 - 1);
                case 'd':
                    return CaseID + (map.Width - 1);
                case 'e':
                    return Combat ? -1 : CaseID - 1;
                case 'f':
                    return CaseID - map.Width;
                case 'g':
                    return Combat ? -1 : CaseID - (map.Width * 2 - 1);
                case 'h':
                    return CaseID - map.Width + 1;
            }
            return -1;
        }

        public static List<Cell> getCellListFromAreaString(Entity.Entity entity,
                                                            int cellID, int castCellID, String zoneStr, int PONum, bool isCC)
        {
            Game.Map.Map map = entity.CurrentFight.map;
            List<Cell> cases = new List<Cell>();
            int c = PONum;
            if (map.Cells[cellID] == null)
                return cases;
            if (zoneStr != "Xb")
                cases.Add(map.Cells[cellID]);
            int taille = Util.getIntByHashedValue(zoneStr.ToCharArray()[c + 1]);
            char[] dirs;
            switch (zoneStr.ToCharArray()[c])
            {
                case 'C':// Cercle 
                         // Ajouter un comdition si le sort touche toute la map, pas besoin de trouver les cells juste prendre les cells des entity prmCLient.acount.character.CurentFight.ListEntity
                    if (zoneStr.Equals("C_C_C_C_"))
                    {
                        cases.Clear();
                        cases.AddRange(entity.CurrentFight.PlayerInFight.Select(
                            x => entity.CurrentFight.map.Cells[x.account.character.FightInfo.FightCell]));
                        break;
                    }
                    for (int a = 0; a < taille; a++)
                    {
                        dirs = new char[] { 'b', 'd', 'f', 'h' };
                        List<Cell> cases2 = new List<Cell>(cases);// on evite les
                                                                  // modifications
                                                                  // concurrentes
                        foreach (Cell aCell in cases2)
                        {
                            foreach (char d in dirs)
                            {
                                int id = GetCaseIDFromDirrection(aCell.ID, d, map, true);
                                if (!(map.Cells.Count > id) || !(id > -1))
                                    continue;
                                Cell cell = map.Cells[id];
                                if (!cases.Contains(cell))
                                    cases.Add(cell);
                            }
                        }
                    }
                    break;

                case 'X':// Croix
                    dirs = new char[] { 'b', 'd', 'f', 'h' };
                    foreach (char d in dirs)
                    {
                        int cID = cellID;
                        for (int a = 0; a < taille; a++)
                        {
                            int id = GetCaseIDFromDirrection(cellID, d, map, true);
                            if (!(map.Cells.Count > id) || !(id > -1))
                                continue;
                            cases.Add(map.Cells[id]);
                            cID = GetCaseIDFromDirrection(cID, d, map, true);
                        }
                    }
                    break;

                case 'L':// Ligne
                    char dir = getDirBetweenTwoCase(castCellID, cellID, map, true);
                    for (int a = 0; a < taille; a++)
                    {
                        int id = GetCaseIDFromDirrection(cellID, dir, map, true);
                        if (!(map.Cells.Count > id) || !(id > -1))
                            continue;
                        cases.Add(map.Cells[id]);
                        cellID = GetCaseIDFromDirrection(cellID, dir, map, true);
                    }
                    break;

                case 'P':// Player?

                    break;

                default:
                    break;
            }
            return cases;
        }
    }
}
