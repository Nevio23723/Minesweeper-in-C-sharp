using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Controller-Klasse: Steuert den Spielablauf und koordiniert Model und View
    /// </summary>
    internal class Game
    {
        private GameModel model;
        private GameView view;
        private History history;

        /// <summary>
        /// Startet das Spiel
        /// </summary>
        public void Start()
        {
            view = new GameView();
            history = new History();

            view.ShowWelcome();

            // Schwierigkeitsgrad auswählen
            IDifficulty difficulty = SelectDifficulty();

            // Model initialisieren
            model = new GameModel(difficulty);

            // Hauptspiel-Loop
            GameLoop();
        }

        /// <summary>
        /// Lässt den Spieler den Schwierigkeitsgrad wählen
        /// </summary>
        private IDifficulty SelectDifficulty()
        {
            int choice = view.GetDifficultyChoice();

            return choice switch
            {
                1 => new Easy(),
                2 => new Medium(),
                3 => new Hard(),
                _ => new Easy()
            };
        }

        /// <summary>
        /// Hauptspiel-Schleife
        /// </summary>
        private void GameLoop()
        {
            while (!model.IsGameOver)
            {
                // Board anzeigen
                view.DrawBoard(model);

                // Eingabe holen
                string input = view.GetPlayerInput();

                if (string.IsNullOrEmpty(input))
                    continue;

                // Undo-Befehl
                if (input == "UNDO")
                {
                    Undo();
                    continue;
                }

                // Eingabe verarbeiten
                ProcessInput(input);
            }

            // Spiel zu Ende - finales Board zeigen
            view.DrawBoard(model);
            view.ShowGameOver(model.IsWon);

            // Fragen ob nochmal spielen
            Console.Write("Nochmal spielen? (J/N): ");
            string answer = Console.ReadLine()?.Trim().ToUpper();
            if (answer == "J" || answer == "JA")
            {
                Start(); // Neues Spiel
            }
        }

        /// <summary>
        /// Verarbeitet Spieler-Eingabe (z.B. "C4")
        /// </summary>
        private void ProcessInput(string input)
        {
            // Parse Eingabe: z.B. "C4" -> x=3, y=2
            if (!TryParseCoordinates(input, out int x, out int y))
            {
                view.ShowMessage("Ungültige Eingabe! Bitte Format wie 'C4' verwenden.");
                System.Threading.Thread.Sleep(1500);
                return;
            }

            // Prüfen ob Koordinaten im gültigen Bereich
            if (x < 0 || x >= model.CurrentDifficulty.Width || 
                y < 0 || y >= model.CurrentDifficulty.Height)
            {
                view.ShowMessage("Koordinaten außerhalb des Spielfelds!");
                System.Threading.Thread.Sleep(1500);
                return;
            }

            // Memento: Zustand VOR der Änderung speichern (laut Sequenzdiagramm)
            GameMemento memento = model.CreateMemento();
            history.Save(memento);

            // Feld aufdecken
            model.Reveal(x, y);
        }

        /// <summary>
        /// Macht den letzten Zug rückgängig (Memento Pattern)
        /// </summary>
        public void Undo()
        {
            GameMemento memento = history.Undo();

            if (memento == null)
            {
                view.ShowMessage("Kein Zug zum Rückgängig machen vorhanden!");
                System.Threading.Thread.Sleep(1500);
                return;
            }

            model.Restore(memento);
            view.ShowMessage("Letzter Zug wurde rückgängig gemacht.");
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Parst Koordinaten-Eingabe wie "C4" in x=3, y=2
        /// </summary>
        private bool TryParseCoordinates(string input, out int x, out int y)
        {
            x = -1;
            y = -1;

            if (string.IsNullOrEmpty(input) || input.Length < 2)
                return false;

            // Buchstaben-Teil (kann A-Z oder AA, AB, etc. sein)
            int letterEndIndex = 0;
            while (letterEndIndex < input.Length && char.IsLetter(input[letterEndIndex]))
            {
                letterEndIndex++;
            }

            if (letterEndIndex == 0)
                return false;

            string rowPart = input.Substring(0, letterEndIndex);
            string colPart = input.Substring(letterEndIndex);

            // Spalte parsen (Zahl)
            if (!int.TryParse(colPart, out int col) || col < 1)
                return false;

            x = col - 1; // 1-basiert -> 0-basiert

            // Zeile parsen (Buchstaben)
            y = ParseRowLabel(rowPart);
            
            return y >= 0;
        }

        /// <summary>
        /// Wandelt Buchstaben in Zeilennummer um (A=0, B=1, ..., AA=26, etc.)
        /// </summary>
        private int ParseRowLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
                return -1;

            label = label.ToUpper();

            if (label.Length == 1)
            {
                return label[0] - 'A';
            }
            else if (label.Length == 2)
            {
                // AA = 26, AB = 27, etc.
                int first = label[0] - 'A' + 1;
                int second = label[1] - 'A';
                return first * 26 + second;
            }

            return -1;
        }
    }
}
