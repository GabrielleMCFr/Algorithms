using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// An Eulerian path in a graph is a path that visits every EDGE (not node) exactly once.
// An Eulerian circuit is a special case where the path starts and ends at the same vertex.
// Conditions for Eulerian Path
// - For Undirected Graphs:
//   A graph has an Eulerian path if:
//     - Exactly 0 or 2 vertices have odd degrees.
//   A graph has an Eulerian circuit if:
//     - All vertices have even degrees.
// - For Directed Graphs:
//   A graph has an Eulerian path if:
//     - At most one vertex has (out-degree − in-degree)=1,
//     - At most one vertex has (in-degree − out-degree)=1,
//     - All other vertices have equal in-degrees and out-degrees.
//   A graph has an Eulerian circuit if:
//     - All vertices have equal in-degrees and out-degrees.
//
// note: for undirected graphs, the degree is the number of edges of a node
// for directed graphs, use in-degree (incoming edges) and out-degree (outgoing edges)
// Time complexity : O(V+E)
public class EulerianPath
{
    public class Graph
    {
        public Dictionary<int, List<int>> AdjList = new(); // adjacency list

        public void AddEdge(int u, int v)
        {
            if (!AdjList.ContainsKey(u))
                AdjList[u] = new List<int>();
            if (!AdjList.ContainsKey(v))
                AdjList[v] = new List<int>();

            AdjList[u].Add(v);
            AdjList[v].Add(u);
        }
    }

    public List<int>? FindEulerianPath(Graph graph)
    {
        // work on a cloned adjacency list to avoid modifying the original graph
        var adjList = CloneAdjacencyList(graph.AdjList);

        // check for Eulerian path by counting vertices with odd degree
        int startVertex = -1, oddCount = 0;

        foreach (var vertex in adjList.Keys)
        {
            if (adjList[vertex].Count % 2 != 0)
            {
                oddCount++;
                startVertex = vertex; // pick an odd-degree vertex as the starting point
            }
        }

        // if there are more than 2 vertices with odd degree, no Eulerian path exists
        if (oddCount != 0 && oddCount != 2)
            return null;

        // if all degrees are even, start from any vertex
        if (startVertex == -1)
            startVertex = adjList.Keys.First();

        // initialize stack for dfs-like traversal and a list to store the path
        Stack<int> stack = new();
        List<int> path = new();

        stack.Push(startVertex);

        while (stack.Count > 0)
        {
            int current = stack.Peek();

            // if the current vertex has no unvisited edges, add it to the path
            if (!adjList.ContainsKey(current) || adjList[current].Count == 0)
            {
                path.Add(current);
                stack.Pop(); // backtrack to previous vertex
            }
            else
            {
                // move to the next vertex and remove the edge
                int next = adjList[current][0];
                adjList[current].Remove(next);
                adjList[next].Remove(current);

                stack.Push(next); // continue traversal from the next vertex
            }
        }

        // reverse the path to get the correct traversal order
        path.Reverse();
        return path;
    }

    public List<int>? FindEulerianCircuit(Graph graph)
    {
        // work on a cloned adjacency list to avoid modifying the original graph
        var adjList = CloneAdjacencyList(graph.AdjList);

        // check if the graph has an Eulerian circuit
        if (!HasEulerianCircuit(graph))
            return null;

        // start from any vertex since all vertices have even degree
        int startVertex = adjList.Keys.First();

        // initialize stack for dfs-like traversal and a list to store the circuit
        Stack<int> stack = new();
        List<int> circuit = new();

        stack.Push(startVertex);

        while (stack.Count > 0)
        {
            int current = stack.Peek();

            // if the current vertex has no unvisited edges, add it to the circuit
            if (!adjList.ContainsKey(current) || adjList[current].Count == 0)
            {
                circuit.Add(current);
                stack.Pop(); // backtrack to previous vertex
            }
            else
            {
                // move to the next vertex and remove the edge
                int next = adjList[current][0];
                adjList[current].Remove(next);
                adjList[next].Remove(current);

                stack.Push(next); // continue traversal from the next vertex
            }
        }

        // verify that the circuit forms a closed loop
        if (circuit.Count > 1 && circuit[0] == circuit[^1])
            return circuit;

        return null; // return null if the circuit is not closed
    }

    private Dictionary<int, List<int>> CloneAdjacencyList(Dictionary<int, List<int>> original)
    {
        // create a deep copy of the adjacency list to avoid modifying the original graph
        var clone = new Dictionary<int, List<int>>();
        foreach (var vertex in original.Keys)
        {
            clone[vertex] = new List<int>(original[vertex]);
        }
        return clone;
    }

    public bool HasEulerianCircuit(Graph graph)
    {
        // check if all vertices have even degree
        foreach (var vertex in graph.AdjList.Keys)
        {
            if (graph.AdjList[vertex].Count % 2 != 0)
                return false; // a vertex with odd degree means no Eulerian circuit
        }

        // check if the graph is connected
        return IsGraphConnected(graph);
    }

    private bool IsGraphConnected(Graph graph)
    {
        // perform DFS or BFS to check connectivity
        var visited = new HashSet<int>();
        int startNode = graph.AdjList.Keys.First(); // pick any starting vertex

        DFS(graph, startNode, visited);

        // the graph is connected if all vertices were visited
        return visited.Count == graph.AdjList.Count;
    }

    private void DFS(Graph graph, int current, HashSet<int> visited)
    {
        visited.Add(current);
        foreach (int neighbor in graph.AdjList[current])
        {
            if (!visited.Contains(neighbor))
                DFS(graph, neighbor, visited);
        }
    }

    public static void Main(string[] args)
    {
        // create a graph
        Graph graph = new Graph();
        graph.AddEdge(0, 1);
        graph.AddEdge(0, 2); // comment this for an eulerian circuit
        graph.AddEdge(1, 2);
        graph.AddEdge(2, 3);
        graph.AddEdge(3, 0);

        var ep = new EulerianPath();

        // check for Eulerian circuit
        List<int> circuit = ep.FindEulerianCircuit(graph);
        if (circuit != null)
        {
            Console.WriteLine("Eulerian circuit:");
            Console.WriteLine(string.Join(" -> ", circuit));
        }
        else
        {
            Console.WriteLine("No Eulerian circuit exists.");
        }

        // find Eulerian path
        List<int> path = ep.FindEulerianPath(graph);
        if (path != null)
        {
            Console.WriteLine("Eulerian path:");
            Console.WriteLine(string.Join(" -> ", path));
        }
        else
        {
            Console.WriteLine("No Eulerian path exists.");
        }
    }
}
