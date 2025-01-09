using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms.Problems
{
    /*
    The goal is to find the minimum transmitters we can use with an action of +- k around it to cover the whole array.
    Note : the houses are not necessarily sequential.
    */
    public class RadioTransmitters
    {
        public static int HackerlandRadioTransmitters(List<int> x, int k)
        {
            // step 1: sort the houses
            x.Sort(); // sort the positions of the houses
            int n = x.Count; // total number of houses
            int i = 0; // index to iterate through the houses
            int transmitters = 0; // counter for the number of transmitters

            while (i < n)
            {
                // step 2: identify the leftmost house not covered
                transmitters++; // add a transmitter
                int loc = x[i] + k; // find the optimal location for the transmitter

                // step 3: move forward to the farthest house covered by this transmitter
                while (i < n && x[i] <= loc)
                {
                    i++;
                }

                // step 4: move further to cover the rest within the transmitter's range
                // note: pre-decrement, equivalent to:
                // i--;
                // loc = x[i] + k;
                loc = x[--i] + k;
                while (i < n && x[i] <= loc)
                {
                    i++;
                }
            }

            return transmitters; // return the minimum number of transmitters
        }

        // example usage
        public static void Main(string[] args)
        {
            List<int> houses = new List<int> { 1, 2, 3, 5, 9 };
            int range = 1;
            Console.WriteLine(hackerlandRadioTransmitters(houses, range)); // expected result: 3
        }

    }
}