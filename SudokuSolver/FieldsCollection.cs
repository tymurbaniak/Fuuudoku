namespace SudokuSolver
{
    public abstract class FieldsCollection : IFieldsCollection
    {
        public abstract IEnumerable<Field> Fields { get; }
        public abstract void Add(Field field);
        public void RemoveNumberFromPossibleNumbers(int number)
        {
            foreach (var field in this.Fields)
            {
                field.RemoveFromPossibleNumbers(number);
            }

            var setCandidates = this.Fields.Where(f => f.PossibleNumbers.Length == 1);

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
