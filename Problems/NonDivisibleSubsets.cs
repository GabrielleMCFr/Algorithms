using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms.Problems
{
    public class NonDivisibleSubsets
    {
        // return the max length of a subset of the list s who satisfies the following condition :
        // any two numbers in this number must not be divisible by k.
        // approach on modulo remainders.
        // Every element in the list can be represented by its k modulo remainder
        // if two numbers a and b who has remainders r1 and r2 are divisible par k, then it means the sum of r1 and r2 = k or a multiple of k.
                    public static int nonDivisibleSubset(int k, List<int> s)
        {
            // compting remainders 
            int[] remainderCounts = new int[k];
            foreach (var num in s)
            {
                remainderCounts[num % k]++;
            }

            // Include max one element where remainder is 0
            int maxSubsetSize = remainderCounts[0] > 0 ? 1 : 0;

            // check other remainder
            for (int i = 1; i <= k / 2; i++)
            {
                if (i == k - i) // case where the remainder is half of k 
                {
                    maxSubsetSize += 1; // Only one element can be included (if two remainders are, then it will be divisible by k)
                }
                else
                {
                    // chose the max
                    // i and  k - i are incompatible, if they are both included the subset will be divisible by k since k - i + i = k
                    maxSubsetSize += Math.Max(remainderCounts[i], remainderCounts[k - i]);
                }
            }

            return maxSubsetSize;
        }
    }
}