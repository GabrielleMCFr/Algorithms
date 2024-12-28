using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// Kosaraju’s Algorithm is used to find Strongly Connected Components (SCCs) in a directed graph. 
// A strongly connected component is a maximal subgraph where every vertex is reachable from every other vertex in the subgraph.

// Steps in Kosaraju's Algorithm
// Perform a DFS on the original graph:
//  - Traverse the graph and push vertices onto a stack in the order of their finishing times.
//    (Finishing time refers to the time when a vertex and all its descendants are fully explored)
// Reverse the graph:
//  - Reverse the directions of all edges in the graph.
// Perform a DFS on the reversed graph:
//  - Pop vertices from the stack (in the order of finishing times) and perform DFS on the reversed graph.
//  - Each DFS traversal gives one strongly connected component (SCC).
// Time Complexity : O(V+E)
public class Kosaraju
{
    // function to perform dfs and record finishing times
    private static void DfsOriginal(int node, Dictionary<int, List<int>> graph, HashSet<int> visited, Stack<int> stack)
    {
        visited.Add(node);
        foreach (var neighbor in graph[node])
        {
            if (!visited.Contains(neighbor))
            {
                DfsOriginal(neighbor, graph, visited, stack);
            }
        }
        stack.Push(node); // push node to stack after exploring all descendants
    }

    // function to perform dfs in the reversed graph
    private static void DfsReversed(int node, Dictionary<int, List<int>> graph, HashSet<int> visited, List<int> scc)
    {
        visited.Add(node);
        scc.Add(node);
        foreach (var neighbor in graph[node])
        {
            if (!visited.Contains(neighbor))
            {
                DfsReversed(neighbor, graph, visited, scc);
            }
        }
    }

    // function to reverse the graph
    private static Dictionary<int, List<int>> ReverseGraph(Dictionary<int, List<int>> graph)
    {
        var reversed = new Dictionary<int, List<int>>();
        foreach (var node in graph.Keys)
        {
            reversed[node] = new List<int>();
        }
        foreach (var node in graph.Keys)
        {
            foreach (var neighbor in graph[node])
            {
                reversed[neighbor].Add(node); // reverse the edge direction
            }
        }
        return reversed;
    }

    public static List<List<int>> KosarajuAlgorithm(Dictionary<int, List<int>> graph)
    {
        var stack = new Stack<int>(); // stack to store finishing times
        var visited = new HashSet<int>();

        // step 1: perform dfs on the original graph and record finishing times
        foreach (var node in graph.Keys)
        {
            if (!visited.Contains(node))
            {
                DfsOriginal(node, graph, visited, stack);
            }
        }

        // step 2: reverse the graph
        var reversedGraph = ReverseGraph(graph);

        // step 3: perform dfs on the reversed graph
        visited.Clear(); // reset visited set for the reversed graph
        var sccs = new List<List<int>>();

        while (stack.Count > 0)
        {
            var node = stack.Pop();
            if (!visited.Contains(node))
            {
                var scc = new List<int>();
                DfsReversed(node, reversedGraph, visited, scc);
                sccs.Add(scc); // add the strongly connected component
            }
        }

        return sccs;
    }

    public static void Main(string[] args)
    {
        // example graph
        var graph = new Dictionary<int, List<int>>
        {
            { 0, new List<int> { 1 } },
            { 1, new List<int> { 2 } },
            { 2, new List<int> { 0, 3 } },
            { 3, new List<int> { 4 } },
            { 4, new List<int> { 5, 7 } },
            { 5, new List<int> { 6 } },
            { 6, new List<int> { 4 } },
            { 7, new List<int> { } }
        };

        var sccs = KosarajuAlgorithm(graph);

        Console.WriteLine("Strongly connected components:");
        foreach (var scc in sccs)
        {
            Console.WriteLine(string.Join(", ", scc));
        }

        // if the whole graph is strongly connected, sccs.Count == 1 and has every node
        if (sccs.Count == 1 && sccs[0].Count == graph.Count)
        {
            Console.WriteLine("The graph is strongly connected.");
        }
        else
        {
            Console.WriteLine("The graph is not strongly connected.");
        }
    }
}