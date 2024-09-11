namespace SudokuSolver.Tests
{
    public class FileUtilsTest
    {
        private int[][] sudokuArray;
        string path = string.Empty;

        [SetUp]
        public void Setup()
        {
            this.sudokuArray = new int[][]
            {
                new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                new int[] { 4, 5, 6, 7, 8, 9, 1, 2, 3 },
                new int[] { 7, 8, 9, 1, 2, 3, 4, 5, 6 },
                new int[] { 9, 1, 2, 3, 4, 5, 6, 7, 8 },
                new int[] { 6, 7, 8, 9, 1, 2, 3, 4, 5 },
                new int[] { 3, 4, 5, 6, 7, 8, 9, 1, 2 },
                new int[] { 8, 9, 1, 2, 3, 4, 5, 6, 7 },
                new int[] { 5, 6, 7, 8, 9, 1, 2, 3, 4 },
                new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 1 }
            };

            this.path = $"{AppDomain.CurrentDomain.BaseDirectory}{Path.PathSeparator}testSudoku";
            var content = string.Join("\n", sudokuArray.Select(l => string.Join("", l)));
            File.WriteAllText(this.path, content);
        }

        [Test]
        public void ReadFileTest()
        {
            var numbersArray = FileUtils.ReadSudokuFromFile(this.path);
            var areAllNumbersEqual = true;

            for (int y = 0; y < numbersArray.Length; y++)
            {
                var numbersLine = numbersArray[y];
                var sudokuLine = this.sudokuArray[y];

                for (int x = 0; x < numbersLine.Length; x++)
                {
                    if (numbersLine[x] != sudokuLine[x])
                    {
                        areAllNumbersEqual = false;
                        break;
                    }
                }
            }

            Assert.That(areAllNumbersEqual, Is.True);
        }
    }
}
