using System;

namespace Code.algorithms
{
    public class QuickSort
    {
        // Quick Sort
        // uses the "divide and conquer" strategy to sort elements in arrays or lists.
        // it implements this by choosing an element as a pivot and using it to partition the array.
        // uime Complexity: O(n log n) generally, but O(n^2) if the pivot is poorly chosen.
        // pivot selection: first element, last element, or median-of-three.
        public int[] SortArray(int[] array, int leftIndex, int rightIndex)
        {
            // base case to end recursion
            if (leftIndex >= rightIndex)
                return array;

            // determine the median of three for the pivot
            int pivot = MedianOfThree(array, leftIndex, rightIndex);

            int i = leftIndex;
            int j = rightIndex;

            // partitioning around the pivot
            while (i <= j)
            {
                while (array[i] < pivot) i++;
                while (array[j] > pivot) j--;

                if (i <= j)
                {
                    Swap(array, i, j);
                    i++;
                    j--;
                }
            }

            // recursive calls to sort the partitions
            if (leftIndex < j)
                SortArray(array, leftIndex, j);
            if (i < rightIndex)
                SortArray(array, i, rightIndex);

            return array;
        }

        private int MedianOfThree(int[] array, int leftIndex, int rightIndex)
        {
            int midIndex = (leftIndex + rightIndex) / 2;

            // sort left, middle, and right elements to find the median
            if (array[leftIndex] > array[midIndex])
                Swap(array, leftIndex, midIndex);
            if (array[leftIndex] > array[rightIndex])
                Swap(array, leftIndex, rightIndex);
            if (array[midIndex] > array[rightIndex])
                Swap(array, midIndex, rightIndex);

            // use the middle element as the pivot after sorting
            return array[midIndex];
        }

        // helper method to swap elements
        private void Swap(int[] array, int index1, int index2)
        {
            int temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
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

            QuickSort quickSort = new QuickSort();
            int[] sortedArray = quickSort.SortArray(array, 0, array.Length - 1);

            Console.WriteLine("Sorted Array:");
            Console.WriteLine(string.Join(", ", sortedArray));
        }
    }
}
