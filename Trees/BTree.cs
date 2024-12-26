using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// This code implements a basic B-tree structure, commonly used in databases and file systems.
// B-trees are designed to maintain sorted data and support efficient insertion, deletion, and search operations.
public class BTree
{
    private int Degree; // the degree of the B-tree, determining the maximum number of children per node
    public Node Root; // the root node of the tree

    public BTree(int degree)
    {
        Degree = degree; // set the degree of the tree
        Root = new Node(true); // initialize the root as a leaf node
    }

    public void Insert(int key)
    {
        Node root = Root;
        // if the root node is full, split it and create a new root
        if (root.Keys.Count == 2 * Degree - 1)
        {
            Node newRoot = new Node(false);
            newRoot.Children.Add(root); // make the current root a child of the new root
            newRoot.SplitChild(0, root, Degree); // split the full root node
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
        public bool IsLeaf; // indicates whether the node is a leaf

        public Node(bool isLeaf)
        {
            IsLeaf = isLeaf; // initialize the node as a leaf or internal node
        }

        public void InsertNonFull(int key, int degree)
        {
            int i = Keys.Count - 1;

            if (IsLeaf)
            {
                // insert the key in the correct position in the leaf node
                while (i >= 0 && Keys[i] > key)
                {
                    i--;
                }
                Keys.Insert(i + 1, key); // insert the key into the leaf node
            }
            else
            {
                // find the child that will receive the key
                while (i >= 0 && Keys[i] > key)
                {
                    i--;
                }
                i++;

                // if the child is full, split it
                if (Children[i].Keys.Count == 2 * degree - 1)
                {
                    SplitChild(i, Children[i], degree);

                    // adjust index if the split moves the target key to the right
                    if (Keys[i] < key)
                    {
                        i++;
                    }
                }
                Children[i].InsertNonFull(key, degree); // recursively insert into the child
            }
        }

        public void SplitChild(int index, Node child, int degree)
        {
            Node newChild = new Node(child.IsLeaf); // create a new node to hold half of the keys and children
            int mid = degree - 1; // calculate the middle index

            // move the last (degree - 1) keys from the child to the new child
            for (int j = 0; j < mid; j++)
            {
                newChild.Keys.Add(child.Keys[mid + 1]);
                child.Keys.RemoveAt(mid + 1);
            }

            // if not a leaf, move the last (degree) children to the new child
            if (!child.IsLeaf)
            {
                for (int j = 0; j < degree; j++)
                {
                    newChild.Children.Add(child.Children[mid + 1]);
                    child.Children.RemoveAt(mid + 1);
                }
            }

            // insert the middle key of the child into the parent node
            Keys.Insert(index, child.Keys[mid]);
            child.Keys.RemoveAt(mid); // remove the promoted key from the child
            Children.Insert(index + 1, newChild); // link the new child to the parent
        }

        public bool Search(int key)
        {
            int i = 0;
            // find the smallest index where key <= Keys[i]
            while (i < Keys.Count && key > Keys[i])
            {
                i++;
            }

            // check if the key is present in the current node
            if (i < Keys.Count && Keys[i] == key)
            {
                return true; // key found
            }

            // if the node is a leaf, the key is not present
            if (IsLeaf)
            {
                return false; // key not found in a leaf
            }

            // recursively search in the appropriate child node
            return Children[i].Search(key);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // create a B-tree with a degree of 3
        var tree = new BTree(3);

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