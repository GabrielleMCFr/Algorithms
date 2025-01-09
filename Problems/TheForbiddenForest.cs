using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    /*
    A problem with Hermione and Ron lost in the forbidden forest. 
    Hermione must wave her wand each time they have to change direction. At the beggining Ron makes a guess
    on how many times she will have to wave her wand, we must determine how many wand moves she does,
    and if it's the same number than Ron's guess.
    The grid is filled with O (valid cell), X (obstacles, trees invalid cell). There is a M (starting point) and * (the goal)
    */
    public class TheForbiddenForest
    {
        // the wand moves at decisions points (where they need to chose a path.)
        // count decisions points while using bfs to find the shortest path.
        // k is Ron's guess on how many times hermione will wave her wand. The matrix is the grid of the forbidden forest.
        public static string countWandMoves(List<string> matrix, int k)
        {
            List<List<char>> grid = matrix.Select(row => row.ToList()).ToList();
            var wandMoves = 0;
            int rows = grid.Count;
            int cols = grid[0].Count;

            // Directions for moving: up, down, left, right
            int[][] directions = new int[][]
            {
                new int[] {-1, 0}, // up
                new int[] {1, 0},  // down
                new int[] {0, -1}, // left
                new int[] {0, 1}   // right
            };

            // Queue for BFS
            Queue<(int row, int col, int moves)> queue = new Queue<(int, int, int)>();

            // Visited set
            HashSet<(int, int)> visited = new HashSet<(int, int)>();

            // Find the starting position
            var startRow = 0;
            var startCol = 0;
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    if (grid[i][j] == 'M') // M is where Hermione and Ron are.
                    {
                        startRow = i;
                        startCol = j;
                    }
                }
            }

            // Enqueue the starting position and mark it as visited
            queue.Enqueue((startRow, startCol, 0));
            visited.Add((startRow, startCol));

            while (queue.Count > 0)
            {
                var (row, col, moves) = queue.Dequeue();

                // If the goal is reached, return the result
                if (grid[row][col] == '*')
                {
                    wandMoves = moves;
                    break;
                }

                // Count the number of valid directions from this cell
                int validDirections = 0;
                foreach (var direction in directions)
                {
                    int newRow = row + direction[0];
                    int newCol = col + direction[1];

                    // Check if the move is valid
                    if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols &&
                        grid[newRow][newCol] != 'X' && !visited.Contains((newRow, newCol)))
                    {
                        validDirections++;
                    }
                }

                // If more than one valid direction, it's a decision point, increment wand moves
                if (validDirections > 1)
                {
                    moves++;
                }

                // Explore all valid neighbors
                foreach (var direction in directions)
                {
                    int newRow = row + direction[0];
                    int newCol = col + direction[1];

                    if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols &&
                        grid[newRow][newCol] != 'X' && !visited.Contains((newRow, newCol)))
                    {
                        queue.Enqueue((newRow, newCol, moves));
                        visited.Add((newRow, newCol));
                    }
                }
            }

            return wandMoves == k ? "Impressed" : "Oops!";
        }

    }
}