using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Mittlerer Schwierigkeitsgrad: 16x16 Feld mit 40 Minen
    /// </summary>
    internal class Medium : IDifficulty
    {
        public int Width => 16;
        public int Height => 16;
        public int Mines => 40;
    }
}
