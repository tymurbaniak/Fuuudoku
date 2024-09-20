using Fuuudoku.Common.Model;

namespace Fuuudoku.Solver.Checkers
{
    internal static class PairsChecker
    {
        public static void Check(IEnumerable<FieldsCollection> fieldsCollections)
        {
            foreach (FieldsCollection fieldsCollection in fieldsCollections)
            {
                //if there are exactly 2 fields with same pair in square, all other possible numbers can be removed from those fields
                CheckFieldsCollection(fieldsCollection);
            }
        }

        private static void CheckFieldsCollection(FieldsCollection fieldsCollection)
        {
            for (int n1 = 1; n1 <= 9; n1++)
            {
                for (int n2 = 1; n2 <= 9; n2++)
                {
                    if (n1 == n2)
                    {
                        continue;
                    }

                    var fieldsWithFirstNumber = fieldsCollection.Fields.Where(f => f.PossibleNumbers.Contains(n1)).ToHashSet(new FieldsCoordsComparer());
                    var fieldsWithSecondNumber = fieldsCollection.Fields.Where(f => f.PossibleNumbers.Contains(n2)).ToHashSet(new FieldsCoordsComparer());

                    if (fieldsWithFirstNumber.Count() == 2 && fieldsWithSecondNumber.Count() == 2
                        && fieldsWithFirstNumber.SetEquals(fieldsWithSecondNumber))
                    {
                        foreach (var field in fieldsWithFirstNumber)
                        {
                            field.RemoveAllPossibleNumbersExcept([n1, n2]);
                        }
                    }
                }
            }
        }

        public class FieldsCoordsComparer : IEqualityComparer<Field>
        {
            public bool Equals(Field x, Field y)
            {
                return x.X == y.X && x.Y == y.Y;
            }

            public int GetHashCode(Field obj) => Math.Pow(obj.X, obj.Y).GetHashCode();
        }
    }
}
