using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// KMP Algorithm Explanation
// The KMP algorithm is used for pattern matching:
// Finds all occurrences of a pattern in a text efficiently.
// Avoids rechecking characters in the text that have already been matched.
//
// Key Components:
// Prefix Table (LPS Array):
// LPS stands for "Longest Prefix which is also Suffix."
// Used to determine where to continue matching after a mismatch.

// Two Phases:
// Preprocessing: Build the LPS array for the pattern.
// Search: Use the LPS array to efficiently match the pattern against the text.
// 
// O(n) with n = pattern length
class KMP
{
    // function to build the LPS (Longest Prefix Suffix) array
    public static int[] BuildLPS(string pattern)
    {
        int n = pattern.Length;
        int[] lps = new int[n];
        int len = 0; // length of the previous longest prefix suffix
        int i = 1;

       lps[0] = 0; // base case: the longest prefix-suffix for the first character is always 0

        while (i < n) // loop through the rest of the pattern to calculate LPS values
        {
            if (pattern[i] == pattern[len]) // case 1: current character matches the prefix character
            {
                len++; // increase the length of the matching prefix-suffix
                lps[i] = len; // store the length in the LPS array
                i++; // move to the next character in the pattern
            }
            else // case 2: current character does NOT match the prefix character
            {
                if (len != 0) // if there is a previously matched prefix
                {
                    len = lps[len - 1]; // fallback to the previous prefix using the LPS value
                    // do not increment i; retry the current character with the new prefix length
                }
                else // if no prefix is matched (len == 0)
                {
                    lps[i] = 0; // no prefix-suffix match for this position
                    i++; // move to the next character
                }
            }
        }

        return lps;
    }

    // KMP Search Function
    public static void KMPSearch(string text, string pattern)
    {
        int m = pattern.Length;
        int n = text.Length;

        // preprocess the pattern to build the LPS array
        int[] lps = BuildLPS(pattern);

        int i = 0; // index for text
        int j = 0; // index for pattern

        while (i < n)
        {
            if (pattern[j] == text[i])
            {
                i++;
                j++;
            }

            if (j == m)
            {
                Console.WriteLine("Pattern found at index " + (i - j));
                j = lps[j - 1]; // continue searching for other matches
            }
            else if (i < n && pattern[j] != text[i])
            {
                if (j != 0)
                {
                    j = lps[j - 1]; // fallback in the pattern
                }
                else
                {
                    i++;
                }
            }
        }
    }

    public static void Main(string[] args)
    {
        string text = "ababcabcabababd";
        string pattern = "ababd";

        Console.WriteLine("Text: " + text);
        Console.WriteLine("Pattern: " + pattern);

        KMPSearch(text, pattern);
    }
}