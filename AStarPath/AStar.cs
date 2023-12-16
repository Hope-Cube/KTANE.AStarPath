using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static System.Console;

namespace AStarPath
{
    internal class AStar
    {
        public static void Path(Point identifier, Point sp, Point ep)
        {
            // Create a 2D array to represent the grid
            AStarNode[,] grid = new AStarNode[11, 11];
            // Initialize the grid with nodes
            for (int x = 0; x < 11; x++) for (int y = 0; y < 11; y++) grid[x, y] = new AStarNode(x, y);
            // Create point list
            List<Point> points = new List<Point>();
            int[] id = GetField(identifier);
            using (Bitmap field = new Bitmap("field.png")) for (int y = 0; y < 11; y++) for (int x = 0; x < 11; x++) if (field.GetPixel((id[1] * 11) + x, (id[0] * 11) + y) == Color.FromArgb(0, 0, 0)) points.Add(new Point(x, y));
            // Mark obstacles in the grid (for example, obstacle at (5,5))
            foreach (Point point in points) grid[point.X, point.Y].IsObstacle = true;
            // Generate neighbors for each node
            for (int x = 0; x < 11; x++) for (int y = 0; y < 11; y++) grid[x, y].GenerateNeighbors(grid);
            // Define the start and end nodes
            AStarNode startNode = grid[GetIndexByNumber(sp.X), GetIndexByNumber(sp.Y)];
            AStarNode endNode = grid[GetIndexByNumber(ep.X), GetIndexByNumber(ep.Y)];
            // Find the path using A* algorithm
            List<AStarNode> path = FindPath(startNode, endNode);
            // Print the path
            if (path != null)
            {
                WriteLine("Path found:");
                Bitmap pathBitmap = new Bitmap(11, 11);
                for (int i = 0; i < path.Count - 1; i += 2)
                {
                    AStarNode currentNode = path[i];
                    AStarNode nextNode = (i + 1 < path.Count) ? path[i + 1] : null;
                    int dx = (nextNode != null) ? nextNode.X - currentNode.X : 0;
                    int dy = (nextNode != null) ? nextNode.Y - currentNode.Y : 0;
                    string direction = GetDirection(dx, dy);
                    WriteLine($"{direction}");
                    pathBitmap.SetPixel(currentNode.X, currentNode.Y, Color.FromArgb(0, 255, 255));
                    if (nextNode != null) pathBitmap.SetPixel(nextNode.X, nextNode.Y, Color.FromArgb(0, 255, 255));
                }
                // Add the last node to the bitmap
                pathBitmap.SetPixel(path.Last().X, path.Last().Y, Color.FromArgb(255, 0, 0));
                path.Insert(0, startNode);
                pathBitmap.SetPixel(path.First().X, path.First().Y, Color.FromArgb(0, 255, 0));
                pathBitmap.Save("path.png");
            }
            else WriteLine("No path found.");
        }
        private static int[] GetField(Point idp)
        {
            return
                (idp == new Point(0, 1) || idp == new Point(5, 2)) ? new int[] { 0, 0 } :
                (idp == new Point(1, 3) || idp == new Point(4, 1)) ? new int[] { 1, 0 } :
                (idp == new Point(3, 3) || idp == new Point(5, 3)) ? new int[] { 2, 0 } :
                (idp == new Point(0, 0) || idp == new Point(3, 0)) ? new int[] { 1, 0 } :
                (idp == new Point(3, 5) || idp == new Point(4, 2)) ? new int[] { 1, 1 } :
                (idp == new Point(2, 4) || idp == new Point(4, 0)) ? new int[] { 1, 2 } :
                (idp == new Point(1, 0) || idp == new Point(5, 1)) ? new int[] { 2, 0 } :
                (idp == new Point(2, 3) || idp == new Point(3, 0)) ? new int[] { 2, 1 } :
                (idp == new Point(0, 4) || idp == new Point(2, 1)) ? new int[] { 2, 2 } :
                new int[] { };
        }
        // Helper method to get the direction based on dx and dy
        private static string GetDirection(int dx, int dy)
        {
            return
                (dx == 1) ? "right" :
                (dx == -1) ? "left" :
                (dy == 1) ? "down" :
                (dy == -1) ? "up" :
                "unknown";
        }
        private static int GetIndexByNumber(int number)
        {
            return (number >= 1 && number <= 6) ? (number * 2) - 2 : -1;
        }
        public static List<AStarNode> FindPath(AStarNode startNode, AStarNode endNode)
        {
            List<AStarNode> openSet = new List<AStarNode>();
            HashSet<AStarNode> closedSet = new HashSet<AStarNode>();
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                AStarNode currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                    {
                        currentNode = openSet[i];
                    }
                }
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);
                if (currentNode == endNode)
                {
                    return RetracePath(startNode, endNode);
                }
                foreach (AStarNode neighbor in currentNode.Neighbors)
                {
                    if (neighbor.IsObstacle || closedSet.Contains(neighbor))
                    {
                        continue;
                    }
                    int newCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                    if (newCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = newCostToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, endNode);
                        neighbor.Parent = currentNode;
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
            // Path not found
            return null;
        }
        private static List<AStarNode> RetracePath(AStarNode startNode, AStarNode endNode)
        {
            List<AStarNode> path = new List<AStarNode>();
            AStarNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return path;
        }
        private static int GetDistance(AStarNode nodeA, AStarNode nodeB)
        {
            int distX = Math.Abs(nodeA.X - nodeB.X);
            int distY = Math.Abs(nodeA.Y - nodeB.Y);
            return distX + distY;
        }
    }
}