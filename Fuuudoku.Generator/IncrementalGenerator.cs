using Fuuudoku.Common.Model;

namespace Fuuudoku.Generator
{
    public static class IncrementalGenerator
    {
        public static Board Generate(int smallSquareSize)
        {
            Board board = new Board(smallSquareSize);

            while (board.FieldList.Any(f => !f.HasNumber))
            {
                var field = board.FieldList
                    .Where(f => !f.HasNumber)
                    .OrderBy(f => f.PossibleNumbersCount)
                    .First();
                var number = field.PossibleNumbers.First();
                field.SetNumber(number);
            }

            return board;
        }
    }
}
