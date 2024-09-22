
using System.Diagnostics;

namespace Fuuudoku.Common.Model
{
    [DebuggerDisplay("PossibleNumbersCount: {PossibleNumbersCount}")]
    public class Field
    {
        public int[] PossibleNumbers { get; private set; }
        public int X { get; }
        public int Y { get; }
        internal IEnumerable<FieldsCollection> Containors { get; }

        private int initialPossibleNumbersArraySize;

        public int Number { get; private set; } = 0;
        public int PossibleNumbersCount => PossibleNumbers.Length;
        public bool HasNumber { get; private set; } = false;

        internal Field(int x, int y, IEnumerable<FieldsCollection> containors, int[] initialPossibleNumbers)
        {
            this.X = x;
            this.Y = y;
            this.Containors = containors;
            this.initialPossibleNumbersArraySize = initialPossibleNumbers.Length;
            this.PossibleNumbers = new int[this.initialPossibleNumbersArraySize];
            Array.Copy(initialPossibleNumbers, this.PossibleNumbers, this.initialPossibleNumbersArraySize);

            foreach (var containor in containors)
            {
                containor.Add(this);
            }
        }

        public void SetNumber(int number)
        {
            if (number == 0)
            {
                return;
            }

            if (HasNumber)
            {
                return;
            }

            Number = number;
            HasNumber = true;
            PossibleNumbers = Array.Empty<int>();

            foreach (var containor in Containors)
            {
                containor.RemoveNumberFromPossibleNumbers(number);
            }
        }

        public void RemoveFromPossibleNumbers(int number)
        {
            if (PossibleNumbers.Length == 1)
            {
                return;
            }

            PossibleNumbers = PossibleNumbers.Where(x => x != number).ToArray();
        }

        public void RemoveAllPossibleNumbersExcept(IEnumerable<int> persistingNumbers)
        {
            PossibleNumbers = persistingNumbers.ToArray();
        }

        internal int[] GetFullArray()
        {
            var size = this.initialPossibleNumbersArraySize;

            var fullArray = new int[size];

            for (int i = 1; i <= size; i++)
            {
                if (PossibleNumbers.Contains(i))
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
    }
}
