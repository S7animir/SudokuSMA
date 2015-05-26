using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SudokuSMA
{
    class SolutionGrid
    {
        private int[] grid;
        private HashSet<int>[] numbersInRows;
        private HashSet<int>[] numbersInColumns;
        private HashSet<int>[] numbersInSquares;
        private static RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider();
        private static IndexesRowsColumnsSquares iRCS = new IndexesRowsColumnsSquares();

        public SolutionGrid()
        {
            grid = new int[SudokuClass.GridSize];
            numbersInRows = NewHashSetArrayOfSizeDim();
            numbersInColumns = NewHashSetArrayOfSizeDim();
            numbersInSquares = NewHashSetArrayOfSizeDim();
        }

        /// <summary>
        /// Get grid copy.
        /// </summary>
        public int[] Grid
        {
            get
            {
                int[] temp = new int[grid.Length];
                Array.Copy(grid, temp, grid.Length);
                return temp;
            }
        }

        /// <summary>
        /// Create new array of HashSets<int> with size Dim.
        /// </summary>
        /// <returns>Return the created array.</returns>
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
        /// Create new solution grid.
        /// </summary>
        public void NewGrid()
        {
            ResetNumbersInRCS();
            FillInGrid(0);
        }

        /// <summary>
        /// Fill numbersInRows, numbersInColumns 
        /// and numbersInSquares with numbers from 1 to Dim included.
        /// </summary>
        private void ResetNumbersInRCS()
        {
            for (int i = 0; i < numbersInRows.Length; ++i)
            {
                for (int number = 1; number <= SudokuClass.Dim; ++number)
                {
                    numbersInRows[i].Add(number);
                }
            }

            for (int i = 0; i < numbersInColumns.Length; ++i)
            {
                for (int number = 1; number <= SudokuClass.Dim; ++number)
                {
                    numbersInColumns[i].Add(number);
                }
            }

            for (int i = 0; i < numbersInSquares.Length; ++i)
            {
                for (int number = 1; number <= SudokuClass.Dim; ++number)
                {
                    numbersInSquares[i].Add(number);
                }
            }
        }

        /// <summary>
        /// Recursive function to fill grid corect.
        /// </summary>
        /// <param name="index">The index in grid that is used in current recursion.</param>
        /// <returns></returns>
        private bool FillInGrid(int index)
        {
            int rowN = iRCS.GetRowN(index);
            int columnN = iRCS.GetColumnN(index);
            int squareN = iRCS.GetSquareN(index);

            int[] oneToDim = OneToDimShuffled();
            for (int i = 0; i < oneToDim.Length; ++i)
            {
                int number = oneToDim[i];
                if (CanSet(number, rowN, columnN, squareN))
                {
                    grid[index] = number;
                    RemoveNumber(number, rowN, columnN, squareN);
                    ++index;
                    if (index == SudokuClass.GridSize || FillInGrid(index))
                    {
                        return true;
                    }
                    --index;
                    AddNumber(number, rowN, columnN, squareN);
                }
            }

            return false;
        }

        /// <summary>
        /// Add number in numbersInRows[row], numbersInColumns[column] 
        /// and numbersInSquares[square].
        /// </summary>
        /// <param name="number">The number to add.</param>
        /// <param name="row">The row in numbersInRows.</param>
        /// <param name="column">The column in numbersInColumns.</param>
        /// <param name="square">The square in numbersInSquares.</param>
        private void AddNumber(int number, int row, int column, int square)
        {
            numbersInRows[row].Add(number);
            numbersInColumns[column].Add(number);
            numbersInSquares[square].Add(number);
        }

        /// <summary>
        /// Remove number from numbersInRows[row], numbersInColumns[column] 
        /// and numbersInSquares[square].
        /// </summary>
        /// <param name="number">The number to remove.</param>
        /// <param name="row">The row in numbersInRows.</param>
        /// <param name="column">The column in numbersInColumns.</param>
        /// <param name="square">The square in numbersInSquares.</param>
        private void RemoveNumber(int number, int row, int column, int square)
        {
            numbersInRows[row].Remove(number);
            numbersInColumns[column].Remove(number);
            numbersInSquares[square].Remove(number);
        }

        /// <summary>
        /// Check if numbersInRows[row], numbersInColumns[column] 
        /// and numbersInSquares[square] contains number.
        /// </summary>
        /// <param name="number">The number to be checked.</param>
        /// <param name="row">The row in numbersInRows.</param>
        /// <param name="column">The column in numbersInColumns.</param>
        /// <param name="square">The square in numbersInSquares.</param>
        /// <returns>Return true if all contains number.</returns>
        private bool CanSet(int number, int row, int column, int square)
        {
            return (numbersInRows[row].Contains(number)
                    && numbersInColumns[column].Contains(number)
                    && numbersInSquares[square].Contains(number));
        }

        /// <summary>
        /// Create new array of ints. Fills it with numbers from 1 to Dim, 
        /// and shuffles it with Fisher–Yates shuffle.
        /// </summary>
        /// <returns>Return the shuffled array.</returns>
        private static int[] OneToDimShuffled()
        {
            int[] oneToDim = new int[SudokuClass.Dim];
            for (int i = 0; i < oneToDim.Length; ++i)
            {
                oneToDim[i] = (i + 1);
            }

            for (int i = 0; i < oneToDim.Length; ++i)
            {
                int j = GetNextFrom0ToDim(rnd);
                int tempI = oneToDim[i];
                oneToDim[i] = oneToDim[j];
                oneToDim[j] = tempI;
            }

            return oneToDim;
        }

        /// <summary>
        /// Return random number from zero to Dim. Zero is included, Dim is excluded.
        /// </summary>
        /// <param name="rnd">Random Number Generator.</param>
        /// <returns>Return random number from zero to Dim. 
        /// Zero is included, Dim is excluded.</returns>
        private static int GetNextFrom0ToDim(RNGCryptoServiceProvider rnd)
        {
            byte[] randomByte = new byte[1];
            do
            {
                rnd.GetBytes(randomByte);
            } while (randomByte[0] > SudokuClass.ZeroToDim);

            return (randomByte[0] % SudokuClass.Dim);
        }



        // override functions.
        public override bool Equals(object obj)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return 333;
        }

        public override string ToString()
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder(SudokuClass.GridSize);
            for (int i = 0; i < grid.Length; ++i)
            {
                s.Append(grid[i]);
            }
            return s.ToString();
        }
    }
}
