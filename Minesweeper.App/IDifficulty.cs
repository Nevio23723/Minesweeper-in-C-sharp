namespace Minesweeper.App
{
    /// <summary>
    /// Interface for defining game difficulty settings.
    /// </summary>
    public interface IDifficulty
    {
        /// <summary>
        /// Gets the width of the board.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the height of the board.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets the number of mines on the board.
        /// </summary>
        int Mines { get; }
        
        /// <summary>
        /// Gets the name of the difficulty.
        /// </summary>
        string Name { get; }
    }
}
