namespace Minesweeper.App
{
    /// <summary>
    /// Represents a single cell on the Minesweeper board.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Gets or sets a value indicating whether this cell contains a mine.
        /// </summary>
        public bool IsMine { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this cell has been revealed.
        /// </summary>
        public bool IsRevealed { get; set; }

        /// <summary>
        /// Gets or sets the number of mines in the neighboring cells.
        /// </summary>
        public int NeighborMines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cell is flagged by the player.
        /// </summary>
        public bool IsFlagged { get; set; }
    }
}
