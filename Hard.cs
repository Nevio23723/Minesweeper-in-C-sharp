using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Schwerer Schwierigkeitsgrad: 30x16 Feld mit 99 Minen
    /// </summary>
    internal class Hard : IDifficulty
    {
        public int Width => 30;
        public int Height => 16;
        public int Mines => 99;
    }
}
