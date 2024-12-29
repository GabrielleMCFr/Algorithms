using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// The Ford-Fulkerson algorithm is a method for solving the maximum flow problem in a flow network. 
// It uses augmenting paths to incrementally increase the flow until no more augmenting paths exist.
// Note : it's more flexible and faster than EdmondsKarp, but it can be unreliable : might not terminate depending on how the paths are chosen. 
// Chose EdmondsKarp (slower, but more robust) unless we have a good heuristic to find paths.
// 
// Algorithm Steps
// Initialize Flow:
//    Set the initial flow in all edges to 0.
// Find Augmenting Path:
//    Use DFS or BFS in the residual graph to find a path from the source to the sink where additional flow can be sent.
// Update Flow:
//    Calculate the bottleneck capacity (minimum residual capacity) along the augmenting path.
// Update the flow along the path:
//    Decrease the residual capacity of forward edges.
//    Increase the residual capacity of reverse edges.
// Repeat:
//    Repeat steps 2–3 until no augmenting path can be found.
// Return Maximum Flow:
//    The maximum flow is the sum of the flows sent along all augmenting paths.
//
// Time complexity : O(EF) where E : edges and F : max flow ( because each augmenting path increases the flow by at least 1 unit, and finding a path takes O(E) times)
public class FordFulkerson
{
    public class FlowNetwork
    {
        public int[,] Capacity; // capacity of edges
        public List<int>[] AdjList; // adjacency list of the graph

        public FlowNetwork(int vertices)
        {
            Capacity = new int[vertices, vertices];
            AdjList = new List<int>[vertices];
            for (int i = 0; i < vertices; i++)
                AdjList[i] = new List<int>();
        }

        public void AddEdge(int u, int v, int capacity)
        {
            Capacity[u, v] = capacity; // set capacity for forward edge
            AdjList[u].Add(v);         // add forward edge to adjacency list
            AdjList[v].Add(u);         // add reverse edge to adjacency list
        }
    }

    public int MaxFlow(FlowNetwork graph, int source, int sink)
    {
        int totalFlow = 0;
        int[] parent = new int[graph.Capacity.GetLength(0)];

        // while there exists an augmenting path
        while (DFS(graph, source, sink, parent))
        {
            // find bottleneck capacity
            int pathFlow = int.MaxValue;
            for (int v = sink; v != source; v = parent[v])
            {
                int u = parent[v];
                pathFlow = Math.Min(pathFlow, graph.Capacity[u, v]);
            }

            // update residual capacities of edges and reverse edges
            for (int v = sink; v != source; v = parent[v])
            {
                int u = parent[v];
                graph.Capacity[u, v] -= pathFlow; // reduce capacity on forward edge
                graph.Capacity[v, u] += pathFlow; // increase capacity on reverse edge
            }

            // add path flow to total flow
            totalFlow += pathFlow;
        }

        return totalFlow;
    }

    private bool DFS(FlowNetwork graph, int source, int sink, int[] parent)
    {
        // standard dfs to find an augmenting path
        bool[] visited = new bool[graph.Capacity.GetLength(0)];
        Stack<int> stack = new Stack<int>();
        stack.Push(source);
        visited[source] = true;
        parent[source] = -1;

        while (stack.Count > 0)
        {
            int u = stack.Pop();

            foreach (int v in graph.AdjList[u])
            {
                if (!visited[v] && graph.Capacity[u, v] > 0) // check residual capacity
                {
                    parent[v] = u;
                    visited[v] = true;

                    if (v == sink) // stop if sink is reached
                        return true;

                    stack.Push(v);
                }
            }
        }

        return false; // no augmenting path found
    }

    public static void Main(string[] args)
    {
        // example: graph with 6 vertices (0 to 5)
        int vertices = 6;
        FlowNetwork graph = new FlowNetwork(vertices);

        // add edges with capacities
        graph.AddEdge(0, 1, 16);
        graph.AddEdge(0, 2, 13);
        graph.AddEdge(1, 2, 10);
        graph.AddEdge(1, 3, 12);
        graph.AddEdge(2, 1, 4);
        graph.AddEdge(2, 4, 14);
        graph.AddEdge(3, 2, 9);
        graph.AddEdge(3, 5, 20);
        graph.AddEdge(4, 3, 7);
        graph.AddEdge(4, 5, 4);

        var ff = new FordFulkerson();
        int maxFlow = ff.MaxFlow(graph, 0, 5);

        Console.WriteLine($"Maximum Flow: {maxFlow}");
    }
}


