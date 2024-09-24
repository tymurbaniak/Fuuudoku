using Fuuudoku.Common.Model;

namespace Fuuudoku.CLI.Tests
{
    public class DotConverterTests
    {
        [Test]
        public void ConvertDotStringToSudoku()
        {
            var dotString = ".9.2..48...68..5....34............34.3...5..1..1..79....9..68...........2..5..1..";
            int[][] expectedArray = [
                    [0, 9, 0, 2, 0, 0, 4, 8, 0],
                    [0, 0, 6, 8, 0, 0, 5, 0, 0],
                    [0, 0, 3, 4, 0, 0, 0, 0, 0],
                    [0, 0, 0, 0, 0, 0, 0, 3, 4],
                    [0, 3, 0, 0, 0, 5, 0, 0, 1],
                    [0, 0, 1, 0, 0, 7, 9, 0, 0],
                    [0, 0, 9, 0, 0, 6, 8, 0, 0],
                    [0, 0, 0, 0, 0, 0, 0, 0, 0],
                    [2, 0, 0, 5, 0, 0, 1, 0, 0]
                ];
            DotConverter.Cvt(dotString, out Board board);
            CollectionAssert.AreEqual(expectedArray, board.ToArray());
        }
    }
}