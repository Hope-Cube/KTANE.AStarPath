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
            WriteLine("Give the id, sp and ep. (0,0,4,6,5,3)");
            int[] ints = ReadLine().Split(',').Select(int.Parse).ToArray();
            Clear();
            AStar.Path(new Point(ints[0], ints[1]), new Point(ints[2], ints[3]), new Point(ints[4], ints[5]));
            ReadKey(true);
        }
    }
}