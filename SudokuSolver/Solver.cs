using System.Diagnostics.CodeAnalysis;

namespace SudokuSolver
{
    public class Solver
    {
        private List<StateNode> history = new List<StateNode>();

        public StateNode Solve(Board board)
        {
            var counter = 0;
            StateNode state = new StateNode(board);
            history.Add(state);

            while (!state.IsSolved && !board.IsUnsolvable)
            {
                state = new StateNode(board);
                counter++;
                SolveProcess(board);
                var poss = board.PrintPossibleNumbers();

                if (BoardDidntChanged(state))
                {
                    var fields = board.FieldList
                        .Where(f => !f.HasNumber)
                        .ToList();

                    if (!fields.Any())
                    {
                        continue;
                    }

                    fields.Sort((Field a, Field b) => a.PossibleNumbersCount.CompareTo(b.PossibleNumbersCount));
                    var field = fields.First();

                    foreach (var possibleNumber in field.PossibleNumbers)
                    {
                        var nextBoard = new Board(board.ToArray());
                        var childField = nextBoard.FieldList.Single(f => f.X == field.X && f.Y == field.Y);
                        childField.RemoveFromPossibleNumbers(possibleNumber);
                        var childState = new Solver().Solve(nextBoard);
                        state.AddChildState(childState);
                    }
                }

                history.Add(state);
            }

            return state;
        }

        private bool BoardDidntChanged(StateNode state)
        {
            if (!this.history.Any())
            {
                return true;
            }

            var previousState = this.history.Last();

            return previousState.Equals(state);
        }

        private void SolveProcess(Board board)
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
            var fieldsCollections = board.Columns;
            CheckPairsInFieldsCollection(fieldsCollections);
        }

        private void CheckRowsPairs(Board board)
        {
            var fieldsCollections = board.Rows;
            CheckPairsInFieldsCollection(fieldsCollections);
        }

        private void CheckSquaresPairs(Board board)
        {
            var fieldsCollections = board.SquaresList;
            CheckPairsInFieldsCollection(fieldsCollections);
        }

        private static void CheckPairsInFieldsCollection(IEnumerable<IFieldsCollection> fieldsCollections)
        {
            foreach (IFieldsCollection fieldsCollection in fieldsCollections)
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

                        var fieldsWithFirsNumber = fieldsCollection.Fields.Where(f => f.PossibleNumbers.Contains(n1)).ToHashSet(new FieldsComparer());
                        var fieldsWithSecondNumber = fieldsCollection.Fields.Where(f => f.PossibleNumbers.Contains(n2)).ToHashSet(new FieldsComparer());

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
            CheckHorizontalyAlignedSquares(board);
        }

        private void CheckHorizontalyAlignedSquares(Board board)
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

        private void CheckSquares(Board board)
        {
            var squares = board.SquaresList;
            SetNumbersInFieldsOfCollection(squares);
        }
    }
}
