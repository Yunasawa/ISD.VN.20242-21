using System;

namespace YNL.JAMOS
{
    public class Book
    {
        public enum Format : byte { Hardcover, Paperback, EBook }

        [Flags]
        public enum Genre : ushort
        {
            None = 0,
            Fiction = 1 << 0,
            NonFiction = 1 << 1,
            Mystery = 1 << 2,
            Thriller = 1 << 3,
            Fantasy = 1 << 4,
            ScienceFiction = 1 << 5,
            Romance = 1 << 6,
            Horror = 1 << 7,
            Historical = 1 << 8,
            Biography = 1 << 9,
            Poetry = 1 << 10,
            Drama = 1 << 11,
            Adventure = 1 << 12,
            Crime = 1 << 13,
            TrueCrime = 1 << 14,
            YoungAdult = 1 << 15
        }

        [System.Serializable]
        public class Data 
        {
            public string Language;
            public ushort NumberOfPage;
            public Format Format;
            public Genre Genre;
        }
    }
}