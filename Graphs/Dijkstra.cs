using System;
using System.Collections.Generic;

namespace Code.algorithms;

// Dijkstra's algorithm
// This is an informed search. 
// Non exhaustive usage cases : find the shortest path or most efficient path in a weighted graph structure but the weights must be positive.
// alternatives for special cases : 
// - negative weights : use Bellman-Ford algorithm.
// - multiple Source-Target Queries: use Floyd-Warshall or Johnson's algorithm.
// - unweighted Graphs: Use BFS
// Time complexity: O((n+e)logn)
public class Dijkstra
{
    // Node class represents each vertex in the graph
    public class Node
    {
        public int Id;                                
        public Dictionary<Node, int> Neighbors;       
        public int GCost = int.MaxValue;              
        public Node Parent;                          

        public Node(int id)
        {
            Id = id;
            Neighbors = new Dictionary<Node, int>(); 
        }
    }

    public List<Node> FindShortestPath(Node start, Node goal)
    {
        var frontier = new PriorityQueue<Node, int>(); // priority queue for nodes
        var visited = new HashSet<Node>();            // set of visited nodes

        start.GCost = 0;                              
        frontier.Enqueue(start, start.GCost);         

        while (frontier.Count > 0)
        {
            var currentNode = frontier.Dequeue();     // get node with lowest GCost

            // goal reached: reconstruct and return the path
            if (currentNode == goal)
                return ReconstructPath(currentNode);

            visited.Add(currentNode);

            foreach (var neighbor in currentNode.Neighbors)
            {
                Node neighborNode = neighbor.Key;
                int edgeWeight = neighbor.Value;

                if (edgeWeight < 0)
                    throw new InvalidOperationException("Graph contains negative weights, which are not supported by Dijkstra's algorithm.");

                // skip if neighbor is already visited
                if (visited.Contains(neighborNode)) continue;

                int tentativeGCost = currentNode.GCost + edgeWeight;

                // update neighbor's cost and parent if a shorter path is found
                if (tentativeGCost < neighborNode.GCost)
                {
                    neighborNode.GCost = tentativeGCost;
                    neighborNode.Parent = currentNode;

                    // enqueue neighbor with updated cost
                    frontier.Enqueue(neighborNode, neighborNode.GCost);
                }
            }
        }

        // return null if no path is found from start to goal
        return null;
    }

    // helper function to reconstruct the path from the goal to the start node
    private List<Node> ReconstructPath(Node node)
    {
        var path = new List<Node>();
        while (node != null)
        {
            path.Add(node);
            node = node.Parent;
        }
        path.Reverse(); 
        return path;
    }
}

// Usage
class Program {
    static void Main()
    {
        // create nodes (for simplicity, we use integer IDs)
        var nodeA = new Dijkstra.Node(0);
        var nodeB = new Dijkstra.Node(1);
        var nodeC = new Dijkstra.Node(2);
        var nodeD = new Dijkstra.Node(3);
        var nodeE = new Dijkstra.Node(4);

        // define weighted edges (connecting nodes with specified weights)
        nodeA.Neighbors[nodeB] = 4;  // Edge A-B with weight 4
        nodeA.Neighbors[nodeC] = 1;  // Edge A-C with weight 1
        nodeB.Neighbors[nodeD] = 1;  // Edge B-D with weight 1
        nodeC.Neighbors[nodeB] = 2;  // Edge C-B with weight 2
        nodeC.Neighbors[nodeD] = 5;  // Edge C-D with weight 5
        nodeD.Neighbors[nodeE] = 3;  // Edge D-E with weight 3

        // init Dijkstra and find the shortest path
        var dijkstra = new Dijkstra();
        List<Dijkstra.Node> path = dijkstra.FindShortestPath(nodeA, nodeE);

        // Print the path
        if (path != null)
        {
            Console.WriteLine("Shortest path:");
            Console.WriteLine(string.Join(" -> ", path.ConvertAll(node => $"Node {node.Id}")));
        }
        else
        {
            Console.WriteLine("No path found.");
        }
    }
}
