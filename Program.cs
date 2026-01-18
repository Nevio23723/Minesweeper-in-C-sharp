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
        /// </summary>
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
}
