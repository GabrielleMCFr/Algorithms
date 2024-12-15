using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.algorithms;

// The N-Queens problem: Place N queens on an N x N chessboard such that no two queens threaten each other.
// This means no two queens can share the same row, column, or diagonal.
public class NQueens
{
    public static IList<IList<string>> SolveNQueens(int n)
    {
        var result = new List<IList<string>>();
        var board = new string[n];
        for (int i = 0; i < n; i++)
            board[i] = new string('.', n); // Initialiser l'échiquier

        Backtrack(result, board, 0);
        return result;
    }

    private static void Backtrack(IList<IList<string>> result, string[] board, int row)
    {
        if (row == board.Length) // Cas de base : toutes les reines sont placées
        {
            result.Add(new List<string>(board));
            return;
        }

        for (int col = 0; col < board.Length; col++)
        {
            if (!IsValid(board, row, col)) continue; 

            // add the queen
            board[row] = board[row].Remove(col, 1).Insert(col, "Q"); 

            // recursive backtracking
            Backtrack(result, board, row + 1); 

            // take the queen off the board
            board[row] = board[row].Remove(col, 1).Insert(col, "."); 
        }
    }

    private static bool IsValid(string[] board, int row, int col)
    {
        // check columns
        for (int i = 0; i < row; i++) 
            if (board[i][col] == 'Q') return false;

        // check upper left diag
        for (int i = row, j = col; i >= 0 && j >= 0; i--, j--) 
            if (board[i][j] == 'Q') return false;

        // check upper right diag
        for (int i = row, j = col; i >= 0 && j < board.Length; i--, j++)
            if (board[i][j] == 'Q') return false;

        return true;
    }
}

class Program
{
    static void Main(string[] args)
    {
        int n = 4;
        var solutions = NQueens.SolveNQueens(n);

        if (solutions.Count == 0)
        {
            Console.WriteLine("No solution found.");
        }

        foreach (var solution in solutions)
        {
            foreach (var row in solution)
            {
                Console.WriteLine(row);
            }
            Console.WriteLine();
        }
    }
}