using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Das Spielfeld-Modell: verwaltet das Board, Minen und Spiellogik
    /// </summary>
    internal class GameModel
    {
        private Cell[,] _board;
        private bool _minesPlaced = false;

        public bool IsGameOver { get; private set; }
        public bool IsWon { get; private set; }
        public IDifficulty CurrentDifficulty { get; private set; }

        /// <summary>
        /// Konstruktor: Initialisiert das Spielfeld mit der gewählten Schwierigkeit
        /// </summary>
        public GameModel(IDifficulty difficulty)
        {
            CurrentDifficulty = difficulty;
            InitializeBoard();
        }

        /// <summary>
        /// Initialisiert das Board mit leeren Zellen
        /// </summary>
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

        /// <summary>
        /// Gibt eine einzelne Zelle zurück
        /// </summary>
        public Cell GetCell(int x, int y)
        {
            if (x < 0 || x >= CurrentDifficulty.Width || y < 0 || y >= CurrentDifficulty.Height)
                return null;
            return _board[x, y];
        }

        /// <summary>
        /// Deckt ein Feld auf. Beim ersten Aufruf werden Minen platziert.
        /// </summary>
        public void Reveal(int x, int y)
        {
            // Ungültige Koordinaten
            if (x < 0 || x >= CurrentDifficulty.Width || y < 0 || y >= CurrentDifficulty.Height)
                return;

            Cell cell = _board[x, y];

            // Bereits aufgedeckt
            if (cell.IsRevealed)
                return;

            // Minen beim ersten Klick platzieren (nicht auf erste Klick-Position)
            if (!_minesPlaced)
            {
                PlaceMines(x, y);
                CalculateNeighborMines();
                _minesPlaced = true;
            }

            // Feld aufdecken
            cell.IsRevealed = true;

            // Mine getroffen - Game Over
            if (cell.IsMine)
            {
                IsGameOver = true;
                return;
            }

            // Wenn keine Nachbarminen: rekursiv Nachbarn aufdecken
            if (cell.NeighborMines == 0)
            {
                RevealNeighbors(x, y);
            }

            // Gewinn prüfen
            CheckWin();
        }

        /// <summary>
        /// Platziert Minen zufällig, aber nicht auf der ersten Klick-Position
        /// </summary>
        private void PlaceMines(int firstX, int firstY)
        {
            Random rand = new Random();
            int minesPlaced = 0;
            int totalMines = CurrentDifficulty.Mines;

            while (minesPlaced < totalMines)
            {
                int x = rand.Next(CurrentDifficulty.Width);
                int y = rand.Next(CurrentDifficulty.Height);

                // Nicht auf erste Position und nicht doppelt platzieren
                if ((x == firstX && y == firstY) || _board[x, y].IsMine)
                    continue;

                _board[x, y].IsMine = true;
                minesPlaced++;
            }
        }

        /// <summary>
        /// Berechnet für alle Zellen die Anzahl der Nachbarminen
        /// </summary>
        private void CalculateNeighborMines()
        {
            for (int x = 0; x < CurrentDifficulty.Width; x++)
            {
                for (int y = 0; y < CurrentDifficulty.Height; y++)
                {
                    if (!_board[x, y].IsMine)
                    {
                        _board[x, y].NeighborMines = CountNeighborMines(x, y);
                    }
                }
            }
        }

        /// <summary>
        /// Zählt die Minen in den 8 umliegenden Feldern
        /// </summary>
        private int CountNeighborMines(int x, int y)
        {
            int count = 0;
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue; // Skip selbst

                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx >= 0 && nx < CurrentDifficulty.Width &&
                        ny >= 0 && ny < CurrentDifficulty.Height &&
                        _board[nx, ny].IsMine)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Deckt rekursiv alle Nachbarn auf (bei 0 Nachbarminen)
        /// </summary>
        private void RevealNeighbors(int x, int y)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx >= 0 && nx < CurrentDifficulty.Width &&
                        ny >= 0 && ny < CurrentDifficulty.Height)
                    {
                        Cell neighbor = _board[nx, ny];
                        if (!neighbor.IsRevealed && !neighbor.IsMine)
                        {
                            neighbor.IsRevealed = true;
                            // Rekursiv weiter aufdecken wenn auch 0 Nachbarn
                            if (neighbor.NeighborMines == 0)
                            {
                                RevealNeighbors(nx, ny);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Prüft ob das Spiel gewonnen ist (alle Nicht-Minen-Felder aufgedeckt)
        /// </summary>
        private void CheckWin()
        {
            for (int x = 0; x < CurrentDifficulty.Width; x++)
            {
                for (int y = 0; y < CurrentDifficulty.Height; y++)
                {
                    Cell cell = _board[x, y];
                    // Wenn eine Nicht-Mine noch nicht aufgedeckt ist: nicht gewonnen
                    if (!cell.IsMine && !cell.IsRevealed)
                        return;
                }
            }

            // Alle Nicht-Minen aufgedeckt: Gewonnen!
            IsWon = true;
            IsGameOver = true;
        }

        /// <summary>
        /// Erstellt ein Memento (Snapshot) des aktuellen Spielzustands
        /// </summary>
        public GameMemento CreateMemento()
        {
            return new GameMemento(_board, IsGameOver);
        }

        /// <summary>
        /// Stellt einen früheren Spielzustand aus einem Memento wieder her
        /// </summary>
        public void Restore(GameMemento memento)
        {
            if (memento == null) return;

            // Board-Größe prüfen
            int w = memento.SavedBoard.GetLength(0);
            int h = memento.SavedBoard.GetLength(1);

            _board = new Cell[w, h];

            // Jeden Cell-Zustand wiederherstellen
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    _board[x, y] = memento.SavedBoard[x, y].Clone();
                }
            }

            IsGameOver = memento.SavedGameOver;
            
            // IsWon neu berechnen falls Game Over
            if (IsGameOver)
            {
                // Prüfen ob es ein Gewinn war (keine aufgedeckte Mine)
                bool foundRevealedMine = false;
                for (int x = 0; x < w && !foundRevealedMine; x++)
                {
                    for (int y = 0; y < h && !foundRevealedMine; y++)
                    {
                        if (_board[x, y].IsMine && _board[x, y].IsRevealed)
                            foundRevealedMine = true;
                    }
                }
                IsWon = !foundRevealedMine && IsGameOver;
            }
            else
            {
                IsWon = false;
            }

            // Minen wurden platziert wenn mindestens eine Mine existiert
            _minesPlaced = false;
            for (int x = 0; x < w && !_minesPlaced; x++)
            {
                for (int y = 0; y < h && !_minesPlaced; y++)
                {
                    if (_board[x, y].IsMine)
                        _minesPlaced = true;
                }
            }
        }
    }
}
