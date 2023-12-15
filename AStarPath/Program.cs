using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static System.Console;

namespace AStarPath
{
    internal class Program
    {
        static void Main()
        {
            // Create a 2D array to represent the grid
            AStarNode[,] grid = new AStarNode[11, 11];
            // Initialize the grid with nodes
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    grid[x, y] = new AStarNode(x, y);
                }
            }
            // Create point list
            List<Point> points = new List<Point>();
            using (Bitmap field = new Bitmap("field.png"))
            {
                for (int y = 0; y < field.Height; y++)
                {
                    for (int x = 0; x < field.Width; x++)
                    {
                        if (field.GetPixel(y, x) == Color.FromArgb(0, 0, 0))
                        {
                            points.Add(new Point(y, x));
                        }
                    }
                }
            }
            // Mark obstacles in the grid (for example, obstacle at (5,5))
            foreach (Point point in points)
            {
                grid[point.X, point.Y].IsObstacle = true;
            }
            // Generate neighbors for each node
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    grid[x, y].GenerateNeighbors(grid);
                }
            }
            // Define the start and end nodes
            WriteLine("Set the start and end points' coords! (6,0,0,2)");
            int[] coords = ReadLine().Split(',').Select(int.Parse).ToArray();
            AStarNode startNode = grid[coords[0], coords[1]];
            AStarNode endNode = grid[coords[2], coords[3]];
            // Find the path using A* algorithm
            List<AStarNode> path = AStar.FindPath(startNode, endNode);            
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
                    if (nextNode != null)
                    {
                        pathBitmap.SetPixel(nextNode.X, nextNode.Y, Color.FromArgb(0, 255, 255));
                    }
                }
                // Add the last node to the bitmap
                pathBitmap.SetPixel(path.Last().X, path.Last().Y, Color.FromArgb(0, 255, 255));
                pathBitmap.Save("path.png");
            }
            else
            {

                WriteLine("No path found.");
            }
            ReadKey(true);
        }
        // Helper method to get the direction based on dx and dy
        private static string GetDirection(int dx, int dy)
        {
            if (dx == 1)
            {
                return "right";
            }
            else if (dx == -1)
            {
                return "left";
            }
            else if (dy == 1)
            {
                return "down";
            }
            else if (dy == -1)
            {
                return "up";
            }
            return "unknown";
        }
    }
}