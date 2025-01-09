using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    public class ShortPalindrome
    {
        // A short palindrome has a length of 4 (a, b, b, a)
        public static int shortPalindrome(string s)
        {
            const int MOD = 1000000007;

            int n = s.Length; // Length of the string

            // Step 1: Precompute cumulative frequencies of all characters
            var frequency = new int[26][];
            for (int i = 0; i < 26; i++)
                frequency[i] = new int[n + 1]; // Each character gets a frequency array (+1 because we use 1 indexing)

            // Fill the frequency array
            // Essentially, it stores the number of times a character appears in the string from the start up to position `i`
            for (int i = 0; i < n; i++)
            {
                // Carry forward the previous frequencies for all characters
                for (int j = 0; j < 26; j++)
                    frequency[j][i + 1] = frequency[j][i];

                // Increment the count of the current character
                // note: `- 'a'` converts an alphabetic character into its numeric equivalent
                frequency[s[i] - 'a'][i + 1]++;
            }

            // Step 2: Count all possible (c1, c2) pairs
            // These pairs represent the two middle characters (c2, c2) of a palindrome of length 4.
            var pairCount = new long[26, 26]; // Array to store the number of possible (c1, c2) pairs
            for (int i = 0; i < n; i++)
            {
                int currentChar = s[i] - 'a'; // Convert the current character `s[i]` to an index (0 for 'a', 1 for 'b', etc.)
                for (int j = 0; j < 26; j++) // Iterate through all possible letters (from 'a' to 'z')
                {
                    // Number of `c2` (j) characters remaining after position `i`
                    int remainingC2 = frequency[j][n] - frequency[j][i + 1];

                    // Add this number of `c2` to the pair `(currentChar, c2)`
                    pairCount[currentChar, j] += remainingC2;
                }
            }

            // Step 3: Calculate the total number of palindromes
            long result = 0; // Variable to store the total number of palindromes found
            for (int i = 0; i < n; i++)
            {
                int currentChar = s[i] - 'a'; // The current character is treated as the last character of a palindrome
                for (int j = 0; j < 26; j++) // Iterate through all possible letters for `c2`
                {
                    // Reduce the contribution of this character in the remaining pairs
                    int remainingC2 = frequency[j][n] - frequency[j][i + 1];
                    pairCount[currentChar, j] -= remainingC2;

                    // Number of `c2` (j) characters before position `i`
                    int precedingC2 = frequency[j][i];

                    // Add to the result the product:
                    // Number of `c2` before × Number of `(c1, c2)` pairs remaining
                    result += (precedingC2 * pairCount[currentChar, j]) % MOD;

                    // Keep the result within bounds (modulo)
                    result %= MOD;
                }
            }

            return (int)result;
        }

        static void Main(string[] args)
        {
            // Test cases
            Console.WriteLine(shortPalindrome("akakak")); // Expected Output: 2
            Console.WriteLine(shortPalindrome("abba"));   // Expected Output: 1
            Console.WriteLine(shortPalindrome("aaaa"));   // Expected Output: 1
        }
    }

}