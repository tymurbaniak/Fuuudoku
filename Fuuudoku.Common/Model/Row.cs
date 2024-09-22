
using System.Diagnostics;

namespace Fuuudoku.Common.Model
{
    [DebuggerDisplay("Row: {Fields}")]
    public class Row(int collectionSize) : FieldsCollection(collectionSize)
    {
        private Field[] fields = new Field[collectionSize];

        public override IEnumerable<Field> Fields => fields;

        public override void Add(Field field)
        {
            fields[field.X] = field;
        }
    }
}
