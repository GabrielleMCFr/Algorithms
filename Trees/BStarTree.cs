using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// This code implements a basic B* tree structure, an extension of B-trees.
// B* trees provide better space utilization by redistributing keys between sibling nodes when possible,
// and they are commonly used in file systems and databases for efficient data storage and retrieval.
public class BStarTree
{
    private int Degree; // the degree of the B* tree, determining the maximum number of children per node
    public Node Root; // the root node of the tree

    public BStarTree(int degree)
    {
        Degree = degree; // set the degree of the tree
        Root = new Node(); // initialize the root as an empty node
    }

    public void Insert(int key)
    {
        // if the root node is full, split it and create a new root
        if (Root.Keys.Count == Degree - 1)
        {
            var newRoot = new Node();
            newRoot.Children.Add(Root); // make the current root a child of the new root
            newRoot.SplitChild(0, Root, Degree); // split the full root node
            Root = newRoot; // update the root pointer
        }
        Root.InsertNonFull(key, Degree); // insert the key into the tree
    }

    public bool Search(int key)
    {
        return Root.Search(key); // delegate search operation to the root node
    }

    public class Node
    {
        public List<int> Keys = new List<int>(); // keys stored in the node
        public List<Node> Children = new List<Node>(); // child pointers for the node

        public void InsertNonFull(int key, int degree)
        {
            // find the position to insert the new key
            int index = Keys.Count - 1;
            if (Children.Count == 0) // if the node is a leaf
            {
                while (index >= 0 && key < Keys[index])
                {
                    index--;
                }
                Keys.Insert(index + 1, key); // insert the key into the correct position
            }
            else // if the node is an internal node
            {
                while (index >= 0 && key < Keys[index])
                {
                    index--;
                }
                index++;
                // check if the child is full
                if (Children[index].Keys.Count == degree - 1)
                {
                    SplitChild(index, Children[index], degree); // split the full child node
                    if (key > Keys[index]) index++; // adjust index if necessary
                }
                Children[index].InsertNonFull(key, degree); // recursively insert into the child
            }
        }

        public void SplitChild(int index, Node child, int degree)
        {
            var newNode = new Node(); // create a new node to hold half of the keys and children
            int mid = degree / 2;

            // move keys and children from the full child to the new node
            newNode.Keys.AddRange(child.Keys.GetRange(mid + 1, child.Keys.Count - mid - 1));
            if (child.Children.Count > 0)
            {
                newNode.Children.AddRange(child.Children.GetRange(mid + 1, child.Children.Count - mid - 1));
            }

            Keys.Insert(index, child.Keys[mid]); // promote the middle key to the parent
            Children.Insert(index + 1, newNode); // link the new node to the parent

            child.Keys.RemoveRange(mid, child.Keys.Count - mid); // remove the moved keys from the child
            if (child.Children.Count > 0)
            {
                child.Children.RemoveRange(mid + 1, child.Children.Count - mid - 1); // remove the moved children
            }
        }

        public bool Search(int key)
        {
            // find the position of the key or the child to search
            int index = Keys.FindIndex(k => k >= key);
            if (index >= 0 && index < Keys.Count && Keys[index] == key)
                return true; // key found in the current node

            if (Children.Count == 0)
                return false; // no children, key not found

            if (index < 0)
                index = 0; // adjust index to the first child if not found

            if (index >= Children.Count)
                index = Children.Count - 1; // adjust index to the last child if out of bounds

            return Children[index].Search(key); // recursively search in the appropriate child
        }

    }
}


class Program
{
    static void Main(string[] args)
    {
        // create a B* tree with a degree of 4
        var tree = new BStarTree(4);

        // insert some keys
        tree.Insert(10);
        tree.Insert(20);
        tree.Insert(5);
        tree.Insert(15);
        tree.Insert(25);

        // search for keys and print results
        Console.WriteLine(tree.Search(10)); // output: True
        Console.WriteLine(tree.Search(20)); // output: True
        Console.WriteLine(tree.Search(30)); // output: False
    }
}