// See https://aka.ms/new-console-template for more information
using SudokuSolver;

Console.WriteLine("Hello, World!");

var path = args.FirstOrDefault();

if (path == null)
{
    Console.WriteLine("Path argument is missing");
    return;
}

var sudokuArray = FileUtils.ReadSudokuFromFile(path);
var sudokuBoard = new Board(sudokuArray);

