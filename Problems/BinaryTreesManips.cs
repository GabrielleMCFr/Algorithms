using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    public class BinaryTreesManips
    {
        //  invert a tree
        public TreeNode InvertTree(TreeNode root) {
            var currentNode = root;

            if (currentNode == null)
                return null;
            
            // swap
            var tmp = currentNode.left;
            currentNode.left = currentNode.right;
            currentNode.right = tmp;

            InvertTree(currentNode.left);
            InvertTree(currentNode.right);

            return root;
        }


        // two trees are the same : 
        public bool IsSameTree(TreeNode p, TreeNode q) {
            // base case: both nodes are null
            if (p == null && q == null) {
                return true;
            }
            
            // if one of the nodes is null or values are different
            if (p == null || q == null || p.val != q.val) {
                return false;
            }
            
            // recursively check the left and right subtrees
            return IsSameTree(p.left, q.left) && IsSameTree(p.right, q.right);
        }

        // is subtree of a tree : 
        // O(n*m) with n : nb of nodes in one tree, and m the nb of nodes in the subtree
        public bool IsSubtree(TreeNode root, TreeNode subRoot) {
            if (root == null) return false; // if root is null, subRoot can't be a subtree

            if (IsIdentical(root, subRoot)) return true; // check if root and subRoot are identical
            // otherwise, check if subRoot is a subtree of either the left or right child of root
            return IsSubtree(root.left, subRoot) || IsSubtree(root.right, subRoot);
        }

        private bool IsIdentical(TreeNode root1, TreeNode root2) {
            if (root1 == null && root2 == null) return true; // both are null, so they are identical
            if (root1 == null || root2 == null) return false; // only one is null, so not identical
            if (root1.val != root2.val) return false; // values don't match, so not identical
            // recursively check left and right subtrees
            return IsIdentical(root1.left, root2.left) && IsIdentical(root1.right, root2.right);
        }


        // max depth of a tree :
        public int MaxDepth(TreeNode root) {
            if (root == null) {
            return 0; // if empty tree, depth of 0
            }
        
            // Recursively find the max depth of subtree right and left
            int leftDepth = MaxDepth(root.left);
            int rightDepth = MaxDepth(root.right);
            
            // Max depth is the biggest depth of the two plus 1 for the current node
            return Math.Max(leftDepth, rightDepth) + 1;
        }

        
        // Find all paths with backtracking
        public IList<IList<int>> AllPaths(TreeNode root) {
            var result = new List<IList<int>>();
            Backtrack(root, new List<int>(), result);
            return result;
        }

        private void Backtrack(TreeNode node, List<int> path, IList<IList<int>> result) {
            if (node == null) return;

            // Add node
            path.Add(node.val);

            // If leaf, add the path to the results
            if (node.left == null && node.right == null) {
                result.Add(new List<int>(path));
            } else {
                // Else, keep search through right and left subtrees
                Backtrack(node.left, path, result);
                Backtrack(node.right, path, result);
            }

            // Backtrack : we take of the node to explore other ways.
            path.RemoveAt(path.Count - 1);
        }
    }
}