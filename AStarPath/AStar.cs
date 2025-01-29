using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AStarPath
{
    internal class AStar
    {
        public static void Path(Point identifier, Point start, Point end)
        {
            AStarNode[,] grid = InitializeGrid();
            HashSet<Point> obstacles = GetObstacles(identifier);
            MarkObstacles(grid, obstacles);

            foreach (var node in grid)
                node.GenerateNeighbors(grid);

            List<AStarNode> path = FindPath(grid[GetIndex(start.X), GetIndex(start.Y)],
                                           grid[GetIndex(end.X), GetIndex(end.Y)]);

            DisplayPath(path);
        }

        private static AStarNode[,] InitializeGrid()
        {
            var grid = new AStarNode[11, 11];
            for (int x = 0; x < 11; x++)
                for (int y = 0; y < 11; y++)
                    grid[x, y] = new AStarNode(x, y);
            return grid;
        }

        private static HashSet<Point> GetObstacles(Point identifier)
        {
            var obstacles = new HashSet<Point>();
            int[] id = GetField(identifier);
            using (Bitmap field = new Bitmap("field.png"))
            {
                for (int y = 0; y < 11; y++)
                    for (int x = 0; x < 11; x++)
                        if (field.GetPixel(id[1] * 11 + x, id[0] * 11 + y) == Color.Black)
                            obstacles.Add(new Point(x, y));
            }
            return obstacles;
        }

        private static void MarkObstacles(AStarNode[,] grid, HashSet<Point> obstacles)
        {
            foreach (var point in obstacles)
                grid[point.X, point.Y].IsObstacle = true;
        }

        private static List<AStarNode> FindPath(AStarNode start, AStarNode end)
        {
            var openSet = new SortedSet<AStarNode>(Comparer<AStarNode>.Create((a, b) =>
                a.FCost == b.FCost ? a.HCost.CompareTo(b.HCost) : a.FCost.CompareTo(b.FCost)));
            var closedSet = new HashSet<AStarNode>();

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                AStarNode current = openSet.Min;
                openSet.Remove(current);
                closedSet.Add(current);

                if (current == end)
                    return RetracePath(start, end);

                foreach (AStarNode neighbor in current.Neighbors)
                {
                    if (neighbor.IsObstacle || closedSet.Contains(neighbor)) continue;

                    int newCost = current.GCost + GetDistance(current, neighbor);
                    if (newCost < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = newCost;
                        neighbor.HCost = GetDistance(neighbor, end);
                        neighbor.Parent = current;
                        openSet.Add(neighbor);
                    }
                }
            }
            return null;
        }

        private static List<AStarNode> RetracePath(AStarNode start, AStarNode end)
        {
            var path = new List<AStarNode>();
            for (var current = end; current != start; current = current.Parent)
                path.Add(current);
            path.Reverse();
            return path;
        }

        private static void DisplayPath(List<AStarNode> path)
        {
            if (path == null)
            {
                Console.WriteLine("No path found.");
                return;
            }

            Console.WriteLine("Path found:");
            foreach (var step in path)
                Console.WriteLine($"({step.X}, {step.Y})");
        }

        private static int[] GetField(Point idp) => new Dictionary<Point, int[]>
        {
            [new Point(0, 1)] = new int[] { 0, 0 },
            [new Point(5, 2)] = new int[] { 0, 0 },
            [new Point(1, 3)] = new int[] { 1, 0 },
            [new Point(4, 1)] = new int[] { 1, 0 },
            [new Point(3, 3)] = new int[] { 2, 0 },
            [new Point(5, 3)] = new int[] { 2, 0 }
        }.TryGetValue(idp, out var value) ? value : new int[] { };

        private static int GetIndex(int num) => (num >= 1 && num <= 6) ? (num * 2) - 2 : -1;

        private static int GetDistance(AStarNode a, AStarNode b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}
