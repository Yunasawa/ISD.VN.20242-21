using System;

namespace YNL.JAMOS.Exceptions
{
    public class UninitializedSystemException : Exception
    {
        public static event Action OnUninitializedSystem;

        public UninitializedSystemException(string message) : base(message)
        {
            OnUninitializedSystem?.Invoke();
        }
    }
}
