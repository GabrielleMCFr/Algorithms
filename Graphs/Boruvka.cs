using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// Borůvka’s Algorithm is a greedy algorithm used to find a Minimum Spanning Tree (MST) of a graph. 
// It is particularly suited for distributed systems and can be implemented in parallel, 
// making it different from Prim's and Kruskal's algorithms
//
// Minimum Spanning Tree (MST):
// A subset of edges that connects all vertices in the graph with the minimum total weight.
// No cycles are allowed.
//
// Algorithm Overview:
// Borůvka’s Algorithm starts with each vertex as a separate component.
// In each iteration, the algorithm selects the lightest edge (minimum weight) that connects each component to a different component.
// These edges are added to the MST.
// Components are merged, and the process repeats until there is only one component.
//
// Time complexity : O(ElogV)
class Boruvka
{
    public class Edge
    {
        public int From, To, Weight;
        public Edge(int from, int to, int weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }
    }

    public class UnionFind
    {
        private int[] parent, rank;

        public UnionFind(int size)
        {
            parent = new int[size];
            rank = new int[size];
            for (int i = 0; i < size; i++)
            {
                parent[i] = i;
                rank[i] = 0;
            }
        }

        public int Find(int x)
        {
            if (parent[x] != x)
            {
                parent[x] = Find(parent[x]);
            }
            return parent[x];
        }

        public void Union(int x, int y)
        {
            int rootX = Find(x);
            int rootY = Find(y);

            if (rootX != rootY)
            {
                if (rank[rootX] < rank[rootY])
                {
                    parent[rootX] = rootY;
                }
                else if (rank[rootX] > rank[rootY])
                {
                    parent[rootY] = rootX;
                }
                else
                {
                    parent[rootY] = rootX;
                    rank[rootX]++;
                }
            }
        }
    }

    public static List<Edge> BoruvkasMST(int vertices, List<Edge> edges)
    {
        UnionFind uf = new UnionFind(vertices);
        List<Edge> mst = new List<Edge>();

        while (mst.Count < vertices - 1)
        {
            Edge[] cheapest = new Edge[vertices];

            // find the lightest edge for each component
            foreach (var edge in edges)
            {
                int component1 = uf.Find(edge.From);
                int component2 = uf.Find(edge.To);

                if (component1 != component2)
                {
                    if (cheapest[component1] == null || edge.Weight < cheapest[component1].Weight)
                    {
                        cheapest[component1] = edge;
                    }

                    if (cheapest[component2] == null || edge.Weight < cheapest[component2].Weight)
                    {
                        cheapest[component2] = edge;
                    }
                }
            }

            // add the cheapest edges to the MST
            for (int i = 0; i < vertices; i++)
            {
                if (cheapest[i] != null)
                {
                    Edge edge = cheapest[i];
                    int component1 = uf.Find(edge.From);
                    int component2 = uf.Find(edge.To);

                    if (component1 != component2)
                    {
                        mst.Add(edge);
                        uf.Union(component1, component2);
                    }
                }
            }
        }

        return mst;
    }

    public static void Main(string[] args)
    {
        List<Edge> edges = new List<Edge>
        {
            new Edge(0, 1, 10),
            new Edge(0, 2, 6),
            new Edge(0, 3, 5),
            new Edge(1, 3, 15),
            new Edge(2, 3, 4)
        };

        int vertices = 4;

        List<Edge> mst = BoruvkasMST(vertices, edges);

        Console.WriteLine("Edges in the Minimum Spanning Tree:");
        foreach (var edge in mst)
        {
            Console.WriteLine($"{edge.From} -- {edge.To}, Weight: {edge.Weight}");
        }
    }
}
