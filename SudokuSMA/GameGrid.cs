using System.Collections.Generic;
using System.Security.Cryptography;

namespace SudokuSMA
{
    class GameGrid
    {
        private int gameGridFilled;
        private int gameGridCorrectFilled;
        private Cell[] grid;
        private HashSet<int>[] clearedNumbersInRows;
        private HashSet<int>[] clearedNumbersInColumns;
        private HashSet<int>[] clearedNumbersInSquares;
        private List<int>[] clearedIndexesInRows;
        private List<int>[] clearedIndexesInColumns;
        private List<int>[] clearedIndexesInSquares;

        private static RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider();
        private static IndexesRowsColumnsSquares iRCS = new IndexesRowsColumnsSquares();

        /// <summary>
        /// Return is the game grid filled.
        /// </summary>
        public bool IsGameGridFilled
        {
            get { return (gameGridFilled == SudokuClass.GridSize); }
        }

        /// <summary>
        /// Return is game grid solved.
        /// </summary>
        public bool IsGameGridSolved
        {
            get { return (gameGridCorrectFilled == SudokuClass.GridSize); }
        }

        public GameGrid() {
            gameGridFilled = gameGridCorrectFilled = 0;
            grid = NewArrayOfCells();
            clearedNumbersInRows = NewHashSetArrayOfSizeDim();
            clearedNumbersInColumns = NewHashSetArrayOfSizeDim();
            clearedNumbersInSquares = NewHashSetArrayOfSizeDim();
            clearedIndexesInRows = NewArrayOfListsWithSizeDim();
            clearedIndexesInColumns = NewArrayOfListsWithSizeDim();
            clearedIndexesInSquares = NewArrayOfListsWithSizeDim();
        }

        /// <summary>
        /// Create new array of Lists<int> with size SudokuClass.Dim.
        /// </summary>
        /// <returns>Return new array of Lists<int></returns>
        private static List<int>[] NewArrayOfListsWithSizeDim()
        {
            List<int>[] list = new List<int>[SudokuClass.Dim];
            for (int i = 0; i < list.Length; ++i)
            {
                list[i] = new List<int>(SudokuClass.Dim);
            }

            return list;
        }

        /// <summary>
        /// Create new array of HashSets<int> with size SudokuClass.Dim.
        /// </summary>
        /// <returns>Return new array of HashSets<int></returns>
        private static HashSet<int>[] NewHashSetArrayOfSizeDim()
        {
            HashSet<int>[] hashSet = new HashSet<int>[SudokuClass.Dim];
            for (int i = 0; i < hashSet.Length; ++i)
            {
                hashSet[i] = new HashSet<int>();
            }
            return hashSet;
        }

        /// <summary>
        /// Create new array of empty Cells with size SudokuClass.GridSize.
        /// </summary>
        /// <returns>Return new array of empty Cells.</returns>
        private static Cell[] NewArrayOfCells()
        {
            Cell[] grid = new Cell[SudokuClass.GridSize];
            for (int i = 0; i < grid.Length; ++i)
            {
                grid[i] = new Cell();
            }

            return grid;
        }

        public void NewGrid(int[] solution, int lvl)
        {
            int[] indexesToClear = GetGridIndexesShuffled();

            for (int i = 0; i < solution.Length; ++i)
            {
                grid[i].Set(solution[i]);
            }

            gameGridFilled = gameGridCorrectFilled = SudokuClass.GridSize;

            List<int> uniqueAfterCleared = new List<int>(SudokuClass.GridSize);
            for (int i = 0; i < 3; ++i)
            {
                int index = indexesToClear[i];
                ClearCellAt(index);
            }

            for (int i = 3; i < indexesToClear.Length; ++i)
            {
                int index = indexesToClear[i];
                if (!IsNotUniqueAfterClearCellAt(index))
                {
                    uniqueAfterCleared.Add(index);
                }
            }
            
            do
            {
                if (uniqueAfterCleared.Count != 0)
                {
                    int index = uniqueAfterCleared[0];
                    uniqueAfterCleared.RemoveAt(0);
                    ClearCellAt(index);
                    uniqueAfterCleared.RemoveAll(IsNotUniqueAfterClearCellAt);
                }
            } while (uniqueAfterCleared.Count != 0);
        }

