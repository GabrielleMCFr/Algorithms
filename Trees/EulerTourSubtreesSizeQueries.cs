using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// This implementation uses Euler's Tour to answer subtree size queries.
// The approach uses start and end times of nodes during DFS to compute the size of subtrees.
public class EulerTourSubtreesSizeQueries
{
    private int[] StartTime, EndTime; // start and end times of each node during the tour
    private List<int>[] Tree; // adjacency list representation of the tree
    private int Timer; // timer to assign unique times during the tour

    public EulerTourSubtreesSizeQueries(int n)
    {
        StartTime = new int[n];
        EndTime = new int[n];
        Tree = new List<int>[n];
        for (int i = 0; i < n; i++)
        {
            Tree[i] = new List<int>();
        }
        Timer = 0;
    }

    public void AddEdge(int u, int v)
    {
        Tree[u].Add(v);
        Tree[v].Add(u); // add bidirectional edge since it is an undirected tree
    }

    public void Preprocess(int root)
    {
        DFS(root, -1); // start DFS from the root
    }

    private void DFS(int node, int parent)
    {
        StartTime[node] = Timer++; // record the start time when the node is first visited
        foreach (int child in Tree[node])
        {
            if (child != parent) // avoid revisiting the parent
            {
                DFS(child, node);
            }
        }
        EndTime[node] = Timer; // record the end time after finishing all descendants
    }

    public int SubtreeSize(int node)
    {
        // the size of the subtree is the range of times [StartTime[node], EndTime[node])
        return EndTime[node] - StartTime[node];
    }

    public static void Main(string[] args)
    {
        int n = 7; // number of nodes
        EulerTourWithoutRMQ tree = new EulerTourWithoutRMQ(n);

        // create a sample tree
        tree.AddEdge(0, 1);
        tree.AddEdge(0, 2);
        tree.AddEdge(1, 3);
        tree.AddEdge(1, 4);
        tree.AddEdge(2, 5);
        tree.AddEdge(2, 6);

        // perform the Euler's Tour with node 0 as the root
        tree.Preprocess(0);

        // query the size of the subtree of various nodes
        Console.WriteLine(tree.SubtreeSize(0)); //  7 (entire tree)
        Console.WriteLine(tree.SubtreeSize(1)); //  3 (nodes 1, 3, 4)
        Console.WriteLine(tree.SubtreeSize(2)); //  3 (nodes 2, 5, 6)
        Console.WriteLine(tree.SubtreeSize(3)); //  1 (leaf node)
    }
}