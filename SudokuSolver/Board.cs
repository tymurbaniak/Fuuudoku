using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public class Board
    {
        private Field[][] fields = new Field[9][];
        private Row[] rows = new Row[9];
        private Column[] columns = new Column[9];
        private Square[][] squares = new Square[3][];
        private IEnumerable<Field> fieldsList => this.fields.SelectMany(f => f);

        public bool IsSolved => this.CalculateIsSolved();
        public int SolvedFieldsCount => this.fieldsList.Count(f => f.HasNumber);
        public IEnumerable<Field> FieldList => this.fieldsList;
        public IEnumerable<Square> SquaresList => this.squares.SelectMany(s => s);
        public IEnumerable<Row> Rows => this.rows;
        public IEnumerable<Column> Columns => this.columns;
        public bool IsUnsolvable => CalculateIsUnsolvable();

        public Board(int[][] sudokuArray)
        {
            this.Load(sudokuArray);
        }

        public Board(int[][] numbersArray, int[][][] possibleNumbersArray)
        {
            this.Load(numbersArray);
            this.LoadPossibleNumbers(possibleNumbersArray);
        }

        private void LoadPossibleNumbers(int[][][] possibleNumbersArray)
        {
            for (int y = 0; y < 9; y++)
            {
                var fieldsLine = this.fields[y];

                for (int x = 0; x < 9; x++)
                {
                    var field = fieldsLine[x];
                    var possibleNumbers = possibleNumbersArray[y][x];
                    field.SetPossibleNumbers(possibleNumbers);
                }
            }
        }

        private void FillBoardWithKnownNumbers(int[][] sudokuArray)
        {
            for (int y = 0; y < 9; y++)
            {
                var fieldsLine = this.fields[y];

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
                if (this.fields[y] == null)
                {
                    this.fields[y] = new Field[9];
                }

                var fieldsLine = this.fields[y];
                var row = this.GetRow(y);

                for (int x = 0; x < 9; x++)
                {
                    var field = new Field(x, y);
                    var column = GetColumn(x);
                    var square = GetSquare(x, y);
                    field.SetSquare(square);
                    field.SetColumn(column);
                    field.SetRow(row);

                    fieldsLine[x] = field;
                    row.AddField(field, x);
                    column.AddField(field, y);
                    square.AddField(field, x, y);
                }
            }
        }

        private Row GetRow(int y)
        {
            if (this.rows[y] == null)
            {
                this.rows[y] = new Row();
            }

            return this.rows[y];
        }

        private Column GetColumn(int x)
        {
            if (this.columns[x] == null)
            {
                this.columns[x] = new Column();
            }

            return this.columns[x];
        }

        private Square GetSquare(int x, int y)
        {
            var sx = (int)Math.Floor((double)x / 3);
            var sy = (int)Math.Floor((double)y / 3);

            if (this.squares[sy] == null)
            {
                this.squares[sy] = new Square[3];
            }

            if (this.squares[sy][sx] == null)
            {
                this.squares[sy][sx] = new Square(sx, sy);
            }

            return this.squares[sy][sx];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int sy = 0; sy < 3; sy++)
            {
                for (int y = 3 * sy; y < (3 * sy) + 3; y++)
                {
                    var lineSb = new StringBuilder();
                    var fieldsLine = this.fields[y];

                    for (int sx = 0; sx < 3; sx++)
                    {
                        for (int x = 3 * sx; x < (3 * sx) + 3; x++)
                        {
                            lineSb.Append($"{fieldsLine[x].Number} ");
                        }

                        if (sx < 3 - 1)
                        {
                            lineSb.Append("| ");
                        }
                    }

                    sb.AppendLine(lineSb.ToString());
                }

                if (sy < 3 - 1)
                {
                    sb.AppendLine("---------------------");
                }
            }

            return sb.ToString();
        }

        public string PrintPossibleCounts()
        {
            var sb = new StringBuilder();

            for (int sy = 0; sy < 3; sy++)
            {
                for (int y = 3 * sy; y < (3 * sy) + 3; y++)
                {
                    var lineSb = new StringBuilder();
                    var fieldsLine = this.fields[y];

                    for (int sx = 0; sx < 3; sx++)
                    {
                        for (int x = 3 * sx; x < (3 * sx) + 3; x++)
                        {
                            lineSb.Append($"{fieldsLine[x].PossibleNumbersCount} ");
                        }

                        if (sx < 3 - 1)
                        {
                            lineSb.Append("| ");
                        }
                    }

                    sb.AppendLine(lineSb.ToString());
                }

                if (sy < 3 - 1)
                {
                    sb.AppendLine("---------------------");
                }
            }

            return sb.ToString();
        }

        public string PrintPossibleNumbers()
        {
            var sb = new StringBuilder();

            for (int y = 0; y < 37; y++)
            {
                for (int x = 0; x < 37; x++)
                {
                    var numberY = y % 4;
                    var numberX = x % 4;
                    var squareY = y % 12;
                    var squareX = x % 12;
                    var fieldX = (int)Math.Floor((double)x / 4);
                    var fieldY = (int)Math.Floor((double)y / 4);

                    if (squareY == 0)
                    {
                        sb.Append("##");
                        continue;
                    }

                    if (squareX == 0)
                    {
                        sb.Append("# ");
                        continue;
                    }

                    if (numberY == 0)
                    {
                        sb.Append("--");
                        continue;
                    }

                    if (numberX == 0)
                    {
                        sb.Append("| ");
                        continue;
                    }

                    var field = this.fields[fieldY][fieldX];

                    if (!field.HasNumber)
                    {
                        var possibleNumbers = field.GetFullArray();
                        var pNumberX = numberX - 1;
                        var pNumberY = numberY - 1;
                        var index = (pNumberY * 3) + pNumberX;
                        var pNumber = possibleNumbers[index];
                        if (pNumber == 0)
                        {
                            sb.Append("  ");
                        }
                        else
                        {
                            sb.Append($"{pNumber} ");
                        }
                    }
                    else
                    {
                        sb.Append("  ");
                    }
                }

                sb.Append('\n');
            }

            return sb.ToString();
        }

        public int[][] ToArray()
        {
            var array = new int[9][];

            for (int y = 0; y < 9; y++)
            {
                array[y] = new int[9];

                for (int x = 0; x < 9; x++)
                {
                    array[y][x] = this.fields[y][x].Number;
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
                    array[y][x] = this.fields[y][x].PossibleNumbers;
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

            for (int y = 0; y < this.squares.Length; y++)
            {
                if (exceptionList.Contains(y))
                {
                    continue;
                }

                alignedSquares.Add(this.squares[y][sx]);
            }

            return alignedSquares;
        }

        public IEnumerable<Square> GetHorizontalyAlignedSquares(int sy, int sx)
        {
            return GetHorizontalyAlignedSquares(sy, [sx]);
        }

        public IEnumerable<Square> GetHorizontalyAlignedSquares(int sy, int[] exceptionList)
        {
            var alignedSquares = new List<Square>();

            for (int x = 0; x < this.squares[sy].Length; x++)
            {
                if (exceptionList.Contains(x))
                {
                    continue;
                }

                alignedSquares.Add(this.squares[sy][x]);
            }

            return alignedSquares;
        }

        private bool CalculateIsSolved()
        {
            var allFieldsFilled = this.fieldsList.All(f => f.HasNumber);

            if (!allFieldsFilled)
            {
                return false;
            }

            var areRowsOk = AllCollectionsHaveDistinctNumbers(this.Rows);
            var areColumnsOk = AllCollectionsHaveDistinctNumbers(this.Columns);
            var areSquaresOk = AllCollectionsHaveDistinctNumbers(this.SquaresList);

            return areRowsOk && areColumnsOk && areSquaresOk;
        }

        private bool CalculateIsUnsolvable()
        {
            var areRowsOk = AllCollectionsHaveDistinctNumbers(this.Rows);
            var areColumnsOk = AllCollectionsHaveDistinctNumbers(this.Columns);
            var areSquaresOk = AllCollectionsHaveDistinctNumbers(this.SquaresList);

            if (!areRowsOk || !areColumnsOk || !areSquaresOk)
            {
                return true;
            }

            var noPossibleNumbers = this.FieldList.All(f => f.PossibleNumbersCount == 0);
            var notAllFieldsHaveNumbers = this.FieldList.Any(f => !f.HasNumber);

            return noPossibleNumbers && notAllFieldsHaveNumbers;
        }

        private bool AllCollectionsHaveDistinctNumbers(IEnumerable<IFieldsCollection> collection)
        {
            return collection.All(c => c.AreAllDistinct());
        }
    }
}
