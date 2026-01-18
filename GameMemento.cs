using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Memento-Klasse: Speichert einen Snapshot des Spielzustands für Undo-Funktion
    /// </summary>
    internal class GameMemento
    {
        /// <summary>Gespeichertes Spielfeld</summary>
        public Cell[,] SavedBoard { get; }
        
        /// <summary>Gespeicherter GameOver-Status</summary>
        public bool SavedGameOver { get; }

        /// <summary>
        /// Konstruktor: Erstellt ein Memento mit einer Kopie des aktuellen Zustands
        /// </summary>
        public GameMemento(Cell[,] board, bool gameOver)
        {
            //Einfache Kopie 
            SavedGameOver = gameOver;

            //abmessung holen
            int w = board.GetLength(0);
            int h = board.GetLength(1);

            //Neuen Array erstellen
            SavedBoard = new Cell[w, h];

            //jede Zelle durchgehen
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    //Jede Zelle einzeln klonen
                    SavedBoard[x, y] = board[x, y].Clone();
                }
            }  
        }
    }
}
