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

    public List<int> FindEulerianPath(Graph graph)
    {
        // check for Eulerian path
        int startVertex = -1, oddCount = 0;

        foreach (var vertex in graph.AdjList.Keys)
        {
            if (graph.AdjList[vertex].Count % 2 != 0)
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
            startVertex = graph.AdjList.Keys.First();

        // sort adjacency lists for deterministic traversal order
        foreach (var vertex in graph.AdjList.Keys)
        {
            graph.AdjList[vertex].Sort();
        }

        // perform Hierholzer's algorithm
        Stack<int> stack = new();
        List<int> path = new();

        stack.Push(startVertex);

        while (stack.Count > 0)
        {
            int current = stack.Peek();

            // if the current vertex has no more unvisited edges, add to path
            if (!graph.AdjList.ContainsKey(current) || graph.AdjList[current].Count == 0)
            {
                path.Add(current);
                stack.Pop();
            }
            else
            {
                // move to the next vertex and remove the edge
                int next = graph.AdjList[current][0];
                graph.AdjList[current].Remove(next);
                graph.AdjList[next].Remove(current);

                stack.Push(next);
            }
        }

        // reverse the path to get the correct traversal order
        path.Reverse();
        return path;
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
        graph.AddEdge(0, 2);
        graph.AddEdge(1, 2);
        graph.AddEdge(2, 3);
        graph.AddEdge(3, 0);

        var ep = new EulerianPath();

        // check for Eulerian circuit
        if (ep.HasEulerianCircuit(graph))
        {
            Console.WriteLine("The graph has an Eulerian circuit.");
        }
        else
        {
            Console.WriteLine("The graph does not have an Eulerian circuit.");
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
