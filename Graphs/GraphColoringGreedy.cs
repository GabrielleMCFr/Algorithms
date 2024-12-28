using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// Graph coloring is a technique to assign colors to the vertices or edges of a graph subject to specific constraints.
// It's widely used in scheduling, map coloring, register allocation, and more.
//
// Note : The greedy coloring algorithm does not guarantee the minimum number of colors (chromatic number).
// The output may vary depending on the vertex order or the structure of the graph.
// O(V+E)
class GraphColoringGreedy
{
    public static int[] GreedyColoring(Dictionary<int, List<int>> graph)
    {
        int vertices = graph.Count;

        // for the example, the greedy algorithm may use up to as many colors as the vertices,
        // but if we want to restrict the assignments to, for example, 3 timeslots, we would limit the number of colors to 3.
        int[] colors = new int[vertices];
        Array.Fill(colors, -1); // initialize all vertices with no color

        colors[0] = 0; // assign the first color to the first vertex

        // iterate over each vertex
        for (int u = 1; u < vertices; u++)
        {
            // create a boolean array to check available colors
            bool[] available = new bool[vertices];
            Array.Fill(available, true);

            // mark colors of adjacent vertices as unavailable
            foreach (var neighbor in graph[u])
            {
                if (colors[neighbor] != -1)
                {
                    available[colors[neighbor]] = false;
                }
            }

            // find the first available color
            for (int c = 0; c < vertices; c++)
            {
                if (available[c])
                {
                    colors[u] = c;
                    break;
                }
            }
        }

        return colors;
    }

    public static void Main(string[] args)
    {
        // The adjcency list represent the constraints : two connected vertices cannot have the same color
        // for ex, 0 cannot have the same color than 1, 2 or 3.
        var graph = new Dictionary<int, List<int>>
        {
            { 0, new List<int> { 1, 2, 3 } },
            { 1, new List<int> { 0, 2, 4 } },
            { 2, new List<int> { 0, 1, 4 } },
            { 3, new List<int> { 0, 4 } },
            { 4, new List<int> { 1, 2, 3 } }
        };

        int[] colors = GreedyColoring(graph);

        Console.WriteLine("Vertex Colors:");
        for (int i = 0; i < colors.Length; i++)
        {
            Console.WriteLine($"Vertex {i}: Color {colors[i]}");
        }
    }
}