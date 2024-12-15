using System;
using System.Collections.Generic;

// Longest Increasing Subsequence (LIS)
// LIS is a subsequence of a list that is strictly increasing, and the goal is to find the length of the longest such subsequence.
// In the basic approach, recursion is used to explore all possible subsequences.
// For each element, we have two choices: include it in the subsequence or ignore it.
// Using Binary Search, the algorithm is optimized to O(n log n).
// LIS implementation with DP with BS:
namespace Code.algorithms
{
    public class LongestIncreasingSubsequence
    {
        public int LengthOfLIS(int[] nums)
        {
            if (nums.Length == 0) return 0;

            // `dp` stores the smallest ending value of subsequences
            List<int> dp = new List<int>();

            foreach (var num in nums)
            {
                // we want to find the first element in dp that is greater than or equal to num
                int pos = dp.BinarySearch(num);

                // if not found, BinarySearch returns the bitwise complement (~pos) of the index where it should be inserted
                if (pos < 0) pos = ~pos;

                // if pos is equal to the size of dp, append the number to the subsequence
                if (pos == dp.Count)
                {
                    dp.Add(num); // this extends the length of our LIS so far
                }
                else
                {
                    dp[pos] = num; // replace the value at pos, this keeps the subsequence valid but optimizes for smaller values
                }
            }

            // the length of dp is the length of the LIS
            return dp.Count;
        }
    }

    class Program
    {
        static void Main()
        {
            var lis = new LongestIncreasingSubsequence();

            int[] arr = { 10, 9, 2, 5, 3, 7, 101, 18 };
            int length = lis.LengthOfLIS(arr);

            Console.WriteLine($"Length of LIS: {length}");
            // output: length of LIS: 4 (subsequence: [2, 3, 7, 101])
        }
    }
}