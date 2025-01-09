using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms.Problems
{
    public class AbsolutePermutation
    {
        /*
     On veut une permutation absolue, Chaque valeur dans la permutation est à une distance absolue exacte de k par rapport à sa position d'origine.
    Math.Abs(pos[i] - i) = k avec pos[i] la valeur a la position i dans la permutation avec 1 based indexing.
    donc i est soit égal a i + k soit i - k (bloc droite ou gauche)
     */

    public static List<int> AbsPermutation(int n, int k)
    {
        // meaning, no permutation
         if (k == 0)
        {
            List<int> naturalPermutation = new List<int>();
            for (int i = 1; i <= n; i++) naturalPermutation.Add(i);
            return naturalPermutation;
        }
        
        // we use blocks of length 2k, to permute the blocks themselves, and this is why n must be divisible by 2k
        if (n % (2 * k) != 0) 
            return new List<int>() {-1};
        
        List<int> permutation = new List<int>();
        bool addK = true;
        
        for (int i = 1; i <= n; i++)
        {
            permutation.Add(addK ? i + k : i - k);
            
            // every k element, we change from i + k to i - k
            if (i % k == 0) 
                addK = !addK;
        }

        return permutation;
        }
    }  
}