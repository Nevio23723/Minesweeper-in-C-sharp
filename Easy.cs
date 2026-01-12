using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    internal class Easy : IDifficulty
    {
        public int Width => 8;
        public int Height => 8;
        public int Mines => 10;

    }
}
