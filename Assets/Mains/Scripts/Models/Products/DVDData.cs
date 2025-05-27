using System;

namespace YNL.JAMOS
{
    public class DVD
    {
        public enum Format : byte { Standard, Bluray }

        [Flags]
        public enum Genre : ushort
        {
            None = 0,
            Action = 1 << 0,
            Adventure = 1 << 1,
            Comedy = 1 << 2,
            Drama = 1 << 3,
            Fantasy = 1 << 4,
            Horror = 1 << 5,
            Mystery = 1 << 6,
            Thriller = 1 << 7,
            ScienceFiction = 1 << 8,
            Animation = 1 << 9,
            Family = 1 << 10,
            Musical = 1 << 11,
            Documentary = 1 << 12,
            Crime = 1 << 13,
            War = 1 << 14,
            Romance = 1 << 15
        }

        [System.Serializable]
        public class Data
        {
            public string Studio;
            public string Duration;
            public Format Format;
            public Genre Genre;
        }
    }
}