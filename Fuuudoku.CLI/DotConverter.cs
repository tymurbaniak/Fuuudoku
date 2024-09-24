using Fuuudoku.Common.Model;

namespace Fuuudoku.CLI
{
    public static class DotConverter
    {
        public static void Cvt(string dotString, out Board board)
        {
            Cvt(dotString, out int[][] array);
            board = new Board(array);
        }

        public static void Cvt(string dotString, out int[][] array)
        {
            int stringLength = dotString.Length;
            double sudokuSideLengthDouble = Math.Sqrt(stringLength);

            if (!double.IsInteger(sudokuSideLengthDouble))
            {
                throw new Exception("Provided dot string has wrong length - square root is not an integer");
            }

            int sudokuSideLength = (int)sudokuSideLengthDouble;

            if (sudokuSideLength > 81)
            {
                throw new Exception("Can't import sudoku with dot string, that has side length greater than 9");
            }

            array = new int[sudokuSideLength][];

            for (int y = 0; y < sudokuSideLength; y++)
            {
                array[y] = new int[sudokuSideLength];

                for (int x = 0; x < sudokuSideLength; x++)
                {
                    char symbol = dotString[x + (y * sudokuSideLength)];

                    if (char.IsDigit(symbol))
                    {
                        array[y][x] = (int)char.GetNumericValue(symbol);
                    }
                    else
                    {
                        array[y][x] = 0;
                    }
                }
            }
        }
    }
}
