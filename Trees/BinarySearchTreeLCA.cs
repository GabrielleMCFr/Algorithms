using System;

namespace Code.algorithms
{
    public class TreeNode
    {
        public int Value;
        public TreeNode Left, Right;

        public TreeNode(int value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
    }

    // Find the lowest common ancestor in a BST
    // Time complexity O(h), where h is the height of the BST. In the worst case, h = n for an unbalanced BST, but for a balanced BST, h = log(n)
    // note : I use here a recursive approach, but an iterative solution can be used too, it can be more space efficient by avoiding stack usage. Doesn't matter for small trees.
    public class BinarySearchTreeLCA
    {
        public TreeNode Root;

        public bool IsValuePresent(TreeNode node, int value)
        {
            if (node == null) return false;
            if (node.Value == value) return true;

            if (value < node.Value)
                return IsValuePresent(node.Left, value);

            return IsValuePresent(node.Right, value);
        }

        public TreeNode FindLCA(TreeNode node, int n1, int n2)
        {
            if (node == null) return null;

            if (n1 < node.Value && n2 < node.Value)
                return FindLCA(node.Left, n1, n2);

            if (n1 > node.Value && n2 > node.Value)
                return FindLCA(node.Right, n1, n2);

            return node; // LCA found
        }

        public TreeNode Insert(TreeNode node, int value)
        {
            if (node == null) return new TreeNode(value);

            if (value < node.Value)
                node.Left = Insert(node.Left, value);
            else if (value > node.Value)
                node.Right = Insert(node.Right, value);

            return node;
        }
    }

    // Example usage
    class Program
    {
        static void Main()
        {
            var bst = new BinarySearchTreeLCA();

            bst.Root = bst.Insert(bst.Root, 20);
            bst.Root = bst.Insert(bst.Root, 8);
            bst.Root = bst.Insert(bst.Root, 22);
            bst.Root = bst.Insert(bst.Root, 4);
            bst.Root = bst.Insert(bst.Root, 12);
            bst.Root = bst.Insert(bst.Root, 10);
            bst.Root = bst.Insert(bst.Root, 14);

            // test for LCA
            int n1 = 10, n2 = 14;

            if (bst.IsValuePresent(bst.Root, n1) && bst.IsValuePresent(bst.Root, n2))
            {
                var lca = bst.FindLCA(bst.Root, n1, n2);
                Console.WriteLine($"LCA of {n1} and {n2}: {lca.Value}");
            }
            else
            {
                Console.WriteLine($"Either {n1} or {n2} is not present in the BST.");
            }
        }
    }
}
