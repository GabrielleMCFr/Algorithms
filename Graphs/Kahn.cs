using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// Use Cases of Kahn’s Algorithm
// Task Scheduling:
// Determine the order in which tasks with dependencies must be completed.
// Build Systems:
// Order files or modules in a build process based on dependencies.
// Compiler Design:
// Resolve symbol dependencies in programming languages.
// Cycle Detection:
// If the graph contains a cycle, Kahn's Algorithm will detect it because:
//  - Vertices in the cycle will never have their in-degree reduced to 0.
//  - The algorithm will terminate with fewer vertices in the topological order than the total number of vertices in the graph.
// NOTE : ONLY WORKS IN DIRECTED ACYCLIC GRAPHS
// Time Complexity : O(V+E)
public class Kahn
{
    public static List<int> KahnsTopologicalSort(Dictionary<int, List<int>> graph, int vertices)
    {
        // step 1: calculate in-degrees
        // note: the indegree of a vertex is the number of incoming edges to that vertex
        int[] inDegree = new int[vertices];
        foreach (var u in graph.Keys)
        {
            foreach (var v in graph[u])
            {
                inDegree[v]++;
            }
        }

        // step 2: initialize a queue with all vertices having in-degree 0
        Queue<int> queue = new Queue<int>();
        for (int i = 0; i < vertices; i++)
        {
            if (inDegree[i] == 0)
            {
                queue.Enqueue(i);
            }
        }

        // step 3: process the queue
        List<int> topologicalOrder = new List<int>();
        while (queue.Count > 0)
        {
            int vertex = queue.Dequeue();
            topologicalOrder.Add(vertex);

            foreach (var neighbor in graph[vertex])
            {
                inDegree[neighbor]--;
                if (inDegree[neighbor] == 0)
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        // step 4: check for cycles
        if (topologicalOrder.Count == vertices)
        {
            return topologicalOrder; // valid topological sort
        }
        else
        {
            throw new InvalidOperationException("The graph contains a cycle and cannot be topologically sorted.");
        }
    }

    public static void Main(string[] args)
    {
        var graph = new Dictionary<int, List<int>>
        {
            { 0, new List<int> { 1, 2 } },
            { 1, new List<int> { 3 } },
            { 2, new List<int> { 3 } },
            { 3, new List<int> { 4 } },
            { 4, new List<int> { } }
        };

        int vertices = 5;

        try
        {
            var result = KahnsTopologicalSort(graph, vertices);
            Console.WriteLine("Topological Sort:");
            Console.WriteLine(string.Join(", ", result));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}