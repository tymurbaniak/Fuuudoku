
using System.Diagnostics;

namespace Fuuudoku.Common.Model
{
    [DebuggerDisplay("Column: {Fields}")]
    public class Column(int collectionSize) : FieldsCollection(collectionSize)
    {
        private Field[] fields = new Field[collectionSize];

        public override IEnumerable<Field> Fields => fields;

        public override void Add(Field field)
        {
            fields[field.Y] = field;
        }
    }
}
