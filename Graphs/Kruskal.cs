using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    public class Edge : IComparable<Edge>
    {
        public int Source { get; set; }
        public int Destination { get; set; }
        public int Weight { get; set; }

        public int CompareTo(Edge other)
        {
            return this.Weight.CompareTo(other.Weight);
        }
    }

    public class Graph
    {
        private int Vertices;
        private List<Edge> Edges;

        public Graph(int vertices)
        {
            this.Vertices = vertices;
            this.Edges = new List<Edge>();
        }

        public void AddEdge(int source, int destination, int weight)
        {
            Edges.Add(new Edge { Source = source, Destination = destination, Weight = weight });
        }

        // Kruskal is used to find a Minimum Spanning Tree (MST) in a weighted graph.
        // A MST is a subgraph who :
        // - covers all vertices: Every vertex of the graph is included.
        // - has no cycles: There are no closed loops in the tree.
        // - has the minimum edges: For a graph with V vertices, the spanning tree will have V−1 edges.
        // O(ElogE)
        public void KruskalMST()
        {
            // sort edges by weight
            Edges.Sort();

            int[] parent = new int[Vertices];
            for (int i = 0; i < Vertices; i++)
                parent[i] = i;

            List<Edge> mst = new List<Edge>();
            foreach (var edge in Edges)
            {
                int rootSource = Find(parent, edge.Source);
                int rootDestination = Find(parent, edge.Destination);

                // meaning, there is no cycles.
                if (rootSource != rootDestination)
                {
                    mst.Add(edge);
                    Union(parent, rootSource, rootDestination);
                }
            }

            Console.WriteLine("Minimum Spanning Tree:");
            foreach (var edge in mst)
            {
                Console.WriteLine($"Edge: {edge.Source} - {edge.Destination}, Weight: {edge.Weight}");
            }
        }

        private int Find(int[] parent, int vertex)
        {
            if (parent[vertex] != vertex)
                parent[vertex] = Find(parent, parent[vertex]);
            return parent[vertex];
        }

        private void Union(int[] parent, int sourceRoot, int destinationRoot)
        {
            parent[sourceRoot] = destinationRoot;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph(6);

            graph.AddEdge(0, 1, 4);
            graph.AddEdge(0, 2, 4);
            graph.AddEdge(1, 2, 2);
            graph.AddEdge(1, 3, 6);
            graph.AddEdge(2, 3, 8);
            graph.AddEdge(3, 4, 9);
            graph.AddEdge(4, 5, 10);
            graph.AddEdge(3, 5, 7);

            graph.KruskalMST();
        }
    }
}