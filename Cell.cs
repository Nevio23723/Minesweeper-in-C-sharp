using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Repräsentiert eine einzelne Zelle im Spielfeld
    /// </summary>
    public class Cell
    {
        /// <summary>Gibt an ob diese Zelle eine Mine enthält</summary>
        public bool IsMine { get; set; }
        
        /// <summary>Gibt an ob diese Zelle bereits aufgedeckt wurde</summary>
        public bool IsRevealed { get; set; }
        
        /// <summary>Anzahl der Minen in den 8 umliegenden Feldern</summary>
        public int NeighborMines { get; set; }


        /// <summary>
        /// Erstellt eine flache Kopie dieser Zelle (wichtig für GameMemento)
        /// </summary>
        public Cell Clone()
        {
            return (Cell)this.MemberwiseClone();
        }
    }
}
