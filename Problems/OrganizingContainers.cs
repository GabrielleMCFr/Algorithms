using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    public class OrganizingContainers
    {
        /*
        David has several containers, each with a number of balls in it. He has just enough containers to sort each type of ball 
        he has into its own container. David wants to sort the balls using his sort method.
        David wants to perform some number of swap operations such that:
        Each container contains only balls of the same type.
        No two balls of the same type are located in different containers.
        */

        public static string organizingContainers(List<List<int>> container)
        {
            // we don't need to do the swap themselves. We only had to compare the sum of balls by type and the sum of capacity of the containers.
            
            var totalTypes = new List<int>(new int[container.Count]);
            var totalContainersCapacity = new List<int>(new int[container.Count]);
            
            for (var i = 0; i < container.Count; i++) {
                for (var j = 0; j < container[0].Count; j++) {
                    totalContainersCapacity[i] += container[i][j];               
                    totalTypes[j] += container[i][j];
                }
            }
            
            // Sort to compare
            totalContainersCapacity.Sort();
            totalTypes.Sort();

            return totalContainersCapacity.SequenceEqual(totalTypes) ? "Possible" : "Impossible";
            
        }
    }
}