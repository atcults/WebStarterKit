using System;

namespace Common.Action
{
    /// <summary>
    /// Wrapper around action that needs clean up at the end
    /// </summary>
    public class DisposableAction : IDisposable
    {
        private readonly System.Action _action;

        public DisposableAction(System.Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }
}