using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;
    public class LKnights
    {
        /*
        This solution calculates the shortest path for a custom knight piece (a, b)
        on an n x n chessboard. Each knight moves in an L-shape: (a, b) or (b, a).
        For all pairs (a, b) where 1 ≤ a, b < n, the goal is to determine the minimum
        number of moves required to go from the top-left corner (0, 0) to the
        bottom-right corner (n-1, n-1). If a position is unreachable, we return -1.
        */

        public static List<List<int>> KnightlOnAChessboard(int n)
        {
            //var results = new List<List<int>>();

            //for (int i = 0; i < n - 1; i++)
            //{
            //    results.Add(new List<int>(new int[n - 1])); 
            //}

            //for (int i = 0; i < n - 1; i++)
            //{
            //    for (int j = 0; j < n - 1; j++)
            //    {
            //        results[i][j] = int.MaxValue;
            //    }
            //}

            // or :
            var results = new List<List<int>>(
                Enumerable.Range(0, n - 1)
                        .Select(_ => Enumerable.Repeat(int.MaxValue, n - 1).ToList())
                        .ToList()
            );

            for (int a = 1; a < n; a++)
            {
                for (int b = 1; b < n; b++)
                {
                    // move (a, b) and (b, a) always get the same result, so no need to recompute them.
                    if (results[b - 1][a - 1] != int.MaxValue)
                        results[a - 1][b - 1] = results[b - 1][a - 1];
                    // we use BFS to get the minimum move for this knight.
                    else
                        results[a - 1][b - 1] = GetMinimumMoves(n, a, b);
                }
            }

            return results;
        }

        // BFS 
        static int GetMinimumMoves(int n, int a, int b)
        {
            // Directions possibles
            int[][] directions = new int[][]
            {
                new int[] {a, b}, new int[] {a, -b}, new int[] {-a, b}, new int[] {-a, -b},
                new int[] {b, a}, new int[] {b, -a}, new int[] {-b, a}, new int[] {-b, -a}
            };

            // queue : x, y, number of moves so far
            var queue = new Queue<(int, int, int)>();
            queue.Enqueue((0, 0, 0));

            // to know if a cell grid has been visited
            bool[,] visited = new bool[n, n];
            visited[0, 0] = true;

            while (queue.Count > 0)
            {
                var (x, y, moves) = queue.Dequeue();

                // if goal
                if (x == n - 1 && y == n - 1)
                    return moves;

                // explore every direction
                foreach (var dir in directions)
                {
                    int newX = x + dir[0];
                    int newY = y + dir[1];

                    // if valid (not off board, and not already visited)
                    if (newX >= 0 && newY >= 0 && newX < n && newY < n && !visited[newX, newY])
                    {
                        visited[newX, newY] = true;
                        queue.Enqueue((newX, newY, moves + 1));
                    }
                }
            }

            // -1 if not possible
            return -1;
        }
    }


class Program
{
    public static void Main(string[] args)
    {
        // for example
        int n = 3;

        List<List<int>> result = LKnights.KnightlOnAChessboard(n);

        Console.WriteLine(String.Join("\n", result.Select(x => String.Join(" ", x))));
    }
}