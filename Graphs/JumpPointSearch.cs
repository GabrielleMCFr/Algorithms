using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// Jump Point Search (JPS) is an optimization of the A* search algorithm designed to speed up pathfinding on uniform-cost grids
//  (e.g., grids where movement costs are constant in all directions). It reduces the number of nodes explored by "jumping" 
//  over unnecessary nodes.
// How it works:
// Instead of exploring all neighbors of a node, JPS "jumps" in a specific direction until it finds:
// A jump point (a node that must be explored).
// An obstacle or the boundary of the grid.
// The goal node.
// Pruning:
// JPS prunes unnecessary nodes by skipping over intermediate nodes that do not influence the shortest path.
// Jump Points:
// A jump point is a node that:
//  - Forces a change in direction (e.g., near an obstacle or at a corner).
//  - Is part of the shortest path to the goal.
// Only these jump points are added to the open list for further exploration.
//
// Basically, it guarantees the same optimal path as A*, but explore fewer nodes. 
// BUT NOTE : It requires a grid, not any graph, and the grid must have uniform costs.
// Time complexity : best case O(d) (here d is the manhattan distance between the start and goal points),
// at worse O(n*m) like A* since JPS does not guarantee skipping nodes in highly constrained environments.
public class JumpPointSearch
{
    // directions: {dx, dy} for 4 cardinal and 4 diagonal directions
    private static int[,] directions =
    {
        {-1, 0}, {1, 0}, {0, -1}, {0, 1}, // cardinal directions
        {-1, -1}, {-1, 1}, {1, -1}, {1, 1} // diagonal directions
    };

    public static List<(int, int)> JPS(int[,] grid, (int, int) start, (int, int) goal)
    {
        // openList stores the nodes to explore, prioritized by their fScore
        var openList = new PriorityQueue<(int, int), int>();
        // gScore stores the cost of the shortest known path to a node
        var gScore = new Dictionary<(int, int), int>();
        // cameFrom maps each node to its parent node in the path
        var cameFrom = new Dictionary<(int, int), (int, int)>();
        
        gScore[start] = 0;
        openList.Enqueue(start, Heuristic(start, goal));

        // main search loop
        while (openList.Count > 0)
        {
            // get the node with the lowest fScore
            var current = openList.Dequeue();
            // if we reached the goal, reconstruct and return the path
            if (current == goal)
            {
                return SimplifyPath(ReconstructPath(cameFrom, current));
            }

            // for each direction, attempt to find a jump point
            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int[] direction = { directions[i, 0], directions[i, 1] };
                var jumpPoint = Jump(grid, current, direction, goal);
                if (jumpPoint != null)
                {
                    var jp = jumpPoint.Value;
                    int tentativeG = gScore[current] + Distance(current, jp);

                    // if the jump point is new or we found a better path to it
                    if (!gScore.ContainsKey(jp) || tentativeG < gScore[jp])
                    {
                        gScore[jp] = tentativeG;
                        int fScore = tentativeG + Heuristic(jp, goal);
                        openList.Enqueue(jp, fScore);
                        cameFrom[jp] = current;
                    }
                }
            }
        }

        // return an empty path if no path is found
        return new List<(int, int)>();
    }

    private static (int, int)? Jump(int[,] grid, (int, int) current, int[] direction, (int, int) goal)
    {
        int x = current.Item1 + direction[0];
        int y = current.Item2 + direction[1];

        // stop if out of bounds or at an obstacle
        if (!IsValid(grid, x, y))
        {
            return null;
        }

        // return the goal if we reach it
        if ((x, y) == goal)
        {
            return (x, y);
        }

        // check if the current node is a jump point
        if (IsJumpPoint(grid, (x, y), direction))
        {
            return (x, y);
        }

        // for diagonal movement, ensure adjacent cells are passable
        if (direction[0] != 0 && direction[1] != 0)
        {
            if (!IsValid(grid, x - direction[0], y) || !IsValid(grid, x, y - direction[1]))
            {
                return null;
            }
        }

        // recursively jump in the current direction
        return Jump(grid, (x, y), direction, goal);
    }

    private static bool IsJumpPoint(int[,] grid, (int, int) current, int[] direction)
    {
        int x = current.Item1;
        int y = current.Item2;

        // check for forced neighbors that indicate a jump point
        if (direction[0] != 0 && direction[1] != 0)
        {
            return (x - direction[0] >= 0 && y + direction[1] < grid.GetLength(1) && grid[x - direction[0], y] == 1 && grid[x, y + direction[1]] == 0) ||
                   (x + direction[0] < grid.GetLength(0) && y - direction[1] >= 0 && grid[x, y - direction[1]] == 1 && grid[x + direction[0], y] == 0);
        }

        // horizontal or vertical forced neighbors
        return (direction[0] == 0 && ((x - 1 >= 0 && grid[x - 1, y] == 1 && grid[x - 1, y + direction[1]] == 0) ||
                                      (x + 1 < grid.GetLength(0) && grid[x + 1, y] == 1 && grid[x + 1, y + direction[1]] == 0))) ||
               (direction[1] == 0 && ((y - 1 >= 0 && grid[x, y - 1] == 1 && grid[x + direction[0], y - 1] == 0) ||
                                      (y + 1 < grid.GetLength(1) && grid[x, y + 1] == 1 && grid[x + direction[0], y + 1] == 0)));
    }

    private static bool IsValid(int[,] grid, int x, int y)
    {
        // return true if the cell is within bounds and not an obstacle
        return x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1) && grid[x, y] == 0;
    }

    private static int Heuristic((int, int) a, (int, int) b)
    {
        // calculate manhattan distance between two points
        return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
    }

    private static int Distance((int, int) a, (int, int) b)
    {
        // calculate the distance between two points
        return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
    }

    private static List<(int, int)> ReconstructPath(Dictionary<(int, int), (int, int)> cameFrom, (int, int) current)
    {
        // reconstruct the path from the goal to the start
        var path = new List<(int, int)>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Add(current);
        path.Reverse();
        return path;
    }

    private static List<(int, int)> SimplifyPath(List<(int, int)> path)
    {
        // remove intermediate points in straight-line segments
        var simplifiedPath = new List<(int, int)>();
        if (path.Count == 0) return simplifiedPath;

        simplifiedPath.Add(path[0]);
        for (int i = 1; i < path.Count - 1; i++)
        {
            if (!IsStraightLine(simplifiedPath[^1], path[i], path[i + 1]))
            {
                simplifiedPath.Add(path[i]);
            }
        }
        simplifiedPath.Add(path[^1]);
        return simplifiedPath;
    }

    private static bool IsStraightLine((int, int) a, (int, int) b, (int, int) c)
    {
        // check if three points lie on a straight line
        return (b.Item1 - a.Item1) * (c.Item2 - b.Item2) ==
               (b.Item2 - a.Item2) * (c.Item1 - b.Item1);
    }

    public static void Main(string[] args)
    {
        // test grid: 0 = empty, 1 = obstacle
        int[,] grid =
        {
            { 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 0 },
            { 0, 0, 0, 1, 0 },
            { 0, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 0 }
        };

        var start = (0, 0);
        var goal = (0, 4);

        var path = JPS(grid, start, goal);

        if (path.Count == 0)
        {
            Console.WriteLine("no path found.");
        }
        else
        {
            Console.WriteLine("path:");
            foreach (var point in path)
            {
                Console.WriteLine($"({point.Item1}, {point.Item2})");
            }
        }
    }
}
