using System;
using System.Collections.Generic;

namespace Code.algorithms
{
    // base class for common graph functionality
    public abstract class BaseGraph
    {
        protected readonly Dictionary<int, List<int>> AdjacencyList;

        protected BaseGraph()
        {
            AdjacencyList = new Dictionary<int, List<int>>();
        }

        public void AddEdge(int source, int destination, bool isDirected)
        {
            if (!AdjacencyList.ContainsKey(source))
            {
                AdjacencyList[source] = new List<int>();
            }
            AdjacencyList[source].Add(destination);

            if (!isDirected)
            {
                if (!AdjacencyList.ContainsKey(destination))
                {
                    AdjacencyList[destination] = new List<int>();
                }
                AdjacencyList[destination].Add(source);
            }
        }
    }

    // directed graph class for cycle detection
    public class DirectedGraph : BaseGraph
    {
        public bool HasCycle()
        {
            var visited = new HashSet<int>();
            var recursionStack = new HashSet<int>();

            foreach (var vertex in AdjacencyList.Keys)
            {
                if (!visited.Contains(vertex))
                {
                    if (DFS(vertex, visited, recursionStack))
                    {
                        return true; // cycle found
                    }
                }
            }

            return false; // no cycle found
        }

        private bool DFS(int vertex, HashSet<int> visited, HashSet<int> recursionStack)
        {
            visited.Add(vertex);
            recursionStack.Add(vertex);

            if (AdjacencyList.TryGetValue(vertex, out var neighbors))
            {
                foreach (var neighbor in neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        if (DFS(neighbor, visited, recursionStack))
                        {
                            return true; // cycle found
                        }
                    }
                    else if (recursionStack.Contains(neighbor))
                    {
                        return true; // cycle found
                    }
                }
            }

            recursionStack.Remove(vertex); // cacktrack
            return false;
        }
    }

    // undirected graph class for cycle detection
    public class UndirectedGraph : BaseGraph
    {
        public bool HasCycle()
        {
            var visited = new HashSet<int>();

            foreach (var vertex in AdjacencyList.Keys)
            {
                if (!visited.Contains(vertex))
                {
                    if (DFS(vertex, -1, visited)) // -1 denotes no parent
                    {
                        return true; // cycle found
                    }
                }
            }

            return false; // no cycle found
        }

        private bool DFS(int vertex, int parent, HashSet<int> visited)
        {
            visited.Add(vertex);

            if (AdjacencyList.TryGetValue(vertex, out var neighbors))
            {
                foreach (var neighbor in neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        if (DFS(neighbor, vertex, visited))
                        {
                            return true; // cycle found
                        }
                    }
                    else if (neighbor != parent) // cycle detected
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    // Example usage
    class Program
    {
        static void Main()
        {
            // ex: directed graph
            var directedGraph = new DirectedGraph();
            directedGraph.AddEdge(0, 1, isDirected: true);
            directedGraph.AddEdge(1, 2, isDirected: true);
            directedGraph.AddEdge(2, 0, isDirected: true); // creates a cycle
            directedGraph.AddEdge(2, 3, isDirected: true);

            Console.WriteLine("Directed graph contains cycle: " + directedGraph.HasCycle());

            // ex : undirected graph
            var undirectedGraph = new UndirectedGraph();
            undirectedGraph.AddEdge(0, 1, isDirected: false);
            undirectedGraph.AddEdge(1, 2, isDirected: false);
            undirectedGraph.AddEdge(2, 0, isDirected: false); // creates a cycle
            undirectedGraph.AddEdge(2, 3, isDirected: false);

            Console.WriteLine("Undirected graph contains cycle: " + undirectedGraph.HasCycle());
        }
    }
}
