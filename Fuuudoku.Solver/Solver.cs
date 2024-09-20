


using System.Runtime.CompilerServices;
using Fuuudoku.Common.Model;
using Fuuudoku.Solver.Checkers;

[assembly: InternalsVisibleToAttribute("Fuuudoku.Solver.Tests")]

namespace SudokuSolver
{
    internal class Solver
    {
        private List<StateNode> history = new List<StateNode>();

        internal StateNode Solve(Board board)
        {
            var counter = 0;
            StateNode state = new StateNode(board);
            history.Add(state);

            while (!state.IsSolved && !board.IsUnsolvable)
            {
                state = new StateNode(board);
                counter++;
                SolveProcess(board);

                if (BoardDidntChange(state))
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

        private bool BoardDidntChange(StateNode state)
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
            PairsChecker.Check(board.SubCollections);
            AlignedSquaresChecker.Check(board);

            SetNumbersInFieldsOfCollection(board.SubCollections);
        }

        private static void SetNumbersInFieldsOfCollection(IEnumerable<FieldsCollection> rows)
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
    }
}
