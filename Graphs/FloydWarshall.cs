using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms.Graphs;

// The Floyd-Warshall Algorithm is a dynamic programming algorithm used to find the shortest paths between all pairs 
// of vertices in a weighted graph. It works for both directed and undirected graphs and supports graphs with negative
// edge weights, as long as there are no negative weight cycles. Note : commonly used with adjacency matrixes
//
// When to Use Floyd-Warshall
// All-Pairs Shortest Path Problems:
// When you need to find the shortest paths between all pairs of vertices, not just from a single source.
// Example: You are given a road network, and you want to compute the shortest travel time between every pair of cities.
// Graphs with Negative Edge Weights:
// It works well with graphs that have negative edge weights (but no negative weight cycles).
// Example: In scenarios where "costs" can decrease along a path, such as in financial arbitrage or resource optimization.
// Dense Graphs:
// It is particularly suited for dense graphs, where the number of edges is close to O(V^2), making O(V^3) competitive
// Problems Involving Multiple Queries:
// Once the algorithm precomputes the shortest distances, answering shortest path queries between any two vertices takes  O(1) time.
//
// DO NOT USE for : 
// - single-source shortest path problems (Dijsktra and BellmanFord are better for this), 
// - on sparse graphs (inefficient).
//
// Time Complexity: O(V^3)
// Space Complexity: O(V^2) due to the dist matrix
public class FloydWarshall
{
    public static void FloydWarshallAlgorithm(int[,] graph, int vertices)
    {
        // step 1: initialize the distance matrix using the input graph
        int[,] dist = new int[vertices, vertices];
        
        for (int i = 0; i < vertices; i++) // iterate over all rows
        {
            for (int j = 0; j < vertices; j++) // iterate over all columns
            {
                dist[i, j] = graph[i, j]; // copy the input graph into the distance matrix
            }
        }

        // step 2: iterate through all vertices as intermediate vertices
        for (int k = 0; k < vertices; k++) // k is the intermediate vertex
        {
            // step 3: iterate through all pairs of source and destination vertices
            for (int i = 0; i < vertices; i++) // i is the source vertex
            {
                for (int j = 0; j < vertices; j++) // j is the destination vertex
                {
                    // step 4: check if the path through k is shorter than the current known path
                    if (dist[i, k] != int.MaxValue && dist[k, j] != int.MaxValue) // ensure both edges exist
                    {
                        dist[i, j] = Math.Min(dist[i, j], dist[i, k] + dist[k, j]); // update the shortest path
                    }
                }
            }
        }

        // step 5: print the resulting shortest distances
        PrintSolution(dist, vertices);
    }

    // helper function to print the distance matrix
    private static void PrintSolution(int[,] dist, int vertices)
    {
        Console.WriteLine("shortest distances between all pairs of vertices:");
        for (int i = 0; i < vertices; i++)
        {
            for (int j = 0; j < vertices; j++)
            {
                // if there is no path, print 'inf'
                if (dist[i, j] == int.MaxValue)
                {
                    Console.Write("inf".PadRight(7));
                }
                else
                {
                    Console.Write(dist[i, j].ToString().PadRight(7));
                }
            }
            Console.WriteLine(); // new line after each row
        }
    }

    public static void Main(string[] args)
    {
        // step 0: input the graph as an adjacency matrix
        // use int.MaxValue to represent infinity (no direct edge)
        int[,] graph = {
            { 0, 3, int.MaxValue, 7 },
            { 8, 0, 2, int.MaxValue },
            { 5, int.MaxValue, 0, 1 },
            { 2, int.MaxValue, int.MaxValue, 0 }
        };

        // number of vertices in the graph
        int vertices = 4;

        // call the floyd-warshall algorithm to compute shortest paths
        FloydWarshallAlgorithm(graph, vertices);
    }
}

