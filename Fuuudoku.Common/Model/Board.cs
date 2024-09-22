using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("SudokuSolver.Tests")]

namespace Fuuudoku.Common.Model
{
    public class Board
    {
        private Field[][] fields;
        private Row[] rows;
        private Column[] columns;
        private Square[][] squares;

        public int SmallSquareSize { get; }
        public int BigSquareSize { get; }
        private IEnumerable<Field> fieldsList => fields.SelectMany(f => f);
        public bool IsSolved => CalculateIsSolved();
        public IEnumerable<Field> FieldList => fieldsList;
        public IEnumerable<Square> SquaresList => squares.SelectMany(s => s);
        internal IEnumerable<Row> Rows => rows;
        internal IEnumerable<Column> Columns => columns;
        public bool IsUnsolvable => CalculateIsUnsolvable();
        internal IReadOnlyList<IReadOnlyList<Field>> FieldsGrid => fields;
        public IReadOnlyList<FieldsCollection> SubCollections =>
            columns.Cast<FieldsCollection>()
            .Concat(rows.Cast<FieldsCollection>())
            .Concat(squares.SelectMany(s => s))
            .ToList();

        public Board(int[][] sudokuArray) : this(GetSizeFromArray(sudokuArray))
        {
            FillBoardWithKnownNumbers(sudokuArray);
        }

        private static int GetSizeFromArray(int[][] sudokuArray)
        {
            var smallSquareSize = Math.Sqrt(sudokuArray.Length);

            if (!double.IsInteger(smallSquareSize))
            {
                throw new Exception($"Square root of array length should be a positive integer");
            }

            return (int)smallSquareSize;
        }

        public Board(int smallSquareSize = 3)
        {
            this.SmallSquareSize = smallSquareSize;
            this.BigSquareSize = (int)Math.Pow(smallSquareSize, 2);
            this.fields = new Field[this.BigSquareSize][];
            this.rows = new Row[this.BigSquareSize];
            this.columns = new Column[this.BigSquareSize];
            this.squares = new Square[this.SmallSquareSize][];

            CreateEmptyBoard();
        }

        private void FillBoardWithKnownNumbers(int[][] sudokuArray)
        {
            for (int y = 0; y < this.BigSquareSize; y++)
            {
                var fieldsLine = fields[y];

                for (int x = 0; x < this.BigSquareSize; x++)
                {
                    var field = fieldsLine[x];
                    var number = sudokuArray[y][x];
                    field.SetNumber(number);
                }
            }
        }

        private void CreateEmptyBoard()
        {
            var possibleNumbers = new int[this.BigSquareSize];

            for (int i = 1; i <= this.BigSquareSize; i++)
            {
                possibleNumbers[i - 1] = i;
            }

            for (int y = 0; y < this.BigSquareSize; y++)
            {
                if (fields[y] == null)
                {
                    fields[y] = new Field[this.BigSquareSize];
                }

                var fieldsLine = fields[y];
                var row = GetRow(y);

                for (int x = 0; x < this.BigSquareSize; x++)
                {
                    var column = GetColumn(x);
                    var square = GetSquare(x, y);
                    var field = new Field(x, y, [row, column, square], possibleNumbers);
                    fieldsLine[x] = field;
                }
            }
        }

        private Row GetRow(int y)
        {
            if (rows[y] == null)
            {
                rows[y] = new Row(this.BigSquareSize);
            }

            return rows[y];
        }

        private Column GetColumn(int x)
        {
            if (columns[x] == null)
            {
                columns[x] = new Column(this.BigSquareSize);
            }

            return columns[x];
        }

        private Square GetSquare(int x, int y)
        {
            var sx = (int)Math.Floor((double)x / this.SmallSquareSize);
            var sy = (int)Math.Floor((double)y / this.SmallSquareSize);

            if (squares[sy] == null)
            {
                squares[sy] = new Square[this.SmallSquareSize];
            }

            if (squares[sy][sx] == null)
            {
                squares[sy][sx] = new Square(sx, sy, this.BigSquareSize);
            }

            return squares[sy][sx];
        }

        public int[][] ToArray()
        {
            var array = new int[this.BigSquareSize][];

            for (int y = 0; y < this.BigSquareSize; y++)
            {
                array[y] = new int[this.BigSquareSize];

                for (int x = 0; x < this.BigSquareSize; x++)
                {
                    array[y][x] = fields[y][x].Number;
                }
            }

            return array;
        }

        public int[][][] PossibleNumbersToArray()
        {
            var array = new int[this.BigSquareSize][][];

            for (int y = 0; y < this.BigSquareSize; y++)
            {
                array[y] = new int[this.BigSquareSize][];

                for (int x = 0; x < this.BigSquareSize; x++)
                {
                    array[y][x] = fields[y][x].PossibleNumbers;
                }
            }

            return array;
        }

        public IEnumerable<Square> GetVerticalyAlignedSquares(int sx, int sy)
        {
            return GetVerticalyAlignedSquares(sx, [sy]);
        }

        public IEnumerable<Square> GetVerticalyAlignedSquares(int sx, int[] exceptionList)
        {
            var alignedSquares = new List<Square>();

            for (int y = 0; y < squares.Length; y++)
            {
                if (exceptionList.Contains(y))
                {
                    continue;
                }

                alignedSquares.Add(squares[y][sx]);
            }

            return alignedSquares;
        }

        public IEnumerable<Square> GetHorizontalyAlignedSquares(int sy, int sx)
        {
            return GetHorizontalyAlignedSquares(sy, [sx]);
        }

        internal IEnumerable<Square> GetHorizontalyAlignedSquares(int sy, int[] exceptionList)
        {
            var alignedSquares = new List<Square>();

            for (int x = 0; x < squares[sy].Length; x++)
            {
                if (exceptionList.Contains(x))
                {
                    continue;
                }

                alignedSquares.Add(squares[sy][x]);
            }

            return alignedSquares;
        }

        private bool CalculateIsSolved()
        {
            var allFieldsFilled = fieldsList.All(f => f.HasNumber);

            if (!allFieldsFilled)
            {
                return false;
            }

            return AllCollectionsHaveDistinctNumbers();
        }

        private bool CalculateIsUnsolvable()
        {
            if (!AllCollectionsHaveDistinctNumbers())
            {
                return true;
            }

            var noPossibleNumbers = FieldList.All(f => f.PossibleNumbersCount == 0);
            var notAllFieldsHaveNumbers = FieldList.Any(f => !f.HasNumber);

            return noPossibleNumbers && notAllFieldsHaveNumbers;
        }

        private bool AllCollectionsHaveDistinctNumbers()
        {
            return this.SubCollections.All(c => c.AreAllDistinct());
        }
    }
}
