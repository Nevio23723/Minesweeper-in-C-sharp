using System;
using System.Collections.Generic;

namespace Minesweeper.App
{
    public class Game
    {
        public Cell[,] Board { get; private set; }
        public IDifficulty Difficulty { get; private set; }
        public bool IsGameOver { get; private set; }
        public bool IsGameWon { get; private set; }
        
        private readonly History _history = new History();
        private bool _firstMove = true;

        public Game(IDifficulty difficulty)
        {
            Difficulty = difficulty;
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            Board = new Cell[Difficulty.Height, Difficulty.Width];
            for (int r = 0; r < Difficulty.Height; r++)
            {
                for (int c = 0; c < Difficulty.Width; c++)
                {
                    Board[r, c] = new Cell();
                }
            }
            IsGameOver = false;
            IsGameWon = false;
            _firstMove = true;
        }

        private void PlaceMines(int safeRow, int safeCol)
        {
            var rand = new Random();
            int minesPlaced = 0;
            while (minesPlaced < Difficulty.Mines)
            {
                int r = rand.Next(Difficulty.Height);
                int c = rand.Next(Difficulty.Width);

                // Ensure we don't place a mine on the first clicked cell or where one already exists
                if (!Board[r, c].IsMine && (r != safeRow || c != safeCol))
                {
                    Board[r, c].IsMine = true;
                    minesPlaced++;
                }
            }
        }

        private void CalculateNeighborMines()
        {
            for (int r = 0; r < Difficulty.Height; r++)
            {
                for (int c = 0; c < Difficulty.Width; c++)
                {
                    if (!Board[r, c].IsMine)
                    {
                        Board[r, c].NeighborMines = CountMinesAround(r, c);
                    }
                }
            }
        }

        private int CountMinesAround(int row, int col)
        {
            int count = 0;
            for (int r = row - 1; r <= row + 1; r++)
            {
                for (int c = col - 1; c <= col + 1; c++)
                {
                    if (IsWithinBounds(r, c) && Board[r, c].IsMine)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private bool IsWithinBounds(int r, int c)
        {
            return r >= 0 && r < Difficulty.Height && c >= 0 && c < Difficulty.Width;
        }

        public void Reveal(int row, int col)
        {
            if (IsGameOver || IsGameWon || !IsWithinBounds(row, col) || Board[row, col].IsRevealed || Board[row, col].IsFlagged)
                return;

            // Save state before move
            SaveState();

            if (_firstMove)
            {
                PlaceMines(row, col);
                CalculateNeighborMines();
                _firstMove = false;
            }

            if (Board[row, col].IsMine)
            {
                Board[row, col].IsRevealed = true;
                IsGameOver = true;
                return;
            }

            RevealRecursive(row, col);
            CheckWinCondition();
        }

        private void RevealRecursive(int row, int col)
        {
            if (!IsWithinBounds(row, col) || Board[row, col].IsRevealed || Board[row, col].IsFlagged)
                return;

            Board[row, col].IsRevealed = true;

            if (Board[row, col].NeighborMines == 0)
            {
                for (int r = row - 1; r <= row + 1; r++)
                {
                    for (int c = col - 1; c <= col + 1; c++)
                    {
                        // Don't reveal the cell itself again (though check handles it), and skip out of bounds
                        if (r != row || c != col) 
                        {
                            RevealRecursive(r, c);
                        }
                    }
                }
            }
        }

        public void SaveState()
        {
            _history.Save(new GameMemento(IsGameOver, IsGameWon, _firstMove, Board));
        }

        public void Undo()
        {
            if (_history.CanUndo)
            {
                var memento = _history.Undo();
                Board = memento.Board;
                IsGameOver = memento.IsGameOver;
                IsGameWon = memento.IsGameWon;
                _firstMove = memento.FirstMove;
            }
        }
        
        public bool CanUndo() => _history.CanUndo;

        private void CheckWinCondition()
        {
            if (IsGameOver) return;

            int revealedCount = 0;
            int totalCells = Difficulty.Height * Difficulty.Width;

            foreach (var cell in Board)
            {
                if (cell.IsRevealed)
                {
                    revealedCount++;
                }
            }

            if (revealedCount == totalCells - Difficulty.Mines)
            {
                IsGameWon = true;
                IsGameOver = true;
            }
        }
    }
}
