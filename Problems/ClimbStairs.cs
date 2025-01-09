using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    public class ClimbStairs
    {
        // Climb stairs
        private static Dictionary<int, int> memo = new Dictionary<int, int>();

    public int ClimbStairs(int n) {
        // To calculate the number of ways to climb the stairs, we can observe that when we are on the nth stair,
        // we have two options:
        // either we climbed one stair from the (n-1)th stair or
        // we climbed two stairs from the (n-2)th stair.

        if (n == 0 || n == 1)
            return 1;
        if (memo.ContainsKey(n))
            return memo[n];

        memo[n] = ClimbStairs(n-1) + ClimbStairs(n-2);

        return memo[n];
        }        
    }
}   