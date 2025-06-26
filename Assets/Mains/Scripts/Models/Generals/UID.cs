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

        public override string ToString() => _id;
    }
}