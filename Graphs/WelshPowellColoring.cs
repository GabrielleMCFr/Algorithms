using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// Key Idea:
// Sort vertices by degree (descending order):
//  - Start with the vertex that has the highest number of neighbors.
//  - This heuristic helps minimize the number of colors used.
// Assign colors greedily:
//  - For each vertex, assign the smallest available color that is not used by its neighbors.
//
// Why Use It?
// It produces a coloring with fewer colors than a naive greedy algorithm because it processes high-degree vertices first.
// Still efficient, with a time complexity of O((V^2) + E) for sorting and adjacency checks
public class WelshPowellColoring
{
    public static int[] WelshPowellColoring(Dictionary<int, List<int>> graph)
    {
        int vertices = graph.Count;
        int[] colors = new int[vertices];
        Array.Fill(colors, -1); // initialize all vertices with no color

        // step 1: sort vertices by degree (descending order)
        var sortedVertices = graph.Keys
            .OrderByDescending(v => graph[v].Count)
            .ToList();

        // step 2: assign colors to vertices
        foreach (var vertex in sortedVertices)
        {
            // create a boolean array to mark available colors
            bool[] available = new bool[vertices];
            Array.Fill(available, true);

            // mark colors used by neighbors as unavailable
            foreach (var neighbor in graph[vertex])
            {
                if (colors[neighbor] != -1)
                {
                    available[colors[neighbor]] = false;
                }
            }

            // assign the smallest available color to the vertex
            for (int c = 0; c < vertices; c++)
            {
                if (available[c])
                {
                    colors[vertex] = c;
                    break;
                }
            }
        }

        return colors;
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

        var colors = WelshPowellColoring(graph);

        Console.WriteLine("Vertex Colors:");
        for (int i = 0; i < colors.Length; i++)
        {
            Console.WriteLine($"Vertex {i}: Color {colors[i]}");
        }
    }
}