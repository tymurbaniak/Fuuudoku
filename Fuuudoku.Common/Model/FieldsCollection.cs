namespace Fuuudoku.Common.Model
{
    public abstract class FieldsCollection
    {
        public abstract IEnumerable<Field> Fields { get; }
        public int CollectionSize { get; }


        public FieldsCollection(int collectionSize)
        {
            this.CollectionSize = collectionSize;
        }

        public abstract void Add(Field field);
        public void RemoveNumberFromPossibleNumbers(int number)
        {
            foreach (var field in Fields)
            {
                field.RemoveFromPossibleNumbers(number);
            }

            var setCandidates = Fields.Where(f => f.PossibleNumbers.Length == 1);

            if (setCandidates.Count() == 1)
            {
                var setCandidate = setCandidates.First();
                setCandidate.SetNumber(setCandidate.PossibleNumbers[0]);
            }
        }

        internal bool AreAllDistinct()
        {
            var fieldsWithNumber = Fields.Where(f => f.HasNumber);

            return fieldsWithNumber.Count() == fieldsWithNumber.DistinctBy(f => f.Number).Count();
        }
    }
}
