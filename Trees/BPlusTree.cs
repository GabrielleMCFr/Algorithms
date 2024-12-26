using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// This code implements a basic B+ tree structure, commonly used in databases and file systems.
// B+ Trees efficiently support sorted data and range queries while maintaining balanced tree properties.
public class BPlusTree
{
    private int Degree; // the maximum number of children per node
    public Node Root; // the root node of the tree

    public BPlusTree(int degree)
    {
        if (degree < 3) throw new ArgumentException("Degree must be at least 3."); // degree must be at least 3 for proper balancing
        Degree = degree;
        Root = new LeafNode(); // initialize root as a leaf node
    }

    public void Insert(int key, string value)
    {
        var root = Root.Insert(key, value, Degree); // delegate insertion to the root node
        if (root != null) Root = root; // update root if splitting caused a new root
    }

    public string Search(int key)
    {
        return Root.Search(key); // delegate search to the root node
    }

    public abstract class Node
    {
        public abstract Node Insert(int key, string value, int degree); // abstract method for inserting into a node
        public abstract string Search(int key); // abstract method for searching within a node

        public List<int> Keys = new List<int>(); // keys stored in the node
    }

    public class InternalNode : Node
    {
        public List<Node> Children = new List<Node>(); // child pointers for the internal node

        public override Node Insert(int key, string value, int degree)
        {
            // find the child to insert the key into
            int index = Keys.FindIndex(k => key < k);
            if (index == -1) index = Keys.Count;

            var child = Children[index].Insert(key, value, degree); // recursively insert into the child

            if (child != null) // check if the child was split
            {
                // insert the promoted key from the split into the current node
                Keys.Insert(index, child.Keys[0]);
                Children.Insert(index + 1, child);

                if (Keys.Count >= degree) // if the current node is full, split it
                {
                    var newInternalNode = new InternalNode();
                    int mid = degree / 2; // calculate the split point

                    // move half the keys and children to the new node
                    newInternalNode.Keys.AddRange(Keys.GetRange(mid + 1, Keys.Count - mid - 1));
                    newInternalNode.Children.AddRange(Children.GetRange(mid + 1, Children.Count - mid - 1));

                    Keys.RemoveRange(mid, Keys.Count - mid);
                    Children.RemoveRange(mid + 1, Children.Count - mid - 1);

                    return newInternalNode; // return the new node to be promoted
                }
            }

            return null; // no split occurred
        }

        public override string Search(int key)
        {
            // find the appropriate child to search
            int index = Keys.FindIndex(k => key < k);
            if (index == -1) index = Keys.Count;
            return Children[index].Search(key); // delegate search to the child
        }
    }

    class LeafNode : Node
    {
        public List<int> Keys = new List<int>(); // keys in the leaf node
        public List<string> Values = new List<string>(); // values corresponding to the keys
        public LeafNode Next; // pointer to the next leaf node for range queries

        public override Node Insert(int key, string value, int degree)
        {
            // find the position to insert the new key
            int index = Keys.BinarySearch(key);
            if (index < 0) index = ~index;

            Keys.Insert(index, key); // insert the key
            Values.Insert(index, value); // insert the value

            if (Keys.Count >= degree) // if the leaf is full, split it
            {
                var newLeafNode = new LeafNode();
                int mid = degree / 2; // calculate the split point

                // move half the keys and values to the new leaf
                newLeafNode.Keys.AddRange(Keys.GetRange(mid, Keys.Count - mid));
                newLeafNode.Values.AddRange(Values.GetRange(mid, Values.Count - mid));

                Keys.RemoveRange(mid, Keys.Count - mid);
                Values.RemoveRange(mid, Values.Count - mid);

                newLeafNode.Next = Next; // link the new leaf to the next leaf
                Next = newLeafNode; // update the current leaf's next pointer

                // promote the first key of the new leaf node
                var newInternalNode = new InternalNode();
                newInternalNode.Keys.Add(newLeafNode.Keys[0]);
                newInternalNode.Children.Add(this);
                newInternalNode.Children.Add(newLeafNode);

                return newInternalNode; // return the new internal node
            }

            return null; // no split occurred
        }

        public override string Search(int key)
        {
            // find the index of the key in the leaf
            int index = Keys.IndexOf(key);
            return index != -1 ? Values[index] : null; // return the value if found, otherwise null
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // create a B+ tree with a degree of 3
        var tree = new BPlusTree(3);

        tree.Insert(10, "Value10");
        tree.Insert(20, "Value20");
        tree.Insert(5, "Value5");
        tree.Insert(15, "Value15");
        tree.Insert(25, "Value25");

        // search for keys
        Console.WriteLine(tree.Search(10)); // output: Value10
        Console.WriteLine(tree.Search(20)); // output: Value20
        Console.WriteLine(tree.Search(30)); // output: null (not found)
    }
}