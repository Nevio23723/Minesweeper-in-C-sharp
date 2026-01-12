using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class Cell
    {
        public bool IsMine { get; set; }
        public bool IsRevealed { get; set; }
        public int NeighborMines { get; set; }


        public Cell Clone() //wichtig für GameMemento
        {
            return (Cell)this.MemberwiseClone();
        }
    }
}
