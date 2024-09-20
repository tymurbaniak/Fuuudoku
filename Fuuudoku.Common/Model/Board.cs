using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleToAttribute("SudokuSolver.Tests")]

namespace Fuuudoku.Common.Model
{
    public class Board
    {
        private Field[][] fields = new Field[9][];
        private Row[] rows = new Row[9];
        private Column[] columns = new Column[9];
        private Square[][] squares = new Square[3][];
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

        public Board(int[][] sudokuArray)
        {
            Load(sudokuArray);
        }

        private void FillBoardWithKnownNumbers(int[][] sudokuArray)
        {
            for (int y = 0; y < 9; y++)
            {
                var fieldsLine = fields[y];

                for (int x = 0; x < 9; x++)
                {
                    var field = fieldsLine[x];
                    var number = sudokuArray[y][x];
                    field.SetNumber(number);
                }
            }
        }

        private void CreateEmptyBoard()
        {
            for (int y = 0; y < 9; y++)
            {
                if (fields[y] == null)
                {
                    fields[y] = new Field[9];
                }

                var fieldsLine = fields[y];
                var row = GetRow(y);

                for (int x = 0; x < 9; x++)
                {
                    var column = GetColumn(x);
                    var square = GetSquare(x, y);
                    var field = new Field(x, y, [row, column, square]);
                    fieldsLine[x] = field;
                }
            }
        }

        private Row GetRow(int y)
        {
            if (rows[y] == null)
            {
                rows[y] = new Row();
            }

            return rows[y];
        }

        private Column GetColumn(int x)
        {
            if (columns[x] == null)
            {
                columns[x] = new Column();
            }

            return columns[x];
        }

        private Square GetSquare(int x, int y)
        {
            var sx = (int)Math.Floor((double)x / 3);
            var sy = (int)Math.Floor((double)y / 3);

            if (squares[sy] == null)
            {
                squares[sy] = new Square[3];
            }

            if (squares[sy][sx] == null)
            {
                squares[sy][sx] = new Square(sx, sy);
            }

            return squares[sy][sx];
        }

        public int[][] ToArray()
        {
            var array = new int[9][];

            for (int y = 0; y < 9; y++)
            {
                array[y] = new int[9];

                for (int x = 0; x < 9; x++)
                {
                    array[y][x] = fields[y][x].Number;
                }
            }

            return array;
        }

        public int[][][] PossibleNumbersToArray()
        {
            var array = new int[9][][];

            for (int y = 0; y < 9; y++)
            {
                array[y] = new int[9][];

                for (int x = 0; x < 9; x++)
                {
                    array[y][x] = fields[y][x].PossibleNumbers;
                }
            }

            return array;
        }

        internal void Load(int[][] sudokuArray)
        {
            CreateEmptyBoard();
            FillBoardWithKnownNumbers(sudokuArray);
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

            var areRowsOk = AllCollectionsHaveDistinctNumbers(Rows);
            var areColumnsOk = AllCollectionsHaveDistinctNumbers(Columns);
            var areSquaresOk = AllCollectionsHaveDistinctNumbers(SquaresList);

            return areRowsOk && areColumnsOk && areSquaresOk;
        }

        private bool CalculateIsUnsolvable()
        {
            var areRowsOk = AllCollectionsHaveDistinctNumbers(Rows);
            var areColumnsOk = AllCollectionsHaveDistinctNumbers(Columns);
            var areSquaresOk = AllCollectionsHaveDistinctNumbers(SquaresList);

            if (!areRowsOk || !areColumnsOk || !areSquaresOk)
            {
                return true;
            }

            var noPossibleNumbers = FieldList.All(f => f.PossibleNumbersCount == 0);
            var notAllFieldsHaveNumbers = FieldList.Any(f => !f.HasNumber);

            return noPossibleNumbers && notAllFieldsHaveNumbers;
        }

        private bool AllCollectionsHaveDistinctNumbers(IEnumerable<FieldsCollection> collection)
        {
            return collection.All(c => c.AreAllDistinct());
        }
    }
}
