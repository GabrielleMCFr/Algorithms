using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// Dinic’s algorithm (also called Dinitz’s algorithm) is a method to solve the maximum flow problem. 
// It improves upon the Ford-Fulkerson method by using the concept of level graphs and blocking flows. 
// It ensures efficient pathfinding and flow augmentation in the network.
// Note : it performs better than Edmondskarp in dense graphs, or bigger graphs.
//
// Key concepts
// Level graphs: (This separation into levels reduces the number of unnecessary checks and ensures faster convergence to the maximum flow)
//   - A level graph is a subgraph of the residual graph where:
//       - Nodes are assigned levels based on their distance from the source using BFS.
//       - Edges only go from a lower level to a higher level.
// Blocking Flow:
//   - A blocking flow is a flow that saturates (meaning, residual capacity = 0 for at least on edge) 
//     some edges in the level graph, ensuring that no more augmenting paths can be found in the current level graph.
// Iterative Refinement:
//   - The algorithm repeatedly builds level graphs, augments flow using blocking flows, and updates the residual graph 
//     until no more paths exist from the source to the sink.
//
// Time complexity : O((V^2)*E), can improve to O((V^(2/3))) on certain sparse graphs like planar graphs.
public class Dinic
{
    public class FlowNetwork
    {
        public int[,] Capacity; // capacity of edges
        public int[,] Flow;     // current flow on edges
        public List<int>[] AdjList; // adjacency list of the graph

        public FlowNetwork(int vertices)
        {
            Capacity = new int[vertices, vertices];
            Flow = new int[vertices, vertices];
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

        // repeat until no augmenting path exists
        while (true)
        {
            // step 1: construct the level graph using bfs
            int[] level = BuildLevelGraph(graph, source, sink);

            // if sink is not reachable, no more augmenting paths
            if (level[sink] == -1) break;

            // step 2: find blocking flows in the level graph using dfs
            int[] ptr = new int[graph.Capacity.GetLength(0)]; // pointer for dfs optimization
            int flow;
            while ((flow = SendFlow(graph, source, int.MaxValue, sink, ptr, level)) > 0)
            {
                totalFlow += flow; // add the blocking flow to the total flow
            }
        }

        // return the total flow from source to sink
        return totalFlow;
    }

    private int[] BuildLevelGraph(FlowNetwork graph, int source, int sink)
    {
        int[] level = new int[graph.Capacity.GetLength(0)];
        Array.Fill(level, -1); // initialize all levels to -1 (unvisited)

        // bfs to assign levels to nodes
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(source);
        level[source] = 0; // source is at level 0

        while (queue.Count > 0)
        {
            int u = queue.Dequeue();

            // explore all neighbors of the current node
            foreach (int v in graph.AdjList[u])
            {
                // add to level graph if residual capacity exists and not visited
                if (level[v] == -1 && graph.Capacity[u, v] - graph.Flow[u, v] > 0)
                {
                    level[v] = level[u] + 1; // set level of neighbor
                    queue.Enqueue(v); // add to bfs queue
                }
            }
        }

        // return the level array
        return level;
    }

    private int SendFlow(FlowNetwork graph, int u, int flow, int sink, int[] ptr, int[] level)
    {
        // if we reach the sink, return the flow
        if (u == sink) return flow;

        // iterate over neighbors starting from the last processed index
        for (; ptr[u] < graph.AdjList[u].Count; ptr[u]++)
        {
            int v = graph.AdjList[u][ptr[u]];

            // check if the edge is valid in the level graph and has residual capacity
            if (level[v] == level[u] + 1 && graph.Capacity[u, v] - graph.Flow[u, v] > 0)
            {
                // calculate the bottleneck flow
                int bottleneck = Math.Min(flow, graph.Capacity[u, v] - graph.Flow[u, v]);

                // recursively send flow
                int pushed = SendFlow(graph, v, bottleneck, sink, ptr, level);

                if (pushed > 0)
                {
                    // update flow for the forward and reverse edges
                    graph.Flow[u, v] += pushed;
                    graph.Flow[v, u] -= pushed;
                    return pushed;
                }
            }
        }

        // no flow was sent from this node
        return 0;
    }

    public static void Main(string[] args)
    {
        // example: graph with 6 vertices (0 to 5)
        int vertices = 6;
        FlowNetwork graph = new FlowNetwork(vertices);

        // add edges with capacities
        graph.AddEdge(0, 1, 16);
        graph.AddEdge(0, 2, 13);
        graph.AddEdge(1, 3, 12);
        graph.AddEdge(2, 1, 4);
        graph.AddEdge(2, 4, 14);
        graph.AddEdge(3, 5, 20);
        graph.AddEdge(4, 3, 7);
        graph.AddEdge(4, 5, 4);

        // create an instance of dinic's algorithm
        var dinics = new DinicsAlgorithm();

        // calculate the maximum flow from source (0) to sink (5)
        int maxFlow = dinics.MaxFlow(graph, 0, 5);

        // output the result
        Console.WriteLine($"Maximum Flow: {maxFlow}");
    }
}
