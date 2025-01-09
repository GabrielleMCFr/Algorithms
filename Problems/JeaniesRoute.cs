using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    /*
    We want to visit some nodes (represented in this problem by cities) in a bigger tree. We can pass through other nodes or not.
    It's a variation of the travelling salesman problem, but with trees. ( a steiner tree problem)
    Since it's trees, we know there is n - 1 edges, and that there are no cycles.
    We need to 1, find the minimal subtree connecting all the nodes we need to visit to prune unecessary nodes
    2, find their LCA,
    3, find the distance from all these nodes to the LCA. This distance * 2 (go and back) is the maximum route jeanie can take. 
    4, to find the minimum route, we need to find which two nodes are the furthest away from each other, calculate the cost 
       of the distance between them, and substract it from the maximum route to avoid to count it twice. It means we start from one 
       of these two nodes and will end in the other.
    */
    public class JeaniesRoute
    {
        // Represents an edge between two nodes with a specific cost
        class Edge {
            public int Cost { get; set; }
            public int Node { get; set; }
        }
        
        // Represents a node in the graph with its edges, parent, and cost to root
        class Node {
            public List<Edge> Edges { get; set; }
            public int ParentNode { get; set; }
            public int CostToRootNode { get; set; }
        }
        
        static int JeaniesRoute(int[] nodesToVisit, int[][] edges) {
            if (nodesToVisit.Count() <= 1) {
                return 0; // If there's only one node to visit, no distance is needed
            }
            
            // Create an array of nodes (graph representation)
            var graph = new Node[edges.Count() + 1];
            
            for (var i = 0; i < graph.Count(); ++i) {
                graph[i] = new Node {
                    Edges = new List<Edge>()
                };
            }

            // Build the adjacency list for the graph from the edges input
            foreach (var edge in edges) {
                var u = edge[0] - 1; // Convert 1-based to 0-based indexing
                var v = edge[1] - 1;
                var d = edge[2];
                
                graph[u].Edges.Add(new Edge {
                    Node = v,
                    Cost = d
                });
                
                graph[v].Edges.Add(new Edge {
                    Node = u,
                    Cost = d
                });
            }
            
            // Convert nodesToVisit to 0-based indexing
            for (var i = 0; i < nodesToVisit.Count(); ++i) {
                --nodesToVisit[i];
            }

            // Convert nodesToVisit into a set for quick lookup
            var nodesToVisitSet = new HashSet<int>(nodesToVisit);

            // Find an internal node to act as the root for the pruned subtree
            var rootNode = FindInternalNode(graph, nodesToVisitSet);
            
            // Prune the graph to keep only necessary nodes (those in nodesToVisit)
            RemoveUnnecessaryNodes(graph, nodesToVisitSet, rootNode);

            // Calculate the cost from each node to the root node
            CalculateCostToRootNode(graph, rootNode);
            
            // Calculate the total cost of the edges in the pruned tree
            var traversedEdgesCost = CalculateTraversedEdgesCost(graph, rootNode);
            
            // Find the furthest node from the root to determine the start of the route
            var startNode = nodesToVisit[0];       
            foreach (var node in nodesToVisit) {
                if (graph[node].CostToRootNode > graph[startNode].CostToRootNode) {
                    startNode = node;
                }
            }
            
            // Recalculate the cost from the new start node
            RecalculateAffectedCostToRootNode(graph, rootNode, startNode);
            
            // Find the furthest node from the start node to determine the end of the route
            var endNode = rootNode;
            foreach (var node in nodesToVisit) {
                if (graph[node].CostToRootNode > graph[endNode].CostToRootNode) {
                    endNode = node;
                }
            }

            // Total distance = 2 * sum of traversed edges - (distance between start and end nodes)
            return 2 * traversedEdgesCost + graph[startNode].CostToRootNode - graph[endNode].CostToRootNode;
        }
        
        // Recalculate the cost to root for affected nodes
        static void RecalculateAffectedCostToRootNode(Node[] graph, int rootNode, int startNode) {
            RecalculateAffectedCostToRootNodeImpl(graph, rootNode, startNode, -1);
        }
        
        static void RecalculateAffectedCostToRootNodeImpl(Node[] graph, int rootNode, int currentNode, int previousNode) {
            if (currentNode == rootNode) {
                return; // Stop if we reach the root
            }
            
            // Negate the cost to root for recalculating from the new start node
            graph[currentNode].CostToRootNode *= -1;
            
            // Recalculate costs for all connected nodes
            foreach (var edge in graph[currentNode].Edges.Where(e => e.Node != previousNode)) {
                CalculateCostToRootNodeImpl(graph, edge.Node, graph[currentNode].CostToRootNode + edge.Cost);
            }
            
            // Continue recalculating for the parent node
            RecalculateAffectedCostToRootNodeImpl(graph, rootNode, graph[currentNode].ParentNode, currentNode);
        }
        
        // Calculates the cost from each node to the root node
        static void CalculateCostToRootNode(Node[] graph, int rootNode) {
            CalculateCostToRootNodeImpl(graph, rootNode, 0);
        }
        
        static void CalculateCostToRootNodeImpl(Node[] graph, int node, int cost) {
            graph[node].CostToRootNode = cost; // Update the cost for this node
            
            // Recursively calculate the cost for child nodes
            foreach (var edge in graph[node].Edges) {
                CalculateCostToRootNodeImpl(graph, edge.Node, cost + edge.Cost);
            }
        }
        
        // Calculate the sum of all traversed edges in the tree
        static int CalculateTraversedEdgesCost(Node[] graph, int rootNode) {
            return graph[rootNode].Edges.Sum(e => e.Cost + CalculateTraversedEdgesCost(graph, e.Node));
        }
        
        // Remove nodes that are not required (not in nodesToVisit)
        static void RemoveUnnecessaryNodes(Node[] graph, HashSet<int> nodesToVisit, int rootNode) {
            RemoveUnnecessaryNodesImpl(graph, nodesToVisit, rootNode, -1);
        }
        
        static bool RemoveUnnecessaryNodesImpl(Node[] graph, HashSet<int> nodesToVisit, int currentNode, int parentNode) {
            var edgesToKeep = new List<Edge>();
            
            // Recursively check child nodes
            foreach (var edge in graph[currentNode].Edges.Where(e => e.Node != parentNode)) {
                if (RemoveUnnecessaryNodesImpl(graph, nodesToVisit, edge.Node, currentNode)) {
                    edgesToKeep.Add(edge);
                }
            }
            
            // Keep only necessary edges
            graph[currentNode].Edges = edgesToKeep;
            graph[currentNode].ParentNode = parentNode;
            
            // Return true if the node is required or has a required descendant
            return edgesToKeep.Any() || nodesToVisit.Contains(currentNode);
        }
        
        // Find an internal node that can act as the root for the subtree
        static int FindInternalNode(Node[] graph, HashSet<int> nodesToVisit) {
            return FindInternalNodeImpl(graph, nodesToVisit, 0, -1);
        }

        static int FindInternalNodeImpl(Node[] graph, HashSet<int> nodesToVisit, int currentNode, int parentNode) {
            foreach (var childNode in graph[currentNode].Edges.Select(e => e.Node).Where(n => n != parentNode)) {
                var internalNode = FindInternalNodeImpl(graph, nodesToVisit, childNode, currentNode);
                
                if (internalNode != -1) {
                    return internalNode;
                }
            }
            
            // Return the parent if this is a required node
            return nodesToVisit.Contains(currentNode) ? parentNode : -1;
        }
    }
}