namespace SudokuSolver.Tests
{
    public class SolveTests
    {
        [TestCaseSource(nameof(TestCases))]
        public void SolveSudokuTest((int[][] sudoku, int[][] solvedSudoku) tc)
        {
            var solver = new Solver();
            var board = solver.Solve(tc.sudoku);

            TestContext.Write(board.ToString());
            CollectionAssert.AreEqual(tc.solvedSudoku, board.ToArray());
        }

        private static int[][] sudoku1 =
            [
                [0, 0, 0, 0, 0, 0, 0, 0, 0],
                [6, 0, 0, 0, 0, 9, 1, 0, 0],
                [0, 0, 7, 0, 0, 5, 0, 0, 2],
                [0, 0, 8, 0, 0, 2, 0, 0, 9],
                [0, 0, 6, 0, 9, 0, 0, 3, 0],
                [0, 0, 0, 3, 4, 0, 0, 0, 0],
                [1, 0, 0, 5, 0, 4, 0, 2, 0],
                [5, 0, 0, 0, 0, 6, 0, 0, 7],
                [4, 0, 0, 0, 0, 0, 0, 0, 3]
            ];

        private static int[][] solvedSudoku1 =
            [
                [2, 5, 1, 8, 6, 3, 9, 7, 4],
                [6, 3, 4, 2, 7, 9, 1, 5, 8],
                [8, 9, 7, 4, 1, 5, 3, 6, 2],
                [3, 1, 8, 6, 5, 2, 7, 4, 9],
                [7, 4, 6, 1, 9, 8, 2, 3, 5],
                [9, 2, 5, 3, 4, 7, 6, 8, 1],
                [1, 7, 9, 5, 3, 4, 8, 2, 6],
                [5, 8, 3, 9, 2, 6, 4, 1, 7],
                [4, 6, 2, 7, 8, 1, 5, 9, 3]
            ];

        private static int[][] sudoku2 =
            [
                [7, 5, 0, 0, 8, 0, 0, 0, 0],
                [0, 0, 8, 3, 0, 0, 7, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0],
                [1, 8, 5, 0, 0, 0, 0, 2, 0],
                [0, 6, 0, 0, 1, 0, 0, 0, 8],
                [0, 0, 0, 9, 2, 0, 0, 0, 0],
                [0, 0, 0, 6, 0, 0, 9, 0, 0],
                [0, 0, 0, 5, 0, 9, 0, 1, 3],
                [0, 4, 0, 0, 0, 0, 0, 0, 0]
            ];

        private static int[][] solvedSudoku2 =
            [

            ];

        public static IEnumerable<(int[][], int[][])> TestCases
        {
            get
            {
                yield return (sudoku1, solvedSudoku1);
                //yield return (sudoku2, solvedSudoku2);
            }
        }
    }
}
