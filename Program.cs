using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Hauptklasse: Einstiegspunkt der Anwendung
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Hauptmethode: Startet das Minesweeper-Spiel
        /// Optional: Uncomment die nächste Zeile um die Unit Tests auszuführen
        /// </summary>
        static void Main(string[] args)
        {
            // Uncomment um Unit Tests auszuführen:
            // MinesweeperTests.RunAllTests();

            Game game = new Game();
            game.Start();
        }
    }
}
