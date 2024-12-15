using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms

// Example of memoization usage. For fibonacci, without memoization we have a O(2^n), and O(n) with memoization.
// It's because we save the result of the same subproblems that are used several time and avoid redondant calculation.
// Note : this will work only until Fibonacci(92), since that is the largest Fibonacci number that fits within the range of a 64-bit signed integer.
// Beyond that, we need to use something else like BigInteger.
public class MemoizationExample
{
    private static readonly Dictionary<int, long> memo = new Dictionary<int, long>();

    public static long Fibonacci(int n)
    {
        if (n <= 1)
            return n;

        if (memo.ContainsKey(n))
            return memo[n];

        memo[n] = Fibonacci(n - 1) + Fibonacci(n - 2);
        return memo[n];
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Memoized Fibonacci Example");

        Console.WriteLine($"Fibonacci(5): {MemoizationExample.Fibonacci(5)}");
        Console.WriteLine($"Fibonacci(10): {MemoizationExample.Fibonacci(10)}");
        Console.WriteLine($"Fibonacci(20): {MemoizationExample.Fibonacci(20)}");

        // test with a larger number
        Console.WriteLine($"Fibonacci(50): {MemoizationExample.Fibonacci(50)}");

        Console.Write("Enter a number to compute its Fibonacci value: ");
        if (int.TryParse(Console.ReadLine(), out int userInput))
        {
            Console.WriteLine($"Fibonacci({userInput}): {MemoizationExample.Fibonacci(userInput)}");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
    }
}
