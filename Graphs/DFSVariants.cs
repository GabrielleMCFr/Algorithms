using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

class DFSVariants
{
    // Graph represented as an adjacency list
    static Dictionary<int, List<(int, int)>> graph = new Dictionary<int, List<(int, int)>>();

    static void Main(string[] args)
    {
        // Example tree structure
        AddEdge(1, 2, 1);  // Add edge (1 <-> 2) with weight 1
        AddEdge(1, 3, 1);
        AddEdge(2, 4, 1);
        AddEdge(2, 5, 1);
        AddEdge(3, 6, 1);
        AddEdge(6, 7, 1);

        // Call DFS
        Console.WriteLine("DFS:");
        HashSet<int> visited = new HashSet<int>();
        DFS(1, -1, visited);

        // Call Post-Order DFS
        Console.WriteLine("\nPost-order DFS:");
        int subtreeSize = PostOrderDFS(1, -1);
        Console.WriteLine($"Subtree size of the entire tree: {subtreeSize}");

        // Call Two-Pass DFS
        Console.WriteLine("\nTwo-pass DFS:");
        int diameter = TwoPassDFS(1);
        Console.WriteLine($"Diameter of the tree: {diameter}");
    }

    static void AddEdge(int u, int v, int weight)
    {
        if (!graph.ContainsKey(u)) graph[u] = new List<(int, int)>();
        if (!graph.ContainsKey(v)) graph[v] = new List<(int, int)>();
        graph[u].Add((v, weight));
        graph[v].Add((u, weight));
    }

    // **DFS**: General traversal
    static void DFS(int node, int parent, HashSet<int> visited)
    {
        Console.WriteLine($"Visiting node: {node}"); // the node is processed (here dummy action like printing) before it's children.
        visited.Add(node);

        foreach (var (neighbor, weight) in graph[node])
        {
            if (neighbor != parent && !visited.Contains(neighbor))
            {
                DFS(neighbor, node, visited);
            }
        }
    }

    // **Post-Order DFS**: Process children before the parent
    // Example: here Calculate the size of each subtree,
    // others usage : finding Lowest Common Ancestors (LCA), or evaluating a tree-based expression...
    static int PostOrderDFS(int node, int parent)
    {
        int subtreeSize = 1;  // Count the current node

        foreach (var (neighbor, weight) in graph[node])
        {
            if (neighbor != parent)
            {
                // Recursively calculate the size of the child subtree
                subtreeSize += PostOrderDFS(neighbor, node);
            }
        }

        // After processing all children, return the size of the subtree
        // The node is processed after its children.
        Console.WriteLine($"Node {node}, Subtree size: {subtreeSize}"); 
        return subtreeSize;
    }

    // **Two-pass DFS**: Find the diameter of the tree (path between the two farthest nodes in the tree
    static int TwoPassDFS(int startNode)
    {
        // first DFS to find the farthest node from the start
        (int farthestNode, int _) = FarthestNodeDFS(startNode, -1, 0);

        // second DFS from the farthest node to calculate the diameter
        (int _, int diameter) = FarthestNodeDFS(farthestNode, -1, 0);

        return diameter;
    }

    // helper function for two-pass DFS to find the farthest node and its distance
    static (int farthestNode, int maxDistance) FarthestNodeDFS(int node, int parent, int distance)
    {
        int farthestNode = node;
        int maxDistance = distance;

        foreach (var (neighbor, weight) in graph[node])
        {
            if (neighbor != parent)
            {
                // recursive DFS to find the farthest node
                var (nextFarthest, nextDistance) = FarthestNodeDFS(neighbor, node, distance + weight);

                if (nextDistance > maxDistance)
                {
                    farthestNode = nextFarthest;
                    maxDistance = nextDistance;
                }
            }
        }

        return (farthestNode, maxDistance);
    }
}