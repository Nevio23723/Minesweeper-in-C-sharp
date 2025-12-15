namespace Minesweeper.App
{
    public class Hard : IDifficulty
    {
        public int Width => 30;
        public int Height => 16;
        public int Mines => 99;
        public string Name => "Hard";
    }
}
