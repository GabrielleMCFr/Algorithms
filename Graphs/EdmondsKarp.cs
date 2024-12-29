using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// The Edmonds-Karp algorithm is an implementation of the Ford-Fulkerson method to compute the maximum flow in a flow network. 
// It uses Breadth-First Search (BFS) to find augmenting paths and iteratively increases the flow along these paths until no more 
// augmenting paths exist.
// Step 1 : Finding augmenting paths:
//    The algorithm uses a breadth-first search (BFS) to find possible paths from point A (source) to point B (sink) in the residual graph.
//    Each path found corresponds to a route through which additional flow can be sent.
// Step 2 : Calculating the sendable flow:
//    Once a path is found, the algorithm determines the minimum residual capacity (the "bottleneck") along the path.
//    his bottleneck represents the maximum amount of flow that can be sent along the path without exceeding the edge capacities.
// Step 3 : Adjusting the residual capacities:
//    For each edge in the path, the algorithm updates the residual capacities:
//       Forward edge: Reduces the available capacity by subtracting the flow sent.
//       Reverse edge: Increases the capacity in the reverse direction to allow for potential flow adjustments if necessary.
// Step 4: Repeating until no augmenting paths remain:
//    The algorithm repeats these steps until no more augmenting paths can be found (i.e., when BFS fails to find a path).
// Step 5 : Returning the maximum total flow:
//    The total flow is the sum of the flows sent along all the augmenting paths found.
//
// Use cases examples:
//   - Computing the maximum flow in a transportation network.
//   - Optimizing resource allocation in networks.
//   - Finding the maximum matching in bipartite graphs.
//   - Optimizing data flow in computer networks.
//   - Determining the minimum cut (capacity of edges separating source and sink) in a graph.
//
// Note about residual graphs :
// How residual capacity works
// Initial Capacity:
// At the start, the residual capacity of an edge is equal to its original capacity, as no flow has been sent through the edge.
// Updating Residual Capacity:
// When flow is sent through an edge, the residual capacity decreases by the amount of flow sent.
// The reverse edge (in the residual graph) is updated with the residual capacity of the flow that can be pushed back (if needed).
// Residual Graph:
// A residual graph is a representation of the flow network where:
// The forward edges have residual capacities equal to the original capacity minus the current flow.
// The reverse edges have residual capacities equal to the current flow.
//
// Time complexity : at worst, O(E*V^2). Since each BFS takes O(V+E) and at worst, it will be repeated E times.
public class EdmondsKarp
{
    public class FlowNetwork
    {
        // residual capacities of the network
        // It's the amount of additional flow that can still pass through an edge in a flow network, given the current flow
        public int[,] Capacity; 
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
            Capacity[u, v] = capacity;
            AdjList[u].Add(v);
            AdjList[v].Add(u); // add reverse edge for residual graph
        }
    }

    public int MaxFlow(FlowNetwork graph, int source, int sink)
    {
        int flow = 0;
        int[] parent = new int[graph.Capacity.GetLength(0)];

        // continue finding augmenting paths until none exist
        // note : the sink in a flow network is the destination node where the flow ends
        while (BFS(graph, source, sink, parent))
        {
            // find the minimum residual capacity along the path
            int pathFlow = int.MaxValue;
            for (int v = sink; v != source; v = parent[v]) // iterating backwards along the augmenting path from the sink to the source
            {
                int u = parent[v];

                // find the bottleneck capacity along the path, which determines the maximum flow possible through this path
                pathFlow = Math.Min(pathFlow, graph.Capacity[u, v]); 
            }

            // update residual capacities of the edges and reverse edges
            for (int v = sink; v != source; v = parent[v])
            {
                int u = parent[v];

                // edge: the original edge in the graph (u -> v)
                // reduce its residual capacity by the flow sent along this path
                graph.Capacity[u, v] -= pathFlow;

                // reverse edge: allows flow to be sent back (v -> u)
                // increase its residual capacity to reflect the flow sent along u -> v
                graph.Capacity[v, u] += pathFlow;
            }

            // add the path flow to the total flow
            flow += pathFlow;
        }

        return flow;
    }

    private bool BFS(FlowNetwork graph, int source, int sink, int[] parent)
    {
        // perform bfs to find an augmenting path
        bool[] visited = new bool[graph.Capacity.GetLength(0)];
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(source);
        visited[source] = true;
        parent[source] = -1;

        while (queue.Count > 0)
        {
            int u = queue.Dequeue();

            foreach (int v in graph.AdjList[u])
            {
                if (!visited[v] && graph.Capacity[u, v] > 0) // check residual capacity
                {
                    queue.Enqueue(v);
                    parent[v] = u;
                    visited[v] = true;

                    if (v == sink) // stop if sink is reached
                        return true;
                }
            }
        }

        return false;
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

        var ek = new EdmondsKarp();
        int maxFlow = ek.MaxFlow(graph, 0, 5);

        Console.WriteLine($"Maximum Flow: {maxFlow}");
    }
}

