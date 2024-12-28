using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// In the k-coloring problem, we aim to assign up to k colors to a graph's vertices such that no two adjacent vertices 
// share the same color. 
// The backtracking algorithm systematically explores all possible color assignments to find a valid solution.
// Note: Unlike greedy coloring, it guarantees finding a valid solution if one exists for the given k, 
// but it does not always find the minimum number of colors required (chromatic number).
// Note : can find the chromatic number, but it would be expensive (need to run the algorithm from 1 to n times,
// with k+1 each time, first who has a solution is the result)
// Time complexity: O(k^V) in the worst case
class KColoringBacktracking
{
    // function to check if it is safe to assign a color to a vertex
    private static bool IsSafe(int vertex, int color, int[] colors, Dictionary<int, List<int>> graph)
    {
        foreach (var neighbor in graph[vertex])
        {
            if (colors[neighbor] == color)
            {
                return false; // neighbor has the same color
            }
        }
        return true;
    }

    // backtracking function to assign colors
    private static bool Solve(int vertex, int k, int[] colors, Dictionary<int, List<int>> graph)
    {
        if (vertex == colors.Length)
        {
            return true; // base case: all vertices have been successfully assigned a color
        }

        for (int c = 0; c < k; c++) // try all available colors from 0 to k-1
        {
            if (IsSafe(vertex, c, colors, graph)) // check if assigning color c to the current vertex is valid
            {
                colors[vertex] = c; // assign the color c to the current vertex

                if (Solve(vertex + 1, k, colors, graph)) // recursively attempt to color the next vertex
                {
                    return true; // if successful, propagate the success up the recursion stack
                }

                colors[vertex] = -1; // backtrack: undo the color assignment and try the next color
            }
        }

        return false; // if no valid color can be assigned, return false to indicate failure
    }

    public static bool KColoring(Dictionary<int, List<int>> graph, int k, out int[] colors)
    {
        int vertices = graph.Count;
        colors = new int[vertices];
        Array.Fill(colors, -1); // initialize all vertices as uncolored

        if (Solve(0, k, colors, graph))
        {
            return true; // successful coloring
        }

        return false; // no valid k-coloring found
    }

    public static void Main(string[] args)
    {
        var graph = new Dictionary<int, List<int>>
        {
            { 0, new List<int> { 1, 2, 3 } },
            { 1, new List<int> { 0, 2, 4 } },
            { 2, new List<int> { 0, 1, 4 } },
            { 3, new List<int> { 0, 4 } },
            { 4, new List<int> { 1, 2, 3 } }
        };

        int k = 3; // number of colors

        if (KColoring(graph, k, out int[] colors))
        {
            Console.WriteLine($"The graph can be colored with {k} colors:");
            for (int i = 0; i < colors.Length; i++)
            {
                Console.WriteLine($"Vertex {i}: Color {colors[i]}");
            }
        }
        else
        {
            Console.WriteLine($"The graph cannot be colored with {k} colors.");
        }
    }
}