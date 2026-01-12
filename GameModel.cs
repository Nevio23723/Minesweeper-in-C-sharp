using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    internal class GameModel
    {
        private Cell[,] _board;
        public bool IsGameOver { get; private set; }
        public IDifficulty CurrentDifficulty { get; private set; }

        public GameModel(IDifficulty difficulty)
        {
            CurrentDifficulty = difficulty;
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            int w = CurrentDifficulty.Width;
            int h = CurrentDifficulty.Height;
            _board = new Cell[w, h];

            //Alle Zellen erstellen
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    _board[x, y] = new Cell();
        }
    }
}
