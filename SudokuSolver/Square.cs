
using System.Diagnostics;
using System.Text;

namespace SudokuSolver
{
    [DebuggerDisplay("{Fields}")]
    public class Square : FieldsCollection
    {
        private Field[][] fields = new Field[3][];
        public int Sx { get; set; }
        public int Sy { get; set; }

        public override IEnumerable<Field> Fields => this.fields.SelectMany(f => f);

        public Square(int sx, int sy)
        {
            this.Sx = sx;
            this.Sy = sy;

            for (int i = 0; i < 3; i++)
            {
                this.fields[i] = new Field[3];
            }
        }

        public override void Add(Field field)
        {
            this.fields[field.Y - (this.Sy * 3)][field.X - (this.Sx * 3)] = field;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    var field = this.fields[y][x];
                    sb.Append($"{field.Number} ");
                }

                sb.Append('\n');
            }

            return sb.ToString();
        }

        public int[] GetSolvedColumns()
        {
            var columns = new int[this.fields[0].Length];
            Array.Fill(columns, 0);

            for (int y = 0; y < this.fields.Length; y++)
            {
                for (int x = 0; x < this.fields[y].Length; x++)
                {
                    if (!this.fields[y][x].HasNumber)
                    {
                        continue;
                    }

                    columns[x]++;
                }
            }

            var indexes = new List<int>();

            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i] != this.fields.Length)
                {
                    continue;
                }

                indexes.Add(i);
            }

            return indexes.ToArray();
        }

        public int[] GetSolvedRows()
        {
            var rows = new int[this.fields.Length];
            Array.Fill(rows, 0);

            for (int y = 0; y < this.fields.Length; y++)
            {
                for (int x = 0; x < this.fields[y].Length; x++)
                {
                    if (!this.fields[y][x].HasNumber)
                    {
                        continue;
                    }

                    rows[x]++;
                }
            }

            var indexes = new List<int>();

            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i] != this.fields.Length)
                {
                    continue;
                }

                indexes.Add(i);
            }

            return indexes.ToArray();
        }

        public Field[] GetSquareColumn(int xs)
        {
            var fields = new List<Field>();

            for (int y = 0; y < this.fields.Length; y++)
            {
                for (int x = 0; x < this.fields[y].Length; x++)
                {
                    if (xs != x)
                    {
                        continue;
                    }

                    fields.Add(this.fields[y][x]);
                }
            }

            return fields.ToArray();
        }

        public int GetOnlyPossibleNumberForColumn(int xs)
        {
            xs = (this.Sx * 3) + xs;

            for (int i = 1; i < 9; i++)
            {
                var fieldsWithSamePossibleNumbers = this.Fields
                    .Where(f => f.PossibleNumbers.Contains(i))
                    .ToArray();

                if (fieldsWithSamePossibleNumbers.Length <= 0 || fieldsWithSamePossibleNumbers.Length > 3)
                {
                    continue;
                }

                if (fieldsWithSamePossibleNumbers.All(f => f.X == xs))
                {
                    return i;
                }
            }

            return -1;
        }

        public int GetOnlyPossibleNumberForColumns(int[] xs)
        {
            foreach (var x in xs)
            {
                var onlyPossibleNumber = GetOnlyPossibleNumberForColumn(x);

                if (onlyPossibleNumber != -1)
                {
                    return onlyPossibleNumber;
                }
            }

            return -1;
        }

        public void RemovePossibleNumberFromColumn(int number, int x)
        {
            x = x % this.fields[0].Length;

            for (int y = 0; y < this.fields.Length; y++)
            {
                this.fields[y][x].RemoveFromPossibleNumbers(number);
            }
        }

        public int GetOnlyPossibleNumberForRow(int ys)
        {
            ys = (this.Sy * 3) + ys;

            for (int i = 1; i < 9; i++)
            {
                var fieldsWithSamePossibleNumbers = this.Fields
                    .Where(f => f.PossibleNumbers.Contains(i))
                    .ToArray();

                if (fieldsWithSamePossibleNumbers.Length <= 0 || fieldsWithSamePossibleNumbers.Length > 3)
                {
                    continue;
                }

                if (fieldsWithSamePossibleNumbers.All(f => f.Y == ys))
                {
                    return i;
                }
            }

            return -1;
        }

        internal void RemovePossibleNumberFromRow(int number, int y)
        {
            y = y % this.fields[0].Length;

            for (int x = 0; x < this.fields[y].Length; x++)
            {
                this.fields[y][x].RemoveFromPossibleNumbers(number);
            }
        }

        internal int[] GetColumnsWithDifferentNumbers(IEnumerable<int> excludedNumbers, int ignoredColumnX)
        {
            var columnsX = new List<int>();

            for (int x = 0; x < this.fields[0].Length; x++)
            {
                if (x == ignoredColumnX)
                {
                    continue;
                }

                var column = this.GetSquareColumn(x).Where(f => f.HasNumber);

                if (column.Select(f => f.Number).Except(excludedNumbers).Any())
                {
                    columnsX.Add(x);
                }
            }

            return columnsX.ToArray();
        }

        internal void RemovePossibleNumberFromAllColumnsExcept(int number, int[] excludedColumnIndexes)
        {
            for (int y = 0; y < this.fields.Length; y++)
            {
                for (int x = 0; x < this.fields[y].Length; x++)
                {
                    if (excludedColumnIndexes.Contains(x))
                    {
                        continue;
                    }

                    this.fields[y][x].RemoveFromPossibleNumbers(number);
                }
            }
        }

        internal int[] GetColumnsWithPossibleNumber(int n)
        {
            var fields = this.Fields.Where(f => f.PossibleNumbers.Contains(n));
            var xs = fields.Select(f => f.X).Distinct().ToArray();

            return xs;
        }
    }
}
