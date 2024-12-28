using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// Union-Find is often used for undirected graphs to:
// - Detect connected components.
// - Check for cycles.
// - Solve problems like building a Minimum Spanning Tree (MST).
//
// Find: Check which component a node belongs to.
// Union: Merge two components when an edge connects them.
// A component being a set of connected nodes in this context.
// 
// Time complexity :
// - Find with Path Compression :  O(a(n)) where a(n) is the inverse Ackermann function.
// - Union with rank : O(1) 
// - Total : O(E*a(n)) for E union-find ops in a graph with V vertices

class UnionFind
{
    private int[] parent, rank;

    // initialize the union-find data structure
    public UnionFind(int size)
    {
        parent = new int[size]; // each node is its own parent initially
        rank = new int[size];   // rank is initialized to 0
        for (int i = 0; i < size; i++)
        {
            parent[i] = i; // each node is its own set
            rank[i] = 0;   // initial rank is 0
        }
    }

    // find the root of the set containing x with path compression
    // Normally, each node in a set points to its parent in the tree representing the set.
    // In path compression, during the Find operation, we make each node on the path point directly to the root of the set.
    // This flattens the tree, reducing the height and making future operations faster.
    public int Find(int x)
    {
        if (parent[x] != x)
        {
            parent[x] = Find(parent[x]); // recursively find the root and compress the path
        }
        return parent[x];
    }

    // union two sets by rank
    // Union by rank is an optimization used in the Union operation to ensure the trees representing sets remain shallow.
    // How it works:
    // Each set has a rank (an estimate of its "height").
    // When performing a union, we attach the tree with the smaller rank to the root of the tree with the larger rank.
    // If the ranks are equal, we pick one arbitrarily as the new root and increase its rank by 1.
    public void Union(int x, int y)
    {
        int rootX = Find(x);
        int rootY = Find(y);

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

    // check if two nodes are in the same set
    public bool IsConnected(int x, int y)
    {
        return Find(x) == Find(y);
    }
}

class UnionFindExamples
{
    // detect connected components in an undirected graph
    public static List<List<int>> ConnectedComponents(int vertices, List<(int, int)> edges)
    {
        var uf = new UnionFind(vertices);

        // union operation for each edge
        foreach (var (u, v) in edges)
        {
            uf.Union(u, v);
        }

        // group nodes by their root
        var components = new Dictionary<int, List<int>>();
        for (int i = 0; i < vertices; i++)
        {
            int root = uf.Find(i);
            if (!components.ContainsKey(root))
            {
                components[root] = new List<int>();
            }
            components[root].Add(i);
        }

        return new List<List<int>>(components.Values);
    }

    // check if a graph contains a cycle
    public static bool ContainsCycle(int vertices, List<(int, int)> edges)
    {
        var uf = new UnionFind(vertices);

        foreach (var (u, v) in edges)
        {
            if (uf.IsConnected(u, v))
            {
                return true; // cycle detected if u and v are already connected
            }
            uf.Union(u, v); // otherwise, union them
        }

        return false; // no cycle found
    }

    // build a minimum spanning tree using kruskal's algorithm
    public static List<(int, int, int)> KruskalsMST(int vertices, List<(int, int, int)> edges)
    {
        var uf = new UnionFind(vertices);
        var mst = new List<(int, int, int)>();

        // sort edges by weight
        edges.Sort((a, b) => a.Item3.CompareTo(b.Item3));

        foreach (var (u, v, weight) in edges)
        {
            if (!uf.IsConnected(u, v))
            {
                mst.Add((u, v, weight)); // add the edge to the mst
                uf.Union(u, v); // union the two components
            }
        }

        return mst;
    }

    public static void Main(string[] args)
    {
        var edgesForCC = new List<(int, int)>
        {
            (0, 1), (1, 2), (3, 4)
        };
        Console.WriteLine("Connected components:");
        var components = ConnectedComponents(5, edgesForCC);
        foreach (var component in components)
        {
            Console.WriteLine(string.Join(", ", component));
        }

        var edgesForCycle = new List<(int, int)>
        {
            (0, 1), (1, 2), (2, 0)
        };
        Console.WriteLine("Graph Contains Cycle: " + ContainsCycle(3, edgesForCycle));

        var edgesForMST = new List<(int, int, int)>
        {
            (0, 1, 10), (0, 2, 6), (0, 3, 5), (1, 3, 15), (2, 3, 4)
        };
        Console.WriteLine("Minimum spanning tree:");
        var mst = KruskalsMST(4, edgesForMST);
        foreach (var (u, v, weight) in mst)
        {
            Console.WriteLine($"{u} -- {v}, Weight: {weight}");
        }
    }
}
