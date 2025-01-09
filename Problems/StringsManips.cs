using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    public class PalindromesAndAnagrams
    {
        public bool IsPalindrome(string s) {

            var charList = new List<char>(); // to collect valid characters

            foreach (var c in s) {
                if (char.IsLetterOrDigit(c)) {
                    charList.Add(char.ToLower(c)); // normalize to lowercase and add to the list
                }
            }

            // convert the List<char> back to a string
            var normalized = new string(charList.ToArray()); // create a string from the list

            var left = 0;
            var right = normalized.Length - 1;
            while (left < right) {
                if (normalized[left] != normalized[right]) {
                    return false; // not a palindrome
                }
                left++;
                right--;
            }
            return true;
        }

        public bool IsAnagram(string s, string t) {
            if (s.Length != t.Length)
                return false;
            
            var dictS = new Dictionary<char, int>();
            var dictT = new Dictionary<char, int>();

            // store each present char and their frequency in each word
            foreach (var c in s) {
                if (!dictS.ContainsKey(c)) {
                    dictS[c] = 0;
                }
                dictS[c] ++;
            }

            foreach (var c in t) {
                if (!dictT.ContainsKey(c)) {
                    dictT[c] = 0;
                }
                dictT[c] ++;
            }

            // if not same count, means that there are diff chars, so no anagrams
            if (dictS.Count != dictT.Count)
                return false;
            
            // compare if the keys and values are different in the two dicts
            foreach (var kvp in dictS) {
                if (!dictT.ContainsKey(kvp.Key) || dictT[kvp.Key] != kvp.Value) {
                    return false;
                }
            }

            return true;
        }
    }

    // length of the longest substring without repeating characters in the substring
    public int LengthOfLongestSubstring(string s)
    {
        Dictionary<char, int> charIndex = new Dictionary<char, int>();
        int maxLength = 0, start = 0;
        
        for (int i = 0; i < s.Length; i++)
        {
            if (charIndex.ContainsKey(s[i]))
            {
                start = Math.Max(start, charIndex[s[i]] + 1);
            }
            charIndex[s[i]] = i;
            maxLength = Math.Max(maxLength, i - start + 1);
        }
        
        return maxLength;
    }
}