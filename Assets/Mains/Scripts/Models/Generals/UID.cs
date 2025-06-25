using Newtonsoft.Json;
using System;

namespace YNL.JAMOS
{
    [Serializable]
    public struct UID
    {
        public string _id;

        [JsonConstructor]
        public UID(string id)
        {
            _id = id;
        }

        public static implicit operator string(UID id) => id._id;
        public static implicit operator UID(string id) => new UID(id);

        public static UID Parse(string id) => new UID(id);

        public static bool TryParse(string id, out UID result)
        {
            result = new UID(id);
            return !string.IsNullOrEmpty(id);
        }

        public override string ToString() => _id;

        public override bool Equals(object obj)
        {
            if (obj is UID other)
            {
                return _id == other._id;
            }
            return false;
        }

        public override int GetHashCode() => _id.GetHashCode();
    }
}