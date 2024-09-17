
using System.Diagnostics;

namespace SudokuSolver
{
    [DebuggerDisplay("{Fields}")]
    public class Column : FieldsCollection
    {
        private Field[] fields = new Field[9];

        public override IEnumerable<Field> Fields => this.fields;

        public override void Add(Field field)
        {
            this.fields[field.Y] = field;
        }
    }
}
