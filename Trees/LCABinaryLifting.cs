using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// This code implements the Lowest Common Ancestor (LCA) algorithm using binary lifting.
// Binary lifting precomputes ancestor relationships for efficient LCA queries in O(log N) time.
public class BinaryLiftingLCA
{
    private int[,] Ancestors; // binary lifting table
    private int[] Depth; // depth of each node
    private int Log; // maximum power of 2 for the given tree
    private List<int>[] Tree; // adjacency list representation of the tree

    public BinaryLiftingLCA(int n)
    {
        Log = (int)Math.Ceiling(Math.Log2(n)) + 1; // calculate the maximum depth for binary lifting
        Ancestors = new int[n, Log + 1];
        Depth = new int[n];
        Tree = new List<int>[n];

        for (int i = 0; i < n; i++)
        {
            Tree[i] = new List<int>();
            for (int j = 0; j <= Log; j++)
            {
                Ancestors[i, j] = -1; // initialize ancestor table with -1
            }
        }
    }

    public void AddEdge(int u, int v)
    {
        Tree[u].Add(v);
        Tree[v].Add(u); // add both directions since the tree is undirected
    }

    public void Preprocess(int root)
    {
        DFS(root, -1); // start DFS from the root
    }

    private void DFS(int node, int parent)
    {
        Ancestors[node, 0] = parent; // set the immediate parent as the first ancestor
        for (int j = 1; j <= Log; j++)
        {
            if (Ancestors[node, j - 1] != -1)
            {
                Ancestors[node, j] = Ancestors[Ancestors[node, j - 1], j - 1];
            }
        }

        foreach (int child in Tree[node])
        {
            if (child != parent)
            {
                Depth[child] = Depth[node] + 1; // calculate depth of the child
                DFS(child, node); // recursively process the child
            }
        }
    }

    public int GetBinaryLiftingLCA(int u, int v)
    {
        // if v is deeper, we swap them so u is always the deepest node.
        if (Depth[u] < Depth[v])
        {
            int temp = u;
            u = v;
            v = temp;
        }

        // Bring u and v to the same depth
        int diff = Depth[u] - Depth[v];
        for (int j = 0; j <= Log; j++)
        {
            if ((diff & (1 << j)) > 0)
            {
                u = Ancestors[u, j];
            }
        }

        if (u == v) return u; // if they meet, return one of them as the LCA

        // lift both u and v until their ancestors converge
        // here, we check from the highest power of 2 (the root), where it has to converge and go down in powers of two jumps, until it doesn't converge anymore.
        // when it doesn't converge anymore, that means the parent is the LCA.
        for (int j = Log; j >= 0; j--)
        {
            if (Ancestors[u, j] != Ancestors[v, j])
            {
                u = Ancestors[u, j];
                v = Ancestors[v, j];
            }
        }

        return Ancestors[u, 0]; // the parent of u (or v) is the LCA
    }

    // for reference, technically we don't need any preprocessing for brute force, but it illustrates the difference.
    // we just go one node up each time instead of jumping by powers of 2.
    public int BruteForceLCA(int u, int v)
    {
        // if v is deeper, we swap them so u is always the deepest node.
        if (Depth[u] < Depth[v])
        {
            int temp = u;
            u = v;
            v = temp;
        }

        // Bring u and v to the same depth
        while (Depth[u] > Depth[v]) {
            // we put u as its parent. Meaning depth - 1.
            u = Ancestors[u, 0];
        }

        if (u == v) return u; // if they meet, return one of them as the LCA

        // Lift both u and v until their ancestors converge
        while (u != v) {
            u = Ancestors[u, 0]; 
            v = Ancestors[v, 0];
        }

        return u; // the parent of u (or v) is the LCA
    }

    public static void Main(string[] args)
    {
        int n = 7; // number of nodes
        BinaryLiftingLCA lca = new BinaryLiftingLCA(n);

        // create a sample tree
        lca.AddEdge(0, 1);
        lca.AddEdge(0, 2);
        lca.AddEdge(1, 3);
        lca.AddEdge(1, 4);
        lca.AddEdge(2, 5);
        lca.AddEdge(2, 6);

        lca.Preprocess(0); // preprocess the tree with 0 as the root

        // perform LCA queries
        Console.WriteLine(lca.GetBinaryLiftingLCA(3, 4)); // output: 1
        Console.WriteLine(lca.GetBinaryLiftingLCA(3, 6)); // output: 0
        Console.WriteLine(lca.GetBinaryLiftingLCA(5, 6)); // output: 2
    }
}
