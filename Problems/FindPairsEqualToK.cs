using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms.Problems
{
    public class FindPairsEqualToK
    {
        // Which two numbers add themselves for a result k
        public int[] TwoSum(int[] nums, int target)
        {
            Dictionary<int, int> seen = new Dictionary<int, int>();
            
            for (int i = 0; i < nums.Length; i++)
            {
                int complement = target - nums[i];
                if (seen.ContainsKey(complement))
                    return new int[] { seen[complement], i };
                
                seen[nums[i]] = i;
            }
            
            return new int[] { };
        }

        // if the difference of two numbers in an array equals a target
        public int[] TwoDiff(int[] nums, int target)
        {
            Dictionary<int, int> seen = new Dictionary<int, int>();
            
            for (int i = 0; i < nums.Length; i++)
            {
                int complement = target + nums[i];
                if (seen.ContainsKey(complement))
                    return new int[] { seen[complement], i };
                
                seen[nums[i]] = i;
            }
            
            return new int[] { };
        }
    }
}