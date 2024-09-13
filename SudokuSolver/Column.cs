
using System.Diagnostics;

namespace SudokuSolver
{
    [DebuggerDisplay("{Fields}")]
    public class Column : IFieldsCollection
    {
        private Field[] fields = new Field[9];

        public IEnumerable<Field> Fields => this.fields;

        internal void AddField(Field field, int y)
        {
            this.fields[y] = field;
        }

        internal void RemoveNumberFromPossibleNumbers(int number)
        {
            foreach (var field in this.fields)
            {
                field.RemoveFromPossibleNumbers(number);
            }

            var setCandidates = this.fields.Where(f => f.PossibleNumbers.Length == 1);

            if (setCandidates.Count() == 1)
            {
                var setCandidate = setCandidates.First();
                setCandidate.SetNumber(setCandidate.PossibleNumbers[0]);
            }
        }

        public bool AreAllDistinct()
        {
            var fieldsWithNumber = this.Fields.Where(f => f.HasNumber);

            return fieldsWithNumber.Count() == fieldsWithNumber.DistinctBy(f => f.Number).Count();
        }
    }
}
