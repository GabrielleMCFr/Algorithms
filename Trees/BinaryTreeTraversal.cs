using System;
using System.Collections.Generic;

namespace Code.algorithms
{
    public class BinaryTreeNode
    {
        public int Value;
        public BinaryTreeNode Left, Right;

        public BinaryTreeNode(int value)
        {
            Value = value;
            Left = Right = null;
        }
    }

    public class BinaryTree
    {
        public BinaryTreeNode Root;

        // inorder traversal (Left → Root → Right)
        public void InorderTraversal(BinaryTreeNode node)
        {
            if (node == null) return;

            InorderTraversal(node.Left);
            Console.Write(node.Value + " ");
            InorderTraversal(node.Right);
        }

        // preorder traversal (Root → Left → Right)
        public void PreorderTraversal(BinaryTreeNode node)
        {
            if (node == null) return;

            Console.Write(node.Value + " ");
            PreorderTraversal(node.Left);
            PreorderTraversal(node.Right);
        }

        // postorder traversal (Left → Right → Root)
        public void PostorderTraversal(BinaryTreeNode node)
        {
            if (node == null) return;

            PostorderTraversal(node.Left);
            PostorderTraversal(node.Right);
            Console.Write(node.Value + " ");
        }

        // level order traversal (BFS)
        public void LevelOrderTraversal(BinaryTreeNode root)
        {
            if (root == null) return;

            var queue = new Queue<BinaryTreeNode>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                Console.Write(current.Value + " ");

                if (current.Left != null) queue.Enqueue(current.Left);
                if (current.Right != null) queue.Enqueue(current.Right);
            }
        }
    }

    // Example usage
    class Program
    {
        static void Main()
        {
            var tree = new BinaryTree
            {
                Root = new BinaryTreeNode(1)
                {
                    Left = new BinaryTreeNode(2)
                    {
                        Left = new BinaryTreeNode(4),
                        Right = new BinaryTreeNode(5)
                    },
                    Right = new BinaryTreeNode(3)
                }
            };

            Console.WriteLine("Inorder Traversal:");
            tree.InorderTraversal(tree.Root);

            Console.WriteLine("\nPreorder Traversal:");
            tree.PreorderTraversal(tree.Root);

            Console.WriteLine("\nPostorder Traversal:");
            tree.PostorderTraversal(tree.Root);

            Console.WriteLine("\nLevel Order Traversal:");
            tree.LevelOrderTraversal(tree.Root);
        }
    }
}