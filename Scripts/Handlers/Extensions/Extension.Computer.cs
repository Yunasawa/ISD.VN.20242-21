using MediaStore.Exceptions;

namespace MediaStore
{
    public static partial class Extension
    {
        public static class Computer
        {
            public static UID GetNextID(UIDType type)
            {
                if (!UID.UIDs.ContainsKey(type))
                {
                    throw new UninitializedSystemException("UID is not initialized yet.");
                }

                return UID.UIDs[type]++;
            }
        }
    }
}