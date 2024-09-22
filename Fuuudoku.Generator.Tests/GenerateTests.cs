using Fuuudoku.Common;

namespace Fuuudoku.Generator.Tests
{
    public class GenerateTests
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void IncrementalGeneratorTest(int smallSquareSize)
        {
            var board = IncrementalGenerator.Generate(smallSquareSize);
            var boardString = StringUtils.PrintBoard(board);
            TestContext.Write(boardString);
        }
    }
}