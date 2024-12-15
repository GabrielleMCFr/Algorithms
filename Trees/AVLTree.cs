using System;

namespace Code.algorithms.Trees
{
    // AVL Tree
    // An AVL tree is a type of binary search tree that remains balanced after each insertion and deletion.
    // This ensures that search, insertion, and deletion operations remain efficient.
    // In an AVL tree, for each node, the height difference between the left and right subtrees is at most 1.
    // If an imbalance is detected after an insertion or deletion, a rotation (single or double) is performed to restore balance.
    // Time complexity: O(log n)
    public class AVLNode
    {
        public int Value;
        public AVLNode Left, Right;
        public int Height;

        public AVLNode(int value)
        {
            Value = value;
            Height = 1; // initial height is 1
        }
    }

    public class AVLTree
    {
        public AVLNode Root;

        private int Height(AVLNode node) => node == null ? 0 : node.Height;

        private int GetBalance(AVLNode node) => node == null ? 0 : Height(node.Left) - Height(node.Right);

        private AVLNode RotateRight(AVLNode y)
        {
            AVLNode x = y.Left;
            AVLNode T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
            x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;

            return x;
        }

        private AVLNode RotateLeft(AVLNode x)
        {
            AVLNode y = x.Right;
            AVLNode T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
            y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;

            return y;
        }

        public void Insert(int value)
        {
            Root = InsertRecursive(Root, value);
        }

        private AVLNode InsertRecursive(AVLNode node, int value)
        {
            if (node == null) return new AVLNode(value);

            if (value < node.Value)
                node.Left = InsertRecursive(node.Left, value);
            else if (value > node.Value)
                node.Right = InsertRecursive(node.Right, value);
            else
                return node; // duplicate value, no insertion

            node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
            int balance = GetBalance(node);

            // left-left case
            if (balance > 1 && value < node.Left.Value)
                return RotateRight(node);

            // right-right case
            if (balance < -1 && value > node.Right.Value)
                return RotateLeft(node);

            // left-right case
            if (balance > 1 && value > node.Left.Value)
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

            // right-left case
            if (balance < -1 && value < node.Right.Value)
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
        }

        // in-order traversal for testing
        public void InOrderTraversal()
        {
            InOrderHelper(Root);
            Console.WriteLine();
        }

        private void InOrderHelper(AVLNode node)
        {
            if (node != null)
            {
                InOrderHelper(node.Left);
                Console.Write($"{node.Value} ");
                InOrderHelper(node.Right);
            }
        }
    }

    class Program
    {
        static void Main()
        {
            AVLTree tree = new AVLTree();

            int[] values = { 10, 20, 30, 40, 50, 25 };
            foreach (var value in values)
            {
                tree.Insert(value);
                Console.WriteLine($"Inserted {value}:");
                tree.InOrderTraversal();
            }

            Console.WriteLine("Final In-order Traversal:");
            tree.InOrderTraversal();
        }
    }
}
