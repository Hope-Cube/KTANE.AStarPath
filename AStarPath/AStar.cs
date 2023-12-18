using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Console;

namespace AStarPath
{
    internal class AStar
    {
        /// <summary>
        /// Finds and visualizes the path between two points on a grid using the A* algorithm.
        /// </summary>
        /// <param name="identifier">The identifier point used to determine the grid and obstacles</param>
        /// <param name="sp">The starting point</param>
        /// <param name="ep">The ending point</param>
        public static void Path(Point identifier, Point sp, Point ep)
        {
            // Create a grid of AStarNodes
            AStarNode[,] grid = new AStarNode[11, 11];
            for (int x = 0; x < 11; x++)
                for (int y = 0; y < 11; y++)
                    grid[x, y] = new AStarNode(x, y);
            // Initialize a list to store obstacle points
            List<Point> points = new List<Point>();
            // Retrieve field information based on the identifier
            int[] id = GetField(identifier);
            // Load the field image and identify obstacle points
            using (Bitmap field = new Bitmap("field.png"))
                for (int y = 0; y < 11; y++)
                    for (int x = 0; x < 11; x++)
                        if (field.GetPixel((id[1] * 11) + x, (id[0] * 11) + y) == Color.FromArgb(0, 0, 0))
                            points.Add(new Point(x, y));
            // Mark AStarNodes corresponding to obstacle points
            foreach (Point point in points)
                grid[point.X, point.Y].IsObstacle = true;
            // Generate neighbors for each AStarNode in the grid
            for (int x = 0; x < 11; x++)
                for (int y = 0; y < 11; y++)
                    grid[x, y].GenerateNeighbors(grid);
            // Retrieve AStarNodes for the starting and ending points
            AStarNode startNode = grid[GetIndexByNumber(sp.X), GetIndexByNumber(sp.Y)];
            AStarNode endNode = grid[GetIndexByNumber(ep.X), GetIndexByNumber(ep.Y)];
            // Find the path using the A* algorithm
            List<AStarNode> path = FindPath(startNode, endNode);
            // Process and visualize the path
            string direction = "";
            if (path != null)
            {
                WriteLine("Path found:");
                // Iterate through the path and mark the nodes
                for (int i = 0; i < path.Count - 1; i += 2)
                {
                    AStarNode currentNode = path[i];
                    AStarNode nextNode = (i + 1 < path.Count) ? path[i + 1] : null;
                    int dx = (nextNode != null) ? nextNode.X - currentNode.X : 0;
                    int dy = (nextNode != null) ? nextNode.Y - currentNode.Y : 0;
                    direction += GetDirection(dx, dy) + "\n";
                }
            }
            else
            {
                // Display a message if no path is found
                direction = "No path found.";
            }
            Write(direction);
        }
        /// <summary>
        /// Retrieves the field information based on the provided Point coordinates.
        /// </summary>
        /// <param name="idp">The Point coordinates</param>
        /// <returns>
        /// An array representing the field information, where the first element
        /// indicates the row and the second element indicates the column.
        /// </returns>
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
        /// <summary>
        /// Gets the direction based on the provided changes in x and y coordinates.
        /// Returns "right" for positive x, "left" for negative x, "down" for positive y,
        /// "up" for negative y, and "unknown" if both x and y are zero.
        /// </summary>
        /// <param name="dx">Change in x coordinate</param>
        /// <param name="dy">Change in y coordinate</param>
        /// <returns>The direction as a string</returns>
        private static string GetDirection(int dx, int dy) => (dx == 1) ? "right" : (dx == -1) ? "left" : (dy == 1) ? "down" : (dy == -1) ? "up" : "unknown";
        /// <summary>
        /// Calculates the index based on the provided number.
        /// If the number is between 1 and 6 (inclusive), the corresponding index is calculated as (number * 2) - 2.
        /// Returns -1 if the number is outside the valid range.
        /// </summary>
        /// <param name="number">The input number</param>
        /// <returns>The calculated index or -1 for an invalid number</returns>
        private static int GetIndexByNumber(int number) => (number >= 1 && number <= 6) ? (number * 2) - 2 : -1;
        /// <summary>
        /// Finds the path using the A* algorithm between the provided start and end nodes.
        /// </summary>
        /// <param name="startNode">The starting node of the path</param>
        /// <param name="endNode">The destination node of the path</param>
        /// <returns>
        /// List of AStarNodes representing the path from the start to the end, or null if no path is found.
        /// </returns>
        private static List<AStarNode> FindPath(AStarNode startNode, AStarNode endNode)
        {
            // Initialize the open and closed sets
            List<AStarNode> openSet = new List<AStarNode>();
            HashSet<AStarNode> closedSet = new HashSet<AStarNode>();
            // Add the start node to the open set
            openSet.Add(startNode);
            // Main A* algorithm loop
            while (openSet.Count > 0)
            {
                // Find the node with the lowest FCost in the open set
                AStarNode currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost ||
                        (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                    {
                        currentNode = openSet[i];
                    }
                }
                // Remove the current node from the open set and add it to the closed set
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);
                // Check if the current node is the destination
                if (currentNode == endNode)
                {
                    // Retrace the path if the destination is reached
                    return RetracePath(startNode, endNode);
                }
                // Explore neighbors
                foreach (AStarNode neighbor in currentNode.Neighbors)
                {
                    // Skip obstacles and already processed nodes
                    if (neighbor.IsObstacle || closedSet.Contains(neighbor))
                    {
                        continue;
                    }
                    // Calculate the cost to reach the neighbor from the current node
                    int newCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                    // Update neighbor's costs and parent if a better path is found
                    if (newCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = newCostToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, endNode);
                        neighbor.Parent = currentNode;

                        // Add neighbor to open set if not already present
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
        /// <summary>
        /// Retraces the path from the end node to the start node using parent pointers.
        /// </summary>
        /// <param name="startNode">The starting node of the path</param>
        /// <param name="endNode">The destination node of the path</param>
        /// <returns>
        /// List of AStarNodes representing the path from the start to the end in reverse order.
        /// </returns>
        private static List<AStarNode> RetracePath(AStarNode startNode, AStarNode endNode)
        {
            // Initialize the list to store the path
            List<AStarNode> path = new List<AStarNode>();
            // Start from the end node
            AStarNode currentNode = endNode;
            // Traverse the path using parent pointers until reaching the start node
            while (currentNode != startNode)
            {
                // Add the current node to the path
                path.Add(currentNode);
                // Move to the parent node
                currentNode = currentNode.Parent;
            }
            // Reverse the path to obtain the correct order from start to end
            path.Reverse();
            // Return the retrace path
            return path;
        }
        /// <summary>
        /// Calculates the Manhattan distance between two AStarNodes.
        /// </summary>
        /// <param name="nodeA">The first node</param>
        /// <param name="nodeB">The second node</param>
        /// <returns>The Manhattan distance between the two nodes</returns>
        private static int GetDistance(AStarNode nodeA, AStarNode nodeB) => Math.Abs(nodeA.X - nodeB.X) + Math.Abs(nodeA.Y - nodeB.Y);
    }
}