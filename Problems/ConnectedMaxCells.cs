using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    /// <summary>
    /// Given a grid filled with 0s and 1s, the task is to find the largest connected group of 1s.
    /// A connected group is defined as a cluster of adjacent 1s, where adjacency includes
    /// horizontal, vertical, and diagonal neighbors.
    /// </summary>
    public class ConnectedMaxCells
    {
        public static int connectedCell(List<List<int>> matrix)
        {
            int n = matrix.Count; // Number of rows in the matrix
            int m = matrix[0].Count; // Number of columns in the matrix
            int MaxRegion = 0; // Variable to store the maximum size of a connected region
            bool[,] visited = new bool[n, m]; // 2D array to track visited cells

            // Directions for visiting neighbors (relative x and y coordinates)
            int[] dx = {-1, -1, -1, 0, 0, 1, 1, 1}; // Change in row index
            int[] dy = {-1, 0, 1, -1, 1, -1, 0, 1}; // Change in column index

            // Depth-First Search function to calculate the size of a connected region
            int DFS(int x, int y) {
                visited[x, y] = true; // Mark the current cell as visited
                var size = 1; // Start with the current cell contributing to the size

                // Visit all 8 possible neighbors
                for (var i = 0; i < 8; i++) {
                    int nx = x + dx[i]; // Row index of the neighbor
                    int ny = y + dy[i]; // Column index of the neighbor

                    // Check if the neighbor is valid, unvisited, and filled (value is 1)
                    if (nx >= 0 && ny >= 0 && nx < n && ny < m && !visited[nx, ny] && matrix[nx][ny] == 1) {
                        size += DFS(nx, ny); // Recursively visit the neighbor and add its size
                    }
                }

                return size; // Return the total size of the connected region
            }

            // Traverse each cell of the matrix
            for (var i = 0; i < n; i++) {
                for (var j = 0; j < m; j++) {
                    // If the cell is filled (value is 1) and not yet visited
                    if (!visited[i, j] && matrix[i][j] == 1) {
                        var size = DFS(i, j); // Calculate the size of the connected region
                        MaxRegion = Math.Max(MaxRegion, size); // Update MaxRegion if this region is larger
                    }
                }
            }

            return MaxRegion; // Return the size of the largest connected region
        }

    }
}