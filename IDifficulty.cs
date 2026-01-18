using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Interface für Schwierigkeitsgrad-Strategien (Strategy Pattern)
    /// </summary>
    internal interface IDifficulty
    {
        /// <summary>Breite des Spielfelds</summary>
        int Width { get; }
        
        /// <summary>Höhe des Spielfelds</summary>
        int Height { get; }
        
        /// <summary>Anzahl der Minen</summary>
        int Mines { get; }
    }
}
