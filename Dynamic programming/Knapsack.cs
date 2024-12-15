using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// The Knapsack problem is about maximizing the total value of items you can carry in a knapsack with a limited capacity.
// You are given two arrays: one for weights and one for values, and you need to decide which items to include to maximize the total value,
// without exceeding the given capacity.
public class Knapsack
{
    // Knapsack
    public int Solve(int[] weights, int[] values, int capacity)
    {
        int n = weights.Length;
        // init the DP table where dp[i][w] represents the maximum possible value
        int[,] dp = new int[n + 1, capacity + 1]; // +1 because we need to include the case for 0 items

        // fill the DP table
        for (int i = 1; i <= n; i++)
        {
            for (int w = 0; w <= capacity; w++)
            {
                // note: the value of the i-th item is values[i - 1] (since indexing starts from 0)
                // and its weight is weights[i - 1].
                if (weights[i - 1] <= w)
                {
                    // case: The item's weight is less than or equal to the remaining capacity
                    // Include its value in our total: This is represented by values[i - 1]
                    // Use weights[i - 1] to figure out how much space in the knapsack will be used by this item.
                    // Access the best solution for the previous items: The value of dp[i-1][w - weights[i-1]]
                    // represents the maximum value we can get with the previous i-1 items and the new reduced capacity
                    // (w - weights[i-1]).
                    dp[i, w] = Math.Max(dp[i - 1, w], values[i - 1] + dp[i - 1, w - weights[i - 1]]);
                }
                else
                {
                    // case: The item's weight is too high to be included, so we just skip it.
                    dp[i, w] = dp[i - 1, w];
                }
            }
        }

        // the maximum value for all items with the given capacity is stored in dp[n][capacity].
        return dp[n, capacity];
    }
}

// usage
class Program
{
    static void Main()
    {
        var knapsack = new Knapsack();
        int[] weights = { 1, 2, 3, 5 };
        int[] values = { 10, 15, 40, 50 };
        int capacity = 6;

        int maxValue = knapsack.Solve(weights, values, capacity);

        Console.WriteLine($"The maximum value that can be carried in the knapsack is: {maxValue}");
    }
}