        /// <summary>
        /// Check is grid not unique after clearing cell at index.
        /// </summary>
        /// <param name="index">The index of cell to clear.</param>
        /// <returns>Return true if grid is not unique.</returns>
        private bool IsNotUniqueAfterClearCellAt(int index)
        {
            int number = ClearCellAt(index);
            bool isSolved = SolveGrid();
            SetCellAtWith(index, number);

            return (!isSolved);
        }

        /// <summary>
        /// Solve grid.
        /// </summary>
        /// <returns>Return true if grid have only one solution.</returns>
        private bool SolveGrid()
        {
            Cell[] gridTemp = new Cell[SudokuClass.GridSize];
            for (int i = 0; i < grid.Length; ++i)
            {
                gridTemp[i] = new Cell(grid[i]);
            }
            int gameGridCorrectFilledTemp = gameGridCorrectFilled;

            SolveAllSingles();

            bool isSolved = false;
            if (gameGridCorrectFilled == SudokuClass.GridSize)
            {
                isSolved = true;
            }
            // TODO else recursiveSolve

            for (int i = 0; i < grid.Length; ++i)
            {
                grid[i] = new Cell(gridTemp[i]);
            }
            gameGridCorrectFilled = gameGridCorrectFilledTemp;

            return isSolved;
        }

        private void SolveAllSingles()
        {
            bool haveSet;
            do
            {
                haveSet = false;
                for (int i = 0; i < grid.Length; ++i)
                {
                    if (grid[i].CanSet())
                    {
                        haveSet = true;
                        ++gameGridCorrectFilled;
                        int number = grid[i].Number;
                        int[] inRow = iRCS.IndexesInRowWithIndex(i);
                        for (int j = 0; j < inRow.Length; ++j)
                        {
                            int index = inRow[j];
                            grid[index].Remove(number);
                        }

                        int[] inColumn = iRCS.IndexesInColumnWithIndex(i);
                        for (int j = 0; j < inColumn.Length; ++j)
                        {
                            int index = inColumn[j];
                            grid[index].Remove(number);
                        }

                        int[] inSquare = iRCS.IndexesInSquareWithIndex(i);
                        for (int j = 0; j < inSquare.Length; ++j)
                        {
                            int index = inSquare[j];
                            grid[index].Remove(number);
                        }
                    }
                }
            } while (haveSet);
        }

        /// <summary>
        /// Set solved number in the cell with index.
        /// </summary>
        /// <param name="index">The index of the cell.</param>
        /// <param name="number">The number of the cell.</param>
        private void SetCellAtWith(int index, int number)
        {
            System.Diagnostics.Debug.Assert(grid[index].SetOK(number));
            grid[index].Set(number);
            ++gameGridCorrectFilled;
            int rowN = iRCS.GetRowN(index);
            clearedNumbersInRows[rowN].Remove(number);
            clearedIndexesInRows[rowN].Remove(index);
            int columnN = iRCS.GetColumnN(index);
            clearedNumbersInColumns[columnN].Remove(number);
            clearedIndexesInColumns[columnN].Remove(index);
            int squareN = iRCS.GetSquareN(index);
            clearedNumbersInSquares[squareN].Remove(number);
            clearedIndexesInSquares[squareN].Remove(index);
        }

