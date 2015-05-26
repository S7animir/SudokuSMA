namespace SudokuSMA
{
    class SudokuClass
    {
        public const int Dim = 9;
        public const int GridSize = Dim * Dim;
        public const int Easy = Dim * 4;
        public const int Medium = Dim * 5;
        public const int Hard = Dim * 6;
        public const int ZeroToDim = ((255 / Dim) * Dim - 1);
        public const int ZeroToGridSize = ((255 / GridSize) * GridSize - 1);

        private int time;
        private int level;
        private SolutionGrid solutionGrid;
        private GameGrid gameGrid;

        /// <summary>
        /// Construct new SudokuClass.
        /// </summary>
        public SudokuClass()
        {
            time = 0;
            level = Easy;
            solutionGrid = new SolutionGrid();
            gameGrid = new GameGrid();
        }

        /// <summary>
        /// Create new game.
        /// </summary>
        /// <param name="lvl">The level of the new game.</param>
        public void NewGame(int lvl)
        {
            level = (lvl != Medium && lvl != Hard) ? Easy : lvl;
            solutionGrid.NewGrid();
            gameGrid.NewGrid(solutionGrid.Grid, level);
            time = System.DateTime.Now.Millisecond;
        }

        







        // override functions.
        public override bool Equals(object obj)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return 333333333;
        }

        public override string ToString()
        {
            string sSolution = solutionGrid.ToString();
            string sGame = gameGrid.ToString();
            string s = "Solution is: " + sSolution + System.Environment.NewLine + System.Environment.NewLine + "Game is: " + sGame + ".";
            
            return s;
        }
    }
}
