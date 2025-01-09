using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    public class LinkedListManips
    {
        public class ListNode
        {
            public int val;          // value of the node
            public ListNode next;    // pointer to the next node


            public ListNode(int value = 0, ListNode nextNode = null)
            {
                val = value;
                next = nextNode;
            }
        }

        public ListNode ReverseList(ListNode head) {
            var curr = head;
            ListNode prev = null;
            ListNode next = null;

            while (curr != null) {
                // store the next node in the list. We need to do this because we will change the current node's next pointer.
                next = curr.next;

                // reverse the link. Instead of pointing to the next node, the current node now points to the previous node.
                curr.next = prev;

                // move the previous pointer to the current node (since the current node is now processed).
                prev = curr;

                // move the current pointer to the next node (to continue processing the rest of the list).
                curr = next;
            }

            return prev;
        }

        public ListNode MergeTwoLists(ListNode list1, ListNode list2) {

            // create a new dummy node to serve as the start of the merged list
            ListNode dummy = new ListNode(0);
            ListNode current = dummy;

            // iterate through both lists until we reach the end of one
            while (list1 != null && list2 != null) {
                if (list1.val <= list2.val) {
                    current.next = list1;  // link the smaller node to the merged list
                    list1 = list1.next;     // move to the next node in list1
                } else {
                    current.next = list2;  // link the smaller node to the merged list
                    list2 = list2.next;     // move to the next node in list2
                }
                current = current.next; // move the current pointer
            }

            // if there are remaining nodes in either list, link them
            if (list1 != null) {
                current.next = list1;
            } else if (list2 != null) {
                current.next = list2;
            }

            // return the next of dummy node which points to the head of the merged list
            return dummy.next;
        }

        // linkedlist detection cycle:
        /*
            * For your reference:
            *
            * SinglyLinkedListNode {
            *     int data;
            *     SinglyLinkedListNode next;
            * }
            *
            */
            static bool hasCycle(SinglyLinkedListNode head) {

                var visited = new HashSet<SinglyLinkedListNode>();
                
                var currentNode = head;
                while (currentNode != null) {
                    visited.Add(currentNode);
                    
                    if (currentNode.next != null && visited.Contains(currentNode.next))
                        return true;
                    
                    currentNode = currentNode.next;
                    
                }
                
                return false;
            }
    }
}