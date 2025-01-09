using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    public class MagicSquare
    {
        /*
        We define a magic square to be an n*n matrix of distinct positive integers from 1 to n2 where the sum of any row, column, or diagonal of length n is always equal to the same number: the magic constant.
        You will be given a 3*3 matrix s of integers in the inclusive range [1, 9]. We can convert any digit a to any other digit b in the range [1, 9] at cost of Abs(a-b). Given s, convert it into a magic square at minimal cost. 
        Print this cost on a new line.
        Note: The resulting magic square must contain distinct integers in the inclusive range [1, 9].
        */

        public static int formingMagicSquare(List<List<int>> s)
        {
            // vu que le carre magique sera que 3par3, on peut juste avoir les 8 carres possibles, 
            // et comparer le carre actuels aux carres possibles et choisir le cout min
            // note : on pourrait utiliser DFS et backtracking, mais ça serait ineficace. Ici on a O(1), 
            // et avec dfs et backtracking, for check toutes les permutations, cad O(n!)
            // Les 8 carres magiques possibles
            int[][][] magicSquares = new int[][][]
            {
                new int[][] { new int[] { 8, 1, 6 }, new int[] { 3, 5, 7 }, new int[] { 4, 9, 2 } },
                new int[][] { new int[] { 6, 1, 8 }, new int[] { 7, 5, 3 }, new int[] { 2, 9, 4 } },
                new int[][] { new int[] { 4, 9, 2 }, new int[] { 3, 5, 7 }, new int[] { 8, 1, 6 } },
                new int[][] { new int[] { 2, 9, 4 }, new int[] { 7, 5, 3 }, new int[] { 6, 1, 8 } },
                new int[][] { new int[] { 8, 3, 4 }, new int[] { 1, 5, 9 }, new int[] { 6, 7, 2 } },
                new int[][] { new int[] { 4, 3, 8 }, new int[] { 9, 5, 1 }, new int[] { 2, 7, 6 } },
                new int[][] { new int[] { 6, 7, 2 }, new int[] { 1, 5, 9 }, new int[] { 8, 3, 4 } },
                new int[][] { new int[] { 2, 7, 6 }, new int[] { 9, 5, 1 }, new int[] { 4, 3, 8 } }
            };

            int minCost = int.MaxValue;

            // Comparer s a chaque carre magique
            foreach (var magicSquare in magicSquares)
            {
                int currentCost = 0;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        currentCost += Math.Abs(s[i][j] - magicSquare[i][j]);
                    }
                }

                minCost = Math.Min(minCost, currentCost);
            }

            return minCost;
            
        }
    }
}