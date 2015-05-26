namespace SudokuSMA
{
    class IndexesRowsColumnsSquares
    {
        public const int Dim = 9;
        public const int GridSize = Dim * Dim;
        private static int dimSqrt = (int)System.Math.Sqrt(Dim);
        private int[][] indexesRowsColumnsSquares;
        private int[][] rowsIndexes;
        private int[][] columnsIndexes;
        private int[][] squaresIndexes;

        public IndexesRowsColumnsSquares()
        {
            indexesRowsColumnsSquares = new int[GridSize][];

            for (int i = 0; i < indexesRowsColumnsSquares.Length; ++i)
            {
                indexesRowsColumnsSquares[i] = new int[3];
                int rowN = (i / Dim);
                int columnN = (i % Dim);
                int squareN = ((rowN / dimSqrt) * dimSqrt + (columnN / dimSqrt));
                indexesRowsColumnsSquares[i][0] = rowN;
                indexesRowsColumnsSquares[i][1] = columnN;
                indexesRowsColumnsSquares[i][2] = squareN;
            }

            rowsIndexes = new int[Dim][];

            for (int i = 0; i < rowsIndexes.Length; ++i)
            {
                rowsIndexes[i] = new int[Dim];
                for (int j = 0; j < rowsIndexes[i].Length; ++j)
                {
                    int indexInRowI = (i * Dim) + j;
                    rowsIndexes[i][j] = indexInRowI;
                }
            }

            columnsIndexes = new int[Dim][];

            for (int i = 0; i < columnsIndexes.Length; ++i)
            {
                columnsIndexes[i] = new int[Dim];
                for (int j = 0; j < columnsIndexes[i].Length; ++j)
                {
                    int indexInColumnI = i + j * Dim;
                    columnsIndexes[i][j] = indexInColumnI;
                }
            }

            squaresIndexes = new int[Dim][];

            for (int i = 0; i < squaresIndexes.Length; ++i)
            {
                squaresIndexes[i] = new int[Dim];
                int indexMin = (((i / dimSqrt) * (Dim * dimSqrt)) + ((i % dimSqrt) * dimSqrt));
                for (int j = 0; j < squaresIndexes[i].Length; ++j)
                {
                    int indexInSquareI = (indexMin + j % dimSqrt + (j / dimSqrt) * Dim);
                    squaresIndexes[i][j] = indexInSquareI;
                }

            }
        }

        public int GetRowN(int index)
        {
            return indexesRowsColumnsSquares[index][0];
        }

        public int GetColumnN(int index)
        {
            return indexesRowsColumnsSquares[index][1];
        }

        public int GetSquareN(int index)
        {
            return indexesRowsColumnsSquares[index][2];
        }

        public int[] IndexesInRow(int row)
        {
            int[] temp = new int[rowsIndexes[row].Length];
            System.Array.Copy(rowsIndexes[row], temp, rowsIndexes[row].Length);
            return temp;
        }

        public int[] IndexesInRowWithIndex(int index)
        {
            int row = indexesRowsColumnsSquares[index][0];
            int[] temp = new int[rowsIndexes[row].Length];
            System.Array.Copy(rowsIndexes[row], temp, rowsIndexes[row].Length);
            return temp;
        }

        public int[] IndexesInColumn(int column)
        {
            int[] temp = new int[columnsIndexes[column].Length];
            System.Array.Copy(columnsIndexes[column], temp, columnsIndexes[column].Length);
            return temp;
        }

        public int[] IndexesInColumnWithIndex(int index)
        {
            int column = indexesRowsColumnsSquares[index][1];
            int[] temp = new int[columnsIndexes[column].Length];
            System.Array.Copy(columnsIndexes[column], temp, columnsIndexes[column].Length);
            return temp;
        }

        public int[] IndexesInSquare(int square)
        {
            int[] temp = new int[squaresIndexes[square].Length];
            System.Array.Copy(squaresIndexes[square], temp, squaresIndexes[square].Length);
            return temp;
        }

        public int[] IndexesInSquareWithIndex(int index)
        {
            int square = indexesRowsColumnsSquares[index][2];
            int[] temp = new int[squaresIndexes[square].Length];
            System.Array.Copy(squaresIndexes[square], temp, squaresIndexes[square].Length);
            return temp;
        }



        // override functions.
        public override bool Equals(object obj)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return 3333;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
