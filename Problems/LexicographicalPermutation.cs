using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms.Problems
{
    public class LexicographicalPermutation
    {
        // String manipulation to create the next lexicographical permutation of a word.
        // Time complexity: O(n + k log k), where n is the string length and k is the size of the suffix.
        /*
        Given a word, create a new word by swapping some or all of its characters. This new word must meet two criteria:

        It must be greater than the original word
        It must be the smallest word that meets the first condition
        */

        public static string biggerIsGreater(string w)
        {
            var chars = w.ToCharArray();
            
            // Step 1: Find the pivot - first char from the right that break decreasing order (meaning, the char i is smaller than i+1)
            // (chars[i] is smaller than chars[i + 1]).
            int i = chars.Length - 2;
            while (i >= 0 && chars[i] >= chars[i + 1])
            {
                i--;
            }

            // If no pivot is found, the word is already the largest permutation, so return "no answer".
            if (i < 0)
            {
                return "no answer";
            }
            
            // Step 2: Find the smallest character larger than the pivot at the right of it.     
            // by comparing chars the right of the pivot
            int j = chars.Length - 1;
            while (chars[j] <= chars[i])
            {
                j--;
            }

            // Step 3: Swap the pivot with this character
            (chars[i], chars[j]) = (chars[j], chars[i]);

            // Step 4: Sort the suffix (all characters to the right of the pivot) in ascending order
            // to ensure the resulting word is the smallest lexicographical permutation greater than the original.
            Array.Sort(chars, i + 1, chars.Length - (i + 1));

            return new string(chars);
        }
    }
}