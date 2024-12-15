using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    // A*
    // Considers the cost to reach a node (g(n)) and the result of the heuristic (h(n)).
    // Expands the node with the smallest value of the cost function: f(n) = g(n) + h(n).
    // IMPORTANT: A* guarantees optimality only if:
    // - h(n) is admissible (it never overestimates the actual cost to the goal).
    // - h(n) is consistent (for every node n and its successor n' with step cost c: h(n) <= h(n') + c).
    // This implementation works for both spatial grids and weighted graphs.
    public class AStar
    {
        public class Node
        {
            public string Id;
            public int? X, Y; // coordinates for heuristics
            public int Weight = 1;      // default weight for custom costs
            public int GCost = int.MaxValue;  // cost from start node
            public int HCost = int.MaxValue;  // heuristic cost to goal
            public Node Parent;       // parent node for path reconstruction

            public int FCost => GCost + HCost; // total cost (f = g + h)

            public Node(string id, int? x = null, int? y = null, int weight = 1)
            {
                Id = id;
                X = x;
                Y = y;
                Weight = weight;
            }

            public override bool Equals(object obj)
            {
                return obj is Node other && Id == other.Id;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }

        public class Graph
        {
            public Dictionary<Node, List<(Node, int)>> AdjacencyList { get; } = new();

            public void AddNode(Node node)
            {
                if (!AdjacencyList.ContainsKey(node))
                    AdjacencyList[node] = new List<(Node, int)>();
            }

            public void AddEdge(Node source, Node target, int weight)
            {
                if (!AdjacencyList.ContainsKey(source))
                    AddNode(source);

                if (!AdjacencyList.ContainsKey(target))
                    AddNode(target);

                AdjacencyList[source].Add((target, weight));
            }
        }

        // main function to find the path from start to goal
        public List<Node> FindPath(Graph graph, Node start, Node goal)
        {
            var frontier = new PriorityQueue<Node, int>();
            var visited = new HashSet<Node>();

            // to track nodes in the frontier
            var inFrontier = new HashSet<Node>();

            // init start node
            start.GCost = 0;
            start.HCost = GetHeuristic(start, goal, "euclidean");
            frontier.Enqueue(start, start.FCost);
            inFrontier.Add(start);

            while (frontier.Count > 0)
            {
                // get the node with the lowest FCost
                var currentNode = frontier.Dequeue();
                inFrontier.Remove(currentNode);

                // if the goal is reached, reconstruct the path
                if (currentNode.Equals(goal))
                    return ReconstructPath(currentNode);

                // mark the current node as visited
                visited.Add(currentNode);

                // process each neighbor
                foreach (var (neighbor, edgeWeight) in graph.AdjacencyList[currentNode])
                {
                    // already evaluated, skip
                    if (visited.Contains(neighbor)) continue;

                    int tentativeGCost = currentNode.GCost + edgeWeight + neighbor.Weight;

                    // if a better path to the neighbor is found
                    if (tentativeGCost < neighbor.GCost)
                    {
                        neighbor.GCost = tentativeGCost;
                        neighbor.HCost = GetHeuristic(neighbor, goal);
                        neighbor.Parent = currentNode;

                        // Add the neighbor to the frontier if not already there
                        if (!inFrontier.Contains(neighbor))
                        {
                            frontier.Enqueue(neighbor, neighbor.FCost);
                            inFrontier.Add(neighbor);
                        }
                    }
                }
            }

            // no path found
            Console.WriteLine("No path found.");
            return null;
        }

        private int GetHeuristic(Node a, Node b, string heuristicType = "euclidean")
        {
            // for simplicity, if we have coordinates, we use them assuming, assuming a grid like graph, 
            // otherwise we treat it as a weighted graph without coordinates.
            // Note: for real-world applications, a heuristic could combine spatial coordinates and node weights
            // to model complex pathfinding scenarios (like a weighted terrain in a spatial grid).
            if (a.X.HasValue && a.Y.HasValue && b.X.HasValue && b.Y.HasValue)
            {
                switch (heuristicType.ToLower())
                {
                    case "manhattan":
                        return GetManhattanDistance(a, b);
                    case "euclidean":
                        return GetEuclideanDistance(a, b);
                    case "cosine":
                        return GetCosineDistance(a, b);
                    default:
                        throw new ArgumentException("Invalid heuristic type. Supported: 'manhattan', 'euclidean', 'cosine'.");
                }
            }

            // for non-spatial graphs, we use the node's weight as the heuristic value
            // this assumes that weights provide an indication of traversal cost
            return a.Weight;
        }

        // Manhattan distance: for grid-like graphs
        private int GetManhattanDistance(Node a, Node b)
        {
            return Math.Abs((int)a.X - (int)b.X) + Math.Abs((int)a.Y - (int)b.Y);
        }

        // L2 distance: for real-world graphs with continuous space
        private int GetEuclideanDistance(Node a, Node b)
        {
            return (int)Math.Sqrt(Math.Pow((int)a.X - (int)b.X, 2) + Math.Pow((int)a.Y - (int)b.Y, 2));
        }

        // Cosine similarity: For graphs with abstract or high-dimensional features
        private int GetCosineDistance(Node a, Node b)
        {
            double dotProduct = (int)a.X * (int)b.X + (int)a.Y * (int)b.Y;
            double magnitudeA = Math.Sqrt(Math.Pow((int)a.X, 2) + Math.Pow((int)a.Y, 2));
            double magnitudeB = Math.Sqrt(Math.Pow((int)b.X, 2) + Math.Pow((int)b.Y, 2));
            double cosineSimilarity = dotProduct / (magnitudeA * magnitudeB);

            // to avoid division by 0
            if (magnitudeA == 0 || magnitudeB == 0)
                return int.MaxValue; // Maximum distance for invalid nodes
    
            // convert similarity (range: -1 to 1) to a distance (range: 0 to 2)
            return (int)((1 - cosineSimilarity) * 100);
        }

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

    // Usage example
    class Program
    {
        static void Main()
        {

            // spatial example
            var graph = new AStar.Graph();

            var nodeA = new AStar.Node("A", 0, 0);
            var nodeB = new AStar.Node("B", 1, 0);
            var nodeC = new AStar.Node("C", 2, 0);
            var nodeD = new AStar.Node("D", 1, 1);
            var nodeE = new AStar.Node("E", 2, 1);

            graph.AddEdge(nodeA, nodeB, 1);
            graph.AddEdge(nodeA, nodeC, 4);
            graph.AddEdge(nodeB, nodeD, 2);
            graph.AddEdge(nodeC, nodeE, 3);
            graph.AddEdge(nodeD, nodeE, 1);

            var aStar = new AStar();
            var path = aStar.FindPath(graph, nodeA, nodeE);
            PrintPath(path, "Spatial graph");   

            // weighted example :
            var graphW = new Graph();

            var nodeAW = new Node("AW", weight: 1);
            var nodeBW = new Node("BW", weight: 3);
            var nodeCW = new Node("CW", weight: 1);
            var nodeDW = new Node("DW", weight: 2);
            var nodeEW = new Node("EW", weight: 1);

            graphW.AddEdge(nodeAW, nodeBW, 1);
            graphW.AddEdge(nodeAW, nodeCW, 4);
            graphW.AddEdge(nodeBW, nodeDW, 2);
            graphW.AddEdge(nodeCW, nodeEW, 3);
            graphW.AddEdge(nodeDW, nodeEW, 1);

            var pathW = aStar.FindPath(graphW, nodeAW, nodeEW);
            PrintPath(pathW, "Weighted graph");
        }

        private static void PrintPath(List<AStar.Node> path, string graphType)
        {
            if (path != null)
            {
                Console.WriteLine($"{graphType}, Path found:");
                foreach (var node in path)
                    Console.WriteLine(node.Id);
            }
            else
            {
                Console.WriteLine($"No path found for {graphType}.");
            }
        }
    }
}

