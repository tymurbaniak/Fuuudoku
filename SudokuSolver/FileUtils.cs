namespace SudokuSolver
{
    public static class FileUtils
    {
        public static int[][] ReadSudokuFromFile(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"There is no file in path: ${path}");

                return null;
            }

            var lines = File.ReadAllLines(path);
            List<int[]> numbers = new List<int[]>();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var numbersLine = new List<int>();

                foreach (char character in line)
                {
                    if (char.IsDigit(character))
                    {
                        numbersLine.Add((int)char.GetNumericValue(character));
                    }
                    else
                    {
                        numbersLine.Add(0);
                    }
                }

                numbers.Add(numbersLine.ToArray());
            }

            return numbers.ToArray();
        }
    }
}
