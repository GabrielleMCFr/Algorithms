using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    public class CircularGraphSequentialPath
    {
        // Min distance in a sequential traversal in a circular graph 
        static void Main()
        {
            // total nb of servers in the graph, 1 to 10 here.
            var total_servers = 10;

            // the list of servers we need to reach
            var servers = new List<int> { 4, 6, 2, 9 };

            var uniqueServers = new HashSet<int>(servers).ToList();
            uniqueServers.Sort(); 
            // Define traversal order
            var traversalOrder = uniqueServers;

            int minTime = 0;

            // Calculate the distances between each server pairs
            for (int i = 1; i < traversalOrder.Count; i++)
            {
                minTime += CalculateDistance(traversalOrder[i - 1], traversalOrder[i], total_servers);
            }

            Console.WriteLine(minTime); 
        }

        // FUnction to calculate the min distance between two nodes on a circular graph
        static int CalculateDistance(int from, int to, int total_servers)
        {
            int clockwise = (to - from + total_servers) % total_servers;
            int counterClockwise = (from - to + total_servers) % total_servers;
            return Math.Min(clockwise, counterClockwise);
        }
    }
}