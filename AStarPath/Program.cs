using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AStarPath
{
    internal class Program
    {
        static void Main(string[] args)
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
            AStarNode startNode = grid[6, 0];
            AStarNode endNode = grid[0, 2];

            // Find the path using A* algorithm
            List<AStarNode> path = AStar.FindPath(startNode, endNode);

            // Print the path
            if (path != null)
            {
                Console.WriteLine("Path found:");
                Bitmap pathBitmap = new Bitmap(11, 11);

                for (int i = 0; i < path.Count - 1; i++)
                {
                    AStarNode currentNode = path[i];
                    AStarNode nextNode = path[i + 1];

                    int dx = nextNode.X - currentNode.X;
                    int dy = nextNode.Y - currentNode.Y;

                    string direction = GetDirection(dx, dy);

                    Console.WriteLine($"({currentNode.X}, {currentNode.Y}), {direction}");

                    pathBitmap.SetPixel(currentNode.X, currentNode.Y, Color.FromArgb(0, 255, 255));
                }

                // Add the last node to the bitmap
                pathBitmap.SetPixel(path.Last().X, path.Last().Y, Color.FromArgb(0, 255, 255));

                pathBitmap.Save("path.png");
            }
            else
            {
                Console.WriteLine("No path found.");
            }
            Console.ReadKey(true);
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