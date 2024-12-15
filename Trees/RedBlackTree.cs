using System;
using System.Collections.Generic;

namespace Code.algorithms
{
    public enum Color
    {
        Red,
        Black
    }

    public class RedBlackNode
    {
        public int Value;
        public Color NodeColor;
        public RedBlackNode Left;
        public RedBlackNode Right;
        public RedBlackNode Parent;

        public RedBlackNode(int value)
        {
            Value = value;
            NodeColor = Color.Red;
            Left = Right = Parent = null;
        }
    }

    // Red-Black trees:
    // Red-Black trees are self-balancing BST that ensure the tree remains approximately balanced,
    // guaranteeing efficient operations (O(log n)) for search, insertion, and deletion. 
    // they are used in scenarios where frequent insertions and deletions are required while maintaining a balanced structure.
    // Time Complexity:
    // - Search: O(log n)
    // - Insertion: O(log n)
    // - Deletion: O(log n)

    // Notes:
    // Every node is either red or black.
    // The root is always black.
    // Red nodes cannot have red children (no two consecutive red nodes).
    // Every path from a node to its descendants contains the same number of black nodes (black-height property).
    public class RedBlackTree
    {
        private RedBlackNode root;

        public RedBlackNode GetRoot() => root;

        public void Insert(int value)
        {
            RedBlackNode newNode = new RedBlackNode(value);
            root = BSTInsert(root, newNode);
            FixViolation(newNode);
        }

        private RedBlackNode BSTInsert(RedBlackNode root, RedBlackNode newNode)
        {
            if (root == null) return newNode;

            if (newNode.Value < root.Value)
            {
                root.Left = BSTInsert(root.Left, newNode);
                root.Left.Parent = root;
            }
            else if (newNode.Value > root.Value) // ignore duplicates
            {
                root.Right = BSTInsert(root.Right, newNode);
                root.Right.Parent = root;
            }

            return root;
        }

        private void FixViolation(RedBlackNode node)
        {
            RedBlackNode parent, grandparent;

            while (node != root && node.NodeColor == Color.Red && node.Parent.NodeColor == Color.Red)
            {
                parent = node.Parent;
                grandparent = parent.Parent;

                // case 1: parent is left child of grandparent
                if (parent == grandparent.Left)
                {
                    RedBlackNode uncle = grandparent.Right;

                    if (uncle != null && uncle.NodeColor == Color.Red) 
                    {
                        grandparent.NodeColor = Color.Red;
                        parent.NodeColor = Color.Black;
                        uncle.NodeColor = Color.Black;
                        node = grandparent;
                    }
                    else 
                    {
                        if (node == parent.Right)
                        {
                            RotateLeft(parent);
                            node = parent;
                            parent = node.Parent;
                        }
                        RotateRight(grandparent);
                        Color temp = parent.NodeColor;
                        parent.NodeColor = grandparent.NodeColor;
                        grandparent.NodeColor = temp;
                        node = parent;
                    }
                }
                else // case 2: parent is right child of grandparent
                {
                    RedBlackNode uncle = grandparent.Left;

                    if (uncle != null && uncle.NodeColor == Color.Red) 
                    {
                        grandparent.NodeColor = Color.Red;
                        parent.NodeColor = Color.Black;
                        uncle.NodeColor = Color.Black;
                        node = grandparent;
                    }
                    else 
                    {
                        if (node == parent.Left)
                        {
                            RotateRight(parent);
                            node = parent;
                            parent = node.Parent;
                        }
                        RotateLeft(grandparent);
                        Color temp = parent.NodeColor;
                        parent.NodeColor = grandparent.NodeColor;
                        grandparent.NodeColor = temp;
                        node = parent;
                    }
                }
            }

            root.NodeColor = Color.Black; // root is always black
        }

        private void RotateLeft(RedBlackNode node)
        {
            RedBlackNode rightChild = node.Right;
            node.Right = rightChild.Left;

            if (node.Right != null) node.Right.Parent = node;

            rightChild.Parent = node.Parent;

            if (node.Parent == null) root = rightChild;
            else if (node == node.Parent.Left) node.Parent.Left = rightChild;
            else node.Parent.Right = rightChild;

            rightChild.Left = node;
            node.Parent = rightChild;
        }

        private void RotateRight(RedBlackNode node)
        {
            RedBlackNode leftChild = node.Left;
            node.Left = leftChild.Right;

            if (node.Left != null) node.Left.Parent = node;

            leftChild.Parent = node.Parent;

            if (node.Parent == null) root = leftChild;
            else if (node == node.Parent.Right) node.Parent.Right = leftChild;
            else node.Parent.Left = leftChild;

            leftChild.Right = node;
            node.Parent = leftChild;
        }

        public void PreOrder()
        {
            PreOrderHelper(root);
        }

        private void PreOrderHelper(RedBlackNode node)
        {
            if (node != null)
            {
                Console.WriteLine($"{node.Value} ({node.NodeColor})");
                PreOrderHelper(node.Left);
                PreOrderHelper(node.Right);
            }
        }

        public bool Search(int value)
        {
            return SearchHelper(root, value);
        }

        private bool SearchHelper(RedBlackNode node, int value)
        {
            if (node == null) return false;
            if (value == node.Value) return true;
            return value < node.Value ? SearchHelper(node.Left, value) : SearchHelper(node.Right, value);
        }
    }

    class Program
    {
        static void Main()
        {
            var rbTree = new RedBlackTree();

            rbTree.Insert(10);
            rbTree.Insert(20);
            rbTree.Insert(30);
            rbTree.Insert(15);

            Console.WriteLine("Pre-order traversal:");
            rbTree.PreOrder();

            Console.WriteLine("\nSearching for 15:");
            Console.WriteLine(rbTree.Search(15)); // true

            Console.WriteLine("\nSearching for 25:");
            Console.WriteLine(rbTree.Search(25)); // false
        }
    }
}
