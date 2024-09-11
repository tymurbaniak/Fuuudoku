



using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SudokuSolver
{
    public class Solver
    {
        private List<int[][]> history = new List<int[][]>();

        public Board Solve(int[][] numbersArray)
        {
            var board = new Board(numbersArray);
            var counter = 0;

            while (!board.IsSolved)
            {
                counter++;
                Solve(board);
                //save current state
                this.history.Add(board.ToArray());
            }

            return board;
        }

        private void Solve(Board board)
        {
            CheckSquares(board);
            CheckAlignedSquares(board);
            CheckSquaresPairs(board);

            CheckRows(board);
            CheckRowsPairs(board);

            CheckColumns(board);
            CheckColumnsPairs(board);
        }

        private void CheckColumnsPairs(Board board)
        {
            foreach (var row in board.Columns)
            {
                var pairs = new List<Tuple<Field, Field>>();

                for (int n1 = 1; n1 <= 9; n1++)
                {
                    for (int n2 = 1; n2 <= 9; n2++)
                    {
                        if (n1 == n2)
                        {
                            continue;
                        }

                        var fieldsWithFirsNumber = row.Fields.Where(f => f.PossibleNumbers.Contains(n1)).ToHashSet(new FieldsComparer());
                        var fieldsWithSecondNumber = row.Fields.Where(f => f.PossibleNumbers.Contains(n2)).ToHashSet(new FieldsComparer());

                        if (fieldsWithFirsNumber.Count() == 2 && fieldsWithSecondNumber.Count() == 2
                            && fieldsWithFirsNumber.SetEquals(fieldsWithSecondNumber))
                        {
                            foreach (var field in fieldsWithFirsNumber)
                            {
                                field.RemoveAllPossibleNumbersExcept([n1, n2]);
                            }
                        }
                    }
                }
            }
        }

        private void CheckRowsPairs(Board board)
        {
            foreach (var row in board.Rows)
            {
                var pairs = new List<Tuple<Field, Field>>();

                for (int n1 = 1; n1 <= 9; n1++)
                {
                    for (int n2 = 1; n2 <= 9; n2++)
                    {
                        if (n1 == n2)
                        {
                            continue;
                        }

                        var fieldsWithFirsNumber = row.Fields.Where(f => f.PossibleNumbers.Contains(n1)).ToHashSet(new FieldsComparer());
                        var fieldsWithSecondNumber = row.Fields.Where(f => f.PossibleNumbers.Contains(n2)).ToHashSet(new FieldsComparer());

                        if (fieldsWithFirsNumber.Count() == 2 && fieldsWithSecondNumber.Count() == 2
                            && fieldsWithFirsNumber.SetEquals(fieldsWithSecondNumber))
                        {
                            foreach (var field in fieldsWithFirsNumber)
                            {
                                field.RemoveAllPossibleNumbersExcept([n1, n2]);
                            }
                        }
                    }
                }
            }
        }

        private void CheckSquaresPairs(Board board)
        {
            foreach (var square in board.SquaresList)
            {
                //if there are exactly 2 fields with same pair in square, all other possible numbers can be removed from those fields
                var pairs = new List<Tuple<Field, Field>>();

                for (int n1 = 1; n1 <= 9; n1++)
                {
                    for (int n2 = 1; n2 <= 9; n2++)
                    {
                        if (n1 == n2)
                        {
                            continue;
                        }

                        var fieldsWithFirsNumber = square.FieldsList.Where(f => f.PossibleNumbers.Contains(n1)).ToHashSet(new FieldsComparer());
                        var fieldsWithSecondNumber = square.FieldsList.Where(f => f.PossibleNumbers.Contains(n2)).ToHashSet(new FieldsComparer());

                        if (fieldsWithFirsNumber.Count() == 2 && fieldsWithSecondNumber.Count() == 2
                            && fieldsWithFirsNumber.SetEquals(fieldsWithSecondNumber))
                        {
                            foreach (var field in fieldsWithFirsNumber)
                            {
                                field.RemoveAllPossibleNumbersExcept([n1, n2]);
                            }
                        }
                    }
                }
            }
        }

        public class FieldsComparer : IEqualityComparer<Field>
        {
            public bool Equals(Field? x, Field? y)
            {
                return x.X == y.X && x.Y == y.Y;
            }

            public int GetHashCode([DisallowNull] Field obj)
            {
                return Math.Pow(obj.X, obj.Y).GetHashCode();
            }
        }

        private void CheckColumns(Board board)
        {
            var columns = board.Columns;
            SetNumbersInFieldsOfCollection(columns);
        }

        private void CheckRows(Board board)
        {
            var rows = board.Rows;
            SetNumbersInFieldsOfCollection(rows);
        }

        private static void SetNumbersInFieldsOfCollection(IEnumerable<IFieldsCollection> rows)
        {
            foreach (var row in rows)
            {
                var fields = row.Fields;

                for (int i = 1; i <= 9; i++)
                {
                    var onlyPossibleFieldWithNumber = fields
                        .Where(f => !f.HasNumber)
                        .Where(f => f.PossibleNumbers.Contains(i));

                    if (onlyPossibleFieldWithNumber.Count() == 1)
                    {
                        var selectedField = onlyPossibleFieldWithNumber.Single();
                        selectedField.SetNumber(i);
                    }
                }
            }
        }

        private void CheckAlignedSquares(Board board)
        {
            CheckVerticalyAlignedSquares(board);
            //CheckHorizontalyAlignedSquares(board);

            //horizontal            
            foreach (var square in board.SquaresList)
            {
                var alignedSquares = board.GetHorizontalyAlignedSquares(square.Sy, square.Sx);
                var ys = square.GetSolvedRows();

                foreach (var alignedSquare in alignedSquares)
                {
                    foreach (var y in ys)
                    {
                        var number = alignedSquare.GetOnlyPossibleNumberForRow(y);

                        if (number == -1)
                        {
                            continue;
                        }

                        var otherVerticalyAlignedSquares = board.GetHorizontalyAlignedSquares(y, [square.Sx, alignedSquare.Sx]);

                        foreach (var otherSquare in otherVerticalyAlignedSquares)
                        {
                            otherSquare.RemovePossibleNumberFromRow(number, y);
                        }
                    }
                }
            }
        }

        private void CheckHorizontalyAlignedSquares(Board board)
        {
            var squares = board.SquaresList;

            foreach (var square in squares)
            {
                var alignedSquares = board.GetHorizontalyAlignedSquares(square.Sx, square.Sy);

                for (int n = 1; n <= 9; n++)
                {
                    //get fields with possible number and check if they are in same column
                    var groups = square.FieldsList
                        .Where(f => f.PossibleNumbers.Contains(n))
                        .GroupBy(f => f.Y);
                    var test1 = square.FieldsList
                        .Where(f => f.PossibleNumbers.Contains(n))
                        .ToArray();

                    //there is only one column with number n, so this column in aligned squares
                    //can't have number n
                    if (groups.Count() == 1)
                    {
                        var y = groups.First().Key;

                        foreach (var alignedSquare in alignedSquares)
                        {
                            alignedSquare.RemovePossibleNumberFromRow(n, y);
                        }
                    }
                }
            }
        }

        private static void CheckVerticalyAlignedSquares(Board board)
        {
            var squares = board.SquaresList;

            foreach (var square in squares)
            {
                var alignedSquares = board.GetVerticalyAlignedSquares(square.Sx, square.Sy);
                var xs = square.GetSolvedColumns();

                foreach (var alignedSquare in alignedSquares)
                {
                    foreach (var x in xs)
                    {
                        var number = alignedSquare.GetOnlyPossibleNumberForColumn(x);

                        if (number == -1)
                        {
                            continue;
                        }

                        var otherVerticalyAlignedSquares = board.GetVerticalyAlignedSquares(square.Sx, [square.Sy, alignedSquare.Sy]);

                        foreach (var otherSquare in otherVerticalyAlignedSquares)
                        {
                            otherSquare.RemovePossibleNumberFromColumn(number, x);
                        }
                    }
                }

                foreach (var x in xs)
                {
                    var solvedColumn = square.GetSquareColumn(x);
                    var solvedColumnNumbers = solvedColumn.Select(c => c.Number);
                    //check in aligned squares if there are columns, that have different numbers than solved column

                    foreach (var alignedSquare in alignedSquares)
                    {
                        int[] axs = alignedSquare.GetColumnsWithDifferentNumbers(solvedColumn.Select(f => f.Number), x);

                        var otherVerticalyAlignedSquares = board.GetVerticalyAlignedSquares(square.Sx, [square.Sy, alignedSquare.Sy]);

                        foreach (var ax in axs)
                        {
                            var differentNumbers = alignedSquare
                                .GetSquareColumn(ax)
                                .Where(f => f.HasNumber)
                                .Select(f => f.Number)
                                .Except(solvedColumnNumbers);

                            foreach (var number in differentNumbers)
                            {
                                foreach (var otherSquare in otherVerticalyAlignedSquares)
                                {
                                    otherSquare.RemovePossibleNumberFromAllColumnsExcept(number, [x, ax]);
                                }
                            }
                        }
                    }
                }

                //check square columns with only possible numbers
                for (int n = 1; n <= 9; n++)
                {
                    //get fields with possible number and check if they are in same column
                    var groups = square.FieldsList
                        .Where(f => f.PossibleNumbers.Contains(n))
                        .GroupBy(f => f.X);
                    var test1 = square.FieldsList
                        .Where(f => f.PossibleNumbers.Contains(n))
                        .ToArray();

                    //there is only one column with number n, so this column in aligned squares
                    //can't have number n
                    if (groups.Count() == 1)
                    {
                        var x = groups.First().Key;

                        foreach (var alignedSquare in alignedSquares)
                        {
                            alignedSquare.RemovePossibleNumberFromColumn(n, x);
                        }
                    }
                }
            }
        }

        private void CheckSquares(Board board)
        {
            var squares = board.SquaresList;
            SetNumbersInFieldsOfCollection(squares);
        }
    }
}
