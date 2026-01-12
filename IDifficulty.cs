using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    internal interface IDifficulty
    {
        int Width { get; }
        int Height { get; }
        int Mines { get; }
    }
}
