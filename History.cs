using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// History-Klasse: Verwaltet den Stack von Mementos für Undo-Funktionalität
    /// </summary>
    internal class History
    {
        private Stack<GameMemento> _undoStack = new Stack<GameMemento>();

        /// <summary>
        /// Speichert ein Memento im Undo-Stack
        /// </summary>
        public void Save(GameMemento memento) => _undoStack.Push(memento);

        /// <summary>
        /// Holt das letzte Memento vom Stack (Undo). Gibt null zurück wenn Stack leer ist.
        /// </summary>
        public GameMemento Undo() => _undoStack.Count > 0 ? _undoStack.Pop() : null;
    }
}
