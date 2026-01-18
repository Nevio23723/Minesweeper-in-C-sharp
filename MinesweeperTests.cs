using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Unit Tests für das Minesweeper-Spiel
    /// Diese Tests können manuell aufgerufen werden oder in ein Test-Framework (xUnit, NUnit) migriert werden
    /// </summary>
    public class MinesweeperTests
    {
        /// <summary>
        /// Test 1: Strategy Pattern - Prüft ob die Schwierigkeitsgrade korrekte Werte haben
        /// </summary>
        public static void Test_StrategyPattern_DifficultySettings()
        {
            Console.WriteLine("=== Test 1: Strategy Pattern - Difficulty Settings ===");

            // Easy testen
            IDifficulty easy = new Easy();
            Assert(easy.Width == 8, "Easy Width sollte 8 sein");
            Assert(easy.Height == 8, "Easy Height sollte 8 sein");
            Assert(easy.Mines == 10, "Easy Mines sollte 10 sein");

            // Medium testen
            IDifficulty medium = new Medium();
            Assert(medium.Width == 16, "Medium Width sollte 16 sein");
            Assert(medium.Height == 16, "Medium Height sollte 16 sein");
            Assert(medium.Mines == 40, "Medium Mines sollte 40 sein");

            // Hard testen
            IDifficulty hard = new Hard();
            Assert(hard.Width == 30, "Hard Width sollte 30 sein");
            Assert(hard.Height == 16, "Hard Height sollte 16 sein");
            Assert(hard.Mines == 99, "Hard Mines sollte 99 sein");

            Console.WriteLine("✓ Alle Strategy Pattern Tests bestanden!\n");
        }

        /// <summary>
        /// Test 2: GameModel Initialisierung - Prüft ob Board korrekt erstellt wird
        /// </summary>
        public static void Test_GameModel_InitializeBoard()
        {
            Console.WriteLine("=== Test 2: GameModel - Initialize Board ===");

            IDifficulty easy = new Easy();
            GameModel model = new GameModel(easy);

            // Prüfen ob alle Zellen erstellt wurden
            int cellCount = 0;
            for (int x = 0; x < easy.Width; x++)
            {
                for (int y = 0; y < easy.Height; y++)
                {
                    Cell cell = model.GetCell(x, y);
                    Assert(cell != null, $"Zelle bei ({x},{y}) sollte nicht null sein");
                    Assert(!cell.IsRevealed, "Zellen sollten initial nicht aufgedeckt sein");
                    cellCount++;
                }
            }

            Assert(cellCount == 64, "Easy Board sollte 64 Zellen haben");
            Assert(!model.IsGameOver, "Spiel sollte initial nicht beendet sein");
            Assert(!model.IsWon, "Spiel sollte initial nicht gewonnen sein");

            Console.WriteLine("✓ Board Initialisierung Tests bestanden!\n");
        }

        /// <summary>
        /// Test 3: Minen platzieren - Prüft ob korrekte Anzahl Minen platziert wird
        /// </summary>
        public static void Test_GameModel_PlaceMines()
        {
            Console.WriteLine("=== Test 3: GameModel - Place Mines ===");

            IDifficulty easy = new Easy();
            GameModel model = new GameModel(easy);

            // Erste Position aufdecken (triggert Minen-Platzierung)
            int firstX = 0, firstY = 0;
            model.Reveal(firstX, firstY);

            // Minen zählen
            int mineCount = 0;
            for (int x = 0; x < easy.Width; x++)
            {
                for (int y = 0; y < easy.Height; y++)
                {
                    Cell cell = model.GetCell(x, y);
                    if (cell.IsMine)
                    {
                        mineCount++;
                        // Erste Position sollte keine Mine sein
                        Assert(!(x == firstX && y == firstY), 
                            "Erste Klick-Position sollte keine Mine haben");
                    }
                }
            }

            Assert(mineCount == easy.Mines, 
                $"Es sollten genau {easy.Mines} Minen platziert sein, gefunden: {mineCount}");

            Console.WriteLine("✓ Minen-Platzierung Tests bestanden!\n");
        }

        /// <summary>
        /// Test 4: Memento Pattern - Speichern und Wiederherstellen des Spielzustands
        /// </summary>
        public static void Test_Memento_SaveAndRestore()
        {
            Console.WriteLine("=== Test 4: Memento Pattern - Save and Restore ===");

            IDifficulty easy = new Easy();
            GameModel model = new GameModel(easy);

            // Ersten Zug machen
            model.Reveal(0, 0);

            // Zustand speichern
            GameMemento memento = model.CreateMemento();
            Assert(memento != null, "Memento sollte erstellt werden");
            Assert(memento.SavedBoard != null, "SavedBoard sollte nicht null sein");

            // Weiteren Zug machen
            model.Reveal(1, 1);
            Cell cell11After = model.GetCell(1, 1);
            Assert(cell11After.IsRevealed, "Zelle (1,1) sollte nach Reveal aufgedeckt sein");

            // Zustand wiederherstellen
            model.Restore(memento);

            // Prüfen ob alter Zustand wiederhergestellt wurde
            Cell cell11Restored = model.GetCell(1, 1);
            Assert(!cell11Restored.IsRevealed, 
                "Zelle (1,1) sollte nach Restore wieder verdeckt sein");

            Cell cell00Restored = model.GetCell(0, 0);
            Assert(cell00Restored.IsRevealed, 
                "Zelle (0,0) sollte nach Restore weiterhin aufgedeckt sein");

            Console.WriteLine("✓ Memento Pattern Tests bestanden!\n");
        }

        /// <summary>
        /// Test 5: History - Undo Stack arbeitet korrekt
        /// </summary>
        public static void Test_History_UndoStack()
        {
            Console.WriteLine("=== Test 5: History - Undo Stack ===");

            History history = new History();

            // Leerer Stack sollte null zurückgeben
            GameMemento emptyUndo = history.Undo();
            Assert(emptyUndo == null, "Undo auf leerem Stack sollte null zurückgeben");

            // Mementos erstellen und speichern
            IDifficulty easy = new Easy();
            GameModel model1 = new GameModel(easy);
            GameMemento memento1 = model1.CreateMemento();
            history.Save(memento1);

            GameModel model2 = new GameModel(easy);
            model2.Reveal(0, 0);
            GameMemento memento2 = model2.CreateMemento();
            history.Save(memento2);

            GameModel model3 = new GameModel(easy);
            model3.Reveal(0, 0);
            model3.Reveal(1, 1);
            GameMemento memento3 = model3.CreateMemento();
            history.Save(memento3);

            // Undo in umgekehrter Reihenfolge (LIFO - Last In First Out)
            GameMemento restored3 = history.Undo();
            Assert(restored3 == memento3, "Erstes Undo sollte memento3 zurückgeben");

            GameMemento restored2 = history.Undo();
            Assert(restored2 == memento2, "Zweites Undo sollte memento2 zurückgeben");

            GameMemento restored1 = history.Undo();
            Assert(restored1 == memento1, "Drittes Undo sollte memento1 zurückgeben");

            // Stack sollte jetzt wieder leer sein
            GameMemento shouldBeNull = history.Undo();
            Assert(shouldBeNull == null, "Undo nach allen Pop sollte null zurückgeben");

            Console.WriteLine("✓ History Stack Tests bestanden!\n");
        }

        /// <summary>
        /// Hilfsmethode für Assertions
        /// </summary>
        private static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception($"ASSERTION FAILED: {message}");
            }
        }

        /// <summary>
        /// Führt alle Tests aus
        /// </summary>
        public static void RunAllTests()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════╗");
            Console.WriteLine("║     MINESWEEPER UNIT TESTS                    ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝\n");

            try
            {
                Test_StrategyPattern_DifficultySettings();
                Test_GameModel_InitializeBoard();
                Test_GameModel_PlaceMines();
                Test_Memento_SaveAndRestore();
                Test_History_UndoStack();

                Console.WriteLine("╔════════════════════════════════════════════════╗");
                Console.WriteLine("║  ✓✓✓ ALLE TESTS ERFOLGREICH BESTANDEN! ✓✓✓   ║");
                Console.WriteLine("╚════════════════════════════════════════════════╝\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ TEST FEHLGESCHLAGEN: {ex.Message}\n");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("Drücken Sie eine Taste um fortzufahren...");
            Console.ReadKey();
        }
    }
}
