using System.Text;
using Fuuudoku.Common.Model;

namespace Fuuudoku.Common
{
    public static class StringUtils
    {
        public static string PrintPossibleNumbersCounts(Board board)
        {
            var sb = new StringBuilder();
            var smallSquareSize = board.SmallSquareSize;
            var bigSquareSize = board.BigSquareSize;

            for (int sy = 0; sy < smallSquareSize; sy++)
            {
                for (int y = smallSquareSize * sy; y < (smallSquareSize * sy) + smallSquareSize; y++)
                {
                    var lineSb = new StringBuilder();
                    var fieldsLine = board.FieldsGrid[y];

                    for (int sx = 0; sx < smallSquareSize; sx++)
                    {
                        for (int x = smallSquareSize * sx; x < (smallSquareSize * sx) + smallSquareSize; x++)
                        {
                            lineSb.Append($"{fieldsLine[x].PossibleNumbersCount} ");
                        }

                        if (sx < smallSquareSize - 1)
                        {
                            lineSb.Append("| ");
                        }
                    }

                    sb.AppendLine(lineSb.ToString());
                }

                if (sy < smallSquareSize - 1)
                {
                    var separatorSb = new StringBuilder();

                    for (int x = 0; x < smallSquareSize + bigSquareSize; x++)
                    {
                        separatorSb.Append('-');
                    }

                    sb.AppendLine(separatorSb.ToString());
                }
            }

            return sb.ToString();
        }

        public static string PrintPossibleNumbers(Board board)
        {
            var sb = new StringBuilder();
            var smallSquareSize = board.SmallSquareSize;
            var bigSquareSize = board.BigSquareSize;
            var lineLength = (bigSquareSize * (smallSquareSize + 1)) + 1;

            for (int y = 0; y < 37; y++)
            {
                for (int x = 0; x < 37; x++)
                {
                    var numberY = y % (smallSquareSize + 1);
                    var numberX = x % (smallSquareSize + 1);
                    var squareY = y % (smallSquareSize + bigSquareSize);
                    var squareX = x % (smallSquareSize + bigSquareSize);
                    var fieldX = (int)Math.Floor((double)x / (smallSquareSize + 1));
                    var fieldY = (int)Math.Floor((double)y / (smallSquareSize + 1));

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

                    var field = board.FieldsGrid[fieldY][fieldX];

                    if (!field.HasNumber)
                    {
                        var possibleNumbers = field.GetFullArray();
                        var pNumberX = numberX - 1;
                        var pNumberY = numberY - 1;
                        var index = (pNumberY * smallSquareSize) + pNumberX;
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

        public static string PrintBoard(Board board)
        {
            var sb = new StringBuilder();
            var smallSquareSize = board.SmallSquareSize;
            var bigSquareSize = board.BigSquareSize;
            var numberSpace = board.FieldList.Select(f => f.Number.ToString().Length).Max();
            var sufixSpace = 1;
            var separatorSpace = 2;
            var lineLength = ((numberSpace + sufixSpace) * bigSquareSize + ((smallSquareSize - 1) * separatorSpace)) - 1;

            for (int sy = 0; sy < smallSquareSize; sy++)
            {
                for (int y = smallSquareSize * sy; y < smallSquareSize * sy + smallSquareSize; y++)
                {
                    var lineSb = new StringBuilder();
                    var fieldsLine = board.FieldsGrid[y];

                    for (int sx = 0; sx < smallSquareSize; sx++)
                    {
                        for (int x = smallSquareSize * sx; x < smallSquareSize * sx + smallSquareSize; x++)
                        {
                            var numberString = fieldsLine[x].Number.ToString();
                            var prefixSpaces = new string(' ', numberSpace - numberString.Length);
                            var sufixSpaces = new string(' ', sufixSpace);
                            lineSb.Append($"{prefixSpaces}{numberString}{sufixSpaces}");
                        }

                        if (sx < smallSquareSize - 1)
                        {
                            lineSb.Append("| ");
                        }
                    }

                    sb.AppendLine(lineSb.ToString());
                }

                if (sy < smallSquareSize - 1)
                {
                    var separatorSb = new StringBuilder();

                    for (int x = 0; x < lineLength; x++)
                    {
                        separatorSb.Append("-");
                    }

                    sb.AppendLine(separatorSb.ToString());
                }
            }

            return sb.ToString();
        }
    }
}
