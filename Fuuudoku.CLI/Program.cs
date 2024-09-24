// See https://aka.ms/new-console-template for more information
using Cocona;
using Fuuudoku.CLI;
using Fuuudoku.Common;
using Fuuudoku.Common.Model;
using SudokuSolver;

CoconaLiteApp.Run(([Option("d")] string dot) =>
{
    SolveSudokuFromDotString(dot);
});

CoconaLiteApp.Run(([Option("p")] string path) =>
{
    if (!File.Exists(path))
    {
        Console.WriteLine($"There is no file on path: {path}");
        return;
    }

    var dotString = File.ReadAllText(path);
    SolveSudokuFromDotString(dotString);
});

void SolveSudokuFromDotString(string dotString)
{
    DotConverter.Cvt(dotString, out Board board);
    var solvedBoard = Solver.Solve(board);
    Console.Write(StringUtils.PrintBoard(solvedBoard));
}