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
            // it guarantees that the edges with the less weight are considered first.
            Edges.Sort();

            // used to handle connected composants in union find.
            // initially, every node is it's own parent. (disconnected)
            int[] parent = new int[Vertices];
            int[] rank = new int[Vertices];

            for (int i = 0; i < Vertices; i++) {
                parent[i] = i;
                rank[i] = 0;
            }
                

            List<Edge> mst = new List<Edge>();
            foreach (var edge in Edges)
            {
                // for each edge, we check if the nodes considered in source and destination of the edge are in the same 'group'
                // meaning, if they are connected, they will have the same root.
                // we do that because if the source root and destination root are the same, it means it will form a cycle.
                int rootSource = Find(parent, edge.Source);
                int rootDestination = Find(parent, edge.Destination);

                // if they are different, there is no cycle.
                // so we fuse the two groups.
                // if they are the same, we ignore that edge, or it would form a cycle.
                if (rootSource != rootDestination)
                {
                    mst.Add(edge);
                    Union(parent, rank, rootSource, rootDestination);
                }
            }

            Console.WriteLine("Minimum Spanning Tree:");
            foreach (var edge in mst)
            {
                Console.WriteLine($"Edge: {edge.Source} - {edge.Destination}, Weight: {edge.Weight}");
            }
        }

        // we find the root. this uses path compression to be quicker.
        // How it works : When we call Find to determine the root of a node:
        // - Traverse up the tree to find the root.
        // - On the way back, update each node to point directly to the root.
        // This flattens the tree structure, making future Find operations faster.
        private int Find(int[] parent, int vertex)
        {
            if (parent[vertex] != vertex)
                parent[vertex] = Find(parent, parent[vertex]);
            return parent[vertex];
        }

        // simple union for simplicity.
        // could result in unbalanced trees, so better use union by rank.
        // private void Union(int[] parent, int sourceRoot, int destinationRoot)
        // {
        //     parent[sourceRoot] = destinationRoot;
        // }

        // union with union by rank
        // ensure the tree stay shallow
        // the idea is to branch the shallower tree to the root of the deepest tree, to "spead" horizontally, and not vertically
        public void Union(int[] parent, int[] rank, int x, int y)
        {
            int rootX = Find(parent, x);
            int rootY = Find(parent, y);

            if (rootX != rootY)
            {
                if (rank[rootX] > rank[rootY])
                {
                    parent[rootY] = rootX; // attach the smaller tree to the larger tree
                }
                else if (rank[rootX] < rank[rootY])
                {
                    parent[rootX] = rootY;
                }
                else
                {
                    parent[rootY] = rootX; // if ranks are equal, we choose one and increment its rank
                    rank[rootX]++;
                }
            }
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