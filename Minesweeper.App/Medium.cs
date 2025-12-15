namespace Minesweeper.App
{
    public class Medium : IDifficulty
    {
        public int Width => 16;
        public int Height => 16;
        public int Mines => 40;
        public string Name => "Medium";
    }
}
