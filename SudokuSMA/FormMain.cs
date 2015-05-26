using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SudokuSMA
{
    public partial class FormMain : Form
    {
        private SudokuClass sudokuGame;

        public FormMain()
        {
            sudokuGame = new SudokuClass();
            InitializeComponent();
            sudokuGame.NewGame(SudokuClass.Easy);
            string s = sudokuGame.ToString();
        }
    }
}
