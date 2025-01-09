using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms.Problems
{
    public class ClimbLeaderBoard
    {
        /*
        An arcade game player wants to climb to the top of the leaderboard and track their ranking. The game uses Dense Ranking, so its leaderboard works like this:
        The player with the highest score is ranked number 1 on the leaderboard.
        Players who have equal scores receive the same ranking number, and the next player(s) receive the immediately following ranking number.
        */

        public static List<int> climbingLeaderboard(List<int> ranked, List<int> player)
        {
            // delete doubles and order
            var distinctRanked = ranked.Distinct().OrderByDescending(r => r).ToList();
            
            var playerRanksArray = new List<int>();
            
            // pointer
            int i = distinctRanked.Count - 1; 
            
            foreach (var score in player)
            {
                while (i >= 0 && score >= distinctRanked[i])
                {
                    i--;
                }
                
                // +1 because it's 0 indexed, and +1 again because the player is classed after the current i. 
                // If the player score is first, i will be -1 so rank = 1
                playerRanksArray.Add(i + 2);
            }
            
            return playerRanksArray;
        }
            }
}