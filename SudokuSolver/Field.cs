
using System.Diagnostics;

namespace SudokuSolver
{
    [DebuggerDisplay("PossibleNumbersCount: {PossibleNumbersCount}")]
    public class Field
    {
        public int[] PossibleNumbers { get; private set; } = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public int X { get; }
        public int Y { get; }
        public IEnumerable<FieldsCollection> Containors { get; }
        public int Number { get; private set; } = 0;
        public int PossibleNumbersCount => this.PossibleNumbers.Length;
        public bool HasNumber { get; private set; } = false;

        public Field(int x, int y, IEnumerable<FieldsCollection> containors)
        {
            this.X = x;
            this.Y = y;
            this.Containors = containors;

            foreach (var containor in containors)
            {
                containor.Add(this);
            }
        }

        internal void SetNumber(int number)
        {
            if (number == 0)
            {
                return;
            }

            if (this.HasNumber)
            {
                return;
            }

            this.Number = number;
            this.HasNumber = true;
            this.PossibleNumbers = Array.Empty<int>();

            foreach (var containor in this.Containors)
            {
                containor.RemoveNumberFromPossibleNumbers(number);
            }
        }

        internal void RemoveFromPossibleNumbers(int number)
        {
            if (this.PossibleNumbers.Length == 1)
            {
                return;
            }

            this.PossibleNumbers = this.PossibleNumbers.Where(x => x != number).ToArray();
        }

        internal void RemoveAllPossibleNumbersExcept(IEnumerable<int> persistingNumbers)
        {
            this.PossibleNumbers = persistingNumbers.ToArray();
        }

        public int[] GetFullArray()
        {
            var fullArray = new int[9];

            for (int i = 1; i <= 9; i++)
            {
                if (this.PossibleNumbers.Contains(i))
                {
                    fullArray[i - 1] = i;
                }
                else
                {
                    fullArray[i - 1] = 0;
                }
            }

            return fullArray;
        }

        internal void SetPossibleNumbers(int[] possibleNumbers)
        {
            this.PossibleNumbers = possibleNumbers;
        }
    }
}
