
namespace SudokuSolver
{
    public class Row : IFieldsCollection
    {
        private Field[] fields = new Field[9];

        public IEnumerable<Field> Fields => this.fields;

        internal void AddField(Field field, int x)
        {
            this.fields[x] = field;
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
    }
}
