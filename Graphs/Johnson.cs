using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// Johnson's Algorithm is an algorithm used to find the shortest paths between all pairs of vertices in a weighted, 
// directed graph, even when the graph contains negative edge weights (but no negative weight cycles). It combines:
// Bellman-Ford Algorithm to reweight the graph. (to eliminate all negative weights while preserving the relative distances between vertices)
// Dijkstra's Algorithm for efficient computation of shortest paths.
//
// note : Johnson is more efficient than FloydWharsall on sparse graphs.
// Complexity : 
// BellmanFord O(VE), Dijkstra O(ElogV) for each vertex, so O(V*ElogV)
// so total : O(VE + V*ELogV) simplifié a O(V*ElogV)
public class Johnson
{
    // represents an edge in the graph
    public class Edge
    {
        public int From, To, Weight;
        public Edge(int from, int to, int weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }
    }

    public static void JohnsonAlgorithm(int vertices, List<Edge> edges)
    {
        // step 1: add a dummy vertex connected to all vertices with weight 0
        int dummy = vertices;
        foreach (var v in Enumerable.Range(0, vertices))
        {
            edges.Add(new Edge(dummy, v, 0));
        }

        // step 2: run Bellman-Ford from the dummy vertex
        // h being shortest distance from the dummy vertex to each vertices, 
        // will be used to reweight the graph to make sure there is no negative value
        int[] h = BellmanFord(vertices + 1, edges, dummy);
        if (h == null)
        {
            Console.WriteLine("Graph contains a negative weight cycle.");
            return;
        }

        // step 3: reweight the graph
        foreach (var edge in edges)
        {
            edge.Weight += h[edge.From] - h[edge.To];
        }

        // step 4: run Dijkstra for each vertex
        foreach (int source in Enumerable.Range(0, vertices))
        {
            int[] distances = Dijkstra(vertices, edges, source, h);

            // print result
            Console.WriteLine($"Shortest paths from vertex {source}:");
            for (int target = 0; target < vertices; target++)
            {
                if (distances[target] == int.MaxValue)
                {
                    Console.WriteLine($"  To {target}: No path");
                }
                else
                {
                    // adjust distances back to the original graph weights
                    int adjustedDistance = distances[target] + h[target] - h[source];

                    Console.WriteLine($"  To {target}: {adjustedDistance}");
                }
            }
        }
    }

    public static int[] BellmanFord(int vertices, List<Edge> edges, int source)
    {
        int[] dist = new int[vertices];
        Array.Fill(dist, int.MaxValue);
        dist[source] = 0;

        for (int i = 0; i < vertices - 1; i++)
        {
            foreach (var edge in edges)
            {
                if (dist[edge.From] != int.MaxValue && dist[edge.From] + edge.Weight < dist[edge.To])
                {
                    dist[edge.To] = dist[edge.From] + edge.Weight;
                }
            }
        }

        // check for negative weight cycles
        foreach (var edge in edges)
        {
            if (dist[edge.From] != int.MaxValue && dist[edge.From] + edge.Weight < dist[edge.To])
            {
                return null; // negative weight cycle detected
            }
        }

        return dist;
    }

    public static int[] Dijkstra(int vertices, List<Edge> edges, int source, int[] h)
    {
        var adjList = new List<(int To, int Weight)>[vertices];
        for (int i = 0; i < vertices; i++)
        {
            adjList[i] = new List<(int To, int Weight)>();
        }

        // populate the adjacency list (ignoring edges involving the dummy node)
        foreach (var edge in edges)
        {
            if (edge.From < vertices && edge.To < vertices) // ignore dummy node edges
            {
                adjList[edge.From].Add((edge.To, edge.Weight));
            }
        }

        int[] dist = new int[vertices];
        Array.Fill(dist, int.MaxValue);
        dist[source] = 0;

        var pq = new PriorityQueue<(int Vertex, int Distance), int>();
        pq.Enqueue((source, 0), 0);

        while (pq.Count > 0)
        {
            var (current, distance) = pq.Dequeue();
            if (distance > dist[current]) continue;

            foreach (var (neighbor, weight) in adjList[current])
            {
                int newDist = dist[current] + weight;
                if (newDist < dist[neighbor])
                {
                    dist[neighbor] = newDist;
                    pq.Enqueue((neighbor, newDist), newDist);
                }
            }
        }

        return dist;
    }

    public static void Main(string[] args)
    {
        // define a graph with negative weights (but no negative cycles)
        List<Edge> edges = new List<Edge>
        {
            new Edge(0, 1, 4),
            new Edge(0, 2, 2),
            new Edge(1, 3, -1),
            new Edge(2, 1, 1),
            new Edge(2, 3, 3)
        };

        int vertices = 4;
        JohnsonAlgorithm(vertices, edges);
    }
}