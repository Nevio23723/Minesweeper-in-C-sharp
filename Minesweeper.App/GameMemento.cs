namespace Minesweeper.App
{
    /// <summary>
    /// Represents a snapshot of the game state.
    /// </summary>
    public class GameMemento
    {
        public bool IsGameOver { get; private set; }
        public bool IsGameWon { get; private set; }
        public bool FirstMove { get; private set; }
        public Cell[,] Board { get; private set; }

        public GameMemento(bool isGameOver, bool isGameWon, bool firstMove, Cell[,] board)
        {
            IsGameOver = isGameOver;
            IsGameWon = isGameWon;
            FirstMove = firstMove;
            // Create a deep copy of the board
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);
            Board = new Cell[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Board[r, c] = new Cell
                    {
                        IsMine = board[r, c].IsMine,
                        IsRevealed = board[r, c].IsRevealed,
                        NeighborMines = board[r, c].NeighborMines,
                        IsFlagged = board[r, c].IsFlagged
                    };
                }
            }
        }
    }
}
