
using System.Diagnostics;

namespace Fuuudoku.Common.Model
{
    [DebuggerDisplay("Square: {Fields}")]
    public class Square : FieldsCollection
    {
        private Field[][] fields;
        public int Sx { get; set; }
        public int Sy { get; set; }
        public int SquareSize { get; }

        public override IEnumerable<Field> Fields => fields.SelectMany(f => f);

        internal Square(int sx, int sy, int collectionSize) : base(collectionSize)
        {
            Sx = sx;
            Sy = sy;
            this.SquareSize = (int)Math.Sqrt(collectionSize);
            this.fields = new Field[SquareSize][];

            for (int i = 0; i < SquareSize; i++)
            {
                fields[i] = new Field[SquareSize];
            }
        }

        public override void Add(Field field)
        {
            fields[field.Y - Sy * this.SquareSize][field.X - Sx * this.SquareSize] = field;
        }

        public void RemovePossibleNumberFromColumn(int number, int x)
        {
            x = x % fields[0].Length;

            for (int y = 0; y < fields.Length; y++)
            {
                fields[y][x].RemoveFromPossibleNumbers(number);
            }
        }

        public void RemovePossibleNumberFromRow(int number, int y)
        {
            y = y % fields[0].Length;

            for (int x = 0; x < fields[y].Length; x++)
            {
                fields[y][x].RemoveFromPossibleNumbers(number);
            }
        }

        public int[] GetColumnsWithPossibleNumber(int n)
        {
            var fields = Fields.Where(f => f.PossibleNumbers.Contains(n));
            var xs = fields.Select(f => f.X).Distinct().ToArray();

            return xs;
        }
    }
}
