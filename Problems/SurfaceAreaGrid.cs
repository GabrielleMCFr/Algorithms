using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms.Problems
{
    public class SurfaceAreaGrid
    {
    // caculate the surface area of cubes of 1*1*1 stacked on a grid in piles to form a shape.
    public static int surfaceArea(List<List<int>> A)
    {
         int rows = A.Count;
        int cols = A[0].Count;
        int surface3d = 0;

    // Iterate through each cell in the grid
    for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // Current height at cell (i, j) (since cubes are 1*1*1, else you would multiply by the height)
                int height = A[i][j];

                // Add top and bottom surfaces (always visible) (1*1 surface * 2)
                surface3d += 2;

                // Add vertical sides
                // If no neighbor exists, the whole side is exposed.
                // If a neighbor exists, subtract the overlap.
                surface3d += (i == 0 ? height : Math.Max(0, height - A[i - 1][j])); // Above
                surface3d += (i == rows - 1 ? height : Math.Max(0, height - A[i + 1][j])); // Below
                surface3d += (j == 0 ? height : Math.Max(0, height - A[i][j - 1])); // Left
                surface3d += (j == cols - 1 ? height : Math.Max(0, height - A[i][j + 1])); // Right
            }
        }

        return surface3d;
        }
    }
}