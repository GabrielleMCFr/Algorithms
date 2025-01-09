using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    /// <summary>
    /// The task is to prevent an agent from reaching designated gateways by severing links in a network of nodes.
    /// In this context, a gateway is a special node in the network that must be protected.
    /// The goal is to determine which links to sever to isolate the agent
    /// </summary>
    public class GatewaysGame
    {
        static void Main(string[] args)
            {
                string[] inputs;

                inputs = Console.ReadLine().Split(' ');
                int N = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
                int L = int.Parse(inputs[1]); // the number of links
                int E = int.Parse(inputs[2]); // the number of exit gateways

                // Graph representation: adjacency list
                Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();
                for (int i = 0; i < N; i++)
                {
                    graph[i] = new List<int>();
                }

                // Parse links
                for (int i = 0; i < L; i++)
                {
                    inputs = Console.ReadLine().Split(' ');
                    int N1 = int.Parse(inputs[0]);
                    int N2 = int.Parse(inputs[1]);
                    graph[N1].Add(N2);
                    graph[N2].Add(N1);
                }

                // Parse gateways
                HashSet<int> gateways = new HashSet<int>();
                for (int i = 0; i < E; i++)
                {
                    int EI = int.Parse(Console.ReadLine());
                    gateways.Add(EI);
                }

                // game loop
                while (true)
                {
                    int SI = int.Parse(Console.ReadLine()); // The index of the node where the agent is located

                    // Find the critical link to sever
                    (int node1, int node2) = FindLinkToSever(graph, gateways, SI);

                    // Sever the link
                    graph[node1].Remove(node2);
                    graph[node2].Remove(node1);

                    // Output the link to sever
                    Console.WriteLine($"{node1} {node2}");
                }
            }

            // If the agent is directly connected to a gateway, the function severs that immediate link.
            // If not, it looks for the next closest link leading to a gateway and severs that.
            static (int, int) FindLinkToSever(Dictionary<int, List<int>> graph, HashSet<int> gateways, int agentPosition)
            {
                // If the agent is directly connected to a gateway, sever that link
                foreach (var neighbor in graph[agentPosition])
                {
                    if (gateways.Contains(neighbor))
                    {
                        return (agentPosition, neighbor);
                    }
                }

                // Otherwise, find the link closest to any gateway
                foreach (var gateway in gateways)
                {
                    foreach (var neighbor in graph[gateway])
                    {
                        return (gateway, neighbor);
                    }
                }

                // Fallback (should not occur with valid input)
                return (-1, -1);
            }
    }
}