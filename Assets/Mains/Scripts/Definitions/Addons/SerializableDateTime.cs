using Newtonsoft.Json;
using System;
using System.Globalization;
using UnityEngine;

namespace YNL.JAMOS
{
    [Serializable]
    public class SerializableDateTime : IComparable<SerializableDateTime>
    {
        public string Value;

        [JsonIgnore] public DateTime DateTime
        {
            get => DateTime.ParseExact(Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            set => Value = value.ToString("dd/MM/yyyy");
        }

        public SerializableDateTime() { }
        public SerializableDateTime(DateTime dateTime)
        {
            DateTime = dateTime;
        }
        [JsonConstructor]
        public SerializableDateTime(string dateTime)
        {
            Value = dateTime;
        }

        public override string ToString() => DateTime.ToString();

        public static implicit operator DateTime(SerializableDateTime serializableDateTime) => serializableDateTime.DateTime;

        public static bool operator >(SerializableDateTime left, DateTime right) => left.DateTime > right;
        public static bool operator <(SerializableDateTime left, DateTime right) => left.DateTime < right;
        public static bool operator >=(SerializableDateTime left, DateTime right) => left.DateTime >= right;
        public static bool operator <=(SerializableDateTime left, DateTime right) => left.DateTime <= right;

        public int CompareTo(SerializableDateTime other)
        {
            if (other == null) return 1;
            return DateTime.CompareTo(other.DateTime);
        }


    }
}