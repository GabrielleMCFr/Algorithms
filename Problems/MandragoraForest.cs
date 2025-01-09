using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

/// <summary>
/// We want to maximize the xp points gained from battling mandragoras.
/// List H is a list of the mandragoras health (highest the health, the strongest the mandragora)
/// we have two choice : eat it, and up our own health with 1, or battle it, and up our xp with our health * the mandragora's health.
/// We must balance long term strategy (eat as most as possible before battle) and short term (battle as much as possible) and find the pivot.
/// O(nlogn)
/// </summary>
public class Result
{
    public static long mandragora(List<int> H)
    {
        // sort the health values in ascending order to consider weak mandragoras first.
        // Because stronger mandragoras give higher xp.
        H.Sort();

        // compute the total health of all mandragoras. We'll need to calculate the xp we can gain if battling remaining mandragoras.
        long totalHealth = 0;
        foreach (int h in H)
        {
            totalHealth += h;
        }

        // initialize variables to track the maximum experience points.
        long maxExperience = 0; // starting xp is 0.
        long currentHealth = 1; // starting health is 1.

        // traverse through the sorted list and calculate maximum experience.
        foreach (int h in H)
        {
            // calculate experience if we battle all remaining mandragoras. (if we take the actual value as pivot)
            maxExperience = Math.Max(maxExperience, currentHealth * totalHealth);

            // eat this mandragora: increment our health and remove its health from the total
            currentHealth++;
            totalHealth -= h;
        }

        // return the maximum experience possible.
        return maxExperience;
    }
}

class Solution
{
    public static void Main(string[] args)
    {
        List<int> H = new List<int> { 3, 2, 5 }; 
        long result = Result.mandragora(H); 
        Console.WriteLine(result); 
    }
}
