using System.Collections.Concurrent;

namespace MediaStore
{
    public struct UID
    {
        public static ConcurrentDictionary<IDType, UID> UIDs = new();

        private uint _id;

        public UID(uint id)
        {
            _id = id;
        }

        public static implicit operator uint(UID id) => id._id;
        public static implicit operator UID(uint id) => new(id);

        public static UID Parse(string id) => new(uint.Parse(id));

        public override string ToString() => $"{_id}";
        public override bool Equals(object? obj)
        {
            if (obj is UID other)
            {
                return _id == other._id;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}
