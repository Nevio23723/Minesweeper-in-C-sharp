namespace Minesweeper.App
{
    public class Easy : IDifficulty
    {
        public int Width => 8;
        public int Height => 8;
        public int Mines => 10;
        public string Name => "Easy";
    }
}
