using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms.Problems
{
    public class QueenAttack
    {
        /*
        - int n: the number of rows and columns in the board
        - int k: the number of obstacles on the board
        - int r_q: the row number of the queen's position
        - int c_q: the column number of the queen's position
        */
        // where a queen can move on a board (the possible attack cases, where there is no obstacle) : 
        public static int queensAttack(int n, int k, int r_q, int c_q, List<List<int>> obstacles)
            {
                int possibleMoves = 0;

                // Convert obstacles into a HashSet for faster lookup
                var obstacleSet = new HashSet<string>(
                    obstacles.Select(obstacle => $"{obstacle[0]},{obstacle[1]}")
                );

                // Directions the queen can move
                int[][] directions = new int[][]
                {
                    new int[] { 0, 1 },  // Right
                    new int[] { 0, -1 }, // Left
                    new int[] { 1, 0 },  // Down
                    new int[] { -1, 0 }, // Up
                    new int[] { 1, 1 },  // Down-Right
                    new int[] { 1, -1 }, // Down-Left
                    new int[] { -1, 1 }, // Up-Right
                    new int[] { -1, -1 } // Up-Left
                };

                // Iterate through each direction
                foreach (var direction in directions)
                {
                    int row = r_q, col = c_q;

                    while (true)
                    {
                        row += direction[0];
                        col += direction[1];

                        // Check boundaries
                        if (row <= 0 || row > n || col <= 0 || col > n) break;

                        // Check for obstacles
                        if (obstacleSet.Contains($"{row},{col}")) break;

                        // Valid move
                        possibleMoves++;
                    }
                }

                return possibleMoves;
            }
    }
}