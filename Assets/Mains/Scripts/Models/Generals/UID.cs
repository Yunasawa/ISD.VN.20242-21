using Newtonsoft.Json;
using System;

namespace YNL.JAMOS
{
    [System.Serializable]
    public struct UID
    {
        public int _id;

        [JsonConstructor]
        public UID(int id)
        {
            _id = id;
        }
        public UID(string id)
        {
            _id = Parse(id);
        }

        public static implicit operator int(UID id) => id._id;
        public static implicit operator UID(int id) => new(id);

        public static UID Parse(string id) => new(int.Parse(id));
        public static bool TryParse(string id, out UID result)
        {
            if (int.TryParse(id, out int value))
            {
                result = value;
                return true;
            }

            throw new FormatException($"Invalid UID format: {id}");
        }

        public override string ToString() => $"{_id.ToString()}";
        public override bool Equals(object obj)
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
