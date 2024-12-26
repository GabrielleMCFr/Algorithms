using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    // Longest Common Subsequence
    // O(s1*s1)
    public class LCS
    {
        public int FindLCSLength(string s1, string s2) {

            // create a DP table with dimensions (s1.Length + 1) x (s2.Length + 1)
            int[,] dp = new int[s1.Length + 1, s2.Length + 1];

            // fill the DP table
            for (int i = 1; i <= s1.Length; i++) {
                for (int j = 1; j <= s2.Length; j++) {
                    // case 1: characters match. If the characters match, we add 1 to the value of dp[i-1][j-1], 
                    // which indicates that we can extend the length of the LCS found so far.
                    if (s1[i - 1] == s2[j - 1]) {
                        dp[i, j] = dp[i - 1, j - 1] + 1; // Include this character
                    } else {
                        // case 2: characters don't match. If the characters do not match, we take the maximum length 
                        // found by either ignoring the current character of s1 or s2
                        dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]); // exclude one character
                    }
                }
            }

            // the length of the LCS will be in dp[s1.Length, s2.Length]
            return dp[s1.Length, s2.Length];
        }

        // here, find LCS with retrieval of the subsequence in the dp table.
        public string FindLCS(string s1, string s2)
        {
            // init dp table
            int[,] dp = new int[s1.Length + 1, s2.Length + 1];

            // fill it
            for (int i = 1; i <= s1.Length; i++)
            {
                for (int j = 1; j <= s2.Length; j++)
                {
                    if (s1[i - 1] == s2[j - 1])
                    {
                        dp[i, j] = dp[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                    }
                }
            }

            // get lcs
            int m = s1.Length, n = s2.Length;
            var lcs = new List<char>(); 

            while (m > 0 && n > 0)
            {
                if (s1[m - 1] == s2[n - 1])
                {
                    // Chars match, we add them
                    lcs.Add(s1[m - 1]);
                    m--;
                    n--;
                }
                else if (dp[m - 1, n] > dp[m, n - 1])
                {
                    // LCS come from top
                    m--;
                }
                else
                {
                    // LCS come from left
                    n--;
                }
            }

            // reverse the lcs since we built it from the end of the dp to beginning
            lcs.Reverse();
            return new string(lcs.ToArray());
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            var lcs = new LCS();

            string s1 = "ABCBDAB";
            string s2 = "BDCAB";

            int lcsLength = lcs.FindLCSLength(s1, s2);

            Console.WriteLine($"The length of the Longest Common Subsequence is: {lcsLength}");
        }
    }
}