using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// generate subsets for a specific length.
public class GenerateSubsets {
    public static List<List<int>> GenerateSubsets(int subsetsLength, List<int> originalSubset)
    {
        var result = new List<List<int>>();
        Backtrack(originalSubset, new List<int>(), 0, result, subsetsLength);
        return result;
    }

    private static void Backtrack(List<int> originalSubset, IList<int> current, int start, List<List<int>> result, int subsetsLength)
    {
        if (current.Count == subsetsLength)
        {
            result.Add(new List<int>(current));
            return;
        }

        for (int i = start; i < originalSubset.Count; i++)
        {
            current.Add(originalSubset[i]);
            Backtrack(originalSubset, current, i + 1, result, subsetsLength);
            current.RemoveAt(current.Count - 1);
        }
    }
}