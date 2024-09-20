using Fuuudoku.Common.Model;

namespace Fuuudoku.Solver.Checkers
{
    internal static class AlignedSquaresChecker
    {
        public static void Check(Board board)
        {
            CheckVerticalyAlignedSquares(board);
            CheckHorizontalyAlignedSquares(board);
        }

        private static void CheckHorizontalyAlignedSquares(Board board)
        {
            var squares = board.SquaresList;

            foreach (var square in squares)
            {
                var alignedSquares = board.GetHorizontalyAlignedSquares(square.Sy, square.Sx);

                for (int n = 1; n <= 9; n++)
                {
                    var groups = square.Fields
                        .Where(f => f.PossibleNumbers.Contains(n))
                        .GroupBy(f => f.Y);

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

                //check square columns with only possible numbers
                for (int n = 1; n <= 9; n++)
                {
                    CheckSingleColumns(square, alignedSquares, n);
                    CheckMultiColumns(square, alignedSquares, n);
                }
            }
        }

        private static void CheckMultiColumns(Square square, IEnumerable<Square> alignedSquares, int n)
        {
            //if aligned squares have possible number in same columns
            //the square must have number in remaining columns
            var fieldsList = new List<Field>();
            var xsWithNumber = square.GetColumnsWithPossibleNumber(n);

            foreach (var alignedSquare in alignedSquares)
            {
                var fields = alignedSquares
                .SelectMany(s => s.Fields)
                .Where(f => f.PossibleNumbers.Contains(n));

                //if aligned square doesnt have fields it will give to much control
                //to other aligned square
                if (!fields.Any())
                {
                    return;
                }

                fieldsList.AddRange(fields);
            }

            var alignedGroups = fieldsList.GroupBy(f => f.X);

            if (alignedGroups.Count() == 2)
            {
                var xs = alignedGroups
                    .Select(g => g.Key);
                var persistingXs = xsWithNumber
                    .Except(xs);

                if (!persistingXs.Any())
                {
                    return;
                }

                xs = xs.Except(persistingXs);

                foreach (var x in xs)
                {
                    square.RemovePossibleNumberFromColumn(n, x);
                }
            }
        }

        private static void CheckSingleColumns(Square square, IEnumerable<Square> alignedSquares, int n)
        {
            //get fields with possible number and check if they are in same column
            var groups = square.Fields
                .Where(f => f.PossibleNumbers.Contains(n))
                .GroupBy(f => f.X);

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
