using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// A Segment Tree is a data structure that allows efficient range queries and updates on an array.
// It divides the array into segments and organizes them into a tree structure, where each node
// represents a range of the array and stores information (e.g., sum, minimum, maximum) about that range.
//This allows the tree to quickly "zoom in" on the specific parts of the array that are relevant to the query, without having to scan the entire array.
//
// Why use a Segment Tree?
// - Efficient range queries: You can quickly compute the result (e.g., sum, min, max) of a subarray or range.
// - Efficient updates: You can modify an element or update a range, and the changes propagate efficiently through the tree.
// - It's ideal for scenarios where you need to frequently query and update a large array.
//
// Time complexity:
// - Build the tree: O(n), where n is the size of the array.
// - Range queries: O(log n), because the tree has a height of log n.
// - Point or range updates: O(log n), for the same reason.
class SegmentTree {
    private int[] tree;
    private int n;

    public SegmentTree(int[] arr) {
        n = arr.Length;
        tree = new int[4 * n]; // sufficient space for segment tree
        Build(arr, 0, 0, n - 1);
    }

    private void Build(int[] arr, int node, int start, int end) {
        if (start == end) {
            tree[node] = arr[start]; // leaf node
        } else {
            int mid = (start + end) / 2;
            Build(arr, 2 * node + 1, start, mid);
            Build(arr, 2 * node + 2, mid + 1, end);
            tree[node] = tree[2 * node + 1] + tree[2 * node + 2]; 
        }
    }

    public int Query(int L, int R) {
        return Query(0, 0, n - 1, L, R);
    }

    private int Query(int node, int start, int end, int L, int R) {
        if (R < start || L > end) {
            return 0; // out of range
        }
        if (L <= start && end <= R) {
            return tree[node]; // fully within range
        }
        int mid = (start + end) / 2;
        int leftSum = Query(2 * node + 1, start, mid, L, R);
        int rightSum = Query(2 * node + 2, mid + 1, end, L, R);
        return leftSum + rightSum;
    }

    public void Update(int index, int value) {
        Update(0, 0, n - 1, index, value);
    }

    private void Update(int node, int start, int end, int idx, int value) {
        if (start == end) {
            tree[node] = value; // update leaf node
        } else {
            int mid = (start + end) / 2;
            if (idx <= mid) {
                Update(2 * node + 1, start, mid, idx, value);
            } else {
                Update(2 * node + 2, mid + 1, end, idx, value);
            }
            tree[node] = tree[2 * node + 1] + tree[2 * node + 2]; // merge
        }
    }
}

class Program {
    static void Main() {
        int[] arr = { 1, 3, 5, 7, 9, 11 };
        SegmentTree st = new SegmentTree(arr);

        Console.WriteLine("Sum of range [1, 3]: " + st.Query(1, 3)); // output: 15
        st.Update(1, 10);
        Console.WriteLine("Sum of range [1, 3] after update: " + st.Query(1, 3)); // output: 22
    }
}