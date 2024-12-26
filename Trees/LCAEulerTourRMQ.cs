using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// This code implements the Lowest Common Ancestor (LCA) algorithm using Euler's Tour and a Segment Tree.
// Euler's Tour linearizes the tree, and the Segment Tree efficiently computes the minimum depth between nodes.
// Euler's Tour is a technique used to linearize a tree into an array by visiting each node multiple times during a Depth First Search (DFS)
public class LCAEulerTourRMQ
{
    private List<int> EulerTour; // stores the nodes in the order of Euler's tour
    private List<int> FirstOccurrence; // stores the first occurrence of each node in Euler's tour
    private List<int> Depth; // stores the depth of nodes during the Euler's tour
    private int[] SegmentTree; // segment tree for RMQ (Range Minimum Query)
    private List<int>[] Tree; // adjacency list representation of the tree
    private int[] NodeToEulerIndex; // maps node to Euler index

    public LCAEulerTourRMQ(int n)
    {
        EulerTour = new List<int>();
        Depth = new List<int>();
        FirstOccurrence = new List<int>(new int[n]);
        NodeToEulerIndex = new int[n];
        Tree = new List<int>[n];
        SegmentTree = new int[4 * (2 * n - 1)]; // allocate enough space for segment tree

        for (int i = 0; i < n; i++)
        {
            Tree[i] = new List<int>();
            FirstOccurrence[i] = -1; // initialize first occurrence with -1
        }
    }

    public void AddEdge(int u, int v)
    {
        Tree[u].Add(v);
        Tree[v].Add(u); // add both directions since the tree is undirected
    }

    public void Preprocess(int root)
    {
        DFS(root, -1, 0); // perform Euler's Tour starting from the root
        BuildSegmentTree(0, 0, EulerTour.Count - 1); // build the segment tree based on Euler tour depth
    }

    private void DFS(int node, int parent, int depth)
    {
        if (FirstOccurrence[node] == -1)
        {
            FirstOccurrence[node] = EulerTour.Count; // store the first occurrence of the node
        }

        EulerTour.Add(node); // add node to Euler's tour
        Depth.Add(depth); // add depth of the node

        foreach (int child in Tree[node])
        {
            if (child != parent)
            {
                DFS(child, node, depth + 1);
                EulerTour.Add(node); // backtrack to the parent node
                Depth.Add(depth);
            }
        }
    }

    private void BuildSegmentTree(int index, int left, int right)
    {
        if (left == right)
        {
            SegmentTree[index] = left;
        }
        else
        {
            int mid = (left + right) / 2;
            BuildSegmentTree(2 * index + 1, left, mid);
            BuildSegmentTree(2 * index + 2, mid + 1, right);

            int leftIndex = SegmentTree[2 * index + 1];
            int rightIndex = SegmentTree[2 * index + 2];
            SegmentTree[index] = Depth[leftIndex] < Depth[rightIndex] ? leftIndex : rightIndex;
        }
    }

    // RMQ is a type of query that finds the minimum value (or its index) in a specific range of an array. 
    // In the context of Euler's Tour:
    // Goal: Given two nodes u and v, find the node in the Euler tour with the smallest depth between the first occurrences of u and v.
    // Why RMQ for LCA:
    // - the LCA of two nodes corresponds to the node with the smallest depth in their range within the Euler tour.
    // - using RMQ on the Depth array helps identify this node efficiently.
    private int RMQ(int index, int segLeft, int segRight, int queryLeft, int queryRight)
    {
        if (queryLeft > segRight || queryRight < segLeft)
        {
            return -1; // out of range
        }

        if (queryLeft <= segLeft && queryRight >= segRight)
        {
            return SegmentTree[index]; // completely in range
        }

        int mid = (segLeft + segRight) / 2;
        int leftIndex = RMQ(2 * index + 1, segLeft, mid, queryLeft, queryRight);
        int rightIndex = RMQ(2 * index + 2, mid + 1, segRight, queryLeft, queryRight);

        if (leftIndex == -1) return rightIndex;
        if (rightIndex == -1) return leftIndex;

        return Depth[leftIndex] < Depth[rightIndex] ? leftIndex : rightIndex;
    }

    public int Query(int u, int v)
    {
        int left = FirstOccurrence[u];
        int right = FirstOccurrence[v];

        if (left > right)
        {
            int temp = left;
            left = right;
            right = temp;
        }

        int eulerIndex = RMQ(0, 0, EulerTour.Count - 1, left, right);
        return EulerTour[eulerIndex];
    }

    public static void Main(string[] args)
    {
        int n = 7; // number of nodes
        LCABasedOnEulerTour lca = new LCABasedOnEulerTour(n);

        // create a sample tree
        lca.AddEdge(0, 1);
        lca.AddEdge(0, 2);
        lca.AddEdge(1, 3);
        lca.AddEdge(1, 4);
        lca.AddEdge(2, 5);
        lca.AddEdge(2, 6);

        lca.Preprocess(0); // preprocess the tree with 0 as the root

        // perform LCA queries
        Console.WriteLine(lca.Query(3, 4)); // output: 1
        Console.WriteLine(lca.Query(3, 6)); // output: 0
        Console.WriteLine(lca.Query(5, 6)); // output: 2
    }
}
