
// bellman ford :
// basically what we use when we have negative weights and don't want an heuristic.
// O(n*e)
// moins efficace que djisktra mais gere les poids negatifs


using System;

namespace Code.algorithms
{
    public class BellmanFord
    {
        // Bellman-Ford algorithm
        // O(V * E)
        // Use when the graph has negative weights and no heuristic is required
        public static void FindShortestPaths(Graph graph, int source)
        {
            int numVertices = graph.Vertices;
            int numEdges = graph.Edges;
            int[] distances = new int[numVertices];

            // step 1: initialize distances from src to all other vertices as INFINITY
            for (int i = 0; i < numVertices; i++)
            {
                distances[i] = int.MaxValue;
            }
            distances[source] = 0; // distance from source to itself is always 0

            // Step 2: Relax all edges |V| - 1 times
            for (int i = 1; i <= numVertices - 1; i++)
            {
                foreach (var edge in graph.EdgesList)
                {
                    int u = edge.Source;
                    int v = edge.Destination;
                    int weight = edge.Weight;

                    if (distances[u] != int.MaxValue && distances[u] + weight < distances[v])
                    {
                        distances[v] = distances[u] + weight;
                    }
                }
            }

            // step 3: check for negative-weight cycles
            // why is it a problem : A negative-weight cycle is a cycle where the total weight is negative.
            // In such a case, the distance keeps decreasing indefinitely
            foreach (var edge in graph.EdgesList)
            {
                int u = edge.Source;
                int v = edge.Destination;
                int weight = edge.Weight;

                if (distances[u] != int.MaxValue && distances[u] + weight < distances[v])
                {
                    Console.WriteLine("Graph contains a negative-weight cycle.");
                    return;
                }
            }

            // print the shortest distances
            Console.WriteLine($"Shortest distances from source vertex {source}:");
            for (int i = 0; i < numVertices; i++)
            {
                Console.WriteLine($"Vertex {i} -> Distance: {(distances[i] == int.MaxValue ? "Infinity" : distances[i].ToString())}");
            }
        }
    }

    public class Edge
    {
        public int Source { get; }
        public int Destination { get; }
        public int Weight { get; }

        public Edge(int source, int destination, int weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }
    }

    public class Graph
    {
        public int Vertices { get; }
        public int Edges => EdgesList.Count;
        public List<Edge> EdgesList { get; }

        public Graph(int vertices)
        {
            Vertices = vertices;
            EdgesList = new List<Edge>();
        }

        public void AddEdge(int source, int destination, int weight)
        {
            EdgesList.Add(new Edge(source, destination, weight));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int numVertices = 5;

            Graph graph = new Graph(numVertices);

            graph.AddEdge(0, 1, -1);
            graph.AddEdge(0, 2, 4);
            graph.AddEdge(1, 2, 3);
            graph.AddEdge(1, 3, 2);
            graph.AddEdge(1, 4, 2);
            graph.AddEdge(3, 2, 5);
            graph.AddEdge(3, 1, 1);
            graph.AddEdge(4, 3, -3);

            BellmanFord.FindShortestPaths(graph, 0);
        }
    }
}
