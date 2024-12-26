using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// This implementation provides a Fenwick Tree (Binary Indexed Tree), a data structure for efficiently handling prefix sums and updates.
// Fenwick Trees are ideal for dynamic problems where the data changes over time.
// Fenwick Trees (also called Binary Indexed Trees) are used to efficiently:
// - Update values in an array (e.g., incrementing a single value or replacing it).
// - Query prefix sums or range sums dynamically, without recomputing sums from scratch.
// Typical use cases:
// - Best for dynamic problems involving sums or XOR operations.
// Note : - A Segment Tree can also handle range queries and updates in O(logn), but require more space (O4n)
// - Not suitable for operations like maximum/minimum, which require a Segment Tree.
// O(logn) for update, prefixsum, rangesum
// O(n) for space
public class FenwickTree
{
    private int[] Tree; // the internal array that represents the Fenwick Tree
    private int Size; // size of the Fenwick Tree

    public FenwickTree(int size)
    {
        Size = size;
        Tree = new int[size + 1]; // fenwick tree uses 1-based indexing, so we allocate size + 1
    }

    // update the Fenwick Tree by adding value to the index
    public void Update(int index, int value)
    {
        index++; // convert to 1-based index

        while (index <= Size)
        {
            Tree[index] += value; // add value to the current position
            index += index & -index; // move to the next index that this node affects
        }
    }

    // get the prefix sum from index 0 to index
    // the prefix sum of an array is the sum of elements from the beginning of the array up to a specified index.
    public int PrefixSum(int index)
    {
        index++; // convert to 1-based index
        int sum = 0;

        while (index > 0)
        {
            sum += Tree[index]; // add the current position's value
            index -= index & -index; // move to the parent node
        }

        return sum;
    }

    // get the sum of values in the range [left, right]
    // the range sum is the sum of elements between two indices in the array
    public int RangeSum(int left, int right)
    {
        return PrefixSum(right) - PrefixSum(left - 1);
    }

    public static void Main(string[] args)
    {
        // create a Fenwick Tree for an array of size 6
        FenwickTree fenwickTree = new FenwickTree(6);

        // update values in the Fenwick Tree (equivalent to adding values to the array)
        fenwickTree.Update(0, 1); // add 1 at index 0
        fenwickTree.Update(1, 3); // add 3 at index 1
        fenwickTree.Update(2, 5); // add 5 at index 2
        fenwickTree.Update(3, 7); // add 7 at index 3
        fenwickTree.Update(4, 9); // add 9 at index 4
        fenwickTree.Update(5, 11); // add 11 at index 5

        // query prefix sums
        Console.WriteLine(fenwickTree.PrefixSum(2)); // output: 9 (1 + 3 + 5)
        Console.WriteLine(fenwickTree.PrefixSum(4)); // output: 25 (1 + 3 + 5 + 7 + 9)

        // query range sums
        Console.WriteLine(fenwickTree.RangeSum(1, 3)); // output: 15 (3 + 5 + 7)
        Console.WriteLine(fenwickTree.RangeSum(2, 5)); // output: 32 (5 + 7 + 9 + 11)
    }
}