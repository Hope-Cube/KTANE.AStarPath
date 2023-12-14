using System.Collections.Generic;

namespace AStarPath
{
    internal class AStarNode
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsObstacle { get; set; }
        public List<AStarNode> Neighbors { get; set; }
        public AStarNode Parent { get; set; }
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost => GCost + HCost;

        public AStarNode(int x, int y, bool isObstacle = false)
        {
            X = x;
            Y = y;
            IsObstacle = isObstacle;
            Neighbors = new List<AStarNode>();
        }

        public void GenerateNeighbors(AStarNode[,] grid)
        {
            Neighbors.Clear();

            int gridSizeX = grid.GetLength(0);
            int gridSizeY = grid.GetLength(1);

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = X + x;
                    int checkY = Y + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        Neighbors.Add(grid[checkX, checkY]);
                    }
                }
            }
        }
    }
}