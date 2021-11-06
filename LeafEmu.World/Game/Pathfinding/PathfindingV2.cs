using LeafEmu.World.Game.Map.Cell;
using System;
using System.Collections.Generic;
using System.Linq;


namespace LeafEmu.World.Game.Pathfinding
{
    public class PathfindingV2
    {
        public Map.Map Map { get; set; }

        public List<CellPathfinding> Cells = new List<CellPathfinding>();
        public static int CELL_DISTANCE_VALUE = 10;

        private List<CellPathfinding> openList = new List<CellPathfinding>();
        private List<CellPathfinding> closedList = new List<CellPathfinding>();
        private bool fightPath;
        public PathfindingV2(Map.Map map, bool fightPath = true)
        {
            this.fightPath = fightPath;
            this.Map = map;
            map.Cells.ForEach(x => Cells.Add(new CellPathfinding(x)));
        }

        private void initialize()
        {
            this.openList = new List<CellPathfinding>();
            this.closedList = new List<CellPathfinding>();
        }

        public CellPathfinding GetCell(int cell)
        {
            return this.Cells.FirstOrDefault(x => x.cell.ID == cell);
        }

        public List<CellPathfinding> FindShortestPath(int startCell, int endCell, List<int> dynObstacles)
        {
            this.initialize();
            try
            {
                var finalPath = new List<CellPathfinding>();
                CellPathfinding startNode = this.GetCell(startCell);
                CellPathfinding endNode = this.GetCell(endCell);

                this.addToOpenList(this.GetCell(startCell));
                CellPathfinding currentNode = null;
                while (this.openList.Count > 0)
                {
                    currentNode = this.getCurrentNode();
                    if (currentNode == endNode)
                        break;

                    this.addToCloseList(currentNode);
                    var neighbours = this.getNeighbours(currentNode, dynObstacles);
                    var maxi = neighbours.Count;
                    for (int i = 0; i < maxi; i++)
                    {
                        var node = neighbours[i];
                        if (this.closedList.Contains(node) || node == null)
                            continue;

                        var newG = node.Parent.g + CELL_DISTANCE_VALUE;
                        var newH = (Math.Abs(endNode.cell.x - node.cell.x) + Math.Abs(endNode.cell.y - node.cell.y));
                        var newF = newH + newG;
                        if (this.openList.Contains(node))
                        {
                            if (newG < node.g)
                            {
                                node.Parent = currentNode;
                                node.g = newG;
                                node.h = newH;
                                node.f = newF;
                            }
                        }
                        else
                        {
                            addToOpenList(node);
                            node.Parent = currentNode;
                            node.g = newG;
                            node.h = newH;
                            node.f = newF;
                        }
                    }
                }

                if (this.openList.Count == 0)
                    return finalPath;

                var lastNode = this.openList.FirstOrDefault(x => x.cell.ID == endCell);
                while (lastNode != startNode)
                {
                    finalPath.Add(lastNode);
                    lastNode = lastNode.Parent;
                }

                finalPath.Reverse();
                return finalPath;
            }
            catch (Exception e)
            {
                //Utilities.ConsoleStyle.Error("Can't find path .. " + e.ToString());
                return new List<CellPathfinding>();
            }
        }

        private void addToCloseList(CellPathfinding cell)
        {
            this.openList.Remove(cell);
            this.closedList.Add(cell);
        }

        private void addToOpenList(CellPathfinding cell)
        {
            this.closedList.Remove(cell);
            this.openList.Add(cell);
        }

        private CellPathfinding getCurrentNode()
        {
            var tmpList = new List<Cell>();
            var maximum = this.openList.Count;
            var minF = 1000000;
            CellPathfinding curNode = null;
            for (int i = 0; i < maximum; i++)
            {
                var node = this.openList[i];
                if (node.f < minF)
                {
                    minF = node.f;
                    curNode = node;
                }
            }
            return curNode;
        }

        private List<CellPathfinding> getNeighbours(CellPathfinding cell, List<int> dyn)
        {
            var neigh = new List<CellPathfinding>();
            var tmpCell = Pathfinding.GetJoinCell(cell.cell.ID, Map, fightPath);
            foreach (var c in tmpCell)
            {
                cell = this.GetCell(c);
                if (cell != null 
                && (cell.cell.IsWalkable || Map.CellForceEnable.Contains(cell.cell.ID)) 
                && !dyn.Contains(c) && !Map.CellForceDisable.Contains(cell.cell.ID))
                {
                    neigh.Add(cell);
                }
            }
            return neigh;
        }
    }
}
