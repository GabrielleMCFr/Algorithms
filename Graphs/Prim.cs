using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// Prim's algorithm is used to find the Minimum Spanning Tree (MST) of a graph. The MST is a subset of edges that:
// - Connects all vertices in the graph.
// - Has the minimum total weight of edges.
// - Does not form cycles.
// 
// Use Cases
// Network Design:
// - Connecting cities with minimum cost for roads or cables.
// - Designing computer networks with minimal wiring costs.
// Approximation for Problems:
// - Approximate solutions for traveling salesman problems in undirected graphs.
// Clustering:
// - Used in some machine learning clustering algorithms like hierarchical clustering.
//
// How Prim's Algorithm Works
// Start from an arbitrary vertex.
//    Initialize the MST with this vertex.
// Greedily pick the smallest edge:
//    At each step, choose the edge with the smallest weight that connects a vertex in the MST to a vertex outside it.
// Repeat until all vertices are in the MST:
//    Continue adding edges with the smallest weight until all vertices are connected.
//
// It's a greedy algorithm: it chooses the best (locally optimal) edge at each step without reconsidering previous choices.
// Once a vertex is added to the MST, it is never replaced or removed.
// Complexity:
// - O(ElogV) when using a priority queue and an adjacency list representation.
// - O(V^2) for dense graphs with adjacency matrix representation.
// - O(EV) for simpler implementations (e.g., adjacency list without a priority queue).
class PrimAlgorithm
{
    public static void PrimMST(int vertices, List<(int, int)>[] graph)
    {
        // priority queue to pick the edge with the smallest weight
        var pq = new PriorityQueue<(int weight, int vertex, int parent), int>();

        // hashset to track visited vertices
        var visited = new HashSet<int>();

        // to store the edges of the mst
        List<(int, int, int)> mstEdges = new List<(int, int, int)>();

        // add the initial vertex (start from vertex 0)
        pq.Enqueue((0, 0, -1), 0); // weight, current vertex, parent vertex

        // while the mst is incomplete
        while (pq.Count > 0 && mstEdges.Count < vertices - 1)
        {
            // pick the smallest edge from the queue
            var (weight, currentVertex, parentVertex) = pq.Dequeue();

            // if the vertex is already visited, skip it
            if (visited.Contains(currentVertex)) continue;

            // mark the vertex as visited
            visited.Add(currentVertex);

            // if this is not the starting vertex, add the edge to the mst
            if (parentVertex != -1)
            {
                mstEdges.Add((parentVertex, currentVertex, weight));
            }

            // add all edges from this vertex to the queue
            foreach (var (neighbor, edgeWeight) in graph[currentVertex])
            {
                if (!visited.Contains(neighbor))
                {
                    pq.Enqueue((edgeWeight, neighbor, currentVertex), edgeWeight);
                }
            }
        }

        // print the edges in the mst
        Console.WriteLine("edges in the minimum spanning tree:");
        foreach (var (u, v, w) in mstEdges)
        {
            Console.WriteLine($"from {u} to {v} with weight {w}");
        }
    }

    public static void Main(string[] args)
    {
        // input graph as an adjacency list
        var graph = new List<(int, int)>[5];
        for (int i = 0; i < 5; i++) graph[i] = new List<(int, int)>();

        graph[0].Add((1, 2));
        graph[0].Add((3, 6));
        graph[1].Add((0, 2));
        graph[1].Add((2, 3));
        graph[1].Add((3, 8));
        graph[1].Add((4, 5));
        graph[2].Add((1, 3));
        graph[2].Add((4, 7));
        graph[3].Add((0, 6));
        graph[3].Add((1, 8));
        graph[4].Add((1, 5));
        graph[4].Add((2, 7));

        PrimMST(5, graph);
    }
}