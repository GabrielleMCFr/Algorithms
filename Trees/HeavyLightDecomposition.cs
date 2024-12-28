using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// This implementation provides Heavy-Light Decomposition (HLD) for trees.
// HLD decomposes a tree into heavy and light paths to allow efficient path queries and updates.
// It is combined with a segment tree to manage range queries and updates.
//
// When to use HLD:
// - HLD is particularly useful for large trees where efficient queries and updates on paths or subtrees are required.
// - It is used in problems involving range queries, path sums, or maximum/minimum values along paths in a tree.
// - It is commonly combined with a segment tree for advanced operations.
//
// Why use HLD:
// - It optimizes queries and updates on paths or subtrees by breaking the tree into smaller, manageable pieces (heavy and light chains).
// - It ensures logarithmic depth traversal for each query by limiting the number of chains.
// - It is efficient for large trees and frequent queries.
//
// What is a Heavy Chain?:
// When decomposing the tree:
// - For every node, the child with the largest subtree size is part of the heavy chain.
// - This ensures the heavy chain carries most of the tree's "weight."
// - Example: Starting from the root, we keep following the "heaviest" child downwards to form a chain.
//
// What is a Light Chain?:
// - Any child not selected as the heaviest child starts a light chain.
// - Light chains represent smaller subtrees branching off from the main heavy chain.
//
// Time Complexity:
// - Preprocessing (DFS + Decomposition): O(N)
// - Query and Update (with Segment Tree): O(log^2(N)) in the worst case
//   - O(log(N)) for traversing chains
//   - O(log(N)) for each segment tree query or update
public class HeavyLightDecomposition
{
    private List<int>[] Tree; // adjacency list for the tree
    private int[] Parent; // parent of each node
    private int[] Depth; // depth of each node
    private int[] SubtreeSize; // size of the subtree rooted at each node
    private int[] ChainHead; // head of the heavy chain for each node
    private int[] Position; // position of each node in the segment tree
    private int CurrentPosition; // keeps track of the position in the segment tree
    private SegmentTree STree; // segment tree to manage chain queries

    public HeavyLightDecomposition(int n)
    {
        Tree = new List<int>[n];
        Parent = new int[n];
        Depth = new int[n];
        SubtreeSize = new int[n];
        ChainHead = new int[n];
        Position = new int[n];
        CurrentPosition = 0;

        for (int i = 0; i < n; i++)
        {
            Tree[i] = new List<int>();
            ChainHead[i] = -1; // initially, no node is assigned a chain
        }

        STree = new SegmentTree(n); // initialize the segment tree
    }

    public void AddEdge(int u, int v)
    {
        Tree[u].Add(v);
        Tree[v].Add(u); // undirected tree
    }

    public void Preprocess(int root)
    {
        DFS(root, -1); // calculate subtree sizes and depths
        Decompose(root, -1, root); // decompose the tree into heavy and light chains
    }

    private void DFS(int node, int parent)
    {
        Parent[node] = parent;
        SubtreeSize[node] = 1;
        Depth[node] = parent == -1 ? 0 : Depth[parent] + 1;

        foreach (int child in Tree[node])
        {
            if (child == parent) continue; // skip the parent
            DFS(child, node);
            SubtreeSize[node] += SubtreeSize[child];
        }
    }

    private void Decompose(int node, int parent, int chainHead)
    {
        ChainHead[node] = chainHead;
        Position[node] = CurrentPosition++;

        // set an initial value in the segment tree (can be adjusted as needed)
        STree.Update(Position[node], node); 

        // find the heavy child (the child with the largest subtree size)
        int heavyChild = -1;
        foreach (int child in Tree[node])
        {
            if (child == parent) continue;
            if (heavyChild == -1 || SubtreeSize[child] > SubtreeSize[heavyChild])
            {
                heavyChild = child;
            }
        }

        // decompose the heavy child in the same chain
        if (heavyChild != -1)
        {
            Decompose(heavyChild, node, chainHead);
        }

        // decompose the light children into new chains
        foreach (int child in Tree[node])
        {
            if (child == parent || child == heavyChild) continue;
            Decompose(child, node, child);
        }
    }

    public int QueryPath(int u, int v, Func<int, int, int> queryFunction)
    {
        int result = 0; // replace with appropriate neutral value, e.g., 0 for sum queries

        while (ChainHead[u] != ChainHead[v])
        {
            // ensure u is deeper than v by swapping them if needed
            if (Depth[ChainHead[u]] < Depth[ChainHead[v]])
            {
                int temp = u;
                u = v;
                v = temp;
            }

            // suery the chain from u to its chain head
            result = queryFunction(result, STree.Query(Position[ChainHead[u]], Position[u]));
            u = Parent[ChainHead[u]]; // move to the parent of the chain head
        }

        // final query within the same chain
        if (Depth[u] > Depth[v])
        {
            int temp = u;
            u = v;
            v = temp;
        }

        result = queryFunction(result, STree.Query(Position[u], Position[v]));
        return result;
    }

    private class SegmentTree
    {
        private int[] Tree;
        private int Size;

        public SegmentTree(int size)
        {
            Size = size;
            Tree = new int[4 * size];
        }

        public void Update(int index, int value, int node = 1, int start = 0, int end = -1)
        {
            if (end == -1) end = Size - 1;

            if (start == end)
            {
                Tree[node] = value;
            }
            else
            {
                int mid = (start + end) / 2;
                int leftChild = 2 * node;
                int rightChild = 2 * node + 1;

                if (index <= mid)
                {
                    Update(index, value, leftChild, start, mid);
                }
                else
                {
                    Update(index, value, rightChild, mid + 1, end);
                }

                Tree[node] = Tree[leftChild] + Tree[rightChild]; 
            }
        }

        public int Query(int left, int right, int node = 1, int start = 0, int end = -1)
        {
            if (end == -1) end = Size - 1;

            if (right < start || left > end)
            {
                return 0; // neutral value for sum queries
            }

            if (left <= start && end <= right)
            {
                return Tree[node];
            }

            int mid = (start + end) / 2;
            int leftChild = 2 * node;
            int rightChild = 2 * node + 1;

            int leftSum = Query(left, right, leftChild, start, mid);
            int rightSum = Query(left, right, rightChild, mid + 1, end);

            return leftSum + rightSum;
        }
    }

    public static void Main(string[] args)
    {
        int n = 9; // number of nodes
        HeavyLightDecomposition hld = new HeavyLightDecomposition(n);

        // create a sample tree
        hld.AddEdge(0, 1);
        hld.AddEdge(0, 2);
        hld.AddEdge(1, 3);
        hld.AddEdge(1, 4);
        hld.AddEdge(2, 5);
        hld.AddEdge(2, 6);
        hld.AddEdge(6, 7);
        hld.AddEdge(6, 8);

        hld.Preprocess(0); // preprocess the tree with node 0 as the root

        // example query
        int result = hld.QueryPath(3, 7, (a, b) => a + b); // example: sum query
        Console.WriteLine("Query result: " + result);
    }
}