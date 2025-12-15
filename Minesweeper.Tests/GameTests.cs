using NUnit.Framework;
using Minesweeper.App;
using System.Linq;

namespace Minesweeper.Tests
{
    public class GameTests
    {
        private class TestDifficulty : IDifficulty
        {
            public int Width => 5;
            public int Height => 5;
            public int Mines => 5;
            public string Name => "Test";
        }

        [Test]
        public void TestInitialization()
        {
            var game = new Game(new TestDifficulty());
            Assert.AreEqual(5, game.Board.GetLength(0)); // Height
            Assert.AreEqual(5, game.Board.GetLength(1)); // Width
            Assert.IsFalse(game.IsGameOver);
            Assert.IsFalse(game.IsGameWon);
        }

        [Test]
        public void TestFirstMoveProtection()
        {
            // Try many times to ensure first click is never a mine
            for (int i = 0; i < 20; i++)
            {
                var game = new Game(new TestDifficulty());
                game.Reveal(0, 0);
                Assert.IsFalse(game.Board[0, 0].IsMine, "First clicked cell should not be a mine");
                Assert.IsFalse(game.IsGameOver, "Game should not be over on first move");
            }
        }

        [Test]
        public void TestLoseCondition()
        {
            var game = new Game(new TestDifficulty());
            // Reveal safe spot to place mines
            game.Reveal(0, 0); 
            
            // Find a mine
            int mineRow = -1;
            int mineCol = -1;
            for(int r = 0; r < 5; r++)
            {
                for(int c = 0; c < 5; c++)
                {
                    if(game.Board[r,c].IsMine)
                    {
                        mineRow = r;
                        mineCol = c;
                        break;
                    }
                }
                if (mineRow != -1) break;
            }

            // Click on mine
            game.Reveal(mineRow, mineCol);
            Assert.IsTrue(game.IsGameOver);
            Assert.IsFalse(game.IsGameWon);
        }

        [Test]
        public void TestWinCondition()
        {
            // Create a very small board: 3x3, 1 mine.
            var simpleDifficulty = new CustomDifficulty(3, 3, 1);
            var game = new Game(simpleDifficulty);
            
            // First click ensures 0,0 is safe. 
            // We need to reveal all non-mine cells.
            game.Reveal(0, 0);

            // Now we cheat by inspecting the board to reveal all non-mines
            for(int r = 0; r < 3; r++)
            {
                for(int c = 0; c < 3; c++)
                {
                    if (!game.Board[r,c].IsMine && !game.Board[r,c].IsRevealed)
                    {
                        game.Reveal(r, c);
                    }
                }
            }

            Assert.IsTrue(game.IsGameWon);
            Assert.IsTrue(game.IsGameOver);
        }

        [Test]
        public void TestUndo()
        {
            var game = new Game(new TestDifficulty());
            game.Reveal(0, 0);
            
            Assert.IsTrue(game.CanUndo());
            
            // Save state check
            bool isRevealedBeforeUndo = game.Board[0, 0].IsRevealed;
            Assert.IsTrue(isRevealedBeforeUndo);

            game.Undo();
            
            // Should be back to initial state (actually, mines will persist if placed, but revealed status should revert)
            Assert.IsFalse(game.Board[0, 0].IsRevealed);
            
            // Check that we can't undo again (history empty)
            Assert.IsFalse(game.CanUndo());
        }

        [Test]
        public void TestDeepCopyMemento()
        {
            var game = new Game(new TestDifficulty());
            game.Reveal(0, 0); // State 1
            
            // manually modify board to ensure memento isn't affected if we didn't save? 
            // Better: Undo and verify modification is gone.
            
            // Pick a cell that is NOT a mine and NOT revealed
            int rTarget = -1, cTarget = -1;
            for(int r=0; r<5; r++)
            {
                for(int c=0; c<5; c++)
                {
                    if(!game.Board[r,c].IsMine && !game.Board[r,c].IsRevealed)
                    {
                        rTarget = r; cTarget = c; break;
                    }
                }
                if (rTarget != -1) break;
            }
            
            if (rTarget != -1)
            {
                game.Reveal(rTarget, cTarget); // State 2
                Assert.IsTrue(game.Board[rTarget, cTarget].IsRevealed);

                game.Undo(); // Back to State 1
                Assert.IsFalse(game.Board[rTarget, cTarget].IsRevealed);
            }
        }

        // Helper for custom setting
        private class CustomDifficulty : IDifficulty
        {
            public int Width { get; }
            public int Height { get; }
            public int Mines { get; }
            public string Name => "Custom";
            public CustomDifficulty(int w, int h, int m) { Width = w; Height = h; Mines = m; }
        }
    }
}
