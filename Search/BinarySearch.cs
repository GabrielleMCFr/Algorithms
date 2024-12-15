using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System;

namespace Code.algorithms
{
// Binary Search is an efficient algorithm for finding the position of a target element in a sorted array or data structure.
// It works by repeatedly dividing the search space into halves until the target is found or the search space becomes empty.
// Limitations : the data structure must be sorted.
// How it works:
// - Start with two pointers: left (beginning of the array) and right (end of the array).
// - Calculate the middle index: mid = left + (right - left) / 2.
// - Compare the value at mid with the target:
//      - If the value matches the target, return the index.
//      - If the target is smaller, move the right pointer to mid - 1 (search the left half).
//      - If the target is larger, move the left pointer to mid + 1 (search the right half).
// Repeat until the target is found or the search space is exhausted.

// Non exhaustive use cases : 
// - locate a target in a sorted list.
// - locate an element in a row-sorted 2D array.
// - locate an element in a globally sorted 2D array.
// - find bounds, determine where a target fits in a sorted array.
// - search for words in a sorted dictionary
// - solve problems like finding square roots or thresholds in sorted data.
    public class BinarySearch
    {
        // binary search in a sorted array
        // O(log n)
        public int BinarySearchInArray(int[] sortedArray, int target)
        {
            int left = 0;
            int right = sortedArray.Length - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;  // to prevent overflow

                if (sortedArray[mid] == target) return mid;  // element found
                if (sortedArray[mid] < target) left = mid + 1;  // search in the right half
                else right = mid - 1;  // search in the left half
            }

            return -1;  // element not found
        }

        // binary Search in a row-sorted 2D matrix
        // O(log n + log m) (using binary search on rows and then columns)
        public int BinarySearchInMatrix(int[,] matrix, int target)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            // perform binary search on the first column to find the row
            int top = 0, bottom = rows - 1;
            while (top <= bottom)
            {
                int midRow = top + (bottom - top) / 2;

                if (target < matrix[midRow, 0]) bottom = midRow - 1;
                else if (target > matrix[midRow, cols - 1]) top = midRow + 1;
                else
                {
                    // target lies in this row, perform binary search on this row
                    int left = 0, right = cols - 1;
                    while (left <= right)
                    {
                        int midCol = left + (right - left) / 2;
                        if (matrix[midRow, midCol] == target) return midRow * cols + midCol;
                        if (matrix[midRow, midCol] < target) left = midCol + 1;
                        else right = midCol - 1;
                    }
                }
            }

            return -1;  // element not found
        }

        // binary search in a fully flattened 2D matrix
        // O(log(n * m))
        public int BinarySearchInFlattenedMatrix(int[,] matrix, int target)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int left = 0;
            int right = rows * cols - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                int midValue = matrix[mid / cols, mid % cols];  // get value at mid index

                if (midValue == target) return mid;  // element found
                if (midValue < target) left = mid + 1;  // search in the right half
                else right = mid - 1;  // search in the left half
            }

            return -1;  // element not found
        }
    }

    // Usage
    class Program {
        static void Main()
        {
            // BS in sorted array
            int[] sortedArray = { 1, 3, 5, 7, 9 };
            BinarySearch bs = new BinarySearch();
            Console.WriteLine(bs.BinarySearchInArray(sortedArray, 7));  // output: 3
            Console.WriteLine(bs.BinarySearchInArray(sortedArray, 4));  // output: -1

            // BS in 2D matrix
            int[,] matrix = {
                { 1, 3, 5 },
                { 7, 9, 11 },
                { 13, 15, 17 }
            };
            Console.WriteLine(bs.BinarySearchInMatrix(matrix, 9));  // output: 4
            Console.WriteLine(bs.BinarySearchInMatrix(matrix, 6));  // output: -1

            // BS in flattened matrix
            Console.WriteLine(bs.BinarySearchInFlattenedMatrix(matrix, 9));  // output: 4
            Console.WriteLine(bs.BinarySearchInFlattenedMatrix(matrix, 6));  // output: -1
        }
    }
}
