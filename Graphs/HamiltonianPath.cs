using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// A Hamiltonian path is a path in a graph that visits every vertex exactly once.
// If the path starts and ends at the same vertex, it is called a Hamiltonian cycle.
// It does not require to visit all edges.
//
// Use cases
// Traveling Salesman Problem (TSP):
//      - Find the shortest route that visits all cities exactly once (a Hamiltonian cycle).
// Route Planning:
//      - Determine a route that passes through all required locations exactly once.
// Scheduling Problems:
//      - Arrange tasks so that all are completed exactly once, in a specific order.
//
// Time complexity : O(V!)  because we explore all possible permutations of vertices, but can be optimized with pruning.
public class HamiltonianPath
{
    public class Graph
    {
        public int Vertices;
        public List<int>[] AdjList;

        public Graph(int vertices)
        {
            Vertices = vertices;
            AdjList = new List<int>[vertices];
            for (int i = 0; i < vertices; i++)
                AdjList[i] = new List<int>();
        }

        public void AddEdge(int u, int v)
        {
            AdjList[u].Add(v);
            AdjList[v].Add(u); // assuming an undirected graph
        }
    }

    public bool FindHamiltonianPath(Graph graph, int current, bool[] visited, List<int> path)
    {
        // add the current vertex to the path
        path.Add(current);

        // mark the current vertex as visited
        visited[current] = true;

        // if all vertices are in the path, we found a Hamiltonian path
        if (path.Count == graph.Vertices)
            return true;

        // explore all neighbors of the current vertex
        foreach (var neighbor in graph.AdjList[current])
        {
            if (!visited[neighbor]) // only proceed if the neighbor is not visited
            {
                if (FindHamiltonianPath(graph, neighbor, visited, path))
                    return true; // if the recursive call succeeds, return true
            }
        }

        // backtrack: unmark the current vertex and remove it from the path
        visited[current] = false;
        path.RemoveAt(path.Count - 1);

        return false; // no Hamiltonian path found from this vertex
    }

    public bool TryAllStartingPoints(Graph graph)
    {
        for (int start = 0; start < graph.Vertices; start++)
        {
            bool[] visited = new bool[graph.Vertices];
            List<int> path = new List<int>();

            if (FindHamiltonianPath(graph, start, visited, path))
            {
                Console.WriteLine("Hamiltonian Path Found:");
                Console.WriteLine(string.Join(" -> ", path));
                return true;
            }
        }

        Console.WriteLine("No Hamiltonian Path exists.");
        return false;
    }

    public static void Main(string[] args)
    {
        // create a graph with 5 vertices
        Graph graph = new Graph(5);

        // add edges
        graph.AddEdge(0, 1);
        graph.AddEdge(0, 2);
        graph.AddEdge(1, 2);
        graph.AddEdge(1, 3);
        graph.AddEdge(2, 4);

        var hp = new HamiltonianPath();

        // try finding a Hamiltonian path from all possible starting points
        hp.TryAllStartingPoints(graph);
    }
}

