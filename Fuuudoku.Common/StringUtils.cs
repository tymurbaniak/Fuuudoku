using System.Text;
using Fuuudoku.Common.Model;

namespace Fuuudoku.Common
{
    internal static class StringUtils
    {
        public static string PrintPossibleNumbersCounts(Board board)
        {
            var sb = new StringBuilder();

            for (int sy = 0; sy < 3; sy++)
            {
                for (int y = 3 * sy; y < (3 * sy) + 3; y++)
                {
                    var lineSb = new StringBuilder();
                    var fieldsLine = board.FieldsGrid[y];

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

        public static string PrintPossibleNumbers(Board board)
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

                    var field = board.FieldsGrid[fieldY][fieldX];

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

        public static string PrintBoard(Board board)
        {
            var sb = new StringBuilder();

            for (int sy = 0; sy < 3; sy++)
            {
                for (int y = 3 * sy; y < 3 * sy + 3; y++)
                {
                    var lineSb = new StringBuilder();
                    var fieldsLine = board.FieldsGrid[y];

                    for (int sx = 0; sx < 3; sx++)
                    {
                        for (int x = 3 * sx; x < 3 * sx + 3; x++)
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
    }
}
