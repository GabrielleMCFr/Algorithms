using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

/// <summary>
/// We are tasked with determining the number of ways to make change for a given amount n using an unlimited supply of coins of specified denominations. This is a classic "coin change problem," which is a common problem in dynamic programming.
// The problem requires us to:
// Use the given denominations of coins.
// Find all unique combinations of coins that sum up to the target amount n
// O(m × n)
/// </summary>
public class CoinsChange
{
    public static long getWays(int n, List<long> c)
    {
        // initialize a dp array of size n + 1 with all zeros
        long[] dp = new long[n + 1];
        
        // base case: there is 1 way to make change for amount 0 (use no coins)
        dp[0] = 1;
        
        // iterate over each coin denomination
        foreach (var coin in c)
        {
            // update the dp array for all amounts from 'coin' to 'n'
            for (int amount = (int)coin; amount <= n; amount++)
            {
                dp[amount] = dp[amount] + dp[amount - (int)coin];
                // explanation:
                // dp[amount] includes all ways to make 'amount' without using 'coin'
                // plus all ways to make 'amount - coin' (and then adding 'coin' to it).
            }
        }
        
        // return the total number of ways to make change for amount 'n'
        return dp[n];
    }

    // Variant : get the minimum number of coins to an amount
    public static int MinCoins(int amount, int[] coins)
    {
        // initialize a dp array where dp[i] is the minimum coins to make amount i
        int[] dp = new int[amount + 1];
        
        // fill the dp array with a large value (infinity equivalent)
        Array.Fill(dp, int.MaxValue);
        
        // base case: 0 coins are needed to make amount 0
        dp[0] = 0;
        
        // iterate through each coin
        foreach (int coin in coins)
        {
            // update dp array for all amounts that can include the current coin
            for (int currentAmount = coin; currentAmount <= amount; currentAmount++)
            {
                // if dp[currentAmount - coin] is not "infinity," update dp[currentAmount] the if is there to avoid overflow by adding 1 to infinity
                if (dp[currentAmount - coin] != int.MaxValue)
                {
                    // the +1 accounts for the current coin being used
                    dp[currentAmount] = Math.Min(dp[currentAmount], dp[currentAmount - coin] + 1); 
                }
            }
        }
        
        // if dp[amount] is still "infinity," it means we can't make the amount
        return dp[amount] == int.MaxValue ? -1 : dp[amount];
    }

    public static void Main(string[] args)
    {
        int n = 3; 
        List<long> c = new List<long> { 8, 3, 1, 2 }; 
        
        long result = getWays(n, c);
        Console.WriteLine(result); 
    }
}
