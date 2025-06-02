using System;
using UnityEngine;

namespace YNL.JAMOS
{
    [Serializable]
    public class SerializableDateTime : IComparable<SerializableDateTime>
    {
        [SerializeField] private string _dateTimeString;

        public DateTime DateTime
        {
            get => DateTime.Parse(_dateTimeString);
            set => _dateTimeString = value.ToString("MM/dd/yyyy");
        }

        public SerializableDateTime(DateTime dateTime)
        {
            DateTime = dateTime;
        }
        public SerializableDateTime(string dateTime)
        {
            _dateTimeString = dateTime;
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