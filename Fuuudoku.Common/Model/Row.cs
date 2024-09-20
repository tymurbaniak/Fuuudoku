
using System.Diagnostics;

namespace Fuuudoku.Common.Model
{
    [DebuggerDisplay("{Fields}")]
    public class Row : FieldsCollection
    {
        private Field[] fields = new Field[9];

        public override IEnumerable<Field> Fields => fields;

        public override void Add(Field field)
        {
            fields[field.X] = field;
        }
    }
}
