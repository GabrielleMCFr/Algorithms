using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;
    
public class BubbleSort
{
    // bubble sort :
    // compare adjacent elements until it's sorted.
    // O(n^2), inefficient
    public int[] SortArray(int[] array)
    {
        var n = array.Length;
        bool swapRequired;
        for (int i = 0; i < n - 1; i++) 
        {
            swapRequired = false;
            for (int j = 0; j < n - i - 1; j++)
                if (array[j] > array[j + 1])
                {
                    var tempVar = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = tempVar;
                    swapRequired = true;
                }
            if (swapRequired == false)
                break; // if nothing has changed, everything is already sorted, we cut it short.
        }
        return array;
    }
}

// Usage
class Program
{
    static void Main()
    {
        int[] array = { 38, 27, 43, 3, 9, 82, 10 };

       Console.WriteLine("Original Array:");
       Console.WriteLine(string.Join(", ", array));

       BubbleSort bubbleSort = new BubbleSort();
       int[] sortedArray = bubbleSort.SortArray(array);

       Console.WriteLine("Sorted Array:");
       Console.WriteLine(string.Join(", ", sortedArray));
    }
}