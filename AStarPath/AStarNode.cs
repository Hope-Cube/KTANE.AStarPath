using System.Collections.Generic;

namespace AStarPath
{
    internal class AStarNode
    {
        // Properties representing the coordinates, obstacle status, neighbors, parent, and costs of the node
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsObstacle { get; set; }
        public List<AStarNode> Neighbors { get; set; }
        public AStarNode Parent { get; set; }
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost => GCost + HCost;
        // Constructor for creating an AStarNode with specified coordinates and obstacle status
        public AStarNode(int x, int y, bool isObstacle = false)
        {
            X = x;
            Y = y;
            IsObstacle = isObstacle;
            Neighbors = new List<AStarNode>();
        }
        // Method to generate neighbors for the node based on a grid
        // This version considers only up, down, left, and right directions (no diagonals)
        // This is optimal for the current scenario with fewer neighbors
        public void GenerateNeighbors(AStarNode[,] grid)
        {
            Neighbors.Clear();
            int gridSizeX = grid.GetLength(0);
            int gridSizeY = grid.GetLength(1);
            // Define the possible offsets for adjacent nodes (up, down, left, right)
            int[] xOffset = { 0, 0, -1, 1 };
            int[] yOffset = { -1, 1, 0, 0 };
            // Check each potential neighbor and add it if within grid bounds
            for (int i = 0; i < xOffset.Length; i++)
            {
                int checkX = X + xOffset[i];
                int checkY = Y + yOffset[i];
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    Neighbors.Add(grid[checkX, checkY]);
                }
            }
        }
        // Original NeighborGenerator method (commented out) considers all eight neighboring nodes, including diagonals
        /*public void GenerateNeighbors(AStarNode[,] grid)
        {
            Neighbors.Clear();
            int gridSizeX = grid.GetLength(0);
            int gridSizeY = grid.GetLength(1);

            // Check each potential neighbor and add it if within grid bounds
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
        }*/
    }
}