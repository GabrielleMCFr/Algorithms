using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// Search Problem:
// - Start with a frontier that contains the initial state
// - Start with an empty explored set
// Repeat:
// - If the frontier is empty, there is no solution
// - Remove a node from the frontier
// - If the node contains the goal state, return the solution
// - Expand the node (get neighboring nodes)
//   - For each neighbor:
//     - If the neighbor is not in the frontier or the explored set:
//       - Add the neighbor to the frontier
//       - Add the neighbor to the explored set (mark as visited)

// DFS and BFS, the only real difference lies in which node is removed from the frontier first and the structure of the frontier.
// DFS (Depth-First Search) uses a stack as the frontier. Nodes are removed from the frontier in LIFO order (Last In, First Out).
// As a result, it explores as deeply as possible before backtracking.
// Time complexity: O(n + edges) for adjacency list, otherwise O(n²) for adjacency matrix.
// Non exhaustive usage cases : 
// - find a path in a unweighted graph, but not necessarily the shortest. It can be more efficient than BFS, good for cases where optimization is not key.
// - cycle detection
// - detect connected components
// - backtracking Problems
// - Perform exhaustive searches in decision-making algorithms.

// BFS (Breadth-First Search) is the opposite; it uses a queue as the frontier (FIFO: First In, First Out).
// This means it explores the shallowest nodes (closest to the starting node) first.
// Time complexity: O(n + edges) for adjacency list, otherwise O(n²) for adjacency matrix.
// Non exhaustive usage cases : 
// - find the shortest path in a unweighted graph
// - find people within a certain "degree" of connection
// - level order traversal 
// - solving shortest reach problems
public class Graph
{
    // Dict to store adjacency list of each vertex
    private Dictionary<int, List<int>> adjacencyList;

    public Graph()
    {
        adjacencyList = new Dictionary<int, List<int>>();
    }

    // adds an edge from vertex `source` to vertex `destination`
    public void AddEdge(int source, int destination)
    {
        if (!adjacencyList.ContainsKey(source))
            adjacencyList[source] = new List<int>();

        adjacencyList[source].Add(destination);

        // uncomment this line if you want the graph to be undirected
        // adjacencyList[destination].Add(source);
    }

    // Breadth-First Search 
    public void BFS(int startVertex)
    {
        // set to keep track of visited nodes
        HashSet<int> visited = new HashSet<int>();

        // queue to store the nodes to visit
        Queue<int> queue = new Queue<int>();

        // mark the start node as visited and enqueue it
        visited.Add(startVertex);
        queue.Enqueue(startVertex);

        Console.WriteLine("BFS Traversal:");

        // loop until there are nodes to process
        while (queue.Count > 0)
        {
            // dequeue a node from the front of the queue
            int vertex = queue.Dequeue();
            Console.Write(vertex + " ");

            // Get all adjacent vertices of the dequeued node
            if (adjacencyList.ContainsKey(vertex))
            {
                foreach (int neighbor in adjacencyList[vertex])
                {
                    // If a neighbor hasn't been visited, mark it as visited and enqueue it
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        Console.WriteLine();
    }

    // BFS in matrix
    static void BFS(List<List<char>> grid, int startRow, int startCol)
    {
        int rows = grid.Count;
        int cols = grid[0].Count;

        // directions for moving: up, down, left, right
        int[][] directions = new int[][]
        {
            new int[] {-1, 0}, // up
            new int[] {1, 0},  // down
            new int[] {0, -1}, // left
            new int[] {0, 1}   // right
        };

        // queue for BFS
        Queue<(int row, int col)> queue = new Queue<(int, int)>();

        // visited set
        HashSet<(int, int)> visited = new HashSet<(int, int)>();

        // enqueue the starting position and mark it as visited
        queue.Enqueue((startRow, startCol));
        visited.Add((startRow, startCol));

        Console.WriteLine("BFS Traversal:");

        while (queue.Count > 0)
        {
            // dequeue a cell
            var (row, col) = queue.Dequeue();
            Console.WriteLine($"Visiting: ({row}, {col})");

            // process neighbors
            foreach (var direction in directions)
            {
                int newRow = row + direction[0];
                int newCol = col + direction[1];

                // check bounds and whether the cell is unvisited and not blocked
                if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols &&
                    grid[newRow][newCol] != 'X' && !visited.Contains((newRow, newCol)))
                {
                    queue.Enqueue((newRow, newCol));
                    visited.Add((newRow, newCol));
                }
            }
        }
    }

    // Recursive Depth-First Search 
    public void DFS(int startVertex)
    {
        // set to keep track of visited nodes
        HashSet<int> visited = new HashSet<int>();

        Console.WriteLine("DFS Traversal:");

        // start DFS from the start vertex
        DFSUtil(startVertex, visited);
        Console.WriteLine();
    }

    // Helper method for DFS traversal using recursion
    private void DFSUtil(int vertex, HashSet<int> visited)
    {
        // mark the current node as visited and print it
        visited.Add(vertex);
        Console.Write(vertex + " ");

        // process all neighbors (adjacent vertices) recursively
        if (adjacencyList.ContainsKey(vertex))
        {
            foreach (int neighbor in adjacencyList[vertex])
            {
                // if a neighbor hasn't been visited, visit it recursively
                if (!visited.Contains(neighbor))
                {
                    DFSUtil(neighbor, visited);
                }
            }
        }
    }
}

public class Program {
    public static void Main()
    {
        // initialize a graph
        Graph graph = new Graph();

        // add edges to the graph
        graph.AddEdge(0, 1);
        graph.AddEdge(0, 2);
        graph.AddEdge(1, 2);
        graph.AddEdge(2, 0);
        graph.AddEdge(2, 3);
        graph.AddEdge(3, 3);

        // perform BFS traversal from vertex 2
        graph.BFS(2);

        // perform DFS traversal from vertex 2
        graph.DFS(2);
    }
}


