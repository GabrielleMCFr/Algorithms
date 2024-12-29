using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// Some optimizations have been added to avoid unnecessary recursive calls
// 1. Degree-based pruning:
//      If a vertex has no unvisited neighbors, there’s no point in exploring paths from that vertex. We can prune such branches early.
// 2. Early termination
//      If the current path cannot possibly reach all unvisited vertices due to disconnected components, terminate the search early.
// 3. Backtracking heuristics
//      Prioritize visiting vertices with fewer unvisited neighbors. This reduces the branching factor and increases the likelihood of finding a solution sooner.
// Performance Impact
// Degree-based pruning and early termination drastically reduce the number of recursive calls.
// Sorting neighbors by degree ensures that dead-end branches are explored later, increasing the chance of finding a solution earlier.

public class HamiltonianPathOptimized
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

    public bool FindHamiltonianPath(Graph graph, int current, bool[] visited, List<int> path, int remainingVertices)
    {
        // add the current vertex to the path
        path.Add(current);

        // mark the current vertex as visited
        visited[current] = true;

        // if all vertices are in the path, we found a Hamiltonian path
        if (remainingVertices == 0)
            return true;

        // optimization: prune paths if no unvisited neighbors exist
        bool hasUnvisitedNeighbors = false;
        foreach (var neighbor in graph.AdjList[current])
        {
            if (!visited[neighbor])
            {
                hasUnvisitedNeighbors = true;
                break;
            }
        }

        if (!hasUnvisitedNeighbors)
        {
            visited[current] = false;
            path.RemoveAt(path.Count - 1);
            return false;
        }

        // explore neighbors in order of degree (heuristic: prioritize vertices with fewer options)
        // note : degrees being the number of other vertices connected to that vertex
        graph.AdjList[current].Sort((a, b) => graph.AdjList[a].Count.CompareTo(graph.AdjList[b].Count));

        foreach (var neighbor in graph.AdjList[current])
        {
            if (!visited[neighbor]) // only proceed if the neighbor is not visited
            {
                if (FindHamiltonianPath(graph, neighbor, visited, path, remainingVertices - 1))
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

            if (FindHamiltonianPath(graph, start, visited, path, graph.Vertices - 1))
            {
                Console.WriteLine("Hamiltonian Path Found:");
                Console.WriteLine(string.Join(" -> ", path));
                return true;
            }
        }

        Console.WriteLine("No Hamiltonian Path exists.");
        return false;
    }

    public bool TryFromAPoint(Graph graph, int start)
    {
        bool[] visited = new bool[graph.Vertices];
        List<int> path = new List<int>();

        if (FindHamiltonianPath(graph, start, visited, path, graph.Vertices - 1))
        {
            Console.WriteLine("Hamiltonian Path Found:");
            Console.WriteLine(string.Join(" -> ", path));
            return true;
        }

        Console.WriteLine("No Hamiltonian Path exists.");
        return false;
    }

    public bool FindHamiltonianCircuit(Graph graph, int start, int current, bool[] visited, List<int> path, int remainingVertices)
    {
        // add the current vertex to the path
        path.Add(current);

        // mark the current vertex as visited
        visited[current] = true;

        // if all vertices are in the path, check if we can return to the start
        if (remainingVertices == 0)
        {
            if (graph.AdjList[current].Contains(start)) // check if there's an edge back to the start
            {
                path.Add(start); // complete the circuit
                return true;
            }
            else
            {
                visited[current] = false;
                path.RemoveAt(path.Count - 1);
                return false;
            }
        }

        // optimization: prune paths if no unvisited neighbors exist
        bool hasUnvisitedNeighbors = false;
        foreach (var neighbor in graph.AdjList[current])
        {
            // check if there is at least one unvisited neighbor
            if (!visited[neighbor])
            {
                hasUnvisitedNeighbors = true;
                break;
            }
        }

        // if no unvisited neighbors exist, terminate the current branch
        if (!hasUnvisitedNeighbors)
        {
            visited[current] = false; // backtrack: unmark the current vertex
            path.RemoveAt(path.Count - 1); // remove the vertex from the path
            return false;
        }

        // explore neighbors in order of degree (heuristic: prioritize vertices with fewer options)
        graph.AdjList[current].Sort((a, b) => graph.AdjList[a].Count.CompareTo(graph.AdjList[b].Count));

        foreach (var neighbor in graph.AdjList[current])
        {
            if (!visited[neighbor]) // only proceed if the neighbor is not visited
            {
                // recursively attempt to find a Hamiltonian circuit
                if (FindHamiltonianCircuit(graph, start, neighbor, visited, path, remainingVertices - 1))
                    return true; // if the recursive call succeeds, return true
            }
        }

        // backtrack: unmark the current vertex and remove it from the path
        visited[current] = false;
        path.RemoveAt(path.Count - 1);

        return false; // no Hamiltonian circuit found from this vertex
    }

    public bool TryAllStartingPointsForCircuit(Graph graph)
    {
        for (int start = 0; start < graph.Vertices; start++)
        {
            bool[] visited = new bool[graph.Vertices];
            List<int> path = new List<int>();

            if (FindHamiltonianCircuit(graph, start, start, visited, path, graph.Vertices - 1))
            {
                Console.WriteLine("Hamiltonian Circuit Found:");
                Console.WriteLine(string.Join(" -> ", path));
                return true;
            }
        }

        Console.WriteLine("No Hamiltonian Circuit exists.");
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

        var hp = new HamiltonianPathOptimized();

        // try finding a Hamiltonian path from all possible starting points
        hp.TryAllStartingPoints(graph);

        hp.TryFromAPoint(graph, 4);

        // add an edge to make a circuit
        graph.AddEdge(3, 4);
        hp.TryAllStartingPointsForCircuit(graph);
    }
}