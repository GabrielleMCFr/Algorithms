using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms.Sort algorithms;

// step 1: get the number of elements in the array
// step 2: start from the second element
// step 3: the current element to be inserted into the sorted part
// step 4: start comparing with the elements before the current element
// step 5: shift elements of arr[0..i-1] that are greater than key, to one position ahead
// step 6: insert the key at its correct position
// Time complexity:
// - best case: O(n) (when the array is already sorted)
// - worst case: O(n^2) (when the array is sorted in reverse order)
public class InsertionSort
{
    public void Sort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 1; i < n; i++)
        {
            int key = arr[i];
            int j = i - 1;

            while (j >= 0 && arr[j] > key)
            {
                arr[j + 1] = arr[j]; // shift the element one position to the right
                j--; // move to the previous element
            }
            arr[j + 1] = key;
        }
    }
}

// Usage
class Program
{
    static void Main()
    {
        int[] array = { 12, 11, 13, 5, 6 };

        Console.WriteLine("Original Array:");
        Console.WriteLine(string.Join(", ", array));

        var insertionSort = new InsertionSort();
        insertionSort.Sort(array);

        Console.WriteLine("Sorted Array:");
        Console.WriteLine(string.Join(", ", array));
    }
}