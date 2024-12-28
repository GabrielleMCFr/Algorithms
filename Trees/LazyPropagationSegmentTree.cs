using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// This implementation demonstrates a segment tree with lazy propagation for range updates and queries.
// Lazy propagation allows efficient range updates without recalculating all values immediately.
// The new value is stored in a lazy array at the node for a pending update,
// and the node's value is updated only when necessary (e.g., during a query or further propagation).
// When to Use Lazy Propagation:
// - When updates affect a range of elements, and you want to defer applying those updates to save computation time.
// - For large datasets, where propagating updates immediately to all affected elements is computationally expensive.
public class LazyPropagationTree
{
    private int[] Tree; // segment tree
    private int[] Lazy; // lazy array for pending updates
    private int Size;

    public LazyPropagationTree(int n)
    {
        Size = n;
        Tree = new int[4 * n]; // allocate enough space for the segment tree
        Lazy = new int[4 * n]; // lazy array to store pending updates
    }

    // build the tree based on the initial array values
    public void Build(int[] array, int node, int start, int end)
    {
        if (start == end)
        {
            Tree[node] = array[start];
        }
        else
        {
            int mid = (start + end) / 2;
            int leftChild = 2 * node + 1;
            int rightChild = 2 * node + 2;

            Build(array, leftChild, start, mid);
            Build(array, rightChild, mid + 1, end);

            Tree[node] = Tree[leftChild] + Tree[rightChild]; // sum operation
        }
    }

    // lazy propagation function to propagate updates
    private void Propagate(int node, int start, int end)
    {
        if (Lazy[node] != 0)
        {
            Tree[node] += Lazy[node] * (end - start + 1); // apply the pending update

            if (start != end) // if not a leaf node
            {
                int leftChild = 2 * node + 1;
                int rightChild = 2 * node + 2;

                Lazy[leftChild] += Lazy[node];
                Lazy[rightChild] += Lazy[node];
            }

            Lazy[node] = 0; // clear the lazy value for this node
        }
    }

    // range update: add a value to all elements in the range [l, r]
    public void UpdateRange(int node, int start, int end, int l, int r, int value)
    {
        Propagate(node, start, end);

        if (start > r || end < l) // completely out of range
        {
            return;
        }

        if (start >= l && end <= r) // completely within range
        {
            Tree[node] += value * (end - start + 1);

            if (start != end)
            {
                int leftChild = 2 * node + 1;
                int rightChild = 2 * node + 2;

                Lazy[leftChild] += value;
                Lazy[rightChild] += value;
            }
            return;
        }

        int mid = (start + end) / 2;
        int leftChildIndex = 2 * node + 1;
        int rightChildIndex = 2 * node + 2;

        UpdateRange(leftChildIndex, start, mid, l, r, value);
        UpdateRange(rightChildIndex, mid + 1, end, l, r, value);

        Tree[node] = Tree[leftChildIndex] + Tree[rightChildIndex];
    }

    // range query: get the sum of elements in the range [l, r]
    public int QueryRange(int node, int start, int end, int l, int r)
    {
        Propagate(node, start, end);

        if (start > r || end < l) // completely out of range
        {
            return 0; // neutral value for sum
        }

        if (start >= l && end <= r) // completely within range
        {
            return Tree[node];
        }

        int mid = (start + end) / 2;
        int leftChildIndex = 2 * node + 1;
        int rightChildIndex = 2 * node + 2;

        int leftSum = QueryRange(leftChildIndex, start, mid, l, r);
        int rightSum = QueryRange(rightChildIndex, mid + 1, end, l, r);

        return leftSum + rightSum;
    }

    public static void Main(string[] args)
    {
        int n = 6;
        int[] array = { 1, 3, 5, 7, 9, 11 };

        LazyPropagationTree tree = new LazyPropagationTree(n);
        tree.Build(array, 0, 0, n - 1);

        Console.WriteLine(tree.QueryRange(0, 0, n - 1, 1, 3)); // sum of elements in range [1, 3]
        tree.UpdateRange(0, 0, n - 1, 1, 3, 10); // add 10 to elements in range [1, 3]
        Console.WriteLine(tree.QueryRange(0, 0, n - 1, 1, 3)); // updated sum of elements in range [1, 3]
    }
}