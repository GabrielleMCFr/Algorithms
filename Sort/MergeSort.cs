using System;

namespace Code.algorithms
{
    public class MergeSort
    {
        // Merge Sort Algorithm:
        // Merge Sort is a divide-and-conquer algorithm.
        // It splits the array into two halves until each subarray contains only one element.
        // Then, it merges the subarrays, sorting elements during the merge.
        // Time Complexity: O(n log n)
        public int[] Sort(int[] array, int left, int right)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Array cannot be null or empty.");

            if (left < right)
            {
                int middle = left + (right - left) / 2;

                // recursively sort the left and right halves
                Sort(array, left, middle);
                Sort(array, middle + 1, right);

                // merge the sorted halves
                Merge(array, left, middle, right);
            }

            return array;
        }

        private void Merge(int[] array, int left, int middle, int right)
        {
            int leftArrayLength = middle - left + 1;
            int rightArrayLength = right - middle;

            // create temporary arrays
            var leftArray = new int[leftArrayLength];
            var rightArray = new int[rightArrayLength];

            // copy data to temporary arrays
            for (int i = 0; i < leftArrayLength; i++)
                leftArray[i] = array[left + i];
            for (int j = 0; j < rightArrayLength; j++)
                rightArray[j] = array[middle + 1 + j];

            // merge the temporary arrays back into the original array
            int leftIndex = 0, rightIndex = 0, mergedIndex = left;

            while (leftIndex < leftArrayLength && rightIndex < rightArrayLength)
            {
                if (leftArray[leftIndex] <= rightArray[rightIndex])
                {
                    array[mergedIndex++] = leftArray[leftIndex++];
                }
                else
                {
                    array[mergedIndex++] = rightArray[rightIndex++];
                }
            }

            // copy remaining elements of leftArray (if any)
            while (leftIndex < leftArrayLength)
            {
                array[mergedIndex++] = leftArray[leftIndex++];
            }

            // copy remaining elements of rightArray (if any)
            while (rightIndex < rightArrayLength)
            {
                array[mergedIndex++] = rightArray[rightIndex++];
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var mergeSort = new MergeSort();
            int[] array = { 12, 11, 13, 5, 6, 7 };

            Console.WriteLine("unsorted Array:");
            Console.WriteLine(string.Join(", ", array));

            mergeSort.Sort(array, 0, array.Length - 1);

            Console.WriteLine("sorted Array:");
            Console.WriteLine(string.Join(", ", array));
        }
    }
}
