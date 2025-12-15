using System;

namespace Minesweeper.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Minesweeper!");
            
            IDifficulty difficulty = SelectDifficulty();
            Game game = new Game(difficulty);

            bool running = true;
            while (running)
            {
                Console.Clear();
                PrintBoard(game);

                if (game.IsGameWon)
                {
                    Console.WriteLine("\nCONGRATULATIONS! You won!");
                    running = false;
                    break;
                }

                if (game.IsGameOver)
                {
                    Console.WriteLine("\nGAME OVER! You stepped on a mine.");
                    // Check if player wants to undo
                    Console.Write("Do you want to Undo? (y/n): ");
                    var undoInput = Console.ReadLine();
                    if (undoInput?.ToLower() == "y")
                    {
                        if (game.CanUndo())
                        {
                            game.Undo();
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Cannot Undo further. Press enter to exit.");
                            Console.ReadLine();
                            running = false;
                            break;
                        }
                    }
                    else
                    {
                        running = false;
                        break;
                    }
                }

                Console.WriteLine("\nOptions:");
                Console.WriteLine(" - Enter coordinate to reveal (e.g. 'A1')");
                Console.WriteLine(" - Enter 'U' to Undo");
                Console.WriteLine(" - Enter 'Q' to Quit");
                Console.Write("Your choice: ");

                string input = Console.ReadLine()?.Trim().ToUpper();

                if (string.IsNullOrEmpty(input)) continue;

                if (input == "Q")
                {
                    running = false;
                }
                else if (input == "U")
                {
                    if (game.CanUndo())
                    {
                        game.Undo();
                    }
                    else
                    {
                        Console.WriteLine("Nothing to undo! Press enter to continue...");
                        Console.ReadLine();
                    }
                }
                else
                {
                    // Try parse coordinate
                    if (TryParseCoordinate(input, out int row, out int col))
                    {
                        game.Reveal(row, col);
                    }
                    else
                    {
                        Console.WriteLine("Invalid format. Use LetterNumber (e.g. A1). Press enter...");
                        Console.ReadLine();
                    }
                }
            }
        }

        static IDifficulty SelectDifficulty()
        {
            Console.WriteLine("Select Difficulty:");
            Console.WriteLine("1. Easy (8x8, 10 Mines)");
            Console.WriteLine("2. Medium (16x16, 40 Mines)");
            Console.WriteLine("3. Hard (30x16, 99 Mines)");
            Console.Write("Choice (1-3): ");

            while (true)
            {
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1": return new Easy();
                    case "2": return new Medium();
                    case "3": return new Hard(); 
                    default:
                        Console.Write("Invalid choice. Please enter 1, 2, or 3: ");
                        break;
                }
            }
        }

        static void PrintBoard(Game game)
        {
            int width = game.Difficulty.Width;
            int height = game.Difficulty.Height;

            // Header (Column numbers)
            Console.Write("    ");
            for (int c = 0; c < width; c++)
            {
                // Align column numbers, assuming max 2 digits for now (up to 30)
                Console.Write($"| {c + 1, -2}");
            }
            Console.WriteLine("|");

            // Separator
            Console.Write("----");
            for (int c = 0; c < width; c++)
            {
                Console.Write("-----");
            }
            Console.WriteLine();

            // Rows
            for (int r = 0; r < height; r++)
            {
                // Row Label (A, B, C...)
                char rowLabel = (char)('A' + r);
                Console.Write($"{rowLabel}   ");

                for (int c = 0; c < width; c++)
                {
                    var cell = game.Board[r, c];
                    string content = "  "; // Default hidden

                    if (cell.IsRevealed)
                    {
                        if (cell.IsMine)
                        {
                            content = "* "; // Mine
                        }
                        else if (cell.NeighborMines > 0)
                        {
                            content = $"{cell.NeighborMines} ";
                        }
                        else
                        {
                            content = "  "; // Empty
                        }
                    }
                    else if (game.IsGameOver && cell.IsMine)
                    {
                        // Show mines on game over if we want, or implemented logic
                        // The instructions say "Entsprechend wird ... das Spielfeld ... neu ausgegeben"
                        // Usually we show all mines on loss.
                        content = "* "; 
                    }

                    if (cell.IsRevealed && !cell.IsMine && cell.NeighborMines == 0)
                    {
                         content = ". "; // Visual indicator for revealed empty
                    }

                    Console.Write($"| {content} ");
                }
                Console.WriteLine("|");
                
                // Row Separator
                Console.Write("----");
                for (int c = 0; c < width; c++)
                {
                    Console.Write("-----");
                }
                Console.WriteLine();
            }
            
            Console.WriteLine($"Mines: {game.Difficulty.Mines}");
        }

        static bool TryParseCoordinate(string input, out int row, out int col)
        {
            row = -1;
            col = -1;
            if (string.IsNullOrWhiteSpace(input) || input.Length < 2) return false;

            // A1 -> Row A (0), Col 1 (0)
            // Parse Row (Letter part)
            // Assuming 1 letter update logic if row > 26 (Hard is 16 height, so A-P, fits in char)
            
            char rowChar = input[0];
            if (!char.IsLetter(rowChar)) return false;
            
            row = char.ToUpper(rowChar) - 'A';

            // Parse Col (Number part)
            string colStr = input.Substring(1);
            if (int.TryParse(colStr, out int c))
            {
                col = c - 1; // 1-based to 0-based
                return true;
            }

            return false;
        }
    }
}
