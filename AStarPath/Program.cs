using System.Drawing;
using System.Linq;
using static System.Console;

namespace AStarPath
{
    internal class Program
    {
        static void Main()
        {
            // Summary:
            // This code snippet prompts the user to input three sets of coordinates (id, sp, and ep),
            // converts the input to an array of integers, clears the console, and then utilizes
            // the AStar class to find and visualize the path between the specified points on a grid.
            // User prompt for input
            WriteLine("Give the id, sp, and ep. (0,0,4,6,5,3)");
            // Read input, split by commas, convert to integers, and store in an array
            int[] ints = ReadLine().Split(',').Select(int.Parse).ToArray();
            // Clear the console screen for a clean display
            Clear();
            // Call the AStar.Path method with the provided points
            AStar.Path(new Point(ints[0], ints[1]), new Point(ints[2], ints[3]), new Point(ints[4], ints[5]));
            // Wait for a key press before closing the console window
            ReadKey(true);
        }
    }
}