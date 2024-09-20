
using System.Diagnostics;

namespace Fuuudoku.Common.Model
{
    [DebuggerDisplay("PossibleNumbersCount: {PossibleNumbersCount}")]
    public class Field
    {
        public int[] PossibleNumbers { get; private set; } = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public int X { get; }
        public int Y { get; }
        internal IEnumerable<FieldsCollection> Containors { get; }
        public int Number { get; private set; } = 0;
        public int PossibleNumbersCount => PossibleNumbers.Length;
        public bool HasNumber { get; private set; } = false;

        internal Field(int x, int y, IEnumerable<FieldsCollection> containors)
        {
            X = x;
            Y = y;
            Containors = containors;

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
            var fullArray = new int[9];

            for (int i = 1; i <= 9; i++)
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
