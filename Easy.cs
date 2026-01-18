using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Einfacher Schwierigkeitsgrad: 8x8 Feld mit 10 Minen
    /// </summary>
    internal class Easy : IDifficulty
    {
        public int Width => 8;
        public int Height => 8;
        public int Mines => 10;

    }
}
