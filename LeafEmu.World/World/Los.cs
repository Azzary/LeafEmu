using LeafEmu.World.Game.Map.Cell;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeafEmu.World.World
{
    class Los
    {

        public static bool GetLos(Game.Map.Map Map, Cell CellStart, Cell CellTarget, List<int> TakenCell)
        {
            double x = CellStart.x + 0.5;
            double y = CellStart.y + 0.5;
            double target_x = CellTarget.x + 0.5;
            double target_y = CellTarget.y + 0.5;
            double previous_x = CellStart.x;
            double previous_y = CellStart.y;
            int type = 0;

            double pas;
            double pad_y;

            double pad_x;
            if (Math.Abs(x - target_x) == Math.Abs(y - target_y))
            {
                pas = Math.Abs(x - target_x);
                pad_x = (target_x > x) ? 1 : -1;
                pad_y = (target_y > y) ? 1 : -1;
                type = 1;
            }
            else if (Math.Abs(x - target_x) > Math.Abs(y - target_y))
            {
                pas = Math.Abs(x - target_x);
                pad_x = (target_x > x) ? 1 : -1;
                pad_y = (target_y - y) / pas;
                pad_y = pad_y * 100;
                pad_y = Math.Ceiling(pad_y) / 100;
                type = 2;
            }
            else
            {
                pas = Math.Abs(y - target_y);
                pad_x = (target_x - x) / pas;
                pad_x = pad_x * 100;
                pad_x = Math.Ceiling(pad_x) / 100;
                pad_y = (target_y > y) ? 1 : -1;
                type = 3;
            }

            int error_T = Convert.ToInt32(Math.Round(Math.Floor(Convert.ToDouble((3 + (pas / 2))))));
            int error_info = Convert.ToInt32(Math.Round(Math.Floor(Convert.ToDouble((97 - (pas / 2))))));

            for (int i = 0; i < pas; i++)
            {
                double cellX, cellY;
                double xPadX = x + pad_x;
                double yPadY = y + pad_y;

                switch (type)
                {
                    case 2:
                        double beforeY = Math.Ceiling(y * 100 + pad_y * 50) / 100;
                        double afterY = Math.Floor(y * 100 + pad_y * 150) / 100;
                        double diffBeforeCenterY = Math.Floor(Math.Abs(Math.Floor(beforeY) * 100 - beforeY * 100)) / 100;
                        double diffCenterAfterY = Math.Ceiling(Math.Abs(Math.Ceiling(afterY) * 100 - afterY * 100)) / 100;

                        cellX = Math.Floor(xPadX);

                        if (Math.Floor(beforeY) == Math.Floor(afterY))
                        {
                            cellY = Math.Floor(yPadY);
                            if ((beforeY == cellY && afterY < cellY) || (afterY == cellY && beforeY < cellY))
                            {
                                cellY = Math.Ceiling(yPadY);
                            }
                            if (CheckCellHiding(cellX, cellY, Map, TakenCell, CellTarget.ID, previous_x, previous_y)) return true;
                            previous_x = cellX;
                            previous_y = cellY;
                        }
                        else if (Math.Ceiling(beforeY) == Math.Ceiling(afterY))
                        {
                            cellY = Math.Ceiling(yPadY);
                            if ((beforeY == cellY && afterY < cellY) || (afterY == cellY && beforeY < cellY))
                            {
                                cellY = Math.Floor(yPadY);
                            }

                            if (CheckCellHiding(cellX, cellY, Map, TakenCell, CellTarget.ID, previous_x, previous_y))
                                return true;

                            previous_x = cellX;
                            previous_y = cellY;
                        }
                        else if (Math.Floor(diffBeforeCenterY * 100) <= error_T)
                        {
                            if (CheckCellHiding(cellX, Math.Floor(afterY), Map, TakenCell, CellTarget.ID, previous_x, previous_y)) return true;
                            previous_x = cellX;
                            previous_y = Math.Floor(afterY);
                        }
                        else if (Math.Floor(diffCenterAfterY * 100) >= error_info)
                        {
                            if (CheckCellHiding(cellX, Math.Floor(beforeY), Map, TakenCell, CellTarget.ID, previous_x, previous_y)) return true;
                            previous_x = cellX;
                            previous_y = Math.Floor(beforeY);
                        }
                        else
                        {
                            if (CheckCellHiding(cellX, Math.Floor(beforeY), Map, TakenCell, CellTarget.ID, previous_x, previous_y)) return true;
                            previous_x = cellX;
                            previous_y = Math.Floor(beforeY);
                            if (CheckCellHiding(cellX, Math.Floor(afterY), Map, TakenCell, CellTarget.ID, previous_x, previous_y)) return true;
                            previous_y = Math.Floor(afterY);
                        }
                        break;

                    case 3:
                        double beforeX = Math.Ceiling(x * 100 + pad_x * 50) / 100;
                        double afterX = Math.Floor(x * 100 + pad_x * 150) / 100;
                        double diffBeforeCenterX = Math.Floor(Math.Abs(Math.Floor(beforeX) * 100 - beforeX * 100)) / 100;
                        double diffCenterAfterX = Math.Ceiling(Math.Abs(Math.Ceiling(afterX) * 100 - afterX * 100)) / 100;

                        cellY = Math.Floor(yPadY);

                        if (Math.Floor(beforeX) == Math.Floor(afterX))
                        {
                            cellX = Math.Floor(xPadX);
                            if ((beforeX == cellX && afterX < cellX) || (afterX == cellX && beforeX < cellX))
                            {
                                cellX = Math.Ceiling(xPadX);
                            }
                            if (CheckCellHiding(cellX, cellY, Map, TakenCell, CellTarget.ID, previous_x, previous_y))
                                return true;
                            previous_x = cellX;
                            previous_y = cellY;
                        }
                        else if (Math.Ceiling(beforeX) == Math.Ceiling(afterX))
                        {
                            cellX = Math.Ceiling(xPadX);

                            if ((beforeX == cellX && afterX < cellX) || (afterX == cellX && beforeX < cellX))
                                cellX = Math.Floor(xPadX);

                            if (CheckCellHiding(cellX, cellY, Map, TakenCell, CellTarget.ID, previous_x, previous_y))
                                return true;

                            previous_x = cellX;
                            previous_y = cellY;
                        }
                        else if (Math.Floor(diffBeforeCenterX * 100) <= error_T)
                        {
                            if (CheckCellHiding(Math.Floor(afterX), cellY, Map, TakenCell, CellTarget.ID, previous_x, previous_y))
                                return true;

                            previous_x = Math.Floor(afterX);
                            previous_y = cellY;
                        }
                        else if (Math.Floor(diffCenterAfterX * 100) >= error_info)
                        {
                            if (CheckCellHiding(Math.Floor(beforeX), cellY, Map, TakenCell, CellTarget.ID, previous_x, previous_y))
                                return true;

                            previous_x = Math.Floor(beforeX);
                            previous_y = cellY;
                        }
                        else
                        {
                            if (CheckCellHiding(Math.Floor(beforeX), cellY, Map, TakenCell, CellTarget.ID, previous_x, previous_y)) return true;
                            previous_x = Math.Floor(beforeX);
                            previous_y = cellY;
                            if (CheckCellHiding(Math.Floor(afterX), cellY, Map, TakenCell, CellTarget.ID, previous_x, previous_y)) return true;
                            previous_x = Math.Floor(afterX);
                        }
                        break;

                    default:
                        if (CheckCellHiding(Math.Floor(xPadX), Math.Floor(yPadY), Map, TakenCell, CellTarget.ID, previous_x, previous_y))
                            return true;
                        previous_x = Math.Floor(xPadX);
                        previous_y = Math.Floor(yPadY);
                        break;
                }

                x = (x * 100 + pad_x * 100) / 100;
                y = (y * 100 + pad_y * 100) / 100;
            }
            return false;
        }

        private static bool CheckCellHiding(double x, double y, Game.Map.Map map, List<int> occupiedCells, int targetCellId, double lastX, double lastY)
        {
            Cell mp = GetCellByCoordinates((int)x, (int)y, map.Cells);
            return mp.IsSightBlocker || (mp.ID != targetCellId && occupiedCells.Contains(mp.ID));
        }


        public static Cell GetCellByCoordinates(int prmX, int prmY, List<Cell> mapCells) => mapCells.FirstOrDefault(cell => cell.x == prmX && cell.y == prmY);
    }
}
