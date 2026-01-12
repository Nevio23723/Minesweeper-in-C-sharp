using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    internal class History
    {
        //erstellen von neuem Stack
        private Stack<GameMemento> _undoStack = new Stack<GameMemento>();

        //
        public void Save(GameMemento memento) => _undoStack.Push(memento);

        //=> expression body; returned den Count, sofern er grösser als 0 ist, ansonsten null.
        public GameMemento Undo() => _undoStack.Count > 0 ? _undoStack.Pop() : null;
    }
}
