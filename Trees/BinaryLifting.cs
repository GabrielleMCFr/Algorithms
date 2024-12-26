using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms.Trees
{
    // get kth anscestor using binary lifting.
    // a method for quickly finding the k-th ancestor of a node or the lowest common ancestor (LCA) in O(logn)
    // use case: Questions about relationships between nodes in a tree, such as "What's the 3rd parent of node X?" or "What's the LCA of nodes X and Y?"
    // key idea: Precompute ancestors at powers of 2 (1st, 2nd, 4th, etc.). instead of traversing the tree 
    // k times, binary lifting allows us to "jump" directly to the ancestors at powers of 2.
    // ex, to find 6th ancestor, we can jump to 2^2 (4th ancestor) then to 2^1 (4)
    class Tree
    {
        private int n; // number of nodes
        private int[,] up; // up[node][i] is the 2^i ancestor of node
        private int[] depth; // depth of each node
        private List<int>[] adj; // adjacency list
        private int log; // max power of 2

        // construct tree
        public Tree(int n)
        {
            this.n = n;
            adj = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                adj[i] = new List<int>();
            }

            log = (int)Math.Ceiling(Math.Log2(n)) + 1; // +1 to handle edge cases
            up = new int[n, log];
            depth = new int[n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < log; j++)
                {
                    up[i, j] = -1; // initialize to -1 (no ancestor)
                }
            }
        }

        public void AddEdge(int u, int v)
        {
            adj[u].Add(v);
            adj[v].Add(u);
        }

        public void Preprocess(int root)
        {
            DFS(root, -1);
        }

        // Here, we use DFS to traverse the tree and precompute all the ancestors
        // at different powers of 2 (2^0, 2^1, 2^2, ...) for each node.
        // This fills the `up` 2D array, which is used for binary lifting.
        private void DFS(int node, int parent)
        {
            // set parent
            up[node, 0] = parent;

            // precompute ancestors for powers of 2 (2^1, 2^2, ...) for the current node
            for (int i = 1; i < log; i++)
            {
                if (up[node, i - 1] != -1)
                {
                    up[node, i] = up[up[node, i - 1], i - 1];
                }
            }

            // traverse all children of the current node
            foreach (int neighbor in adj[node])
            {
                if (neighbor != parent) // Avoid backtracking to the parent node
                {
                    // adjust child depth
                    depth[neighbor] = depth[node] + 1;

                    // recursively process children
                    DFS(neighbor, node);
                }
            }
        }


        // Or we can also precompute in the same loop
        // public void DFS(int node, int parent)
        // {
        //     up[node, 0] = parent; // set direct parent
        //     // for each child of the current node
        //     foreach (int child in adj[node])
        //     {
        //         if (child != parent) // avoid backtracking to the parent
        //         {
        //             depth[child] = depth[node] + 1; // update depth of the child
        //             up[child, 0] = node; // set the direct parent of the child
        //             // precompute all 2^i ancestors for the child
        //             for (int j = 1; j < log; j++)
        //             {
        //                 if (up[child, j - 1] != -1)
        //                 {
        //                     up[child, j] = up[up[child, j - 1], j - 1];
        //                 }
        //             }
        //             DFS(child, node); // recursively process the child's subtree
        //         }
        //     }
        // }

        // using binary lifting.
        public int GetKthAncestor(int node, int k)
        {
            for (int i = 0; k > 0; i++)
            {
                if (k % 2 == 1) // if the current power of 2 is part of k (same than (k & 1) == 1)
                {
                    node = up[node, i]; // jump to the 2^i-th ancestor
                    if (node == -1) break; // if the ancestor doesn't exist, stop
                }
                k = k / 2; // divide k by 2 (right shift, same than k >>= 1)
            }
            return node;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            // number of nodes in the tree
            int n = 9;

            Tree tree = new Tree(n);

            tree.AddEdge(0, 1);
            tree.AddEdge(0, 2);
            tree.AddEdge(1, 3);
            tree.AddEdge(1, 4);
            tree.AddEdge(2, 5);
            tree.AddEdge(2, 6);
            tree.AddEdge(4, 7);
            tree.AddEdge(4, 8);

            // set root and preprocess for binary lifting
            int root = 0;

            // to fill the up table, used in binary lifting
            tree.Preprocess(root);

            // test k-th ancestor queries
            Console.WriteLine("Testing k-th ancestor queries:");
            Console.WriteLine($"3rd ancestor of node 7: {tree.GetKthAncestor(7, 3)}"); // expected: 0
            Console.WriteLine($"2nd ancestor of node 8: {tree.GetKthAncestor(8, 2)}"); // expected: 1
            Console.WriteLine($"1st ancestor of node 5: {tree.GetKthAncestor(5, 1)}"); // expected: 2

            // edge case: k > depth of the node
            Console.WriteLine($"10th ancestor of node 6: {tree.GetKthAncestor(6, 10)}"); // expected: -1
        }
    }
}