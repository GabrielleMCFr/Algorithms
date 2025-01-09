using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    public class RollingHashSearchPatternInGrid
    {
        // optimized with rolling hash:
        // the rolling hash is an efficient technique used to quickly compute the hash 
        // of a substring while allowing the hash to be updated as the substring "slides."
        // this accelerates algorithms that require comparing many substrings, 
        // such as searching for a pattern in a string or grid.

        public static string GridSearch(List<string> G, List<string> P)
        {
            int R = G.Count;       // number of rows in the grid
            int C = G[0].Length;   // number of columns in the grid
            int r = P.Count;       // number of rows in the pattern
            int c = P[0].Length;   // number of columns in the pattern

            // compute hashes for the rows of the pattern
            List<int> patternHashes = new List<int>();
            foreach (string line in P)
            {
                patternHashes.Add(ComputeHash(line));
            }

            // iterate over each possible position in the grid
            for (int i = 0; i <= R - r; i++)
            {
                for (int j = 0; j <= C - c; j++)
                {
                    // check if the hashes of the rows match
                    bool isMatch = true;
                    for (int k = 0; k < r; k++)
                    {
                        string gridSubstring = G[i + k].Substring(j, c);
                        int gridHash = ComputeHash(gridSubstring);

                        if (gridHash != patternHashes[k])
                        {
                            isMatch = false;
                            break; // no need to continue if one row does not match
                        }
                    }

                    if (isMatch)
                    {
                        return "YES"; // pattern found
                    }
                }
            }

            return "NO"; // pattern not found
        }

        // rolling hash function for a given string
        private static int ComputeHash(string s, int baseVal = 31, int mod = 1000000007)
        {
            long hash = 0;
            long power = 1;

            for (int i = s.Length - 1; i >= 0; i--)
            {
                hash = (hash + (s[i] - '0') * power) % mod;
                power = (power * baseVal) % mod;
            }

            return (int)hash;
        }
    }
}