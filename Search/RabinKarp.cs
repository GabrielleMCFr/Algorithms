using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms.Search
{
    // The Rabin-Karp algorithm uses a hash function to efficiently compare a pattern's hash 
    // to a rolling hash computed for substrings in the text. 
    // It ensures fast pattern matching by avoiding direct character comparisons in most cases.
    // O(n+m)
    public class RabinKarp
    {
        public static int Search(string text, string pattern)
        {
            int n = text.Length;     
            int m = pattern.Length; 

            // hash base, usually a primary number or a number close to the alphabet count 
            int baseVal = 31;  

            // modulo to avoid overflow (need a big primary number)
            int mod = 1000000007;    

            // calculate pattern hash and hash of the first substring
            long patternHash = 0;
            long currentHash = 0;
            long power = 1;

            for (int i = 0; i < m; i++)
            {
                patternHash = (patternHash * baseVal + (pattern[i] - 'a' + 1)) % mod;
                currentHash = (currentHash * baseVal + (text[i] - 'a' + 1)) % mod;

                if (i < m - 1)
                {
                    power = (power * baseVal) % mod; 
                }
            }

            // Check the text for the pattern
            for (int i = 0; i <= n - m; i++)
            {
                // if hashes are the same
                if (currentHash == patternHash)
                {
                    // check to avoid collisions
                    if (text.Substring(i, m) == pattern) 
                    {
                        // pattern found at i
                        return i; 
                    }
                }

                // Update hash for the next subsstring
                if (i < n - m)
                {
                    currentHash = (currentHash - (text[i] - 'a' + 1) * power) % mod;
                    currentHash = (currentHash * baseVal + (text[i + m] - 'a' + 1)) % mod;

                    // make sure the hash is positive
                    if (currentHash < 0) currentHash += mod;
                }
            }

            // pattern not found
            return -1; 
        }

        public static void Main(string[] args)
        {
            string text = "abcdefg";
            string pattern = "cde";

            int result = Search(text, pattern);

            if (result != -1)
            {
                Console.WriteLine($"Motif trouvé à l'indice {result}");
            }
            else
            {
                Console.WriteLine("Motif non trouvé");
            }
        }
    }
}