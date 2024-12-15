using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

using System;
using System.Collections.Generic;

namespace Code.algorithms
{
    public class HeapSort
    {
        // Heap Sort
        // constructs a max-heap and repeatedly extracts the root (largest element) to sort the array.
        // Usage : heap Sort is better for sorting static arrays
        // O(n log n)
        public int[] SortArray(int[] array, int size)
        {
            if (size <= 1)
                return array;

            // build the max-heap
            for (int i = size / 2 - 1; i >= 0; i--)
            {
                Heapify(array, size, i);
            }

            // extract elements from the heap one by one
            for (int i = size - 1; i >= 0; i--)
            {
                // swap the root (largest element) with the last element
                Swap(array, 0, i);

                // restore the max-heap property on the reduced heap
                Heapify(array, i, 0);
            }

            return array;
        }

        // helper method to maintain the max-heap property
        private void Heapify(int[] array, int heapSize, int rootIndex)
        {
            int largest = rootIndex; // assuming the root is the largest element
            int leftChild = 2 * rootIndex + 1;
            int rightChild = 2 * rootIndex + 2;

            // check if the left child is larger than the root
            if (leftChild < heapSize && array[leftChild] > array[largest])
            {
                largest = leftChild;
            }

            // check if the right child is larger than the current largest
            if (rightChild < heapSize && array[rightChild] > array[largest])
            {
                largest = rightChild;
            }

            // if the largest element is not the root, swap and continue heapifying
            if (largest != rootIndex)
            {
                Swap(array, rootIndex, largest);
                Heapify(array, heapSize, largest);
            }
        }

        // helper method to swap elements in the array
        private void Swap(int[] array, int index1, int index2)
        {
            int temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }
    }

    // Or, sort with a priority queue.
    // O(nlogn)
    // Usage : best for dynamic or streaming inputs
    class PriorityQueueSortExample
    {
        public static void Sort(int[] arr)
        {
            PriorityQueue<int, int> priorityQueue = new PriorityQueue<int, int>();

            foreach (var item in arr)
            {
                priorityQueue.Enqueue(item, item); // asc order
                // priorityQueue.Enqueue(item, -item); // desc order
            }

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = priorityQueue.Dequeue(); // get the smallest 
            }
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

            // heap Sort
            HeapSort heapSort = new HeapSort();
            int[] heapSortedArray = heapSort.SortArray((int[])array.Clone(), array.Length);

            Console.WriteLine("Heap Sorted Array:");
            Console.WriteLine(string.Join(", ", heapSortedArray));

            // priority queue Sort
            int[] pqSortedArray = (int[])array.Clone();
            PriorityQueueSortExample.Sort(pqSortedArray);

            Console.WriteLine("Priority Queue Sorted Array:");
            Console.WriteLine(string.Join(", ", pqSortedArray));
        }
    }
}


