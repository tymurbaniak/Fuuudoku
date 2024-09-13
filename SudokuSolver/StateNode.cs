
namespace SudokuSolver
{
    public class StateNode : IEquatable<StateNode>
    {
        private List<StateNode> states = new List<StateNode>();
        private Board board;

        public int[][] NumbersArray { get; }
        public int[][][] PossibleNumbersArray { get; }
        public IReadOnlyList<StateNode> States => states;
        public bool IsSolved => CheckSolveState();
        public Board Board => board;

        public StateNode(Board board)
        {
            this.board = board;
            this.NumbersArray = board.ToArray();
            this.PossibleNumbersArray = board.PossibleNumbersToArray();
        }

        public StateNode GetSolved()
        {
            if (this.board.IsSolved)
            {
                return this;
            }

            foreach (var state in this.States)
            {
                var board = state.GetSolved();

                if (board != null)
                {
                    return board;
                }
            }

            return null;
        }

        private bool CheckSolveState()
        {
            if (this.states.Count == 0)
            {
                return this.board.IsSolved;
            }
            else
            {
                return this.states.Any(x => x.IsSolved);
            }
        }

        internal void AddChildState(StateNode childState)
        {
            this.states.Add(childState);
        }

        public bool Equals(StateNode? other)
        {
            if (other == null) throw new ArgumentNullException("StateNode is null");

            var othersFields = other.NumbersArray.SelectMany(f => f);
            var thisFields = this.NumbersArray.SelectMany(f => f);
            var otherPossibleNumbers = other.PossibleNumbersArray.SelectMany(p => p.SelectMany(n => n));
            var thisPossibleNumbers = other.PossibleNumbersArray.SelectMany(p => p.SelectMany(n => n));

            return othersFields.SequenceEqual(thisFields) && otherPossibleNumbers.SequenceEqual(thisPossibleNumbers);
        }
    }
}
