namespace MediaStore.Exceptions
{
    public class MediaNotFoundException : Exception
    {
        public static event Action? OnMediaNorFound;

        public MediaNotFoundException(string message) : base(message)
        {
            OnMediaNorFound?.Invoke();
        }
    }
}
