using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms
{
    public class SlidingWindowExample
    {
        /// <summary>
        /// The max profit problem is which day to choose to buy something, then sell it. The difference between the two must be the biggest possible.
        /// We must buy before we sell obviously
        /// prices is an array of prices, the indexes are the days.
        /// </summary>
        public int MaxProfit(int[] prices) {
            var buy = 0;
            var sell = 1;
            var profit = 0;

            while (sell < prices.Length) {
                var tmpProfit = prices[sell] - prices[buy];
                if (tmpProfit > profit)
                    profit = tmpProfit;
                
                if (prices[sell] < prices[buy])
                    buy = sell; 

                sell++;
            }

            return profit;
        }
    }
}