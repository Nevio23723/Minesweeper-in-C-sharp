using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// View-Klasse: Verantwortlich für die Darstellung auf der Konsole
    /// </summary>
    internal class GameView
    {
        /// <summary>
        /// Zeigt die Willkommensnachricht an
        /// </summary>
        public void ShowWelcome()
        {
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("       MINORSWEEPER - WILLKOMMEN!");
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine();
        }

        /// <summary>
        /// Fragt den Spieler nach dem Schwierigkeitsgrad
        /// </summary>
        /// <returns>Die gewählte Schwierigkeitsstufe (1=Easy, 2=Medium, 3=Hard)</returns>
        public int GetDifficultyChoice()
        {
            Console.WriteLine("Bitte wählen Sie einen Schwierigkeitsgrad:");
            Console.WriteLine("1 - Free  (8x8,  10 Minen)");
            Console.WriteLine("2 - Solala  (16x16, 40 Minen)");
            Console.WriteLine("3 - V320 typa lvl   (30x16, 99 Minen)");
            Console.Write("\nIhre Wahl (1-3): ");

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 3)
            {
                return choice;
            }

            Console.WriteLine("Ungültige Eingabe. Einfach wird gewählt.");
            return 1;
        }

        /// <summary>
        /// Zeichnet das Spielfeld auf die Konsole
        /// </summary>
        public void DrawBoard(GameModel model)
        {
            Console.Clear();
            Console.WriteLine();

            int width = model.CurrentDifficulty.Width;
            int height = model.CurrentDifficulty.Height;

            // Spalten-Header (Zahlen)
            Console.Write("    ");
            for (int x = 0; x < width; x++)
            {
                Console.Write($"| {x + 1,2} ");
            }
            Console.WriteLine("|");

            // Trennlinie
            Console.Write("----");
            for (int x = 0; x < width; x++)
            {
                Console.Write("-----");
            }
            Console.WriteLine("-");

            // Zeilen (mit Buchstaben)
            for (int y = 0; y < height; y++)
            {
                // Zeilen-Header (Buchstaben A-Z, dann AA, AB, etc.)
                Console.Write($" {GetRowLabel(y),2} ");

                // Zellen der Zeile
                for (int x = 0; x < width; x++)
                {
                    Cell cell = model.GetCell(x, y);
                    Console.Write("| ");

                    if (cell.IsRevealed)
                    {
                        if (cell.IsMine)
                        {
                            Console.Write(" * "); // Mine
                        }
                        else if (cell.NeighborMines == 0)
                        {
                            Console.Write("   "); // Leer
                        }
                        else
                        {
                            Console.Write($" {cell.NeighborMines} "); // Zahl
                        }
                    }
                    else
                    {
                        Console.Write("   "); // Nicht aufgedeckt
                    }
                }
                Console.WriteLine("|");

                // Trennlinie
                Console.Write("----");
                for (int x = 0; x < width; x++)
                {
                    Console.Write("-----");
                }
                Console.WriteLine("-");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Wandelt eine Zeilennummer in einen Buchstaben um (0=A, 1=B, ...., 26=AA, etc.)
        /// </summary>
        private string GetRowLabel(int row)
        {
            if (row < 26)
                return ((char)('A' + row)).ToString();
            
            // Für größere Boards: AA, AB, AC, ...
            int first = row / 26 - 1;
            int second = row % 26;
            return $"{(char)('A' + first)}{(char)('A' + second)}";
        }

        /// <summary>
        /// Holt Eingabe vom Spieler (z.B. "C4" oder "undo")
        /// </summary>
        public string GetPlayerInput()
        {
            Console.Write("Gewünschtes Feld (z.B. C4) oder 'undo' zum Rückgängig machen: ");
            return Console.ReadLine()?.Trim().ToUpper();
        }

        /// <summary>
        /// Zeigt Game Over Nachricht an
        /// </summary>
        public void ShowGameOver(bool won)
        {
            Console.WriteLine();
            if (won)
            {
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("             Beste wo git!    ");
                Console.WriteLine("         6er schnitt C Scharf!");
                Console.WriteLine("═══════════════════════════════════════");
            }
            else
            {
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("           Du Bot    ");
                Console.WriteLine("      Mine getroffen!");
                Console.WriteLine("═══════════════════════════════════════");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Zeigt eine allgemeine Nachricht an
        /// </summary>
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
