namespace SudokuSMA
{
    class Cell
    {
        private int number;
        private System.Collections.Generic.HashSet<int> numbers;

        public Cell()
        {
            number = 0;
            numbers = new System.Collections.Generic.HashSet<int>();
        }

        public Cell(Cell cell)
        {
            number = cell.number;
            numbers = new System.Collections.Generic.HashSet<int>();
            foreach (int num in cell.numbers)
            {
                numbers.Add(num);
            }
        }

        public void Set(int num)
        {
            number = num;
            numbers.Clear();
        }

        public int Number
        {
            get { return number; }
        }

        public void Clear()
        {
            numbers.Clear();
        }

        public void Add(System.Collections.Generic.HashSet<int> nums)
        {
            System.Diagnostics.Debug.Assert(numbers.Count == 0);
            number = 0;
            numbers.UnionWith(nums);
        }

        /// <summary>
        /// Check is OK to set(solve) cell. It is used only in debugging.
        /// </summary>
        /// <param name="num">The number to be set(solved with).</param>
        /// <returns>Return is the cell set(solved) properly.</returns>
        public bool SetOK(int num)
        {
            return (number == 0 && numbers.Contains(num));
        }

        public bool CanSet()
        {
            if (numbers.Count == 1)
            {
                foreach (int num in numbers)
                {
                    number = num;
                    break;
                }
                numbers.Clear();
                return true;
            }

            return false;
        }

        public void Remove(int number)
        {
            numbers.Remove(number);
        }



        // override functions
        public override bool Equals(object obj)
        {
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (int number in numbers)
            {
                hash += number;
            }

            hash *= (17 * 23);

            return hash;
        }

        public override string ToString()
        {
            return number.ToString();
        }
    }
}
