using System.Collections.Generic;

namespace Minesweeper.App
{
    /// <summary>
    /// Manages the history of game states for undo functionality.
    /// </summary>
    public class History
    {
        private readonly Stack<GameMemento> _history = new Stack<GameMemento>();

        public void Save(GameMemento memento)
        {
            _history.Push(memento);
        }

        public GameMemento Undo()
        {
            if (_history.Count > 0)
            {
                return _history.Pop();
            }
            return null;
        }

        public bool CanUndo => _history.Count > 0;
    }
}