        /// <summary>
        /// Clear cell at index and add all posible numbers in that cell.
        /// </summary>
        /// <param name="index">The index of cell to clear</param>
        /// <returns>Return the solved number in the cell.</returns>
        private int ClearCellAt(int index)
        {
            int number = grid[index].Number;
            int row = iRCS.GetRowN(index);
            clearedNumbersInRows[row].Add(number);
            int column = iRCS.GetColumnN(index);
            clearedNumbersInColumns[column].Add(number);
            int square = iRCS.GetSquareN(index);
            clearedNumbersInSquares[square].Add(number);
            --gameGridCorrectFilled;
            
            foreach (int idx in clearedIndexesInRows[row])
            {
                int columnTemp = iRCS.GetColumnN(idx);
                int squareTemp = iRCS.GetSquareN(idx);
                HashSet<int> canHave = new HashSet<int>();
                canHave.UnionWith(clearedNumbersInRows[row]);
                canHave.IntersectWith(clearedNumbersInColumns[columnTemp]);
                canHave.IntersectWith(clearedNumbersInSquares[squareTemp]);
                grid[idx].Add(canHave);
            }

            foreach (int idx in clearedIndexesInColumns[column])
            {
                int rowTemp = iRCS.GetRowN(idx);
                int squareTemp = iRCS.GetSquareN(idx);
                HashSet<int> canHave = new HashSet<int>();
                canHave.UnionWith(clearedNumbersInColumns[column]);
                canHave.IntersectWith(clearedNumbersInRows[rowTemp]);
                canHave.IntersectWith(clearedNumbersInSquares[squareTemp]);
                grid[idx].Add(canHave);
            }

            foreach (int idx in clearedIndexesInSquares[square])
            {
                int rowTemp = iRCS.GetRowN(idx);
                int columnTemp = iRCS.GetColumnN(idx);
                HashSet<int> canHave = new HashSet<int>();
                canHave.UnionWith(clearedNumbersInSquares[square]);
                canHave.IntersectWith(clearedNumbersInRows[rowTemp]);
                canHave.IntersectWith(clearedNumbersInColumns[columnTemp]);
                grid[idx].Add(canHave);
            }

            clearedIndexesInRows[row].Add(index);
            clearedIndexesInColumns[column].Add(index);
            clearedIndexesInSquares[square].Add(index);
            grid[index].Add();

            return number;
        }

        /// <summary>
        /// Greate new array of ints with numbers from zero to SudokuClass.GridSize.
        /// Zeto is included, SudokuClass.GridSize is exluded.
        /// Shuffles it with Fisher–Yates shuffle.
        /// </summary>
        /// <returns>Return the shuffled array.</returns>
        private static int[] GetGridIndexesShuffled()
        {
            int[] indexes = new int[SudokuClass.GridSize];
            for (int i = 0; i < indexes.Length; ++i)
            {
                indexes[i] = i;
            }

            for (int i = 0; i < indexes.Length; ++i)
            {
                int j = GetNextFrom0ToGridSize(rnd);
                int temp = indexes[i];
                indexes[i] = indexes[j];
                indexes[j] = temp;
            }

            return indexes;
        }

        /// <summary>
        /// Return random number from zero to SudokuClass.GridSize. 
        /// Zero is included, SudokuClass.GridSize is excluded.
        /// </summary>
        /// <param name="rnd">Random Number Generator.</param>
        /// <returns>Return random number from zero to SudokuClass.GridSize. 
        /// Zero is included, SudokuClass.GridSize is excluded.</returns>
        private static int GetNextFrom0ToGridSize(RNGCryptoServiceProvider rnd)
        {
            byte[] randomByte = new byte[1];
            do
            {
                rnd.GetBytes(randomByte);
            } while (randomByte[0] > SudokuClass.ZeroToGridSize);

            return (randomByte[0] % SudokuClass.GridSize);
        }



        // override functions.
        public override bool Equals(object obj)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return 333333;
        }

        public override string ToString()
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder(SudokuClass.GridSize);
            for (int i = 0; i < grid.Length; ++i)
            {
                s.Append(grid[i].ToString());
            }
            return s.ToString();
        }
    }
}
