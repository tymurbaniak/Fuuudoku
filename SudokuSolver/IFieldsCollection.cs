namespace SudokuSolver
{
    internal interface IFieldsCollection
    {
        IEnumerable<Field> Fields { get; }
        bool AreAllDistinct();
    }
}
